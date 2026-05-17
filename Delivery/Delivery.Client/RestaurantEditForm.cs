using System;
using System.Data;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Delivery.Client.Models;

namespace Delivery.Client
{
    public partial class RestaurantEditForm : Form
    {
        // userId ของเจ้าของร้านที่ login เข้ามา
        // ใช้สำหรับโหลดร้านที่ผูกกับ user นี้
        private readonly int userId;

        // restaurantId จริงของร้าน
        // จะถูกโหลดจาก API ตอนเปิดฟอร์ม
        private int restaurantId;

        // DataTable สำหรับ bind กับ DataGridView
        // ใช้เก็บรายการเมนูทั้งหมดบนหน้าจอ
        private DataTable menuTable = new DataTable();

        // Constructor ของหน้า RestaurantEditForm
        //
        // Workflow:
        // 1. รับ userId ของเจ้าของร้าน
        // 2. สร้าง UI ผ่าน InitializeComponent()
        // 3. ผูก event หลักของฟอร์ม
        //
        // Input:
        // - userId: user ที่เป็นเจ้าของร้าน
        //
        // Side effects:
        // - ผูก event สำหรับ load/save/add/delete menu
        public RestaurantEditForm(int userId)
        {
            InitializeComponent();

            dataGridMenu.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dataGridMenu.MultiSelect = true;

            // เก็บ userId ไว้ใช้โหลดข้อมูลร้าน
            this.userId = userId;

            // โหลดข้อมูลร้านและเมนูเมื่อ form เปิด
            Load += RestaurantEditForm_Load;

            // ปุ่มบันทึกการแก้ไขร้านและเมนู
            buttonSave.Click += ButtonSave_Click;

            // ปุ่มเพิ่มเมนูใหม่ใน grid
            buttonAdd.Click += ButtonAdd_Click;

            // ปุ่มลบเมนูที่เลือก
            buttonDelete.Click += ButtonDelete_Click;
        }

        // โหลดข้อมูลร้านและรายการเมนูจาก API
        //
        // Workflow:
        // 1. โหลดร้านจาก userId
        // 2. ถ้าไม่พบร้าน ให้แจ้งเตือนและปิดฟอร์ม
        // 3. โหลดเมนูของร้าน
        // 4. สร้าง DataTable สำหรับ bind กับ DataGridView
        // 5. เติมข้อมูลเมนูลง grid
        //
        // Side effects:
        // - เปลี่ยนค่า restaurantId
        // - bind data เข้า DataGridView
        // - แสดงชื่อร้านใน TextBox
        private async void RestaurantEditForm_Load(object? sender, EventArgs e)
        {
            try
            {
                // โหลดร้านที่ผูกกับ user นี้
                var restaurant = await RestUtil.GetAsync<RestaurantDto>(
                    $"restaurants/by-user/{userId}");

                // ถ้า user นี้ไม่มีร้าน ให้หยุด flow ทันที
                if (restaurant == null)
                {
                    MessageBox.Show("Restaurant not found for this user.");

                    Close();
                    return;
                }

                // เก็บ restaurantId ไว้ใช้กับ API อื่น ๆ
                restaurantId = restaurant.RestaurantId;

                // แสดงชื่อร้านใน textbox
                textRestaurantName.Text = restaurant.Name;

                // โหลดรายการเมนูทั้งหมดของร้าน
                var menuItems = await RestUtil.GetAsync<List<MenuItemDto>>(
                    $"restaurants/{restaurantId}/menu");

                // สร้าง DataTable ใหม่
                // ใช้เป็น datasource ของ DataGridView
                menuTable = new DataTable();

                // item_id ใช้แยกเมนูเก่ากับเมนูใหม่
                menuTable.Columns.Add("item_id", typeof(int));

                // ชื่อเมนู
                menuTable.Columns.Add("name", typeof(string));

                // ราคาเมนู
                menuTable.Columns.Add("price", typeof(decimal));

                // ถ้ามีเมนู ให้เติมข้อมูลลง table
                if (menuItems != null)
                {
                    // sort ตาม itemId เพื่อให้ลำดับใน grid คงที่
                    foreach (var item in menuItems.OrderBy(i => i.ItemId))
                    {
                        menuTable.Rows.Add(
                            item.ItemId,
                            item.Name,
                            item.Price);
                    }
                }

                // bind table เข้า DataGridView
                dataGridMenu.DataSource = menuTable;

                // ซ่อน item_id เพราะเป็น internal id
                // user ไม่จำเป็นต้องเห็น
                if (dataGridMenu.Columns["item_id"] != null)
                {
                    dataGridMenu.Columns["item_id"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                // แจ้ง error ตอนโหลดข้อมูลร้านหรือเมนู
                MessageBox.Show(ex.Message);
            }
        }

        // เพิ่มแถวใหม่ใน DataGridView
        //
        // Workflow:
        // 1. ตรวจว่า DataTable ถูกสร้างแล้วหรือยัง
        // 2. เพิ่มแถวว่างเข้า menuTable
        //
        // หมายเหตุ:
        // item_id = DBNull หมายถึงเมนูใหม่
        // ตอน Save ระบบจะใช้ POST แทน PUT
        //
        // Side effects:
        // - เพิ่ม row ใหม่ใน grid
        private void ButtonAdd_Click(object? sender, EventArgs e)
        {
            // ป้องกันกรณี DataTable ยังไม่พร้อมใช้งาน
            if (menuTable.Columns.Count == 0)
            {
                return;
            }

            // เพิ่มแถวใหม่
            //
            // DBNull.Value ใช้แทน "ยังไม่มี item_id"
            // เพื่อบอกว่าเมนูนี้ยังไม่ถูกสร้างใน database
            menuTable.Rows.Add(DBNull.Value, string.Empty, 0m);
        }

        // ลบเมนูที่ถูกเลือกใน DataGridView
        //
        // Workflow:
        // 1. ตรวจว่ามี row ถูกเลือกหรือไม่
        // 2. วนลูปทุก row ที่เลือก
        // 3. ถ้าเมนูนี้มี item_id แล้ว → DELETE API
        // 4. ลบ row ออกจาก grid
        //
        // Side effects:
        // - ลบเมนูใน database
        // - ลบ row บน UI
        private async void ButtonDelete_Click(object? sender, EventArgs e)
        {
            // ถ้า user ยังไม่ได้เลือก row
            if (dataGridMenu.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a row to delete.");
                return;
            }

            try
            {
                // รองรับการลบหลาย row พร้อมกัน
                foreach (DataGridViewRow row in dataGridMenu.SelectedRows)
                {
                    // ข้าม placeholder row ของ DataGridView
                    if (row.IsNewRow)
                    {
                        continue;
                    }

                    // อ่าน item_id ของเมนู
                    object? itemIdValue = row.Cells["item_id"].Value;

                    // ถ้ามี item_id แปลว่าเมนูนี้มีอยู่ใน database แล้ว
                    if (itemIdValue != null && itemIdValue != DBNull.Value)
                    {
                        int itemId = Convert.ToInt32(itemIdValue);

                        // ลบเมนูจาก server ก่อน
                        var response = await RestUtil.DeleteAsync(
                            $"restaurants/menu/{itemId}?restaurantId={restaurantId}");

                        response.EnsureSuccessStatusCode();
                    }

                    // ลบ row ออกจาก grid
                    dataGridMenu.Rows.Remove(row);
                }
            }
            catch (Exception ex)
            {
                // แจ้ง error ระหว่างลบเมนู
                MessageBox.Show(ex.Message);
            }
        }

        // บันทึกข้อมูลร้านและรายการเมนูทั้งหมด
        //
        // Workflow:
        // 1. validate ชื่อร้าน
        // 2. PUT อัปเดตชื่อร้าน
        // 3. วนทุก row ของเมนู
        // 4. validate ชื่อเมนูและราคา
        // 5. ถ้าไม่มี item_id → POST สร้างเมนูใหม่
        // 6. ถ้ามี item_id → PUT อัปเดตเมนูเดิม
        //
        // Side effects:
        // - อัปเดตข้อมูลร้านใน database
        // - สร้าง/แก้ไขเมนูใน database
        // - แถวใหม่จะได้รับ item_id จาก server
        private async void ButtonSave_Click(object? sender, EventArgs e)
        {
            // validate ชื่อร้าน
            if (string.IsNullOrWhiteSpace(textRestaurantName.Text))
            {
                MessageBox.Show("Restaurant name is required.");
                return;
            }

            try
            {
                // =========================
                // UPDATE RESTAURANT
                // =========================

                // trim เพื่อลบ space หน้า/หลัง
                var restaurantPayload =
                    new RestaurantUpdateRequest(
                        textRestaurantName.Text.Trim());

                // อัปเดตชื่อร้าน
                var restaurantResponse = await RestUtil.PutAsync(
                    $"restaurants/{restaurantId}",
                    restaurantPayload);

                restaurantResponse.EnsureSuccessStatusCode();

                // =========================
                // UPSERT MENU ITEMS
                // =========================

                // วนทุก row ของ grid
                foreach (DataGridViewRow row in dataGridMenu.Rows)
                {
                    // ข้าม placeholder row
                    if (row.IsNewRow)
                    {
                        continue;
                    }

                    // อ่านค่าจาก grid
                    string name =
                        row.Cells["name"].Value?.ToString()
                        ?? string.Empty;

                    string priceText =
                        row.Cells["price"].Value?.ToString()
                        ?? "0";

                    // validate ชื่อเมนู
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        MessageBox.Show("Menu item name is required.");
                        return;
                    }

                    // validate ราคา
                    if (!decimal.TryParse(priceText, out decimal price))
                    {
                        MessageBox.Show("Invalid price value.");
                        return;
                    }

                    // payload สำหรับ create/update menu item
                    var payload = new MenuItemUpsertRequest(name, price);

                    // อ่าน item_id
                    object? itemIdValue = row.Cells["item_id"].Value;

                    // =========================
                    // CREATE NEW ITEM
                    // =========================

                    // ถ้ายังไม่มี item_id
                    // แปลว่าเป็นเมนูใหม่
                    if (itemIdValue == null || itemIdValue == DBNull.Value)
                    {
                        // สร้างเมนูใหม่
                        var response = await RestUtil.PostResponseAsync(
                            $"restaurants/{restaurantId}/menu",
                            payload);

                        response.EnsureSuccessStatusCode();

                        // อ่าน item_id ใหม่จาก server
                        int newId = await RestUtil.ReadAsAsync<int>(response);

                        // ใส่ item_id กลับเข้า row
                        // เพื่อให้ครั้งถัดไประบบรู้ว่าเป็นเมนูเดิม
                        row.Cells["item_id"].Value = newId;
                    }

                    // =========================
                    // UPDATE EXISTING ITEM
                    // =========================
                    else
                    {
                        int itemId = Convert.ToInt32(itemIdValue);

                        // อัปเดตเมนูเดิม
                        var response = await RestUtil.PutAsync(
                            $"restaurants/menu/{itemId}",
                            payload);

                        response.EnsureSuccessStatusCode();
                    }
                }

                // แจ้งว่าบันทึกสำเร็จ
                MessageBox.Show("Saved.");
            }
            catch (Exception ex)
            {
                // แจ้ง error ระหว่าง save
                MessageBox.Show(ex.Message);
            }
        }
    }
}
