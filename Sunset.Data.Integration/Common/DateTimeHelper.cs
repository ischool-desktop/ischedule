using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunset.Data.Integration
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// 標準日期時間格式，例：2007/05/10 14:30:05
        /// </summary>
        public const string StdDateTimeFormat = "yyyy/MM/dd HH:mm:ss";
        /// <summary>
        /// 標準日期格式，例：2007/05/10
        /// </summary>
        public const string StdDateFormat = "yyyy/MM/dd";
        /// <summary>
        /// 標準時間格式，例：14:30:05
        /// </summary>
        public const string StdTimeFormat = "HH:mm:ss";

        public static string ToDisplayString(DateTime? dt)
        {
            if (dt.HasValue)
                return dt.Value.ToShortDateString();
            else
                return "";
        }

        public static string ToDisplayString(DateTime dt)
        {
            return dt.ToString(StdDateFormat);
        }

        public static string ToDisplayString(string dateTimeString)
        {
            DateTime dt;

            if (DateTime.TryParse(dateTimeString, out dt))
                return dt.ToString(StdDateFormat);
            else
                return string.Empty;
        }

        /// <summary>
        /// 使用內鍵的 DateTime 的 TryParse 處理。
        /// </summary>
        /// <param name="dtString">日期字串。</param>
        /// <returns></returns>
        public static DateTime? Parse(string dtString)
        {
            DateTime dt;

            //if (DateTime.TryParse(dtString, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out dt))

            if (DateTime.TryParse(dtString, out dt))
                return dt;
            else
                return null;
        }

        /// <summary>
        /// 將日期轉換為統一年、月、日及秒基準，並將來源小時及分鐘複製至原參數。
        /// </summary>
        /// <param name="DateTime"></param>
        /// <returns></returns>
        public static DateTime ToHourMinute(this DateTime DateTime)
        {
            DateTime NewDateTime = new DateTime(1900, 1, 1, DateTime.Hour, DateTime.Minute, 0);

            return NewDateTime;
        }

        /// <summary>
        /// 運用TestWeekDay、TestTime、TestDuration及TestWeekFlag與Appointment間是否有交集。
        /// </summary>
        /// <param name="Appointment"></param>
        /// <param name="TestWeekDay"></param>
        /// <param name="TestTime"></param>
        /// <param name="TestDuration"></param>
        /// <param name="TestWeekFlag"></param>
        /// <returns>若有交集傳回true，若沒有交集傳回false。</returns>
        public static bool IntersectsWith(int WeekDay, DateTime BeginTime, int Duration, Byte WeekFlag, int TestWeekDay, DateTime TestTime, int TestDuration, Byte TestWeekFlag)
        {
            //將TestTime的年、月、日及秒設為與Appointment一致，以確保只是單純針對小時及分來做時間差的運算
            DateTime NewBeginTime = BeginTime.ToHourMinute();

            DateTime NewTestTime = TestTime.ToHourMinute();

            //判斷星期幾及WeekFlag是否相同
            if ((WeekDay == TestWeekDay) && ((WeekFlag & TestWeekFlag) > 0))
            {
                //將Appointment的NewBeginTime減去NewTestTime
                int nTimeDif = (int)NewBeginTime.Subtract(NewTestTime).TotalMinutes;

                //狀況一：假設nTimeDif為正，並且大於NewTestTime，代表兩者沒有交集，傳回false。
                //舉例：
                //Appointment.BeginTime為10：00，其Duration為40。
                //TestTime為9：00，其Duration為50。
                if (nTimeDif >= TestDuration)
                    return false;

                //狀況二：假設nTimeDiff為正，並且小於TestDuration，代表兩者有交集，傳回true。
                //舉例：
                //Appointment.BeginTime為10：00，其Duration為40。
                //TestTime為9：00，其Duration為80。
                if (nTimeDif >= 0)
                    return true;
                //狀況三：假設nTimeDiff為負值，將nTimeDiff成為正值，若是-nTimeDiff小於Appointment.Duration；
                //代表NewBeginTime在NewTestTime之前，並且NewBegin與NewTestTime的絕對差值小於Appointment.Duration的時間
                //舉例：
                //Appointment.BeginTime為10：00，其Duration為40。
                //TestTime為10：30，其Duration為20。
                else if (-nTimeDif < Duration)
                    return true;
            }

            //其他狀況傳回沒有交集
            //舉例：
            //Appointment.BeginTime為10：00，其Duration為40。
            //TestTime為10：50，其Duration為20。
            return false;

            #region VB
            //Public Function IsFreeTime(ByVal TestWeekDay As Integer, ByVal TestTime As Date, ByVal TestDuration As Integer, ByVal TestWeekFlag As Byte) As Boolean
            //    Dim nTimeDif As Integer
            //    Dim apMember As Appointment

            //    IsFreeTime = True

            //    For Each apMember In mCol
            //        With apMember
            //        If .WeekDay > TestWeekDay Then Exit For
            //        If (TestWeekDay = .WeekDay) And ((TestWeekFlag And .WeekFlag) > 0) Then
            //            nTimeDif = CInt((.BeginTime - TestTime) / CDate("0:1"))
            //            ' The test period is wholely above this appointment
            //            If nTimeDif >= TestDuration Then Exit For
            //            If nTimeDif >= 0 Then
            //                'Conflict! test period starts earlier than this appointment
            //                IsFreeTime = False
            //                Exit For
            //            ElseIf -nTimeDif < .Duration Then
            //                'Conflict! this appointment starts earlier than test period
            //                IsFreeTime = False
            //                Exit For
            //            End If
            //        End If
            //        End With
            //    Next
            //End Function
            #endregion
        }
    }
}