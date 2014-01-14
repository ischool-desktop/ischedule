using System.Collections.Generic;
using System.Drawing;
using Sunset.Data;

namespace ischedule.test
{
    /// <summary>
    /// Decorator for Scheduler, it is a wrapper class for panel that contains a scheduler.
    /// </summary>
    class DecScheduler
    {
        private DevComponents.DotNetBar.PanelEx pnlContainer;
        private SchedulerType schType = SchedulerType.Teacher;
        private int colCount = -1;
        private int rowCount = -1;

        private int colHeaderHeight = 30;
        private int rowHeaderWidth = 30;

        private Dictionary<string, DevComponents.DotNetBar.PanelEx> cells;  //所有的 panel
        private Dictionary<string, DecPeriod> decPeriods;   //所有的panel 的decorator
        private Dictionary<string, DevComponents.DotNetBar.PanelEx> headerCells;

        private List<CEvent> dataSource;        //所有的課程分段
        private TimeTable currentTbl;           //目前的上課時間表
        private List<Period> busyPeriods;       //不排課時段

        /// <summary>
        /// pnl : 整個課表的 container
        /// schType : 課表類型
        /// 
        /// </summary>
        /// <param name="pnl"></param>
        /// <param name="schType"></param>
        public DecScheduler(DevComponents.DotNetBar.PanelEx pnl, SchedulerType schType)
        {
            this.pnlContainer = pnl;
            this.schType = schType;
            this.cells = new Dictionary<string, DevComponents.DotNetBar.PanelEx>();
            this.decPeriods = new Dictionary<string, DecPeriod>();
            this.headerCells = new Dictionary<string, DevComponents.DotNetBar.PanelEx>();
            
        }

        /* 當某一節次被按下 時 */
        void dec_OnPeriodClicked(object sender, PeriodEventArgs e)
        {
            //Clear selected for all cells ;
            foreach (string key in this.decPeriods.Keys)
                this.decPeriods[key].IsSelected = false;

            DecPeriod period = (DecPeriod)sender;
            period.IsSelected = true;

        }

        #region ==== Methods ======

        public List<CEvent> DataSource
        {
            get { return this.dataSource; }
            set { 
                this.dataSource = value;
                this.fillData();
            }
        }

        public void SetCourseSections(List<CEvent> data)
        {
            this.dataSource = data;
        }
        

        public void SetTimeTable(TimeTable tbl)
        {
            if (this.currentTbl != null && this.currentTbl.TimeTableID == tbl.TimeTableID)
                return;
            else
            {
                this.currentTbl = tbl;
                this.colCount = tbl.Periods.MaxWeekDay;
                this.rowCount = tbl.Periods.MaxPeriodNo;
                //this.createHeaders();
                this.createCells();
            }
        }

        public void InitSchedule(int weekday, int periodcount)
        {
            this.currentTbl = null;
            this.colCount = weekday;
            this.rowCount = periodcount;
            //this.createHeaders();
            this.createCells();
        }

        public void SetBusyPeriods(List<Period> busyPeriods)
        {
            this.busyPeriods = busyPeriods;
            foreach (Period pd in this.busyPeriods)
            {
                string key = string.Format("{0}_{1}", pd.WeekDay, pd.PeriodNo);
                DecPeriod decPd = this.decPeriods[key];
                decPd.SetAsBusy();

            }
        }

        /* 重新調整大小 */
        public void Resize()
        {
            this.createCells();
        }

        /*  填入資料   */
        public void fillData()
        {
            if (this.dataSource == null)
                return;

            Dictionary<string, CEvent> dicTemp = new Dictionary<string, CEvent>();
            foreach (CEvent v in this.dataSource)
            {
                if (v.WeekDay > 0)
                {
                    string key = string.Format("{0}_{1}", v.WeekDay, v.PeriodNo);
                    dicTemp.Add(key, v);
                }
            }

            foreach (string key in this.decPeriods.Keys)
            {
                if (dicTemp.ContainsKey(key))
                    this.decPeriods[key].Data = dicTemp[key];
                //else
                //    this.decPeriods[key].Data = null;
            }
        }

        #endregion


        #region ======= private functions =======
       

        private void createCells()
        {
            if (this.pnlContainer == null)
                return;

            //prepare timetable periods，便於判斷是否是無效的格子。
            Dictionary<string, Period> dicTimeTablePeriods = new Dictionary<string,Period>();    //上課時間表的所有節次，便於搜尋
            if (this.currentTbl != null) 
            {
                foreach (Period pd in this.currentTbl.Periods)
                {
                    string key = string.Format("{0}_{1}", pd.WeekDay, pd.PeriodNo);
                    dicTimeTablePeriods.Add(key, pd);
                }
            }
            bool isInit = (dicTimeTablePeriods.Count == 0);     //通常是在畫面初始化時期才為 true

            //hide all Cells
            foreach (string key in this.cells.Keys)
                this.cells[key].Visible = false;

            this.pnlContainer.SuspendLayout();
            this.pnlContainer.Controls.Clear();

            int pnlWidth = (this.pnlContainer.Size.Width - this.rowHeaderWidth) / colCount;
            int pnlHeight = (this.pnlContainer.Size.Height - this.colHeaderHeight) / rowCount;

            /* Create Headers */
            for (int i = 0; i < colCount; i++)
            {
                Point p = new Point(rowHeaderWidth + i * pnlWidth, 0);
                Size s = new Size(pnlWidth, colHeaderHeight);
                string name = string.Format("header_0_{0}", (i + 1).ToString());
                DevComponents.DotNetBar.PanelEx pnl = null;
                if (this.headerCells.ContainsKey(name))
                {
                    pnl = this.headerCells[name];
                    pnl.Location = p;
                    pnl.Size = s;
                    pnl.Visible = true;
                }
                else
                {
                    pnl = this.makePanel(name, (i+1).ToString(), p, s);
                    this.headerCells.Add(name, pnl);
                }
                this.pnlContainer.Controls.Add(pnl);
            }
            for (int j = 0; j < rowCount; j++)
            {
                Point p = new Point(0, colHeaderHeight + j * pnlHeight);
                Size s = new Size(rowHeaderWidth, pnlHeight);
                string name = string.Format("header_{0}_0", (j + 1).ToString());
                DevComponents.DotNetBar.PanelEx pnl = null;
                if (this.headerCells.ContainsKey(name))
                {
                    pnl = this.headerCells[name];
                    pnl.Location = p;
                    pnl.Size = s;
                    pnl.Visible = true;
                }
                else
                {
                    pnl = this.makePanel(name, (j+1).ToString(), p, s);
                    this.headerCells.Add(name, pnl);
                }
                this.pnlContainer.Controls.Add(pnl);
            }

            /* Originize Cells */
            for (int i = 0; i < colCount; i++)
            {
                for (int j = 0; j < rowCount; j++)
                {
                    string name = string.Format("{0}_{1}", (i+1).ToString(), (j+1).ToString());
                    Point p = new Point(rowHeaderWidth + i * pnlWidth, colHeaderHeight + j * pnlHeight);
                    Size s = new Size(pnlWidth, pnlHeight);

                    DevComponents.DotNetBar.PanelEx pnl = null;
                    if (this.cells.ContainsKey(name))
                    {
                        pnl = this.cells[name];
                        pnl.Location = p;
                        pnl.Size = s;
                        pnl.Visible = true;
                    }
                    else
                    {
                        pnl = this.makePanel(name, "", p, s);
                        this.cells.Add(name, pnl);
                        DecPeriod dec = new DecPeriod(pnl, i+1, j+1, this.schType);
                        dec.BackColor = Color.White;
                        this.decPeriods.Add(name, dec);
                        dec.OnPeriodClicked += new PeriodClickedHandler(dec_OnPeriodClicked);
                    }

                    DecPeriod decP = this.decPeriods[name];
                    decP.IsValid = (isInit) ? true :  dicTimeTablePeriods.ContainsKey(name);
                    
                    this.pnlContainer.Controls.Add(pnl);
                }
            }
            this.pnlContainer.ResumeLayout();
        }



        private DevComponents.DotNetBar.PanelEx makePanel(string name, string txt, Point location, Size size)
        {
            DevComponents.DotNetBar.PanelEx pnl = new DevComponents.DotNetBar.PanelEx();
            pnl.CanvasColor = System.Drawing.SystemColors.Control;
            pnl.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            pnl.Location = location;
            pnl.Name = name;
            pnl.Size = size;
            pnl.Style.Alignment = System.Drawing.StringAlignment.Center;
            pnl.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            pnl.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            pnl.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            pnl.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            pnl.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            pnl.Style.GradientAngle = 90;
            pnl.TabIndex = 1;
            pnl.Text = txt;

            return pnl;
        }

        #endregion
    }
}
