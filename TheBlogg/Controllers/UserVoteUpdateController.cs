using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using TheBlogg.Data;
using TheBlogg.Models;

namespace TheBlogg.Controllers
{
    [Route("api")]
    [ApiController]
    public class UserVoteUpdateController : ControllerBase
    {
        private readonly IUserVoteRepository _userVoteRepository;

        public UserVoteUpdateController(IUserVoteRepository userVoteRepository)
        {
            _userVoteRepository = userVoteRepository;
        }

        [HttpPut("update-userVote")]
        public IActionResult UpdateComment([FromForm] UserVote model)
        {
            if (ModelState.IsValid)
            {
                var userVote = _userVoteRepository.GetById(model.Id);
                
                if (userVote == null)
                {
                    userVote = new UserVote();
                    userVote.Vote = 0+model.Vote;
                    userVote.ArticleId = model.ArticleId;
                    userVote.UserId = model.UserId;

                    _userVoteRepository.Create(userVote);
                    return Ok(userVote);
                }
                else
                {
                    userVote.Vote = userVote.Vote + model.Vote;

                    _userVoteRepository.Update(userVote);
                    return Ok(userVote);
                }
            }
            else
            {
                return BadRequest("invalid modelstate");
            }
        }
    }
}
