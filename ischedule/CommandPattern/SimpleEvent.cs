
namespace ischedule
{
    /// <summary>
    /// 簡單事件，記錄事件系統編號、星期及節次。
    /// </summary>
    public class SimpleEvent
    {
        /// <summary>
        /// 事件系統編號
        /// </summary>
        public string EventID { get; set; }

        /// <summary>
        /// 星期
        /// </summary>
        public int Weekday { get; set; }

        /// <summary>
        /// 節次
        /// </summary>
        public int PeriodNo { get; set; }
    }
}