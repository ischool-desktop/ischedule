using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ischedule.UserControls
{
    /*
     * 此類別為了消除 TableLayoutPanel 在 resize 時會閃爍不停的問題，
 *  How To Create a Flicker Free TableLayoutPanel
 *  http://www.richard-banks.org/2007/09/how-to-create-flicker-free.html
 * */
    public partial class ucNiceTableLayoutPanel : TableLayoutPanel
    {
        public ucNiceTableLayoutPanel()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint |
              ControlStyles.OptimizedDoubleBuffer |
              ControlStyles.UserPaint, true);
        }

        public ucNiceTableLayoutPanel(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint |
              ControlStyles.OptimizedDoubleBuffer |
              ControlStyles.UserPaint, true);
        }
    }
}
