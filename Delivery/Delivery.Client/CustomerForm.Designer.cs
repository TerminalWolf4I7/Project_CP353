using System.Drawing;
using System.Windows.Forms;

namespace Delivery.Client
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
			btnOrderStatus = new Button();
			btnLogout = new Button();
			lblSubheader = new Label();
			panelSearch = new Panel();
			lblSearchIcon = new Label();
			flpRestaurants = new FlowLayoutPanel();
			panelTopBar.SuspendLayout();
			panelSearch.SuspendLayout();
			SuspendLayout();
			// 
			// panelTopBar
			// 
			panelTopBar.BackColor = Color.FromArgb(39, 174, 96);
			panelTopBar.Controls.Add(lblAppTitle);
			panelTopBar.Controls.Add(lblGreeting);
			panelTopBar.Controls.Add(btnOrderStatus);
			panelTopBar.Controls.Add(btnLogout);
			panelTopBar.Dock = DockStyle.Top;
			panelTopBar.Location = new Point(0, 0);
			panelTopBar.Margin = new Padding(2, 2, 2, 2);
			panelTopBar.Name = "panelTopBar";
			panelTopBar.Size = new Size(896, 78);
			panelTopBar.TabIndex = 2;
			// 
			// lblAppTitle
			// 
			lblAppTitle.AutoSize = true;
			lblAppTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
			lblAppTitle.ForeColor = Color.White;
			lblAppTitle.Location = new Point(18, 9);
			lblAppTitle.Margin = new Padding(2, 0, 2, 0);
			lblAppTitle.Name = "lblAppTitle";
			lblAppTitle.Size = new Size(156, 30);
			lblAppTitle.TabIndex = 0;
			lblAppTitle.Text = "\U0001f6f5  DELIVERY";
			// 
			// lblGreeting
			// 
			lblGreeting.AutoSize = true;
			lblGreeting.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
			lblGreeting.ForeColor = Color.White;
			lblGreeting.Location = new Point(18, 35);
			lblGreeting.Margin = new Padding(2, 0, 2, 0);
			lblGreeting.Name = "lblGreeting";
			lblGreeting.Size = new Size(309, 41);
			lblGreeting.TabIndex = 1;
			lblGreeting.Text = "อยากกินอะไรวันนี้? 😋";
			// 
			// btnOrderStatus
			// 
			btnOrderStatus.BackColor = Color.White;
			btnOrderStatus.Cursor = Cursors.Hand;
			btnOrderStatus.FlatStyle = FlatStyle.Flat;
			btnOrderStatus.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
			btnOrderStatus.ForeColor = Color.FromArgb(39, 174, 96);
			btnOrderStatus.Location = new Point(641, 27);
			btnOrderStatus.Margin = new Padding(2, 2, 2, 2);
			btnOrderStatus.Name = "btnOrderStatus";
			btnOrderStatus.Size = new Size(115, 32);
			btnOrderStatus.TabIndex = 2;
			btnOrderStatus.Text = "สถานะออเดอร์";
			btnOrderStatus.UseVisualStyleBackColor = false;
			btnOrderStatus.Click += BtnOrderStatus_Click;
			// 
			// btnLogout
			// 
			btnLogout.BackColor = Color.White;
			btnLogout.Cursor = Cursors.Hand;
			btnLogout.FlatStyle = FlatStyle.Flat;
			btnLogout.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
			btnLogout.ForeColor = Color.FromArgb(231, 76, 60);
			btnLogout.Location = new Point(760, 27);
			btnLogout.Margin = new Padding(2, 2, 2, 2);
			btnLogout.Name = "btnLogout";
			btnLogout.Size = new Size(108, 32);
			btnLogout.TabIndex = 3;
			btnLogout.Text = "ออกจากระบบ";
			btnLogout.UseVisualStyleBackColor = false;
			btnLogout.Click += BtnLogout_Click;
			// 
			// lblSubheader
			// 
			lblSubheader.AutoSize = true;
			lblSubheader.Font = new Font("Segoe UI", 10F);
			lblSubheader.ForeColor = Color.FromArgb(100, 100, 100);
			lblSubheader.Location = new Point(60, 7);
			lblSubheader.Margin = new Padding(2, 0, 2, 0);
			lblSubheader.Name = "lblSubheader";
			lblSubheader.Size = new Size(229, 19);
			lblSubheader.TabIndex = 1;
			lblSubheader.Text = "ร้านอาหารทั้งหมด — คลิกที่การ์ดเพื่อดูเมนู";
			// 
			// panelSearch
			// 
			panelSearch.BackColor = Color.FromArgb(240, 248, 244);
			panelSearch.Controls.Add(lblSearchIcon);
			panelSearch.Controls.Add(lblSubheader);
			panelSearch.Dock = DockStyle.Top;
			panelSearch.Location = new Point(0, 78);
			panelSearch.Margin = new Padding(2, 2, 2, 2);
			panelSearch.Name = "panelSearch";
			panelSearch.Size = new Size(896, 33);
			panelSearch.TabIndex = 1;
			// 
			// lblSearchIcon
			// 
			lblSearchIcon.AutoSize = true;
			lblSearchIcon.Font = new Font("Segoe UI Emoji", 14F);
			lblSearchIcon.Location = new Point(18, 2);
			lblSearchIcon.Margin = new Padding(2, 0, 2, 0);
			lblSearchIcon.Name = "lblSearchIcon";
			lblSearchIcon.Size = new Size(38, 26);
			lblSearchIcon.TabIndex = 0;
			lblSearchIcon.Text = "🏪";
			// 
			// flpRestaurants
			// 
			flpRestaurants.AutoScroll = true;
			flpRestaurants.BackColor = Color.FromArgb(245, 248, 250);
			flpRestaurants.Dock = DockStyle.Fill;
			flpRestaurants.Location = new Point(0, 111);
			flpRestaurants.Margin = new Padding(2, 2, 2, 2);
			flpRestaurants.Name = "flpRestaurants";
			flpRestaurants.Padding = new Padding(18, 12, 18, 12);
			flpRestaurants.Size = new Size(896, 345);
			flpRestaurants.TabIndex = 0;
			// 
			// CustomerForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = Color.FromArgb(245, 248, 250);
			ClientSize = new Size(896, 456);
			Controls.Add(flpRestaurants);
			Controls.Add(panelSearch);
			Controls.Add(panelTopBar);
			Margin = new Padding(2, 2, 2, 2);
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
        private Button btnLogout;
        private Button btnOrderStatus;
    }
}
