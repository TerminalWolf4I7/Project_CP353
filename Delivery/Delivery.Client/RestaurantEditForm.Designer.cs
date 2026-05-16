using System.Drawing;
using System.Windows.Forms;

namespace Delivery.Client
{
    // Partial class ของ RestaurantEditForm ฝั่ง Designer
    // ไฟล์นี้ทำหน้าที่สร้างและจัด layout ของ UI controls
    // ส่วน business logic เช่น โหลดเมนู, save, delete จะอยู่ใน RestaurantEditForm.cs
    partial class RestaurantEditForm
    {
        /// <summary>
        /// Container สำหรับเก็บ component/resource ที่ถูกสร้างโดย Designer
        /// ใช้ร่วมกับ Dispose() เพื่อ cleanup memory/resource ตอนปิดฟอร์ม
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Cleanup resource ที่ form นี้ใช้งานอยู่
        /// </summary>
        /// <param name="disposing">
        /// true = dispose managed resource ได้
        /// false = ถูกเรียกจาก finalizer path
        /// </param>
        protected override void Dispose(bool disposing)
        {
            // ถ้ามี component ถูกสร้างไว้ ให้ dispose ทิ้งเพื่อลด memory leak
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            // เรียก cleanup ของ base Form ต่อ
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Method หลักที่ WinForms Designer ใช้สร้าง UI ของหน้า RestaurantEditForm
        ///
        /// หน้านี้มี workflow หลัก:
        /// 1. แก้ชื่อร้าน
        /// 2. ดู/แก้ไขรายการเมนู
        /// 3. เพิ่มหรือลบเมนู
        /// 4. บันทึกการเปลี่ยนแปลงกลับไปยัง server
        /// </summary>
        private void InitializeComponent()
        {
            // =========================
            // TOP BAR
            // =========================

            // แถบบนสุดของหน้า
            panelTopBar = new Panel();

            // หัวข้อของฟอร์ม
            lblTitle = new Label();

            // =========================
            // RESTAURANT NAME SECTION
            // =========================

            // Panel สำหรับแถวชื่อร้าน
            panelNameRow = new Panel();

            // Label "ชื่อร้าน"
            labelRestaurantName = new Label();

            // TextBox สำหรับแก้ชื่อร้าน
            textRestaurantName = new TextBox();

            // =========================
            // MENU GRID
            // =========================

            // ตารางสำหรับแสดงและแก้ไขเมนู
            dataGridMenu = new DataGridView();

            // =========================
            // BUTTON SECTION
            // =========================

            // Panel ด้านล่างสำหรับปุ่มต่าง ๆ
            panelButtons = new Panel();

            // ปุ่มเพิ่มเมนู
            buttonAdd = new Button();

            // ปุ่มลบเมนู
            buttonDelete = new Button();

            // ปุ่มบันทึก
            buttonSave = new Button();

            // Suspend layout ชั่วคราวเพื่อลด redraw ระหว่างสร้าง UI
            panelTopBar.SuspendLayout();
            panelNameRow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridMenu).BeginInit();
            panelButtons.SuspendLayout();
            SuspendLayout();

            // =====================================================
            // TOP BAR PANEL
            // =====================================================

            // ใช้สีฟ้าเป็น primary theme ของหน้า restaurant management
            panelTopBar.BackColor = Color.FromArgb(52, 152, 219);

            // เพิ่ม title ลง top bar
            panelTopBar.Controls.Add(lblTitle);

            // Dock ด้านบนเต็มความกว้าง
            panelTopBar.Dock = DockStyle.Top;

            panelTopBar.Location = new Point(0, 0);
            panelTopBar.Name = "panelTopBar";
            panelTopBar.Size = new Size(750, 50);
            panelTopBar.TabIndex = 0;

            // =====================================================
            // TITLE LABEL
            // =====================================================

            // หัวข้อหลักของหน้า
            lblTitle.AutoSize = true;

            // ใช้ font ใหญ่และ bold เพื่อสร้าง hierarchy
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);

            // สีขาวเพื่อ contrast กับพื้นหลังสีฟ้า
            lblTitle.ForeColor = Color.White;

            lblTitle.Location = new Point(20, 10);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(300, 25);
            lblTitle.TabIndex = 0;

            // ใช้ emoji เพื่อให้ UI ดู modern/friendly มากขึ้น
            lblTitle.Text = "✏️  แก้ไขเมนูอาหาร";

            // =====================================================
            // RESTAURANT NAME ROW
            // =====================================================

            // Section สำหรับแก้ชื่อร้าน
            panelNameRow.BackColor = Color.FromArgb(240, 248, 255);

            panelNameRow.Controls.Add(labelRestaurantName);
            panelNameRow.Controls.Add(textRestaurantName);

            // วางใต้ top bar
            panelNameRow.Dock = DockStyle.Top;

            panelNameRow.Location = new Point(0, 50);
            panelNameRow.Name = "panelNameRow";
            panelNameRow.Size = new Size(750, 45);
            panelNameRow.TabIndex = 1;

            // =====================================================
            // RESTAURANT NAME LABEL
            // =====================================================

            // Label บอก field สำหรับชื่อร้าน
            labelRestaurantName.AutoSize = true;

            labelRestaurantName.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            // ใช้สีเข้มเพื่อให้อ่านง่ายบนพื้นอ่อน
            labelRestaurantName.ForeColor = Color.FromArgb(44, 62, 80);

            labelRestaurantName.Location = new Point(20, 12);
            labelRestaurantName.Name = "labelRestaurantName";
            labelRestaurantName.Size = new Size(100, 19);
            labelRestaurantName.TabIndex = 0;

            labelRestaurantName.Text = "🏪 ชื่อร้าน :";

            // =====================================================
            // RESTAURANT NAME TEXTBOX
            // =====================================================

            // TextBox สำหรับแก้ไขชื่อร้าน
            // ค่าเริ่มต้นจะถูกโหลดจาก API ใน RestaurantEditForm_Load()
            textRestaurantName.Font = new Font("Segoe UI", 10F);

            textRestaurantName.Location = new Point(140, 10);
            textRestaurantName.Name = "textRestaurantName";

            // กว้างพอสำหรับชื่อร้านยาว ๆ
            textRestaurantName.Size = new Size(580, 25);

            textRestaurantName.TabIndex = 1;

            // =====================================================
            // DATA GRID MENU
            // =====================================================

            // ตารางหลักสำหรับแก้ไขเมนูอาหาร
            //
            // Workflow:
            // - โหลด menu items จาก API
            // - bind ผ่าน DataTable
            // - user สามารถแก้ชื่อ/ราคาได้ตรง ๆ
            dataGridMenu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // พื้นหลังขาวเพื่อให้อ่านง่าย
            dataGridMenu.BackgroundColor = Color.White;

            dataGridMenu.BorderStyle = BorderStyle.None;

            // ใช้เส้นแบ่งเฉพาะแนวนอนเพื่อลด visual noise
            dataGridMenu.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            dataGridMenu.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            // =========================
            // HEADER STYLE
            // =========================

            // ปรับ style ของ header ให้ตรงกับ theme ของระบบ
            dataGridMenu.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,

                // ใช้ bold เพื่อแยก header ออกจากข้อมูล
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),

                // ป้องกันสี selection เปลี่ยนตอน focus
                SelectionBackColor = Color.FromArgb(52, 152, 219),

                // จัดข้อความ header ให้อยู่กลาง
                Alignment = DataGridViewContentAlignment.MiddleCenter
            };

            dataGridMenu.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            // =========================
            // CELL STYLE
            // =========================

            // style ของ cell ปกติ
            dataGridMenu.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,

                ForeColor = Color.FromArgb(44, 62, 80),

                Font = new Font("Segoe UI", 10F),

                // สี selection อ่อนเพื่อให้อ่านข้อความได้ต่อ
                SelectionBackColor = Color.FromArgb(200, 230, 255),

                SelectionForeColor = Color.FromArgb(44, 62, 80),

                // เพิ่ม padding เพื่อให้อ่านง่ายขึ้น
                Padding = new Padding(5)
            };

            // ปิด visual style ของ Windows default
            // เพื่อใช้สี custom ได้จริง
            dataGridMenu.EnableHeadersVisualStyles = false;

            // สีเส้น grid
            dataGridMenu.GridColor = Color.FromArgb(230, 230, 230);

            dataGridMenu.Location = new Point(20, 105);
            dataGridMenu.Name = "dataGridMenu";

            // ซ่อน row header ด้านซ้าย
            dataGridMenu.RowHeadersVisible = false;

            // เพิ่มความสูง row เพื่อให้อ่านง่าย
            dataGridMenu.RowTemplate.Height = 35;

            dataGridMenu.Size = new Size(710, 250);

            dataGridMenu.TabIndex = 2;

            // =====================================================
            // BUTTON PANEL
            // =====================================================

            // Panel ด้านล่างสำหรับ action buttons
            panelButtons.BackColor = Color.FromArgb(245, 248, 250);

            panelButtons.Controls.Add(buttonAdd);
            panelButtons.Controls.Add(buttonDelete);
            panelButtons.Controls.Add(buttonSave);

            // Dock ล่างสุดของ form
            panelButtons.Dock = DockStyle.Bottom;

            panelButtons.Location = new Point(0, 370);
            panelButtons.Name = "panelButtons";
            panelButtons.Size = new Size(750, 60);
            panelButtons.TabIndex = 3;

            // =====================================================
            // ADD BUTTON
            // =====================================================

            // ปุ่มเพิ่มเมนูใหม่ใน DataGrid
            buttonAdd.BackColor = Color.FromArgb(39, 174, 96);

            // เปลี่ยน cursor เป็น hand เพื่อบอกว่า clickable
            buttonAdd.Cursor = Cursors.Hand;

            buttonAdd.FlatAppearance.BorderSize = 0;
            buttonAdd.FlatStyle = FlatStyle.Flat;

            buttonAdd.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            buttonAdd.ForeColor = Color.White;

            buttonAdd.Location = new Point(20, 10);
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Size = new Size(160, 40);

            buttonAdd.TabIndex = 0;

            // ใช้สีเขียวเพื่อสื่อถึง action เพิ่มข้อมูล
            buttonAdd.Text = "➕ เพิ่มเมนู";

            buttonAdd.UseVisualStyleBackColor = false;

            // =====================================================
            // DELETE BUTTON
            // =====================================================

            // ปุ่มลบเมนูที่เลือก
            buttonDelete.BackColor = Color.FromArgb(231, 76, 60);

            buttonDelete.Cursor = Cursors.Hand;

            buttonDelete.FlatAppearance.BorderSize = 0;
            buttonDelete.FlatStyle = FlatStyle.Flat;

            buttonDelete.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            buttonDelete.ForeColor = Color.White;

            buttonDelete.Location = new Point(200, 10);
            buttonDelete.Name = "buttonDelete";
            buttonDelete.Size = new Size(160, 40);

            buttonDelete.TabIndex = 1;

            // ใช้สีแดงเพื่อสื่อถึง action ลบข้อมูล
            buttonDelete.Text = "🗑️ ลบเมนู";

            buttonDelete.UseVisualStyleBackColor = false;

            // =====================================================
            // SAVE BUTTON
            // =====================================================

            // ปุ่มบันทึกข้อมูลร้านและเมนูทั้งหมด
            buttonSave.BackColor = Color.FromArgb(52, 152, 219);

            buttonSave.Cursor = Cursors.Hand;

            buttonSave.FlatAppearance.BorderSize = 0;
            buttonSave.FlatStyle = FlatStyle.Flat;

            buttonSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            buttonSave.ForeColor = Color.White;

            // วางไว้ขวาสุดเพื่อแยกจาก add/delete
            buttonSave.Location = new Point(570, 10);

            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(160, 40);

            buttonSave.TabIndex = 2;

            // ใช้ icon floppy disk เพื่อสื่อถึง save action
            buttonSave.Text = "💾 บันทึก";

            buttonSave.UseVisualStyleBackColor = false;

            // =====================================================
            // FORM CONFIGURATION
            // =====================================================

            AutoScaleDimensions = new SizeF(7F, 15F);

            // scale UI ตาม DPI/font ของระบบ
            AutoScaleMode = AutoScaleMode.Font;

            // สีพื้นหลังหลักของฟอร์ม
            BackColor = Color.FromArgb(245, 248, 250);

            // ขนาดหน้าต่างหลัก
            ClientSize = new Size(750, 430);

            // เพิ่ม controls หลักเข้า form
            Controls.Add(dataGridMenu);
            Controls.Add(panelButtons);
            Controls.Add(panelNameRow);
            Controls.Add(panelTopBar);

            Name = "RestaurantEditForm";

            // เปิด form กลางหน้าจอ
            StartPosition = FormStartPosition.CenterScreen;

            // ชื่อบน title bar
            Text = "Delivery App — แก้ไขเมนู";

            // Resume layout หลังสร้าง UI เสร็จ
            panelTopBar.ResumeLayout(false);
            panelTopBar.PerformLayout();

            panelNameRow.ResumeLayout(false);
            panelNameRow.PerformLayout();

            ((System.ComponentModel.ISupportInitialize)dataGridMenu).EndInit();

            panelButtons.ResumeLayout(false);

            ResumeLayout(false);
        }

        #endregion

        // =========================
        // CONTROL DECLARATIONS
        // Controls เหล่านี้จะถูกใช้งานใน RestaurantEditForm.cs
        // =========================

        // แถบบนของฟอร์ม
        private Panel panelTopBar;

        // หัวข้อหลักของหน้า
        private Label lblTitle;

        // Section สำหรับชื่อร้าน
        private Panel panelNameRow;

        // Label "ชื่อร้าน"
        private Label labelRestaurantName;

        // TextBox สำหรับแก้ชื่อร้าน
        private TextBox textRestaurantName;

        // ตารางรายการเมนู
        private DataGridView dataGridMenu;

        // Panel สำหรับปุ่ม action
        private Panel panelButtons;

        // ปุ่มบันทึกข้อมูล
        private Button buttonSave;

        // ปุ่มเพิ่มเมนู
        private Button buttonAdd;

        // ปุ่มลบเมนู
        private Button buttonDelete;
    }
}