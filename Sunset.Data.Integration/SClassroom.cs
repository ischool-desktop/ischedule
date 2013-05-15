using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;
//using FISCA.Data;
using FISCA.DSAClient;

namespace Sunset.Data.Integration
{
    /// <summary>
    /// 排課來源場地資料
    /// </summary>
    public class SClassroom
    {
        /// <summary>
        /// 建構式
        /// </summary>
        public SClassroom()
        {
            SourceIDs = new List<SourceID>();
        }

        #region public property
        /// <summary>
        /// 場地系統編號，為場地名稱
        /// </summary>
        public string ID { get; set;}

        /// <summary>
        /// 來源學校的場地系統編號
        /// </summary>
        public List<SourceID> SourceIDs { get; set; }

        /// <summary>
        /// 場地名稱
        /// </summary>
        public string ClassroomName { get; set;}

        /// <summary>
        /// 班級容納數
        /// </summary>
        public int Capacity { get; set;}

        /// <summary>
        /// 無場地使用限制
        /// </summary>
        public bool LocationOnly { get; set;}

        /// <summary>
        /// 地點系統編號，為地點名稱
        /// </summary>
        public string LocationID { get; set; }

        /// <summary>
        /// 輸出成字串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            List<string> strSourceIDs = new List<string>();
            SourceIDs.ForEach(x=>strSourceIDs.Add(""+x));

            return ID + "," + string.Join(",", strSourceIDs.ToArray()) + "," + ClassroomName + "," + Capacity + "," + LocationOnly + "," + LocationID;
        }

        /// <summary>
        /// 從XML載入
        /// </summary>
        /// <param name="Element"></param>
        public void Load(XElement Element)
        {
            this.ID = Element.AttributeText("ID");
            this.ClassroomName = Element.AttributeText("ClassroomName");
            this.Capacity = Convert.ToInt32(Element.AttributeText("Capacity"));
            this.LocationOnly = Bool.Parse(Element.AttributeText("LocationOnly"));
            this.LocationID = Element.AttributeText("LocationID");

            foreach (XElement SubElement in Element.Element("SourceIDs").Elements("SourceID"))
            {
                SourceID SourceID = new SourceID();
                SourceID.Load(SubElement);
                this.SourceIDs.Add(SourceID);
            }
        }

        /// <summary>
        /// 從XML載入
        /// </summary>
        /// <param name="Element"></param>
        public void Load(XElement Element,string DSNS)
        {
            string ClassroomID = Element.ElementText("Uid");
            string ClassroomName = Element.ElementText("Name");
            int Capacity = Convert.ToInt32(Element.ElementText("Capacity"));
            bool LocationOnly = Bool.Parse(Element.ElementText("LocationOnly"));
            string LocationName = Element.ElementText("LocationName");

            this.ID = ClassroomName;
            this.SourceIDs.Add(new SourceID(DSNS, ClassroomID));
            this.ClassroomName = ClassroomName;
            this.Capacity = Capacity;
            this.LocationOnly = LocationOnly;
            this.LocationID = LocationName;
        }

        /// <summary>
        /// 從資料列載入
        /// </summary>
        /// <param name="Row"></param>
        /// <param name="DSNS"></param>
        public void Load(DataRow Row, string DSNS)
        {
            string ClassroomID = Row.Field<string>("uid");
            string ClassroomName = Row.Field<string>("name");
            int Capacity = Convert.ToInt32(Row.Field<string>("capacity"));
            bool LocationOnly = Convert.ToBoolean(Row.Field<string>("location_only"));
            string LocationName = Row.Field<string>("locationname");

            this.ID = ClassroomName;
            this.SourceIDs.Add(new SourceID(DSNS, ClassroomID));
            this.ClassroomName = ClassroomName;
            this.Capacity = Capacity;
            this.LocationOnly = LocationOnly;
            this.LocationID = LocationName;
        }

        /// <summary>
        /// 轉換成XElement
        /// </summary>
        /// <returns></returns>
        public XElement ToXML()
        {
            XElement Element = new XElement("Classroom");
            Element.SetAttributeValue("ID", ID);
            Element.SetAttributeValue("ClassroomName", ClassroomName);
            Element.SetAttributeValue("Capacity", Capacity);
            Element.SetAttributeValue("LocationOnly",LocationOnly);
            Element.SetAttributeValue("LocationID", LocationID);

            XElement SubElement = new XElement("SourceIDs");
            SourceIDs.ForEach(x => SubElement.Add(x.ToXML()));
            Element.Add(SubElement);

            return Element;
        }
        #endregion

        #region static method
        ///// <summary>
        ///// 從單一資料來源取得場地資料
        ///// </summary>
        ///// <param name="Connection">連線物件</param>
        ///// <param name="SQL">SQL指令</param>
        ///// <returns></returns>
        //private static List<SClassroom> Select(Connection Connection, string SQL)
        //{
        //    #region 取得班級原始資料
        //    QueryHelper Helper = new QueryHelper(Connection);
        //    DataTable vDataTable = Helper.Select(SQL);
        //    List<SClassroom> Classrooms = new List<SClassroom>();
        //    #endregion

        //    #region 將原始資料轉換成場地物件
        //    foreach (DataRow Row in vDataTable.Rows)
        //    {
        //        SClassroom Classroom = new SClassroom();

        //        Classroom.Load(Row,Connection.AccessPoint.Name);

        //        Classrooms.Add(Classroom);
        //    }
        //    #endregion

        //    return Classrooms;
        //}

        /// <summary>
        /// 從單一資料來源取得場地資料
        /// </summary>
        /// <param name="Connection">連線物件</param>
        /// <param name="SQL">SQL指令</param>
        /// <returns></returns>
        private static List<SClassroom> Select(Connection Connection)
        {
            #region 取得班級原始資料
            XElement Element = ContractService.GetClassroom(Connection);

            List<SClassroom> Classrooms = new List<SClassroom>();
            #endregion

            #region 將原始資料轉換成場地物件
            foreach (XElement SubElement in Element.Elements("Classroom"))
            {
                SClassroom Classroom = new SClassroom();

                Classroom.Load(SubElement, Connection.AccessPoint.Name);

                Classrooms.Add(Classroom);
            }
            #endregion

            return Classrooms;
        }


        /// <summary>
        /// 根據場地名稱加以合併
        /// </summary>
        /// <param name="Classrooms"></param>
        /// <returns></returns>
        private static Tuple<List<SClassroom>,List<string>> MergeByName(List<SClassroom> Classrooms)
        {
            var DupClassroomGroups = Classrooms
                .GroupBy(x => x.ClassroomName)
                .Where(x => x.Count() > 1);

            List<string> NotEqualClassroomNames = new List<string>();

            List<SClassroom> RemoveList = new List<SClassroom>();

            foreach (var DupClassroomGroup in DupClassroomGroups)
            {
                List<SClassroom> DupClassrooms = DupClassroomGroup.ToList();

                SClassroom FirstClassroom = DupClassrooms[0];
                string strFirstClassroom = "來源學校：" + FirstClassroom.SourceIDs[0].DSNS + ",場地名稱：" + FirstClassroom.ClassroomName + ",場地容納數：" + FirstClassroom.Capacity + ",場地無使用限制：" + (FirstClassroom.LocationOnly ? "是" : "否") + ",地點名稱：" + FirstClassroom.LocationID;
                bool HasAdd = false;

                for (int i = 1; i < DupClassrooms.Count ; i++)
                {
                    if (DupClassrooms[i].Capacity.Equals(FirstClassroom.Capacity) &&
                        DupClassrooms[i].LocationOnly.Equals(FirstClassroom.LocationOnly) &&
                        DupClassrooms[i].LocationID.Equals(FirstClassroom.LocationID))
                    {
                        SourceID Source = DupClassrooms[i].SourceIDs[0];
                        FirstClassroom.SourceIDs.Add(new SourceID(Source.DSNS, Source.ID));
                        RemoveList.Add(DupClassrooms[i]);
                    }
                    else
                    {
                        if (!HasAdd)
                        {
                            NotEqualClassroomNames.Add(strFirstClassroom);
                            HasAdd = true;
                        }

                        NotEqualClassroomNames.Add("來源學校：" + DupClassrooms[i].SourceIDs[0].DSNS + ",場地名稱：" + DupClassrooms[i].ClassroomName + ",場地容納數：" + DupClassrooms[i].Capacity + ",場地無使用限制：" + (DupClassrooms[i].LocationOnly ? "是" : "否") + ",地點名稱：" + DupClassrooms[i].LocationID);
                        break;
                    }
                }
            }

            RemoveList.ForEach(x => Classrooms.Remove(x));

            return new Tuple<List<SClassroom>, List<string>>(Classrooms, NotEqualClassroomNames);
        }

        /// <summary>
        /// 取得場地資料
        /// </summary>
        /// <param name="Connections"></param>
        /// <param name="SchoolYear"></param>
        /// <param name="Semester"></param>
        /// <returns></returns>
        public static SIntegrationResult<SClassroom> Select(List<Connection> Connections)
        {
            SIntegrationResult<SClassroom> Result = new SIntegrationResult<SClassroom>();

            #region 取得不同資料來源的場地，使用非同步執行
            //Parallel.ForEach(Connections.ToConcurrentQueue(), x =>
            Connections.ForEach(x =>
            {
                try
                {
                    List<SClassroom> Classrooms = Select(x);
                    Result.Data.AddRange(Classrooms);
                }
                catch (Exception e)
                {
                    Result.AddMessage("下載場地資料時發生錯誤，連線來源『" + x.AccessPoint.Name + "』");
                    Result.AddMessage(e.Message);
                    Result.IsSuccess = false;
                }
            }
            );
            #endregion

            if (Result.IsSuccess)
            {
                #region 若有超過一個以上的連線資訊，則檢查場地屬性是否相同
                if (Connections.Count > 1)
                {
                    try
                    {
                        Tuple<List<SClassroom>, List<string>> MergeResult = MergeByName(Result.Data);
                        if (MergeResult.Item2.Count > 0)
                        {
                            Result.AddMessage("以下場地在多個學校存在，但是其屬性值不相同，請檢查!");
                            MergeResult.Item2.ForEach(x => Result.AddMessage(x));
                            Result.IsSuccess = false;
                        }
                        else
                        {
                            Result.Data = MergeResult.Item1;
                            Result.AddMessage("已成功下載" + Result.Data.Count + "筆場地資料!若場地名稱及相關屬性都相同，已合併為相同的排課資源!");
                        }
                    }
                    catch (Exception e)
                    {
                        Result.AddMessage("合併場地名稱並檢查屬性一致時發生錯誤");
                        Result.AddMessage(e.Message);
                        Result.IsSuccess = false;
                    }
                }
                else
                    Result.AddMessage("已成功下載" + Result.Data.Count + "筆場地資料!");
                #endregion
            }

            return Result;

            //return Select(Connections, NativeQuery.ClassroomSQL);
        }

        ///// <summary>
        ///// 根據測試SQL取得場地資料
        ///// </summary>
        ///// <param name="Connections"></param>
        ///// <returns></returns>
        //public static SIntegrationResult<SClassroom> SelectTest(List<Connection> Connections)
        //{
        //    return Select(Connections, NativeQuery.ClassroomSQL);
        //}

        ///// <summary>
        ///// 從多個資料來源取得場地資料
        ///// </summary>
        ///// <param name="Connections"></param>
        ///// <param name="SchoolYear"></param>
        ///// <param name="Semester"></param>
        ///// <returns></returns>
        //public static SIntegrationResult<SClassroom> Select(List<Connection> Connections,string SQL)
        //{
        //    SIntegrationResult<SClassroom> Result = new SIntegrationResult<SClassroom>();

        //    #region 取得不同資料來源的場地，使用非同步執行
        //    //Parallel.ForEach(Connections.ToConcurrentQueue(), x =>
        //    Connections.ForEach(x=>
        //    {
        //        try
        //        {
        //            List<SClassroom> Classrooms = Select(x, SQL);
        //            Result.Data.AddRange(Classrooms);
        //        }
        //        catch (Exception e)
        //        {
        //            Result.AddMessage("下載場地資料時發生錯誤，連線來源『" + x.AccessPoint.Name + "』");
        //            Result.AddMessage(e.Message);
        //            Result.IsSuccess = false;
        //        }
        //    }
        //    );
        //    #endregion

        //    if (Result.IsSuccess)
        //    {
        //        #region 若有超過一個以上的連線資訊，則檢查場地屬性是否相同
        //        if (Connections.Count > 1)
        //        {
        //            try
        //            {
        //                Tuple<List<SClassroom>, List<string>> MergeResult = MergeByName(Result.Data);
        //                if (MergeResult.Item2.Count > 0)
        //                {
        //                    Result.AddMessage("以下場地在多個學校存在，但是其屬性值不相同，請檢查!");
        //                    MergeResult.Item2.ForEach(x => Result.AddMessage(x));
        //                    Result.IsSuccess = false;
        //                }
        //                else
        //                {
        //                    Result.Data = MergeResult.Item1;
        //                    Result.AddMessage("已成功下載" + Result.Data.Count + "筆場地資料!若場地名稱及相關屬性都相同，已合併為相同的排課資源!");
        //                }
        //            }
        //            catch (Exception e)
        //            {
        //                Result.AddMessage("合併場地名稱並檢查屬性一致時發生錯誤");
        //                Result.AddMessage(e.Message);
        //                Result.IsSuccess = false;
        //            }
        //        }else 
        //            Result.AddMessage("已成功下載" + Result.Data.Count + "筆場地資料!");
        //        #endregion
        //    }

        //    return Result;

        //    #region VB
        //    //RaiseEvent CreateDBProgress(80)
        //    //CopyTable cnSQL, cnJET, _
        //    //    "SELECT * FROM Classroom WHERE Active=1", _
        //    //    "Classroom"
        //    #endregion
        //}

        #endregion
    }
}