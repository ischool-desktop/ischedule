using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using ischedule.Properties;
using Sunset.Data;

namespace ischedule
{
    /// <summary>
    /// 功課表
    /// </summary>
    public partial class usrLPView : UserControl
    {
        private const int lvWho = 1;
        private const int lvWhom = 2;
        private const int lvWhere = 3;

        private Color lvBusyBackColor = Color.FromArgb(230, 230, 230);
        private Color lvBusyForeColor = Color.FromArgb(86, 86, 86);

        //private const string lvBusyBackColor = "#e6e6e6";
        //private const string lvBusyForeColor = "#565656";

        private Color lvFreeBackColor = Color.White;
        private Color lvFreeForeColor = Color.FromArgb(86, 86, 86);

        //private const string lvFreeBackColor = "White";
        //private const string lvFreeForeColor = "#565656";

        private Color lvScheduledBackColor = Color.Snow;
        private Color lvScheduledForeColor = Color.FromArgb(2,123,204);

        //private const string lvScheduledBackColor = "Snow";
        //private const string lvScheduledForeColor = "#027bcc";

        private Color lvTimeTableBackColor = Color.FromArgb(230, 230, 230);
        private Color lvTimeTableForeColor = Color.FromArgb(86, 86, 86);

        //private const string lvTimeTableBackColor = "#e6e6e6";
        //private const string lvTimeTableForeColor = "#565656";

        private Color lvSchedulableBackColor = Color.FromArgb(254, 252, 128);
        private Color lvSchedulableForeColor = Color.White;

        //private const string lvSchedulableBackColor = "#fefc80";
        //private const string lvSchedulableForeColor = "White";

        private Color lvNoSolutionBackColor = Color.FromArgb(230, 230, 230);
        private Color lvNoSolutionForeColor = Color.FromArgb(86,86,86);

        //private const string lvNoSolutionBackColor = "#e6e6e6";
        //private const string lvNoSolutionForeColor = "#565656";

        private const string strSolutionPath = "/Sunset.Presentation;component/images/Sunset/燈泡_亮.png";
        private const string strNoSolutionPath = "/Sunset.Presentation;component/images/Sunset/燈泡_暗.png";
        private const string strDeleteEventPath = "關閉.png";
        private const double mScaleUnit = 0.05;

        //local variables for property
        private int mType;
        private string mObjID;
        private double mScale = 1;
        private string mObjName = string.Empty;

        //local variables
        private Scheduler schLocal = Scheduler.Instance;
        private LPViewOption mOption = new LPViewOption();
        private frmProgress frmASProgress;
        private Dictionary<string, int> colTestEvents = new Dictionary<string, int>(); //Holds a list of test event ids
        private string idTestEvent = string.Empty; //Active test event id
        private List<string> idTestEvents = new List<string>();
        private string idTestCourseID = string.Empty;
        private string idXchgEvent = string.Empty; //Active exchange event id
        private Appointments apsCur;
        private TimeTable ttCur;
        private CEventBindingList Transfers = new CEventBindingList();
        private LPViewLayoutHelper mLPViewHelper;
        private List<int> Periods = new List<int>();
        private List<ucPeriod> SelectedPeriods = new List<ucPeriod>(); //記錄目前選取的分課

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public usrLPView()
        {
            InitializeComponent();
            
            this.mLPViewHelper = new LPViewLayoutHelper(this);            

            foreach (Control vControl in this.Controls)
            {
                if (vControl.Name.StartsWith("uc"))
                {
                    ucPeriod Period = vControl as ucPeriod;
                    Period.ActionEvent = (sender, e) =>
                    {
                        PictureBox picAction = sender as PictureBox;

                        if (picAction.Tag!=null && picAction.Tag.Equals("Delete"))
                        {
                            string ucPeriodName = picAction.Parent.Name;

                            int WeekDay = int.MinValue;
                            int PeriodNo = int.MinValue;

                            int.TryParse(ucPeriodName.Substring(8, 1), out WeekDay);
                            int.TryParse(ucPeriodName.Substring(9, ucPeriodName.Length - 9), out PeriodNo);

                            //若是星期或節次小於或等於0則不繼續執行
                            if (WeekDay <= 0 || PeriodNo <= 0) return;

                            Cursor = Cursors.WaitCursor;

                            string idEvent = Unschedule(WeekDay, PeriodNo);

                            //if (!string.IsNullOrEmpty(idEvent))
                            //{
                            //    AddTestEvent(idEvent);
                            //}

                            CEvent localEvent = schLocal.CEvents[idEvent];

                            Cursor = Cursors.Arrow;
                        }
                    };
                }
            }

            #region Scheduler相關事件
            schLocal.AutoScheduleComplete += (sender, e) =>
            {
                string idBottom = e.EventList[e.BottomIndex].EventID;
                bool bUpdateContent = false;

                foreach (CEvent evtMember in e.EventList)
                {
                    if (IsRelatedEvent(evtMember.EventID))
                    {
                        if (evtMember.WeekDay != 0)
                        {
                            RemoveTestEvent(evtMember.EventID);
                        }

                        bUpdateContent = true;
                    }
                    if (evtMember.EventID == idBottom) return;
                }
                if (bUpdateContent) UpdateContent();
            };

            //自動排課中顯示進度
            schLocal.AutoScheduleProgress += (sender, e) =>
            {
                if (frmASProgress != null)
                {
                    frmASProgress.ChangeProgress(e.nCurIndex);
                    e.Cancel = frmASProgress.UserAbort;
                }

                #region VB
                //    DoEvents
                //    If Not (frmASProgress Is Nothing) Then
                //        frmASProgress.ChangeProgress nCurIndex
                //        Cancel = frmASProgress.UserAbort
                //    End If
                #endregion
            };

            schLocal.EventBeforeDelete += (sender, e) =>
            {
                if (idXchgEvent == e.EventID)
                    idXchgEvent = string.Empty;
                RemoveTestEvent(e.EventID);
            };

            schLocal.EventLocked += (sender, e) =>
            {
                if (IsRelatedEvent(e.EventID))
                    UpdateContent();
            };

            schLocal.EventPropertyChanged += (sender, e) =>
            {
                RemoveTestEvent(e.EventID);

                if (IsRelatedEvent(e.EventID))
                    AddTestEvent(e.EventID);
            };

            schLocal.EventScheduled += (sender, e) =>
            {
                RemoveTestEvent(e.EventID);
                if (IsRelatedEvent(e.EventID))
                    UpdateContent();
            };

            schLocal.EventsFreed += (sender, e) =>
            {
                foreach (CEvent evt in e.EventList)
                    if (IsRelatedEvent(evt.EventID))
                    {
                        idTestEvent = string.Empty;
                        idTestCourseID = string.Empty;
                        UpdateContent();
                        break;
                    }
            };

            schLocal.EventUnlocked += (sender, e) =>
            {
                if (IsRelatedEvent(e.EventID))
                    UpdateContent();
            };
            #endregion
        }

        #region methods

        /// <summary>
        /// 設定功課表所屬的資源型態及資源編號並依此設定功課表內容。
        /// </summary>
        /// <param name="AssocType">資源型態，分為evWho、evWhom、evWhere。</param>
        /// <param name="AssocID">資源編號，若是evWho則其值為教師編號。</param>
        public void SetAssocObject(int AssocType, string AssocID)
        {
            #region 初始化區域變數及功課表抬頭
            mType = AssocType;
            mObjID = AssocID;
            idTestEvent = string.Empty; //測試的EventID
            idXchgEvent = string.Empty; //調課的EventID

            switch (AssocType)
            {
                case lvWho:
                    schLocal.Teachers[mObjID].UseAppointments(0);    //預設使用第一個行事曆
                    apsCur = schLocal.Teachers[mObjID].Appointments; //取得資源已被排定的約會
                    ttCur = schLocal.TimeTables[0];
                    Tag = schLocal.Teachers[mObjID].Name + "功課表";
                    mObjName = schLocal.Teachers[mObjID].Name;
                    break;
                case lvWhom:
                    apsCur = schLocal.Classes[mObjID].Appointments;
                    ttCur = schLocal.TimeTables[schLocal.Classes[mObjID].TimeTableID];
                    Tag = schLocal.Classes[mObjID].Name + "功課表";
                    mObjName = schLocal.Classes[mObjID].Name;
                    //cmdXchgEvent.Visibility = System.Windows.Visibility.Visible;
                    break;
                case lvWhere:
                    apsCur = new Appointments();

                    for (int i = 0; i < schLocal.Classrooms[mObjID].Capacity; i++)
                    {
                        schLocal.Classrooms[mObjID].UseAppointments(i); //預設使用第一個行事曆

                        foreach (Appointment App in schLocal.Classrooms[mObjID].Appointments)
                            apsCur.Add(App);
                    }
                    ttCur = schLocal.TimeTables[0];
                    Tag = schLocal.Classrooms[mObjID].Name + "功課表";
                    mObjName = schLocal.Classrooms[mObjID].Name;
                    break;
            }
            #endregion

            #region 初始化行事曆數量
            mOption.CapacityIndex = 0;
            #endregion

            //Textboxes and imgTest
            ClearTestEvent();

            #region 初始化顯示教師、班級、場地及科目選項
            //Checkboxes
            switch (AssocType)
            {
                case lvWho:
                    mOption.IsTeacher = false;
                    mOption.IsClass = true;
                    mOption.IsClassroom = true;
                    mOption.IsSubject = true;
                    mOption.IsSubjectAlias = false;
                    break;
                case lvWhom:
                    mOption.IsTeacher = true;
                    mOption.IsClass = false;
                    mOption.IsClassroom = true;
                    mOption.IsSubject = true;
                    mOption.IsSubjectAlias = false;
                    break;
                case lvWhere:
                    mOption.IsTeacher = true;
                    mOption.IsClass = true;
                    mOption.IsClassroom = false;
                    mOption.IsSubject = true;
                    mOption.IsSubjectAlias = false;
                    break;
            }
            #endregion

            #region 初始化功課表內容
            //Redraw grid
            //RedrawGrid();

            //Fill content
            //UpdateContent();
            #endregion
        }

        /// <summary>
        /// 根據分課表編號同步TimeTable編號
        /// </summary>
        /// <param name="EventID">分課表編號</param>
        public void SyncTimeTable(string EventID)
        {
            if (!string.IsNullOrEmpty(EventID))
                ChangeTimeTable(GetTimeTableID(EventID));
        }
        #endregion

        #region private function
        /// <summary>
        /// 判斷某個事件是否已被排程
        /// </summary>
        /// <param name="EventID">事件編號</param>
        /// <returns></returns>
        private bool IsScheduled(string EventID)
        {
            return schLocal.CEvents[EventID].WeekDay != 0;
        }

        /// <summary>
        /// 根據事件編號判斷事件的資源屬性是否與ObjID相等
        /// </summary>
        /// <param name="EventID"></param>
        /// <returns></returns>
        private bool IsRelatedEvent(string EventID)
        {
            //根據事件編號取得事件
            CEvent evtTest = schLocal.CEvents[EventID];

            //Determine if this event is related to this eventlist
            //根據LPView的判斷：
            //型態是lvWho，則判斷事件的教師編號是否會與mObjID相等
            //型態是lvWhom，則判斷事件的班級編號是否會與mObjID相等
            //型態是lvWhere，則判斷事件的場地編號是否會與mObjID相等
            switch (mType)
            {
                case lvWho:
                    return evtTest.TeacherID1 == mObjID || evtTest.TeacherID2 == mObjID || evtTest.TeacherID3 == mObjID;
                case lvWhom:
                    return evtTest.ClassID == mObjID;
                case lvWhere:
                    return evtTest.ClassroomID == mObjID;
            }

            return false;
        }

        /// <summary>
        /// 更新時間表
        /// Change the current Timetable view and update the grids
        /// </summary>
        /// <param name="nTimeTableID"></param>
        private void ChangeTimeTable(string nTimeTableID)
        {
            mOption.TimeTableID = nTimeTableID;

            ttCur = schLocal.TimeTables[nTimeTableID];

            RedrawGrid();

            UpdateContent();
        }

        /// <summary>
        /// 根據事件編號取得時間表編號
        /// </summary>
        /// <param name="EventID">事件編號</param>
        /// <returns>時間表編號</returns>
        private string GetTimeTableID(string EventID)
        {
            return string.IsNullOrEmpty(EventID) ? string.Empty : schLocal.CEvents[EventID].TimeTableID;
        }

        /// <summary>
        /// 根據星期及節次釋放分課
        /// </summary>
        /// <param name="nWeekDay">星期幾</param>
        /// <param name="nPeriodNo">節次</param>
        /// <returns>事件編號</returns>
        private string Unschedule(int nWeekDay, int nPeriodNo)
        {
            Period prdTest;
            Appointments appTests;
            Appointment appTest;

            #region Current TimeTable的Period存在才繼續
            prdTest = ttCur.Periods.GetPeriod(nWeekDay, nPeriodNo);

            if (prdTest == null)
                return string.Empty;
            #endregion

            #region Current Appointment存在才繼續
            appTests = apsCur.GetAppointments(prdTest.WeekDay, prdTest.BeginTime, prdTest.Duration);

            if (appTests.Count == 0)
                return string.Empty;
            else if (appTests.Count == 1 && !string.IsNullOrEmpty(appTests[0].EventID))
                appTest = appTests[0];
            else if (appTests.Count >= 2)
            {
                //List<string> EventIDs = new List<string>();

                //foreach(Appointment app in appTests)
                //{
                //    if (!string.IsNullOrEmpty(app.EventID))
                //        EventIDs.Add(app.EventID);
                //}

                //if (EventIDs.Count > 0)
                //{
                //    frmEventSelector EventSelector = new frmEventSelector(EventIDs);

                //    if (EventSelector.ShowDialog()== true)
                //    {
                //        #region 沒有鎖定才繼續
                //        CEvent localEvent = schLocal.CEvents[appTest.EventID];

                //        if (localEvent.ManualLock) return string.Empty;
                //        #endregion

                //        #region 嘗試釋放事件
                //        if (GetTimeTableID(appTest.EventID) == ttCur.TimeTableID)
                //        {
                //            schLocal.FreeEvent(appTest.EventID);
                //            return appTest.EventID;
                //        }
                //        #endregion
                //    }
                //}

                appTest = appTests[0];

                foreach (Appointment vapp in appTests)
                {
                    if (!string.IsNullOrEmpty(vapp.EventID) && vapp.EventID.Equals(idTestEvent))
                        appTest = vapp;
                }
            }
            else
                return string.Empty;
            #endregion

            //取得Appointment的EventID
            if (string.IsNullOrEmpty(appTest.EventID)) return string.Empty;

            #region 沒有鎖定才繼續
            CEvent localEvent = schLocal.CEvents[appTest.EventID];

            if (localEvent.ManualLock) return string.Empty;
            #endregion

            #region 嘗試釋放事件
            if (GetTimeTableID(appTest.EventID) == ttCur.TimeTableID)
            {
                schLocal.FreeEvent(appTest.EventID);
                return appTest.EventID;
            }
            #endregion

            return string.Empty;

            #region VB
            //Private Function Unschedule(ByVal nWeekDay As Integer, ByVal nPeriodNo As Integer) As Long
            //    Dim prdTest As Period
            //    Dim appTest As Appointment

            //    Unschedule = 0
            //    Set prdTest = ttCur.Periods.GetPeriod(nWeekDay, nPeriodNo)
            //    If prdTest Is Nothing Then Exit Function
            //    With prdTest
            //        Set appTest = apsCur.GetAppointment(.WeekDay, .BeginTime, .Duration, nWeekFlag)
            //        If appTest Is Nothing Then Exit Function
            //        If appTest.RefID = 0 Then Exit Function
            //        If schLocal.CEvents(CStr(appTest.RefID)).ManualLock Then Exit Function
            //        If GetTimeTableID(appTest.RefID) = ttCur.TimeTableID Then
            //            schLocal.FreeEvent appTest.RefID
            //            Unschedule = appTest.RefID
            //        End If
            //    End With
            //End Function
            #endregion
        }

        /// <summary>
        /// 根據星期及節次釋放分課
        /// </summary>
        /// <param name="nWeekDay">星期幾</param>
        /// <param name="nPeriodNo">節次</param>
        /// <returns>事件編號</returns>
        private List<string> Unschedule(List<string> EventIDs)
        {
            List<string> UnscheduleEventIDs = new List<string>();

            foreach (string EventID in EventIDs)
            {
                if (!string.IsNullOrEmpty(EventID))
                {
                    CEvent localEvent = schLocal.CEvents[EventID];

                    if (!localEvent.ManualLock)
                    {
                        if (GetTimeTableID(EventID) == ttCur.TimeTableID)
                        {
                            schLocal.FreeEvent(EventID);
                            UnscheduleEventIDs.Add(EventID);
                        }
                    }
                }
            }

            return UnscheduleEventIDs;
        }

        /// <summary>
        /// 根據星期及節次取得事件編號
        /// </summary>
        /// <param name="nWeekDay"></param>
        /// <param name="nPeriodNo"></param>
        /// <returns></returns>
        private string GetEventID(int nWeekDay, int nPeriodNo)
        {
            Appointments appTests;
            Appointment appTest = null;

            #region Period存在才繼續
            Period prdTest = ttCur.Periods.GetPeriod(nWeekDay, nPeriodNo);

            if (prdTest == null) return string.Empty;
            #endregion

            #region Appointment存在才繼續
            //Get appontment that occupied the period
            appTests = apsCur.GetAppointments(prdTest.WeekDay, prdTest.BeginTime, prdTest.Duration);

            //若是Appointment不為null才繼續
            /*
            if (appTests.Count == 0)
                return string.Empty;
            else if (appTests.Count == 1 && !string.IsNullOrEmpty(appTests[0].EventID))
                appTest = appTests[0];
            else if (appTests.Count == 2 && !string.IsNullOrEmpty(appTests[0].EventID) && !string.IsNullOrEmpty(appTests[1].EventID))
                appTest = appTests[1].WeekFlag == 2 ? appTests[1] : appTests[0];
            else if (appTests.Count > 2 && !string.IsNullOrEmpty(appTests[0].EventID))
                appTest = appTests[0];
            else
                return string.Empty;
             */

            if (appTests.Count == 0)
                return string.Empty;
            else if (appTests.Count == 1 && !string.IsNullOrEmpty(appTests[0].EventID))
                appTest = appTests[0];
            else if (appTests.Count >= 2)
            {
                List<string> EventIDs = new List<string>();

                foreach (Appointment appSelector in appTests)
                    if (!string.IsNullOrEmpty(appSelector.EventID))
                        EventIDs.Add(appSelector.EventID);

                frmEventSelector Selector = new frmEventSelector(EventIDs);

                if (Selector.ShowDialog() == DialogResult.OK)
                {
                    List<string> vEventIDs = Selector.SelectedEventIDs;

                    if (Selector.SelectorType == 1)
                    {
                        if (vEventIDs.Count == 1)
                        {
                            string vEventID = vEventIDs[0];
                            CEvent evtTest = schLocal.CEvents[vEventID];

                            if (!string.IsNullOrEmpty(vEventID) && !evtTest.ManualLock)
                            {
                                SetTestEvent(vEventID);
                                UpdateContent();
                            };

                            idTestEvents.Clear();
                        }
                        else
                        {
                            foreach (string vEventID in vEventIDs)
                            {
                                CEvent evtTest = schLocal.CEvents[vEventIDs[0]];

                                if (!evtTest.ManualLock)
                                    idTestEvents.Add(vEventID);
                            }

                            if (idTestEvents.Count > 0)
                            {
                                SetTestEvent(idTestEvents[0]);
                                idTestEvents = vEventIDs;

                                UpdateContent();
                            }
                        }
                    }
                    else if (Selector.SelectorType == 2)
                    {
                        List<string> idEvents = Unschedule(vEventIDs);

                        if (idEvents.Count > 0)
                        {
                            idEvents.ForEach(x => AddTestEvent(x));
                        }
                    }

                    return string.Empty;
                }
            }
            else
                return string.Empty;
            #endregion

            //若是EventID不為0才繼續
            //if (string.IsNullOrEmpty(appTest.EventID)) return string.Empty;

            //Make sure the event is allocated under same timetable
            if (appTest != null)
                if (GetTimeTableID(appTest.EventID) == ttCur.TimeTableID)
                    return appTest.EventID;

            return string.Empty;
        }

        /// <summary>
        /// Toggle the lock condition of a particular event indicated by Weekday and period
        /// </summary>
        /// <param name="nWeekDay">星期</param>
        /// <param name="nPeriodNo">節次</param>
        private void ToggleLock(int nWeekDay, int nPeriodNo)
        {
            Appointment appTest;

            #region Period存在才繼續
            Period prdTest = ttCur.Periods.GetPeriod(nWeekDay, nPeriodNo);

            if (prdTest == null) return;
            #endregion

            #region Appointment存在才繼續
            //Get appontment that occupied the period
            appTest = apsCur.GetAppointment(prdTest.WeekDay, prdTest.BeginTime, prdTest.Duration, mOption.WeekFlag);

            //若是Appointment不為null才繼續
            if (appTest == null) return;
            #endregion

            //若是EventID不為0才繼續
            if (string.IsNullOrEmpty(appTest.EventID)) return;

            //Make sure the event is allocated under same timetable
            if (GetTimeTableID(appTest.EventID) == ttCur.TimeTableID)
            {
                if (schLocal.CEvents[appTest.EventID].ManualLock)
                    schLocal.UnlockEvent(appTest.EventID);
                else
                    schLocal.LockEvent(appTest.EventID);
            }

            #region VB
            //'Toggle the lock condition of a particular event indicated by Weekday and period
            //Private Sub ToggleLock(ByVal nWeekDay As Integer, ByVal nPeriodNo As Integer)
            //    Dim prdTest As Period
            //    Dim appTest As Appointment

            //    'Get corresponding PERIOD object
            //    Set prdTest = ttCur.Periods.GetPeriod(nWeekDay, nPeriodNo)
            //    If prdTest Is Nothing Then Exit Sub

            //    With prdTest
            //        'Get  appointment that occupied the period
            //        Set appTest = apsCur.GetAppointment(.WeekDay, .BeginTime, .Duration, nWeekFlag)
            //        If appTest Is Nothing Then Exit Sub
            //        If appTest.RefID = 0 Then Exit Sub
            //        'Make sure the event is allocated under same timetable
            //        If GetTimeTableID(appTest.RefID) = ttCur.TimeTableID Then
            //            If schLocal.CEvents(CStr(appTest.RefID)).ManualLock Then
            //                schLocal.UnlockEvent appTest.RefID
            //            Else
            //                schLocal.LockEvent appTest.RefID
            //            End If
            //        End If
            //    End With
            //End Sub
            #endregion
        }

        /// <summary>
        /// 根據TimeTable.Periods的MaxWeekDay來決定顯示的欄位
        /// </summary>
        private void RedrawGrid()
        {
            #region 取得時間表節次清單
            Periods.Clear();

            foreach (Period vPeriod in ttCur.Periods)
            {
                if (vPeriod.PeriodNo != 0)
                    if (!Periods.Contains(vPeriod.PeriodNo))
                        Periods.Add(vPeriod.PeriodNo);
            }

            Periods.Sort();
            #endregion

            //取得目前時間表的星期及節次數目
            Tuple<int, int> WeekPeriodCount = mLPViewHelper.GetWeekdayAndPeriod();

            //若星期與節次相同，則只將內容清空
            if (WeekPeriodCount.Item1 != ttCur.Periods.MaxWeekDay ||
                WeekPeriodCount.Item2 != Periods.Count)
            {
                //重新繪製時間表
                mLPViewHelper.ResetLayout();
              
                mLPViewHelper.SetWeekday(ttCur.Periods.MaxWeekDay);
                mLPViewHelper.SetPeriod(Periods);
                mLPViewHelper.SetCells(ttCur.Periods.MaxWeekDay, Periods.Count);
                mLPViewHelper.ResumeLayout();
            }
        }

        /// <summary>
        /// 重新填入DataGrid及畫面上相關的內容
        /// </summary>
        private void UpdateContent()
        {
            this.SelectedPeriods.ForEach(x => x.SetBlank());
            this.SelectedPeriods.Clear();

            Appointment appTest = null;
            int nSolution = 0;
            string idLast = string.Empty;
            bool bLastXchgable = false;
            Panel strEvtInfo = null;

            Stopwatch watch = Stopwatch.StartNew();

            mLPViewHelper.SetCellsToBlank();

            watch.Stop();

            Console.WriteLine("" + watch.ElapsedMilliseconds);

            #region  針對時間表當中的每個時段
            foreach (Period prdMember in ttCur.Periods)            
            {
                int CurrentWeekday = prdMember.WeekDay;
                int CurrentPeriod = -1;

                for (int i = 0; i < Periods.Count; i++)
                    if (Periods[i].Equals(prdMember.PeriodNo))
                    {
                        CurrentPeriod = i + 1;
                        break;
                    }

                //若節次不為0才繼續
                if (prdMember.PeriodNo != 0)
                {
                    //ucPeriod aucPeriod = mLPViewHelper.GetPeriod(CurrentWeekday, CurrentPeriod);

                    //aucPeriod.SetBlank();

                    bool IsSetWeekDayText = false;

                    //根據Period檢查Appointments是否有對應的空閒時間
                    appTest = apsCur.GetAppointment(
                        prdMember.WeekDay,
                        prdMember.BeginTime,
                        prdMember.Duration,
                        mOption.WeekFlag);

                    if (AssocObjType == Constants.lvWhere)
                    {
                        apsCur = new Appointments();

                        for (int i = 0; i < schLocal.Classrooms[mObjID].Capacity; i++)
                        {
                            schLocal.Classrooms[mObjID].UseAppointments(i); //預設使用第一個行事曆

                            foreach (Appointment App in schLocal.Classrooms[mObjID].Appointments)
                                apsCur.Add(App);
                        }
                    }

                    Appointments appTests = apsCur.GetAppointments(prdMember.WeekDay,prdMember.BeginTime,prdMember.Duration);

                    #region Appointment 判斷
                    if (appTests == null || appTests.Count ==0)
                    {
                        ucPeriod ucPeriod = mLPViewHelper.GetPeriod(CurrentWeekday, CurrentPeriod);

                        ucPeriod.SetStyle(lvFreeBackColor,lvFreeForeColor, Resources.blank);

                        string DisplayStr = prdMember.Disable ? prdMember.DisableMessage : string.Empty;

                        SetDisplayStr(ucPeriod, DisplayStr);

                        #region 空閒時段
                        //available time
                        //itemCurrent.SetWeekDayText(
                        //    prdMember.WeekDay,
                        //    strEvtInfo,
                        //    lvFreeBackColor,
                        //    lvFreeForeColor,
                        //    "images/blank.png");

                        IsSetWeekDayText = false;
                        #endregion
                    }
                    else if (appTests.IsMultipleEvents())
                    {
                        #region 若Appointment的Timetable等於目前的TimeTable則顯示，否則顯示TimeTable名稱
                        if (GetTimeTableID(appTests[0].EventID) == ttCur.TimeTableID)
                        {
                            //判斷是否為單雙週課
                            if (appTests.IsSDAppointments())
                            {
                                //this event can be viewed locally
                                #region Display Resource Name，同時顯示單週及雙週課程
                                ucPeriod ucPeriod = mLPViewHelper.GetPeriod(CurrentWeekday, CurrentPeriod);

                                SetComplexDisplayStr(ucPeriod, appTests[0].EventID, appTests[1].EventID);

                                CEvent eventS = schLocal.CEvents[appTests[0].EventID];
                                CEvent eventD = schLocal.CEvents[appTests[1].EventID];
                                #endregion

                                Color BorderColor = Color.DarkGray;

                                #region 判斷單週及雙週的格式顏色及線條大小
                                if ((eventS.CourseID.Equals(idTestCourseID) || eventS.EventID.Equals(idTestEvent)) || (eventD.CourseID.Equals(idTestCourseID) || eventD.EventID.Equals(idTestEvent)))
                                    BorderColor = Color.Orange;

                                int BorderThickness = 1;

                                if ((eventS.CourseID.Equals(idTestCourseID) || eventS.EventID.Equals(idTestEvent)) || (eventD.CourseID.Equals(idTestCourseID) || eventD.EventID.Equals(idTestEvent)))
                                    BorderThickness = 2;
                                #endregion

                                Bitmap Picture = Resources.blank;

                                if (eventS.ManualLock || eventD.ManualLock)
                                    Picture = Resources.鎖定;
                                else if (eventS.EventID.Equals(idTestEvent) || eventD.EventID.Equals(idTestEvent))
                                    Picture = Resources.解除鎖定;

                                ucPeriod.SetStyle(lvScheduledBackColor, lvScheduledForeColor, Picture, BorderColor, BorderThickness);
                                //itemCurrent.SetWeekDayText(prdMember.WeekDay, strEvtInfo, lvScheduledBackColor, lvScheduledForeColor, Picture, BorderColor, BorderThickness);

                                IsSetWeekDayText = true;
                              
                            }//判斷是否為群組課程
                            else if (appTests.IsGroupAppointments())
                            {
                                //this event can be viewed locally
                                #region Display Resource Name，同時顯示單週及雙週課程
                                ucPeriod ucPeriod = mLPViewHelper.GetPeriod(CurrentWeekday, CurrentPeriod);

                                List<string> EventIDs = appTests.Select(x=>x.EventID).ToList();

                                SetGroupDisplayStr(ucPeriod,EventIDs);

                                //strEvtInfo = GetGroupDisplayStr(EventIDs);
                                #endregion

                                #region 判斷單週及雙週的格式顏色及線條大小
                                Color BorderColor = Color.DarkGray;
                                int BorderThickness = 1;

                                foreach(Appointment app in appTests)
                                {
                                    CEvent evtLocal = schLocal.CEvents[app.EventID];

                                    if ((evtLocal.CourseID.Equals(idTestCourseID) || evtLocal.EventID.Equals(idTestEvent)))
                                        BorderColor = Color.Orange;

                                    if ((evtLocal.CourseID.Equals(idTestCourseID) || evtLocal.EventID.Equals(idTestEvent)))
                                        BorderThickness = 2;
                                }
                                #endregion

                                Bitmap Picture = Resources.blank;

                                foreach(Appointment app in appTests)
                                {
                                    CEvent evtLocal = schLocal.CEvents[app.EventID];

                                    if (evtLocal.ManualLock)
                                        Picture = Resources.鎖定;
                                    else if (evtLocal.EventID.Equals(idTestEvent))
                                        Picture = Resources.解除鎖定;
                                }

                                ucPeriod.SetStyle(lvScheduledBackColor, lvScheduledForeColor, Picture, BorderColor, BorderThickness);

                                //itemCurrent.SetWeekDayText(prdMember.WeekDay, strEvtInfo, lvScheduledBackColor, lvScheduledForeColor, Picture, BorderColor, BorderThickness);

                                IsSetWeekDayText = true;
                                #endregion
                            }//判斷是否為場地容納多課程
                            else if (AssocObjType == Constants.lvWhere)
                            {
                                //場地課表多容納課程的顯示；顯示課程數（例如英文..3+)及Tooltip。
                                //this event can be viewed locally
                                #region Display Resource Name，同時顯示場地課程
                                ucPeriod ucPeriod = mLPViewHelper.GetPeriod(CurrentWeekday, CurrentPeriod);

                                List<string> EventIDs = appTests.Select(x=>x.EventID).ToList();
                                GetWhereDisplayStr(ucPeriod,EventIDs);
                                #endregion

                                #region 判斷單週及雙週的格式顏色及線條大小
                                Color BorderColor = Color.DarkGray;
                                int BorderThickness = 1;

                                foreach(Appointment app in appTests)
                                {
                                    CEvent evtLocal = schLocal.CEvents[app.EventID];

                                    if ((evtLocal.CourseID.Equals(idTestCourseID) || evtLocal.EventID.Equals(idTestEvent)))
                                        BorderColor = Color.Orange;

                                    if ((evtLocal.CourseID.Equals(idTestCourseID) || evtLocal.EventID.Equals(idTestEvent)))
                                        BorderThickness = 2;
                                }
                                #endregion
                                //當有多門場地狀況一律顯示空白
                                Bitmap Picture = Resources.blank;

                                //foreach(Appointment app in appTests)
                                //{
                                //    CEvent evtLocal = schLocal.CEvents[app.EventID];

                                //    if (evtLocal.ManualLock)
                                //        Picture = "images/sunset/鎖定.png";
                                //    else if (EventIDs.Contains(idTestEvent))
                                //        Picture = "images/sunset/關閉.png";
                                //    //else if (evtLocal.EventID.Equals(idTestEvent))
                                //    //    Picture = "images/sunset/關閉.png";
                                //}

                                //itemCurrent.SetWeekDayText(prdMember.WeekDay, strEvtInfo, lvScheduledBackColor, lvScheduledForeColor, Picture, BorderColor, BorderThickness);

                                ucPeriod.SetStyle(lvScheduledBackColor, lvScheduledForeColor, Picture, BorderColor, BorderThickness);

                                IsSetWeekDayText = true;
                            }
                            else
                            {
 
                            }
                        }
                        else //當資源的TimeTableID不等於Appointment的TimeTableID，那麼設定顯示名稱為Appointment的TimeTableID
                        {
                            ucPeriod ucPeriod = mLPViewHelper.GetPeriod(CurrentWeekday, CurrentPeriod);

                            ucPeriod.SetStyle(lvTimeTableBackColor, lvTimeTableForeColor, Resources.blank);

                            SetComplexDisplayStr(ucPeriod, appTests[0].EventID, appTests[1].EventID);

                            //strEvtInfo = GetComplexDisplayStr(appTests[0].EventID, appTests[1].EventID);
                            //itemCurrent.SetWeekDayText(prdMember.WeekDay, strEvtInfo, lvTimeTableBackColor, lvTimeTableForeColor, "images/blank.png");
                         
                            IsSetWeekDayText = true;
                        }
                        #endregion

                        //#region 查詢是否可調課
                        //if (!string.IsNullOrEmpty(idXchgEvent))
                        //{
                        //    if (idXchgEvent == appTest.EventID)
                        //    {
                        //         itemCurrent.SetWeekDayColor(prdMember.WeekDay, lvScheduledBackColor,lvScheduledForeColor);
                        //    }
                        //    else if (idLast == appTest.EventID)
                        //    {
                        //        if (bLastXchgable)
                        //            itemCurrent.SetWeekDayColor(prdMember.WeekDay,lvSchedulableBackColor, lvSchedulableForeColor);
                        //    }
                        //    else if (schLocal.CEvents[idXchgEvent].WhoID!=schLocal.CEvents[appTest.EventID].WhoID && schLocal.IsEventExchangable(idXchgEvent,appTest.EventID))
                        //    {
                        //        itemCurrent.SetWeekDayColor(prdMember.WeekDay,lvSchedulableBackColor, lvSchedulableForeColor);
                        //        bLastXchgable = true;
                        //    }
                        //    else
                        //        bLastXchgable = false;
                        //}
                        //#endregion 
                    }
                    else if (appTests.Count >= 1)
                    {
                        appTest = appTests[0];

                        #region 若EventID為空值，則為不排課時段
                        if (string.IsNullOrEmpty(appTest.EventID))
                        {
                            ucPeriod ucPeriod = mLPViewHelper.GetPeriod(CurrentWeekday, CurrentPeriod);

                            ucPeriod.SetStyle(lvBusyBackColor,lvBusyForeColor,Resources.blank);                          

                            string BusyDesc = !string.IsNullOrEmpty(appTest.Description) ? appTest.Description : "不排課時段";

                            SetDisplayStr(ucPeriod, BusyDesc);

                            IsSetWeekDayText = true;

                            #region 不排課時段
                            //busy time
                            //itemCurrent.SetWeekDayText(
                            //prdMember.WeekDay,
                            //strEvtInfo,
                            //lvBusyBackColor,
                            //lvBusyForeColor,
                            //"images/Sunset/不排課時段.png");
                            #endregion
                        }
                        #endregion
                        else
                        {
                            #region 若Appointment的Timetable等於目前的TimeTable則顯示，否則顯示TimeTable名稱

                            string CC = GetTimeTableID(appTest.EventID);

                            if ( CC == ttCur.TimeTableID)
                            {
                                #region 顯示已排課時段
                                //this event can be viewed locally
                                #region Display Resource Name
                                CEvent eventLocal = schLocal.CEvents[appTest.EventID];
                                #endregion

                                Color BorderColor = eventLocal.CourseID.Equals(idTestCourseID) || eventLocal.EventID.Equals(idTestEvent) ? Color.Orange : Color.DarkGray;
                                int BorderThickness = eventLocal.CourseID.Equals(idTestCourseID) || eventLocal.EventID.Equals(idTestEvent) ? 2 : 1;
                                Bitmap Picture = Resources.blank;
                                string Action = string.Empty;

                                ucPeriod ucPeriod = mLPViewHelper.GetPeriod(CurrentWeekday, CurrentPeriod);

                                //只有在排課模式才會顯示『鎖定』及『關閉』
                                if (eventLocal.ManualLock)
                                    Picture = Resources.鎖定;
                                else if (eventLocal.EventID.Equals(idTestEvent))
                                {
                                    SelectedPeriods.Add(ucPeriod);
                                    Picture = Resources.刪除;
                                    Action = "Delete";
                                }

                                ucPeriod.SetStyle(lvScheduledBackColor,lvScheduledForeColor , Picture, BackColor, BorderThickness,Action);
                                SetEventDisplayStr(ucPeriod, appTest.EventID);

                                IsSetWeekDayText = true;
                                #endregion
                            }
                            else //當資源的TimeTableID不等於Appointment的TimeTableID，那麼設定顯示名稱為Appointment的TimeTableID
                            {
                                CC = GetTimeTableID(appTest.EventID);

                                ucPeriod ucPeriod = mLPViewHelper.GetPeriod(CurrentWeekday, CurrentPeriod);

                                ucPeriod.SetStyle(lvTimeTableBackColor,lvTimeTableForeColor,Resources.blank);
                                SetEventDisplayStr(ucPeriod, appTest.EventID);

                                //strEvtInfo = GetEventDisplayStr(appTest.EventID);
                                //itemCurrent.SetWeekDayText(prdMember.WeekDay,strEvtInfo ,lvTimeTableBackColor,lvTimeTableForeColor, "images/blank.png");
                                IsSetWeekDayText = true;
                            }
                            #endregion

                            //#region 查詢是否可調課
                            //if (!string.IsNullOrEmpty(idXchgEvent))
                            //{
                            //    if (idXchgEvent == appTest.EventID)
                            //    {
                            //         itemCurrent.SetWeekDayColor(prdMember.WeekDay, lvScheduledBackColor,lvScheduledForeColor);
                            //    }
                            //    else if (idLast == appTest.EventID)
                            //    {
                            //        if (bLastXchgable)
                            //            itemCurrent.SetWeekDayColor(prdMember.WeekDay,lvSchedulableBackColor, lvSchedulableForeColor);
                            //    }
                            //    //else if (schLocal.CEvents[idXchgEvent].WhoID!=schLocal.CEvents[appTest.EventID].WhoID && schLocal.IsEventExchangable(idXchgEvent,appTest.EventID))
                            //    else if (schLocal.IsEventExchangable(idXchgEvent, appTest.EventID))
                            //    {
                            //        itemCurrent.SetWeekDayColor(prdMember.WeekDay,lvSchedulableBackColor, lvSchedulableForeColor);
                            //        bLastXchgable = true;
                            //    }
                            //    else
                            //        bLastXchgable = false;
                            //}
                            //#endregion
                        }
                        idLast = appTest.EventID;
                    }

                    #region 查詢排課解
                    if ((!string.IsNullOrEmpty(idTestEvent) && !schLocal.CEvents[idTestEvent].ManualLock) || 
                        idTestEvents.Count > 0)
                    {
                        if (idTestEvents.Count > 0)
                        {
                            bool IsSchedule = true;

                            foreach (string vidTestEvent in idTestEvents)
                            {
                                if (GetTimeTableID(vidTestEvent) == ttCur.TimeTableID)
                                {
                                    IsSchedule = schLocal.IsSchedulable(vidTestEvent, prdMember.WeekDay, prdMember.PeriodNo);

                                    if (!IsSchedule)
                                    {
                                        if (!IsSetWeekDayText)
                                        {
                                            ucPeriod ucPeriod = mLPViewHelper.GetPeriod(CurrentWeekday, CurrentPeriod);

                                            ucPeriod.SetStyle(lvNoSolutionBackColor, lvNoSolutionForeColor, Resources.blank);

                                            if (!string.IsNullOrEmpty(schLocal.ReasonDesc.AssocID))
                                            {
                                                LabelX lblPeriod1 = ucPeriod.GetLabel(1);
                                                lblPeriod1.SetAssocUI(schLocal.ReasonDesc.AssocType, schLocal.ReasonDesc.AssocID, this.AssocObjType, this.AssocObjID, schLocal.ReasonDesc.AssocName);
                                                ucPeriod.SetContent(2, schLocal.ReasonDesc.Desc);
                                            }
                                            else
                                                ucPeriod.SetContent(1, schLocal.ReasonDesc.Desc);

                                            #region 無解情況
                                            //itemCurrent.SetWeekDayText(prdMember.WeekDay, Panel, lvNoSolutionBackColor, lvNoSolutionForeColor, "images/blank.png");
                                            #endregion
                                        }
                                        break;
                                    }
                                }
                            }

                            if (IsSchedule)
                            {
                                nSolution++;

                                ucPeriod ucPeriod = mLPViewHelper.GetPeriod(CurrentWeekday, CurrentPeriod);

                                ucPeriod.SetStyle(lvSchedulableBackColor,lvSchedulableForeColor,Resources.blank);

                                //itemCurrent.SetWeekDayColor(prdMember.WeekDay, lvSchedulableBackColor, lvSchedulableForeColor);
                            }
                        }
                        else if (GetTimeTableID(idTestEvent) == ttCur.TimeTableID)
                        {
                            bool IsSchedule = schLocal.IsSchedulable(idTestEvent, prdMember.WeekDay, prdMember.PeriodNo);

                            if (IsSchedule)
                            {
                                nSolution++;

                                ucPeriod ucPeriod = mLPViewHelper.GetPeriod(CurrentWeekday, CurrentPeriod);
                                ucPeriod.SetStyle(lvSchedulableBackColor, lvSchedulableForeColor, Resources.blank);

                                //itemCurrent.SetWeekDayColor(prdMember.WeekDay, lvSchedulableBackColor, lvSchedulableForeColor);
                            }
                            else
                            {
                                if (!IsSetWeekDayText)
                                {
                                    ucPeriod ucPeriod = mLPViewHelper.GetPeriod(CurrentWeekday, CurrentPeriod);
                                    ucPeriod.SetStyle(lvNoSolutionBackColor , lvNoSolutionForeColor, Resources.blank);

                                    if (!string.IsNullOrEmpty(schLocal.ReasonDesc.AssocID))
                                    {                                      
                                        LabelX lblPeriod1 = ucPeriod.GetLabel(1);
                                        lblPeriod1.SetAssocUI(schLocal.ReasonDesc.AssocType, schLocal.ReasonDesc.AssocID, this.AssocObjType, this.AssocObjID, schLocal.ReasonDesc.AssocName);
                                        ucPeriod.SetContent(2, schLocal.ReasonDesc.Desc);
                                    }else
                                        ucPeriod.SetContent(1, schLocal.ReasonDesc.Desc);

                                    #region 無解情況
                                    //itemCurrent.SetWeekDayText(prdMember.WeekDay, Panel, lvNoSolutionBackColor, lvNoSolutionForeColor, "images/blank.png");
                                    #endregion
                                }
                            }
                        }
                    }
                    #endregion
                }

                if ((appTest != null) && (prdMember.PeriodNo != 0))
                    idLast = appTest.EventID;
            }

            SelectedPeriods.ForEach(x => x.IsSelected = true);

            for (int i = 1; i < SelectedPeriods.Count; i++)
                SelectedPeriods[i].SetTopToZero();

            #endregion

            //#region 顯示解決方案
            ////if (!string.IsNullOrEmpty(idTestEvent))
            ////    lblSolution.Content = nSolution + "方案";

            //watch.Stop();
 
            //Console.WriteLine(""+watch.Elapsed.TotalMilliseconds);

            //watch.Restart();

            //fgLPView.ItemsSource = LPViewItems.Values;

            //watch.Stop();
            //#endregion

            //Console.WriteLine("" + watch.Elapsed.TotalMilliseconds);

            mLPViewHelper.ResumeLayout();
        }

        /// <summary>
        /// 取得事件顯示字串
        /// </summary>
        /// <returns></returns>
        private void SetEventDisplayStr(ucPeriod Period, string EventID)
        {
            Period.AddEventControl(mOption, EventID, this.AssocObjType, this.AssocObjID);
        }

        /// <summary>
        /// 取得顯示字串物件
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        private void SetDisplayStr(ucPeriod Period, string Message)
        {
            Period.SetContent(1, Message);
        }

        /// <summary>
        /// 取得場地顯示字串
        /// </summary>
        /// <returns></returns>
        private void GetWhereDisplayStr(ucPeriod Period, List<string> EventIDs)
        {
            string DisplayEventID = EventIDs[0];

            foreach (string EventID in EventIDs)
                if (EventID.Equals(idTestEvent))
                    DisplayEventID = idTestEvent;


            Period.AddEventControl(mOption, DisplayEventID, this.AssocObjType, this.AssocObjID, Top);
            Period.SetContent(2, "共有" + EventIDs.Count + "門分課");

            //Top = 0;

            //foreach (string EventID in EventIDs)
            //    Top = pnlTooltip.AddEventControl(mOption, EventID, this.AssocObjType, this.AssocObjID, Top);

            //pnlTooltip.Height = Top + Delta;
            //Panel.ToolTip = pnlTooltip;

            //return Panel;
        }

        /// <summary>
        /// 取得群組課程字串
        /// </summary>
        /// <param name="EventIDs"></param>
        /// <returns></returns>
        private void SetGroupDisplayStr(ucPeriod Period, List<string> EventIDs)
        {
            CEvent eventLocal = schLocal.CEvents[EventIDs[0]];

            //顯示群組名稱
            Period.SetContent(1, eventLocal.CourseGroup);

            //Label lbl = new Label();
            //lbl.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;
            //lbl.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            //lbl.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            //lbl.VerticalContentAlignment = System.Windows.VerticalAlignment.Top;
            //lbl.Content = eventLocal.CourseGroup;

            //Canvas.SetLeft(lbl, Left);
            //Canvas.SetTop(lbl, Top);
            //Panel.Children.Add(lbl);

            //foreach (string EventID in EventIDs)
            //    Top = pnlTooltip.AddEventControl(mOption, EventID, this.AssocObjType, this.AssocObjID, Top);

            //pnlTooltip.Height = Top + Delta;
            //Panel.ToolTip = pnlTooltip;

            //return Panel;
        }

        /// <summary>
        /// 取得單雙週顯示字串
        /// </summary>
        /// <param name="EventIDS"></param>
        /// <param name="EventIDD"></param>
        /// <returns></returns>
        private void SetComplexDisplayStr(ucPeriod Period, string EventIDS, string EventIDD)
        {
            int DefaultIndex = 1;

            Period.AddEventControl(mOption, EventIDS, this.AssocObjType, this.AssocObjID, DefaultIndex);
            Period.AddEventControl(mOption, EventIDD, this.AssocObjType, this.AssocObjID, DefaultIndex);
        }
        #endregion

        #region TestEvent
        /// <summary>
        /// 設定測試分課
        /// </summary>
        /// <param name="idEvent">分課系統編號</param>
        private void SetTestEvent(string idEvent)
        {
            //目前的測試事件編號等於傳入的事件編號，則不繼續執行
            if (idTestEvent == idEvent) return;

            //取得事件物件，並設定目前的測試事件編號
            CEvent evtTest = schLocal.CEvents[idEvent];
            idTestEvent = idEvent;
            idTestCourseID = evtTest.CourseID;
        }

        /// <summary>
        /// 清除測試分課
        /// </summary>
        private void ClearTestEvent()
        {
            colTestEvents.Clear();

            idTestEvent = string.Empty;
            Transfers.Clear();
        }

        /// <summary>
        /// 檢查分課表編號是否有存在測試分課表編號集合中
        /// </summary>
        /// <param name="EventID">分課表編號</param>
        /// <returns></returns>
        public bool TestEventExists(string EventID)
        {
            return colTestEvents.ContainsKey(EventID);
        }

        /// <summary>
        /// 加入測試分課表編號
        /// </summary>
        /// <param name="EventID">分課表編號</param>
        public void AddTestEvent(string EventID)
        {
            //判斷測試分課表是否存在
            bool bExists = colTestEvents.ContainsKey(EventID);

            #region 分課已存在則設為此分課
            if (bExists)
            {

            }
            #endregion
            #region 加入到測試清單
            else
            {
                if (IsRelatedEvent(EventID))
                {
                    //CEvents evtTests = new CEvents();
                    CEvent evtTest = schLocal.CEvents[EventID];

                    //evtTests.Add(evtTest);

                    //schLocal.GetSolutionCounts(evtTests);

                    //ListBoxItem NewItem = new ListBoxItem();
                    //NewItem.Cursor = Cursors.Hand;
                    //NewItem.Content = new usrTestEvent(evtTest);

                    Transfers.Add(evtTest);

                    //grdTestEvent.Items.Add(evtTest.TransferTo());


                    //lstTestEvent.Items.Add(NewItem);
                    colTestEvents.Add(evtTest.EventID, evtTest.Length); //加入到測試清單
                }
            }
            #endregion
        }

        /// <summary>
        /// 移除測試分課表編號
        /// </summary
        /// <param name="EventID">分課表編號</param>
        public void RemoveTestEvent(string EventID)
        {
            int nIndex = 0;
            bool bExists = colTestEvents.ContainsKey(EventID); //判斷測試事件集合是否包含分課表編號
            CEvent RemoveTestEvent = null;
            //若包含分課表編號則移除
            if (bExists)
            {
                //若測試分課表集合中包含分課表編號，就找出其索引位置
                for (nIndex = 0; nIndex < colTestEvents.Count; nIndex++)
                    if (colTestEvents.Keys.ToList()[nIndex] == EventID)
                        break;

                //將分課表編號自測試集合中移除
                colTestEvents.Remove(EventID);

                #region 自ListBox移除
                for (int i = 0; i < Transfers.Count; i++)
                {
                    //ListBoxItem Item = lstTestEvent.Items[i] as ListBoxItem;
                    //usrTestEvent TestEvent = Item.Content as usrTestEvent;
                    CEvent TestEvent = Transfers[i];

                    if (TestEvent.EventID.Equals(EventID))
                        RemoveTestEvent = TestEvent;
                }

                if (RemoveTestEvent != null)
                {
                    Transfers.Remove(RemoveTestEvent);
                }

                //if (SelectedIndex >= 0)
                //    grdTestEvent.Items.RemoveAt(SelectedIndex);
                //lstTestEvent.Items.RemoveAt(SelectedIndex);
                #endregion
            }

            //若沒有存在則離開
            if (!bExists)
                return;

            //若集合數量為0就清空測試分課表相關資訊，並重新繪製功課表
            if (colTestEvents.Count == 0)
            {
                ClearTestEvent();
                UpdateContent();
                return;
            }
        }

        /// <summary>
        /// 取得目前的測試分課表編號
        /// </summary>
        /// <returns></returns>
        public string CurrentTestEventID()
        {
            return idTestEvent;
        }
        #endregion

        #region Properties
        /// <summary>
        /// 資源類型，有lvWho、lvWhom及lvWhere
        /// </summary>
        public int AssocObjType
        {
            get { return mType; }
        }

        /// <summary>
        /// 資源編號
        /// </summary>
        public string AssocObjID
        {
            get { return mObjID; }
        }

        /// <summary>
        /// 取得目前時間表
        /// </summary>
        public string CurrentTimeTableID
        {
            get 
            {
                if (ttCur != null)
                    return ttCur.TimeTableID;
                else
                    return string.Empty;
            }
        }
        #endregion

        #region DataGrid Event
        /// <summary>
        /// 選取測試分課
        /// </summary>
        /// <param name="Item"></param>
        public void SelectTestEvent(string EventID)
        {
            if (!string.IsNullOrEmpty(EventID))
            {
                SetTestEvent(EventID);
                ChangeTimeTable(GetTimeTableID(EventID));
            }
        }

        [DllImport("kernel32.dll")]
        public static extern bool Beep(int BeepFreq, int BeepDuration);

        /// <summary>
        /// 按下節次
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucPeriod_Click(object sender, EventArgs e)
        {
            ucPeriod Period = sender as ucPeriod;

            int WeekDay = int.MinValue; 
            int PeriodNo = int.MinValue;            

            int.TryParse(Period.Name.Substring(8, 1),out WeekDay);
            int.TryParse(Period.Name.Substring(9, Period.Name.Length - 9),out PeriodNo);

            //若是星期或節次小於或等於0則不繼續執行
            if (WeekDay <= 0 || PeriodNo <= 0) return;

            //若為可排課顏色則進行排課
            if (Period.BackColor.Equals(lvSchedulableBackColor))
            {
                if (idTestEvents.Count > 0)
                {
                    Cursor = Cursors.WaitCursor;

                    List<string> idAddEvents = new List<string>();
                    List<string> idEvents = new List<string>();

                    idTestEvents.ForEach(x => idEvents.Add(x));
                    idTestEvents.Clear();

                    foreach (string idEvent in idEvents)
                    {
                        if (IsRelatedEvent(idEvent))
                        {
                            if (IsScheduled(idEvent))
                                schLocal.FreeEvent(idEvent);

                            string nTimeTableID = GetTimeTableID(idEvent);

                            if (nTimeTableID == ttCur.TimeTableID)
                            {
                                //若拖到畫面上的nPeriodNo或nWeekDay為0則加入測試事件
                                if (PeriodNo == 0 || WeekDay == 0)
                                {
                                    AddTestEvent(idEvent);
                                }
                                else
                                {
                                    //實際安排事件
                                    bool localScheduled = schLocal.ScheduleEvent(idEvent, WeekDay, PeriodNo);

                                    //若無法安排事件，則發出Beep聲，並加入測試事件
                                    if (!localScheduled)
                                    {
                                        Beep(1, 1);
                                        idAddEvents.Add(idEvent);
                                    }
                                }
                            }
                            else
                                idAddEvents.Add(idEvent);
                        }
                    }

                    idTestEvent = string.Empty;

                    idAddEvents.ForEach(x => AddTestEvent(x));

                    Cursor = Cursors.Arrow;
                }
                //嘗試解析事件編號
                else if (!string.IsNullOrEmpty(idTestEvent))
                {
                    string idEvent = idTestEvent;

                    //判斷是否為相關事件或相同的時間表才繼續執行
                    if (IsRelatedEvent(idEvent))
                    {
                        if (IsScheduled(idEvent))
                            schLocal.FreeEvent(idEvent);

                        string nTimeTableID = GetTimeTableID(idEvent);

                        if (nTimeTableID == ttCur.TimeTableID)
                        {
                            Cursor = Cursors.WaitCursor;

                            //若拖到畫面上的nPeriodNo或nWeekDay為0則加入測試事件
                            if (PeriodNo == 0 || WeekDay == 0)
                            {
                                AddTestEvent(idEvent);
                            }
                            else
                            {
                                idTestEvent = string.Empty;
                                //實際安排事件
                                bool localScheduled = schLocal.ScheduleEvent(idEvent, WeekDay, PeriodNo);

                                //若無法安排事件，則發出Beep聲，並加入測試事件
                                if (!localScheduled)
                                {
                                    Beep(1, 1);
                                    AddTestEvent(idEvent);
                                }
                            }

                            Cursor = Cursors.Arrow;
                        }
                        else
                            AddTestEvent(idEvent);
                    }
                }
            }//若為已排課
            else if (Period.BackColor.Equals(lvScheduledBackColor))
            {
                Cursor = Cursors.WaitCursor;

                string idEvent = GetEventID(WeekDay, PeriodNo);

                if (!string.IsNullOrEmpty(idEvent))
                {
                    CEvent evtTest = schLocal.CEvents[idEvent];

                    if (!string.IsNullOrEmpty(idEvent) && !evtTest.ManualLock)
                    {
                        SetTestEvent(idEvent);
                        UpdateContent();
                    };
                }

                Cursor = Cursors.Arrow;
            }

            //    try
            //    {
            //        if (e.RightButton == MouseButtonState.Pressed)
            //            return;

            //        int WeekDay = Constants.NullValue;
            //        int PeriodNo = Constants.NullValue;

            //        Point MousePoint = DataGridHelper.GetMouseRowColumn(e.OriginalSource);

            //        if (((int)MousePoint.X != Constants.NullValue) && ((int)MousePoint.Y != Constants.NullValue))
            //        {
            //            dynamic SelectedItem = fgLPView.Items[(int)MousePoint.Y];

            //            WeekDay = (int)MousePoint.X;
            //            PeriodNo = SelectedItem.PeriodNo;
            //        }

            //        if ((e.LeftButton == MouseButtonState.Pressed && (Keyboard.IsKeyDown(Key.LeftAlt)))
            //            || (e.LeftButton == MouseButtonState.Pressed && Keyboard.IsKeyToggled(Key.RightAlt)))
            //        {
            //            if ((WeekDay != Constants.NullValue) && (PeriodNo != Constants.NullValue))
            //                ToggleLock(WeekDay, PeriodNo);
            //        }
            //        else if (e.LeftButton == MouseButtonState.Pressed && IsDeleteImage(e.OriginalSource))
            //        {
            //            Image CurrentImage = (e.OriginalSource as System.Windows.Controls.Image);

            //            string strImage = "" + CurrentImage.Source;

            //            if (strImage.Contains(strDeleteEventPath))
            //            {
            //                if (WeekDay <= 0 || PeriodNo <= 0) return;

            //                Cursor = Cursors.Wait;

            //                string idEvent = Unschedule(WeekDay, PeriodNo);

            //                if (!string.IsNullOrEmpty(idEvent))
            //                {
            //                    AddTestEvent(idEvent);
            //                    UpdateTestEventSolutionCount();
            //                }

            //                CEvent localEvent = schLocal.CEvents[idEvent];

            //                Cursor = Cursors.Arrow;

            //            }
            //        }
            //        else if (e.LeftButton == MouseButtonState.Pressed)
            //        {
            //            //若是星期或節次小於或等於0則不繼續執行
            //            if (WeekDay <= 0 || PeriodNo <= 0) return;

            //            string CellBackColor = GetCellBackColor((DependencyObject)e.OriginalSource);

            //            if (CellBackColor.Equals(lvSchedulableBackColor))
            //            {
            //                if (idTestEvents.Count > 0)
            //                {
            //                    Cursor = Cursors.Wait;

            //                    List<string> idAddEvents = new List<string>();
            //                    List<string> idEvents = new List<string>();

            //                    idTestEvents.ForEach(x => idEvents.Add(x));
            //                    idTestEvents.Clear();

            //                    foreach (string idEvent in idEvents)
            //                    {
            //                        if (IsRelatedEvent(idEvent))
            //                        {
            //                            if (IsScheduled(idEvent))
            //                                schLocal.FreeEvent(idEvent);

            //                            string nTimeTableID = GetTimeTableID(idEvent);

            //                            if (nTimeTableID == ttCur.TimeTableID)
            //                            {
            //                                //若拖到畫面上的nPeriodNo或nWeekDay為0則加入測試事件
            //                                if (PeriodNo == 0 || WeekDay == 0)
            //                                {
            //                                    AddTestEvent(idEvent);
            //                                }
            //                                else
            //                                {
            //                                    //實際安排事件
            //                                    bool localScheduled = schLocal.ScheduleEvent(idEvent, WeekDay, PeriodNo);

            //                                    //若無法安排事件，則發出Beep聲，並加入測試事件
            //                                    if (!localScheduled)
            //                                    {
            //                                        Beep(1, 1);
            //                                        idAddEvents.Add(idEvent);
            //                                    }
            //                                }
            //                            }
            //                            else
            //                                idAddEvents.Add(idEvent);
            //                        }
            //                    }

            //                    idTestEvent = string.Empty;

            //                    idAddEvents.ForEach(x => AddTestEvent(x));

            //                    UpdateTestEventSolutionCount();

            //                    Cursor = Cursors.Arrow;
            //                }
            //                //嘗試解析事件編號
            //                else if (!string.IsNullOrEmpty(idTestEvent))
            //                {
            //                    string idEvent = idTestEvent;

            //                    //判斷是否為相關事件或相同的時間表才繼續執行
            //                    if (IsRelatedEvent(idEvent))
            //                    {
            //                        if (IsScheduled(idEvent))
            //                            schLocal.FreeEvent(idEvent);

            //                        string nTimeTableID = GetTimeTableID(idEvent);

            //                        if (nTimeTableID == ttCur.TimeTableID)
            //                        {
            //                            Cursor = Cursors.Wait;

            //                            //若拖到畫面上的nPeriodNo或nWeekDay為0則加入測試事件
            //                            if (PeriodNo == 0 || WeekDay == 0)
            //                            {
            //                                AddTestEvent(idEvent);
            //                                UpdateTestEventSolutionCount();
            //                            }
            //                            else
            //                            {
            //                                idTestEvent = string.Empty;
            //                                //實際安排事件
            //                                bool localScheduled = schLocal.ScheduleEvent(idEvent, WeekDay, PeriodNo);

            //                                //若無法安排事件，則發出Beep聲，並加入測試事件
            //                                if (!localScheduled)
            //                                {
            //                                    Beep(1, 1);
            //                                    AddTestEvent(idEvent);
            //                                    UpdateTestEventSolutionCount();
            //                                }
            //                            }

            //                            Cursor = Cursors.Arrow;
            //                        }
            //                        else
            //                            AddTestEvent(idEvent);
            //                    }
            //                }
            //            }
            //            else if (CellBackColor.Equals(lvScheduledBackColor))
            //            {
            //                Cursor = Cursors.Wait;

            //                grdTestEvent.SelectedIndex = -1;

            //                string idEvent = GetEventID(WeekDay, PeriodNo);

            //                if (!string.IsNullOrEmpty(idEvent))
            //                {
            //                    CEvent evtTest = schLocal.CEvents[idEvent];

            //                    if (!string.IsNullOrEmpty(idEvent) && !evtTest.ManualLock)
            //                    {
            //                        SetTestEvent(idEvent);
            //                        UpdateContent();
            //                    };
            //                }

            //                Cursor = Cursors.Arrow;
            //            }
            //        }
            //    }
            //    catch (Exception ve)
            //    {
            //        FISCA.ErrorBox.Show("排入分課時發生錯誤！" + System.Environment.NewLine + ve.Message, ve);
            //    }
        }

        ///// <summary>
        ///// 取得儲存格顏色
        ///// </summary>
        ///// <param name="depObj"></param>
        ///// <returns></returns>
        //private string GetCellBackColor(DependencyObject depObj)
        //{
        //    if (!(depObj is UIElement))
        //        return string.Empty;

        //    string CellBackColor = string.Empty;

        //    Panel currentPanel = null;

        //    while ((depObj != null) && !(depObj is Panel))
        //        depObj = VisualTreeHelper.GetParent(depObj);

        //    if (depObj != null)
        //    {
        //        currentPanel = (Panel)depObj;
        //        CellBackColor = "" + currentPanel.Tag;
        //    }

        //    return CellBackColor;
        //}

        ///// <summary>
        ///// 根據顏色變更滑鼠指標
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void fgLPView_MouseMove(object sender, MouseEventArgs e)
        //{
        //    string CellBackColor = GetCellBackColor((DependencyObject)e.OriginalSource);

        //    Cursor = (CellBackColor.Equals(lvSchedulableBackColor) || CellBackColor.Equals(lvScheduledBackColor)) ? Cursors.Hand : Cursors.Arrow;
        //}

        ///// <summary>
        ///// 是否為刪除影像
        ///// </summary>
        ///// <param name="OriginalSource"></param>
        ///// <returns></returns>
        //private bool IsDeleteImage(object Source)
        //{
        //    if (Source is System.Windows.Controls.Image)
        //    {
        //        Image CurrentImage = (Source as System.Windows.Controls.Image);

        //        string strImage = "" + CurrentImage.Source;

        //        if (strImage.Contains(strDeleteEventPath))
        //            return true;
        //    }

        //    return false;
        //}

        ///// <summary>
        ///// 排課操作模式
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void ScheduleModeAction(object sender, MouseButtonEventArgs e)
        //{
        //    try
        //    {
        //        if (e.RightButton == MouseButtonState.Pressed)
        //            return;

        //        int WeekDay = Constants.NullValue;
        //        int PeriodNo = Constants.NullValue;

        //        Point MousePoint = DataGridHelper.GetMouseRowColumn(e.OriginalSource);

        //        if (((int)MousePoint.X != Constants.NullValue) && ((int)MousePoint.Y != Constants.NullValue))
        //        {
        //            dynamic SelectedItem = fgLPView.Items[(int)MousePoint.Y];

        //            WeekDay = (int)MousePoint.X;
        //            PeriodNo = SelectedItem.PeriodNo;
        //        }

        //        if ((e.LeftButton == MouseButtonState.Pressed && (Keyboard.IsKeyDown(Key.LeftAlt)))
        //            || (e.LeftButton == MouseButtonState.Pressed && Keyboard.IsKeyToggled(Key.RightAlt)))
        //        {
        //            if ((WeekDay != Constants.NullValue) && (PeriodNo != Constants.NullValue))
        //                ToggleLock(WeekDay, PeriodNo);
        //        }
        //        else if (e.LeftButton == MouseButtonState.Pressed && IsDeleteImage(e.OriginalSource))
        //        {
        //            Image CurrentImage = (e.OriginalSource as System.Windows.Controls.Image);

        //            string strImage = "" + CurrentImage.Source;

        //            if (strImage.Contains(strDeleteEventPath))
        //            {
        //                if (WeekDay <= 0 || PeriodNo <= 0) return;

        //                Cursor = Cursors.Wait;

        //                string idEvent = Unschedule(WeekDay, PeriodNo);

        //                if (!string.IsNullOrEmpty(idEvent))
        //                {
        //                    AddTestEvent(idEvent);
        //                    UpdateTestEventSolutionCount();
        //                }

        //                CEvent localEvent = schLocal.CEvents[idEvent];

        //                Cursor = Cursors.Arrow;

        //            }
        //        }
        //        else if (e.LeftButton == MouseButtonState.Pressed)
        //        {
        //            //若是星期或節次小於或等於0則不繼續執行
        //            if (WeekDay <= 0 || PeriodNo <= 0) return;

        //            string CellBackColor = GetCellBackColor((DependencyObject)e.OriginalSource);

        //            if (CellBackColor.Equals(lvSchedulableBackColor))
        //            {
        //                if (idTestEvents.Count > 0)
        //                {
        //                    Cursor = Cursors.Wait;

        //                    List<string> idAddEvents = new List<string>();
        //                    List<string> idEvents = new List<string>();

        //                    idTestEvents.ForEach(x => idEvents.Add(x));
        //                    idTestEvents.Clear();

        //                    foreach (string idEvent in idEvents)
        //                    {
        //                        if (IsRelatedEvent(idEvent))
        //                        {
        //                            if (IsScheduled(idEvent))
        //                                schLocal.FreeEvent(idEvent);

        //                            string nTimeTableID = GetTimeTableID(idEvent);

        //                            if (nTimeTableID == ttCur.TimeTableID)
        //                            {
        //                                //若拖到畫面上的nPeriodNo或nWeekDay為0則加入測試事件
        //                                if (PeriodNo == 0 || WeekDay == 0)
        //                                {
        //                                    AddTestEvent(idEvent);
        //                                }
        //                                else
        //                                {
        //                                    //實際安排事件
        //                                    bool localScheduled = schLocal.ScheduleEvent(idEvent, WeekDay, PeriodNo);

        //                                    //若無法安排事件，則發出Beep聲，並加入測試事件
        //                                    if (!localScheduled)
        //                                    {
        //                                        Beep(1, 1);
        //                                        idAddEvents.Add(idEvent);
        //                                    }
        //                                }
        //                            }
        //                            else
        //                                idAddEvents.Add(idEvent);
        //                        }
        //                    }

        //                    idTestEvent = string.Empty;

        //                    idAddEvents.ForEach(x => AddTestEvent(x));

        //                    UpdateTestEventSolutionCount();

        //                    Cursor = Cursors.Arrow;
        //                }
        //                //嘗試解析事件編號
        //                else if (!string.IsNullOrEmpty(idTestEvent))
        //                {
        //                    string idEvent = idTestEvent;

        //                    //判斷是否為相關事件或相同的時間表才繼續執行
        //                    if (IsRelatedEvent(idEvent))
        //                    {
        //                        if (IsScheduled(idEvent))
        //                            schLocal.FreeEvent(idEvent);

        //                        string nTimeTableID = GetTimeTableID(idEvent);

        //                        if (nTimeTableID == ttCur.TimeTableID)
        //                        {
        //                            Cursor = Cursors.Wait;

        //                            //若拖到畫面上的nPeriodNo或nWeekDay為0則加入測試事件
        //                            if (PeriodNo == 0 || WeekDay == 0)
        //                            {
        //                                AddTestEvent(idEvent);
        //                                UpdateTestEventSolutionCount();
        //                            }
        //                            else
        //                            {
        //                                idTestEvent = string.Empty;
        //                                //實際安排事件
        //                                bool localScheduled = schLocal.ScheduleEvent(idEvent, WeekDay, PeriodNo);

        //                                //若無法安排事件，則發出Beep聲，並加入測試事件
        //                                if (!localScheduled)
        //                                {
        //                                    Beep(1, 1);
        //                                    AddTestEvent(idEvent);
        //                                    UpdateTestEventSolutionCount();
        //                                }
        //                            }

        //                            Cursor = Cursors.Arrow;
        //                        }
        //                        else
        //                            AddTestEvent(idEvent);
        //                    }
        //                }
        //            }
        //            else if (CellBackColor.Equals(lvScheduledBackColor))
        //            {
        //                Cursor = Cursors.Wait;

        //                grdTestEvent.SelectedIndex = -1;

        //                string idEvent = GetEventID(WeekDay, PeriodNo);

        //                if (!string.IsNullOrEmpty(idEvent))
        //                {
        //                    CEvent evtTest = schLocal.CEvents[idEvent];

        //                    if (!string.IsNullOrEmpty(idEvent) && !evtTest.ManualLock)
        //                    {
        //                        SetTestEvent(idEvent);
        //                        UpdateContent();
        //                    };
        //                }

        //                Cursor = Cursors.Arrow;
        //            }
        //        }
        //    }
        //    catch (Exception ve)
        //    {
        //        FISCA.ErrorBox.Show("排入分課時發生錯誤！" + System.Environment.NewLine + ve.Message, ve);
        //    }
        //}

        ///// <summary>
        ///// 用滑鼠點再加Alt可以將事件Lock或是UnLock。
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void fgLPView_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    ScheduleModeAction(sender, e);
        //}

        ///// <summary>
        ///// DataGrid右鍵選單
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void fgLPView_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (!(Constants.ExecutionMode == ScheduleMode.modSchedule))
        //        return;

        //    Point MousePoint = DataGridHelper.GetMouseRowColumn(e.OriginalSource);

        //    if (((int)MousePoint.X != Constants.NullValue) && ((int)MousePoint.Y != Constants.NullValue))
        //    {
        //        {
        //            dynamic SelectedItem = fgLPView.Items[(int)MousePoint.Y];

        //            int WeekDay = (int)MousePoint.X;
        //            int PeriodNo = SelectedItem.PeriodNo;

        //            string CellBackColor = GetCellBackColor((DependencyObject)e.OriginalSource);

        //            //判斷儲存格的背景若為綠色才加入選單
        //            if (CellBackColor.Equals(lvScheduledBackColor))
        //            {
        //                ContextMenu menuLPView = new ContextMenu();

        //                menuLPView.SetResourceReference(ContextMenu.StyleProperty, "LPViewContextMenuStyle");

        //                MenuItem itemToggle = new MenuItem();
        //                itemToggle.Header = "鎖定/解鎖（Alt+Click）";
        //                itemToggle.SetResourceReference(ContextMenu.StyleProperty, "LPViewContextMenuItemStyle");
        //                itemToggle.Click += (vsender, ve) => ToggleLock(WeekDay, PeriodNo);
        //                menuLPView.Items.Add(itemToggle);

        //                MenuItem itemAddToTestEvent = new MenuItem();
        //                itemAddToTestEvent.Header = "重新安排分課（Click）";
        //                itemAddToTestEvent.SetResourceReference(ContextMenu.StyleProperty, "LPViewContextMenuItemStyle");
        //                itemAddToTestEvent.Click += (vsender, ve) =>
        //                {
        //                    if (WeekDay <= 0 || PeriodNo <= 0) return;

        //                    Cursor = Cursors.Wait;

        //                    string idEvent = Unschedule(WeekDay, PeriodNo);

        //                    if (!string.IsNullOrEmpty(idEvent))
        //                    {
        //                        AddTestEvent(idEvent);
        //                        UpdateTestEventSolutionCount();
        //                    }

        //                    Cursor = Cursors.Arrow;
        //                };
        //                menuLPView.Items.Add(itemAddToTestEvent);

        //                MenuItem itemWho = new MenuItem();
        //                itemWho.Header = "顯示/不顯示教師";
        //                itemWho.SetResourceReference(ContextMenu.StyleProperty, "LPViewContextMenuItemStyle");
        //                itemWho.Click += (vsender, ve) =>
        //                {
        //                    mOption.IsWho = !mOption.IsWho;
        //                    UpdateContent();
        //                };
        //                menuLPView.Items.Add(itemWho);

        //                MenuItem itemWhom = new MenuItem();
        //                itemWhom.Header = "顯示/不顯示班級";
        //                itemWhom.SetResourceReference(ContextMenu.StyleProperty, "LPViewContextMenuItemStyle");
        //                itemWhom.Click += (vsender, ve) =>
        //                {
        //                    mOption.IsWhom = !mOption.IsWhom;
        //                    UpdateContent();
        //                };
        //                menuLPView.Items.Add(itemWhom);

        //                MenuItem itemWhere = new MenuItem();
        //                itemWhere.Header = "顯示/不顯示場地";
        //                itemWhere.SetResourceReference(ContextMenu.StyleProperty, "LPViewContextMenuItemStyle");
        //                itemWhere.Click += (vsender, ve) =>
        //                {
        //                    mOption.IsWhere = !mOption.IsWhere;
        //                    UpdateContent();
        //                };
        //                menuLPView.Items.Add(itemWhere);

        //                MenuItem itemWhat = new MenuItem();
        //                itemWhat.Header = "顯示/不顯示科目";
        //                itemWhat.SetResourceReference(ContextMenu.StyleProperty, "LPViewContextMenuItemStyle");
        //                itemWhat.Click += (vsender, ve) =>
        //                {
        //                    mOption.IsWhat = !mOption.IsWhat;
        //                    UpdateContent();
        //                };
        //                menuLPView.Items.Add(itemWhat);

        //                MenuItem itemWhatAlias = new MenuItem();
        //                itemWhatAlias.Header = "顯示/不顯示簡稱";
        //                itemWhatAlias.SetResourceReference(ContextMenu.StyleProperty, "LPViewContextMenuItemStyle");
        //                itemWhatAlias.Click += (vsender, ve) =>
        //                {
        //                    mOption.IsWhatAlias = !mOption.IsWhatAlias;
        //                    UpdateContent();
        //                };
        //                menuLPView.Items.Add(itemWhatAlias);

        //                fgLPView.ContextMenu = menuLPView;
        //            }
        //            else
        //                fgLPView.ContextMenu = null;
        //        }
        //    }
        //}
        #endregion
    }
}