using System;
using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using FISCA.DSAClient;

namespace Sunset.Data.Integration
{
    /// <summary>
    /// 排課來源班級資料
    /// </summary>
    public class SClass
    {
        #region public property
        /// <summary>
        /// 排課系統使用的班級系統編號，為DSNS加上原本系統編號
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 課程名稱
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 時間表系統編號，為時間表名稱
        /// </summary>
        public string TimeTableID { get; set; }

        /// <summary>
        /// 班導師姓名
        /// </summary>
        public string TeacherName { get; set; }

        /// <summary>
        /// 年級
        /// </summary>
        public string GradeYear { get; set; }

        /// <summary>
        /// 命名規則
        /// </summary>
        public string NamingRule { get; set; }

        /// <summary>
        /// 輸出成字串
        /// </summary>
        /// <returns></returns>
        public override string  ToString()
        {
            return ID + "," + ClassName + "," + TimeTableID + "," + TeacherName+","+GradeYear+","+NamingRule;
        }

        /// <summary>
        /// 從XML載入
        /// </summary>
        /// <param name="Element"></param>
        public void Load(XElement Element)
        {
            ID = Element.AttributeText("ID");
            ClassName = Element.AttributeText("ClassName");
            TimeTableID = Element.AttributeText("TimeTableID");
            TeacherName = Element.AttributeText("TeacherName");
            GradeYear = Element.AttributeText("GradeYear");
            NamingRule = Element.AttributeText("NamingRule");
        }

        /// <summary>
        /// 從資料列載入
        /// </summary>
        /// <param name="Row"></param>
        /// <param name="DSNS"></param>
        public void Load(XElement Element, string DSNS)
        {
            //<Class>
            //    <ID>1477</ID>
            //    <ClassName>實驗五乙</ClassName>
            //    <TimeTableID/>
            //    <TimeTableName/>
            //</Class>

            string ClassID = Element.ElementText("ID");
            string ClassName = Element.ElementText("ClassName");
            string TimeTableID = Element.ElementText("TimeTableID");
            //string TimeTableName = Element.ElementText("TimeTableName"); //不會用到
            string TeacherName = Element.ElementText("TeacherName");
            string GradeYear = Element.ElementText("GradeYear");
            string NamingRule = Element.ElementText("NamingRule");

            this.ID = DSNS + "," + ClassID;
            this.TimeTableID = string.IsNullOrEmpty(TimeTableID) ? string.Empty : DSNS + "," + TimeTableID; //若TimeTableIDe為空白就為空白
            this.ClassName = ClassName;
            this.TeacherName = TeacherName;
            this.GradeYear = GradeYear;
            this.NamingRule = NamingRule;
        }

        /// <summary>
        /// 從資料列載入
        /// </summary>
        /// <param name="Row"></param>
        /// <param name="DSNS"></param>
        //public void Load(DataRow Row,string DSNS)
        //{
        //    string ClassID = Row.Field<string>("id");
        //    string ClassName = Row.Field<string>("class_name");
        //    string TimeTableID = Row.Field<string>("ref_timetable_id");
        //    //string TimeTableName = Row.Field<string>("timetablename"); //不會用到
        //    string TeacherName = Row.Field<string>("teacher_name");

        //    this.ID = DSNS + "," + ClassID;
        //    this.TimeTableID = string.IsNullOrEmpty(TimeTableID) ? string.Empty : DSNS + "," + TimeTableID;
        //    this.ClassName = ClassName;
        //    this.TeacherName = TeacherName;
        //}

        /// <summary>
        /// 轉換成XElement
        /// </summary>
        /// <returns></returns>
        public XElement ToXML()
        {
            XElement Element = new XElement("Class");
            Element.SetAttributeValue("ID",ID);
            Element.SetAttributeValue("ClassName",ClassName);
            Element.SetAttributeValue("TimeTableID",TimeTableID);
            Element.SetAttributeValue("TeacherName", TeacherName);
            Element.SetAttributeValue("GradeYear", GradeYear);
            Element.SetAttributeValue("NamingRule", NamingRule);

            return Element;
        }
        #endregion

        #region public static method
        ///// <summary>
        ///// 從單一資料來源取得班級資料
        ///// </summary>
        ///// <param name="Connection">連線物件</param>
        ///// <param name="strSQL">SQL指令</param>
        ///// <returns></returns>
        //public static List<SClass> Select(Connection Connection, string strSQL)
        //{
        //    #region 取得班級原始資料
        //    QueryHelper Helper = new QueryHelper(Connection);

        //    DataTable vDataTable = Helper.Select(strSQL);

        //    string DSNS = Connection.AccessPoint.Name;

        //    List<SClass> Classes = new List<SClass>();
        //    #endregion

        //    #region 將原始資料轉換成班級物件
        //    foreach (DataRow Row in vDataTable.Rows)
        //    {
        //        SClass Class = new SClass();
        //        Class.Load(Row, DSNS);
        //        Classes.Add(Class);
        //    }
        //    #endregion

        //    return Classes;
        //}

        /// <summary>
        /// 從單一資料來源取得班級資料
        /// </summary>
        /// <param name="Connection">連線物件</param>
        /// <param name="SchoolYear">學年度</param>
        /// <param name="Semester">學期</param>
        /// <returns>班級物件列表</returns>
        public static List<SClass> Select(Connection Connection, string SchoolYear, string Semester)
        {
            XElement Element = ContractService.GetClass(Connection, SchoolYear, Semester);

            string DSNS = Connection.AccessPoint.Name;

            List<SClass> Classes = new List<SClass>();
            #endregion

            #region 將原始資料轉換成班級物件
            foreach (XElement SubElement in Element.Elements("Class"))
            {
                SClass Class = new SClass();
                Class.Load(SubElement, DSNS);
                Classes.Add(Class);
            }
            #endregion

            return Classes;
        }

        /// <summary>
        /// 根據學年度學期從多個來源下載資料
        /// </summary>
        /// <param name="Connections">多個資料來源</param>
        /// <param name="SchoolYear">學年度</param>
        /// <param name="Semester">學期</param>
        /// <returns></returns>
        public static SIntegrationResult<SClass> Select(List<Connection> Connections, string SchoolYear, string Semester)
        {
            #region 取得不同資料來源的班級，使用非同步執行
            SIntegrationResult<SClass> Result = new SIntegrationResult<SClass>();

            //Parallel.ForEach(Connections.ToConcurrentQueue(), x =>
            Connections.ForEach(x =>
            {
                try
                {
                    List<SClass> Classes = Select(x, SchoolYear, Semester);
                    Result.Data.AddRange(Classes);
                }
                catch (Exception e)
                {
                    Result.AddMessage("下載班級資料時發生錯誤，連線來源『" + x.AccessPoint.Name + "』");
                    Result.AddMessage(e.Message);
                    Result.IsSuccess = false;
                }
            }
            );
            #endregion

            if (Result.IsSuccess)
                Result.AddMessage("已成功下載" + Result.Data.Count + "筆班級資料");

            return Result;


            //StringBuilder strBuilder = new StringBuilder();
            //strBuilder.AppendFormat(NativeQuery.ClassTemplateSQL , SchoolYear, Semester);
            //string strExecuteSQL = strBuilder.ToString();

            //return Select(Connections, strExecuteSQL);
        }

        ///// <summary>
        ///// 根據開始日期及結束日期從週課程分段下載資料（舊版已未使用）
        ///// </summary>
        ///// <param name="Connections">多個資料來源</param>
        ///// <param name="StartDate">開始日期</param>
        ///// <param name="EndDate">結束日期</param>
        ///// <returns></returns>
        //public static SIntegrationResult<SClass> Select(List<Connection> Connections,DateTime StartDate,DateTime EndDate)
        //{
        //    StringBuilder strBuilder = new StringBuilder();
        //    strBuilder.AppendFormat(NativeQuery.WeekClassTemplateSQL,
        //        StartDate.ToShortDateString(),
        //        EndDate.ToShortDateString());

        //    string strExecuteSQL = strBuilder.ToString();

        //    return Select(Connections, strExecuteSQL);
        //}

        /// <summary>
        /// 根據多個連線來源的學年度及學期下載班級，只有該學期有的班級資料。
        /// </summary>
        /// <param name="Connections">多個連線來源</param>
        /// <param name="DSNSSchoolYearSemesters">連線來源的學年度及學期</param>
        /// <returns></returns>
        public static SIntegrationResult<SClass> Select(List<Connection> Connections, Dictionary<string, SchoolYearSemester> DSNSSchoolYearSemesters)
        {
            #region 取得不同資料來源的班級，使用非同步執行
            SIntegrationResult<SClass> Result = new SIntegrationResult<SClass>();

            Connections.ForEach(x =>
            {
                try
                {
                    if (DSNSSchoolYearSemesters.ContainsKey(x.AccessPoint.Name))
                    {
                        //StringBuilder strBuilder = new StringBuilder();
                        //strBuilder.AppendFormat(NativeQuery.ClassTemplateSQL, DSNSSchoolYearSemesters[x.AccessPoint.Name].SchoolYear, DSNSSchoolYearSemesters[x.AccessPoint.Name].Semester);
                        //string strExecuteSQL = strBuilder.ToString();

                        List<SClass> Classes = Select(x, 
                            DSNSSchoolYearSemesters[x.AccessPoint.Name].SchoolYear,
                            DSNSSchoolYearSemesters[x.AccessPoint.Name].Semester);
                        Result.Data.AddRange(Classes);
                    }
                }
                catch (Exception e)
                {
                    Result.AddMessage("下載班級資料時發生錯誤，連線來源『" + x.AccessPoint.Name + "』");
                    Result.AddMessage(e.Message);
                    Result.IsSuccess = false;
                }
            }
            );
            #endregion

            if (Result.IsSuccess)
                Result.AddMessage("已成功下載" + Result.Data.Count + "筆班級資料");

            return Result;
        }

        ///// <summary>
        ///// 根據測試的SQL指定下載班級資料
        ///// </summary>
        ///// <param name="Connections"></param>
        ///// <returns></returns>
        //public static SIntegrationResult<SClass> SelectTest(List<Connection> Connections)
        //{
        //    return Select(Connections,NativeQuery.ClassTestSQL);
        //}

        ///// <summary>
        ///// 從多個資料來源取得班級資料
        ///// </summary>
        ///// <param name="Connections"></param>
        ///// <param name="SchoolYear"></param>
        ///// <param name="Semester"></param>
        ///// <returns></returns>
        //public static SIntegrationResult<SClass> Select(List<Connection> Connections,string SQL)
        //{
        //    #region 取得不同資料來源的班級，使用非同步執行
        //    SIntegrationResult<SClass> Result = new SIntegrationResult<SClass>();

        //    //Parallel.ForEach(Connections.ToConcurrentQueue(), x =>
        //    Connections.ForEach(x=>
        //        {
        //            try
        //            {
        //                List<SClass> Classes = Select(x, SQL);
        //                Result.Data.AddRange(Classes);
        //            }
        //            catch (Exception e)
        //            {
        //                Result.AddMessage("下載班級資料時發生錯誤，連線來源『"+x.AccessPoint.Name+"』");
        //                Result.AddMessage(e.Message);
        //                Result.IsSuccess = false;
        //            }
        //        }
        //    );
        //    #endregion

        //    if (Result.IsSuccess)
        //        Result.AddMessage("已成功下載" + Result.Data.Count + "筆班級資料");

        //    return Result;

        //    #region VB
        //    //RaiseEvent CreateDBProgress(5)
        //    //CopyTable cnSQL, cnJET, _
        //    //    "SELECT DISTINCT Class.* FROM Course INNER JOIN CCRequire ON Course.CourseID = CCRequire.CourseID INNER JOIN Class ON CCRequire.ClassID = Class.ClassID WHERE Course.SchoolYear=" & SchoolYear & " AND Course.Semester=" & Semester, _
        //    //    "Class"
        //    #endregion
        //}
    }
}