using System;

namespace Sunset.Data
{
    /// <summary>
    /// 調課事件
    /// </summary>
    public class ExchangeCEvent
    {
        /// <summary>
        /// 來源課程分段系統編號
        /// </summary>
        public int SrcCEventID { get; set; }

        /// <summary>
        /// 目標課程分段系統編號
        /// </summary>
        public int DesCEventID { get; set; }

        /// <summary>
        /// 來源調課日期
        /// </summary>
        public DateTime SrcExchangeDate { get; set; }

        /// <summary>
        /// 目標調課日期
        /// </summary>
        public DateTime DesExchangeDate { get; set; }

        /// <summary>
        /// 調課原因
        /// </summary>
        public string Reason { get; set; }
    }
}