using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sunset.Data;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace ischedule
{
    public static class SchedulerHelper
    {
        private static string GetTimeTableID(string EventID)
        {
            return Scheduler.Instance.CEvents[EventID].TimeTableID;
        }

        /// <summary>
        /// 是否為群組課程
        /// </summary>
        /// <param name="Events"></param>
        /// <returns></returns>
        public static bool IsGroupEvents(this List<CEvent> Events)
        {
            Scheduler schLocal = Scheduler.Instance;

            if (Events.Count < 2)
                return false;

            foreach (CEvent Event in Events)
                if (string.IsNullOrWhiteSpace(Event.EventID))
                    return false;

            string CourseGroup = Events[0].CourseGroup;

            if (string.IsNullOrEmpty(CourseGroup))
                return false;

            foreach (CEvent Event in Events)
            {
                string CurCourseGroup = Event.CourseGroup;

                if (!CourseGroup.Equals(CurCourseGroup))
                    return false;
            }

            return true; 
        }

        /// <summary>
        /// 是否為群組課程
        /// </summary>
        /// <param name="Apps"></param>
        /// <returns></returns>
        public static bool IsGroupAppointments(this Appointments Apps)
        {
            if (Apps.Count < 2)
                return false;

            foreach (Appointment App in Apps)
                if (string.IsNullOrWhiteSpace(App.EventID))
                    return false;

            string CourseGroup = Scheduler.Instance.CEvents[Apps[0].EventID].CourseGroup;

            if (string.IsNullOrEmpty(CourseGroup))
                return false;

            foreach (Appointment App in Apps)
            {
                string CurCourseGroup = Scheduler.Instance.CEvents[App.EventID].CourseGroup;

                if (!CourseGroup.Equals(CurCourseGroup))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 判斷是否為單雙週
        /// </summary>
        /// <param name="Events"></param>
        /// <returns></returns>
        public static bool IsSDAppointments(this List<CEvent> Events)
        {
            //若不為兩個時段則回傳false
            if (Events.Count != 2)
                return false;

            //必須兩個Appointment都不為空值
            if (string.IsNullOrWhiteSpace(Events[0].EventID) || 
                string.IsNullOrWhiteSpace(Events[1].EventID))
                return false;

            //兩個時間表必須要一樣
            if (GetTimeTableID(Events[0].EventID) != GetTimeTableID(Events[1].EventID))
                return false;

            //判斷兩者是否加起來為3
            if ((Events[0].WeekFlag + Events[1].WeekFlag) != 3)
                return false;

            return true;
        }

        /// <summary>
        /// 判斷是否為單雙週
        /// </summary>
        /// <param name="Apps"></param>
        /// <returns></returns>
        public static bool IsSDAppointments(this Appointments Apps)
        {
            //若不為兩個時段則回傳false
            if (Apps.Count != 2)
                return false;

            //必須兩個Appointment都不為空值
            if (string.IsNullOrWhiteSpace(Apps[0].EventID) || string.IsNullOrWhiteSpace(Apps[1].EventID))
                return false;

            //兩個時間表必須要一樣
            if (GetTimeTableID(Apps[0].EventID) != GetTimeTableID(Apps[1].EventID))
                return false;

            //判斷兩者是否加起來為3
            if ((Apps[0].WeekFlag + Apps[1].WeekFlag) != 3)
                return false;

            return true;
        }

        /// <summary>
        /// 判斷是否為多筆分課情況
        /// </summary>
        /// <param name="Apps"></param>
        /// <returns></returns>
        public static bool IsMultipleEvents(this Appointments Apps)
        {
            //若不為兩個時段則回傳false
            if (Apps.Count < 2)
                return false;

            //必須兩個Appointment都不為空值
            foreach (Appointment App in Apps)
                if (string.IsNullOrWhiteSpace(App.EventID))
                    return false;
            return true;
        }

        //public static Canvas GetUnScheduleUI(int AssocType, string AssocID, int LPViewAssocObjType, string LPViewAssocID, string Name, string Desc)
        //{
        //    Canvas Panel = new Canvas();

        //    Panel.FlowDirection = System.Windows.FlowDirection.LeftToRight;
        //    Panel.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
        //    Panel.VerticalAlignment = System.Windows.VerticalAlignment.Top;

        //    double Left = 0;
        //    double Top = 0;
        //    double Delta = 15;

        //    if (!string.IsNullOrEmpty(AssocID))
        //    {
        //        Label lblConflictResource = LPViewHelper.GetAssocUI(AssocType, AssocID, LPViewAssocObjType, LPViewAssocID, Name);
        //        Panel.Children.Add(lblConflictResource);
        //        Canvas.SetLeft(lblConflictResource, Left);
        //        Canvas.SetTop(lblConflictResource, Top);
        //        Top += Delta;
        //    }

        //    Label lblConflictDescription = new Label();
        //    lblConflictDescription.Content = Desc;
        //    Canvas.SetLeft(lblConflictDescription, Left);
        //    Canvas.SetTop(lblConflictDescription, Top);

        //    Panel.Children.Add(lblConflictDescription);

        //    return Panel;
        //}

        /// <summary>
        /// 取得分課顯示資訊
        /// </summary>
        /// <param name="AssocType"></param>
        /// <param name="AssocID"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public static void SetAssocUI(this LabelX Label,int AssocType, string AssocID, int LPViewAssocObjType, string LPViewAssocID, string Name)
        {
            if (Name.Equals("無") || (AssocType.Equals(LPViewAssocObjType) && AssocID.Equals(LPViewAssocID)))
            {
                Label.Text = Name;
            }
            else
            {
                Label.Text = Name;
                Label.Tag = AssocType + ":" + AssocID; 

                //Hyperlink link = new Hyperlink();
                //link.SetResourceReference(ContextMenu.StyleProperty, "LPViewLinkStyle");
                //link.Click += (sender, e) => (Application.Current.MainWindow as frmMain).OpenLPViewByAssocID(AssocType, AssocID);
                //Run d = new Run();
                //d.Text = Name;
                //link.Inlines.Add(d);
                //lbl.Content = link;
            }
        }

        private static Label GeAdditionalUI(byte WeekFlag, Label lbl)
        {
            if (WeekFlag == 1)
                lbl.Text += "單";
            else if (WeekFlag == 2)
                lbl.Text += "雙";

            return lbl;
        }

        /// <summary>
        /// 取得單雙週顯示字串
        /// </summary>
        /// <param name="WeekFlag"></param>
        /// <returns></returns>
        public static string GetWeekFlagStr(this byte WeekFlag)
        {
            if (WeekFlag == 1)
                return "(單)";
            else if (WeekFlag == 2)
                return "(雙)";

            return string.Empty;
        }

        public static void AddEventControl(this ucPeriod Period, LPViewOption Option, string EventID, int LPViewAssocObjType, string LPViewAssocID, int DefaultIndex = 1)
        {
            Scheduler schLocal = Scheduler.Instance;

            CEvent eventLocal = schLocal.CEvents[EventID];

            if (Option.IsWhom)
            {
                LabelX Label = Period.GetLabel(DefaultIndex);
                SetAssocUI(Label,Constants.lvWhom, eventLocal.ClassID, LPViewAssocObjType, LPViewAssocID, schLocal.Classes[eventLocal.ClassID].Name);
                DefaultIndex++;
            }

            if (Option.IsWhat)
            {
                LabelX Label = Period.GetLabel(DefaultIndex);
                Label.Text = schLocal.Subjects[eventLocal.SubjectID].Name + GetWeekFlagStr(eventLocal.WeekFlag);
                DefaultIndex++;
            }

            if (Option.IsWhatAlias)
            {
                LabelX Label = Period.GetLabel(DefaultIndex);
                Label.Text = eventLocal.SubjectAlias + GetWeekFlagStr(eventLocal.WeekFlag);
                DefaultIndex++;
            }

            if (Option.IsWho)
            {
                LabelX Label = Period.GetLabel(DefaultIndex);
                List<string> WhoNames = new List<string>();
                if (!string.IsNullOrEmpty(eventLocal.TeacherID1))
                {
                    WhoNames.Add(eventLocal.TeacherID1);
                    //Label lbl1 = GetAssocUI(Constants.lvWho, eventLocal.WhoID1, LPViewAssocObjType, LPViewAssocID, schLocal.Whos[eventLocal.WhoID1].Name);
                    //lbl1.Left = vLeft;
                    //lbl1.Top = Top;
                    //Panel.Controls.Add(lbl1);
                    //vLeft += eventLocal.WhoID1.Length * vDelta;
                }

                if (!string.IsNullOrEmpty(eventLocal.TeacherID2))
                {
                    WhoNames.Add(eventLocal.TeacherID2);
                    //Label lbl2 = GetAssocUI(Constants.lvWho, eventLocal.WhoID2, LPViewAssocObjType, LPViewAssocID, schLocal.Whos[eventLocal.WhoID2].Name);
                    //lbl2.Left = vLeft;
                    //lbl2.Top = Top;
                    //Panel.Controls.Add(lbl2);
                    //vLeft += eventLocal.WhoID2.Length * vDelta;
                }

                if (!string.IsNullOrEmpty(eventLocal.TeacherID3))
                {
                    WhoNames.Add(eventLocal.TeacherID3);
                    //Label lbl3 = GetAssocUI(Constants.lvWho, eventLocal.WhoID3, LPViewAssocObjType, LPViewAssocID, schLocal.Whos[eventLocal.WhoID3].Name);
                    //lbl3.Left = vLeft;
                    //lbl3.Top = Top;
                    //Panel.Controls.Add(lbl3);
                }

                if (WhoNames.Count > 1)
                    Label.Text = string.Join(",", WhoNames.ToArray());
                else //連結提供只有當一位授課教師時
                    SetAssocUI(Label, Constants.lvWho, eventLocal.TeacherID1, LPViewAssocObjType, LPViewAssocID, schLocal.Teachers[eventLocal.TeacherID1].Name);

                DefaultIndex++;
            }

            if (Option.IsWhere)
            {
                LabelX Label = Period.GetLabel(DefaultIndex);
                SetAssocUI(Label,Constants.lvWhere, eventLocal.ClassroomID, LPViewAssocObjType, LPViewAssocID, schLocal.Classrooms[eventLocal.ClassroomID].Name);
                DefaultIndex++;
            }

            if (!string.IsNullOrWhiteSpace(eventLocal.Message))
            {
                LabelX Label = Period.GetLabel(DefaultIndex);
                //lblA.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;
                //lblA.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                //lblA.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                //lblA.VerticalContentAlignment = System.Windows.VerticalAlignment.Top;
                Label.Text = eventLocal.Message;
                DefaultIndex++;
            }
        }

        public static double AddEventControl2(this Panel Panel, LPViewOption Option, string EventID, int LPViewAssocObjType, string LPViewAssocID, double DefaultTop = 0)
        {
            double Top = DefaultTop;
            int vLeft = 0;
            int vDelta = 3;
            double Delta = 15;

            //StackPanel sPanel = new StackPanel();
            //sPanel.Orientation = Orientation.Horizontal;
            //sPanel.CanHorizontallyScroll = true;
            //sPanel.FlowDirection = FlowDirection.LeftToRight;
            //sPanel.Width = 110;

            //Panel.FlowDirection = System.Windows.FlowDirection.LeftToRight;
            //Panel.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            //Panel.VerticalAlignment = System.Windows.VerticalAlignment.Top;

            Scheduler schLocal = Scheduler.Instance;

            CEvent eventLocal = schLocal.CEvents[EventID];

            if (Option.IsWhom)
            {
                //Label lbl = GetAssocUI(Constants.lvWhom, eventLocal.WhomID, LPViewAssocObjType, LPViewAssocID, schLocal.Whoms[eventLocal.WhomID].Name);
                //sPanel.Children.Add(lbl);
                //Canvas.SetLeft(lbl, vLeft);
                //Canvas.SetTop(lbl, Top);
                //Panel.Children.Add(lbl);
                //vLeft += eventLocal.WhomID.Length * vDelta;
            }

            if (Option.IsWhat)
            {
                Label lbl = new Label();
                lbl.Text = schLocal.Subjects[eventLocal.SubjectID].Name + GetWeekFlagStr(eventLocal.WeekFlag);
                //sPanel.Children.Add(lbl);
                //Canvas.SetLeft(lbl, vLeft);
                //Canvas.SetTop(lbl, Top);
                //Panel.Children.Add(lbl);
                //vLeft += (schLocal.Whats[eventLocal.WhatID].Name + GetWeekFlagStr(eventLocal.WeekFlag)).Length * vDelta;
            }

            if (Option.IsWhatAlias)
            {
                Label lbl = new Label();
                lbl.Text = eventLocal.SubjectAlias + GetWeekFlagStr(eventLocal.WeekFlag);
                //sPanel.Children.Add(lbl);
                //Canvas.SetLeft(lbl, vLeft);
                //Canvas.SetTop(lbl, Top);
                //Panel.Children.Add(lbl);
                //vLeft += (eventLocal.WhatAlias + GetWeekFlagStr(eventLocal.WeekFlag)).Length * vDelta;
            }

            if (Option.IsWho)
            {
                List<string> WhoNames = new List<string>();

                if (!string.IsNullOrEmpty(eventLocal.TeacherID1))
                {
                    WhoNames.Add(eventLocal.TeacherID1);
                    //Label lbl1 = GetAssocUI(Constants.lvWho, eventLocal.WhoID1, LPViewAssocObjType, LPViewAssocID, schLocal.Whos[eventLocal.WhoID1].Name);
                    //sPanel.Children.Add(lbl1);
                    //Canvas.SetLeft(lbl1, vLeft);
                    //Canvas.SetTop(lbl1, Top);
                    //Panel.Children.Add(lbl1);
                    //vLeft += eventLocal.WhoID1.Length * vDelta;
                }

                if (!string.IsNullOrEmpty(eventLocal.TeacherID2))
                {
                    WhoNames.Add(eventLocal.TeacherID2);
                    //Label lbl2 = GetAssocUI(Constants.lvWho, eventLocal.WhoID2, LPViewAssocObjType, LPViewAssocID, schLocal.Whos[eventLocal.WhoID2].Name);
                    //sPanel.Children.Add(lbl2);
                    //Canvas.SetLeft(lbl2, vLeft);
                    //Canvas.SetTop(lbl2, Top);
                    //Panel.Children.Add(lbl2);
                    //vLeft += eventLocal.WhoID2.Length * vDelta;
                }

                if (!string.IsNullOrEmpty(eventLocal.TeacherID3))
                {
                    WhoNames.Add(eventLocal.TeacherID3);
                    //Label lbl3 = GetAssocUI(Constants.lvWho, eventLocal.WhoID3, LPViewAssocObjType, LPViewAssocID, schLocal.Whos[eventLocal.WhoID3].Name);
                    //sPanel.Children.Add(lbl3);
                    //Canvas.SetLeft(lbl3, vLeft);
                    //Canvas.SetTop(lbl3, Top);
                    //Panel.Children.Add(lbl3);
                    //vLeft += eventLocal.WhoID3.Length * vDelta;
                }
            }

            if (Option.IsWhere)
            {
                //Label lbl = GetAssocUI(Constants.lvWhere, eventLocal.WhereID, LPViewAssocObjType, LPViewAssocID, schLocal.Wheres[eventLocal.WhereID].Name);
                //sPanel.Children.Add(lbl);
                //Canvas.SetLeft(lbl, vLeft);
                //Canvas.SetTop(lbl, Top);
                //Panel.Children.Add(lbl);
                //vLeft += eventLocal.WhereID.Length * vDelta;
            }

            if (!string.IsNullOrWhiteSpace(eventLocal.Message))
            {
                Label lblA = new Label();
                lblA.Text = eventLocal.Message;
                //sPanel.Children.Add(lblA);
                //Canvas.SetLeft(lblA, vLeft);
                //Canvas.SetTop(lblA, Top);
                //Panel.Children.Add(lblA);
                //vLeft += eventLocal.Message.Length * vDelta;
            }

            //Canvas.SetLeft(sPanel, vLeft);
            //Canvas.SetTop(sPanel, Top);
            //Panel.Children.Add(sPanel);

            return Top + Delta;
        }

    }
}