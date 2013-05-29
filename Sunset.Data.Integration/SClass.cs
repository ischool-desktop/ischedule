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

        #region public static method
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
            SIntegrationResult<SClass> Result = new SIntegrationResult<SClass>();

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

            if (Result.IsSuccess)
                Result.AddMessage("已成功下載" + Result.Data.Count + "筆班級資料");

            return Result;
        }

        /// <summary>
        /// 從單一資料來源取得班級資料
        /// </summary>
        /// <param name="Connection">連線物件</param>
        /// <param name="SchoolYear">學年度</param>
        /// <param name="Semester">學期</param>
        /// <returns>班級物件列表</returns>
        public static List<SClass> Select(Connection Connection)
        {
            XElement Element = ContractService.GetClassEx(Connection);

            string DSNS = Connection.AccessPoint.Name;

            List<SClass> Classes = new List<SClass>();

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
        public static SIntegrationResult<SClass> Select(List<Connection> Connections)
        {
            SIntegrationResult<SClass> Result = new SIntegrationResult<SClass>();

            Connections.ForEach(x =>
            {
                try
                {
                    List<SClass> Classes = Select(x);
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

            if (Result.IsSuccess)
                Result.AddMessage("已成功下載" + Result.Data.Count + "筆班級資料");

            return Result;
        }
        #endregion
    }
}