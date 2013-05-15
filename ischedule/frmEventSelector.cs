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
    public partial class frmEventSelector : BaseForm
    {
        private List<string> EventIDs;

        public frmEventSelector(List<string> EventIDs)
        {
            InitializeComponent();
        }

        public List<string> SelectedEventIDs
        {
            get
            {
                return EventIDs;
            }
        }        

        public int SelectorType { get { return 1;}}
    }
}