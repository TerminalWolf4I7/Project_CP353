using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace Delivery
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


        private void RiderOrderForm_Load(
            object sender,
            EventArgs e)
        {
            LoadMyOrder();
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

            object result =
                cmd.ExecuteScalar();

            if (result == null)
                throw new Exception(
                    "Rider not found.");

            return Convert.ToInt32(
                result);
        }


        private void LoadMyOrder()
        {
            try
            {
                using var conn =
                    new NpgsqlConnection(
                        Database.connectionString);

                conn.Open();


                int riderId =
                    GetRiderId(conn);


                string sql = @"
                    SELECT
                        o.order_id,
                        u.user_id,
                        u.name,
                        o.restaurant_id,
                        o.status
                    FROM orders o
                    JOIN users u
                    ON o.user_id = u.user_id
                    WHERE o.rider_id=@id
                    AND o.status='Delivering'";


                using var da =
                    new NpgsqlDataAdapter(
                        sql,
                        conn);

                da.SelectCommand.Parameters.AddWithValue(
                    "@id",
                    riderId);


                DataTable dt =
                    new DataTable();

                da.Fill(dt);

                dataGridOrder.DataSource = dt;


                buttonComplete.Enabled =
                    dt.Rows.Count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message);
            }
        }


        private void ButtonComplete_Click(
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
                using var conn =
                    new NpgsqlConnection(
                        Database.connectionString);

                conn.Open();

                using var tx =
                    conn.BeginTransaction();


                // ลบรายการอาหารใน order ก่อน
                string deleteItemsSql = @"
            DELETE FROM order_items
            WHERE order_id=@order";

                using var deleteItemsCmd =
                    new NpgsqlCommand(
                        deleteItemsSql,
                        conn,
                        tx);

                deleteItemsCmd.Parameters.AddWithValue(
                    "@order",
                    orderId);

                deleteItemsCmd.ExecuteNonQuery();



                // ลบ order
                string deleteOrderSql = @"
            DELETE FROM orders
            WHERE order_id=@order";

                using var deleteOrderCmd =
                    new NpgsqlCommand(
                        deleteOrderSql,
                        conn,
                        tx);

                deleteOrderCmd.Parameters.AddWithValue(
                    "@order",
                    orderId);

                deleteOrderCmd.ExecuteNonQuery();



                // rider กลับเป็น Available
                string riderSql = @"
            UPDATE riders
            SET status='Available'
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
                    "Delivery completed.");


                LoadMyOrder();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message);
            }
        }
        //private void ButtonComplete_Click(
        //    object sender,
        //    EventArgs e)
        //{
        //    if (dataGridOrder.Rows.Count == 0)
        //        return;


        //    int orderId =
        //        Convert.ToInt32(
        //            dataGridOrder.Rows[0]
        //            .Cells["order_id"]
        //            .Value);


        //    try
        //    {
        //        using var conn =
        //            new NpgsqlConnection(
        //                Database.connectionString);

        //        conn.Open();

        //        using var tx =
        //            conn.BeginTransaction();


        //        string orderSql = @"
        //            UPDATE orders
        //            SET status='Completed'
        //            WHERE order_id=@order";

        //        using var orderCmd =
        //            new NpgsqlCommand(
        //                orderSql,
        //                conn,
        //                tx);

        //        orderCmd.Parameters.AddWithValue(
        //            "@order",
        //            orderId);

        //        orderCmd.ExecuteNonQuery();



        //        string riderSql = @"
        //            UPDATE riders
        //            SET status='Available'
        //            WHERE user_id=@id";

        //        using var riderCmd =
        //            new NpgsqlCommand(
        //                riderSql,
        //                conn,
        //                tx);

        //        riderCmd.Parameters.AddWithValue(
        //            "@id",
        //            _userId);

        //        riderCmd.ExecuteNonQuery();


        //        tx.Commit();


        //        MessageBox.Show(
        //            "Delivery completed.");


        //        LoadMyOrder();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(
        //            ex.Message);
        //    }
        //}
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