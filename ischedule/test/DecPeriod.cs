using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using DevComponents.DotNetBar;
using System.Windows.Forms;
using Sunset.Data;

namespace ischedule.test
{
     delegate void PeriodClickedHandler(object sender, PeriodEventArgs e);

    class DecPeriod
    {

        private DevComponents.DotNetBar.PanelEx _pnl;
        private SchedulerType _schType = SchedulerType.Teacher;
        private int _colIndex = -1;
        private int _rowIndex = -1;
        private CEvent _vo;
        private List<CEvent> _events = new List<CEvent>();  //此節次的課程分段，可能多個，如單雙週，或場地可同時多門課程。
        private bool _selected = false;
        private PictureBox picBox;
        private Label lbl1;
        private Label lbl2;
        private Label lbl3;
        private Panel pnlCover;

        private Color unselectedColor = Color.FromArgb(234, 234, 234);

        private bool isBindEvent = false;
        private bool isValid;   //是否有效的節次，用來辨別出現在課表中，但不屬於上課時間表的節次。

        private Dictionary<string, Label> dicLables;

        public event PeriodClickedHandler OnPeriodClicked;

        /*  Constructor  */
        public DecPeriod(DevComponents.DotNetBar.PanelEx pnl, int colIndex, int rowIndex, SchedulerType schType)
        {
            this._pnl = pnl;
            this._schType = schType;
            this._colIndex = colIndex;
            this._rowIndex = rowIndex;
            /*
            this.pnlCover = new Panel();
            this.pnlCover.Size = new Size(this._pnl.Size.Width-2, this._pnl.Size.Height-2);
            this.pnlCover.Location = new Point(1, 1);
            this.pnlCover.Click += new EventHandler(_pnl_Click);
            this.pnlCover.MouseEnter += new EventHandler(_pnl_MouseEnter);
            this.pnlCover.MouseLeave += new EventHandler(_pnl_MouseLeave);
            this.pnlCover.BackColor = Color.Yellow;
            this.pnlCover.BringToFront();
            this._pnl.Controls.Add(this.pnlCover);
            */
            
            /* 註冊事件  */
            
            this._pnl.Click += new EventHandler(_pnl_Click);
            this._pnl.MouseEnter += new EventHandler(_pnl_MouseEnter);
            this._pnl.MouseLeave += new EventHandler(_pnl_MouseLeave);
            
            /* picBox */
            this.picBox = new PictureBox();
            Size s = new Size(16, 16);
            Point pt = new Point(this._pnl.Width - s.Width - 6, this._pnl.Height - s.Height - 6);
            this.picBox.Size = s;
            this.picBox.Location = pt;
            //this.picBox.Image = Properties.Resources.busy;
            this.picBox.SizeMode = PictureBoxSizeMode.StretchImage;
            this.picBox.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this._pnl.Controls.Add(this.picBox);
            this.picBox.Click += new EventHandler(picBox_Click);

            /* 建立 label */
            this.dicLables = new Dictionary<string, Label>();
            int initX = 6;
            int initY = 6;
            int labelHeight = 20;
            for (int i = 0; i < 3; i++)
            {
                Label lbl = new Label();
                lbl.Name = string.Format("label{0}", (i + 1).ToString());
                lbl.Visible = true;
                lbl.Text = "This is :" + i.ToString();
                Point p = new Point(initX, initY + i * labelHeight);
                lbl.Location = p;
                if (i == 0)
                    this.lbl1 = lbl;
                else if (i == 1)
                    this.lbl2 = lbl;
                else if (i == 2)
                    this.lbl3 = lbl;


                lbl.Click += new EventHandler(_pnl_Click);

                lbl.MouseEnter += new EventHandler(lbl_MouseEnter);
                lbl.MouseLeave += new EventHandler(lbl_MouseLeave);

                /*    * */
                this.dicLables.Add(lbl.Name, lbl);
                this._pnl.Controls.Add(lbl);
            }
        }

        void picBox_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void lbl_MouseLeave(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.ForeColor = Color.Black;
            this._pnl_MouseLeave(sender, e);
        }

        void lbl_MouseEnter(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.ForeColor = Color.Red;
            this.MouseEnterHandler(sender, e);
        }

        void _pnl_MouseLeave(object sender, EventArgs e)
        {
            this._pnl.Cursor = Cursors.Default;
            Console.WriteLine("_pnl_MouseLeave");
        }

        void _pnl_MouseEnter(object sender, EventArgs e)
        {
            
            this.MouseEnterHandler(sender, e);
            if (!this.isBindEvent)
            {
                /*
                this.lbl1.MouseEnter += new EventHandler(lbl_MouseEnter);
                this.lbl1.MouseLeave += new EventHandler(lbl_MouseLeave);
                this.lbl1.Click += new EventHandler(_pnl_Click);

                this.picBox.Click += new EventHandler(picBox_Click);
                this.isBindEvent = true;
                this.pnlCover.Visible = false;
                 * */
                 
            }
             
            Console.WriteLine("_pnl_MouseEnter");
        }

        private void MouseEnterHandler(object sender, EventArgs e)
        {
            this._pnl.Cursor = Cursors.Hand;

        }


        void _pnl_Click(object sender, EventArgs e)
        {
            /* 可已先行處理，如果有需要的話。 */

            /* 再把事件丟出去給上層容器 */
            if (OnPeriodClicked != null)
            {
                    this.OnPeriodClicked(this, new PeriodEventArgs(this._colIndex, this._rowIndex, this._vo));
            }
        }

        #region properties
        public bool IsSelected
        {
            get { return this.IsSelected; }
            set
            {
                this._selected = value;
                if (this._selected)
                    this._pnl.Style.BorderColor.Color = Color.Red;
                else
                    this._pnl.Style.BorderColor.ColorSchemePart = eColorSchemePart.PanelBorder;
            }
        }

        public CEvent Data
        {
            get { return this._vo; }
            set { 
                this._vo = value;
                this.picBox.Image = (this._vo.ManualLock) ? Properties.Resources.lock_3 : null ;
                this.BackColor = (this._vo.ManualLock) ? this.unselectedColor : Color.White;
                this.lbl1.Text = (this._vo == null ? "" :  this._vo.DisplaySubjectName);
                switch (this._schType)
                {
                    case SchedulerType.Teacher :
                        this.lbl2.Text = this._vo.DisplayClassName;
                        this.lbl3.Text = this._vo.DisplayClassroomName;
                        break;
                    case SchedulerType.Class:
                        this.lbl2.Text = this._vo.DisplayTeacherName;
                        this.lbl3.Text = this._vo.DisplayClassroomName;
                        break;
                    case SchedulerType.Classroom:
                        this.lbl2.Text = this._vo.DisplayTeacherName;
                        this.lbl3.Text = this._vo.DisplayClassName;
                        break;
                }
            }
        }

        public DevComponents.DotNetBar.PanelEx Panel
        {
            get { return this._pnl; }
        }

        public Color BackColor
        {
            get { return this._pnl.Style.BackColor1.Color; }
            set
            {
                this._pnl.Style.BackColor1.Color = value;
                this._pnl.Style.BackColor2.Color = value;
            }
        }
        #endregion

        #region ===========  Method decleration  ========
        public void AdjustAppearance(Point location, Size size)
        {
            this._pnl.Location = location;
            this._pnl.Size = size;
        }

        /// <summary>
        /// 註明為不排課時段
        /// </summary>
        public void SetAsBusy()
        {
            this.lbl1.Text = "不排課時段";
            this.lbl2.Text = "";
            this.lbl3.Text = "";
            this.picBox.Image = Properties.Resources.busy;
            this.BackColor = this.unselectedColor;
            
        }

        /// <summary>
        /// 標記為無效的節次。
        /// 通常在畫出上課時間表的矩陣時，某些節次並不在上課時間表範圍內，就呼叫此方法設為無效。
        /// </summary>
        public bool IsValid
        {
            get { return this.isValid ;}
            set {
                this.isValid = value;
                this.lbl1.Text = "";
                this.BackColor = (this.isValid) ? Color.White : this.unselectedColor;

            }
        }

        /* 因為Bind Event Handler 效能慢，所以選在顯示之後， 再呼叫此方法 bind events */
        //public void AttachEvents()
        //{
        //    this._pnl.Click += (object sender, EventArgs e) =>
        //    {
        //        this.IsSelected = true;
        //        if (OnPeriodClicked != null)
        //        {
        //            if (this._vo != null)
        //                this.OnPeriodClicked(this, new PeriodEventArgs(this._colIndex, this._rowIndex, this._vo));
        //        }
        //    };
        //}

        
        #endregion
    }

    class PeriodEventArgs : EventArgs
    {
        public int Weekday;
        public int Period;
        public CEvent Value;
        public PeriodEventArgs(int weekday, int period, CEvent vo)
        {
            this.Weekday = weekday;
            this.Period = period;
            this.Value = vo;
        }
    }
}
