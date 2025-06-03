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
    public partial class FormStart : Form
    {
        public FormStart()
        {
            InitializeComponent();
        }

        public bool Video
        {
            get; set;
        }

        private void btnVideo_Click(object sender, EventArgs e)
        {
            Video = true;
            this.Close();
        }

        private void btnImages_Click(object sender, EventArgs e)
        {
            Video = false;
            this.Close();
        }
    }
}
