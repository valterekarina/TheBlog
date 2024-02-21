namespace TheBlog_API.Models
{
    public partial class User
    {
        public User()
        {
            Articles = new HashSet<Article>();
            Comments = new HashSet<Comment>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? Role { get; set; }
        public bool CanCreateArticle { get; set; }
        public bool CanComment { get; set; }
        public bool CanRank { get; set; }

        public virtual ICollection<Article> Articles { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
