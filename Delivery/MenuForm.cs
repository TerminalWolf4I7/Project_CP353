<<<<<<< HEAD
using System;
using System.Data;
=======
﻿using System;
using System.Collections.Generic;
using System.Drawing;
>>>>>>> 3b511a8f186b3fd1172301f2fbdc94c361aa531c
using System.Windows.Forms;
using Npgsql;

namespace Delivery
{
    public partial class MenuForm : Form
    {
        private int restaurantId;
<<<<<<< HEAD
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
=======
        private string restaurantName;

        public static List<string> CartItems = new List<string>();

        public MenuForm(int id, string name)
        {
            InitializeComponent();
            restaurantId = id;
            restaurantName = name;
            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "Menu - " + restaurantName;
            this.Size = new Size(980, 720);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            Panel mainPanel = new Panel();
            mainPanel.BackColor = Color.FromArgb(42, 107, 190);
            mainPanel.Size = new Size(930, 640);
            mainPanel.Location = new Point(20, 20);
            this.Controls.Add(mainPanel);

            Label title = new Label();
            title.Text = "เมนูอาหาร - " + restaurantName;
            title.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            title.ForeColor = Color.White;
            title.TextAlign = ContentAlignment.MiddleCenter;
            title.Size = new Size(mainPanel.Width, 60);
            title.Location = new Point(0, 15);
            mainPanel.Controls.Add(title);

            List<string[]> menus = new List<string[]>();

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(Database.connectionString))
                {
                    conn.Open();

                    string query = @"
                        SELECT name, price, description
                        FROM menu_items
                        WHERE restaurant_id = @restaurantId
                        AND is_available = true
                        ORDER BY item_id
                    ";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@restaurantId", restaurantId);

>>>>>>> 3b511a8f186b3fd1172301f2fbdc94c361aa531c
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
<<<<<<< HEAD
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
=======
                                menus.Add(new string[]
                                {
                                    reader["name"].ToString(),
                                    reader["price"].ToString(),
                                    reader["description"].ToString()
                                });
>>>>>>> 3b511a8f186b3fd1172301f2fbdc94c361aa531c
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
<<<<<<< HEAD
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
=======
                MessageBox.Show(ex.Message);
            }

            int cardWidth = 245;
            int cardHeight = 125;
            int startX = 45;
            int startY = 95;
            int gapX = 45;
            int gapY = 25;

            for (int i = 0; i < menus.Count; i++)
            {
                Panel card = new Panel();
                card.Size = new Size(cardWidth, cardHeight);
                card.BackColor = Color.White;
                card.Cursor = Cursors.Hand;

                int row = i / 3;
                int col = i % 3;

                card.Location = new Point(
                    startX + col * (cardWidth + gapX),
                    startY + row * (cardHeight + gapY)
                );

                string name = menus[i][0];
                string price = menus[i][1];
                string desc = menus[i][2];

                string itemText = $"{name} - {price} บาท";

                Label nameLabel = new Label();
                nameLabel.Text = name;
                nameLabel.Font = new Font("Segoe UI", 15, FontStyle.Bold);
                nameLabel.TextAlign = ContentAlignment.MiddleCenter;
                nameLabel.Size = new Size(cardWidth, 45);
                nameLabel.Location = new Point(0, 8);

                Label priceLabel = new Label();
                priceLabel.Text = price + " บาท";
                priceLabel.Font = new Font("Segoe UI", 13, FontStyle.Bold);
                priceLabel.ForeColor = Color.FromArgb(42, 107, 190);
                priceLabel.TextAlign = ContentAlignment.MiddleCenter;
                priceLabel.Size = new Size(cardWidth, 30);
                priceLabel.Location = new Point(0, 55);

                Label descLabel = new Label();
                descLabel.Text = desc;
                descLabel.Font = new Font("Segoe UI", 9, FontStyle.Regular);
                descLabel.TextAlign = ContentAlignment.MiddleCenter;
                descLabel.Size = new Size(cardWidth - 20, 35);
                descLabel.Location = new Point(10, 88);

                card.Tag = itemText;
                nameLabel.Tag = itemText;
                priceLabel.Tag = itemText;
                descLabel.Tag = itemText;

                card.Click += MenuCard_Click;
                nameLabel.Click += MenuCard_Click;
                priceLabel.Click += MenuCard_Click;
                descLabel.Click += MenuCard_Click;

                card.Controls.Add(nameLabel);
                card.Controls.Add(priceLabel);
                card.Controls.Add(descLabel);

                mainPanel.Controls.Add(card);
            }

            Button backButton = new Button();
            backButton.Text = "Back";
            backButton.Size = new Size(140, 55);
            backButton.Location = new Point(45, 565);
            backButton.BackColor = Color.White;
            backButton.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            backButton.FlatStyle = FlatStyle.Flat;
            backButton.FlatAppearance.BorderSize = 0;
            backButton.Click += BackButton_Click;
            mainPanel.Controls.Add(backButton);

            Button cartButton = new Button();
            cartButton.Text = "ตะกร้า";
            cartButton.Size = new Size(190, 55);
            cartButton.Location = new Point(690, 565);
            cartButton.BackColor = Color.White;
            cartButton.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            cartButton.FlatStyle = FlatStyle.Flat;
            cartButton.FlatAppearance.BorderSize = 0;
            cartButton.Click += CartButton_Click;
            mainPanel.Controls.Add(cartButton);
        }

        private void MenuCard_Click(object sender, EventArgs e)
        {
            Control control = sender as Control;
            string item = control.Tag.ToString();

            CartItems.Add(item);

            MessageBox.Show("เพิ่มลงตะกร้าแล้ว\n" + item);
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            CustomerForm customerForm = new CustomerForm();
            customerForm.Show();
            this.Close();
        }

        private void CartButton_Click(object sender, EventArgs e)
        {
            CartForm cartForm = new CartForm(CartItems, this, restaurantName);
            cartForm.Show();
            this.Hide();
        }
    }
}
>>>>>>> 3b511a8f186b3fd1172301f2fbdc94c361aa531c
