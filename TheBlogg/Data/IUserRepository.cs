using TheBlogg.Models;

namespace TheBlogg.Data
{
    public interface IUserRepository
    {
        User Create(User user);
        User GetByEmail(string email);
        User GetById(int id);
        User Update(User user);
        User Delete(User user);
    }
}
