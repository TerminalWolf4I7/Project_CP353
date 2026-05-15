using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Npgsql;

namespace Delivery
{
    public partial class CartForm : Form
    {
        private List<string> cartItems;
        private Form menuForm;
        private string restaurantName;
        private int userId;
        private FlowLayoutPanel itemPanel;
        private Label totalLabel;

        public CartForm(List<string> items, Form previousMenuForm, string restaurant, int userId)
        {
            InitializeComponent();

            cartItems = items;
            menuForm = previousMenuForm;
            restaurantName = restaurant;
            this.userId = userId;

            SetupUI();
            LoadCartItems();
        }

        private void SetupUI()
        {
            this.Text = "Cart";
            this.Size = new Size(900, 680);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            Panel mainPanel = new Panel();
            mainPanel.BackColor = Color.FromArgb(42, 107, 190);
            mainPanel.Size = new Size(850, 600);
            mainPanel.Location = new Point(20, 20);
            this.Controls.Add(mainPanel);

            Label title = new Label();
            title.Text = "ตะกร้าสินค้า";
            title.Font = new Font("Segoe UI", 24, FontStyle.Bold);
            title.ForeColor = Color.White;
            title.TextAlign = ContentAlignment.MiddleCenter;
            title.Size = new Size(mainPanel.Width, 60);
            title.Location = new Point(0, 25);
            mainPanel.Controls.Add(title);

            itemPanel = new FlowLayoutPanel();
            itemPanel.Size = new Size(650, 330);
            itemPanel.Location = new Point(100, 105);
            itemPanel.BackColor = Color.White;
            itemPanel.AutoScroll = true;
            itemPanel.FlowDirection = FlowDirection.TopDown;
            itemPanel.WrapContents = false;
            itemPanel.Padding = new Padding(10);
            mainPanel.Controls.Add(itemPanel);

            totalLabel = new Label();
            totalLabel.Text = "รวมทั้งหมด: 0 บาท";
            totalLabel.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            totalLabel.ForeColor = Color.White;
            totalLabel.TextAlign = ContentAlignment.MiddleCenter;
            totalLabel.Size = new Size(mainPanel.Width, 45);
            totalLabel.Location = new Point(0, 455);
            mainPanel.Controls.Add(totalLabel);

            Button backButton = CreateBottomButton("Back", 100, 520);
            backButton.Click += BackButton_Click;
            mainPanel.Controls.Add(backButton);

            Button clearButton = CreateBottomButton("ล้างตะกร้า", 345, 520);
            clearButton.Click += ClearButton_Click;
            mainPanel.Controls.Add(clearButton);

            Button orderButton = CreateBottomButton("สั่งซื้อ", 600, 520);
            orderButton.Click += OrderButton_Click;
            mainPanel.Controls.Add(orderButton);
        }

        private Button CreateBottomButton(string text, int x, int y)
        {
            Button button = new Button();
            button.Text = text;
            button.Size = new Size(150, 50);
            button.Location = new Point(x, y);
            button.BackColor = Color.White;
            button.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            return button;
        }

        private void LoadCartItems()
        {
            itemPanel.Controls.Clear();

            if (cartItems.Count == 0)
            {
                Label emptyLabel = new Label();
                emptyLabel.Text = "ยังไม่มีสินค้าในตะกร้า";
                emptyLabel.Font = new Font("Segoe UI", 16, FontStyle.Bold);
                emptyLabel.TextAlign = ContentAlignment.MiddleCenter;
                emptyLabel.Size = new Size(610, 280);
                itemPanel.Controls.Add(emptyLabel);

                totalLabel.Text = "รวมทั้งหมด: 0 บาท";
                return;
            }

            var groupedItems = cartItems
                .GroupBy(item => item)
                .Select(group => new
                {
                    Name = group.Key,
                    Quantity = group.Count(),
                    Price = GetPrice(group.Key)
                })
                .ToList();

            int total = 0;

            foreach (var item in groupedItems)
            {
                total += item.Price * item.Quantity;

                Panel row = new Panel();
                row.Size = new Size(610, 65);
                row.BackColor = Color.FromArgb(245, 245, 245);
                row.Margin = new Padding(5, 5, 5, 8);

                Label nameLabel = new Label();
                nameLabel.Text = item.Name;
                nameLabel.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                nameLabel.Size = new Size(250, 45);
                nameLabel.Location = new Point(15, 12);
                nameLabel.TextAlign = ContentAlignment.MiddleLeft;
                row.Controls.Add(nameLabel);

                Label qtyLabel = new Label();
                qtyLabel.Text = "x" + item.Quantity;
                qtyLabel.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                qtyLabel.Size = new Size(60, 45);
                qtyLabel.Location = new Point(280, 12);
                qtyLabel.TextAlign = ContentAlignment.MiddleCenter;
                row.Controls.Add(qtyLabel);

                Label priceLabel = new Label();
                priceLabel.Text = (item.Price * item.Quantity) + " บาท";
                priceLabel.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                priceLabel.ForeColor = Color.FromArgb(42, 107, 190);
                priceLabel.Size = new Size(120, 45);
                priceLabel.Location = new Point(365, 12);
                priceLabel.TextAlign = ContentAlignment.MiddleCenter;
                row.Controls.Add(priceLabel);

                Button removeButton = new Button();
                removeButton.Text = "ลบ";
                removeButton.Size = new Size(70, 35);
                removeButton.Location = new Point(515, 15);
                removeButton.BackColor = Color.White;
                removeButton.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                removeButton.FlatStyle = FlatStyle.Flat;
                removeButton.Tag = item.Name;
                removeButton.Click += RemoveButton_Click;
                row.Controls.Add(removeButton);

                itemPanel.Controls.Add(row);
            }

            totalLabel.Text = "รวมทั้งหมด: " + total + " บาท";
        }

        private int GetPrice(string itemText)
        {
            string[] parts = itemText.Split('-');

            if (parts.Length < 2)
                return 0;

            string priceText = parts[1]
                .Replace("บาท", "")
                .Trim();

            int.TryParse(priceText, out int price);
            return price;
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string itemName = btn.Tag.ToString();

            cartItems.Remove(itemName);
            LoadCartItems();
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            cartItems.Clear();
            LoadCartItems();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            menuForm.Show();
            this.Close();
        }

        private void OrderButton_Click(object sender, EventArgs e)
        {
            if (cartItems.Count == 0)
            {
                MessageBox.Show("กรุณาเลือกสินค้าก่อนสั่งซื้อ");
                return;
            }

            try
            {
                using (var conn = new NpgsqlConnection(Database.connectionString))
                {
                    conn.Open();
                    // 1. หา Cart ID ปัจจุบัน
                    string getCart = "SELECT cart_id FROM carts WHERE user_id = @uid ORDER BY created_at DESC LIMIT 1";
                    int cartId = 0;
                    using (var cmd = new NpgsqlCommand(getCart, conn))
                    {
                        cmd.Parameters.AddWithValue("@uid", this.userId);
                        var result = cmd.ExecuteScalar();
                        if (result != null) cartId = (int)result;
                    }

                    if (cartId > 0)
                    {
                        // 2. ลบของในตะกร้าใน DB ทิ้ง
                        string deleteItems = "DELETE FROM cart_items WHERE cart_id = @cid";
                        using (var cmd = new NpgsqlCommand(deleteItems, conn))
                        {
                            cmd.Parameters.AddWithValue("@cid", cartId);
                            cmd.ExecuteNonQuery();
                        }

                        // 3. ลบตัวตะกร้าเก่าทิ้งไปด้วยเลย (จะได้ไม่ซ้ำซ้อน)
                        string deleteCart = "DELETE FROM carts WHERE cart_id = @cid";
                        using (var cmd = new NpgsqlCommand(deleteCart, conn))
                        {
                            cmd.Parameters.AddWithValue("@cid", cartId);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
            }

            MessageBox.Show("สั่งซื้อสำเร็จ!");
            cartItems.Clear();

            OrderStatusForm statusForm = new OrderStatusForm(restaurantName, userId);
            statusForm.Show();

            menuForm.Close();
            this.Close();
        }
    }
}