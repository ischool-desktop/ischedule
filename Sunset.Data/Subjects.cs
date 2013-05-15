using System.Collections.Generic;

namespace Sunset.Data
{
    /// <summary>
    /// 科目集合
    /// </summary>
    public class Subjects : IEnumerable<Subject>
    {
        private Dictionary<string, Subject> mWhats;

        /// <summary>
        /// 建構式
        /// </summary>
        public Subjects()
        {
            //初始化科目集合
            mWhats = new Dictionary<string, Subject>();
        }

        /// <summary>
        /// 新增科目
        /// </summary>
        /// <param name="NewWhat">科目物件</param>
        /// <returns>新增的科目物件，若是沒有新增成功則回傳null。</returns>
        public Subject Add(Subject NewWhat)
        {
            //假設已有包含鍵值就傳回null
            if (mWhats.ContainsKey("" + NewWhat.WhatID))
                return null;

            //將科目加入到集合中
            mWhats.Add("" + NewWhat.WhatID, NewWhat);

            //回傳新增的科目
            return NewWhat;
        }

        /// <summary>
        /// 根據鍵值移除科目
        /// </summary>
        /// <param name="Key">科目編號</param>
        public void Remove(string Key)
        {
            //假設科目集合包含鍵值，就移除該科目
            if (mWhats.ContainsKey(Key))
                mWhats.Remove(Key);
        }

        /// <summary>
        /// 根據科目編號判斷在集合中是否有該科目
        /// </summary>
        /// <param name="WhatID">科目編號</param>
        /// <returns>是否包含</returns>
        public bool Exists(string WhatID)
        {
            return mWhats.ContainsKey("" + WhatID);
        }

        /// <summary>
        /// 根據鍵值從集合中取得科目
        /// </summary>
        /// <param name="Key">科目編號</param>
        /// <returns>科目</returns>
        public Subject this[string Key]
        {
            get 
            { 
                //假設在科目集合中有包含該鍵值，就傳回該科目，否則傳回null
                return mWhats.ContainsKey(Key)?mWhats[Key]:null;
            }
        }

        /// <summary>
        /// 科目集合數量
        /// </summary>
        public int Count { get { return mWhats.Count; } }

        /// <summary>
        /// 清除所有資料
        /// </summary>
        public void Clear()
        {
            mWhats.Clear();
        }

        #region IEnumerable<What> Members

        public IEnumerator<Subject> GetEnumerator()
        {
            return mWhats.Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return mWhats.Values.GetEnumerator();
        }

        #endregion
    }
}