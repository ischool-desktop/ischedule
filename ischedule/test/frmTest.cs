using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ischedule.test
{
    public partial class frmTest : Form
    {
        public frmTest()
        {
            InitializeComponent();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            (new frmTestScheduler()).Show();
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            (new frmTestScheduleDecorator()).Show();
        }
    }
}
