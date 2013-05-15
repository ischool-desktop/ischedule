using Sunset.Data.Integration;

namespace Sunset.Data
{
    /// <summary>
    /// 班級
    /// <remarks>
    /// 1.班級只會有一個行事曆，學生沒辦法身分同時上兩個課程。
    /// 2.時間表概念主要在班級上定義，可以是否有情境教師及班級是否會有時間表概念。
    /// </remarks>
    /// </summary>
    public class Class
    {
        private string mWhomID;
        private string mName;
        private string mTeacherName;
        private string mTimeTableID;
        private Appointments mAppointments;
        private string mGradeYear;
        private string mNamingRule;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="WhomID">班級編號</param>
        /// <param name="Name">班級姓名</param>
        /// <param name="TimeTableID">時間表編號</param>
        public Class(string WhomID,string Name,string TimeTableID,string TeacherName,string GradeYear,string NamingRule)
        {
            //指定班級編號、姓名及時間表編號
            this.mWhomID = WhomID;
            this.mName = Name;
            this.mTimeTableID = TimeTableID;
            this.mTeacherName = TeacherName;
            this.mGradeYear = GradeYear;
            this.mNamingRule = NamingRule;

            //初始化行事曆
            this.mAppointments = new Appointments();
        }

        /// <summary>
        /// 班級編號
        /// </summary>
        public string WhomID { get { return this.mWhomID; } }

        /// <summary>
        /// 班級名稱
        /// </summary>
        public string Name { get { return this.mName; } }

        /// <summary>
        /// 年級
        /// </summary>
        public string GradeYear { get { return this.mGradeYear; } }

        /// <summary>
        /// 班級命名規則
        /// </summary>
        public string NamingRule { get { return this.mNamingRule; } }

        /// <summary>
        /// 授課教師姓名
        /// </summary>
        public string TeacherName { get { return this.mTeacherName; } }

        /// <summary>
        /// 時間表編號
        /// </summary>
        public string TimeTableID { get { return this.mTimeTableID; } }        

        /// <summary>
        /// 行事曆
        /// </summary>
        public Appointments Appointments { get { return this.mAppointments; } }

        /// <summary>
        /// 場地總時數
        /// </summary>
        public int TotalHour { get; set; }

        /// <summary>
        /// 已排課時數
        /// </summary>
        public int AllocHour { get; set; }
    }

    public static class Whom_Extension
    {
        /// <summary>
        /// 取得升級後班級名稱
        /// </summary>
        /// <param name="Whom"></param>
        /// <returns></returns>
        public static string GetUpgradeClassName(this Class Whom)
        {
            int GradeYear = Int.Parse(Whom.GradeYear);

            string ClassName = ParseClassName(Whom.NamingRule, GradeYear + 1);

            return ClassName;
        }

        /// <summary>
        /// 檢查是否為合法的命名規則
        /// </summary>
        /// <param name="namingRule">班級命名規則</param>
        /// <returns></returns>
        private static bool ValidateNamingRule(string namingRule)
        {
            return namingRule.IndexOf('{') < namingRule.IndexOf('}');
        }

        /// <summary>
        /// 根據班級命名規則及年級解析出班級名稱
        /// </summary>
        /// <param name="namingRule">班級命名規則</param>
        /// <param name="gradeYear">年級</param>
        /// <returns></returns>
        private static string ParseClassName(this string namingRule, int gradeYear)
        {
            if (gradeYear >= 6)
                gradeYear -= 6;
            gradeYear--;
            if (!ValidateNamingRule(namingRule))
                return namingRule;
            string classlist_firstname = "", classlist_lastname = "";
            if (namingRule.Length == 0) return "{" + (gradeYear + 1) + "}";

            string tmp_convert = namingRule;

            // 找出"{"之前文字 並放入 classlist_firstname , 並除去"{"
            if (tmp_convert.IndexOf('{') > 0)
            {
                classlist_firstname = tmp_convert.Substring(0, tmp_convert.IndexOf('{'));
                tmp_convert = tmp_convert.Substring(tmp_convert.IndexOf('{') + 1, tmp_convert.Length - (tmp_convert.IndexOf('{') + 1));
            }
            else tmp_convert = tmp_convert.TrimStart('{');

            // 找出 } 之後文字 classlist_lastname , 並除去"}"
            if (tmp_convert.IndexOf('}') > 0 && tmp_convert.IndexOf('}') < tmp_convert.Length - 1)
            {
                classlist_lastname = tmp_convert.Substring(tmp_convert.IndexOf('}') + 1, tmp_convert.Length - (tmp_convert.IndexOf('}') + 1));
                tmp_convert = tmp_convert.Substring(0, tmp_convert.IndexOf('}'));
            }
            else tmp_convert = tmp_convert.TrimEnd('}');

            // , 存入 array
            string[] listArray = new string[tmp_convert.Split(',').Length];
            listArray = tmp_convert.Split(',');

            // 檢查是否在清單範圍
            if (gradeYear >= 0 && gradeYear < listArray.Length)
            {
                tmp_convert = classlist_firstname + listArray[gradeYear] + classlist_lastname;
            }
            else
            {
                tmp_convert = classlist_firstname + "{" + (gradeYear + 1) + "}" + classlist_lastname;
            }
            return tmp_convert;
        }
    }
}