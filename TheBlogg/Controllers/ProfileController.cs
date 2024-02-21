using Microsoft.AspNetCore.Mvc;
using TheBlogg.Data;
using TheBlogg.Services;

namespace TheBlogg.Controllers
{
    [Route("api")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly JWTService _jwtService;

        public ProfileController(IUserRepository userRepository, JWTService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        [HttpGet("profile")]
        public IActionResult Profile()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                var token = _jwtService.Verify(jwt);
                int userId = int.Parse(token.Issuer);
                var user = _userRepository.GetById(userId);

                return Ok(user);
            }
            catch
            {
                return Unauthorized();
            }

        }
    }
}
