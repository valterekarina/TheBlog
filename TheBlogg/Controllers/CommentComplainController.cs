using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheBlogg.Data;
using TheBlogg.Models;

namespace TheBlogg.Controllers
{
    [Route("api")]
    [ApiController]
    public class CommentComplainController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;

        public CommentComplainController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [HttpPut("complain-comment")]
        public IActionResult ComplainComment([FromForm] Comment model)
        {
            if (ModelState.IsValid)
            {
                var comment = _commentRepository.GetById(model.Id);
                if (comment == null)
                {
                    return NotFound("comment not found");
                }
                else
                {
                    comment.IsComplained = model.IsComplained;
                    _commentRepository.Update(comment);
                    return Ok(comment);
                }
            }
            else
            {
                return BadRequest("invalid modelstate");
            }
        }
    }
}
