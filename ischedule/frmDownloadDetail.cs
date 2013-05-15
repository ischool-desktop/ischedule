using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ischedule
{
    public partial class frmDownloadDetail : BaseForm
    {
        public frmDownloadDetail(string Content)
        {
            InitializeComponent();

            txtContent.Text = Content;
        }

        private void frmDownloadDetail_Load(object sender, EventArgs e)
        {
            btnExit.Click += (vsender, ve) => this.Close();
        }
    }
}
