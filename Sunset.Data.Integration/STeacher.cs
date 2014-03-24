using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FISCA.DSAClient;

namespace Sunset.Data.Integration
{
    /// <summary>
    /// 排課來源教師資料
    /// </summary>
    public class STeacher
    {
        /// <summary>
        /// 建構式
        /// </summary>
        public STeacher()
        {
            ID = string.Empty;
            Name = string.Empty;
            Comment = string.Empty;
            SourceIDs = new List<SourceID>();
        }

        #region public property
        /// <summary>
        /// 系統編號，為教師名稱
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 教師名稱，為教師姓名加上教師暱稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 基本授課時數
        /// </summary>
        public int? BasicLength { get; set;}

        /// <summary>
        /// 兼課時數
        /// </summary>
        public int? ExtraLength { get; set;}

        /// <summary>
        /// 輔導時數
        /// </summary>
        public int? CounselingLength {get; set;}

        /// <summary>
        /// 代碼
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 專長
        /// </summary>
        public string Expertise { get; set; }

        /// <summary>
        /// 註記
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 教師來源DSNS及系統編號
        /// </summary>
        public List<SourceID> SourceIDs { get; set; }

        /// <summary>
        /// 輸出成字串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            List<string> strSourceIDs = new List<string>();

            SourceIDs.ForEach(x => strSourceIDs.Add(""+x));

            return ID + "," + Name + "," + string.Join(",", strSourceIDs.ToArray()) + "," + BasicLength + "," + ExtraLength +"," +CounselingLength +","+Comment;
        }

        /// <summary>
        /// 從XML載入
        /// </summary>
        /// <param name="Element"></param>
        public void Load(XElement Element)
        {
            this.ID = Element.AttributeText("ID");
            this.Name = Element.AttributeText("Name");
            this.BasicLength = Int.ParseAllowNull(Element.AttributeText("BasicLength"));
            this.ExtraLength = Int.ParseAllowNull(Element.AttributeText("ExtraLength"));
            this.CounselingLength = Int.ParseAllowNull(Element.AttributeText("CounselingLength"));

            this.Code = Element.AttributeText("Code");
            this.Expertise = Element.AttributeText("Expertise");
            this.Comment = Element.AttributeText("Comment");

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
        /// <param name="DSNS"></param>
        public void Load(XElement Element, string DSNS)
        {
            string ID = Element.ElementText("Id");
            string Name = Element.ElementText("TeacherName");
            string NickName = Element.ElementText("TeacherNickName");
            string FullName = !string.IsNullOrEmpty(NickName) ? Name + "(" + NickName +")" : Name;
            int? BasicLength = Int.ParseAllowNull(Element.ElementText("BasicLength"));
            int? ExtraLength = Int.ParseAllowNull(Element.ElementText("ExtraLength"));
            int? CounselingLength = Int.ParseAllowNull(Element.ElementText("CounselingLength"));

            string Code = Element.ElementText("Code");
            string Expertise = Element.ElementText("Expertise");
            string Comment = Element.ElementText("Comment");

            this.ID = FullName;
            this.Name = FullName;
            this.BasicLength = BasicLength;
            this.ExtraLength = ExtraLength;
            this.CounselingLength = CounselingLength;
            this.Code = Code;
            this.Expertise = Expertise;
            this.Comment = Comment;
            this.SourceIDs.Add(new SourceID(DSNS, ID));
        }

        /// <summary>
        /// 轉換成XElement
        /// </summary>
        /// <returns></returns>
        public XElement ToXML()
        {
            XElement Element = new XElement("Teacher");
            Element.SetAttributeValue("ID", ID);
            Element.SetAttributeValue("Name", Name);
            Element.SetAttributeValue("BasicLength", Int.GetString(BasicLength));
            Element.SetAttributeValue("ExtraLength", Int.GetString(ExtraLength));
            Element.SetAttributeValue("CounselingLength", Int.GetString(CounselingLength));
            Element.SetAttributeValue("Code", Code);
            Element.SetAttributeValue("Expertise", Expertise);
            Element.SetAttributeValue("Comment", Comment);

            XElement SubElement = new XElement("SourceIDs");
            SourceIDs.ForEach(x => SubElement.Add(x.ToXML()));
            Element.Add(SubElement);

            return Element;
        }
        #endregion

        #region static method
        /// <summary>
        /// 根據學年度學期從多個來源下載資料
        /// </summary>
        /// <param name="Connections">多個資料來源</param>
        /// <param name="SchoolYear">學年度</param>
        /// <param name="Semester">學期</param>
        /// <returns></returns>
        public static SIntegrationResult<STeacher> SelectByCourseSection(List<Connection> Connections, string SchoolYear, string Semester)
        {
            #region 取得不同資料來源的教師，使用非同步執行
            SIntegrationResult<STeacher> Result = new SIntegrationResult<STeacher>();

            Connections.ForEach(x =>
            {
                try
                {
                    List<STeacher> Teachers = SelectByCourseSection(x, SchoolYear, Semester);

                    foreach (STeacher Teacher in Teachers)
                    {
                        //根據姓名比對教師是否存在
                        STeacher FindTeacher = Result.Data.Find(y=>y.Name.Equals(Teacher.Name));

                        //若不存在則新增
                        if (FindTeacher==null)
                        {
                            Result.Data.Add(Teacher);
                        }else
                        {
                            //若已存在則將DSNS加入
                            foreach (SourceID SourceID in Teacher.SourceIDs)
                            {
                                if (FindTeacher.SourceIDs.Find(y => y.DSNS.Equals(SourceID.DSNS))==null)
                                    FindTeacher.SourceIDs.Add(SourceID);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Result.AddMessage("下載教師資料時發生錯誤，連線來源『" + x.AccessPoint.Name + "』");
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

        /// <summary>
        /// 從單一資料來源取得教師，依據課程分段
        /// </summary>
        /// <param name="Connection"></param>
        /// <returns></returns>
        public static List<STeacher> SelectByCourseSection(Connection Connection,string SchoolYear,string Semester)
        {
            //<CourseSection>
            //    <TeacherName1/>
            //    <TeacherName2/>
            //    <TeacherName3/>
            //</CourseSection>

            XElement Element = ContractService
                .GetTeacherByCourseSectionNew(Connection,SchoolYear,Semester);

            List<string> TeacehrNames = new List<string>();
            List<STeacher> Teachers = new List<STeacher>();

            #region 將原始資料轉換成教師資料
            foreach (XElement SubElement in Element.Elements("Teacher"))
            {
                string Name = SubElement.ElementText("Name");
                if (!string.IsNullOrEmpty(Name) &&
                    !TeacehrNames.Contains(Name))
                    TeacehrNames.Add(Name);
            }
            #endregion

            foreach (string TeacherName in TeacehrNames)
            {
                STeacher Teacher = new STeacher();
                Teacher.ID = TeacherName;
                Teacher.Name = TeacherName;
                Teacher.SourceIDs.Add(new SourceID(Connection.AccessPoint.Name, TeacherName));
                Teachers.Add(Teacher);
            }

            return Teachers;
        }

        /// <summary>
        /// 從單一資料來源取得教師資料
        /// </summary>
        /// <param name="Connection">連線物件</param>
        /// <returns>教師物件列表</returns>
        private static List<STeacher> Select(Connection Connection,string ServiceName)
        {
            #region 取得教師原始資料

            XElement Element = ContractService.GetTeacher(Connection,ServiceName);

            List<STeacher> Teachers = new List<STeacher>();
            #endregion

            #region 將原始資料轉換成教師資料
            foreach(XElement SubElement in Element.Elements("Teacher"))
            {
                STeacher Teacher = new STeacher();

                Teacher.Load(SubElement, Connection.AccessPoint.Name);

                Teachers.Add(Teacher);
            }
            #endregion

            return Teachers; 
        }

        /// <summary>
        /// 根據教師姓名加以合併
        /// </summary>
        /// <param name="Teachers"></param>
        /// <returns></returns>
        private static Tuple<List<STeacher>,string> MergeByName(List<STeacher> Teachers)        
        {
            StringBuilder strBuilder = new StringBuilder();

            var DupTeacherGroups = Teachers
                .GroupBy(x => x.Name)
                .Where(x => x.Count() > 1);

            strBuilder.AppendLine("共有" + DupTeacherGroups.Count() + "位教師在2個以上的學校授課");

            List<STeacher> RemoveList = new List<STeacher>();

            foreach (var DupTeacherGroup in DupTeacherGroups)
            {
                List<STeacher> DupTeachers = DupTeacherGroup.ToList();

                for(int i=1;i<DupTeachers.Count;i++)
                {
                    SourceID Source = DupTeachers[i].SourceIDs[0];
                    DupTeachers[0].SourceIDs.Add(new SourceID(Source.DSNS, Source.ID));
                    RemoveList.Add(DupTeachers[i]);
                }

                string strSource = string.Join(",",DupTeachers[0]
                    .SourceIDs
                    .Select(x => x.DSNS)
                    .ToArray());

                strBuilder.AppendLine("教師『"+DupTeachers[0].Name+"』同時在2個以上的學校（"+strSource +"）授課，已將其視為視為同樣教師。");
            }

            RemoveList.ForEach(x => Teachers.Remove(x));

            //目前教師還是依據教師姓名來合併，所以基本節數、兼課時數、輔導時數及備註要設為一樣。

            return new Tuple<List<STeacher>, string>(Teachers, strBuilder.ToString());
        }

        /// <summary>
        /// 從多個資料來源取得教師資料
        /// </summary>
        /// <param name="Connections"></param>
        /// <returns></returns>
        public static SIntegrationResult<STeacher> Select(List<Connection> Connections,string ServiceName)
        {
            #region 取得不同資料來源的課程，使用非同步執行
            SIntegrationResult<STeacher> Result = new SIntegrationResult<STeacher>();

            Connections.ForEach(x =>
            {
                try
                {
                    List<STeacher> Teachers = Select(x,ServiceName);
                    Result.Data.AddRange(Teachers);
                }
                catch (Exception e)
                {
                    Result.AddMessage("下載教師資料時發生錯誤，連線來源『" + x.AccessPoint.Name + "』");
                    Result.AddMessage(e.Message);
                    Result.IsSuccess = false;
                }
            }
            );
            #endregion

            #region 若有超過一個以上的連線資訊，則將同名同姓的教師合併
            if (Result.IsSuccess)
            {
                if (Connections.Count > 1)
                {
                    try
                    {
                        Tuple<List<STeacher>, string> MergeResult = MergeByName(Result.Data);
                        Result.Data = MergeResult.Item1;
                        Result.AddMessage(MergeResult.Item2);
                    }
                    catch (Exception e)
                    {
                        Result.AddMessage("合併教師資源時發生錯誤");
                        Result.AddMessage(e.Message);
                        Result.IsSuccess = false;
                    }
                }

                Result.AddMessage("已成功下載" + Result.Data.Count + "筆教師資料!");
            }
            #endregion

            return Result;
        }
        #endregion
    }
}