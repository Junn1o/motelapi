using System.ComponentModel.DataAnnotations;

namespace motel.Models.Domain
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public string rolename { get; set; }
        public List<User> user { get; set; }
    }
}
