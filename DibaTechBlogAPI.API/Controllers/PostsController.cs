using DibaTechBlogAPI.API.Models;
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
    public class PostsController : ControllerBase
    {
        private readonly BlogDbContext _context;
        public PostsController(BlogDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var posts = await _context.Posts
                .Include(p => p.Images)
                .Include(p => p.PostCategories).ThenInclude(pc => pc.Category)
                .Include(p => p.PostTags).ThenInclude(pt => pt.Tag)
                .ToListAsync();
            return Ok(posts);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id)
        {
            var post = await _context.Posts
                .Include(p => p.Images)
                .Include(p => p.PostCategories).ThenInclude(pc => pc.Category)
                .Include(p => p.PostTags).ThenInclude(pt => pt.Tag)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (post == null) return NotFound();
            return Ok(post);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PostRequest request)
        {
            var post = new Post
            {
                Title = request.Title,
                Content = request.Content,
                CreatedAt = DateTime.UtcNow
            };
            foreach (var catId in request.CategoryIds)
            {
                post.PostCategories.Add(new PostCategory { CategoryId = catId });
            }
            foreach (var tagId in request.TagIds)
            {
                post.PostTags.Add(new PostTag { TagId = tagId });
            }
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = post.Id }, post);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PostRequest request)
        {
            var post = await _context.Posts
                .Include(p => p.PostCategories)
                .Include(p => p.PostTags)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (post == null) return NotFound();
            post.Title = request.Title;
            post.Content = request.Content;
            post.UpdatedAt = DateTime.UtcNow;
            post.PostCategories.Clear();
            post.PostTags.Clear();
            foreach (var catId in request.CategoryIds)
            {
                post.PostCategories.Add(new PostCategory { CategoryId = catId });
            }
            foreach (var tagId in request.TagIds)
            {
                post.PostTags.Add(new PostTag { TagId = tagId });
            }
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null) return NotFound();
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
