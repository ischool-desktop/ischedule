using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;
using Sunset.Data;

namespace ischedule
{
    /// <summary>
    /// 時間表不排課編輯器
    /// </summary>
    public class TimeTableBusyEditor
    {
        private DataGridViewX grdEditor;
        private List<int> Weekdays = new List<int>();
        private List<int> Periods = new List<int>();
        private List<TimeTableSec> mTimeTableSecs;
        private List<SchPeriod> mPeriods = new List<SchPeriod>();
        private Dictionary<int, int> mWeekdayRowIndex = new Dictionary<int, int>();
        public string DefaultBusyMessage { get { return "不排課"; } }

        /// <summary>
        /// 建構式，傳入DataGridViewX
        /// </summary>
        /// <param name="grdEditor"></param>
        public TimeTableBusyEditor(DataGridViewX grdEditor)
        {
            if (grdEditor == null)
                throw new Exception("建構式傳入的DataGridViewX不得為null。");

            this.grdEditor = grdEditor;
        }

        /// <summary>
        /// 設為可排課時段
        /// </summary>
        public void SetFree()
        {
            foreach (DataGridViewCell Cell in grdEditor.SelectedCells)
            {
                if (Cell.ColumnIndex != 0 && Cell.Style.BackColor != Color.LightGray)
                    Cell.Value = string.Empty;
            }
        }

        /// <summary>
        /// 設定不排課時段
        /// </summary>
        public void SetBusy()
        {
            foreach (DataGridViewCell Cell in grdEditor.SelectedCells)
            {
                if (Cell.ColumnIndex != 0 && Cell.Style.BackColor != Color.LightGray)
                    Cell.Value = DefaultBusyMessage;
            }
        }

        /// <summary>
        /// 設定不排課時段訊息
        /// </summary>
        public void SetBusyDes()
        {
            frmInputBox InputBox = new frmInputBox("請輸入不排課訊息");

            if (InputBox.ShowDialog() == DialogResult.OK)
            {
                string Message = InputBox.Message;

                foreach (DataGridViewCell Cell in grdEditor.SelectedCells)
                {
                    if (Cell.ColumnIndex != 0 && Cell.Style.BackColor != Color.LightGray)
                        Cell.Value = InputBox.Message;
                }
            }
        }

        /// <summary>
        /// 檢查兩個時間表時段是否有衝突
        /// </summary>
        /// <param name="BeginPeriod"></param>
        /// <param name="TestPeriod"></param>
        /// <returns></returns>
        private bool IsTimeIntersectsWith(SchPeriod BeginPeriod, SchPeriod TestPeriod)
        {
            //若星期不相同則不會相衝
            if (BeginPeriod.Weekday != TestPeriod.Weekday)
                return false;

            //將TestTime的年、月、日及秒設為與Appointment一致，以確保只是單純針對小時及分來做時間差的運算
            DateTime BeginTime = new DateTime(1900, 1, 1, BeginPeriod.Hour, BeginPeriod.Minute, 0);

            DateTime TestTime = new DateTime(1900, 1, 1, TestPeriod.Hour, TestPeriod.Minute, 0);

            //將Appointment的NewBeginTime減去NewTestTime
            int nTimeDif = (int)BeginTime.Subtract(TestTime).TotalMinutes;

            //狀況一：假設nTimeDif為正，並且大於NewTestTime，代表兩者沒有交集，傳回false。
            //舉例：
            //Appointment.BeginTime為10：00，其Duration為40。
            //TestTime為9：00，其Duration為50。
            if (nTimeDif >= TestPeriod.Duration)
                return false;

            //狀況二：假設nTimeDiff為正，並且小於TestDuration，代表兩者有交集，傳回true。
            //舉例：
            //Appointment.BeginTime為10：00，其Duration為40。
            //TestTime為9：00，其Duration為80。
            if (nTimeDif >= 0)
                return true;
            //狀況三：假設nTimeDiff為負值，將nTimeDiff成為正值，若是-nTimeDiff小於Appointment.Duration；
            //代表NewBeginTime在NewTestTime之前，並且NewBegin與NewTestTime的絕對差值小於Appointment.Duration的時間
            //舉例：
            //Appointment.BeginTime為10：00，其Duration為40。
            //TestTime為10：30，其Duration為20。
            else if (-nTimeDif < BeginPeriod.Duration)
                return true;

            //其他狀況傳回沒有交集
            //舉例：
            //Appointment.BeginTime為10：00，其Duration為40。
            //TestTime為10：50，其Duration為20。
            return false;
        }

        /// <summary>
        /// 設定時間表
        /// </summary>
        /// <param name="TimeTableSecs"></param>
        public void SetTimeTableSecs(List<TimeTableSec> TimeTableSecs)
        {
            grdEditor.SuspendLayout();

            this.mTimeTableSecs = TimeTableSecs;

            foreach (TimeTableSec TimeTableSec in TimeTableSecs)
            {
                if (!Weekdays.Contains(TimeTableSec.WeekDay))
                    Weekdays.Add(TimeTableSec.WeekDay);

                if (!Periods.Contains(TimeTableSec.Period))
                    Periods.Add(TimeTableSec.Period);
            }

            Weekdays.Sort();

            grdEditor.Rows.Clear();
            grdEditor.Columns.Clear();
            grdEditor.Columns.Add("colEmpty", " ");             

            foreach(int Weekday in Weekdays)
                grdEditor.Columns.Add("col" + Weekday,GetWeekDayText(Weekday));

            grdEditor.Columns[0].Width = 30;

            for (int i = 1; i < grdEditor.Columns.Count; i++)
                grdEditor.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;         

            Periods.Sort();
            mWeekdayRowIndex = new Dictionary<int, int>();

            foreach (int Period in Periods)
            {
                int Index = grdEditor.Rows.Add();
                grdEditor.Rows[Index].Height = 45;
                grdEditor.Rows[Index].SetValues("" + Period);

                if (!mWeekdayRowIndex.ContainsKey(Period))
                    mWeekdayRowIndex.Add(Period, Index);           
            }

            foreach (DataGridViewRow Row in grdEditor.Rows)
            {
                int Period = int.Parse(""+Row.Cells[0].Value);

                foreach (DataGridViewCell Cell in Row.Cells)
                {
                    if (Cell.ColumnIndex != 0)
                    {
                        int Weekday = Cell.ColumnIndex;

                        if (mTimeTableSecs
                            .Find(x => x.WeekDay.Equals(Weekday) && x.Period.Equals(Period)) == null)
                        {
                            DataGridViewCellStyle Style = new DataGridViewCellStyle();
                            Style.BackColor = Color.LightGray;
                            Cell.Style = Style;
                        }
                    }
                }
            }

            grdEditor.ResumeLayout();
        }

        /// <summary>
        /// 設定已排課節次
        /// </summary>
        public void SetScheduledPeriod(List<SchPeriod> vPeriods)
        {
 
        }

        /// <summary>
        /// 設定不排課節次
        /// </summary>
        /// <param name="vBusyPeriods"></param>
        public void SetPeriods(List<SchPeriod> vPeriods)
        {
            List<TimeTableSec> BusyTimeTableSecs = new List<TimeTableSec>();
            List<TimeTableSec> ScheduledTimeTableSecs = new List<TimeTableSec>();

            foreach (DataGridViewRow Row in grdEditor.Rows)
            {
                foreach (DataGridViewCell Cell in Row.Cells)
                {
                    if (Cell.ColumnIndex!=0)
                        Cell.Value = "";
                }
            }

            if (this.mTimeTableSecs == null || this.mTimeTableSecs.Count == 0)
                return;

            this.mPeriods = vPeriods;

            foreach (SchPeriod vPeriod in mPeriods)
            {
                List<TimeTableSec> Secs = this.mTimeTableSecs
                    .FindAll(x => x.WeekDay.Equals(vPeriod.Weekday));

                foreach (TimeTableSec Sec in Secs)
                {
                    SchPeriod secPeriod = Sec.ToPeriod();

                    if (vPeriod.IsTimeIntersectsWith(secPeriod))
                    {
                        if (string.IsNullOrEmpty(vPeriod.EventID))
                        {
                            Sec.DisableMessage = vPeriod.Desc;
                            BusyTimeTableSecs.Add(Sec);
                        }
                        else
                        {
                            ScheduledTimeTableSecs.Add(Sec);
                        }
                    }
                }
            }

            grdEditor.SuspendLayout();

            foreach (TimeTableSec BusyTimeTableSec in BusyTimeTableSecs)
            {
                int RowIndex = mWeekdayRowIndex[BusyTimeTableSec.Period];
                grdEditor.Rows[RowIndex].Cells[BusyTimeTableSec.WeekDay].Value = string.IsNullOrEmpty(BusyTimeTableSec.DisableMessage) ? "X" : BusyTimeTableSec.DisableMessage;
            }

            foreach (TimeTableSec ScheduledTimeTableSec in ScheduledTimeTableSecs)
            {
                int RowIndex = mWeekdayRowIndex[ScheduledTimeTableSec.Period];

                DataGridViewCellStyle Style = new DataGridViewCellStyle();
                Style.BackColor = Color.LightGray;

                grdEditor.Rows[RowIndex].Cells[ScheduledTimeTableSec.WeekDay].Style = Style; 
            }

            grdEditor.ResumeLayout();
        }

        /// <summary>
        /// 取得不排課節次
        /// </summary>
        /// <returns></returns>
        public List<SchPeriod> GetPeriods()
        {
            List<SchPeriod> Periods = new List<SchPeriod>();

            //針對每個格子
            foreach (DataGridViewRow row in grdEditor.Rows)
            {
                foreach(DataGridViewCell cell in row.Cells)
                {
                    if (cell.ColumnIndex != 0)
                    {
                        string CellValue = "" + cell.Value;

                        //判斷是否為不排課，若是的話加入到節次清單中
                        if (!string.IsNullOrEmpty(CellValue))
                        {
                            int Weekday = cell.ColumnIndex;
                            int Period = int.Parse("" + row.Cells[0].Value);

                            TimeTableSec vTimeTableSec = mTimeTableSecs
                                .Find(x => x.WeekDay.Equals(Weekday) && x.Period.Equals(Period));

                            if (vTimeTableSec != null)
                            {
                                SchPeriod vTimeTableSecPeriod = vTimeTableSec.ToPeriod();
                                vTimeTableSecPeriod.Desc = CellValue.Equals("X")?string.Empty:CellValue;
                                Periods.Add(vTimeTableSecPeriod);
                            }
                        }
                    }
                }
            }

            return Periods;
        }

        public string GetWeekDayText(int WeekDay)
        {
            switch (WeekDay)
            {
                case 1:
                    return "一";
                case 2:
                    return "二";
                case 3:
                    return "三";
                case 4:
                    return "四";
                case 5:
                    return "五";
                case 6:
                    return "六";
                case 7:
                    return "日";
                default:
                return string.Empty;
            }
        }
    }
}