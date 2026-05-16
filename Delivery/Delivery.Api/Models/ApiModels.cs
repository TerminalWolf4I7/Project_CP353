namespace Delivery.Api.Models
{
    /// <summary>
    /// ApiModels: รวมรวบโครงสร้างข้อมูล (Data Transfer Objects - DTOs) 
    /// ที่ใช้สำหรับการสื่อสารและรับส่งข้อมูลระหว่าง API และ WinForms Client
    /// โดยใช้ 'record' เพื่อให้ข้อมูลมีความปลอดภัย (Immutable) และง่ายต่อการทำ JSON Serialization
    /// </summary>

    // --- ส่วนที่ 1: ระบบยืนยันตัวตน (Authentication Models) ---

    /// <summary> คำขอเข้าสู่ระบบ: ส่งเฉพาะรหัสผู้ใช้เพื่อตรวจสอบ </summary>
    public sealed record LoginRequest(int UserId);
    
    /// <summary> ข้อมูลตอบกลับหลังล็อกอิน: บอกรหัสผู้ใช้และบทบาท (customer, restaurant, rider) </summary>
    public sealed record LoginResponse(int UserId, string Role);


    // --- ส่วนที่ 2: ระบบร้านอาหารและเมนู (Restaurant & Menu Models) ---

    /// <summary> ข้อมูลสรุปของร้านอาหาร สำหรับแสดงผลในรายการร้านค้า </summary>
    public sealed record RestaurantDto(int RestaurantId, string Name, string? Address, string? Phone);

    /// <summary> รายละเอียดของเมนูอาหารแต่ละรายการ รวมถึงสถานะความพร้อมขาย </summary>
    public sealed record MenuItemDto(int ItemId, int RestaurantId, string Name, decimal Price, string? Description, bool IsAvailable);
    
    /// <summary> คำขออัปเดตข้อมูลร้านอาหาร (เช่น การเปลี่ยนชื่อร้าน) </summary>
    public sealed record RestaurantUpdateRequest(string Name);

    /// <summary> ข้อมูลสำหรับเพิ่มหรือแก้ไขเมนูอาหาร (ใช้ตอนบันทึกข้อมูลเมนู) </summary>
    public sealed record MenuItemUpsertRequest(string Name, decimal Price);


    // --- ส่วนที่ 3: ระบบตะกร้าสินค้า (Shopping Cart Models) ---

    /// <summary> คำขอเพิ่มสินค้าลงตะกร้า: ระบุเพียงรหัสอาหารและจำนวน </summary>
    public sealed record CartItemRequest(int ItemId, int Quantity);

    /// <summary> รายละเอียดสินค้าแต่ละชิ้นในตะกร้า: แสดงทั้งชื่อและราคาปัจจุบัน </summary>
    public sealed record CartItemDto(int ItemId, string Name, int Quantity, decimal Price);

    /// <summary> ข้อมูลสรุปของตะกร้าทั้งหมด: ประกอบด้วยรหัสเจ้าของ, ร้านที่สั่ง, รายการสินค้า และยอดรวม </summary>
    public sealed record CartSummaryDto(int CartId, int UserId, int RestaurantId, decimal Total, List<CartItemDto> Items);


    // --- ส่วนที่ 4: ระบบคำสั่งซื้อ (Order Models) ---

    /// <summary> คำขอยืนยันการสั่งซื้อ: ระบุตัวตนผู้สั่งและร้านที่สั่ง </summary>
    public sealed record CheckoutRequest(int UserId, int RestaurantId);

    /// <summary> ข้อมูลพื้นฐานของออเดอร์: ใช้ในหน้าประวัติหรือรายการออเดอร์รวม </summary>
    public sealed record OrderDto(int OrderId, int UserId, int RestaurantId, int? RiderId, decimal TotalPrice, string Status);

    /// <summary> รายการอาหารย่อยที่อยู่ในออเดอร์นั้นๆ (เก็บราคา ณ วันที่สั่งซื้อ) </summary>
    public sealed record OrderItemDto(int ItemId, string Name, int Quantity, decimal Price);

    /// <summary> รายละเอียดออเดอร์เชิงลึก: ใช้เมื่อต้องการดูข้อมูลลูกค้าร่วมกับข้อมูลออเดอร์ </summary>
    public sealed record OrderDetailDto(int OrderId, int UserId, string CustomerName, int RestaurantId, decimal TotalPrice, string Status);
    

    // --- ส่วนที่ 5: ระบบไรเดอร์ (Rider Models) ---

    /// <summary> ข้อมูลออเดอร์ปัจจุบันที่ไรเดอร์กำลังจัดส่ง (ใช้แสดงผลในหน้าจอไรเดอร์) </summary>
    public sealed record RiderCurrentOrderDto(int OrderId, int UserId, string CustomerName, int RestaurantId, string Status);


    // --- ส่วนที่ 6: การจัดการสถานะ (Status Management Models) ---

    /// <summary> คำขอเปลี่ยนสถานะออเดอร์ (เช่น ส่งข้อมูลว่า 'Cooking' หรือ 'Delivered') </summary>
    public sealed record UpdateStatusRequest(string Status);
}


