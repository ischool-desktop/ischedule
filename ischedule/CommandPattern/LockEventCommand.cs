using Sunset.Data;

namespace ischedule
{
    /// <summary>
    /// 鎖定事件
    /// </summary>
    public class LockEventCommand : IScheduleCommand
    {
        #region ICommand Members
        private string EventID;
        private Scheduler schLocal = Scheduler.Instance;

        /// <summary>
        /// 建構式，傳入事件系統編號
        /// </summary>
        /// <param name="EventID"></param>
        public LockEventCommand(string EventID)
        {
            this.EventID = EventID;
        }
        
        /// <summary>
        /// 執行
        /// </summary>
        public void Do()
        {
            schLocal.LockEvent(this.EventID);   
        }

        /// <summary>
        /// 回復
        /// </summary>
        public void Undo()
        {
            schLocal.UnlockEvent(this.EventID);
        }

        /// <summary>
        /// 描述
        /// </summary>
        /// <returns></returns>
        public string Description()
        {
            return "鎖定事件";
        }
        #endregion
    }
}
