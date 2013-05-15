using System.Collections.Generic;

namespace Sunset.Data
{
    /// <summary>
    /// 地點集合
    /// </summary>
    public class Locations : IEnumerable<Location>
    {
        private Dictionary<string, Location> mLocations;

        /// <summary>
        /// 建構式
        /// </summary>
        public Locations()
        {
            //初始化地點集合
            mLocations = new Dictionary<string, Location>();
        }

        /// <summary>
        /// 新增地點
        /// </summary>
        /// <param name="NewLocation">地點物件</param>
        /// <returns>新增的地點物件，若是沒有新增成功則回傳null。</returns>
        public Location Add(Location NewLocation)
        {
            //假設已有包含鍵值就傳回null
            if (mLocations.ContainsKey(""+NewLocation.LocID))
                return null;

            //將地點加入到集合中
            mLocations.Add(""+NewLocation.LocID, NewLocation);

            //回傳新增的地點
            return NewLocation;
        }

        /// <summary>
        /// 根據鍵值移除地點
        /// </summary>
        /// <param name="Key">地點編號</param>
        public void Remove(string Key)
        {
            mLocations.Remove(Key);
        }

        /// <summary>
        /// 根據地點編號判斷在集合中是否有該地點
        /// </summary>
        /// <param name="LocID">地點編號</param>
        /// <returns>是否包含</returns>
        public bool Exists(long LocID)
        {
            return mLocations.ContainsKey("" + LocID);
        }

        /// <summary>
        /// 根據鍵值從集合中取得場地
        /// </summary>
        /// <param name="Key">場地編號</param>
        /// <returns></returns>
        public Location this[string Key]
        {
            get
            {
                //假設在場地集合中有包含該鍵值，就傳回該場地，否則傳回null
                return mLocations.ContainsKey(Key) ? mLocations[Key] : null;
            }
        }

        /// <summary>
        /// 場地集合數量
        /// </summary>
        public int Count { get { return mLocations.Count; } }

        /// <summary>
        /// 清除所有地點
        /// </summary>
        public void Clear()
        {
            mLocations.Clear();
        }

        #region IEnumerable<Location> Members

        public IEnumerator<Location> GetEnumerator()
        {
            return mLocations.Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return mLocations.Values.GetEnumerator();
        }

        #endregion
    }
}