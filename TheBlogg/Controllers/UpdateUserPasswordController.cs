using Microsoft.AspNetCore.Mvc;
using TheBlogg.Data;
using TheBlogg.Models;

namespace TheBlogg.Controllers
{
    [Route("api")]
    [ApiController]
    public class UpdateUserPasswordController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserPasswordController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPut("update-password")]
        public IActionResult UpdatePassword(UpdatePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userRepository.GetByEmail(model.EmailOld);
                if (user != null)
                {
                    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.PasswordHash);

                    _userRepository.Update(user);

                    return Ok(user);
                }
                else
                {
                    return NotFound("User Not Found");
                }
            }
            else
            {
                return BadRequest("Invalid modelstate");
            }
        }
    }
}
