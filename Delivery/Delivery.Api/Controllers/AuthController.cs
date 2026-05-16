using Delivery.Api.Models;
using Delivery.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Api.Controllers
{
    /// <summary>
    /// AuthController: จัดการระบบพิสูจน์ตัวตน (Authentication) และการเข้าสู่ระบบ
    /// ทำหน้าที่เป็นประตูแรกในการตรวจสอบว่าผู้ใช้มีตัวตนอยู่ในระบบหรือไม่ และมีบทบาท (Role) ใด
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public sealed class AuthController : ControllerBase
    {
        private readonly DeliveryDbContext _db;

        /// <summary>
        /// Constructor รับ DbContext เข้ามาผ่าน Dependency Injection
        /// เพื่อใช้ในการสืบค้นข้อมูลผู้ใช้งานจากฐานข้อมูล PostgreSQL
        /// </summary>
        public AuthController(DeliveryDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// [POST] api/auth/login
        /// ใช้สำหรับการตรวจสอบการเข้าสู่ระบบโดยใช้รหัสผู้ใช้งาน (User ID)
        /// ในระบบจำลองนี้ เราจะเช็คแค่ว่า ID มีอยู่จริงไหม และส่งบทบาทกลับไปให้ Client จัดการเปิดหน้าจอที่เหมาะสม
        /// </summary>
        /// <param name="req">LoginRequest ที่ประกอบด้วย UserId จากหน้าจอ Login</param>
        /// <returns>
        /// - 200 OK: หากพบผู้ใช้งาน จะส่ง UserId และ Role (เช่น 'customer', 'restaurant', 'rider')
        /// - 404 Not Found: หากไม่พบรหัสผู้ใช้งานที่ระบุในฐานข้อมูล
        /// </returns>
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest req)
        {
            // ทำการค้นหาข้อมูล User ในตาราง Users โดยใช้ Primary Key (UserId)
            var user = await _db.Users.FindAsync(req.UserId);
            
            // กรณีไม่พบข้อมูลผู้ใช้งานในระบบ
            if (user == null)
            {
                // ส่ง Error Message กลับไปเพื่อให้ Client แสดงผลแจ้งเตือนผู้ใช้
                return NotFound("User ID not found");
            }

            // หากพบข้อมูล จะสร้าง LoginResponse เพื่อส่งข้อมูลที่จำเป็นกลับไป
            // Client จะใช้ข้อมูล Role ในการตัดสินใจว่าจะเปิดฟอร์มสำหรับ ลูกค้า, ร้านอาหาร หรือไรเดอร์
            return Ok(new LoginResponse(user.UserId, user.Role));
        }
    }
}

