using System.Linq;
using System.Drawing;
using Npgsql;
using System.Data;
using System.Windows.Forms;

namespace Delivery
{
    public partial class CustomerForm : Form
    {
        private int userId;

        public CustomerForm(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            this.Load += CustomerForm_Load;
        }

        private void CustomerForm_Load(object sender, EventArgs e)
        {
            LoadRestaurants();
        }

        private void LoadRestaurants()
        {
            try
            {
                flpRestaurants.Controls.Clear();
                using (NpgsqlConnection conn = new NpgsqlConnection(Database.connectionString))
                {
                    conn.Open();
                    string query = "SELECT restaurant_id, name, address, phone FROM restaurants";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                string name = reader.GetString(1);
                                string address = reader.GetString(2);

                                // สร้าง Card Panel
                                Panel card = new Panel();
                                card.Size = new Size(350, 200);
                                card.BackColor = Color.White;
                                card.Margin = new Padding(15);
                                card.BorderStyle = BorderStyle.FixedSingle;
                                card.Cursor = Cursors.Hand;
                                card.Tag = id; // เก็บ ID ไว้ใน Tag

                                // ชื่อร้าน
                                Label lblName = new Label();
                                lblName.Text = name;
                                lblName.Font = new Font("Segoe UI", 14, FontStyle.Bold);
                                lblName.Location = new Point(20, 20);
                                lblName.AutoSize = true;
                                lblName.ForeColor = Color.FromArgb(44, 62, 80);
                                card.Controls.Add(lblName);

                                // ที่อยู่
                                Label lblAddr = new Label();
                                lblAddr.Text = "📍 " + address;
                                lblAddr.Font = new Font("Segoe UI", 10);
                                lblAddr.Location = new Point(20, 60);
                                lblAddr.Size = new Size(310, 80);
                                card.Controls.Add(lblAddr);

                                // ปุ่ม "ดูเมนู" แบบหลอกๆ ให้ดูสวย
                                Label btnFake = new Label();
                                btnFake.Text = "ดูเมนูอาหาร >";
                                btnFake.ForeColor = Color.FromArgb(46, 204, 113);
                                btnFake.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                                btnFake.Location = new Point(230, 160);
                                btnFake.AutoSize = true;
                                card.Controls.Add(btnFake);

                                // เพิ่ม Click Event ให้ทั้ง Card
                                card.Click += RestaurantCard_Click;
                                foreach (Control ctrl in card.Controls)
                                {
                                    ctrl.Click += (s, e) => RestaurantCard_Click(card, e);
                                }

                                flpRestaurants.Controls.Add(card);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading restaurants: " + ex.Message);
            }
        }

        private void RestaurantCard_Click(object? sender, EventArgs e)
        {
            if (sender is Panel card)
            {
                int restaurantId = (int)card.Tag;
                string restaurantName = card.Controls.OfType<Label>().First().Text;

                MenuForm menuForm = new MenuForm(restaurantId, this.userId);
                menuForm.Text = "เมนูของร้าน: " + restaurantName;
                menuForm.ShowDialog();
            }
        }
    }
}
