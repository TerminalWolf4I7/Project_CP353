using System.Drawing;
using System.Windows.Forms;

namespace Delivery.Client
{
    // Partial class ของ CustomerForm ฝั่ง Designer
    // ไฟล์นี้ใช้สร้าง UI สำหรับหน้าลูกค้าเลือกร้านอาหาร
    // ส่วน logic เช่น โหลดร้าน, เปิดเมนู, logout จะอยู่ใน CustomerForm.cs
    partial class CustomerForm
    {
        // Container สำหรับเก็บ component/resource ของ WinForms
        // ใช้ cleanup ตอนปิดฟอร์ม
        private System.ComponentModel.IContainer components = null;

        // Cleanup resource ของฟอร์ม
        protected override void Dispose(bool disposing)
        {
            // ถ้ามี component ถูกสร้างไว้ ให้ dispose ทิ้งเพื่อลด memory leak
            if (disposing && (components != null))
                components.Dispose();

            // เรียก Dispose ของ base Form ต่อ
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        // Method หลักสำหรับสร้าง UI ของ Customer Dashboard
        //
        // Workflow ของหน้านี้:
        // 1. แสดงรายการร้านอาหารทั้งหมด
        // 2. ลูกค้าเลือกการ์ดร้าน
        // 3. เปิดหน้าเมนูของร้าน
        // 4. ลูกค้าตรวจสอบสถานะออเดอร์หรือ logout ได้
        private void InitializeComponent()
        {
            // =========================
            // TOP BAR
            // =========================

            // Header ด้านบนของหน้า
            panelTopBar = new Panel();

            // ชื่อแอป
            lblAppTitle = new Label();

            // Greeting หลักของหน้า
            lblGreeting = new Label();

            // ปุ่มดูสถานะออเดอร์
            btnOrderStatus = new Button();

            // ปุ่ม logout
            btnLogout = new Button();

            // =========================
            // SEARCH / SUBHEADER SECTION
            // =========================

            // ข้อความอธิบายรายการร้าน
            lblSubheader = new Label();

            // Panel แสดง section ร้านอาหาร
            panelSearch = new Panel();

            // Icon ร้านอาหาร
            lblSearchIcon = new Label();

            // =========================
            // RESTAURANT LIST
            // =========================

            // FlowLayoutPanel สำหรับแสดง restaurant cards แบบ dynamic
            flpRestaurants = new FlowLayoutPanel();

            // Suspend layout ชั่วคราวเพื่อลด redraw ระหว่างสร้าง UI
            panelTopBar.SuspendLayout();
            panelSearch.SuspendLayout();
            SuspendLayout();

            // =====================================================
            // TOP BAR PANEL
            // =====================================================

            // ใช้สีเขียวเป็น theme หลักของ customer flow
            panelTopBar.BackColor = Color.FromArgb(39, 174, 96);

            // เพิ่ม controls ลงใน header
            panelTopBar.Controls.Add(lblAppTitle);
            panelTopBar.Controls.Add(lblGreeting);
            panelTopBar.Controls.Add(btnOrderStatus);
            panelTopBar.Controls.Add(btnLogout);

            // Dock ด้านบนเต็มหน้าต่าง
            panelTopBar.Dock = DockStyle.Top;

            panelTopBar.Location = new Point(0, 0);
            panelTopBar.Margin = new Padding(2, 2, 2, 2);
            panelTopBar.Name = "panelTopBar";

            // ความสูงของ header
            panelTopBar.Size = new Size(896, 78);

            panelTopBar.TabIndex = 2;

            // =====================================================
            // APP TITLE LABEL
            // =====================================================

            // ชื่อแอป Delivery
            lblAppTitle.AutoSize = true;

            // ใช้ font bold เพื่อสร้าง visual hierarchy
            lblAppTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);

            // สีขาวเพื่อ contrast กับพื้นหลังสีเขียว
            lblAppTitle.ForeColor = Color.White;

            lblAppTitle.Location = new Point(18, 9);
            lblAppTitle.Margin = new Padding(2, 0, 2, 0);
            lblAppTitle.Name = "lblAppTitle";

            lblAppTitle.Size = new Size(156, 30);

            lblAppTitle.TabIndex = 0;

            // ใช้ emoji scooter เพื่อสร้าง brand identity
            lblAppTitle.Text = "\U0001f6f5  DELIVERY";

            // =====================================================
            // GREETING LABEL
            // =====================================================

            // Greeting หลักของหน้า customer
            lblGreeting.AutoSize = true;

            // ใช้ font ใหญ่เพื่อเป็น focus หลักของหน้า
            lblGreeting.Font = new Font("Segoe UI", 22F, FontStyle.Bold);

            lblGreeting.ForeColor = Color.White;

            lblGreeting.Location = new Point(18, 35);

            lblGreeting.Margin = new Padding(2, 0, 2, 0);

            lblGreeting.Name = "lblGreeting";

            lblGreeting.Size = new Size(309, 41);

            lblGreeting.TabIndex = 1;

            // ข้อความสร้าง engagement กับผู้ใช้
            lblGreeting.Text = "อยากกินอะไรวันนี้? 😋";

            // =====================================================
            // ORDER STATUS BUTTON
            // =====================================================

            // ปุ่มเปิดหน้าติดตามสถานะออเดอร์
            btnOrderStatus.BackColor = Color.White;

            // เปลี่ยน cursor เพื่อบอกว่า clickable
            btnOrderStatus.Cursor = Cursors.Hand;

            btnOrderStatus.FlatStyle = FlatStyle.Flat;

            btnOrderStatus.Font =
                new Font("Segoe UI", 10F, FontStyle.Bold);

            // ใช้สีเขียว theme เดียวกับระบบ
            btnOrderStatus.ForeColor =
                Color.FromArgb(39, 174, 96);

            btnOrderStatus.Location = new Point(641, 27);

            btnOrderStatus.Margin = new Padding(2, 2, 2, 2);

            btnOrderStatus.Name = "btnOrderStatus";

            btnOrderStatus.Size = new Size(115, 32);

            btnOrderStatus.TabIndex = 2;

            btnOrderStatus.Text = "สถานะออเดอร์";

            btnOrderStatus.UseVisualStyleBackColor = false;

            // Event สำหรับเปิดหน้า order status
            btnOrderStatus.Click += BtnOrderStatus_Click;

            // =====================================================
            // LOGOUT BUTTON
            // =====================================================

            // ปุ่มออกจากระบบ
            btnLogout.BackColor = Color.White;

            btnLogout.Cursor = Cursors.Hand;

            btnLogout.FlatStyle = FlatStyle.Flat;

            btnLogout.Font =
                new Font("Segoe UI", 10F, FontStyle.Bold);

            // ใช้สีแดงเพื่อสื่อถึง logout action
            btnLogout.ForeColor =
                Color.FromArgb(231, 76, 60);

            btnLogout.Location = new Point(760, 27);

            btnLogout.Margin = new Padding(2, 2, 2, 2);

            btnLogout.Name = "btnLogout";

            btnLogout.Size = new Size(108, 32);

            btnLogout.TabIndex = 3;

            btnLogout.Text = "ออกจากระบบ";

            btnLogout.UseVisualStyleBackColor = false;

            // Event สำหรับ logout
            btnLogout.Click += BtnLogout_Click;

            // =====================================================
            // SUBHEADER LABEL
            // =====================================================

            // ข้อความอธิบาย section ร้านอาหาร
            lblSubheader.AutoSize = true;

            lblSubheader.Font = new Font("Segoe UI", 10F);

            // ใช้สีเทาเพื่อลด visual priority
            lblSubheader.ForeColor =
                Color.FromArgb(100, 100, 100);

            lblSubheader.Location = new Point(60, 7);

            lblSubheader.Margin = new Padding(2, 0, 2, 0);

            lblSubheader.Name = "lblSubheader";

            lblSubheader.Size = new Size(229, 19);

            lblSubheader.TabIndex = 1;

            lblSubheader.Text =
                "ร้านอาหารทั้งหมด — คลิกที่การ์ดเพื่อดูเมนู";

            // =====================================================
            // SEARCH PANEL
            // =====================================================

            // Panel สำหรับ section รายการร้าน
            panelSearch.BackColor =
                Color.FromArgb(240, 248, 244);

            panelSearch.Controls.Add(lblSearchIcon);
            panelSearch.Controls.Add(lblSubheader);

            // Dock ใต้ top bar
            panelSearch.Dock = DockStyle.Top;

            panelSearch.Location = new Point(0, 78);

            panelSearch.Margin = new Padding(2, 2, 2, 2);

            panelSearch.Name = "panelSearch";

            panelSearch.Size = new Size(896, 33);

            panelSearch.TabIndex = 1;

            // =====================================================
            // SEARCH ICON
            // =====================================================

            // Icon ร้านอาหาร
            lblSearchIcon.AutoSize = true;

            // ใช้ Segoe UI Emoji เพื่อรองรับ unicode emoji
            lblSearchIcon.Font =
                new Font("Segoe UI Emoji", 14F);

            lblSearchIcon.Location = new Point(18, 2);

            lblSearchIcon.Margin = new Padding(2, 0, 2, 0);

            lblSearchIcon.Name = "lblSearchIcon";

            lblSearchIcon.Size = new Size(38, 26);

            lblSearchIcon.TabIndex = 0;

            lblSearchIcon.Text = "🏪";

            // =====================================================
            // FLOW LAYOUT PANEL
            // =====================================================

            // Container หลักสำหรับ restaurant cards
            //
            // Runtime:
            // CustomerForm.cs จะสร้าง restaurant cards แบบ dynamic
            // แล้วเพิ่มเข้า panel นี้
            flpRestaurants.AutoScroll = true;

            // สีพื้นหลังอ่อนเพื่อแยก content จาก top bar
            flpRestaurants.BackColor =
                Color.FromArgb(245, 248, 250);

            // Fill พื้นที่ที่เหลือทั้งหมด
            flpRestaurants.Dock = DockStyle.Fill;

            flpRestaurants.Location = new Point(0, 111);

            flpRestaurants.Margin = new Padding(2, 2, 2, 2);

            flpRestaurants.Name = "flpRestaurants";

            // Padding ด้านในเพื่อให้ card ไม่ชิดขอบ
            flpRestaurants.Padding =
                new Padding(18, 12, 18, 12);

            flpRestaurants.Size = new Size(896, 345);

            flpRestaurants.TabIndex = 0;

            // =====================================================
            // FORM CONFIGURATION
            // =====================================================

            AutoScaleDimensions = new SizeF(7F, 15F);

            // รองรับ DPI/font scaling
            AutoScaleMode = AutoScaleMode.Font;

            // สีพื้นหลังหลักของฟอร์ม
            BackColor = Color.FromArgb(245, 248, 250);

            // ขนาดหน้าต่างหลัก
            ClientSize = new Size(896, 456);

            // เพิ่ม controls หลักเข้า form
            Controls.Add(flpRestaurants);
            Controls.Add(panelSearch);
            Controls.Add(panelTopBar);

            Margin = new Padding(2, 2, 2, 2);

            Name = "CustomerForm";

            // เปิดฟอร์มตรงกลางหน้าจอ
            StartPosition = FormStartPosition.CenterScreen;

            // ชื่อบน title bar
            Text = "Delivery App — เลือกร้านอาหาร";

            // Resume layout หลังสร้าง UI เสร็จ
            panelTopBar.ResumeLayout(false);
            panelTopBar.PerformLayout();

            panelSearch.ResumeLayout(false);
            panelSearch.PerformLayout();

            ResumeLayout(false);
        }

        #endregion

        // =========================
        // CONTROL DECLARATIONS
        // Controls เหล่านี้จะถูกใช้งานใน CustomerForm.cs
        // =========================

        // Header ด้านบนของหน้า
        private Panel panelTopBar;

        // ชื่อแอป
        private Label lblAppTitle;

        // Greeting หลักของหน้า
        private Label lblGreeting;

        // Section ร้านอาหาร
        private Panel panelSearch;

        // Icon ร้านอาหาร
        private Label lblSearchIcon;

        // ข้อความอธิบายรายการร้าน
        private Label lblSubheader;

        // Container สำหรับ restaurant cards
        private FlowLayoutPanel flpRestaurants;

        // ปุ่ม logout
        private Button btnLogout;

        // ปุ่มดูสถานะออเดอร์
        private Button btnOrderStatus;
    }
}
