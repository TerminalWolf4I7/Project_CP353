namespace Delivery.Client.Models
{
    /// <summary>
    /// ApiModels (Client Side): โครงสร้างข้อมูลสำหรับรับ-ส่งข้อมูลกับ Backend API
    /// ทำหน้าที่เป็น 'Contract' หรือข้อตกลงในการสื่อสารข้อมูลผ่านรูปแบบ JSON
    /// ข้อมูลเหล่านี้จะถูกใช้โดย RestUtil ในการทำ Serialization/Deserialization
    /// </summary>

    // --- ส่วนที่ 1: ขั้นตอนการเข้าสู่ระบบ (Authentication Phase) ---

    /// <summary> ใช้ส่งรหัสผู้ใช้ที่ระบุในหน้า Login ไปยัง API </summary>
    public sealed record LoginRequest(int UserId);
    
    /// <summary> ข้อมูลที่ได้รับจาก API เพื่อบอกว่าผู้ใช้คนนี้เป็นใครและมีสิทธิ์เข้าถึงหน้าจอใด </summary>
    public sealed record LoginResponse(int UserId, string Role);


    // --- ส่วนที่ 2: ข้อมูลร้านค้าและเมนู (Restaurant & Menu Catalog) ---

    /// <summary> ข้อมูลร้านอาหารสำหรับแสดงผลในหน้าลิสต์รายการร้านค้าเพื่อให้ลูกค้าเลือกสั่ง </summary>
    public sealed record RestaurantDto(int RestaurantId, string Name, string? Address, string? Phone);

    /// <summary> ข้อมูลเมนูอาหารที่ดึงมาจาก API เพื่อนำไปแสดงในหน้าเลือกสั่งอาหาร (MenuForm) </summary>
    public sealed record MenuItemDto(int ItemId, int RestaurantId, string Name, decimal Price, string? Description, bool IsAvailable);
    
    /// <summary> ใช้เมื่อเจ้าของร้านต้องการส่งข้อมูลเพื่ออัปเดตชื่อร้านในหน้าจัดการร้านอาหาร </summary>
    public sealed record RestaurantUpdateRequest(string Name);

    /// <summary> ใช้เมื่อต้องการเพิ่มเมนูใหม่หรือแก้ไขราคาอาหารในหน้าจัดการเมนู </summary>
    public sealed record MenuItemUpsertRequest(string Name, decimal Price);


    // --- ส่วนที่ 3: ระบบตะกร้าสินค้า (Shopping Cart Logic) ---

    /// <summary> ใช้ส่งรหัสอาหารและจำนวนที่ลูกค้าต้องการเพิ่มลงตะกร้าไปยัง API </summary>
    public sealed record CartItemRequest(int ItemId, int Quantity);

    /// <summary> รายละเอียดสินค้าในตะกร้าที่ได้รับกลับมาจาก API เพื่อนำมาแสดงในตาราง (DataGridView) </summary>
    public sealed record CartItemDto(int ItemId, string Name, int Quantity, decimal Price);

    /// <summary> ข้อมูลสรุปยอดรวมของตะกร้าและรายการสินค้าทั้งหมด เพื่อแสดงผลในหน้าสรุปยอด </summary>
    public sealed record CartSummaryDto(int CartId, int UserId, int RestaurantId, decimal Total, List<CartItemDto> Items);


    // --- ส่วนที่ 4: การจัดการคำสั่งซื้อ (Order & Status Tracking) ---

    /// <summary> ใช้ส่งข้อมูลยืนยันการสั่งซื้อ (Checkout) เมื่อลูกค้ากดปุ่มยืนยันในตะกร้า </summary>
    public sealed record CheckoutRequest(int UserId, int RestaurantId);

    /// <summary> ข้อมูลสรุปออเดอร์ที่ใช้แสดงในหน้าประวัติการสั่งซื้อ หรือรายการออเดอร์ของร้าน </summary>
    public sealed record OrderDto(int OrderId, int UserId, int RestaurantId, int? RiderId, decimal TotalPrice, string Status);

    /// <summary> รายละเอียดรายการอาหารภายในออเดอร์ เพื่อแสดงให้เห็นว่าออเดอร์นั้นๆ มีเมนูอะไรบ้าง </summary>
    public sealed record OrderItemDto(int ItemId, string Name, int Quantity, decimal Price);

    /// <summary> ข้อมูลออเดอร์แบบละเอียดที่รวมชื่อลูกค้าไว้ด้วย เพื่อให้ร้านอาหารหรือไรเดอร์ทราบชื่อผู้รับ </summary>
    public sealed record OrderDetailDto(int OrderId, int UserId, string CustomerName, int RestaurantId, decimal TotalPrice, string Status);
    

    // --- ส่วนที่ 5: มุมมองสำหรับไรเดอร์ (Rider Specific View) ---

    /// <summary> ข้อมูลงานปัจจุบันของไรเดอร์ที่กำลังจัดส่งอยู่ เพื่อนำไปแสดงในหน้าจอติดตามงานของไรเดอร์ </summary>
    public sealed record RiderCurrentOrderDto(int OrderId, int UserId, string CustomerName, int RestaurantId, string Status);


    // --- ส่วนที่ 6: คำสั่งอัปเดตสถานะ (Status Update Actions) ---

    /// <summary> ใช้ส่งสถานะใหม่ของออเดอร์ไปยัง API เช่น 'Cooking', 'Delivering' หรือ 'Delivered' </summary>
    public sealed record UpdateStatusRequest(string Status);
}


