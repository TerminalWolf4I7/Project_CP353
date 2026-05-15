using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Npgsql;

namespace Delivery
{
    public partial class CustomerForm : Form
    {
        private int userId;

        // สีโทนของแต่ละร้าน (วนซ้ำ) เพื่อความสวยงาม
        private Color[] accentColors = new Color[]
        {
            Color.FromArgb(39, 174, 96),   // เขียว
            Color.FromArgb(52, 152, 219),  // ฟ้า
            Color.FromArgb(231, 76, 60),   // แดง
            Color.FromArgb(155, 89, 182),  // ม่วง
            Color.FromArgb(230, 126, 34),  // ส้ม
            Color.FromArgb(26, 188, 156),  // เขียวมิ้นท์
        };

        private string[] categoryEmojis = new string[]
        {
            "🍛", "🍜", "🍣", "☕", "🍕", "🥩", "🥗", "🍱"
        };

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
                    string query = "SELECT restaurant_id, name, address, phone FROM restaurants ORDER BY restaurant_id";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        int index = 0;
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            string address = reader.IsDBNull(2) ? "" : reader.GetString(2);
                            string phone = reader.IsDBNull(3) ? "" : reader.GetString(3);

                            Color accent = accentColors[index % accentColors.Length];
                            string emoji = categoryEmojis[index % categoryEmojis.Length];

                            Panel card = CreateRestaurantCard(id, name, address, phone, accent, emoji);
                            flpRestaurants.Controls.Add(card);
                            index++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading restaurants: " + ex.Message);
            }
        }

        private Panel CreateRestaurantCard(int id, string name, string address, string phone, Color accent, string emoji)
        {
            // === Card Container ===
            Panel card = new Panel();
            card.Size = new Size(370, 210);
            card.BackColor = Color.White;
            card.Margin = new Padding(12);
            card.Cursor = Cursors.Hand;
            card.Tag = id;

            // === Left Accent Bar ===
            Panel accentBar = new Panel();
            accentBar.Size = new Size(8, 210);
            accentBar.Location = new Point(0, 0);
            accentBar.BackColor = accent;
            card.Controls.Add(accentBar);

            // === Emoji Circle ===
            Panel emojiCircle = new Panel();
            emojiCircle.Size = new Size(64, 64);
            emojiCircle.Location = new Point(25, 25);
            emojiCircle.BackColor = Color.FromArgb(
                Math.Min(accent.R + 180, 255),
                Math.Min(accent.G + 180, 255),
                Math.Min(accent.B + 180, 255));
            card.Controls.Add(emojiCircle);

            Label lblEmoji = new Label();
            lblEmoji.Text = emoji;
            lblEmoji.Font = new Font("Segoe UI Emoji", 22F);
            lblEmoji.Size = new Size(64, 64);
            lblEmoji.Location = new Point(0, 0);
            lblEmoji.TextAlign = ContentAlignment.MiddleCenter;
            emojiCircle.Controls.Add(lblEmoji);

            // === Restaurant Name ===
            Label lblName = new Label();
            lblName.Text = name;
            lblName.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblName.ForeColor = Color.FromArgb(25, 25, 25);
            lblName.Location = new Point(105, 22);
            lblName.Size = new Size(250, 35);
            card.Controls.Add(lblName);

            // === Address ===
            Label lblAddr = new Label();
            lblAddr.Text = "📍  " + address;
            lblAddr.Font = new Font("Segoe UI", 9F);
            lblAddr.ForeColor = Color.FromArgb(120, 120, 120);
            lblAddr.Location = new Point(105, 62);
            lblAddr.Size = new Size(250, 40);
            card.Controls.Add(lblAddr);

            // === Phone ===
            Label lblPhone = new Label();
            lblPhone.Text = "📞  " + phone;
            lblPhone.Font = new Font("Segoe UI", 9F);
            lblPhone.ForeColor = Color.FromArgb(120, 120, 120);
            lblPhone.Location = new Point(105, 100);
            lblPhone.Size = new Size(250, 25);
            card.Controls.Add(lblPhone);

            // === Divider ===
            Panel divider = new Panel();
            divider.Size = new Size(340, 1);
            divider.Location = new Point(20, 148);
            divider.BackColor = Color.FromArgb(235, 235, 235);
            card.Controls.Add(divider);

            // === Footer: "ดูเมนู" button ===
            Label lblSeeMenu = new Label();
            lblSeeMenu.Text = "ดูเมนูทั้งหมด  →";
            lblSeeMenu.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblSeeMenu.ForeColor = accent;
            lblSeeMenu.Location = new Point(22, 160);
            lblSeeMenu.AutoSize = true;
            card.Controls.Add(lblSeeMenu);

            // === Add click to all children ===
            card.Click += RestaurantCard_Click;
            foreach (Control ctrl in card.Controls)
            {
                ctrl.Click += (s, e) => RestaurantCard_Click(card, e);
                ctrl.Cursor = Cursors.Hand;
                foreach (Control child in ctrl.Controls)
                {
                    child.Click += (s, e) => RestaurantCard_Click(card, e);
                    child.Cursor = Cursors.Hand;
                }
            }

            return card;
        }

        private void RestaurantCard_Click(object? sender, EventArgs e)
        {
            Panel card = sender as Panel;
            if (card == null) return;

            // หา root card (ถ้ากดมาจาก child)
            while (card.Tag == null || !(card.Tag is int))
            {
                if (card.Parent is Panel p) card = p;
                else return;
            }

            int restaurantId = (int)card.Tag;
            string restaurantName = "";

            // หาชื่อร้านจาก Label แรก
            foreach (Control ctrl in card.Controls)
            {
                if (ctrl is Label lbl && lbl.Font.Size >= 14F)
                {
                    restaurantName = lbl.Text;
                    break;
                }
            }

            MenuForm menuForm = new MenuForm(restaurantId, restaurantName, this.userId);
            menuForm.Show();
            this.Hide();
        }
    }
}