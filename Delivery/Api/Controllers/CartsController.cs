using System.Linq;
using Delivery.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace Delivery.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class CartsController : ControllerBase
    {
        [HttpGet("{userId:int}/{restaurantId:int}")]
        public ActionResult<CartSummaryDto> GetCart(int userId, int restaurantId)
        {
            using var conn = new NpgsqlConnection(Database.connectionString);
            conn.Open();

            string cartSql = @"
                SELECT cart_id
                FROM carts
                WHERE user_id = @uid AND restaurant_id = @rid
                ORDER BY created_at DESC
                LIMIT 1";

            int cartId;
            using (var cmd = new NpgsqlCommand(cartSql, conn))
            {
                cmd.Parameters.AddWithValue("@uid", userId);
                cmd.Parameters.AddWithValue("@rid", restaurantId);
                object? result = cmd.ExecuteScalar();

                if (result == null)
                {
                    return Ok(new CartSummaryDto(0, userId, restaurantId, 0, new List<CartItemDto>()));
                }

                cartId = (int)result;
            }

            string itemsSql = @"
                SELECT mi.item_id, mi.name, ci.quantity, mi.price
                FROM cart_items ci
                JOIN menu_items mi ON ci.item_id = mi.item_id
                WHERE ci.cart_id = @cid";

            var items = new List<CartItemDto>();
            using (var cmd = new NpgsqlCommand(itemsSql, conn))
            {
                cmd.Parameters.AddWithValue("@cid", cartId);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    items.Add(new CartItemDto(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetInt32(2),
                        reader.GetDecimal(3)));
                }
            }

            decimal total = items.Sum(i => i.Price * i.Quantity);
            return Ok(new CartSummaryDto(cartId, userId, restaurantId, total, items));
        }

        [HttpPost("{userId:int}/{restaurantId:int}/items")]
        public IActionResult AddToCart(int userId, int restaurantId, [FromBody] CartItemRequest req)
        {
            using var conn = new NpgsqlConnection(Database.connectionString);
            conn.Open();

            string cartSql = @"
                SELECT cart_id
                FROM carts
                WHERE user_id = @uid AND restaurant_id = @rid
                ORDER BY created_at DESC
                LIMIT 1";

            int cartId = 0;
            using (var cmd = new NpgsqlCommand(cartSql, conn))
            {
                cmd.Parameters.AddWithValue("@uid", userId);
                cmd.Parameters.AddWithValue("@rid", restaurantId);
                object? result = cmd.ExecuteScalar();
                if (result != null)
                {
                    cartId = (int)result;
                }
            }

            if (cartId == 0)
            {
                string createCart = "INSERT INTO carts (user_id, restaurant_id) VALUES (@uid, @rid) RETURNING cart_id";
                using var cmd = new NpgsqlCommand(createCart, conn);
                cmd.Parameters.AddWithValue("@uid", userId);
                cmd.Parameters.AddWithValue("@rid", restaurantId);
                cartId = (int)cmd.ExecuteScalar();
            }

            string checkItem = "SELECT cart_item_id, quantity FROM cart_items WHERE cart_id = @cid AND item_id = @iid";
            int cartItemId = 0;
            int currentQty = 0;

            using (var cmd = new NpgsqlCommand(checkItem, conn))
            {
                cmd.Parameters.AddWithValue("@cid", cartId);
                cmd.Parameters.AddWithValue("@iid", req.ItemId);
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    cartItemId = reader.GetInt32(0);
                    currentQty = reader.GetInt32(1);
                }
            }

            if (cartItemId > 0)
            {
                string updateQty = "UPDATE cart_items SET quantity = @qty WHERE cart_item_id = @ciid";
                using var cmd = new NpgsqlCommand(updateQty, conn);
                cmd.Parameters.AddWithValue("@qty", currentQty + req.Quantity);
                cmd.Parameters.AddWithValue("@ciid", cartItemId);
                cmd.ExecuteNonQuery();
            }
            else
            {
                string insertItem = "INSERT INTO cart_items (cart_id, item_id, quantity) VALUES (@cid, @iid, @qty)";
                using var cmd = new NpgsqlCommand(insertItem, conn);
                cmd.Parameters.AddWithValue("@cid", cartId);
                cmd.Parameters.AddWithValue("@iid", req.ItemId);
                cmd.Parameters.AddWithValue("@qty", req.Quantity);
                cmd.ExecuteNonQuery();
            }

            return Ok();
        }

        [HttpDelete("{userId:int}/{restaurantId:int}/items/{itemId:int}")]
        public IActionResult RemoveFromCart(int userId, int restaurantId, int itemId)
        {
            using var conn = new NpgsqlConnection(Database.connectionString);
            conn.Open();

            string cartSql = @"
                SELECT cart_id
                FROM carts
                WHERE user_id = @uid AND restaurant_id = @rid
                ORDER BY created_at DESC
                LIMIT 1";

            int cartId = 0;
            using (var cmd = new NpgsqlCommand(cartSql, conn))
            {
                cmd.Parameters.AddWithValue("@uid", userId);
                cmd.Parameters.AddWithValue("@rid", restaurantId);
                object? result = cmd.ExecuteScalar();
                if (result != null)
                {
                    cartId = (int)result;
                }
            }

            if (cartId == 0)
            {
                return Ok();
            }

            string checkItem = "SELECT cart_item_id, quantity FROM cart_items WHERE cart_id = @cid AND item_id = @iid";
            int cartItemId = 0;
            int currentQty = 0;

            using (var cmd = new NpgsqlCommand(checkItem, conn))
            {
                cmd.Parameters.AddWithValue("@cid", cartId);
                cmd.Parameters.AddWithValue("@iid", itemId);
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    cartItemId = reader.GetInt32(0);
                    currentQty = reader.GetInt32(1);
                }
            }

            if (cartItemId > 0)
            {
                if (currentQty > 1)
                {
                    string updateQty = "UPDATE cart_items SET quantity = @qty WHERE cart_item_id = @ciid";
                    using var cmd = new NpgsqlCommand(updateQty, conn);
                    cmd.Parameters.AddWithValue("@qty", currentQty - 1);
                    cmd.Parameters.AddWithValue("@ciid", cartItemId);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    string deleteItem = "DELETE FROM cart_items WHERE cart_item_id = @ciid";
                    using var cmd = new NpgsqlCommand(deleteItem, conn);
                    cmd.Parameters.AddWithValue("@ciid", cartItemId);
                    cmd.ExecuteNonQuery();
                }
            }

            return Ok();
        }
    }
}
