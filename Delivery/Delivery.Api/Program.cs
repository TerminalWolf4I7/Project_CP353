// จุดเริ่มต้นของโปรเจกต์ API (Backend): ทำหน้าที่ตั้งค่าการเชื่อมต่อฐานข้อมูล
// และเปิดบริการ REST API เพื่อให้ Client สามารถส่งข้อมูลมาประมวลผลได้
// โดยมีการลงทะเบียน DbContext สำหรับคุยกับ PostgreSQL และตั้งค่าพอร์ตการทำงาน 5000

using Delivery.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<DeliveryDbContext>(options =>
    // เชื่อมต่อกับ PostgreSQL บนเครื่องตัวเอง (localhost) พอร์ต 5432
    // ใช้ฐานข้อมูลชื่อ DeliveryDB ด้วย Username/Password ที่กำหนดไว้
    options.UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=password;Database=DeliveryDB"));

// กำหนดให้ API รันอยู่ที่พอร์ต 5000
// หมายเหตุ: ฝั่ง Client (RestUtil.cs) ต้องตั้งค่า URL ให้ตรงกันที่นี่ด้วย
builder.WebHost.UseUrls("http://localhost:5000");

var app = builder.Build();

app.MapControllers();
// health check แบบง่าย — ยืนยันว่า API รันอยู่
app.MapGet("/", () => "Delivery API running");

app.Run();
