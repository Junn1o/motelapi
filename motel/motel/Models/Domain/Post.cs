
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace motel.Models.Domain
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public int userId { get; set; }
        public User user { get; set; }
        public Post_Manage post_manage { get; set; }
        public List<Post_Category> post_category {get; set; }
        public string status { get; set; }
        public bool isHire { get; set; }
        [DataType(DataType.Date)]
        public DateTime datecreatedroom { get; set; }
        public int area { get; set; }
        public string address { get; set; }
        [NotMapped]
        public IFormFile[]? FileUri { set; get; }
        public string? actualFile { get; set; }
    }
}
