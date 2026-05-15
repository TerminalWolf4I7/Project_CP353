using System.Drawing;
using System.Windows.Forms;

namespace Delivery
{
    partial class CustomerForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            panelTopBar = new Panel();
            lblAppTitle = new Label();
            lblGreeting = new Label();
            lblSubheader = new Label();
            panelSearch = new Panel();
            lblSearchIcon = new Label();
            flpRestaurants = new FlowLayoutPanel();
            panelTopBar.SuspendLayout();
            panelSearch.SuspendLayout();
            SuspendLayout();

            // === panelTopBar ===
            panelTopBar.BackColor = Color.FromArgb(39, 174, 96);
            panelTopBar.Dock = DockStyle.Top;
            panelTopBar.Size = new Size(1280, 130);
            panelTopBar.Controls.Add(lblAppTitle);
            panelTopBar.Controls.Add(lblGreeting);

            // lblAppTitle
            lblAppTitle.Text = "🛵  DELIVERY";
            lblAppTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblAppTitle.ForeColor = Color.White;
            lblAppTitle.AutoSize = true;
            lblAppTitle.Location = new Point(30, 20);
            lblAppTitle.Name = "lblAppTitle";

            // lblGreeting
            lblGreeting.Text = "อยากกินอะไรวันนี้? 😋";
            lblGreeting.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblGreeting.ForeColor = Color.White;
            lblGreeting.AutoSize = true;
            lblGreeting.Location = new Point(30, 55);
            lblGreeting.Name = "lblGreeting";

            // === panelSearch (decorative bar) ===
            panelSearch.BackColor = Color.FromArgb(240, 248, 244);
            panelSearch.Dock = DockStyle.Top;
            panelSearch.Size = new Size(1280, 55);
            panelSearch.Controls.Add(lblSearchIcon);
            panelSearch.Controls.Add(lblSubheader);

            // lblSearchIcon
            lblSearchIcon.Text = "🏪";
            lblSearchIcon.Font = new Font("Segoe UI Emoji", 14F);
            lblSearchIcon.AutoSize = true;
            lblSearchIcon.Location = new Point(25, 12);
            lblSearchIcon.Name = "lblSearchIcon";

            // lblSubheader
            lblSubheader.Text = "ร้านอาหารทั้งหมด — คลิกที่การ์ดเพื่อดูเมนู";
            lblSubheader.Font = new Font("Segoe UI", 10F);
            lblSubheader.ForeColor = Color.FromArgb(100, 100, 100);
            lblSubheader.AutoSize = true;
            lblSubheader.Location = new Point(60, 16);
            lblSubheader.Name = "lblSubheader";

            // === flpRestaurants ===
            flpRestaurants.AutoScroll = true;
            flpRestaurants.BackColor = Color.FromArgb(245, 248, 250);
            flpRestaurants.Dock = DockStyle.Fill;
            flpRestaurants.Name = "flpRestaurants";
            flpRestaurants.Padding = new Padding(25, 20, 25, 20);
            flpRestaurants.TabIndex = 0;

            // === Form ===
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 248, 250);
            ClientSize = new Size(1280, 760);
            Controls.Add(flpRestaurants);
            Controls.Add(panelSearch);
            Controls.Add(panelTopBar);
            Name = "CustomerForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Delivery App — เลือกร้านอาหาร";

            panelTopBar.ResumeLayout(false);
            panelTopBar.PerformLayout();
            panelSearch.ResumeLayout(false);
            panelSearch.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelTopBar;
        private Label lblAppTitle;
        private Label lblGreeting;
        private Panel panelSearch;
        private Label lblSearchIcon;
        private Label lblSubheader;
        private FlowLayoutPanel flpRestaurants;
    }
}