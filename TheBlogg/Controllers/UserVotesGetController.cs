using Microsoft.AspNetCore.Http;
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
    public class UserVotesGetController : ControllerBase
    {
        private readonly TheBlogDbContext _dbContext;

        public UserVotesGetController(TheBlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("get-user-votes")]
        public async Task<ActionResult<IEnumerable<UserVote>>> GetUserVote()
        {
            if (_dbContext.UserVotes == null)
            {
                return NotFound();
            }

            return await _dbContext.UserVotes.ToListAsync();
        }
    }
}
