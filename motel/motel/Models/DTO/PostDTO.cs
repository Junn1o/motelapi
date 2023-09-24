using motel.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace motel.Models.DTO
{
    public class PostDTO
    {
        public int Id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string address { get; set; }
        public decimal price { get; set; }
        public int area { get; set; }
        public string authorname { get; set; }
        public string status { get; set; }
        public string isHire { get; set; }
        public string actualFile { get; set; }
        public string FormattedDatecreated { get; set; }
        public string FormattedDateapprove { get; set; }
        public List<string> categorylist { get; set; }
    }
    public class PostNoIdDTO
    {
        public string title { get; set; }
        public string description { get; set; }
        public string address { get; set; }
        public decimal price { get; set; }
        public int area { get; set; }
        public string authorname { get; set; }
        public string status { get; set; }
        public string isHire { get; set; }
        public string actualFile { get; set; }
        public string FormattedDatecreated { get; set; }
        public string FormattedDateapprove { get; set; }
        public List<string> categorylist { get; set; }
    }
    public class AddPostDTO
    {
        public string title { get; set; }
        public string description { get; set; }
        public string address { get; set; }
        public decimal price { get; set; }
        public int area { get; set; }
        public int userId { get; set; }
        public string? status { get; set; }
        public bool? isHire { get; set; }
        public IFormFile[]? FileUri { set; get; }
        public string? actualFile { get; set; }
        public DateTime? dateCreated { get; set; }
        public List<int> categoryids { get; set; }
    }
    public class UpdatePost_Manage
    {
        public int adminId { get; set; }
        public DateTime dateApprove { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string address { get; set; }
        public decimal price { get; set; }
        public int area { get; set; }
        public int userId { get; set; }
        public string status { get; set; }
        public bool isHire { get; set; }
        public IFormFile[]? FileUri { set; get; }
        public string? actualFile { get; set; }
        public DateTime dateCreated { get; set; }
        public List<int> categoryids { get; set; }
    }
}
