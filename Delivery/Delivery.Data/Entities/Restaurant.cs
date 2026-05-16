using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Delivery.Data.Entities
{
    /// <summary>
    /// Restaurant Entity: แทนตาราง "restaurants"
    /// เก็บข้อมูลรายละเอียดพื้นฐานของร้านอาหารแต่ละแห่งที่เข้าร่วมในระบบ
    /// </summary>
    [Table("restaurants")]
    public class Restaurant
    {
        /// <summary> รหัสร้านอาหาร (Primary Key) </summary>
        [Key]
        [Column("restaurant_id")]
        public int RestaurantId { get; set; }

        /// <summary> รหัสเจ้าของร้าน (Foreign Key): เชื่อมไปยังตาราง Users เพื่อระบุว่าใครเป็นเจ้าของ </summary>
        [Column("user_id")]
        public int UserId { get; set; }

        /// <summary> ชื่อร้านอาหารที่แสดงผลให้ลูกค้าเห็น </summary>
        [Required]
        [MaxLength(255)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary> ที่อยู่ตั้งของร้านอาหาร สำหรับการปักหมุดหรือจัดส่ง </summary>
        [Column("address")]
        public string? Address { get; set; }

        /// <summary> เบอร์โทรศัพท์ติดต่อของร้านอาหาร </summary>
        [MaxLength(50)]
        [Column("phone")]
        public string? Phone { get; set; }

        // --- Navigation Properties ---

        /// <summary> ข้อมูล User ที่เป็นเจ้าของร้านอาหารนี้ </summary>
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        /// <summary> รายการเมนูอาหารทั้งหมดที่สังกัดอยู่ในร้านนี้ </summary>
        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

        /// <summary> รายการตะกร้าสินค้าของลูกค้าที่กำลังเลือกซื้อจากร้านนี้ </summary>
        public ICollection<Cart> Carts { get; set; } = new List<Cart>();

        /// <summary> ประวัติรายการสั่งซื้อทั้งหมดที่ลูกค้าสั่งจากร้านอาหารนี้ </summary>
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}


