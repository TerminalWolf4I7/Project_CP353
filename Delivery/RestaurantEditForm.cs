using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace Delivery
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

        private void RestaurantEditForm_Load(object? sender, EventArgs e)
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(Database.connectionString))
                {
                    conn.Open();

                    string restaurantQuery = "SELECT restaurant_id, name FROM restaurants WHERE user_id = @user_id";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(restaurantQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@user_id", userId);

                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                restaurantId = reader.GetInt32(0);
                                textRestaurantName.Text = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                            }
                            else
                            {
                                MessageBox.Show("Restaurant not found for this user.");
                                Close();
                                return;
                            }
                        }
                    }

                    string menuQuery = "SELECT item_id, name, price FROM menu_items WHERE restaurant_id = @restaurant_id ORDER BY item_id";

                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(menuQuery, conn))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@restaurant_id", restaurantId);
                        menuTable = new DataTable();
                        adapter.Fill(menuTable);
                    }

                    dataGridMenu.DataSource = menuTable;

                    if (dataGridMenu.Columns["item_id"] != null)
                    {
                        dataGridMenu.Columns["item_id"].Visible = false;
                    }
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

        private void ButtonDelete_Click(object? sender, EventArgs e)
        {
            if (dataGridMenu.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a row to delete.");
                return;
            }

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(Database.connectionString))
                {
                    conn.Open();

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

                            using (NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM menu_items WHERE item_id = @item_id AND restaurant_id = @restaurant_id", conn))
                            {
                                cmd.Parameters.AddWithValue("@item_id", itemId);
                                cmd.Parameters.AddWithValue("@restaurant_id", restaurantId);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        dataGridMenu.Rows.Remove(row);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButtonSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textRestaurantName.Text))
            {
                MessageBox.Show("Restaurant name is required.");
                return;
            }

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(Database.connectionString))
                {
                    conn.Open();

                    using (NpgsqlTransaction transaction = conn.BeginTransaction())
                    {
                        string updateRestaurant = "UPDATE restaurants SET name = @name WHERE restaurant_id = @restaurant_id";

                        using (NpgsqlCommand cmd = new NpgsqlCommand(updateRestaurant, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@name", textRestaurantName.Text.Trim());
                            cmd.Parameters.AddWithValue("@restaurant_id", restaurantId);
                            cmd.ExecuteNonQuery();
                        }

                        string updateMenu = "UPDATE menu_items SET name = @name, price = @price WHERE item_id = @item_id AND restaurant_id = @restaurant_id";
                        string insertMenu = "INSERT INTO menu_items (restaurant_id, name, price) VALUES (@restaurant_id, @name, @price) RETURNING item_id";

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
                                transaction.Rollback();
                                return;
                            }

                            if (!decimal.TryParse(priceText, out decimal price))
                            {
                                MessageBox.Show("Invalid price value.");
                                transaction.Rollback();
                                return;
                            }

                            object? itemIdValue = row.Cells["item_id"].Value;

                            if (itemIdValue == null || itemIdValue == DBNull.Value)
                            {
                                using (NpgsqlCommand cmd = new NpgsqlCommand(insertMenu, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@restaurant_id", restaurantId);
                                    cmd.Parameters.AddWithValue("@name", name);
                                    cmd.Parameters.AddWithValue("@price", price);

                                    object? newId = cmd.ExecuteScalar();
                                    row.Cells["item_id"].Value = newId;
                                }
                            }
                            else
                            {
                                int itemId = Convert.ToInt32(itemIdValue);

                                using (NpgsqlCommand cmd = new NpgsqlCommand(updateMenu, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@name", name);
                                    cmd.Parameters.AddWithValue("@price", price);
                                    cmd.Parameters.AddWithValue("@item_id", itemId);
                                    cmd.Parameters.AddWithValue("@restaurant_id", restaurantId);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }

                        transaction.Commit();
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
