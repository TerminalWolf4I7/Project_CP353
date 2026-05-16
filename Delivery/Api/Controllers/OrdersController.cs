using System;
using System.Linq;
using Delivery.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace Delivery.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class OrdersController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<OrderDto>> GetOrders([FromQuery] int? userId, [FromQuery] int? restaurantId, [FromQuery] int? riderId, [FromQuery] string? status)
        {
            var list = new List<OrderDto>();

            using var conn = new NpgsqlConnection(Database.connectionString);
            conn.Open();

            string sql = @"
                SELECT order_id, user_id, restaurant_id, rider_id, total_price, status
                FROM orders
                WHERE (@uid IS NULL OR user_id = @uid)
                  AND (@rid IS NULL OR restaurant_id = @rid)
                  AND (@rider IS NULL OR rider_id = @rider)
                  AND (@status IS NULL OR status = @status)
                ORDER BY order_id";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.Add(new NpgsqlParameter<int?>("@uid", userId));
            cmd.Parameters.Add(new NpgsqlParameter<int?>("@rid", restaurantId));
            cmd.Parameters.Add(new NpgsqlParameter<int?>("@rider", riderId));
            cmd.Parameters.Add(new NpgsqlParameter<string?>("@status", status));

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new OrderDto(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetInt32(2),
                    reader.IsDBNull(3) ? null : reader.GetInt32(3),
                    reader.GetDecimal(4),
                    reader.GetString(5)));
            }

            return Ok(list);
        }

        [HttpGet("{orderId:int}/details")]
        public ActionResult<OrderDetailDto> GetOrderDetails(int orderId)
        {
            using var conn = new NpgsqlConnection(Database.connectionString);
            conn.Open();

            string sql = @"
                SELECT o.order_id, o.user_id, u.name, o.restaurant_id, o.total_price, o.status
                FROM orders o
                JOIN users u ON o.user_id = u.user_id
                WHERE o.order_id = @id";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", orderId);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return NotFound();
            }

            var dto = new OrderDetailDto(
                reader.GetInt32(0),
                reader.GetInt32(1),
                reader.GetString(2),
                reader.GetInt32(3),
                reader.GetDecimal(4),
                reader.GetString(5));

            return Ok(dto);
        }

        [HttpGet("{orderId:int}")]
        public ActionResult<List<OrderItemDto>> GetOrderItems(int orderId)
        {
            var list = new List<OrderItemDto>();

            using var conn = new NpgsqlConnection(Database.connectionString);
            conn.Open();

            string sql = @"
                SELECT m.name, oi.quantity, oi.price, oi.item_id
                FROM order_items oi
                JOIN menu_items m ON oi.item_id = m.item_id
                WHERE oi.order_id = @id";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", orderId);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new OrderItemDto(
                    reader.GetInt32(3),
                    reader.GetString(0),
                    reader.GetInt32(1),
                    reader.GetDecimal(2)));
            }

            return Ok(list);
        }

        [HttpPost("checkout")]
        public ActionResult<int> Checkout([FromBody] CheckoutRequest req)
        {
            using var conn = new NpgsqlConnection(Database.connectionString);
            conn.Open();

            using var trans = conn.BeginTransaction();

            string cartSql = @"
                SELECT cart_id
                FROM carts
                WHERE user_id = @uid AND restaurant_id = @rid
                ORDER BY created_at DESC
                LIMIT 1";

            int cartId;
            using (var cmd = new NpgsqlCommand(cartSql, conn, trans))
            {
                cmd.Parameters.AddWithValue("@uid", req.UserId);
                cmd.Parameters.AddWithValue("@rid", req.RestaurantId);
                object? result = cmd.ExecuteScalar();

                if (result == null)
                {
                    return BadRequest("Cart not found");
                }

                cartId = (int)result;
            }

            string itemsSql = @"
                SELECT ci.item_id, ci.quantity, mi.price
                FROM cart_items ci
                JOIN menu_items mi ON ci.item_id = mi.item_id
                WHERE ci.cart_id = @cid";

            var items = new List<(int ItemId, int Qty, decimal Price)>();
            using (var cmd = new NpgsqlCommand(itemsSql, conn, trans))
            {
                cmd.Parameters.AddWithValue("@cid", cartId);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    items.Add((reader.GetInt32(0), reader.GetInt32(1), reader.GetDecimal(2)));
                }
            }

            if (items.Count == 0)
            {
                return BadRequest("Cart empty");
            }

            decimal total = items.Sum(i => i.Price * i.Qty);

            string createOrder = @"
                INSERT INTO orders (user_id, restaurant_id, total_price, status)
                VALUES (@uid, @rid, @price, 'Pending')
                RETURNING order_id";

            int orderId;
            using (var cmd = new NpgsqlCommand(createOrder, conn, trans))
            {
                cmd.Parameters.AddWithValue("@uid", req.UserId);
                cmd.Parameters.AddWithValue("@rid", req.RestaurantId);
                cmd.Parameters.AddWithValue("@price", total);
                orderId = (int)cmd.ExecuteScalar();
            }

            foreach (var item in items)
            {
                string orderItem = @"
                    INSERT INTO order_items (order_id, item_id, quantity, price)
                    VALUES (@oid, @iid, @qty, @price)";
                using var cmd = new NpgsqlCommand(orderItem, conn, trans);
                cmd.Parameters.AddWithValue("@oid", orderId);
                cmd.Parameters.AddWithValue("@iid", item.ItemId);
                cmd.Parameters.AddWithValue("@qty", item.Qty);
                cmd.Parameters.AddWithValue("@price", item.Price);
                cmd.ExecuteNonQuery();
            }

            string deleteCartItems = "DELETE FROM cart_items WHERE cart_id = @cid";
            using (var cmd = new NpgsqlCommand(deleteCartItems, conn, trans))
            {
                cmd.Parameters.AddWithValue("@cid", cartId);
                cmd.ExecuteNonQuery();
            }

            string deleteCart = "DELETE FROM carts WHERE cart_id = @cid";
            using (var cmd = new NpgsqlCommand(deleteCart, conn, trans))
            {
                cmd.Parameters.AddWithValue("@cid", cartId);
                cmd.ExecuteNonQuery();
            }

            trans.Commit();

            return Ok(orderId);
        }

        [HttpPatch("{orderId:int}/status")]
        public IActionResult UpdateStatus(int orderId, [FromBody] UpdateStatusRequest req)
        {
            using var conn = new NpgsqlConnection(Database.connectionString);
            conn.Open();

            string sql = "UPDATE orders SET status = @status WHERE order_id = @id";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@status", req.Status);
            cmd.Parameters.AddWithValue("@id", orderId);
            cmd.ExecuteNonQuery();

            return Ok();
        }

        [HttpDelete("{orderId:int}")]
        public IActionResult DeleteOrder(int orderId)
        {
            using var conn = new NpgsqlConnection(Database.connectionString);
            conn.Open();

            using var tx = conn.BeginTransaction();

            string deleteItems = "DELETE FROM order_items WHERE order_id = @id";
            using (var cmd = new NpgsqlCommand(deleteItems, conn, tx))
            {
                cmd.Parameters.AddWithValue("@id", orderId);
                cmd.ExecuteNonQuery();
            }

            string deleteOrder = "DELETE FROM orders WHERE order_id = @id";
            using (var cmd = new NpgsqlCommand(deleteOrder, conn, tx))
            {
                cmd.Parameters.AddWithValue("@id", orderId);
                cmd.ExecuteNonQuery();
            }

            tx.Commit();
            return Ok();
        }

        [HttpPatch("{orderId:int}/accept-rider/{userId:int}")]
        public IActionResult AcceptRiderOrder(int orderId, int userId)
        {
            using var conn = new NpgsqlConnection(Database.connectionString);
            conn.Open();

            int riderId = GetRiderId(conn, userId);

            using var tx = conn.BeginTransaction();

            string orderSql = @"
                UPDATE orders
                SET rider_id = @rider,
                    status = 'Delivering'
                WHERE order_id = @order
                  AND rider_id IS NULL";

            using var orderCmd = new NpgsqlCommand(orderSql, conn, tx);
            orderCmd.Parameters.AddWithValue("@rider", riderId);
            orderCmd.Parameters.AddWithValue("@order", orderId);

            int affected = orderCmd.ExecuteNonQuery();
            if (affected == 0)
            {
                tx.Rollback();
                return Conflict("Order already taken");
            }

            string riderSql = @"
                UPDATE riders
                SET status = 'Delivering'
                WHERE user_id = @id";

            using var riderCmd = new NpgsqlCommand(riderSql, conn, tx);
            riderCmd.Parameters.AddWithValue("@id", userId);
            riderCmd.ExecuteNonQuery();

            tx.Commit();
            return Ok();
        }

        private static int GetRiderId(NpgsqlConnection conn, int userId)
        {
            string sql = @"
                SELECT rider_id
                FROM riders
                WHERE user_id=@id";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", userId);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }
    }
}
