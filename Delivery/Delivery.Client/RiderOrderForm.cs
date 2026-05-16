using System;
using System.Data;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Delivery.Client.Models;

namespace Delivery.Client
{
    public partial class RiderOrderForm : Form
    {
        private readonly int _userId;

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
                var response = await RestUtil.GetResponseAsync($"riders/{_userId}/current-order");
                DataTable dt = new DataTable();
                dt.Columns.Add("order_id", typeof(int));
                dt.Columns.Add("user_id", typeof(int));
                dt.Columns.Add("name", typeof(string));
                dt.Columns.Add("restaurant_id", typeof(int));
                dt.Columns.Add("status", typeof(string));

                if (response.IsSuccessStatusCode)
                {
                    // API ส่งกลับมาเป็น list ของ order ทั้งหมดที่ rider รับอยู่
                    var orders = await RestUtil.ReadAsAsync<List<RiderCurrentOrderDto>>(response);
                    if (orders != null)
                    {
                        foreach (var order in orders)
                        {
                            dt.Rows.Add(order.OrderId, order.UserId, order.CustomerName, order.RestaurantId, order.Status);
                        }
                    }
                }

                dataGridOrder.DataSource = dt;
                dataGridOrder.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridOrder.MultiSelect = false;
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
            if (dataGridOrder.SelectedRows.Count == 0)
            {
                MessageBox.Show("กรุณาเลือก order ที่ต้องการส่งสำเร็จ");
                return;
            }

            int orderId =
                Convert.ToInt32(
                    dataGridOrder.SelectedRows[0]
                    .Cells["order_id"]
                    .Value);


            try
            {
                var response = await RestUtil.PostAsync($"riders/{_userId}/complete/{orderId}");
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
    }
}
