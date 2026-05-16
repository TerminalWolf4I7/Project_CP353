namespace Delivery.Client
{
    partial class RestaurantForm
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
            label1 = new Label();
            button1 = new Button();
            button2 = new Button();
            buttonLogout = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(567, 44);
            label1.Name = "label1";
            label1.Size = new Size(54, 25);
            label1.TabIndex = 0;
            label1.Text = "ร้าน 1";
            // 
            // button1
            // 
            button1.Location = new Point(362, 154);
            button1.Name = "button1";
            button1.Size = new Size(489, 142);
            button1.TabIndex = 1;
            button1.Text = "รับ order";
            button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Location = new Point(362, 335);
            button2.Name = "button2";
            button2.Size = new Size(489, 142);
            button2.TabIndex = 2;
            button2.Text = "edit";
            button2.UseVisualStyleBackColor = true;
            // 
            // buttonLogout
            // 
            buttonLogout.Location = new Point(362, 516);
            buttonLogout.Name = "buttonLogout";
            buttonLogout.Size = new Size(489, 80);
            buttonLogout.TabIndex = 3;
            buttonLogout.Text = "Logout";
            buttonLogout.UseVisualStyleBackColor = true;
            // 
            // RestaurantForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1239, 772);
            Controls.Add(buttonLogout);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label1);
            Name = "RestaurantForm";
            Text = "Restaurant";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button button1;
        private Button button2;
        private Button buttonLogout;
    }
}
