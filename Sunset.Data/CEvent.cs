using System;
using System.Collections.Generic;

namespace Sunset.Data
{
    /// <summary>
    /// 延伸方法
    /// </summary>
    public static class CEventExtension
    {
        private static Scheduler eventLocal = Scheduler.Instance;

        /// <summary>
        /// 取得教系統編號
        /// </summary>
        /// <param name="Event"></param>
        /// <param name="Index"></param>
        /// <returns></returns>
        public static string GetTeacherID(this CEvent Event, int Index)
        {
            if (Index == 1)
                return Event.TeacherID1;
            else if (Index == 2)
                return Event.TeacherID2;
            else if (Index == 3)
                return Event.TeacherID3;
            else
                return string.Empty;
        }

        /// <summary>
        /// 取得教師名稱字串
        /// </summary>
        /// <param name="Event"></param>
        /// <returns></returns>
        public static string GetTeacherString(this CEvent Event,bool AddTeacherCode)
        {
            List<string> Names = new List<string>();

            if (!string.IsNullOrEmpty(Event.TeacherID1))
                Names.Add(Event.TeacherID1 + (AddTeacherCode? eventLocal.Teachers[Event.TeacherID1].Code :string.Empty) );
            if (!string.IsNullOrEmpty(Event.TeacherID2))
                Names.Add(Event.TeacherID2 + (AddTeacherCode ? eventLocal.Teachers[Event.TeacherID2].Code : string.Empty));
            if (!string.IsNullOrEmpty(Event.TeacherID3))
                Names.Add(Event.TeacherID3 + (AddTeacherCode ? eventLocal.Teachers[Event.TeacherID3].Code : string.Empty));

            return string.Join(",", Names.ToArray());
        }
    }

    /// <summary>
    /// 課程分段事件
    /// </summary>
    public class CEvent
    {
        private Scheduler schLocal = Scheduler.Instance;

        /// <summary>
        /// 建構式
        /// </summary>
        public CEvent()
        {
            SolutionCount = -1;
        }

        /// <summary>
        /// 事件編號
        /// </summary>
        public string EventID { get; set; }

        /// <summary>
        /// 教師編號一
        /// </summary>
        public string TeacherID1 { get; set; }

        /// <summary>
        /// 教師編號二
        /// </summary>
        public string TeacherID2 { get; set; }

        /// <summary>
        /// 教師編號三
        /// </summary>
        public string TeacherID3 { get; set; }

        /// <summary>
        /// 場地編號
        /// </summary>
        public string ClassroomID { get; set; }

        /// <summary>
        /// 科目編號
        /// </summary>
        public string SubjectID { get; set; }

        /// <summary>
        /// 科目簡稱
        /// </summary>
        public string SubjectAlias { get; set; }

        /// <summary>
        /// 課程名稱
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 註記
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 班級編號
        /// </summary>
        public string ClassID { get; set; }

        /// <summary>
        /// 節次編號
        /// </summary>
        public int PeriodNo { get; set;}

        /// <summary>
        /// 星期幾
        /// </summary>
        public int WeekDay { get; set;}

        /// <summary>
        /// 行事曆編號
        /// </summary>
        public Byte WeekFlag { get; set; }

        /// <summary>
        /// 星期運算子
        /// </summary>
        public int WeekDayOp { get; set; }

        /// <summary>
        /// 星期運算元
        /// </summary>
        public string WeekDayVar { get; set; }

        /// <summary>
        /// 節次運算子
        /// </summary>
        public int PeriodOp { get; set; }

        /// <summary>
        /// 節次運算元
        /// </summary>
        public string PeriodVar { get; set; }

        /// <summary>
        /// 允許跨中午
        /// </summary>
        public bool AllowLongBreak { get; set; }

        /// <summary>
        /// 允許重複
        /// </summary>
        public bool AllowDuplicate { get; set; }

        /// <summary>
        /// 動手鎖定（不可修改）
        /// </summary>
        public bool ManualLock { get; set; }

        /// <summary>
        /// 課程編號
        /// </summary>
        public string CourseID { get; set; }

        /// <summary>
        /// 時間表編號
        /// </summary>
        public string TimeTableID { get; set; }

        /// <summary>
        /// 優先次序
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 可能解
        /// </summary>
        public long SolutionCount { get; set; }

        /// <summary>
        /// 長度，以節次為單位
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// 週次，用於調代課系統
        /// </summary>
        public int? Week { get; set; }

        /// <summary>
        /// 日期，用於調代課系統
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// 課程分段說明
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 不連天排課
        /// </summary>
        public bool LimitNextDay { get; set; }

        /// <summary>
        /// 課程群組
        /// </summary>
        public string CourseGroup { get; set; }

        /// <summary>
        /// 星期條件，在設定時會解析出WeekDayOp及WeekDayVar
        /// </summary>
        public string WeekDayCondition
        {
            get
            {
                //取得星期條件
                switch(WeekDayOp)
                {
                    case Constants.opNone:
                        return string.Empty + WeekDayVar; 
                    case Constants.opEqual:
                        return "=" + WeekDayVar;
                    case Constants.opGreater:
                        return ">" + WeekDayVar;
                    case Constants.opGreaterOrEqual:
                        return ">=" + WeekDayVar;
                    case Constants.opLess:
                        return "<" + WeekDayVar;
                    case Constants.opLessOrEqual:
                        return "<=" + WeekDayVar;
                    case Constants.opNotEqual:
                        return "<>" + WeekDayVar;
                };

                return WeekDayVar;
            }
            set
            {
                if (value.Equals(string.Empty))
                {
                    WeekDayOp = Constants.opNone;
                    WeekDayVar = string.Empty;
                }
                else
                {
                    switch (value.Substring(0, 1))
                    {
                        case "=":
                            WeekDayOp = Constants.opEqual;
                            WeekDayVar = value.Substring(1, value.Length - 1);
                            break;
                        case ">":
                            if (value.Substring(1, 1).Equals("="))
                            {
                                WeekDayOp = Constants.opGreaterOrEqual;
                                WeekDayVar = value.Substring(2, value.Length - 2);
                            }
                            else
                            {
                                WeekDayOp = Constants.opGreater;
                                WeekDayVar = value.Substring(1, value.Length - 1);
                            };
                            break;
                        case "<":
                            if (value.Substring(1, 1).Equals("="))
                            {
                                WeekDayOp = Constants.opLessOrEqual;
                                WeekDayVar = value.Substring(2, value.Length - 2);
                            }
                            else if (value.Substring(1, 1).Equals(">"))
                            {
                                WeekDayOp = Constants.opNotEqual;
                                WeekDayVar = value.Substring(2, value.Length - 2);
                            }
                            else
                            {
                                WeekDayOp = Constants.opLessOrEqual;
                                WeekDayVar = value.Substring(1, value.Length - 1);
                            }
                            break;
                        default:
                            WeekDayOp = Constants.opEqual;
                            WeekDayVar = value;
                            break;
                    };
                }
            }

            #region VB
            //Public Property Get WeekDayCondition() As String
            //    Dim strWDC As String

            //    Select Case mWDCOperator
            //        Case opNone
            //            strWDC = ""
            //        Case opEqual
            //            strWDC = "="
            //        Case opGreater
            //            strWDC = ">"
            //        Case opGreaterOrEqual
            //            strWDC = ">="
            //        Case opLess
            //            strWDC = "<"
            //        Case opLessOrEqual
            //            strWDC = "<="
            //        Case opNotEqual
            //            strWDC = "<>"
            //    End Select
            //    WeekDayCondition = strWDC & mWDCOperand
            //End Property

            //Public Property Let WeekDayCondition(ByVal strNewValue As String)
            //    If strNewValue = "" Then
            //        mWDCOperator = opNone
            //        mWDCOperand = ""
            //        Exit Property
            //    End If

            //    Select Case Left(strNewValue, 1)
            //        Case "="
            //            mWDCOperator = opEqual
            //            mWDCOperand = Right(strNewValue, Len(strNewValue) - 1)
            //        Case ">"
            //            If Mid(strNewValue, 2, 1) = "=" Then
            //                mWDCOperator = opGreaterOrEqual
            //                mWDCOperand = Right(strNewValue, Len(strNewValue) - 2)
            //            Else
            //                mWDCOperator = opGreater
            //                mWDCOperand = Right(strNewValue, Len(strNewValue) - 1)
            //            End If
            //        Case "<"
            //            If Mid(strNewValue, 2, 1) = "=" Then
            //                mWDCOperator = opLessOrEqual
            //                mWDCOperand = Right(strNewValue, Len(strNewValue) - 2)
            //            ElseIf Mid(strNewValue, 2, 1) = ">" Then
            //                mWDCOperator = opNotEqual
            //                mWDCOperand = Right(strNewValue, Len(strNewValue) - 2)
            //            Else
            //                mWDCOperator = opLess
            //                mWDCOperand = Right(strNewValue, Len(strNewValue) - 1)
            //            End If
            //        Case Else
            //            mWDCOperator = 1
            //            mWDCOperand = strNewValue
            //    End Select
            //End Property
            #endregion
        }

        /// <summary>
        /// 星期條件，在設定時會解析出PeriodOp及PeriodVar
        /// </summary>
        public string PeriodCondition
        {
            get
            {
                //取得星期條件
                switch (PeriodOp)
                {
                    case Constants.opNone:
                        return string.Empty + PeriodVar;
                    case Constants.opEqual:
                        return "=" + PeriodVar;
                    case Constants.opGreater:
                        return ">" + PeriodVar;
                    case Constants.opGreaterOrEqual:
                        return ">=" + PeriodVar;
                    case Constants.opLess:
                        return "<" + PeriodVar;
                    case Constants.opLessOrEqual:
                        return "<=" + PeriodVar;
                    case Constants.opNotEqual:
                        return "<>" + PeriodVar;
                };

                return  string.Empty + PeriodVar;
            }
            set
            {
                if (value.Equals(string.Empty))
                {
                    PeriodOp = Constants.opNone;
                    PeriodVar = string.Empty;
                }
                else
                {
                    switch (value.Substring(0, 1))
                    {
                        case "=":
                            PeriodOp = Constants.opEqual;
                            PeriodVar = value.Substring(1, value.Length - 1);
                            break;
                        case ">":
                            if (value.Substring(1, 1).Equals("="))
                            {
                                PeriodOp = Constants.opGreaterOrEqual;
                                PeriodVar = value.Substring(2, value.Length - 2);
                            }
                            else
                            {
                                PeriodOp = Constants.opGreater;
                                PeriodVar = value.Substring(1, value.Length - 1);
                            };
                            break;
                        case "<":
                            if (value.Substring(1, 1).Equals("="))
                            {
                                PeriodOp = Constants.opLessOrEqual;
                                PeriodVar = value.Substring(2, value.Length - 2);
                            }
                            else if (value.Substring(1, 1).Equals(">"))
                            {
                                PeriodOp = Constants.opNotEqual;
                                PeriodVar = value.Substring(2, value.Length - 2);
                            }
                            else
                            {
                                PeriodOp = Constants.opLessOrEqual;
                                PeriodVar = value.Substring(1, value.Length - 1);
                            }
                            break;
                        default:
                            PeriodOp = Constants.opEqual;
                            PeriodVar = value;
                            break;
                    };
                }
            }

            #region VB
            //Public Property Get PeriodCondition() As String
            //    Dim strPC As String

            //    Select Case mPCOperator
            //        Case opNone
            //            strPC = ""
            //        Case opEqual
            //            strPC = "="
            //        Case opGreater
            //            strPC = ">"
            //        Case opGreaterOrEqual
            //            strPC = ">="
            //        Case opLess
            //            strPC = "<"
            //        Case opLessOrEqual
            //            strPC = "<="
            //        Case opNotEqual
            //            strPC = "<>"
            //    End Select
            //    PeriodCondition = strPC & mPCOperand
            //End Property

            //Public Property Let PeriodCondition(ByVal strNewValue As String)
            //    If strNewValue = "" Then
            //        mPCOperator = opNone
            //        mPCOperand = ""
            //        Exit Property
            //    End If

            //    Select Case Left(strNewValue, 1)
            //        Case "="
            //            mPCOperator = opEqual
            //            mPCOperand = Right(strNewValue, Len(strNewValue) - 1)
            //        Case ">"
            //            If Mid(strNewValue, 2, 1) = "=" Then
            //                mPCOperator = opGreaterOrEqual
            //                mPCOperand = Right(strNewValue, Len(strNewValue) - 2)
            //            Else
            //                mPCOperator = opGreater
            //                mPCOperand = Right(strNewValue, Len(strNewValue) - 1)
            //            End If
            //        Case "<"
            //            If Mid(strNewValue, 2, 1) = "=" Then
            //                mPCOperator = opLessOrEqual
            //                mPCOperand = Right(strNewValue, Len(strNewValue) - 2)
            //            ElseIf Mid(strNewValue, 2, 1) = ">" Then
            //                mPCOperator = opNotEqual
            //                mPCOperand = Right(strNewValue, Len(strNewValue) - 2)
            //            Else
            //                mPCOperator = opLess
            //                mPCOperand = Right(strNewValue, Len(strNewValue) - 1)
            //            End If
            //        Case Else
            //            mPCOperator = opEqual
            //            mPCOperand = strNewValue
            //    End Select
            //End Property
            #endregion
        }

        /// <summary>
        /// 顯示動手鎖定
        /// </summary>
        public string DisplayManualLock
        {
            get
            {
                return ManualLock ? "是" : string.Empty;
            }
        }

        /// <summary>
        /// 顯示解決方案
        /// </summary>
        public string DisplaySolutionCount
        {
            get
            {
                return SolutionCount == -1 ? "-" : "" + SolutionCount;
            }
        }

        /// <summary>
        /// 顯示教師姓名
        /// </summary>
        public string DisplayTeacherName
        {
            get
            {
                return this.GetTeacherString(false);
            }
        }

        /// <summary>
        /// 顯示班級名稱
        /// </summary>
        public string DisplayClassName
        {
            get
            {
                return schLocal.Classes[this.ClassID].Name;
            }
        }

        /// <summary>
        /// 顯示科目名稱
        /// </summary>
        public string DisplaySubjectName
        {
            get
            {
                return schLocal.Subjects[this.SubjectID].Name;
            }
        }

        /// <summary>
        /// 顯示場地名稱
        /// </summary>
        public string DisplayClassroomName
        {
            get
            {
                return schLocal.Classrooms[this.ClassroomID].Name; 
            }
        }

        /// <summary>
        /// 顯示跨中午
        /// </summary>
        public string DisplayAllowLongBreak
        {
            get
            {
                return AllowLongBreak ? "是" : "否";
            }
        }

        /// <summary>
        /// 顯示允許重覆
        /// </summary>
        public string DisplayAllowDuplicate
        {
            get
            {
                return AllowDuplicate ? "是" : "否";
            }
        }

        /// <summary>
        /// 顯示不連天排課
        /// </summary>
        public string DisplayLimitNextDay
        {
            get
            {
                return this.LimitNextDay ? "是" : "否";
            }
        }

        /// <summary>
        /// 顯示單雙週
        /// </summary>
        public string DisplayWeekFlag
        {
            get
            {
                List<string> DisplayWeekFlags = new List<string>() { "單", "雙", "單雙" };

                if (WeekFlag <= DisplayWeekFlags.Count)
                    return DisplayWeekFlags[WeekFlag - 1];
                else
                    return "錯誤"; 
            }
        }

        /// <summary>
        /// 顯示時間表名稱
        /// </summary>
        public string DispalyTimeTableName
        {
            get
            {
                return schLocal.TimeTables[this.TimeTableID].Name;
            }
        }

        /// <summary>
        /// 顏色索引
        /// </summary>
        public int ColorIndex
        { 
            get 
            {
                if (this.WeekDay != 0)
                    return 0;                   //假設星期不為0，代表已經排定分課表；底色為綠色，前景色為白色。
                else if (this.SolutionCount == 0)
                    return 1;                   //若解決方案為0；底色為紅色，前景色為白色。
                else if (this.SolutionCount >= 1)
                    return 2;                   //若有解決方案；底色為橘色，前景色為黑色。
                else
                    return 3;                   //若事件已被釋放且未計算解決方案；底色為白色，前景色為黑色。
            }
        }
    }
}