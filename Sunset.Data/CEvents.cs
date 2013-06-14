using System.Collections.Generic;
using System.Linq;

namespace Sunset.Data
{
    /// <summary>
    /// 事件集合
    /// </summary>
    public class CEvents : IEnumerable<CEvent>
    {
        private List<CEvent> mCEvents;

        /// <summary>
        /// 建構式
        /// </summary>
        public CEvents()
        {
            //初始化集合
            mCEvents = new List<CEvent>();
        }

        /// <summary>
        /// 取得指定教師的課程分段
        /// </summary>
        /// <param name="teacherID"></param>
        /// <returns></returns>
        public List<CEvent> GetByTeacherID(string teacherID)
        {
            List<CEvent> result = new List<CEvent>();

            foreach (CEvent mCEvent in mCEvents)
            {
                if (mCEvent.TeacherID1.Equals(teacherID) ||
                    mCEvent.TeacherID2.Equals(teacherID) ||
                    mCEvent.TeacherID3.Equals(teacherID))
                    result.Add(mCEvent);
            }

            return result;
        }

        /// <summary>
        /// 取得指定場地的課程分段
        /// </summary>
        /// <param name="ClassroomID"></param>
        /// <returns></returns>
        public List<CEvent> GetByClassroomID(string ClassroomID)
        {
            List<CEvent> result = new List<CEvent>();

            foreach (CEvent mCEvent in mCEvents)
            {
                if (mCEvent.ClassroomID.Equals(ClassroomID))
                    result.Add(mCEvent);
            }

            return result;
        }

        /// <summary>
        /// 取得指定班級的課程分段 
        /// </summary>
        /// <param name="classID"></param>
        /// <returns></returns>
        public List<CEvent> GetByClassID(string ClassID)
        {
            List<CEvent> result = new List<CEvent>();

            foreach (CEvent mCEvent in mCEvents)
            {
                if (mCEvent.ClassID.Equals(ClassID))
                    result.Add(mCEvent);
            }

            return result;
        }

        /// <summary>
        /// 根據課程系統編號取得分課
        /// </summary>
        /// <param name="CourseID"></param>
        /// <returns></returns>
        public CEvents GetByCourseID(string CourseID)
        {
            CEvents Events = new CEvents();

            foreach (CEvent Event in mCEvents.FindAll(x => x.CourseID.Equals(CourseID) && x.WeekDay != 0))
            {
                Events.Add(Event);
            }

            return Events;
        }

        public CEvents GetByAllocGroup(CEvent NewEvent)
        {
            CEvents Events = new CEvents();

            if (string.IsNullOrEmpty(NewEvent.CourseGroup))
                return Events;

            //取得相同群組，分段長度一樣，且尚未排課；並且是不同課程
            List<CEvent> GroupEvents = mCEvents
                .FindAll(x =>
                    !x.CourseID.Equals(NewEvent.CourseID) &&
                    x.CourseGroup.Equals(NewEvent.CourseGroup) &&
                    x.Length.Equals(NewEvent.Length) &&
                    x.WeekDay.Equals(NewEvent.WeekDay) &&
                    x.PeriodNo.Equals(NewEvent.PeriodNo) &&
                    x.WeekFlag.Equals(NewEvent.WeekFlag) &&
                    x.WeekDayCondition.Equals(NewEvent.WeekDayCondition) &&
                    x.PeriodCondition.Equals(NewEvent.PeriodCondition) &&
                    x.AllowLongBreak.Equals(NewEvent.AllowLongBreak)
                    );

            List<string> CourseIDs = new List<string>();

            //將課程群組加入到結果中，而且一個課程只能有一個
            foreach (CEvent Event in GroupEvents)
            {
                if (!CourseIDs.Contains(Event.CourseID))
                {
                    Events.Add(Event);
                    CourseIDs.Add(Event.CourseID);
                }
            }

            return Events;
        }

        /// <summary>
        /// 根據分課取得相同群組的分課，若有相同課程一樣長度的分課，則取得第一個。
        /// </summary>
        /// <param name="NewEvent"></param>
        /// <returns></returns>
        public CEvents GetByTestGroup(CEvent NewEvent)
        {
            CEvents Events = new CEvents();

            if (string.IsNullOrEmpty(NewEvent.CourseGroup))
                return Events;

            //取得相同群組，分段長度一樣，且尚未排課或排在一起；並且是不同課程
            List<CEvent> GroupEvents = mCEvents
                .FindAll(x =>
                    !x.CourseID.Equals(NewEvent.CourseID) &&
                    x.CourseGroup.Equals(NewEvent.CourseGroup) &&
                    x.Length.Equals(NewEvent.Length) &&
                    (x.WeekDay.Equals(0) || (x.WeekDay.Equals(NewEvent.WeekDay) && x.PeriodNo.Equals(NewEvent.PeriodNo))) &&
                    x.WeekFlag.Equals(NewEvent.WeekFlag) &&
                    x.WeekDayCondition.Equals(NewEvent.WeekDayCondition) &&
                    x.PeriodCondition.Equals(NewEvent.PeriodCondition) &&
                    x.AllowLongBreak.Equals(NewEvent.AllowLongBreak));

            List<string> CourseIDs = new List<string>();

            //將課程群組加入到結果中，而且一個課程只能有一個
            foreach (CEvent Event in GroupEvents)
            {
                if (!CourseIDs.Contains(Event.CourseID))
                {
                    Events.Add(Event);
                    CourseIDs.Add(Event.CourseID);
                }
            }

            return Events;
        }

        /// <summary>
        /// 新增事件
        /// </summary>
        /// <param name="NewEvent">事件物件</param>
        /// <returns>新增的事件物件，若是沒有新增成功則回傳null。</returns>
        public CEvent Add(CEvent NewEvent)
        {
            if (mCEvents.Select(x => x.EventID).Contains(NewEvent.EventID))
                return null;

            mCEvents.Add(NewEvent);

            return NewEvent;
        }

        /// <summary>
        /// 新增事件到指定事件編號位置之前
        /// </summary>
        /// <param name="NewEvent">事件</param>
        /// <param name="EventID">事件編號</param>
        /// <returns>新增的事件物件，若是沒有新增成功則回傳null。</returns>
        public CEvent AddBefore(CEvent NewEvent, string EventID)
        {
            if (mCEvents.Select(x => x.EventID).Contains(NewEvent.EventID))
                return null;

            int Index = mCEvents.FindIndex(x => x.EventID == EventID);

            if (Index >= 0)
                mCEvents.Insert(Index, NewEvent);
            else
                mCEvents.Add(NewEvent);

            return NewEvent;
        }

        /// <summary>
        /// 根據索引值移除事件
        /// </summary>
        /// <param name="Index">索引值</param>
        public void Remove(int Index)
        {
            //判斷索引值是否超出範圍
            if (mCEvents.Count > 0)
                if (Index >= 0 && Index < mCEvents.Count)
                    mCEvents.RemoveAt(Index);
        }

        /// <summary>
        /// 根據事件編號移除事件
        /// </summary>
        /// <param name="EventID">事件編號</param>
        public void RemoveID(string EventID)
        {
            mCEvents.RemoveAll(x => x.EventID == EventID);
        }

        /// <summary>
        /// 清空集合
        /// </summary>
        public void Clear()
        {
            mCEvents.Clear();
            mCEvents = new List<CEvent>();
        }

        /// <summary>
        /// 根據事件編號判斷事件是否存在
        /// </summary>
        /// <param name="EventID">事件編號</param>
        /// <returns>是否存在</returns>
        public bool Exists(string EventID)
        {
            return mCEvents.Find(x => x.EventID == EventID) != null;
        }

        /// <summary>
        /// 根據事件編號取得事件
        /// </summary>
        /// <param name="Key">事件編號</param>
        /// <returns>事件物件，若是不存在則傳回null。</returns>
        public CEvent this[string Key]
        {
            get
            {
                return mCEvents.Find(x => x.EventID.Equals(Key));
            }
        }

        /// <summary>
        /// 根據索引取得事件
        /// </summary>
        /// <param name="Index">索引</param>
        /// <returns>事件物件，若是不存在則傳回null。</returns>
        public CEvent this[int Index]
        {
            get
            {
                return Index >= 0 && Index < mCEvents.Count ? mCEvents[Index] : null;
            }
        }

        /// <summary>
        /// 事件集合數量
        /// </summary>
        public int Count { get { return mCEvents.Count; } }

        #region IEnumerable<CEvent> Members

        public IEnumerator<CEvent> GetEnumerator()
        {
            return mCEvents.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return mCEvents.GetEnumerator();
        }

        #endregion
    }
}