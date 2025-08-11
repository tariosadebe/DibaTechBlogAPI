namespace DibaTechBlogAPI.Domain.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<PostTag> PostTags { get; set; } = new List<PostTag>();
    }
}
