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
                using (NpgsqlConnection conn =
                    new NpgsqlConnection(Database.connectionString))
                {
                    conn.Open();

                    string query =
                        "SELECT Role FROM Users WHERE UserNumber=@id";

                    using (NpgsqlCommand cmd =
                        new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", txtUserId.Text);

                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            string role = result.ToString();

                            MessageBox.Show("Login Success");

                            if (role == "Customer")
                            {
                                CustomerForm customer =
                                    new CustomerForm();

                                customer.Show();
                            }
                            else if (role == "Restaurant")
                            {
                                RestaurantForm restaurant =
                                    new RestaurantForm();

                                restaurant.Show();
                            }
                            else if (role == "Rider")
                            {
                                RiderForm rider =
                                    new RiderForm();

                                rider.Show();
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