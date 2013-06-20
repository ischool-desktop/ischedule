using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using ischedule.Properties;
using Sunset.Data;

namespace ischedule
{
    public delegate void PeriodClickedHandler(object sender, PeriodEventArgs e);

    /// <summary>
    /// 排課顏色
    /// </summary>
    public class SchedulerColor
    {
        public static Color lvBusyBackColor = Color.FromArgb(230, 230, 230);
        public static Color lvBusyForeColor = Color.FromArgb(86, 86, 86);

        //private const string lvBusyBackColor = "#e6e6e6";
        //private const string lvBusyForeColor = "#565656";

        public static Color lvFreeBackColor = Color.White;
        public static Color lvFreeForeColor = Color.FromArgb(86, 86, 86);

        //private const string lvFreeBackColor = "White";
        //private const string lvFreeForeColor = "#565656";

        public static Color lvScheduledBackColor = Color.Snow;
        public static Color lvScheduledForeColor = Color.FromArgb(2, 123, 204);

        //private const string lvScheduledBackColor = "Snow";
        //private const string lvScheduledForeColor = "#027bcc";

        public static Color lvTimeTableBackColor = Color.FromArgb(230, 230, 230);
        public static Color lvTimeTableForeColor = Color.FromArgb(86, 86, 86);

        //private const string lvTimeTableBackColor = "#e6e6e6";
        //private const string lvTimeTableForeColor = "#565656";

        public static Color lvSchedulableBackColor = Color.FromArgb(254, 252, 128);
        public static Color lvSchedulableForeColor = Color.White;

        //private const string lvSchedulableBackColor = "#fefc80";
        //private const string lvSchedulableForeColor = "White";

        public static Color lvNoSolutionBackColor = Color.FromArgb(230, 230, 230);
        public static Color lvNoSolutionForeColor = Color.FromArgb(86, 86, 86);

        //private const string lvNoSolutionBackColor = "#e6e6e6";
        //private const string lvNoSolutionForeColor = "#565656";
    }

    /// <summary>
    /// 節次類別
    /// </summary>
    public class DecPeriod
    {
        private LPViewOption Option;
        private const int lblCount = 4;
        private string TimeTableID = string.Empty;
        private DevComponents.DotNetBar.PanelEx _pnl;
        private SchedulerType _schType = SchedulerType.Teacher;
        private int _colIndex = -1;
        private int _rowIndex = -1;
        //private CEvent _vo;
        private List<CEvent> _events = new List<CEvent>();  //此節次的課程分段，可能多個，如單雙週，或場地可同時多門課程。
        private bool _selected = false;
        private PictureBox picBox;
        private Label lbl1;
        private Label lbl2;
        private Label lbl3;
        private Label lbl4;
        //private Panel pnlCover;
        private Color unselectedColor = Color.FromArgb(234, 234, 234);

        private bool isBindEvent = false;
        private bool isValid;   //是否有效的節次，用來辨別出現在課表中，但不屬於上課時間表的節次。

        private Dictionary<string, Label> dicLables;

        /// <summary>
        /// 星期
        /// </summary>
        public int Weekday { get { return this._colIndex; } }

        /// <summary>
        /// 節次
        /// </summary>
        public int Period { get { return this._rowIndex; } }

        /// <summary>
        /// 節次按下時的事件
        /// </summary>
        public event PeriodClickedHandler OnPeriodClicked;

        /*  Constructor  */
        public DecPeriod(DevComponents.DotNetBar.PanelEx pnl, 
            int colIndex, 
            int rowIndex,
            SchedulerType schType)
        {
            this._pnl = pnl;
            this._schType = schType;
            this._colIndex = colIndex;
            this._rowIndex = rowIndex;
            this._pnl.Tag = this;
            
            /* 註冊事件  */

            this._pnl.Click += new EventHandler(_pnl_MouseEnter);
            this._pnl.MouseEnter += new EventHandler(_pnl_MouseEnter);

            //this._pnl.MouseLeave += new EventHandler(_pnl_MouseLeave);
            
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
            //this.picBox.Click += new EventHandler(picBox_Click);

            /* 建立 label */
            this.dicLables = new Dictionary<string, Label>();
            int initX = 5;
            int initY = 5;
            int labelHeight = 18;
            for (int i = 0; i < lblCount; i++)
            {
                Label lbl = new Label();
                lbl.Name = string.Format("label{0}", (i + 1).ToString());
                lbl.Visible = true;
                lbl.AutoSize = true;
                lbl.Text = string.Empty;
                Point p = new Point(initX, initY + i * labelHeight);
                lbl.Location = p;
                if (i == 0)
                    this.lbl1 = lbl;
                else if (i == 1)
                    this.lbl2 = lbl;
                else if (i == 2)
                    this.lbl3 = lbl;
                else if (i == 3)
                    this.lbl4 = lbl;

                /*
                lbl.Click += new EventHandler(_pnl_Click);

                lbl.MouseEnter += new EventHandler(lbl_MouseEnter);
                lbl.MouseLeave += new EventHandler(lbl_MouseLeave);

                    * */
                this.dicLables.Add(lbl.Name, lbl);
                this._pnl.Controls.Add(lbl);
            }
        }

        /// <summary>
        /// 根據時間表編號初始化內容
        /// </summary>
        /// <param name="TimeTableID"></param>
        public void InitialContent(string TimeTableID,LPViewOption Option)
        {
            this.isBindEvent = false;
            this.Option = Option;
            this.TimeTableID = TimeTableID;
            this.lbl1.Text = string.Empty;
            this.lbl1.Tag = string.Empty;
            this.lbl1.Visible = true;
            this.lbl1.MouseEnter -= new EventHandler(lbl_MouseEnter);
            this.lbl1.MouseLeave -= new EventHandler(lbl_MouseLeave);
            this.lbl1.Click -= new EventHandler(_label_Click);

            this.lbl2.Text = string.Empty;
            this.lbl2.Tag = string.Empty;
            this.lbl2.Visible = true;
            this.lbl2.MouseEnter -= new EventHandler(lbl_MouseEnter);
            this.lbl2.MouseLeave -= new EventHandler(lbl_MouseLeave);
            this.lbl2.Click -= new EventHandler(_label_Click);

            this.lbl3.Text = string.Empty;
            this.lbl3.Tag = string.Empty;
            this.lbl3.Visible = true;
            this.lbl3.MouseEnter -= new EventHandler(lbl_MouseEnter);
            this.lbl3.MouseLeave -= new EventHandler(lbl_MouseLeave);
            this.lbl3.Click -= new EventHandler(_label_Click);

            this.lbl4.Text = string.Empty;
            this.lbl4.Tag = string.Empty;
            this.lbl4.Visible = true;
            this.lbl4.MouseEnter -= new EventHandler(lbl_MouseEnter);
            this.lbl4.MouseLeave -= new EventHandler(lbl_MouseLeave);
            this.lbl4.Click -= new EventHandler(_label_Click);

            this.picBox.Image = Resources.blank;
            this.picBox.Tag = string.Empty;
            this.picBox.Click -= new EventHandler(picBox_Click);
           
            this._pnl.Style.BorderColor.ColorSchemePart = eColorSchemePart.PanelBorder;
            this._pnl.Style.BorderWidth = 1;
            this._pnl.Click -= new EventHandler(_pnl_Click);
            this._pnl.MouseLeave -= new EventHandler(_pnl_MouseLeave);
            this.BackColor = SchedulerColor.lvTimeTableBackColor;

            this._events.Clear();
        }

        void picBox_Click(object sender, EventArgs e)
        {
            /* 再把事件丟出去給上層容器 */
            if (OnPeriodClicked != null)
            {
                this.OnPeriodClicked(this.picBox , new PeriodEventArgs(this._colIndex, this._rowIndex, this._events));
            }
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
            //this._pnl.Cursor = Cursors.Default;
            Console.WriteLine("_pnl_MouseLeave");
        }

        void _pnl_MouseEnter(object sender, EventArgs e)
        {            
            this.MouseEnterHandler(sender, e);

            if (!this.isBindEvent)
            {
                if (!string.IsNullOrEmpty("" + this.lbl1.Tag))
                {
                    this.lbl1.MouseEnter += new EventHandler(lbl_MouseEnter);
                    this.lbl1.MouseLeave += new EventHandler(lbl_MouseLeave);
                    this.lbl1.Click += new EventHandler(_label_Click);
                }

                if (!string.IsNullOrEmpty("" + this.lbl2.Tag))
                {
                    this.lbl2.MouseEnter += new EventHandler(lbl_MouseEnter);
                    this.lbl2.MouseLeave += new EventHandler(lbl_MouseLeave);
                    this.lbl2.Click += new EventHandler(_label_Click);
                }

                if (!string.IsNullOrEmpty("" + this.lbl3.Tag))
                {
                    this.lbl3.MouseEnter += new EventHandler(lbl_MouseEnter);
                    this.lbl3.MouseLeave += new EventHandler(lbl_MouseLeave);
                    this.lbl3.Click += new EventHandler(_label_Click);
                }

                if (!string.IsNullOrEmpty("" + this.lbl4.Tag))
                {
                    this.lbl4.MouseEnter += new EventHandler(lbl_MouseEnter);
                    this.lbl4.MouseLeave += new EventHandler(lbl_MouseLeave);
                    this.lbl4.Click += new EventHandler(_label_Click);
                }

                this._pnl.Click += new EventHandler(_pnl_Click);
                this._pnl.MouseLeave += new EventHandler(_pnl_MouseLeave);

                this.picBox.Click += new EventHandler(picBox_Click);
                this.isBindEvent = true;                 
            }
             
            Console.WriteLine("_pnl_MouseEnter");
        }

        private void MouseEnterHandler(object sender, EventArgs e)
        {
            this._pnl.Cursor = Cursors.Hand;

        }

        void _label_Click(object sender, EventArgs e)
        {
            /* 再把事件丟出去給上層容器 */
            if (OnPeriodClicked != null)
            {
                this.OnPeriodClicked(sender, new PeriodEventArgs(this._colIndex, this._rowIndex, this._events));
            }
        }

        void _pnl_Click(object sender, EventArgs e)
        {
            /* 可已先行處理，如果有需要的話。 */

            /* 再把事件丟出去給上層容器 */
            if (OnPeriodClicked != null)
            {
                this.OnPeriodClicked(this, new PeriodEventArgs(this._colIndex, this._rowIndex, this._events));
            }
        }

        #region properties
        /// <summary>
        /// 是否選取
        /// </summary>
        public bool IsSelected
        {
            get { return this._selected; }
            set
            {
                this._selected = value;
                if (this._selected)
                {
                    this._pnl.Style.BorderColor.Color = Color.Orange;
                    this._pnl.Style.BorderWidth = 2;

                    if (!("" + this.picBox.Tag).Equals("lock"))
                    {
                        this.picBox.Image = Resources.delete1;
                        this.picBox.Tag = "delete";
                    }
                }
                else
                {
                    this._pnl.Style.BorderColor.ColorSchemePart = eColorSchemePart.PanelBorder;
                    this._pnl.Style.BorderWidth = 1;

                    if (!("" + this.picBox.Tag).Equals("busy"))
                    {
                        this.picBox.Image = Resources.blank;
                        this.picBox.Tag = string.Empty;
                    }
                }
            }
        }

        /// <summary>
        /// 設為可排課時段
        /// </summary>
        public void SetSchedulable()
        {
            picBox.Image = Resources.blank;
            BackColor = SchedulerColor.lvSchedulableBackColor;
            lbl1.Visible = true;
            lbl2.Visible = true;
            lbl3.Visible = true;
            lbl4.Visible = true;
            this._events = new List<CEvent>();
        }

        /// <summary>
        /// 註明為不排課時段
        /// </summary>
        public void SetAsBusy(string BusyDesc)
        {
            this.lbl1.Text = BusyDesc;
            this.lbl2.Text = string.Empty;
            this.lbl3.Text = string.Empty;
            this.lbl4.Text = string.Empty;
            this.picBox.Image = Properties.Resources.busy;
            this.picBox.Tag = "busy";
            this.BackColor = this.unselectedColor;
            this._events = new List<CEvent>();
        }

        /// <summary>
        /// 設定無解
        /// </summary>
        /// <param name="ReasonDesc">無解原因</param>
        public void SetNoSolution(string ReasonDesc)
        {
            BackColor = SchedulerColor.lvNoSolutionBackColor;
            lbl1.Text = ReasonDesc;
            this._events = new List<CEvent>();
        }

        /// <summary>
        /// 設定無解，可連結資源
        /// </summary>
        /// <param name="ReasonDesc"></param>
        /// <param name="AssocType"></param>
        /// <param name="AssocID"></param>
        /// <param name="AssocName"></param>
        public void SetNoSolution(string ReasonDesc,SchedulerType AssocType,string AssocID,string AssocName)
        {
            lbl1.Text = AssocName;
            lbl1.Tag = ("" + AssocType) + "：" + AssocID;
            lbl2.Text = ReasonDesc;
            this._events = new List<CEvent>();
            BackColor = SchedulerColor.lvNoSolutionBackColor;
        }

        /// <summary>
        /// 設定多筆分課資料
        /// </summary>
        private void SetMultipleEvents()
        {
            #region 若Appointment的Timetable等於目前的TimeTable則顯示，否則顯示TimeTable名稱
            if (this._events[0].TimeTableID.Equals(this.TimeTableID))
            {
                #region 單雙週課程
                if (this._events.IsSDAppointments())
                {
                    CEvent eventS = this._events[0];
                    CEvent eventD = this._events[1];                    

                    this.picBox.Image = eventS.ManualLock || eventD.ManualLock ? Resources.lock_3 : Resources.blank;
                    this.picBox.Tag = eventS.ManualLock || eventD.ManualLock ? "lock" : string.Empty;

                    switch (this._schType)
                    {
                        case SchedulerType.Teacher:
                            this.lbl1.Text = eventS.DisplaySubjectName + eventS.WeekFlag.GetWeekFlagStr();
                            this.lbl1.Tag = string.Empty;

                            this.lbl2.Text = eventS.DisplayClassName;        
                            this.lbl2.Tag = !string.IsNullOrEmpty(eventS.ClassID) ? string.Format("Class：{0}", eventS.ClassID) : string.Empty;

                            this.lbl3.Text = eventD.DisplaySubjectName + eventD.WeekFlag.GetWeekFlagStr();
                            this.lbl3.Tag = string.Empty;

                            this.lbl4.Text = eventD.DisplayClassName;
                            this.lbl4.Tag = !string.IsNullOrEmpty(eventD.ClassID) ? string.Format("Class：{0}", eventD.ClassID) : string.Empty;

                            break;
                        case SchedulerType.Class:
                            this.lbl1.Text = eventS.DisplaySubjectName + eventS.WeekFlag.GetWeekFlagStr();
                            this.lbl1.Tag = string.Empty;

                            this.lbl2.Text = eventS.DisplayTeacherName;
                            this.lbl2.Tag = !string.IsNullOrEmpty(eventS.TeacherID1) ? string.Format("Teacher：{0}", eventS.TeacherID1) : string.Empty;

                            this.lbl3.Text = eventD.DisplaySubjectName + eventD.WeekFlag.GetWeekFlagStr();
                            this.lbl3.Tag = string.Empty;

                            this.lbl4.Text = eventD.DisplayTeacherName;
                            this.lbl4.Tag = !string.IsNullOrEmpty(eventD.TeacherID1) ? string.Format("Teacher：{0}", eventD.TeacherID1) : string.Empty;

                            break;
                        case SchedulerType.Classroom:
                            this.lbl1.Text = eventS.DisplaySubjectName + eventS.WeekFlag.GetWeekFlagStr();
                            this.lbl1.Tag = string.Empty;

                            this.lbl2.Text = eventS.DisplayClassName;
                            this.lbl2.Tag = !string.IsNullOrEmpty(eventS.ClassID) ? string.Format("Class：{0}", eventS.ClassID) : string.Empty;

                            this.lbl3.Text = eventD.DisplaySubjectName + eventD.WeekFlag.GetWeekFlagStr();
                            this.lbl3.Tag = string.Empty;

                            this.lbl4.Text = eventD.DisplayClassName;
                            this.lbl4.Tag = !string.IsNullOrEmpty(eventD.ClassID) ? string.Format("Class：{0}", eventD.ClassID) : string.Empty;

                            break;
                    }
                }
                #endregion
                #region 群組課程，只顯示群組名稱
                else if (this._events.IsGroupEvents())
                {
                    CEvent eventGroup = this._events[0];

                    this.BackColor = SchedulerColor.lvScheduledBackColor;

                    this.picBox.Image = eventGroup.ManualLock ? Resources.lock_3 : Resources.blank;
                    this.picBox.Tag = eventGroup.ManualLock ? "lock" : string.Empty;

                    this.lbl1.Text = eventGroup.CourseGroup;
                    this.lbl1.Tag = this._events;
                }
                #endregion
                #region 場地容納多課程
                else if (_schType == SchedulerType.Classroom)
                {
                    CEvent eventWhere = this._events[0];

                    this.BackColor = SchedulerColor.lvScheduledBackColor;

                    this.picBox.Image = eventWhere.ManualLock ? Resources.lock_3 : Resources.blank;
                    this.picBox.Tag = eventWhere.ManualLock ? "lock" : string.Empty;

                    this.lbl1.Text = eventWhere.DisplaySubjectName;
                    this.lbl1.Tag = string.Empty;

                    this.lbl2.Text = eventWhere.DisplayClassName;
                    this.lbl2.Tag = !string.IsNullOrEmpty(eventWhere.ClassID) ? string.Format("Class：{0}", eventWhere.ClassID) : string.Empty;

                    this.lbl3.Text = "共有" + this._events.Count + "門分課";
                    this.lbl3.Tag = string.Empty;
                }
                #endregion
                else
                {
                    //未知的...
                }
            }
            #endregion
            else 
            {                
                CEvent eventLocal = this._events[0];

                this.picBox.Image = eventLocal.ManualLock ? Resources.lock_3 : Resources.blank;
                this.picBox.Tag = eventLocal.ManualLock ? "lock" : string.Empty;

                this.BackColor = SchedulerColor.lvTimeTableBackColor;

                this.lbl1.Text = (eventLocal == null ? "" : eventLocal.DisplaySubjectName);

                switch (this._schType)
                {
                    case SchedulerType.Teacher:
                        this.lbl2.Text = eventLocal.DisplayClassName;
                        this.lbl2.Tag = !string.IsNullOrEmpty(eventLocal.ClassID) ? string.Format("Class：{0}", eventLocal.ClassID) : string.Empty;

                        this.lbl3.Text = eventLocal.DisplayClassroomName;
                        this.lbl3.Tag = !string.IsNullOrEmpty(eventLocal.ClassroomID) ? string.Format("Classroom：{0}", eventLocal.ClassroomID) : string.Empty;

                        this.lbl4.Text = "多門分課...";
                        this.lbl4.Tag = this._events;

                        break;
                    case SchedulerType.Class:
                        this.lbl2.Text = eventLocal.DisplayTeacherName;
                        this.lbl2.Tag = !string.IsNullOrEmpty(eventLocal.TeacherID1) ? string.Format("Teacher：{0}", eventLocal.TeacherID1) : string.Empty;

                        this.lbl3.Text = eventLocal.DisplayClassroomName;
                        this.lbl3.Tag = !string.IsNullOrEmpty(eventLocal.ClassroomID) ? string.Format("Classroom：{0}", eventLocal.ClassroomID) : string.Empty;

                        this.lbl4.Text = "多門分課...";
                        this.lbl4.Tag = this._events;
                        
                        break;
                    case SchedulerType.Classroom:
                        this.lbl2.Text = eventLocal.DisplayTeacherName;
                        this.lbl2.Tag = !string.IsNullOrEmpty(eventLocal.TeacherID1) ? string.Format("Teacher：{0}", eventLocal.TeacherID1) : string.Empty;

                        this.lbl3.Text = eventLocal.DisplayClassName;
                        this.lbl3.Tag = !string.IsNullOrEmpty(eventLocal.ClassID) ? string.Format("Class：{0}", eventLocal.ClassID) : string.Empty;

                        this.lbl4.Text = "多門分課...";
                        this.lbl4.Tag = this._events;
                        
                        break;
                }
            }
        }

        private void SetLabelText(ref int Index,string Text, string Tag)
        {
            if (Index == 1)
            {
                lbl1.Text = Text;
                lbl1.Tag = Tag;
            }
            else if (Index == 2)
            {
                lbl2.Text = Text;
                lbl2.Tag = Tag;
            }
            else if (Index == 3)
            {
                lbl3.Text = Text;
                lbl3.Tag = Tag;
            }
            else if (Index == 4)
            {
                lbl4.Text = Text;
                lbl4.Tag = Tag;
            }

            Index++;
        }

        private void SetSingleEvent(CEvent _vo)
        {
            this.picBox.Image = (_vo.ManualLock) ? Properties.Resources.lock_3 : null;
            this.picBox.Tag = (_vo.ManualLock) ? "lock" : string.Empty;

            if (!string.IsNullOrEmpty(TimeTableID) && !TimeTableID.Equals(_vo.TimeTableID))
                this.BackColor = this.unselectedColor;
            else
                this.BackColor = SchedulerColor.lvScheduledBackColor;           

            int index = 1;

            if (Option.IsSubject)
                SetLabelText(ref index,_vo.DisplaySubjectName,string.Empty);

            if (Option.IsSubjectAlias)
                SetLabelText(ref index, _vo.SubjectAlias, string.Empty);

            if (Option.IsClass)
                SetLabelText(
                    ref index,
                    _vo.DisplayClassName,
                    !string.IsNullOrEmpty(_vo.ClassID) ? string.Format("Class：{0}", _vo.ClassID) : string.Empty);

            if (Option.IsTeacher)
                SetLabelText(
                    ref index,
                    _vo.DisplayTeacherName,
                    !string.IsNullOrEmpty(_vo.TeacherID1) ? string.Format("Teacher：{0}", _vo.TeacherID1) : string.Empty);

            if (Option.IsClassroom)
                SetLabelText(
                    ref index, 
                    _vo.DisplayClassroomName,
                    !string.IsNullOrEmpty(_vo.ClassroomID) ? string.Format("Classroom：{0}", _vo.ClassroomID) : string.Empty);
        }

        /// <summary>
        /// 指定事件
        /// </summary>
        public List<CEvent> Data
        {
            get { return this._events; }
            set { 
                this._events = value;

                if (_events.Count == 1)                
                {
                    CEvent _vo = _events[0];

                    SetSingleEvent(_vo);
                }else 
                    SetMultipleEvents();
            }
        }

        public DevComponents.DotNetBar.PanelEx Panel
        {
            get { return this._pnl; }
        }

        /// <summary>
        /// 背景顏色
        /// </summary>
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

    /// <summary>
    /// 節次事件
    /// </summary>
    public class PeriodEventArgs : EventArgs
    {
        public int Weekday;
        public int Period;
        public List<CEvent> Value;
        public PeriodEventArgs(int weekday, int period, List<CEvent> vo)
        {
            this.Weekday = weekday;
            this.Period = period;
            this.Value = vo;
        }
    }
}
