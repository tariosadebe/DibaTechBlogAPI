using Microsoft.AspNetCore.Http;

namespace DibaTechBlogAPI.API.Models
{
    public class ImageUploadRequest
    {
        public int PostId { get; set; }
        public IFormFile File { get; set; } = null!;
    }
}
