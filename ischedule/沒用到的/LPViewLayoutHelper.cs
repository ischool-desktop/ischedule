using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace ischedule
{
    public class LPViewLayoutHelper
    {
        #region Const
        private const int WeekdayHeight = 20;
        private const int PeriodWidth = 20;
        #endregion

        private List<string> strWeekdays = new List<string>() { "一", "二", "三", "四", "五", "六", "日" };
        private Dictionary<int, LabelX> WeekdayCache = new Dictionary<int, LabelX>();
        private Dictionary<int, LabelX> PeriodCache = new Dictionary<int, LabelX>();
        private Dictionary<string, ucPeriod> ucPeriodCache = new Dictionary<string, ucPeriod>();

        private List<LabelX> CurrentWeekdays = new List<LabelX>();
        private List<LabelX> CurrentPeriods = new List<LabelX>();
        private Dictionary<string, ucPeriod> CurrentPeriodCaches = new Dictionary<string, ucPeriod>();
        
        private UserControl LPViewControl;

        public LPViewLayoutHelper(UserControl LPViewControl)
        {
            this.LPViewControl = LPViewControl;
            this.LPViewControl.Dock = DockStyle.Fill;
            this.LPViewControl.Resize += (sender,e) =>
            {
                ResizeWeekdayControl();
                ResizePeriodControl();
                ResizeCellControl();
            };

            WeekdayCache = new Dictionary<int, LabelX>();
            PeriodCache = new Dictionary<int, LabelX>();
            ucPeriodCache = new Dictionary<string, ucPeriod>();
        }

        #region private function
        /// <summary>
        /// 調整星期的寬度及左方位置
        /// </summary>
        private void ResizeWeekdayControl()
        {
            if (CurrentWeekdays.Count == 0)
                return;

            int ColumnWidth = (int)((this.LPViewControl.Width - 20)/CurrentWeekdays.Count);

            for(int i=0;i<CurrentWeekdays.Count;i++)
            {
                LabelX Label = CurrentWeekdays[i];
                Label.Width = ColumnWidth;
                Label.Left = 20 + (i * ColumnWidth); 
            }
        }

        /// <summary>
        /// 調整節次的高度及上方位置
        /// </summary>
        private void ResizePeriodControl()
        {
            if (CurrentPeriods.Count == 0)
                return;

            int RowHeight = (int)((this.LPViewControl.Height - 20) / CurrentPeriods.Count);

            for (int i = 0; i < CurrentPeriods.Count; i++)
            {
                LabelX Label = CurrentPeriods[i];
                Label.Height = RowHeight;
                Label.Top = 20 + (i * RowHeight);
            }
        }

        /// <summary>
        /// 調整格子大小
        /// </summary>
        private void ResizeCellControl()
        {
            LPViewControl.Visible = false;

            if (CurrentWeekdays.Count == 0 ||
                CurrentPeriods.Count == 0)
                return;

            int ColumnWidth = (int)((this.LPViewControl.Width - 20) / CurrentWeekdays.Count);
            int RowHeight = (int)((this.LPViewControl.Height - 20) / CurrentPeriods.Count);

            foreach (string Key in CurrentPeriodCaches.Keys)
            {
                string[] values = Key.Split(new char[] { ',' });

                int Column = int.Parse(values[0]);
                int Row = int.Parse(values[1]);

                ucPeriod Period = CurrentPeriodCaches[Key];
                int Left = 20 + ((Column-1) * ColumnWidth);
                int Top = 20 + ((Row-1) * RowHeight);

                Period.SetBounds(Left , Top ,ColumnWidth,RowHeight);
            }

            LPViewControl.Visible = true;
        }
        #endregion

        public Tuple<int, int> GetWeekdayAndPeriod()
        {
            return new Tuple<int, int>(CurrentWeekdays.Count,CurrentPeriods.Count);
        }

        /// <summary>
        /// 設定星期
        /// </summary>
        /// <param name="Weekday"></param>
        public void SetWeekday(int Weekday)
        {
            //若星期為0則離開
            if (Weekday == 0)
                return;

            //將目前的星期清空
            CurrentWeekdays.Clear();

            //填入星期列表
            for (int i=1;i<=Weekday;i++)
            {
                Control[] Controls = LPViewControl.Controls.Find("lblWeekday" + i, false);

                if (Controls.Length > 0)
                {
                    LabelX lblWeekday = Controls[0] as LabelX;

                    lblWeekday.Top = 0;
                    lblWeekday.Height = WeekdayHeight;

                    lblWeekday.Text = strWeekdays[i-1];
                    CurrentWeekdays.Add(lblWeekday);
                }
            }

            ResizeWeekdayControl();

            CurrentWeekdays.ForEach
            (x =>
              {
                  x.Visible = true;
              }
            );
        }

        /// <summary>
        /// 根據星期及節次取得節次物件
        /// </summary>
        /// <param name="Weekday">星期</param>
        /// <param name="Period">節次</param>
        /// <returns>節次物件</returns>
        public ucPeriod GetPeriod(int Weekday, int Period)
        {
            string Key = Weekday + "," + Period;

            if (CurrentPeriodCaches.ContainsKey(Key))
            {
                ucPeriod ucPeriod = CurrentPeriodCaches[Key];

                ucPeriod.IsSelected = false;

                return ucPeriod;
            }
            else
                return null;
        }

        /// <summary>
        /// 設定節次
        /// </summary>
        /// <param name="Periods"></param>
        public void SetPeriod(List<int> Periods)
        {
            if (Periods.Count == 0)
                return;

            CurrentPeriods.Clear();

            for (int i = 1; i <= Periods.Count; i++)
            {
                Control[] Controls = LPViewControl.Controls.Find("lblPeriod" + i, false);

                if (Controls.Length > 0)
                {
                    LabelX Period = Controls[0] as LabelX;
                    Period.Left = 0;
                    Period.Width = PeriodWidth;
                    Period.Text = "" + i;
                    CurrentPeriods.Add(Period);
                }
            }

            ResizePeriodControl();

            CurrentPeriods.ForEach
            (x =>
                {
                    x.Visible = true;
                }
            );
        }

        public void SetCellsToBlank()
        {
            foreach (ucPeriod Period in CurrentPeriodCaches.Values)
                Period.SetBlank();
        }

        /// <summary>
        /// 設定格子
        /// </summary>
        /// <param name="ColumnCount"></param>
        /// <param name="RowCount"></param>
        public void SetCells(int ColumnCount,int RowCount)
        {
            CurrentPeriodCaches.Clear();

            for (int ColIndex = 1; ColIndex <= ColumnCount; ColIndex++)
                for (int RowIndex = 1; RowIndex <= RowCount ; RowIndex++)
                {
                    string Key = ColIndex + "," + RowIndex;

                    Control[] Controls = LPViewControl.Controls.Find("ucPeriod"+ColIndex+RowIndex ,false);

                    if (Controls.Length > 0)
                    {
                        ucPeriod Period = Controls[0] as ucPeriod;

                        Period.Visible = true;
                        CurrentPeriodCaches.Add(Key,Period);
                    }
                }

            ResizeCellControl();

            foreach (ucPeriod Period in CurrentPeriodCaches.Values)
            {
                Period.Visible = true;
            }
        }

        public void ResetLayout()
        {
            LPViewControl.Visible = false;

            LPViewControl.SuspendLayout();

            foreach (Control Control in LPViewControl.Controls)
            {
                Control.Visible = false;
            }
        }

        public void ResumeLayout()
        {
            LPViewControl.ResumeLayout();

            LPViewControl.Visible = true;
        }
    }
}