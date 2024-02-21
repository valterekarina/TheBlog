namespace TheBlogg.Models
{
    public class UpdatePermissionsModel
    {
        public string Email { get; set; }
        public bool CanCreateArticle { get; set; }
        public bool CanComment { get; set; }
        public bool CanRank { get; set; }
    }
}
