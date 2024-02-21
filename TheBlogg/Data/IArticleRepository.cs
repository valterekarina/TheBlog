using TheBlogg.Models;

namespace TheBlogg.Data
{
    public interface IArticleRepository
    {
        Article Create(Article article);
        Article GetById(int id);
        Article Update(Article article);
        Article Delete(Article article);
    }
}