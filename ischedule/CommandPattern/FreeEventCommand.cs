using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sunset.Data;

namespace ischedule
{
    /// <summary>
    /// 釋放事件
    /// </summary>
    public class FreeEventCommand : IScheduleCommand
    {
        private string EventID;
        private Scheduler schLocal = Scheduler.Instance;

        #region IScheduleCommand Members

        /// <summary>
        /// 建構式，傳入事件系統編號
        /// </summary>
        /// <param name="EventID"></param>
        public FreeEventCommand(string EventID)
        {
            this.EventID = EventID;
        }

        /// <summary>
        /// 執行
        /// </summary>
        public void Do()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 回復
        /// </summary>
        public void Undo()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 描述
        /// </summary>
        /// <returns></returns>
        public string Description()
        {
            return "釋放事件";
        }

        #endregion
    }
}