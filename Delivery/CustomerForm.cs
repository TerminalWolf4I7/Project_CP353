<<<<<<< HEAD
using System.Linq;
using System.Drawing;
using Npgsql;
using System.Data;
=======
﻿using System;
using System.Drawing;
>>>>>>> 3b511a8f186b3fd1172301f2fbdc94c361aa531c
using System.Windows.Forms;

namespace Delivery
{
    public partial class CustomerForm : Form
    {
        private int userId;

        public CustomerForm(int userId)
        {
            InitializeComponent();
<<<<<<< HEAD
            this.userId = userId;
            this.Load += CustomerForm_Load;
        }

        private void CustomerForm_Load(object sender, EventArgs e)
        {
            LoadRestaurants();
        }

        private void LoadRestaurants()
        {
            try
            {
                flpRestaurants.Controls.Clear();
                using (NpgsqlConnection conn = new NpgsqlConnection(Database.connectionString))
                {
                    conn.Open();
                    string query = "SELECT restaurant_id, name, address, phone FROM restaurants";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                string name = reader.GetString(1);
                                string address = reader.GetString(2);

                                // สร้าง Card Panel
                                Panel card = new Panel();
                                card.Size = new Size(350, 200);
                                card.BackColor = Color.White;
                                card.Margin = new Padding(15);
                                card.BorderStyle = BorderStyle.FixedSingle;
                                card.Cursor = Cursors.Hand;
                                card.Tag = id; // เก็บ ID ไว้ใน Tag

                                // ชื่อร้าน
                                Label lblName = new Label();
                                lblName.Text = name;
                                lblName.Font = new Font("Segoe UI", 14, FontStyle.Bold);
                                lblName.Location = new Point(20, 20);
                                lblName.AutoSize = true;
                                lblName.ForeColor = Color.FromArgb(44, 62, 80);
                                card.Controls.Add(lblName);

                                // ที่อยู่
                                Label lblAddr = new Label();
                                lblAddr.Text = "📍 " + address;
                                lblAddr.Font = new Font("Segoe UI", 10);
                                lblAddr.Location = new Point(20, 60);
                                lblAddr.Size = new Size(310, 80);
                                card.Controls.Add(lblAddr);

                                // ปุ่ม "ดูเมนู" แบบหลอกๆ ให้ดูสวย
                                Label btnFake = new Label();
                                btnFake.Text = "ดูเมนูอาหาร >";
                                btnFake.ForeColor = Color.FromArgb(46, 204, 113);
                                btnFake.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                                btnFake.Location = new Point(230, 160);
                                btnFake.AutoSize = true;
                                card.Controls.Add(btnFake);

                                // เพิ่ม Click Event ให้ทั้ง Card
                                card.Click += RestaurantCard_Click;
                                foreach (Control ctrl in card.Controls)
                                {
                                    ctrl.Click += (s, e) => RestaurantCard_Click(card, e);
                                }

                                flpRestaurants.Controls.Add(card);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading restaurants: " + ex.Message);
            }
        }

        private void RestaurantCard_Click(object? sender, EventArgs e)
        {
            if (sender is Panel card)
            {
                int restaurantId = (int)card.Tag;
                string restaurantName = card.Controls.OfType<Label>().First().Text;

                MenuForm menuForm = new MenuForm(restaurantId, this.userId);
                menuForm.Text = "เมนูของร้าน: " + restaurantName;
                menuForm.ShowDialog();
            }
=======
            this.Controls.Clear();
            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "Customer";
            this.Size = new Size(900, 580);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(42, 107, 190);

            Button btnRestaurant1 = new Button();
            btnRestaurant1.Text = "ร้าน 1";
            btnRestaurant1.Size = new Size(470, 150);
            btnRestaurant1.Location = new Point(210, 100);
            btnRestaurant1.BackColor = Color.White;
            btnRestaurant1.Font = new Font("Segoe UI", 20, FontStyle.Regular);
            btnRestaurant1.FlatStyle = FlatStyle.Flat;
            btnRestaurant1.FlatAppearance.BorderSize = 0;
            btnRestaurant1.Click += btnRestaurant1_Click;
            this.Controls.Add(btnRestaurant1);

            Button btnRestaurant2 = new Button();
            btnRestaurant2.Text = "ร้าน 2";
            btnRestaurant2.Size = new Size(470, 150);
            btnRestaurant2.Location = new Point(210, 310);
            btnRestaurant2.BackColor = Color.White;
            btnRestaurant2.Font = new Font("Segoe UI", 20, FontStyle.Regular);
            btnRestaurant2.FlatStyle = FlatStyle.Flat;
            btnRestaurant2.FlatAppearance.BorderSize = 0;
            btnRestaurant2.Click += btnRestaurant2_Click;
            this.Controls.Add(btnRestaurant2);

            Button logoutButton = new Button();
            logoutButton.Text = "Back";
            logoutButton.Size = new Size(120, 45);
            logoutButton.Location = new Point(30, 30);
            logoutButton.BackColor = Color.White;
            logoutButton.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            logoutButton.FlatStyle = FlatStyle.Flat;
            logoutButton.FlatAppearance.BorderSize = 0;
            logoutButton.Click += LogoutButton_Click;
            this.Controls.Add(logoutButton);
        }

        private void btnRestaurant1_Click(object sender, EventArgs e)
        {
            MenuForm menuForm = new MenuForm(1, "ร้าน 1");
            menuForm.Show();
            this.Hide();
        }

        private void btnRestaurant2_Click(object sender, EventArgs e)
        {
            MenuForm menuForm = new MenuForm(2, "ร้าน 2");
            menuForm.Show();
            this.Hide();
        }

        private void LogoutButton_Click(object sender, EventArgs e)
        {
            Login loginForm = new Login();
            loginForm.Show();
            this.Hide();
>>>>>>> 3b511a8f186b3fd1172301f2fbdc94c361aa531c
        }
    }
}