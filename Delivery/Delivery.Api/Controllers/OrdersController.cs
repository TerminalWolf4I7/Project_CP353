using Delivery.Api.Models;
using Delivery.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class OrdersController : ControllerBase
    {
        private readonly DeliveryDbContext _db;

        public OrdersController(DeliveryDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderDto>>> GetOrders([FromQuery] int? userId, [FromQuery] int? restaurantId, [FromQuery] int? riderId, [FromQuery] string? status)
        {
            var query = _db.Orders.AsQueryable();

            if (userId.HasValue)
                query = query.Where(o => o.UserId == userId.Value);
            if (restaurantId.HasValue)
                query = query.Where(o => o.RestaurantId == restaurantId.Value);
            if (riderId.HasValue)
                query = query.Where(o => o.RiderId == riderId.Value);
            if (!string.IsNullOrEmpty(status))
                query = query.Where(o => o.Status == status);

            var list = await query
                .OrderBy(o => o.OrderId)
                .Select(o => new OrderDto(o.OrderId, o.UserId, o.RestaurantId, o.RiderId, o.TotalPrice, o.Status))
                .ToListAsync();

            return Ok(list);
        }

        [HttpGet("{orderId:int}/details")]
        public async Task<ActionResult<OrderDetailDto>> GetOrderDetails(int orderId)
        {
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

        [HttpGet("{orderId:int}")]
        public async Task<ActionResult<List<OrderItemDto>>> GetOrderItems(int orderId)
        {
            var list = await _db.OrderItems
                .Where(oi => oi.OrderId == orderId)
                .Join(_db.MenuItems,
                    oi => oi.ItemId,
                    mi => mi.ItemId,
                    (oi, mi) => new OrderItemDto(oi.ItemId, mi.Name, oi.Quantity, oi.Price))
                .ToListAsync();

            return Ok(list);
        }

        [HttpPost("checkout")]
        public async Task<ActionResult<int>> Checkout([FromBody] CheckoutRequest req)
        {
            var cart = await _db.Carts
                .Where(c => c.UserId == req.UserId && c.RestaurantId == req.RestaurantId)
                .OrderByDescending(c => c.CreatedAt)
                .FirstOrDefaultAsync();

            if (cart == null)
            {
                return BadRequest("Cart not found");
            }

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

            decimal total = cartItems.Sum(i => i.Price * i.Quantity);

            var order = new Data.Entities.Order
            {
                UserId = req.UserId,
                RestaurantId = req.RestaurantId,
                TotalPrice = total,
                Status = "Pending"
            };

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

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

            // Remove cart items and cart
            var itemsToRemove = await _db.CartItems
                .Where(ci => ci.CartId == cart.CartId)
                .ToListAsync();
            _db.CartItems.RemoveRange(itemsToRemove);
            _db.Carts.Remove(cart);

            await _db.SaveChangesAsync();

            return Ok(order.OrderId);
        }

        [HttpPatch("{orderId:int}/status")]
        public async Task<IActionResult> UpdateStatus(int orderId, [FromBody] UpdateStatusRequest req)
        {
            var order = await _db.Orders.FindAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }

            order.Status = req.Status;
            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{orderId:int}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var items = await _db.OrderItems
                .Where(oi => oi.OrderId == orderId)
                .ToListAsync();
            _db.OrderItems.RemoveRange(items);

            var order = await _db.Orders.FindAsync(orderId);
            if (order != null)
            {
                _db.Orders.Remove(order);
            }

            await _db.SaveChangesAsync();
            return Ok();
        }

        [HttpPatch("{orderId:int}/accept-rider/{userId:int}")]
        public async Task<IActionResult> AcceptRiderOrder(int orderId, int userId)
        {
            var rider = await _db.Riders
                .Where(r => r.UserId == userId)
                .FirstOrDefaultAsync();

            if (rider == null)
            {
                return NotFound("Rider not found");
            }

            var order = await _db.Orders.FindAsync(orderId);
            if (order == null || order.RiderId != null)
            {
                return Conflict("Order already taken");
            }

            order.RiderId = rider.RiderId;
            order.Status = "Delivering";
            rider.Status = "Delivering";

            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
