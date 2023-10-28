using motel.Models.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace motel.Models.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string gender { get; set; }
        public string address { get; set; }
        public string password { get; set; }
        public string phone { get; set; }
        public int roleId { get; set; }
        public string? rolename { get; set; }
        public int tierId { get; set; }
        public string? tier { get; set; }
        public string? actualFile { get; set; }
        public string birthday { get; set; }
        public string datecreated { get; set; }
        //public List<string> post_manages { get; set; }
        public List<Post> posts { get; set; }

    }
    public class UserNoIdDTO
    {
        public string fullname { get; set; }
        public string gender { get; set; }
        public string address { get; set; }
        public string password { get; set; }
        public string phone { get; set; }
        public string rolename { get; set; }
        public string tier { get; set; }
        public string? actualFile { get; set; }
        public string birthday { get; set; }
        public string datecreated { get; set; }
        //public List<string> post_manages { get; set; }
        public List<PostDTO> posts { get; set; }
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
        //public DateTime birthday { get; set; }
        public string? birthday { get; set; }
        public DateTime datecreated { get; set; }
        public bool gender { get; set; }
        public IFormFile? FileUri { set; get; }
        public string? actualFile { get; set; }
    }
    public class UpdateUserBasic
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string address { get; set; }
        public bool gender { get; set; }
        public string? birthday { get; set; }
        public string phone { get; set; }
        public int tierId { get; set; }
        public int roleId { get; set; }
        //public IFormFile? FileUri { set; get; }
        //public string? actualFile { get; set; }

    }
    public class UserListResult
    {
        public List<UserDTO> Users { get; set; }
        public int total { get; set; }
        public int TotalPages { get; set; }
    }

    public class UserRoleDTO
    {
        public int Id { get; set; }
        public string fullname { get; set; }
        public string rolename { get; set; }
        public string phone { get; set; }
        public int roleId { get; set; }
    }
    public class UserRoleResult
    {
        public List<UserRoleDTO> Users { get; set; }
        public int total { get; set; }
        public int TotalPages { get; set; }
    }
    public class EditUserRoleDTO
    {
        public List<int> userids { get; set; }
        public int roleid { get; set; }
    }
}
