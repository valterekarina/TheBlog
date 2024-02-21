using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TheBlogg.Data;
using TheBlogg.Models;
using TheBlogg.Services;

namespace TheBlogg.Controllers
{
    [Route("api")]
    [ApiController]
    public class CreateArticleController : ControllerBase
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IImageService _imageService;

        public CreateArticleController(IArticleRepository articleRepository, IUserRepository userRepository, IImageService imageService)
        {
            _articleRepository = articleRepository;
            _userRepository = userRepository;
            _imageService = imageService;
        }

        [HttpPost("create-article")]
        public async Task<ActionResult> CreateArticle([FromForm] CreateArticleModel model)
        {
            if (ModelState.IsValid)
            {
                var article = new Article
                {
                    Title = model.Title,
                    Description = model.Description,
                    ImageUrl = await _imageService.SaveImage(model.ImageFile), //model.ImageUrl,//await _articleRepository.SaveImage(model.ImageFile),
                    Rating = 0,
                    UserId = model.UserId,
                    User = _userRepository.GetById(model.UserId),
                    //Comments = new List<Comment>()
                };

                return Created("success", _articleRepository.Create(article));
            }
            else
            {
                return BadRequest("invalid modelstate");
            }
        }
    }
}
