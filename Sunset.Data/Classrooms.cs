using System.Collections.Generic;

namespace Sunset.Data
{
    /// <summary>
    /// 場地集合
    /// </summary>
    public class Classrooms : IEnumerable<Classroom>
    {
        private Dictionary<string,Classroom> mClassrooms;

        /// <summary>
        /// 建構式
        /// </summary>
        public Classrooms()
        {
            //初始化場地集合
            mClassrooms = new Dictionary<string, Classroom>();
        }

        /// <summary>
        /// 新增場地
        /// </summary>
        /// <param name="NewWhere">場地物件</param>
        /// <returns>新增的場地物件，若是沒有新增成功則回傳null。</returns>
        public Classroom Add(Classroom NewWhere)
        {
            if (mClassrooms.ContainsKey(NewWhere.ClassroomID))
                return null;

            mClassrooms.Add(NewWhere.ClassroomID,NewWhere);

            return NewWhere;
        }

        /// <summary>
        /// 根據鍵值移除場地
        /// </summary>
        /// <param name="Key">場地編號</param>
        public void Remove(string Key)
        {
            //假設場地集合包含鍵值，就移除該場地
            if (mClassrooms.ContainsKey(Key))
                mClassrooms.Remove(Key);
        }

        /// <summary>
        /// 根據場地編號判斷在集合中是否有該場地
        /// </summary>
        /// <param name="WhereID">場地編號</param>
        /// <returns>是否包含</returns>
        public bool Exists(string WhereID)
        {
            return mClassrooms.ContainsKey("" + WhereID);
        }

        /// <summary>
        /// 根據鍵值從集合中取得場地
        /// </summary>
        /// <param name="Key">場地編號</param>
        /// <returns>場地</returns>
        public Classroom this[string Key]
        {
            get { return mClassrooms.ContainsKey(Key)?mClassrooms[Key]:null; }
        }

        /// <summary>
        /// 場地集合數量
        /// </summary>
        public int Count { get { return mClassrooms.Count; } }

        /// <summary>
        /// 有分課的場地數量
        /// </summary>
        public int HasTotalHourCount
        {
            get
            {
                int vHasTotalHourCount = 0;

                foreach (Classroom vClassroom in mClassrooms.Values)
                    if (vClassroom.TotalHour > 0)
                        vHasTotalHourCount++;

                return vHasTotalHourCount;
            }
        }
        /// <summary>
        /// 清除所有資料
        /// </summary>
        public void Clear()
        {
            mClassrooms.Clear();
        }

        #region IEnumerable<Where> Members

        public IEnumerator<Classroom> GetEnumerator()
        {
            return mClassrooms.Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return mClassrooms.Values.GetEnumerator();
        }

        #endregion
    }
}