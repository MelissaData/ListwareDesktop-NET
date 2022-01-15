namespace ListwareDesktop.Windows
{
    partial class SetConfigurationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetConfigurationForm));
            this.setConfigurationFormDataGridView = new System.Windows.Forms.DataGridView();
            this.setConfigurationFormDGVOptionName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.setConfigurationFormDGVOptionValues = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.setConfigurationFormSaveButton = new System.Windows.Forms.Button();
            this.setConfigurationFormCancelButton = new System.Windows.Forms.Button();
            this.setConfigurationFormCheckedListBox = new System.Windows.Forms.CheckedListBox();
            ((System.ComponentModel.ISupportInitialize)(this.setConfigurationFormDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // setConfigurationFormDataGridView
            // 
            this.setConfigurationFormDataGridView.AllowUserToAddRows = false;
            this.setConfigurationFormDataGridView.AllowUserToDeleteRows = false;
            this.setConfigurationFormDataGridView.AllowUserToResizeRows = false;
            this.setConfigurationFormDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.setConfigurationFormDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.setConfigurationFormDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.setConfigurationFormDGVOptionName,
            this.setConfigurationFormDGVOptionValues});
            this.setConfigurationFormDataGridView.Dock = System.Windows.Forms.DockStyle.Top;
            this.setConfigurationFormDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.setConfigurationFormDataGridView.Location = new System.Drawing.Point(0, 0);
            this.setConfigurationFormDataGridView.Name = "setConfigurationFormDataGridView";
            this.setConfigurationFormDataGridView.RowHeadersVisible = false;
            this.setConfigurationFormDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.setConfigurationFormDataGridView.Size = new System.Drawing.Size(401, 231);
            this.setConfigurationFormDataGridView.TabIndex = 0;
            this.setConfigurationFormDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.setConfigurationFormDataGridView_CellClick);
            // 
            // setConfigurationFormDGVOptionName
            // 
            this.setConfigurationFormDGVOptionName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.setConfigurationFormDGVOptionName.HeaderText = "Option Name";
            this.setConfigurationFormDGVOptionName.Name = "setConfigurationFormDGVOptionName";
            this.setConfigurationFormDGVOptionName.ReadOnly = true;
            this.setConfigurationFormDGVOptionName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.setConfigurationFormDGVOptionName.Width = 75;
            // 
            // setConfigurationFormDGVOptionValues
            // 
            this.setConfigurationFormDGVOptionValues.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.setConfigurationFormDGVOptionValues.HeaderText = "Value";
            this.setConfigurationFormDGVOptionValues.Name = "setConfigurationFormDGVOptionValues";
            this.setConfigurationFormDGVOptionValues.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.setConfigurationFormDGVOptionValues.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // setConfigurationFormSaveButton
            // 
            this.setConfigurationFormSaveButton.Location = new System.Drawing.Point(67, 235);
            this.setConfigurationFormSaveButton.Name = "setConfigurationFormSaveButton";
            this.setConfigurationFormSaveButton.Size = new System.Drawing.Size(75, 23);
            this.setConfigurationFormSaveButton.TabIndex = 1;
            this.setConfigurationFormSaveButton.Text = "Save";
            this.setConfigurationFormSaveButton.UseVisualStyleBackColor = true;
            this.setConfigurationFormSaveButton.Click += new System.EventHandler(this.setConfigurationFormSaveButton_Click);
            // 
            // setConfigurationFormCancelButton
            // 
            this.setConfigurationFormCancelButton.Location = new System.Drawing.Point(252, 235);
            this.setConfigurationFormCancelButton.Name = "setConfigurationFormCancelButton";
            this.setConfigurationFormCancelButton.Size = new System.Drawing.Size(75, 23);
            this.setConfigurationFormCancelButton.TabIndex = 2;
            this.setConfigurationFormCancelButton.Text = "Cancel";
            this.setConfigurationFormCancelButton.UseVisualStyleBackColor = true;
            this.setConfigurationFormCancelButton.Click += new System.EventHandler(this.setConfigurationFormCancelButton_Click);
            // 
            // setConfigurationFormCheckedListBox
            // 
            this.setConfigurationFormCheckedListBox.CheckOnClick = true;
            this.setConfigurationFormCheckedListBox.FormattingEnabled = true;
            this.setConfigurationFormCheckedListBox.Location = new System.Drawing.Point(207, 88);
            this.setConfigurationFormCheckedListBox.Name = "setConfigurationFormCheckedListBox";
            this.setConfigurationFormCheckedListBox.Size = new System.Drawing.Size(120, 94);
            this.setConfigurationFormCheckedListBox.TabIndex = 3;
            this.setConfigurationFormCheckedListBox.Visible = false;
            // 
            // SetConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(401, 267);
            this.Controls.Add(this.setConfigurationFormCheckedListBox);
            this.Controls.Add(this.setConfigurationFormCancelButton);
            this.Controls.Add(this.setConfigurationFormSaveButton);
            this.Controls.Add(this.setConfigurationFormDataGridView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SetConfigurationForm";
            this.Text = "Set Configuration";
            ((System.ComponentModel.ISupportInitialize)(this.setConfigurationFormDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView setConfigurationFormDataGridView;
        private System.Windows.Forms.Button setConfigurationFormSaveButton;
        private System.Windows.Forms.Button setConfigurationFormCancelButton;
        private System.Windows.Forms.CheckedListBox setConfigurationFormCheckedListBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn setConfigurationFormDGVOptionName;
        private System.Windows.Forms.DataGridViewTextBoxColumn setConfigurationFormDGVOptionValues;
    }
}