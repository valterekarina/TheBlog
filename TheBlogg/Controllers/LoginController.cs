using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheBlogg.Data;
using TheBlogg.Models;
using TheBlogg.Services;

namespace TheBlogg.Controllers
{
    [Route("api")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly JWTService _jwtService;

        public LoginController(IUserRepository userRepository, JWTService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginModel model)
        {
            var user = _userRepository.GetByEmail(model.Email);

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

            //return Ok(new { message = "success" });
            return Ok(user);
        }
    }
}
