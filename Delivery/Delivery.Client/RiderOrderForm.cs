using System;
using System.Data;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Delivery.Client.Models;

namespace Delivery.Client
{
    public partial class RiderOrderForm : Form
    {
        // userId ของ Rider ที่ login อยู่
        // ใช้โหลดงานปัจจุบันและยืนยันส่งสำเร็จ
        private readonly int _userId;

        // Constructor ของหน้า RiderOrderForm
        //
        // Workflow:
        // 1. รับ userId ของ Rider
        // 2. สร้าง UI ผ่าน InitializeComponent()
        // 3. ผูก event โหลดข้อมูลและปุ่มต่าง ๆ
        //
        // Input:
        // - userId: Rider ปัจจุบัน
        //
        // Side effects:
        // - ผูก event โหลดงาน Rider
        // - ผูกปุ่ม complete delivery
        // - ผูกปุ่มย้อนกลับ
        public RiderOrderForm(int userId)
        {
            InitializeComponent();

            // เก็บ rider ปัจจุบันไว้ใช้ทั้งฟอร์ม
            _userId = userId;

            // โหลดงานของ Rider เมื่อเปิดฟอร์ม
            Load +=
                RiderOrderForm_Load;

            // ปุ่มยืนยันส่งสำเร็จ
            buttonComplete.Click +=
                ButtonComplete_Click;

            // ปุ่มกลับไปหน้า RiderForm
            buttonBack.Click +=
                ButtonBack_Click;
        }

        // Event handler ตอนเปิดฟอร์ม
        //
        // Workflow:
        // - โหลดงานปัจจุบันของ Rider
        //
        // Side effects:
        // - โหลดข้อมูลเข้า DataGrid
        private async void RiderOrderForm_Load(
            object sender,
            EventArgs e)
        {
            await LoadMyOrderAsync();
        }

        // โหลดงานปัจจุบันของ Rider จาก API
        //
        // Workflow:
        // 1. เรียก API current-order
        // 2. สร้าง DataTable
        // 3. เติมข้อมูล order ลง table
        // 4. bind เข้า DataGridView
        // 5. เปิด/ปิดปุ่ม complete ตามจำนวน order
        //
        // Side effects:
        // - เปลี่ยน datasource ของ dataGridOrder
        // - เปลี่ยน Enabled state ของ buttonComplete
        private async Task LoadMyOrderAsync()
        {
            try
            {
                // โหลด current order ของ Rider คนนี้
                var response = await RestUtil.GetResponseAsync(
                    $"riders/{_userId}/current-order");

                // สร้าง DataTable สำหรับ bind เข้า DataGrid
                DataTable dt = new DataTable();

                dt.Columns.Add("order_id", typeof(int));
                dt.Columns.Add("user_id", typeof(int));
                dt.Columns.Add("name", typeof(string));
                dt.Columns.Add("restaurant_id", typeof(int));
                dt.Columns.Add("status", typeof(string));

                // ถ้า API ตอบกลับ success
                if (response.IsSuccessStatusCode)
                {
                    // อ่านรายการ order จาก response
                    var orders =
                        await RestUtil.ReadAsAsync<List<RiderCurrentOrderDto>>(
                            response);

                    // เติมข้อมูลลง DataTable
                    if (orders != null)
                    {
                        foreach (var order in orders)
                        {
                            dt.Rows.Add(
                                order.OrderId,
                                order.UserId,
                                order.CustomerName,
                                order.RestaurantId,
                                order.Status);
                        }
                    }
                }

                // bind table เข้า DataGridView
                dataGridOrder.DataSource = dt;

                // เลือกทั้ง row เวลากด
                dataGridOrder.SelectionMode =
                    DataGridViewSelectionMode.FullRowSelect;

                // ให้เลือกได้ทีละ order
                dataGridOrder.MultiSelect = false;

                // เปิดปุ่ม complete เฉพาะกรณีมีงานอยู่
                buttonComplete.Enabled =
                    dt.Rows.Count > 0;
            }
            catch (Exception ex)
            {
                // แจ้ง error ตอนโหลดงาน Rider
                MessageBox.Show(
                    ex.Message);
            }
        }

        // Rider ยืนยันว่าจัดส่งสำเร็จ
        //
        // Workflow:
        // 1. ตรวจว่ามี order ถูกเลือกหรือไม่
        // 2. อ่าน orderId
        // 3. POST complete ไปยัง API
        // 4. server อัปเดตสถานะ order เป็น Delivered/Completed
        // 5. reload ข้อมูลใหม่
        //
        // Side effects:
        // - เปลี่ยนสถานะ order ฝั่ง server
        // - ลูกค้าจะเห็นสถานะใหม่ใน OrderStatusForm
        // - reload DataGrid
        private async void ButtonComplete_Click(
            object sender,
            EventArgs e)
        {
            // ต้องเลือก order ก่อน
            if (dataGridOrder.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "กรุณาเลือก order ที่ต้องการส่งสำเร็จ");

                return;
            }

            // อ่าน orderId จาก row ที่เลือก
            int orderId =
                Convert.ToInt32(
                    dataGridOrder.SelectedRows[0]
                    .Cells["order_id"]
                    .Value);

            try
            {
                // แจ้ง server ว่า Rider ส่งงานสำเร็จแล้ว
                var response = await RestUtil.PostAsync(
                    $"riders/{_userId}/complete/{orderId}");

                // ถ้า API คืน error จะ throw exception
                response.EnsureSuccessStatusCode();

                // แจ้งผู้ใช้ว่าส่งสำเร็จ
                MessageBox.Show("Delivery completed.");

                // reload งานใหม่
                // ปกติ order นี้จะหายไปจาก current order แล้ว
                await LoadMyOrderAsync();
            }
            catch (Exception ex)
            {
                // แจ้ง error ตอน complete delivery
                MessageBox.Show(ex.Message);
            }
        }

        // ปุ่มย้อนกลับไปหน้า RiderForm
        //
        // Workflow:
        // - ปิดฟอร์มปัจจุบัน
        //
        // หมายเหตุ:
        // ใช้ Close() เพราะ RiderForm ยังเปิดอยู่ด้านหลัง
        private void ButtonBack_Click(
            object sender,
            EventArgs e)
        {
            Close();
        }

        // Event handler ที่ถูก generate จาก Designer
        //
        // ตอนนี้ยังไม่ได้ใช้งานจริง
        // สามารถลบได้ถ้าไม่มี event binding เหลืออยู่ใน Designer
        private void buttonComplete_Click_1(
            object sender,
            EventArgs e)
        {
        }
    }
}
