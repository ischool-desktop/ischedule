using System;
using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
//using FISCA.Data;
using FISCA.DSAClient;

namespace Sunset.Data.Integration
{
    /// <summary>
    /// 排課來源時間表分段
    /// </summary>
    public class STimeTableSec
    {
        /// <summary>
        /// 時間表系統編號，為DSNS加上來源TimeTableID
        /// </summary>
        public string TimeTableID { get; set;}

        /// <summary>
        /// 星期
        /// </summary>
        public int WeekDay { get; set;}

        /// <summary>
        /// 節次
        /// </summary>
        public int PeriodNo { get; set;}

        /// <summary>
        /// 開始時間
        /// </summary>
        public DateTime Begintime { get; set;}

        /// <summary>
        /// 持續分鐘
        /// </summary>
        public int Duration { get; set;}

        /// <summary>
        /// 顯示節次
        /// </summary>
        public int DispPeriod { get; set;}

        /// <summary>
        /// 地點系統編號，為地點名稱
        /// </summary>
        public string LocationID { get; set;}

        /// <summary>
        /// 不排課
        /// </summary>
        public bool Disable { get; set; }

        /// <summary>
        /// 不排課訊息
        /// </summary>
        public string DisableMessage { get; set; }

        /// <summary>
        /// 輸出成字串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            List<string> Values = new List<string>() { TimeTableID, "" + WeekDay, "" + PeriodNo, Begintime.ToShortTimeString(), "" + Duration, "" + DispPeriod, LocationID,""+Disable,DisableMessage};

            return string.Join(",", Values.ToArray());
        }

        /// <summary>
        /// 從XML載入
        /// </summary>
        /// <param name="Element"></param>
        public void Load(XElement Element)
        {
            this.TimeTableID = Element.AttributeText("TimeTableID");
            this.WeekDay = Convert.ToInt32(Element.AttributeText("WeekDay"));
            this.PeriodNo = Convert.ToInt32(Element.AttributeText("PeriodNo"));
            this.DispPeriod = Convert.ToInt32(Element.AttributeText("DispPeriod"));
            this.Begintime = Convert.ToDateTime(Element.AttributeText("Begintime"));
            this.Duration = Convert.ToInt32(Element.AttributeText("Duration"));
            this.LocationID = Element.AttributeText("LocationID");
            this.Disable = Bool.Parse(Element.AttributeText("Disable"));
            this.DisableMessage = Element.AttributeText("DisableMessage");
        }

        /// <summary>
        /// 從資料列載入
        /// </summary>
        /// <param name="Row"></param>
        /// <param name="DSNS"></param>
        public void Load(XElement Element,string DSNS)
        {
            string TimeTableID = Element.ElementText("TimeTableID");
            string TimeTableName = Element.ElementText("TimeTableName");
            int WeekDay = Convert.ToInt32(Element.ElementText("Weekday"));
            int PeriodNo = Convert.ToInt32(Element.ElementText("Period"));
            DateTime BeginTime = Convert.ToDateTime(Element.ElementText("BeginTime"));
            int Duration = Convert.ToInt32(Element.ElementText("Duration"));
            int DisplayPeriod = Convert.ToInt32(Element.ElementText("DisplayPeriod"));
            string LocationID = Element.ElementText("LocationID");
            string LocationName = Element.ElementText("LocationName");
            bool Disable = Bool.Parse(Element.ElementText("Disable"));
            string DisableMessage = Element.ElementText("DisableMessage");

            this.TimeTableID = DSNS + "," + TimeTableID;
            this.WeekDay = WeekDay;
            this.PeriodNo = PeriodNo;
            this.Duration = Duration;
            this.Begintime = BeginTime;
            this.DispPeriod = DisplayPeriod;
            this.LocationID = LocationName;
            this.Disable = Disable;
            this.DisableMessage = DisableMessage;
        }

        /// <summary>
        /// 從資料列載入
        /// </summary>
        /// <param name="Row"></param>
        /// <param name="DSNS"></param>
        public void Load(DataRow Row, string DSNS)
        {
            string TimeTableID = Row.Field<string>("ref_timetable_id");
            string TimeTableName = Row.Field<string>("timetablename");
            int WeekDay = Convert.ToInt32(Row.Field<string>("weekday"));
            int PeriodNo = Convert.ToInt32(Row.Field<string>("period"));
            DateTime BeginTime = Convert.ToDateTime(Row.Field<string>("begin_time"));
            int Duration = Convert.ToInt32(Row.Field<string>("duration"));
            int DisplayPeriod = Convert.ToInt32(Row.Field<string>("display_period"));
            string LocationID = Row.Field<string>("ref_location_id");
            string LocationName = Row.Field<string>("locationname");
         
            this.TimeTableID = DSNS + "," + TimeTableID;
            this.WeekDay = WeekDay;
            this.PeriodNo = PeriodNo;
            this.Duration = Duration;
            this.Begintime = BeginTime;
            this.DispPeriod = DisplayPeriod;
            this.LocationID = LocationName;
        }

        /// <summary>
        /// 轉換成XElement
        /// </summary>
        /// <returns></returns>
        public XElement ToXML()
        {
            XElement Element = new XElement("TimeTableSec");
            Element.SetAttributeValue("TimeTableID", TimeTableID);
            Element.SetAttributeValue("WeekDay", WeekDay);
            Element.SetAttributeValue("PeriodNo",PeriodNo);
            Element.SetAttributeValue("Duration",Duration);
            Element.SetAttributeValue("Begintime" ,Begintime);
            Element.SetAttributeValue("DispPeriod", DispPeriod);
            Element.SetAttributeValue("LocationID",LocationID);
            Element.SetAttributeValue("Disable",Disable);
            Element.SetAttributeValue("DisableMessage",DisableMessage);

            return Element;
        }
        #region static method
        ///// <summary>
        ///// 從單一資料來源取得時間表分段資料
        ///// </summary>
        ///// <param name="Connection">連線物件</param>
        ///// <param name="SQL">SQL指令</param>
        ///// <returns></returns>
        //private static List<STimeTableSec> Select(Connection Connection, string SQL)
        //{
        //    #region 取得時間表分段原始資料
        //    QueryHelper Helper = new QueryHelper(Connection);
        //    DataTable vDataTable = Helper.Select(SQL);
        //    List<STimeTableSec> TimeTableSecs = new List<STimeTableSec>();
        //    #endregion    

        //    #region 將原始資料轉換成時間表分段物件
        //    foreach (DataRow Row in vDataTable.Rows)
        //    {
        //        STimeTableSec TimeTableSec = new STimeTableSec();

        //        TimeTableSec.Load(Row, Connection.AccessPoint.Name);

        //        TimeTableSecs.Add(TimeTableSec);
        //    }
        //    #endregion

        //    return TimeTableSecs;
        //}

        /// <summary>
        /// 從單一資料來源取得時間表分段資料
        /// </summary>
        /// <param name="Connection">連線物件</param>
        /// <param name="SQL">SQL指令</param>
        /// <returns></returns>
        private static List<STimeTableSec> Select(Connection Connection)
        {
            #region 取得時間表分段原始資料

            XElement Element = ContractService.GetTimeTableSec(Connection);

            List<STimeTableSec> TimeTableSecs = new List<STimeTableSec>();
            #endregion

            #region 將原始資料轉換成時間表分段物件
            foreach(XElement SubElement in Element.Elements("TimetableSection"))
            {
                STimeTableSec TimeTableSec = new STimeTableSec();

                TimeTableSec.Load(SubElement, Connection.AccessPoint.Name);

                TimeTableSecs.Add(TimeTableSec);
            }
            #endregion

            return TimeTableSecs;
        }


        /// <summary>
        /// 取得時間表分段資料
        /// </summary>
        /// <param name="Connections"></param>
        /// <returns></returns>
        public static SIntegrationResult<STimeTableSec> Select(List<Connection> Connections)
        {
            SIntegrationResult<STimeTableSec> Result = new SIntegrationResult<STimeTableSec>();

            #region 取得不同資料來源的時間表，使用非同步執行
            //Parallel.ForEach(Connections.ToConcurrentQueue(), x =>
            Connections.ForEach(x =>
            {
                try
                {
                    List<STimeTableSec> TimeTableSecs = Select(x);
                    Result.Data.AddRange(TimeTableSecs);
                }
                catch (Exception e)
                {
                    Result.AddMessage("下載時間表分段時發生錯誤，連線來源『" + x.AccessPoint.Name + "』");
                    Result.AddMessage(e.Message);
                    Result.IsSuccess = false;
                }
            }
            );
            #endregion

            if (Result.IsSuccess)
                Result.AddMessage("已成功下載" + Result.Data.Count + "筆時間表分段資料!");

            return Result;

            //return Select(Connections, NativeQuery.TimeTableSecSQL);
        }

        ///// <summary>
        ///// 根據測試SQL取得時間表分段資料
        ///// </summary>
        ///// <param name="Connections"></param>
        ///// <returns></returns>
        //public static SIntegrationResult<STimeTableSec> SelectTest(List<Connection> Connections)
        //{
        //    return Select(Connections, NativeQuery.TimeTableSecSQL);
        //}

        ///// <summary>
        ///// 從多個資料來源取得時間表分段資料
        ///// </summary>
        ///// <param name="Connections"></param>
        ///// <returns></returns>
        //public static SIntegrationResult<STimeTableSec> Select(List<Connection> Connections, string SQL)
        //{
        //    SIntegrationResult<STimeTableSec> Result = new SIntegrationResult<STimeTableSec>();

        //    #region 取得不同資料來源的時間表，使用非同步執行
        //    //Parallel.ForEach(Connections.ToConcurrentQueue(), x =>
        //    Connections.ForEach(x=>
        //    {
        //        try
        //        {
        //            List<STimeTableSec> TimeTableSecs = Select(x, SQL);
        //            Result.Data.AddRange(TimeTableSecs);
        //        }
        //        catch (Exception e)
        //        {
        //            Result.AddMessage("下載時間表分段時發生錯誤，連線來源『" + x.AccessPoint.Name + "』");
        //            Result.AddMessage(e.Message);
        //            Result.IsSuccess = false;
        //        }
        //    }
        //    );
        //    #endregion

        //    if (Result.IsSuccess)
        //        Result.AddMessage("已成功下載" + Result.Data.Count + "筆時間表分段資料!");

        //    return Result;

        //    #region VB
        //    //CopyTable cnSQL, cnJET, _
        //    //    "SELECT TimeTableSec.* FROM TimeTableSec INNER JOIN TimeTable ON TimeTableSec.TimeTableID=TimeTable.TimeTableID WHERE TimeTable.Active=1", _
        //    //    "TimeTableSec"
        //    #endregion
        //}
        #endregion

    }
}