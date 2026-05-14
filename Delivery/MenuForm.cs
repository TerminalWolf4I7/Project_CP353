using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Npgsql;

namespace Delivery
{
    public partial class MenuForm : Form
    {
        private int restaurantId;
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

                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                menus.Add(new string[]
                                {
                                    reader["name"].ToString(),
                                    reader["price"].ToString(),
                                    reader["description"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
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