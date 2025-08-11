using DibaTechBlogAPI.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DibaTechBlogAPI.Infrastructure.Data
{
    public class BlogDbContext : IdentityDbContext<AppUser>
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options) { }

        public DbSet<Post> Posts => Set<Post>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<Image> Images => Set<Image>();
        public DbSet<PostCategory> PostCategories => Set<PostCategory>();
        public DbSet<PostTag> PostTags => Set<PostTag>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<PostCategory>().HasKey(pc => new { pc.PostId, pc.CategoryId });
            modelBuilder.Entity<PostTag>().HasKey(pt => new { pt.PostId, pt.TagId });
        }
    }
}
