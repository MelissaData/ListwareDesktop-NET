using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ListwareDesktop.Windows
{
    internal partial class OverwriteWarningForm : Form
    {
        internal bool overwrite { get; set; }
        internal bool doNotShowPrompt { get; set; }

        internal OverwriteWarningForm()
        {
            InitializeComponent();
        }

        private void overwriteWarningFormContinueButton_Click(object sender, EventArgs e)
        {
            this.doNotShowPrompt = overwriteWarningFormCheckBox.Checked;
            this.overwrite = true;
            this.Close();
        }

        private void overwriteWarningFormCancelButton_Click(object sender, EventArgs e)
        {
            this.doNotShowPrompt = overwriteWarningFormCheckBox.Checked;
            this.overwrite = false;
            this.Close();
        }
    }
}
