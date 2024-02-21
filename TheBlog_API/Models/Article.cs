namespace TheBlog_API.Models
{
    public partial class Article
    {
        public Article()
        {
            Comments = new HashSet<Comment>();
        }

        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public int Rating { get; set; }
        public int? UserId { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
