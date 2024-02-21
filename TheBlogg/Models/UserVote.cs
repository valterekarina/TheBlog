namespace TheBlogg.Models
{
    public class UserVote
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int ArticleId { get; set; }
        public Article Article { get; set; }

        public int Vote {  get; set; }
    }
}
