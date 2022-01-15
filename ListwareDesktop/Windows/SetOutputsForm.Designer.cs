namespace ListwareDesktop.Windows
{
    partial class SetOutputsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetOutputsForm));
            this.setOutputsFormDataGridView = new System.Windows.Forms.DataGridView();
            this.FieldName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SelectColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.setOutputsFormSaveButton = new System.Windows.Forms.Button();
            this.setOutputsFormCancelButton = new System.Windows.Forms.Button();
            this.setOutputsFormSelectAllButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.setOutputsFormDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // setOutputsFormDataGridView
            // 
            this.setOutputsFormDataGridView.AllowUserToAddRows = false;
            this.setOutputsFormDataGridView.AllowUserToDeleteRows = false;
            this.setOutputsFormDataGridView.AllowUserToResizeRows = false;
            this.setOutputsFormDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.setOutputsFormDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.setOutputsFormDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FieldName,
            this.SelectColumn});
            this.setOutputsFormDataGridView.Dock = System.Windows.Forms.DockStyle.Top;
            this.setOutputsFormDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.setOutputsFormDataGridView.Location = new System.Drawing.Point(0, 0);
            this.setOutputsFormDataGridView.Name = "setOutputsFormDataGridView";
            this.setOutputsFormDataGridView.RowHeadersVisible = false;
            this.setOutputsFormDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.setOutputsFormDataGridView.Size = new System.Drawing.Size(398, 448);
            this.setOutputsFormDataGridView.TabIndex = 0;
            // 
            // FieldName
            // 
            this.FieldName.HeaderText = "FieldName";
            this.FieldName.Name = "FieldName";
            this.FieldName.ReadOnly = true;
            // 
            // SelectColumn
            // 
            this.SelectColumn.FalseValue = "false";
            this.SelectColumn.HeaderText = "Select";
            this.SelectColumn.Name = "SelectColumn";
            this.SelectColumn.TrueValue = "true";
            // 
            // setOutputsFormSaveButton
            // 
            this.setOutputsFormSaveButton.Location = new System.Drawing.Point(62, 455);
            this.setOutputsFormSaveButton.Name = "setOutputsFormSaveButton";
            this.setOutputsFormSaveButton.Size = new System.Drawing.Size(75, 23);
            this.setOutputsFormSaveButton.TabIndex = 1;
            this.setOutputsFormSaveButton.Text = "Save";
            this.setOutputsFormSaveButton.UseVisualStyleBackColor = true;
            this.setOutputsFormSaveButton.Click += new System.EventHandler(this.setOutputsFormSaveButton_Click);
            // 
            // setOutputsFormCancelButton
            // 
            this.setOutputsFormCancelButton.Location = new System.Drawing.Point(263, 455);
            this.setOutputsFormCancelButton.Name = "setOutputsFormCancelButton";
            this.setOutputsFormCancelButton.Size = new System.Drawing.Size(75, 23);
            this.setOutputsFormCancelButton.TabIndex = 2;
            this.setOutputsFormCancelButton.Text = "Cancel";
            this.setOutputsFormCancelButton.UseVisualStyleBackColor = true;
            this.setOutputsFormCancelButton.Click += new System.EventHandler(this.setOutputsFormCancelButton_Click);
            // 
            // setOutputsFormSelectAllButton
            // 
            this.setOutputsFormSelectAllButton.Location = new System.Drawing.Point(162, 455);
            this.setOutputsFormSelectAllButton.Name = "setOutputsFormSelectAllButton";
            this.setOutputsFormSelectAllButton.Size = new System.Drawing.Size(73, 23);
            this.setOutputsFormSelectAllButton.TabIndex = 3;
            this.setOutputsFormSelectAllButton.Text = "Select All";
            this.setOutputsFormSelectAllButton.UseVisualStyleBackColor = true;
            this.setOutputsFormSelectAllButton.Click += new System.EventHandler(this.setOutputsFormSelectAllButton_Click);
            // 
            // SetOutputsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(398, 490);
            this.Controls.Add(this.setOutputsFormSelectAllButton);
            this.Controls.Add(this.setOutputsFormCancelButton);
            this.Controls.Add(this.setOutputsFormSaveButton);
            this.Controls.Add(this.setOutputsFormDataGridView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SetOutputsForm";
            this.Text = "Set Outputs";
            ((System.ComponentModel.ISupportInitialize)(this.setOutputsFormDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView setOutputsFormDataGridView;
        private System.Windows.Forms.Button setOutputsFormSaveButton;
        private System.Windows.Forms.Button setOutputsFormCancelButton;
        private System.Windows.Forms.Button setOutputsFormSelectAllButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn FieldName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SelectColumn;
    }
}