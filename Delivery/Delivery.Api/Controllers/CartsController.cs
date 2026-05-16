using Delivery.Api.Models;
using Delivery.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Api.Controllers
{
    /// <summary>
    /// CartsController: จัดการระบบตะกร้าสินค้า (Shopping Cart)
    /// ทำหน้าที่เป็นตัวกลางในการเก็บรวบรวมรายการอาหารที่ลูกค้าเลือก ก่อนที่จะทำการสั่งซื้อจริง (Checkout)
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public sealed class CartsController : ControllerBase
    {
        private readonly DeliveryDbContext _db;

        public CartsController(DeliveryDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// [GET] api/carts/{userId}/{restaurantId}
        /// ดึงข้อมูลรายการอาหารที่อยู่ในตะกร้าของลูกค้า ณ ปัจจุบัน สำหรับร้านอาหารที่ระบุ
        /// </summary>
        /// <param name="userId">รหัสลูกค้าเจ้าของตะกร้า</param>
        /// <param name="restaurantId">รหัสร้านอาหารที่ต้องการดูรายการในตะกร้า</param>
        /// <returns>CartSummaryDto: ข้อมูลสรุปรายการอาหารทั้งหมดและยอดรวมราคา</returns>
        [HttpGet("{userId:int}/{restaurantId:int}")]
        public async Task<ActionResult<CartSummaryDto>> GetCart(int userId, int restaurantId)
        {
            // ค้นหาตะกร้าล่าสุดของผู้ใช้งานรายนี้ที่ผูกกับร้านอาหารที่เลือก
            var cart = await _db.Carts
                .Where(c => c.UserId == userId && c.RestaurantId == restaurantId)
                .OrderByDescending(c => c.CreatedAt)
                .FirstOrDefaultAsync();

            // หากไม่พบตะกร้า (ลูกค้ายังไม่ได้เริ่มเลือกของ) จะส่งข้อมูลตะกร้าเปล่ากลับไป
            if (cart == null)
            {
                return Ok(new CartSummaryDto(0, userId, restaurantId, 0, new List<CartItemDto>()));
            }

            // ดึงรายการอาหาร (CartItems) ทั้งหมดที่อยู่ในตะกร้านี้ 
            // โดยทำการ Join กับตาราง MenuItems เพื่อเอาชื่อเมนูและราคาล่าสุดมาแสดงผล
            var items = await _db.CartItems
                .Where(ci => ci.CartId == cart.CartId)
                .Join(_db.MenuItems,
                    ci => ci.ItemId,
                    mi => mi.ItemId,
                    (ci, mi) => new CartItemDto(mi.ItemId, mi.Name, ci.Quantity, mi.Price))
                .ToListAsync();

            // คำนวณราคาสุทธิโดยการรวม (ราคา * จำนวน) ของทุกรายการ
            decimal total = items.Sum(i => i.Price * i.Quantity);
            
            // ส่งข้อมูลสรุปกลับไปให้ Client แสดงผลในหน้าตะกร้าสินค้า
            return Ok(new CartSummaryDto(cart.CartId, userId, restaurantId, total, items));
        }

        /// <summary>
        /// [POST] api/carts/{userId}/{restaurantId}/items
        /// เพิ่มรายการอาหารลงในตะกร้า หากสินค้าชนิดนั้นมีอยู่แล้วจะทำการเพิ่มจำนวน (Quantity)
        /// </summary>
        /// <param name="req">CartItemRequest: รหัสอาหารและจำนวนที่ต้องการเพิ่ม</param>
        [HttpPost("{userId:int}/{restaurantId:int}/items")]
        public async Task<IActionResult> AddToCart(int userId, int restaurantId, [FromBody] CartItemRequest req)
        {
            // ตรวจสอบว่ามีตะกร้าสำหรับผู้ใช้งานและร้านค้านี้อยู่แล้วหรือไม่
            var cart = await _db.Carts
                .Where(c => c.UserId == userId && c.RestaurantId == restaurantId)
                .OrderByDescending(c => c.CreatedAt)
                .FirstOrDefaultAsync();

            // หากยังไม่มีตะกร้า (เริ่มสั่งครั้งแรกของร้านนี้) ให้สร้างตะกร้าใหม่
            if (cart == null)
            {
                cart = new Data.Entities.Cart
                {
                    UserId = userId,
                    RestaurantId = restaurantId
                };
                _db.Carts.Add(cart);
                await _db.SaveChangesAsync(); // บันทึกเพื่อให้ Database Generate CartId
            }

            // ตรวจสอบว่าในตะกร้านี้มีรายการอาหารชนิดเดียวกันอยู่แล้วหรือไม่
            var existingItem = await _db.CartItems
                .Where(ci => ci.CartId == cart.CartId && ci.ItemId == req.ItemId)
                .FirstOrDefaultAsync();

            if (existingItem != null)
            {
                // หากมีอยู่แล้ว ให้บวกจำนวนเพิ่มจากเดิม (เช่น สั่งข้าวกะเพราเพิ่มอีก 1)
                existingItem.Quantity += req.Quantity;
            }
            else
            {
                // หากยังไม่มีรายการนี้ ให้เพิ่มเป็นรายการใหม่ในตะกร้า
                _db.CartItems.Add(new Data.Entities.CartItem
                {
                    CartId = cart.CartId,
                    ItemId = req.ItemId,
                    Quantity = req.Quantity
                });
            }

            // บันทึกการเปลี่ยนแปลงทั้งหมดลงฐานข้อมูล
            await _db.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// [DELETE] api/carts/{userId}/{restaurantId}/items/{itemId}
        /// ลดจำนวนรายการอาหารลง 1 ชิ้น หากจำนวนเหลือ 0 จะทำการลบรายการนั้นออกจากตะกร้า
        /// </summary>
        [HttpDelete("{userId:int}/{restaurantId:int}/items/{itemId:int}")]
        public async Task<IActionResult> RemoveFromCart(int userId, int restaurantId, int itemId)
        {
            // ค้นหาตะกร้าที่ต้องการลบสินค้า
            var cart = await _db.Carts
                .Where(c => c.UserId == userId && c.RestaurantId == restaurantId)
                .OrderByDescending(c => c.CreatedAt)
                .FirstOrDefaultAsync();

            if (cart == null)
            {
                return Ok(); // หากไม่มีตะกร้า ไม่ต้องทำอะไร
            }

            // ค้นหารายการอาหารที่ระบุในตะกร้า
            var cartItem = await _db.CartItems
                .Where(ci => ci.CartId == cart.CartId && ci.ItemId == itemId)
                .FirstOrDefaultAsync();

            if (cartItem != null)
            {
                // หากมีจำนวนมากกว่า 1 ให้ลดลงทีละ 1
                if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity -= 1;
                }
                else
                {
                    // หากเหลือเพียง 1 ชิ้นแล้วลบอีก ให้เอาออกจากตาราง CartItems ไปเลย
                    _db.CartItems.Remove(cartItem);
                }

                // ยืนยันการลบหรืออัปเดตลงฐานข้อมูล
                await _db.SaveChangesAsync();
            }

            return Ok();
        }
    }
}

