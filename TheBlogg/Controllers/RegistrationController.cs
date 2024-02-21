using Microsoft.AspNetCore.Mvc;
using TheBlogg.Data;
using TheBlogg.Models;

namespace TheBlogg.Controllers
{
    [Route("api")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public RegistrationController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        public IActionResult Register(RegistrationModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Name = model.Name,
                    Email = model.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.PasswordHash),
                    Role = "user",
                    CanCreateArticle = false,
                    CanComment = false,
                    CanRank = false
                };

                return Created("success", _userRepository.Create(user));
            }
            else
            {
                return BadRequest("fail");
            }
        }
    }
}
