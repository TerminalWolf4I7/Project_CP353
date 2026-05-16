using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Delivery.Data.Entities
{
    /// <summary>
    /// CartItem Entity: แทนตาราง "cart_items"
    /// เก็บรายการสินค้าแต่ละชนิดที่ลูกค้าเลือกใส่ไว้ในตะกร้า รวมถึงจำนวนที่ต้องการ
    /// </summary>
    [Table("cart_items")]
    public class CartItem
    {
        /// <summary> รหัสรายการย่อยในตะกร้า (Primary Key) </summary>
        [Key]
        [Column("cart_item_id")]
        public int CartItemId { get; set; }

        /// <summary> รหัสตะกร้าหลักที่รายการนี้สังกัดอยู่ (Foreign Key): เชื่อมไปยังตาราง Carts </summary>
        [Column("cart_id")]
        public int CartId { get; set; }

        /// <summary> รหัสเมนูอาหารที่ลูกค้าเลือก (Foreign Key): เชื่อมไปยังตาราง MenuItems </summary>
        [Column("item_id")]
        public int ItemId { get; set; }

        /// <summary> จำนวนสินค้าที่ต้องการสั่งซื้อ </summary>
        [Column("quantity")]
        public int Quantity { get; set; }

        // --- Navigation Properties ---

        /// <summary> ข้อมูลตะกร้าสินค้าหลัก (หัวตะกร้า) </summary>
        [ForeignKey(nameof(CartId))]
        public Cart Cart { get; set; } = null!;

        /// <summary> ข้อมูลรายละเอียดเมนูอาหารที่ถูกเลือกใส่ตะกร้า </summary>
        [ForeignKey(nameof(ItemId))]
        public MenuItem MenuItem { get; set; } = null!;
    }
}


