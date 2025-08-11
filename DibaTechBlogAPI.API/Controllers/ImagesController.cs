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

        [HttpPost("upload/{postId}")]
        public async Task<IActionResult> Upload(int postId, IFormFile file)
        {
            var post = await _context.Posts.FindAsync(postId);
            if (post == null) return NotFound();
            if (file == null || file.Length == 0) return BadRequest("No file uploaded.");

            var uploads = Path.Combine(_env.ContentRootPath, "uploads");
            if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploads, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var image = new Image { Url = $"/uploads/{fileName}", PostId = postId };
            _context.Images.Add(image);
            await _context.SaveChangesAsync();
            return Ok(image);
        }

        [HttpGet("{postId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetImages(int postId)
        {
            var images = await _context.Images.Where(i => i.PostId == postId).ToListAsync();
            return Ok(images);
        }
    }
}
