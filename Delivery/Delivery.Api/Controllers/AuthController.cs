using Delivery.Api.Models;
using Delivery.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class AuthController : ControllerBase
    {
        private readonly DeliveryDbContext _db;

        public AuthController(DeliveryDbContext db)
        {
            _db = db;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest req)
        {
            var user = await _db.Users.FindAsync(req.UserId);
            if (user == null)
            {
                return NotFound("User ID not found");
            }

            return Ok(new LoginResponse(user.UserId, user.Role));
        }
    }
}
