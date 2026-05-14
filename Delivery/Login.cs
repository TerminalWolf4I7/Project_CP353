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
                if (!int.TryParse(txtUserId.Text, out int userId))
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

                        if (result == null)
                        {
                            MessageBox.Show("User ID not found");
                            return;
                        }

                        string role = result.ToString();

                        Form nextForm = null;

                        if (role == "Customer")
                            nextForm = new CustomerForm();
                        else if (role == "Restaurant")
                            nextForm = new RestaurantForm();
                        else if (role == "Rider")
                            nextForm = new RiderForm();
                        else
                        {
                            MessageBox.Show("Unknown role: " + role);
                            return;
                        }

                        MessageBox.Show("Login Success");

                        nextForm.FormClosed += (s, args) => Application.Exit();
                        nextForm.Show();
                        this.Hide();
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