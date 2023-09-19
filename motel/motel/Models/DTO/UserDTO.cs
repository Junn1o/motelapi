using motel.Models.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace motel.Models.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
       public string fullname { get; set; }
        public bool gender { get; set; }
        public string address { get; set; }
        public string password { get; set; }
        public string phone { get; set; }
        public string? rolename { get; set; }
        public string? tier { get; set; }
        public string? actualFile { get; set; }
        public string FormattedBirthday { get; set; }
        public string FormattedDatecreated { get; set; }
        //public List<string> post_manages { get; set; }
        //public List<string> posts { get; set; }

    }
    public class UserNoIdDTO
    {
        
        public string fullname { get; set; }
        public bool gender { get; set; }
        public string address { get; set; }
        public string password { get; set; }
        public string phone { get; set; }
        public string rolename { get; set; }
        public string tier { get; set; }
        public string? actualFile { get; set; }
        public string FormattedBirthday { get; set; }
        public string FormattedDatecreated { get; set; }
        //public List<string> post_manages { get; set; }
        //public List<string> posts { get; set; }
    }
    public class AddUserDTO
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string address { get; set; }
        public string password { get; set; }
        public string phone { get; set; }
        public int tierId { get; set; }
        public int roleId { get; set; }
        //public List<int>? post_manage_Id { get; set; }
        //public List<int>? postId { get; set; }
        public DateTime birthday { get; set; }
        public string FormattedBirthday { get; set; }
        public DateTime datecreated { get; set; }
        public bool gender { get; set; }
        [NotMapped]
        public IFormFile? FileUri { set; get; }
        public string? actualFile { get; set; }
    }
    public class LoginModel
    {
        public string Phone { get; set; }
        public string Password { get; set; }
    }
}
