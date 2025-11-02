using System.ComponentModel.DataAnnotations;

namespace motel.Models.Domain
{
    public class Tier_User
    {
        [Key]
        public int Id { get; set; }
        public int tierId { get; set; }
        public Tiers tiers { get; set; }
        public int userId { get; set; }
        public User user { get; set; }
        public int? credit { get; set; }
        [DataType(DataType.Date)]
        public DateTime? regDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? expireDate { get; set; }

    }
}
