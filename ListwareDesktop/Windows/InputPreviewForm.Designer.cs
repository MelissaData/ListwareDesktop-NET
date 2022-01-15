namespace ListwareDesktop.Windows
{
    partial class InputPreviewForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InputPreviewForm));
            this.previewDataGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.previewDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // previewDataGridView
            // 
            this.previewDataGridView.AllowUserToAddRows = false;
            this.previewDataGridView.AllowUserToDeleteRows = false;
            this.previewDataGridView.AllowUserToResizeRows = false;
            this.previewDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.previewDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.previewDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.previewDataGridView.Location = new System.Drawing.Point(0, 0);
            this.previewDataGridView.Name = "previewDataGridView";
            this.previewDataGridView.ReadOnly = true;
            this.previewDataGridView.RowHeadersVisible = false;
            this.previewDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.previewDataGridView.Size = new System.Drawing.Size(551, 261);
            this.previewDataGridView.TabIndex = 0;
            // 
            // InputPreviewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 261);
            this.Controls.Add(this.previewDataGridView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "InputPreviewForm";
            this.Text = "Input Preview";
            ((System.ComponentModel.ISupportInitialize)(this.previewDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView previewDataGridView;
    }
}