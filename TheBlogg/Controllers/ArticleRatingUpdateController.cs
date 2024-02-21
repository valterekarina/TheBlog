using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TheBlogg.Data;
using TheBlogg.Models;
using TheBlogg.Services;

namespace TheBlogg.Controllers
{
    [Route("api")]
    [ApiController]
    public class ArticleRatingUpdateController : ControllerBase
    {
        private readonly IArticleRepository _articleRepository;

        public ArticleRatingUpdateController(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        [HttpPut("update-rating")]
        public IActionResult UpdateArticle([FromForm] ArticleRatingModel model)
        {
            if (ModelState.IsValid)
            {
                var article = _articleRepository.GetById(model.Id);
                if (article == null)
                {
                    return NotFound("article not found");
                }
                else
                {
                    article.Rating = model.Rating;
                    //Comments = new List<Comment>()

                    _articleRepository.Update(article);
                    return Ok(article);
                }
            }
            else
            {
                return BadRequest("invalid modelstate");
            }
        }
    }
}
