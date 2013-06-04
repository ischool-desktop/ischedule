using System.Collections.Generic;
using Sunset.Data;

namespace ischedule.test
{
    class TeacherWrapper
    {
        private Teacher t;
        public TeacherWrapper(Teacher t)
        {
            this.t = t;
        }

        public override string ToString()
        {
            string result = "";
            List<CEvent> events = Scheduler.Instance.CEvents.GetEventsByTeacherID(t.TeacherID);
            if (events.Count > 0)
                result = string.Format("{0} ({1})", t.Name, events.Count.ToString());
            
            return result ;
        }

        public Teacher Teacher
        {
            get { return this.t; }
        }
    }
}
