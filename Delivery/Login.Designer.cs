using System.Drawing;
using System.Windows.Forms;

namespace Delivery
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
            lblBrand = new Label();
            lblTagline = new Label();
            lblEmoji = new Label();
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

            // === panelLeft (Green Branding) ===
            panelLeft.BackColor = Color.FromArgb(39, 174, 96);
            panelLeft.Dock = DockStyle.Left;
            panelLeft.Size = new Size(380, 600);
            panelLeft.Controls.Add(lblEmoji);
            panelLeft.Controls.Add(lblBrand);
            panelLeft.Controls.Add(lblTagline);

            // lblEmoji
            lblEmoji.Text = "🛵";
            lblEmoji.Font = new Font("Segoe UI Emoji", 52F);
            lblEmoji.ForeColor = Color.White;
            lblEmoji.AutoSize = true;
            lblEmoji.Location = new Point(130, 150);
            lblEmoji.Name = "lblEmoji";

            // lblBrand
            lblBrand.Text = "DELIVERY";
            lblBrand.Font = new Font("Segoe UI", 32F, FontStyle.Bold);
            lblBrand.ForeColor = Color.White;
            lblBrand.AutoSize = true;
            lblBrand.Location = new Point(80, 280);
            lblBrand.Name = "lblBrand";

            // lblTagline
            lblTagline.Text = "อาหารอร่อย ส่งถึงมือคุณ\nเร็วกว่าที่คิด 🔥";
            lblTagline.Font = new Font("Segoe UI", 11F);
            lblTagline.ForeColor = Color.FromArgb(200, 255, 220);
            lblTagline.Size = new Size(300, 70);
            lblTagline.Location = new Point(40, 360);
            lblTagline.Name = "lblTagline";
            lblTagline.TextAlign = ContentAlignment.MiddleCenter;

            // === panelRight (White Background) ===
            panelRight.BackColor = Color.FromArgb(245, 248, 250);
            panelRight.Dock = DockStyle.Fill;
            panelRight.Controls.Add(panelCard);
            panelRight.Controls.Add(lblVersion);

            // === panelCard (Login Card) ===
            panelCard.BackColor = Color.White;
            panelCard.Size = new Size(360, 380);
            panelCard.Location = new Point(50, 100);
            panelCard.Name = "panelCard";
            panelCard.Controls.Add(lblWelcome);
            panelCard.Controls.Add(lblSubtitle);
            panelCard.Controls.Add(lblUserHint);
            panelCard.Controls.Add(txtUserId);
            panelCard.Controls.Add(btnLogin);

            // lblWelcome
            lblWelcome.Text = "ยินดีต้อนรับ! 👋";
            lblWelcome.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblWelcome.ForeColor = Color.FromArgb(30, 30, 30);
            lblWelcome.AutoSize = true;
            lblWelcome.Location = new Point(30, 35);
            lblWelcome.Name = "lblWelcome";

            // lblSubtitle
            lblSubtitle.Text = "เข้าสู่ระบบเพื่อสั่งอาหารอร่อยๆ";
            lblSubtitle.Font = new Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = Color.Gray;
            lblSubtitle.AutoSize = true;
            lblSubtitle.Location = new Point(30, 85);
            lblSubtitle.Name = "lblSubtitle";

            // lblUserHint
            lblUserHint.Text = "USER ID";
            lblUserHint.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblUserHint.ForeColor = Color.FromArgb(39, 174, 96);
            lblUserHint.AutoSize = true;
            lblUserHint.Location = new Point(30, 145);
            lblUserHint.Name = "lblUserHint";

            // txtUserId
            txtUserId.Font = new Font("Segoe UI", 13F);
            txtUserId.Location = new Point(30, 170);
            txtUserId.Name = "txtUserId";
            txtUserId.PlaceholderText = "เช่น  1, 2, 3...";
            txtUserId.Size = new Size(300, 40);
            txtUserId.BorderStyle = BorderStyle.FixedSingle;
            txtUserId.TabIndex = 0;

            // btnLogin
            btnLogin.BackColor = Color.FromArgb(39, 174, 96);
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(30, 250);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(300, 55);
            btnLogin.TabIndex = 1;
            btnLogin.Text = "เข้าสู่ระบบ  →";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.Click += btnLogin_Click;

            // lblVersion
            lblVersion.Text = "Delivery App v1.0  |  CP353 Project";
            lblVersion.Font = new Font("Segoe UI", 8F);
            lblVersion.ForeColor = Color.LightGray;
            lblVersion.AutoSize = true;
            lblVersion.Location = new Point(90, 540);
            lblVersion.Name = "lblVersion";

            // === Form ===
            AcceptButton = btnLogin;
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(840, 600);
            Controls.Add(panelRight);
            Controls.Add(panelLeft);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "Login";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Delivery App — Login";

            panelLeft.ResumeLayout(false);
            panelLeft.PerformLayout();
            panelCard.ResumeLayout(false);
            panelCard.PerformLayout();
            panelRight.ResumeLayout(false);
            panelRight.PerformLayout();
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
