using Microsoft.AspNetCore.Mvc;
using TheBlog_API.Data;
using TheBlog_API.Models;
using TheBlog_API.Services;

namespace TheBlog_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly TheBlogApiDbContext _context;
        private readonly JWTService _jwtService;

        public AuthenticationController(TheBlogApiDbContext context, JWTService jwtService)
        {
            _jwtService = jwtService;
            _context = context;
        }

        [HttpPost]
        public IActionResult Login(Autentication model)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);

            if (user == null)
            {
                return BadRequest(new { message = "Invalid Email" });
            }

            if (!BCrypt.Net.BCrypt.Verify(model.PasswordHash, user.PasswordHash))
            {
                return BadRequest(new { message = "Invalid Password" });
            }

            var jwt = _jwtService.Generate(user.Id);

            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true,
            });
            return Ok(jwt);
        }
    }
}
