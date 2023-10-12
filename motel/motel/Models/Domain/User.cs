using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace motel.Models.Domain
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string address { get; set; }
        public string password { get; set; }
        public string phone { get; set; }
        public Tier_User users_tier { get; set; }
        public Role role { get; set; }
        public int roleId { get; set; }
        public List<Post_Manage> post_manage { get; set; }   
        public List<Post> post { get; set; }
        [DataType(DataType.Date)]
        public DateTime birthday { get; set; }
        [DataType(DataType.Date)]
        public DateTime datecreated { get; set; }
        public bool gender { get; set; }
        [NotMapped]
        public IFormFile? FileUri { set; get; }
        public string? actualFile { get; set; }
    }
}
