//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Sunset.Data;

//namespace ischedule
//{
//    public class ScheduleEventCommand : IScheduleCommand
//    {
//        private List<SimpleEvent> SimpleEvents;
//        private Scheduler schLocal = Scheduler.Instance;

//        #region IScheduleCommand Members

//        public void Do(List<SimpleEvent> SimpleEvents)
//        {
//            foreach(SimpleEvent SimpleEvent in SimpleEvents)
//            {
//                schLocal.ScheduleEvent(SimpleEvent.EventID, SimpleEvent.Weekday, SimpleEvent.PeriodNo);
//            }
//        }

//        public void Undo()
//        {
//            CEvents evtsFree = new CEvents();

//            foreach(SimpleEvent SimpleEvent in SimpleEvents)
//            {
//                CEvent evtFree = schLocal.CEvents[SimpleEvent.EventID];
//                evtsFree.Add(evtFree);
//            }

//            schLocal.FreeEvents(evtsFree);
//        }

//        public string Description()
//        {
//            return string.Empty;
//        }
//        #endregion
//    }
//}