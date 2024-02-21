using Microsoft.AspNetCore.Mvc;
using TheBlogg.Data;
using TheBlogg.Models;

namespace TheBlogg.Controllers
{
    [Route("api")]
    [ApiController]
    public class CommentDeleteController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;

        public CommentDeleteController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [HttpDelete("delete-comment")]
        public IActionResult Delete([FromForm] Comment model)
        {
            if (ModelState.IsValid)
            {
                var comment = _commentRepository.GetById(model.Id);

                if (comment == null)
                {
                    return NotFound("comment not found");
                }

                _commentRepository.Delete(comment);
                return Ok("comment deleted");
            }
            else
            {
                return BadRequest("Invalid modelstate");
            }
        }
    }
}
