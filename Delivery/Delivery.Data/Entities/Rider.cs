using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Delivery.Data.Entities
{
    /// <summary>
    /// Rider Entity: แทนตาราง "riders"
    /// เก็บข้อมูลรายละเอียดการทำงานของไรเดอร์ (ผู้ส่งอาหาร)
    /// </summary>
    [Table("riders")]
    public class Rider
    {
        /// <summary> รหัสไรเดอร์ (Primary Key): รหัสเฉพาะสำหรับใช้ในระบบจัดส่ง </summary>
        [Key]
        [Column("rider_id")]
        public int RiderId { get; set; }

        /// <summary> รหัสผู้ใช้งาน (Foreign Key): เชื่อมไปยังตาราง Users เพื่อระบุตัวตนจริงของไรเดอร์ </summary>
        [Column("user_id")]
        public int UserId { get; set; }

        /// <summary> ประเภทของยานพาหนะที่ใช้ (เช่น 'Motorcycle', 'Bicycle', 'Car') </summary>
        [MaxLength(100)]
        [Column("vehicle_type")]
        public string? VehicleType { get; set; }

        /// <summary> 
        /// สถานะการทำงานปัจจุบันของไรเดอร์
        /// ค่าที่เป็นไปได้: 'available' (พร้อมรับงาน), 'delivering' (กำลังไปส่งอาหาร), 'offline' (ปิดรับงาน)
        /// </summary>
        [Required]
        [MaxLength(50)]
        [Column("status")]
        public string Status { get; set; } = string.Empty;

        // --- Navigation Properties ---

        /// <summary> ข้อมูลผู้ใช้งานที่ผูกกับไรเดอร์คนนี้ </summary>
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        /// <summary> ประวัติรายการออเดอร์ทั้งหมดที่ไรเดอร์คนนี้รับหน้าที่จัดส่ง </summary>
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}


