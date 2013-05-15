using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using FISCA.DSAClient;

namespace Sunset.Data.Integration
{
    /// <summary>
    /// 排課來源教師不排課時段
    /// </summary>
    public class STeacherBusy
    {
        /// <summary>
        /// 教師系統編號，為教師名稱
        /// </summary>
        public string TeacherID { get; set; }

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
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 地點系統編號，為地點名稱
        /// </summary>
        public string LocationID { get; set; }

        /// <summary>
        /// 來源DSNS名稱
        /// </summary>
        public string DSNS { get; set; }

        /// <summary>
        /// 輸出成串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            List<string> Values = new List<string>(){TeacherID,""+WeekDay,BeginTime.Hour+":"+BeginTime.Minute,""+Duration,LocationID};

            return string.Join(",", Values.ToArray());
        }

        /// <summary>
        /// 從XML載入
        /// </summary>
        /// <param name="Element"></param>
        public void Load(XElement Element)
        {
            this.TeacherID = Element.AttributeText("TeacherID");
            this.WeekDay = Convert.ToInt32(Element.AttributeText("WeekDay"));
            this.BeginTime = Convert.ToDateTime(Element.AttributeText("BeginTime"));
            this.Duration = Convert.ToInt32(Element.AttributeText("Duration"));
            this.LocationID = Element.AttributeText("LocationID");
            this.DSNS = Element.AttributeText("DSNS");
            this.Description = Element.AttributeText("Description");
        }

        /// <summary>
        /// 從資料列載入
        /// </summary>
        /// <param name="Element"></param>
        /// <param name="DSNS"></param>
        public void Load(XElement Element, string DSNS)
        {
            string TeacherID = Element.ElementText("TeacherID");
            string TeacherName = Element.ElementText("TeacherName");
            string TeacherNickName = Element.ElementText("TeacherNickName");
            string TeacherFullName = TeacherName + TeacherNickName;

            int WeekDay = Convert.ToInt32(Element.ElementText("Weekday"));
            DateTime BeginTime = Convert.ToDateTime(Element.ElementText("BeginTime"));
            int Duration = Convert.ToInt32(Element.ElementText("Duration"));
            string LocationID = Element.ElementText("LocationID");
            string LocationName = Element.ElementText("LocationName");
            string Description = Element.ElementText("BusyDescription");

            this.TeacherID = TeacherFullName;
            this.WeekDay = WeekDay;
            this.BeginTime = BeginTime;
            this.Duration = Duration;
            this.LocationID = LocationName;
            this.DSNS = DSNS;
            this.Description = Description;
        }

        /// <summary>
        /// 從資料列載入
        /// </summary>
        /// <param name="Row"></param>
        /// <param name="DSNS"></param>
        public void Load(DataRow Row, string DSNS)
        {
            string TeacherID = Row.Field<string>("ref_teacher_id");
            string TeacherName = Row.Field<string>("teacher_name");
            string TeacherNickName = Row.Field<string>("nickname");
            string TeacherFullName = TeacherName + TeacherNickName;
            int WeekDay = Convert.ToInt32(Row.Field<string>("weekday"));
            DateTime BeginTime = Convert.ToDateTime(Row.Field<string>("begin_time"));
            int Duration = Convert.ToInt32(Row.Field<string>("duration"));
            string LocationID = Row.Field<string>("ref_location_id");
            string LocationName = Row.Field<string>("locationname");
            string BusyDescription = Row.Field<string>("busy_description");

            this.TeacherID = TeacherFullName;
            this.WeekDay = WeekDay;
            this.BeginTime = BeginTime;
            this.Duration = Duration;
            this.LocationID = LocationName;
            this.DSNS = DSNS;
        }

        /// <summary>
        /// 轉換成XElement
        /// </summary>
        /// <returns></returns>
        public XElement ToXML()
        {
            XElement Element = new XElement("TeacherBusy");
            Element.SetAttributeValue("TeacherID",TeacherID);
            Element.SetAttributeValue("WeekDay",WeekDay);
            Element.SetAttributeValue("BeginTime",BeginTime);
            Element.SetAttributeValue("Duration",Duration);
            Element.SetAttributeValue("LocationID",LocationID);
            Element.SetAttributeValue("DSNS",DSNS);
            Element.SetAttributeValue("Description", Description);

            return Element;
        }
        #region static method

        /// <summary>
        /// 從單一資料來源取得教師不排課時段資料
        /// </summary>
        /// <param name="Connection">連線物件</param>
        /// <param name="SQL">SQL指令</param>
        /// <returns></returns>
        private static List<STeacherBusy> Select(Connection Connection)
        {
            #region 取得教師不排課時段原始資料
            XElement Element = ContractService.GetTeacherExBusy(Connection);

            List<STeacherBusy> TeacherBusys = new List<STeacherBusy>();
            #endregion

            #region 將原始資料轉換成教師不排課時段物件
            foreach (XElement SubElement in Element.Elements("TeacherBusy"))
            {
                STeacherBusy TeacherBusy = new STeacherBusy();
                TeacherBusy.Load(SubElement,Connection.AccessPoint.Name);

                TeacherBusys.Add(TeacherBusy);
            }
            #endregion

            return TeacherBusys;
        }

        /// <summary>
        /// 根據教師名稱做群組檢查教師不排課時段是否有衝突
        /// </summary>
        /// <param name="TeacherBusys"></param>
        /// <returns></returns>
        private static Tuple<List<STeacherBusy>, List<string>> TimeConflictTestByName(List<STeacherBusy> TeacherBusys)
        {
            List<string> TeacherBusyTimeConflict = new List<string>();

            List<STeacherBusy> RemoveList = new List<STeacherBusy>();

            //根據教師名稱做群組
            var TeacherGroups = TeacherBusys.GroupBy(x => x.TeacherID);

            //針對每個教師群組
            foreach (var TeacherGroup in TeacherGroups)           
            {
                //將教師不排課時段段依星期及節次排序；不能假設來源的資料是已排序好的
                var vTeacherBusys = from TeacherBusy in TeacherGroup orderby TeacherBusy.WeekDay,TeacherBusy.BeginTime select TeacherBusy;

                List<STeacherBusy> SortedTeacherBusys = vTeacherBusys.ToList();

                #region 從第一筆開始，與前一筆比較是否有時間衝突，有的話記錄下來
                for (int i=1;i<SortedTeacherBusys.Count;i++)
                {
                    string PreTeacherName = SortedTeacherBusys[i-1].TeacherID;
                    int PreWeekDay = SortedTeacherBusys[i-1].WeekDay;
                    DateTime PreDateTime = SortedTeacherBusys[i-1].BeginTime;
                    int PreDuration = SortedTeacherBusys[i-1].Duration;
                    string PreAccessPoint = SortedTeacherBusys[i-1].DSNS;
                    string PreDesc = SortedTeacherBusys[i - 1].Description;

                    string CurTeacherName = SortedTeacherBusys[i].TeacherID;
                    int CurWeekDay = SortedTeacherBusys[i].WeekDay;
                    DateTime CurDateTime = SortedTeacherBusys[i].BeginTime;
                    int CurDuration = SortedTeacherBusys[i].Duration;
                    string CurAccessPoint = SortedTeacherBusys[i].DSNS;
                    string CurDesc = SortedTeacherBusys[i].Description;

                    if (DateTimeHelper.IntersectsWith(
                        PreWeekDay,PreDateTime,PreDuration,3,
                        CurWeekDay,CurDateTime,CurDuration,3))
                    {
                        if (PreWeekDay.Equals(CurWeekDay) && PreDateTime.Equals(CurDateTime) && PreDuration.Equals(CurDuration) && PreDesc.Equals(CurDesc))
                        {
                            RemoveList.Add(SortedTeacherBusys[i - 1]);
                        }
                        else
                        {
                            TeacherBusyTimeConflict.Add("教師:" + PreTeacherName + "星期:" + PreWeekDay + ",時間:" + PreDateTime.Hour + ":" + PreDateTime.Minute + ",持續分鐘：" + PreDuration + ",來源：" + PreAccessPoint);
                            TeacherBusyTimeConflict.Add("教師:" + CurTeacherName + "星期:" + CurWeekDay + ",時間:" + CurDateTime.Hour + ":" + CurDateTime.Minute + ",持續分鐘：" + CurDuration + ",來源：" + CurAccessPoint);
                        }
                    }
                }
                #endregion
            }

            RemoveList.ForEach(x => TeacherBusys.Remove(x));

            return new Tuple<List<STeacherBusy>, List<string>>(TeacherBusys, TeacherBusyTimeConflict);
        }

        ///// <summary>
        ///// 根據測試SQL取得教師不排課時段資料
        ///// </summary>
        ///// <param name="Connections"></param>
        ///// <returns></returns>
        //public static SIntegrationResult<STeacherBusy> SelectTest(List<Connection> Connections)
        //{
        //    return Select(Connections, NativeQuery.TeacherBusySQL);
        //}

        /// <summary>
        /// 從多個資料來源取得教師不排課時段資料
        /// </summary>
        /// <param name="Connections"></param>
        /// <returns></returns>
        public static SIntegrationResult<STeacherBusy> Select(List<Connection> Connections)
        {
            SIntegrationResult<STeacherBusy> Result = new SIntegrationResult<STeacherBusy>();

            #region 取得不同資料來源的教師不排課時段，使用非同步執行
            //Parallel.ForEach(Connections.ToConcurrentQueue(), x =>
            Connections.ForEach(x =>
            {
                try
                {
                    List<STeacherBusy> TeacherBusys = Select(x);
                    Result.Data.AddRange(TeacherBusys);
                }
                catch (Exception e)
                {
                    Result.AddMessage("下載教師不排課時段時發生錯誤，連線來源『" + x.AccessPoint.Name + "』");
                    Result.AddMessage(e.Message);
                    Result.IsSuccess = false;
                }
            }
            );
            #endregion

            #region 檢查教師不排課時段是否有衝突
            if (Result.IsSuccess)
            {
                try
                {
                    Tuple<List<STeacherBusy>, List<string>> TimeConflictMergeResult = TimeConflictTestByName(Result.Data);

                    if (TimeConflictMergeResult.Item2.Count > 0)
                    {
                        Result.AddMessage("教師不排課時段有時間衝突，請檢查!");
                        TimeConflictMergeResult.Item2.ForEach(x => Result.AddMessage(x));
                        Result.IsSuccess = false;
                    }
                    else
                    {
                        Result.Data = TimeConflictMergeResult.Item1;
                        Result.AddMessage("已成功下載" + Result.Data.Count + "筆教師不排課時段!已依教師名稱合併為相同的排課資源!");
                    }
                }
                catch (Exception e)
                {
                    Result.AddMessage("檢查教師不排課時段是否有衝突時發生錯誤");
                    Result.AddMessage(e.Message);
                    Result.IsSuccess = false;
                }
            }
            #endregion

            return Result;

            #region VB
            //CopyTable cnSQL, cnJET, _
            //    "SELECT * FROM TeacherBusy", _
            //    "TeacherBusy"
            #endregion
        }

        ///// <summary>
        ///// 從多個資料來源取得教師不排課時段資料
        ///// </summary>
        ///// <param name="Connections"></param>
        ///// <returns></returns>
        //public static SIntegrationResult<STeacherBusy> Select(List<Connection> Connections, string SQL)
        //{
        //    SIntegrationResult<STeacherBusy> Result = new SIntegrationResult<STeacherBusy>();

        //    #region 取得不同資料來源的教師不排課時段，使用非同步執行
        //    //Parallel.ForEach(Connections.ToConcurrentQueue(), x =>
        //    Connections.ForEach(x=>
        //    {
        //        try
        //        {
        //            List<STeacherBusy> TeacherBusys = Select(x, SQL);
        //            Result.Data.AddRange(TeacherBusys);
        //        }
        //        catch (Exception e)
        //        {
        //            Result.AddMessage("下載教師不排課時段時發生錯誤，連線來源『" + x.AccessPoint.Name + "』");
        //            Result.AddMessage(e.Message);
        //            Result.IsSuccess = false;
        //        }
        //    }
        //    );
        //    #endregion

        //    #region 檢查教師不排課時段是否有衝突
        //    if (Result.IsSuccess)
        //    {
        //        try
        //        {
        //            Tuple<List<STeacherBusy>, List<string>> TimeConflictMergeResult = TimeConflictTestByName(Result.Data);

        //            if (TimeConflictMergeResult.Item2.Count > 0)
        //            {
        //                Result.AddMessage("教師不排課時段有時間衝突，請檢查!");
        //                TimeConflictMergeResult.Item2.ForEach(x => Result.AddMessage(x));
        //                Result.IsSuccess = false;
        //            }
        //            else
        //            {
        //                Result.Data = TimeConflictMergeResult.Item1;
        //                Result.AddMessage("已成功下載" + Result.Data.Count + "筆教師不排課時段!已依教師名稱合併為相同的排課資源!");
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            Result.AddMessage("檢查教師不排課時段是否有衝突時發生錯誤");
        //            Result.AddMessage(e.Message);
        //            Result.IsSuccess = false;
        //        }
        //    }
        //    #endregion

        //    return Result;

        //    #region VB
        //    //CopyTable cnSQL, cnJET, _
        //    //    "SELECT * FROM TeacherBusy", _
        //    //    "TeacherBusy"
        //    #endregion
        //}
        #endregion
    }
}