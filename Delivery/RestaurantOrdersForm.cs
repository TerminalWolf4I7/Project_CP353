using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace Delivery
{
    public partial class RestaurantOrdersForm : Form
    {
        private readonly int userId;
        private int restaurantId;
        private DataTable ordersTable = new DataTable();

        public RestaurantOrdersForm(int userId)
        {
            InitializeComponent();
            this.userId = userId;

            Load += RestaurantOrdersForm_Load;
            dataGridOrders.SelectionChanged += DataGridOrders_SelectionChanged;
            buttonAccept.Click += ButtonAccept_Click;
            buttonDecline.Click += ButtonDecline_Click;
            buttonFinishCooking.Click += ButtonFinishCooking_Click;
        }

        private void RestaurantOrdersForm_Load(object? sender, EventArgs e)
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(Database.connectionString))
                {
                    conn.Open();

                    string restaurantQuery = "SELECT restaurant_id FROM restaurants WHERE user_id = @user_id";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(restaurantQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@user_id", userId);
                        object? result = cmd.ExecuteScalar();

                        if (result == null || result == DBNull.Value)
                        {
                            MessageBox.Show("Restaurant not found for this user.");
                            Close();
                            return;
                        }

                        restaurantId = Convert.ToInt32(result);
                    }
                }

                LoadOrders();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadOrders()
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(Database.connectionString))
                {
                    conn.Open();

                    string ordersQuery = "SELECT order_id, status FROM orders WHERE restaurant_id = @restaurant_id AND (status = 'Pending' OR status = 'Cooking') ORDER BY order_id";

                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(ordersQuery, conn))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@restaurant_id", restaurantId);
                        ordersTable = new DataTable();
                        adapter.Fill(ordersTable);
                    }
                }

                dataGridOrders.DataSource = ordersTable;
                UpdateButtonVisibility();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DataGridOrders_SelectionChanged(object? sender, EventArgs e)
        {
            UpdateButtonVisibility();
        }

        private void UpdateButtonVisibility()
        {
            buttonAccept.Visible = false;
            buttonDecline.Visible = false;
            buttonFinishCooking.Visible = false;

            if (dataGridOrders.SelectedRows.Count == 0)
            {
                return;
            }

            DataGridViewRow row = dataGridOrders.SelectedRows[0];
            string status = row.Cells["status"].Value?.ToString() ?? string.Empty;

            if (status == "Pending")
            {
                buttonAccept.Visible = true;
                buttonDecline.Visible = true;
            }
            else if (status == "Cooking")
            {
                buttonFinishCooking.Visible = true;
            }
        }

        private void ButtonAccept_Click(object? sender, EventArgs e)
        {
            UpdateOrderStatus("Pending", "Cooking");
        }

        private void ButtonDecline_Click(object? sender, EventArgs e)
        {
            DeleteOrder("Pending");
        }

        private void ButtonFinishCooking_Click(object? sender, EventArgs e)
        {
            UpdateOrderStatus("Cooking", "Waiting for rider");
        }

        private void DeleteOrder(string expectedStatus)
        {
            if (dataGridOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select an order.");
                return;
            }

            DataGridViewRow row = dataGridOrders.SelectedRows[0];
            object? orderIdValue = row.Cells["order_id"].Value;
            string currentStatus = row.Cells["status"].Value?.ToString() ?? string.Empty;

            if (orderIdValue == null || orderIdValue == DBNull.Value)
            {
                MessageBox.Show("Order not found.");
                return;
            }

            if (!string.Equals(currentStatus, expectedStatus, StringComparison.OrdinalIgnoreCase))
            {
                LoadOrders();
                return;
            }

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(Database.connectionString))
                {
                    conn.Open();

                    using (var trans = conn.BeginTransaction())
                    {
                        string deleteItemsQuery = "DELETE FROM order_items WHERE order_id = @order_id";

                        using (NpgsqlCommand cmd = new NpgsqlCommand(deleteItemsQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@order_id", Convert.ToInt32(orderIdValue));
                            cmd.ExecuteNonQuery();
                        }

                        string deleteOrderQuery = "DELETE FROM orders WHERE order_id = @order_id AND status = @current_status";

                        using (NpgsqlCommand cmd = new NpgsqlCommand(deleteOrderQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@order_id", Convert.ToInt32(orderIdValue));
                            cmd.Parameters.AddWithValue("@current_status", expectedStatus);

                            int affectedRows = cmd.ExecuteNonQuery();

                            if (affectedRows == 0)
                            {
                                trans.Rollback();
                                MessageBox.Show("No orders were deleted.");
                                return;
                            }
                        }

                        trans.Commit();
                    }
                }

                LoadOrders();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateOrderStatus(string expectedStatus, string newStatus)
        {
            if (dataGridOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select an order.");
                return;
            }

            DataGridViewRow row = dataGridOrders.SelectedRows[0];
            object? orderIdValue = row.Cells["order_id"].Value;
            string currentStatus = row.Cells["status"].Value?.ToString() ?? string.Empty;

            if (orderIdValue == null || orderIdValue == DBNull.Value)
            {
                MessageBox.Show("Order not found.");
                return;
            }

            if (!string.Equals(currentStatus, expectedStatus, StringComparison.OrdinalIgnoreCase))
            {
                LoadOrders();
                return;
            }

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(Database.connectionString))
                {
                    conn.Open();

                    string updateQuery = "UPDATE orders SET status = @new_status WHERE order_id = @order_id AND status = @current_status";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@new_status", newStatus);
                        cmd.Parameters.AddWithValue("@order_id", Convert.ToInt32(orderIdValue));
                        cmd.Parameters.AddWithValue("@current_status", expectedStatus);

                        int affectedRows = cmd.ExecuteNonQuery();

                        if (affectedRows == 0)
                        {
                            MessageBox.Show("No orders were updated.");
                        }
                    }
                }

                LoadOrders();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
