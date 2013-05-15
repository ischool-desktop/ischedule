using System;

namespace Sunset.Data
{
    /// <summary>
    /// 約會，視為單獨資源中已被排定的事件
    /// </summary>
    public class Appointment
    {
        private int mWeekDay;
        private DateTime mBeginTime;
        private int mDuration;
        private byte mWeekFlag;
        private string mEventID;
        private string mLocID;
        private string mWhatID;
        private string mDescription;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="WeekDay">星期幾</param>
        /// <param name="BeginTime">開始時間</param>
        /// <param name="Duration">持續分鐘</param>
        /// <param name="WeekFlag">單雙週，單週為1、單雙週為2、單雙週為3。</param>
        /// <param name="EventID">所屬實體編號，目前可能為教師、場地或班級</param>
        /// <param name="LocID">地點編號</param>
        /// <param name="WhatID">科目編號</param>
        public Appointment(
            int WeekDay,DateTime BeginTime,int Duration,Byte WeekFlag,
            string EventID,string LocID,string WhatID,string Description)
        {
            //初始化時間相關變數
            this.mWeekDay = WeekDay;
            this.mBeginTime = BeginTime;
            this.mDuration = Duration;
            this.mWeekFlag = WeekFlag;
            this.mDescription = Description;

            //初始化資源相關變數
            this.mEventID = EventID;
            this.mLocID = LocID;
            this.mWhatID = WhatID;
        }

        /// <summary>
        /// 事件（CEvent）編號
        /// </summary>
        public string EventID { get { return mEventID; } }

        /// <summary>
        /// 地點編號
        /// </summary>
        public string LocID { get { return mLocID; } }

        /// <summary>
        /// 科目編號
        /// </summary>
        public string WhatID { get { return mWhatID; } }

        /// <summary>
        /// 星期幾
        /// </summary>
        public int WeekDay { get { return mWeekDay; } }

        /// <summary>
        /// 開始時間
        /// </summary>
        public DateTime BeginTime { get { return mBeginTime; } }

        /// <summary>
        /// 持續分鐘
        /// </summary>
        public int Duration { get { return mDuration; } }

        /// <summary>
        /// 單雙週，單週為1、單雙週為2、單雙週為3。
        /// </summary>
        public Byte WeekFlag { get { return mWeekFlag; } }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get { return mDescription; } }

        /// <summary>
        /// 結束時間。
        /// 唯讀，由 BeginTime + Duration 算出。
        /// </summary>
        public DateTime EndTime
        {
            get { return this.BeginTime.AddMinutes(this.Duration); }
        }
    }
}