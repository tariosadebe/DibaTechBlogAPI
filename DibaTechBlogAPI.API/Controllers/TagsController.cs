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
    public class TagsController : ControllerBase
    {
        private readonly BlogDbContext _context;
        public TagsController(BlogDbContext context) { _context = context; }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll() => Ok(await _context.Tags.ToListAsync());

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Tag tag)
        {
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAll), new { id = tag.Id }, tag);
        }
    }
}
