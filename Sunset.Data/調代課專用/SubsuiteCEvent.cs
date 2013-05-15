using System;

namespace Sunset.Data
{
    /// <summary>
    /// 代課事件
    /// </summary>
    public class SubsuiteCEvent
    {
        /// <summary>
        /// 事件系統編號
        /// </summary>
        public int CEventID { get; set; }

        /// <summary>
        /// 代課日期
        /// </summary>
        public DateTime SubstituteDate { get; set; }

        /// <summary>
        /// 代課教師姓名
        /// </summary>
        public string SubstituteWhoID { get; set; }

        /// <summary>
        /// 代課教師系統編號
        /// </summary>
        public string WhoID { get; set; }

        /// <summary>
        /// 假別名稱
        /// </summary>
        public string AbsenceName { get; set; }

        /// <summary>
        /// 鐘點費
        /// </summary>
        public int HourlyPay { get; set; }
    }
}