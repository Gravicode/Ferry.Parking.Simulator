using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace sample
{
    public partial class FormLic : Form
    {
        public FormLic()
        {
            InitializeComponent();

            ShowCurrentLicInfo();
        }

        public void ShowCurrentLicInfo()
        {
            string license_key;
            string comments;
            int channels;
            DateTime expDate;
            DTK.LPR.Lib.License.GetActivatedLicenseInfo(out license_key, out comments, out channels, out expDate);
            if (license_key.Length > 0)
            {
                txtLicInfo.Text = "License key: " + license_key + "\r\n";
                txtLicInfo.Text += "Channels: " + channels.ToString() + "\r\n";
                txtLicInfo.Text += "Comments: " + comments + "\r\n";
                if (expDate != DateTime.MaxValue)
                    txtLicInfo.Text += "Expiration date: " + expDate.ToString() + "\n";
            }
            else
            {
                txtLicInfo.Text = "No license activated on this computer.\r\nPlease contact at support@dtksoft.com to request trial license.";
            }
        }

        private void btnActivate_Click(object sender, EventArgs e)
        {
            int ret = DTK.LPR.Lib.License.ActivateLicenseOnline(txtLicenseKey.Text, "");
            if (ret == 0)
            {
                MessageBox.Show("The license has been successfully activated.", "License activation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ShowCurrentLicInfo();
            }
            else
                MessageBox.Show("Error activating license. Error code: " + ret.ToString(), "License activation", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
