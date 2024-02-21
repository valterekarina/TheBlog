using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace TheBlogg.Services
{
    public interface IImageService
    {
        Task<string> SaveImage(IFormFile imageFile);
        void DeleteImage(string imageUrl);
    }
}