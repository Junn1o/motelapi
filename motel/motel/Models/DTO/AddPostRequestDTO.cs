namespace motel.Models.DTO
{
    public class AddPostRequestDTO
    {
        public string title { get; set; }
        public string description { get; set; }
        public string address { get; set; }
        public decimal price { get; set; }
        public int area { get; set; }
        public int authorid { get; set; }
        public string status { get; set; }
        public bool isHire { get; set; }
        public IFormFile[] FileUri { set; get; }
        public string actualFile { get; set; }
        public DateTime dateCreated { get; set; }
        public List<int> categoryids { get; set; }
    }
}
