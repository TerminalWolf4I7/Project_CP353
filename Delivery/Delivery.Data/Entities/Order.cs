using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Delivery.Data.Entities
{
    /// <summary>
    /// Order Entity: แทนตาราง "orders"
    /// ทำหน้าที่เก็บข้อมูลหลักของคำสั่งซื้อ (Header) ที่เกิดขึ้นในระบบ รวมถึงสถานะการจัดส่งและยอดรวมราคาสุทธิ
    /// </summary>
    [Table("orders")]
    public class Order
    {
        /// <summary> รหัสออเดอร์ (Primary Key): รหัสอ้างอิงหลักสำหรับคำสั่งซื้อนี้ </summary>
        [Key]
        [Column("order_id")]
        public int OrderId { get; set; }

        /// <summary> รหัสลูกค้าที่สั่งซื้อ (Foreign Key): เชื่อมโยงไปยังตาราง Users </summary>
        [Column("user_id")]
        public int UserId { get; set; }

        /// <summary> รหัสร้านอาหารที่รับออเดอร์นี้ (Foreign Key): เชื่อมโยงไปยังตาราง Restaurants </summary>
        [Column("restaurant_id")]
        public int RestaurantId { get; set; }

        /// <summary> 
        /// รหัสไรเดอร์ที่รับหน้าที่จัดส่ง (Foreign Key)
        /// จะมีค่าเป็น Null ในตอนแรก จนกว่าจะมีไรเดอร์กดรับงานผ่านหน้าจอ Rider
        /// </summary>
        [Column("rider_id")]
        public int? RiderId { get; set; }

        /// <summary> ยอดราคารวมทั้งหมดของออเดอร์นี้ (ยอดเงินที่ลูกค้าต้องจ่ายจริง) </summary>
        [Column("total_price", TypeName = "decimal(10,2)")]
        public decimal TotalPrice { get; set; }

        /// <summary> 
        /// สถานะปัจจุบันของออเดอร์ เพื่อใช้ในการติดตามขั้นตอนการทำงาน
        /// ค่าที่แนะนำ: 'Pending' (รอร้านยืนยัน), 'Cooking' (ร้านเตรียมอาหาร), 
        /// 'Delivering' (ไรเดอร์กำลังจัดส่ง), 'Delivered' (ส่งอาหารสำเร็จ)
        /// </summary>
        [Required]
        [MaxLength(50)]
        [Column("status")]
        public string Status { get; set; } = string.Empty;

        /// <summary> วันที่และเวลาที่ออเดอร์ถูกสร้างขึ้น (เวลาที่ลูกค้ากดสั่งซื้อ) </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // --- Navigation Properties: ส่วนที่ใช้เข้าถึงข้อมูลที่เกี่ยวข้องผ่าน EF Core ---

        /// <summary> ข้อมูลรายละเอียดของผู้สั่งซื้อ (ลูกค้า) </summary>
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        /// <summary> ข้อมูลรายละเอียดของร้านอาหารที่ถูกสั่ง </summary>
        [ForeignKey(nameof(RestaurantId))]
        public Restaurant Restaurant { get; set; } = null!;

        /// <summary> ข้อมูลรายละเอียดของไรเดอร์ผู้ส่งงาน (จะเข้าถึงได้เมื่อมีไรเดอร์รับงานแล้ว) </summary>
        [ForeignKey(nameof(RiderId))]
        public Rider? Rider { get; set; }

        /// <summary> รายการอาหารย่อยทั้งหมดที่อยู่ในออเดอร์ใบนี้ (เชื่อมไปยังตาราง OrderItems) </summary>
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}


