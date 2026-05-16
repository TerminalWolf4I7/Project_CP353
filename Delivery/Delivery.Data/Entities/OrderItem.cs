using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Delivery.Data.Entities
{
    /// <summary>
    /// OrderItem Entity: แทนตาราง "order_items"
    /// เก็บรายละเอียดรายการสินค้าแต่ละรายการที่บรรจุอยู่ในออเดอร์นั้นๆ
    /// </summary>
    [Table("order_items")]
    public class OrderItem
    {
        /// <summary> รหัสรายการย่อยในออเดอร์ (Primary Key) </summary>
        [Key]
        [Column("order_item_id")]
        public int OrderItemId { get; set; }

        /// <summary> รหัสออเดอร์หลักที่รายการนี้สังกัดอยู่ (Foreign Key): เชื่อมไปยังตาราง Orders </summary>
        [Column("order_id")]
        public int OrderId { get; set; }

        /// <summary> รหัสเมนูอาหารที่ถูกสั่ง (Foreign Key): เชื่อมไปยังตาราง MenuItems </summary>
        [Column("item_id")]
        public int ItemId { get; set; }

        /// <summary> จำนวนที่สั่งซื้อในรายการนี้ </summary>
        [Column("quantity")]
        public int Quantity { get; set; }

        /// <summary> 
        /// ราคาต่อหน่วย ณ วันที่สั่งซื้อ (Price Snapshot)
        /// สำคัญมาก: ต้องเก็บแยกจากตาราง MenuItems เพื่อป้องกันยอดรวมผิดเพี้ยนหากร้านเปลี่ยนราคาภายหลัง
        /// </summary>
        [Column("price", TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        // --- Navigation Properties ---

        /// <summary> ข้อมูลออเดอร์หลักที่รายการนี้เป็นส่วนประกอบ </summary>
        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; } = null!;

        /// <summary> ข้อมูลรายละเอียดเมนูอาหารที่สั่ง </summary>
        [ForeignKey(nameof(ItemId))]
        public MenuItem MenuItem { get; set; } = null!;
    }
}


