using System;
using System.Data;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Delivery
{
    public partial class RiderOrderForm : Form
    {
        private readonly int _userId;
        private readonly HttpClient httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5000/api/") };

        public RiderOrderForm(int userId)
        {
            InitializeComponent();

            _userId = userId;

            Load +=
                RiderOrderForm_Load;

            buttonComplete.Click +=
                ButtonComplete_Click;
            buttonBack.Click +=
                ButtonBack_Click;
        }


        private async void RiderOrderForm_Load(
            object sender,
            EventArgs e)
        {
            await LoadMyOrderAsync();
        }

        private async Task LoadMyOrderAsync()
        {
            try
            {
                var response = await httpClient.GetAsync($"riders/{_userId}/current-order");
                DataTable dt = new DataTable();
                dt.Columns.Add("order_id", typeof(int));
                dt.Columns.Add("user_id", typeof(int));
                dt.Columns.Add("name", typeof(string));
                dt.Columns.Add("restaurant_id", typeof(int));
                dt.Columns.Add("status", typeof(string));

                if (response.IsSuccessStatusCode)
                {
                    var order = await response.Content.ReadFromJsonAsync<Delivery.Api.Models.RiderCurrentOrderDto>();
                    if (order != null)
                    {
                        dt.Rows.Add(order.OrderId, order.UserId, order.CustomerName, order.RestaurantId, order.Status);
                    }
                }

                dataGridOrder.DataSource = dt;
                buttonComplete.Enabled = dt.Rows.Count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message);
            }
        }


        private async void ButtonComplete_Click(
            object sender,
            EventArgs e)
        {
            if (dataGridOrder.Rows.Count == 0)
                return;


            int orderId =
                Convert.ToInt32(
                    dataGridOrder.Rows[0]
                    .Cells["order_id"]
                    .Value);


            try
            {
                var response = await httpClient.PostAsync($"riders/{_userId}/complete/{orderId}", null);
                response.EnsureSuccessStatusCode();

                MessageBox.Show("Delivery completed.");
                await LoadMyOrderAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButtonBack_Click(
    object sender,
    EventArgs e)
        {
            Close();
        }

        private void buttonComplete_Click_1(object sender, EventArgs e)
        {

        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            httpClient.Dispose();
            base.OnFormClosed(e);
        }
    }
}