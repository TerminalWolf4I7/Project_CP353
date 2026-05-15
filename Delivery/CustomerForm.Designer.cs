namespace Delivery
{
    partial class CustomerForm
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
            flpRestaurants = new FlowLayoutPanel();
            labelHeader = new Label();
            SuspendLayout();
            // 
            // flpRestaurants
            // 
            flpRestaurants.AutoScroll = true;
            flpRestaurants.BackColor = Color.WhiteSmoke;
            flpRestaurants.Dock = DockStyle.Fill;
            flpRestaurants.Location = new Point(0, 80);
            flpRestaurants.Name = "flpRestaurants";
            flpRestaurants.Padding = new Padding(20);
            flpRestaurants.Size = new Size(1272, 678);
            flpRestaurants.TabIndex = 0;
            // 
            // labelHeader
            // 
            labelHeader.BackColor = Color.FromArgb(46, 204, 113);
            labelHeader.Dock = DockStyle.Top;
            labelHeader.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            labelHeader.ForeColor = Color.White;
            labelHeader.Location = new Point(0, 0);
            labelHeader.Name = "labelHeader";
            labelHeader.Size = new Size(1272, 80);
            labelHeader.TabIndex = 1;
            labelHeader.Text = "  เลือกร้านอาหารขวัญใจคุณ";
            labelHeader.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // CustomerForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1272, 758);
            Controls.Add(flpRestaurants);
            Controls.Add(labelHeader);
            Name = "CustomerForm";
            Text = "Delivery App - Home";
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel flpRestaurants;
        private Label labelHeader;
    }
}