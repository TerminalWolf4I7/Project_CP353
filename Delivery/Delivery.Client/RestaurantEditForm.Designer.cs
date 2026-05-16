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
			labelRestaurantName.Location = new Point(17, 14);
			labelRestaurantName.Margin = new Padding(2, 0, 2, 0);
			labelRestaurantName.Name = "labelRestaurantName";
			labelRestaurantName.Size = new Size(98, 15);
			labelRestaurantName.TabIndex = 0;
			labelRestaurantName.Text = "Restaurant Name";
			// 
			// textRestaurantName
			// 
			textRestaurantName.Location = new Point(132, 13);
			textRestaurantName.Margin = new Padding(2, 2, 2, 2);
			textRestaurantName.Name = "textRestaurantName";
			textRestaurantName.Size = new Size(267, 23);
			textRestaurantName.TabIndex = 1;
			// 
			// dataGridMenu
			// 
			dataGridMenu.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridMenu.Location = new Point(17, 43);
			dataGridMenu.Margin = new Padding(2, 2, 2, 2);
			dataGridMenu.Name = "dataGridMenu";
			dataGridMenu.RowHeadersWidth = 62;
			dataGridMenu.Size = new Size(521, 180);
			dataGridMenu.TabIndex = 2;
			// 
			// buttonSave
			// 
			buttonSave.Location = new Point(451, 233);
			buttonSave.Margin = new Padding(2, 2, 2, 2);
			buttonSave.Name = "buttonSave";
			buttonSave.Size = new Size(87, 24);
			buttonSave.TabIndex = 5;
			buttonSave.Text = "Save";
			buttonSave.UseVisualStyleBackColor = true;
			// 
			// buttonAdd
			// 
			buttonAdd.Location = new Point(17, 233);
			buttonAdd.Margin = new Padding(2, 2, 2, 2);
			buttonAdd.Name = "buttonAdd";
			buttonAdd.Size = new Size(87, 24);
			buttonAdd.TabIndex = 3;
			buttonAdd.Text = "Add";
			buttonAdd.UseVisualStyleBackColor = true;
			// 
			// buttonDelete
			// 
			buttonDelete.Location = new Point(115, 233);
			buttonDelete.Margin = new Padding(2, 2, 2, 2);
			buttonDelete.Name = "buttonDelete";
			buttonDelete.Size = new Size(87, 24);
			buttonDelete.TabIndex = 4;
			buttonDelete.Text = "Delete";
			buttonDelete.UseVisualStyleBackColor = true;
			// 
			// RestaurantEditForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(561, 276);
			Controls.Add(buttonDelete);
			Controls.Add(buttonAdd);
			Controls.Add(buttonSave);
			Controls.Add(dataGridMenu);
			Controls.Add(textRestaurantName);
			Controls.Add(labelRestaurantName);
			Margin = new Padding(2, 2, 2, 2);
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
