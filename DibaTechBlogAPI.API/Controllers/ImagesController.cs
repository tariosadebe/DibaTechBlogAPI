using DibaTechBlogAPI.Domain.Entities;
using DibaTechBlogAPI.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DibaTechBlogAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ImagesController : ControllerBase
    {
        private readonly BlogDbContext _context;
        private readonly IWebHostEnvironment _env;
        public ImagesController(BlogDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] DibaTechBlogAPI.API.Models.ImageUploadRequest request)
        {
            var post = await _context.Posts.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == request.PostId);
            if (post == null) return NotFound("Post not found");
            if (request.File == null || request.File.Length == 0) return BadRequest("No file uploaded");

            var uploads = Path.Combine(_env.ContentRootPath, "uploads");
            if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);
            var fileName = Guid.NewGuid() + Path.GetExtension(request.File.FileName);
            var filePath = Path.Combine(uploads, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.File.CopyToAsync(stream);
            }
            var image = new Image { Url = $"/uploads/{fileName}", PostId = request.PostId };
            post.Images.Add(image);
            await _context.SaveChangesAsync();
            return Ok(image);
        }
    }
}
