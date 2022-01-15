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
    public partial class AboutForm : Form
    {
        public AboutForm(string version)
        {
            InitializeComponent();
            versionLabel.Text += " " + version;
        }

        private void techSupportEmailLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("mailto:tech@melissadata.com");
        }

        private void wikiLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://wiki.melissadata.com/index.php?title=Listware_Desktop");
        }

        private void websiteLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.melissa.com");
        }
    }
}
