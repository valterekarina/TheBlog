using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TheBlogg.Data;
using TheBlogg.Models;
using TheBlogg.Services;

namespace TheBlogg.Controllers
{
    [Route("api")]
    [ApiController]
    public class ArticleUpdateController : ControllerBase
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IImageService _imageService;

        public ArticleUpdateController(IArticleRepository articleRepository, IImageService imageService)
        {
            _articleRepository = articleRepository;
            _imageService = imageService;
        }

        [HttpPut("update-article")]
        public async Task<ActionResult> UpdateArticle([FromForm] Article model)
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
                    if (model.ImageFile == null)
                    {
                        article.Title = model.Title;
                        article.Description = model.Description;
                        //Comments = new List<Comment>()

                        _articleRepository.Update(article);
                        return Ok(article);
                    }
                    else
                    {
                        _imageService.DeleteImage(model.ImageUrl);

                        article.Title = model.Title;
                        article.Description = model.Description;
                        article.ImageUrl = await _imageService.SaveImage(model.ImageFile);
                        //Comments = new List<Comment>()

                        _articleRepository.Update(article);
                        return Ok(article);
                    }
                }
            }
            else
            {
                return BadRequest("invalid modelstate");
            }
        }
    }
}
