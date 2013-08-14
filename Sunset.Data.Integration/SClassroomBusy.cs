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
    /// 排課來源場地不排課時段
    /// </summary>
    public class SClassroomBusy
    {
        /// <summary>
        /// 場地系統編號，為場地名稱
        /// </summary>
        public string ClassroomID { get; set; }

        /// <summary>
        /// 星期
        /// </summary>
        public int WeekDay { get; set; }

        /// <summary>
        /// 開始時間
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 持續分鐘
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// 單雙週
        /// </summary>
        public Byte WeekFlag { get; set; }

        /// <summary>
        /// 不排課描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 來源DSNS
        /// </summary>
        public string DSNS { get; set; }

        /// <summary>
        /// 輸出成字串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            List<string> Values = new List<string>() { ClassroomID, "" + WeekDay, BeginTime.Hour + ":" + BeginTime.Minute, "" + Duration,""+WeekFlag,Description};

            return string.Join(",", Values.ToArray());
        }

        /// <summary>
        /// 從XML載入
        /// </summary>
        /// <param name="Element"></param>
        public void Load(XElement Element)
        {
            this.ClassroomID = Element.AttributeText("ClassroomID");
            this.WeekDay = Convert.ToInt32(Element.AttributeText("WeekDay"));
            this.BeginTime = Convert.ToDateTime(Element.AttributeText("BeginTime"));
            this.Duration = Convert.ToInt32(Element.AttributeText("Duration"));
            this.WeekFlag = Convert.ToByte(Element.AttributeText("WeekFlag"));
            this.DSNS = Element.AttributeText("DSNS");
            this.Description = Element.AttributeText("Description");
        }

        /// <summary>
        /// 從XElement載入
        /// </summary>
        /// <param name="Element"></param>
        /// <param name="DSNS"></param>
        public void Load(XElement Element,string DSNS)
        {
            string ClassroomID = Element.ElementText("ClassroomID");
            string ClassroomName = Element.ElementText("ClassroomName");           
            int WeekDay = Convert.ToInt32(Element.ElementText("Weekday"));
            DateTime BeginTime = Convert.ToDateTime(Element.ElementText("BeginTime"));
            int Duration = Convert.ToInt32(Element.ElementText("Duration"));
            Byte WeekFlag = Convert.ToByte(Element.ElementText("WeekFlag"));
            string Description = Element.ElementText("BusyDescription");

            this.ClassroomID = ClassroomName;
            this.WeekDay = WeekDay;
            this.BeginTime = BeginTime;
            this.Duration = Duration;
            this.WeekFlag = WeekFlag;
            this.Description = Description;

            this.DSNS = DSNS;
        }

        /// <summary>
        /// 從資料列載入
        /// </summary>
        /// <param name="Row"></param>
        /// <param name="DSNS"></param>
        public void Load(DataRow Row, string DSNS)
        {
            string ClassroomID = Row.Field<string>("ref_classroom_id");
            string ClassroomName = Row.Field<string>("name");
            int WeekDay = Convert.ToInt32(Row.Field<string>("weekday"));
            DateTime BeginTime = Convert.ToDateTime(Row.Field<string>("begin_time"));
            int Duration = Convert.ToInt32(Row.Field<string>("duration"));
            Byte WeekFlag = Convert.ToByte(Row.Field<string>("week_flag"));

            this.ClassroomID = ClassroomName;
            this.WeekDay = WeekDay;
            this.BeginTime = BeginTime;
            this.Duration = Duration;
            this.WeekFlag = WeekFlag;
            this.DSNS = DSNS;
        }

        /// <summary>
        /// 轉換成XElement
        /// </summary>
        /// <returns></returns>
        public XElement ToXML()
        {
            XElement Element = new XElement("ClassroomBusy");
            Element.SetAttributeValue("ClassroomID", ClassroomID);
            Element.SetAttributeValue("WeekDay",WeekDay);
            Element.SetAttributeValue("BeginTime", BeginTime);
            Element.SetAttributeValue("Duration", Duration);
            Element.SetAttributeValue("WeekFlag", WeekFlag);
            Element.SetAttributeValue("DSNS", DSNS);
            Element.SetAttributeValue("Description", Description);

            return Element;
        }
        #region static method
        ///// <summary>
        ///// 從單一資料來源取得場地不排課時段資料
        ///// </summary>
        ///// <param name="Connection">連線物件</param>
        ///// <param name="SQL">SQL指令</param>
        ///// <returns></returns>
        //private static List<SClassroomBusy> Select(Connection Connection, string SQL)
        //{
        //    #region 取得場地不排課時段原始資料
        //    QueryHelper Helper = new QueryHelper(Connection);
        //    DataTable vDataTable = Helper.Select(SQL);
        //    List<SClassroomBusy> ClassroomBusys = new List<SClassroomBusy>();
        //    #endregion

        //    #region 將原始資料轉換成場地不排課時段物件
        //    foreach (DataRow Row in vDataTable.Rows)
        //    {
        //        SClassroomBusy ClassroomBusy = new SClassroomBusy();

        //        ClassroomBusy.Load(Row,Connection.AccessPoint.Name);

        //        ClassroomBusys.Add(ClassroomBusy);
        //    }
        //    #endregion

        //    return ClassroomBusys;
        //}

        /// <summary>
        /// 從單一資料來源取得場地不排課時段資料
        /// </summary>
        /// <param name="Connection">連線物件</param>
        /// <param name="SQL">SQL指令</param>
        /// <returns></returns>
        private static List<SClassroomBusy> Select(Connection Connection)
        {
            #region 取得場地不排課時段原始資料

            XElement Element = ContractService.GetClassroomBusy(Connection);

            List<SClassroomBusy> ClassroomBusys = new List<SClassroomBusy>();
            #endregion

            #region 將原始資料轉換成場地不排課時段物件
            foreach (XElement SubElement in Element.Elements("ClassroomBusy"))
            {
                SClassroomBusy ClassroomBusy = new SClassroomBusy();

                ClassroomBusy.Load(SubElement, Connection.AccessPoint.Name);

                ClassroomBusys.Add(ClassroomBusy);
            }
            #endregion

            return ClassroomBusys;
        }


        /// <summary>
        /// 根據場地名稱做群組檢查場地不排課時段是否有衝突
        /// </summary>
        /// <param name="ClassroomBusys"></param>
        /// <returns></returns>
        private static Tuple<List<SClassroomBusy>, List<string>> TimeConflictTestByName(List<SClassroomBusy> ClassroomBusys)
        {
            List<string> ClassroomBusyTimeConflict = new List<string>();

            List<SClassroomBusy> RemoveList = new List<SClassroomBusy>();

            //根據場地名稱做群組
            var ClassroomGroups = ClassroomBusys.GroupBy(x => x.ClassroomID);

            //針對每個場地群組
            foreach (var ClassroomGroup in ClassroomGroups)
            {
                //將場地不排課時段段依星期及節次排序；不能假設來源的資料是已排序好的
                var vClassroomBusys = from ClassroomBusy in ClassroomGroup orderby ClassroomBusy.WeekDay,ClassroomBusy.BeginTime select ClassroomBusy;

                List<SClassroomBusy> SortedClassroomBusys = vClassroomBusys.ToList();

                #region 從第一筆開始，與前一筆比較是否有時間衝突，有的話記錄下來
                for (int i=1;i<SortedClassroomBusys.Count;i++)
                {
                    string PreClassroomName = SortedClassroomBusys[i-1].ClassroomID;
                    int PreWeekDay = SortedClassroomBusys[i-1].WeekDay;
                    DateTime PreDateTime = SortedClassroomBusys[i-1].BeginTime;
                    int PreDuration = SortedClassroomBusys[i-1].Duration;
                    Byte PreWeekFlag = SortedClassroomBusys[i - 1].WeekFlag;
                    string PreAccessPoint = SortedClassroomBusys[i-1].DSNS;

                    string CurClassroomName = SortedClassroomBusys[i].ClassroomID;
                    int CurWeekDay = SortedClassroomBusys[i].WeekDay;
                    DateTime CurDateTime = SortedClassroomBusys[i].BeginTime;
                    int CurDuration = SortedClassroomBusys[i].Duration;
                    Byte CurWeekFlag = SortedClassroomBusys[i].WeekFlag;
                    string CurAccessPoint = SortedClassroomBusys[i].DSNS;

                    if (DateTimeHelper.IntersectsWith(
                        PreWeekDay,PreDateTime,PreDuration,PreWeekFlag,
                        CurWeekDay,CurDateTime,CurDuration,CurWeekFlag))
                    {
                        if (PreWeekDay.Equals(CurWeekDay) && 
                            PreDateTime.Equals(CurDateTime) && 
                            PreDuration.Equals(CurDuration))
                        {
                            RemoveList.Add(SortedClassroomBusys[i - 1]);
                        }
                        else
                        {
                            ClassroomBusyTimeConflict.Add("場地:" + PreClassroomName + "星期:" + PreWeekDay + ",時間:" + PreDateTime.Hour + ":" + PreDateTime.Minute + ",持續分鐘：" + PreDuration + ",單雙週:" + PreWeekFlag + ",來源：" + PreAccessPoint);
                            ClassroomBusyTimeConflict.Add("場地:" + CurClassroomName + "星期:" + CurWeekDay + ",時間:" + CurDateTime.Hour + ":" + CurDateTime.Minute + ",持續分鐘：" + CurDuration + ",單雙週:" + CurWeekFlag + ",來源：" + CurAccessPoint);
                        }
                    }
                }
                #endregion
            }

            RemoveList.ForEach(x => ClassroomBusys.Remove(x));

            return new Tuple<List<SClassroomBusy>, List<string>>(ClassroomBusys, ClassroomBusyTimeConflict);
        }

        /// <summary>
        /// 取得場地不排課時段資料
        /// </summary>
        /// <param name="Connections"></param>
        /// <returns></returns>
        public static SIntegrationResult<SClassroomBusy> Select(List<Connection> Connections)
        {
            SIntegrationResult<SClassroomBusy> Result = new SIntegrationResult<SClassroomBusy>();

            #region 取得不同資料來源的場地不排課時段，使用非同步執行
            //Parallel.ForEach(Connections.ToConcurrentQueue(), x =>
            Connections.ForEach(x =>
            {
                try
                {
                    List<SClassroomBusy> ClassroomBusys = Select(x);
                    Result.Data.AddRange(ClassroomBusys);
                }
                catch (Exception e)
                {
                    Result.AddMessage("下載場地不排課時段時發生錯誤，連線來源『" + x.AccessPoint.Name + "』");
                    Result.AddMessage(e.Message);
                    Result.IsSuccess = false;
                }
            }
            );
            #endregion

            #region 檢查場地不排課時段是否有衝突
            if (Result.IsSuccess)
            {
                try
                {
                    Tuple<List<SClassroomBusy>, List<string>> TimeConflictMergeResult = TimeConflictTestByName(Result.Data);

                    if (TimeConflictMergeResult.Item2.Count > 0)
                    {
                        Result.AddMessage("場地不排課時段有時間衝突，請檢查!");
                        TimeConflictMergeResult.Item2.ForEach(x => Result.AddMessage(x));
                        Result.IsSuccess = false;
                    }
                    else
                    {
                        Result.Data = TimeConflictMergeResult.Item1;
                        Result.AddMessage("已成功下載" + Result.Data.Count + "筆場地不排課時段!已依場地名稱合併為相同的排課資源!");
                    }
                }
                catch (Exception e)
                {
                    Result.AddMessage("檢查場地不排課時段是否有衝突時發生錯誤");
                    Result.AddMessage(e.Message);
                    Result.IsSuccess = false;
                }
            }
            #endregion

            return Result;

            //return Select(Connections, NativeQuery.ClassroomBusySQL);
        }

        ///// <summary>
        ///// 根據測試SQL取得場地不排課時段資料
        ///// </summary>
        ///// <param name="Connections"></param>
        ///// <returns></returns>
        //public static SIntegrationResult<SClassroomBusy> SelectTest(List<Connection> Connections)
        //{
        //    return Select(Connections, NativeQuery.ClassroomBusySQL);
        //}

        ///// <summary>
        ///// 從多個資料來源取得場地不排課時段資料
        ///// </summary>
        ///// <param name="Connections"></param>
        ///// <returns></returns>
        //public static SIntegrationResult<SClassroomBusy> Select(List<Connection> Connections, string SQL)
        //{
        //    SIntegrationResult<SClassroomBusy> Result = new SIntegrationResult<SClassroomBusy>();

        //    #region 取得不同資料來源的場地不排課時段，使用非同步執行
        //    //Parallel.ForEach(Connections.ToConcurrentQueue(), x =>
        //    Connections.ForEach(x =>
        //    {
        //        try
        //        {
        //            List<SClassroomBusy> ClassroomBusys = Select(x, SQL);
        //            Result.Data.AddRange(ClassroomBusys);
        //        }
        //        catch (Exception e)
        //        {
        //            Result.AddMessage("下載場地不排課時段時發生錯誤，連線來源『" + x.AccessPoint.Name + "』");
        //            Result.AddMessage(e.Message);
        //            Result.IsSuccess = false;
        //        }
        //    }
        //    );
        //    #endregion

        //    #region 檢查場地不排課時段是否有衝突
        //    if (Result.IsSuccess)
        //    {
        //        try
        //        {
        //            Tuple<List<SClassroomBusy>, List<string>> TimeConflictMergeResult = TimeConflictTestByName(Result.Data);

        //            if (TimeConflictMergeResult.Item2.Count > 0)
        //            {
        //                Result.AddMessage("場地不排課時段有時間衝突，請檢查!");
        //                TimeConflictMergeResult.Item2.ForEach(x => Result.AddMessage(x));
        //                Result.IsSuccess = false;
        //            }
        //            else
        //            {
        //                Result.Data = TimeConflictMergeResult.Item1;
        //                Result.AddMessage("已成功下載" + Result.Data.Count + "筆場地不排課時段!已依場地名稱合併為相同的排課資源!");
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            Result.AddMessage("檢查場地不排課時段是否有衝突時發生錯誤");
        //            Result.AddMessage(e.Message);
        //            Result.IsSuccess = false;
        //        }
        //    }
        //    #endregion

        //    return Result;

        //    #region
        //    //CopyTable cnSQL, cnJET, _
        //    //    "SELECT * FROM ClassroomBusy", _
        //    //    "ClassroomBusy"
        //    #endregion
        //}
        #endregion
    }
}