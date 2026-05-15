using System;
using System.Windows.Forms;
using Npgsql;

namespace Delivery
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                int userId;

                if (!int.TryParse(txtUserId.Text, out userId))
                {
                    MessageBox.Show("Please enter number only");
                    return;
                }

                using (NpgsqlConnection conn = new NpgsqlConnection(Database.connectionString))
                {
                    conn.Open();

                    string query = "SELECT role FROM users WHERE user_id = @id";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", userId);

                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            string role = result.ToString();

                            MessageBox.Show("Login Success");

                            if (role == "Customer")
                            {
                                CustomerForm customer = new CustomerForm(userId);
                                customer.Show();
                            }
                            else if (role == "Restaurant")
                            {
                                RestaurantForm restaurant = new RestaurantForm(userId);
                                restaurant.Show();
                            }
                            else if (role == "Rider")
                            {
                                RiderForm rider = new RiderForm();
                                rider.Show();
                            }
                            else
                            {
                                MessageBox.Show("Unknown role: " + role);
                                return;
                            }

                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("User ID not found");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}