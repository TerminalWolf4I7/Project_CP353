namespace Delivery.Client
{
    // Partial class ของ OrderStatusForm ฝั่ง Designer
    // ไฟล์นี้ใช้สำหรับกำหนดค่าเริ่มต้นของ Form และ resource management
    // ส่วน UI จริงและ business logic อยู่ในไฟล์ OrderStatusForm.cs
    partial class OrderStatusForm
    {
        /// <summary>
        /// Container สำหรับเก็บ component/resource ที่ถูกสร้างโดย WinForms Designer
        /// ใช้ร่วมกับ Dispose() เพื่อ cleanup memory/resource ตอนปิด form
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Cleanup resource ที่ form ใช้งานอยู่
        /// </summary>
        /// <param name="disposing">
        /// true = สามารถ dispose managed resources ได้
        /// false = ถูกเรียกจาก finalizer path ไม่ควรแตะ managed objects
        /// </param>
        protected override void Dispose(bool disposing)
        {
            // ถ้ากำลัง dispose แบบปกติ และมี component ค้างอยู่
            // ให้ cleanup resource ทั้งหมดเพื่อลด memory leak
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            // เรียก Dispose ของ base Form ต่อ
            // เพื่อให้ WinForms cleanup resource ภายในของตัวเอง
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Method เริ่มต้นที่ WinForms Designer ใช้สร้าง Form
        ///
        /// หมายเหตุ:
        /// ในโปรเจกต์นี้ UI หลักถูกสร้างด้วย code ใน SetupUI()
        /// ดังนั้น InitializeComponent() จึงมีแค่ configuration ขั้นพื้นฐานของ Form
        /// </summary>
        private void InitializeComponent()
        {
            // Container หลักสำหรับเก็บ component ของ form นี้
            this.components = new System.ComponentModel.Container();

            // เปิด AutoScale เพื่อให้ UI scale ตาม DPI / font ของระบบ
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            // กำหนดขนาดเริ่มต้นของหน้าต่าง
            // ภายหลังจะถูก override ใน SetupUI()
            this.ClientSize = new System.Drawing.Size(800, 450);

            // ชื่อเริ่มต้นของหน้าต่างบน title bar
            // จะถูกเปลี่ยนอีกครั้งใน SetupUI()
            this.Text = "OrderStatusForm";
        }

        #endregion
    }
}