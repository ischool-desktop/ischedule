using System;

namespace ischedule
{
    /// <summary>
    /// 時間表時段
    /// </summary>
    public class TimeTableSec
    {
        /// <summary>
        /// 時間表時段系統編號
        /// </summary>
        public string TimeTableSecID { get; set; }

        /// <summary>
        /// 時間表系統編號
        /// </summary>
        public string TimeTableID { get; set; }

        /// <summary>
        /// 星期
        /// </summary>
        public int WeekDay { get; set; }

        /// <summary>
        /// 節次
        /// </summary>
        public int Period { get; set; }

        /// <summary>
        /// 開始時間
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 持續時間
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// 顯示節次
        /// </summary>
        public int DispPeriod { get; set; }

        /// <summary>
        /// 地點系統編號
        /// </summary>
        public long LocationID { get; set; }

        /// <summary>
        /// 不排課訊息
        /// </summary>
        public string DisableMessage { get; set; }
    }
}