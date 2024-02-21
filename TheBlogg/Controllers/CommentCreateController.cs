using Microsoft.AspNetCore.Mvc;
using System;
using TheBlogg.Data;
using TheBlogg.Models;

namespace TheBlogg.Controllers
{
    [Route("api")]
    [ApiController]
    public class CommentCreateController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IArticleRepository _articleRepository;

        public CommentCreateController(ICommentRepository commentRepository, IArticleRepository articleRepository)
        {
            _commentRepository = commentRepository;
            _articleRepository = articleRepository;
        }

        [HttpPost("add-comment")]
        public IActionResult AddComment([FromForm] Comment model)
        {
            if (ModelState.IsValid)
            {
                var article = _articleRepository.GetById(model.ArticleId);

                if (article == null)
                {
                    return NotFound("Article not found");
                }

                var comment = new Comment
                {
                    Text = model.Text,
                    CreatedAt = DateTime.Now,
                    IsReported = false,
                    IsBlocked = false,
                    IsComplained = false,
                    UserId = model.UserId,
                    ArticleId = model.ArticleId
                };

                _commentRepository.Create(comment);
                return Ok(comment);
            }
            else
            {
                return BadRequest("invalid modelstate");
            }
        }
    }
}
