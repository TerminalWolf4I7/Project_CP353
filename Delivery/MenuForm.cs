using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace Delivery
{
    public partial class MenuForm : Form
    {
        private int restaurantId;
        private string restaurantName;
        private int userId;
        private readonly HttpClient httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5000/api/") };

        public MenuForm(int restaurantId, string restaurantName, int userId)
        {
            InitializeComponent();
            this.restaurantId = restaurantId;
            this.restaurantName = restaurantName;
            this.userId = userId;
            this.Load += MenuForm_Load;
            btnCheckout.Click += BtnCheckout_Click;
            btnBack.Click += BtnBack_Click;
        }

        private void MenuForm_Load(object sender, EventArgs e)
        {
            lblRestaurantName.Text = restaurantName; // ใช้ชื่อที่ส่งมาจากหน้า CustomerForm
            _ = LoadMenuAsync();
            _ = UpdateTotalAsync();
        }

        private async Task LoadMenuAsync()
        {
            try
            {
                flpMenu.Controls.Clear();
                var items = await httpClient.GetFromJsonAsync<List<Delivery.Api.Models.MenuItemDto>>(
                    $"restaurants/{restaurantId}/menu");

                if (items == null)
                {
                    return;
                }

                foreach (var item in items)
                {
                    int id = item.ItemId;
                    string name = item.Name;
                    decimal price = item.Price;
                    string desc = item.Description ?? string.Empty;

                    Panel card = new Panel();
                    card.Size = new Size(220, 280);
                    card.BackColor = Color.White;
                    card.Margin = new Padding(10);
                    card.BorderStyle = BorderStyle.FixedSingle;

                    Label lblName = new Label { Text = name, Font = new Font("Segoe UI", 12, FontStyle.Bold), Location = new Point(10, 10), AutoSize = true };
                    Label lblPrice = new Label { Text = price.ToString("N2") + " บาท", ForeColor = Color.FromArgb(46, 204, 113), Font = new Font("Segoe UI", 11, FontStyle.Bold), Location = new Point(10, 40), AutoSize = true };
                    Label lblDesc = new Label { Text = desc, Font = new Font("Segoe UI", 9), Location = new Point(10, 75), Size = new Size(200, 120) };

                    Button btnAdd = new Button
                    {
                        Text = "เพิ่ม (+)",
                        BackColor = Color.FromArgb(46, 204, 113),
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Font = new Font("Segoe UI", 9, FontStyle.Bold),
                        Location = new Point(10, 220),
                        Size = new Size(95, 45),
                        Tag = id
                    };
                    btnAdd.Click += BtnAddToCart_Click;

                    Button btnRemove = new Button
                    {
                        Text = "ลด (-)",
                        BackColor = Color.FromArgb(231, 76, 60),
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Font = new Font("Segoe UI", 9, FontStyle.Bold),
                        Location = new Point(115, 220),
                        Size = new Size(95, 45),
                        Tag = id
                    };
                    btnRemove.Click += BtnRemoveFromCart_Click;

                    card.Controls.Add(lblName);
                    card.Controls.Add(lblPrice);
                    card.Controls.Add(lblDesc);
                    card.Controls.Add(btnAdd);
                    card.Controls.Add(btnRemove);
                    flpMenu.Controls.Add(card);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading menu: " + ex.Message);
            }
        }

        private void BtnBack_Click(object? sender, EventArgs e)
        {
            CustomerForm customerForm = new CustomerForm(userId);
            customerForm.Show();
            Close();
        }

        private async void BtnAddToCart_Click(object? sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                int itemId = (int)btn.Tag;
                await AddToCartAsync(itemId);
                await UpdateTotalAsync();
            }
        }

        private async void BtnRemoveFromCart_Click(object? sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                int itemId = (int)btn.Tag;
                await RemoveFromCartAsync(itemId);
                await UpdateTotalAsync();
            }
        }

        private async Task AddToCartAsync(int itemId)
        {
            try
            {
                var payload = new Delivery.Api.Models.CartItemRequest(itemId, 1);
                var response = await httpClient.PostAsJsonAsync(
                    $"carts/{userId}/{restaurantId}/items",
                    payload);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private async Task RemoveFromCartAsync(int itemId)
        {
            try
            {
                var response = await httpClient.DeleteAsync(
                    $"carts/{userId}/{restaurantId}/items/{itemId}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error removing item: " + ex.Message);
            }
        }

        private async Task UpdateTotalAsync()
        {
            try
            {
                var cart = await httpClient.GetFromJsonAsync<Delivery.Api.Models.CartSummaryDto>(
                    $"carts/{userId}/{restaurantId}");

                decimal total = cart?.Total ?? 0;
                lblTotal.Text = $"ราคารวม: {total.ToString("N2")} บาท";
            }
            catch { }
        }

        private async void BtnCheckout_Click(object? sender, EventArgs e)
        {
            try
            {
                var cart = await httpClient.GetFromJsonAsync<Delivery.Api.Models.CartSummaryDto>(
                    $"carts/{userId}/{restaurantId}");

                if (cart == null || cart.Items.Count == 0)
                {
                    MessageBox.Show("ตะกร้าว่างเปล่า กรุณาเลือกอาหารก่อนครับ");
                    return;
                }

                var payload = new Delivery.Api.Models.CheckoutRequest(userId, restaurantId);
                var response = await httpClient.PostAsJsonAsync("orders/checkout", payload);
                response.EnsureSuccessStatusCode();

                int orderId = await response.Content.ReadFromJsonAsync<int>();
                MessageBox.Show($"สั่งซื้อสำเร็จ! เลขที่ใบสั่งซื้อของคุณคือ: {orderId}");

                OrderStatusForm statusForm = new OrderStatusForm(this.restaurantName, this.userId, orderId);
                statusForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            httpClient.Dispose();
            base.OnFormClosed(e);
        }
    }
}
