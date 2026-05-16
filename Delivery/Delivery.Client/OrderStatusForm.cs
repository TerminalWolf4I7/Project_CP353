using System;
using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Delivery.Client.Models;

namespace Delivery.Client
{
    public partial class OrderStatusForm : Form
    {
        // เก็บชื่อร้านสำหรับแสดงบน UI
        private string restaurantName;

        // เก็บเลข order ปัจจุบัน
        // ใช้สำหรับ poll status และลบ order ตอนลูกค้ากดยืนยันรับอาหาร
        private int orderId;

        // เก็บ userId ของลูกค้าที่ login อยู่
        // ใช้สำหรับกลับไปหน้า CustomerForm ของ user เดิม
        private int userId;

        // Label แสดงสถานะหลัก เช่น Pending, Cooking, Delivering
        private Label statusLabel;

        // Label แสดงข้อความอธิบายขั้นตอนภาษาไทย
        private Label detailLabel;

        // Progress bar แสดงความคืบหน้าของ order
        private ProgressBar progressBar;

        // Timer สำหรับ polling สถานะ order จาก API ทุก ๆ 3 วินาที
        private System.Windows.Forms.Timer statusTimer;

        // เก็บ index ของขั้นตอนปัจจุบันใน progress flow
        private int currentStep = 0;

        // ปุ่มยืนยันว่าลูกค้าได้รับอาหารแล้ว
        private Button receivedButton;

        // Mapping ของขั้นตอนการจัดส่งแบบภาษาไทย
        // index จะสัมพันธ์กับ progress bar และ status flow
        private string[] steps =
        {
            "ร้านรับออเดอร์แล้ว",
            "ร้านกำลังทำอาหาร",
            "กำลังหา Rider",
            "Rider กำลังไปส่ง",
            "จัดส่งสำเร็จ"
        };

        // Constructor ของหน้า OrderStatusForm
        //
        // Workflow:
        // 1. รับข้อมูลร้าน, user และ order จากหน้าก่อนหน้า
        // 2. สร้าง UI ทั้งหมดผ่าน SetupUI()
        // 3. เริ่ม polling สถานะ order จาก server
        //
        // Input:
        // - restaurant: ชื่อร้านอาหาร
        // - userId: ลูกค้าที่เป็นเจ้าของ order
        // - orderId: เลข order ที่ต้องติดตามสถานะ
        public OrderStatusForm(string restaurant, int userId, int orderId)
        {
            InitializeComponent();

            // เก็บข้อมูล context ของ order ปัจจุบันไว้ใช้ทั้ง form
            this.restaurantName = restaurant;
            this.userId = userId;
            this.orderId = orderId;

            // สร้าง controls ของหน้า status แบบ dynamic ด้วย code
            SetupUI();

            // เริ่ม polling API เพื่อติดตามสถานะ order แบบ realtime
            StartStatusPolling();
        }

        // สร้าง UI ของหน้า Order Status ด้วย code-behind
        //
        // Workflow:
        // 1. ตั้งค่าหน้าต่างหลัก
        // 2. สร้าง panel หลัก
        // 3. สร้าง section แสดงข้อมูลร้านและ order
        // 4. สร้าง progress section สำหรับสถานะ order
        // 5. สร้างปุ่มกลับหน้า Customer และปุ่มยืนยันรับอาหาร
        //
        // Side effects:
        // - เพิ่ม controls จำนวนมากเข้า Form
        // - กำหนดค่าให้ field controls เช่น statusLabel, progressBar
        private void SetupUI()
        {
            // =========================
            // FORM CONFIGURATION
            // =========================

            this.Text = "Order Status";

            // กำหนดขนาดหน้าต่างหลัก
            this.Size = new Size(900, 650);

            // เปิดหน้าต่างตรงกลางหน้าจอ
            this.StartPosition = FormStartPosition.CenterScreen;

            // ใช้พื้นหลังสีขาวเพื่อให้ content อ่านง่าย
            this.BackColor = Color.White;

            // =========================
            // MAIN PANEL
            // =========================

            // Panel หลักของหน้า ใช้เป็น container ของทุก section
            Panel mainPanel = new Panel();

            // ใช้สีน้ำเงินเพื่อแยกหน้า status ออกจากหน้า menu/customer
            mainPanel.BackColor = Color.FromArgb(42, 107, 190);

            mainPanel.Size = new Size(850, 580);
            mainPanel.Location = new Point(20, 20);

            // เพิ่ม main panel เข้า form
            this.Controls.Add(mainPanel);

            // =========================
            // TITLE LABEL
            // =========================

            // หัวข้อหลักของหน้า
            Label title = new Label();

            title.Text = "สถานะคำสั่งซื้อ";

            // ใช้ font ใหญ่เพื่อสร้าง hierarchy ของ UI
            title.Font = new Font("Segoe UI", 24, FontStyle.Bold);

            title.ForeColor = Color.White;

            // จัดข้อความให้อยู่ตรงกลาง
            title.TextAlign = ContentAlignment.MiddleCenter;

            title.Size = new Size(mainPanel.Width, 60);
            title.Location = new Point(0, 20);

            mainPanel.Controls.Add(title);

            // =========================
            // INFO CARD
            // แสดงข้อมูลร้านและ order id
            // =========================

            Panel infoCard = new Panel();

            infoCard.BackColor = Color.White;
            infoCard.Size = new Size(680, 120);

            // จัด card ให้อยู่กึ่งกลางของ panel หลัก
            infoCard.Location = new Point(85, 95);

            mainPanel.Controls.Add(infoCard);

            // =========================
            // RESTAURANT LABEL
            // =========================

            // แสดงชื่อร้านอาหารของ order นี้
            Label restaurantLabel = new Label();

            restaurantLabel.Text = restaurantName;

            restaurantLabel.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            restaurantLabel.ForeColor = Color.Black;

            restaurantLabel.Size = new Size(620, 45);
            restaurantLabel.Location = new Point(30, 20);

            infoCard.Controls.Add(restaurantLabel);

            // =========================
            // ORDER ID LABEL
            // =========================

            // แสดงเลข order เพื่อให้ user อ้างอิงได้
            Label orderLabel = new Label();

            orderLabel.Text = "Order ID: #" + orderId;

            orderLabel.Font = new Font("Segoe UI", 13);

            // ใช้สีเทาเพื่อลด visual priority รองจากชื่อร้าน
            orderLabel.ForeColor = Color.DimGray;

            orderLabel.Size = new Size(620, 35);
            orderLabel.Location = new Point(33, 70);

            infoCard.Controls.Add(orderLabel);

            // =========================
            // STATUS LABEL
            // =========================

            // Label หลักสำหรับแสดงสถานะปัจจุบันจาก API
            statusLabel = new Label();

            statusLabel.Text = "Status: กำลังดำเนินการ";

            statusLabel.Font = new Font("Segoe UI", 19, FontStyle.Bold);

            statusLabel.ForeColor = Color.White;
            statusLabel.TextAlign = ContentAlignment.MiddleCenter;

            statusLabel.Size = new Size(mainPanel.Width, 45);
            statusLabel.Location = new Point(0, 245);

            mainPanel.Controls.Add(statusLabel);

            // =========================
            // DETAIL LABEL
            // =========================

            // Label อธิบาย flow แบบภาษาไทย
            // ค่าเริ่มต้นคือขั้นตอนแรกของ order
            detailLabel = new Label();

            detailLabel.Text = steps[0];

            detailLabel.Font = new Font("Segoe UI", 17, FontStyle.Bold);

            detailLabel.ForeColor = Color.White;
            detailLabel.TextAlign = ContentAlignment.MiddleCenter;

            detailLabel.Size = new Size(mainPanel.Width, 40);
            detailLabel.Location = new Point(0, 300);

            mainPanel.Controls.Add(detailLabel);

            // =========================
            // PROGRESS BAR
            // =========================

            // แสดงความคืบหน้าของ order ตาม step index
            progressBar = new ProgressBar();

            progressBar.Size = new Size(650, 30);
            progressBar.Location = new Point(100, 355);

            // เริ่มตั้งแต่ step 0 และจบที่ step 4
            progressBar.Minimum = 0;
            progressBar.Maximum = 4;

            // ค่าเริ่มต้นก่อน polling status
            progressBar.Value = 0;

            mainPanel.Controls.Add(progressBar);

            // =========================
            // FLOW LABEL
            // =========================

            // แสดง flow ทั้งหมดของ order เพื่อให้ user เข้าใจลำดับขั้น
            Label flowLabel = new Label();

            flowLabel.Text = "รับออเดอร์  >  กำลังทำ  >  หา Rider  >  กำลังไปส่ง  >  เสร็จสิ้น";

            flowLabel.Font = new Font("Segoe UI", 11, FontStyle.Bold);

            flowLabel.ForeColor = Color.White;
            flowLabel.TextAlign = ContentAlignment.MiddleCenter;

            flowLabel.Size = new Size(mainPanel.Width, 35);
            flowLabel.Location = new Point(0, 405);

            mainPanel.Controls.Add(flowLabel);

            // =========================
            // CUSTOMER BUTTON
            // =========================

            // ปุ่มกลับหน้า customer โดยไม่ยุ่งกับ order
            Button customerButton = new Button();

            customerButton.Text = "กลับหน้า Customer";

            customerButton.Size = new Size(220, 50);
            customerButton.Location = new Point(150, 485);

            customerButton.BackColor = Color.White;

            customerButton.Font = new Font("Segoe UI", 13, FontStyle.Bold);

            customerButton.FlatStyle = FlatStyle.Flat;
            customerButton.FlatAppearance.BorderSize = 0;

            // ผูก event สำหรับกลับหน้า customer
            customerButton.Click += CustomerButton_Click;

            mainPanel.Controls.Add(customerButton);

            // =========================
            // RECEIVED BUTTON
            // =========================

            // ปุ่มสำหรับยืนยันว่าลูกค้าได้รับอาหารแล้ว
            // จะถูกแสดงเมื่อ order ถึงขั้นตอนสุดท้ายเท่านั้น
            receivedButton = new Button();

            receivedButton.Text = "✅ ได้รับอาหารแล้ว";

            receivedButton.Size = new Size(250, 50);
            receivedButton.Location = new Point(470, 485);

            // ใช้สีเขียวเพื่อสื่อถึง action สำเร็จ
            receivedButton.BackColor = Color.FromArgb(46, 204, 113);

            receivedButton.ForeColor = Color.White;

            receivedButton.Font = new Font("Segoe UI", 13, FontStyle.Bold);

            receivedButton.FlatStyle = FlatStyle.Flat;
            receivedButton.FlatAppearance.BorderSize = 0;

            // เปลี่ยน cursor เพื่อบอกว่า clickable
            receivedButton.Cursor = Cursors.Hand;

            // ซ่อนปุ่มไว้ก่อนจนกว่า order จะเสร็จ
            receivedButton.Visible = false;

            // ผูก event สำหรับ confirm รับอาหาร
            receivedButton.Click += ReceivedButton_Click;

            mainPanel.Controls.Add(receivedButton);
        }

        // เริ่มระบบ polling สถานะ order
        //
        // Workflow:
        // 1. สร้าง Timer
        // 2. ตั้ง interval เป็น 3 วินาที
        // 3. ผูก event Tick
        // 4. เริ่ม Timer
        // 5. โหลด status ครั้งแรกทันที
        //
        // Side effects:
        // - เริ่มเรียก API ทุก 3 วินาที
        private void StartStatusPolling()
        {
            statusTimer = new System.Windows.Forms.Timer();

            // poll ทุก 3 วินาที
            statusTimer.Interval = 3000;

            // ทุกครั้งที่ timer tick จะ refresh status ใหม่
            statusTimer.Tick += StatusTimer_Tick;

            statusTimer.Start();

            // โหลด status ครั้งแรกทันทีโดยไม่ต้องรอ timer รอบแรก
            _ = RefreshStatusAsync();
        }

        // Event handler ของ Timer
        //
        // หน้าที่เดียวคือ trigger refresh status ใหม่
        private void StatusTimer_Tick(object sender, EventArgs e)
        {
            _ = RefreshStatusAsync();
        }

        // โหลดสถานะ order ล่าสุดจาก API แล้วอัปเดต UI
        //
        // Workflow:
        // 1. เรียก API order details
        // 2. ถ้า order ถูกลบแล้ว (404) ให้ถือว่าเสร็จสมบูรณ์
        // 3. อ่านข้อมูล status จาก response
        // 4. อัปเดต progress และข้อความบน UI
        //
        // Side effects:
        // - เปลี่ยนข้อความและ progress บนหน้าจอ
        // - อาจหยุด timer ถ้า order เสร็จแล้ว
        private async Task RefreshStatusAsync()
        {
            try
            {
                // โหลดข้อมูล order ล่าสุดจาก server
                var response = await RestUtil.GetResponseAsync($"orders/{orderId}/details");

                // ถ้า API คืน 404 แปลว่า order ถูกลบออกจากระบบแล้ว
                // ใช้เป็นสัญญาณว่า order เสร็จสิ้น
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    ShowCompletedStatus();
                    return;
                }

                // ถ้า status code เป็น error อื่น จะ throw exception
                response.EnsureSuccessStatusCode();

                // แปลง response body เป็น DTO
                var details = await RestUtil.ReadAsAsync<OrderDetailDto>(response);

                // ป้องกัน null reference ถ้า deserialize ไม่สำเร็จ
                if (details == null)
                {
                    return;
                }

                // อัปเดต progress และข้อความตาม status ล่าสุด
                UpdateStatusDisplay(details.Status);
            }
            catch
            {
                // ตั้งใจ ignore error ระหว่าง polling
                // เพื่อให้ timer รอบถัดไปลองใหม่อัตโนมัติ
            }
        }

        // แปลง status string จาก API ไปเป็น UI state
        //
        // Workflow:
        // 1. map status → step index
        // 2. update label และ progress bar
        // 3. ถ้า order เสร็จแล้ว ให้หยุด polling
        // 4. แสดงปุ่มยืนยันรับอาหาร
        //
        // Input:
        // - status: string สถานะจาก API
        //
        // Side effects:
        // - เปลี่ยน progress UI
        // - หยุด timer
        // - แสดง receivedButton
        private void UpdateStatusDisplay(string status)
        {
            // แสดง raw status จาก API
            statusLabel.Text = $"Status: {status}";

            // Mapping status จาก backend → step ของ UI
            int stepIndex = status switch
            {
                "Pending" => 0,
                "Cooking" => 1,
                "Waiting for rider" => 2,
                "Delivering" => 3,
                "Completed" => 4,
                "Delivered" => 4,
                "Success" => 4,

                // fallback ถ้า backend ส่ง status ที่ UI ไม่รู้จัก
                _ => -1
            };

            // ถ้า status ถูก map ได้สำเร็จ
            if (stepIndex >= 0)
            {
                currentStep = stepIndex;

                // แสดงข้อความภาษาไทยของขั้นตอนนั้น
                detailLabel.Text = steps[stepIndex];

                // อัปเดต progress bar
                progressBar.Value = stepIndex;

                // ถ้าอยู่ขั้นตอนสุดท้าย
                if (stepIndex == steps.Length - 1)
                {
                    // หยุด polling เพราะ order จบแล้ว
                    statusTimer.Stop();

                    // เปิดให้ลูกค้ายืนยันรับอาหาร
                    receivedButton.Visible = true;
                }
            }
            else
            {
                // fallback กรณี status ไม่อยู่ใน mapping
                detailLabel.Text = status;
            }
        }

        // แสดงสถานะ completed แบบ force
        //
        // ใช้ในกรณี API คืน 404
        // ซึ่งตีความว่า order ถูกลบหรือจบ flow ไปแล้ว
        //
        // Side effects:
        // - เปลี่ยน progress ไปขั้นสุดท้าย
        // - หยุด timer
        // - แสดง receivedButton
        private void ShowCompletedStatus()
        {
            // ขยับ progress ไปขั้นตอนสุดท้าย
            currentStep = steps.Length - 1;

            statusLabel.Text = "Status: เสร็จสิ้น";

            // แสดงข้อความสุดท้ายของ flow
            detailLabel.Text = steps[currentStep];

            // progress เต็ม
            progressBar.Value = currentStep;

            // ไม่ต้อง poll ต่อแล้ว
            statusTimer.Stop();

            // เปิดปุ่ม confirm รับอาหาร
            receivedButton.Visible = true;
        }

        // Event handler เมื่อลูกค้ายืนยันว่าได้รับอาหารแล้ว
        //
        // Workflow:
        // 1. DELETE order จากระบบ
        // 2. แจ้งขอบคุณผู้ใช้
        // 3. เปิดหน้า CustomerForm
        // 4. ปิด OrderStatusForm
        //
        // Side effects:
        // - ลบ order ฝั่ง server
        // - ปิด form ปัจจุบัน
        private async void ReceivedButton_Click(object? sender, EventArgs e)
        {
            try
            {
                // ลบ order ออกจากระบบหลังลูกค้ายืนยันรับของ
                var response = await RestUtil.DeleteAsync($"orders/{orderId}");

                response.EnsureSuccessStatusCode();

                // แจ้งผู้ใช้ว่ากระบวนการเสร็จสมบูรณ์แล้ว
                MessageBox.Show("ขอบคุณที่ใช้บริการครับ! 🙏");

                // กลับไปหน้า customer ของ user เดิม
                CustomerForm customerForm = new CustomerForm(userId);

                customerForm.Show();

                // ปิดหน้าสถานะ order
                this.Close();
            }
            catch (Exception ex)
            {
                // แจ้ง error ถ้าลบ order ไม่สำเร็จ
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // กลับหน้า Customer โดยไม่ยุ่งกับ order ปัจจุบัน
        //
        // ใช้กรณี user แค่อยากออกจากหน้าติดตามสถานะชั่วคราว
        private void CustomerButton_Click(object sender, EventArgs e)
        {
            CustomerForm customerForm = new CustomerForm(userId);

            customerForm.Show();

            this.Close();
        }

        // Lifecycle hook ตอน form ถูกปิด
        //
        // Workflow:
        // 1. หยุด timer polling
        // 2. ป้องกัน timer ยิง API ต่อหลัง form ถูกปิด
        // 3. เรียก base implementation ต่อ
        //
        // Side effects:
        // - หยุด polling API
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // ป้องกัน memory leak และ background polling ค้าง
            statusTimer?.Stop();

            base.OnFormClosed(e);
        }
    }
}