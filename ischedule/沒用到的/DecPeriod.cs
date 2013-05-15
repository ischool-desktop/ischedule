//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Windows.Forms;
//using DevComponents.DotNetBar;
//using System.Threading.Tasks;

//namespace ischedule
//{
//    public delegate void PeriodClickedHandler(object sender, PeriodEventArgs e);

//    /// <summary>
//    /// 節次物件
//    /// </summary>
//    public class DecPeriod
//    {
//        private DevComponents.DotNetBar.PanelEx _pnl;
//        private int _colIndex = -1;
//        private int _rowIndex = -1;
//        private PeriodContent _vo;
//        private bool _selected = false;
//        private PictureBox picBox;

//        private Dictionary<string, Label> dicLables;

//        public event PeriodClickedHandler OnPeriodClicked;

//        /*  Constructor  */
//        public DecPeriod(DevComponents.DotNetBar.PanelEx pnl, int colIndex, int rowIndex)
//        {
//            this._pnl = pnl;
//            this._colIndex = colIndex;
//            this._rowIndex = rowIndex;

//            /* 建立 label */
//            this.dicLables = new Dictionary<string, Label>();
//            int initX = 3;
//            int initY = 3;
//            int labelHeight = 21;
//            int labelCount = 6;

//            for (int i = 0; i < labelCount ; i++)
//            {
//                Label lbl = new Label();
//                lbl.Name = string.Format("label{0}", (i + 1).ToString());
//                lbl.Visible = true;
//                lbl.Text = string.Empty;
//                Point p = new Point(initX, initY + i * labelHeight);
//                lbl.Location = p;

//                //lbl.Click += new EventHandler(_pnl_Click);
//                //lbl.MouseEnter += new EventHandler(lbl_MouseEnter);
//                //lbl.MouseLeave += new EventHandler(lbl_MouseLeave);
//                /* * */
//                this.dicLables.Add(lbl.Name, lbl);
//                this._pnl.Controls.Add(lbl);
//            }

//            Task.Factory.StartNew(() =>
//                {
//                    for (int i = 0; i < labelCount; i++)
//                    {
//                        string strName = string.Format("label{0}", (i + 1).ToString());

//                        if (this.dicLables.ContainsKey(strName))
//                        {
//                            Label lbl = this.dicLables[strName];
//                            lbl.Click += new EventHandler(_pnl_Click);
//                            lbl.MouseEnter += new EventHandler(lbl_MouseEnter);
//                            lbl.MouseLeave += new EventHandler(lbl_MouseLeave);
//                        }
//                    }

//                    /* 註冊事件 */
//                    this._pnl.Click += new EventHandler(_pnl_Click);
//                    this._pnl.MouseEnter += new EventHandler(_pnl_MouseEnter);
//                    this._pnl.MouseLeave += new EventHandler(_pnl_MouseLeave);
//                }
//            );
//        }

//        void lbl_MouseLeave(object sender, EventArgs e)
//        {
//            Label lbl = (Label)sender;

//            if (!string.IsNullOrEmpty("" + lbl.Tag))
//                lbl.ForeColor = Color.Blue;
//            else 
//                lbl.ForeColor = Color.Black;

//            this._pnl_MouseLeave(sender, e);
//        }

//        void lbl_MouseEnter(object sender, EventArgs e)
//        {
//            Label lbl = (Label)sender;
//            lbl.ForeColor = Color.Red;
//            this._pnl_MouseEnter(sender, e);
//        }

//        void _pnl_MouseLeave(object sender, EventArgs e)
//        {
//            this._pnl.Cursor = Cursors.Default;
//        }

//        void _pnl_MouseEnter(object sender, EventArgs e)
//        {
//            this._pnl.Cursor = Cursors.Hand;
//        }

//        void _pnl_Click(object sender, EventArgs e)
//        {
//            /* 可已先行處理，如果有需要的話。 */

//            /* 再把事件丟出去給上層容器 */
//            if (OnPeriodClicked != null)
//            {
//                this.OnPeriodClicked(this, new PeriodEventArgs(this._colIndex, this._rowIndex, this._vo));
//            }
//        }

//        #region properties
//        public bool IsSelected
//        {
//            get { return this.IsSelected; }
//            set
//            {
//                this._selected = value;
//                if (this._selected)
//                    this._pnl.Style.BorderColor.Color = Color.Red;
//                else
//                    this._pnl.Style.BorderColor.ColorSchemePart = eColorSchemePart.PanelBorder;
//            }
//        }

//        public PeriodContent Data
//        {
//            get { return this._vo; }
//            set
//            {
//                this._vo = value;

//                if (this._vo!=null)
//                {
//                    if (this._vo.LabelContents!=null)
//                    {
//                        for(int i=0;i<this._vo.LabelContents.Count;i++)
//                        {
//                            string lblName = string.Format("label{0}", (i + 1).ToString());

//                            if (dicLables.ContainsKey(lblName))
//                            {
//                                dicLables[lblName].Text = this._vo.LabelContents[i].Text;
//                                dicLables[lblName].Tag = this._vo.LabelContents[i].Tag;

//                                if (!string.IsNullOrEmpty("" + dicLables[lblName].Tag))
//                                    dicLables[lblName].ForeColor = Color.Blue;
//                            }
//                        }
//                    }
//                }
//            }
//        }

//        public DevComponents.DotNetBar.PanelEx Panel
//        {
//            get { return this._pnl; }
//        }

//        public Color BackColor
//        {
//            get { return this._pnl.Style.BackColor1.Color; }
//            set
//            {
//                this._pnl.Style.BackColor1.Color = value;
//                this._pnl.Style.BackColor2.Color = value;
//            }
//        }
//        #endregion

//        #region ===========  Method decleration  ========
//        public void AdjustAppearance(Point location, Size size)
//        {
//            this._pnl.Location = location;
//            this._pnl.Size = size;
//        }

//        /* 因為Bind Event Handler 效能慢，所以選在顯示之後， 再呼叫此方法 bind events */
//        //public void AttachEvents()
//        //{
//        //    this._pnl.Click += (object sender, EventArgs e) =>
//        //    {
//        //        this.IsSelected = true;
//        //        if (OnPeriodClicked != null)
//        //        {
//        //            if (this._vo != null)
//        //                this.OnPeriodClicked(this, new PeriodEventArgs(this._colIndex, this._rowIndex, this._vo));
//        //        }
//        //    };
//        //}


//        #endregion
//    }

//    public class PeriodEventArgs : EventArgs
//    {
//        public int Weekday;
//        public int Period;
//        public PeriodContent Value;
//        public PeriodEventArgs(int weekday, int period, PeriodContent vo)
//        {
//            this.Weekday = weekday;
//            this.Period = period;
//            this.Value = vo;
//        }
//    }
//}