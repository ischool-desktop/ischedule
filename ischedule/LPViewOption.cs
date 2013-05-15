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

        public bool IsWho { get; set; }

        public bool IsWhere { get; set; }

        public bool IsWhom { get; set; }

        public bool IsWhat { get; set; }

        public bool IsWhatAlias { get; set; }

        public bool IsTimeTable { get; set; }

        public LPViewOption()
        {
            TimeTableID = string.Empty;
            CapacityIndex = 0;
            WeekFlag = 1;
            IsWho = false;
            IsWhere = false;
            IsWhom = false;
            IsWhat = false;
            IsWhatAlias = false;
            IsTimeTable = false;
        }
    }
}