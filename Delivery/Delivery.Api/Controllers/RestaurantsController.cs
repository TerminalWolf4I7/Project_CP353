using Delivery.Api.Models;
using Delivery.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class RestaurantsController : ControllerBase
    {
        private readonly DeliveryDbContext _db;

        public RestaurantsController(DeliveryDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<List<RestaurantDto>>> GetRestaurants()
        {
            var list = await _db.Restaurants
                .OrderBy(r => r.RestaurantId)
                .Select(r => new RestaurantDto(r.RestaurantId, r.Name, r.Address, r.Phone))
                .ToListAsync();

            return Ok(list);
        }

        [HttpGet("by-user/{userId:int}")]
        public async Task<ActionResult<RestaurantDto>> GetRestaurantByUser(int userId)
        {
            var r = await _db.Restaurants
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();

            if (r == null)
            {
                return NotFound();
            }

            return Ok(new RestaurantDto(r.RestaurantId, r.Name, r.Address, r.Phone));
        }

        [HttpGet("{restaurantId:int}/menu")]
        public async Task<ActionResult<List<MenuItemDto>>> GetMenu(int restaurantId)
        {
            var list = await _db.MenuItems
                .Where(m => m.RestaurantId == restaurantId && m.IsAvailable)
                .OrderBy(m => m.ItemId)
                .Select(m => new MenuItemDto(m.ItemId, m.RestaurantId, m.Name, m.Price, m.Description, m.IsAvailable))
                .ToListAsync();

            return Ok(list);
        }

        [HttpPut("{restaurantId:int}")]
        public async Task<IActionResult> UpdateRestaurant(int restaurantId, [FromBody] RestaurantUpdateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Name is required");
            }

            var restaurant = await _db.Restaurants.FindAsync(restaurantId);
            if (restaurant == null)
            {
                return NotFound();
            }

            restaurant.Name = request.Name.Trim();
            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("{restaurantId:int}/menu")]
        public async Task<ActionResult<int>> AddMenuItem(int restaurantId, [FromBody] MenuItemUpsertRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Name is required");
            }

            var item = new Data.Entities.MenuItem
            {
                RestaurantId = restaurantId,
                Name = request.Name.Trim(),
                Price = request.Price
            };

            _db.MenuItems.Add(item);
            await _db.SaveChangesAsync();

            return Ok(item.ItemId);
        }

        [HttpPut("menu/{itemId:int}")]
        public async Task<IActionResult> UpdateMenuItem(int itemId, [FromBody] MenuItemUpsertRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Name is required");
            }

            var item = await _db.MenuItems.FindAsync(itemId);
            if (item == null)
            {
                return NotFound();
            }

            item.Name = request.Name.Trim();
            item.Price = request.Price;
            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("menu/{itemId:int}")]
        public async Task<IActionResult> DeleteMenuItem(int itemId, [FromQuery] int restaurantId)
        {
            var item = await _db.MenuItems
                .Where(m => m.ItemId == itemId && m.RestaurantId == restaurantId)
                .FirstOrDefaultAsync();

            if (item != null)
            {
                _db.MenuItems.Remove(item);
                await _db.SaveChangesAsync();
            }

            return Ok();
        }
    }
}
