namespace Delivery
{
    partial class RestaurantOrdersForm
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
            dataGridOrders = new DataGridView();
            buttonAccept = new Button();
            buttonDecline = new Button();
            buttonFinishCooking = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridOrders).BeginInit();
            SuspendLayout();
            // 
            // dataGridOrders
            // 
            dataGridOrders.AllowUserToAddRows = false;
            dataGridOrders.AllowUserToDeleteRows = false;
            dataGridOrders.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridOrders.Location = new Point(24, 24);
            dataGridOrders.MultiSelect = false;
            dataGridOrders.Name = "dataGridOrders";
            dataGridOrders.ReadOnly = true;
            dataGridOrders.RowHeadersWidth = 62;
            dataGridOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridOrders.Size = new Size(752, 360);
            dataGridOrders.TabIndex = 0;
            // 
            // buttonAccept
            // 
            buttonAccept.Location = new Point(24, 404);
            buttonAccept.Name = "buttonAccept";
            buttonAccept.Size = new Size(140, 40);
            buttonAccept.TabIndex = 1;
            buttonAccept.Text = "Accept";
            buttonAccept.UseVisualStyleBackColor = true;
            // 
            // buttonDecline
            // 
            buttonDecline.Location = new Point(188, 404);
            buttonDecline.Name = "buttonDecline";
            buttonDecline.Size = new Size(140, 40);
            buttonDecline.TabIndex = 2;
            buttonDecline.Text = "Decline";
            buttonDecline.UseVisualStyleBackColor = true;
            // 
            // buttonFinishCooking
            // 
            buttonFinishCooking.Location = new Point(352, 404);
            buttonFinishCooking.Name = "buttonFinishCooking";
            buttonFinishCooking.Size = new Size(180, 40);
            buttonFinishCooking.TabIndex = 3;
            buttonFinishCooking.Text = "Finish Cooking";
            buttonFinishCooking.UseVisualStyleBackColor = true;
            // 
            // RestaurantOrdersForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 470);
            Controls.Add(buttonFinishCooking);
            Controls.Add(buttonDecline);
            Controls.Add(buttonAccept);
            Controls.Add(dataGridOrders);
            Name = "RestaurantOrdersForm";
            Text = "Restaurant Orders";
            ((System.ComponentModel.ISupportInitialize)dataGridOrders).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridOrders;
        private Button buttonAccept;
        private Button buttonDecline;
        private Button buttonFinishCooking;
    }
}
