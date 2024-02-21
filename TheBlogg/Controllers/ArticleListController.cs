using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBlogg.Data;
using TheBlogg.Models;

namespace TheBlogg.Controllers
{
    [Route("api")]
    [ApiController]
    public class ArticleListController : ControllerBase
    {
        private readonly TheBlogDbContext _dbContext;

        public ArticleListController(TheBlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("aricle-list")]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticleList()
        {
            if (_dbContext.Articles == null)
            {
                return NotFound();
            }

            return await _dbContext.Articles
                .Select(x => new Article()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    ImageUrl = x.ImageUrl,
                    Rating = x.Rating,
                    UserId = x.UserId,
                    Comments = x.Comments,
                    ImageSrc = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.ImageUrl)
                })
                .ToListAsync();
        }
    }
}
