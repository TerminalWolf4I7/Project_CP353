using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Npgsql;

namespace Delivery
{
    public partial class RiderForm : Form
    {
        private readonly int _userId;

        private System.Windows.Forms.Timer refreshTimer;

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



        private void RiderForm_Load(
            object sender,
            EventArgs e)
        {
            LoadOrders();

            CheckCurrentOrder();
        }


        private int GetRiderId(
            NpgsqlConnection conn)
        {
            string sql = @"
                SELECT rider_id
                FROM riders
                WHERE user_id=@id";

            using var cmd =
                new NpgsqlCommand(
                    sql,
                    conn);

            cmd.Parameters.AddWithValue(
                "@id",
                _userId);

            return Convert.ToInt32(
                cmd.ExecuteScalar());
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


        private void LoadOrders()
        {
            try
            {
                using var conn =
                    new NpgsqlConnection(
                        Database.connectionString);

                conn.Open();


                string sql = @"
            SELECT
                order_id,
                restaurant_id,
                status,
                COALESCE(rider_id,0) AS rider_id
            FROM orders
            ORDER BY order_id";


                using var da =
                    new NpgsqlDataAdapter(
                        sql,
                        conn);


                DataTable dt =
                    new DataTable();


                da.Fill(dt);


                dataGridOrders.DataSource =
                    dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message);
            }
        }



        private void CheckCurrentOrder()
        {
            using var conn =
                new NpgsqlConnection(
                    Database.connectionString);

            conn.Open();


            int riderId =
                GetRiderId(conn);


            string sql = @"
                SELECT COUNT(*)
                FROM orders
                WHERE rider_id=@id
                AND status='Delivering'";

            using var cmd =
                new NpgsqlCommand(
                    sql,
                    conn);

            cmd.Parameters.AddWithValue(
                "@id",
                riderId);


            long count =
                Convert.ToInt64(
                    cmd.ExecuteScalar());


            buttonRiderOrder.Enabled =
                count > 0;
        }



        private void ButtonViewDetails_Click(
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
                using var conn =
                    new NpgsqlConnection(
                        Database.connectionString);

                conn.Open();


                string orderSql = @"
            SELECT
                o.order_id,
                u.name,
                o.restaurant_id,
                o.total_price,
                o.status
            FROM orders o
            JOIN users u
            ON o.user_id = u.user_id
            WHERE o.order_id=@id";


                using var orderCmd =
                    new NpgsqlCommand(
                        orderSql,
                        conn);

                orderCmd.Parameters.AddWithValue(
                    "@id",
                    orderId);


                StringBuilder sb =
                    new StringBuilder();


                using (var reader =
                    orderCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        sb.AppendLine(
                            $"Order ID : {reader["order_id"]}");

                        sb.AppendLine(
                            $"Customer : {reader["name"]}");

                        sb.AppendLine(
                            $"Restaurant : {reader["restaurant_id"]}");

                        sb.AppendLine(
                            $"Status : {reader["status"]}");

                        sb.AppendLine(
                            $"Total : {reader["total_price"]}");

                        sb.AppendLine();
                        sb.AppendLine("Items:");
                    }
                }



                string itemSql = @"
            SELECT
                m.name,
                oi.quantity,
                oi.price
            FROM order_items oi
            JOIN menu_items m
            ON oi.item_id = m.item_id
            WHERE oi.order_id=@id";


                using var itemCmd =
                    new NpgsqlCommand(
                        itemSql,
                        conn);

                itemCmd.Parameters.AddWithValue(
                    "@id",
                    orderId);


                using var itemReader =
                    itemCmd.ExecuteReader();


                while (itemReader.Read())
                {
                    sb.AppendLine(
                        $"{itemReader["name"]} " +
                        $"x {itemReader["quantity"]} " +
                        $"({itemReader["price"]})");
                }


                MessageBox.Show(
                    sb.ToString(),
                    $"Order {orderId}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message);
            }
        }



        private void ButtonAcceptOrder_Click(
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
                using var conn =
                    new NpgsqlConnection(
                        Database.connectionString);

                conn.Open();


                int riderId =
                    GetRiderId(conn);


                using var tx =
                    conn.BeginTransaction();


                // รับ order ได้เฉพาะ order ที่ยังไม่มี rider
                string orderSql = @"
            UPDATE orders
            SET rider_id=@rider,
                status='Delivering'
            WHERE order_id=@order
            AND rider_id IS NULL";


                using var orderCmd =
                    new NpgsqlCommand(
                        orderSql,
                        conn,
                        tx);

                orderCmd.Parameters.AddWithValue(
                    "@rider",
                    riderId);

                orderCmd.Parameters.AddWithValue(
                    "@order",
                    orderId);


                int affected =
                    orderCmd.ExecuteNonQuery();


                // มีคนอื่นรับไปแล้ว
                if (affected == 0)
                {
                    tx.Rollback();

                    MessageBox.Show(
                        "Order นี้ถูกรับไปแล้ว");

                    LoadOrders();

                    return;
                }


                string riderSql = @"
            UPDATE riders
            SET status='Delivering'
            WHERE user_id=@id";


                using var riderCmd =
                    new NpgsqlCommand(
                        riderSql,
                        conn,
                        tx);

                riderCmd.Parameters.AddWithValue(
                    "@id",
                    _userId);

                riderCmd.ExecuteNonQuery();


                tx.Commit();


                MessageBox.Show(
                    "Order accepted.");


                LoadOrders();

                CheckCurrentOrder();
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
            LoadOrders();

            CheckCurrentOrder();
        }
        protected override void OnFormClosed(
    FormClosedEventArgs e)
        {
            refreshTimer.Stop();

            base.OnFormClosed(e);
        }
    }
}