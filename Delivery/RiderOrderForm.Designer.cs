namespace Delivery
{
    partial class RiderOrderForm
    {
        private System.ComponentModel.IContainer components = null;

        private DataGridView dataGridOrder;

        private Button buttonComplete;

        private Button buttonBack;

        protected override void Dispose(
            bool disposing)
        {
            if (disposing &&
                components != null)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }



        private void InitializeComponent()
        {
            dataGridOrder = new DataGridView();
            buttonComplete = new Button();
            buttonBack = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridOrder).BeginInit();
            SuspendLayout();
            // 
            // dataGridOrder
            // 
            dataGridOrder.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridOrder.Location = new Point(12, 12);
            dataGridOrder.Name = "dataGridOrder";
            dataGridOrder.ReadOnly = true;
            dataGridOrder.Size = new Size(760, 380);
            dataGridOrder.TabIndex = 0;
            // 
            // buttonComplete
            // 
            buttonComplete.Location = new Point(12, 410);
            buttonComplete.Name = "buttonComplete";
            buttonComplete.Size = new Size(150, 35);
            buttonComplete.TabIndex = 1;
            buttonComplete.Text = "Deliver Success";
            buttonComplete.Click += buttonComplete_Click_1;
            // 
            // buttonBack
            // 
            buttonBack.Location = new Point(180, 410);
            buttonBack.Name = "buttonBack";
            buttonBack.Size = new Size(150, 35);
            buttonBack.TabIndex = 2;
            buttonBack.Text = "Back";
            buttonBack.UseVisualStyleBackColor = true;
            // 
            // RiderOrderForm
            // 
            ClientSize = new Size(784, 461);
            Controls.Add(buttonBack);
            Controls.Add(buttonComplete);
            Controls.Add(dataGridOrder);
            Name = "RiderOrderForm";
            Text = "My Delivery";
            ((System.ComponentModel.ISupportInitialize)dataGridOrder).EndInit();
            ResumeLayout(false);
        }
    }
}