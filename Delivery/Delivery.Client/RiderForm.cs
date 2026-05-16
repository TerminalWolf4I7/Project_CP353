using System;
using System.Data;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Delivery.Client.Models;

namespace Delivery.Client
{
    public partial class RiderForm : Form
    {
        // userId ของ Rider ที่ login อยู่
        // ใช้ผูก rider กับออเดอร์ที่รับ
        private readonly int _userId;

        // Timer สำหรับ refresh ข้อมูลอัตโนมัติ
        // ใช้ reload รายการออเดอร์ทุก 30 วินาที
        private System.Windows.Forms.Timer refreshTimer;

        // Constructor ของหน้า RiderForm
        //
        // Workflow:
        // 1. รับ userId ของ Rider
        // 2. สร้าง UI ผ่าน InitializeComponent()
        // 3. ผูก event ต่าง ๆ ของฟอร์ม
        // 4. ตั้ง Timer refresh ข้อมูลอัตโนมัติ
        //
        // Input:
        // - userId: Rider ที่ login เข้ามา
        //
        // Side effects:
        // - เริ่ม timer refresh ทุก 30 วินาที
        // - ผูก event ของปุ่มและ DataGrid
        public RiderForm(int userId)
        {
            InitializeComponent();

            // เก็บ rider ปัจจุบันไว้ใช้ทั้งฟอร์ม
            _userId = userId;

            // โหลดข้อมูลครั้งแรกเมื่อเปิดฟอร์ม
            Load += RiderForm_Load;

            // เมื่อเปลี่ยนแถวที่เลือก ให้ update ปุ่มต่าง ๆ
            dataGridOrders.SelectionChanged +=
                DataGridOrders_SelectionChanged;

            // ปุ่มดูรายละเอียดออเดอร์
            buttonViewDetails.Click +=
                ButtonViewDetails_Click;

            // ปุ่มรับงาน
            buttonAcceptOrder.Click +=
                ButtonAcceptOrder_Click;

            // ปุ่มเปิดหน้างานปัจจุบันของ Rider
            buttonRiderOrder.Click +=
                ButtonRiderOrder_Click;

            // ปุ่ม logout
            buttonLogout.Click +=
                BtnLogout_Click;

            // ปิดปุ่มไว้ก่อนจนกว่าจะมีข้อมูลหรือมีการเลือกแถว
            buttonViewDetails.Enabled = false;
            buttonAcceptOrder.Enabled = false;
            buttonRiderOrder.Enabled = false;

            // =========================
            // AUTO REFRESH TIMER
            // =========================

            refreshTimer = new System.Windows.Forms.Timer();

            // refresh ทุก 30 วินาที
            refreshTimer.Interval = 30000;

            // ทุกครั้งที่ timer tick จะ reload ข้อมูลใหม่
            refreshTimer.Tick += RefreshTimer_Tick;

            refreshTimer.Start();
        }

        // Event handler เมื่อเปิดฟอร์ม Rider ครั้งแรก
        //
        // Workflow:
        // 1. โหลดรายการออเดอร์ที่รอ Rider
        // 2. เช็คว่า Rider คนนี้มีงานค้างอยู่หรือไม่
        //
        // Side effects:
        // - โหลดข้อมูลเข้า DataGrid
        // - เปิด/ปิดปุ่ม "งานของฉัน"
        private async void RiderForm_Load(
            object sender,
            EventArgs e)
        {
            await LoadOrdersAsync();

            await CheckCurrentOrderAsync();
        }

        // Event handler เมื่อมีการเลือกแถวใน DataGrid
        //
        // Workflow:
        // - ถ้ามี order ถูกเลือก → เปิดปุ่มดูรายละเอียด/รับงาน
        // - ถ้าไม่มี → ปิดปุ่ม
        //
        // Side effects:
        // - เปลี่ยน Enabled state ของปุ่ม
        private void DataGridOrders_SelectionChanged(
            object sender,
            EventArgs e)
        {
            // เช็คว่ามี row ถูกเลือกหรือไม่
            bool hasSelection =
                dataGridOrders.SelectedRows.Count > 0;

            // เปิด/ปิดปุ่มดูรายละเอียด
            buttonViewDetails.Enabled =
                hasSelection;

            // เปิด/ปิดปุ่มรับงาน
            buttonAcceptOrder.Enabled =
                hasSelection;
        }

        // โหลดรายการออเดอร์ที่กำลังรอ Rider
        //
        // Workflow:
        // 1. เรียก API หา order status = Waiting for rider
        // 2. สร้าง DataTable
        // 3. เติมข้อมูล order ลง table
        // 4. bind เข้า DataGridView
        //
        // Side effects:
        // - เปลี่ยน datasource ของ dataGridOrders
        private async Task LoadOrdersAsync()
        {
            try
            {
                // โหลดเฉพาะ order ที่ยังไม่มี Rider รับ
                var orders = await RestUtil.GetAsync<List<OrderDto>>(
                    "orders?status=Waiting%20for%20rider");

                // สร้าง table สำหรับ bind กับ DataGrid
                DataTable dt = new DataTable();

                dt.Columns.Add("order_id", typeof(int));
                dt.Columns.Add("restaurant_id", typeof(int));
                dt.Columns.Add("status", typeof(string));
                dt.Columns.Add("rider_id", typeof(int));

                // ถ้ามี order ให้เติมข้อมูลลง table
                if (orders != null)
                {
                    foreach (var order in orders.OrderBy(o => o.OrderId))
                    {
                        dt.Rows.Add(
                            order.OrderId,
                            order.RestaurantId,
                            order.Status,

                            // ถ้ายังไม่มี rider ให้ใช้ 0 แทน null
                            order.RiderId ?? 0);
                    }
                }

                // bind table เข้า DataGridView
                dataGridOrders.DataSource = dt;
            }
            catch (Exception ex)
            {
                // แจ้ง error ตอนโหลด order
                MessageBox.Show(
                    ex.Message);
            }
        }

        // เช็คว่า Rider มีงานที่รับไว้แล้วหรือไม่
        //
        // Workflow:
        // 1. เรียก API current-order
        // 2. ถ้ามี order ปัจจุบัน → เปิดปุ่ม "งานของฉัน"
        //
        // Side effects:
        // - เปลี่ยน Enabled state ของ buttonRiderOrder
        private async Task CheckCurrentOrderAsync()
        {
            // API นี้จะคืน success ถ้า rider มี order ปัจจุบัน
            var response = await RestUtil.GetResponseAsync(
                $"riders/{_userId}/current-order");

            // เปิดปุ่มเฉพาะกรณีมีงานค้าง
            buttonRiderOrder.Enabled =
                response.IsSuccessStatusCode;
        }

        // แสดงรายละเอียด order และรายการอาหารทั้งหมด
        //
        // Workflow:
        // 1. อ่าน orderId จากแถวที่เลือก
        // 2. โหลดข้อมูล order detail
        // 3. โหลดรายการอาหารใน order
        // 4. สร้างข้อความสรุป
        // 5. แสดงใน MessageBox
        //
        // Side effects:
        // - เรียก API หลาย endpoint
        // - แสดง popup รายละเอียด order
        private async void ButtonViewDetails_Click(
            object sender,
            EventArgs e)
        {
            // อ่าน orderId จาก row ที่เลือก
            int orderId =
                Convert.ToInt32(
                    dataGridOrders.SelectedRows[0]
                    .Cells["order_id"]
                    .Value);

            try
            {
                // โหลดรายละเอียดหลักของ order
                var details = await RestUtil.GetAsync<OrderDetailDto>(
                    $"orders/{orderId}/details");

                // ถ้าไม่พบ order
                if (details == null)
                {
                    MessageBox.Show("Order not found.");
                    return;
                }

                // โหลดรายการอาหารใน order
                var items = await RestUtil.GetAsync<List<OrderItemDto>>(
                    $"orders/{orderId}");

                // ใช้ StringBuilder เพื่อลด overhead ตอนต่อ string หลายครั้ง
                StringBuilder sb = new StringBuilder();

                // =========================
                // ORDER SUMMARY
                // =========================

                sb.AppendLine($"Order ID : {details.OrderId}");
                sb.AppendLine($"Customer : {details.CustomerName}");
                sb.AppendLine($"Restaurant : {details.RestaurantId}");
                sb.AppendLine($"Status : {details.Status}");
                sb.AppendLine($"Total : {details.TotalPrice}");

                sb.AppendLine();
                sb.AppendLine("Items:");

                // =========================
                // ORDER ITEMS
                // =========================

                if (items != null)
                {
                    foreach (var item in items)
                    {
                        sb.AppendLine(
                            $"{item.Name} x {item.Quantity} ({item.Price})");
                    }
                }

                // แสดงรายละเอียดทั้งหมดใน popup
                MessageBox.Show(
                    sb.ToString(),
                    $"Order {orderId}");
            }
            catch (Exception ex)
            {
                // แจ้ง error ตอนโหลดรายละเอียด order
                MessageBox.Show(
                    ex.Message);
            }
        }

        // Rider รับออเดอร์
        //
        // Workflow:
        // 1. อ่าน orderId จาก row ที่เลือก
        // 2. PATCH ไปยัง API accept-rider
        // 3. ถ้าสำเร็จ → ผูก Rider กับ order
        // 4. reload รายการ order ใหม่
        // 5. refresh สถานะ current order
        //
        // หมายเหตุ:
        // ถ้ามี Rider คนอื่นรับไปก่อน API จะคืน error
        //
        // Side effects:
        // - เปลี่ยน owner ของ order ฝั่ง server
        // - reload DataGrid
        private async void ButtonAcceptOrder_Click(
            object sender,
            EventArgs e)
        {
            // อ่าน orderId จาก row ที่เลือก
            int orderId =
                Convert.ToInt32(
                    dataGridOrders.SelectedRows[0]
                    .Cells["order_id"]
                    .Value);

            try
            {
                // Rider รับ order นี้
                var response = await RestUtil.PatchAsync(
                    $"orders/{orderId}/accept-rider/{_userId}");

                // ถ้ารับสำเร็จ
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Order accepted.");
                }
                else
                {
                    // กรณี race condition:
                    // Rider คนอื่นรับไปก่อนแล้ว
                    MessageBox.Show("Order นี้ถูกรับไปแล้ว");
                }

                // reload รายการ order ใหม่
                await LoadOrdersAsync();

                // refresh สถานะ current order ของ rider
                await CheckCurrentOrderAsync();
            }
            catch (Exception ex)
            {
                // แจ้ง error ตอนรับ order
                MessageBox.Show(
                    ex.Message);
            }
        }

        // Logout ออกจากระบบ
        //
        // Workflow:
        // 1. แสดง confirmation dialog
        // 2. ถ้าผู้ใช้กดยืนยัน
        //    - หยุด timer
        //    - เปิด Login form
        //    - ปิด RiderForm
        //
        // Side effects:
        // - หยุด auto refresh
        // - เปิด Login form
        // - ปิด RiderForm
        private void BtnLogout_Click(
            object sender,
            EventArgs e)
        {
            // popup ยืนยันก่อน logout
            var result =
                MessageBox.Show(
                    "คุณต้องการออกจากระบบใช่หรือไม่?",
                    "ยืนยันการออกจากระบบ",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

            // ถ้าผู้ใช้กด Yes
            if (result == DialogResult.Yes)
            {
                // หยุด timer เพื่อไม่ให้ background refresh ค้าง
                refreshTimer.Stop();

                // กลับไปหน้า Login
                Login loginForm =
                    new Login();

                loginForm.Show();

                // ปิด RiderForm ปัจจุบัน
                Close();
            }
        }

        // เปิดหน้า RiderOrderForm
        //
        // ใช้สำหรับดูงานที่ Rider รับไว้แล้ว
        //
        // Side effects:
        // - เปิด RiderOrderForm
        private void ButtonRiderOrder_Click(
            object sender,
            EventArgs e)
        {
            // เปิดหน้าจัดการ order ปัจจุบันของ Rider
            RiderOrderForm form =
                new RiderOrderForm(
                    _userId);

            form.Show();
        }

        // Event handler ของ Timer
        //
        // Workflow:
        // - refresh รายการ order รอรับ
        // - refresh current order ของ Rider
        //
        // Side effects:
        // - เรียก API ทุก 30 วินาที
        private void RefreshTimer_Tick(
            object sender,
            EventArgs e)
        {
            _ = LoadOrdersAsync();

            _ = CheckCurrentOrderAsync();
        }

        // Lifecycle hook ตอนปิดฟอร์ม
        //
        // Workflow:
        // 1. หยุด timer refresh
        // 2. ป้องกัน timer ยิง API ต่อหลัง form ถูกปิด
        // 3. เรียก base implementation ต่อ
        //
        // Side effects:
        // - หยุด background polling
        protected override void OnFormClosed(
            FormClosedEventArgs e)
        {
            // ป้องกัน memory leak และ timer ค้าง
            refreshTimer.Stop();

            base.OnFormClosed(e);
        }
    }
}
