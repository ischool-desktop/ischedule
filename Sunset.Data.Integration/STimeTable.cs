using System;
using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
//using FISCA.Data;
using FISCA.DSAClient;

namespace Sunset.Data.Integration
{
    /// <summary>
    /// 排課來源時間表資料
    /// </summary>
    public class STimeTable
    {
        /// <summary>
        /// 建構式
        /// </summary>
        public STimeTable()
        {

        }

        /// <summary>
        /// 系統編號，為DSNS加上來源TimeTableID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 時間表名稱
        /// </summary>
        public string TimeTableName { get; set; }

        /// <summary>
        /// 輸出成字串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ID + "," + TimeTableName;
        }

        /// <summary>
        /// 從XML載入
        /// </summary>
        /// <param name="Element"></param>
        public void Load(XElement Element)
        {
            this.ID = Element.AttributeText("ID");
            this.TimeTableName = Element.AttributeText("TimeTableName");
        }

        /// <summary>
        /// 從XElement載入
        /// </summary>
        /// <param name="Element"></param>
        /// <param name="DSNS"></param>
        public void Load(XElement Element,string DSNS)
        {
            string TimeTableID = Element.ElementText("Uid");
            string TimeTableName = Element.ElementText("Name");

            this.ID = DSNS + "," + TimeTableID;
            this.TimeTableName = TimeTableName;
        }

        /// <summary>
        /// 從資料列載入
        /// </summary>
        /// <param name="Row"></param>
        /// <param name="DSNS"></param>
        public void Load(DataRow Row, string DSNS)
        {
            string TimeTableID = Row.Field<string>("uid");
            string TimeTableName = Row.Field<string>("name");

            this.ID = DSNS + "," + TimeTableID;
            this.TimeTableName = TimeTableName;
        }

        /// <summary>
        /// 轉換成XElement
        /// </summary>
        /// <returns></returns>
        public XElement ToXML()
        {
            XElement Element = new XElement("TimeTable");
            Element.SetAttributeValue("ID", ID);
            Element.SetAttributeValue("TimeTableName", TimeTableName);

            return Element;
        }
        #region static method
        ///// <summary>
        ///// 從單一資料來源取得時間表資料
        ///// </summary>
        ///// <param name="Connection">連線物件</param>
        ///// <param name="SQL">SQL指令</param>
        ///// <returns></returns>
        //private static List<STimeTable> Select(Connection Connection, string SQL)
        //{
        //    #region 取得時間表原始資料
        //    QueryHelper Helper = new QueryHelper(Connection);
        //    DataTable vDataTable = Helper.Select(SQL);
        //    List<STimeTable> TimeTables = new List<STimeTable>();
        //    #endregion

        //    #region 將原始資料轉換成場地物件
        //    foreach (DataRow Row in vDataTable.Rows)
        //    {
        //        STimeTable TimeTable = new STimeTable();

        //        TimeTable.Load(Row,Connection.AccessPoint.Name);

        //        TimeTables.Add(TimeTable);
        //    }
        //    #endregion

        //    return TimeTables;
        //}

        /// <summary>
        /// 從單一資料來源取得時間表資料
        /// </summary>
        /// <param name="Connection">連線物件</param>
        /// <param name="SQL">SQL指令</param>
        /// <returns></returns>
        private static List<STimeTable> Select(Connection Connection)
        {
            #region 取得時間表原始資料

            XElement Element = ContractService.GetTimeTable(Connection);

            List<STimeTable> TimeTables = new List<STimeTable>();
            #endregion

            #region 將原始資料轉換成場地物件
            foreach (XElement SubElement in Element.Elements("TimeTable"))
            {
                STimeTable TimeTable = new STimeTable();

                TimeTable.Load(SubElement, Connection.AccessPoint.Name);

                TimeTables.Add(TimeTable);
            }
            #endregion

            return TimeTables;
        }


        /// <summary>
        /// 取得時間表資料
        /// </summary>
        /// <param name="Connections"></param>
        /// <returns></returns>
        public static SIntegrationResult<STimeTable> Select(List<Connection> Connections)
        {
            SIntegrationResult<STimeTable> Result = new SIntegrationResult<STimeTable>();

            #region 取得不同資料來源的時間表，使用非同步執行
            Connections.ForEach(x =>
            {
                try
                {
                    List<STimeTable> TimeTables = Select(x);
                    Result.Data.AddRange(TimeTables);
                }
                catch (Exception e)
                {
                    Result.AddMessage("下載時間表時發生錯誤，連線來源『" + x.AccessPoint.Name + "』");
                    Result.AddMessage(e.Message);
                    Result.IsSuccess = false;
                }
            }
            );
            #endregion

            if (Result.IsSuccess)
                Result.AddMessage("已成功下載" + Result.Data.Count + "筆時間表資料!");

            return Result;

            //return Select(Connections, NativeQuery.TimeTableSQL);
        }

        ///// <summary>
        ///// 根據測試SQL取得時間表資料
        ///// </summary>
        ///// <param name="Connections"></param>
        ///// <returns></returns>
        //public static SIntegrationResult<STimeTable> SelectTest(List<Connection> Connections)
        //{
        //    SIntegrationResult<STimeTable> Result = new SIntegrationResult<STimeTable>();

        //    #region 取得不同資料來源的時間表，使用非同步執行
        //    //Parallel.ForEach(Connections.ToConcurrentQueue(), x =>
        //    Connections.ForEach(x =>
        //    {
        //        try
        //        {
        //            List<STimeTable> TimeTables = Select(x, NativeQuery.TimeTableSQL);
        //            Result.Data.AddRange(TimeTables);
        //        }
        //        catch (Exception e)
        //        {
        //            Result.AddMessage("下載時間表時發生錯誤，連線來源『" + x.AccessPoint.Name + "』");
        //            Result.AddMessage(e.Message);
        //            Result.IsSuccess = false;
        //        }
        //    }
        //    );
        //    #endregion

        //    if (Result.IsSuccess)
        //        Result.AddMessage("已成功下載" + Result.Data.Count + "筆時間表資料!");

        //    return Result;

        //    //return Select(Connections, NativeQuery.TimeTableSQL);
        //}

        ///// <summary>
        ///// 從多個資料來源取得時間表資料
        ///// </summary>
        ///// <param name="Connections"></param>
        ///// <returns></returns>
        //public static SIntegrationResult<STimeTable> Select(List<Connection> Connections, string SQL)
        //{
        //    SIntegrationResult<STimeTable> Result = new SIntegrationResult<STimeTable>();

        //    #region 取得不同資料來源的時間表，使用非同步執行
        //    //Parallel.ForEach(Connections.ToConcurrentQueue(), x =>
        //    Connections.ForEach(x=>
        //    {
        //        try
        //        {
        //            List<STimeTable> TimeTables = Select(x, SQL);
        //            Result.Data.AddRange(TimeTables);
        //        }
        //        catch (Exception e)
        //        {
        //            Result.AddMessage("下載時間表時發生錯誤，連線來源『" + x.AccessPoint.Name + "』");
        //            Result.AddMessage(e.Message);
        //            Result.IsSuccess = false;
        //        }
        //    }
        //    );
        //    #endregion

        //    if (Result.IsSuccess)
        //        Result.AddMessage("已成功下載" + Result.Data.Count + "筆時間表資料!");

        //    return Result;

        //    #region VB
        //    //CopyTable cnSQL, cnJET, _
        //    //    "SELECT * FROM TimeTable WHERE Active=1", _
        //    //    "TimeTable"
        //    #endregion
        //}
        #endregion

    }
}