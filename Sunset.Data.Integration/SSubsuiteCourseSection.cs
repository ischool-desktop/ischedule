using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using FISCA.Data;
using FISCA.DSAClient;

namespace Sunset.Data.Integration
{
    /// <summary>
    /// 代課記錄
    /// </summary>
    public class SSubsuiteCourseSection : ICreateTime
    {
        /// <summary>
        /// 代課系統編號，為DSNS加上來源系統編號
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 課程分段系統編號，為DSNS加上來源系統編號
        /// </summary>
        public string CourseSectionID { get; set; }

        /// <summary>
        /// 代課日期
        /// </summary>
        public DateTime SubstituteDate { get; set; }

        /// <summary>
        /// 代課教師系統編號，為教師名稱
        /// </summary>
        public string TeacherID { get; set; }

        /// <summary>
        /// 假別名稱
        /// </summary>
        public string AbsenceName { get; set; }

        /// <summary>
        /// 鐘點費
        /// </summary>
        public int HourlyPay { get; set; }

        /// <summary>
        /// 代課建立時間
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 代課原因
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 從資料載入
        /// </summary>
        /// <param name="Row"></param>
        /// <param name="DSNS"></param>
        public void Load(DataRow Row, string DSNS)
        {
            string ID = Row.Field<string>("uid");
            string CourseSectionID = Row.Field<string>("ref_course_section_id");
            string TeacherName = Row.Field<string>("teacher_name");
            string AbsenceName = Row.Field<string>("absence_name");
            string SubstituteDate = Row.Field<string>("substitute_date");
            string CreateTime = Row.Field<string>("create_time");
            string HourlyPay = Row.Field<string>("hourly_pay");
            string Reason = Row.Field<string>("reason");

            #region 實際存到物件中
            this.ID = DSNS + "," + ID;
            this.CourseSectionID = DSNS + "," + CourseSectionID;
            this.TeacherID = TeacherName;  //教師ID為教師姓名
            this.AbsenceName = AbsenceName;
            this.HourlyPay = int.Parse(HourlyPay);
            this.SubstituteDate = DateTime.Parse(SubstituteDate);
            this.CreateTime = DateTime.Parse(CreateTime);
            this.Reason = Reason;
            #endregion
        }

        /// <summary>
        /// 從單一資料來源取得調課資料
        /// </summary>
        /// <param name="Connection">連線物件</param>
        /// <param name="SQL">SQL指令</param>
        /// <returns></returns>
        public static List<SSubsuiteCourseSection> Select(Connection Connection, string SQL)
        {
            #region 取得課程分段原始資料
            QueryHelper Helper = new QueryHelper(Connection);
            DataTable vDataTable = Helper.Select(SQL);
            List<SSubsuiteCourseSection> CourseSections = new List<SSubsuiteCourseSection>();
            #endregion

            #region 將原始資料轉換成課程分段物件
            foreach (DataRow Row in vDataTable.Rows)
            {
                SSubsuiteCourseSection CourseSection = new SSubsuiteCourseSection();

                CourseSection.Load(Row, Connection.AccessPoint.Name);

                CourseSections.Add(CourseSection);
            }
            #endregion

            return CourseSections;
        }


        /// <summary>
        /// 依開始日期及結束日期從多個資料來源取得代課資料
        /// </summary>
        /// <param name="Connections"></param>
        /// <param name="StartDate">開始日期</param>
        /// <param name="EndDate">結束日期</param>
        /// <returns></returns>
        public static SIntegrationResult<SSubsuiteCourseSection> Select(List<Connection> Connections, DateTime StartDate, DateTime EndDate)
        {
            #region 取得不同資料來源的調課記錄，使用非同步執行
            SIntegrationResult<SSubsuiteCourseSection> Result = new SIntegrationResult<SSubsuiteCourseSection>();

            Connections.ForEach(x =>
            {
                try
                {
                    #region 產生實際執行的SQL指令
                    StringBuilder strBuilder = new StringBuilder();
                    strBuilder.AppendFormat(NativeQuery.SubstituteCourseSectionTemplateSQL, StartDate.ToShortDateString(), EndDate.ToShortDateString());
                    string strExecuteSQL = strBuilder.ToString();
                    #endregion

                    List<SSubsuiteCourseSection> Subsuites = Select(x, strExecuteSQL);
                    Result.Data.AddRange(Subsuites);
                }
                catch (Exception e)
                {
                    Result.AddMessage("下載代課資料時發生錯誤，連線來源『" + x.AccessPoint.Name + "』");
                    Result.AddMessage(e.Message);
                    Result.IsSuccess = false;
                }
            }
            );
            #endregion

            if (Result.IsSuccess)
                Result.AddMessage("已成功下載" + Result.Data.Count + "筆代課資料");

            return Result;
        }
    }
}