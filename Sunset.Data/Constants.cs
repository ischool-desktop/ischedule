using System;

namespace Sunset.Data
{
    [Flags]
    public enum MaskOptions
    {
        maskWho = 1,
        maskWhere = 2,
        maskOther = 4
    }

    public enum ScheduleMode
    {
        modSchedule,modSchedulePlus
    }

    public static class Constants
    {

        static Constants()
        {
            ExecutionMode = ScheduleMode.modSchedulePlus;
        }

//This module defines constants share among forms. Each class module
//will define its own constants again in its module code to assure code
//independence

//-- Error codes
// - Appointment object
//public const ErrAppLocked = vbObjectError + 1

// - Period object
//public const errPrdLocked = vbObjectError + 2

// - Scheduler object
//public const errDbAlreadyOpen = vbObjectError + 1001
//public const errDbAlreadyClose = vbObjectError + 1002
//public const errDbNotOpenYet = vbObjectError + 1003

//-- Return values

        /// <summary>
        /// 排課執行模式
        /// </summary>
        public static ScheduleMode ExecutionMode { get; set; }

        /// <summary>
        /// 目前星期開始，以週一為啟始
        /// </summary>
        public static DateTime CurrentWeekStartDate { get; set; }

        //Appointments object
        public const int apsNoConflict = 0;
        public const int apsTimeConflict = 1;
        public const int apsTooFar = 2;
        public const int apsDupWhat = 3;
        public const int apsDupDayWhat = 4;
        public const int apsBusyConflict = 5;

        //CEvent object
        public const int opNone = 0;
        public const int opEqual = 1;
        public const int opGreater = 2;
        public const int opGreaterOrEqual = 3;
        public const int opLess = 4;
        public const int opLessOrEqual = 5;
        public const int opNotEqual = 6;

        //Scheduler object

        public const int tsOk = 0;
        public const int tsWeekDayConflict = 1;
        public const int tsPeriodConflict = 2;
        public const int tsWhoConflict = 3;
        public const int tsDistanceFar = 4;
        public const int tsWhomConflict = 5;
        public const int tsDupWhat = 6;
        public const int tsWhereConflict = 7;
        public const int tsCannotFit = 8;
        public const int tsLongBreak = 9;
        public const int tsLocConflict = 10;
        public const int tsDupDayWhat = 11;
        public const int tsGroupConflict = 12;

        //-- Resource string IDs
        // - frmMain
        public const int rsLoadEventConflict = 2000;
        public const int rsWhereBusyConflict = 2001;
        public const int rsWhoBusyConflict = 2002;
        public const int rsWhereBusyInvalid = 2003;
        public const int rsDBFilePrompt = 2004;
        public const int rsConnectFail = 2005;

        // - frmEventsView
        public const int rsMasterEventList = 2200;
        public const int rsEventlist = 2201;
        public const int rsEventID = 2202;
        public const int rsWho = 2203;
        public const int rsWhom = 2204;
        public const int rsWhat = 2205;
        public const int rsWhere = 2206;
        public const int rsWeekDay = 2207;
        public const int rsPeriod = 2208;
        public const int rsLength = 2209;
        public const int rsWeekDayCondition = 2210;
        public const int rsPeriodCondition = 2211;
        public const int rsLongBreak = 2212;
        public const int rsAllowDup = 2213;
        public const int rsWeekFlag = 2214;
        public const int rsPriority = 2215;
        public const int rsSolutionCount = 2227;
        public const int rsYes = 2216;
        public const int rsNo = 2217;
        public const int rsOdd = 2218;
        public const int rsEven = 2219;
        public const int rsBoth = 2220;
        public const int rsErr = 2221;
        public const int rsAutoschedulePrompt = 2222;
        public const int rsNoSolutionFound = 2223;
        public const int rsCustomEventList = 2224;
        public const int rsExchangeEvent = 2225;
        public const int rsClearXchgEvent = 2226;

        // - frmLPView
        public const int rsLocalPeriod = 2400;
        public const int rsSchedulable = 2401;
        public const int rsMon = 2402;
        public const int rsTue = 2403;
        public const int rsWed = 2404;
        public const int rsThu = 2405;
        public const int rsFri = 2406;
        public const int rsSat = 2407;
        public const int rsSun = 2408;
        public const int rsWeekDayConflict = 2409;
        public const int rsPeriodConflict = 2410;
        public const int rsWhoConflict = 2411;
        public const int rsDistanceFar = 2412;
        public const int rsWhomConflict = 2413;
        public const int rsDupWhat = 2414;
        public const int rsWhereConflict = 2415;
        public const int rsCannotFit = 2416;
        public const int rsLongBreakConflict = 2417;
        public const int rsLocConflict = 2418;

        // - frmWhoList & frmWhomList
        public const int rsID = 2600;
        public const int rsWhoName = 2601;
        public const int rsWhomName = 2602;
        public const int rsCapacity = 2603;
        public const int rsTimetable = 2604;
        public const int rsSemesters = 1057;

        //-- Others
        public const int rsWhoNull = 2900;
        public const int rsWhomNull = 2901;
        public const int rsWhereNull = 2902;
        public const int rsWhatNull = 2906;
        public const int rsTimeTableNull = 2903;
        public const int rsTotalHour = 2904;
        public const int rsAllocHour = 2905;

        // - frmEventsView Form
        public const int evAll = 0;
        public const int evWho = 1;
        public const int evWhom = 2;
        public const int evWhere = 3;
        public const int evWhat = 4;
        public const int evCustom = 5;

        // - frmLPView Form
        public const int lvWho = 1;
        public const int lvWhom = 2;
        public const int lvWhere = 3;

        // - Null Values
        public const int NullValue = -1;
        public const string NullString = "";
    }
}