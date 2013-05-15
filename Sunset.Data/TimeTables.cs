using System.Collections.Generic;
using System.Linq;

namespace Sunset.Data
{
    /// <summary>
    /// 時間表集合
    /// </summary>
    public class TimeTables : IEnumerable<TimeTable>
    {
        private Dictionary<string,TimeTable> mTimeTables;

        /// <summary>
        /// 建構式
        /// </summary>
        public TimeTables()
        {
            //初始化時間表集合
            mTimeTables = new Dictionary<string, TimeTable>();
        }

        /// <summary>
        /// 根據時間表系統編號尋找位置，用來顯示時間表顏色用
        /// </summary>
        /// <param name="vTimeTableID">時間表系統編號</param>
        /// <returns></returns>
        public int FindIndexByID(string vTimeTableID)
        {
            int Index = 1;

            foreach (string TimeTableID in mTimeTables.Keys)
            {
                if (TimeTableID.Equals(vTimeTableID))
                    return Index;

                Index++;
            }

            return 0;
        }

        /// <summary>
        /// 新增時間表
        /// </summary>
        /// <param name="NewTimeTable">時間表物件</param>
        /// <returns>新增的時間表物件，若是沒有新增成功則回傳null。</returns>
        public TimeTable Add(TimeTable NewTimeTable)
        {
            //假設已有包含鍵值就傳回null
            if (mTimeTables.ContainsKey("" + NewTimeTable.TimeTableID))
                return null;

            //將時間表加入到集合中
            mTimeTables.Add("" + NewTimeTable.TimeTableID,NewTimeTable);

            //回傳新增的科目
            return NewTimeTable;
        }

        /// <summary>
        /// 根據時間表編號判斷在集合中是否有該科目
        /// </summary>
        /// <param name="TimeTableID">時間表編號</param>
        /// <returns>是否包含</returns>
        public bool Exists(string TimeTableID)
        {
            return mTimeTables.ContainsKey("" + TimeTableID);
        }

        /// <summary>
        /// 根據鍵值移除時間表
        /// </summary>
        /// <param name="Key"></param>
        public void Remove(string Key)
        {
            //假設時間表集合包含鍵值，就移除該科目
            if (mTimeTables.ContainsKey(Key))
                mTimeTables.Remove(Key); 
        }

        /// <summary>
        /// 根據鍵值從集合中取得時間表
        /// </summary>
        /// <param name="Key">時間表編號</param>
        /// <returns></returns>
        public TimeTable this[string Key]
        {
            get
            {
                //假設在時間表集合中有包含該鍵值，就傳回該時間表，否則傳回null
                return mTimeTables.ContainsKey(Key) ? mTimeTables[Key] : null;
            }
        }

        /// <summary>
        /// 根據索引從集合中取得時間表
        /// </summary>
        /// <param name="Index">時間表索引</param>
        /// <returns></returns>
        public TimeTable this[int Index]
        {
            get
            {
                if (Index >= 0 && Index < mTimeTables.Values.Count)
                    return mTimeTables.Values.ToList()[Index];

                return null;
            } 
        }

        /// <summary>
        /// 時間表集合數量
        /// </summary>
        public int Count { get { return mTimeTables.Count; } }

        /// <summary>
        /// 清除所有資料
        /// </summary>
        public void Clear()
        {
            mTimeTables.Clear();
        }

        #region IEnumerable<TimeTable> Members

        public IEnumerator<TimeTable> GetEnumerator()
        {
            return mTimeTables.Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return mTimeTables.Values.GetEnumerator();
        }

        #endregion
    }
}