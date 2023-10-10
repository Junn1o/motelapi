using System.ComponentModel.DataAnnotations;
namespace motel.Models.Domain
{
    public class Post_Category
    {
        [Key]
        public int Id { get; set; }
        public int categoryId { get; set; }
        public Category category { get; set; }
        public int postId { get; set; }
        public Post post { get; set; }
    }
}