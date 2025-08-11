namespace DibaTechBlogAPI.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<PostCategory> PostCategories { get; set; } = new List<PostCategory>();
    }
}
