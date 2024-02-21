using System.Linq;
using TheBlogg.Models;

namespace TheBlogg.Data
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly TheBlogDbContext _context;

        public ArticleRepository(TheBlogDbContext context)
        {
            _context = context;
        }

        public Article Create(Article article)
        {
            _context.Articles.Add(article);
            article.Id = _context.SaveChanges();
            return article;
        }

        public Article GetById(int id)
        {
            return _context.Articles.FirstOrDefault(a => a.Id == id);
        }

        public Article Update(Article article)
        {
            _context.Articles.Update(article);
            _context.SaveChanges();
            return article;
        }

        public Article Delete(Article article)
        {
            _context.Articles.Remove(article);
            _context.SaveChanges();
            return article;
        }
    }
}
