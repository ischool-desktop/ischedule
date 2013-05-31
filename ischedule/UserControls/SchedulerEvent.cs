﻿using System;

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
        public static void RaiseChanged(PeriodEventArgs Period)
        {
            if (PeriodSelected!=null)
                PeriodSelected(null,Period);
        }

        /// <summary>
        /// 節次選取事件
        /// </summary>
        public static event EventHandler<PeriodEventArgs> PeriodSelected;
    }
}