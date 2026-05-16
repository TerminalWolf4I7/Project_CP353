using System.Drawing;
using System.Windows.Forms;

namespace Delivery.Client
{
    // Partial class ของ RestaurantOrdersForm ฝั่ง Designer
    // ไฟล์นี้ใช้สร้าง UI สำหรับหน้าจัดการออเดอร์ของร้านอาหาร
    // ส่วน logic เช่น โหลด order, เปลี่ยนสถานะ, ลบ order จะอยู่ใน RestaurantOrdersForm.cs
    partial class RestaurantOrdersForm
    {
        /// <summary>
        /// Container สำหรับเก็บ component/resource ของ WinForms
        /// ใช้ร่วมกับ Dispose() เพื่อ cleanup resource ตอนปิดฟอร์ม
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
        /// Method หลักสำหรับสร้าง UI ของหน้าจัดการออเดอร์
        ///
        /// Workflow ของหน้านี้:
        /// 1. แสดงรายการออเดอร์ของร้าน
        /// 2. เลือก order จาก DataGrid
        /// 3. แสดง action ตาม status ของ order
        /// 4. ร้านสามารถรับ/ปฏิเสธ/ทำอาหารเสร็จได้
        /// </summary>
        private void InitializeComponent()
        {
            // =========================
            // TOP BAR
            // =========================

            // Header ด้านบนของหน้า
            panelTopBar = new Panel();

            // หัวข้อหน้าจัดการออเดอร์
            lblTitle = new Label();

            // =========================
            // ORDERS GRID
            // =========================

            // ตารางรายการออเดอร์
            dataGridOrders = new DataGridView();

            // =========================
            // BUTTON PANEL
            // =========================

            // Panel ด้านล่างสำหรับ action buttons
            panelButtons = new Panel();

            // ปุ่มรับออเดอร์
            buttonAccept = new Button();

            // ปุ่มปฏิเสธออเดอร์
            buttonDecline = new Button();

            // ปุ่มแจ้งว่าทำอาหารเสร็จ
            buttonFinishCooking = new Button();

            // Suspend layout ชั่วคราวเพื่อลด redraw ระหว่างสร้าง UI
            panelTopBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridOrders).BeginInit();
            panelButtons.SuspendLayout();
            SuspendLayout();

            // =====================================================
            // TOP BAR PANEL
            // =====================================================

            // ใช้สีส้ม theme เดียวกับ Restaurant Dashboard
            panelTopBar.BackColor = Color.FromArgb(230, 126, 34);

            // เพิ่ม title ลง header
            panelTopBar.Controls.Add(lblTitle);

            // Dock ด้านบนเต็มหน้าต่าง
            panelTopBar.Dock = DockStyle.Top;

            panelTopBar.Location = new Point(0, 0);
            panelTopBar.Name = "panelTopBar";

            // ความสูงของ header
            panelTopBar.Size = new Size(900, 55);

            panelTopBar.TabIndex = 0;

            // =====================================================
            // TITLE LABEL
            // =====================================================

            // หัวข้อหลักของหน้า
            lblTitle.AutoSize = true;

            // ใช้ font ใหญ่เพื่อสร้าง hierarchy ของ UI
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);

            // สีขาวเพื่อ contrast กับพื้นหลังสีส้ม
            lblTitle.ForeColor = Color.White;

            lblTitle.Location = new Point(20, 10);
            lblTitle.Name = "lblTitle";

            lblTitle.Size = new Size(300, 30);

            lblTitle.TabIndex = 0;

            // ใช้ emoji เพื่อเพิ่ม visual identity
            lblTitle.Text = "📦  รายการออเดอร์";

            // =====================================================
            // DATA GRID ORDERS
            // =====================================================

            // ปิดการเพิ่ม row เองจาก UI
            // เพราะข้อมูลมาจาก API เท่านั้น
            dataGridOrders.AllowUserToAddRows = false;

            // ปิดการลบ row ตรง ๆ จาก grid
            // การลบต้องผ่าน business logic
            dataGridOrders.AllowUserToDeleteRows = false;

            // ให้ column ขยายเต็มพื้นที่ grid
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

            // ปรับ style ของ header ให้เข้ากับ theme ร้านอาหาร
            dataGridOrders.ColumnHeadersDefaultCellStyle =
                new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(230, 126, 34),

                    ForeColor = Color.White,

                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),

                    // กันสี selection เปลี่ยนตอน focus
                    SelectionBackColor = Color.FromArgb(230, 126, 34),

                    // จัดข้อความให้อยู่กลาง
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                };

            dataGridOrders.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            // =========================
            // CELL STYLE
            // =========================

            // style ของข้อมูลใน grid
            dataGridOrders.DefaultCellStyle =
                new DataGridViewCellStyle
                {
                    BackColor = Color.White,

                    ForeColor = Color.FromArgb(44, 62, 80),

                    Font = new Font("Segoe UI", 10F),

                    // สี selection อ่อนเพื่อให้อ่านข้อความง่าย
                    SelectionBackColor = Color.FromArgb(255, 230, 200),

                    SelectionForeColor = Color.FromArgb(44, 62, 80),

                    // เพิ่ม padding เพื่อให้อ่านง่ายขึ้น
                    Padding = new Padding(5)
                };

            // ปิด visual style ของ Windows default
            // เพื่อให้ใช้สี custom ได้จริง
            dataGridOrders.EnableHeadersVisualStyles = false;

            // สีเส้น grid
            dataGridOrders.GridColor = Color.FromArgb(230, 230, 230);

            dataGridOrders.Location = new Point(20, 65);

            // อนุญาตเลือกได้ทีละ order
            dataGridOrders.MultiSelect = false;

            dataGridOrders.Name = "dataGridOrders";

            // Grid นี้ใช้ดูข้อมูลเท่านั้น
            // ไม่ให้แก้ไขตรง ๆ
            dataGridOrders.ReadOnly = true;

            // ซ่อน row header ด้านซ้าย
            dataGridOrders.RowHeadersVisible = false;

            // เพิ่มความสูง row เพื่อให้อ่านง่าย
            dataGridOrders.RowTemplate.Height = 40;

            // เลือกทั้ง row เวลากด
            dataGridOrders.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dataGridOrders.Size = new Size(860, 330);

            dataGridOrders.TabIndex = 1;

            // =====================================================
            // BUTTON PANEL
            // =====================================================

            // Panel ด้านล่างสำหรับ action buttons
            panelButtons.BackColor = Color.FromArgb(245, 248, 250);

            panelButtons.Controls.Add(buttonAccept);
            panelButtons.Controls.Add(buttonDecline);
            panelButtons.Controls.Add(buttonFinishCooking);

            // Dock ล่างสุดของฟอร์ม
            panelButtons.Dock = DockStyle.Bottom;

            panelButtons.Location = new Point(0, 410);

            panelButtons.Name = "panelButtons";

            panelButtons.Size = new Size(900, 65);

            panelButtons.TabIndex = 2;

            // =====================================================
            // ACCEPT BUTTON
            // =====================================================

            // ปุ่มรับออเดอร์
            // ใช้กับ order ที่ status = Pending
            buttonAccept.BackColor = Color.FromArgb(39, 174, 96);

            buttonAccept.Cursor = Cursors.Hand;

            buttonAccept.FlatAppearance.BorderSize = 0;

            buttonAccept.FlatStyle = FlatStyle.Flat;

            buttonAccept.Font = new Font("Segoe UI", 11F, FontStyle.Bold);

            buttonAccept.ForeColor = Color.White;

            buttonAccept.Location = new Point(20, 10);

            buttonAccept.Name = "buttonAccept";

            buttonAccept.Size = new Size(200, 45);

            buttonAccept.TabIndex = 0;

            // สีเขียว = action เชิงบวก
            buttonAccept.Text = "✅ รับออเดอร์";

            buttonAccept.UseVisualStyleBackColor = false;

            // =====================================================
            // DECLINE BUTTON
            // =====================================================

            // ปุ่มปฏิเสธออเดอร์
            // ใช้ลบ order ที่ยัง Pending
            buttonDecline.BackColor = Color.FromArgb(231, 76, 60);

            buttonDecline.Cursor = Cursors.Hand;

            buttonDecline.FlatAppearance.BorderSize = 0;

            buttonDecline.FlatStyle = FlatStyle.Flat;

            buttonDecline.Font = new Font("Segoe UI", 11F, FontStyle.Bold);

            buttonDecline.ForeColor = Color.White;

            buttonDecline.Location = new Point(240, 10);

            buttonDecline.Name = "buttonDecline";

            buttonDecline.Size = new Size(200, 45);

            buttonDecline.TabIndex = 1;

            // สีแดง = action ลบ/ปฏิเสธ
            buttonDecline.Text = "❌ ปฏิเสธ";

            buttonDecline.UseVisualStyleBackColor = false;

            // =====================================================
            // FINISH COOKING BUTTON
            // =====================================================

            // ปุ่มแจ้งว่าทำอาหารเสร็จแล้ว
            // ใช้กับ order ที่ status = Cooking
            buttonFinishCooking.BackColor = Color.FromArgb(230, 126, 34);

            buttonFinishCooking.Cursor = Cursors.Hand;

            buttonFinishCooking.FlatAppearance.BorderSize = 0;

            buttonFinishCooking.FlatStyle = FlatStyle.Flat;

            buttonFinishCooking.Font = new Font("Segoe UI", 11F, FontStyle.Bold);

            buttonFinishCooking.ForeColor = Color.White;

            // วางไว้ด้านขวาเพื่อแยกจาก accept/decline
            buttonFinishCooking.Location = new Point(680, 10);

            buttonFinishCooking.Name = "buttonFinishCooking";

            buttonFinishCooking.Size = new Size(200, 45);

            buttonFinishCooking.TabIndex = 2;

            // สีส้ม = action workflow ของร้านอาหาร
            buttonFinishCooking.Text = "🍳 ทำอาหารเสร็จ";

            buttonFinishCooking.UseVisualStyleBackColor = false;

            // =====================================================
            // FORM CONFIGURATION
            // =====================================================

            AutoScaleDimensions = new SizeF(7F, 15F);

            // รองรับ DPI/font scaling
            AutoScaleMode = AutoScaleMode.Font;

            // สีพื้นหลังหลักของฟอร์ม
            BackColor = Color.FromArgb(245, 248, 250);

            // ขนาดหน้าต่างหลัก
            ClientSize = new Size(900, 475);

            // เพิ่ม controls หลักเข้า form
            Controls.Add(dataGridOrders);
            Controls.Add(panelButtons);
            Controls.Add(panelTopBar);

            Name = "RestaurantOrdersForm";

            // เปิดฟอร์มตรงกลางหน้าจอ
            StartPosition = FormStartPosition.CenterScreen;

            // ชื่อบน title bar
            Text = "Delivery App — ออเดอร์ร้านอาหาร";

            // Resume layout หลังสร้าง UI เสร็จ
            panelTopBar.ResumeLayout(false);
            panelTopBar.PerformLayout();

            ((System.ComponentModel.ISupportInitialize)dataGridOrders).EndInit();

            panelButtons.ResumeLayout(false);

            ResumeLayout(false);
        }

        #endregion

        // =========================
        // CONTROL DECLARATIONS
        // Controls เหล่านี้จะถูกใช้งานใน RestaurantOrdersForm.cs
        // =========================

        // Header ด้านบนของหน้า
        private Panel panelTopBar;

        // หัวข้อหน้าจัดการออเดอร์
        private Label lblTitle;

        // ตารางรายการออเดอร์
        private DataGridView dataGridOrders;

        // Panel สำหรับ action buttons
        private Panel panelButtons;

        // ปุ่มรับออเดอร์
        private Button buttonAccept;

        // ปุ่มปฏิเสธออเดอร์
        private Button buttonDecline;

        // ปุ่มแจ้งว่าทำอาหารเสร็จ
        private Button buttonFinishCooking;
    }
}