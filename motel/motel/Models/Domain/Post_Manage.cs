using System.ComponentModel.DataAnnotations;

namespace motel.Models.Domain
{
    public class Post_Manage
    {
        [Key] public int Id { get; set; }
        public int postId { get; set; }
        public Post post { get; set; }
        public int userAdminId { get; set; }
        public User user { get; set; }
        public DateTime dateapproved { get; set; }
    }
}
