namespace motel.Models.DTO
{
    public class TiersDTO
    {
        public int Id { get; set; }
        public string tiername { get; set; }
        public List<string> Users { get; set; }
    }
    public class TiersNoIdDTO
    {
        public string tiername { get; set; }
        public List<string>? Users { get; set; }
    }
    public class AddTiersDTO
    {
        public string tiername { get; set; }
    }
    public class TiersListResult
    {
        public List<TiersDTO> Tiers { get; set; }
        public int total { get; set; }
        public int TotalPages { get; set; }
    }
}
