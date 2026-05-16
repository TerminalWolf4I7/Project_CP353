using Delivery.Api.Models;
using Delivery.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Api.Controllers
{
    /// <summary>
    /// RestaurantsController: จัดการข้อมูลร้านอาหารและรายการเมนู (Menu Management)
    /// ใช้สำหรับแสดงรายการร้านค้าให้ลูกค้า และให้เจ้าของร้านจัดการเมนูของตนเอง
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public sealed class RestaurantsController : ControllerBase
    {
        private readonly DeliveryDbContext _db;

        public RestaurantsController(DeliveryDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// [GET] api/restaurants
        /// ดึงรายชื่อร้านอาหารทั้งหมดที่มีอยู่ในระบบ เพื่อแสดงผลในหน้าแรกของลูกค้า
        /// </summary>
        /// <returns>List of RestaurantDto: รายชื่อร้านอาหารพร้อมข้อมูลเบื้องต้น</returns>
        [HttpGet]
        public async Task<ActionResult<List<RestaurantDto>>> GetRestaurants()
        {
            // ดึงข้อมูลร้านอาหารทั้งหมด เรียงตาม ID และแปลงเป็น DTO
            var list = await _db.Restaurants
                .OrderBy(r => r.RestaurantId)
                .Select(r => new RestaurantDto(r.RestaurantId, r.Name, r.Address, r.Phone))
                .ToListAsync();

            return Ok(list);
        }

        /// <summary>
        /// [GET] api/restaurants/by-user/{userId}
        /// ค้นหาข้อมูลร้านอาหารโดยใช้รหัสผู้ใช้งาน (เจ้าของร้าน) 
        /// ใช้หลังจากเจ้าของร้านล็อกอิน เพื่อดึงข้อมูลร้านของตนเองมาแสดงผล
        /// </summary>
        [HttpGet("by-user/{userId:int}")]
        public async Task<ActionResult<RestaurantDto>> GetRestaurantByUser(int userId)
        {
            // ค้นหาร้านอาหารที่ผูกกับ UserId ของเจ้าของร้าน
            var r = await _db.Restaurants
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();

            if (r == null)
            {
                return NotFound();
            }

            // ส่งข้อมูลร้านอาหารกลับไป
            return Ok(new RestaurantDto(r.RestaurantId, r.Name, r.Address, r.Phone));
        }

        /// <summary>
        /// [GET] api/restaurants/{restaurantId}/menu
        /// ดึงรายการเมนูอาหารทั้งหมดของร้านที่ระบุ โดยจะดึงเฉพาะรายการที่ 'พร้อมขาย' (IsAvailable = true)
        /// </summary>
        [HttpGet("{restaurantId:int}/menu")]
        public async Task<ActionResult<List<MenuItemDto>>> GetMenu(int restaurantId)
        {
            // ดึงเมนูที่สถานะพร้อมขาย เรียงตามลำดับการเพิ่ม
            var list = await _db.MenuItems
                .Where(m => m.RestaurantId == restaurantId && m.IsAvailable)
                .OrderBy(m => m.ItemId)
                .Select(m => new MenuItemDto(m.ItemId, m.RestaurantId, m.Name, m.Price, m.Description, m.IsAvailable))
                .ToListAsync();

            return Ok(list);
        }

        /// <summary>
        /// [PUT] api/restaurants/{restaurantId}
        /// อัปเดตข้อมูลพื้นฐานของร้านอาหาร เช่น ชื่อร้าน
        /// </summary>
        [HttpPut("{restaurantId:int}")]
        public async Task<IActionResult> UpdateRestaurant(int restaurantId, [FromBody] RestaurantUpdateRequest request)
        {
            // ตรวจสอบความถูกต้องของข้อมูล (ชื่อห้ามว่าง)
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Name is required");
            }

            // ค้นหาร้านอาหารที่ต้องการแก้ไข
            var restaurant = await _db.Restaurants.FindAsync(restaurantId);
            if (restaurant == null)
            {
                return NotFound();
            }

            // แก้ไขและบันทึกข้อมูล
            restaurant.Name = request.Name.Trim();
            await _db.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// [POST] api/restaurants/{restaurantId}/menu
        /// เพิ่มรายการเมนูอาหารใหม่เข้าไปในร้านค้า
        /// </summary>
        /// <param name="request">MenuItemUpsertRequest: ชื่อและราคาเมนู</param>
        [HttpPost("{restaurantId:int}/menu")]
        public async Task<ActionResult<int>> AddMenuItem(int restaurantId, [FromBody] MenuItemUpsertRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Name is required");
            }

            // สร้าง Object เมนูใหม่
            var item = new Data.Entities.MenuItem
            {
                RestaurantId = restaurantId,
                Name = request.Name.Trim(),
                Price = request.Price
            };

            // เพิ่มลงฐานข้อมูล
            _db.MenuItems.Add(item);
            await _db.SaveChangesAsync();

            // ส่ง ID ของเมนูที่สร้างใหม่กลับไป
            return Ok(item.ItemId);
        }

        /// <summary>
        /// [PUT] api/restaurants/menu/{itemId}
        /// แก้ไขรายละเอียดของเมนูอาหารที่มีอยู่แล้ว (ชื่อและราคา)
        /// </summary>
        [HttpPut("menu/{itemId:int}")]
        public async Task<IActionResult> UpdateMenuItem(int itemId, [FromBody] MenuItemUpsertRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Name is required");
            }

            // ค้นหาเมนูที่ต้องการแก้ไข
            var item = await _db.MenuItems.FindAsync(itemId);
            if (item == null)
            {
                return NotFound();
            }

            // อัปเดตข้อมูลเมนู
            item.Name = request.Name.Trim();
            item.Price = request.Price;
            await _db.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// [DELETE] api/restaurants/menu/{itemId}
        /// ลบรายการเมนูอาหารออกจากร้าน
        /// </summary>
        /// <param name="restaurantId">ต้องระบุเพื่อยืนยันว่าเป็นเมนูของร้านนี้จริงๆ</param>
        [HttpDelete("menu/{itemId:int}")]
        public async Task<IActionResult> DeleteMenuItem(int itemId, [FromQuery] int restaurantId)
        {
            // ตรวจสอบความถูกต้องก่อนลบ (ต้องตรงทั้งรหัสเมนูและรหัสร้าน)
            var item = await _db.MenuItems
                .Where(m => m.ItemId == itemId && m.RestaurantId == restaurantId)
                .FirstOrDefaultAsync();

            if (item != null)
            {
                // ลบรายการออกจากฐานข้อมูล
                _db.MenuItems.Remove(item);
                await _db.SaveChangesAsync();
            }

            return Ok();
        }
    }
}


