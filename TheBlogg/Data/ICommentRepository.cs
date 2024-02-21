using TheBlogg.Models;

namespace TheBlogg.Data
{
    public interface ICommentRepository
    {
        Comment Create(Comment comment);
        Comment GetById(int id);
        Comment Update(Comment comment);
        Comment Delete(Comment comment);
    }
}