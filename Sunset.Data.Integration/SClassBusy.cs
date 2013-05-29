using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using FISCA.DSAClient;

namespace Sunset.Data.Integration
{
    /// <summary>
    /// 排課來源班級不排課時段
    /// </summary>
    public class SClassBusy
    {
        /// <summary>
        /// 班級系統編號，為班級名稱
        /// </summary>
        public string ClassID { get; set; }

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
        /// 來源DSNS名稱
        /// </summary>
        public string DSNS { get; set; }

        /// <summary>
        /// 輸出成串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            List<string> Values = new List<string>() { ClassID, "" + WeekDay, BeginTime.Hour + ":" + BeginTime.Minute, "" + Duration };

            return string.Join(",", Values.ToArray());
        }

        /// <summary>
        /// 從XML載入
        /// </summary>
        /// <param name="Element"></param>
        public void Load(XElement Element)
        {
            this.ClassID = Element.AttributeText("ClassID");
            this.WeekDay = Convert.ToInt32(Element.AttributeText("WeekDay"));
            this.BeginTime = Convert.ToDateTime(Element.AttributeText("BeginTime"));
            this.Duration = Convert.ToInt32(Element.AttributeText("Duration"));
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
            string ClassID = Element.ElementText("ClassID");
            string ClassName = Element.ElementText("ClassName");

            int WeekDay = Convert.ToInt32(Element.ElementText("Weekday"));
            DateTime BeginTime = Convert.ToDateTime(Element.ElementText("BeginTime"));
            int Duration = Convert.ToInt32(Element.ElementText("Duration"));
            string Description = Element.ElementText("BusyDescription");

            this.ClassID = DSNS + ","+ ClassID;
            this.WeekDay = WeekDay;
            this.BeginTime = BeginTime;
            this.Duration = Duration;
            this.DSNS = DSNS;
            this.Description = Description;
        }

        /// <summary>
        /// 轉換成XElement
        /// </summary>
        /// <returns></returns>
        public XElement ToXML()
        {
            XElement Element = new XElement("ClassBusy");
            Element.SetAttributeValue("ClassID", ClassID);
            Element.SetAttributeValue("WeekDay", WeekDay);
            Element.SetAttributeValue("BeginTime", BeginTime);
            Element.SetAttributeValue("Duration", Duration);
            Element.SetAttributeValue("DSNS", DSNS);
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
        private static List<SClassBusy> Select(Connection Connection)
        {
            #region 取得班級不排課時段原始資料
            XElement Element = ContractService.GetClassExBusy(Connection);

            List<SClassBusy> ClassBusys = new List<SClassBusy>();
            #endregion

            #region 將原始資料轉換成班級不排課時段物件
            foreach (XElement SubElement in Element.Elements("ClassBusy"))
            {
                SClassBusy ClassBusy = new SClassBusy();
                ClassBusy.Load(SubElement, Connection.AccessPoint.Name);

                ClassBusys.Add(ClassBusy);
            }
            #endregion

            return ClassBusys;
        }

        /// <summary>
        /// 根據班級名稱做群組檢查班級不排課時段是否有衝突
        /// </summary>
        /// <param name="ClassBusys"></param>
        /// <returns></returns>
        private static Tuple<List<SClassBusy>, List<string>> TimeConflictTestByName(List<SClassBusy> ClassBusys)
        {
            List<string> ClassBusyTimeConflict = new List<string>();

            List<SClassBusy> RemoveList = new List<SClassBusy>();

            //根據班級名稱做群組
            var ClassGroups = ClassBusys.GroupBy(x => x.ClassID);

            //針對每個班級群組
            foreach (var ClassGroup in ClassGroups)
            {
                //將班級不排課時段段依星期及節次排序；不能假設來源的資料是已排序好的
                var vClassBusys = from ClassBusy in ClassGroup orderby ClassBusy.WeekDay, ClassBusy.BeginTime select ClassBusy;

                List<SClassBusy> SortedClassBusys = vClassBusys.ToList();

                #region 從第一筆開始，與前一筆比較是否有時間衝突，有的話記錄下來
                for (int i = 1; i < SortedClassBusys.Count; i++)
                {
                    string PreClassName = SortedClassBusys[i - 1].ClassID;
                    int PreWeekDay = SortedClassBusys[i - 1].WeekDay;
                    DateTime PreDateTime = SortedClassBusys[i - 1].BeginTime;
                    int PreDuration = SortedClassBusys[i - 1].Duration;
                    string PreAccessPoint = SortedClassBusys[i - 1].DSNS;
                    string PreDesc = SortedClassBusys[i - 1].Description;

                    string CurClassName = SortedClassBusys[i].ClassID;
                    int CurWeekDay = SortedClassBusys[i].WeekDay;
                    DateTime CurDateTime = SortedClassBusys[i].BeginTime;
                    int CurDuration = SortedClassBusys[i].Duration;
                    string CurAccessPoint = SortedClassBusys[i].DSNS;
                    string CurDesc = SortedClassBusys[i].Description;

                    if (DateTimeHelper.IntersectsWith(
                        PreWeekDay, PreDateTime, PreDuration, 3,
                        CurWeekDay, CurDateTime, CurDuration, 3))
                    {
                        if (PreWeekDay.Equals(CurWeekDay) && 
                            PreDateTime.Equals(CurDateTime) && 
                            PreDuration.Equals(CurDuration))
                        {
                            RemoveList.Add(SortedClassBusys[i - 1]);
                        }
                        else
                        {
                            //目前當有衝突時就不予理會
                            //ClassBusyTimeConflict.Add("班級:" + PreClassName + "星期:" + PreWeekDay + ",時間:" + PreDateTime.Hour + ":" + PreDateTime.Minute + ",持續分鐘：" + PreDuration + ",來源：" + PreAccessPoint);
                            //ClassBusyTimeConflict.Add("班級:" + CurClassName + "星期:" + CurWeekDay + ",時間:" + CurDateTime.Hour + ":" + CurDateTime.Minute + ",持續分鐘：" + CurDuration + ",來源：" + CurAccessPoint);
                        }
                    }
                }
                #endregion
            }

            RemoveList.ForEach(x => ClassBusys.Remove(x));

            return new Tuple<List<SClassBusy>, List<string>>(ClassBusys, ClassBusyTimeConflict);
        }

        /// <summary>
        /// 從多個資料來源取得班級不排課時段資料
        /// </summary>
        /// <param name="Connections"></param>
        /// <returns></returns>
        public static SIntegrationResult<SClassBusy> Select(List<Connection> Connections)
        {
            SIntegrationResult<SClassBusy> Result = new SIntegrationResult<SClassBusy>();

            #region 取得不同資料來源的班級不排課時段，使用非同步執行
            Connections.ForEach(x =>
            {
                try
                {
                    List<SClassBusy> ClassBusys = Select(x);
                    Result.Data.AddRange(ClassBusys);
                }
                catch (Exception e)
                {
                    Result.AddMessage("下載班級不排課時段時發生錯誤，連線來源『" + x.AccessPoint.Name + "』");
                    Result.AddMessage(e.Message);
                    Result.IsSuccess = false;
                }
            }
            );
            #endregion

            #region 檢查班級不排課時段是否有衝突
            if (Result.IsSuccess)
            {
                try
                {
                    Tuple<List<SClassBusy>, List<string>> TimeConflictMergeResult = TimeConflictTestByName(Result.Data);

                    if (TimeConflictMergeResult.Item2.Count > 0)
                    {
                        Result.AddMessage("班級不排課時段有時間衝突，請檢查!");
                        TimeConflictMergeResult.Item2.ForEach(x => Result.AddMessage(x));
                        Result.IsSuccess = false;
                    }
                    else
                    {
                        Result.Data = TimeConflictMergeResult.Item1;
                        Result.AddMessage("已成功下載" + Result.Data.Count + "筆班級不排課時段!已依班級名稱合併為相同的排課資源!");
                    }
                }
                catch (Exception e)
                {
                    Result.AddMessage("檢查班級不排課時段是否有衝突時發生錯誤");
                    Result.AddMessage(e.Message);
                    Result.IsSuccess = false;
                }
            }
            #endregion

            return Result;
        }
        #endregion
    }
}