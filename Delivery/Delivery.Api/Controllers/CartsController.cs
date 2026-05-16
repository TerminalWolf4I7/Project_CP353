using Delivery.Api.Models;
using Delivery.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class CartsController : ControllerBase
    {
        private readonly DeliveryDbContext _db;

        public CartsController(DeliveryDbContext db)
        {
            _db = db;
        }

        [HttpGet("{userId:int}/{restaurantId:int}")]
        public async Task<ActionResult<CartSummaryDto>> GetCart(int userId, int restaurantId)
        {
            var cart = await _db.Carts
                .Where(c => c.UserId == userId && c.RestaurantId == restaurantId)
                .OrderByDescending(c => c.CreatedAt)
                .FirstOrDefaultAsync();

            if (cart == null)
            {
                return Ok(new CartSummaryDto(0, userId, restaurantId, 0, new List<CartItemDto>()));
            }

            var items = await _db.CartItems
                .Where(ci => ci.CartId == cart.CartId)
                .Join(_db.MenuItems,
                    ci => ci.ItemId,
                    mi => mi.ItemId,
                    (ci, mi) => new CartItemDto(mi.ItemId, mi.Name, ci.Quantity, mi.Price))
                .ToListAsync();

            decimal total = items.Sum(i => i.Price * i.Quantity);
            return Ok(new CartSummaryDto(cart.CartId, userId, restaurantId, total, items));
        }

        [HttpPost("{userId:int}/{restaurantId:int}/items")]
        public async Task<IActionResult> AddToCart(int userId, int restaurantId, [FromBody] CartItemRequest req)
        {
            var cart = await _db.Carts
                .Where(c => c.UserId == userId && c.RestaurantId == restaurantId)
                .OrderByDescending(c => c.CreatedAt)
                .FirstOrDefaultAsync();

            if (cart == null)
            {
                cart = new Data.Entities.Cart
                {
                    UserId = userId,
                    RestaurantId = restaurantId
                };
                _db.Carts.Add(cart);
                await _db.SaveChangesAsync();
            }

            var existingItem = await _db.CartItems
                .Where(ci => ci.CartId == cart.CartId && ci.ItemId == req.ItemId)
                .FirstOrDefaultAsync();

            if (existingItem != null)
            {
                existingItem.Quantity += req.Quantity;
            }
            else
            {
                _db.CartItems.Add(new Data.Entities.CartItem
                {
                    CartId = cart.CartId,
                    ItemId = req.ItemId,
                    Quantity = req.Quantity
                });
            }

            await _db.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{userId:int}/{restaurantId:int}/items/{itemId:int}")]
        public async Task<IActionResult> RemoveFromCart(int userId, int restaurantId, int itemId)
        {
            var cart = await _db.Carts
                .Where(c => c.UserId == userId && c.RestaurantId == restaurantId)
                .OrderByDescending(c => c.CreatedAt)
                .FirstOrDefaultAsync();

            if (cart == null)
            {
                return Ok();
            }

            var cartItem = await _db.CartItems
                .Where(ci => ci.CartId == cart.CartId && ci.ItemId == itemId)
                .FirstOrDefaultAsync();

            if (cartItem != null)
            {
                if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity -= 1;
                }
                else
                {
                    _db.CartItems.Remove(cartItem);
                }

                await _db.SaveChangesAsync();
            }

            return Ok();
        }
    }
}
