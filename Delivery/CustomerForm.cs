using System;
using System.Drawing;
using System.Windows.Forms;

namespace Delivery
{
    public partial class CustomerForm : Form
    {
        public CustomerForm()
        {
            InitializeComponent();
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
        }
    }
}