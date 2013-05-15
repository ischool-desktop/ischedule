
namespace Sunset.Data
{
    /// <summary>
    /// 時間表
    /// </summary>
    public class TimeTable
    {
        private string mTimeTableID;
        private string mName;
        private Periods mPeriods;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="TimeTableID">時間表編號</param>
        /// <param name="Name">時間表名稱</param>
        public TimeTable(string TimeTableID,string Name)
        {
            //指定時間表編號
            this.mTimeTableID = TimeTableID;
            this.mName = Name;

            //初始化節次集合
            mPeriods = new Periods();
        }

        /// <summary>
        /// 時間表編號
        /// </summary>
        public string TimeTableID { get { return mTimeTableID; } }

        /// <summary>
        /// 時間表名稱
        /// </summary>
        public string Name { get { return mName; } }

        /// <summary>
        /// 節次集合
        /// </summary>
        public Periods Periods {get {return mPeriods;}}
    }
}