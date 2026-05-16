using System.Drawing;
using System.Windows.Forms;

namespace Delivery.Client
{
    // Partial class ของ RiderForm ฝั่ง Designer
    // ไฟล์นี้ใช้สร้าง UI Dashboard ของ Rider
    // ส่วน logic เช่น โหลดออเดอร์, รับงาน, refresh timer จะอยู่ใน RiderForm.cs
    partial class RiderForm
    {
        /// <summary>
        /// Container สำหรับเก็บ component/resource ของ WinForms
        /// ใช้ร่วมกับ Dispose() เพื่อ cleanup memory/resource ตอนปิดฟอร์ม
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // =========================
        // CONTROL DECLARATIONS
        // Controls เหล่านี้จะถูกใช้งานใน RiderForm.cs
        // =========================

        // ตารางแสดงออเดอร์ที่รอ Rider รับงาน
        private System.Windows.Forms.DataGridView dataGridOrders;

        // ปุ่มดูรายละเอียดออเดอร์
        private System.Windows.Forms.Button buttonViewDetails;

        // ปุ่มรับงาน
        private System.Windows.Forms.Button buttonAcceptOrder;

        // ปุ่มเปิดหน้างานของ Rider
        private System.Windows.Forms.Button buttonRiderOrder;

        // ปุ่ม logout
        private Button buttonLogout;

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
        /// Method หลักสำหรับสร้าง UI ของ Rider Dashboard
        ///
        /// Workflow ของหน้านี้:
        /// 1. แสดงรายการออเดอร์ที่รอ Rider
        /// 2. Rider เลือก order จาก DataGrid
        /// 3. Rider ดูรายละเอียดหรือรับงาน
        /// 4. Rider เปิดดูงานปัจจุบันของตัวเองได้
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

            // หัวข้อหลักของหน้า
            lblRiderTitle = new Label();

            // ปุ่ม logout
            buttonLogout = new Button();

            // =========================
            // CONTENT SECTION
            // =========================

            // พื้นที่หลักของ dashboard
            panelContent = new Panel();

            // ข้อความอธิบายการใช้งาน
            lblSubheader = new Label();

            // ตารางรายการออเดอร์
            dataGridOrders = new DataGridView();

            // Panel สำหรับ action buttons
            panelButtons = new Panel();

            // ปุ่มดูรายละเอียด
            buttonViewDetails = new Button();

            // ปุ่มงานของฉัน
            buttonRiderOrder = new Button();

            // ปุ่มรับงาน
            buttonAcceptOrder = new Button();

            // Suspend layout ชั่วคราวเพื่อลด redraw ระหว่างสร้าง UI
            panelTopBar.SuspendLayout();
            panelContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridOrders).BeginInit();
            panelButtons.SuspendLayout();
            SuspendLayout();

            // =====================================================
            // TOP BAR PANEL
            // =====================================================

            // ใช้สีฟ้าเป็น theme ของ Rider dashboard
            panelTopBar.BackColor = Color.FromArgb(52, 152, 219);

            // เพิ่ม controls ลงใน header
            panelTopBar.Controls.Add(lblAppTitle);
            panelTopBar.Controls.Add(lblRiderTitle);
            panelTopBar.Controls.Add(buttonLogout);

            // Dock ด้านบนเต็มหน้าต่าง
            panelTopBar.Dock = DockStyle.Top;

            panelTopBar.Location = new Point(0, 0);
            panelTopBar.Name = "panelTopBar";

            // ความสูงของ header
            panelTopBar.Size = new Size(900, 80);

            panelTopBar.TabIndex = 0;

            // =====================================================
            // APP TITLE LABEL
            // =====================================================

            // หัวข้อ dashboard ของ Rider
            lblAppTitle.AutoSize = true;

            lblAppTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);

            // ใช้สีฟ้าอ่อนเพื่อ contrast กับพื้นหลัง
            lblAppTitle.ForeColor = Color.FromArgb(200, 230, 255);

            lblAppTitle.Location = new Point(20, 8);

            lblAppTitle.Name = "lblAppTitle";

            lblAppTitle.Size = new Size(200, 20);

            lblAppTitle.TabIndex = 0;

            // ใช้ emoji เพื่อเพิ่ม visual identity
            lblAppTitle.Text = "🏍️  RIDER DASHBOARD";

            // =====================================================
            // RIDER TITLE LABEL
            // =====================================================

            // หัวข้อหลักของหน้ารายการงาน
            lblRiderTitle.AutoSize = true;

            // ใช้ font ใหญ่เพื่อเป็นจุด focus หลัก
            lblRiderTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);

            lblRiderTitle.ForeColor = Color.White;

            lblRiderTitle.Location = new Point(20, 35);

            lblRiderTitle.Name = "lblRiderTitle";

            lblRiderTitle.Size = new Size(400, 37);

            lblRiderTitle.TabIndex = 1;

            lblRiderTitle.Text = "ออเดอร์ที่รอไรเดอร์รับงาน";

            // =====================================================
            // LOGOUT BUTTON
            // =====================================================

            // ปุ่มออกจากระบบ
            buttonLogout.BackColor = Color.White;

            // เปลี่ยน cursor เพื่อบอกว่า clickable
            buttonLogout.Cursor = Cursors.Hand;

            buttonLogout.FlatStyle = FlatStyle.Flat;

            buttonLogout.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            // ใช้สีแดงเพื่อสื่อถึง logout action
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

            // เพิ่ม controls หลักเข้า panel
            panelContent.Controls.Add(dataGridOrders);
            panelContent.Controls.Add(lblSubheader);
            panelContent.Controls.Add(panelButtons);

            // Fill พื้นที่ที่เหลือจาก top bar
            panelContent.Dock = DockStyle.Fill;

            panelContent.Location = new Point(0, 80);

            panelContent.Name = "panelContent";

            panelContent.Size = new Size(900, 420);

            panelContent.TabIndex = 1;

            // =====================================================
            // SUBHEADER LABEL
            // =====================================================

            // ข้อความอธิบาย flow การใช้งาน
            lblSubheader.AutoSize = true;

            lblSubheader.Font = new Font("Segoe UI", 10F);

            // ใช้สีเทาเพื่อลด visual priority
            lblSubheader.ForeColor = Color.FromArgb(100, 100, 100);

            lblSubheader.Location = new Point(20, 12);

            lblSubheader.Name = "lblSubheader";

            lblSubheader.Size = new Size(300, 19);

            lblSubheader.TabIndex = 0;

            lblSubheader.Text = "📋  เลือกออเดอร์แล้วกดรับงานได้เลย";

            // =====================================================
            // DATA GRID ORDERS
            // =====================================================

            // ปิดการเพิ่ม row เองจาก UI
            // เพราะข้อมูลมาจาก API เท่านั้น
            dataGridOrders.AllowUserToAddRows = false;

            // ปิดการลบ row ตรง ๆ จาก grid
            dataGridOrders.AllowUserToDeleteRows = false;

            // ให้ column ขยายเต็มพื้นที่
            dataGridOrders.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            // พื้นหลังสีขาวเพื่อให้อ่านง่าย
            dataGridOrders.BackgroundColor = Color.White;

            dataGridOrders.BorderStyle = BorderStyle.None;

            // ใช้เส้นแบ่งเฉพาะแนวนอนเพื่อลด visual noise
            dataGridOrders.CellBorderStyle =
                DataGridViewCellBorderStyle.SingleHorizontal;

            dataGridOrders.ColumnHeadersBorderStyle =
                DataGridViewHeaderBorderStyle.None;

            // =========================
            // HEADER STYLE
            // =========================

            // ปรับ style ของ header ให้เข้ากับ Rider theme
            dataGridOrders.ColumnHeadersDefaultCellStyle =
                new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(52, 152, 219),

                    ForeColor = Color.White,

                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),

                    // กันสี selection เปลี่ยนตอน focus
                    SelectionBackColor = Color.FromArgb(52, 152, 219),

                    // จัดข้อความให้อยู่กลาง
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                };

            dataGridOrders.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            // =========================
            // CELL STYLE
            // =========================

            dataGridOrders.DefaultCellStyle =
                new DataGridViewCellStyle
                {
                    BackColor = Color.White,

                    ForeColor = Color.FromArgb(44, 62, 80),

                    Font = new Font("Segoe UI", 10F),

                    // สี selection ฟ้าอ่อนเพื่อ match theme
                    SelectionBackColor = Color.FromArgb(200, 230, 255),

                    SelectionForeColor = Color.FromArgb(44, 62, 80),

                    // เพิ่ม padding เพื่อให้อ่านง่าย
                    Padding = new Padding(5)
                };

            // ปิด visual style ของ Windows default
            dataGridOrders.EnableHeadersVisualStyles = false;

            // สีเส้น grid
            dataGridOrders.GridColor = Color.FromArgb(230, 230, 230);

            dataGridOrders.Location = new Point(20, 40);

            // Rider เลือกได้ทีละ order
            dataGridOrders.MultiSelect = false;

            dataGridOrders.Name = "dataGridOrders";

            // Grid นี้ใช้ดูข้อมูลเท่านั้น
            dataGridOrders.ReadOnly = true;

            // ซ่อน row header ด้านซ้าย
            dataGridOrders.RowHeadersVisible = false;

            // เพิ่มความสูง row เพื่อให้อ่านง่าย
            dataGridOrders.RowTemplate.Height = 40;

            // เลือกทั้ง row
            dataGridOrders.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dataGridOrders.Size = new Size(860, 300);

            dataGridOrders.TabIndex = 1;

            // =====================================================
            // BUTTON PANEL
            // =====================================================

            // Panel สำหรับ action buttons ของ Rider
            panelButtons.Controls.Add(buttonViewDetails);
            panelButtons.Controls.Add(buttonRiderOrder);
            panelButtons.Controls.Add(buttonAcceptOrder);

            panelButtons.Location = new Point(20, 350);

            panelButtons.Name = "panelButtons";

            panelButtons.Size = new Size(860, 55);

            panelButtons.TabIndex = 2;

            // =====================================================
            // VIEW DETAILS BUTTON
            // =====================================================

            // ปุ่มดูรายละเอียดออเดอร์
            buttonViewDetails.BackColor =
                Color.FromArgb(149, 165, 166);

            buttonViewDetails.Cursor = Cursors.Hand;

            buttonViewDetails.FlatAppearance.BorderSize = 0;

            buttonViewDetails.FlatStyle = FlatStyle.Flat;

            buttonViewDetails.Font =
                new Font("Segoe UI", 11F, FontStyle.Bold);

            buttonViewDetails.ForeColor = Color.White;

            buttonViewDetails.Location = new Point(0, 5);

            buttonViewDetails.Name = "buttonViewDetails";

            buttonViewDetails.Size = new Size(200, 45);

            buttonViewDetails.TabIndex = 0;

            // สีเทา = action รอง
            buttonViewDetails.Text = "🔍 ดูรายละเอียด";

            buttonViewDetails.UseVisualStyleBackColor = false;

            // =====================================================
            // RIDER ORDER BUTTON
            // =====================================================

            // ปุ่มเปิดดูงานปัจจุบันของ Rider
            buttonRiderOrder.BackColor =
                Color.FromArgb(155, 89, 182);

            buttonRiderOrder.Cursor = Cursors.Hand;

            buttonRiderOrder.FlatAppearance.BorderSize = 0;

            buttonRiderOrder.FlatStyle = FlatStyle.Flat;

            buttonRiderOrder.Font =
                new Font("Segoe UI", 11F, FontStyle.Bold);

            buttonRiderOrder.ForeColor = Color.White;

            buttonRiderOrder.Location = new Point(220, 5);

            buttonRiderOrder.Name = "buttonRiderOrder";

            buttonRiderOrder.Size = new Size(200, 45);

            buttonRiderOrder.TabIndex = 1;

            // สีม่วง = action เฉพาะของ Rider
            buttonRiderOrder.Text = "🛵 งานของฉัน";

            buttonRiderOrder.UseVisualStyleBackColor = false;

            // =====================================================
            // ACCEPT ORDER BUTTON
            // =====================================================

            // ปุ่มรับงานที่เลือก
            buttonAcceptOrder.BackColor =
                Color.FromArgb(39, 174, 96);

            buttonAcceptOrder.Cursor = Cursors.Hand;

            buttonAcceptOrder.FlatAppearance.BorderSize = 0;

            buttonAcceptOrder.FlatStyle = FlatStyle.Flat;

            buttonAcceptOrder.Font =
                new Font("Segoe UI", 11F, FontStyle.Bold);

            buttonAcceptOrder.ForeColor = Color.White;

            // วางไว้ขวาสุดเพื่อเน้นเป็น primary action
            buttonAcceptOrder.Location = new Point(660, 5);

            buttonAcceptOrder.Name = "buttonAcceptOrder";

            buttonAcceptOrder.Size = new Size(200, 45);

            buttonAcceptOrder.TabIndex = 2;

            // สีเขียว = action ยืนยัน/รับงาน
            buttonAcceptOrder.Text = "✅ รับงานนี้";

            buttonAcceptOrder.UseVisualStyleBackColor = false;

            // =====================================================
            // FORM CONFIGURATION
            // =====================================================

            AutoScaleDimensions = new SizeF(7F, 15F);

            // รองรับ DPI/font scaling
            AutoScaleMode = AutoScaleMode.Font;

            // สีพื้นหลังหลักของฟอร์ม
            BackColor = Color.FromArgb(245, 248, 250);

            // ขนาดหน้าต่างหลัก
            ClientSize = new Size(900, 500);

            // เพิ่ม controls หลักเข้า form
            Controls.Add(panelContent);
            Controls.Add(panelTopBar);

            Name = "RiderForm";

            // เปิดฟอร์มกลางหน้าจอ
            StartPosition = FormStartPosition.CenterScreen;

            // ชื่อบน title bar
            Text = "Delivery App — Rider";

            // Resume layout หลังสร้าง UI เสร็จ
            panelTopBar.ResumeLayout(false);
            panelTopBar.PerformLayout();

            panelContent.ResumeLayout(false);
            panelContent.PerformLayout();

            ((System.ComponentModel.ISupportInitialize)dataGridOrders).EndInit();

            panelButtons.ResumeLayout(false);

            ResumeLayout(false);
        }

        #endregion

        // =========================
        // CONTROL DECLARATIONS
        // Controls เหล่านี้จะถูกใช้งานใน RiderForm.cs
        // =========================

        // Header ด้านบนของ Rider dashboard
        private Panel panelTopBar;

        // ชื่อ dashboard/app
        private Label lblAppTitle;

        // หัวข้อหลักของหน้า
        private Label lblRiderTitle;

        // พื้นที่หลักของ dashboard
        private Panel panelContent;

        // ข้อความอธิบายการใช้งาน
        private Label lblSubheader;

        // Panel สำหรับ action buttons
        private Panel panelButtons;
    }
}