using System;
using System.Data;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Delivery
{
    public partial class RiderForm : Form
    {
        private readonly int _userId;

        private System.Windows.Forms.Timer refreshTimer;
        private readonly HttpClient httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5000/api/") };

        public RiderForm(int userId)
        {
            InitializeComponent();

            _userId = userId;

            Load += RiderForm_Load;

            dataGridOrders.SelectionChanged +=
                DataGridOrders_SelectionChanged;

            buttonViewDetails.Click +=
                ButtonViewDetails_Click;

            buttonAcceptOrder.Click +=
                ButtonAcceptOrder_Click;

            buttonRiderOrder.Click +=
                ButtonRiderOrder_Click;

            buttonLogout.Click += 
                BtnLogout_Click;


            buttonViewDetails.Enabled = false;
            buttonAcceptOrder.Enabled = false;
            buttonRiderOrder.Enabled = false;

            refreshTimer = new System.Windows.Forms.Timer();

            refreshTimer.Interval = 30000;

            refreshTimer.Tick += RefreshTimer_Tick;

            refreshTimer.Start();


        }



        private async void RiderForm_Load(
            object sender,
            EventArgs e)
        {
            await LoadOrdersAsync();
            await CheckCurrentOrderAsync();
        }


        private void DataGridOrders_SelectionChanged(
            object sender,
            EventArgs e)
        {
            bool hasSelection =
                dataGridOrders.SelectedRows.Count > 0;

            buttonViewDetails.Enabled =
                hasSelection;

            buttonAcceptOrder.Enabled =
                hasSelection;
        }


        private async Task LoadOrdersAsync()
        {
            try
            {
                var orders = await httpClient.GetFromJsonAsync<List<Delivery.Api.Models.OrderDto>>(
                    "orders?status=Waiting%20for%20rider");

                DataTable dt = new DataTable();
                dt.Columns.Add("order_id", typeof(int));
                dt.Columns.Add("restaurant_id", typeof(int));
                dt.Columns.Add("status", typeof(string));
                dt.Columns.Add("rider_id", typeof(int));

                if (orders != null)
                {
                    foreach (var order in orders.OrderBy(o => o.OrderId))
                    {
                        dt.Rows.Add(order.OrderId, order.RestaurantId, order.Status, order.RiderId ?? 0);
                    }
                }

                dataGridOrders.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message);
            }
        }



        private async Task CheckCurrentOrderAsync()
        {
            var response = await httpClient.GetAsync($"riders/{_userId}/current-order");
            buttonRiderOrder.Enabled = response.IsSuccessStatusCode;
        }



        private async void ButtonViewDetails_Click(
            object sender,
            EventArgs e)
        {
            int orderId =
                Convert.ToInt32(
                    dataGridOrders.SelectedRows[0]
                    .Cells["order_id"]
                    .Value);

            try
            {
                var details = await httpClient.GetFromJsonAsync<Delivery.Api.Models.OrderDetailDto>(
                    $"orders/{orderId}/details");

                if (details == null)
                {
                    MessageBox.Show("Order not found.");
                    return;
                }

                var items = await httpClient.GetFromJsonAsync<List<Delivery.Api.Models.OrderItemDto>>(
                    $"orders/{orderId}");

                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"Order ID : {details.OrderId}");
                sb.AppendLine($"Customer : {details.CustomerName}");
                sb.AppendLine($"Restaurant : {details.RestaurantId}");
                sb.AppendLine($"Status : {details.Status}");
                sb.AppendLine($"Total : {details.TotalPrice}");
                sb.AppendLine();
                sb.AppendLine("Items:");

                if (items != null)
                {
                    foreach (var item in items)
                    {
                        sb.AppendLine($"{item.Name} x {item.Quantity} ({item.Price})");
                    }
                }

                MessageBox.Show(sb.ToString(), $"Order {orderId}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message);
            }
        }



        private async void ButtonAcceptOrder_Click(
            object sender,
            EventArgs e)
        {
            int orderId =
                Convert.ToInt32(
                    dataGridOrders.SelectedRows[0]
                    .Cells["order_id"]
                    .Value);

            try
            {
                var response = await httpClient.PatchAsync(
                    $"orders/{orderId}/accept-rider/{_userId}",
                    null);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Order accepted.");
                }
                else
                {
                    MessageBox.Show("Order นี้ถูกรับไปแล้ว");
                }

                await LoadOrdersAsync();
                await CheckCurrentOrderAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message);
            }
        }

        private void BtnLogout_Click(
    object sender,
    EventArgs e)
        {
            var result =
                MessageBox.Show(
                    "คุณต้องการออกจากระบบใช่หรือไม่?",
                    "ยืนยันการออกจากระบบ",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                refreshTimer.Stop();

                Login loginForm =
                    new Login();

                loginForm.Show();

                Close();
            }
        }

        //รีเฟส
        private void ButtonRiderOrder_Click(
            object sender,
            EventArgs e)
        {
            RiderOrderForm form =
                new RiderOrderForm(
                    _userId);

            form.Show();
        }
        private void RefreshTimer_Tick(
    object sender,
    EventArgs e)
        {
            _ = LoadOrdersAsync();
            _ = CheckCurrentOrderAsync();
        }
        protected override void OnFormClosed(
    FormClosedEventArgs e)
        {
            refreshTimer.Stop();
            httpClient.Dispose();

            base.OnFormClosed(e);
        }
    }
}