using System.Drawing;
using System.Windows.Forms;

namespace Delivery
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
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(1000, 100);
            panelHeader.TabIndex = 0;
            // 
            // lblRestaurantName
            // 
            lblRestaurantName.AutoSize = true;
            lblRestaurantName.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblRestaurantName.ForeColor = Color.White;
            lblRestaurantName.Location = new Point(20, 20);
            lblRestaurantName.Name = "lblRestaurantName";
            lblRestaurantName.Size = new Size(365, 60);
            lblRestaurantName.TabIndex = 0;
            lblRestaurantName.Text = "ชื่อร้านอาหาร...";
            // 
            // flpMenu
            // 
            flpMenu.AutoScroll = true;
            flpMenu.BackColor = Color.WhiteSmoke;
            flpMenu.Dock = DockStyle.Fill;
            flpMenu.Location = new Point(0, 100);
            flpMenu.Name = "flpMenu";
            flpMenu.Padding = new Padding(20);
            flpMenu.Size = new Size(1000, 500);
            flpMenu.TabIndex = 1;
            // 
            // panelFooter
            // 
            panelFooter.BackColor = Color.White;
            panelFooter.Controls.Add(lblTotal);
            panelFooter.Controls.Add(btnCheckout);
            panelFooter.Dock = DockStyle.Bottom;
            panelFooter.Location = new Point(0, 600);
            panelFooter.Name = "panelFooter";
            panelFooter.Size = new Size(1000, 120);
            panelFooter.TabIndex = 2;
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTotal.ForeColor = Color.FromArgb(44, 62, 80);
            lblTotal.Location = new Point(30, 35);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(244, 45);
            lblTotal.TabIndex = 1;
            lblTotal.Text = "ราคารวม: 0 บาท";
            // 
            // btnCheckout
            // 
            btnCheckout.BackColor = Color.FromArgb(46, 204, 113);
            btnCheckout.FlatStyle = FlatStyle.Flat;
            btnCheckout.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnCheckout.ForeColor = Color.White;
            btnCheckout.Location = new Point(650, 25);
            btnCheckout.Name = "btnCheckout";
            btnCheckout.Size = new Size(320, 70);
            btnCheckout.TabIndex = 0;
            btnCheckout.Text = "ยืนยันการสั่งซื้อ (Checkout)";
            btnCheckout.UseVisualStyleBackColor = false;
            // 
            // MenuForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 720);
            Controls.Add(flpMenu);
            Controls.Add(panelFooter);
            Controls.Add(panelHeader);
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
    }
}
