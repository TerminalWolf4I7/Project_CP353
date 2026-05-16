using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows.Forms;
using Delivery.Client.Models;

namespace Delivery.Client
{
    public partial class RestaurantForm : Form
    {
        // userId ของเจ้าของร้านที่ login เข้ามา
        // ใช้โหลดข้อมูลร้านและส่งต่อไปยังฟอร์มจัดการต่าง ๆ
        private readonly int userId;

        // Constructor ของหน้า RestaurantForm
        //
        // Workflow:
        // 1. รับ userId ของร้านที่ login สำเร็จ
        // 2. สร้าง UI controls ผ่าน InitializeComponent()
        // 3. ผูก event ต่าง ๆ ของฟอร์ม
        //
        // Input:
        // - userId: เจ้าของร้านที่กำลังใช้งานระบบ
        //
        // Side effects:
        // - ผูก event โหลดข้อมูลร้าน
        // - ผูกปุ่มจัดการออเดอร์
        // - ผูกปุ่มแก้ไขร้าน
        // - ผูกปุ่ม logout
        public RestaurantForm(int userId)
        {
            InitializeComponent();

            // เก็บ userId ของร้านปัจจุบันไว้ใช้ทั้งฟอร์ม
            this.userId = userId;

            // โหลดข้อมูลร้านเมื่อฟอร์มเปิด
            Load += RestaurantForm_Load;

            // ปุ่มดูรายการออเดอร์ของร้าน
            button1.Click += ButtonOrders_Click;

            // ปุ่มแก้ไขข้อมูลร้านและเมนู
            button2.Click += ButtonEdit_Click;

            // ปุ่มออกจากระบบ
            buttonLogout.Click += BtnLogout_Click;
        }

        // โหลดข้อมูลร้านของ user นี้จาก API
        //
        // Workflow:
        // 1. เรียก API เพื่อหาร้านที่ผูกกับ userId
        // 2. ถ้าเจอร้าน ให้แสดงชื่อร้านบน label
        //
        // Side effects:
        // - เปลี่ยนข้อความบน label1
        // - แสดง MessageBox ถ้า API error
        private async void RestaurantForm_Load(object? sender, EventArgs e)
        {
            try
            {
                // โหลดข้อมูลร้านจาก userId ของเจ้าของร้าน
                var restaurant = await RestUtil.GetAsync<RestaurantDto>(
                    $"restaurants/by-user/{userId}");

                // ถ้าพบร้าน ให้แสดงชื่อร้านบน UI
                if (restaurant != null)
                {
                    label1.Text = restaurant.Name;
                }
            }
            catch (Exception ex)
            {
                // แจ้ง error เช่น API ล่ม หรือ network error
                MessageBox.Show(ex.Message);
            }
        }

        // เปิดหน้าแก้ไขข้อมูลร้านและเมนู
        //
        // Workflow:
        // 1. สร้าง RestaurantEditForm
        // 2. ส่ง userId ไปให้ฟอร์มใหม่
        // 3. เปิดแบบ Modal ด้วย ShowDialog()
        //
        // หมายเหตุ:
        // ใช้ Modal เพื่อบังคับให้ user จัดการหน้า edit ให้เสร็จก่อน
        // แล้วค่อยกลับมาหน้า RestaurantForm
        //
        // Side effects:
        // - เปิด RestaurantEditForm
        private void ButtonEdit_Click(object? sender, EventArgs e)
        {
            // สร้างฟอร์มแก้ไขร้านของ user ปัจจุบัน
            RestaurantEditForm editForm =
                new RestaurantEditForm(userId);

            // เปิดแบบ modal dialog
            editForm.ShowDialog(this);
        }

        // เปิดหน้าจัดการออเดอร์ของร้าน
        //
        // Workflow:
        // 1. สร้าง RestaurantOrdersForm
        // 2. ส่ง userId ไปโหลดออเดอร์ของร้านนี้
        // 3. เปิดแบบ Modal
        //
        // Side effects:
        // - เปิด RestaurantOrdersForm
        private void ButtonOrders_Click(object? sender, EventArgs e)
        {
            // สร้างฟอร์มจัดการออเดอร์ของร้าน
            RestaurantOrdersForm ordersForm =
                new RestaurantOrdersForm(userId);

            // เปิดแบบ modal เพื่อ focus การจัดการออเดอร์
            ordersForm.ShowDialog(this);
        }

        // Logout ออกจากระบบ
        //
        // Workflow:
        // 1. แสดง dialog ยืนยัน logout
        // 2. ถ้าผู้ใช้กดยืนยัน
        //    - เปิด Login form ใหม่
        //    - ปิด RestaurantForm ปัจจุบัน
        //
        // Side effects:
        // - เปิด Login form
        // - ปิด RestaurantForm
        private void BtnLogout_Click(object? sender, EventArgs e)
        {
            // แสดง confirmation dialog ก่อน logout
            var result = MessageBox.Show(
                "คุณต้องการออกจากระบบใช่หรือไม่?",
                "ยืนยันการออกจากระบบ",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            // ถ้าผู้ใช้กด Yes
            if (result == DialogResult.Yes)
            {
                // กลับไปหน้า Login
                Login loginForm = new Login();

                loginForm.Show();

                // ปิดฟอร์มร้านปัจจุบัน
                Close();
            }
        }
    }
}
