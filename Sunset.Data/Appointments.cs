using System;
using System.Collections.Generic;
using System.Linq;

namespace Sunset.Data
{
    /// <summary>
    /// 約會集合，相當於週行事曆
    /// </summary>
    public class Appointments : IEnumerable<Appointment>
    {
        private List<Appointment> mAppointments;

        /// <summary>
        /// 建構式
        /// </summary>
        public Appointments()
        {
            //初始化約會集合
            mAppointments = new List<Appointment>();
        }

        /// <summary>
        /// 根據索引值取得約會
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>約會物件</returns>
        public Appointment this[int index]
        {
            get
            {
                return (index>=0) && (index<mAppointments.Count)?mAppointments[index]:null;
            }
        }

        /// <summary>
        /// 約會集合數量
        /// </summary>
        public int Count
        {
            get { return mAppointments.Count; }
        }

        /// <summary>
        /// 加入約會，依照時間由小到大加入
        /// </summary>
        /// <param name="NewAppointment">約會</param>
        /// <returns>約會物件</returns>
        public Appointment Add(Appointment NewAppointment)
        {
            int nPos = this.GetSortOrder(NewAppointment);

            if (nPos >= mAppointments.Count)
                mAppointments.Add(NewAppointment);
            else
                mAppointments.Insert(nPos, NewAppointment);

            return NewAppointment;
        }

        /// <summary>
        /// 依照集合中的順序（Index）移除約會 
        /// </summary>
        /// <param name="Index">索引</param>
        public void Remove(Appointment App)
        {
            mAppointments.Remove(App);
        }

        /// <summary>
        /// 依照事件（CEvent）編號移除約會物件
        /// </summary>
        /// <param name="EventID">分課表編號</param>
        public void RemoveByID(string EventID)
        {
            List<Appointment> RemoveAppointments = mAppointments.FindAll(x => x.EventID.Equals(EventID));

            mAppointments
                .FindAll              
                (x => x.EventID.Equals(EventID))
                .ForEach(x=>mAppointments.Remove(x));
        }

        /// <summary>
        /// 根據條件尋找在約會集合中符合的約會，主要邏輯為判斷兩個約會之間是否有交集
        /// </summary>
        /// <param name="TestWeekDay">星期幾</param>
        /// <param name="TestTime">時間（幾點幾分），假設日期是一樣的</param>
        /// <param name="TestDuration">分鐘</param>
        /// <param name="TestWeekFlag">行事曆編號</param>
        /// <returns>約會集合</returns>
        public Appointments GetAppointments(int TestWeekDay, DateTime TestTime, int TestDuration, Byte TestWeekFlag)
        {
            return mAppointments
                .FindAll(x => x.IntersectsWith(TestWeekDay, TestTime, TestDuration, TestWeekFlag))
                .ToAppointments();

            #region VB
            //Public Function GetAppointments(ByVal TestWeekDay As Integer, ByVal TestTime As Date, ByVal TestDuration As Integer, ByVal TestWeekFlag As Byte) As Appointments
            //    Dim nTimeDif As Integer
            //    Dim apMember As Appointment
            //    Dim apsResult As Appointments

            //    Set apsResult = New Appointments

            //    For Each apMember In mCol
            //        With apMember
            //        If .WeekDay > TestWeekDay Then Exit For
            //        If (TestWeekDay = .WeekDay) And ((TestWeekFlag And .WeekFlag) > 0) Then
            //            nTimeDif = CInt((.BeginTime - TestTime) / CDate("0:1"))
            //            If nTimeDif >= TestDuration Then Exit For
            //            If nTimeDif >= 0 Then
            //                apsResult.Add apMember
            //            ElseIf -nTimeDif < .Duration Then
            //                apsResult.Add apMember
            //            End If
            //        End If
            //        End With
            //    Next    
            //    Set GetAppointments = apsResult
            //End Function
            #endregion
        }

        public Appointments GetAppointments(int TestWeekDay, DateTime TestTime, int TestDuration)
        {
            return mAppointments
                .FindAll(x => x.IntersectsWith(TestWeekDay, TestTime, TestDuration))
                .ToAppointments(); 
        }

        public Appointment GetAppointment(int TestWeekDay, DateTime TestTime, int TestDuration, Byte TestWeekFlag)
        {
            Appointments Appointments = GetAppointments(TestWeekDay, TestTime, TestDuration, TestWeekFlag);

            return Appointments.Count > 0 ? Appointments[0] : null;

            #region VB
            //Public Function GetAppointment(ByVal TestWeekDay As Integer, ByVal TestTime As Date, ByVal TestDuration As Integer, ByVal TestWeekFlag As Byte) As Appointment
            //    Dim apsResult As Appointments


            //    Set apsResult = GetAppointments(TestWeekDay, TestTime, TestDuration, TestWeekFlag)

            //    If apsResult.Count > 0 Then
            //        Set GetAppointment = apsResult(1)
            //    Else
            //        Set GetAppointment = Nothing
            //    End If
            //End Function
            #endregion
        }

        public bool IsFreeTime(int TestWeekDay, DateTime TestTime, int TestDuration, Byte TestWeekFlag)
        {
            return mAppointments
                .TrueForAll(x => !x.IntersectsWith(TestWeekDay, TestTime, TestDuration, TestWeekFlag));

            #region VB
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
            #endregion
        }

        public bool IsFreePeriod(Period TestPeriod, Byte TestWeekFlag)
        {
            return IsFreeTime(TestPeriod.WeekDay, TestPeriod.BeginTime, TestPeriod.Duration, TestWeekFlag);

            #region VB
            //Public Function IsFreePeriod(ByVal TestPeriod As Period, ByVal TestWeekFlag As Byte) As Boolean
            //    With TestPeriod
            //        IsFreePeriod = IsFreeTime(.WeekDay, .BeginTime, .Duration, TestWeekFlag)
            //    End With
            //End Function
            #endregion
        }

        public bool IsFreePeriods(Periods TestPeriods,Byte TestWeekFlag)
        {
            bool Result = true;

            mAppointments.ForEach
            (
                x => 
                {
                     bool TestFree = TestPeriods.ToList().TrueForAll(y => !x.IntersectsWith(y.WeekDay, y.BeginTime, y.Duration, TestWeekFlag));

                     if (!TestFree) Result = false;
                }                           
            );            

            return Result;

            #region VB
            //'NOTICE: IsFreePeriods assumes that TestPrds is sorted by time, and of the same location and same weekday
            //Public Function IsFreePeriods(TestPrds As Periods, ByVal TestWeekFlag As Byte) As Boolean
            //    Dim TestWeekDay As Integer
            //    Dim dblMinUnit As Double
            //    Dim nPrdsIndex As Integer
            //    Dim nPrdsCount As Integer
            //    Dim nTimeDif As Integer
            //    Dim apMember As Appointment
            //    Dim prdTest As Period

            //    dblMinUnit = CDate("0:1")

            //    IsFreePeriods = True

            //    nPrdsCount = TestPrds.Count
            //    ' Move to first period
            //    nPrdsIndex = 1
            //    Set prdTest = TestPrds(1)
            //    ' Initialize variables for check loop
            //    TestWeekDay = prdTest.WeekDay

            //    For Each apMember In mCol
            //        With apMember
            //        If .WeekDay > TestWeekDay Then Exit For
            //        If (TestWeekDay = .WeekDay) And ((TestWeekFlag And .WeekFlag) > 0) Then
            //            nTimeDif = CInt((.BeginTime - prdTest.BeginTime) / dblMinUnit)
            //            If nTimeDif >= prdTest.Duration Then
            //                ' The test period is wholely above current appointment
            //                ' Move to next period that is not wholely above the current appointment
            //                nPrdsIndex = nPrdsIndex + 1
            //                Do While nPrdsIndex <= nPrdsCount
            //                    Set prdTest = TestPrds(nPrdsIndex)
            //                    nTimeDif = CInt((.BeginTime - prdTest.BeginTime) / dblMinUnit)
            //                    If nTimeDif < prdTest.Duration Then Exit Do
            //                    nPrdsIndex = nPrdsIndex + 1
            //                Loop
            //                If nPrdsIndex > nPrdsCount Then Exit Function
            //            End If
            //            If Not (prdTest Is Nothing) Then
            //                If nTimeDif >= 0 Then
            //                    'Conflict! test period starts earlier than current appointment
            //                    IsFreePeriods = False
            //                    Exit For
            //                ElseIf -nTimeDif < .Duration Then
            //                    'Conflict! current appointment starts earlier than test period
            //                    IsFreePeriods = False
            //                    Exit For
            //                End If
            //            End If
            //            ' The test period is wholely below current appointment
            //        End If
            //        End With
            //    Next
            //End Function
            #endregion
        }

        /// <summary>
        /// NOTICE: CheckWho assumes that TestPrds is sorted by time, and of the same location and same weekday
        /// CheckWho is specifically designed for Who object to increase check performance, it checks a group
        /// of time period instead of one.
        /// CheckWho will check distance and time conflict at the same time.
        /// Return Value:
        ///   0 - No Conflict
        ///   1 - Time Conflict
        ///   2 - Distance Conflict
        /// </summary>
        /// <param name="TestPeriods"></param>
        /// <param name="TestDists"></param>
        /// <param name="TestWeekFlag"></param>
        /// <returns></returns>
        public int CheckWho(Periods TestPeriods, Distances TestDists, Byte TestWeekFlag)
        {
            int TestWeekDay = TestPeriods[0].WeekDay; //假設TestPeriods的WeekDay都一樣
            int nPrdsIndex = 0;
            int nPrdsCount = TestPeriods.Count;
            int nTimeDif;
            DateTime datLastEndTime = new DateTime();
            string nLastLoc = string.Empty; //Keep track of current LocID
            int nLastPos = 0;  //0-None, 1-Appointments, 2-Test Periods
            Period prdTest = TestPeriods[0];

            #region VB
            //    Dim TestWeekDay As Integer
            //    Dim dblMinUnit As Double
            //    Dim nPrdsIndex As Integer
            //    Dim nPrdsCount As Integer
            //    Dim nTimeDif As Integer
            //    Dim apMember As Appointment
            //    Dim datLastEndTime As Date
            //    Dim nLastLoc As Long   'Keep track of current LocID
            //    Dim nLastPos As Integer '0-None, 1-Appointments, 2-Test Periods
            //    Dim prdTest As Period
            //    Dim TimeA As Date
            //    Dim TimeB As Date
            //    dblMinUnit = CDate("0:1")
            //    CheckWho = apsNoConflict
            //    nPrdsCount = TestPrds.Count
            
            //    ' Move to first period
            //    nPrdsIndex = 1
            //    Set prdTest = TestPrds(1)
            
            //    ' Initialize variables for check loop
            //    TestWeekDay = prdTest.WeekDay
            //    nLastPos = 0
            #endregion

            //注意：apMember由小到大依星期及時間排序。
            //針對每個apMember，用每個Period來測試是否有衝突。
            foreach (Appointment apMember in mAppointments)
            {
                //mAppointments會依照星期幾、小時及分鐘由小到大排序，假設第一個WeekDay就大於TestWeekDay，代表後面的WeekDay都大於TestWeekDay
                //若apMember.WeekDay大於TestWeekDay會傳回No Conflict
                if (apMember.WeekDay > TestWeekDay)
                    break;

                //If (TestWeekDay = .WeekDay) And ((TestWeekFlag And .WeekFlag) > 0) Then
                //假設TestWeekDay等於WeekDay，以及TestWeekFlag等於WeekFlag
                if ((TestWeekDay == apMember.WeekDay) && ((TestWeekFlag & apMember.WeekFlag)>0))
                {
                    //將apMember.BeginTime減去BeginTime
                    nTimeDif = apMember.BeginTime.TimeDiff(prdTest.BeginTime);

                    #region The test period is wholely above current appointment
                    //當apMember.BeginTime大於prdTest.BeginTime+prdTest.Duration
                    //假設apMember.BeginTime不大於prdTest.BeginTime，代表後面的prdTest都不大於apMember
                    if (nTimeDif >= prdTest.Duration)
                    {
                        //Check distance
                        //初始化nLastPos為0所以不會執行到，在最下面會將nLastPos設為1
                        if ((nLastPos == 1) && (prdTest.LocID != nLastLoc))
                            if (TestDists.GetDistance(nLastLoc, prdTest.LocID) > prdTest.BeginTime.TimeDiff(datLastEndTime))
                                return Constants.apsTooFar;

                        //datLastEndTime為BeginTime加上prdTest.Duration
                        datLastEndTime = prdTest.BeginTime.AddMinutes(prdTest.Duration);
                        //nLastLoc為prdTest.LocID
                        nLastLoc = prdTest.LocID;
                        nPrdsIndex++;

                        //循訪TestPeriods，判斷prdTest是否有與apMember.BeginTime有交集
                        while (nPrdsIndex < nPrdsCount)
                        {
                            prdTest = TestPeriods[nPrdsIndex];

                            nTimeDif = apMember.BeginTime.TimeDiff(prdTest.BeginTime);

                            if (nTimeDif < prdTest.Duration)
                                break;

                            //Save last information and move to next period
                            datLastEndTime = prdTest.BeginTime.AddMinutes(prdTest.Duration);
                            nLastLoc = prdTest.LocID;
                            nPrdsIndex++;
                        }

                        //若nPrdsIndex大於等於nPrdsCount代表apMember.Begin的時間都大於prdTest的時間，代表No Conflict
                        if (nPrdsIndex >= nPrdsCount) prdTest = null;

                        nLastPos = 2;
                    }
                    #endregion

                    #region Conflict! test period starts earlier than current appointment
                    //若是prdTest不為null
                    if (prdTest != null)
                    {                        
                        if (nTimeDif >= 0)
                            return Constants.apsTimeConflict;
                        else if (-nTimeDif < apMember.Duration)
                            return Constants.apsTimeConflict;
                    }
                    #endregion

                    if ((nLastPos == 2) && (nLastLoc != apMember.LocID))
                        if (TestDists.GetDistance(nLastLoc, apMember.LocID) > apMember.BeginTime.TimeDiff(datLastEndTime))
                            return Constants.apsTooFar;

                    //No more periods to test
                    //若prdTest為null代表apMember.BeginTime都大於Periods，因為apMember由小到大排序，所以其他的apMember都會大於Periods
                    if (prdTest == null)
                        return Constants.apsNoConflict;

                    //跑每個apMember最後會將nLastPos設為1
                    nLastPos = 1;
                    datLastEndTime = apMember.BeginTime.AddMinutes(apMember.Duration);
                    nLastLoc = apMember.LocID;                    
                }
            }

            if ((prdTest!=null) && (nLastPos==1))
                if (prdTest.LocID != nLastLoc)
                    if (TestDists.GetDistance(nLastLoc,prdTest.LocID)> prdTest.BeginTime.TimeDiff(datLastEndTime))
                        return Constants.apsTooFar;    

            return Constants.apsNoConflict;

            #region VB
            //'NOTICE: CheckWho assumes that TestPrds is sorted by time, and of the same location and same weekday
            //'CheckWho is specifically designed for Who object to increase check performance, it checks a group
            //'of time period instead of one.
            //'CheckWho will check distance and time conflict at the same time.
            //'Return Value:
            //'   0 - No Conflict
            //'   1 - Time Conflict
            //'   2 - Distance Conflict

            //Public Function CheckWho(TestPrds As Periods, TestDists As Distances, ByVal TestWeekFlag As Byte) As Integer
            //    Dim TestWeekDay As Integer
            //    Dim dblMinUnit As Double
            //    Dim nPrdsIndex As Integer
            //    Dim nPrdsCount As Integer
            //    Dim nTimeDif As Integer
            //    Dim apMember As Appointment
            //    Dim datLastEndTime As Date
            //    Dim nLastLoc As Long   'Keep track of current LocID
            //    Dim nLastPos As Integer '0-None, 1-Appointments, 2-Test Periods
            //    Dim prdTest As Period
            //    Dim TimeA As Date
            //    Dim TimeB As Date

            //    dblMinUnit = CDate("0:1")

            //    CheckWho = apsNoConflict

            //    nPrdsCount = TestPrds.Count
            //    ' Move to first period
            //    nPrdsIndex = 1
            //    Set prdTest = TestPrds(1)
            //    ' Initialize variables for check loop
            //    TestWeekDay = prdTest.WeekDay
            //    nLastPos = 0

            //    For Each apMember In mCol
            //        With apMember
            //        If .WeekDay > TestWeekDay Then Exit For
            //        If (TestWeekDay = .WeekDay) And ((TestWeekFlag And .WeekFlag) > 0) Then
            //            TimeA = Hour(.BeginTime) & ":" & Minute(.BeginTime) & ":" & Second(.BeginTime)
            //            TimeB = Hour(prdTest.BeginTime) & ":" & Minute(prdTest.BeginTime) & ":" & Second(prdTest.BeginTime)
            //            nTimeDif = CInt((.BeginTime - prdTest.BeginTime) / dblMinUnit)
            //'            nTimeDif = CInt((TimeA - TimeB) / dblMinUnit)
            //            If nTimeDif >= prdTest.Duration Then
            //                ' The test period is wholely above current appointment
            //                If (nLastPos = 1) And (prdTest.LocID <> nLastLoc) Then
            //                    'Check distance
            //                    If TestDists.GetDistance(nLastLoc, prdTest.LocID) > CInt((prdTest.BeginTime - datLastEndTime) / dblMinUnit) Then
            //                        CheckWho = apsTooFar
            //                        Exit Function
            //                    End If
            //                End If
            //                ' Move to next period that is not wholely above the current appointment
            //                datLastEndTime = prdTest.BeginTime + (dblMinUnit * prdTest.Duration)
            //                nLastLoc = prdTest.LocID
            //                nPrdsIndex = nPrdsIndex + 1
            //                Do While nPrdsIndex <= nPrdsCount
            //                    Set prdTest = TestPrds(nPrdsIndex)
            //                    nTimeDif = CInt((.BeginTime - prdTest.BeginTime) / dblMinUnit)
            //'                    nTimeDif = CInt((TimeA - TimeB) / dblMinUnit)

            //                    If nTimeDif < prdTest.Duration Then Exit Do
            //                    ' Save last information and move to next period
            //                    datLastEndTime = prdTest.BeginTime + (dblMinUnit * prdTest.Duration)
            //                    nLastLoc = prdTest.LocID
            //                    nPrdsIndex = nPrdsIndex + 1
            //                Loop
            //                If nPrdsIndex > nPrdsCount Then Set prdTest = Nothing
            //                nLastPos = 2
            //            End If
            //            If Not (prdTest Is Nothing) Then
            //                If nTimeDif >= 0 Then
            //                    'Conflict! test period starts earlier than current appointment
            //                    CheckWho = apsTimeConflict
            //                    Exit For
            //                ElseIf -nTimeDif < .Duration Then
            //                    'Conflict! current appointment starts earlier than test period
            //                    CheckWho = apsTimeConflict
            //                    Exit For
            //                End If
            //            End If
            //            ' The test period is wholely below current appointment or
            //            ' there is no more test period that is wholely below current appointment
            //            If (nLastPos = 2) And (nLastLoc <> .LocID) Then
            //                'Check distance
            //                If TestDists.GetDistance(nLastLoc, .LocID) > CInt((.BeginTime - datLastEndTime) / dblMinUnit) Then
            //                    CheckWho = apsTooFar
            //                    Exit Function
            //                End If
            //            End If
            //            If prdTest Is Nothing Then
            //                'No more periods to test
            //                Exit Function
            //            End If
            //            nLastPos = 1
            //            datLastEndTime = .BeginTime + (dblMinUnit * .Duration)
            //            nLastLoc = .LocID
            //        End If
            //        End With
            //    Next

            //    If Not (prdTest Is Nothing) And (nLastPos = 1) Then
            //        If prdTest.LocID <> nLastLoc Then
            //            'Check distance
            //            If TestDists.GetDistance(nLastLoc, prdTest.LocID) > CInt((prdTest.BeginTime - datLastEndTime) / dblMinUnit) Then
            //                CheckWho = apsTooFar
            //            End If
            //        End If
            //    End If
            //End Function
            #endregion
        }


        /// <summary>
        /// NOTICE: CheckWhom assumes that TestPrds is sorted by time, and of the same location and same weekday
        /// CheckWhom is specifically designed for Whom object to increase check performance, it checks a group
        /// of time period instead of one.
        /// CheckWhom will check whatid duplication and time conflict at the same time.
        /// </summary>
        /// <param name="TestPrds">測試時段</param>
        /// <param name="WhatID">科目編號</param>
        /// <param name="TestWeekFlag">單雙週</param>
        /// <returns>
        /// Return Value:
        /// 0 - No Conflict
        /// 1 - Time Conflict
        /// 3 - WhatID Duplicate
        /// </returns>
        public int CheckWhom(Periods TestPrds,string WhatID,byte TestWeekFlag,string EventID,bool AllowDuplicate,bool LimitNextDay)
        {
            int TestWeekDay = TestPrds[0].WeekDay;
            int nTimeDif;
            int nPrdsIndex = 0;
            int nPrdsCount = TestPrds.Count;
            Period prdTest = TestPrds[0];
            List<int> WhatIDWeekdays = new List<int>();

            #region 針對班級上所有約會（mAppointments），依照WeekDay由小到大排序
            foreach (Appointment apMember in mAppointments)
            {
                #region 若設定連天不排課，則將相同科目的星期加入到清單中
                //if (LimitNextDay)
                //{
                //    if (apMember.WhatID.Equals(WhatID) && !apMember.EventID.Equals(EventID))
                //        if (!WhatIDWeekdays.Contains(apMember.WeekDay))
                //            WhatIDWeekdays.Add(apMember.WeekDay);
                //}
                #endregion

                //若第一個Appointment的WeekDay大於TestWeekDay，後面Appointment的WeekDay也一定大於TestWeekDay
                //就一定是No Conflict
                if (apMember.WeekDay > TestWeekDay)
                    break;

                #region 若測試星期與約會日期相同，且單雙週狀況一致
                if ((TestWeekDay == apMember.WeekDay) && ((TestWeekFlag & apMember.WeekFlag)>0))
                {
                    if (prdTest != null)
                    {
                        nTimeDif = apMember.BeginTime.TimeDiff(prdTest.BeginTime);

                        #region The test period is wholely above current appointment
                        //Move to next period that is not wholely above the current appointment
                        //舉例：Period為9：10~10：00，而Appointment為10：10~11：00
                        if (nTimeDif >= prdTest.Duration)
                        {                          
                            nPrdsIndex++;

                            //針對每個Period檢查看時間是否與apMember有交集
                            while (nPrdsIndex < nPrdsCount)
                            {
                                prdTest = TestPrds[nPrdsIndex];

                                nTimeDif = apMember.BeginTime.TimeDiff(prdTest.BeginTime);

                                if (nTimeDif < prdTest.Duration) break;

                                //Save last information and move to next period
                                nPrdsIndex++;
                            }

                            //Period與Appointment完全沒有交集，將prdTest設為null
                            if (nPrdsIndex >= nPrdsCount) prdTest = null;
                        }
                        #endregion

                        if (prdTest != null)
                            if (nTimeDif >= 0)
                                return Constants.apsTimeConflict; //Appointment的開始時間大於Period的開始時間，兩者有交集，例如Appointment為9：00~10：00，而Period為8：30~9：30
                            else if (-nTimeDif < apMember.Duration)
                                return Constants.apsTimeConflict; //Appointment的開始時間小於Period的開始時間，兩者有交集，例如Appointment為8：00~9：00，而Period為8：30~9：30
                    }

                    //Check duplicate WhatID
                    if (!AllowDuplicate && !apMember.EventID.Equals(EventID))
                        if (apMember.WhatID == WhatID)
                            return Constants.apsDupWhat; 
                }
                #endregion
            }
            #endregion

            //根據測試星期判斷是否有連天排課的情況
            //if (LimitNextDay)
            //   if (WhatIDWeekdays.Contains(TestWeekDay - 1) || WhatIDWeekdays.Contains(TestWeekDay + 1))
            //       return Constants.apsDupDayWhat;

            //檢查是否有連天排課情況

            return Constants.apsNoConflict;

            #region VB
            //'NOTICE: CheckWhom assumes that TestPrds is sorted by time, and of the same location and same weekday
            //'CheckWhom is specifically designed for Whom object to increase check performance, it checks a group
            //'of time period instead of one.
            //'CheckWhom will check whatid duplication and time conflict at the same time.
            //'Return Value:
            //'   0 - No Conflict
            //'   1 - Time Conflict
            //'   3 - WhatID Duplicate

            //Public Function CheckWhom(TestPrds As Periods, ByVal WhatID As Long, ByVal TestWeekFlag As Byte) As Integer
            //    Dim TestWeekDay As Integer
            //    Dim dblMinUnit As Double
            //    Dim nTimeDif As Integer
            //    Dim apMember As Appointment
            //    Dim nPrdsIndex As Integer
            //    Dim nPrdsCount As Integer
            //    Dim prdTest As Period

            //    dblMinUnit = CDate("0:1")

            //    CheckWhom = apsNoConflict

            //    nPrdsCount = TestPrds.Count
            //    ' Move to first period
            //    nPrdsIndex = 1
            //    Set prdTest = TestPrds(1)
            //    ' Initialize variables for check loop
            //    TestWeekDay = prdTest.WeekDay

            //    For Each apMember In mCol
            //        With apMember
            //        If .WeekDay > TestWeekDay Then Exit For
            //        If (TestWeekDay = .WeekDay) And ((TestWeekFlag And .WeekFlag) > 0) Then
            //            If Not (prdTest Is Nothing) Then
            //                nTimeDif = CInt((.BeginTime - prdTest.BeginTime) / dblMinUnit)
            //                If nTimeDif >= prdTest.Duration Then
            //                    ' The test period is wholely above current appointment
            //                    ' Move to next period that is not wholely above the current appointment
            //                    nPrdsIndex = nPrdsIndex + 1
            //                    Do While nPrdsIndex <= nPrdsCount
            //                        Set prdTest = TestPrds(nPrdsIndex)
            //                        nTimeDif = CInt((.BeginTime - prdTest.BeginTime) / dblMinUnit)
            //                        If nTimeDif < prdTest.Duration Then Exit Do
            //                        ' Save last information and move to next period
            //                        nPrdsIndex = nPrdsIndex + 1
            //                    Loop
            //                    If nPrdsIndex > nPrdsCount Then Set prdTest = Nothing
            //                End If
            //                If Not (prdTest Is Nothing) Then
            //                    If nTimeDif >= 0 Then
            //                        'Conflict! test period starts earlier than current appointment
            //                        CheckWhom = apsTimeConflict
            //                        Exit For
            //                    ElseIf -nTimeDif < .Duration Then
            //                        'Conflict! current appointment starts earlier than test period
            //                        CheckWhom = apsTimeConflict
            //                        Exit For
            //                    End If
            //                End If
            //            End If

            //            'Check duplicate WhatID
            //            If .WhatID = WhatID Then
            //                CheckWhom = apsDupWhat
            //                Exit Function
            //            End If
            //        End If
            //        End With
            //    Next
            //End Function
            #endregion
        }

        #region IEnumerable<Appointment> Members

        public IEnumerator<Appointment> GetEnumerator()
        {
            return mAppointments.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return mAppointments.GetEnumerator();
        }

        #endregion
    }
}