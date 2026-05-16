using Delivery.Api.Models;
using Delivery.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Api.Controllers
{
    /// <summary>
    /// OrdersController: ศูนย์กลางการจัดการคำสั่งซื้อ (Orders)
    /// ทำหน้าที่ตั้งแต่กระบวนการ Checkout, ติดตามสถานะออเดอร์, ไปจนถึงการมอบหมายงานให้ไรเดอร์
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public sealed class OrdersController : ControllerBase
    {
        private readonly DeliveryDbContext _db;

        public OrdersController(DeliveryDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// [GET] api/orders
        /// ค้นหาและดึงรายการออเดอร์ตามตัวกรองที่ระบุ (Filter)
        /// ใช้สำหรับหน้าประวัติการสั่งซื้อของลูกค้า, รายการออเดอร์ของร้าน และรายการงานของไรเดอร์
        /// </summary>
        /// <param name="userId">ตัวกรอง: รหัสลูกค้า</param>
        /// <param name="restaurantId">ตัวกรอง: รหัสร้านอาหาร</param>
        /// <param name="riderId">ตัวกรอง: รหัสไรเดอร์</param>
        /// <param name="status">ตัวกรอง: สถานะออเดอร์ (เช่น 'Pending', 'Delivering')</param>
        /// <returns>List of OrderDto: รายการออเดอร์ที่ตรงตามเงื่อนไข</returns>
        [HttpGet]
        public async Task<ActionResult<List<OrderDto>>> GetOrders([FromQuery] int? userId, [FromQuery] int? restaurantId, [FromQuery] int? riderId, [FromQuery] string? status)
        {
            var query = _db.Orders.AsQueryable();

            // ตรวจสอบพารามิเตอร์และเพิ่มเงื่อนไขการค้นหาเข้าไปใน Query
            if (userId.HasValue)
                query = query.Where(o => o.UserId == userId.Value);
            if (restaurantId.HasValue)
                query = query.Where(o => o.RestaurantId == restaurantId.Value);
            if (riderId.HasValue)
                query = query.Where(o => o.RiderId == riderId.Value);
            if (!string.IsNullOrEmpty(status))
                query = query.Where(o => o.Status == status);

            // ดึงข้อมูลและแปลงเป็น DTO เพื่อส่งออกไปยัง Client
            var list = await query
                .OrderBy(o => o.OrderId)
                .Select(o => new OrderDto(o.OrderId, o.UserId, o.RestaurantId, o.RiderId, o.TotalPrice, o.Status))
                .ToListAsync();

            return Ok(list);
        }

        /// <summary>
        /// [GET] api/orders/{orderId}/details
        /// ดึงข้อมูลรายละเอียดสรุปของออเดอร์เพียง 1 รายการ พร้อมข้อมูลชื่อลูกค้า
        /// </summary>
        [HttpGet("{orderId:int}/details")]
        public async Task<ActionResult<OrderDetailDto>> GetOrderDetails(int orderId)
        {
            // ทำการ Join ข้อมูลระหว่างตาราง Orders และ Users เพื่อเอาชื่อลูกค้ามาแสดงผล
            var result = await _db.Orders
                .Where(o => o.OrderId == orderId)
                .Join(_db.Users,
                    o => o.UserId,
                    u => u.UserId,
                    (o, u) => new OrderDetailDto(o.OrderId, o.UserId, u.Name, o.RestaurantId, o.TotalPrice, o.Status))
                .FirstOrDefaultAsync();

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// [GET] api/orders/{orderId}
        /// ดึงรายการอาหาร (Items) ทั้งหมดที่อยู่ในออเดอร์ที่ระบุ
        /// </summary>
        [HttpGet("{orderId:int}")]
        public async Task<ActionResult<List<OrderItemDto>>> GetOrderItems(int orderId)
        {
            // Join กับตาราง MenuItems เพื่อดึงชื่อเมนูอาหารมาแสดงคู่กับจำนวนและราคาที่สั่ง
            var list = await _db.OrderItems
                .Where(oi => oi.OrderId == orderId)
                .Join(_db.MenuItems,
                    oi => oi.ItemId,
                    mi => mi.ItemId,
                    (oi, mi) => new OrderItemDto(oi.ItemId, mi.Name, oi.Quantity, oi.Price))
                .ToListAsync();

            return Ok(list);
        }

        /// <summary>
        /// [POST] api/orders/checkout
        /// กระบวนการสำคัญ: ยืนยันการสั่งซื้อ โดยจะย้ายรายการจากตะกร้า (Cart) มาเป็นออเดอร์จริง (Order)
        /// </summary>
        /// <param name="req">CheckoutRequest: รหัสลูกค้าและรหัสร้าน</param>
        /// <returns>orderId: รหัสออเดอร์ที่สร้างขึ้นใหม่</returns>
        [HttpPost("checkout")]
        public async Task<ActionResult<int>> Checkout([FromBody] CheckoutRequest req)
        {
            // 1. ตรวจสอบว่าลูกค้ามีตะกร้าสินค้าของร้านนี้อยู่จริงหรือไม่
            var cart = await _db.Carts
                .Where(c => c.UserId == req.UserId && c.RestaurantId == req.RestaurantId)
                .OrderByDescending(c => c.CreatedAt)
                .FirstOrDefaultAsync();

            if (cart == null)
            {
                return BadRequest("Cart not found");
            }

            // 2. ดึงรายการอาหารทั้งหมดในตะกร้าและดึงราคาล่าสุดจากเมนู
            var cartItems = await _db.CartItems
                .Where(ci => ci.CartId == cart.CartId)
                .Join(_db.MenuItems,
                    ci => ci.ItemId,
                    mi => mi.ItemId,
                    (ci, mi) => new { ci.ItemId, ci.Quantity, mi.Price })
                .ToListAsync();

            if (cartItems.Count == 0)
            {
                return BadRequest("Cart empty");
            }

            // 3. คำนวณยอดรวมราคาสุทธิ
            decimal total = cartItems.Sum(i => i.Price * i.Quantity);

            // 4. สร้างข้อมูลออเดอร์หลักในตาราง Orders กำหนดสถานะเริ่มต้นเป็น "Pending"
            var order = new Data.Entities.Order
            {
                UserId = req.UserId,
                RestaurantId = req.RestaurantId,
                TotalPrice = total,
                Status = "Pending"
            };

            _db.Orders.Add(order);
            await _db.SaveChangesAsync(); // บันทึกเพื่อให้ได้ OrderId สำหรับใช้ในตารางถัดไป

            // 5. บันทึกรายการอาหารลงในตาราง OrderItems (บันทึกราคา ณ ขณะที่สั่ง เพื่อป้องกันการเปลี่ยนราคาทีหลัง)
            foreach (var item in cartItems)
            {
                _db.OrderItems.Add(new Data.Entities.OrderItem
                {
                    OrderId = order.OrderId,
                    ItemId = item.ItemId,
                    Quantity = item.Quantity,
                    Price = item.Price
                });
            }

            // 6. เคลียร์ตะกร้าสินค้า (ลบทิ้งทั้งหัวตะกร้าและรายการย่อย) เพราะสั่งซื้อสำเร็จแล้ว
            var itemsToRemove = await _db.CartItems
                .Where(ci => ci.CartId == cart.CartId)
                .ToListAsync();
            _db.CartItems.RemoveRange(itemsToRemove);
            _db.Carts.Remove(cart);

            // ยืนยันการเปลี่ยนแปลงทั้งหมดลงฐานข้อมูล (Transaction)
            await _db.SaveChangesAsync();

            return Ok(order.OrderId);
        }

        /// <summary>
        /// [PATCH] api/orders/{orderId}/status
        /// อัปเดตสถานะความคืบหน้าของออเดอร์ (ใช้โดยร้านอาหารและไรเดอร์)
        /// </summary>
        /// <param name="req">UpdateStatusRequest: สถานะใหม่ที่ต้องการเปลี่ยน</param>
        [HttpPatch("{orderId:int}/status")]
        public async Task<IActionResult> UpdateStatus(int orderId, [FromBody] UpdateStatusRequest req)
        {
            var order = await _db.Orders.FindAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }

            // แก้ไขสถานะ เช่น 'Pending' -> 'Cooking'
            order.Status = req.Status;
            await _db.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// [DELETE] api/orders/{orderId}
        /// ลบข้อมูลออเดอร์ออกจากระบบ (ใช้เมื่อออเดอร์สิ้นสุดกระบวนการ หรือถูกยกเลิก)
        /// </summary>
        [HttpDelete("{orderId:int}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            // ลบรายการอาหารย่อยก่อนเพื่อรักษา Integrity ของฐานข้อมูล
            var items = await _db.OrderItems
                .Where(oi => oi.OrderId == orderId)
                .ToListAsync();
            _db.OrderItems.RemoveRange(items);

            // ลบออเดอร์หลัก
            var order = await _db.Orders.FindAsync(orderId);
            if (order != null)
            {
                _db.Orders.Remove(order);
            }

            await _db.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// [PATCH] api/orders/{orderId}/accept-rider/{userId}
        /// ไรเดอร์กดรับงานที่รออยู่ (สถานะ Pending/Cooking) เพื่อเปลี่ยนเป็นสถานะกำลังจัดส่ง (Delivering)
        /// </summary>
        /// <param name="userId">รหัสผู้ใช้งานของไรเดอร์ (ใช้ค้นหา RiderId)</param>
        [HttpPatch("{orderId:int}/accept-rider/{userId:int}")]
        public async Task<IActionResult> AcceptRiderOrder(int orderId, int userId)
        {
            // ตรวจสอบสถานะและรหัสไรเดอร์จากตาราง Riders
            var rider = await _db.Riders
                .Where(r => r.UserId == userId)
                .FirstOrDefaultAsync();

            if (rider == null)
            {
                return NotFound("Rider not found");
            }

            // ตรวจสอบว่าออเดอร์นี้ยังว่างอยู่และไม่มีไรเดอร์คนอื่นรับไปก่อนหน้า (ป้องกัน Race Condition)
            var order = await _db.Orders.FindAsync(orderId);
            if (order == null || order.RiderId != null)
            {
                return Conflict("Order already taken");
            }

            // ผูกไรเดอร์เข้ากับออเดอร์
            order.RiderId = rider.RiderId;
            // เปลี่ยนสถานะออเดอร์เป็น 'Delivering'
            order.Status = "Delivering";
            // เปลี่ยนสถานะไรเดอร์เป็น 'Delivering' เพื่อให้คนอื่นรู้ว่าไม่ว่าง
            rider.Status = "Delivering";

            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}


