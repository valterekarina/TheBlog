using TheBlogg.Models;

namespace TheBlogg.Data
{
    public interface IUserVoteRepository
    {
        UserVote Create(UserVote userVote);
        UserVote Delete(UserVote userVote);
        UserVote GetById(int id);
        UserVote Update(UserVote userVote);
    }
}