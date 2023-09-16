using System.ComponentModel.DataAnnotations;

namespace motel.Models.Domain
{
    public class Tiers
    {
        [Key]
        public int Id { get; set; }
        public string tiername { get; set; }
        public List<User> user { get; set; }
    }
}
