using Delivery.Api.Models;
using Delivery.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class RidersController : ControllerBase
    {
        private readonly DeliveryDbContext _db;

        public RidersController(DeliveryDbContext db)
        {
            _db = db;
        }

        [HttpGet("by-user/{userId:int}")]
        public async Task<ActionResult<int>> GetRiderIdByUser(int userId)
        {
            var rider = await _db.Riders
                .Where(r => r.UserId == userId)
                .FirstOrDefaultAsync();

            if (rider == null)
            {
                return NotFound();
            }

            return Ok(rider.RiderId);
        }

        [HttpGet("{userId:int}/current-order")]
        public async Task<ActionResult<List<RiderCurrentOrderDto>>> GetCurrentOrders(int userId)
        {
            var result = await (
                from o in _db.Orders
                join r in _db.Riders on o.RiderId equals r.RiderId
                join u in _db.Users on o.UserId equals u.UserId
                where r.UserId == userId && o.Status == "Delivering"
                orderby o.OrderId
                select new RiderCurrentOrderDto(o.OrderId, o.UserId, u.Name, o.RestaurantId, o.Status)
            ).ToListAsync();

            if (result.Count == 0)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost("{userId:int}/complete/{orderId:int}")]
        public async Task<IActionResult> CompleteOrder(int userId, int orderId)
        {
            var rider = await _db.Riders
                .Where(r => r.UserId == userId)
                .FirstOrDefaultAsync();

            if (rider == null)
            {
                return NotFound("Rider not found");
            }

            var order = await _db.Orders
                .Where(o => o.OrderId == orderId && o.RiderId == rider.RiderId)
                .FirstOrDefaultAsync();

            if (order == null)
            {
                return NotFound("Order not found");
            }

            order.Status = "Delivered";
            rider.Status = "Available";

            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
