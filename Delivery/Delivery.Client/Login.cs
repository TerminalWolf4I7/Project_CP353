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

				var payload = new LoginRequest(userId);
				var response = await RestUtil.PostResponseAsync("auth/login", payload);

				if (!response.IsSuccessStatusCode)
				{
					MessageBox.Show("User ID not found");
					return;
				}

				var login = await RestUtil.ReadAsAsync<LoginResponse>(response);
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

		private void lblSubtitle_Click(object sender, EventArgs e)
		{

		}
	}
}
