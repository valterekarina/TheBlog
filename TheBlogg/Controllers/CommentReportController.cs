using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheBlogg.Data;
using TheBlogg.Models;

namespace TheBlogg.Controllers
{
    [Route("api")]
    [ApiController]
    public class CommentReportController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;

        public CommentReportController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [HttpPut("report-comment")]
        public IActionResult ReportComment([FromForm] Comment model)
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
                    comment.IsReported = model.IsReported;
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
