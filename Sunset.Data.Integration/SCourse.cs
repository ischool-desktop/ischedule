//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Text;
//using System.Threading.Tasks;
//using FISCA.Data;
//using FISCA.DSAClient;

//namespace Sunset.Data.Integration
//{
//    /// <summary>
//    /// 排課來源課程資料
//    /// </summary>
//    public class SCourse
//    {
//        #region public property
//        /// <summary>
//        /// 排課系統使用的課程系統編號
//        /// </summary>
//        public string ID { get; set; }

//        /// <summary>
//        /// 科目名稱，高中為科目名稱加級別
//        /// </summary>
//        public string Subject { get; set; }

//        /// <summary>
//        /// 班級系統編號，為DSNS加來源班級系統編號
//        /// </summary>
//        public string ClassID { get; set; }

//        /// <summary>
//        /// 教師系統編號，為教師名稱
//        /// </summary>
//        public string TeacherID { get; set; }

//        /// <summary>
//        /// 時間表系統編號，為時間表名稱
//        /// </summary>
//        public string TimeTableID { get; set; }

//        /// <summary>
//        /// 是否允許課程分段排在同天
//        /// </summary>
//        public bool AllowDup { get; set; }

//        /// <summary>
//        /// 輸出成字串
//        /// </summary>
//        /// <returns></returns>
//        public override string ToString()
//        {
//            return ID + "," + "," + Subject + "," + ClassID + "," + 
//                TeacherID + "," + TimeTableID + "," + AllowDup;
//        }
//        #endregion

//        #region static method
//        /// <summary>
//        /// 取得級別顯示名稱
//        /// </summary>
//        /// <param name="p"></param>
//        /// <returns></returns>
//        private static string GetNumber(int p)
//        {
//            string levelNumber;
//            switch (p)
//            {
//                #region 對應levelNumber
//                case 1:
//                    levelNumber = "I";
//                    break;
//                case 2:
//                    levelNumber = "II";
//                    break;
//                case 3:
//                    levelNumber = "III";
//                    break;
//                case 4:
//                    levelNumber = "IV";
//                    break;
//                case 5:
//                    levelNumber = "V";
//                    break;
//                case 6:
//                    levelNumber = "VI";
//                    break;
//                case 7:
//                    levelNumber = "VII";
//                    break;
//                case 8:
//                    levelNumber = "VIII";
//                    break;
//                case 9:
//                    levelNumber = "IX";
//                    break;
//                case 10:
//                    levelNumber = "X";
//                    break;
//                default:
//                    levelNumber = "" + (p);
//                    break;
//                #endregion
//            }
//            return levelNumber;
//        }

//        /// <summary>
//        /// 從單一資料來源取得課程資料
//        /// </summary>
//        /// <param name="Connection">連線物件</param>
//        /// <param name="strSQL">SQL指令</param>
//        /// <returns></returns>
//        public static List<SCourse> Select(Connection Connection, string strSQL)
//        {
//            #region 取得課程原始資料
//            QueryHelper Helper = new QueryHelper(Connection);
//            DataTable vDataTable = Helper.Select(strSQL);
//            List<SCourse> Courses = new List<SCourse>();
//            #endregion

//            #region 將原始資料轉換成課程物件
//            foreach (DataRow Row in vDataTable.Rows)
//            {
//                string CourseID = Row.Field<string>("id");
//                string SubjectName = Row.Field<string>("subject");
//                string SubjectLevel = Row.Field<string>("subj_level");
//                string SubjectFullName = !string.IsNullOrWhiteSpace(SubjectLevel) ? SubjectName + GetNumber(Convert.ToInt32(SubjectLevel)) : SubjectName;
//                string ClassID = Row.Field<string>("ref_class_id");
//                string TeacherID = Row.Field<string>("ref_teacher_id");
//                string TeacherFullName = Row.Field<string>("teacher_name") + Row.Field<string>("nickname");
//                string TimeTableID = Row.Field<string>("timetableid");
//                string TimeTableName = Row.Field<string>("timetablename");
//                bool AllowDup = Convert.ToBoolean(Row.Field<string>("allowdup"));

//                SCourse Course = new SCourse();
//                Course.ID = Connection.AccessPoint.Name +","+ CourseID;
//                Course.Subject = SubjectFullName;
//                Course.ClassID = Connection.AccessPoint.Name +","+ ClassID;
//                Course.TeacherID = TeacherFullName;
//                Course.TimeTableID = TimeTableName;

//                Courses.Add(Course);
//            }
//            #endregion

//            return Courses;
//        }

//        /// <summary>
//        /// 根據學年度及學期下載課程資料
//        /// </summary>
//        /// <param name="Connections"></param>
//        /// <param name="SchoolYear"></param>
//        /// <param name="Semester"></param>
//        /// <returns></returns>
//        public static SIntegrationResult<SCourse> Select(List<Connection> Connections, int SchoolYear, int Semester)
//        {
//            #region 產生實際執行的SQL指令
//            StringBuilder strBuilder = new StringBuilder();
//            strBuilder.AppendFormat(NativeQuery.CourseTemplateSQL, SchoolYear, Semester);
//            string strExecuteSQL = strBuilder.ToString();
//            #endregion

//            return Select(Connections, strExecuteSQL);
//        }

//        /// <summary>
//        /// 根據測試的SQL指定下載課程資料
//        /// </summary>
//        /// <param name="Connections"></param>
//        /// <returns></returns>
//        public static SIntegrationResult<SCourse> SelectTest(List<Connection> Connections)
//        {
//            return Select(Connections, NativeQuery.CourseTestSQL);
//        }

//        /// <summary>
//        /// 從多個資料來源取得課程資料
//        /// </summary>
//        /// <param name="Connections"></param>
//        /// <param name="SchoolYear"></param>
//        /// <param name="Semester"></param>
//        /// <returns></returns>
//        public static SIntegrationResult<SCourse> Select(List<Connection> Connections,string SQL)
//        {
//            #region 取得不同資料來源的課程，使用非同步執行
//            SIntegrationResult<SCourse> Result = new SIntegrationResult<SCourse>();

//            Parallel.ForEach(Connections.ToConcurrentQueue(), x =>
//            {
//                try
//                {
//                    List<SCourse> Courses = Select(x, SQL);
//                    Result.Data.AddRange(Courses);
//                }
//                catch (Exception e)
//                {
//                    Result.AddMessage("下載課程資料時發生錯誤，連線來源『" + x.AccessPoint.Name + "』");
//                    Result.AddMessage(e.Message);
//                    Result.IsSuccess = false;
//                }
//            }
//            );
//            #endregion

//            if (Result.IsSuccess)
//                Result.AddMessage("已成功下載" + Result.Data.Count + "筆課程資料");

//            return Result;

//            #region VB
//            // RaiseEvent CreateDBProgress(15)
//            //CopyTable cnSQL, cnJET, _
//            //    "SELECT Course.*,c.ClassID FROM Course INNER JOIN (SELECT Course.CourseID,MIN(CCRequire.ClassID) ClassID FROM Course LEFT OUTER JOIN CCRequire ON Course.CourseID=CCRequire.CourseID WHERE Course.SchoolYear=" & SchoolYear & " AND Course.Semester=" & Semester & " GROUP BY Course.CourseID) c ON Course.CourseID=c.CourseID", _
//            //    "Course"
//            #endregion
//        }
//        #endregion
//    }
//}