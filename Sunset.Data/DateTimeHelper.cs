using System;
using System.Collections.Generic;

namespace Sunset.Data
{
    public static class DateTimeHelper
    {

        public static void Shuffle<T>(this IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary>
        /// 取得一週開始
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="startOfWeek"></param>
        /// <returns></returns>
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-1 * diff).Date;
        }

        /// <summary>
        /// 將日期轉換為統一年、月、日及秒基準，並將來源小時及分鐘複製至原參數。
        /// </summary>
        /// <param name="DateTime"></param>
        /// <returns></returns>
        public static DateTime ToHourMinute(this DateTime DateTime)
        {
            DateTime NewDateTime = new DateTime(1900, 1, 1, DateTime.Hour, DateTime.Minute,0);

            return NewDateTime;
        }

        /// <summary>
        /// 將兩個時間相減，最後會傳回分鐘差距。
        /// </summary>
        /// <param name="DateTime"></param>
        /// <param name="TestDateTime"></param>
        /// <returns>分鐘數，若DateTime大於TestDate，則回傳正值；若DateTime小於TestDateTime，則回傳負值。</returns>
        public static int TimeDiff(this DateTime DateTime,DateTime TestDateTime)
        {
            DateTime NewDateTime = DateTime.ToHourMinute();
            DateTime NewTestDateTime = TestDateTime.ToHourMinute();

            return (int)NewDateTime.Subtract(NewTestDateTime).TotalMinutes;
        }
        
        /// <summary>
        /// 判斷DateTime是否在TestDateTime之前
        /// </summary>
        /// <param name="DateTime"></param>
        /// <param name="TestDateTime"></param>
        /// <returns>若是DateTime在TestDateTime之前則傳回true，否則傳回false。</returns>
        public static bool Before(this DateTime DateTime,DateTime TestDateTime)
        {
            DateTime NewDateTime = DateTime.ToHourMinute();
            DateTime NewTestDateTime = TestDateTime.ToHourMinute();

            return (NewDateTime.Ticks - NewTestDateTime.Ticks)<0;
        }

        /// <summary>
        /// 判斷DateTime是否在TestDateTime之後
        /// </summary>
        /// <param name="DateTime"></param>
        /// <param name="TestDateTime"></param>
        /// <returns>若是DateTime在TestDateTime之後則傳回true，否則傳回false。</returns>
        public static bool After(this DateTime DateTime, DateTime TestDateTime)
        {
            DateTime NewDateTime = DateTime.ToHourMinute();
            DateTime NewTestDateTime = TestDateTime.ToHourMinute();

            return (NewDateTime.Ticks - NewTestDateTime.Ticks) > 0;
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
        public static bool IntersectsWith(this Appointment Appointment, int TestWeekDay, DateTime TestTime, int TestDuration)
        {
            //將TestTime的年、月、日及秒設為與Appointment一致，以確保只是單純針對小時及分來做時間差的運算
            DateTime NewBeginTime = Appointment.BeginTime.ToHourMinute();

            DateTime NewTestTime = TestTime.ToHourMinute();

            //判斷星期幾及WeekFlag是否相同
            if (Appointment.WeekDay == TestWeekDay)
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
                else if (-nTimeDif < Appointment.Duration)
                    return true;
            }

            //其他狀況傳回沒有交集
            //舉例：
            //Appointment.BeginTime為10：00，其Duration為40。
            //TestTime為10：50，其Duration為20。
            return false;
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
        public static bool IntersectsWith(this Appointment Appointment,int TestWeekDay, DateTime TestTime, int TestDuration, Byte TestWeekFlag)
        {
            //將TestTime的年、月、日及秒設為與Appointment一致，以確保只是單純針對小時及分來做時間差的運算
            DateTime NewBeginTime = Appointment.BeginTime.ToHourMinute();

            DateTime NewTestTime = TestTime.ToHourMinute();            

            //判斷星期幾及WeekFlag是否相同
            if ((Appointment.WeekDay == TestWeekDay) && ((Appointment.WeekFlag & TestWeekFlag)>0))
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
                else if (-nTimeDif < Appointment.Duration)
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

        /// <summary>
        /// 將Appointment列表轉為Appointments
        /// </summary>
        /// <param name="AppointmentList"></param>
        /// <returns></returns>
        public static Appointments ToAppointments(this List<Appointment> AppointmentList)
        {
            Appointments result = new Appointments();

            AppointmentList.ForEach(x=>result.Add(x));

            return result;
        }

        /// <summary>
        /// 取得代表約會的符號，為星期、小時及分鐘的字串組合，再用數字解析。
        /// </summary>
        /// <param name="NewAppointment"></param>
        /// <returns></returns>
        public static int ToSortSymbol(this Appointment NewAppointment)
        {
            string strWeekDay = "" + NewAppointment.WeekDay;
            string strHour = ("" + NewAppointment.BeginTime.Hour).PadLeft(2,'0');
            string strMinute = ("" + NewAppointment.BeginTime.Minute).PadLeft(2,'0');            

            return int.Parse(strWeekDay + strHour + strMinute);
        }

        /// <summary>
        /// 取得代表時段的符號，為星期、小時及分鐘的字串組合，再用數字解析。
        /// </summary>
        /// <param name="NewPeriod"></param>
        /// <returns></returns>
        public static int ToSortSymbol(this Period NewPeriod)
        {
            string strWeekDay = "" + NewPeriod.WeekDay;
            string strHour = (""+ NewPeriod.BeginTime.Hour).PadLeft(2,'0');
            string strMinute = (""+ NewPeriod.BeginTime.Minute).PadLeft(2,'0');

            return int.Parse(strWeekDay + strHour + strMinute);
        }

        /// <summary>
        /// 取得NewAppointment在vAppointments裡插入的順序
        /// </summary>
        /// <param name="vAppointments"></param>
        /// <param name="NewAppointment"></param>
        /// <returns></returns>
        public static int GetSortOrder(this Appointments vAppointments,Appointment NewAppointment)
        {
            int nPos = 0;
            int NewAppointmentSymbol = NewAppointment.ToSortSymbol();

            while (nPos < vAppointments.Count)
            {
                int AppointmentSymbol = vAppointments[nPos].ToSortSymbol();

                if (NewAppointmentSymbol  < AppointmentSymbol)
                    break;

                nPos++;
            }

            return nPos; 
        }

        /// <summary>
        /// 取得NewPeriod在vPeriods裡插入的順序
        /// </summary>
        /// <param name="vPeriods"></param>
        /// <param name="NewPeriod"></param>
        /// <returns></returns>
        public static int GetSortOrder(this Periods vPeriods, Period NewPeriod)
        {
            int nPos = 0;
            int NewPeriodSymbol = NewPeriod.ToSortSymbol();

            while (nPos < vPeriods.Count)
            {
                int PeriodSymbol = vPeriods[nPos].ToSortSymbol();

                if (NewPeriodSymbol < PeriodSymbol)
                    break;

                nPos++;
            }

            return nPos;
        }
    }
}