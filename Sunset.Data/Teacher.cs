using System.Collections.Generic;
using Sunset.Data.Integration;

namespace Sunset.Data
{
    /// <summary>
    /// 教師
    /// <remarks>
    /// 教師沒有時間表的概念，時間表的概念在班級中定義。
    /// </remarks>
    /// </summary>
    public class Teacher
    {
        private string mWhoID;
        private string mName;
        private int mCapacity;
        private int? mBasicLength;
        private int? mExtraLength;
        private int? mCounselingLength;
        private string mComment;
        private List<Appointments> mAppointmentsList;
        private Appointments mAppointments;

        private Dictionary<int, List<Appointment>> dicBusyAppointments = new Dictionary<int,List<Appointment>>();   //<weekday, List<Appointment>> 專記錄不排課時段的 Appointment
        private Dictionary<string, List<Period>> dicBusyPeriods = new Dictionary<string,List<Period>>();  //<TimetableID, List<Period>> 

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="WhoID">教師編號</param>
        /// <param name="Name">教師名稱</param>
        /// <param name="Capacity">行事曆數量</param>
        public Teacher(string WhoID, string Name, int Capacity, int? BasicLength, int? ExtraLength, int? CounselingLength,string Comment)
        {
            //指定教師編號及姓名
            this.mWhoID = WhoID;
            this.mName = Name;
            this.mBasicLength = BasicLength;
            this.mExtraLength = ExtraLength;
            this.mCounselingLength = CounselingLength;
            this.mComment = Comment;

            //若是行事曆數量小於1，那麼設行事曆數量為1，也就是教師一定會有預設的行事曆
            if (Capacity < 1)
                Capacity = 1;

            this.mCapacity = Capacity;

            //新增行事曆列表物件
            mAppointmentsList = new List<Appointments>();

            //根據行事曆數量新增行事曆
            for (int i = 0; i < this.mCapacity; i++)
                mAppointmentsList.Add(new Appointments());

            //設定使用中的行事曆為第一個行事曆（從0開始）
            mAppointments = mAppointmentsList[0];

            this.SourceIDs = new List<SourceID>();
        }

        /// <summary>
        /// 切換行事曆
        /// </summary>
        /// <param name="nWhich">行事曆索引</param>
        public void UseAppointments(int nWhich)
        {
            //若是索引小於0，那麼設為0
            if (nWhich < 0) nWhich = 0;

            //若是索引大於等於行事曆數量，將索引設為行事曆數量減1（C#索引從0開始）
            if (nWhich >= mCapacity) nWhich = mCapacity - 1;

            //切換使用中的行事歷（根據nWhich變數）
            mAppointments = mAppointmentsList[nWhich];
        }

        /// <summary>
        /// 教師編號
        /// </summary>
        public string WhoID { get { return mWhoID; } }

        /// <summary>
        /// 教師姓名
        /// </summary>
        public string Name { get { return mName; } }

        /// <summary>
        /// 行事曆數量
        /// </summary>
        public int Capacity { get { return mCapacity; } }

        /// <summary>
        /// 教學總時數 
        /// </summary>
        public int TotalHour { get; set; }

        /// <summary>
        /// 已排課時數
        /// </summary>
        public int AllocHour { get; set; }

        /// <summary>
        /// 基本授課時數
        /// </summary>
        public int? BasicLength { get { return mBasicLength; } }

        /// <summary>
        /// 兼課時數
        /// </summary>
        public int? ExtraLength { get { return mExtraLength; } }

        /// <summary>
        /// 輔導時數
        /// </summary>
        public int? CounselingLength { get { return mCounselingLength; } }

        /// <summary>
        /// 註解
        /// </summary>
        public string Comment { get { return mComment; } }

        /// <summary>
        /// 約會集合，相當於行事曆
        /// </summary>
        public Appointments Appointments { get { return mAppointments; } }

        /// <summary>
        /// 教師來源DSNS及系統編號
        /// </summary>
        public List<SourceID> SourceIDs { get; set; }


        /// <summary>
        /// 取得不排課時段
        /// idea : 當 Appointment.EventID 為空值時，就表示是不排課時段。
        /// </summary>
        public Dictionary<int, List<Appointment>> GetBusyAppointmentsGroupByWeekday()
        {  
            this.dicBusyAppointments = new Dictionary<int, List<Appointment>>();
            foreach (Appointment app in this.mAppointmentsList[0])
            {
                //如果 eventid 是空值，代表不排課時段所造成的appointment
                if (string.IsNullOrEmpty(app.EventID))
                {
                    if (!this.dicBusyAppointments.ContainsKey(app.WeekDay))
                        this.dicBusyAppointments[app.WeekDay] = new List<Appointment>();

                    this.dicBusyAppointments[app.WeekDay].Add(app);
                }
            }
            
            return this.dicBusyAppointments;
        }

        /// <summary>
        /// 取得不忙碌時段
        /// </summary>
        /// <returns></returns>
        public List<Appointment> GetBusyAppointments()
        {
            List<Appointment> Apps = new List<Appointment>();

            foreach (Appointment App in this.mAppointmentsList[0])
            {
                //如果 eventid 是空值，代表不排課時段所造成的appointment
                if (string.IsNullOrEmpty(App.EventID))
                    Apps.Add(App);
            }

            return Apps;
        }

        /// <summary>
        /// 重新整理教師不排課時段。
        /// 當使用者調整教師的不排課時段時，建議呼叫此方法以重新整理記憶體中的資料。
        /// </summary>
        public void RefreshBusyAppintments()
        {
            List<string> timeTables = new List<string>();
            foreach (string timeTableID in this.dicBusyPeriods.Keys)
            {
                timeTables.Add(timeTableID);
            }
            this.dicBusyPeriods.Clear();
            foreach (string timeTableID in timeTables)
            {
                this.GetBusyPeriods(timeTableID);
            }
        }

        /// <summary>
        /// 取得這位教師在指定上課時間表的不排課節次
        /// 如果曾經取過，就會 cache 起來。
        /// </summary>
        /// <param name="TimeTableID"></param>
        /// <returns></returns>
        public List<Period> GetBusyPeriods(string TimeTableID)
        {  
            if (!this.dicBusyPeriods.ContainsKey(TimeTableID))
            {
                this.dicBusyPeriods[TimeTableID] = new List<Period>();
                TimeTable tbl = Scheduler.Instance.TimeTables[TimeTableID];
                if (tbl != null)
                {
                    Dictionary<int, List<Appointment>> busyAppointments = this.GetBusyAppointmentsGroupByWeekday();
                    foreach (Period prd in tbl.Periods)
                    {
                        if (busyAppointments.ContainsKey(prd.WeekDay))
                        {
                            foreach (Appointment apmt in busyAppointments[prd.WeekDay])
                            {
                                bool isAvalable = (apmt.BeginTime >= prd.EndTime && apmt.EndTime <= prd.BeginTime);
                                if (!isAvalable)
                                    this.dicBusyPeriods[TimeTableID].Add(prd);
                            }
                        }
                    }
                }
            }
            return this.dicBusyPeriods[TimeTableID];
        }
    }
}