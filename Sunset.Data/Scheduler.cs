using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using FISCA.DSAClient;
using Sunset.Data.Integration;
using System.Diagnostics;

namespace Sunset.Data
{
    /// <summary>
    /// 排課主程式
    /// </summary>
    public class Scheduler
    {
        #region Event

        #region AutoScheduleStart
        public class AutoScheduleStartEventArgs : EventArgs
        {
            public int nTotCount { get; set; }

            public AutoScheduleStartEventArgs(int nTotCount)
            {
                this.nTotCount = nTotCount;
            }
        }

        public event EventHandler<AutoScheduleStartEventArgs> AutoScheduleStart;
        #endregion    

        #region AutoScheduleProgress
        public class AutoScheduleProgressEventArgs : EventArgs
        {
            public int nCurIndex { get; set; }
            public bool Cancel { get; set; }

            public AutoScheduleProgressEventArgs(int nCurIndex,bool Cancel)
            {
                this.nCurIndex = nCurIndex;
                this.Cancel = Cancel;
            }
        }

        public event EventHandler<AutoScheduleProgressEventArgs> AutoScheduleProgress;
        #endregion

        #region AutoScheduleComplete
        public class AutoScheduleCompleteEventArgs : EventArgs
        {
            public CEvents EventList { get; set; }
            public int BottomIndex { get; set; }

            public AutoScheduleCompleteEventArgs(CEvents EventList, int BottomIndex)
            {
                this.EventList = EventList;
                this.BottomIndex = BottomIndex;
            }
        }

        public event EventHandler<AutoScheduleCompleteEventArgs> AutoScheduleComplete;
        #endregion

        #region EventSolCountUpdated
        public class EventSolCountUpdatedEventArgs : EventArgs
        {
            public CEvents Eventlist { get; set; }

            public EventSolCountUpdatedEventArgs(CEvents Eventlist)
            {
                this.Eventlist = Eventlist;
            }
        }

        public event EventHandler<EventSolCountUpdatedEventArgs> EventSolCountUpdated;
        #endregion

        #region EventScheduled

        public class EventScheduledEventArgs : EventArgs     
        {
            public string EventID { get; set;}

            public EventScheduledEventArgs(string EventID)
            {
                this.EventID = EventID;
            }
        }

        public event EventHandler<EventScheduledEventArgs> EventScheduled;
        #endregion

        #region EventsFreed
        public class EventsFreedEventArgs : EventArgs
        {
            public CEvents EventList { get; set;}

            public EventsFreedEventArgs(CEvents EventList)
            {
                this.EventList = EventList;
            }
        }

        public event EventHandler<EventsFreedEventArgs> EventsFreed;
        #endregion

        #region EventLocked
        public class EventLockedEventArgs : EventArgs
        {
            public List<string> EventIDs { get; set;}

            public EventLockedEventArgs(List<string> EventIDs)
            {
                this.EventIDs = EventIDs;
            }
        }

        public event EventHandler<EventLockedEventArgs> EventLocked;
        #endregion

        #region EventUnlocked
        public class EventUnlockedEventArgs : EventArgs
        {
            public List<string> EventIDs {get; set;}

            public EventUnlockedEventArgs(List<string> EventIDs)
            {
                this.EventIDs = EventIDs;
            }
        }

        public event EventHandler<EventUnlockedEventArgs> EventUnlocked;
        #endregion

        #region EventPropertyBeforeChange
        public class EventPropertyBeforeChangeEventArgs : EventArgs
        {
            public string EventID{ get; set;}

            public MaskOptions ChangeFlag { get; set;}

            public EventPropertyBeforeChangeEventArgs(string EventID,MaskOptions ChangeFlag)
            {
                this.EventID = EventID;
                this.ChangeFlag = ChangeFlag;
            }
        }

        public event EventHandler<EventPropertyBeforeChangeEventArgs> EventPropertyBeforeChange;
        #endregion

        #region EventPropertyChanged
        public class EventPropertyChangedEventArgs : EventArgs
        {
            public string EventID { get; set;}

            public MaskOptions ChangeFlag { get; set;}

            public EventPropertyChangedEventArgs(string EventID, MaskOptions ChangeFlag)
            {
                this.EventID = EventID;
                this.ChangeFlag = ChangeFlag;
            }
        }

        public event EventHandler<EventPropertyChangedEventArgs> EventPropertyChanged;
        #endregion

        #region EventInserted
        public class EventInsertedEventArgs : EventArgs
        {
            public string EventID { get; set;}

            public EventInsertedEventArgs(string EventID)
            {
                this.EventID = EventID;
            }
        }

        public event EventHandler<EventInsertedEventArgs> EventInserted;
        #endregion

        #region EventBeforeDelete
        public class EventBeforeDeleteEventArgs : EventArgs
        {
            public string EventID { get; set; }

            public EventBeforeDeleteEventArgs(string EventID)
            {
                this.EventID = EventID;
            }
        }

        public event EventHandler<EventBeforeDeleteEventArgs> EventBeforeDelete;
        #endregion

        #region EventDeleted
        public class EventDeletedEventArgs : EventArgs
        {
            public string EventID { get; set; }

            public EventDeletedEventArgs(string EventID)
            {
                this.EventID = EventID;
            }
        }

        public event EventHandler<EventDeletedEventArgs> EventDeleted;
        #endregion

        #region WhoBusyConflict
        public class WhoBusyConflictEventArgs : EventArgs
        {
            public string WhoID { get; set; }

            public WhoBusyConflictEventArgs(string WhoID)
            {
                this.WhoID = WhoID;
            }
        }

        public event EventHandler<WhoBusyConflictEventArgs> WhoBusyConflict;
        #endregion

        #region WhereBusyConflict
        public class WhereBusyConflictEventArgs : EventArgs
        {
            public string WhereID { get; set; }

            public WhereBusyConflictEventArgs(string WhereID)
            {
                this.WhereID = WhereID;
            }
        }

        public event EventHandler<WhereBusyConflictEventArgs> WhereBusyConflict;
        #endregion

        #region WhomBusyConflict
        public class WhomBusyConflictEventArgs : EventArgs
        {
            public string WhomID { get; set; }

            public WhomBusyConflictEventArgs(string WhomID)
            {
                this.WhomID = WhomID;
            }
        }

        public event EventHandler<WhomBusyConflictEventArgs> WhomBusyConflict;
        #endregion

        #region WhereBusyInvalid
        public class WhereBusyInvalidEventArgs : EventArgs
        {
            public string WhereID { get; set; }

            public WhereBusyInvalidEventArgs(string WhereID)
            {
                this.WhereID = WhereID;
            }
        }

        public event EventHandler<WhereBusyInvalidEventArgs> WhereBusyInvalid;
        #endregion

        #region EventLoadConflict
        public class EventLoadConflictEventArgs : EventArgs
        {
            public string EventID { get; set;}

            public EventLoadConflictEventArgs(string EventID)
            {
                this.EventID = EventID;
            }
        }

        public event EventHandler<EventLoadConflictEventArgs> EventLoadConflict;
        #endregion

        #region SaveSourceStart
        /// <summary>
        /// 儲存資料來源事件參數
        /// </summary>
        public class SaveSourceStartEventArgs : EventArgs
        {
            public int TotItem { get; set; }

            public SaveSourceStartEventArgs(int TotItem)
            {
                this.TotItem = TotItem;
            }
        }

        /// <summary>
        /// 儲存資料來源開始
        /// </summary>
        public event EventHandler<SaveSourceStartEventArgs> SaveSourceStart;
        #endregion

        #region SaveSourceProgress
        /// <summary>
        /// 儲存資料來源進度
        /// </summary>
        public class SaveSourceProgressEventArgs : EventArgs
        {
            /// <summary>
            /// 進度
            /// </summary>
            public int Progress { get; set; }

            /// <summary>
            /// 建構式，傳入進度
            /// </summary>
            /// <param name="Progress"></param>
            public SaveSourceProgressEventArgs(int Progress)
            {
                this.Progress = Progress;
            }
        }

        /// <summary>
        /// 儲存資料來源
        /// </summary>
        public event EventHandler<SaveSourceProgressEventArgs> SaveSourceProgress;
        #endregion

        #region SaveSourceComplete
        /// <summary>
        /// 儲存資料來源完成
        /// </summary>
        public event EventHandler SaveSourceComplete;
        #endregion

        #region DownloadSource
        /// <summary>
        /// 下載資料開始事件參數
        /// </summary>
        public class DownloadSourceStartEventArgs : EventArgs
        {
            /// <summary>
            /// 所有資料項目數量
            /// </summary>
            public int TotItem { get; set; }

            /// <summary>
            /// 建構式，傳入所有項目數量
            /// </summary>
            /// <param name="TotItem"></param>
            public DownloadSourceStartEventArgs(int TotItem)
            {
                this.TotItem = TotItem;
            }
        }

        /// <summary>
        /// 下載資料開始事件
        /// </summary>
        public event EventHandler<DownloadSourceStartEventArgs> DownloadSourceStart;
        #endregion

        #region DownloadSourceProgress
        /// <summary>
        /// 下載資料進度事件參數
        /// </summary>
        public class DownloadSourceProgressEventArgs : EventArgs
        {
            /// <summary>
            /// 進度
            /// </summary>
            public int Progress { get; set; }

            /// <summary>
            /// 建構式，傳入進度
            /// </summary>
            /// <param name="Progress"></param>
            public DownloadSourceProgressEventArgs(int Progress)
            {
                this.Progress = Progress;
            }
        }

        /// <summary>
        /// 下載資料進度
        /// </summary>
        public event EventHandler<DownloadSourceProgressEventArgs> DownloadSourceProgress;
        #endregion

        #region DownloadSourceComplete
        /// <summary>
        /// 下載資料來源完成
        /// </summary>
        public event EventHandler DownloadSourceComplete;
        #endregion

        #region ImportSourceStart
        /// <summary>
        /// 匯入資料開始事件參數
        /// </summary>
        public class ImportSourceStartEventArgs : EventArgs
        {
            /// <summary>
            /// 所有項目數量
            /// </summary>
            public int TotItem { get; set; }

            /// <summary>
            /// 建構式，傳入項目數量
            /// </summary>
            /// <param name="TotItem"></param>
            public ImportSourceStartEventArgs(int TotItem)
            {
                this.TotItem = TotItem;
            }
        }

        /// <summary>
        /// 匯入資料開始事件
        /// </summary>
        public event EventHandler<ImportSourceStartEventArgs> ImportSourceStart;
        #endregion

        #region ImportSourceProgress
        /// <summary>
        /// 匯入資料進度事件參數
        /// </summary>
        public class ImportSourceProgressEventArgs : EventArgs
        {
            /// <summary>
            /// 進度
            /// </summary>
            public int Progress { get; set; }

            /// <summary>
            /// 建構式，傳入進度
            /// </summary>
            /// <param name="Progress"></param>
            public ImportSourceProgressEventArgs(int Progress)
            {
                this.Progress = Progress;
            }
        }

        /// <summary>
        /// 匯入資料進度事件
        /// </summary>
        public event EventHandler<ImportSourceProgressEventArgs> ImportSourceProgress;
        #endregion

        #region ImportSourceComplete
        /// <summary>
        /// 匯入資料進度事件參數
        /// </summary>
        public class ImportSourceCompleteEventArgs : EventArgs
        {
            /// <summary>
            /// 進度
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// 建構式，傳入進度
            /// </summary>
            /// <param name="Progress"></param>
            public ImportSourceCompleteEventArgs(string Message)
            {
                this.Message = Message;
            }
        }

        /// <summary>
        /// 匯入資料完成事件
        /// </summary>
        public event EventHandler<ImportSourceCompleteEventArgs> ImportSourceComplete;
        #endregion

        #region UploadSourceStart
        /// <summary>
        /// 上傳資料開始事件參數
        /// </summary>
        public class UploadSourceStartEventArgs : EventArgs
        {
            /// <summary>
            /// 所有項目數量
            /// </summary>
            public int TotItem { get; set; }

            /// <summary>
            /// 建構式，傳入項目數量
            /// </summary>
            /// <param name="TotItem"></param>
            public UploadSourceStartEventArgs(int TotItem)
            {
                this.TotItem = TotItem;
            }
        }

        /// <summary>
        /// 上傳資料開始事件
        /// </summary>
        public event EventHandler<UploadSourceStartEventArgs> UploadSourceStart;
        #endregion

        #region UploadSourceProgress
        /// <summary>
        /// 上傳資料進度事件參數
        /// </summary>
        public class UploadSourceProgressEventArgs : EventArgs
        {
            /// <summary>
            /// 進度
            /// </summary>
            public int Progress { get; set; }

            /// <summary>
            /// 訊息
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// 建構式，傳入進度
            /// </summary>
            /// <param name="Progress"></param>
            public UploadSourceProgressEventArgs(int Progress,string Message)
            {
                this.Progress = Progress;
                this.Message = Message;
            }
        }

        /// <summary>
        /// 上傳資料進度事件
        /// </summary>
        public event EventHandler<UploadSourceProgressEventArgs> UploadSourceProgress;
        #endregion

        #region UploadSourceComplete
        /// <summary>
        /// 上傳資料進度事件參數
        /// </summary>
        public class UploadSourceCompleteEventArgs : EventArgs
        {
            /// <summary>
            /// 是否上傳成功
            /// </summary>
            public bool IsSuccess { get; set; }

            /// <summary>
            /// 成功或失敗訊息
            /// </summary>
            public string Message { get; set;}

            /// <summary>
            /// 建構式
            /// </summary>
            /// <param name="Success">是否上傳成功</param>
            /// <param name="Message">成功或失敗訊息</param>
            public UploadSourceCompleteEventArgs(bool IsSuccess, string Message)
            {
                this.IsSuccess = IsSuccess;
                this.Message = Message;
            }
        }

        /// <summary>
        /// 匯入資料完成事件
        /// </summary>
        public event EventHandler<UploadSourceCompleteEventArgs> UploadSourceComplete;
        #endregion
        #endregion

        #region Private
        //constants
        private const int AutoScheduleNotifyThreshold = 5;
        
        //local variables for private use
        //private OleDbConnection cnMain;  //reference to current database
        private SchedulerSource schSource = SchedulerSource.Source;
        private long idNext; //store the next available ID
        private static Scheduler mScheduler = null;
        private string mSaveFilePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\scheduler.xml";
        private string mSavePassword = string.Empty;

        //shared variables for TestSchedule and AllocEvent
        private CEvent evtTested;
        private List<GroupCEvent> evtTesteds;
        private int nWhoAvailable;
        private int nWhereAvailable;
        private Classroom whrTest;
        private Teacher whoTest1;
        private Teacher whoTest2;
        private Teacher whoTest3;
        private Class whmTest;
        private Periods prdsUse;

        //調代課
        private List<CEventUpdate> mEventUpdateList = new List<CEventUpdate>();
        public List<CEventUpdate> EventUpdateList {  get { return mEventUpdateList; }  set { mEventUpdateList = value; }}
        public DateTime StartWeekDate { get; private set; } //週開始日期
        public DateTime EndWeekDate { get; private set; }   //週結束日期
        #endregion

        #region Properties

        public static Scheduler Instance
        {
            get
            {
                if (mScheduler == null)
                    mScheduler = new Scheduler();

                return mScheduler;
            }
        }

        /// <summary>
        /// 星期變數
        /// </summary>
        public Variables WeekDayVariables { get; set; }

        /// <summary>
        /// 節次變數
        /// </summary>
        public Variables PeriodVariables { get; set; }

        /// <summary>
        /// 地點集合
        /// </summary>
        public Locations Locations { get; set; }

        /// <summary>
        /// 距離集合
        /// </summary>
        public Distances Distances { get; set; }

        /// <summary>
        /// 時間表集合
        /// </summary>
        public TimeTables TimeTables { get; set; }

        /// <summary>
        /// 科目集合
        /// </summary>
        public Subjects Subjects { get; set; }

        /// <summary>
        /// 班級集合
        /// </summary>
        public Classes Classes { get; set; }

        /// <summary>
        /// 場地集合
        /// </summary>
        public Classrooms Classrooms { get; set; }

        /// <summary>
        /// 教師集合
        /// </summary>
        public Teachers Teachers { get; set; }

        /// <summary>
        /// 事件集合
        /// </summary>
        public CEvents CEvents { get; set; }

        /// <summary>
        /// 事由
        /// </summary>
        public int Reason { get; set; }

        /// <summary>
        /// 無法排課原因
        /// </summary>
        public class ReasonDescription
        {
            /// <summary>
            /// 資源名稱
            /// </summary>
            public string AssocName { get; set; }

            /// <summary>
            /// 資源類別
            /// </summary>
            public int AssocType { get; set; }

            /// <summary>
            /// 資源系統編號
            /// </summary>
            public string AssocID { get; set; }

            /// <summary>
            /// 描述
            /// </summary>
            public string Desc { get; set; }
        }


        /// <summary>
        /// 事由詳細說明
        /// </summary>
        public ReasonDescription ReasonDesc { get; set; }

        /// <summary>
        /// 學年度
        /// </summary>
        public string SchoolYear { get; set; }

        /// <summary>
        /// 學期
        /// </summary>
        public string Semester { get; set; }

        /// <summary>
        /// 資料庫是否開啟
        /// </summary>
        public bool IsOpen { get; private set;}
        #endregion

        #region Constructor
        /// <summary>
        /// 建構式
        /// </summary>
        public Scheduler()
        {
            IsOpen = false;
            idNext = 1;
        }
        #endregion

        internal class GroupCEvent
        {
            public CEvent evtTested { get; set;}
            public Classroom whrTest { get; set;}
            public Teacher whoTest1 { get; set;}
            public Teacher whoTest2 { get; set; }
            public Teacher whoTest3 { get; set; }
            public Class whmTest { get; set;}
            public Periods prdsUse { get; set;}
        }

        #region Public functions
        /// <summary>
        /// 從多個資料來源下載資料
        /// </summary>
        /// <param name="Connections">多個資料來源</param>
        /// <param name="SchoolYear">學年度</param>
        /// <param name="Semester">學期</param>
        /// <returns>是否下載成功</returns>
        public bool Download(List<Connection> Connections, string SchoolYear, string Semester)
        {
            bool IsSuccess = false;

            if (schSource.IsSuccess)
                throw new Exception("Scheduler source already opened");
            try
            {
                if (DownloadSourceStart != null)
                    DownloadSourceStart(this, new DownloadSourceStartEventArgs(100));

                    IsSuccess = schSource.Download(Connections, SchoolYear, Semester, 
                    x =>
                    {
                        if (DownloadSourceProgress != null)
                            DownloadSourceProgress(this, new DownloadSourceProgressEventArgs(x)); 
                    });

                if (DownloadSourceComplete != null)
                    DownloadSourceComplete(this, null);
            }
            catch (Exception e)
            {
                throw e;
            }

            return IsSuccess;
        }

        /// <summary>
        /// 將CourseSection物件轉為CEvent物件
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private CEvent SCourseSectionToCEvent(SCourseSection x)
        {
            CEvent evtNew = new CEvent();

            evtNew.EventID = x.ID;
            evtNew.TeacherID1 = x.TeacherName1;
            evtNew.TeacherID2 = x.TeacherName2;
            evtNew.TeacherID3 = x.TeacherName3;
            evtNew.ClassroomID = x.ClassroomID;
            evtNew.ClassID = x.ClassID;
            evtNew.SubjectID = x.Subject;
            evtNew.SubjectAlias = x.SubjectAlias;
            evtNew.CourseName = x.CourseName;
            evtNew.Length = x.Length;
            evtNew.WeekFlag = x.WeekFlag;
            evtNew.Priority = 1;
            evtNew.WeekDayCondition = x.WeekdayCond;
            evtNew.PeriodCondition = x.PeriodCond;
            evtNew.ManualLock = x.Lock;
            evtNew.AllowLongBreak = x.Longbreak;
            evtNew.AllowDuplicate = x.AllowDup;
            evtNew.CourseGroup = x.CourseGroup;
            evtNew.LimitNextDay = x.LimitNextDay;
            evtNew.Comment = x.Comment;

            evtNew.WeekDay = 0;
            evtNew.PeriodNo = 0;

            evtNew.SolutionCount = -1;

            evtNew.CourseID = x.CourseID;
            evtNew.TimeTableID = x.TimeTableID;

            #region Check ID validility
            if (!Teachers.Exists(evtNew.TeacherID1)) evtNew.TeacherID1 = Constants.NullString;
            if (!Teachers.Exists(evtNew.TeacherID2)) evtNew.TeacherID2 = Constants.NullString;
            if (!Teachers.Exists(evtNew.TeacherID3)) evtNew.TeacherID3 = Constants.NullString;

            if (!Classes.Exists(evtNew.ClassID)) evtNew.ClassID = Constants.NullString;
            if (!Classrooms.Exists(evtNew.ClassroomID)) evtNew.ClassroomID = Constants.NullString;
            if (!Subjects.Exists(evtNew.SubjectID)) evtNew.SubjectID = Constants.NullString;
            #endregion

            return evtNew;
        }

        /// <summary>
        /// 從Sunset.Data.Integration匯入排課資料
        /// </summary>
        /// <param name="Connections">多個資料來源</param>
        /// <param name="SchoolYear">學年度</param>
        /// <param name="Semester">學期</param>
        public void Import()
        {
            #region 宣告變數
            Teacher whoBusy;
            Classroom whrBusy;
            Class whmBusy;
            int nIndex;
            int nTestWeekDay;
            int nTestPeriod;
            int nProgress = 0; ;
            int nTotRec;
            StringBuilder strEvtConflict = new StringBuilder();
            #endregion

            #region 建立資料庫連線
            if (!schSource.IsSuccess)
                throw new Exception("Scheduler source not open");
            #endregion

            #region 初始化資料
            idNext = 1;
            CreateChildObjects(); //不懂為何要CreateChildObjects();
            #endregion

            #region 載入地點
            schSource.LocationResult.Data.ForEach
            (x=>
                {
                    Location locNew = new Location(x.ID, x.LocationName);
                    Locations.Add(locNew);
                }
            );
            #endregion

            #region 載入時間表（TimeTable）
            schSource.TimeTableResult.Data.ForEach
            (x=>
                {
                    TimeTable ttNew = new TimeTable(x.ID,x.TimeTableName);
                    TimeTables.Add(ttNew);
                }
            );
            #endregion

            #region 載入節次對照
            schSource.TimeTableSecsResult.Data.ForEach
            (x =>
                {
                    Period prdNew = new Period(
                        x.WeekDay,
                        x.PeriodNo,
                        x.DispPeriod,
                        x.Begintime,
                        x.Duration,
                        x.LocationID,
                        x.Disable,
                        x.DisableMessage
                    );

                    if (TimeTables.Exists(x.TimeTableID))
                        TimeTables[x.TimeTableID].Periods.Add(prdNew);
                }
            );
            #endregion

            #region 載入教師
            schSource.TeacherResult.Data.ForEach
            (x =>
                {
                    Teacher whoNew = new Teacher(x.ID, x.Name,1,x.BasicLength,x.ExtraLength,x.CounselingLength,x.Comment);
                    x.SourceIDs.ForEach(y => whoNew.SourceIDs.Add(y));
                    Teachers.Add(whoNew);
                }
            );
            #endregion

            #region 載入場地
            schSource.ClassroomResult.Data.ForEach
            (x =>
                {
                    Classroom whrNew = new Classroom(
                        x.ID,
                        x.ClassroomName,
                        x.Capacity,
                        x.LocationID,
                        x.LocationOnly
                        );
                    x.SourceIDs.ForEach(y => whrNew.SourceIDs.Add(y));
                    Classrooms.Add(whrNew);
                }
            );
            #endregion

            #region 載入班級
            schSource.ClassResult.Data.ForEach
            (x =>
                {
                    Class whmNew = new Class(x.ID, x.ClassName, x.TimeTableID,x.TeacherName,x.GradeYear,x.NamingRule);
                    Classes.Add(whmNew);
                }
            );
            #endregion

            #region 載入科目
            schSource.Subjects.ForEach(x=>Subjects.Add(new Subject(x,x)));
            #endregion

            #region 載入教師不排課時段
            schSource.TeacherBusysResult.Data.ForEach
            (x =>
                {
                    if (Teachers.Exists(x.TeacherID))
                    {
                        Appointment appNew = new Appointment(x.WeekDay, x.BeginTime, x.Duration, 3, string.Empty, x.LocationID, string.Empty,x.Description);

                        whoBusy = Teachers[x.TeacherID];

                        nIndex = 0;

                        #region 檢查教師是否有空閒時間
                        while (nIndex < whoBusy.Capacity)
                        {
                            //切換行事曆
                            whoBusy.UseAppointments(nIndex);

                            //檢查空閒時間
                            if (whoBusy.Appointments.IsFreeTime(appNew.WeekDay, appNew.BeginTime, appNew.Duration, appNew.WeekFlag))
                            {
                                //若有空閒時間則加入約會
                                whoBusy.Appointments.Add(appNew);
                                break;
                            }
                            nIndex++;
                        }

                        //檢查各個行事曆，若是都沒有空閒時間則發出事件
                        if (nIndex >= whoBusy.Capacity)
                            if (WhoBusyConflict != null)
                                WhoBusyConflict(this, new WhoBusyConflictEventArgs(whoBusy.TeacherID));
                        #endregion
                    }
                }
            );
            #endregion

            #region 載入場地不排課時段（Classroom Busy）
            schSource.ClassroomBusysResult.Data.ForEach
            (x =>
                {
                    if (Classrooms.Exists(x.ClassroomID))
                    {
                        Appointment appNew = new Appointment(x.WeekDay, x.BeginTime, x.Duration, x.WeekFlag, string.Empty, string.Empty, string.Empty,x.Description);

                        whrBusy = Classrooms[x.ClassroomID];

                        #region 若是場地無使用限制，則不能有ClassroomBusy
                        if (whrBusy.LocOnly)
                        {
                            //場地無行事曆限制，發出WhereBusyInvalid事件
                            if (WhereBusyInvalid != null)
                                WhereBusyInvalid(this, new WhereBusyInvalidEventArgs(whrBusy.ClassroomID));
                        }
                        else
                        {
                            nIndex = 0;

                            while (nIndex < whrBusy.Capacity)
                            {
                                //切換行事曆
                                whrBusy.UseAppointments(nIndex);

                                //檢查空閒時間
                                if (whrBusy.Appointments.IsFreeTime(appNew.WeekDay, appNew.BeginTime, appNew.Duration, appNew.WeekFlag))
                                {
                                    //若有空閒時間則加入約會
                                    whrBusy.Appointments.Add(appNew);
                                    break;
                                }
                                nIndex++;
                            }

                            //檢查各個行事曆，若是都沒有空閒時間則發出事件
                            if (nIndex >= whrBusy.Capacity)
                                if (WhereBusyConflict != null)
                                    WhereBusyConflict(this, new WhereBusyConflictEventArgs(whrBusy.ClassroomID));
                        }
                        #endregion
                    }
                }
            );
            #endregion

            #region 載入班級不排課時段（Class Busy）
            schSource.ClassBusysResult.Data.ForEach
            (x =>
            {
                if (Classes.Exists(x.ClassID))
                {                 
                    Appointment appNew = new Appointment(
                        x.WeekDay, 
                        x.BeginTime,
                        x.Duration, 
                        3, 
                        string.Empty, 
                        string.Empty, 
                        string.Empty, 
                        x.Description);

                    whmBusy = Classes[x.ClassID];

                    nIndex = 0;

                    //檢查空閒時間
                    if (whmBusy.Appointments.IsFreeTime(
                        appNew.WeekDay, 
                        appNew.BeginTime, 
                        appNew.Duration, 
                        appNew.WeekFlag))
                    {
                        //若有空閒時間則加入約會
                        whmBusy.Appointments.Add(appNew);
                    }
                    else
                    {
                        if (WhomBusyConflict != null)
                            WhomBusyConflict(this, new WhomBusyConflictEventArgs(whmBusy.ClassID));
                    }
                }
            }
            );
            #endregion

            #region 載入課程分段（CEvent）

            nTotRec = schSource.CourseSectionResult.Data.Count;

            idNext = 1;

            if (ImportSourceStart != null)
                ImportSourceStart(this, new ImportSourceStartEventArgs(nTotRec));

            //在匯入課程分段的課程中，可能會有群組課程，需要先行轉換的情況
            List<SCourseSection> AllocGroupSections = new List<SCourseSection>();

            schSource.CourseSectionResult.Data.ForEach
            (x =>
               {
                   //是否不在已安排的群組課程分段中
                   if (!AllocGroupSections.Contains(x))
                   {
                       CEvent evtNew = SCourseSectionToCEvent(x);

                       //設定nTestWeekDay及nTestPeriod，用來測試可否排定Scheduler
                       nTestWeekDay = x.WeekDay;
                       nTestPeriod = x.PeriodNo;
                       //Validate weekday and period
                       if (nTestWeekDay == 0) nTestPeriod = 0;
                       if (nTestPeriod == 0) nTestWeekDay = 0;

                       int WeekDayVar;
                       int PeriodVar;

                       //Fill weekday and period if the condition is set
                       //若有指定星期及節次，則直接改為指定的星期及節次
                       if ((evtNew.WeekDayOp == Constants.opEqual) &&
                           (evtNew.PeriodOp == Constants.opEqual) &&
                           (int.TryParse(evtNew.WeekDayVar, out WeekDayVar) &&
                           int.TryParse(evtNew.PeriodVar, out PeriodVar)))
                       {
                           nTestWeekDay = WeekDayVar;
                           nTestPeriod = PeriodVar;
                           evtNew.ManualLock = true;
                       }

                       idNext = GetNext(idNext, x.ID);

                       if (!evtNew.TimeTableID.IsNullValue())
                       {
                           if (nTestWeekDay != 0)
                           {
                               #region 判斷是否為群組課程做特殊處理
                               if (!string.IsNullOrEmpty(x.CourseGroup))
                               {
                                   #region 針對群組課程先行轉換
                                   //尋找對應的群組課程，不含自己
                                   List<SCourseSection> Sections = schSource.CourseSectionResult.Data
                                       .FindAll(y=>y.CourseGroup.Equals(x.CourseGroup) && !y.ID.Equals(x.ID));

                                   List<CEvent> evtsGroup = new List<CEvent>();

                                   //針對群組課程的課課程分段
                                   foreach (SCourseSection Section in Sections)
                                   {
                                       //轉換為內部實際使用的課程分段
                                       CEvent evtGroup = SCourseSectionToCEvent(Section);

                                       //加入到已安排的課程分段
                                       AllocGroupSections.Add(Section);

                                       //加入到事件列表中
                                       CEvents.Add(evtGroup);

                                       //加入到群組課程中，等下實際安排
                                       evtsGroup.Add(evtGroup);

                                       //增加時數
                                       IncTotalHour(evtGroup);
                                   }
                                   #endregion

                                   //測試可否排定事件，將evtNew安排在nTestWeekDay及nTestPeriod
                                    Reason = TestSchedule(evtNew, nTestWeekDay, nTestPeriod, true);

                                    //假設傳回0則實際安排事件
                                    if (Reason == 0)
                                    {
                                        AllocEvent(true);
                                    }
                                    else
                                    {
                                        strEvtConflict.AppendLine(
                                            "課程名稱：" + evtNew.CourseName +
                                            ",星期：" + evtNew.WeekDay +
                                            ",節次：" + evtNew.PeriodNo + ",衝突原因：(" + ReasonDesc.AssocType + ")" + ReasonDesc.AssocName + "," + ReasonDesc.Desc);

                                        if (EventLoadConflict != null)
                                            EventLoadConflict(this, new EventLoadConflictEventArgs(evtNew.EventID));
                                    }

                                    foreach (CEvent evtGroup in evtsGroup)
                                        if (evtNew.WeekDay != 0)
                                            IncAllocHour(evtNew);
                               }
                               #endregion
                               else
                               {
                                   //測試可否排定事件，將evtNew安排在nTestWeekDay及nTestPeriod
                                   Reason = TestSchedule(evtNew, nTestWeekDay, nTestPeriod, false);

                                   //假設傳回0則實際安排事件
                                   if (Reason == 0)
                                       AllocEvent(false);
                                   else
                                   {
                                       strEvtConflict.AppendLine(
                                           "課程名稱：" + evtNew.CourseName +
                                           ",星期：" + evtNew.WeekDay +
                                           ",節次：" + evtNew.PeriodNo + ",衝突原因：(" + ReasonDesc.AssocType + ")" + ReasonDesc.AssocName + "," + ReasonDesc.Desc);

                                       if (EventLoadConflict != null)
                                           EventLoadConflict(this, new EventLoadConflictEventArgs(evtNew.EventID));
                                   }
                               }
                           }

                           CEvents.Add(evtNew);

                           //Calculate WHOs,WHOMs and WHEREs TotalHour and AllocHour
                           IncTotalHour(evtNew);

                           if (evtNew.WeekDay != 0)
                               IncAllocHour(evtNew);

                           //Update progress indicator
                           nProgress++;

                           if (ImportSourceProgress != null)
                               ImportSourceProgress(this, new ImportSourceProgressEventArgs(nProgress));
                       }
                   }
               }
            );
            #endregion

            #region 載入學年度及學期（SchoolYear、Semester）
            SchoolYear = SchedulerSource.Source.SchoolYear;
            Semester = SchedulerSource.Source.Semester;

            if (ImportSourceComplete != null)
                ImportSourceComplete(this, new ImportSourceCompleteEventArgs(strEvtConflict.ToString()));
            #endregion

            GetSolutionCounts(CEvents);

            IsOpen = true;
        }

        private long GetNext(long idNext,string EventID)
        {
            string[] strEventIDs = EventID.Split(new char[] { ',' });

            if (strEventIDs.Length == 2)
            {
                int iEventID;

                if (int.TryParse(strEventIDs[1], out iEventID))
                    if (iEventID >= idNext)
                        return iEventID + 1;
            }

            return idNext;
        }

        /// <summary>
        /// 上傳排課資料
        /// </summary>
        public Tuple<bool, string> Upload(List<Connection> Connections)
        {
            if (!SchedulerSource.Source.IsSuccess && !IsOpen)
                throw new Exception("scheduler source is not open!");

            if (UploadSourceStart != null)
                UploadSourceStart(this, new UploadSourceStartEventArgs(CEvents.Count));

            List<SCourseSection> CourseSections = GetSCourseSection();

            #region 轉換教師不排課時段
            List<STeacherBusy> TeacherBusys = new List<STeacherBusy>();

            foreach (Teacher vTeacher in Teachers)
            {
                foreach (Appointment vApp in vTeacher.GetAppointments())
                {
                    if (string.IsNullOrEmpty(vApp.EventID))
                    {
                        foreach (SourceID vSourceID in vTeacher.SourceIDs)
                        {
                            string TeacherID = vSourceID.ID;
                            string DSNS = vSourceID.DSNS;

                            STeacherBusy TeacherBusy = new STeacherBusy();

                            TeacherBusy.DSNS = DSNS;
                            TeacherBusy.TeacherID = vSourceID.ID;

                            TeacherBusy.WeekDay = vApp.WeekDay;
                            TeacherBusy.BeginTime = vApp.BeginTime;
                            TeacherBusy.Duration = vApp.Duration;
                            TeacherBusy.Description = vApp.Description;
                            TeacherBusy.LocationID = vApp.LocID;

                            TeacherBusys.Add(TeacherBusy);
                        }
                    }
                }
            }
            #endregion


            #region 轉換班級不排課時段
            List<SClassBusy> ClassBusys = new List<SClassBusy>();

            foreach (Class vClass in Classes)
            {
                if (string.IsNullOrEmpty(vClass.ClassID))
                    continue;

                string[] ClassIDs = vClass.ClassID.Split(new char[] { ',' });
                string DSNS = ClassIDs[0];
                string ClassID = ClassIDs[1];

                foreach (Appointment vApp in vClass.GetAppointments())
                {
                    if (string.IsNullOrEmpty(vApp.EventID))
                    {
                        SClassBusy ClassBusy = new SClassBusy();

                        ClassBusy.DSNS = DSNS;
                        ClassBusy.ClassID = ClassID;

                        ClassBusy.WeekDay = vApp.WeekDay;
                        ClassBusy.BeginTime = vApp.BeginTime;
                        ClassBusy.Duration = vApp.Duration;
                        ClassBusy.Description = vApp.Description;

                        ClassBusys.Add(ClassBusy);
                    }
                }
            }
            #endregion

            #region 轉換場地不排課時段
            List<SClassroomBusy> ClassroomBusys = new List<SClassroomBusy>();

            foreach (Classroom vClassroom in Classrooms)
            {
                foreach (Appointment vApp in vClassroom.GetAppointments())
                {
                    if (string.IsNullOrEmpty(vApp.EventID))
                    {
                        foreach (SourceID vSourceID in vClassroom.SourceIDs)
                        {
                            string ClassroomID = vSourceID.ID;
                            string DSNS = vSourceID.DSNS;

                            SClassroomBusy ClassroomBusy = new SClassroomBusy();

                            ClassroomBusy.DSNS = DSNS;
                            ClassroomBusy.ClassroomID = ClassroomID;

                            ClassroomBusy.WeekDay = vApp.WeekDay;
                            ClassroomBusy.BeginTime = vApp.BeginTime;
                            ClassroomBusy.Duration = vApp.Duration;
                            ClassroomBusy.Description = vApp.Description;
                            ClassroomBusy.WeekFlag = vApp.WeekFlag;

                            ClassroomBusys.Add(ClassroomBusy);
                        }
                    }
                }
            }
            #endregion

            Tuple<bool, string> UploadResult = SchedulerSource.Source.Upload(Connections,
                CourseSections,
                TeacherBusys,
                ClassBusys,
                ClassroomBusys,
                (x,y) =>
                {
                    if (UploadSourceProgress != null)
                        UploadSourceProgress(this, new UploadSourceProgressEventArgs(x,y));
                });

            if (UploadSourceComplete != null)
                UploadSourceComplete(this, new UploadSourceCompleteEventArgs(UploadResult.Item1 , UploadResult.Item2));

            return UploadResult;
        }

        /// <summary>
        /// 開啟資料庫，讀取資料庫的資料至物件模型
        /// </summary>
        /// <param name="FilePath">資料庫路徑</param>
        public void OpenByBase64(string FilePath,string Password)
        {
            if (IsOpen)
                throw new Exception("scheduler source is open!");

            SchedulerSource.Source.OpenByBase64(FilePath,Password, x =>
            {

            });

            if (SchedulerSource.Source.IsSuccess)
                Import();
        }

        /// <summary>
        /// 開啟資料庫，讀取資料庫的資料至物件模型
        /// </summary>
        /// <param name="FilePath">資料庫路徑</param>
        public void Open(string FilePath)
        {
            if (IsOpen)
                throw new Exception("scheduler source is open!");

            SchedulerSource.Source.Open(FilePath, x =>
            {

            });

            if (SchedulerSource.Source.IsSuccess)
                Import();
        }

        /// <summary>
        /// 關閉資料庫，將資料庫連線關閉並釋放排課相關物件模型
        /// </summary>
        public void Close()
        {
            SchedulerSource.Source.Close();

            idNext = 1;

            ReleaseChildObjects();

            IsOpen = false;

            #region VB
            //If cnMain Is Nothing Then
            //    Err.Raise errDbAlreadyClose, , "Database already closed"
            //End If
            //cnMain.Close
            //Set cnMain = Nothing
            //idNext = 1
            //ReleaseChildObjects 
            #endregion
        }

        /// <summary>
        /// 取得教師不排課時段
        /// </summary>
        /// <returns></returns>
        private List<STeacherBusy> GetOfflineTeacherBusy()
        {
            #region 轉換教師不排課時段
            List<STeacherBusy> TeacherBusys = new List<STeacherBusy>();

            foreach (Teacher vTeacher in Teachers)
            {
                foreach (Appointment vApp in vTeacher.GetAppointments())
                {
                    if (string.IsNullOrEmpty(vApp.EventID))
                    {                       
                        STeacherBusy TeacherBusy = new STeacherBusy();

                        TeacherBusy.DSNS = string.Empty;
                        TeacherBusy.TeacherID = vTeacher.TeacherID;

                        TeacherBusy.WeekDay = vApp.WeekDay;
                        TeacherBusy.BeginTime = vApp.BeginTime;
                        TeacherBusy.Duration = vApp.Duration;
                        TeacherBusy.Description = vApp.Description;
                        TeacherBusy.LocationID = vApp.LocID;

                        TeacherBusys.Add(TeacherBusy);
                    }
                }
            }
            #endregion

            return TeacherBusys;
        }

        /// <summary>
        /// 取得班級不排課時段
        /// </summary>
        /// <returns></returns>
        private List<SClassBusy> GetSClassBusy()
        {
            #region 轉換班級不排課時段
            List<SClassBusy> ClassBusys = new List<SClassBusy>();

            foreach (Class vClass in Classes)
            {
                if (string.IsNullOrEmpty(vClass.ClassID))
                    continue;

                string[] ClassIDs = vClass.ClassID.Split(new char[]{','});
                string DSNS = ClassIDs[0];
                string ClassID = ClassIDs[1];

                foreach (Appointment vApp in vClass.GetAppointments())
                {
                    if (string.IsNullOrEmpty(vApp.EventID))
                    {
                        SClassBusy ClassBusy = new SClassBusy();

                        ClassBusy.DSNS = DSNS;
                        ClassBusy.ClassID = vClass.ClassID;
                        ClassBusy.WeekDay = vApp.WeekDay;
                        ClassBusy.BeginTime = vApp.BeginTime;
                        ClassBusy.Duration = vApp.Duration;
                        ClassBusy.Description = vApp.Description;

                        ClassBusys.Add(ClassBusy);
                    }
                }
            }
            #endregion

            return ClassBusys;
        }

        /// <summary>
        /// 取得場地不排課時段
        /// </summary>
        /// <returns></returns>
        private List<SClassroomBusy> GetSClassroomBusy()
        {
            #region 轉換場地不排課時段
            List<SClassroomBusy> ClassroomBusys = new List<SClassroomBusy>();

            foreach (Classroom vClassroom in Classrooms)
            {
                foreach (Appointment vApp in vClassroom.GetAppointments())
                {
                    if (string.IsNullOrEmpty(vApp.EventID))
                    {
                        SClassroomBusy ClassroomBusy = new SClassroomBusy();

                        ClassroomBusy.DSNS = string.Empty;
                        ClassroomBusy.ClassroomID = vClassroom.ClassroomID;

                        ClassroomBusy.WeekDay = vApp.WeekDay;
                        ClassroomBusy.BeginTime = vApp.BeginTime;
                        ClassroomBusy.Duration = vApp.Duration;
                        ClassroomBusy.Description = vApp.Description;
                        ClassroomBusy.WeekFlag = vApp.WeekFlag;

                        ClassroomBusys.Add(ClassroomBusy);
                    }
                }
            }
            #endregion

            return ClassroomBusys;
        }

        /// <summary>
        /// 取得課程分段
        /// </summary>
        /// <returns></returns>
        private List<SCourseSection> GetSCourseSection()
        {
            List<SCourseSection> CourseSections = new List<SCourseSection>();

            foreach (CEvent CEvent in CEvents)
            {
                SCourseSection CourseSection = new SCourseSection();

                #region ID
                CourseSection.CourseID = CEvent.CourseID;
                CourseSection.ClassroomID = CEvent.ClassroomID;
                CourseSection.TeacherName1 = CEvent.TeacherID1;
                CourseSection.TeacherName2 = CEvent.TeacherID2;
                CourseSection.TeacherName3 = CEvent.TeacherID3;
                CourseSection.ID = CEvent.EventID;
                CourseSection.ClassID = CEvent.ClassID;
                CourseSection.TimeTableID = CEvent.TimeTableID;
                #endregion

                #region Property
                CourseSection.WeekDay = CEvent.WeekDay;
                CourseSection.PeriodNo = CEvent.PeriodNo;
                CourseSection.Length = CEvent.Length;

                CourseSection.WeekdayCond = CEvent.WeekDayCondition;
                CourseSection.PeriodCond = CEvent.PeriodCondition;

                CourseSection.Subject = CEvent.SubjectID;
                CourseSection.SubjectAlias = CEvent.SubjectAlias;
                CourseSection.CourseName = CEvent.CourseName;

                CourseSection.AllowDup = CEvent.AllowDuplicate;
                CourseSection.Longbreak = CEvent.AllowLongBreak;
                CourseSection.Lock = CEvent.ManualLock;
                CourseSection.WeekFlag = CEvent.WeekFlag;

                CourseSection.CourseGroup = CEvent.CourseGroup;
                CourseSection.LimitNextDay = CEvent.LimitNextDay;
                CourseSection.Comment = CEvent.Comment;
                #endregion

                CourseSections.Add(CourseSection);
            }

            return CourseSections;
        }

        /// <summary>
        /// 用Base64儲存排課資料
        /// </summary>
        /// <param name="FilePath"></param>
        public void SaveByBase64(string FilePath,string Password)
        {
            if (!SchedulerSource.Source.IsSuccess && !IsOpen)
                throw new Exception("scheduler source is not open!");

            if (!string.IsNullOrWhiteSpace(FilePath))
                mSaveFilePath = FilePath;

            if (!string.IsNullOrWhiteSpace(Password))
                mSavePassword = Password;

            if (SaveSourceStart != null)
                SaveSourceStart(this, new SaveSourceStartEventArgs(CEvents.Count));

            List<SCourseSection> CourseSections = GetSCourseSection();

            List<STeacherBusy> TeacherBusys = GetOfflineTeacherBusy();

            List<SClassBusy> ClassBusys = GetSClassBusy();

            List<SClassroomBusy> ClassroomBusys = GetSClassroomBusy();

            SchedulerSource.Source.SaveByBase64(
                CourseSections,
                TeacherBusys,
                ClassBusys,
                ClassroomBusys,
                mSaveFilePath,
                mSavePassword,
                x =>
                {
                    if (SaveSourceProgress != null)
                        SaveSourceProgress(this, new SaveSourceProgressEventArgs(x));
                });

            if (SaveSourceComplete != null)
                SaveSourceComplete(this, new EventArgs());
        }

        /// <summary>
        /// 儲存排課資料
        /// </summary>
        /// <param name="FilePath"></param>
        public void Save(string FilePath)
        {
            if (!SchedulerSource.Source.IsSuccess && !IsOpen)
                throw new Exception("scheduler source is not open!");

            if (!string.IsNullOrWhiteSpace(FilePath))
                mSaveFilePath = FilePath;

            if (SaveSourceStart!=null)
                SaveSourceStart(this,new SaveSourceStartEventArgs(CEvents.Count));

            List<SCourseSection> CourseSections = GetSCourseSection();

            List<STeacherBusy> TeacherBusys = GetOfflineTeacherBusy();

            List<SClassBusy> ClassBusys = GetSClassBusy();

            List<SClassroomBusy> ClassroomBusys = GetSClassroomBusy();

            SchedulerSource.Source.Save(
                CourseSections,
                TeacherBusys,
                ClassBusys,
                ClassroomBusys,
                mSaveFilePath,
                x =>
                {
                    if (SaveSourceProgress != null)
                        SaveSourceProgress(this, new SaveSourceProgressEventArgs(x));
                });

            if (SaveSourceComplete != null)
                SaveSourceComplete(this, new EventArgs());
        }

        /// <summary>
        /// 改變週行事曆，下載週調代課記錄
        /// </summary>
        public void ChangeWeekSchedule(List<Connection> Connections,DateTime StartDate,DateTime EndDate)
        {
            Connections.ForEach
            (x =>
                {

                }
            ); 
        }

        /// <summary>
        /// 自動排課
        /// </summary>
        /// <param name="EventList">事件列表</param>
        /// <returns>事件列表最後有排課的事件編號</returns>
        public string AutoSchedule(CEvents EventList)
        {
            string Result = string.Empty;
            int nCurIndex = 0;
            int nTotItem = EventList.Count;
            int nMostNear;
            Periods prdsTest;
            Period prdTest;
            bool bMoveForward;
            bool bUserAbort;
            int nThreshold = 0;
            CEvent evtTest;

            if (AutoScheduleStart != null)
                AutoScheduleStart(this, new AutoScheduleStartEventArgs(nTotItem));

            #region Decrease the alloc hour for relating resource
            //減去相關資源的已安排時數
            foreach (CEvent evtEach in EventList)
                if (evtEach.WeekDay != 0) DecAllocHour(evtEach);
            #endregion

            #region Find first unscheduled event
            //找到第一個未排課分課
            while (nCurIndex < nTotItem-1)
            {
                if (EventList[nCurIndex].WeekDay == 0)
                    break;
                nCurIndex++;
            }
            #endregion

            #region Solution finding loop

            nMostNear = nCurIndex;
            bMoveForward = true;

            do 
            {
                //Notify 
                nThreshold++;

                #region 當nThreshold大於AutoScheduleNotifyThreshold時，發出AutoScheduleProgress
                if (nThreshold > AutoScheduleNotifyThreshold)
                {
                    if (AutoScheduleProgress != null)
                    {
                        AutoScheduleProgressEventArgs EventArgs = new AutoScheduleProgressEventArgs(nCurIndex,false);

                        AutoScheduleProgress(this, EventArgs );

                        if (EventArgs.Cancel)
                            break;
                    }

                    nThreshold = 0;
                }
                #endregion

                evtTest = EventList[nCurIndex];

                //假設有手動鎖定的情況，就直接跳到下個或上個事件
                if (evtTest.ManualLock)
                {
                    if (bMoveForward)
                        nCurIndex++;
                    else
                        nCurIndex--;
                }
                else
                {
                    //取得事件對應時間表的節次
                    prdsTest = TimeTables[evtTest.TimeTableID].Periods;
                    prdsTest.RandomPeriods();

                    //若事件未安排，則prdTest為第一節
                    if (evtTest.WeekDay == 0)
                        //prdTest = prdsTest.GetNextRandomPeriod();
                        prdTest = prdsTest.FirstPeriod(0);
                    else
                    {
                        //若事件有安排，則prdTest為目前節數的下一節
                        //prdTest = prdsTest.GetNextRandomPeriod();
                        prdTest = prdsTest.NextPeriod(evtTest.WeekDay, evtTest.PeriodNo);
                        ReleaseEvent(evtTest, true);
                    }

                    //根據prdTest測試是否可安排事件
                    while (prdTest != null)
                    {
                        if (TestSchedule(evtTest, prdTest.WeekDay, prdTest.PeriodNo,true) == 0)
                        {
                            AllocEvent(true);
                            break;
                        }
                        //prdTest = prdsTest.GetNextRandomPeriod();
                        prdTest = prdsTest.NextPeriod(prdTest.WeekDay, prdTest.PeriodNo);
                    }

                    //Determine to move forward or backward
                    //沒有排課成功的情況
                    if (prdTest == null)
                    {
                        bMoveForward = false;
                        nCurIndex--;
                    }
                    else
                    {
                        bMoveForward = true;
                        if (nMostNear < nCurIndex) nMostNear = nCurIndex;
                        nCurIndex++;
                    }
                }

            }while(nCurIndex>=0 && nCurIndex<nTotItem);
            #endregion

            //若傳回空白代表排課成功，若傳回事件編號代表該事件無解
            Result = nCurIndex>=nTotItem ? string.Empty : EventList[nMostNear].EventID;

            //Increase the alloc hour for relating resource
            foreach (CEvent each in EventList)
                if (each.WeekDay != 0) IncAllocHour(each);

            if (AutoScheduleComplete != null)
                AutoScheduleComplete(this, new AutoScheduleCompleteEventArgs(EventList,nMostNear));

            //Update solution counts
            UpdateEventsSolutionCount(EventList);

            return Result;
        }

        /// <summary>
        ///  根據事件編號、星期及節次來判斷是否可安排事件
        /// </summary>
        /// <param name="EventID">事件編號</param>
        /// <param name="WeekDay">星期</param>
        /// <param name="PeriodNo">節次</param>
        /// <returns>是否可安排事件，若可傳回true，若不行傳回false。</returns>
        public bool IsSchedulable(string EventID,int WeekDay,int PeriodNo)
        {
            Reason = TestSchedule(CEvents[EventID], WeekDay, PeriodNo,true);

            return Reason == 0 ? true : false;

            #region VB
            //mReason = TestSchedule(mCEvents(CStr(EventID)), WeekDay, PeriodNo)
            //If mReason = 0 Then
            //    IsSchedulable = True
            //Else
            //    IsSchedulable = False
            //End If
            #endregion
        }

        /// <summary>
        /// 根據事件編號（EventID）釋放事件，並重新計算解決方案及已排課節數。
        /// </summary>
        /// <param name="EventID"></param>
        public void FreeEvent(string EventID)
        {
            CEvents EventList = new CEvents();

            EventList.Add(CEvents[EventID]);

            FreeEvents(EventList);

            #region VB
            //Dim EventList As CEvents    
            //Set EventList = New CEvents    
            //EventList.Add mCEvents(CStr(EventID))
            //FreeEvents EventList 
            #endregion
        }

        /// <summary>
        /// 根據事件列表（EventList）釋放事件，並重新計算解決方案以及已排課節數。
        /// </summary>
        /// <param name="EventList">事件列表</param>
        public void FreeEvents(CEvents EventList)
        {
            Stopwatch watch = new Stopwatch();

            watch.Start();

            CEvents evtsRelate = new CEvents();

            foreach (CEvent evt in EventList)
            {
                //假設事件不為鎖定，以及星期不為0才進行
                if (!evt.ManualLock)
                    if (evt.WeekDay != 0)
                    {
                        #region 釋放相關事件
                        CEvents evtsRelease = CEvents.GetByAllocGroup(evt);

                        foreach (CEvent evtRelease in evtsRelease)
                        {
                            DecAllocHour(evtRelease);
                            ReleaseEvent(evtRelease,false);
                            evtsRelate.Add(evtRelease);
                        }
                        #endregion

                        //減少事件的已排課節數
                        DecAllocHour(evt);
                        //釋放事件
                        ReleaseEvent(evt,false);                       
                   }
            }

            foreach (CEvent evtRelate in evtsRelate)
                EventList.Add(evtRelate);

            watch.Stop();
            Console.WriteLine("" + watch.Elapsed.TotalMilliseconds);

            watch.Restart();

            //重新計算事件解決方案
            UpdateEventsSolutionCount(EventList);

            watch.Stop();
            Console.WriteLine("" + watch.Elapsed.TotalMilliseconds);

            if (EventsFreed != null)
                EventsFreed(this, new EventsFreedEventArgs(EventList));
        }

        /// <summary>
        /// 根據事件編號、星期及節次來安排事件
        /// </summary>
        /// <param name="EventID">事件編號</param>
        /// <param name="WeekDay">星期</param>
        /// <param name="PeriodNo">節次</param>
        /// <returns>是否有安排成功，若是傳回true、若不是傳回false。</returns>
        public bool ScheduleEvent(string EventID,int WeekDay,int PeriodNo)
        {
            CEvent curEvent = CEvents[EventID];

            //若WeekDay為0代表沒有被安排事件
            if (curEvent.WeekDay == 0)
            {
                //測試是否可以安排事件
                evtTested = null;
                evtTesteds.Clear();
                Reason = TestSchedule(curEvent, WeekDay, PeriodNo,true);

                if (Reason == 0)
                {
                    AllocEvent(true); //安排事件

                    IncAllocHour(curEvent); //增加已安排事件的節數

                    UpdateEventSolutionCount(EventID); //更新解決方案數

                    if (EventScheduled != null)
                        EventScheduled(this, new EventScheduledEventArgs(EventID));

                    #region 針對群組排課狀況進行更新
                    if (evtTesteds != null)
                    {
                        CEvents evtsUpdate = new CEvents();

                        //針對現有的群組課程物件列表
                        foreach(GroupCEvent evtGroup in evtTesteds)
                        {
                            //取得群組課程中的分課
                            CEvent evtUpdate = evtGroup.evtTested;

                            //若是群組課程分課系統編號不等於現在的系統編號，而且群組名稱相同
                            if (!curEvent.EventID.Equals(evtUpdate.EventID) 
                                && curEvent.CourseGroup.Equals(evtUpdate.CourseGroup))
                            {
                                //增加相關資源
                                IncAllocHour(evtUpdate);
                                evtsUpdate.Add(evtUpdate);
                            }
                        }

                        if (evtsUpdate.Count > 0)
                        {
                            //重新計算解決方案
                            UpdateEventsSolutionCount(evtsUpdate);

                            foreach (CEvent evtUpdate in evtsUpdate)
                            {
                                if (EventScheduled != null)
                                    EventScheduled(this, new EventScheduledEventArgs(evtUpdate.EventID)); 
                            }
                        }
                    }
                    #endregion

                    return true;
                }
            }

            return false;

            #region VB
            //Dim curEvent As CEvent
    
            //ScheduleEvent = False
            //Set curEvent = mCEvents(CStr(EventID))
    
            //If curEvent.WeekDay = 0 Then
            //    mReason = TestSchedule(curEvent, WeekDay, PeriodNo)
            //    If mReason = 0 Then
            //        AllocEvent
            //        IncAllocHour curEvent
            //        UpdateEventSolutionCount EventID
            //        ScheduleEvent = True
            //        RaiseEvent EventScheduled(EventID)
            //    End If
            //End If
            #endregion 
        }

        /// <summary>
        /// 根據事件編號鎖定事件，讓事件不能被修改
        /// 1.該事件不能已被鎖定。
        /// 2.該事件已被安排，WeekDay不能為0。
        /// </summary>
        /// <param name="EventID">事件編號</param>
        /// <returns>若有進行鎖定，回傳true，否則回傳false。</returns>
        public bool LockEvent(List<string> EventIDs)
        {
            List<string> LockEventIDs = new List<string>();

            foreach (string EventID in EventIDs)
            {
                if (!string.IsNullOrWhiteSpace(EventID) && 
                    CEvents.Exists(EventID))
                {
                    CEvent evtLock = CEvents["" + EventID];

                    if ((!evtLock.ManualLock) && (evtLock.WeekDay != 0))
                    {
                        evtLock.ManualLock = true;
                        LockEventIDs.Add(EventID);
                    }
                }
            }

            if (LockEventIDs.Count > 0 && EventLocked != null)
            {
                EventLocked(this, new EventLockedEventArgs(EventIDs));
                return true;
            }

            return false;
       }

        /// <summary>
        /// 根據事件編號解除鎖定，當ManualLock為true時才會解除鎖定。
        /// </summary>
        /// <param name="EventID">事件編號</param>
        public void UnlockEvent(List<string> EventIDs)
        {
            List<string> UnlockEventIDs = new List<string>();

            foreach (string EventID in EventIDs)
            {
                if (!string.IsNullOrWhiteSpace(EventID) &&
                    CEvents.Exists(EventID))
                {
                    CEvent evtLock = CEvents["" + EventID];

                    if (evtLock.ManualLock)
                    {
                        evtLock.ManualLock = false;
                        UnlockEventIDs.Add(EventID);
                    }
                } 
            }

            if (UnlockEventIDs.Count > 0 && EventUnlocked!= null)
            {
                EventUnlocked(this, new EventUnlockedEventArgs(EventIDs));
            }
        }

        /// <summary>
        /// 改變事件屬性
        /// </summary>
        /// <param name="idEvent">事件編號</param>
        /// <param name="WhoID">教師編號</param>
        /// <param name="WhereID">場地編號</param>
        /// <param name="WeekFlag">單雙週，單週為1、單雙週為2、單雙週為3。</param>
        /// <param name="WeekDayCondition">星期條件</param>
        /// <param name="PeriodCondition">節次條件</param>
        /// <param name="AllowLongBreak">是否允許跨中午</param>
        /// <param name="AllowDuplicate">是否允許重複</param>
        public void ChangeEventProperty(string idEvent, 
                               string WhoID1,
                               string WhoID2,
                               string WhoID3,
                               string WhereID,
                               Byte WeekFlag,
                               string WeekDayCondition,
                               string PeriodCondition,
                               bool AllowLongBreak,
                               bool AllowDuplicate,
                               string Comment)
        {
            CEvent evtChange = CEvents["" + idEvent];

            MaskOptions MaskOption = new MaskOptions();

            //只有在WeekDay為0時才能改變事件屬性
            if (evtChange.WeekDay != 0) return;

            #region VB
            //Dim evtChange As CEvent
            //Dim evtEnum As CEvent
            //Dim nFlag As Byte

            //Set evtChange = mCEvents(CStr(idEvent))
            //'cannot change the property of a already scheduled event
            //If evtChange.WeekDay <> 0 Then Exit Sub
            //nFlag = 0
            #endregion

            #region 判斷屬性是否有改變
            if (evtChange.TeacherID1 != WhoID1) MaskOption = MaskOptions.maskWho;
            if (evtChange.TeacherID2 != WhoID2) MaskOption = MaskOptions.maskWho;
            if (evtChange.TeacherID3 != WhoID3) MaskOption = MaskOptions.maskWho;

            if (evtChange.ClassroomID != WhereID) MaskOption |= MaskOptions.maskWhere;
            if (evtChange.WeekFlag != WeekFlag) MaskOption |=  MaskOptions.maskOther;
            if (evtChange.WeekDayCondition != DelSpaces(WeekDayCondition)) MaskOption |= MaskOptions.maskOther;
            if (evtChange.PeriodCondition != DelSpaces(PeriodCondition)) MaskOption |= MaskOptions.maskOther;
            if (evtChange.AllowLongBreak != AllowLongBreak) MaskOption |= MaskOptions.maskOther;
            if (evtChange.AllowDuplicate != AllowDuplicate) MaskOption |= MaskOptions.maskOther;
            if (evtChange.Comment != Comment) MaskOption |= MaskOptions.maskOther;
            #endregion

            //觸發改變屬性前事件
            if (EventPropertyBeforeChange != null)
                EventPropertyBeforeChange(this, new EventPropertyBeforeChangeEventArgs(evtChange.EventID,MaskOption));

            #region 實際改變屬性
            DecTotalHour(evtChange);

            if ((MaskOption & MaskOptions.maskWho) == MaskOptions.maskWho) evtChange.TeacherID1 = WhoID1;
            if ((MaskOption & MaskOptions.maskWho) == MaskOptions.maskWho) evtChange.TeacherID2 = WhoID2;
            if ((MaskOption & MaskOptions.maskWho) == MaskOptions.maskWho) evtChange.TeacherID3 = WhoID3;

            if ((MaskOption & MaskOptions.maskWhere) == MaskOptions.maskWhere) evtChange.ClassroomID = WhereID;
            if ((MaskOption & MaskOptions.maskOther) == MaskOptions.maskOther)
            {
                evtChange.WeekFlag = WeekFlag;
                evtChange.WeekDayCondition = DelSpaces(WeekDayCondition);
                evtChange.PeriodCondition = DelSpaces(PeriodCondition);
                evtChange.AllowLongBreak = AllowLongBreak;
                evtChange.AllowDuplicate = AllowDuplicate;
                evtChange.Comment = Comment;
            }

            IncTotalHour(evtChange);
            evtChange.SolutionCount = -1;
            #endregion

            //觸發改變屬性後事件
            if (EventPropertyChanged != null)
                EventPropertyChanged(this, new EventPropertyChangedEventArgs(evtChange.EventID,MaskOption));

            #region VB
            //With evtChange
            //    If .WhoID <> WhoID Then nFlag = nFlag Or maskWho
            //    If .WhereID <> WhereID Then nFlag = nFlag Or maskWhere
            //    If .WeekFlag <> WeekFlag Then nFlag = nFlag Or maskOther
            //    If .WeekDayCondition <> DelSpaces(WeekDayCondition) Then nFlag = nFlag Or maskOther
            //    If .PeriodCondition <> DelSpaces(PeriodCondition) Then nFlag = nFlag Or maskOther
            //    If .AllowLongBreak <> AllowLongBreak Then nFlag = nFlag Or maskOther
            //    If .AllowDuplicate <> AllowDuplicate Then nFlag = nFlag Or maskOther

            //    RaiseEvent EventPropertyBeforeChange(.EventID, nFlag)
            //    DecTotalHour evtChange

            //    If (nFlag And maskWho) > 0 Then .WhoID = WhoID
            //    If (nFlag And maskWhere) > 0 Then .WhereID = WhereID
            //    If (nFlag And maskOther) > 0 Then
            //        .WeekFlag = WeekFlag
            //        .WeekDayCondition = DelSpaces(WeekDayCondition)
            //        .PeriodCondition = DelSpaces(PeriodCondition)
            //        .AllowLongBreak = AllowLongBreak
            //        .AllowDuplicate = AllowDuplicate
            //    End If

            //    IncTotalHour evtChange
            //    .SolutionCount = -1
            //    RaiseEvent EventPropertyChanged(.EventID, nFlag)
            #endregion

            //change other related events that belong to the same course
            if (((MaskOption & MaskOptions.maskWho) == MaskOptions.maskWho) || (evtChange.AllowDuplicate != AllowDuplicate))
            {
                MaskOptions nEnumMaskOption = new MaskOptions();

                foreach (CEvent evtEnum in CEvents)
                {
                    if ((evtEnum.CourseID == evtChange.CourseID) && (evtEnum.WeekDay == 0)
                        && (evtEnum.EventID != evtChange.EventID))
                    {
                        if (evtEnum.AllowDuplicate != AllowDuplicate)
                            nEnumMaskOption |= MaskOptions.maskOther;

                        if (evtEnum.TeacherID1 != WhoID1)
                        {
                            nEnumMaskOption |= MaskOptions.maskWho;
                        }

                        if (evtEnum.TeacherID1 != WhoID2)
                        {
                            nEnumMaskOption |= MaskOptions.maskWho;
                        }

                        if (evtEnum.TeacherID1 != WhoID3)
                        {
                            nEnumMaskOption |= MaskOptions.maskWho;
                        }

                        if (EventPropertyBeforeChange != null)
                            EventPropertyBeforeChange(this, new EventPropertyBeforeChangeEventArgs(evtEnum.EventID, nEnumMaskOption));

                        DecTotalHour(evtEnum);
                        evtEnum.TeacherID1 = WhoID1;
                        evtEnum.TeacherID2 = WhoID2;
                        evtEnum.TeacherID3 = WhoID3;
                        evtEnum.AllowDuplicate = AllowDuplicate;
                        IncTotalHour(evtEnum);
                        evtEnum.SolutionCount = -1;

                        if (EventPropertyChanged != null)
                            EventPropertyChanged(this, new EventPropertyChangedEventArgs(evtChange.EventID, nEnumMaskOption));
                    }
                }
            }

            #region VB
            //    'change other related events that belong to the same course
            //    If (nFlag And maskWho > 0) Or (.AllowDuplicate <> AllowDuplicate) Then
            //        Dim nEnumFlag As Byte

            //        For Each evtEnum In mCEvents
            //            If (evtEnum.CourseID = .CourseID) And (evtEnum.WeekDay = 0) _
            //            And (evtEnum.EventID <> .EventID) Then
            //                nEnumFlag = 0
            //                If evtEnum.AllowDuplicate <> AllowDuplicate Then
            //                    nEnumFlag = nEnumFlag Or maskOther
            //                End If
            //                If evtEnum.WhoID <> WhoID Then
            //                    nEnumFlag = nEnumFlag Or maskWho
            //                End If

            //                RaiseEvent EventPropertyBeforeChange(evtEnum.EventID, nEnumFlag)
            //                DecTotalHour evtEnum
            //                evtEnum.WhoID = WhoID
            //                evtEnum.AllowDuplicate = AllowDuplicate
            //                IncTotalHour evtEnum
            //                evtEnum.SolutionCount = -1
            //                RaiseEvent EventPropertyChanged(evtEnum.EventID, nEnumFlag)
            //            End If
            //        Next evtEnum
            //    End If
            //End With
            #endregion
        }

        /// <summary>
        /// 改變事件長度
        /// </summary>
        /// <param name="idEvent">事件編號</param>
        /// <param name="Length">長度</param>
        public void ChangeEventLength(string idEvent,int Length)
        {
            CEvent evtChange = CEvents["" + idEvent];

            if (evtChange.WeekDay != 0) return;

            if (evtChange.Length != Length)
            {
                if (EventPropertyBeforeChange != null)
                    EventPropertyBeforeChange(this, new EventPropertyBeforeChangeEventArgs(idEvent, MaskOptions.maskOther));

                DecTotalHour(evtChange);
                evtChange.Length = Length;
                IncTotalHour(evtChange);
                evtChange.SolutionCount = -1;

                if (EventPropertyChanged != null)
                    EventPropertyChanged(this, new EventPropertyChangedEventArgs(idEvent, MaskOptions.maskOther));
            }

            #region VB
            //Dim evtChange As CEvent
    
            //Set evtChange = mCEvents(CStr(idEvent))
            //'cannot change the property of a already scheduled event
            //If evtChange.WeekDay <> 0 Then Exit Sub

            //If evtChange.Length <> Length Then
            //    RaiseEvent EventPropertyBeforeChange(idEvent, maskOther)
            //    DecTotalHour evtChange
            //    evtChange.Length = Length
            //    IncTotalHour evtChange
            //    evtChange.SolutionCount = -1
            //    RaiseEvent EventPropertyChanged(idEvent, maskOther)
            //End If
            #endregion
        }

        /// <summary>
        /// 新增事件
        /// </summary>
        /// <param name="evtInsert">事件</param>
        public void InsertEvent(CEvent evtInsert)
        {
            string EventID = string.Empty;
            CEvent evtMember;

            foreach (CEvent evtEach in CEvents)
            {
                if (evtInsert.Priority < evtEach.Priority)
                {
                    EventID = evtEach.EventID;
                    break;
                }
            }

            Func<string, long, string> SetEventID = (x, y) =>
            {
                string[] strEventIDs = x.Split(new char[] { ',' });

                return strEventIDs[0] +","+ y;
            };

            evtInsert.EventID = SetEventID(evtInsert.EventID, idNext);
            idNext++;

            evtMember = string.IsNullOrEmpty(EventID) ? CEvents.Add(evtInsert) : CEvents.AddBefore(evtInsert, EventID);

            if (evtMember != null)
            {
                IncTotalHour(evtMember);

                if (EventInserted != null)
                    EventInserted(this, new EventInsertedEventArgs(evtInsert.EventID));
            }

            #region VB
            //Dim idRef As Long
            //Dim evtMember As CEvent
    
            //idRef = 0
            //For Each evtMember In mCEvents
            //    If evtInsert.Priority < evtMember.Priority Then
            //        idRef = evtMember.EventID
            //        Exit For
            //    End If
            //Next evtMember
    
            //evtInsert.EventID = idNext
            //idNext = idNext + 1
    
            //If idRef = 0 Then
            //    Set evtMember = mCEvents.Add(evtInsert)
            //Else
            //    Set evtMember = mCEvents.AddBefore(evtInsert, idRef)
            //End If
    
            //If Not (evtMember Is Nothing) Then
            //    IncTotalHour evtMember
            //    RaiseEvent EventInserted(evtInsert.EventID)
            //End If 
            #endregion
        }

        /// <summary>
        /// 根據事件編號刪除事件
        /// </summary>
        /// <param name="EventID">事件</param>
        public void DeleteEvent(string EventID)
        {
            CEvent curEvent = CEvents[EventID];

            //若是事件已被安排則不能刪除事件
            if (curEvent.WeekDay != 0) return;

            if (EventBeforeDelete != null)
                EventBeforeDelete(this, new EventBeforeDeleteEventArgs(EventID));

            DecTotalHour(curEvent);
            CEvents.RemoveID(EventID);

            if (EventDeleted != null)
                EventDeleted(this, new EventDeletedEventArgs(EventID));

            #region VB
            //Dim curEvent As CEvent
            //Set curEvent = mCEvents(CStr(EventID))
            //If curEvent.WeekDay <> 0 Then Exit Sub
            //RaiseEvent EventBeforeDelete(EventID)
            //DecTotalHour curEvent
            //mCEvents.RemoveID EventID
            //RaiseEvent EventDeleted(EventID) 
            #endregion
        }

        /// <summary>
        /// 檢查時段有空的教師
        /// </summary>
        /// <param name="WeekDay">星期</param>
        /// <param name="BeginTime">開始時間</param>
        /// <param name="Duration">持續分鐘</param>
        /// <param name="WeekFlag">單雙週，單週為1、單雙週為2、單雙週為3。</param>
        /// <returns>傳回有空的教師清單</returns>
        public Teachers GetWhoAvailable(int WeekDay,DateTime BeginTime,int Duration,byte WeekFlag)
        {
            Teachers whosCand = new Teachers();

            //針對每位教師
            foreach (Teacher whoCand in Teachers)
            {
                //若教師編號不為NullValue
                if (!whoCand.TeacherID.IsNullValue())
                {
                    //針對教師每個行事曆
                    for (int i = 0; i < whoCand.Capacity; i++)
                    {
                        //引用教師行事曆
                        whoCand.UseAppointments(i);
                        //檢查教師是否有空
                        if (whoCand.Appointments.IsFreeTime(WeekDay, BeginTime, Duration, WeekFlag))
                        {
                            //將教師加入到清單中
                            whosCand.Add(whoCand);
                            break;
                        }
                    }
                }
            }

            return whosCand;

            #region VB
            //Dim whoCand As Who
            //Dim whosCand As Whos
            //Dim nWhichApp As Integer
    
            //Set whosCand = New Whos
    
            //For Each whoCand In mWhos
            //    If Not IsNullValue(whoCand.WhoID) Then
            //        For nWhichApp = 1 To whoCand.Capacity
            //            whoCand.UseAppointments nWhichApp
            //            If whoCand.Appointments.IsFreeTime(WeekDay, BeginTime, Duration, WeekFlag) Then
            //                whosCand.Add whoCand
            //                Exit For
            //            End If
            //        Next nWhichApp
            //    End If
            //Next whoCand

            //Set GetWhoAvailable = whosCand
            #endregion
        }

        /// <summary>
        /// 尋找代課教師清單
        /// </summary>
        /// <param name="EventID">事件編號</param>
        /// <returns>可代課教師清單</returns>
        public Teachers GetCandidates(string EventID)
        {
            CEvent evtTest = CEvents[EventID];
            Teachers whosCand = new Teachers();
            int nSaveWeekDay;
            int nSavePeriod;
            string idSaveWho;

            #region VB
            //Dim whoCand As Who
            //Dim whosCand As Whos
            //Dim nSaveWeekDay As Integer
            //Dim nSavePeriod As Integer
            //Dim idSaveWho As Long
            //Dim evtTest As CEvent
            //Dim evtMember As CEvent
    
            //Set evtTest = mCEvents(CStr(EventID))
            //Set whosCand = New Whos
            #endregion

            //

            //針對每個Event尋找不同授課教師，但是相同科目的教師清單
            foreach (CEvent evtMember in CEvents)
            {
                if (evtTest.TeacherID1!=evtMember.TeacherID1 && 
                    (!evtMember.TeacherID1.IsNullValue()) && 
                    (evtTest.SubjectID == evtMember.SubjectID))
                {
                    whosCand.Add(Teachers[evtMember.TeacherID1]);
                }
            }

            //假設事件已被安排
            if (evtTest.WeekDay != 0)
            {
                nSaveWeekDay = evtTest.WeekDay;
                nSavePeriod = evtTest.PeriodNo;
                idSaveWho = evtTest.TeacherID1;

                ReleaseEvent(evtTest,false);

                List<string> RemoveIDs = new List<string>();

                #region 測試代課教師可否安排到此事件
                foreach (Teacher whoCand in whosCand)
                {
                    evtTest.TeacherID1 = whoCand.TeacherID;
                    if (TestSchedule(evtTest, nSaveWeekDay, nSavePeriod,true) != 0)
                        RemoveIDs.Add(evtTest.TeacherID1);
                }
                #endregion

                //移除不能代課教師
                RemoveIDs.ForEach(x=>whosCand.Remove("" + x));

                #region 將事件重新安排至原教師
                evtTest.TeacherID1 = idSaveWho;

                if (TestSchedule(evtTest, nSaveWeekDay, nSavePeriod,false) == 0)
                    AllocEvent(false);
                #endregion
            }

            return whosCand;

            #region VB
            //With evtTest
    
            //For Each evtMember In mCEvents
            //    If (.WhoID <> evtMember.WhoID) And (Not IsNullValue(evtMember.WhoID)) _
            //       And (.WhatID = evtMember.WhatID) Then
            //        whosCand.Add mWhos(CStr(evtMember.WhoID))
            //    End If
            //Next evtMember
    
            //If .WeekDay <> 0 Then
            //    nSaveWeekDay = .WeekDay
            //    nSavePeriod = .PeriodNo
            //    idSaveWho = .WhoID
            //    ReleaseEvent evtTest
        
            //    For Each whoCand In whosCand
            //        .WhoID = whoCand.WhoID
            //        If TestSchedule(evtTest, nSaveWeekDay, nSavePeriod) <> 0 Then
            //            whosCand.Remove CStr(.WhoID)
            //        End If
            //    Next whoCand
        
            //    .WhoID = idSaveWho
            //    If TestSchedule(evtTest, nSaveWeekDay, nSavePeriod) = 0 Then AllocEvent
            //End If
    
            //End With
    
            //Set GetCandidates = whosCand
            //Set whosCand = Nothing
            #endregion
        }

        /// <summary>
        /// 測試兩個Event是否可交換
        /// </summary>
        /// <param name="EventIDA"></param>
        /// <param name="EventIDB"></param>
        /// <returns>是否能交換，若能交換傳回true，若不能交換傳回false。</returns>
        public bool IsEventExchangable(string EventIDA,string EventIDB)
        {
            bool IsExchangable = false;

            CEvent evtA = CEvents[EventIDA];
            CEvent evtB = CEvents[EventIDB];

            int nSaveWeekDayA;
            int nSaveWeekDayB;
            int nSavePeriodA;
            int nSavePeriodB;

            #region VB
            //Dim evtA As CEvent
            //Dim evtB As CEvent
            //Dim nSaveWeekDayA As Integer
            //Dim nSavePeriodA As Integer
            //Dim nSaveWeekDayB As Integer
            //Dim nSavePeriodB As Integer

            //IsEventExchangable = False
    
            //Set evtA = mCEvents(CStr(EventIDA))
            //Set evtB = mCEvents(CStr(EventIDB))
            #endregion

            //兩個事件長度不一樣，則不可交換（傳回false）
            if (evtA.Length != evtB.Length) return false;

            #region 將EventA、EventB的星期及節次保留
            nSaveWeekDayA = evtA.WeekDay;
            nSaveWeekDayB = evtB.WeekDay;

            nSavePeriodA = evtA.PeriodNo;
            nSavePeriodB = evtB.PeriodNo;
            #endregion

            //假設evtA或evtB的星期不為0，就先釋放此事件（Event）
            if (nSaveWeekDayA != 0)
            {
                ReleaseEvent(evtA,true);
            }

            if (nSaveWeekDayB != 0)
            {
                ReleaseEvent(evtB,true);
            }

            //測試evtA是否可換到evtB的星期及節次
            if (nSaveWeekDayB!=0)
                if (TestSchedule(evtA,nSaveWeekDayB,nSavePeriodB,true) == 0)
                    IsExchangable = true;

            //測試evtB是否可換到evtA的星期及節次
            if ((IsExchangable) && (nSaveWeekDayA != 0))
                if (TestSchedule(evtB, nSaveWeekDayA, nSavePeriodA,true) != 0)
                    IsExchangable = false;

            //重新安排evtA的星期及節次
            if (nSaveWeekDayA != 0)
                if (TestSchedule(evtA, nSaveWeekDayA, nSavePeriodA,true) == 0)
                    AllocEvent(true);

            //重新安排evtB的星期及節次
            if (nSaveWeekDayB != 0)
                if (TestSchedule(evtB, nSaveWeekDayB, nSavePeriodB,true) == 0)
                    AllocEvent(true);

            #region VB
            //If evtA.Length <> evtB.Length Then Exit Function
    
            //'Save original weekday and period
            //nSaveWeekDayA = evtA.WeekDay
            //nSaveWeekDayB = evtB.WeekDay
            //nSavePeriodA = evtA.PeriodNo
            //nSavePeriodB = evtB.PeriodNo
            //'Release events
            //If nSaveWeekDayA <> 0 Then ReleaseEvent evtA
            //If nSaveWeekDayB <> 0 Then ReleaseEvent evtB
    
            //If nSaveWeekDayB <> 0 Then
            //    If TestSchedule(evtA, nSaveWeekDayB, nSavePeriodB) = 0 Then IsEventExchangable = True
            //End If
            //If IsEventExchangable And nSaveWeekDayA <> 0 Then
            //    If TestSchedule(evtB, nSaveWeekDayA, nSavePeriodA) <> 0 Then IsEventExchangable = False
            //End If
    
            //'Restore
            //If nSaveWeekDayA <> 0 Then
            //    If TestSchedule(evtA, nSaveWeekDayA, nSavePeriodA) = 0 Then AllocEvent
            //End If
            //If nSaveWeekDayB <> 0 Then
            //    If TestSchedule(evtB, nSaveWeekDayB, nSavePeriodB) = 0 Then AllocEvent
            //End If
            #endregion

            return IsExchangable;
        }

        /// <summary>
        /// 測試兩個Event是否可交換
        /// </summary>
        /// <param name="EventIDA"></param>
        /// <param name="EventIDB"></param>
        /// <returns>是否能交換，若能交換傳回true，若不能交換傳回false。</returns>
        public bool ExchangeEvent(string EventIDA, string EventIDB)
        {
            bool IsExchangable = false;

            CEvent evtA = CEvents[EventIDA];
            CEvent evtB = CEvents[EventIDB];

            int nSaveWeekDayA,nSaveWeekDayB;
            int nSavePeriodA,nSavePeriodB;

            //兩個事件長度不一樣，則不可交換（傳回false）
            if (evtA.Length != evtB.Length) return false;

            #region 將EventA、EventB的星期及節次保留
            nSaveWeekDayA = evtA.WeekDay;
            nSaveWeekDayB = evtB.WeekDay;

            nSavePeriodA = evtA.PeriodNo;
            nSavePeriodB = evtB.PeriodNo;
            #endregion

            //假設evtA或evtB的星期不為0，釋放此事件（Event）
            if (nSaveWeekDayA != 0) ReleaseEvent(evtA,true);
            if (nSaveWeekDayB != 0) ReleaseEvent(evtB,true);

            //測試evtA是否可換到evtB的星期及節次
            if (nSaveWeekDayB != 0)
                if (TestSchedule(evtA, nSaveWeekDayB, nSavePeriodB,true) == 0)
                    IsExchangable = true;

            //測試evtB是否可換到evtA的星期及節次
            if ((IsExchangable) && (nSaveWeekDayA != 0))
                if (TestSchedule(evtB, nSaveWeekDayA, nSavePeriodA,true) != 0)
                    IsExchangable = false;

            if (IsExchangable)
            {
                AllocEvent(true);

                TestSchedule(evtA, nSaveWeekDayB, nSavePeriodB,true);

                AllocEvent(true);

                UpdateEventSolutionCount(evtA.EventID); //更新解決方案數
                UpdateEventSolutionCount(evtB.EventID); //更新解決方案數

                //若為調代課模式，則記錄調課記錄
                //if (Constants.ExecutionMode == ScheduleMode.modSchedulePlus)
                //    AddExchangeUpdate(EventIDA,nSaveWeekDayA,nSavePeriodA,EventIDB,nSaveWeekDayB,nSavePeriodB);
            }

            return IsExchangable;
        }


        /// <summary>
        /// 根據SolutionCount來排序事件
        /// </summary>
        /// <param name="EventList"></param>
        /// <returns></returns>
        public CEvents SortEvent(CEvents EventList)
        {
            CEvents SortEvents = new CEvents();

            foreach (CEvent Event in EventList.OrderBy(x => x.SolutionCount))
                SortEvents.Add(Event);

            return SortEvents;
        }

        /// <summary>
        /// 計算事件的解決方案
        /// </summary>
        /// <param name="evtsCalc"></param>
        public void GetSolutionCounts(CEvents evtsCalc)
        {
            Stopwatch watch = new Stopwatch();

            watch.Start();

            foreach (CEvent evtCalc in evtsCalc)
            {
                //假設WeekDay為0代表Event沒有被安排，才可計算SolutionCount
                if (evtCalc.WeekDay == 0)
                {
                    //假設SolutionCount為-1才計算
                    if (evtCalc.SolutionCount == -1)
                        evtCalc.SolutionCount = GetSolutionCount(evtCalc.EventID);
                }
                else
                    evtCalc.SolutionCount = -1;
            }

            watch.Stop();
            Console.WriteLine("" + watch.Elapsed.TotalMilliseconds);

            if (EventSolCountUpdated != null)
                EventSolCountUpdated(this, new EventSolCountUpdatedEventArgs(evtsCalc));

            #region VB
            //Dim evtCalc As CEvent
    
            //For Each evtCalc In evtsCalc
            //    With evtCalc
            //    If .WeekDay = 0 Then
            //        If .SolutionCount = -1 Then
            //            .SolutionCount = GetSolutionCount(.EventID)
            //        End If
            //    Else
            //        .SolutionCount = -1
            //    End If
            //    End With
            //Next evtCalc

            //RaiseEvent EventSolCountUpdated(evtsCalc)
            #endregion
        }
        #endregion

        #region Private functions

        /// <summary>
        /// 建立排課相關物件，初始化CEvents、Whos、Wheres、Whoms、Whats、Locations、Distances、WeekDayVariables、PeriodVariables、TimeTables。
        /// </summary>
        private void CreateChildObjects()
        {
            CEvents = new CEvents();
            Teachers = new Teachers();
            Classrooms = new Classrooms();
            Classes = new Classes();
            Subjects = new Subjects();
            Locations = new Locations();
            Distances = new Distances();
            WeekDayVariables = new Variables();
            PeriodVariables = new Variables();
            TimeTables = new TimeTables();

            #region VB
            //Set mCEvents = New CEvents
            //Set mWhos = New Whos
            //Set mWheres = New Wheres
            //Set mWhoms = New Whoms
            //Set mWhats = New Whats
            //Set mLocations = New Locations
            //Set mDistances = New Distances
            //Set mWVar = New Variables
            //Set mPVar = New Variables
            //Set mTimeTables = New TimeTables
            #endregion

            //'Inerst NULL objects into related collections

            Teachers.Add(new Teacher(Constants.NullString, "無", 0,null,null,null,Constants.NullString));
            Classes.Add(new Class(Constants.NullString, "無",Constants.NullString,Constants.NullString,Constants.NullString,Constants.NullString));
            Classrooms.Add(new Classroom(Constants.NullString, "無", 0, Constants.NullString, true));
            Subjects.Add(new Subject(Constants.NullString, "無"));
            TimeTables.Add(new TimeTable(Constants.NullString, "未指定"));

            #region VB
            //Dim whoNew As Who
            //Dim whmNew As Whom
            //Dim whrNew As Where
            //Dim ttbNew As TimeTable
            //Dim whtNew As What
    
            //Set whoNew = New Who
            //whoNew.SetWho NullValue, LoadResString(rsWhoNull), 0
            //mWhos.Add whoNew
    
            //Set whmNew = New Whom
            //whmNew.SetWhom NullValue, LoadResString(rsWhomNull), NullValue
            //mWhoms.Add whmNew
    
            //Set whrNew = New Where
            //whrNew.SetWhere NullValue, LoadResString(rsWhereNull), 0, NullValue, True
            //mWheres.Add whrNew
    
            //Set whtNew = New What
            //whtNew.SetWhat NullValue, LoadResString(rsWhatNull)
            //mWhats.Add whtNew
    
            //Set ttbNew = New TimeTable
            //ttbNew.SetTimeTable NullValue, LoadResString(rsTimeTableNull)
            //mTimeTables.Add ttbNew 
            #endregion
        }

        /// <summary>
        /// 釋放排課相關物件，將CEvents、Whos、Wheres、Whoms、Whats、Locations、Distances、WeekDayVariables、PeriodVariables設為null。
        /// </summary>
        private void ReleaseChildObjects()
        {
            CEvents.Clear();
            CEvents = null;
            Teachers.Clear();
            Teachers = null;
            Classrooms.Clear();
            Classrooms = null;
            Classes.Clear();
            Classes = null;
            Subjects.Clear();
            Subjects = null;
            Locations.Clear();
            Locations = null;
            Distances.Clear();
            Distances = null;
            WeekDayVariables.Clear();
            WeekDayVariables = null;
            PeriodVariables.Clear();
            PeriodVariables = null;
            TimeTables.Clear();
            TimeTables = null;

            #region VB
            //Set mCEvents = Nothing
            //Set mWhos = Nothing
            //Set mWheres = Nothing
            //Set mWhoms = Nothing
            //Set mWhats = Nothing
            //Set mLocations = Nothing
            //Set mDistances = Nothing
            //Set mWVar = Nothing
            //Set mPVar = Nothing
            //Set mTimeTables = Nothing 
            #endregion
        }

        /// <summary>
        /// 測試課程分段（CEvent、CourseSection）在星期幾及節次可否排課。
        /// </summary>
        /// <param name="NewEvent">課程分段</param>
        /// <param name="WeekDay">星期幾</param>
        /// <param name="PeriodNo">節次</param>
        /// <returns></returns>
        private int TestSchedule(CEvent NewEvent,int WeekDay,int PeriodNo,bool TestGroup)
        {
            #region 宣告變數及初始化
            bool bPass;
            int intTempVal;
            Period prdBreak;
            Period prdTest;
            Periods prdsTest;

            evtTested = null;

            ReasonDesc = new ReasonDescription();

            //取得課程分段（CEvent、CourseSection）的場地
            whrTest = NewEvent.ClassroomID.IsNullValue() ? null : Classrooms["" + NewEvent.ClassroomID];
    
            //取得課程分段（CEvent、CourseSection）的班級
            whmTest = NewEvent.ClassID.IsNullValue() ? null : Classes["" + NewEvent.ClassID];
            
            //取得課程分段（CEvent、CourseSection）的授課教師一
            whoTest1 = NewEvent.TeacherID1.IsNullValue() ? null : Teachers["" + NewEvent.TeacherID1];
            //取得課程分段（CEvent、CourseSection）的授課教師二
            whoTest2 = NewEvent.TeacherID2.IsNullValue() ? null : Teachers["" + NewEvent.TeacherID2];
            //取得課程分段（CEvent、CourseSection）的授課教師三
            whoTest3 = NewEvent.TeacherID3.IsNullValue() ? null : Teachers["" + NewEvent.TeacherID3];
            
            //取得課程分段（CEvent、CourseSection）時間表節次列表
            prdsTest = TimeTables["" + NewEvent.TimeTableID].Periods;
            
            //取得星期幾的中午休息時間
            prdBreak = prdsTest.GetBreakPeriod(WeekDay);
            
            //取得星期節次
            prdTest = prdsTest.GetPeriodIgroneDisable(WeekDay, PeriodNo);

            //若是沒有對應的節次傳回Constants.tsCannotFit
            if (prdTest == null)
            {
                //ReasonDesc.Desc = "未排節數不足" + System.Environment.NewLine + "無法排課";
                ReasonDesc.Desc = "節數" + NewEvent.Length + "塞不下";
                return Constants.tsCannotFit;
            }
            else if (prdTest.Disable)
            {
                ReasonDesc.Desc = prdTest.DisableMessage;
                return Constants.tsCannotFit; 
            }

            #endregion

            prdsUse = new Periods();
            prdsUse.Add(prdTest);

            //bPass用來判斷是否要檢查『跨中午』
            //1.prdBreak不為null 
            //2.NewEvent不允許跨中午 
            //3.NewEvent長度大於0
            bPass = (prdBreak != null) && (!NewEvent.AllowLongBreak) && (NewEvent.Length > 1);

            //判斷prdTest的時間是否在prdBreak之前
            if (bPass) bPass = prdTest.BeginTime.Before(prdBreak.BeginTime);

            #region 檢查課程分段中時間表的節次地點，是否與課程分段中的場地地點一致；若不一致的話傳回地點衝突。
            for (intTempVal = PeriodNo +1;intTempVal<= (PeriodNo+NewEvent.Length-1);intTempVal++)
            {
                prdTest = prdsTest.GetPeriodIgroneDisable(WeekDay, intTempVal);

                if (prdTest == null)
                {
                    //ReasonDesc.Desc = "未排節數不足" + System.Environment.NewLine + "無法排課";
                    ReasonDesc.Desc = "節數" + NewEvent.Length + "塞不下";
                    return Constants.tsCannotFit;
                }
                else if (prdTest.Disable)
                {
                    ReasonDesc.Desc = prdTest.DisableMessage;
                    return Constants.tsCannotFit;
                }

                if (whrTest != null)
                    if (!(prdTest.LocID.IsNullValue() || whrTest.LocID.IsNullValue()))
                        if (prdTest.LocID != whrTest.LocID)
                        {
                            ReasonDesc.Desc = "地點"+prdTest.LocID+"、"+whrTest.LocID +"不符";
                            return Constants.tsLocConflict;
                        }
                prdsUse.Add(prdTest);
            }
            #endregion

            #region Long break check
            if (bPass)
                if (prdTest.BeginTime.After(prdBreak.BeginTime))
                {
                    ReasonDesc.Desc = "此課程不允" + System.Environment.NewLine + "跨中午排課";
                    //ReasonDesc.Desc = "不可跨中午";
                    return Constants.tsLongBreak;
                }
            #endregion

            #region Check weekday condition
            if (NewEvent.WeekDayOp > 0)
            {
                int WeekDayVar;

                if (int.TryParse(NewEvent.WeekDayVar ,out WeekDayVar))
                {
                    switch (NewEvent.WeekDayOp)
                    {
                        case Constants.opEqual:
                            bPass = WeekDay == WeekDayVar;
                            break;
                        case Constants.opGreater:
                            bPass = WeekDay > WeekDayVar;
                            break;
                        case Constants.opGreaterOrEqual:
                            bPass = WeekDay >= WeekDayVar;
                            break;
                        case Constants.opLess:
                            bPass = WeekDay < WeekDayVar;
                            break;
                        case Constants.opLessOrEqual:
                            bPass = WeekDay <= WeekDayVar;
                            break;
                        case Constants.opNotEqual:
                            bPass = WeekDay != WeekDayVar;
                            break;
                        default:
                            bPass = true;
                            break;
                    }

                    if (!bPass)
                    {
                        ReasonDesc.Desc = "此課程必需排在" + System.Environment.NewLine + "星期" + NewEvent.WeekDayCondition;
                        //ReasonDesc.Desc = "星期"+NewEvent.WeekDayCondition+"不符";
                        return Constants.tsWeekDayConflict;
                    }
                }
                else if (NewEvent.WeekDayOp == Constants.opEqual)
                    if (WeekDayVariables.Exists(NewEvent.WeekDayVar))
                        if (!WeekDayVariables[NewEvent.WeekDayVar].Fit(WeekDay))
                        {
                            ReasonDesc.Desc = "此課程必需排在" + System.Environment.NewLine + "星期" + NewEvent.WeekDayCondition;
                            //ReasonDesc.Desc = "星期" + NewEvent.WeekDayCondition + "不符";
                            return Constants.tsWeekDayConflict;
                        }
            }

            #endregion

            #region Check period condition
            if (NewEvent.PeriodOp > 0)
            {
                int PeriodVar;

                if (int.TryParse(NewEvent.PeriodVar, out PeriodVar))
                {
                    switch (NewEvent.PeriodOp)
                    {
                        case Constants.opEqual:
                            bPass = PeriodNo == PeriodVar;
                            break;
                        case Constants.opGreater:
                            bPass = PeriodNo > PeriodVar;
                            break;
                        case Constants.opGreaterOrEqual:
                            bPass = PeriodNo >= PeriodVar;
                            break;
                        case Constants.opLess:
                            bPass = PeriodNo < PeriodVar;
                            break;
                        case Constants.opLessOrEqual:
                            bPass = PeriodNo <= PeriodVar;
                            break;
                        case Constants.opNotEqual:
                            bPass = PeriodNo != PeriodVar;
                            break;
                        default:
                            bPass = true;
                            break;
                    }

                    if (!bPass)
                    {
                        ReasonDesc.Desc = "此課程必需排在" + System.Environment.NewLine + "第" + NewEvent.PeriodCondition + "節";
                        //ReasonDesc.Desc = "節次" + NewEvent.PeriodCondition + "不符";
                        return Constants.tsPeriodConflict;
                    }
                }
                else if (NewEvent.PeriodOp == Constants.opEqual)
                    if (PeriodVariables.Exists(NewEvent.PeriodVar))
                        if (!PeriodVariables[NewEvent.PeriodVar].Fit(PeriodNo))
                        {
                            ReasonDesc.Desc = "此課程必需排在" + System.Environment.NewLine + "第" + NewEvent.PeriodCondition + "節";
                            //ReasonDesc.Desc = "節次" + NewEvent.PeriodCondition + "不符";
                            return Constants.tsPeriodConflict;
                        }
            }
            #endregion

            #region Check WHOM time conflict and duplicate WHAT
            if (whmTest != null)
            {
                intTempVal = whmTest.Appointments.CheckWhom(prdsUse, NewEvent.SubjectID, NewEvent.WeekFlag,NewEvent.EventID,NewEvent.AllowDuplicate,NewEvent.LimitNextDay);

                if (intTempVal != 0)
                {
                    switch(intTempVal)
                    {
                        case 1:
                            ReasonDesc.AssocID = NewEvent.ClassID;
                            ReasonDesc.AssocName = Classes[NewEvent.ClassID].Name;
                            ReasonDesc.AssocType = Constants.lvWhom;
                            ReasonDesc.Desc = "未排節數不足" + System.Environment.NewLine + "無法排課";
                            //ReasonDesc.Desc = "塞不下";
                            return Constants.tsWhomConflict;
                        case 3:
                            ReasonDesc.Desc = "今天已經排" + System.Environment.NewLine + Subjects[NewEvent.SubjectID].Name;
                            return Constants.tsDupWhat;
                        case 4:
                            ReasonDesc.Desc = "隔日已排" + System.Environment.NewLine + Subjects[NewEvent.SubjectID].Name;
                            return Constants.tsDupWhat;
                        default:
                            ReasonDesc.AssocID = NewEvent.ClassID;
                            ReasonDesc.AssocName = Classes[NewEvent.ClassID].Name;
                            ReasonDesc.AssocType = Constants.lvWhom;
                            ReasonDesc.Desc = "未排節數不足" + System.Environment.NewLine + "無法排課";
                            return Constants.tsWhomConflict;
                    }
                }   
            }

            #endregion

            #region Check WHERE time conflict            
            if (whrTest != null) 
            {
                if (!whrTest.LocOnly) //判斷是否不為無限制使用場地
                {
                    if (whrTest.Capacity > 1) //判斷行事曆是否大於1個
                    {
                        intTempVal = 0;
                        bPass = false;
                        while(intTempVal < whrTest.Capacity)
                        {
                            whrTest.UseAppointments(intTempVal);
                            if (whrTest.Appointments.IsFreePeriods(prdsUse,NewEvent.WeekFlag))
                            {
                                nWhereAvailable = intTempVal;
                                bPass = true;
                                break;
                            }
                            intTempVal++;
                        }

                        if (!bPass)
                        {
                            ReasonDesc.AssocID = NewEvent.ClassroomID;
                            ReasonDesc.AssocName = Classrooms[NewEvent.ClassroomID].Name;
                            ReasonDesc.AssocType = Constants.lvWhere;
                            ReasonDesc.Desc = "未排節數不足" + System.Environment.NewLine + "無法排課";
                            return Constants.tsWhereConflict;
                        }
                    }
                    else if (!whrTest.Appointments.IsFreePeriods(prdsUse, NewEvent.WeekFlag))
                    {
                        ReasonDesc.AssocID = NewEvent.ClassroomID;
                        ReasonDesc.AssocName = Classrooms[NewEvent.ClassroomID].Name;
                        ReasonDesc.AssocType = Constants.lvWhere;
                        ReasonDesc.Desc = "未排節數不足" + System.Environment.NewLine + "無法排課";
                        return Constants.tsWhereConflict;
                    }
                }
            }
            #endregion

            #region Check WHO time conflict and distance conflict
            List<Teacher> whoTests = new List<Teacher>();
            whoTests.Add(whoTest1);
            whoTests.Add(whoTest2);
            whoTests.Add(whoTest3);

            for (int i = 0; i < whoTests.Count; i++)
            {
                Teacher whoTest = whoTests[i];

                if (whoTest != null)
                {
                    if (whoTest.Capacity > 1)
                    {
                        intTempVal = 0;
                        bPass = false;

                        while (intTempVal < whoTest.Capacity)
                        {
                            whoTest.UseAppointments(intTempVal);
                            if (whoTest.Appointments.CheckWho(prdsUse, Distances, NewEvent.WeekFlag) == 0)
                            {
                                nWhoAvailable = intTempVal;
                                bPass = true;
                                break;
                            }
                            intTempVal++;
                        }

                        if (!bPass)
                        {
                            ReasonDesc.AssocID = NewEvent.GetTeacherID(i+1);
                            ReasonDesc.AssocName = Teachers[NewEvent.GetTeacherID(i+1)].Name;
                            ReasonDesc.AssocType = Constants.lvWho;
                            ReasonDesc.Desc = "未排節數不足" + System.Environment.NewLine + "無法排課";
                            return Constants.tsWhoConflict;
                        }
                    }
                    else
                    {
                        intTempVal = whoTest.Appointments.CheckWho(prdsUse, Distances, NewEvent.WeekFlag);

                        if (intTempVal != 0)
                        {
                            switch (intTempVal)
                            {
                                case 1:
                                    ReasonDesc.AssocID = NewEvent.GetTeacherID(i+1);
                                    ReasonDesc.AssocName = Teachers[NewEvent.GetTeacherID(i+1)].Name;
                                    ReasonDesc.AssocType = Constants.lvWho;
                                    ReasonDesc.Desc = "未排節數不足" + System.Environment.NewLine + "無法排課";
                                    return Constants.tsWhoConflict;
                                case 2:
                                    ReasonDesc.Desc = "通車來不及";
                                    return Constants.tsDistanceFar;
                                default:
                                    ReasonDesc.AssocID = NewEvent.GetTeacherID(i+1);
                                    ReasonDesc.AssocName = Teachers[NewEvent.GetTeacherID(i+1)].Name;
                                    ReasonDesc.AssocType = Constants.lvWho;
                                    ReasonDesc.Desc = "未排節數不足" + System.Environment.NewLine + "無法排課";
                                    return Constants.tsWhoConflict;
                            }
                        }
                    }
                }
            }
            #endregion

            //不連天排課
            #region 若設定連天不排課，則將相同科目的星期加入到清單中
            if (NewEvent.LimitNextDay)
            {
                List<int> WhatIDWeekdays = new List<int>();

                //取得已排課課程分段
                CEvents evtsCourse = CEvents.GetScheduledByCourseID(NewEvent.CourseID);

                //將課程分段的星期加入到列表
                foreach (CEvent evtCourse in evtsCourse)
                {
                    if (!WhatIDWeekdays.Contains(evtCourse.WeekDay) && !evtCourse.EventID.Equals(NewEvent.EventID))
                        WhatIDWeekdays.Add(evtCourse.WeekDay);
                }

                //判斷是否有隔天排課的情況
                if (WhatIDWeekdays.Contains(WeekDay - 1) || WhatIDWeekdays.Contains(WeekDay + 1))
                {
                    ReasonDesc.Desc = "隔日已排" + System.Environment.NewLine + Subjects[NewEvent.SubjectID].Name;
                    return Constants.tsDupWhat;
                }
            }
            #endregion

            //測試群組排課
            if (!string.IsNullOrEmpty(NewEvent.CourseGroup) && TestGroup)
            {
                //取得群組課程分段
                CEvents evtTests = CEvents.GetByTestGroup(NewEvent);

                evtTesteds = new List<GroupCEvent>();

                Periods pprdsUse = prdsUse;
                Class pwhmTest = whmTest;
                Teacher pwhoTest1 = whoTest1;
                Teacher pwhoTest2 = whoTest2;
                Teacher pwhoTest3 = whoTest3;
                Classroom pwhrTest = whrTest;

                //針對每個課程分段測試可否排課
                foreach (CEvent Event in evtTests)
                {
                    int testResult = TestSchedule(Event, WeekDay, PeriodNo, false);

                    if (testResult != 0)
                    {
                        //若是班級衝突的狀況則加以忽略，群組課程一個班級同個時間可以上多門課
                        if (testResult == Constants.tsWhomConflict)
                        {
                            GroupCEvent evtGroupTest = new GroupCEvent();

                            evtGroupTest.evtTested = Event;
                            evtGroupTest.prdsUse = prdsUse;
                            evtGroupTest.whmTest = whmTest;
                            evtGroupTest.whoTest1 = whoTest1;
                            evtGroupTest.whoTest2 = whoTest2;
                            evtGroupTest.whoTest3 = whoTest3;
                            evtGroupTest.whrTest = whrTest;

                            evtTesteds.Add(evtGroupTest);
                        }//否則回傳群組衝突
                        else
                        {
                            evtTesteds = null;

                            return Constants.tsGroupConflict;
                        }
                    }
                    else
                    {
                        GroupCEvent evtGroupTest = new GroupCEvent();

                        evtGroupTest.evtTested = Event;
                        evtGroupTest.prdsUse = prdsUse;
                        evtGroupTest.whmTest = whmTest;
                        evtGroupTest.whoTest1 = whoTest1;
                        evtGroupTest.whoTest2 = whoTest2;
                        evtGroupTest.whoTest3 = whoTest3;
                        evtGroupTest.whrTest = whrTest;

                        evtTesteds.Add(evtGroupTest);
                    }
                }

                prdsUse = pprdsUse;
                whmTest = pwhmTest;
                whoTest1 = pwhoTest1;
                whoTest2 = pwhoTest2;
                whoTest3 = pwhoTest3;
                whrTest = pwhrTest;
            }

            evtTested = NewEvent;

            return 0;
        }

        /// <summary>
        /// 1.安排事件到各個資源，必需先呼叫TestSchedule。
        /// 2.To use AllocEvent, TestSchedule must be called just before AllocEvent
        /// </summary>
        private void AllocEvent(bool AllocGroup)
        {
            #region 宣告變數
            Appointments appsNew;
            Appointment appNew;
            #endregion

            if (evtTested == null) return; //evtTested需不為null才能安排事件
            if (evtTested.WeekDay != 0) return; //evtTested.WeekDay需為0才能安排事件

            #region 準備Appointment安排到各個資源
            appsNew = new Appointments();

            foreach (Period prdMember in prdsUse)
            {
                appNew = new Appointment(
                    prdMember.WeekDay,
                    prdMember.BeginTime,
                    prdMember.Duration,
                    evtTested.WeekFlag,
                    evtTested.EventID,
                    prdMember.LocID,
                    evtTested.SubjectID,
                    string.Empty);

                appsNew.Add(appNew);
            }
            #endregion

            #region 將Appointments安排到Who
            List<Teacher> whoTests = new List<Teacher>();
            whoTests.Add(whoTest1);
            whoTests.Add(whoTest2);
            whoTests.Add(whoTest3);

            for (int i = 0; i < whoTests.Count; i++)
            {
                Teacher whoTest = whoTests[i];

                if (whoTest != null)
                {
                    //若whoTest.Capacity大於1，則根據nWhoAvailable引用行事曆
                    if (whoTest.Capacity > 1)
                        whoTest.UseAppointments(nWhoAvailable);

                    foreach (Appointment appMember in appsNew)
                        whoTest.Appointments.Add(appMember);
                }
            }
            #endregion

            #region 將Appointments安排到Where
            if (whrTest != null)
                if (!whrTest.LocOnly)
                {
                    //若whrTest.Capacity大於1，則根據nWhereAvailable引用行事曆
                    if (whrTest.Capacity > 1)
                        whrTest.UseAppointments(nWhereAvailable);

                    foreach (Appointment appMember in appsNew)
                        whrTest.Appointments.Add(appMember);
                }
            #endregion

            #region 將Appointments安排到Whom
            if (whmTest != null)
                foreach (Appointment appMember in appsNew)
                    whmTest.Appointments.Add(appMember);
            #endregion

            #region Set WeekDay and Period variables
            evtTested.WeekDay = prdsUse[0].WeekDay;
            evtTested.PeriodNo = prdsUse[0].PeriodNo;

            int WeekDayVar;
            int PeriodVar;

            if ((evtTested.WeekDayOp == Constants.opEqual) && !int.TryParse(evtTested.WeekDayVar, out WeekDayVar))
            {
                if (!WeekDayVariables.Exists(evtTested.WeekDayVar))
                {
                    Variable varNew = new Variable(evtTested.WeekDayVar);
                    WeekDayVariables.Add(varNew);
                }
                WeekDayVariables[evtTested.WeekDayVar].AddValue(evtTested.WeekDay);
            }

            if ((evtTested.PeriodOp == Constants.opEqual) && !int.TryParse(evtTested.PeriodVar, out PeriodVar))
            {
                if (!PeriodVariables.Exists(evtTested.PeriodVar))
                {
                    Variable varNew = new Variable(evtTested.PeriodVar);
                    PeriodVariables.Add(varNew);
                }
                PeriodVariables[evtTested.PeriodVar].AddValue(evtTested.PeriodNo);
            }
            #endregion

            #region Clear shared variables
            evtTested = null;
            whrTest  = null;
            whoTest1 = null;
            whoTest2 = null;
            whoTest3 = null;
            whmTest = null;
            prdsUse = null;
            #endregion

            if (AllocGroup && evtTesteds != null)
            {
                foreach (GroupCEvent evtGroupTest in evtTesteds)
                {
                    evtTested = evtGroupTest.evtTested;
                    whrTest = evtGroupTest.whrTest;
                    whoTest1 = evtGroupTest.whoTest1;
                    whoTest2 = evtGroupTest.whoTest2;
                    whoTest3 = evtGroupTest.whoTest3;
                    whmTest = evtGroupTest.whmTest;
                    prdsUse = evtGroupTest.prdsUse;

                    AllocEvent(false);
                }
            }
        }

        /// <summary>
        /// 釋放Event排定的特會
        /// </summary>
        /// <param name="TargetEvent"></param>
        private void ReleaseEvent(CEvent TargetEvent,bool ReleaseGroup)
        {
            if (TargetEvent.WeekDay == 0) return;

            #region 群組釋放課程
            //if (ReleaseGroup)
            //{
            //    CEvents evtsRelease = CEvents.GetByAllocGroup(TargetEvent);

            //    foreach (CEvent evtRelease in evtsRelease)
            //        ReleaseEvent(evtRelease, false);
            //}
            #endregion

            //If TargetEvent.WeekDay = 0 Then Exit Sub

            #region Release Who

            for (int v = 1; v <= 3; v++)
            {
                if (!TargetEvent.GetTeacherID(v).IsNullValue())
                    if (Teachers.Exists(TargetEvent.GetTeacherID(v)))
                    {
                        Teacher whoRemove = Teachers[TargetEvent.GetTeacherID(v)];

                        if (whoRemove.Capacity > 0)
                        {
                            for (int i = 0; i < whoRemove.Capacity; i++)
                            {
                                //切換行事曆
                                whoRemove.UseAppointments(i);
                                //根據Event編號移除對應的約會
                                whoRemove.Appointments.RemoveByID(TargetEvent.EventID);
                            }
                        }
                        else
                            whoRemove.Appointments.RemoveByID(TargetEvent.EventID); //根據Event編號移除對應的約會
                    } 
            }
            #endregion

            #region VB
            //With TargetEvent
            //'Release Who
            //If Not IsNullValue(.WhoID) Then
            //    Set whoRemove = mWhos(CStr(.WhoID))
            //    If whoRemove.Capacity > 0 Then
            //        For nIndex = 1 To whoRemove.Capacity
            //            whoRemove.UseAppointments nIndex
            //            whoRemove.Appointments.RemoveID .EventID
            //        Next nIndex
            //    Else
            //        whoRemove.Appointments.RemoveID .EventID
            //    End If
            //End If
            #endregion

            #region Release Where
            if (!TargetEvent.EventID.IsNullValue())
                if (Classrooms.Exists(TargetEvent.ClassroomID))
                {
                    Classroom whrRemove = Classrooms[TargetEvent.ClassroomID];
                    if (!whrRemove.LocOnly)
                    {
                        if (whrRemove.Capacity > 0)
                        {
                            for (int i = 0; i < whrRemove.Capacity; i++)
                            {
                                //切換行事曆
                                whrRemove.UseAppointments(i);
                                //根據Event編號移除對應的約會
                                whrRemove.Appointments.RemoveByID(TargetEvent.EventID);
                            }
                        }
                        else
                            whrRemove.Appointments.RemoveByID(TargetEvent.EventID); //根據Event編號移除對應的約會
                    }
                }
            #endregion

            #region VB
            //'Release Where
            //If Not IsNullValue(.WhereID) Then
            //    Set whrRemove = mWheres(CStr(.WhereID))
            //    If Not whrRemove.LocOnly Then
            //        If whrRemove.Capacity > 0 Then
            //            For nIndex = 1 To whrRemove.Capacity
            //                whrRemove.UseAppointments nIndex
            //                whrRemove.Appointments.RemoveID .EventID
            //            Next nIndex
            //        Else
            //            whrRemove.Appointments.RemoveID .EventID
            //        End If
            //    End If
            //End If
            #endregion

            if (!TargetEvent.ClassID.IsNullValue())
                if (Classes.Exists(TargetEvent.ClassID))
                    Classes["" + TargetEvent.ClassID].Appointments.RemoveByID(TargetEvent.EventID);

            #region Clear WeekDay variables
            int WeekDay;
            int Period;

            if ((TargetEvent.WeekDayOp == Constants.opEqual) && (!int.TryParse(TargetEvent.WeekDayVar, out WeekDay)))
                WeekDayVariables[TargetEvent.WeekDayVar].DelValue(TargetEvent.WeekDay);

            if ((TargetEvent.PeriodOp == Constants.opEqual) && (!int.TryParse(TargetEvent.PeriodVar, out Period)))
                PeriodVariables[TargetEvent.PeriodVar].DelValue(TargetEvent.PeriodNo);
            #endregion

            TargetEvent.WeekDay = 0;
            TargetEvent.PeriodNo = 0;
        }

        /// <summary>
        /// 根據TargetEvent增加對應Who、Whom及Where的排課總節數
        /// </summary>
        /// <param name="TargetEvent"></param>
        private void IncTotalHour(CEvent TargetEvent)
        {
            if (!TargetEvent.TeacherID1.IsNullValue())
                if (Teachers.Exists(TargetEvent.TeacherID1))
                    Teachers[TargetEvent.TeacherID1].TotalHour += TargetEvent.Length;

            if (!TargetEvent.TeacherID2.IsNullValue())
                if (Teachers.Exists(TargetEvent.TeacherID2))
                    Teachers[TargetEvent.TeacherID2].TotalHour += TargetEvent.Length;

            if (!TargetEvent.TeacherID3.IsNullValue())
                if (Teachers.Exists(TargetEvent.TeacherID3))
                    Teachers[TargetEvent.TeacherID3].TotalHour += TargetEvent.Length;

            if (!TargetEvent.ClassID.IsNullValue())
                if (Classes.Exists(TargetEvent.ClassID))
                    Classes[TargetEvent.ClassID].TotalHour += TargetEvent.Length;

            if (!TargetEvent.ClassroomID.IsNullValue())
                if (Classrooms.Exists(TargetEvent.ClassroomID))
                    Classrooms[TargetEvent.ClassroomID].TotalHour += TargetEvent.Length;
        }

        /// <summary>
        /// 根據TargetEvent減少對應Who、Whom及Where的排課總節數
        /// </summary>
        /// <param name="TargetEvent"></param>
        private void DecTotalHour(CEvent TargetEvent)
        {
            if (!TargetEvent.TeacherID1.IsNullValue())
                if (Teachers.Exists(TargetEvent.TeacherID1))
                    Teachers[TargetEvent.TeacherID1].TotalHour -= TargetEvent.Length;

            if (!TargetEvent.TeacherID2.IsNullValue())
                if (Teachers.Exists(TargetEvent.TeacherID2))
                    Teachers[TargetEvent.TeacherID2].TotalHour -= TargetEvent.Length;

            if (!TargetEvent.TeacherID3.IsNullValue())
                if (Teachers.Exists(TargetEvent.TeacherID3))
                    Teachers[TargetEvent.TeacherID3].TotalHour -= TargetEvent.Length;

            if (!TargetEvent.ClassID.IsNullValue())
                if (Classes.Exists(TargetEvent.ClassID))
                    Classes[TargetEvent.ClassID].TotalHour -= TargetEvent.Length;

            if (!TargetEvent.ClassroomID.IsNullValue())
                if (Classrooms.Exists(TargetEvent.ClassroomID))
                    Classrooms[TargetEvent.ClassroomID].TotalHour -= TargetEvent.Length;
        }

        /// <summary>
        /// 根據TargetEvent增加對應Who、Whom及Where的已排課總節數
        /// </summary>
        /// <param name="TargetEvent"></param>
        private void IncAllocHour(CEvent TargetEvent)
        {
            if (!TargetEvent.TeacherID1.IsNullValue())
                if (Teachers.Exists(TargetEvent.TeacherID1))
                    Teachers[TargetEvent.TeacherID1].AllocHour += TargetEvent.Length;

            if (!TargetEvent.TeacherID2.IsNullValue())
                if (Teachers.Exists(TargetEvent.TeacherID2))
                    Teachers[TargetEvent.TeacherID2].AllocHour += TargetEvent.Length;

            if (!TargetEvent.TeacherID3.IsNullValue())
                if (Teachers.Exists(TargetEvent.TeacherID3))
                    Teachers[TargetEvent.TeacherID3].AllocHour += TargetEvent.Length;

            if (!TargetEvent.ClassID.IsNullValue())
                if (Classes.Exists(TargetEvent.ClassID))
                    Classes[TargetEvent.ClassID].AllocHour += TargetEvent.Length;

            if (!TargetEvent.ClassroomID.IsNullValue())
                if (Classrooms.Exists(TargetEvent.ClassroomID))
                    Classrooms[TargetEvent.ClassroomID].AllocHour += TargetEvent.Length; 
        }

        /// <summary>
        /// 根據TargetEvent減少對應Who、Whom及Where的已排課總節數
        /// </summary>
        /// <param name="TargetEvent"></param>
        private void DecAllocHour(CEvent TargetEvent)
        {
            if (!TargetEvent.TeacherID1.IsNullValue())
                if (Teachers.Exists(TargetEvent.TeacherID1))
                    Teachers[TargetEvent.TeacherID1].AllocHour -= TargetEvent.Length;

            if (!TargetEvent.TeacherID2.IsNullValue())
                if (Teachers.Exists(TargetEvent.TeacherID2))
                    Teachers[TargetEvent.TeacherID2].AllocHour -= TargetEvent.Length;

            if (!TargetEvent.TeacherID3.IsNullValue())
                if (Teachers.Exists(TargetEvent.TeacherID3))
                    Teachers[TargetEvent.TeacherID3].AllocHour -= TargetEvent.Length;

            if (!TargetEvent.ClassID.IsNullValue())
                if (Classes.Exists(TargetEvent.ClassID))
                    Classes[TargetEvent.ClassID].AllocHour -= TargetEvent.Length;

            if (!TargetEvent.ClassroomID.IsNullValue())
                if (Classrooms.Exists(TargetEvent.ClassroomID))
                    Classrooms[TargetEvent.ClassroomID].AllocHour -= TargetEvent.Length;
        }

        /// <summary>
        /// 針對事件，找出事件相關的資源（教師、班級及場地），更新這些資源所影響的事件安排解。
        /// </summary>
        /// <param name="EventID"></param>
        private void UpdateEventSolutionCount(string EventID)
        {
            CEvents evtsCalc = new CEvents();

            evtsCalc.Add(CEvents[EventID]);

            UpdateEventsSolutionCount(evtsCalc);

            #region VB
            //Dim evtsCalc As CEvents
    
            //Set evtsCalc = New CEvents
            //evtsCalc.Add mCEvents(CStr(EventID))
            //UpdateEventsSolutionCount evtsCalc 
            #endregion
        }

        /// <summary>
        /// 針對事件列表，找出事件相關的資源（教師、班級及場地），更新這些資源所影響的事件安排解。
        /// </summary>
        /// <param name="evtsCalc">事件列表</param>
        private void UpdateEventsSolutionCount(CEvents evtsCalc)
        {
            Stopwatch watch = new Stopwatch();

            watch.Start();

            #region 初始化
            Teachers whosAffected = new Teachers();
            Classes whmAffected = new Classes();
            Classrooms whrsAffected = new Classrooms();
            CEvents evtsAffected = new CEvents();
            #endregion

            #region 找出影響事件的相關資源
            foreach (CEvent evtCalc in evtsCalc)
            {
                if (!evtCalc.TeacherID1.IsNullValue())
                    if (Teachers.Exists(evtCalc.TeacherID1))
                        whosAffected.Add(Teachers[evtCalc.TeacherID1]);

                if (!evtCalc.TeacherID2.IsNullValue())
                    if (Teachers.Exists(evtCalc.TeacherID2))
                        whosAffected.Add(Teachers[evtCalc.TeacherID2]);

                if (!evtCalc.TeacherID3.IsNullValue())
                    if (Teachers.Exists(evtCalc.TeacherID3))
                        whosAffected.Add(Teachers[evtCalc.TeacherID3]);

                if (!evtCalc.ClassID.IsNullValue())
                    if (Classes.Exists(evtCalc.ClassID))
                        whmAffected.Add(Classes[evtCalc.ClassID]);

                if (!evtCalc.ClassroomID.IsNullValue())
                    if (Classes.Exists(evtCalc.ClassroomID))
                        whrsAffected.Add(Classrooms[evtCalc.ClassroomID]);
            }
            #endregion

            #region 針對所有的事件，找出與資源（whosAffected、whmAffected及whrsAffected）相關的事件
            foreach (CEvent evtCalc in CEvents)
            {
                if (whosAffected.Exists(evtCalc.TeacherID1) || whosAffected.Exists(evtCalc.TeacherID2) || whosAffected.Exists(evtCalc.TeacherID3))
                    evtsAffected.Add(evtCalc);
                else if (whmAffected.Exists(evtCalc.ClassID))
                    evtsAffected.Add(evtCalc);
                else if (whrsAffected.Exists(evtCalc.ClassroomID))
                    evtsAffected.Add(evtCalc);
            }
            #endregion

            #region 計算影響事件的可能解
            foreach (CEvent evtCalc in evtsAffected)
                evtCalc.SolutionCount = evtCalc.WeekDay == 0 ? GetSolutionCount(evtCalc.EventID) : -1;

            watch.Stop();
            Console.WriteLine(""+watch.Elapsed.TotalMilliseconds);

            if (EventSolCountUpdated != null)
                EventSolCountUpdated(this, new EventSolCountUpdatedEventArgs(evtsCalc));
            #endregion
        }

        /// <summary>
        /// 取得事件在時間表中可被安排的節次數
        /// </summary>
        /// <param name="EventID">事件編號</param>
        /// <returns>可被安排節次數</returns>
        private long GetSolutionCount(string EventID)
        {
            Periods prdsTest = TimeTables[CEvents[EventID].TimeTableID].Periods;
            long nSolutions = 0;

            foreach (Period prdTest in prdsTest)
            {
                if (prdTest.PeriodNo != 0)
                    if (IsSchedulable(EventID, prdTest.WeekDay, prdTest.PeriodNo))
                        nSolutions++;
            }

            return nSolutions;

            #region VB
            //Dim prdsTest As Periods
            //Dim prdTest As Period
            //Dim nSolutions As Long
    
            //Set prdsTest = mTimeTables(CStr(mCEvents(CStr(EventID)).TimeTableID)).Periods
    
            //nSolutions = 0
            //For Each prdTest In prdsTest
            //    With prdTest
            //    If .PeriodNo <> 0 Then
            //        If IsSchedulable(EventID, .WeekDay, .PeriodNo) Then
            //            nSolutions = nSolutions + 1
            //        End If
            //    End If
            //    End With
            //Next prdTest

            //GetSolutionCount = nSolutions
            #endregion
        }

        /// <summary>
        /// 將字串中的空白刪除
        /// </summary>
        /// <param name="strSource">來源字串</param>
        /// <returns>傳回已將空白刪除的字串</returns>
        private string DelSpaces(string strSource)
        {
            return strSource.Replace(" ","");
        }

        #endregion
    }
}