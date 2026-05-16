using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Delivery.Data.Entities
{
    /// <summary>
    /// User Entity: แทนตาราง "users" ในฐานข้อมูล
    /// เป็นตารางหลักที่เก็บข้อมูลผู้ใช้งานทุกคนในระบบ ไม่ว่าจะเป็น ลูกค้า, เจ้าของร้านอาหาร หรือไรเดอร์
    /// </summary>
    [Table("users")]
    public class User
    {
        /// <summary> รหัสผู้ใช้งาน (Primary Key): ใช้เป็นตัวระบุตัวตนหลักในระบบ </summary>
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }

        /// <summary> ชื่อ-นามสกุล หรือชื่อที่ใช้แสดงผลของผู้ใช้งาน </summary>
        [Required]
        [MaxLength(255)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary> 
        /// บทบาทของผู้ใช้ (Role): ใช้สำหรับแยกประเภทและกำหนดสิทธิ์การเข้าถึงหน้าจอต่างๆ
        /// ค่าที่เป็นไปได้: 'customer' (ลูกค้า), 'restaurant' (เจ้าของร้าน), 'rider' (ผู้ส่งอาหาร)
        /// </summary>
        [Required]
        [MaxLength(50)]
        [Column("role")]
        public string Role { get; set; } = string.Empty;

        // --- Navigation Properties: ส่วนที่เชื่อมโยงไปยังตารางอื่นๆ (Relationships) ---

        /// <summary> รายชื่อร้านอาหารที่ผู้ใช้คนนี้เป็นเจ้าของ (ใช้ในกรณี Role = 'restaurant') </summary>
        public ICollection<Restaurant> Restaurants { get; set; } = new List<Restaurant>();

        /// <summary> ข้อมูลรายละเอียดการเป็นไรเดอร์ (ใช้ในกรณี Role = 'rider') </summary>
        public ICollection<Rider> Riders { get; set; } = new List<Rider>();

        /// <summary> รายการตะกร้าสินค้าที่ผู้ใช้คนนี้กำลังเลือกซื้ออยู่ (ใช้ในกรณี Role = 'customer') </summary>
        public ICollection<Cart> Carts { get; set; } = new List<Cart>();

        /// <summary> ประวัติรายการสั่งซื้อทั้งหมดของผู้ใช้คนนี้ </summary>
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}


