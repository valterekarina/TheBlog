using System.Linq;
using TheBlogg.Models;

namespace TheBlogg.Data
{
    public class CommentRepository : ICommentRepository
    {
        private readonly TheBlogDbContext _context;

        public CommentRepository(TheBlogDbContext context)
        {
            _context = context;
        }

        public Comment Create(Comment comment)
        {
            _context.Comments.Add(comment);
            comment.Id = _context.SaveChanges();
            return comment;
        }

        public Comment GetById(int id)
        {
            return _context.Comments.FirstOrDefault(a => a.Id == id);
        }

        public Comment Update(Comment comment)
        {
            _context.Comments.Update(comment);
            _context.SaveChanges();
            return comment;
        }

        public Comment Delete(Comment comment)
        {
            _context.Comments.Remove(comment);
            _context.SaveChanges();
            return comment;
        }
    }
}
