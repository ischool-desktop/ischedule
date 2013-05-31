using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ischedule
{
    /// <summary>
    /// 節次顯示物件
    /// </summary>
    public class LPViewReportItem
    {
        /// <summary>
        /// 節次編號
        /// </summary>
        public int PeriodNo { get; set; }

        /// <summary>
        /// 時間
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// 週一文字
        /// </summary>
        public string Mon { get; set; }

        /// <summary>
        /// 週二文字
        /// </summary>
        public string Tue { get; set; }

        /// <summary>
        /// 週三文字
        /// </summary>
        public string Wed { get; set; }

        /// <summary>
        /// 週四文字
        /// </summary>
        public string Thu { get; set; }

        /// <summary>
        /// 週五文字
        /// </summary>
        public string Fri { get; set; }

        /// <summary>
        /// 週六文字
        /// </summary>
        public string Sat { get; set; }

        /// <summary>
        /// 週日文字
        /// </summary>
        public string Sun { get; set; }

        /// <summary>
        /// 建構式，將所有顏色設為白色，並將所有文字設為string.Empty。
        /// </summary>
        public LPViewReportItem()
        {
            PeriodNo = 0;

            Mon = string.Empty;
            Tue = string.Empty;
            Wed = string.Empty;
            Thu = string.Empty;
            Fri = string.Empty;
            Sat = string.Empty;
            Sun = string.Empty;
        }

        /// <summary>
        /// 根據星期取得文字
        /// </summary>
        /// <param name="WeekDay"></param>
        /// <returns></returns>
        public string GetWeekDayText(int WeekDay)
        {
            switch (WeekDay)
            {
                case 1:
                    return Mon;
                case 2:
                    return Tue;
                case 3:
                    return Wed;
                case 4:
                    return Thu;
                case 5:
                    return Fri;
                case 6:
                    return Sat;
                case 7:
                    return Sun;
            }

            return string.Empty;
        }

        /// <summary>
        /// 設定星期資訊
        /// </summary>
        /// <param name="WeekDay">星期幾</param>
        /// <param name="Text">文字</param>
        public void SetWeekDayText(int WeekDay, string Text)
        {
            switch (WeekDay)
            {
                case 1:
                    Mon = Text;
                    break;
                case 2:
                    Tue = Text;
                    break;
                case 3:
                    Wed = Text;
                    break;
                case 4:
                    Thu = Text;
                    break;
                case 5:
                    Fri = Text;
                    break;
                case 6:
                    Sat = Text;
                    break;
                case 7:
                    Sun = Text;
                    break;
            }
        }
    }
}
