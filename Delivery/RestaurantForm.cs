using System;
using System.Windows.Forms;
using Npgsql;

namespace Delivery
{
    public partial class RestaurantForm : Form
    {
        private readonly int userId;

        public RestaurantForm(int userId)
        {
            InitializeComponent();
            this.userId = userId;

            Load += RestaurantForm_Load;
            button1.Click += ButtonOrders_Click;
            button2.Click += ButtonEdit_Click;
        }

        private void RestaurantForm_Load(object? sender, EventArgs e)
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(Database.connectionString))
                {
                    conn.Open();

                    string query = "SELECT name FROM restaurants WHERE user_id = @user_id";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@user_id", userId);
                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            label1.Text = result.ToString();
                        }
                    }
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
    }

}
