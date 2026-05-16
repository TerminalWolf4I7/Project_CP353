using Delivery.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace Delivery.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public ActionResult<LoginResponse> Login([FromBody] LoginRequest req)
        {
            using var conn = new NpgsqlConnection(Database.connectionString);
            conn.Open();

            string sql = "SELECT role FROM users WHERE user_id = @id";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", req.UserId);

            object? result = cmd.ExecuteScalar();
            if (result == null)
            {
                return NotFound("User ID not found");
            }

            return Ok(new LoginResponse(req.UserId, result.ToString() ?? string.Empty));
        }
    }
}
