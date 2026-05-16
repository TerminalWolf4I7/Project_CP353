using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Delivery.Client.Models;

namespace Delivery.Client
{
    public partial class MenuForm : Form
    {
        // เก็บ id ของร้านที่ลูกค้าเลือกเข้ามาดูเมนู
        // ใช้กับ API endpoint หลายจุด เช่น โหลดเมนู, ดูตะกร้า, checkout
        private int restaurantId;

        // เก็บชื่อร้านไว้สำหรับแสดงบน UI และส่งต่อไปยังหน้าสถานะออเดอร์
        private string restaurantName;

        // เก็บ id ของลูกค้าที่ login อยู่
        // ใช้ผูกตะกร้าและออเดอร์ให้ตรงกับ user ปัจจุบัน
        private int userId;

        // Constructor ของหน้า MenuForm
        //
        // Workflow:
        // 1. รับข้อมูลร้านและ user จากหน้าก่อนหน้า
        // 2. สร้าง UI controls ผ่าน InitializeComponent()
        // 3. เก็บค่า restaurantId, restaurantName, userId ไว้ใช้ทั้ง form
        // 4. ผูก event หลักของหน้า เช่น Load, Checkout, Back
        //
        // Input:
        // - restaurantId: id ของร้านที่ต้องการโหลดเมนู
        // - restaurantName: ชื่อร้านสำหรับแสดงบน UI
        // - userId: id ของลูกค้าที่กำลังใช้งาน
        public MenuForm(int restaurantId, string restaurantName, int userId)
        {
            InitializeComponent();

            // เก็บ context ของร้านและ user ไว้ใช้ใน method อื่น ๆ ของ form
            this.restaurantId = restaurantId;
            this.restaurantName = restaurantName;
            this.userId = userId;

            // เมื่อ form โหลด ให้เริ่มโหลดเมนูและยอดรวมตะกร้า
            this.Load += MenuForm_Load;

            // ปุ่ม checkout ใช้สร้าง order จากตะกร้าปัจจุบัน
            btnCheckout.Click += BtnCheckout_Click;

            // ปุ่มย้อนกลับไปหน้าเลือกร้าน
            btnBack.Click += BtnBack_Click;
        }

        // Event handler เมื่อหน้า MenuForm โหลดขึ้นมาครั้งแรก
        //
        // Workflow:
        // 1. แสดงชื่อร้านบน label
        // 2. โหลดเมนูของร้านจาก API
        // 3. โหลดยอดรวมตะกร้าปัจจุบันของ user
        //
        // หมายเหตุ:
        // ใช้ fire-and-forget ด้วย "_ =" เพราะ event นี้ไม่ใช่ async
        // แต่ถ้าอยาก handle error แบบละเอียดกว่า อาจเปลี่ยนเป็น async void ได้
        private void MenuForm_Load(object sender, EventArgs e)
        {
            // แสดงชื่อร้านที่ถูกส่งมาจากหน้าก่อนหน้า
            lblRestaurantName.Text = restaurantName;

            // โหลดรายการเมนูแบบ async โดยไม่ block UI thread
            _ = LoadMenuAsync();

            // โหลดราคารวมของตะกร้าปัจจุบัน
            _ = UpdateTotalAsync();
        }

        // โหลดเมนูของร้านจาก API แล้วสร้าง menu card ขึ้นมาใน UI
        //
        // Workflow:
        // 1. ล้างรายการเมนูเดิมออกจาก FlowLayoutPanel
        // 2. เรียก API เพื่อดึงเมนูของ restaurantId ปัจจุบัน
        // 3. วนลูปสร้าง card สำหรับแต่ละเมนู
        // 4. ในแต่ละ card จะมีชื่อ, ราคา, คำอธิบาย, ปุ่มเพิ่ม และปุ่มลด
        // 5. เพิ่ม card ทั้งหมดเข้า flpMenu
        //
        // Side effects:
        // - ล้างและสร้าง control ใหม่ใน flpMenu
        // - ผูก event click ให้ปุ่มเพิ่ม/ลดของแต่ละเมนู
        // - แสดง MessageBox ถ้าโหลดข้อมูลล้มเหลว
        private async Task LoadMenuAsync()
        {
            try
            {
                // เคลียร์เมนูเก่าก่อนโหลดใหม่ เพื่อป้องกัน item ซ้ำบนหน้าจอ
                flpMenu.Controls.Clear();

                // ดึงรายการเมนูของร้านจาก API
                // endpoint นี้ขึ้นกับ restaurantId ที่ user เลือกเข้ามา
                var items = await RestUtil.GetAsync<List<MenuItemDto>>(
                    $"restaurants/{restaurantId}/menu");

                // ถ้า API คืน null ให้หยุดทำงานทันที
                // ป้องกัน NullReferenceException ตอน foreach
                if (items == null)
                {
                    return;
                }

                // สร้าง menu card หนึ่งใบต่อหนึ่งเมนู
                foreach (var item in items)
                {
                    // แยกค่าจาก DTO ออกมาเป็น local variable
                    // ทำให้โค้ดตอนสร้าง UI อ่านง่ายขึ้น
                    int id = item.ItemId;
                    string name = item.Name;
                    decimal price = item.Price;
                    string desc = item.Description ?? string.Empty;

                    // Panel นี้ทำหน้าที่เป็น card ของเมนูแต่ละรายการ
                    Panel card = new Panel();
                    card.Size = new Size(220, 280);
                    card.BackColor = Color.White;
                    card.Margin = new Padding(10);
                    card.BorderStyle = BorderStyle.FixedSingle;

                    // Label แสดงชื่อเมนู
                    // ใช้ font bold เพื่อให้เป็นหัวข้อหลักของ card
                    Label lblName = new Label
                    {
                        Text = name,
                        Font = new Font("Segoe UI", 12, FontStyle.Bold),
                        Location = new Point(10, 10),
                        AutoSize = true
                    };

                    // Label แสดงราคา
                    // ใช้สีเขียวเพื่อสื่อถึงราคาหรือ action เชิงบวกใน theme ของแอป
                    Label lblPrice = new Label
                    {
                        Text = price.ToString("N2") + " บาท",
                        ForeColor = Color.FromArgb(46, 204, 113),
                        Font = new Font("Segoe UI", 11, FontStyle.Bold),
                        Location = new Point(10, 40),
                        AutoSize = true
                    };

                    // Label แสดงคำอธิบายเมนู
                    // กำหนด Size fixed เพื่อให้ข้อความอยู่ในพื้นที่ของ card
                    Label lblDesc = new Label
                    {
                        Text = desc,
                        Font = new Font("Segoe UI", 9),
                        Location = new Point(10, 75),
                        Size = new Size(200, 120)
                    };

                    // ปุ่มเพิ่มเมนูลงตะกร้า
                    // ใช้ Tag เก็บ itemId เพื่อให้ event handler รู้ว่ากดเมนูไหน
                    Button btnAdd = new Button
                    {
                        Text = "เพิ่ม (+)",
                        BackColor = Color.FromArgb(46, 204, 113),
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Font = new Font("Segoe UI", 9, FontStyle.Bold),
                        Location = new Point(10, 220),
                        Size = new Size(95, 45),
                        Tag = id
                    };

                    // ผูกปุ่มเพิ่มกับ handler กลาง
                    // handler จะอ่าน itemId จาก btn.Tag
                    btnAdd.Click += BtnAddToCart_Click;

                    // ปุ่มลดหรือลบเมนูออกจากตะกร้า
                    // ใช้ Tag เก็บ itemId เหมือนปุ่มเพิ่ม เพื่อ reuse handler pattern เดียวกัน
                    Button btnRemove = new Button
                    {
                        Text = "ลด (-)",
                        BackColor = Color.FromArgb(231, 76, 60),
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Font = new Font("Segoe UI", 9, FontStyle.Bold),
                        Location = new Point(115, 220),
                        Size = new Size(95, 45),
                        Tag = id
                    };

                    // ผูกปุ่มลดกับ handler สำหรับลบ item ออกจากตะกร้า
                    btnRemove.Click += BtnRemoveFromCart_Click;

                    // ประกอบ control ทั้งหมดเข้า card
                    card.Controls.Add(lblName);
                    card.Controls.Add(lblPrice);
                    card.Controls.Add(lblDesc);
                    card.Controls.Add(btnAdd);
                    card.Controls.Add(btnRemove);

                    // เพิ่ม card เข้า FlowLayoutPanel เพื่อให้เรียงรายการเมนูอัตโนมัติ
                    flpMenu.Controls.Add(card);
                }
            }
            catch (Exception ex)
            {
                // แจ้ง error ตอนโหลดเมนู เช่น API ล่ม, network error หรือ deserialize failed
                MessageBox.Show("Error loading menu: " + ex.Message);
            }
        }

        // Event handler สำหรับปุ่มย้อนกลับ
        //
        // Workflow:
        // 1. สร้าง CustomerForm ใหม่โดยส่ง userId เดิมกลับไป
        // 2. แสดงหน้า CustomerForm
        // 3. ปิดหน้า MenuForm ปัจจุบัน
        //
        // Side effect:
        // - เปิด form ใหม่
        // - ปิด form นี้
        private void BtnBack_Click(object? sender, EventArgs e)
        {
            // กลับไปหน้าเลือกร้านของ user คนเดิม
            CustomerForm customerForm = new CustomerForm(userId);

            // แสดงหน้าลูกค้า
            customerForm.Show();

            // ปิดหน้าเมนูปัจจุบันเพื่อไม่ให้ form ค้าง
            Close();
        }

        // Event handler เมื่อกดปุ่มเพิ่มเมนู
        //
        // Workflow:
        // 1. ตรวจว่า sender เป็น Button จริงหรือไม่
        // 2. อ่าน itemId จาก Button.Tag
        // 3. เรียก API เพิ่ม item ลงตะกร้า
        // 4. อัปเดตราคารวมบน UI
        //
        // Side effects:
        // - เพิ่ม quantity ในตะกร้าฝั่ง server
        // - อัปเดต label ราคารวม
        private async void BtnAddToCart_Click(object? sender, EventArgs e)
        {
            // ใช้ pattern matching เพื่อให้แน่ใจว่า event มาจาก Button
            if (sender is Button btn)
            {
                // itemId ถูกเก็บไว้ใน Tag ตอนสร้างปุ่มของแต่ละเมนู
                int itemId = (int)btn.Tag;

                // เพิ่ม item ลงตะกร้าฝั่ง server
                await AddToCartAsync(itemId);

                // โหลดราคารวมใหม่หลังจากตะกร้าเปลี่ยน
                await UpdateTotalAsync();
            }
        }

        // Event handler เมื่อกดปุ่มลดเมนู
        //
        // Workflow:
        // 1. ตรวจว่า sender เป็น Button
        // 2. อ่าน itemId จาก Button.Tag
        // 3. เรียก API เพื่อลดหรือลบ item ออกจากตะกร้า
        // 4. อัปเดตราคารวมใหม่
        //
        // Side effects:
        // - เปลี่ยนข้อมูลตะกร้าฝั่ง server
        // - อัปเดต label ราคารวม
        private async void BtnRemoveFromCart_Click(object? sender, EventArgs e)
        {
            // ป้องกันการ cast ผิด type ถ้า event ถูกเรียกจาก control อื่น
            if (sender is Button btn)
            {
                // ดึง itemId จากปุ่มที่ถูกกด
                int itemId = (int)btn.Tag;

                // ลบหรือลด item ในตะกร้า
                await RemoveFromCartAsync(itemId);

                // โหลดราคารวมใหม่หลังจากตะกร้าถูกแก้ไข
                await UpdateTotalAsync();
            }
        }

        // เพิ่มเมนูหนึ่งชิ้นลงในตะกร้าของ user ปัจจุบัน
        //
        // Workflow:
        // 1. สร้าง payload ที่ระบุ itemId และ quantity = 1
        // 2. POST ไปยัง cart endpoint ของ user และ restaurant นี้
        // 3. ตรวจสอบว่า API ตอบกลับ success หรือไม่
        //
        // Input:
        // - itemId: id ของเมนูที่ต้องการเพิ่ม
        //
        // Side effects:
        // - อัปเดตตะกร้าฝั่ง server
        // - แสดง MessageBox ถ้า request ล้มเหลว
        private async Task AddToCartAsync(int itemId)
        {
            try
            {
                // ระบุว่าจะเพิ่ม item นี้จำนวน 1 ชิ้น
                var payload = new CartItemRequest(itemId, 1);

                // ส่ง request ไปเพิ่ม item ใน cart ของ userId + restaurantId ปัจจุบัน
                var response = await RestUtil.PostResponseAsync(
                    $"carts/{userId}/{restaurantId}/items",
                    payload);

                // ถ้า status code ไม่ใช่ success จะ throw exception
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                // แจ้ง error เช่น API error, network error หรือ server reject request
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // ลดหรือลบเมนูออกจากตะกร้าของ user ปัจจุบัน
        //
        // Workflow:
        // 1. เรียก DELETE endpoint ด้วย userId, restaurantId และ itemId
        // 2. ให้ server เป็นคนตัดสินใจว่าจะลด quantity หรือ remove item
        // 3. ตรวจสอบ response ว่าสำเร็จหรือไม่
        //
        // Input:
        // - itemId: id ของเมนูที่ต้องการลบหรือลดจำนวน
        //
        // Side effects:
        // - อัปเดตตะกร้าฝั่ง server
        // - แสดง MessageBox ถ้า request ล้มเหลว
        private async Task RemoveFromCartAsync(int itemId)
        {
            try
            {
                // ลบหรือลด item จาก cart ตาม logic ฝั่ง API
                var response = await RestUtil.DeleteAsync(
                    $"carts/{userId}/{restaurantId}/items/{itemId}");

                // ถ้า API ตอบกลับ error จะ throw exception
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                // แจ้ง error ตอนลบ item เช่น item ไม่มีอยู่ใน cart หรือ API ล่ม
                MessageBox.Show("Error removing item: " + ex.Message);
            }
        }

        // โหลดตะกร้าปัจจุบันจาก API แล้วอัปเดตราคารวมบน UI
        //
        // Workflow:
        // 1. GET cart summary ของ user และ restaurant นี้
        // 2. อ่านค่า Total จาก response
        // 3. ถ้า cart เป็น null ให้ใช้ยอดรวมเป็น 0
        // 4. แสดงยอดรวมบน lblTotal
        //
        // Side effect:
        // - เปลี่ยนข้อความบน lblTotal
        //
        // หมายเหตุ:
        // catch ว่างไว้เพื่อไม่รบกวน user ถ้าโหลด total ไม่สำเร็จ
        // แต่ใน production จริงควร log error ไว้ debug
        private async Task UpdateTotalAsync()
        {
            try
            {
                // โหลดข้อมูลสรุปตะกร้าของ user กับร้านนี้
                var cart = await RestUtil.GetAsync<CartSummaryDto>(
                    $"carts/{userId}/{restaurantId}");

                // ถ้า cart เป็น null ให้ถือว่ายอดรวมเป็น 0
                decimal total = cart?.Total ?? 0;

                // แสดงราคารวมแบบมี comma/ทศนิยม 2 ตำแหน่ง
                lblTotal.Text = $"ราคารวม: {total.ToString("N2")} บาท";
            }
            catch
            {
                // ตั้งใจไม่แสดง error เพื่อไม่ให้ UI เด้งเตือนถี่เกินไป
                // โดยเฉพาะกรณี update total หลังเพิ่ม/ลบ item
            }
        }

        // Event handler สำหรับปุ่ม Checkout
        //
        // Workflow:
        // 1. โหลด cart ปัจจุบันจาก API
        // 2. ตรวจว่าตะกร้าว่างหรือไม่
        // 3. ถ้ามีสินค้า ให้ส่ง CheckoutRequest ไปสร้าง order
        // 4. อ่าน orderId ที่ API ส่งกลับมา
        // 5. แจ้งผู้ใช้ว่าสั่งซื้อสำเร็จ
        // 6. เปิดหน้า OrderStatusForm เพื่อติดตามสถานะออเดอร์
        // 7. ซ่อนหน้า MenuForm ปัจจุบัน
        //
        // Output:
        // - ได้ orderId จาก API หลัง checkout สำเร็จ
        //
        // Side effects:
        // - สร้าง order ใหม่ฝั่ง server
        // - เปิด OrderStatusForm
        // - ซ่อน MenuForm นี้
        private async void BtnCheckout_Click(object? sender, EventArgs e)
        {
            try
            {
                // โหลดข้อมูลตะกร้าล่าสุดก่อน checkout
                // เพื่อป้องกันการ checkout ตอนตะกร้าว่างหรือข้อมูลไม่ sync
                var cart = await RestUtil.GetAsync<CartSummaryDto>(
                    $"carts/{userId}/{restaurantId}");

                // ถ้าไม่มี cart หรือไม่มี item ให้หยุด flow
                // ป้องกันการสร้าง order ว่าง
                if (cart == null || cart.Items.Count == 0)
                {
                    MessageBox.Show("ตะกร้าว่างเปล่า กรุณาเลือกอาหารก่อนครับ");
                    return;
                }

                // เตรียมข้อมูลสำหรับสร้าง order
                // ฝั่ง server จะใช้ userId + restaurantId ไปดึง cart แล้วสร้าง order
                var payload = new CheckoutRequest(userId, restaurantId);

                // ส่ง checkout request ไปยัง API
                var response = await RestUtil.PostResponseAsync("orders/checkout", payload);

                // ถ้า checkout ไม่สำเร็จ ให้ throw exception เพื่อไปเข้า catch
                response.EnsureSuccessStatusCode();

                // อ่าน orderId ที่ server สร้างกลับมา
                int orderId = await RestUtil.ReadAsAsync<int>(response);

                // แจ้งเลขออเดอร์ให้ user รู้หลังสั่งซื้อสำเร็จ
                MessageBox.Show($"สั่งซื้อสำเร็จ! เลขที่ใบสั่งซื้อของคุณคือ: {orderId}");

                // เปิดหน้าติดตามสถานะออเดอร์
                // ส่งชื่อร้าน, userId และ orderId ไปให้หน้าถัดไปใช้โหลดสถานะ
                OrderStatusForm statusForm = new OrderStatusForm(this.restaurantName, this.userId, orderId);

                // แสดงหน้าสถานะออเดอร์
                statusForm.Show();

                // ซ่อนหน้าเมนูไว้แทนการ Close()
                // เพื่อไม่ให้ flow การใช้งานถูกตัดทันทีถ้าหน้านี้ยังต้องค้างใน lifecycle
                this.Hide();
            }
            catch (Exception ex)
            {
                // แจ้ง error ที่เกิดระหว่าง checkout เช่น API ล่ม, cart invalid หรือ parse orderId ไม่ได้
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
