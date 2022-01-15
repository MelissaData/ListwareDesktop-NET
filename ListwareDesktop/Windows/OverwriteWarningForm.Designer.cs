namespace ListwareDesktop.Windows
{
    partial class OverwriteWarningForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OverwriteWarningForm));
            this.overwriteWarningFormContinueButton = new System.Windows.Forms.Button();
            this.overwriteWarningFormCheckBox = new System.Windows.Forms.CheckBox();
            this.overwriteWarningFormLabel = new System.Windows.Forms.Label();
            this.overwriteWarningFormCancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // overwriteWarningFormContinueButton
            // 
            this.overwriteWarningFormContinueButton.Location = new System.Drawing.Point(12, 35);
            this.overwriteWarningFormContinueButton.Name = "overwriteWarningFormContinueButton";
            this.overwriteWarningFormContinueButton.Size = new System.Drawing.Size(75, 23);
            this.overwriteWarningFormContinueButton.TabIndex = 0;
            this.overwriteWarningFormContinueButton.Text = "Continue";
            this.overwriteWarningFormContinueButton.UseVisualStyleBackColor = true;
            this.overwriteWarningFormContinueButton.Click += new System.EventHandler(this.overwriteWarningFormContinueButton_Click);
            // 
            // overwriteWarningFormCheckBox
            // 
            this.overwriteWarningFormCheckBox.AutoSize = true;
            this.overwriteWarningFormCheckBox.Location = new System.Drawing.Point(26, 64);
            this.overwriteWarningFormCheckBox.Name = "overwriteWarningFormCheckBox";
            this.overwriteWarningFormCheckBox.Size = new System.Drawing.Size(172, 17);
            this.overwriteWarningFormCheckBox.TabIndex = 1;
            this.overwriteWarningFormCheckBox.Text = "Do not show this prompt again.";
            this.overwriteWarningFormCheckBox.UseVisualStyleBackColor = true;
            // 
            // overwriteWarningFormLabel
            // 
            this.overwriteWarningFormLabel.AutoSize = true;
            this.overwriteWarningFormLabel.Location = new System.Drawing.Point(69, 9);
            this.overwriteWarningFormLabel.Name = "overwriteWarningFormLabel";
            this.overwriteWarningFormLabel.Size = new System.Drawing.Size(74, 13);
            this.overwriteWarningFormLabel.TabIndex = 2;
            this.overwriteWarningFormLabel.Text = "Overwrite file?";
            // 
            // overwriteWarningFormCancelButton
            // 
            this.overwriteWarningFormCancelButton.Location = new System.Drawing.Point(132, 35);
            this.overwriteWarningFormCancelButton.Name = "overwriteWarningFormCancelButton";
            this.overwriteWarningFormCancelButton.Size = new System.Drawing.Size(75, 23);
            this.overwriteWarningFormCancelButton.TabIndex = 3;
            this.overwriteWarningFormCancelButton.Text = "Cancel";
            this.overwriteWarningFormCancelButton.UseVisualStyleBackColor = true;
            this.overwriteWarningFormCancelButton.Click += new System.EventHandler(this.overwriteWarningFormCancelButton_Click);
            // 
            // OverwriteWarningForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(219, 85);
            this.Controls.Add(this.overwriteWarningFormCancelButton);
            this.Controls.Add(this.overwriteWarningFormLabel);
            this.Controls.Add(this.overwriteWarningFormCheckBox);
            this.Controls.Add(this.overwriteWarningFormContinueButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OverwriteWarningForm";
            this.Text = "Warning";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button overwriteWarningFormContinueButton;
        private System.Windows.Forms.CheckBox overwriteWarningFormCheckBox;
        private System.Windows.Forms.Label overwriteWarningFormLabel;
        private System.Windows.Forms.Button overwriteWarningFormCancelButton;
    }
}