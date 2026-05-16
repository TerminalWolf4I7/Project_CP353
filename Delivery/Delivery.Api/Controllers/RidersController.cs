using Delivery.Api.Models;
using Delivery.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Api.Controllers
{
    /// <summary>
    /// RidersController: จัดการข้อมูลและการทำงานของไรเดอร์ (Rider Management)
    /// ทำหน้าที่รับงาน, ดูออเดอร์ที่ต้องส่ง และยืนยันเมื่อส่งอาหารเสร็จสิ้น
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public sealed class RidersController : ControllerBase
    {
        private readonly DeliveryDbContext _db;

        public RidersController(DeliveryDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// [GET] api/riders/by-user/{userId}
        /// ค้นหา RiderId จาก UserId ของผู้ใช้งานที่ล็อกอินเข้ามา
        /// เนื่องจากระบบแยกตาราง Users และ Riders ออกจากกัน จึงต้องทำการค้นหาความเชื่อมโยงนี้
        /// </summary>
        /// <param name="userId">รหัสผู้ใช้งาน</param>
        /// <returns>riderId: รหัสไรเดอร์ที่ใช้ในการอ้างอิงออเดอร์</returns>
        [HttpGet("by-user/{userId:int}")]
        public async Task<ActionResult<int>> GetRiderIdByUser(int userId)
        {
            // ค้นหาข้อมูลในตาราง Riders ที่ผูกกับ UserId นี้
            var rider = await _db.Riders
                .Where(r => r.UserId == userId)
                .FirstOrDefaultAsync();

            if (rider == null)
            {
                return NotFound();
            }

            return Ok(rider.RiderId);
        }

        /// <summary>
        /// [GET] api/riders/{userId}/current-order
        /// ดึงรายการออเดอร์ที่ไรเดอร์คนนี้กำลังทำการจัดส่งอยู่ ณ ขณะนี้
        /// </summary>
        /// <param name="userId">รหัสผู้ใช้งานของไรเดอร์</param>
        /// <returns>List of RiderCurrentOrderDto: รายการงานที่กำลังดำเนินการอยู่</returns>
        [HttpGet("{userId:int}/current-order")]
        public async Task<ActionResult<List<RiderCurrentOrderDto>>> GetCurrentOrders(int userId)
        {
            // ใช้การ Query แบบ Join หลายตาราง (Orders, Riders, Users) 
            // เพื่อรวมข้อมูลออเดอร์, ข้อมูลไรเดอร์ และชื่อลูกค้าที่จะไปส่งอาหาร
            var result = await (
                from o in _db.Orders
                join r in _db.Riders on o.RiderId equals r.RiderId
                join u in _db.Users on o.UserId equals u.UserId
                where r.UserId == userId && o.Status == "Delivering" // คัดเฉพาะงานที่กำลังส่ง
                orderby o.OrderId
                select new RiderCurrentOrderDto(o.OrderId, o.UserId, u.Name, o.RestaurantId, o.Status)
            ).ToListAsync();

            if (result.Count == 0)
            {
                // หากไม่มีงานที่กำลังส่งอยู่ จะส่ง 404 กลับไป (Client จะแสดงว่า 'ไม่มีงานปัจจุบัน')
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// [POST] api/riders/{userId}/complete/{orderId}
        /// กระบวนการยืนยันการส่งอาหารสำเร็จ โดยไรเดอร์เป็นผู้กด
        /// </summary>
        /// <param name="userId">รหัสผู้ใช้งานของไรเดอร์</param>
        /// <param name="orderId">รหัสออเดอร์ที่ส่งสำเร็จแล้ว</param>
        [HttpPost("{userId:int}/complete/{orderId:int}")]
        public async Task<IActionResult> CompleteOrder(int userId, int orderId)
        {
            // 1. ค้นหาข้อมูลไรเดอร์
            var rider = await _db.Riders
                .Where(r => r.UserId == userId)
                .FirstOrDefaultAsync();

            if (rider == null)
            {
                return NotFound("Rider not found");
            }

            // 2. ตรวจสอบว่าออเดอร์ที่แจ้งมา เป็นออเดอร์ที่ไรเดอร์คนนี้รับผิดชอบอยู่จริงหรือไม่
            var order = await _db.Orders
                .Where(o => o.OrderId == orderId && o.RiderId == rider.RiderId)
                .FirstOrDefaultAsync();

            if (order == null)
            {
                return NotFound("Order not found");
            }

            // 3. อัปเดตสถานะออเดอร์เป็น 'Delivered' (ส่งสำเร็จ)
            order.Status = "Delivered";
            
            // 4. อัปเดตสถานะไรเดอร์ให้กลับมาว่าง 'Available' เพื่อรับงานใหม่ได้
            rider.Status = "Available";

            // ยืนยันการเปลี่ยนแปลงลงฐานข้อมูล
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}


