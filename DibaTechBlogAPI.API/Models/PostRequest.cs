using System.Collections.Generic;

namespace DibaTechBlogAPI.API.Models
{
    public class PostRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public List<int> CategoryIds { get; set; } = new();
        public List<int> TagIds { get; set; } = new();
    }
}
