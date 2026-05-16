using System;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows.Forms;

namespace Delivery
{
    public partial class Login : Form
    {
        private readonly HttpClient httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5000/api/") };

        public Login()
        {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtUserId.Text, out int userId))
                {
                    MessageBox.Show("Please enter number only");
                    return;
                }

                var payload = new Delivery.Api.Models.LoginRequest(userId);
                var response = await httpClient.PostAsJsonAsync("auth/login", payload);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("User ID not found");
                    return;
                }

                var login = await response.Content.ReadFromJsonAsync<Delivery.Api.Models.LoginResponse>();
                if (login == null)
                {
                    MessageBox.Show("Login failed");
                    return;
                }

                string role = login.Role;
                Form nextForm = null;

                if (role == "Customer")
                {
                    nextForm = new CustomerForm(userId);
                }
                else if (role == "Restaurant")
                {
                    nextForm = new RestaurantForm(userId);
                }
                else if (role == "Rider")
                {
                    nextForm = new RiderForm(userId);
                }
                else
                {
                    MessageBox.Show("Unknown role: " + role);
                    return;
                }

                MessageBox.Show("Login Success");

                nextForm.FormClosed += (s, args) =>
                {
                    if (!Application.OpenForms.OfType<Login>().Any())
                    {
                        Application.Exit();
                    }
                };

                nextForm.Show();
                Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtUserId_TextChanged(object sender, EventArgs e)
        {

        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            httpClient.Dispose();
            base.OnFormClosed(e);
        }
    }
}