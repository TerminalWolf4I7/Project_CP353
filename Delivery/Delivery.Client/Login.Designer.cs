using System.Drawing;
using System.Windows.Forms;

namespace Delivery.Client
{
    // Partial class สำหรับหน้า Login
    // ไฟล์นี้เป็นฝั่ง Designer ที่ใช้สร้างและจัด layout ของ UI
    partial class Login
    {
        // Container สำหรับเก็บ resource/component ของ WinForms
        // เพื่อให้สามารถ dispose ทิ้งได้ตอนปิด form
        private System.ComponentModel.IContainer components = null;

        // จัดการ cleanup resource ตอน form ถูกทำลาย
        protected override void Dispose(bool disposing)
        {
            // ถ้ามี component ค้างอยู่ให้ dispose เพื่อลด memory leak
            if (disposing && (components != null)) components.Dispose();

            // เรียก dispose ของ base form ต่อ
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        // Method หลักสำหรับสร้าง UI component ทั้งหมดของหน้า Login
        // WinForms Designer จะ generate code ส่วนนี้อัตโนมัติ
        private void InitializeComponent()
        {
            // =========================
            // สร้าง component หลักของหน้า
            // =========================
            panelLeft = new Panel();
            lblEmoji = new Label();
            lblBrand = new Label();
            lblTagline = new Label();

            panelRight = new Panel();
            panelCard = new Panel();

            lblWelcome = new Label();
            lblSubtitle = new Label();
            lblUserHint = new Label();

            txtUserId = new TextBox();
            btnLogin = new Button();

            lblVersion = new Label();

            // SuspendLayout ช่วยหยุด redraw ชั่วคราว
            // เพื่อเพิ่ม performance ระหว่างสร้าง UI
            panelLeft.SuspendLayout();
            panelRight.SuspendLayout();
            panelCard.SuspendLayout();
            SuspendLayout();

            // =====================================================
            // LEFT PANEL
            // ส่วน branding / marketing ของแอป
            // =====================================================

            panelLeft.BackColor = Color.FromArgb(39, 174, 96);

            // เพิ่ม component ลงใน panel ซ้าย
            panelLeft.Controls.Add(lblEmoji);
            panelLeft.Controls.Add(lblBrand);
            panelLeft.Controls.Add(lblTagline);

            // Dock ซ้ายเต็มความสูงของ form
            panelLeft.Dock = DockStyle.Left;

            panelLeft.Location = new Point(0, 0);
            panelLeft.Margin = new Padding(2);
            panelLeft.Name = "panelLeft";

            // กำหนดความกว้าง fixed สำหรับ branding area
            panelLeft.Size = new Size(266, 360);
            panelLeft.TabIndex = 1;

            // =====================================================
            // LOGO EMOJI
            // ใช้ emoji แทน icon delivery เพื่อให้ UI ดู friendly
            // =====================================================

            lblEmoji.AutoSize = true;

            // ใช้ Segoe UI Emoji เพื่อรองรับ unicode emoji
            lblEmoji.Font = new Font("Segoe UI Emoji", 52F);

            lblEmoji.ForeColor = Color.White;
            lblEmoji.Location = new Point(67, 72);
            lblEmoji.Margin = new Padding(2, 0, 2, 0);
            lblEmoji.Name = "lblEmoji";
            lblEmoji.Size = new Size(136, 94);
            lblEmoji.TabIndex = 0;

            // Unicode scooter emoji
            lblEmoji.Text = "\U0001f6f5";

            // =====================================================
            // BRAND TITLE
            // =====================================================

            lblBrand.AutoSize = true;

            // ใช้ font ใหญ่และ bold เพื่อสร้าง visual hierarchy
            lblBrand.Font = new Font("Segoe UI", 32F, FontStyle.Bold);

            lblBrand.ForeColor = Color.White;
            lblBrand.Location = new Point(28, 157);
            lblBrand.Margin = new Padding(2, 0, 2, 0);
            lblBrand.Name = "lblBrand";
            lblBrand.Size = new Size(222, 59);
            lblBrand.TabIndex = 1;
            lblBrand.Text = "DELIVERY";

            // =====================================================
            // TAGLINE
            // ใช้ข้อความ marketing สั้น ๆ เพื่อเพิ่ม personality ให้ app
            // =====================================================

            lblTagline.Font = new Font("Segoe UI", 11F);

            // ใช้สีเขียวอ่อนเพื่อ contrast กับพื้นหลัง
            lblTagline.ForeColor = Color.FromArgb(200, 255, 220);

            lblTagline.Location = new Point(28, 216);
            lblTagline.Margin = new Padding(2, 0, 2, 0);
            lblTagline.Name = "lblTagline";
            lblTagline.Size = new Size(210, 42);
            lblTagline.TabIndex = 2;

            // \n สำหรับขึ้นบรรทัดใหม่
            lblTagline.Text = "อาหารอร่อย ส่งถึงมือคุณ\nเร็วกว่าที่คิด 🔥";

            // จัดข้อความให้อยู่กลาง panel
            lblTagline.TextAlign = ContentAlignment.MiddleCenter;

            // =====================================================
            // RIGHT PANEL
            // ส่วน interaction หลักของ user
            // =====================================================

            panelRight.BackColor = Color.FromArgb(245, 248, 250);

            // panelCard คือ container หลักของ form login
            panelRight.Controls.Add(panelCard);

            // footer version app
            panelRight.Controls.Add(lblVersion);

            // Fill พื้นที่ที่เหลือจาก panel ซ้าย
            panelRight.Dock = DockStyle.Fill;

            panelRight.Location = new Point(266, 0);
            panelRight.Margin = new Padding(2);
            panelRight.Name = "panelRight";
            panelRight.Size = new Size(322, 360);
            panelRight.TabIndex = 0;

            // =====================================================
            // LOGIN CARD
            // กล่องสีขาวตรงกลางสำหรับ form login
            // =====================================================

            panelCard.BackColor = Color.White;

            // เพิ่ม control ที่เกี่ยวข้องกับ login flow
            panelCard.Controls.Add(lblWelcome);
            panelCard.Controls.Add(lblSubtitle);
            panelCard.Controls.Add(lblUserHint);
            panelCard.Controls.Add(txtUserId);
            panelCard.Controls.Add(btnLogin);

            panelCard.Location = new Point(35, 60);
            panelCard.Margin = new Padding(2);
            panelCard.Name = "panelCard";
            panelCard.Size = new Size(252, 228);
            panelCard.TabIndex = 0;

            // =====================================================
            // HEADER TEXT
            // =====================================================

            lblWelcome.AutoSize = true;
            lblWelcome.Font = new Font("Segoe UI", 20F, FontStyle.Bold);

            // ใช้สีเข้มเพื่อให้อ่านง่าย
            lblWelcome.ForeColor = Color.FromArgb(30, 30, 30);

            lblWelcome.Location = new Point(21, 21);
            lblWelcome.Margin = new Padding(2, 0, 2, 0);
            lblWelcome.Name = "lblWelcome";
            lblWelcome.Size = new Size(206, 37);
            lblWelcome.TabIndex = 0;
            lblWelcome.Text = "ยินดีต้อนรับ! 👋";

            // =====================================================
            // SUBTITLE
            // ข้อความอธิบาย flow การ login
            // =====================================================

            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Segoe UI", 10F);

            // ใช้สีเทาเพื่อลด visual priority
            lblSubtitle.ForeColor = Color.Gray;

            lblSubtitle.Location = new Point(21, 58);
            lblSubtitle.Margin = new Padding(2, 0, 2, 0);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(161, 19);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "เข้าสู่ระบบเพื่อสั่งอาหารอร่อยๆ";

            // Event handler สำหรับ click subtitle
            // อาจใช้ debug/testing หรือ interactive effect
            lblSubtitle.Click += lblSubtitle_Click;

            // =====================================================
            // USER ID LABEL
            // =====================================================

            lblUserHint.AutoSize = true;

            // ใช้ font bold เพื่อเน้น field label
            lblUserHint.Font = new Font("Segoe UI", 8F, FontStyle.Bold);

            // ใช้สี theme เดียวกับ brand
            lblUserHint.ForeColor = Color.FromArgb(39, 174, 96);

            lblUserHint.Location = new Point(21, 93);
            lblUserHint.Margin = new Padding(2, 0, 2, 0);
            lblUserHint.Name = "lblUserHint";
            lblUserHint.Size = new Size(48, 13);
            lblUserHint.TabIndex = 2;
            lblUserHint.Text = "USER ID";

            // =====================================================
            // INPUT FIELD
            // รับค่า User ID สำหรับ login
            // =====================================================

            txtUserId.BorderStyle = BorderStyle.FixedSingle;

            // ใช้ font ใหญ่ขึ้นเพื่อให้อ่านง่ายเวลา user พิมพ์
            txtUserId.Font = new Font("Segoe UI", 13F);

            txtUserId.Location = new Point(21, 108);
            txtUserId.Margin = new Padding(2);
            txtUserId.Name = "txtUserId";

            // Placeholder ช่วยบอก format input ที่คาดหวัง
            txtUserId.PlaceholderText = "เช่น  1, 2, 3...";

            txtUserId.Size = new Size(211, 31);
            txtUserId.TabIndex = 0;

            // Trigger event ทุกครั้งที่ text เปลี่ยน
            // อาจใช้ validate input realtime
            txtUserId.TextChanged += txtUserId_TextChanged;

            // =====================================================
            // LOGIN BUTTON
            // จุดเริ่มต้นของ authentication flow
            // =====================================================

            // ใช้สีหลักของระบบ
            btnLogin.BackColor = Color.FromArgb(39, 174, 96);

            // เปลี่ยน cursor เพื่อบอกว่า clickable
            btnLogin.Cursor = Cursors.Hand;

            // ลบ border default ของ WinForms
            btnLogin.FlatAppearance.BorderSize = 0;

            // ใช้ flat style เพื่อให้ UI modern ขึ้น
            btnLogin.FlatStyle = FlatStyle.Flat;

            btnLogin.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;

            btnLogin.Location = new Point(21, 150);
            btnLogin.Margin = new Padding(2);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(210, 33);
            btnLogin.TabIndex = 1;

            // ใช้ arrow เพื่อเพิ่ม visual cue ว่าจะไปต่อ
            btnLogin.Text = "เข้าสู่ระบบ  →";

            btnLogin.UseVisualStyleBackColor = false;

            // Event หลักสำหรับ login process
            btnLogin.Click += btnLogin_Click;

            // =====================================================
            // VERSION FOOTER
            // ใช้สำหรับ debug/support/version tracking
            // =====================================================

            lblVersion.AutoSize = true;
            lblVersion.Font = new Font("Segoe UI", 8F);

            // ใช้สีอ่อนเพราะไม่ใช่ข้อมูลสำคัญหลัก
            lblVersion.ForeColor = Color.LightGray;

            lblVersion.Location = new Point(63, 324);
            lblVersion.Margin = new Padding(2, 0, 2, 0);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(178, 13);
            lblVersion.TabIndex = 1;
            lblVersion.Text = "Delivery App v1.0  |  CP353 Project";

            // =====================================================
            // FORM CONFIGURATION
            // ตั้งค่าพื้นฐานของหน้าต่าง Login
            // =====================================================

            // กด Enter แล้ว trigger ปุ่ม login อัตโนมัติ
            AcceptButton = btnLogin;

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;

            ClientSize = new Size(588, 360);

            // เพิ่ม panel หลักลงใน form
            Controls.Add(panelRight);
            Controls.Add(panelLeft);

            // ล็อกขนาด form ไม่ให้ resize
            FormBorderStyle = FormBorderStyle.FixedDialog;

            Margin = new Padding(2);

            // ปิด maximize เพื่อควบคุม layout UI
            MaximizeBox = false;

            Name = "Login";

            // เปิด form ตรงกลางหน้าจอ
            StartPosition = FormStartPosition.CenterScreen;

            Text = "Delivery App — Login";

            // ResumeLayout กลับมาวาด UI ตามปกติ
            panelLeft.ResumeLayout(false);
            panelLeft.PerformLayout();

            panelRight.ResumeLayout(false);
            panelRight.PerformLayout();

            panelCard.ResumeLayout(false);
            panelCard.PerformLayout();

            ResumeLayout(false);
        }

        #endregion

        // =========================
        // COMPONENT DECLARATIONS
        // ประกาศ control ที่ใช้ใน form
        // =========================

        private Panel panelLeft;
        private Label lblBrand;
        private Label lblTagline;
        private Label lblEmoji;

        private Panel panelRight;
        private Panel panelCard;

        private Label lblWelcome;
        private Label lblSubtitle;
        private Label lblUserHint;

        private TextBox txtUserId;

        private Button btnLogin;

        private Label lblVersion;
    }
}