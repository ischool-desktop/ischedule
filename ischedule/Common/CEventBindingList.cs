using System;
using System.ComponentModel;
using Sunset.Data;

namespace ischedule
{
    internal class CEventBindingList : SortableBindingList<CEvent>
    {
        protected override Comparison<CEvent> GetComparer(PropertyDescriptor prop)
        {
            Comparison<CEvent> comparer = null;

            switch (prop.Name)
            {
                case "DisplayManualLock":
                    comparer = new Comparison<CEvent>(delegate(CEvent x, CEvent y)
                    {
                        if (x != null)
                            if (y != null)
                                return (x.DisplayManualLock.CompareTo(y.DisplayManualLock));
                            else
                                return 1;
                        else if (y != null)
                            return -1;
                        else
                            return 0;
                    });
                    break;
                case "DisplaySolutionCount":
                    comparer = new Comparison<CEvent>(delegate(CEvent x, CEvent y)
                    {
                        if (x != null)
                            if (y != null)
                                return (x.DisplaySolutionCount.CompareTo(y.DisplaySolutionCount));
                            else
                                return 1;
                        else if (y != null)
                            return -1;
                        else
                            return 0;
                    });
                    break;
                case "WeekDay":
                    comparer = new Comparison<CEvent>(delegate(CEvent x, CEvent y)
                    {
                        if (x != null)
                            if (y != null)
                                return (x.WeekDay.CompareTo(y.WeekDay));
                            else
                                return 1;
                        else if (y != null)
                            return -1;
                        else
                            return 0;
                    });
                    break;
                case "PeriodNo":
                    comparer = new Comparison<CEvent>(delegate(CEvent x, CEvent y)
                    {
                        if (x != null)
                            if (y != null)
                                return (x.PeriodNo.CompareTo(y.PeriodNo));
                            else
                                return 1;
                        else if (y != null)
                            return -1;
                        else
                            return 0;
                    });
                    break;
                case "DisplayTeacherName":
                    comparer = new Comparison<CEvent>(delegate(CEvent x, CEvent y)
                    {
                        if (x != null)
                            if (y != null)
                                return (x.DisplayTeacherName.CompareTo(y.DisplayTeacherName));
                            else
                                return 1;
                        else if (y != null)
                            return -1;
                        else
                            return 0;
                    });
                    break;
                case "DisplayClassName":
                    comparer = new Comparison<CEvent>(delegate(CEvent x, CEvent y)
                    {
                        if (x != null)
                            if (y != null)
                                return (x.DisplayClassName.CompareTo(y.DisplayClassName));
                            else
                                return 1;
                        else if (y != null)
                            return -1;
                        else
                            return 0;
                    });
                    break;
                case "DisplaySubjectName":
                    comparer = new Comparison<CEvent>(delegate(CEvent x, CEvent y)
                    {
                        if (x != null)
                            if (y != null)
                                return (x.DisplaySubjectName.CompareTo(y.DisplaySubjectName));
                            else
                                return 1;
                        else if (y != null)
                            return -1;
                        else
                            return 0;
                    });
                    break;
                case "SubjectAlias":
                    comparer = new Comparison<CEvent>(delegate(CEvent x, CEvent y)
                    {
                        if (x != null)
                            if (y != null)
                                return (x.SubjectAlias.CompareTo(y.SubjectAlias));
                            else
                                return 1;
                        else if (y != null)
                            return -1;
                        else
                            return 0;
                    });
                    break;
                case "CourseName":
                    comparer = new Comparison<CEvent>(delegate(CEvent x, CEvent y)
                    {
                        if (x != null)
                            if (y != null)
                                return (x.CourseName.CompareTo(y.CourseName));
                            else
                                return 1;
                        else if (y != null)
                            return -1;
                        else
                            return 0;
                    });
                    break;
                case "DisplayClassroomName":
                    comparer = new Comparison<CEvent>(delegate(CEvent x, CEvent y)
                    {
                        if (x != null)
                            if (y != null)
                                return (x.DisplayClassroomName.CompareTo(y.DisplayClassroomName));
                            else
                                return 1;
                        else if (y != null)
                            return -1;
                        else
                            return 0;
                    });
                    break;
                case "Length":
                    comparer = new Comparison<CEvent>(delegate(CEvent x, CEvent y)
                    {
                        if (x != null)
                            if (y != null)
                                return (x.Length.CompareTo(y.Length));
                            else
                                return 1;
                        else if (y != null)
                            return -1;
                        else
                            return 0;
                    });
                    break;
                case "WeekDayCondition":
                    comparer = new Comparison<CEvent>(delegate(CEvent x, CEvent y)
                    {
                        if (x != null)
                            if (y != null)
                                return (x.WeekDayCondition.CompareTo(y.WeekDayCondition));
                            else
                                return 1;
                        else if (y != null)
                            return -1;
                        else
                            return 0;
                    });
                    break;
                case "PeriodCondition":
                    comparer = new Comparison<CEvent>(delegate(CEvent x, CEvent y)
                    {
                        if (x != null)
                            if (y != null)
                                return (x.PeriodCondition.CompareTo(y.PeriodCondition));
                            else
                                return 1;
                        else if (y != null)
                            return -1;
                        else
                            return 0;
                    });
                    break;
                case "DisplayAllowLongBreak":
                    comparer = new Comparison<CEvent>(delegate(CEvent x, CEvent y)
                    {
                        if (x != null)
                            if (y != null)
                                return (x.DisplayAllowLongBreak.CompareTo(y.DisplayAllowLongBreak));
                            else
                                return 1;
                        else if (y != null)
                            return -1;
                        else
                            return 0;
                    });
                    break;
                case "DisplayAllowDuplicate":
                    comparer = new Comparison<CEvent>(delegate(CEvent x, CEvent y)
                    {
                        if (x != null)
                            if (y != null)
                                return (x.DisplayAllowDuplicate.CompareTo(y.DisplayAllowDuplicate));
                            else
                                return 1;
                        else if (y != null)
                            return -1;
                        else
                            return 0;
                    });
                    break;
                case "DisplayWeekFlag":
                    comparer = new Comparison<CEvent>(delegate(CEvent x, CEvent y)
                    {
                        if (x != null)
                            if (y != null)
                                return (x.DisplayWeekFlag.CompareTo(y.DisplayWeekFlag));
                            else
                                return 1;
                        else if (y != null)
                            return -1;
                        else
                            return 0;
                    });
                    break;
                case "DisplayLimitNextDay":
                    comparer = new Comparison<CEvent>(delegate(CEvent x, CEvent y)
                    {
                        if (x != null)
                            if (y != null)
                                return (x.DisplayLimitNextDay.CompareTo(y.DisplayLimitNextDay));
                            else
                                return 1;
                        else if (y != null)
                            return -1;
                        else
                            return 0;
                    });
                    break;
                case "CourseGroup":
                    comparer = new Comparison<CEvent>(delegate(CEvent x, CEvent y)
                    {
                        if (x != null)
                            if (y != null)
                                return (x.CourseGroup.CompareTo(y.CourseGroup));
                            else
                                return 1;
                        else if (y != null)
                            return -1;
                        else
                            return 0;
                    });
                    break;
                case "Priority":
                    comparer = new Comparison<CEvent>(delegate(CEvent x, CEvent y)
                    {
                        if (x != null)
                            if (y != null)
                                return (x.Priority.CompareTo(y.Priority));
                            else
                                return 1;
                        else if (y != null)
                            return -1;
                        else
                            return 0;
                    });
                    break;
                case "DispalyTimeTableName":
                    comparer = new Comparison<CEvent>(delegate(CEvent x, CEvent y)
                    {
                        if (x != null)
                            if (y != null)
                                return (x.DispalyTimeTableName.CompareTo(y.DispalyTimeTableName));
                            else
                                return 1;
                        else if (y != null)
                            return -1;
                        else
                            return 0;
                    });
                    break;
            }

            return comparer;
        }
    }
}