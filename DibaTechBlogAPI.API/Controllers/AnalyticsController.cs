using DibaTechBlogAPI.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DibaTechBlogAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AnalyticsController : ControllerBase
    {
        private readonly BlogDbContext _context;
        public AnalyticsController(BlogDbContext context) { _context = context; }

        [HttpGet("counts")]
        public async Task<IActionResult> GetCounts()
        {
            var postCount = await _context.Posts.CountAsync();
            var categoryCount = await _context.Categories.CountAsync();
            var tagCount = await _context.Tags.CountAsync();
            var imageCount = await _context.Images.CountAsync();
            return Ok(new { postCount, categoryCount, tagCount, imageCount });
        }
    }
}
