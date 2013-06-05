using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sunset.Data;

namespace ischedule
{
    internal class SimpleEvent
    {
        public string EventID { get; set; }

        public int Weekday { get; set; }

        public int PeriodNo { get; set; } 
    }

    /// <summary>
    /// 釋放事件
    /// </summary>
    public class FreeEventCommand : IScheduleCommand
    {
        private CEvents evtsFree;
        private List<SimpleEvent> SimpleEvents = new List<SimpleEvent>();
        private Scheduler schLocal = Scheduler.Instance;

        #region IScheduleCommand Members

        /// <summary>
        /// 建構式，傳入事件系統編號
        /// </summary>
        /// <param name="EventID"></param>
        public FreeEventCommand(CEvents evtsFree)
        {
            if (evtsFree == null)
                throw new NullReferenceException("事件列表不得為null");

            this.evtsFree = evtsFree;
        }

        /// <summary>
        /// 執行
        /// </summary>
        public void Do()
        {
            this.SimpleEvents.Clear();

            foreach (CEvent evtFree in evtsFree)
            {
                SimpleEvent Event = new SimpleEvent();
                Event.EventID = evtFree.EventID;
                Event.Weekday = evtFree.WeekDay;
                Event.PeriodNo = evtFree.PeriodNo;
                SimpleEvents.Add(Event);
            }

            schLocal.FreeEvents(this.evtsFree);
        }

        /// <summary>
        /// 回復
        /// </summary>
        public void Undo()
        {
            for (int i = 0; i < this.SimpleEvents.Count; i++)
            {
                SimpleEvent eventSimple = this.SimpleEvents[this.SimpleEvents.Count - 1 - i];
                schLocal.ScheduleEvent(eventSimple.EventID, eventSimple.Weekday, eventSimple.PeriodNo);
            }
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