namespace Delivery
{
    partial class Login
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panelMain = new Panel();
            lblUserHint = new Label();
            lblTitle = new Label();
            lblSubtitle = new Label();
            txtUserId = new TextBox();
            btnLogin = new Button();
            panelMain.SuspendLayout();
            SuspendLayout();
            // 
            // panelMain
            // 
            panelMain.BackColor = Color.White;
            panelMain.BorderStyle = BorderStyle.FixedSingle;
            panelMain.Controls.Add(lblUserHint);
            panelMain.Controls.Add(lblTitle);
            panelMain.Controls.Add(lblSubtitle);
            panelMain.Controls.Add(txtUserId);
            panelMain.Controls.Add(btnLogin);
            panelMain.Location = new Point(150, 80);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(500, 400);
            panelMain.TabIndex = 0;
            // 
            // lblUserHint
            // 
            lblUserHint.AutoSize = true;
            lblUserHint.Font = new Font("Segoe UI", 8F);
            lblUserHint.ForeColor = Color.Silver;
            lblUserHint.Location = new Point(70, 165);
            lblUserHint.Name = "lblUserHint";
            lblUserHint.Size = new Size(61, 21);
            lblUserHint.TabIndex = 4;
            lblUserHint.Text = "User ID";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 28F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(46, 204, 113);
            lblTitle.Location = new Point(90, 40);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(330, 74);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "DELIVERY";
            // 
            // lblSubtitle
            // 
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = Color.Gray;
            lblSubtitle.Location = new Point(135, 110);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(244, 28);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "Welcome to the future app";
            // 
            // txtUserId
            // 
            txtUserId.Font = new Font("Segoe UI", 14F);
            txtUserId.Location = new Point(70, 190);
            txtUserId.Name = "txtUserId";
            txtUserId.PlaceholderText = "กรอก User ID ของคุณ";
            txtUserId.Size = new Size(360, 45);
            txtUserId.TabIndex = 2;
            txtUserId.TextAlign = HorizontalAlignment.Center;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.FromArgb(46, 204, 113);
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(70, 260);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(360, 60);
            btnLogin.TabIndex = 3;
            btnLogin.Text = "เข้าสู่ระบบ";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // Login
            // 
            AcceptButton = btnLogin;
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(240, 240, 240);
            ClientSize = new Size(800, 560);
            Controls.Add(panelMain);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "Login";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Delivery App - Login";
            panelMain.ResumeLayout(false);
            panelMain.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelMain;
        private Label lblUserHint;
        private Label lblTitle;
        private Label lblSubtitle;
        private TextBox txtUserId;
        private Button btnLogin;
    }
}
