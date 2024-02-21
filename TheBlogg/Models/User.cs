using System.Collections.Generic;

namespace TheBlogg.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public bool CanCreateArticle { get; set; }
        public bool CanComment { get; set; }
        public bool CanRank { get; set; }

        public List<Article> Articles { get; set; }
        public List<Comment> Comments { get; set; }
        public List<UserVote> UserVotes { get; set; }
    }
}
