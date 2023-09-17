namespace motel.Models.DTO
{
    public class RoleDTO
    {
        public int Id { get; set; }
        public string rolename { get; set; }
        public List<string> Users { get; set; }
    }
    public class RolesNoIdDTO
    {
        public string rolename { get; set; }
        public List<string>? Users { get; set; }
    }
    public class AddRoleDTO
    {
        public string rolename { get; set; }
    }
}
