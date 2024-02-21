namespace TheBlog_API.Models
{
    public partial class Comment
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsReported { get; set; }
        public bool IsBlocked { get; set; }
        public int? UserId { get; set; }
        public int ArticleId { get; set; }
        public bool? IsComplained { get; set; }

        public virtual Article Article { get; set; } = null!;
        public virtual User? User { get; set; }
    }
}
