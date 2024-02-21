using Microsoft.AspNetCore.Mvc;
using System;
using TheBlogg.Data;
using TheBlogg.Models;

namespace TheBlogg.Controllers
{
    [Route("api")]
    [ApiController]
    public class CommentUpdateController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;

        public CommentUpdateController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [HttpPut("update-comment")]
        public IActionResult UpdateComment([FromForm] Comment model)
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
                    comment.Text = model.Text;
                    comment.CreatedAt = DateTime.Now;
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
