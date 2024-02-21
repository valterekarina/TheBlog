using System;

namespace TheBlogg.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsReported { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsComplained {  get; set; }

        public int? UserId { get; set; }
        public User User { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
    }
}
