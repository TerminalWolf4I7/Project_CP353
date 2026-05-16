using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Delivery.Data.Entities
{
    /// <summary>
    /// MenuItem Entity: แทนตาราง "menu_items"
    /// เก็บข้อมูลรายการอาหารแต่ละเมนูที่ร้านอาหารลงทะเบียนไว้ในระบบ
    /// </summary>
    [Table("menu_items")]
    public class MenuItem
    {
        /// <summary> รหัสเมนูอาหาร (Primary Key) </summary>
        [Key]
        [Column("item_id")]
        public int ItemId { get; set; }

        /// <summary> รหัสร้านอาหารที่เมนูนี้สังกัดอยู่ (Foreign Key) </summary>
        [Column("restaurant_id")]
        public int RestaurantId { get; set; }

        /// <summary> ชื่อเมนูอาหาร (เช่น 'ข้าวกะเพราไก่', 'ผัดไทย') </summary>
        [Required]
        [MaxLength(255)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary> ราคาอาหารต่อหน่วย (รองรับทศนิยม 2 ตำแหน่ง) </summary>
        [Column("price", TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        /// <summary> รายละเอียดเพิ่มเติมของเมนู (เช่น ข้อมูลโภชนาการ หรือส่วนผสม) </summary>
        [Column("description")]
        public string? Description { get; set; }

        /// <summary> สถานะความพร้อมของเมนู (true = พร้อมขาย, false = สินค้าหมด/งดขายชั่วคราว) </summary>
        [Column("is_available")]
        public bool IsAvailable { get; set; } = true;

        // --- Navigation Properties ---

        /// <summary> ข้อมูลร้านอาหารที่เป็นเจ้าของเมนูอาหารรายการนี้ </summary>
        [ForeignKey(nameof(RestaurantId))]
        public Restaurant Restaurant { get; set; } = null!;

        /// <summary> รายการอ้างอิงในตะกร้าสินค้า (เมนูนี้ถูกเลือกไปใส่ในตะกร้ากี่ครั้ง/ที่ไหนบ้าง) </summary>
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

        /// <summary> ประวัติการถูกสั่งซื้อในออเดอร์ต่างๆ (ใช้สำหรับดูยอดขายรายเมนู) </summary>
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}


