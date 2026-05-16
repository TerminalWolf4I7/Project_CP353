using System;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Delivery.Client.Models;

namespace Delivery.Client
{
    public partial class RestaurantOrdersForm : Form
    {
        // userId ของเจ้าของร้านที่ login อยู่
        // ใช้หา restaurantId และโหลดออเดอร์ของร้านนี้
        private readonly int userId;

        // restaurantId จริงของร้าน
        // จะถูกโหลดจาก API ตอนเปิดฟอร์ม
        private int restaurantId;

        // DataTable สำหรับ bind ข้อมูลเข้า DataGridView
        // ใช้เก็บรายการออเดอร์บน UI
        private DataTable ordersTable = new DataTable();

        // Constructor ของหน้า RestaurantOrdersForm
        //
        // Workflow:
        // 1. รับ userId ของร้าน
        // 2. สร้าง UI controls ผ่าน InitializeComponent()
        // 3. ผูก event ต่าง ๆ ของฟอร์ม
        //
        // Input:
        // - userId: เจ้าของร้านที่ login เข้ามา
        //
        // Side effects:
        // - ผูก event โหลดออเดอร์
        // - ผูก event ปุ่มรับ/ปฏิเสธ/ทำอาหารเสร็จ
        public RestaurantOrdersForm(int userId)
        {
            InitializeComponent();

            // เก็บ userId ของร้านปัจจุบันไว้ใช้ทั้งฟอร์ม
            this.userId = userId;

            // โหลดข้อมูลร้านและออเดอร์เมื่อเปิดฟอร์ม
            Load += RestaurantOrdersForm_Load;

            // เปลี่ยนปุ่ม action ตาม order ที่เลือก
            dataGridOrders.SelectionChanged += DataGridOrders_SelectionChanged;

            // ปุ่มรับออเดอร์
            buttonAccept.Click += ButtonAccept_Click;

            // ปุ่มปฏิเสธออเดอร์
            buttonDecline.Click += ButtonDecline_Click;

            // ปุ่มแจ้งว่าทำอาหารเสร็จแล้ว
            buttonFinishCooking.Click += ButtonFinishCooking_Click;
        }

        // โหลด restaurantId จาก userId แล้วโหลดรายการออเดอร์
        //
        // Workflow:
        // 1. เรียก API หา restaurant ของ user นี้
        // 2. ถ้าไม่พบร้าน → แจ้งเตือนและปิดฟอร์ม
        // 3. เก็บ restaurantId
        // 4. โหลดรายการออเดอร์ของร้าน
        //
        // Side effects:
        // - เปลี่ยนค่า restaurantId
        // - โหลดข้อมูลเข้า DataGridView
        private async void RestaurantOrdersForm_Load(object? sender, EventArgs e)
        {
            try
            {
                // โหลดร้านที่ผูกกับ user ปัจจุบัน
                var restaurant = await RestUtil.GetAsync<RestaurantDto>(
                    $"restaurants/by-user/{userId}");

                // ถ้า user นี้ไม่มีร้าน ให้หยุด flow
                if (restaurant == null)
                {
                    MessageBox.Show("Restaurant not found for this user.");

                    Close();
                    return;
                }

                // เก็บ restaurantId สำหรับใช้กับ API ออเดอร์
                restaurantId = restaurant.RestaurantId;

                // โหลดรายการออเดอร์ของร้าน
                await LoadOrdersAsync();
            }
            catch (Exception ex)
            {
                // แจ้ง error เช่น API ล่ม หรือ network error
                MessageBox.Show(ex.Message);
            }
        }

        // โหลดรายการออเดอร์ของร้านจาก API
        //
        // Workflow:
        // 1. เรียก API โหลด orders ของร้าน
        // 2. filter เฉพาะ Pending และ Cooking
        // 3. สร้าง DataTable
        // 4. bind เข้า DataGridView
        // 5. อัปเดตปุ่ม action ตาม order ที่เลือก
        //
        // Side effects:
        // - เปลี่ยน datasource ของ dataGridOrders
        // - เปลี่ยนสถานะการมองเห็นของปุ่มต่าง ๆ
        private async Task LoadOrdersAsync()
        {
            try
            {
                // โหลด order ทั้งหมดของร้าน
                var orders = await RestUtil.GetAsync<List<OrderDto>>(
                    $"orders?restaurantId={restaurantId}");

                // สร้าง DataTable ใหม่
                ordersTable = new DataTable();

                // เก็บเลข order
                ordersTable.Columns.Add("order_id", typeof(int));

                // เก็บสถานะ order
                ordersTable.Columns.Add("status", typeof(string));

                // ถ้ามี order ให้ filter และเติมลง table
                if (orders != null)
                {
                    foreach (var order in orders
                        // ร้านสนใจเฉพาะ order ที่ยังต้องจัดการ
                        .Where(o => o.Status == "Pending" || o.Status == "Cooking")

                        // sort ตาม orderId เพื่อให้อ่านง่าย
                        .OrderBy(o => o.OrderId))
                    {
                        ordersTable.Rows.Add(
                            order.OrderId,
                            order.Status);
                    }
                }

                // bind table เข้า DataGridView
                dataGridOrders.DataSource = ordersTable;

                // อัปเดตปุ่มตามแถวที่ถูกเลือก
                UpdateButtonVisibility();
            }
            catch (Exception ex)
            {
                // แจ้ง error ตอนโหลด order
                MessageBox.Show(ex.Message);
            }
        }

        // Event handler เมื่อผู้ใช้เลือกแถวอื่นใน DataGrid
        //
        // หน้าที่หลักคือ refresh ปุ่ม action
        // ให้ตรงกับสถานะของ order ที่ถูกเลือก
        private void DataGridOrders_SelectionChanged(object? sender, EventArgs e)
        {
            UpdateButtonVisibility();
        }

        // อัปเดตปุ่ม action ตามสถานะของ order ที่เลือก
        //
        // Workflow:
        // - Pending  → แสดง รับ/ปฏิเสธ
        // - Cooking  → แสดง ทำเสร็จ
        // - อื่น ๆ    → ซ่อนทุกปุ่ม
        //
        // Side effects:
        // - เปลี่ยน Visible ของปุ่มต่าง ๆ
        private void UpdateButtonVisibility()
        {
            // ซ่อนทุกปุ่มก่อนเป็นค่าเริ่มต้น
            buttonAccept.Visible = false;
            buttonDecline.Visible = false;
            buttonFinishCooking.Visible = false;

            // ถ้ายังไม่มี order ถูกเลือก
            if (dataGridOrders.SelectedRows.Count == 0)
            {
                return;
            }

            // อ่าน row ที่ถูกเลือก
            DataGridViewRow row = dataGridOrders.SelectedRows[0];

            // อ่านสถานะของ order
            string status =
                row.Cells["status"].Value?.ToString()
                ?? string.Empty;

            // ถ้า order ยังรอร้านตอบรับ
            if (status == "Pending")
            {
                // ร้านสามารถรับหรือปฏิเสธได้
                buttonAccept.Visible = true;
                buttonDecline.Visible = true;
            }

            // ถ้าร้านกำลังทำอาหาร
            else if (status == "Cooking")
            {
                // ร้านสามารถแจ้งว่าทำเสร็จแล้วได้
                buttonFinishCooking.Visible = true;
            }
        }

        // Event handler สำหรับปุ่มรับออเดอร์
        //
        // Workflow:
        // Pending → Cooking
        //
        // Side effects:
        // - PATCH เปลี่ยนสถานะ order
        private void ButtonAccept_Click(object? sender, EventArgs e)
        {
            _ = UpdateOrderStatusAsync(
                "Pending",
                "Cooking");
        }

        // Event handler สำหรับปุ่มปฏิเสธออเดอร์
        //
        // Workflow:
        // - ลบ order ที่ยัง Pending
        //
        // Side effects:
        // - DELETE order จาก server
        private void ButtonDecline_Click(object? sender, EventArgs e)
        {
            _ = DeleteOrderAsync("Pending");
        }

        // Event handler สำหรับปุ่มทำอาหารเสร็จ
        //
        // Workflow:
        // Cooking → Waiting for rider
        //
        // Side effects:
        // - PATCH เปลี่ยนสถานะ order
        private void ButtonFinishCooking_Click(object? sender, EventArgs e)
        {
            _ = UpdateOrderStatusAsync(
                "Cooking",
                "Waiting for rider");
        }

        // ลบ order ที่เลือก
        //
        // Workflow:
        // 1. ตรวจว่ามี order ถูกเลือกหรือไม่
        // 2. ตรวจว่า order ยังอยู่ใน expectedStatus
        // 3. DELETE order ผ่าน API
        // 4. reload grid ใหม่
        //
        // Input:
        // - expectedStatus: status ที่อนุญาตให้ลบได้
        //
        // หมายเหตุ:
        // มีการเช็ค status ซ้ำเพื่อกัน race condition
        // เช่น order ถูกเปลี่ยนสถานะจาก client อื่นระหว่างใช้งาน
        //
        // Side effects:
        // - ลบ order ฝั่ง server
        // - reload DataGridView
        private async Task DeleteOrderAsync(string expectedStatus)
        {
            // ต้องมี order ถูกเลือกก่อน
            if (dataGridOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select an order.");
                return;
            }

            DataGridViewRow row = dataGridOrders.SelectedRows[0];

            // อ่าน order id
            object? orderIdValue = row.Cells["order_id"].Value;

            // อ่าน status ปัจจุบัน
            string currentStatus =
                row.Cells["status"].Value?.ToString()
                ?? string.Empty;

            // ป้องกัน order id หายหรือ invalid
            if (orderIdValue == null || orderIdValue == DBNull.Value)
            {
                MessageBox.Show("Order not found.");
                return;
            }

            // ถ้าสถานะไม่ตรง expectedStatus
            // แปลว่าข้อมูลอาจ stale → reload ใหม่
            if (!string.Equals(
                currentStatus,
                expectedStatus,
                StringComparison.OrdinalIgnoreCase))
            {
                await LoadOrdersAsync();
                return;
            }

            try
            {
                // ลบ order จากระบบ
                var response = await RestUtil.DeleteAsync(
                    $"orders/{Convert.ToInt32(orderIdValue)}");

                response.EnsureSuccessStatusCode();

                // reload grid หลังลบสำเร็จ
                await LoadOrdersAsync();
            }
            catch (Exception ex)
            {
                // แจ้ง error ตอนลบ order
                MessageBox.Show(ex.Message);
            }
        }

        // อัปเดตสถานะ order
        //
        // Workflow:
        // 1. ตรวจว่ามี order ถูกเลือก
        // 2. ตรวจ status ปัจจุบันตรง expectedStatus หรือไม่
        // 3. PATCH status ใหม่ไปยัง API
        // 4. reload grid
        //
        // Input:
        // - expectedStatus: status เดิมที่คาดหวัง
        // - newStatus: status ใหม่ที่ต้องการเปลี่ยน
        //
        // Side effects:
        // - เปลี่ยนสถานะ order ฝั่ง server
        // - reload DataGridView
        private async Task UpdateOrderStatusAsync(
            string expectedStatus,
            string newStatus)
        {
            // ต้องมี order ถูกเลือกก่อน
            if (dataGridOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select an order.");
                return;
            }

            DataGridViewRow row = dataGridOrders.SelectedRows[0];

            // อ่าน order id
            object? orderIdValue = row.Cells["order_id"].Value;

            // อ่านสถานะปัจจุบัน
            string currentStatus =
                row.Cells["status"].Value?.ToString()
                ?? string.Empty;

            // ป้องกัน order id invalid
            if (orderIdValue == null || orderIdValue == DBNull.Value)
            {
                MessageBox.Show("Order not found.");
                return;
            }

            // เช็ค status ซ้ำเพื่อกันข้อมูล stale/race condition
            if (!string.Equals(
                currentStatus,
                expectedStatus,
                StringComparison.OrdinalIgnoreCase))
            {
                await LoadOrdersAsync();
                return;
            }

            try
            {
                // payload สำหรับ PATCH status
                var payload = new UpdateStatusRequest(newStatus);

                // เปลี่ยนสถานะ order
                var response = await RestUtil.PatchAsync(
                    $"orders/{Convert.ToInt32(orderIdValue)}/status",
                    payload);

                response.EnsureSuccessStatusCode();

                // reload grid หลังอัปเดตสำเร็จ
                await LoadOrdersAsync();
            }
            catch (Exception ex)
            {
                // แจ้ง error ตอน update status
                MessageBox.Show(ex.Message);
            }
        }
    }
}