using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ischedule
{
    public partial class ucScheduler_1 : UserControl
    {
        public ucScheduler_1()
        {
            InitializeComponent();
        }

        public void SetTimeTable(object TimeTable)
        {
            //this.scheduler1.SetTimeTable(TimeTable);
        }

        public void setCourseSections(List<object> sections)
        {
           // this.scheduler1.SetSections(sections);
        }

        public void refreshLayout()
        {
            //this.adjustColumnWidth();
            //this.adjustRowHeight();
        }
        /*
        private void adjustColumnWidth()
        {
            int gridWidth = this.Width - this.expandablePanel1.Width  - this.expandableSplitter1.Width ;
            int columnWidth = (gridWidth - this.dataGridViewX1.RowHeadersWidth) / this.dataGridViewX1.Columns.Count;
            foreach (DataGridViewColumn dc in this.dataGridViewX1.Columns)
            {
                dc.Width = columnWidth;
                dc.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            //this.dataGridViewX1.Refresh();
            Console.WriteLine("Column Width changed");
        }

        private void adjustRowHeight()
        {
            int rowHeight = this.calculateRowHeight(this.dataGridViewX1.Rows.Count);
            if (rowHeight > 0)
            {
                foreach (DataGridViewRow dr in this.dataGridViewX1.Rows)
                    dr.Height = rowHeight;
            }
        }

        private int calculateRowHeight(int rowCount)
        {
            if (rowCount > 0)
            {
                int gridHeight = this.Height ;
                return (gridHeight - this.dataGridViewX1.ColumnHeadersHeight) / rowCount;
            }
            else
                return 0;
        }
        */

        //============   Event Handler
        private void expandableSplitter1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            this.refreshLayout();
        }

        private void expandablePanel1_ExpandedChanged(object sender, DevComponents.DotNetBar.ExpandedChangeEventArgs e)
        {
            this.refreshLayout();
        }

        private void ucScheduler_1_Load(object sender, EventArgs e)
        {
            //this.dec_grid();
        }

        /*
        private void dec_grid()
        {
            DevComponents.DotNetBar.Controls.DataGridViewX dg = this.dataGridViewX1 ;
            dg.ReadOnly = true;
            dg.AllowUserToAddRows = false;
            dg.AllowUserToDeleteRows = false;
            dg.AllowUserToOrderColumns = false;
            dg.AllowUserToResizeColumns = false;
            dg.AllowUserToResizeRows = false;
            dg.SelectionMode = DataGridViewSelectionMode.CellSelect;
        }
         * */
    }
}
