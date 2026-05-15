using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace Delivery
{
    public partial class MenuForm : Form
    {
        private int restaurantId;
        private int userId;

        public MenuForm(int restaurantId, int userId)
        {
            InitializeComponent();
            this.restaurantId = restaurantId;
            this.userId = userId;
            this.Load += MenuForm_Load;
            btnCheckout.Click += BtnCheckout_Click;
        }

        private void MenuForm_Load(object sender, EventArgs e)
        {
            LoadMenu();
            UpdateTotal();
        }

        private void LoadMenu()
        {
            try
            {
                flpMenu.Controls.Clear();
                using (NpgsqlConnection conn = new NpgsqlConnection(Database.connectionString))
                {
                    conn.Open();
                    string query = "SELECT item_id, name, price, description FROM menu_items WHERE restaurant_id = @rid AND is_available = TRUE";
                    
                    // ดึงชื่อร้านมาแสดงที่ Header ด้วย
                    string nameQuery = "SELECT name FROM restaurants WHERE restaurant_id = @rid";
                    using (var nameCmd = new NpgsqlCommand(nameQuery, conn))
                    {
                        nameCmd.Parameters.AddWithValue("@rid", restaurantId);
                        lblRestaurantName.Text = nameCmd.ExecuteScalar()?.ToString() ?? "ร้านอาหาร";
                    }

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@rid", restaurantId);
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                string name = reader.GetString(1);
                                decimal price = reader.GetDecimal(2);
                                string desc = reader.IsDBNull(3) ? "" : reader.GetString(3);

                                // สร้าง Item Card
                                Panel card = new Panel();
                                card.Size = new Size(220, 280);
                                card.BackColor = Color.White;
                                card.Margin = new Padding(10);
                                card.BorderStyle = BorderStyle.FixedSingle;

                                Label lblName = new Label { Text = name, Font = new Font("Segoe UI", 12, FontStyle.Bold), Location = new Point(10, 10), AutoSize = true };
                                Label lblPrice = new Label { Text = price.ToString("N2") + " บาท", ForeColor = Color.FromArgb(46, 204, 113), Font = new Font("Segoe UI", 11, FontStyle.Bold), Location = new Point(10, 40), AutoSize = true };
                                Label lblDesc = new Label { Text = desc, Font = new Font("Segoe UI", 9), Location = new Point(10, 75), Size = new Size(200, 120) };
                                
                                Button btnAdd = new Button { 
                                    Text = "เพิ่มลงตะกร้า +", 
                                    BackColor = Color.FromArgb(46, 204, 113), 
                                    ForeColor = Color.White, 
                                    FlatStyle = FlatStyle.Flat,
                                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                                    Location = new Point(10, 220), 
                                    Size = new Size(200, 45),
                                    Tag = id // เก็บ Item ID
                                };
                                btnAdd.Click += BtnAddToCart_Click;

                                card.Controls.Add(lblName);
                                card.Controls.Add(lblPrice);
                                card.Controls.Add(lblDesc);
                                card.Controls.Add(btnAdd);

                                flpMenu.Controls.Add(card);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading menu: " + ex.Message);
            }
        }

        private void BtnAddToCart_Click(object? sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                int itemId = (int)btn.Tag;
                AddToCart(itemId);
                UpdateTotal();
            }
        }

        private void AddToCart(int itemId)
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(Database.connectionString))
                {
                    conn.Open();
                    string simpleFindCart = "SELECT cart_id FROM carts WHERE user_id = @uid ORDER BY created_at DESC LIMIT 1";
                    int cartId = 0;
                    using (var cmd = new NpgsqlCommand(simpleFindCart, conn))
                    {
                        cmd.Parameters.AddWithValue("@uid", this.userId);
                        var result = cmd.ExecuteScalar();
                        if (result != null) cartId = (int)result;
                    }

                    if (cartId == 0)
                    {
                        string createCart = "INSERT INTO carts (user_id) VALUES (@uid) RETURNING cart_id";
                        using (var cmd = new NpgsqlCommand(createCart, conn))
                        {
                            cmd.Parameters.AddWithValue("@uid", this.userId);
                            cartId = (int)cmd.ExecuteScalar();
                        }
                    }

                    string checkItem = "SELECT cart_item_id, quantity FROM cart_items WHERE cart_id = @cid AND item_id = @iid";
                    int cartItemId = 0;
                    int currentQty = 0;
                    using (var cmd = new NpgsqlCommand(checkItem, conn))
                    {
                        cmd.Parameters.AddWithValue("@cid", cartId);
                        cmd.Parameters.AddWithValue("@iid", itemId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                cartItemId = reader.GetInt32(0);
                                currentQty = reader.GetInt32(1);
                            }
                        }
                    }

                    if (cartItemId > 0)
                    {
                        string updateQty = "UPDATE cart_items SET quantity = @qty WHERE cart_item_id = @ciid";
                        using (var cmd = new NpgsqlCommand(updateQty, conn))
                        {
                            cmd.Parameters.AddWithValue("@qty", currentQty + 1);
                            cmd.Parameters.AddWithValue("@ciid", cartItemId);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string insertItem = "INSERT INTO cart_items (cart_id, item_id, quantity) VALUES (@cid, @iid, 1)";
                        using (var cmd = new NpgsqlCommand(insertItem, conn))
                        {
                            cmd.Parameters.AddWithValue("@cid", cartId);
                            cmd.Parameters.AddWithValue("@iid", itemId);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void UpdateTotal()
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(Database.connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT SUM(ci.quantity * mi.price) 
                        FROM cart_items ci 
                        JOIN menu_items mi ON ci.item_id = mi.item_id 
                        JOIN carts c ON ci.cart_id = c.cart_id 
                        WHERE c.user_id = @uid AND c.cart_id = (SELECT cart_id FROM carts WHERE user_id = @uid ORDER BY created_at DESC LIMIT 1)";
                    
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@uid", this.userId);
                        var result = cmd.ExecuteScalar();
                        decimal total = (result == DBNull.Value) ? 0 : Convert.ToDecimal(result);
                        lblTotal.Text = $"ราคารวม: {total.ToString("N2")} บาท";
                    }
                }
            }
            catch { }
        }

        private void BtnCheckout_Click(object? sender, EventArgs e)
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(Database.connectionString))
                {
                    conn.Open();
                    using (var trans = conn.BeginTransaction())
                    {
                        // 1. หาตะกร้าล่าสุด
                        string getCart = "SELECT cart_id FROM carts WHERE user_id = @uid ORDER BY created_at DESC LIMIT 1";
                        int cartId;
                        using (var cmd = new NpgsqlCommand(getCart, conn))
                        {
                            cmd.Parameters.AddWithValue("@uid", this.userId);
                            var result = cmd.ExecuteScalar();
                            if (result == null) return;
                            cartId = (int)result;
                        }

                        // 2. ดึงรายการในตะกร้ามาคำนวณราคารวม
                        string getItems = "SELECT ci.item_id, ci.quantity, mi.price FROM cart_items ci JOIN menu_items mi ON ci.item_id = mi.item_id WHERE ci.cart_id = @cid";
                        decimal totalPrice = 0;
                        DataTable dtItems = new DataTable();
                        using (var cmd = new NpgsqlCommand(getItems, conn))
                        {
                            cmd.Parameters.AddWithValue("@cid", cartId);
                            using (var adapter = new NpgsqlDataAdapter(cmd))
                            {
                                adapter.Fill(dtItems);
                            }
                        }

                        if (dtItems.Rows.Count == 0)
                        {
                            MessageBox.Show("ตะกร้าว่างเปล่า กรุณาเลือกอาหารก่อนครับ");
                            return;
                        }

                        foreach (DataRow row in dtItems.Rows)
                        {
                            totalPrice += Convert.ToDecimal(row["price"]) * Convert.ToInt32(row["quantity"]);
                        }

                        // 3. สร้าง Order
                        string createOrder = "INSERT INTO orders (user_id, restaurant_id, total_price, status) VALUES (@uid, @rid, @price, 'Pending') RETURNING order_id";
                        int orderId;
                        using (var cmd = new NpgsqlCommand(createOrder, conn))
                        {
                            cmd.Parameters.AddWithValue("@uid", this.userId);
                            cmd.Parameters.AddWithValue("@rid", this.restaurantId);
                            cmd.Parameters.AddWithValue("@price", totalPrice);
                            orderId = (int)cmd.ExecuteScalar();
                        }

                        // 4. ย้ายของลง Order Items
                        foreach (DataRow row in dtItems.Rows)
                        {
                            string createOrderItem = "INSERT INTO order_items (order_id, item_id, quantity, price) VALUES (@oid, @iid, @qty, @price)";
                            using (var cmd = new NpgsqlCommand(createOrderItem, conn))
                            {
                                cmd.Parameters.AddWithValue("@oid", orderId);
                                cmd.Parameters.AddWithValue("@iid", row["item_id"]);
                                cmd.Parameters.AddWithValue("@qty", row["quantity"]);
                                cmd.Parameters.AddWithValue("@price", row["price"]);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        // 5. ล้างตะกร้า หรือ สร้างตะกร้าใหม่สำหรับครั้งหน้า
                        string newCart = "INSERT INTO carts (user_id) VALUES (@uid)";
                        using (var cmd = new NpgsqlCommand(newCart, conn))
                        {
                            cmd.Parameters.AddWithValue("@uid", this.userId);
                            cmd.ExecuteNonQuery();
                        }

                        trans.Commit();
                        MessageBox.Show($"สั่งซื้อสำเร็จ! เลขที่ใบสั่งซื้อของคุณคือ: {orderId}\nสถานะ: Pending (รอร้านค้ายืนยัน)");
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
