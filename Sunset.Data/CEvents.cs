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
        private Dictionary<string, CEvent> dicAllEvents;    //Dictionary<courseSectionID, CEvent>
        private Dictionary<string, List<CEvent>> dicCourseEvents;  //Dictionary<courseID, List<CEvents>>
        private Dictionary<string, List<CEvent>> dicTeacher1Events;  //Dictionary<teacherID, List<CEvents>>
        private Dictionary<string, List<CEvent>> dicTeacher2Events;
        private Dictionary<string, List<CEvent>> dicTeacher3Events;
        private Dictionary<string, List<CEvent>> dicClassEvents;  //Dictionary<classID, List<CEvents>>
        private Dictionary<string, List<CEvent>> dicPlaceEvents;  //Dictionary<placeID, List<CEvents>>

        /// <summary>
        /// 建構式
        /// </summary>
        public CEvents()
        {
            //初始化集合
            mCEvents = new List<CEvent>();

            this.dicAllEvents = new Dictionary<string, CEvent>();
            this.dicClassEvents = new Dictionary<string, List<CEvent>>();
            this.dicCourseEvents = new Dictionary<string, List<CEvent>>();
            this.dicPlaceEvents = new Dictionary<string, List<CEvent>>();
            this.dicTeacher1Events = new Dictionary<string, List<CEvent>>();
            this.dicTeacher2Events = new Dictionary<string, List<CEvent>>();
            this.dicTeacher3Events = new Dictionary<string, List<CEvent>>();
        }

        /* 取得指定課程的 課程分段 */
        public List<CEvent> GetEventsByCourseID(string courseID)
        {
            return this.getEventsByKey(this.dicCourseEvents, courseID);
        }

        /* 取得指定教師的 課程分段 */
        public List<CEvent> GetEventsByTeacherID(string teacherID)
        {
            List<CEvent> Teacher1Events = this.getEventsByKey(this.dicTeacher1Events, teacherID);

            List<CEvent> Teacher2Events = this.getEventsByKey(this.dicTeacher2Events, teacherID);

            List<CEvent> Teacher3Events = this.getEventsByKey(this.dicTeacher3Events, teacherID);

            return Teacher1Events.Union(Teacher2Events).Union(Teacher3Events).ToList();
        }
        /* 取得指定場地的 課程分段 */
        public List<CEvent> GetEventsByClassroomID(string placeID)
        {
            return this.getEventsByKey(this.dicPlaceEvents, placeID);
        }
        /* 取得指定班級的 課程分段 */
        public List<CEvent> GetEventsByClassID(string classID)
        {
            return this.getEventsByKey(this.dicClassEvents, classID);
        }

        private List<CEvent> getEventsByKey(Dictionary<string, List<CEvent>> dicSource, string key)
        {
            List<CEvent> result = null;
            if (dicSource.ContainsKey(key))
                result = dicSource[key];
            else
                result = new List<CEvent>();

            return result;
        }

        public CEvents GetScheduledByCourseID(string CourseID)
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

            //Add By CourseID
            this.addByKey(this.dicCourseEvents, NewEvent.CourseID, NewEvent);

            //Add By TeacherID
            this.addByKey(this.dicTeacher1Events, NewEvent.TeacherID1, NewEvent);

            this.addByKey(this.dicTeacher2Events, NewEvent.TeacherID2, NewEvent);

            this.addByKey(this.dicTeacher3Events, NewEvent.TeacherID3, NewEvent);

            //Add By ClassID
            this.addByKey(this.dicClassEvents, NewEvent.ClassID, NewEvent);

            //Add By PlaceID
            this.addByKey(this.dicPlaceEvents, NewEvent.ClassroomID, NewEvent);

            return NewEvent;
        }

        private void addByKey(Dictionary<string, List<CEvent>> dicTarget, string key, CEvent newEvent)
        {
            if (!dicTarget.ContainsKey(key))
            {
                dicTarget.Add(key, new List<CEvent>());
            }
            dicTarget[key].Add(newEvent);
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