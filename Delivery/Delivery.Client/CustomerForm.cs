using System;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows.Forms;
using Delivery.Client.Models;

namespace Delivery.Client
{
    /// <summary>
    /// CustomerForm เป็นหน้าหลักสำหรับลูกค้า (Customer) 
    /// ทำหน้าที่แสดงรายการร้านอาหารและจัดการการนำทางไปยังส่วนต่างๆ ของลูกค้า
    /// </summary>
    public partial class CustomerForm : Form
    {
        private int userId; // รหัสลูกค้าที่ล็อกอินเข้ามา

        // ชุดสีสำหรับตกแต่งการ์ดร้านอาหารให้ดูสวยงามและหลากหลาย
        private Color[] accentColors = new Color[]
        {
            Color.FromArgb(39, 174, 96),  // Green
            Color.FromArgb(52, 152, 219), // Blue
            Color.FromArgb(231, 76, 60),  // Red
            Color.FromArgb(155, 89, 182), // Purple
            Color.FromArgb(230, 126, 34), // Orange
            Color.FromArgb(26, 188, 156), // Teal
        };

        // Emoji แทนประเภทหรือสัญลักษณ์ของร้านอาหาร
        private string[] categoryEmojis = new string[]
        {
            "🍛", "🍜", "🍣", "☕", "🍕", "🥩", "🥗", "🍱"
        };

        /// <summary>
        /// Constructor รับ userId เพื่อใช้ในการดึงข้อมูลส่วนตัวหรือออเดอร์ของลูกค้ารายนั้น
        /// </summary>
        public CustomerForm(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            this.Load += CustomerForm_Load;
        }

        /// <summary>
        /// เมื่อฟอร์มถูกโหลด ให้เริ่มดึงข้อมูลร้านอาหารจาก API ทันที
        /// </summary>
        private async void CustomerForm_Load(object sender, EventArgs e)
        {
            await LoadRestaurantsAsync();
        }

        /// <summary>
        /// ฟังก์ชันสำหรับดึงรายการร้านอาหารทั้งหมดจาก API และนำมาสร้างเป็น UI Card แสดงผล
        /// </summary>
        private async Task LoadRestaurantsAsync()
        {
            try
            {
                // ล้างข้อมูลเก่าใน FlowLayoutPanel ก่อนโหลดใหม่
                flpRestaurants.Controls.Clear();
                
                // เรียก API ดึงรายชื่อร้านอาหาร
                var restaurants = await RestUtil.GetAsync<List<RestaurantDto>>("restaurants");

                if (restaurants == null)
                {
                    return;
                }

                int index = 0;
                foreach (var restaurant in restaurants)
                {
                    // เลือกสีและ emoji แบบวนลูปตามลำดับ
                    Color accent = accentColors[index % accentColors.Length];
                    string emoji = categoryEmojis[index % categoryEmojis.Length];

                    // สร้าง UI Card สำหรับร้านอาหารแต่ละร้าน
                    Panel card = CreateRestaurantCard(
                        restaurant.RestaurantId,
                        restaurant.Name,
                        restaurant.Address ?? string.Empty,
                        restaurant.Phone ?? string.Empty,
                        accent,
                        emoji);

                    flpRestaurants.Controls.Add(card);
                    index++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ไม่สามารถโหลดรายการร้านอาหารได้: " + ex.Message);
            }
        }

        /// <summary>
        /// ฟังก์ชันสร้าง Panel (Card) ของร้านอาหารแบบ Programmatic 
        /// เพื่อให้ง่ายต่อการจัดการดีไซน์และการแสดงผลแบบ Dynamic
        /// </summary>
        private Panel CreateRestaurantCard(int id, string name, string address, string phone, Color accent, string emoji)
        {
            Panel card = new Panel();
            card.Size = new Size(370, 210);
            card.BackColor = Color.White;
            card.Margin = new Padding(12);
            card.Cursor = Cursors.Hand;
            card.Tag = id; // เก็บรหัสร้านไว้ใน Tag เพื่อใช้ตอนคลิก

            // แถบสีด้านซ้ายของการ์ด
            Panel accentBar = new Panel();
            accentBar.Size = new Size(8, 210);
            accentBar.Location = new Point(0, 0);
            accentBar.BackColor = accent;
            card.Controls.Add(accentBar);

            // วงกลมพื้นหลังของ Emoji
            Panel emojiCircle = new Panel();
            emojiCircle.Size = new Size(64, 64);
            emojiCircle.Location = new Point(25, 25);
            emojiCircle.BackColor = Color.FromArgb(
                Math.Min(accent.R + 180, 255),
                Math.Min(accent.G + 180, 255),
                Math.Min(accent.B + 180, 255));
            card.Controls.Add(emojiCircle);

            // ตัว Emoji ของร้าน
            Label lblEmoji = new Label();
            lblEmoji.Text = emoji;
            lblEmoji.Font = new Font("Segoe UI Emoji", 22F);
            lblEmoji.Size = new Size(64, 64);
            lblEmoji.Location = new Point(0, 0);
            lblEmoji.TextAlign = ContentAlignment.MiddleCenter;
            emojiCircle.Controls.Add(lblEmoji);

            // ชื่อร้านอาหาร
            Label lblName = new Label();
            lblName.Text = name;
            lblName.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblName.ForeColor = Color.FromArgb(25, 25, 25);
            lblName.Location = new Point(105, 22);
            lblName.Size = new Size(250, 35);
            card.Controls.Add(lblName);

            // ที่อยู่ร้าน
            Label lblAddr = new Label();
            lblAddr.Text = "📍  " + address;
            lblAddr.Font = new Font("Segoe UI", 9F);
            lblAddr.ForeColor = Color.FromArgb(120, 120, 120);
            lblAddr.Location = new Point(105, 62);
            lblAddr.Size = new Size(250, 40);
            card.Controls.Add(lblAddr);

            // เบอร์โทรศัพท์
            Label lblPhone = new Label();
            lblPhone.Text = "📞  " + phone;
            lblPhone.Font = new Font("Segoe UI", 9F);
            lblPhone.ForeColor = Color.FromArgb(120, 120, 120);
            lblPhone.Location = new Point(105, 100);
            lblPhone.Size = new Size(250, 25);
            card.Controls.Add(lblPhone);

            // เส้นคั่นระหว่างข้อมูลร้านกับปุ่มเมนู
            Panel divider = new Panel();
            divider.Size = new Size(340, 1);
            divider.Location = new Point(20, 148);
            divider.BackColor = Color.FromArgb(235, 235, 235);
            card.Controls.Add(divider);

            // ข้อความแนะนำการคลิก
            Label lblSeeMenu = new Label();
            lblSeeMenu.Text = "ดูเมนูทั้งหมด  →";
            lblSeeMenu.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblSeeMenu.ForeColor = accent;
            lblSeeMenu.Location = new Point(22, 160);
            lblSeeMenu.AutoSize = true;
            card.Controls.Add(lblSeeMenu);

            // ผูก Event การคลิกให้กับทุกส่วนประกอบในการ์ด
            card.Click += RestaurantCard_Click;
            foreach (Control ctrl in card.Controls)
            {
                ctrl.Click += (s, e) => RestaurantCard_Click(card, e);
                ctrl.Cursor = Cursors.Hand;
                foreach (Control child in ctrl.Controls)
                {
                    child.Click += (s, e) => RestaurantCard_Click(card, e);
                    child.Cursor = Cursors.Hand;
                }
            }

            return card;
        }

        /// <summary>
        /// เมื่อคลิกที่การ์ดร้านอาหาร จะทำการเปิดหน้าเมนูอาหารของร้านนั้นๆ
        /// </summary>
        private void RestaurantCard_Click(object? sender, EventArgs e)
        {
            Panel card = sender as Panel;
            if (card == null) return;

            // ตรวจสอบว่า Control ที่คลิกมี Tag (รหัสร้าน) หรือไม่ ถ้าไม่มีให้ไล่หาจาก Parent
            while (card.Tag == null || !(card.Tag is int))
            {
                if (card.Parent is Panel p) card = p;
                else return;
            }

            int restaurantId = (int)card.Tag;
            string restaurantName = "";

            // ดึงชื่อร้านจากการ์ดเพื่อไปแสดงผลในหน้าถัดไป
            foreach (Control ctrl in card.Controls)
            {
                if (ctrl is Label lbl && lbl.Font.Size >= 14F)
                {
                    restaurantName = lbl.Text;
                    break;
                }
            }

            // เปิด MenuForm และส่งข้อมูลร้านและผู้ใช้ไป
            MenuForm menuForm = new MenuForm(restaurantId, restaurantName, this.userId);
            menuForm.Show();
            this.Hide(); // ซ่อนหน้าหลักลูกค้า
        }

        /// <summary>
        /// จัดการการออกจากระบบ
        /// </summary>
        private void BtnLogout_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show("คุณต้องการออกจากระบบใช่หรือไม่?", "ยืนยันการออกจากระบบ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Login loginForm = new Login();
                loginForm.Show();
                this.Close(); // ปิดหน้าฟอร์มลูกค้า
            }
        }

        /// <summary>
        /// เปิดหน้าสำหรับติดตามสถานะออเดอร์ล่าสุดของลูกค้า
        /// </summary>
        private async void BtnOrderStatus_Click(object? sender, EventArgs e)
        {
            try
            {
                // ดึงรายการออเดอร์ทั้งหมดของลูกค้าคนนี้
                var orders = await RestUtil.GetAsync<List<OrderDto>>($"orders?userId={userId}");

                if (orders == null || orders.Count == 0)
                {
                    MessageBox.Show("คุณยังไม่มีออเดอร์ กรุณาสั่งอาหารก่อนครับ");
                    return;
                }

                // ค้นหาออเดอร์ที่สถานะยังไม่เสร็จสิ้น หรือใช้ออเดอร์ล่าสุดถ้าเสร็จหมดแล้ว
                var activeOrder = orders
                    .Where(o => o.Status != "Success" && o.Status != "Completed")
                    .OrderByDescending(o => o.OrderId)
                    .FirstOrDefault();

                var targetOrder = activeOrder ?? orders.OrderByDescending(o => o.OrderId).First();

                // ค้นหาชื่อร้านเพื่อส่งไปแสดงผลในหน้าสถานะ
                string restaurantName = "ร้านอาหาร";
                try
                {
                    var restaurants = await RestUtil.GetAsync<List<RestaurantDto>>("restaurants");
                    var restaurant = restaurants?.FirstOrDefault(r => r.RestaurantId == targetOrder.RestaurantId);
                    if (restaurant != null)
                    {
                        restaurantName = restaurant.Name;
                    }
                }
                catch { }

                // เปิดหน้า OrderStatusForm
                OrderStatusForm statusForm = new OrderStatusForm(restaurantName, this.userId, targetOrder.OrderId);
                statusForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message);
            }
        }
    }
}

