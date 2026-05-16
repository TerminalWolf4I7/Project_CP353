using System.Drawing;
using System.Windows.Forms;

namespace Delivery.Client
{
    partial class MenuForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			panelHeader = new Panel();
			lblRestaurantName = new Label();
			btnBack = new Button();
			flpMenu = new FlowLayoutPanel();
			panelFooter = new Panel();
			lblTotal = new Label();
			btnCheckout = new Button();
			panelHeader.SuspendLayout();
			panelFooter.SuspendLayout();
			SuspendLayout();
			// 
			// panelHeader
			// 
			panelHeader.BackColor = Color.FromArgb(46, 204, 113);
			panelHeader.Controls.Add(lblRestaurantName);
			panelHeader.Controls.Add(btnBack);
			panelHeader.Dock = DockStyle.Top;
			panelHeader.Location = new Point(0, 0);
			panelHeader.Margin = new Padding(2, 2, 2, 2);
			panelHeader.Name = "panelHeader";
			panelHeader.Size = new Size(700, 60);
			panelHeader.TabIndex = 0;
			// 
			// lblRestaurantName
			// 
			lblRestaurantName.AutoSize = true;
			lblRestaurantName.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
			lblRestaurantName.ForeColor = Color.White;
			lblRestaurantName.Location = new Point(14, 12);
			lblRestaurantName.Margin = new Padding(2, 0, 2, 0);
			lblRestaurantName.Name = "lblRestaurantName";
			lblRestaurantName.Size = new Size(220, 41);
			lblRestaurantName.TabIndex = 0;
			lblRestaurantName.Text = "ชื่อร้านอาหาร...";
			// 
			// btnBack
			// 
			btnBack.BackColor = Color.White;
			btnBack.FlatStyle = FlatStyle.Flat;
			btnBack.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
			btnBack.ForeColor = Color.FromArgb(46, 204, 113);
			btnBack.Location = new Point(589, 18);
			btnBack.Margin = new Padding(2, 2, 2, 2);
			btnBack.Name = "btnBack";
			btnBack.Size = new Size(90, 35);
			btnBack.TabIndex = 1;
			btnBack.Text = "← กลับ";
			btnBack.UseVisualStyleBackColor = false;
			// 
			// flpMenu
			// 
			flpMenu.AutoScroll = true;
			flpMenu.BackColor = Color.WhiteSmoke;
			flpMenu.Dock = DockStyle.Fill;
			flpMenu.Location = new Point(0, 60);
			flpMenu.Margin = new Padding(2, 2, 2, 2);
			flpMenu.Name = "flpMenu";
			flpMenu.Padding = new Padding(14, 12, 14, 12);
			flpMenu.Size = new Size(700, 300);
			flpMenu.TabIndex = 1;
			// 
			// panelFooter
			// 
			panelFooter.BackColor = Color.White;
			panelFooter.Controls.Add(lblTotal);
			panelFooter.Controls.Add(btnCheckout);
			panelFooter.Dock = DockStyle.Bottom;
			panelFooter.Location = new Point(0, 360);
			panelFooter.Margin = new Padding(2, 2, 2, 2);
			panelFooter.Name = "panelFooter";
			panelFooter.Size = new Size(700, 72);
			panelFooter.TabIndex = 2;
			// 
			// lblTotal
			// 
			lblTotal.AutoSize = true;
			lblTotal.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
			lblTotal.ForeColor = Color.FromArgb(44, 62, 80);
			lblTotal.Location = new Point(21, 21);
			lblTotal.Margin = new Padding(2, 0, 2, 0);
			lblTotal.Name = "lblTotal";
			lblTotal.Size = new Size(171, 30);
			lblTotal.TabIndex = 1;
			lblTotal.Text = "ราคารวม: 0 บาท";
			// 
			// btnCheckout
			// 
			btnCheckout.BackColor = Color.FromArgb(46, 204, 113);
			btnCheckout.FlatStyle = FlatStyle.Flat;
			btnCheckout.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
			btnCheckout.ForeColor = Color.White;
			btnCheckout.Location = new Point(455, 21);
			btnCheckout.Margin = new Padding(2, 2, 2, 2);
			btnCheckout.Name = "btnCheckout";
			btnCheckout.Size = new Size(224, 36);
			btnCheckout.TabIndex = 0;
			btnCheckout.Text = "ยืนยันการสั่งซื้อ (Checkout)";
			btnCheckout.UseVisualStyleBackColor = false;
			// 
			// MenuForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(700, 432);
			Controls.Add(flpMenu);
			Controls.Add(panelFooter);
			Controls.Add(panelHeader);
			Margin = new Padding(2, 2, 2, 2);
			Name = "MenuForm";
			Text = "Menu List";
			panelHeader.ResumeLayout(false);
			panelHeader.PerformLayout();
			panelFooter.ResumeLayout(false);
			panelFooter.PerformLayout();
			ResumeLayout(false);
		}

		#endregion

		private Panel panelHeader;
        private Label lblRestaurantName;
        private FlowLayoutPanel flpMenu;
        private Panel panelFooter;
        private Label lblTotal;
        private Button btnCheckout;
        private Button btnBack;
    }
}
