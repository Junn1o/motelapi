using System.ComponentModel.DataAnnotations;

namespace motel.Models.Domain
{
    public class Tiers
    {
        [Key]
        public int Id { get; set; }
        public string tiername { get; set; }
        public decimal price { get; set; }
        public List<Tier_User> tier_user { get; set; }
    }
}
