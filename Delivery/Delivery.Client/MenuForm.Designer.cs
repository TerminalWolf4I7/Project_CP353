using System.Drawing;
using System.Windows.Forms;

namespace Delivery.Client
{
    // Partial class ของ MenuForm ฝั่ง Designer
    // ไฟล์นี้รับผิดชอบการสร้างและจัดวาง UI controls เท่านั้น
    // ส่วน logic เช่น โหลดเมนู เพิ่มตะกร้า checkout จะอยู่ในไฟล์ MenuForm.cs อีกไฟล์
    partial class MenuForm
    {
        /// <summary>
        /// Container สำหรับเก็บ component/resource ที่ Designer สร้างขึ้น
        /// ใช้ร่วมกับ Dispose() เพื่อ cleanup resource ตอนปิด form
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Cleanup resource ที่ถูกใช้งานโดย form นี้
        /// </summary>
        /// <param name="disposing">
        /// true = dispose managed resources ได้ เช่น components;
        /// false = ถูกเรียกจาก finalizer path ไม่ควรแตะ managed resource
        /// </param>
        protected override void Dispose(bool disposing)
        {
            // ถ้า form กำลัง dispose แบบปกติ และมี components อยู่
            // ให้ dispose controls/resources ที่ Designer ผูกไว้ เพื่อลด memory leak
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            // เรียก Dispose ของ base Form ต่อ เพื่อให้ WinForms cleanup ส่วนของตัวเอง
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Method หลักที่ Designer ใช้สร้าง UI ของหน้า MenuForm
        /// ไม่ควรแก้ structure หลักด้วยมือถ้าใช้ Visual Studio Designer อยู่
        /// เพราะ Designer อาจ generate ทับได้
        /// </summary>
        private void InitializeComponent()
        {
            // =========================
            // สร้าง controls หลักของหน้า
            // =========================

            // Header ด้านบน ใช้แสดงชื่อร้านและปุ่มย้อนกลับ
            panelHeader = new Panel();

            // Label แสดงชื่อร้านอาหารที่ user เลือกเข้ามา
            lblRestaurantName = new Label();

            // ปุ่มกลับไปหน้าเลือกร้าน
            btnBack = new Button();

            // พื้นที่หลักสำหรับแสดงรายการเมนูแบบ card หลาย ๆ ใบ
            flpMenu = new FlowLayoutPanel();

            // Footer ด้านล่าง ใช้แสดงยอดรวมและปุ่ม checkout
            panelFooter = new Panel();

            // Label แสดงราคารวมของตะกร้าปัจจุบัน
            lblTotal = new Label();

            // ปุ่มยืนยันการสั่งซื้อ
            btnCheckout = new Button();

            // หยุด layout ชั่วคราวระหว่างตั้งค่า controls
            // ช่วยลดการ redraw ซ้ำ ๆ ตอนสร้าง UI
            panelHeader.SuspendLayout();
            panelFooter.SuspendLayout();
            SuspendLayout();

            // =====================================================
            // HEADER PANEL
            // แถบบนของหน้า ใช้เป็น navigation + context ของร้าน
            // =====================================================

            // ใช้สีเขียวเป็น brand color ของระบบ delivery
            panelHeader.BackColor = Color.FromArgb(46, 204, 113);

            // เพิ่มชื่อร้านและปุ่มกลับเข้าไปใน header
            panelHeader.Controls.Add(lblRestaurantName);
            panelHeader.Controls.Add(btnBack);

            // Dock ด้านบน เพื่อให้ header กินความกว้างเต็มหน้าต่าง
            panelHeader.Dock = DockStyle.Top;

            panelHeader.Location = new Point(0, 0);
            panelHeader.Margin = new Padding(2, 2, 2, 2);
            panelHeader.Name = "panelHeader";

            // ความสูง 60px ทำให้ header พอดีกับชื่อร้านและปุ่ม back
            panelHeader.Size = new Size(700, 60);
            panelHeader.TabIndex = 0;

            // =====================================================
            // RESTAURANT NAME LABEL
            // แสดงชื่อร้านที่ถูกส่งมาจาก CustomerForm/MenuForm constructor
            // =====================================================

            lblRestaurantName.AutoSize = true;

            // ใช้ font ใหญ่และ bold เพื่อบอกว่าเป็นหัวข้อหลักของหน้า
            lblRestaurantName.Font = new Font("Segoe UI", 22F, FontStyle.Bold);

            // สีขาวเพื่อ contrast กับพื้นหลังสีเขียว
            lblRestaurantName.ForeColor = Color.White;

            lblRestaurantName.Location = new Point(14, 12);
            lblRestaurantName.Margin = new Padding(2, 0, 2, 0);
            lblRestaurantName.Name = "lblRestaurantName";
            lblRestaurantName.Size = new Size(220, 41);
            lblRestaurantName.TabIndex = 0;

            // ค่า default ตอน Designer สร้าง
            // Runtime จะถูกแทนด้วย restaurantName ใน MenuForm_Load()
            lblRestaurantName.Text = "ชื่อร้านอาหาร...";

            // =====================================================
            // BACK BUTTON
            // ปุ่มกลับไปหน้าเลือกร้าน
            // Event Click ถูกผูกใน MenuForm.cs constructor
            // =====================================================

            btnBack.BackColor = Color.White;

            // ใช้ FlatStyle เพื่อให้ปุ่มดูเรียบกว่า default WinForms button
            btnBack.FlatStyle = FlatStyle.Flat;

            btnBack.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            // ใช้สีเดียวกับ header เพื่อคุม theme ให้สม่ำเสมอ
            btnBack.ForeColor = Color.FromArgb(46, 204, 113);

            // วางปุ่มไว้ด้านขวาของ header
            btnBack.Location = new Point(589, 18);
            btnBack.Margin = new Padding(2, 2, 2, 2);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(90, 35);
            btnBack.TabIndex = 1;

            // ใช้ลูกศรซ้ายเป็น visual cue ว่าปุ่มนี้คือย้อนกลับ
            btnBack.Text = "← กลับ";
            btnBack.UseVisualStyleBackColor = false;

            // =====================================================
            // MENU FLOW LAYOUT PANEL
            // พื้นที่กลางสำหรับแสดงการ์ดเมนูทั้งหมด
            // =====================================================

            // เปิด scroll เพราะจำนวนเมนูอาจมากกว่าพื้นที่หน้าจอ
            flpMenu.AutoScroll = true;

            // สีพื้นหลังอ่อน เพื่อแยกพื้นที่ content ออกจาก header/footer
            flpMenu.BackColor = Color.WhiteSmoke;

            // Fill พื้นที่ที่เหลือระหว่าง header และ footer
            flpMenu.Dock = DockStyle.Fill;

            // เริ่มหลัง header เพราะ header Dock Top สูง 60
            flpMenu.Location = new Point(0, 60);
            flpMenu.Margin = new Padding(2, 2, 2, 2);
            flpMenu.Name = "flpMenu";

            // Padding ด้านในช่วยให้ card เมนูไม่ชิดขอบเกินไป
            flpMenu.Padding = new Padding(14, 12, 14, 12);

            flpMenu.Size = new Size(700, 300);
            flpMenu.TabIndex = 1;

            // =====================================================
            // FOOTER PANEL
            // แถบล่างสำหรับแสดงยอดรวมและปุ่ม Checkout
            // =====================================================

            panelFooter.BackColor = Color.White;

            // เพิ่ม label ยอดรวมและปุ่ม checkout เข้า footer
            panelFooter.Controls.Add(lblTotal);
            panelFooter.Controls.Add(btnCheckout);

            // Dock ด้านล่าง เพื่อให้ footer อยู่ล่างสุดเสมอ
            panelFooter.Dock = DockStyle.Bottom;

            panelFooter.Location = new Point(0, 360);
            panelFooter.Margin = new Padding(2, 2, 2, 2);
            panelFooter.Name = "panelFooter";

            // ความสูง 72px เพียงพอสำหรับปุ่ม checkout และยอดรวม
            panelFooter.Size = new Size(700, 72);
            panelFooter.TabIndex = 2;

            // =====================================================
            // TOTAL LABEL
            // แสดงยอดรวมของตะกร้า
            // ถูกอัปเดตจาก UpdateTotalAsync() ใน MenuForm.cs
            // =====================================================

            lblTotal.AutoSize = true;

            // ใช้ font ใหญ่และ bold เพราะยอดรวมเป็นข้อมูลสำคัญก่อน checkout
            lblTotal.Font = new Font("Segoe UI", 16F, FontStyle.Bold);

            // สีเข้ม อ่านง่ายบนพื้นขาว
            lblTotal.ForeColor = Color.FromArgb(44, 62, 80);

            lblTotal.Location = new Point(21, 21);
            lblTotal.Margin = new Padding(2, 0, 2, 0);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(171, 30);
            lblTotal.TabIndex = 1;

            // ค่าเริ่มต้นก่อนโหลดตะกร้าจาก API
            lblTotal.Text = "ราคารวม: 0 บาท";

            // =====================================================
            // CHECKOUT BUTTON
            // ปุ่มยืนยันการสั่งซื้อ
            // Event Click ถูกผูกกับ BtnCheckout_Click ใน MenuForm.cs
            // =====================================================

            // ใช้สีเขียวเพื่อสื่อถึง action หลักของหน้านี้
            btnCheckout.BackColor = Color.FromArgb(46, 204, 113);

            btnCheckout.FlatStyle = FlatStyle.Flat;

            // ใช้ font ใหญ่และ bold เพื่อเน้นว่าเป็น primary action
            btnCheckout.Font = new Font("Segoe UI", 14F, FontStyle.Bold);

            btnCheckout.ForeColor = Color.White;

            // วางไว้ด้านขวาของ footer เพื่อแยกจากยอดรวมด้านซ้าย
            btnCheckout.Location = new Point(455, 21);
            btnCheckout.Margin = new Padding(2, 2, 2, 2);
            btnCheckout.Name = "btnCheckout";
            btnCheckout.Size = new Size(224, 36);
            btnCheckout.TabIndex = 0;
            btnCheckout.Text = "ยืนยันการสั่งซื้อ (Checkout)";
            btnCheckout.UseVisualStyleBackColor = false;

            // =====================================================
            // FORM CONFIGURATION
            // ตั้งค่าหน้าต่างหลักของ MenuForm
            // =====================================================

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;

            // ขนาดหน้าต่างหลักของหน้าเมนู
            ClientSize = new Size(700, 432);

            // ลำดับการ Add controls สำคัญกับ Dock layout
            // flpMenu = Fill, panelFooter = Bottom, panelHeader = Top
            Controls.Add(flpMenu);
            Controls.Add(panelFooter);
            Controls.Add(panelHeader);

            Margin = new Padding(2, 2, 2, 2);
            Name = "MenuForm";

            // Title bar ของหน้าต่าง
            Text = "Menu List";

            // Resume layout หลังตั้งค่า controls เสร็จ
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();

            panelFooter.ResumeLayout(false);
            panelFooter.PerformLayout();

            ResumeLayout(false);
        }

        #endregion

        // =========================
        // CONTROL DECLARATIONS
        // ประกาศ field ของ controls ที่ใช้ใน MenuForm
        // Logic ฝั่ง MenuForm.cs จะเรียกใช้งาน controls เหล่านี้
        // =========================

        // Header ด้านบนของหน้า
        private Panel panelHeader;

        // แสดงชื่อร้านอาหารปัจจุบัน
        private Label lblRestaurantName;

        // Container หลักสำหรับใส่ menu cards แบบ dynamic
        private FlowLayoutPanel flpMenu;

        // Footer ด้านล่างสำหรับยอดรวมและ checkout
        private Panel panelFooter;

        // แสดงราคารวมของตะกร้า
        private Label lblTotal;

        // ปุ่ม checkout เพื่อสร้าง order
        private Button btnCheckout;

        // ปุ่มกลับไปหน้า CustomerForm
        private Button btnBack;
    }
}