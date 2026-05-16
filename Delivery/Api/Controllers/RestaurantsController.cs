using Delivery.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace Delivery.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class RestaurantsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<RestaurantDto>> GetRestaurants()
        {
            var list = new List<RestaurantDto>();

            using var conn = new NpgsqlConnection(Database.connectionString);
            conn.Open();

            string sql = "SELECT restaurant_id, name, address, phone FROM restaurants ORDER BY restaurant_id";
            using var cmd = new NpgsqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new RestaurantDto(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.IsDBNull(2) ? null : reader.GetString(2),
                    reader.IsDBNull(3) ? null : reader.GetString(3)));
            }

            return Ok(list);
        }

        [HttpGet("by-user/{userId:int}")]
        public ActionResult<RestaurantDto> GetRestaurantByUser(int userId)
        {
            using var conn = new NpgsqlConnection(Database.connectionString);
            conn.Open();

            string sql = "SELECT restaurant_id, name, address, phone FROM restaurants WHERE user_id = @uid";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@uid", userId);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return NotFound();
            }

            var dto = new RestaurantDto(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.IsDBNull(2) ? null : reader.GetString(2),
                reader.IsDBNull(3) ? null : reader.GetString(3));

            return Ok(dto);
        }

        [HttpGet("{restaurantId:int}/menu")]
        public ActionResult<List<MenuItemDto>> GetMenu(int restaurantId)
        {
            var list = new List<MenuItemDto>();

            using var conn = new NpgsqlConnection(Database.connectionString);
            conn.Open();

            string sql = @"
                SELECT item_id, restaurant_id, name, price, description, is_available
                FROM menu_items
                WHERE restaurant_id = @rid AND is_available = TRUE
                ORDER BY item_id";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@rid", restaurantId);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new MenuItemDto(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetString(2),
                    reader.GetDecimal(3),
                    reader.IsDBNull(4) ? null : reader.GetString(4),
                    reader.GetBoolean(5)));
            }

            return Ok(list);
        }

        [HttpPut("{restaurantId:int}")]
        public IActionResult UpdateRestaurant(int restaurantId, [FromBody] RestaurantUpdateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Name is required");
            }

            using var conn = new NpgsqlConnection(Database.connectionString);
            conn.Open();

            string sql = "UPDATE restaurants SET name = @name WHERE restaurant_id = @id";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@name", request.Name.Trim());
            cmd.Parameters.AddWithValue("@id", restaurantId);
            cmd.ExecuteNonQuery();

            return Ok();
        }

        [HttpPost("{restaurantId:int}/menu")]
        public ActionResult<int> AddMenuItem(int restaurantId, [FromBody] MenuItemUpsertRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Name is required");
            }

            using var conn = new NpgsqlConnection(Database.connectionString);
            conn.Open();

            string sql = "INSERT INTO menu_items (restaurant_id, name, price) VALUES (@rid, @name, @price) RETURNING item_id";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@rid", restaurantId);
            cmd.Parameters.AddWithValue("@name", request.Name.Trim());
            cmd.Parameters.AddWithValue("@price", request.Price);

            int itemId = Convert.ToInt32(cmd.ExecuteScalar());
            return Ok(itemId);
        }

        [HttpPut("menu/{itemId:int}")]
        public IActionResult UpdateMenuItem(int itemId, [FromBody] MenuItemUpsertRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Name is required");
            }

            using var conn = new NpgsqlConnection(Database.connectionString);
            conn.Open();

            string sql = "UPDATE menu_items SET name = @name, price = @price WHERE item_id = @id";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@name", request.Name.Trim());
            cmd.Parameters.AddWithValue("@price", request.Price);
            cmd.Parameters.AddWithValue("@id", itemId);
            cmd.ExecuteNonQuery();

            return Ok();
        }

        [HttpDelete("menu/{itemId:int}")]
        public IActionResult DeleteMenuItem(int itemId, [FromQuery] int restaurantId)
        {
            using var conn = new NpgsqlConnection(Database.connectionString);
            conn.Open();

            string sql = "DELETE FROM menu_items WHERE item_id = @id AND restaurant_id = @rid";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", itemId);
            cmd.Parameters.AddWithValue("@rid", restaurantId);
            cmd.ExecuteNonQuery();

            return Ok();
        }
    }
}
