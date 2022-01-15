namespace ListwareDesktop.Windows
{
    partial class SetInputsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetInputsForm));
            this.setInputsFormDataGridView = new System.Windows.Forms.DataGridView();
            this.setInputsFormSaveButton = new System.Windows.Forms.Button();
            this.setInputsFormCancelButton = new System.Windows.Forms.Button();
            this.Inputs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.setInputsFormDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // setInputsFormDataGridView
            // 
            this.setInputsFormDataGridView.AllowUserToAddRows = false;
            this.setInputsFormDataGridView.AllowUserToDeleteRows = false;
            this.setInputsFormDataGridView.AllowUserToResizeRows = false;
            this.setInputsFormDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.setInputsFormDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.setInputsFormDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Inputs});
            this.setInputsFormDataGridView.Dock = System.Windows.Forms.DockStyle.Top;
            this.setInputsFormDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.setInputsFormDataGridView.Location = new System.Drawing.Point(0, 0);
            this.setInputsFormDataGridView.Name = "setInputsFormDataGridView";
            this.setInputsFormDataGridView.RowHeadersVisible = false;
            this.setInputsFormDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.setInputsFormDataGridView.Size = new System.Drawing.Size(284, 436);
            this.setInputsFormDataGridView.TabIndex = 0;
            // 
            // setInputsFormSaveButton
            // 
            this.setInputsFormSaveButton.Location = new System.Drawing.Point(36, 442);
            this.setInputsFormSaveButton.Name = "setInputsFormSaveButton";
            this.setInputsFormSaveButton.Size = new System.Drawing.Size(75, 23);
            this.setInputsFormSaveButton.TabIndex = 1;
            this.setInputsFormSaveButton.Text = "Save";
            this.setInputsFormSaveButton.UseVisualStyleBackColor = true;
            this.setInputsFormSaveButton.Click += new System.EventHandler(this.setInputsFormSaveButton_Click);
            // 
            // setInputsFormCancelButton
            // 
            this.setInputsFormCancelButton.Location = new System.Drawing.Point(179, 442);
            this.setInputsFormCancelButton.Name = "setInputsFormCancelButton";
            this.setInputsFormCancelButton.Size = new System.Drawing.Size(75, 23);
            this.setInputsFormCancelButton.TabIndex = 2;
            this.setInputsFormCancelButton.Text = "Cancel";
            this.setInputsFormCancelButton.UseVisualStyleBackColor = true;
            this.setInputsFormCancelButton.Click += new System.EventHandler(this.setInputsFormCancelButton_Click);
            // 
            // Inputs
            // 
            this.Inputs.HeaderText = "Inputs";
            this.Inputs.Name = "Inputs";
            this.Inputs.ReadOnly = true;
            this.Inputs.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // SetInputsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(284, 477);
            this.Controls.Add(this.setInputsFormCancelButton);
            this.Controls.Add(this.setInputsFormSaveButton);
            this.Controls.Add(this.setInputsFormDataGridView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SetInputsForm";
            this.Text = "Set Inputs";
            ((System.ComponentModel.ISupportInitialize)(this.setInputsFormDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView setInputsFormDataGridView;
        private System.Windows.Forms.Button setInputsFormSaveButton;
        private System.Windows.Forms.Button setInputsFormCancelButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn Inputs;
    }
}