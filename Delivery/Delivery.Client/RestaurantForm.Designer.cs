using System.Drawing;
using System.Windows.Forms;

namespace Delivery.Client
{
    // Partial class ของ RestaurantForm ฝั่ง Designer
    // ไฟล์นี้ใช้สร้าง UI layout ของหน้า Dashboard ร้านอาหาร
    // ส่วน logic เช่น โหลดข้อมูลร้าน เปิดฟอร์มออเดอร์/แก้เมนู จะอยู่ใน RestaurantForm.cs
    partial class RestaurantForm
    {
        /// <summary>
        /// Container สำหรับเก็บ component/resource ของ WinForms
        /// ใช้ร่วมกับ Dispose() เพื่อ cleanup memory/resource ตอนปิดฟอร์ม
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Cleanup resource ที่ form นี้ใช้งานอยู่
        /// </summary>
        /// <param name="disposing">
        /// true = dispose managed resources ได้
        /// false = ถูกเรียกจาก finalizer path
        /// </param>
        protected override void Dispose(bool disposing)
        {
            // ถ้ามี component ถูกสร้างไว้ ให้ dispose ทิ้งเพื่อลด memory leak
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            // เรียก Dispose ของ base Form ต่อ
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Method หลักสำหรับสร้าง UI ของ Restaurant Dashboard
        ///
        /// Workflow ของหน้านี้:
        /// 1. แสดงชื่อร้านที่ login อยู่
        /// 2. ให้ร้านเข้าหน้าจัดการออเดอร์
        /// 3. ให้ร้านเข้าหน้าแก้ไขเมนู
        /// 4. รองรับ logout กลับหน้า Login
        /// </summary>
        private void InitializeComponent()
        {
            // =========================
            // TOP BAR
            // =========================

            // Header ด้านบนของ dashboard
            panelTopBar = new Panel();

            // ชื่อ dashboard/app
            lblAppTitle = new Label();

            // Label สำหรับแสดงชื่อร้าน
            label1 = new Label();

            // ปุ่ม logout
            buttonLogout = new Button();

            // =========================
            // CONTENT SECTION
            // =========================

            // พื้นที่หลักของ dashboard
            panelContent = new Panel();

            // ข้อความอธิบายส่วนจัดการร้าน
            lblSubheader = new Label();

            // ปุ่มจัดการออเดอร์
            button1 = new Button();

            // ปุ่มแก้ไขเมนู
            button2 = new Button();

            // Suspend layout ชั่วคราวเพื่อลด redraw ระหว่างสร้าง UI
            panelTopBar.SuspendLayout();
            panelContent.SuspendLayout();
            SuspendLayout();

            // =====================================================
            // TOP BAR PANEL
            // =====================================================

            // ใช้สีส้มเพื่อสื่อถึงร้านอาหาร/food theme
            panelTopBar.BackColor = Color.FromArgb(230, 126, 34);

            // เพิ่ม controls ลงใน header
            panelTopBar.Controls.Add(lblAppTitle);
            panelTopBar.Controls.Add(label1);
            panelTopBar.Controls.Add(buttonLogout);

            // Dock ด้านบนเต็มหน้าต่าง
            panelTopBar.Dock = DockStyle.Top;

            panelTopBar.Location = new Point(0, 0);
            panelTopBar.Name = "panelTopBar";

            // ความสูงพอสำหรับ title + ชื่อร้าน
            panelTopBar.Size = new Size(900, 80);

            panelTopBar.TabIndex = 0;

            // =====================================================
            // APP TITLE LABEL
            // =====================================================

            // หัวข้อ dashboard หลัก
            lblAppTitle.AutoSize = true;

            lblAppTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);

            // ใช้สีครีมอ่อนเพื่อลด contrast จากสีขาวล้วน
            lblAppTitle.ForeColor = Color.FromArgb(255, 235, 205);

            lblAppTitle.Location = new Point(20, 8);
            lblAppTitle.Name = "lblAppTitle";

            lblAppTitle.Size = new Size(200, 20);

            lblAppTitle.TabIndex = 0;

            // ใช้ emoji เพื่อเพิ่ม visual identity
            lblAppTitle.Text = "🍳  RESTAURANT DASHBOARD";

            // =====================================================
            // RESTAURANT NAME LABEL
            // =====================================================

            // Label แสดงชื่อร้านปัจจุบัน
            // Runtime จะถูกอัปเดตจาก API ใน RestaurantForm_Load()
            label1.AutoSize = true;

            // ใช้ font ใหญ่เพื่อให้เป็นข้อมูลหลักของหน้า
            label1.Font = new Font("Segoe UI", 20F, FontStyle.Bold);

            label1.ForeColor = Color.White;

            label1.Location = new Point(20, 35);
            label1.Name = "label1";

            label1.Size = new Size(300, 37);

            label1.TabIndex = 1;

            // placeholder ตอนออกแบบ UI
            label1.Text = "ชื่อร้าน...";

            // =====================================================
            // LOGOUT BUTTON
            // =====================================================

            // ปุ่มออกจากระบบ
            buttonLogout.BackColor = Color.White;

            // เปลี่ยน cursor เพื่อบอกว่า clickable
            buttonLogout.Cursor = Cursors.Hand;

            buttonLogout.FlatStyle = FlatStyle.Flat;

            buttonLogout.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            // ใช้สีแดงเพื่อสื่อถึง action ออกจากระบบ
            buttonLogout.ForeColor = Color.FromArgb(231, 76, 60);

            // วางไว้ขวาสุดของ header
            buttonLogout.Location = new Point(760, 25);

            buttonLogout.Name = "buttonLogout";

            buttonLogout.Size = new Size(115, 35);

            buttonLogout.TabIndex = 2;

            buttonLogout.Text = "ออกจากระบบ";

            buttonLogout.UseVisualStyleBackColor = false;

            // =====================================================
            // CONTENT PANEL
            // =====================================================

            // พื้นที่หลักของ dashboard
            panelContent.BackColor = Color.FromArgb(245, 248, 250);

            // เพิ่ม controls หลักของ dashboard
            panelContent.Controls.Add(lblSubheader);
            panelContent.Controls.Add(button1);
            panelContent.Controls.Add(button2);

            // Fill พื้นที่ที่เหลือจาก top bar
            panelContent.Dock = DockStyle.Fill;

            panelContent.Location = new Point(0, 80);
            panelContent.Name = "panelContent";

            panelContent.Size = new Size(900, 420);

            panelContent.TabIndex = 1;

            // =====================================================
            // SUBHEADER LABEL
            // =====================================================

            // ข้อความอธิบายการใช้งาน dashboard
            lblSubheader.AutoSize = true;

            lblSubheader.Font = new Font("Segoe UI", 10F);

            // ใช้สีเทาเพื่อลด visual priority
            lblSubheader.ForeColor = Color.FromArgb(100, 100, 100);

            lblSubheader.Location = new Point(40, 20);

            lblSubheader.Name = "lblSubheader";

            lblSubheader.Size = new Size(300, 19);

            lblSubheader.TabIndex = 0;

            lblSubheader.Text = "📋  เลือกจัดการร้านอาหารของคุณ";

            // =====================================================
            // ORDERS BUTTON
            // =====================================================

            // ปุ่มเข้าสู่หน้าจัดการออเดอร์
            //
            // Logic จริงจะถูก bind ใน RestaurantForm.cs:
            // button1.Click += ButtonOrders_Click;
            button1.BackColor = Color.FromArgb(230, 126, 34);

            button1.Cursor = Cursors.Hand;

            // ลบ border default เพื่อให้ UI modern ขึ้น
            button1.FlatAppearance.BorderSize = 0;

            button1.FlatStyle = FlatStyle.Flat;

            // ใช้ font ใหญ่เพราะเป็น primary action ของร้าน
            button1.Font = new Font("Segoe UI", 16F, FontStyle.Bold);

            button1.ForeColor = Color.White;

            // ขนาดใหญ่เพื่อให้คลิกง่ายและเห็นชัด
            button1.Location = new Point(40, 60);

            button1.Name = "button1";

            button1.Size = new Size(820, 140);

            button1.TabIndex = 1;

            button1.Text = "📦  รับออเดอร์ / ดูรายการสั่งซื้อ";

            button1.UseVisualStyleBackColor = false;

            // =====================================================
            // EDIT MENU BUTTON
            // =====================================================

            // ปุ่มเข้าสู่หน้าแก้ไขเมนูอาหาร
            //
            // Logic จริงจะถูก bind ใน RestaurantForm.cs:
            // button2.Click += ButtonEdit_Click;
            button2.BackColor = Color.FromArgb(52, 152, 219);

            button2.Cursor = Cursors.Hand;

            button2.FlatAppearance.BorderSize = 0;

            button2.FlatStyle = FlatStyle.Flat;

            button2.Font = new Font("Segoe UI", 16F, FontStyle.Bold);

            button2.ForeColor = Color.White;

            // วางไว้ใต้ปุ่มออเดอร์
            button2.Location = new Point(40, 220);

            button2.Name = "button2";

            button2.Size = new Size(820, 140);

            button2.TabIndex = 2;

            button2.Text = "✏️  แก้ไขเมนูอาหาร";

            button2.UseVisualStyleBackColor = false;

            // =====================================================
            // FORM CONFIGURATION
            // =====================================================

            AutoScaleDimensions = new SizeF(7F, 15F);

            // รองรับ DPI/font scaling ของระบบ
            AutoScaleMode = AutoScaleMode.Font;

            // สีพื้นหลังหลักของฟอร์ม
            BackColor = Color.FromArgb(245, 248, 250);

            // ขนาดหน้าต่างหลักของ dashboard
            ClientSize = new Size(900, 500);

            // เพิ่ม controls หลักเข้า form
            Controls.Add(panelContent);
            Controls.Add(panelTopBar);

            Name = "RestaurantForm";

            // เปิดฟอร์มตรงกลางหน้าจอ
            StartPosition = FormStartPosition.CenterScreen;

            // ชื่อบน title bar
            Text = "Delivery App — ร้านอาหาร";

            // Resume layout หลังสร้าง UI เสร็จ
            panelTopBar.ResumeLayout(false);
            panelTopBar.PerformLayout();

            panelContent.ResumeLayout(false);
            panelContent.PerformLayout();

            ResumeLayout(false);
        }

        #endregion

        // =========================
        // CONTROL DECLARATIONS
        // Controls เหล่านี้จะถูกใช้งานใน RestaurantForm.cs
        // =========================

        // Header ด้านบนของ dashboard
        private Panel panelTopBar;

        // ชื่อ dashboard/app
        private Label lblAppTitle;

        // Label แสดงชื่อร้าน
        private Label label1;

        // ปุ่ม logout
        private Button buttonLogout;

        // พื้นที่หลักของ dashboard
        private Panel panelContent;

        // ข้อความอธิบายหน้า dashboard
        private Label lblSubheader;

        // ปุ่มจัดการออเดอร์
        private Button button1;

        // ปุ่มแก้ไขเมนู
        private Button button2;
    }
}