namespace ListwareDesktop.Windows
{
    partial class AboutForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.companyLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.techSupportEmailLinkLabel = new System.Windows.Forms.LinkLabel();
            this.wikiLabel = new System.Windows.Forms.Label();
            this.wikiLinkLabel = new System.Windows.Forms.LinkLabel();
            this.websiteLinkLabel = new System.Windows.Forms.LinkLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.versionLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // companyLabel
            // 
            this.companyLabel.AutoSize = true;
            this.companyLabel.Location = new System.Drawing.Point(12, 101);
            this.companyLabel.Name = "companyLabel";
            this.companyLabel.Size = new System.Drawing.Size(176, 39);
            this.companyLabel.TabIndex = 0;
            this.companyLabel.Text = "Melissa Global Intelligence\r\n22382 Avenida Empresa\r\nRancho Santa Margarita, CA 92" +
    "688";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 169);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(246, 39);
            this.label5.TabIndex = 4;
            this.label5.Text = "To obtain technical support on this plugin, contact \r\nMelissa Technical Support a" +
    "t 1-800-635-4772 x4 \r\nor email";
            // 
            // techSupportEmailLinkLabel
            // 
            this.techSupportEmailLinkLabel.AutoSize = true;
            this.techSupportEmailLinkLabel.Location = new System.Drawing.Point(53, 195);
            this.techSupportEmailLinkLabel.Name = "techSupportEmailLinkLabel";
            this.techSupportEmailLinkLabel.Size = new System.Drawing.Size(117, 13);
            this.techSupportEmailLinkLabel.TabIndex = 5;
            this.techSupportEmailLinkLabel.TabStop = true;
            this.techSupportEmailLinkLabel.Text = "tech@melissadata.com";
            this.techSupportEmailLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.techSupportEmailLinkLabel_LinkClicked);
            // 
            // wikiLabel
            // 
            this.wikiLabel.AutoSize = true;
            this.wikiLabel.Location = new System.Drawing.Point(12, 147);
            this.wikiLabel.Name = "wikiLabel";
            this.wikiLabel.Size = new System.Drawing.Size(205, 13);
            this.wikiLabel.TabIndex = 6;
            this.wikiLabel.Text = "For Information or Tutorials please visit our";
            // 
            // wikiLinkLabel
            // 
            this.wikiLinkLabel.AutoSize = true;
            this.wikiLinkLabel.Location = new System.Drawing.Point(214, 147);
            this.wikiLinkLabel.Name = "wikiLinkLabel";
            this.wikiLinkLabel.Size = new System.Drawing.Size(25, 13);
            this.wikiLinkLabel.TabIndex = 7;
            this.wikiLinkLabel.TabStop = true;
            this.wikiLinkLabel.Text = "wiki";
            this.wikiLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.wikiLinkLabel_LinkClicked);
            // 
            // websiteLinkLabel
            // 
            this.websiteLinkLabel.AutoSize = true;
            this.websiteLinkLabel.Location = new System.Drawing.Point(12, 88);
            this.websiteLinkLabel.Name = "websiteLinkLabel";
            this.websiteLinkLabel.Size = new System.Drawing.Size(91, 13);
            this.websiteLinkLabel.TabIndex = 8;
            this.websiteLinkLabel.TabStop = true;
            this.websiteLinkLabel.Text = "www.melissa.com";
            this.websiteLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.websiteLinkLabel_LinkClicked);
            // 
            // pictureBox1
            // 
            this.pictureBox1.ErrorImage = null;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(26, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(213, 70);
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Location = new System.Drawing.Point(12, 75);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(127, 13);
            this.versionLabel.TabIndex = 10;
            this.versionLabel.Text = "Listware Desktop Version";
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(261, 216);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.websiteLinkLabel);
            this.Controls.Add(this.wikiLinkLabel);
            this.Controls.Add(this.wikiLabel);
            this.Controls.Add(this.techSupportEmailLinkLabel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.companyLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.Text = "About Melissa";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label companyLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.LinkLabel techSupportEmailLinkLabel;
        private System.Windows.Forms.Label wikiLabel;
        private System.Windows.Forms.LinkLabel wikiLinkLabel;
        private System.Windows.Forms.LinkLabel websiteLinkLabel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label versionLabel;
    }
}