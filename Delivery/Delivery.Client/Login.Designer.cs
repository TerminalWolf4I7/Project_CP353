using System.Drawing;
using System.Windows.Forms;

namespace Delivery.Client
{
    partial class Login
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
            panelLeft = new Panel();
            lblEmoji = new Label();
            lblBrand = new Label();
            lblTagline = new Label();
            panelRight = new Panel();
            panelCard = new Panel();
            lblWelcome = new Label();
            lblSubtitle = new Label();
            lblUserHint = new Label();
            txtUserId = new TextBox();
            btnLogin = new Button();
            lblVersion = new Label();
            panelLeft.SuspendLayout();
            panelRight.SuspendLayout();
            panelCard.SuspendLayout();
            SuspendLayout();
            // 
            // panelLeft
            // 
            panelLeft.BackColor = Color.FromArgb(39, 174, 96);
            panelLeft.Controls.Add(lblEmoji);
            panelLeft.Controls.Add(lblBrand);
            panelLeft.Controls.Add(lblTagline);
            panelLeft.Dock = DockStyle.Left;
            panelLeft.Location = new Point(0, 0);
            panelLeft.Margin = new Padding(2, 2, 2, 2);
            panelLeft.Name = "panelLeft";
            panelLeft.Size = new Size(266, 360);
            panelLeft.TabIndex = 1;
            // 
            // lblEmoji
            // 
            lblEmoji.AutoSize = true;
            lblEmoji.Font = new Font("Segoe UI Emoji", 52F);
            lblEmoji.ForeColor = Color.White;
            lblEmoji.Location = new Point(91, 90);
            lblEmoji.Margin = new Padding(2, 0, 2, 0);
            lblEmoji.Name = "lblEmoji";
            lblEmoji.Size = new Size(136, 94);
            lblEmoji.TabIndex = 0;
            lblEmoji.Text = "\U0001f6f5";
            // 
            // lblBrand
            // 
            lblBrand.AutoSize = true;
            lblBrand.Font = new Font("Segoe UI", 32F, FontStyle.Bold);
            lblBrand.ForeColor = Color.White;
            lblBrand.Location = new Point(56, 168);
            lblBrand.Margin = new Padding(2, 0, 2, 0);
            lblBrand.Name = "lblBrand";
            lblBrand.Size = new Size(222, 59);
            lblBrand.TabIndex = 1;
            lblBrand.Text = "DELIVERY";
            // 
            // lblTagline
            // 
            lblTagline.Font = new Font("Segoe UI", 11F);
            lblTagline.ForeColor = Color.FromArgb(200, 255, 220);
            lblTagline.Location = new Point(28, 216);
            lblTagline.Margin = new Padding(2, 0, 2, 0);
            lblTagline.Name = "lblTagline";
            lblTagline.Size = new Size(210, 42);
            lblTagline.TabIndex = 2;
            lblTagline.Text = "อาหารอร่อย ส่งถึงมือคุณ\nเร็วกว่าที่คิด 🔥";
            lblTagline.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panelRight
            // 
            panelRight.BackColor = Color.FromArgb(245, 248, 250);
            panelRight.Controls.Add(panelCard);
            panelRight.Controls.Add(lblVersion);
            panelRight.Dock = DockStyle.Fill;
            panelRight.Location = new Point(266, 0);
            panelRight.Margin = new Padding(2, 2, 2, 2);
            panelRight.Name = "panelRight";
            panelRight.Size = new Size(322, 360);
            panelRight.TabIndex = 0;
            // 
            // panelCard
            // 
            panelCard.BackColor = Color.White;
            panelCard.Controls.Add(lblWelcome);
            panelCard.Controls.Add(lblSubtitle);
            panelCard.Controls.Add(lblUserHint);
            panelCard.Controls.Add(txtUserId);
            panelCard.Controls.Add(btnLogin);
            panelCard.Location = new Point(35, 60);
            panelCard.Margin = new Padding(2, 2, 2, 2);
            panelCard.Name = "panelCard";
            panelCard.Size = new Size(252, 228);
            panelCard.TabIndex = 0;
            // 
            // lblWelcome
            // 
            lblWelcome.AutoSize = true;
            lblWelcome.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblWelcome.ForeColor = Color.FromArgb(30, 30, 30);
            lblWelcome.Location = new Point(21, 21);
            lblWelcome.Margin = new Padding(2, 0, 2, 0);
            lblWelcome.Name = "lblWelcome";
            lblWelcome.Size = new Size(206, 37);
            lblWelcome.TabIndex = 0;
            lblWelcome.Text = "ยินดีต้อนรับ! 👋";
            // 
            // lblSubtitle
            // 
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = Color.Gray;
            lblSubtitle.Location = new Point(21, 51);
            lblSubtitle.Margin = new Padding(2, 0, 2, 0);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(161, 19);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "เข้าสู่ระบบเพื่อสั่งอาหารอร่อยๆ";
            // 
            // lblUserHint
            // 
            lblUserHint.AutoSize = true;
            lblUserHint.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblUserHint.ForeColor = Color.FromArgb(39, 174, 96);
            lblUserHint.Location = new Point(21, 87);
            lblUserHint.Margin = new Padding(2, 0, 2, 0);
            lblUserHint.Name = "lblUserHint";
            lblUserHint.Size = new Size(48, 13);
            lblUserHint.TabIndex = 2;
            lblUserHint.Text = "USER ID";
            // 
            // txtUserId
            // 
            txtUserId.BorderStyle = BorderStyle.FixedSingle;
            txtUserId.Font = new Font("Segoe UI", 13F);
            txtUserId.Location = new Point(21, 102);
            txtUserId.Margin = new Padding(2, 2, 2, 2);
            txtUserId.Name = "txtUserId";
            txtUserId.PlaceholderText = "เช่น  1, 2, 3...";
            txtUserId.Size = new Size(211, 31);
            txtUserId.TabIndex = 0;
            txtUserId.TextChanged += txtUserId_TextChanged;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.FromArgb(39, 174, 96);
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(21, 150);
            btnLogin.Margin = new Padding(2, 2, 2, 2);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(210, 33);
            btnLogin.TabIndex = 1;
            btnLogin.Text = "เข้าสู่ระบบ  →";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // lblVersion
            // 
            lblVersion.AutoSize = true;
            lblVersion.Font = new Font("Segoe UI", 8F);
            lblVersion.ForeColor = Color.LightGray;
            lblVersion.Location = new Point(63, 324);
            lblVersion.Margin = new Padding(2, 0, 2, 0);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(178, 13);
            lblVersion.TabIndex = 1;
            lblVersion.Text = "Delivery App v1.0  |  CP353 Project";
            // 
            // Login
            // 
            AcceptButton = btnLogin;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(588, 360);
            Controls.Add(panelRight);
            Controls.Add(panelLeft);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(2, 2, 2, 2);
            MaximizeBox = false;
            Name = "Login";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Delivery App — Login";
            panelLeft.ResumeLayout(false);
            panelLeft.PerformLayout();
            panelRight.ResumeLayout(false);
            panelRight.PerformLayout();
            panelCard.ResumeLayout(false);
            panelCard.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelLeft;
        private Label lblBrand;
        private Label lblTagline;
        private Label lblEmoji;
        private Panel panelRight;
        private Panel panelCard;
        private Label lblWelcome;
        private Label lblSubtitle;
        private Label lblUserHint;
        private TextBox txtUserId;
        private Button btnLogin;
        private Label lblVersion;
    }
}
