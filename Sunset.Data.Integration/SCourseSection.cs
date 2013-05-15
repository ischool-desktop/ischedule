using System;
using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using FISCA.DSAClient;

namespace Sunset.Data.Integration
{
    /// <summary>
    /// 排課來源課程分段資料
    /// </summary>
    public class SCourseSection
    {        
        #region public property
        /// <summary>
        /// 課程分段系統編號，為DSNS加上來源系統編號
        /// </summary>
        public string ID { get; set;}
    
        /// <summary>
        /// 課程系統編號，為DSNS加上來源系統編號
        /// </summary>
        public string CourseID { get; set; }

        /// <summary>
        /// 星期
        /// </summary>
        public int WeekDay { get; set; }

        /// <summary>
        /// 節次
        /// </summary>
        public int PeriodNo { get; set; }

        /// <summary>
        /// 節數
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// 星期條件
        /// </summary>
        public string WeekdayCond { get; set; }

        /// <summary>
        /// 節次條件
        /// </summary>
        public string PeriodCond { get; set; }

        /// <summary>
        /// 單雙週
        /// </summary>
        public Byte WeekFlag { get; set; }

        /// <summary>
        /// 跨中午
        /// </summary>
        public bool Longbreak { get; set; }

        /// <summary>
        /// 鎖定
        /// </summary>
        public bool Lock { get; set; }

        /// <summary>
        /// 場地系統編號，為場地名稱
        /// </summary>
        public string ClassroomID { get; set; }

        /// <summary>
        /// 所屬課程科目名稱
        /// </summary>
        public string Subject { get; set;}

        /// <summary>
        /// 科目簡稱
        /// </summary>
        public string SubjectAlias { get; set; }

        /// <summary>
        /// 課程名稱
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 班級系統編號，為DSNS加上來源系統編號
        /// </summary>
        public string ClassID { get; set;}

        /// <summary>
        /// 允許重覆
        /// </summary>
        public bool AllowDup {get; set;}

        /// <summary>
        /// 時間表系統編號，為時間表名稱
        /// </summary>
        public string TimeTableID { get; set;}

        /// <summary>
        /// 授課教師一
        /// </summary>
        public string TeacherName1 { get; set;}

        /// <summary>
        /// 授課教師二
        /// </summary>
        public string TeacherName2 { get; set; }

        /// <summary>
        /// 授課教師三
        /// </summary>
        public string TeacherName3 { get; set; }

        /// <summary>
        /// 課程群組
        /// </summary>
        public string CourseGroup { get; set; }

        /// <summary>
        /// 不連天排課
        /// </summary>
        public bool LimitNextDay { get; set; }

        /// <summary>
        /// 註記
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 輸出字串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ID + "," + CourseID + "," + WeekDay + "," + PeriodNo + "," + Length + "," + WeekdayCond + "," + PeriodCond + "," + WeekFlag + "," + Longbreak + "," + Lock + "," + ClassroomID + "," + CourseGroup + "," + LimitNextDay + "," + Comment;
        }

        /// <summary>
        /// 從XML載入
        /// </summary>
        /// <param name="Element"></param>
        public void Load(XElement Element)
        {
            #region 系統編號
            this.ID = Element.AttributeText("ID");
            this.CourseID = Element.AttributeText("CourseID");
            this.ClassroomID = Element.AttributeText("ClassroomID");
            this.ClassID = Element.AttributeText("ClassID");

            string TeacherID = Element.AttributeText("TeacherID");

            this.TeacherName1 = Element.AttributeText("TeacherName1");

            if (string.IsNullOrEmpty(this.TeacherName1))
                if (!string.IsNullOrEmpty(TeacherID))
                    this.TeacherName1 = TeacherID;

            this.TeacherName2 = Element.AttributeText("TeacherName2");
            this.TeacherName3 = Element.AttributeText("TeacherName3");
            this.TimeTableID = Element.AttributeText("TimeTableID");
            #endregion

            #region 排課屬性
            this.Subject = Element.AttributeText("Subject");
            this.SubjectAlias = Element.AttributeText("SubjectAlias");
            this.CourseName = Element.AttributeText("CourseName");
            this.AllowDup = Bool.Parse(Element.AttributeText("AllowDup"));
            this.Length = Convert.ToInt32(Element.AttributeText("Length"));
            this.WeekDay =  Convert.ToInt32(Element.AttributeText("WeekDay"));
            this.PeriodNo = Convert.ToInt32(Element.AttributeText("PeriodNo"));
            this.WeekdayCond = Element.AttributeText("WeekdayCond");
            this.PeriodCond = Element.AttributeText("PeriodCond");
            this.WeekFlag = Convert.ToByte(Element.AttributeText("WeekFlag"));
            this.Longbreak = Bool.Parse(Element.AttributeText("Longbreak"));
            this.Lock = Bool.Parse(Element.AttributeText("Lock"));
            this.CourseGroup = Element.AttributeText("CourseGroup");
            this.LimitNextDay = Bool.Parse(Element.AttributeText("LimitNextDay"));
            this.Comment = Element.AttributeText("Comment");
            #endregion
        }

        /// <summary>
        /// 從XML列載入
        /// </summary>
        /// <param name="Row"></param>
        /// <param name="DSNS"></param>
        public void Load(XElement Element, string DSNS)
        {
            string CourseSectionID = Element.ElementText("Uid");
            string CourseID = Element.ElementText("CourseID");
            string ClassroomID = Element.ElementText("ClassroomID");
            string ClassroomName = Element.ElementText("ClassroomName");
            int Weekday = Convert.ToInt32(Element.ElementText("Weekday"));
            int PeriodNo = Convert.ToInt32(Element.ElementText("Period"));
            int Length = Convert.ToInt32(Element.ElementText("Length"));
            string WeekdayCond = Element.ElementText("WeekdayCondition");
            string PeriodCond = Element.ElementText("PeriodCondition");
            string sWeekFlag = Element.ElementText("WeekFlag");
            byte WeekFlag = string.IsNullOrWhiteSpace(sWeekFlag) ? (byte)3 : Convert.ToByte(sWeekFlag);
            bool Longbreak = Bool.Parse(Element.ElementText("LongBreak"));
            bool Lock = Bool.Parse(Element.ElementText("Lock"));
            string SubjectName = Element.ElementText("Subject");
            string SubjectLevel = Element.ElementText("SubjectLevel");
            string SubjectFullName = !string.IsNullOrWhiteSpace(SubjectLevel) ? SubjectName + GetNumber(Convert.ToInt32(SubjectLevel)) : SubjectName;

            string strSubjectAliasName = Element.ElementText("SubjectAliasName");
            string strCourseName = Element.ElementText("CourseName");

            string ClassID = Element.ElementText("ClassID");
            bool AllowDup = Bool.Parse(Element.ElementText("AllowDuplicate"));

            string TimeTableID = Element.ElementText("TimeTableID");
            //string TimeTableName = Element.ElementText("TimeTableName");
            //string TeacherName = Element.ElementText("TeacherName");
            //string TeacherNickName = Element.ElementText("TeacherNickName");
            //string TeacherFullName = TeacherName + TeacherNickName;
            string TeacherName1 = Element.ElementText("TeacherName1");
            string TeacherName2 = Element.ElementText("TeacherName2");
            string TeacherName3 = Element.ElementText("TeacherName3");

            string CourseGroup = Element.ElementText("Group");
            bool LimitNextDay = Bool.Parse(Element.ElementText("LimitNextDay"));
            string Comment = Element.ElementText("Comment");

            #region 系統編號
            this.ID = DSNS + "," + CourseSectionID;
            this.CourseID = DSNS + "," + CourseID;
            this.ClassroomID = ClassroomName;
            this.ClassID = DSNS + "," + ClassID;
            this.TeacherName1 = TeacherName1;
            this.TeacherName2 = TeacherName2;
            this.TeacherName3 = TeacherName3;
            this.TimeTableID = DSNS + "," + TimeTableID;
            #endregion

            #region 排課屬性
            this.Subject = SubjectFullName;
            this.SubjectAlias = strSubjectAliasName;
            this.CourseName = strCourseName;
            this.AllowDup = AllowDup;
            this.Length = Length;
            this.WeekDay = Weekday;
            this.PeriodNo = PeriodNo;
            this.WeekdayCond = WeekdayCond;
            this.PeriodCond = PeriodCond;
            this.WeekFlag = WeekFlag;
            this.Longbreak = Longbreak;
            this.Lock = Lock;
            this.LimitNextDay = LimitNextDay;
            this.CourseGroup = CourseGroup;
            this.Comment = Comment;
            #endregion
        }

        /// <summary>
        /// 從資料列載入
        /// </summary>
        /// <param name="Row"></param>
        /// <param name="DSNS"></param>
        public void Load(DataRow Row, string DSNS)
        {
            string CourseSectionID = Row.Field<string>("uid");
            string CourseID = Row.Field<string>("ref_course_id");
            string ClassroomID = Row.Field<string>("ref_classroom_id");
            string ClassroomName = Row.Field<string>("classroomname");
            int Weekday = Convert.ToInt32(Row.Field<string>("weekday"));
            int PeriodNo = Convert.ToInt32(Row.Field<string>("period"));
            int Length = Convert.ToInt32(Row.Field<string>("length"));
            string WeekdayCond = Row.Field<string>("weekday_condition");
            string PeriodCond = Row.Field<string>("period_condition");
            string sWeekFlag = Row.Field<string>("week_flag");
            byte WeekFlag = string.IsNullOrWhiteSpace(sWeekFlag) ? (byte)3 : Convert.ToByte(sWeekFlag);
            bool Longbreak = Convert.ToBoolean(Row.Field<string>("long_break"));
            bool Lock = Convert.ToBoolean(Row.Field<string>("lock"));

            string SubjectName = Row.Field<string>("subject");
            string SubjectLevel = Row.Field<string>("subj_level");
            string SubjectFullName = !string.IsNullOrWhiteSpace(SubjectLevel) ? SubjectName + GetNumber(Convert.ToInt32(SubjectLevel)) : SubjectName;

            string strSubjectAliasName = Row.Field<string>("subject_alias_name");
            string strCourseName = Row.Field<string>("course_name");

            string ClassID = Row.Field<string>("ref_class_id");
            bool AllowDup = Convert.ToBoolean(Row.Field<string>("allow_duplicate"));
            string TimeTableID = Row.Field<string>("ref_timetable_id");
            string TimeTableName = Row.Field<string>("timetablename");
            string TeacherName = Row.Field<string>("teacher_name");
            string TeacherNickName = Row.Field<string>("nickname");
            string TeacherFullName = TeacherName + TeacherNickName;
            string TeacherName1 = Row.Field<string>("teacher_name_1");
            string TeacherName2 = Row.Field<string>("teacher_name_2");
            string TeacherName3 = Row.Field<string>("teacher_name_3");

            #region 系統編號
            this.ID = DSNS + "," + CourseSectionID;
            this.CourseID = DSNS +"," + CourseID;
            this.ClassroomID = ClassroomName;
            this.ClassID = DSNS + "," + ClassID;
            this.TeacherName1 = TeacherName1;
            this.TeacherName2 = TeacherName2;
            this.TeacherName3 = TeacherName3;
            this.TimeTableID = DSNS + "," + TimeTableID;
            #endregion

            #region 排課屬性
            this.Subject = SubjectFullName;
            this.SubjectAlias = strSubjectAliasName;
            this.CourseName = strCourseName;
            this.AllowDup = AllowDup;
            this.Length = Length;
            this.WeekDay = Weekday;
            this.PeriodNo = PeriodNo;
            this.WeekdayCond = WeekdayCond;
            this.PeriodCond = PeriodCond;
            this.WeekFlag = WeekFlag;
            this.Longbreak = Longbreak;
            this.Lock = Lock;
            #endregion
        }

        /// <summary>
        /// 轉換成XElement
        /// </summary>
        /// <returns></returns>
        public XElement ToXML()
        {
            XElement Element = new XElement("CourseSection");
            Element.SetAttributeValue("ID", ID);
            Element.SetAttributeValue("CourseID", CourseID);
            Element.SetAttributeValue("TeacherName1", TeacherName1);
            Element.SetAttributeValue("TeacherName2", TeacherName2);
            Element.SetAttributeValue("TeacherName3", TeacherName3);
            Element.SetAttributeValue("ClassID", ClassID);
            Element.SetAttributeValue("ClassroomID", ClassroomID);
            Element.SetAttributeValue("TimeTableID", TimeTableID);

            Element.SetAttributeValue("Subject",Subject);
            Element.SetAttributeValue("SubjectAlias", SubjectAlias);
            Element.SetAttributeValue("CourseName",CourseName);
            Element.SetAttributeValue("AllowDup", AllowDup);
            Element.SetAttributeValue("Length", Length);
            Element.SetAttributeValue("WeekDay", WeekDay);
            Element.SetAttributeValue("PeriodNo", PeriodNo);
            Element.SetAttributeValue("WeekdayCond", WeekdayCond);
            Element.SetAttributeValue("PeriodCond", PeriodCond);
            Element.SetAttributeValue("WeekFlag",WeekFlag);
            Element.SetAttributeValue("Longbreak", Longbreak);
            Element.SetAttributeValue("Lock",Lock);

            Element.SetAttributeValue("CourseGroup", this.CourseGroup);
            Element.SetAttributeValue("LimitNextDay", LimitNextDay);
            Element.SetAttributeValue("Comment", Comment);

            return Element;
        }
        #endregion

        #region static method
        /// <summary>
        /// 取得級別顯示名稱
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private static string GetNumber(int p)
        {
            string levelNumber;
            switch (p)
            {
                #region 對應levelNumber
                case 1:
                    levelNumber = "I";
                    break;
                case 2:
                    levelNumber = "II";
                    break;
                case 3:
                    levelNumber = "III";
                    break;
                case 4:
                    levelNumber = "IV";
                    break;
                case 5:
                    levelNumber = "V";
                    break;
                case 6:
                    levelNumber = "VI";
                    break;
                case 7:
                    levelNumber = "VII";
                    break;
                case 8:
                    levelNumber = "VIII";
                    break;
                case 9:
                    levelNumber = "IX";
                    break;
                case 10:
                    levelNumber = "X";
                    break;
                default:
                    levelNumber = "" + (p);
                    break;
                #endregion
            }
            return levelNumber;
        }

        ///// <summary>
        ///// 從單一資料來源取得課程分段資料
        ///// </summary>
        ///// <param name="Connection">連線物件</param>
        ///// <param name="SQL">SQL指令</param>
        ///// <returns></returns>
        //public static List<SCourseSection> Select(Connection Connection, string SQL)
        //{
        //    #region 取得課程分段原始資料
        //    QueryHelper Helper = new QueryHelper(Connection);
        //    DataTable vDataTable = Helper.Select(SQL);
        //    List<SCourseSection> CourseSections = new List<SCourseSection>();
        //    #endregion

        //    #region 將原始資料轉換成課程分段物件
        //    foreach (DataRow Row in vDataTable.Rows)
        //    {
        //        SCourseSection CourseSection = new SCourseSection();

        //        CourseSection.Load(Row,Connection.AccessPoint.Name);

        //        CourseSections.Add(CourseSection);
        //    }
        //    #endregion

        //    return CourseSections;
        //}

        /// <summary>
        /// 從單一資料來源取得課程分段資料
        /// </summary>
        /// <param name="Connection">連線物件</param>
        /// <param name="SQL">SQL指令</param>
        /// <returns></returns>
        public static List<SCourseSection> Select(Connection Connection,string SchoolYear,string Semester)
        {
            XElement Element = ContractService.GetCourseSectionNew(Connection, SchoolYear, Semester);

            #region 取得課程分段原始資料
            List<SCourseSection> CourseSections = new List<SCourseSection>();
            #endregion

            #region 將原始資料轉換成課程分段物件
            foreach (XElement SubElement in Element.Elements("CourseSection"))
            {
                SCourseSection CourseSection = new SCourseSection();

                CourseSection.Load(SubElement, Connection.AccessPoint.Name);

                CourseSections.Add(CourseSection);
            }
            #endregion

            return CourseSections;
        }

        /// <summary>
        /// 依學年度及學期從多個資料來源取得課程分段資料
        /// </summary>
        /// <param name="Connections"></param>
        /// <param name="SchoolYear"></param>
        /// <param name="Semester"></param>
        /// <returns></returns>
        public static SIntegrationResult<SCourseSection> Select(List<Connection> Connections, string SchoolYear, string Semester)
        {
            SIntegrationResult<SCourseSection> Result = new SIntegrationResult<SCourseSection>();

            #region 取得不同資料來源的課程分段，使用非同步執行
            Connections.ForEach(x =>
            {
                try
                {
                    List<SCourseSection> CourseSections = Select(x,SchoolYear,Semester);
                    Result.Data.AddRange(CourseSections);
                }
                catch (Exception e)
                {
                    Result.AddMessage("下載課程分段資料時發生錯誤，連線來源『" + x.AccessPoint.Name + "』");
                    Result.AddMessage(e.Message);
                    Result.IsSuccess = false;
                }
            }
            );
            #endregion

            #region 若有超過一個以上的連線資訊，則重新編號
            if (Result.IsSuccess)
                Result.AddMessage("已成功下載" + Result.Data.Count + "筆課程分段資料");
            #endregion

            return Result;
        }

        ///// <summary>
        ///// 依開始日期及結束日期從多個資料來源取得課程分段資料
        ///// </summary>
        ///// <param name="Connections"></param>
        ///// <param name="StartDate">開始日期</param>
        ///// <param name="EndDate">結束日期</param>
        ///// <returns></returns>
        //public static SIntegrationResult<SCourseSection> Select(List<Connection> Connections,DateTime StartDate,DateTime EndDate)
        //{
        //    #region 產生實際執行的SQL指令
        //    StringBuilder strBuilder = new StringBuilder();
        //    strBuilder.AppendFormat(NativeQuery.WeekCourseSectionTemplateSQL, StartDate.ToShortDateString(), EndDate.ToShortDateString());
        //    string strExecuteSQL = strBuilder.ToString();
        //    #endregion

        //    return Select(Connections, strExecuteSQL);
        //}

        /// <summary>
        /// 依各學校的學年度及學期取得學期課程分段（用來做虛擬課程分段）
        /// </summary>
        /// <param name="Connections">多個連線來源</param>
        /// <param name="DSNSSchoolYearSemesters">連線來源的學年度及學期</param>
        /// <returns></returns>
        public static SIntegrationResult<SCourseSection> Select(List<Connection> Connections,Dictionary<string,SchoolYearSemester> DSNSSchoolYearSemesters)
        {
            SIntegrationResult<SCourseSection> Result = new SIntegrationResult<SCourseSection>();

            #region 取得不同資料來源的課程分段，使用非同步執行
            Connections.ForEach(x =>
            {
                try
                {
                    if (DSNSSchoolYearSemesters.ContainsKey(x.AccessPoint.Name))
                    {
                        #region 產生實際執行的SQL指令
                        //StringBuilder strBuilder = new StringBuilder();

                        //strBuilder.AppendFormat(NativeQuery.ScheduledCourseSectionTemplateSQL, DSNSSchoolYearSemesters[x.AccessPoint.Name].SchoolYear, DSNSSchoolYearSemesters[x.AccessPoint.Name].Semester);
                        //string strExecuteSQL = strBuilder.ToString();
                        #endregion

                        List<SCourseSection> CourseSections = Select(x, DSNSSchoolYearSemesters[x.AccessPoint.Name].SchoolYear,DSNSSchoolYearSemesters[x.AccessPoint.Name].Semester);
                        Result.Data.AddRange(CourseSections);
                    }
                }
                catch (Exception e)
                {
                    Result.AddMessage("下載課程分段資料時發生錯誤，連線來源『" + x.AccessPoint.Name + "』");
                    Result.AddMessage(e.Message);
                    Result.IsSuccess = false;
                }
            }
            );
            #endregion

            #region 若有超過一個以上的連線資訊，則重新編號
            if (Result.IsSuccess)
                Result.AddMessage("已成功下載" + Result.Data.Count + "筆課程分段資料");
            #endregion

            return Result;
        }

        ///// <summary>
        ///// 根據測試的SQL指定下載課程分段資料
        ///// </summary>
        ///// <param name="Connections"></param>
        ///// <returns></returns>
        //public static SIntegrationResult<SCourseSection> SelectTest(List<Connection> Connections)
        //{
        //    return Select(Connections, NativeQuery.CourseSectionTestSQL);
        //}

        ///// <summary>
        ///// 從多個資料來源取得課程分段資料
        ///// </summary>
        ///// <param name="Connections"></param>
        ///// <param name="SchoolYear"></param>
        ///// <param name="Semester"></param>
        ///// <returns></returns>
        //public static SIntegrationResult<SCourseSection> Select(List<Connection> Connections,string SQL)
        //{
        //    SIntegrationResult<SCourseSection> Result = new SIntegrationResult<SCourseSection>();

        //    #region 取得不同資料來源的課程分段，使用非同步執行
        //    //Parallel.ForEach(Connections.ToConcurrentQueue(), x =>
        //    Connections.ForEach(x=>
        //    {
        //        try
        //        {
        //            List<SCourseSection> CourseSections = Select(x, SQL);
        //            Result.Data.AddRange(CourseSections);
        //        }
        //        catch (Exception e)
        //        {
        //            Result.AddMessage("下載課程分段資料時發生錯誤，連線來源『" + x.AccessPoint.Name + "』");
        //            Result.AddMessage(e.Message);
        //            Result.IsSuccess = false;
        //        }
        //    }
        //    );
        //    #endregion

        //    #region 若有超過一個以上的連線資訊，則重新編號
        //    if (Result.IsSuccess)
        //        Result.AddMessage("已成功下載" + Result.Data.Count + "筆課程分段資料");
        //    #endregion

        //    return Result;

        //    #region VB
        //    //RaiseEvent CreateDBProgress(50)
        //    //CopyTable cnSQL, cnJET, _
        //    //"SELECT CourseSection.* FROM CourseSection INNER JOIN Course ON CourseSection.CourseID = Course.CourseID WHERE Course.SchoolYear=" & SchoolYear & " AND Course.Semester=" & Semester, _
        //    //"CourseSection"
        //    #endregion
        //}
        #endregion
    }
}