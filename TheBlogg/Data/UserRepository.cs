﻿using System.Linq;
using TheBlogg.Models;

namespace TheBlogg.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly TheBlogDbContext _context;

        public UserRepository(TheBlogDbContext context)
        {
            _context = context;
        }
        public User Create(User user)
        {
            _context.Users.Add(user);
            user.Id = _context.SaveChanges();
            return user;
        }

        public User GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public User GetById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public User Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
            return user;
        }

        public User Delete(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
            return user;
        }
    }
}
