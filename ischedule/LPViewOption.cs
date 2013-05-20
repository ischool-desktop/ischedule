using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ischedule
{
    public class LPViewOption
    {
        public string TimeTableID { get; set; }

        public int CapacityIndex { get; set; }

        public byte WeekFlag { get; set; }

        public bool IsTeacher { get; set; }

        public bool IsClassroom { get; set; }

        public bool IsClass { get; set; }

        public bool IsSubject { get; set; }

        public bool IsSubjectAlias { get; set; }

        public bool IsTimeTable { get; set; }

        public LPViewOption()
        {
            TimeTableID = string.Empty;
            CapacityIndex = 0;
            WeekFlag = 1;
            IsTeacher = false;
            IsClassroom = false;
            IsClass = false;
            IsSubject = false;
            IsSubjectAlias = false;
            IsTimeTable = false;
        }
    }
}