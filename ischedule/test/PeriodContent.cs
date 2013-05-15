using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using ischedule.Properties;
using Sunset.Data;

namespace ischedule
{
    public class PeriodContent
    {
        public string Text { get; set; }

        public Bitmap Picture { get; set; }

        public Color BackColor { get; set; }
     
        public Color ForeColor { get; set; }

        public List<LabelContent> LabelContents { get; set; }

        public PeriodContent()
        {
            Picture = Resources.blank;
            BackColor = Color.White;
            ForeColor = Color.White;
            LabelContents = new List<LabelContent>();
        }

        private LabelContent GetAssocUI(int AssocType, 
            string AssocID, 
            int LPViewAssocObjType, 
            string LPViewAssocID, 
            string Name)
        {
            if (Name.Equals("無") || (AssocType.Equals(LPViewAssocObjType) && AssocID.Equals(LPViewAssocID)))
            {
                return new LabelContent(Name, string.Empty);
            }
            else
            {
                return new LabelContent(Name,AssocType + ":" + AssocID);
            }
        }

        public void SetEvent(LPViewOption Option, 
            string EventID, 
            int LPViewAssocObjType,
            string LPViewAssocID, 
            int DefaultIndex = 1)
        {
            LabelContents.Clear();

            Scheduler schLocal = Scheduler.Instance;

            CEvent eventLocal = schLocal.CEvents[EventID];

            if (Option.IsWhom)
                LabelContents.Add(GetAssocUI(Constants.lvWhom, eventLocal.ClassID, LPViewAssocObjType, LPViewAssocID, schLocal.Classes[eventLocal.ClassID].Name));

            if (Option.IsWhat)
                LabelContents.Add(new LabelContent(schLocal.Subjects[eventLocal.SubjectID].Name + eventLocal.WeekFlag.GetWeekFlagStr(),string.Empty));

            if (Option.IsWhatAlias)
                LabelContents.Add(new LabelContent(eventLocal.SubjectAlias + eventLocal.WeekFlag.GetWeekFlagStr(), string.Empty));

            if (Option.IsWho)
            {
                List<string> WhoNames = new List<string>();
                if (!string.IsNullOrEmpty(eventLocal.TeacherID1))
                    WhoNames.Add(eventLocal.TeacherID1);

                if (!string.IsNullOrEmpty(eventLocal.TeacherID2))
                    WhoNames.Add(eventLocal.TeacherID2);

                if (!string.IsNullOrEmpty(eventLocal.TeacherID3))
                    WhoNames.Add(eventLocal.TeacherID3);

                if (WhoNames.Count > 1)
                    LabelContents.Add(new LabelContent(string.Join(",", WhoNames.ToArray()),string.Empty));
                else //連結提供只有當一位授課教師時
                {
                    LabelContent lblContent = GetAssocUI(Constants.lvWho, eventLocal.TeacherID1, LPViewAssocObjType, LPViewAssocID, schLocal.Teachers[eventLocal.TeacherID1].Name);
                    LabelContents.Add(lblContent);
                }
            }

            if (Option.IsWhere)
            {
                LabelContent lblContent = GetAssocUI(Constants.lvWhere, eventLocal.ClassroomID, LPViewAssocObjType, LPViewAssocID, schLocal.Classrooms[eventLocal.ClassroomID].Name);
                LabelContents.Add(lblContent);
            }

            if (!string.IsNullOrWhiteSpace(eventLocal.Message))            
            {
                LabelContent lblContent = new LabelContent(eventLocal.Message, string.Empty);
                LabelContents.Add(lblContent);
            }
        }
    }
}
