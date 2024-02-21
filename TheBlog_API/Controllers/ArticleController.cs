using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheBlog_API.Data;
using TheBlog_API.Models;

namespace TheBlog_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly TheBlogApiDbContext _context;

        public ArticleController(TheBlogApiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticles()
        {
            if (_context.Articles == null)
            {
                return NotFound();
            }

            return await _context.Articles.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Article>> GetArticle(int id)
        {
            if (_context.Articles == null)
            {
                return NotFound();
            }

            var article = await _context.Articles.FindAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            return article;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Article>> CreateArticle(CreateArticleModel model)
        {
            var article = new Article
            {
                Title = model.Title,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                Rating = 0,
                UserId = model.UserId
            };

            _context.Articles.Add(article);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetArticle), new { id = article.Id }, article);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult> UpdateArticle(ArticleUpdateModel model)
        {
            var article = await _context.Articles.FindAsync(model.Id);
            if (article == null)
            {
                return NotFound("article not found");
            }
            else
            {

                article.Title = model.Title;
                article.Description = model.Description;

                _context.Entry(article).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(article);

            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Article>> DeleteArticle(int id)
        {
            if (_context.Articles == null)
            {
                return NotFound();
            }

            var article = await _context.Articles.FindAsync(id);

            if (article == null)
            {
                return NotFound();
            }
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
            return Ok(); ;
        }
    }
}
