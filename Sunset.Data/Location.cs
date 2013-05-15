
namespace Sunset.Data
{
    /// <summary>
    /// 地點
    /// </summary>
    public class Location
    {
        private string mLocID;
        private string mName;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="LocID">地點編號</param>
        /// <param name="Name">地點名稱</param>
        public Location(string LocID, string Name)
        {
            this.mLocID = LocID;
            this.mName = Name;
        }

        /// <summary>
        /// 地點編號
        /// </summary>
        public string LocID { get { return this.mLocID; } }

        /// <summary>
        /// 地點名稱
        /// </summary>
        public string Name { get { return this.mName; } }
    }
}