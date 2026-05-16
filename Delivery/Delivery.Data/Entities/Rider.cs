using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Delivery.Data.Entities
{
    [Table("riders")]
    public class Rider
    {
        [Key]
        [Column("rider_id")]
        public int RiderId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [MaxLength(100)]
        [Column("vehicle_type")]
        public string? VehicleType { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("status")]
        public string Status { get; set; } = string.Empty;

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
