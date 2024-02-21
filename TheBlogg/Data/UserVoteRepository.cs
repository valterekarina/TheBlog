using System.Linq;
using TheBlogg.Models;

namespace TheBlogg.Data
{
    public class UserVoteRepository : IUserVoteRepository
    {
        private readonly TheBlogDbContext _context;

        public UserVoteRepository(TheBlogDbContext context)
        {
            _context = context;
        }
        public UserVote Create(UserVote userVote)
        {
            _context.UserVotes.Add(userVote);
            userVote.Id = _context.SaveChanges();
            return userVote;
        }

        public UserVote GetById(int id)
        {
            return _context.UserVotes.FirstOrDefault(u => u.Id == id);
        }

        public UserVote Update(UserVote userVote)
        {
            _context.UserVotes.Update(userVote);
            _context.SaveChanges();
            return userVote;
        }

        public UserVote Delete(UserVote userVote)
        {
            _context.UserVotes.Remove(userVote);
            _context.SaveChanges();
            return userVote;
        }
    }
}
