using Microsoft.AspNetCore.Mvc;
using TheBlogg.Data;
using TheBlogg.Models;
using TheBlogg.Services;

namespace TheBlogg.Controllers
{
    [Route("api")]
    [ApiController]
    public class ArticleDeleteController : ControllerBase
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IImageService _imageService;

        public ArticleDeleteController(IArticleRepository articleRepository, IImageService imageService)
        {
            _articleRepository = articleRepository;
            _imageService = imageService;
        }

        [HttpDelete("delete-article")]
        public IActionResult DeleteArticle([FromForm] Article model)
        {
            if (ModelState.IsValid)
            {
                var article = _articleRepository.GetById(model.Id);

                if (article == null)
                {
                    return NotFound("article not found");
                }

                _imageService.DeleteImage(model.ImageUrl);

                _articleRepository.Delete(article);
                return Ok("article deleted");
            }
            else
            {
                return BadRequest("Invalid modelstate");
            }
        }
    }
}
