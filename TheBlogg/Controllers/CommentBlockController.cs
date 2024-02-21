using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using TheBlogg.Data;
using TheBlogg.Models;

namespace TheBlogg.Controllers
{
    [Route("api")]
    [ApiController]
    public class CommentBlockController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;

        public CommentBlockController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [HttpPut("block-comment")]
        public IActionResult BlockComment([FromForm] Comment model)
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
                    comment.IsBlocked = model.IsBlocked;
                    comment.IsReported = model.IsReported;
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
