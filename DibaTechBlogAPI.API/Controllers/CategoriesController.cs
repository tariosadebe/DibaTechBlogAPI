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
    public class CategoriesController : ControllerBase
    {
        private readonly BlogDbContext _context;
        public CategoriesController(BlogDbContext context) { _context = context; }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll() => Ok(await _context.Categories.ToListAsync());

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAll), new { id = category.Id }, category);
        }
    }
}
