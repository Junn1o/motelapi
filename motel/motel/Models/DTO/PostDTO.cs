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
        public bool isHire { get; set; }
        public string actualFile { get; set; }
        public string FormattedDatecreated { get; set; }
        public string FormattedDateapprove { get; set; }
        public List<string> categorylist { get; set; }
    }
}
