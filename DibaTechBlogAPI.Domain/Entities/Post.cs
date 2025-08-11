namespace DibaTechBlogAPI.Domain.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ICollection<Image> Images { get; set; } = new List<Image>();
        public ICollection<PostCategory> PostCategories { get; set; } = new List<PostCategory>();
        public ICollection<PostTag> PostTags { get; set; } = new List<PostTag>();
    }
}
