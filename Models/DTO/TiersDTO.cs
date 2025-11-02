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
        public List<UserTierDTO>? ListUsers { get; set; }
    }
    public class UserTierDTO
    {
        public int Id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string birthday { get; set; }
        public string address { get; set; }
        public bool gender { get; set; }
        public string phone { get; set; }
        public string rolename { get; set; }
       // public RoleDTO Role { get; set; }

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
