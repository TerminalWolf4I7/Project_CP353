using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Delivery.Data.Entities
{
    /// <summary>
    /// Cart Entity: แทนตาราง "carts"
    /// ทำหน้าที่เก็บข้อมูลส่วนหัว (Header) ของตะกร้าสินค้าสำหรับลูกค้าแต่ละคน
    /// </summary>
    [Table("carts")]
    public class Cart
    {
        /// <summary> รหัสตะกร้าสินค้า (Primary Key) </summary>
        [Key]
        [Column("cart_id")]
        public int CartId { get; set; }

        /// <summary> รหัสลูกค้าเจ้าของตะกร้า (Foreign Key): เชื่อมไปยังตาราง Users </summary>
        [Column("user_id")]
        public int UserId { get; set; }

        /// <summary> รหัสร้านอาหารที่ลูกค้ากำลังเลือกซื้อ (Foreign Key): ตะกร้า 1 ใบจะผูกกับ 1 ร้านอาหารเสมอ </summary>
        [Column("restaurant_id")]
        public int RestaurantId { get; set; }

        /// <summary> วันที่และเวลาที่ลูกค้าเริ่มสร้างตะกร้านี้ขึ้นมา </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // --- Navigation Properties ---

        /// <summary> ข้อมูลลูกค้าผู้เป็นเจ้าของตะกร้า </summary>
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        /// <summary> ข้อมูลร้านอาหารที่ถูกเลือกสินค้าใส่ตะกร้า </summary>
        [ForeignKey(nameof(RestaurantId))]
        public Restaurant Restaurant { get; set; } = null!;

        /// <summary> รายการสินค้าต่างๆ ที่ถูกบรรจุอยู่ในตะกร้าใบนี้ (เชื่อมไปยัง CartItems) </summary>
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}


