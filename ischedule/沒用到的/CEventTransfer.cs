//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using Sunset.Data;

//namespace ischedule
//{
//    internal class CEventTransfer : INotifyPropertyChanged
//    {
//        private string mLock;
//        private string mSolutionCount;
//        private string mWhoName;
//        private string mWhomName;
//        private string mWhatName;
//        private string mWhatAliasName;
//        private string mCourseName;
//        private string mWhereName;
//        private int mWeekDay;
//        private int mPeriodNo;
//        private int mLength;
//        private string mWeekDayCondition;
//        private string mPeriodCondition;
//        private string mAllowLongBreak;
//        private string mAllowDuplicate;
//        private string mWeekFlag;
//        private string mLimitNextDay;
//        private string mCourseGroup;
//        private int mPriority;
//        private string mTimeTableName;
//        private string mEventID;
//        private int mColorIndex;

//        /// <summary>
//        /// 顯示鎖定
//        /// </summary>
//        public string Lock 
//        {
//            get { return mLock;}

//            set 
//            {
//                mLock = value; 
//                NotifyPropertyChanged("Lock");
//            }
//        }

//        /// <summary>
//        /// 顯示解決方案
//        /// </summary>
//        public string SolutionCount
//        {
//            get { return mSolutionCount;}

//            set 
//            {
//                mSolutionCount = value;
//                NotifyPropertyChanged("SolutionCount");
//            }
//        }

//        /// <summary>
//        /// 教師
//        /// </summary>
//        public string WhoName
//        {
//            get { return mWhoName; }

//            set
//            {
//                mWhoName = value;
//                NotifyPropertyChanged("WhoName");
//            }
//        }

//        /// <summary>
//        /// 班級
//        /// </summary>
//        public string WhomName
//        {
//            get { return mWhomName; }

//            set
//            {
//                mWhomName = value;
//                NotifyPropertyChanged("WhomName");
//            }
//        }

//        /// <summary>
//        /// 科目
//        /// </summary>
//        public string WhatName 
//        {
//            get { return mWhatName;}

//            set 
//            {
//                mWhatName = value;
//                NotifyPropertyChanged("WhatName");
//            }
//        }

//        /// <summary>
//        /// 科目簡稱
//        /// </summary>
//        public string WhatAliasName
//        {
//            get { return mWhatAliasName;}
            
//            set 
//            {
//                mWhatAliasName = value;
//                NotifyPropertyChanged("WhatAliasName");
//            }
//        }

//        /// <summary>
//        /// 課程名稱
//        /// </summary>
//        public string CourseName 
//        { 
//            get { return mCourseName;}

//            set
//            {
//                mCourseName = value;
//                NotifyPropertyChanged("CourseName");
//            }
//        }

//        /// <summary>
//        /// 場地
//        /// </summary>
//        public string WhereName
//        {
//            get { return mWhereName;}

//            set
//            {
//                mWhereName = value;
//                NotifyPropertyChanged("WhereName");
//            }
//        }

//        /// <summary>
//        /// 星期
//        /// </summary>
//        public int WeekDay
//        {
//            get { return mWeekDay; }

//            set 
//            {
//                 mWeekDay = value;
//                 NotifyPropertyChanged("WeekDay");
//            }
//        }

//        /// <summary>
//        /// 節次
//        /// </summary>
//        public int PeriodNo
//        {
//            get { return mPeriodNo; }

//            set
//            {
//                mPeriodNo = value;
//                NotifyPropertyChanged("PeriodNo");
//            }
//        }

//        /// <summary>
//        /// 節數
//        /// </summary>
//        public int Length
//        {
//            get { return mLength; }

//            set
//            {
//                mLength = value;
//                NotifyPropertyChanged("Length");
//            }
//        }

//        /// <summary>
//        /// 星期條件
//        /// </summary>
//        public string WeekDayCondition
//        {
//            get { return mWeekDayCondition;}

//            set 
//            {
//                mWeekDayCondition = value;
//                NotifyPropertyChanged("WeekDayCondition");
//            }
//        }

//        /// <summary>
//        /// 節次條件
//        /// </summary>
//        public string PeriodCondition
//        {
//            get { return mPeriodCondition;}

//            set
//            {
//                mPeriodCondition = value;
//                NotifyPropertyChanged("PeriodCondition");
//            }
//        }

//        /// <summary>
//        /// 顯示跨中午
//        /// </summary>
//        public string AllowLongBreak
//        {
//            get { return mAllowLongBreak; }

//            set 
//            {
//                mAllowLongBreak = value;
//                NotifyPropertyChanged("AllowLongBreak");
//            }
//        }

//        /// <summary>
//        /// 顯示重覆
//        /// </summary>
//        public string AllowDuplicate
//        {
//            get { return mAllowDuplicate; }

//            set
//            {
//                mAllowDuplicate = value;
//                NotifyPropertyChanged("AllowDuplicate");
//            }
//        }

//        /// <summary>
//        /// 顯示單雙週
//        /// </summary>
//        public string WeekFlag 
//        {
//            get { return mWeekFlag; }

//            set
//            {
//                mWeekFlag = value;
//                NotifyPropertyChanged("WeekFlag"); 
//            }
//        }

//        /// <summary>
//        /// 不連天排課
//        /// </summary>
//        public string LimitNextDay
//        {
//            get { return mLimitNextDay; }

//            set
//            {
//                mLimitNextDay = value;
//                NotifyPropertyChanged("LimitNextDay");
//            }
//        }

//        /// <summary>
//        /// 課程群組
//        /// </summary>
//        public string CourseGroup
//        {
//            get { return mCourseGroup; }

//            set
//            {
//                mCourseGroup = value;
//                NotifyPropertyChanged("CourseGroup");
//            }
//        }

//        /// <summary>
//        /// 優先
//        /// </summary>
//        public int Priority
//        {
//            get { return mPriority; }

//            set
//            {
//                mPriority = value;
//                NotifyPropertyChanged("Priority");
//            }
//        }

//        /// <summary>
//        /// 時間表名稱
//        /// </summary>
//        public string TimeTableName
//        {
//            get { return mTimeTableName; }

//            set
//            {
//                mTimeTableName = value;
//                NotifyPropertyChanged("TimeTableName");
//            }
//        }

//        /// <summary>
//        /// 排課事件系統編號
//        /// </summary>
//        public string EventID
//        {
//            get { return mEventID; }
            
//            set
//            {
//                mEventID = value;
//                NotifyPropertyChanged("EventID");
//            }
//        }

//        /// <summary>
//        /// 顏色索引
//        /// </summary>
//        public int ColorIndex
//        { 
//            get { return mColorIndex;}

//            set
//            {
//                mColorIndex = value;
//                NotifyPropertyChanged("ColorIndex");
//            }
//        }

//        #region INotifyPropertyChanged Members
//        private void NotifyPropertyChanged(String info)
//        {
//            if (PropertyChanged != null)
//            {
//                PropertyChanged(this, new PropertyChangedEventArgs(info));
//            }
//        }

//        public event PropertyChangedEventHandler PropertyChanged;

//        #endregion
//    }

//    internal static class CEventTransferTo
//    {
//        /// <summary>
//        /// 取得單雙週顯示文字
//        /// </summary>
//        /// <param name="WeekFlag"></param>
//        /// <returns></returns>
//        private static string DisplayWeekFlag(byte WeekFlag)
//        {
//            List<string> DisplayWeekFlags = new List<string>() { "單", "雙", "單雙" };

//            if (WeekFlag <= DisplayWeekFlags.Count)
//                return DisplayWeekFlags[WeekFlag - 1];
//            else
//                return "錯誤";
//        }

//        /// <summary>
//        /// 將分課內容轉為顯示物件
//        /// </summary>
//        /// <param name="evtSource"></param>
//        /// <returns></returns>
//        internal static CEventTransfer TransferTo(this CEvent evtSource)
//        {
//            Scheduler schLocal = Scheduler.Instance;

//            CEventTransfer evtTransfer = new CEventTransfer();

//            evtTransfer.Lock = evtSource.ManualLock ? "是" : string.Empty;
//            evtTransfer.SolutionCount = evtSource.SolutionCount == -1 ? "-" : "" + evtSource.SolutionCount;
//            evtTransfer.WhoName = evtSource.GetTeacherString();
//            evtTransfer.WhomName = schLocal.Classes[evtSource.ClassID].Name;
//            evtTransfer.WhatName = schLocal.Subjects[evtSource.SubjectID].Name;
//            evtTransfer.WhatAliasName = evtSource.SubjectAlias;
//            evtTransfer.CourseName = evtSource.CourseName;
//            evtTransfer.WhereName = schLocal.Classrooms["" + evtSource.ClassroomID].Name;
//            evtTransfer.WeekDay = evtSource.WeekDay;
//            evtTransfer.PeriodNo = evtSource.PeriodNo;
//            evtTransfer.Length = evtSource.Length;
//            evtTransfer.WeekDayCondition = evtSource.WeekDayCondition;
//            evtTransfer.PeriodCondition = evtSource.PeriodCondition;
//            evtTransfer.AllowLongBreak = evtSource.AllowLongBreak ? "是" : "否";
//            evtTransfer.AllowDuplicate = evtSource.AllowDuplicate ? "是" : "否";
//            evtTransfer.LimitNextDay = evtSource.LimitNextDay ? "是" : "否";
//            evtTransfer.CourseGroup = evtSource.CourseGroup;
//            evtTransfer.WeekFlag = DisplayWeekFlag(evtSource.WeekFlag);
//            evtTransfer.Priority = evtSource.Priority;
//            evtTransfer.TimeTableName = schLocal.TimeTables[evtSource.TimeTableID].Name;
//            evtTransfer.EventID = evtSource.EventID;

//            #region 判斷顏色種類
//            string SolutionCount = evtTransfer.SolutionCount.Equals("-") ? "-1" : evtTransfer.SolutionCount;
//            int iSolutionCount = -1;
//            int.TryParse(SolutionCount, out iSolutionCount);

//            if (evtTransfer.WeekDay != 0)
//                evtTransfer.ColorIndex = 0; //假設星期不為0，代表已經排定分課表；底色為綠色，前景色為白色。
//            else if (iSolutionCount == 0)
//                evtTransfer.ColorIndex = 1; //若解決方案為0；底色為紅色，前景色為白色。
//            else if (iSolutionCount >= 1)
//                evtTransfer.ColorIndex = 2; //若有解決方案；底色為橘色，前景色為黑色。
//            else
//                evtTransfer.ColorIndex = 3; //若事件已被釋放且未計算解決方案；底色為白色，前景色為黑色。
//            #endregion

//            return evtTransfer;
//        }
//    }
//}