using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBlogg.Models
{
    public class CreateArticleModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int UserId { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
