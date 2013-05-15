using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using FISCA.Data;
using FISCA.DSAClient;

namespace Sunset.Data.Integration
{
    /// <summary>
    /// 調課記錄
    /// </summary>
    public class SExchangeCourseSection : ICreateTime
    {
        /// <summary>
        /// 來源調課記錄系統編號，為DSNS加上來源系統編號
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 來源課程分段系統編號，為DSNS加上來源系統編號
        /// </summary>
        public string SrcCourseSectionID { get; set; }

        /// <summary>
        /// 目標課程分段系統編號，為DSNS加上目標系統編號
        /// </summary>
        public string DesCourseSectionID { get; set; }

        /// <summary>
        /// 來源調課日期
        /// </summary>
        public DateTime SrcExchangeDate { get; set; }

        /// <summary>
        /// 目標調課日期
        /// </summary>
        public DateTime DesExchangeDate { get; set; }

        /// <summary>
        /// 調課建立時間
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 調課原因
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
            string SrcCourseSectionID = Row.Field<string>("src_course_section_id");
            string DesCourseSectionID = Row.Field<string>("des_course_section_id");
            string SrcExchangeDate = Row.Field<string>("src_exchange_date");
            string DesExchangeDate = Row.Field<string>("des_exchange_date");
            string CreateTime = Row.Field<string>("create_time");
            string Reason = Row.Field<string>("reason");

            #region 實際存到物件中
            this.ID = DSNS + "," + ID;
            this.SrcCourseSectionID = DSNS + "," + SrcCourseSectionID;
            this.DesCourseSectionID = DSNS + "," + DesCourseSectionID;
            this.SrcExchangeDate = DateTime.Parse(SrcExchangeDate);
            this.DesExchangeDate = DateTime.Parse(DesExchangeDate);
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
        public static List<SExchangeCourseSection> Select(Connection Connection, string SQL)
        {
            #region 取得課程分段原始資料
            QueryHelper Helper = new QueryHelper(Connection);
            DataTable vDataTable = Helper.Select(SQL);
            List<SExchangeCourseSection> CourseSections = new List<SExchangeCourseSection>();
            #endregion

            #region 將原始資料轉換成課程分段物件
            foreach (DataRow Row in vDataTable.Rows)
            {
                SExchangeCourseSection CourseSection = new SExchangeCourseSection();

                CourseSection.Load(Row, Connection.AccessPoint.Name);

                CourseSections.Add(CourseSection);
            }
            #endregion

            return CourseSections;
        }

        /// <summary>
        /// 依開始日期及結束日期從多個資料來源取得調課資料
        /// </summary>
        /// <param name="Connections">多個連線來源</param>
        /// <param name="StartDate">開始日期</param>
        /// <param name="EndDate">結束日期</param>
        /// <returns></returns>
        public static SIntegrationResult<SExchangeCourseSection> Select(List<Connection> Connections, DateTime StartDate,DateTime EndDate)
        {
            #region 取得不同資料來源的調課記錄，使用非同步執行
            SIntegrationResult<SExchangeCourseSection> Result = new SIntegrationResult<SExchangeCourseSection>();

            Connections.ForEach(x =>
            {
                try
                {
                    #region 產生實際執行的SQL指令
                    StringBuilder strBuilder = new StringBuilder();
                    strBuilder.AppendFormat(NativeQuery.ExchangeCourseSectionTemplateSQL,StartDate.ToShortDateString(),EndDate.ToShortDateString());
                    string strExecuteSQL = strBuilder.ToString();
                    #endregion

                    List<SExchangeCourseSection> Exchanges = Select(x, strExecuteSQL);
                    Result.Data.AddRange(Exchanges);
                }
                catch (Exception e)
                {
                    Result.AddMessage("下載調課資料時發生錯誤，連線來源『" + x.AccessPoint.Name + "』");
                    Result.AddMessage(e.Message);
                    Result.IsSuccess = false;
                }
            }
            );
            #endregion

            if (Result.IsSuccess)
                Result.AddMessage("已成功下載" + Result.Data.Count + "筆調課資料");

            return Result;
        }
    }
}