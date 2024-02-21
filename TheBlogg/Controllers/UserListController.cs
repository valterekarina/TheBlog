using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheBlogg.Data;
using TheBlogg.Models;

namespace TheBlogg.Controllers
{
    [Route("api")]
    [ApiController]
    public class UserListController : ControllerBase
    {
        private readonly TheBlogDbContext _dbContext;
        private readonly IUserRepository _userRepository;

        public UserListController(TheBlogDbContext dbContext, IUserRepository userRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
        }

        [HttpGet("get-users")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            if (_dbContext.Users == null)
            {
                return NotFound();
            }

            return await _dbContext.Users.ToListAsync();
        }

        [HttpPut("update-permissions")]
        public IActionResult UpdateProfile(UpdatePermissionsModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userRepository.GetByEmail(model.Email);
                if (user != null)
                {
                    user.CanCreateArticle = model.CanCreateArticle;
                    user.CanComment = model.CanComment;
                    user.CanRank = model.CanRank;

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
