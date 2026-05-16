namespace Delivery.Client
{
    partial class RestaurantEditForm
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
            labelRestaurantName = new Label();
            textRestaurantName = new TextBox();
            dataGridMenu = new DataGridView();
            buttonSave = new Button();
            buttonAdd = new Button();
            buttonDelete = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridMenu).BeginInit();
            SuspendLayout();
            // 
            // labelRestaurantName
            // 
            labelRestaurantName.AutoSize = true;
            labelRestaurantName.Location = new Point(24, 24);
            labelRestaurantName.Name = "labelRestaurantName";
            labelRestaurantName.Size = new Size(141, 25);
            labelRestaurantName.TabIndex = 0;
            labelRestaurantName.Text = "Restaurant Name";
            // 
            // textRestaurantName
            // 
            textRestaurantName.Location = new Point(188, 21);
            textRestaurantName.Name = "textRestaurantName";
            textRestaurantName.Size = new Size(380, 31);
            textRestaurantName.TabIndex = 1;
            // 
            // dataGridMenu
            // 
            dataGridMenu.AllowUserToAddRows = true;
            dataGridMenu.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridMenu.Location = new Point(24, 72);
            dataGridMenu.Name = "dataGridMenu";
            dataGridMenu.RowHeadersWidth = 62;
            dataGridMenu.Size = new Size(744, 300);
            dataGridMenu.TabIndex = 2;
            // 
            // buttonSave
            // 
            buttonSave.Location = new Point(644, 388);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(124, 40);
            buttonSave.TabIndex = 5;
            buttonSave.Text = "Save";
            buttonSave.UseVisualStyleBackColor = true;
            // 
            // buttonAdd
            // 
            buttonAdd.Location = new Point(24, 388);
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Size = new Size(124, 40);
            buttonAdd.TabIndex = 3;
            buttonAdd.Text = "Add";
            buttonAdd.UseVisualStyleBackColor = true;
            // 
            // buttonDelete
            // 
            buttonDelete.Location = new Point(164, 388);
            buttonDelete.Name = "buttonDelete";
            buttonDelete.Size = new Size(124, 40);
            buttonDelete.TabIndex = 4;
            buttonDelete.Text = "Delete";
            buttonDelete.UseVisualStyleBackColor = true;
            // 
            // RestaurantEditForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(buttonDelete);
            Controls.Add(buttonAdd);
            Controls.Add(buttonSave);
            Controls.Add(dataGridMenu);
            Controls.Add(textRestaurantName);
            Controls.Add(labelRestaurantName);
            Name = "RestaurantEditForm";
            Text = "Restaurant Edit";
            ((System.ComponentModel.ISupportInitialize)dataGridMenu).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelRestaurantName;
        private TextBox textRestaurantName;
        private DataGridView dataGridMenu;
        private Button buttonSave;
        private Button buttonAdd;
        private Button buttonDelete;
    }
}
