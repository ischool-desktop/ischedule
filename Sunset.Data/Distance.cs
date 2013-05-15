
namespace Sunset.Data
{
    /// <summary>
    /// 距離
    /// </summary>
    public class Distance
    {
        private long mLocA;
        private long mLocB;
        private long mDistanceTime;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="LocA">來源地點</param>
        /// <param name="LocB">目的地點</param>
        /// <param name="DistanceTime">地點間時間</param>
        public Distance(long LocA,long LocB,long DistanceTime)
        {
            mLocA = LocA;
            mLocB = LocB;
            mDistanceTime = DistanceTime;
        }

        /// <summary>
        /// 來源地點
        /// </summary>
        public long LocA { get { return mLocA; } }

        /// <summary>
        /// 目的地點
        /// </summary>
        public long LocB { get { return mLocB; } }

        /// <summary>
        /// 地點間時間（分鐘）
        /// </summary>
        public long DistanceTime { get { return mDistanceTime; } }
    }
}