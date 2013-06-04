using System;
using System.Collections.Generic;

namespace Sunset.Data
{
    /// <summary>
    /// 班級集合
    /// </summary>
    public class Classes : IEnumerable<Class>
    {
        private Dictionary<string, Class> mClasses;

        /// <summary>
        /// 建構式
        /// </summary>
        public Classes()
        {
            //初始化班級集合
            mClasses = new Dictionary<string, Class>();
        }

        /// <summary>
        /// 新增班級
        /// </summary>
        /// <param name="NewClass">班級物件</param>
        /// <returns>新增的班級物件，若是沒有新增成功則回傳null。</returns>
        public Class Add(Class NewClass)
        {
            //假設已有包含鍵值就傳回null
            if (mClasses.ContainsKey("" + NewClass.ClassID))
                return null;

            //將班級加入到集合中
            mClasses.Add("" + NewClass.ClassID, NewClass);

            //回傳新增的班級
            return NewClass;
        }

        /// <summary>
        /// 根據鍵值移除班級
        /// </summary>
        /// <param name="Key">班級編號</param>
        public void Remove(string Key)
        {
            //假設班級集合包含鍵值，就移除該班級
            if (mClasses.ContainsKey(Key))
                mClasses.Remove(Key);
        }

        /// <summary>
        /// 根據班級編號判斷在集合中是否有該班級
        /// </summary>
        /// <param name="WhomID">班級編號</param>
        /// <returns>是否包含</returns>
        public bool Exists(string WhomID)
        {
            return mClasses.ContainsKey(WhomID);
        }

        /// <summary>
        /// 根據鍵值從集合中取得班級
        /// </summary>
        /// <param name="Key">班級編號</param>
        /// <returns>班級</returns>
        public Class this[string Key]
        {
            get 
            {
                //假設在班級集合中有包含該鍵值，就傳回該班級，否則傳回null
                return mClasses.ContainsKey(Key) ? mClasses[Key] : null; 
            }
        }

        /// <summary>
        /// 根據名稱取得班級
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public Class GetByName(string Name)
        {
            foreach (string Key in mClasses.Keys)
            {
                if (mClasses[Key].Name.Equals(Name))
                    return mClasses[Key];
            }

            return null;
        }

        /// <summary>
        /// 清除所有資料
        /// </summary>
        public void Clear()
        {
            mClasses.Clear();
        }

        /// <summary>
        /// 班級集合數量
        /// </summary>
        public int Count { get { return mClasses.Count; } }

        /// <summary>
        /// 有分課的班級數量
        /// </summary>
        public int HasTotalHourCount
        {
            get
            {
                int vHasTotalHourCount = 0;

                foreach (Class vClass in mClasses.Values)
                    if (vClass.TotalHour > 0)
                        vHasTotalHourCount++;

                return vHasTotalHourCount;
            }
        }

        #region IEnumerable<Whom> Members

        public IEnumerator<Class> GetEnumerator()
        {
            return mClasses.Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return mClasses.Values.GetEnumerator();
        }

        #endregion
    }
}