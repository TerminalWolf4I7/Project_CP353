using System;
using System.Data;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Delivery.Client.Models;

namespace Delivery.Client
{
    public partial class RestaurantEditForm : Form
    {
        private readonly int userId;
        private int restaurantId;
        private DataTable menuTable = new DataTable();

        public RestaurantEditForm(int userId)
        {
            InitializeComponent();
            this.userId = userId;

            Load += RestaurantEditForm_Load;
            buttonSave.Click += ButtonSave_Click;
            buttonAdd.Click += ButtonAdd_Click;
            buttonDelete.Click += ButtonDelete_Click;
        }

        private async void RestaurantEditForm_Load(object? sender, EventArgs e)
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
                textRestaurantName.Text = restaurant.Name;

                var menuItems = await RestUtil.GetAsync<List<MenuItemDto>>(
                    $"restaurants/{restaurantId}/menu");

                menuTable = new DataTable();
                menuTable.Columns.Add("item_id", typeof(int));
                menuTable.Columns.Add("name", typeof(string));
                menuTable.Columns.Add("price", typeof(decimal));

                if (menuItems != null)
                {
                    foreach (var item in menuItems.OrderBy(i => i.ItemId))
                    {
                        menuTable.Rows.Add(item.ItemId, item.Name, item.Price);
                    }
                }

                dataGridMenu.DataSource = menuTable;

                if (dataGridMenu.Columns["item_id"] != null)
                {
                    dataGridMenu.Columns["item_id"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButtonAdd_Click(object? sender, EventArgs e)
        {
            if (menuTable.Columns.Count == 0)
            {
                return;
            }

            menuTable.Rows.Add(DBNull.Value, string.Empty, 0m);
        }

        private async void ButtonDelete_Click(object? sender, EventArgs e)
        {
            if (dataGridMenu.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a row to delete.");
                return;
            }

            try
            {
                foreach (DataGridViewRow row in dataGridMenu.SelectedRows)
                {
                    if (row.IsNewRow)
                    {
                        continue;
                    }

                    object? itemIdValue = row.Cells["item_id"].Value;

                    if (itemIdValue != null && itemIdValue != DBNull.Value)
                    {
                        int itemId = Convert.ToInt32(itemIdValue);
                        var response = await RestUtil.DeleteAsync($"restaurants/menu/{itemId}?restaurantId={restaurantId}");
                        response.EnsureSuccessStatusCode();
                    }

                    dataGridMenu.Rows.Remove(row);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void ButtonSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textRestaurantName.Text))
            {
                MessageBox.Show("Restaurant name is required.");
                return;
            }

            try
            {
                var restaurantPayload = new RestaurantUpdateRequest(textRestaurantName.Text.Trim());
                var restaurantResponse = await RestUtil.PutAsync(
                    $"restaurants/{restaurantId}",
                    restaurantPayload);
                restaurantResponse.EnsureSuccessStatusCode();

                foreach (DataGridViewRow row in dataGridMenu.Rows)
                {
                    if (row.IsNewRow)
                    {
                        continue;
                    }

                    string name = row.Cells["name"].Value?.ToString() ?? string.Empty;
                    string priceText = row.Cells["price"].Value?.ToString() ?? "0";

                    if (string.IsNullOrWhiteSpace(name))
                    {
                        MessageBox.Show("Menu item name is required.");
                        return;
                    }

                    if (!decimal.TryParse(priceText, out decimal price))
                    {
                        MessageBox.Show("Invalid price value.");
                        return;
                    }

                    var payload = new MenuItemUpsertRequest(name, price);
                    object? itemIdValue = row.Cells["item_id"].Value;

                    if (itemIdValue == null || itemIdValue == DBNull.Value)
                    {
                        var response = await RestUtil.PostResponseAsync(
                            $"restaurants/{restaurantId}/menu",
                            payload);
                        response.EnsureSuccessStatusCode();

                        int newId = await RestUtil.ReadAsAsync<int>(response);
                        row.Cells["item_id"].Value = newId;
                    }
                    else
                    {
                        int itemId = Convert.ToInt32(itemIdValue);
                        var response = await RestUtil.PutAsync(
                            $"restaurants/menu/{itemId}",
                            payload);
                        response.EnsureSuccessStatusCode();
                    }
                }

                MessageBox.Show("Saved.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
