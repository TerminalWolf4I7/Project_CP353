using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows.Forms;
using Delivery.Client.Models;

namespace Delivery.Client
{
    public partial class Login : Form
    {
        // Constructor ของหน้า Login
        // หน้าที่หลักคือเรียก InitializeComponent() เพื่อสร้าง UI controls ทั้งหมดจากไฟล์ Designer
        public Login()
        {
            InitializeComponent();
        }

        // Event handler หลักเมื่อผู้ใช้กดปุ่ม Login
        //
        // Workflow:
        // 1. อ่านค่า User ID จาก TextBox
        // 2. ตรวจสอบว่า User ID เป็นตัวเลขหรือไม่
        // 3. ส่ง User ID ไปยัง API auth/login เพื่อยืนยันตัวตน
        // 4. อ่านผลลัพธ์จาก API แล้วเช็ค Role ของผู้ใช้
        // 5. เปิด Form ที่ตรงกับ Role เช่น Customer, Restaurant หรือ Rider
        // 6. ซ่อนหน้า Login ไว้ เพื่อให้กลับมาจัดการ lifecycle ของ app ได้
        //
        // Side effects:
        // - เรียก API ภายนอกผ่าน RestUtil
        // - เปิด Form ใหม่ตาม role
        // - ซ่อน Login Form ด้วย Hide()
        // - อาจปิด Application ถ้าไม่มี Login form ค้างอยู่
        private async void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                // ตรวจสอบ input ก่อนเรียก API
                // ถ้า user กรอกค่าไม่ใช่ตัวเลข จะหยุด flow ทันทีเพื่อป้องกัน request ที่ invalid
                if (!int.TryParse(txtUserId.Text, out int userId))
                {
                    MessageBox.Show("Please enter number only");
                    return;
                }

                // สร้าง request payload สำหรับส่งไป login API
                // ในระบบนี้ใช้ userId เป็นตัวระบุตัวตนแทน username/password
                var payload = new LoginRequest(userId);

                // ส่ง request ไปยัง endpoint auth/login
                // response จะบอกว่า userId นี้มีอยู่จริงและ login ได้หรือไม่
                var response = await RestUtil.PostResponseAsync("auth/login", payload);

                // ถ้า API ตอบกลับเป็น error เช่น 404, 400, 500
                // ให้ถือว่า login ไม่สำเร็จและหยุด workflow
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("User ID not found");
                    return;
                }

                // แปลง response body เป็น LoginResponse
                // โดยคาดหวังว่าจะมีข้อมูลอย่างน้อยคือ Role ของผู้ใช้
                var login = await RestUtil.ReadAsAsync<LoginResponse>(response);

                // ป้องกันกรณี API success แต่ response body ว่างหรือ parse ไม่ได้
                if (login == null)
                {
                    MessageBox.Show("Login failed");
                    return;
                }

                // ดึง role จากผลลัพธ์ login
                // role นี้ใช้ตัดสินใจว่าจะส่งผู้ใช้ไปหน้าไหนต่อ
                string role = login.Role;

                // เตรียมตัวแปรสำหรับ Form ถัดไป
                // จะถูก assign ตาม role ของผู้ใช้
                Form nextForm = null;

                // ถ้าเป็นลูกค้า ให้เปิดหน้า CustomerForm
                // ส่ง userId ไปด้วยเพื่อให้หน้าถัดไปโหลดข้อมูลของ user คนนี้ได้
                if (role == "Customer")
                {
                    nextForm = new CustomerForm(userId);
                }
                // ถ้าเป็นร้านอาหาร ให้เปิดหน้า RestaurantForm
                // userId ใช้ระบุร้าน/บัญชีที่ login เข้ามา
                else if (role == "Restaurant")
                {
                    nextForm = new RestaurantForm(userId);
                }
                // ถ้าเป็นไรเดอร์ ให้เปิดหน้า RiderForm
                // userId ใช้โหลดงานจัดส่งหรือข้อมูลที่เกี่ยวข้องกับไรเดอร์คนนี้
                else if (role == "Rider")
                {
                    nextForm = new RiderForm(userId);
                }
                // ถ้า role ที่ API ส่งมาไม่ตรงกับ role ที่ client รองรับ
                // ให้หยุด flow เพื่อป้องกันการเปิดหน้าผิดหรือ state ผิดพลาด
                else
                {
                    MessageBox.Show("Unknown role: " + role);
                    return;
                }

                // แจ้งผู้ใช้ว่า login สำเร็จก่อนเข้าสู่หน้าหลักตาม role
                MessageBox.Show("Login Success");

                // ผูก event เมื่อ Form ถัดไปถูกปิด
                // ใช้ควบคุม lifecycle ของ application หลังจากซ่อนหน้า Login
                nextForm.FormClosed += (s, args) =>
                {
                    // ถ้าไม่มี Login form เหลืออยู่ใน OpenForms แล้ว
                    // ให้ปิด application เพื่อไม่ให้ process ค้างอยู่เบื้องหลัง
                    if (!Application.OpenForms.OfType<Login>().Any())
                    {
                        Application.Exit();
                    }
                };

                // แสดง Form ตาม role ของผู้ใช้
                nextForm.Show();

                // ซ่อนหน้า Login แทนการ Close()
                // เพราะ Close() อาจทำให้ application ปิดทันทีถ้า Login เป็น main form
                Hide();
            }
            catch (Exception ex)
            {
                // จับ error ที่เกิดขึ้นระหว่าง login flow
                // เช่น network error, API error, deserialize error หรือ runtime exception อื่น ๆ
                MessageBox.Show(ex.Message);
            }
        }

        // Event handler เมื่อข้อความในช่อง User ID เปลี่ยน
        //
        // ตอนนี้ยังไม่มี logic
        // ในอนาคตสามารถใช้สำหรับ:
        // - validate input แบบ realtime
        // - เปิด/ปิดปุ่ม Login
        // - ล้าง error message เมื่อ user แก้ไข input
        private void txtUserId_TextChanged(object sender, EventArgs e)
        {
        }

        // Event handler เมื่อคลิก subtitle
        //
        // ตอนนี้ยังไม่มี logic
        // ปกติถ้าไม่ได้ใช้จริง สามารถลบ event binding ใน Designer ได้
        // หรือใช้ในอนาคตสำหรับแสดง help / hint / signup information
        private void lblSubtitle_Click(object sender, EventArgs e)
        {
        }
    }
}