using System;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows.Forms;

namespace Delivery
{
    public partial class RestaurantForm : Form
    {
        private readonly int userId;
        private readonly HttpClient httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5000/api/") };

        public RestaurantForm(int userId)
        {
            InitializeComponent();
            this.userId = userId;

            Load += RestaurantForm_Load;
            button1.Click += ButtonOrders_Click;
            button2.Click += ButtonEdit_Click;
            buttonLogout.Click += BtnLogout_Click;
        }

        private async void RestaurantForm_Load(object? sender, EventArgs e)
        {
            try
            {
                var restaurant = await httpClient.GetFromJsonAsync<Delivery.Api.Models.RestaurantDto>(
                    $"restaurants/by-user/{userId}");

                if (restaurant != null)
                {
                    label1.Text = restaurant.Name;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButtonEdit_Click(object? sender, EventArgs e)
        {
            RestaurantEditForm editForm = new RestaurantEditForm(userId);
            editForm.ShowDialog(this);
        }

        private void ButtonOrders_Click(object? sender, EventArgs e)
        {
            RestaurantOrdersForm ordersForm = new RestaurantOrdersForm(userId);
            ordersForm.ShowDialog(this);
        }

        private void BtnLogout_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show("คุณต้องการออกจากระบบใช่หรือไม่?", "ยืนยันการออกจากระบบ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Login loginForm = new Login();
                loginForm.Show();
                Close();
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            httpClient.Dispose();
            base.OnFormClosed(e);
        }
    }
}
