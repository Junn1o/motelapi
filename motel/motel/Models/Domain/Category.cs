    using System.ComponentModel.DataAnnotations;
namespace motel.Models.Domain
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string name { get; set; }
        public List<Post_Category> post_category { get; set; }
    }
}
