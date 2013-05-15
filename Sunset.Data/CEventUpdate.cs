
namespace Sunset.Data
{
    /// <summary>
    /// 課程分段異動記錄
    /// </summary>
    public class CEventUpdate
    {
        /// <summary>
        /// 事件系統編號
        /// </summary>
        public string EventID { get; set; }
    }

    /// <summary>
    /// 安排事件
    /// </summary>
    public class CEventScheduledUpdate : CEventUpdate
    {
 
    }

    /// <summary>
    /// 釋放事件
    /// </summary>
    public class CEventFreeUpdate : CEventUpdate
    {
        /// <summary>
        /// 星期，當釋放事件時才會有值
        /// </summary>
        public int WeekDay { get; set; }

        /// <summary>
        /// 節次，當釋放事件時才會有值
        /// </summary>
        public int PeriodNo { get; set; } 
    }

    /// <summary>
    /// 變更屬性
    /// </summary>
    public class CEventChangeWhoUpdate : CEventUpdate
    {
        /// <summary>
        /// 原來教師
        /// </summary>
        public string WhoID { get; set; } 
    }
}