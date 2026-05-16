using System;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Delivery.Client.Models;

namespace Delivery.Client
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

        private async void RestaurantOrdersForm_Load(object? sender, EventArgs e)
        {
            try
            {
                var restaurant = await RestUtil.GetAsync<RestaurantDto>(
                    $"restaurants/by-user/{userId}");

                if (restaurant == null)
                {
                    MessageBox.Show("Restaurant not found for this user.");
                    Close();
                    return;
                }

                restaurantId = restaurant.RestaurantId;
                await LoadOrdersAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async Task LoadOrdersAsync()
        {
            try
            {
                var orders = await RestUtil.GetAsync<List<OrderDto>>(
                    $"orders?restaurantId={restaurantId}");

                ordersTable = new DataTable();
                ordersTable.Columns.Add("order_id", typeof(int));
                ordersTable.Columns.Add("status", typeof(string));

                if (orders != null)
                {
                    foreach (var order in orders
                        .Where(o => o.Status == "Pending" || o.Status == "Cooking")
                        .OrderBy(o => o.OrderId))
                    {
                        ordersTable.Rows.Add(order.OrderId, order.Status);
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
            _ = UpdateOrderStatusAsync("Pending", "Cooking");
        }

        private void ButtonDecline_Click(object? sender, EventArgs e)
        {
            _ = DeleteOrderAsync("Pending");
        }

        private void ButtonFinishCooking_Click(object? sender, EventArgs e)
        {
            _ = UpdateOrderStatusAsync("Cooking", "Waiting for rider");
        }

        private async Task DeleteOrderAsync(string expectedStatus)
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
                await LoadOrdersAsync();
                return;
            }

            try
            {
                var response = await RestUtil.DeleteAsync($"orders/{Convert.ToInt32(orderIdValue)}");
                response.EnsureSuccessStatusCode();

                await LoadOrdersAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async Task UpdateOrderStatusAsync(string expectedStatus, string newStatus)
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
                await LoadOrdersAsync();
                return;
            }

            try
            {
                var payload = new UpdateStatusRequest(newStatus);
                var response = await RestUtil.PatchAsync(
                    $"orders/{Convert.ToInt32(orderIdValue)}/status",
                    payload);
                response.EnsureSuccessStatusCode();

                await LoadOrdersAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
