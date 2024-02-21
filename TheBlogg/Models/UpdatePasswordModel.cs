namespace TheBlogg.Models
{
    public class UpdatePasswordModel
    {
        public string EmailOld { get; set; }
        public string PasswordHash { get; set; }
    }
}
