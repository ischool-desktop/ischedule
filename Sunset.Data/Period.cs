using System;

namespace Sunset.Data
{
    /// <summary>
    /// 節次
    /// </summary>
    public class Period
    {
        int mWeekDay;
        int mPeriodNo;
        int mDisplayPeriod;
        DateTime mBeginTime;
        int mDuration;
        string mLocID;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="WeekDay">星期幾</param>
        /// <param name="PeriodNo">節次編號</param>
        /// <param name="DispPeriod">節次顯示</param>
        /// <param name="BeginTime">開始時間</param>
        /// <param name="Duration">持續時間</param>
        public Period(int WeekDay,int PeriodNo,int DispPeriod,DateTime BeginTime,int Duration,string LocID,bool Disable,string DisableMessage)
        {
            //初始化設定
            this.mWeekDay = WeekDay;
            this.mPeriodNo = PeriodNo;
            this.mDisplayPeriod = DispPeriod;
            this.mBeginTime = BeginTime;
            this.mDuration = Duration;
            this.mLocID = LocID;
            this.Disable = Disable;
            this.DisableMessage = DisableMessage;
        }

        /// <summary>
        /// 星期幾
        /// </summary>
        public int WeekDay { get { return mWeekDay; } }

        /// <summary>
        /// 節次編號
        /// </summary>
        public int PeriodNo { get { return mPeriodNo; } }

        /// <summary>
        /// 顯示節次，當各時間表混合列印時所用的節次
        /// </summary>
        public int DisplayPeriod { get { return mDisplayPeriod; } }

        /// <summary>
        /// 開始時間
        /// </summary>
        public DateTime BeginTime { get { return mBeginTime; } }

        /// <summary>
        /// 持續時間, 以分鐘計算。
        /// </summary>
        public int Duration { get { return mDuration; } }

        /// <summary>
        /// 是否不排課
        /// </summary>
        public bool Disable { get; set; }

        /// <summary>
        /// 不排課訊息
        /// </summary>
        public string DisableMessage { get; set; }

        /// <summary>
        /// 地點編號
        /// </summary>
        public string LocID { get { return mLocID; } }

        public override string ToString()
        {
             return "WeekDay:"+WeekDay+",PeriodNo:"+PeriodNo+",DispPeriod:"+DisplayPeriod+",BeginTime:"+BeginTime.ToShortTimeString()+",Duration:"+Duration+",LocID:"+LocID;
        }

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