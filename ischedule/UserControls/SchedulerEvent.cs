using System;

namespace ischedule
{
    /// <summary>
    /// 排課課程更新事件
    /// </summary>
    public static class SchedulerEvents
    {
        /// <summary>
        /// 引發排課課程更新事件
        /// </summary>
        public static void RaisePeriodSelected(PeriodEventArgs Period)
        {
            if (PeriodSelected!=null)
                PeriodSelected(null,Period);
        }

        /// <summary>
        /// 引發選取多筆事件
        /// </summary>
        public static void RaiseClearEvent()
        {
            if (ClearEvent != null)
                ClearEvent(null, new EventArgs());
        }

        /// <summary>
        /// 引發不排課時段編輯事件
        /// </summary>
        public static void RaiseBusyEditorEvent()
        {
            if (BusyEditorEvent != null)
                BusyEditorEvent(null, new EventArgs()); 
        }

        /// <summary>
        /// 節次選取事件
        /// </summary>
        public static event EventHandler<PeriodEventArgs> PeriodSelected;

        /// <summary>
        /// 選取多個事件
        /// </summary>
        public static event EventHandler ClearEvent;

        /// <summary>
        /// 不排課時段修改事件
        /// </summary>
        public static event EventHandler BusyEditorEvent;
    }
}