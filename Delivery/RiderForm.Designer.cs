namespace Delivery
{
    partial class RiderForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.DataGridView dataGridOrders;
        private System.Windows.Forms.Button buttonViewDetails;
        private System.Windows.Forms.Button buttonAcceptOrder;
        private System.Windows.Forms.Button buttonRiderOrder;
        private Button buttonLogout;

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
            buttonViewDetails = new Button();
            buttonAcceptOrder = new Button();
            buttonRiderOrder = new Button();
            buttonLogout = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridOrders).BeginInit();
            SuspendLayout();
            // 
            // dataGridOrders
            // 
            dataGridOrders.AllowUserToAddRows = false;
            dataGridOrders.AllowUserToDeleteRows = false;
            dataGridOrders.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridOrders.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridOrders.Location = new Point(12, 12);
            dataGridOrders.MultiSelect = false;
            dataGridOrders.Name = "dataGridOrders";
            dataGridOrders.ReadOnly = true;
            dataGridOrders.RowHeadersVisible = false;
            dataGridOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridOrders.Size = new Size(760, 400);
            dataGridOrders.TabIndex = 0;
            // 
            // buttonViewDetails
            // 
            buttonViewDetails.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonViewDetails.Location = new Point(12, 420);
            buttonViewDetails.Name = "buttonViewDetails";
            buttonViewDetails.Size = new Size(120, 35);
            buttonViewDetails.TabIndex = 1;
            buttonViewDetails.Text = "View Details";
            buttonViewDetails.UseVisualStyleBackColor = true;
            // 
            // buttonAcceptOrder
            // 
            buttonAcceptOrder.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonAcceptOrder.Location = new Point(264, 420);
            buttonAcceptOrder.Name = "buttonAcceptOrder";
            buttonAcceptOrder.Size = new Size(120, 35);
            buttonAcceptOrder.TabIndex = 2;
            buttonAcceptOrder.Text = "Accept Order";
            buttonAcceptOrder.UseVisualStyleBackColor = true;
            // 
            // buttonRiderOrder
            // 
            buttonRiderOrder.Location = new Point(138, 420);
            buttonRiderOrder.Name = "buttonRiderOrder";
            buttonRiderOrder.Size = new Size(120, 35);
            buttonRiderOrder.TabIndex = 3;
            buttonRiderOrder.Text = "Rider Order";
            buttonRiderOrder.UseVisualStyleBackColor = true;
            // 
            // buttonLogout
            // 
            buttonLogout.Location = new Point(652, 424);
            buttonLogout.Name = "buttonLogout";
            buttonLogout.Size = new Size(120, 35);
            buttonLogout.TabIndex = 3;
            buttonLogout.Text = "Logout";
            buttonLogout.UseVisualStyleBackColor = true;
            // 
            // RiderForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 471);
            Controls.Add(buttonLogout);
            Controls.Add(buttonRiderOrder);
            Controls.Add(buttonAcceptOrder);
            Controls.Add(buttonViewDetails);
            Controls.Add(dataGridOrders);
            Name = "RiderForm";
            Text = "Rider";
            ((System.ComponentModel.ISupportInitialize)dataGridOrders).EndInit();
            ResumeLayout(false);
        }

        #endregion
    }
}