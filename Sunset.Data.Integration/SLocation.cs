using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Linq;
//using FISCA.Data;
using FISCA.DSAClient;

namespace Sunset.Data.Integration
{
    /// <summary>
    /// 排課來源地點資料
    /// </summary>
    public class SLocation
    {
        /// <summary>
        /// 建構式
        /// </summary>
        public SLocation()
        {
            SourceIDs = new List<SourceID>();
        }

        /// <summary>
        /// 地點系統編號，為地點名稱
        /// </summary>
        public string ID { get; set ;}

        /// <summary>
        /// 來源學校的系統編號
        /// </summary>
        public List<SourceID> SourceIDs { get; set; }

        /// <summary>
        /// 地點名稱
        /// </summary>
        public string LocationName { get; set;}

        /// <summary>
        /// 輸出成字串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            List<string> strSourceIDs = new List<string>();
            SourceIDs.ForEach(x => strSourceIDs.Add("" + x));

            return ID + "," + string.Join(",", strSourceIDs.ToArray()) + "," + LocationName;
        }

        /// <summary>
        /// 從XML載入
        /// </summary>
        /// <param name="Element"></param>
        public void Load(XElement Element)
        {
            this.ID = Element.AttributeText("ID");
            this.LocationName = Element.AttributeText("LocationName");

            foreach (XElement SubElement in Element.Element("SourceIDs").Elements("SourceID"))
            {
                SourceID SourceID = new SourceID();
                SourceID.Load(SubElement);
                this.SourceIDs.Add(SourceID);
            }
        }

        /// <summary>
        /// 從資料列載入
        /// </summary>
        /// <param name="Row"></param>
        /// <param name="DSNS"></param>
        public void Load(DataRow Row, string DSNS)
        {
            string LocationID = Row.Field<string>("uid");
            string LocationName = Row.Field<string>("name");

            this.ID = LocationName;
            this.SourceIDs.Add(new SourceID(DSNS, LocationID));
            this.LocationName = LocationName;
        }

                /// <summary>
        /// 從資料列載入
        /// </summary>
        /// <param name="Row"></param>
        /// <param name="DSNS"></param>
        public void Load(XElement Element, string DSNS)
        {
            string LocationID = Element.ElementText("Uid");
            string LocationName = Element.ElementText("Name");

            this.ID = LocationName;
            this.SourceIDs.Add(new SourceID(DSNS, LocationID));
            this.LocationName = LocationName;
        }

        /// <summary>
        /// 轉換成XElement
        /// </summary>
        /// <returns></returns>
        public XElement ToXML()
        {
            XElement Element = new XElement("Location");
            Element.SetAttributeValue("ID", ID);
            Element.SetAttributeValue("LocationName", LocationName);

            XElement SubElement = new XElement("SourceIDs");
            SourceIDs.ForEach(x => SubElement.Add(x.ToXML()));
            Element.Add(SubElement);

            return Element;
        }
        #region static method
        ///// <summary>
        ///// 從單一資料來源取得地點資料
        ///// </summary>
        ///// <param name="Connection">連線物件</param>
        ///// <param name="SQL">SQL指令</param>
        ///// <returns></returns>
        //private static List<SLocation> Select(Connection Connection, string SQL)
        //{
        //    #region 取得班級原始資料
        //    QueryHelper Helper = new QueryHelper(Connection);
        //    DataTable vDataTable = Helper.Select(SQL);
        //    List<SLocation> Locations = new List<SLocation>();
        //    #endregion

        //    #region 將原始資料轉換成地點物件
        //    foreach (DataRow Row in vDataTable.Rows)
        //    {
        //        SLocation Location = new SLocation();

        //        Location.Load(Row,Connection.AccessPoint.Name);

        //        Locations.Add(Location);
        //    }
        //    #endregion

        //    return Locations;
        //}

        /// <summary>
        /// 從單一資料來源取得地點資料
        /// </summary>
        /// <param name="Connection">連線物件</param>
        /// <returns>地點物件列表</returns>
        private static List<SLocation> Select(Connection Connection)
        {
            XElement Element = ContractService.GetLocation(Connection);

            List<SLocation> Locations = new List<SLocation>();
            #endregion

            #region 將原始資料轉換成地點物件
            foreach (XElement SubElement in Element.Elements("Location"))
            {
                SLocation Location = new SLocation();

                Location.Load(SubElement,Connection.AccessPoint.Name);

                Locations.Add(Location);
            }
            #endregion

            return Locations;
        }

        /// <summary>
        /// 根據地點名稱加以合併
        /// </summary>
        /// <param name="Locations">地點列表</param>
        /// <returns></returns>
        private static Tuple<List<SLocation>, string> MergeByName(List<SLocation> Locations)
        {
            StringBuilder strBuilder = new StringBuilder();

            var DupLocationGroups = Locations
                .GroupBy(x => x.LocationName)
                .Where(x => x.Count() > 1);

            strBuilder.AppendLine("共有" + DupLocationGroups.Count() + "個地點在2個以上的學校重覆");

            List<SLocation> RemoveList = new List<SLocation>();

            foreach (var DupLocationGroup in DupLocationGroups)
            {
                List<SLocation> DupLocations = DupLocationGroup.ToList();

                for (int i = 1; i < DupLocations.Count; i++)
                {
                    SourceID Source = DupLocations[i].SourceIDs[0];
                    DupLocations[0].SourceIDs.Add(new SourceID(Source.DSNS, Source.ID));
                    RemoveList.Add(DupLocations[i]);
                }

                string strSource = string.Join(",", DupLocations[0]
                    .SourceIDs
                    .Select(x => x.DSNS)
                    .ToArray());

                strBuilder.AppendLine("地點『" + DupLocations[0].LocationName + "』同時在2個以上的學校（" + strSource + "）設定，已將其視為視為同樣地點。");
            }

            RemoveList.ForEach(x => Locations.Remove(x));

            return new Tuple<List<SLocation>, string>(Locations, strBuilder.ToString());
        }

        ///// <summary>
        ///// 取得地點資料
        ///// </summary>
        ///// <param name="Connections"></param>
        ///// <returns></returns>
        //public static SIntegrationResult<SLocation> Select(List<Connection> Connections, int SchoolYear, int Semester)
        //{
        //    return Select(Connections, NativeQuery.LocationSQL);
        //}

        /// <summary>
        /// 根據測試SQL取得地點資料
        /// </summary>
        /// <param name="Connections"></param>
        /// <returns></returns>
        public static SIntegrationResult<SLocation> Select(List<Connection> Connections)
        {
            SIntegrationResult<SLocation> Result = new SIntegrationResult<SLocation>();

            #region 取得不同資料來源的地點，使用非同步執行
            //Parallel.ForEach(Connections.ToConcurrentQueue(), x =>
            Connections.ForEach(x =>
            {
                try
                {
                    List<SLocation> Locations = Select(x);
                    Result.Data.AddRange(Locations);
                }
                catch (Exception e)
                {
                    Result.AddMessage("下載地點資料時發生錯誤，連線來源『" + x.AccessPoint.Name + "』");
                    Result.AddMessage(e.Message);
                    Result.IsSuccess = false;
                }
            }
            );
            #endregion

            if (Result.IsSuccess)
            {
                #region 若有超過一個以上的連線資訊，則進行合併
                if (Connections.Count > 1)
                {
                    try
                    {
                        Tuple<List<SLocation>, string> MergeResult = MergeByName(Result.Data);

                        if (!string.IsNullOrEmpty(MergeResult.Item2))
                            Result.AddMessage(MergeResult.Item2);
                        Result.Data = MergeResult.Item1;
                    }
                    catch (Exception e)
                    {
                        Result.AddMessage("合併地點時發生錯誤");
                        Result.AddMessage(e.Message);
                        Result.IsSuccess = false;
                    }
                }
                #endregion

                if (Result.IsSuccess)
                    Result.AddMessage("已成功下載" + Result.Data.Count + "筆地點資料!");
            }

            return Result;

            //舊的寫法
            //return Select(Connections, NativeQuery.LocationSQL);
        }

        ///// <summary>
        ///// 從多個資料來源取得地點資料
        ///// </summary>
        ///// <param name="Connections"></param>
        ///// <returns></returns>
        //public static SIntegrationResult<SLocation> Select(List<Connection> Connections, string SQL)
        //{
        //    SIntegrationResult<SLocation> Result = new SIntegrationResult<SLocation>();

        //    #region 取得不同資料來源的地點，使用非同步執行
        //    //Parallel.ForEach(Connections.ToConcurrentQueue(), x =>
        //    Connections.ForEach(x=>
        //    {
        //        try
        //        {
        //            List<SLocation> Locations = Select(x, SQL);
        //            Result.Data.AddRange(Locations);
        //        }
        //        catch (Exception e)
        //        {
        //            Result.AddMessage("下載地點資料時發生錯誤，連線來源『" + x.AccessPoint.Name + "』");
        //            Result.AddMessage(e.Message);
        //            Result.IsSuccess = false;
        //        }
        //    }
        //    );
        //    #endregion

        //    if (Result.IsSuccess)
        //    {
        //        #region 若有超過一個以上的連線資訊，則進行合併
        //        if (Connections.Count > 1)
        //        {
        //            try
        //            {
        //                Tuple<List<SLocation>, string> MergeResult = MergeByName(Result.Data);

        //                if (!string.IsNullOrEmpty(MergeResult.Item2))
        //                    Result.AddMessage(MergeResult.Item2);
        //                Result.Data = MergeResult.Item1;
        //            }
        //            catch (Exception e)
        //            {
        //                Result.AddMessage("合併地點時發生錯誤");
        //                Result.AddMessage(e.Message);
        //                Result.IsSuccess = false;
        //            }
        //        }
        //        #endregion

        //        if (Result.IsSuccess)
        //            Result.AddMessage("已成功下載" + Result.Data.Count + "筆地點資料!");
        //    }

        //    return Result;

        //    #region VB
        //    //CopyTable cnSQL, cnJET, _
        //    //    "SELECT * FROM Location WHERE Active=1", _
        //    //    "Location"
        //    #endregion
        //}
    }
}