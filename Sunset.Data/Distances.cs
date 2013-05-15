using System.Collections.Generic;

namespace Sunset.Data
{
    /// <summary>
    /// 距離集合
    /// </summary>
    public class Distances : IEnumerable<Distance>
    {
        private Dictionary<string,Distance> mDistances;

        /// <summary>
        /// 建構式
        /// </summary>
        public Distances()
        {
            //初始化距離集合
            mDistances = new Dictionary<string, Distance>();
        }

        /// <summary>
        /// 新增距離
        /// </summary>
        /// <param name="NewDistance">距離物件</param>
        /// <returns>新增的科目物件，若是沒有新增成功則回傳null。</returns>
        public Distance Add(Distance NewDistance)
        {
            //距離物件鍵值為來源地點加上目的地點，其值小的在前面
            string sKey = NewDistance.LocA < NewDistance.LocB ? NewDistance.LocA + "-" + NewDistance.LocB : NewDistance.LocB + "-" + NewDistance.LocA;

            //假設已有包含鍵值就傳回null
            if (mDistances.ContainsKey(sKey))
                return null;

            //將距離加入到集合中
            mDistances.Add(sKey, NewDistance);

            //回傳新增的距離
            return NewDistance;
        }

        /// <summary>
        /// 根據鍵值移除距離
        /// </summary>
        /// <param name="Key">距離鍵值</param>
        public void Remove(string Key)
        {
            //假設距離集合包含鍵值，就移除該距離
            if (mDistances.ContainsKey(Key))
                mDistances.Remove(Key);
        }

        /// <summary>
        /// 根據鍵值從集合中取得距離
        /// </summary>
        /// <param name="Key">距離鍵值</param>
        /// <returns></returns>
        public Distance this[string Key]
        {
            get 
            {
                //假設在距離集合中有包含該鍵值，就傳回該距離，否則傳回null
                return mDistances.ContainsKey(Key)?mDistances[Key]:null;
            }
        }

        /// <summary>
        /// 距離集合數量
        /// </summary>
        public int Count
        {
            get { return mDistances.Count; }
        }

        /// <summary>
        /// 根據兩個地點取得距離
        /// </summary>
        /// <param name="LocC">來源地點</param>
        /// <param name="LocD">目的地點</param>
        /// <returns>距離時間，若是集合中未包含『來源地點』及『目的地點』的組合則傳回0。</returns>
        public long GetDistance(string LocC,string LocD)
        {
            //距離物件鍵值為來源地點加上目的地點，其值小的在前面
            string sKey = LocC + "-" + LocD;
            //string sKey = LocC < LocD ? LocC + "-" + LocD : LocD + "-" + LocC;

            //假設在集合中有包含鍵值就回傳距離，否則就回傳0
            return (mDistances.ContainsKey(sKey))? mDistances[sKey].DistanceTime:0;
        }

        /// <summary>
        /// 清除所有資料
        /// </summary>
        public void Clear()
        {
            mDistances.Clear();
        }

        #region IEnumerable<Distance> Members

        public IEnumerator<Distance> GetEnumerator()
        {
            return mDistances.Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return mDistances.Values.GetEnumerator();
        }

        #endregion
    }
}