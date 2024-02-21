using Microsoft.AspNetCore.Mvc;
using TheBlogg.Data;
using TheBlogg.Models;

namespace TheBlogg.Controllers
{
    [Route("api")]
    [ApiController]
    public class UpdateUserProfileController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserProfileController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPut("update-profile")]
        public IActionResult UpdateProfile(UpdateProfileModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userRepository.GetByEmail(model.EmailOld);
                if (user != null)
                {
                    user.Name = model.Name;
                    user.Email = model.Email;

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
