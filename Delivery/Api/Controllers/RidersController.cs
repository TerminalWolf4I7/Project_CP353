using Delivery.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace Delivery.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class RidersController : ControllerBase
    {
        [HttpGet("by-user/{userId:int}")]
        public ActionResult<int> GetRiderIdByUser(int userId)
        {
            using var conn = new NpgsqlConnection(Database.connectionString);
            conn.Open();

            string sql = "SELECT rider_id FROM riders WHERE user_id = @id";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", userId);

            object? result = cmd.ExecuteScalar();
            if (result == null)
            {
                return NotFound();
            }

            return Ok(Convert.ToInt32(result));
        }

        [HttpGet("{userId:int}/current-order")]
        public ActionResult<RiderCurrentOrderDto> GetCurrentOrder(int userId)
        {
            using var conn = new NpgsqlConnection(Database.connectionString);
            conn.Open();

            string sql = @"
                SELECT o.order_id, o.user_id, u.name, o.restaurant_id, o.status
                FROM orders o
                JOIN riders r ON o.rider_id = r.rider_id
                JOIN users u ON o.user_id = u.user_id
                WHERE r.user_id = @id
                  AND o.status = 'Delivering'
                ORDER BY o.order_id
                LIMIT 1";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", userId);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return NotFound();
            }

            var dto = new RiderCurrentOrderDto(
                reader.GetInt32(0),
                reader.GetInt32(1),
                reader.GetString(2),
                reader.GetInt32(3),
                reader.GetString(4));

            return Ok(dto);
        }

        [HttpPost("{userId:int}/complete/{orderId:int}")]
        public IActionResult CompleteOrder(int userId, int orderId)
        {
            using var conn = new NpgsqlConnection(Database.connectionString);
            conn.Open();

            using var tx = conn.BeginTransaction();

            string updateOrderSql = @"
                UPDATE orders
                SET status = 'Success'
                WHERE order_id = @order
                  AND rider_id = (SELECT rider_id FROM riders WHERE user_id = @id)";

            int affected;
            using (var updateOrderCmd = new NpgsqlCommand(updateOrderSql, conn, tx))
            {
                updateOrderCmd.Parameters.AddWithValue("@order", orderId);
                updateOrderCmd.Parameters.AddWithValue("@id", userId);
                affected = updateOrderCmd.ExecuteNonQuery();
            }

            if (affected == 0)
            {
                tx.Rollback();
                return NotFound("Order not found");
            }

            string riderSql = "UPDATE riders SET status = 'Available' WHERE user_id = @id";
            using (var riderCmd = new NpgsqlCommand(riderSql, conn, tx))
            {
                riderCmd.Parameters.AddWithValue("@id", userId);
                riderCmd.ExecuteNonQuery();
            }

            tx.Commit();
            return Ok();
        }
    }
}
