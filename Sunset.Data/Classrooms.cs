using System.Collections.Generic;

namespace Sunset.Data
{
    /// <summary>
    /// 場地集合
    /// </summary>
    public class Classrooms : IEnumerable<Classroom>
    {
        private Dictionary<string,Classroom> mWheres;

        /// <summary>
        /// 建構式
        /// </summary>
        public Classrooms()
        {
            //初始化場地集合
            mWheres = new Dictionary<string, Classroom>();
        }

        /// <summary>
        /// 新增場地
        /// </summary>
        /// <param name="NewWhere">場地物件</param>
        /// <returns>新增的場地物件，若是沒有新增成功則回傳null。</returns>
        public Classroom Add(Classroom NewWhere)
        {
            if (mWheres.ContainsKey(NewWhere.WhereID))
                return null;

            mWheres.Add(NewWhere.WhereID,NewWhere);

            return NewWhere;
        }

        /// <summary>
        /// 根據鍵值移除場地
        /// </summary>
        /// <param name="Key">場地編號</param>
        public void Remove(string Key)
        {
            //假設場地集合包含鍵值，就移除該場地
            if (mWheres.ContainsKey(Key))
                mWheres.Remove(Key);
        }

        /// <summary>
        /// 根據場地編號判斷在集合中是否有該場地
        /// </summary>
        /// <param name="WhereID">場地編號</param>
        /// <returns>是否包含</returns>
        public bool Exists(string WhereID)
        {
            return mWheres.ContainsKey("" + WhereID);
        }

        /// <summary>
        /// 根據鍵值從集合中取得場地
        /// </summary>
        /// <param name="Key">場地編號</param>
        /// <returns>場地</returns>
        public Classroom this[string Key]
        {
            get { return mWheres.ContainsKey(Key)?mWheres[Key]:null; }
        }

        /// <summary>
        /// 場地集合數量
        /// </summary>
        public int Count { get { return mWheres.Count; } }

        /// <summary>
        /// 清除所有資料
        /// </summary>
        public void Clear()
        {
            mWheres.Clear();
        }

        #region IEnumerable<Where> Members

        public IEnumerator<Classroom> GetEnumerator()
        {
            return mWheres.Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return mWheres.Values.GetEnumerator();
        }

        #endregion
    }
}