using System.Drawing;
using System.Windows.Forms;

namespace Delivery.Client
{
    // Partial class ของ RiderOrderForm ฝั่ง Designer
    // ไฟล์นี้ใช้สร้าง UI สำหรับหน้าดู "งานจัดส่งปัจจุบัน" ของ Rider
    // ส่วน logic เช่น โหลด order และ complete delivery จะอยู่ใน RiderOrderForm.cs
    partial class RiderOrderForm
    {
        // Container สำหรับเก็บ component/resource ของ WinForms
        // ใช้ cleanup ตอนปิดฟอร์ม
        private System.ComponentModel.IContainer components = null;

        // =========================
        // CONTROL DECLARATIONS
        // Controls เหล่านี้จะถูกใช้งานใน RiderOrderForm.cs
        // =========================

        // ตารางแสดงงานปัจจุบันของ Rider
        private DataGridView dataGridOrder;

        // ปุ่มยืนยันว่าจัดส่งสำเร็จ
        private Button buttonComplete;

        // ปุ่มย้อนกลับไป RiderForm
        private Button buttonBack;

        // Cleanup resource ของฟอร์ม
        protected override void Dispose(bool disposing)
        {
            // ถ้ามี component ถูกสร้างไว้ ให้ dispose ทิ้งเพื่อลด memory leak
            if (disposing && components != null)
            {
                components.Dispose();
            }

            // เรียก Dispose ของ base Form ต่อ
            base.Dispose(disposing);
        }

        // Method หลักสำหรับสร้าง UI ของ RiderOrderForm
        //
        // Workflow ของหน้านี้:
        // 1. แสดงงานที่ Rider รับอยู่
        // 2. Rider ตรวจสอบข้อมูล order
        // 3. Rider กดยืนยันส่งสำเร็จ
        // 4. ระบบอัปเดตสถานะ order ฝั่ง server
        private void InitializeComponent()
        {
            // =========================
            // TOP BAR
            // =========================

            // Header ด้านบนของหน้า
            panelTopBar = new Panel();

            // หัวข้อหลักของหน้า
            lblTitle = new Label();

            // =========================
            // ORDER GRID
            // =========================

            // ตารางแสดง order ปัจจุบันของ Rider
            dataGridOrder = new DataGridView();

            // =========================
            // BUTTON PANEL
            // =========================

            // Panel สำหรับ action buttons
            panelButtons = new Panel();

            // ปุ่ม complete delivery
            buttonComplete = new Button();

            // ปุ่มย้อนกลับ
            buttonBack = new Button();

            // Suspend layout ชั่วคราวเพื่อลด redraw ระหว่างสร้าง UI
            panelTopBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridOrder).BeginInit();
            panelButtons.SuspendLayout();
            SuspendLayout();

            // =====================================================
            // TOP BAR PANEL
            // =====================================================

            // ใช้สีม่วงเพื่อแยก flow "งานปัจจุบัน"
            // ออกจาก Rider dashboard หลัก
            panelTopBar.BackColor = Color.FromArgb(155, 89, 182);

            // เพิ่ม title เข้า header
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

            // ใช้ font ใหญ่เพื่อให้เป็นจุด focus หลัก
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);

            // สีขาวเพื่อ contrast กับพื้นหลังสีม่วง
            lblTitle.ForeColor = Color.White;

            lblTitle.Location = new Point(20, 10);

            lblTitle.Name = "lblTitle";

            lblTitle.Size = new Size(300, 30);

            lblTitle.TabIndex = 0;

            // ใช้ emoji เพื่อเพิ่ม visual identity
            lblTitle.Text = "🛵  งานจัดส่งของฉัน";

            // =====================================================
            // DATA GRID ORDER
            // =====================================================

            // ปิดการเพิ่ม row เองจาก UI
            // เพราะข้อมูลมาจาก API เท่านั้น
            dataGridOrder.AllowUserToAddRows = false;

            // ปิดการลบ row ตรง ๆ จาก UI
            dataGridOrder.AllowUserToDeleteRows = false;

            // ให้ column ขยายเต็มพื้นที่ grid
            dataGridOrder.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            // พื้นหลังสีขาวเพื่อให้อ่านง่าย
            dataGridOrder.BackgroundColor = Color.White;

            dataGridOrder.BorderStyle = BorderStyle.None;

            // ใช้เส้นแบ่งแนวนอนเพื่อลด visual noise
            dataGridOrder.CellBorderStyle =
                DataGridViewCellBorderStyle.SingleHorizontal;

            dataGridOrder.ColumnHeadersBorderStyle =
                DataGridViewHeaderBorderStyle.None;

            // =========================
            // HEADER STYLE
            // =========================

            // ปรับ style ของ header ให้ match กับ theme สีม่วง
            dataGridOrder.ColumnHeadersDefaultCellStyle =
                new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(155, 89, 182),

                    ForeColor = Color.White,

                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),

                    // กันสี selection เปลี่ยนตอน focus
                    SelectionBackColor = Color.FromArgb(155, 89, 182),

                    // จัดข้อความให้อยู่กลาง
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                };

            dataGridOrder.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            // =========================
            // CELL STYLE
            // =========================

            dataGridOrder.DefaultCellStyle =
                new DataGridViewCellStyle
                {
                    BackColor = Color.White,

                    ForeColor = Color.FromArgb(44, 62, 80),

                    Font = new Font("Segoe UI", 10F),

                    // สีม่วงอ่อนเวลา select row
                    SelectionBackColor = Color.FromArgb(230, 210, 245),

                    SelectionForeColor = Color.FromArgb(44, 62, 80),

                    // เพิ่ม padding เพื่อให้อ่านง่าย
                    Padding = new Padding(5)
                };

            // ปิด visual style default ของ Windows
            dataGridOrder.EnableHeadersVisualStyles = false;

            // สีเส้น grid
            dataGridOrder.GridColor = Color.FromArgb(230, 230, 230);

            dataGridOrder.Location = new Point(20, 65);

            dataGridOrder.Name = "dataGridOrder";

            // Grid นี้ใช้ดูข้อมูลเท่านั้น
            dataGridOrder.ReadOnly = true;

            // ซ่อน row header ด้านซ้าย
            dataGridOrder.RowHeadersVisible = false;

            // เพิ่มความสูง row เพื่อให้อ่านง่าย
            dataGridOrder.RowTemplate.Height = 40;

            dataGridOrder.Size = new Size(860, 320);

            dataGridOrder.TabIndex = 1;

            // =====================================================
            // BUTTON PANEL
            // =====================================================

            // Panel สำหรับปุ่ม action ของ Rider
            panelButtons.BackColor =
                Color.FromArgb(245, 248, 250);

            panelButtons.Controls.Add(buttonComplete);
            panelButtons.Controls.Add(buttonBack);

            // Dock ด้านล่างของฟอร์ม
            panelButtons.Dock = DockStyle.Bottom;

            panelButtons.Location = new Point(0, 400);

            panelButtons.Name = "panelButtons";

            panelButtons.Size = new Size(900, 65);

            panelButtons.TabIndex = 2;

            // =====================================================
            // COMPLETE BUTTON
            // =====================================================

            // ปุ่มยืนยันว่าจัดส่งสำเร็จแล้ว
            buttonComplete.BackColor =
                Color.FromArgb(39, 174, 96);

            // เปลี่ยน cursor เพื่อบอกว่า clickable
            buttonComplete.Cursor = Cursors.Hand;

            buttonComplete.FlatAppearance.BorderSize = 0;

            buttonComplete.FlatStyle = FlatStyle.Flat;

            buttonComplete.Font =
                new Font("Segoe UI", 11F, FontStyle.Bold);

            buttonComplete.ForeColor = Color.White;

            // วางไว้ขวาสุดเพื่อเป็น primary action
            buttonComplete.Location = new Point(660, 10);

            buttonComplete.Name = "buttonComplete";

            buttonComplete.Size = new Size(220, 45);

            buttonComplete.TabIndex = 0;

            // สีเขียว = action สำเร็จ/confirm
            buttonComplete.Text = "✅ จัดส่งสำเร็จ";

            buttonComplete.UseVisualStyleBackColor = false;

            // Event นี้ถูก generate โดย Designer
            // Logic จริงจะถูก bind เพิ่มใน RiderOrderForm.cs
            buttonComplete.Click += buttonComplete_Click_1;

            // =====================================================
            // BACK BUTTON
            // =====================================================

            // ปุ่มย้อนกลับไปหน้า Rider dashboard
            buttonBack.BackColor =
                Color.FromArgb(149, 165, 166);

            buttonBack.Cursor = Cursors.Hand;

            buttonBack.FlatAppearance.BorderSize = 0;

            buttonBack.FlatStyle = FlatStyle.Flat;

            buttonBack.Font =
                new Font("Segoe UI", 11F, FontStyle.Bold);

            buttonBack.ForeColor = Color.White;

            // วางไว้ซ้ายสุดเพื่อเป็น secondary action
            buttonBack.Location = new Point(20, 10);

            buttonBack.Name = "buttonBack";

            buttonBack.Size = new Size(200, 45);

            buttonBack.TabIndex = 1;

            // ใช้ลูกศรซ้ายเป็น visual cue ว่าย้อนกลับ
            buttonBack.Text = "⬅️ กลับ";

            buttonBack.UseVisualStyleBackColor = false;

            // =====================================================
            // FORM CONFIGURATION
            // =====================================================

            AutoScaleDimensions = new SizeF(7F, 15F);

            // รองรับ DPI/font scaling
            AutoScaleMode = AutoScaleMode.Font;

            // สีพื้นหลังหลักของฟอร์ม
            BackColor = Color.FromArgb(245, 248, 250);

            // ขนาดหน้าต่างหลัก
            ClientSize = new Size(900, 465);

            // เพิ่ม controls หลักเข้า form
            Controls.Add(dataGridOrder);
            Controls.Add(panelButtons);
            Controls.Add(panelTopBar);

            Name = "RiderOrderForm";

            // เปิดฟอร์มตรงกลางหน้าจอ
            StartPosition = FormStartPosition.CenterScreen;

            // ชื่อบน title bar
            Text = "Delivery App — งานจัดส่ง";

            // Resume layout หลังสร้าง UI เสร็จ
            panelTopBar.ResumeLayout(false);
            panelTopBar.PerformLayout();

            ((System.ComponentModel.ISupportInitialize)dataGridOrder).EndInit();

            panelButtons.ResumeLayout(false);

            ResumeLayout(false);
        }

        // =========================
        // CONTROLS USED IN THIS FORM
        // =========================

        // Header ด้านบนของหน้า
        private Panel panelTopBar;

        // หัวข้อหลักของหน้า
        private Label lblTitle;

        // Panel สำหรับปุ่ม action
        private Panel panelButtons;
    }
}