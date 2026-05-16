using System;
using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Delivery
{
    public partial class OrderStatusForm : Form
    {
        private string restaurantName;
        private int orderId;
        private int userId; // เพิ่ม userId

        private Label statusLabel;
        private Label detailLabel;
        private ProgressBar progressBar;
        private System.Windows.Forms.Timer statusTimer;
        private int currentStep = 0;
        private readonly HttpClient httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5000/api/") };

        private string[] steps =
        {
            "ร้านรับออเดอร์แล้ว",
            "ร้านกำลังทำอาหาร",
            "กำลังหา Rider",
            "Rider กำลังไปส่ง",
            "จัดส่งสำเร็จ"
        };

        public OrderStatusForm(string restaurant, int userId, int orderId) // เพิ่ม userId ใน constructor
        {
            InitializeComponent();

            this.restaurantName = restaurant;
            this.userId = userId;
            this.orderId = orderId;

            SetupUI();
            StartStatusPolling();
        }

        private void SetupUI()
        {
            this.Text = "Order Status";
            this.Size = new Size(900, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            Panel mainPanel = new Panel();
            mainPanel.BackColor = Color.FromArgb(42, 107, 190);
            mainPanel.Size = new Size(850, 580);
            mainPanel.Location = new Point(20, 20);
            this.Controls.Add(mainPanel);

            Label title = new Label();
            title.Text = "สถานะคำสั่งซื้อ";
            title.Font = new Font("Segoe UI", 24, FontStyle.Bold);
            title.ForeColor = Color.White;
            title.TextAlign = ContentAlignment.MiddleCenter;
            title.Size = new Size(mainPanel.Width, 60);
            title.Location = new Point(0, 20);
            mainPanel.Controls.Add(title);

            Panel infoCard = new Panel();
            infoCard.BackColor = Color.White;
            infoCard.Size = new Size(680, 120);
            infoCard.Location = new Point(85, 95);
            mainPanel.Controls.Add(infoCard);

            Label restaurantLabel = new Label();
            restaurantLabel.Text = restaurantName;
            restaurantLabel.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            restaurantLabel.ForeColor = Color.Black;
            restaurantLabel.Size = new Size(620, 45);
            restaurantLabel.Location = new Point(30, 20);
            infoCard.Controls.Add(restaurantLabel);

            Label orderLabel = new Label();
            orderLabel.Text = "Order ID: #" + orderId;
            orderLabel.Font = new Font("Segoe UI", 13);
            orderLabel.ForeColor = Color.DimGray;
            orderLabel.Size = new Size(620, 35);
            orderLabel.Location = new Point(33, 70);
            infoCard.Controls.Add(orderLabel);

            statusLabel = new Label();
            statusLabel.Text = "Status: กำลังดำเนินการ";
            statusLabel.Font = new Font("Segoe UI", 19, FontStyle.Bold);
            statusLabel.ForeColor = Color.White;
            statusLabel.TextAlign = ContentAlignment.MiddleCenter;
            statusLabel.Size = new Size(mainPanel.Width, 45);
            statusLabel.Location = new Point(0, 245);
            mainPanel.Controls.Add(statusLabel);

            detailLabel = new Label();
            detailLabel.Text = steps[0];
            detailLabel.Font = new Font("Segoe UI", 17, FontStyle.Bold);
            detailLabel.ForeColor = Color.White;
            detailLabel.TextAlign = ContentAlignment.MiddleCenter;
            detailLabel.Size = new Size(mainPanel.Width, 40);
            detailLabel.Location = new Point(0, 300);
            mainPanel.Controls.Add(detailLabel);

            progressBar = new ProgressBar();
            progressBar.Size = new Size(650, 30);
            progressBar.Location = new Point(100, 355);
            progressBar.Minimum = 0;
            progressBar.Maximum = 4;
            progressBar.Value = 0;
            mainPanel.Controls.Add(progressBar);

            Label flowLabel = new Label();
            flowLabel.Text = "รับออเดอร์  >  กำลังทำ  >  หา Rider  >  กำลังไปส่ง  >  เสร็จสิ้น";
            flowLabel.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            flowLabel.ForeColor = Color.White;
            flowLabel.TextAlign = ContentAlignment.MiddleCenter;
            flowLabel.Size = new Size(mainPanel.Width, 35);
            flowLabel.Location = new Point(0, 405);
            mainPanel.Controls.Add(flowLabel);

            Button customerButton = new Button();
            customerButton.Text = "กลับหน้า Customer";
            customerButton.Size = new Size(220, 50);
            customerButton.Location = new Point(315, 485);
            customerButton.BackColor = Color.White;
            customerButton.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            customerButton.FlatStyle = FlatStyle.Flat;
            customerButton.FlatAppearance.BorderSize = 0;
            customerButton.Click += CustomerButton_Click;
            mainPanel.Controls.Add(customerButton);
        }

        private void StartStatusPolling()
        {
            statusTimer = new System.Windows.Forms.Timer();
            statusTimer.Interval = 3000;
            statusTimer.Tick += StatusTimer_Tick;
            statusTimer.Start();
            _ = RefreshStatusAsync();
        }

        private void StatusTimer_Tick(object sender, EventArgs e)
        {
            _ = RefreshStatusAsync();
        }

        private async Task RefreshStatusAsync()
        {
            try
            {
                var response = await httpClient.GetAsync($"orders/{orderId}/details");

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    ShowCompletedStatus();
                    return;
                }

                response.EnsureSuccessStatusCode();

                var details = await response.Content.ReadFromJsonAsync<Delivery.Api.Models.OrderDetailDto>();
                if (details == null)
                {
                    return;
                }

                UpdateStatusDisplay(details.Status);
            }
            catch
            {
            }
        }

        private void UpdateStatusDisplay(string status)
        {
            statusLabel.Text = $"Status: {status}";

            int stepIndex = status switch
            {
                "Pending" => 0,
                "Cooking" => 1,
                "Waiting for rider" => 2,
                "Delivering" => 3,
                "Completed" => 4,
                "Delivered" => 4,
                "Success" => 4,
                _ => -1
            };

            if (stepIndex >= 0)
            {
                currentStep = stepIndex;
                detailLabel.Text = steps[stepIndex];
                progressBar.Value = stepIndex;

                if (stepIndex == steps.Length - 1)
                {
                    statusTimer.Stop();
                }
            }
            else
            {
                detailLabel.Text = status;
            }
        }

        private void ShowCompletedStatus()
        {
            currentStep = steps.Length - 1;
            statusLabel.Text = "Status: เสร็จสิ้น";
            detailLabel.Text = steps[currentStep];
            progressBar.Value = currentStep;
            statusTimer.Stop();
        }

        private void CustomerButton_Click(object sender, EventArgs e)
        {
            // ส่ง userId กลับไปให้ CustomerForm ด้วย
            CustomerForm customerForm = new CustomerForm(userId);
            customerForm.Show();
            this.Close();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            statusTimer?.Stop();
            httpClient.Dispose();
            base.OnFormClosed(e);
        }
    }
}