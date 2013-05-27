using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FISCA.DSAClient;
using Sunset.NewCourse;

namespace Sunset.Data.Integration
{
    /// <summary>
    /// 排課資料來源
    /// </summary>
    public class SchedulerSource
    {
        /// <summary>
        /// 唯一的資料來源
        /// </summary>
        private static SchedulerSource mSource = null;

        /// <summary>
        /// 連線來源是否開啟
        /// </summary>
        public bool IsSuccess { get; private set; }

        /// <summary>
        /// 連線來源
        /// </summary>
        public List<string> DSNSNames
        {
            get
            {
                List<string> Names = new List<string>();

                CourseSectionResult.Data.ForEach(x =>
                    {
                        string[] IDs = x.ID.Split(new char[] { ',' });

                        if (IDs.Count() > 1 && !Names.Contains(IDs[0]))
                            Names.Add(IDs[0]);
                    }
                );

                return Names;
            }
        }

        /// <summary>
        /// 學年度
        /// </summary>
        public string SchoolYear { get; private set; }

        /// <summary>
        /// 學期
        /// </summary>
        public string Semester { get; private set; }

        #region 排課資料
        /// <summary>
        /// 班級資料
        /// </summary>
        public SIntegrationResult<SClass> ClassResult { get; private set; }
        /// <summary>
        /// 教師資料
        /// </summary>
        public SIntegrationResult<STeacher> TeacherResult { get; private set; }
        /// <summary>
        /// 完整教師資料
        /// </summary>
        public SIntegrationResult<STeacher> FullTeacherResult { get; private set; }
        /// <summary>
        /// 課程分段資料
        /// </summary>
        public SIntegrationResult<SCourseSection> CourseSectionResult { get; private set; }
        /// <summary>
        /// 場地資料
        /// </summary>
        public SIntegrationResult<SClassroom> ClassroomResult { get; private set; }
        /// <summary>
        /// 地點資料
        /// </summary>
        public SIntegrationResult<SLocation> LocationResult { get; private set; }
        /// <summary>
        /// 時間表資料
        /// </summary>
        public SIntegrationResult<STimeTable> TimeTableResult { get; private set; }
        /// <summary>
        /// 時間表分段資料
        /// </summary>
        public SIntegrationResult<STimeTableSec> TimeTableSecsResult { get; private set; }
        /// <summary>
        /// 教師不排課時段資料
        /// </summary>
        public SIntegrationResult<STeacherBusy> TeacherBusysResult { get; private set; }
        /// <summary>
        /// 班級不排課時段資料
        /// </summary>
        public SIntegrationResult<SClassBusy> ClassBusysResult { get; private set; }
        /// <summary>
        /// 場地不排課時段資料
        /// </summary>
        public SIntegrationResult<SClassroomBusy> ClassroomBusysResult { get; private set; }
        /// <summary>
        /// 科目資料
        /// </summary>
        public List<string> Subjects { get; private set; }
        #endregion

        /// <summary>
        /// 建構式
        /// </summary>
        private SchedulerSource()
        {
            IsSuccess = false;
        }

        /// <summary>
        /// 來源資料
        /// </summary>
        public static SchedulerSource Source
        {
            get
            {
                if (mSource == null)
                    mSource = new SchedulerSource();

                return mSource;
            }
        }
       
        /// <summary>
        /// 根據排課系統班級編號，尋找對應的DSNS來源ID
        /// </summary>
        /// <param name="DSNS">學校主機名稱</param>
        /// <param name="InternalClassID">排課系統班級編號</param>
        /// <returns>來源班級系統編號</returns>
        public string FindSourceClassID(string InternalClassID,Dictionary<string,string> ClassNameIDs)
        {
            if (ClassNameIDs.ContainsKey(InternalClassID))
                return ClassNameIDs[InternalClassID];
            else 
                return string.Empty;
        }

        /// <summary>
        /// 根據排課系統場地編號，尋找對應的DSNS來源ID
        /// </summary>
        /// <param name="DSNS">學校主機名稱</param>
        /// <param name="InternalClassroomID">排課系統場地編號</param>
        /// <returns>來源場地系統編號</returns>
        public string FindSourceClassroomID(string DSNS,string InternalClassroomID)
        {
            //根據Internal ID找到對應的場地
            SClassroom Classroom = ClassroomResult.Data
                .Find(x => x.ID.Equals(InternalClassroomID));

            //根據DSNS找到External ID
            SourceID SourceID = Classroom.SourceIDs
                .Find(x => x.DSNS.Equals(DSNS));

            return SourceID != null ? SourceID.ID : string.Empty;
        }

        /// <summary>
        /// 根據排課系統教師編號，尋找對應的DSNS來源ID
        /// </summary>
        /// <param name="DSNS">學校主機名稱</param>
        /// <param name="InternalTeacherID">排課系統教師編號</param>
        /// <returns>來源教師系統編號</returns>
        public string FindSourceTeacherID(string DSNS, string InternalTeacherID)
        {
            //根據Internal ID找到對應的教師
            STeacher Teacher = TeacherResult.Data.
                Find(x => x.ID.Equals(InternalTeacherID));

            //根據DSNS找到External ID
            SourceID SourceID = Teacher.SourceIDs.
                Find(x => x.DSNS.Equals(DSNS));

            return (SourceID != null)?SourceID.ID:string.Empty;
        }

        /// <summary>
        /// 根據教師名稱及DSNS取得教師系統編號
        /// </summary>
        /// <param name="TeacherName"></param>
        /// <param name="DSNS"></param>
        /// <returns></returns>
        private int? GetTeacherID(string TeacherName,string DSNS)
        {
            //若教師名稱或DSNS為空白則回傳空白系統編號
            if (string.IsNullOrWhiteSpace(TeacherName)
                || string.IsNullOrWhiteSpace(DSNS))
                return null;

            if (FullTeacherResult == null)
                return null;

            //根據名稱取得教師記錄
            STeacher Teacher = FullTeacherResult.Data
                .Find(x => x.Name.Equals(TeacherName));

            //若教師記錄為null則回傳空白
            if (Teacher == null)
                return null;

            //根據DSNS尋找SourceID
            SourceID Source = Teacher.SourceIDs
                .Find(x => x.DSNS.Equals(DSNS));

            //若Source為null則回傳空白
            if (Source == null)
                return null;

            int result;

            int.TryParse(Source.ID,out result);

            return result;
        }

        /// <summary>
        /// 上傳課程分段
        /// </summary>
        /// <returns></returns>
        public Tuple<bool,string> Upload(List<Connection> Connections,
            List<SCourseSection> CourseSections,
            List<STeacherBusy> TeacherBusys,
            List<SClassBusy> ClassBusys,
            List<SClassroomBusy> ClassroomBusys,
            Action<int,string> Progress)
        {
            Progress.Invoke(0,"建立上傳資料結構中");

            //鍵為DSNS，值為該學校的資料
            Dictionary<string, UploadData> UploadData = new Dictionary<string, UploadData>();
            //排課獨立課程對應班級系統編號
            Dictionary<string, string> CourseClassIDs = new Dictionary<string,string>();

            Dictionary<string, string> ClassNameIDs = new Dictionary<string,string>();

            bool IsUploadable = true;
            StringBuilder strBuilder = new StringBuilder();

            #region 依照DSNS建立上傳資料物件
            Connections.ForEach(x =>
                {
                    if (!UploadData.ContainsKey(x.AccessPoint.Name))
                    {
                        UploadData.Add(x.AccessPoint.Name, new UploadData());
                        UploadData[x.AccessPoint.Name].Connection = x;
                    }

                    if (!ClassNameIDs.ContainsKey(x.AccessPoint.Name))
                    {
                        Dictionary<string,string> vClassNameIDs = ContractService.GetClassNameIDs(x);

                        foreach (string ClassName in vClassNameIDs.Keys)
                        {
                            if (!ClassNameIDs.ContainsKey(ClassName))
                                ClassNameIDs.Add(ClassName, vClassNameIDs[ClassName]);
                        }
                    }
                }
            );
            #endregion
            Progress.Invoke(100, "已建立好上傳資料結構!");

            Progress.Invoke(0,"將分課依學校放到上傳資料結構中");

            int ProgressValue = 0;

            #region 針對不排課時段進行轉換
            foreach (STeacherBusy TeacherBusy in TeacherBusys)
                if (UploadData.ContainsKey(TeacherBusy.DSNS))
                    UploadData[TeacherBusy.DSNS].TeacherBusys.Add(TeacherBusy);

            foreach (SClassBusy ClassBusy in ClassBusys)
                if (UploadData.ContainsKey(ClassBusy.DSNS))
                    UploadData[ClassBusy.DSNS].ClassBusys.Add(ClassBusy);

            foreach (SClassroomBusy ClassroomBusy in ClassroomBusys)
                if (UploadData.ContainsKey(ClassroomBusy.DSNS))
                    UploadData[ClassroomBusy.DSNS].ClassroomBusys.Add(ClassroomBusy);
            #endregion

            #region 針對每筆課程分段，轉換成實際的上傳物件
            CourseSections.ForEach(x =>
                {
                    SchedulerCourseSection CourseSection = new SchedulerCourseSection();                    

                    #region 解析課程名稱為DSNS及來源課程系統編號
                    string[] CourseID = x.CourseID.Split(new char[]{','});
                    string DSNS = string.Empty;
                    string SourceCourseID = string.Empty;

                    if (CourseID.Length == 2)
                    {
                        DSNS = CourseID[0];
                        SourceCourseID = CourseID[1];
                    }
                    else
                    {
                        strBuilder.AppendLine("課程系統編號『"+x.CourseID+"不合法。");
                        IsUploadable = false;
                    }
                    #endregion
                    string ClassroomID = x.ClassroomID; //先設定為內部場地編號
                    string TeacherID = x.TeacherName1;  //先設定為內部教師編號
                    string ClassID = x.ClassID;         //先設定為內部場地編號

                    #region 將排課系統內部場地編號轉成外部場地系統編號，場地系統編號可能空白。
                    if (!string.IsNullOrEmpty(ClassroomID))
                    {
                        ClassroomID = FindSourceClassroomID(DSNS, ClassroomID);

                        if (string.IsNullOrWhiteSpace(ClassroomID))
                        {
                            strBuilder.AppendLine("場地『" + x.ClassroomID + "』不存在於來源學校『" + DSNS + "』中。");
                            IsUploadable = false;
                        }
                    }
                    #endregion

                    #region 將排課系統內部教師編號轉換成外部教師系統編號，教師系統編號可能空白
                    //if (!string.IsNullOrEmpty(TeacherID))
                    //{
                    //    TeacherID = FindSourceTeacherID(DSNS, TeacherID);

                    //    if (string.IsNullOrWhiteSpace(TeacherID))
                    //    {
                    //        strBuilder.AppendLine("教師『"+x.TeacherID+"』不存在於來源學校『"+DSNS+"』中。");
                    //        IsUploadable = false;
                    //    }
                    //}
                    #endregion

                    #region 將排課系統內部班級編號轉換成外部班級系統編號，班級系統編號可能空白
                    if (!string.IsNullOrEmpty(ClassID))
                    {
                        string vClassID = FindSourceClassID(ClassID, ClassNameIDs);

                        if (!string.IsNullOrEmpty(vClassID))
                        {
                            if (!CourseClassIDs.ContainsKey(x.CourseID))
                            {
                                CourseClassIDs.Add(x.CourseID,vClassID);
                            }
                        }
                    }
                    #endregion

                    #region 設定課程分段外部編號
                    try
                    {
                        CourseSection.CourseID = int.Parse(SourceCourseID); //設定來源學校系統編號

                        int? intClassroomID = null;

                        if (!string.IsNullOrEmpty(ClassroomID))
                            intClassroomID = int.Parse(ClassroomID);

                        CourseSection.ClassroomID = intClassroomID; //設定來源學校場地系統編號
                    }
                    catch (Exception e)
                    {
                        strBuilder.AppendLine("轉換課程系統編號或場地系統編號錯誤，訊息：『"+e.Message+"』");
                        IsUploadable = false; 
                    }
                    #endregion                  

                    #region 設定課程分段屬性
                    CourseSection.Length = x.Length;
                    CourseSection.Lock = x.Lock;
                    CourseSection.LongBreak = x.Longbreak;
                    CourseSection.WeekDay = x.WeekDay;
                    CourseSection.Period = x.PeriodNo;
                    CourseSection.WeekDayCond = x.WeekdayCond;
                    CourseSection.PeriodCond = x.PeriodCond;
                    CourseSection.WeekFlag = x.WeekFlag;
                    CourseSection.Comment = x.Comment;
                    CourseSection.TeacherName1 = x.TeacherName1;
                    CourseSection.TeacherID1 = GetTeacherID(x.TeacherName1,DSNS);
                    CourseSection.TeacherName2 = x.TeacherName2;
                    CourseSection.TeacherID2 = GetTeacherID(x.TeacherName2, DSNS);
                    CourseSection.TeacherName3 = x.TeacherName3;
                    CourseSection.TeacherID3 = GetTeacherID(x.TeacherName3, DSNS);                 
                    #endregion

                    if (UploadData.ContainsKey(DSNS))
                    {
                        UploadData[DSNS].CourseSections.Add(CourseSection);

                    //    #region 將課程加入到上傳清單中
                    //    if (!UploadData[DSNS].Courses.ContainsKey(SourceCourseID))
                    //    {
                    //        UploadCourse Course = new UploadCourse();
                    //        Course.CourseID = SourceCourseID;
                    //        Course.AllowDup = x.AllowDup;
                    //        Course.TeacherID = TeacherID;
                    //        UploadData[DSNS].Courses.Add(SourceCourseID, Course);
                    //    }
                    //    #endregion
                    }

                    ProgressValue++;

                    Progress.Invoke((int)((float)ProgressValue/(float)CourseSections.Count*100), "將分課依學校放到上傳資料結構中");
                }
            );
            #endregion
            Progress.Invoke(100, "已將分課依學校放到上傳資料結構!");

            if (IsUploadable)
            {
                foreach (string DSNS in UploadData.Keys)
                {
                    Progress.Invoke(0, "上傳『"+DSNS+"』資料中（準備連線中）");

                    //DSConnection Connection = new DSConnection(UploadData[DSNS].Connection);
                    //AccessHelper UDTHelper = new AccessHelper(Connection);
                    //QueryHelper QueryHelper = new QueryHelper(UploadData[DSNS].Connection);

                    #region 刪除課程分段，依課程系統編號
                    if (UploadData[DSNS].CourseSections.Count > 0)
                    {
                        try
                        {
                            //Progress.Invoke(40, "上傳『" + DSNS + "』資料中（查詢上傳學校課程分課）");

                            //找出現有課程分段
                            //string strCondition = "ref_course_id in (" + string.Join(",", UploadData[DSNS].Courses.Keys.ToArray()) + ")";
                            //List<CourseSection> DeleteCourseSections = UDTHelper.Select<CourseSection>(strCondition);

                            Progress.Invoke(80, "上傳『" + DSNS + "』資料中（刪除上傳學校課程分課）");
                            //實際刪除課程分段
                            //if (DeleteCourseSections.Count > 0)
                            //    UDTHelper.DeletedValues(DeleteCourseSections);


                            List<string> CourseIDs = UploadData[DSNS].CourseSections
                                .Select(x=>""+x.CourseID)
                                .Distinct()
                                .ToList();

                            ContractService.DeleteCourseSections(UploadData[DSNS].Connection,CourseIDs);
                        }
                        catch (Exception e)
                        {
                            #region 若刪除失敗則不繼續進行，直接回傳結果
                            strBuilder.AppendLine("刪除學校『"+DSNS+"』的課程分段時發生錯誤，詳細錯誤訊息『"+e.Message+"』");
                            IsUploadable = false;
                            return new Tuple<bool, string>(IsUploadable, strBuilder.ToString());
                            #endregion
                        }
                    }
                    Progress.Invoke(100, "上傳『" + DSNS + "』資料中（已刪除上傳學校課程分課）");
                    #endregion

                    #region 新增課程分段
                    if (UploadData[DSNS].CourseSections.Count > 0)
                    {
                        try
                        {
                            Progress.Invoke(0, "上傳『" + DSNS + "』資料中（上傳課程分段）");

                            #region 分批將課程分段上傳，以300個為單位，分3個執行緒跑
                            if (UploadData[DSNS].CourseSections.Count > 0)
                            {
                                //FunctionSpliter<CourseSection, string> Spliter = new FunctionSpliter<CourseSection, string>(500, 1);
                                //Spliter.Function = x => UDTHelper.InsertValues(x);
                                //Spliter.ProgressChange = x => Progress.Invoke(x, "上傳『" + DSNS + "』資料中（上傳課程分段）");
                                //NewIDs = Spliter.Execute(UploadData[DSNS].CourseSections);   

                                Dictionary<int, List<SchedulerCourseSection>> Courses = new Dictionary<int, List<SchedulerCourseSection>>();

                                #region 將課程分段依課程進行分類
                                foreach (SchedulerCourseSection Section in UploadData[DSNS].CourseSections)
                                {
                                    if (!Courses.ContainsKey(Section.CourseID))
                                        Courses.Add(Section.CourseID, new List<SchedulerCourseSection>());

                                    Courses[Section.CourseID].Add(Section);
                                }
                                #endregion

                                XElement RequestElement = XElement.Load(new StringReader("<Request/>"));

                                foreach (int CourseID in Courses.Keys)
                                {
                                    List<SchedulerCourseSection> Sections = Courses[CourseID];

                                    bool IsTeacherName1Equal = true;
                                    bool IsTeacherName2Equal = true;
                                    bool IsTeacherName3Equal = true;
                                    bool IsClassroomEqual = true;

                                    string TeacherName1 = Sections[0].TeacherName1;
                                    string TeacherName2 = Sections[0].TeacherName2;
                                    string TeacherName3 = Sections[0].TeacherName3;
                                    int? ClassroomID = Sections[0].ClassroomID;                                 

                                    for (int i = 1; i < Sections.Count; i++)
                                    {
                                        if (!Sections[i].TeacherName1.Equals(TeacherName1))
                                            IsTeacherName1Equal = false;

                                        if (!Sections[i].TeacherName2.Equals(TeacherName2))
                                            IsTeacherName2Equal = false;

                                        if (!Sections[i].TeacherName3.Equals(TeacherName3))
                                            IsTeacherName3Equal = false;

                                        if (!Sections[i].ClassroomID.Equals(ClassroomID))
                                            IsClassroomEqual = false;
                                    }

                                    XElement CourseExtensionElement = new XElement("CourseExtension");
                                    XElement CourseElement = new XElement("Condition");
                                    CourseElement.Add(new XElement("UID", "" + CourseID));

                                    XElement FieldElement = new XElement("Field");

                                    if (IsClassroomEqual)
                                       FieldElement.Add(new XElement("RefClassroomID",Int.GetString(ClassroomID)));

                                    if (IsTeacherName1Equal)
                                        FieldElement.Add(new XElement("TeacherName1", TeacherName1));

                                    if (IsTeacherName2Equal)
                                        FieldElement.Add(new XElement("TeacherName2", TeacherName2));

                                    if (IsTeacherName3Equal)
                                        FieldElement.Add(new XElement("TeacherName3", TeacherName3));

                                    string FullCourseID = DSNS +"," +CourseID;

                                    if (CourseClassIDs.ContainsKey(FullCourseID))
                                        FieldElement.Add(new XElement("RefClassID", CourseClassIDs[FullCourseID]));

                                    CourseExtensionElement.Add(FieldElement);
                                    CourseExtensionElement.Add(CourseElement);

                                    //<Request>
                                    //    <CourseExtension>
                                    //        <Field>
                                    //            <!--以下為非必要欄位-->
                                    //            <RefClassroomID></RefClassroomID>
                                    //            <TeacherName1></TeacherName1>
                                    //            <TeacherName2></TeacherName2>
                                    //            <TeacherName3></TeacherName3>
                                    //        </Field>
                                    //        <Condition>
                                    //            <!--以下為必要條件-->
                                    //            <UID></UID>
                                    //        </Condition>
                                    //    </CourseExtension>
                                    //</Request>

                                    if (FieldElement.Elements().Count()>0)
                                        RequestElement.Add(CourseExtensionElement);
                                }

                                ContractService.UpdateCourseExtension(UploadData[DSNS].Connection, RequestElement);

                                ContractService.InsertCourseSections(UploadData[DSNS].Connection,UploadData[DSNS].CourseSections);
                            }
                            #endregion

                            strBuilder.AppendLine("已上傳至『"+DSNS +"』共" + UploadData[DSNS].CourseSections.Count + "筆課程分段");

                            Progress.Invoke(100, "上傳『" + DSNS + "』資料中（已上傳課程分段）");
                        }
                        catch (Exception e)
                        {
                            #region 若新增課程分段失敗則不繼續進行，直接回傳結果
                            strBuilder.AppendLine("新增學校『" + DSNS + "』的課程分段時發生錯誤，詳細錯誤訊息『" + e.Message + "』");
                            IsUploadable = false;
                            return new Tuple<bool, string>(IsUploadable, strBuilder.ToString());
                            #endregion
                        }
                    }
                    #endregion

                    #region 更新課程屬性
                    //if (UploadData[DSNS].Courses.Count > 0)
                    //{
                    //    Progress.Invoke(0, "上傳『" + DSNS + "』資料中（更新課程屬性）");

                    //    //找出現有課程分段
                    //    string strCourseIDs = string.Join(",", UploadData[DSNS].Courses.Keys.ToArray());
                    //    string strCourseCondition = "ref_course_id in (" + strCourseIDs + ")";
                    //    string strTCInstructCondition = "select ref_teacher_id,ref_course_id from tc_instruct where sequence=1 and ref_course_id in ("+strCourseIDs+")"; 

                    //    //List<CourseExtension> CourseExtensions = UDTHelper.Select<CourseExtension>(strCourseCondition);
                    //    Dictionary<string, string> CourseTeacher = new Dictionary<string, string>();
                    //    Dictionary<string, string> UpdateCourseTeacher = new Dictionary<string, string>();

                    //    //DataTable Table = QueryHelper.Select(strTCInstructCondition);

                    //    //foreach (DataRow Row in Table.Rows)
                    //    //{
                    //    //    string CourseID = Row.Field<string>("ref_course_id");
                    //    //    string TeacherID = Row.Field<string>("ref_teacher_id");

                    //    //    if (!CourseTeacher.ContainsKey(CourseID))
                    //    //        CourseTeacher.Add(CourseID, TeacherID);
                    //    //}

                    //    //List<CourseExtension> UpdateCourseExtensions = new List<CourseExtension>();
                    //    Dictionary<string, bool> UpdateCourseExtensions = new Dictionary<string, bool>();

                    //    ProgressValue = 0;

                    //    foreach (UploadCourse UploadCourse in UploadData[DSNS].Courses.Values)
                    //    {
                    //        if (!UpdateCourseExtensions.ContainsKey(UploadCourse.CourseID))
                    //            UpdateCourseExtensions.Add(UploadCourse.CourseID, UploadCourse.AllowDup);

                    //       //CourseExtension CourseExtension = CourseExtensions
                    //        //    .Find(x=>(""+x.CourseID).Equals(UploadCourse.CourseID ));

                    //        //if (CourseExtension != null && CourseExtension.AllowDup!= UploadCourse.AllowDup)
                    //        //{
                    //        //    CourseExtension.AllowDup = UploadCourse.AllowDup;
                    //        //    UpdateCourseExtensions.Add(CourseExtension);
                    //        //}

                    //        //if (CourseTeacher.ContainsKey(UploadCourse.CourseID) && CourseTeacher[UploadCourse.CourseID] != UploadCourse.TeacherID)
                    //        UpdateCourseTeacher.Add(UploadCourse.CourseID, UploadCourse.TeacherID);

                    //        Progress.Invoke((int)((float)ProgressValue / (float)CourseSections.Count * 100), "上傳『" + DSNS + "』資料中（更新排課課程屬性）");

                    //        ProgressValue++;
                    //    }

                    //    ContractService.UpdateCourseAllowDup(UploadData[DSNS].Connection, UpdateCourseExtensions);

                    //    //if (UpdateCourseExtensions.Count > 0)
                    //    //    UDTHelper.UpdateValues(UpdateCourseExtensions);

                    //    if (UpdateCourseTeacher.Count > 0)
                    //    {
                    //        try
                    //        {
                    //            Connection Conn = UploadData[DSNS].Connection;

                    //            Envelope Request = new Envelope();

                    //            XElement Element = XElement.Load(new StringReader("<Request/>"));

                    //            #region 更新多筆主要授課教師
                    //            //<Request>
                    //            //<TCInstruct>
                    //            //<CourseID>11101</CourseID>
                    //            //<TeacherID>8653</TeacherID>
                    //            //</TCInstruct>
                    //            //<TCInstruct>
                    //            //<CourseID>10197</CourseID>
                    //            //<TeacherID>8678</TeacherID>
                    //            //</TCInstruct>
                    //            //</Request>
                    //            #endregion

                    //            foreach (string CourseID in UpdateCourseTeacher.Keys)
                    //            {
                    //                #region 要更新課程授課教師，課程系統編號，及教師系統編號皆不得為空白
                    //                if (!string.IsNullOrEmpty(CourseID) && 
                    //                    !string.IsNullOrEmpty(UpdateCourseTeacher[CourseID]))
                    //                {
                    //                    XElement SubElement = XElement.Load(new StringReader("<TCInstruct><CourseID>" + CourseID + "</CourseID><TeacherID>" + UpdateCourseTeacher[CourseID] + "</TeacherID></TCInstruct>"));

                    //                    Element.Add(SubElement);
                    //                }
                    //                #endregion
                    //            }

                    //            Request.Body = new XmlStringHolder(Element.ToString());

                    //            //Envelope Response = Conn.CallService("_.UpdatePrimaryTeacher", Request);
                    //        }
                    //        catch (Exception ve)
                    //        {
                    //            strBuilder.AppendLine("更新學校『" + DSNS + "』的課程授課教師時發生錯誤，詳細錯誤訊息『" + ve.Message + "』");
                    //            IsUploadable = false;
                    //            return new Tuple<bool, string>(IsUploadable, strBuilder.ToString());
                    //        }
                    //    }

                    //    Progress.Invoke(100, "上傳『" + DSNS + "』資料中（已更新課程屬性）");
                    //}
                    #endregion

                    #region 刪除不排課時段
                    //刪除及新增教師不排課時段
                    if (UploadData[DSNS].TeacherBusys.Count >0)
                    {
                        try
                        {
                            //ContractService.DeleteTeacherBusy(UploadData[DSNS].Connection);
                        }
                        catch (Exception e)
                        {
                            #region 若刪除失敗則不繼續進行，直接回傳結果
                            strBuilder.AppendLine("刪除學校『" + DSNS + "』的教師不排課時段時發生錯誤，詳細錯誤訊息『" + e.Message + "』");
                            IsUploadable = false;
                            return new Tuple<bool, string>(IsUploadable, strBuilder.ToString());
                            #endregion
                        }

                        try
                        {
                            //ContractService.InsertTeacherBusy(UploadData[DSNS].Connection,UploadData[DSNS].TeacherBusys);
                        }
                        catch (Exception e)
                        {
                            #region 若刪除失敗則不繼續進行，直接回傳結果
                            strBuilder.AppendLine("新增學校『" + DSNS + "』的教師不排課時段時發生錯誤，詳細錯誤訊息『" + e.Message + "』");
                            IsUploadable = false;
                            return new Tuple<bool, string>(IsUploadable, strBuilder.ToString());
                            #endregion 
                        }                       
                    }

                    //刪除班級不排課時段
                    if (UploadData[DSNS].ClassBusys.Count > 0)
                    {
                        try
                        {
                            //ContractService.DeleteClassBusy(UploadData[DSNS].Connection);
                        }
                        catch (Exception e)
                        {
                            #region 若刪除失敗則不繼續進行，直接回傳結果
                            strBuilder.AppendLine("刪除學校『" + DSNS + "』的班級不排課時段時發生錯誤，詳細錯誤訊息『" + e.Message + "』");
                            IsUploadable = false;
                            return new Tuple<bool, string>(IsUploadable, strBuilder.ToString());
                            #endregion
                        }

                        try
                        {
                            //ContractService.InsertClassBusy(UploadData[DSNS].Connection,UploadData[DSNS].ClassBusys);
                        }
                        catch (Exception e)
                        {
                            #region 若刪除失敗則不繼續進行，直接回傳結果
                            strBuilder.AppendLine("新增學校『" + DSNS + "』的教師不排課時段時發生錯誤，詳細錯誤訊息『" + e.Message + "』");
                            IsUploadable = false;
                            return new Tuple<bool, string>(IsUploadable, strBuilder.ToString());
                            #endregion
                        }
                    }

                    //刪除場地不排課時段
                    if (UploadData[DSNS].ClassroomBusys.Count > 0)
                    {
                        try
                        {
                            //ContractService.DeleteClassroomBusy(UploadData[DSNS].Connection);
                        }
                        catch (Exception e)
                        {
                            #region 若刪除失敗則不繼續進行，直接回傳結果
                            strBuilder.AppendLine("刪除學校『" + DSNS + "』的場地不排課時段時發生錯誤，詳細錯誤訊息『" + e.Message + "』");
                            IsUploadable = false;
                            return new Tuple<bool, string>(IsUploadable, strBuilder.ToString());
                            #endregion
                        }

                        try
                        {
                            //ContractService.InsertClassroomBusy(UploadData[DSNS].Connection,UploadData[DSNS].ClassroomBusys);
                        }
                        catch (Exception e)
                        {
                            #region 若刪除失敗則不繼續進行，直接回傳結果
                            strBuilder.AppendLine("新增學校『" + DSNS + "』的場地不排課時段時發生錯誤，詳細錯誤訊息『" + e.Message + "』");
                            IsUploadable = false;
                            return new Tuple<bool, string>(IsUploadable, strBuilder.ToString());
                            #endregion
                        }
                    }

                    Progress.Invoke(100, "上傳『" + DSNS + "』不排課資料中（已刪除上傳學校不排課時段）");
                    #endregion

                    Progress.Invoke(100, "已上傳完『" + DSNS + "』所有排課資料");
                }
            }

            return new Tuple<bool,string>(IsUploadable,strBuilder.ToString());
        }

        /// <summary>
        /// 下載學期排課資料
        /// </summary>
        /// <param name="Connections"></param>
        /// <param name="SchoolYear"></param>
        /// <param name="Semester"></param>
        /// <param name="Progress"></param>
        /// <returns></returns>
        public bool Download(List<Connection> Connections, string SchoolYear, string Semester,Action<int> Progress)
        {
            this.IsSuccess = true;
            this.SchoolYear = SchoolYear;
            this.Semester = Semester;

            try
            {
                if (Progress!=null)
                    Progress(0);

                ClassResult = SClass.Select(Connections, SchoolYear, Semester);
                IsSuccess &= ClassResult.IsSuccess;

                ClassBusysResult = SClassBusy.Select(Connections);
                IsSuccess &= ClassBusysResult.IsSuccess;

                if (Progress != null)
                    Progress(10);

                TeacherResult = STeacher.SelectByCourseSection(Connections,SchoolYear,Semester);
                IsSuccess &= TeacherResult.IsSuccess;

                FullTeacherResult = STeacher.Select(Connections);

                IsSuccess &= FullTeacherResult.IsSuccess;

                if (Progress != null)
                    Progress(20);

                TeacherBusysResult = STeacherBusy.Select(Connections);
                IsSuccess &= TeacherBusysResult.IsSuccess;

                if (Progress != null)
                    Progress(30);

                ClassroomResult = SClassroom.Select(Connections);
                IsSuccess &= ClassroomResult.IsSuccess;

                if (Progress != null)
                    Progress(40);

                ClassroomBusysResult = SClassroomBusy.Select(Connections);
                IsSuccess &= ClassroomBusysResult.IsSuccess;

                if (Progress != null)
                    Progress(50);

                LocationResult = SLocation.Select(Connections);
                IsSuccess &= LocationResult.IsSuccess;

                if (Progress != null)
                    Progress(60);

                TimeTableResult = STimeTable.Select(Connections);
                IsSuccess &= TimeTableResult.IsSuccess;

                if (Progress != null)
                    Progress(70);

                TimeTableSecsResult = STimeTableSec.Select(Connections);
                IsSuccess &= TimeTableSecsResult.IsSuccess;

                if (Progress != null)
                    Progress(80);

                CourseSectionResult = SCourseSection.Select(Connections, SchoolYear, Semester);
                IsSuccess &= CourseSectionResult.IsSuccess;

                if (Progress != null)
                    Progress(90);
            }
            catch (Exception e)
            {
                IsSuccess = false;
                throw e;
            }

            if (IsSuccess) Subjects = CourseSectionResult.Data.Select(x => x.Subject).Distinct().ToList();

            Progress(100);

            return IsSuccess;
        }

        private void SetElement(XElement Element, Action<int> Progress)
        {
            try
            {
                ClassResult = new SIntegrationResult<SClass>();
                ClassResult.Data = new List<SClass>();

                XElement elmClasses = Element.Element("Classes");

                if (elmClasses != null)
                {
                    foreach (XElement SubElement in elmClasses.Elements("Class"))
                    {
                        SClass Class = new SClass();
                        Class.Load(SubElement);
                        ClassResult.Data.Add(Class);
                    }
                }

                ClassBusysResult = new SIntegrationResult<SClassBusy>();
                ClassBusysResult.Data = new List<SClassBusy>();

                XElement elmClassBuys = Element.Element("ClassBusys");

                if (elmClassBuys != null)
                {
                    foreach (XElement SubElement in elmClassBuys.Elements("ClassBusy"))
                    {
                        SClassBusy ClassBusy = new SClassBusy();
                        ClassBusy.Load(SubElement);
                        ClassBusysResult.Data.Add(ClassBusy);
                    }
                }

                Progress.Invoke(10);

                TeacherResult = new SIntegrationResult<STeacher>();
                TeacherResult.Data = new List<STeacher>();

                XElement elmTeachers = Element.Element("Teachers");

                if (elmTeachers != null)
                {
                    foreach (XElement SubElement in elmTeachers.Elements("Teacher"))
                    {
                        STeacher Teacher = new STeacher();
                        Teacher.Load(SubElement);
                        TeacherResult.Data.Add(Teacher);
                    }
                }

                FullTeacherResult = new SIntegrationResult<STeacher>();
                FullTeacherResult.Data = new List<STeacher>();

                XElement elmFullTeacher = Element.Element("FullTeachers");

                if (elmFullTeacher != null)
                {
                    foreach (XElement SubElement in elmFullTeacher.Elements("Teacher"))
                    {
                        STeacher Teacher = new STeacher();
                        Teacher.Load(SubElement);
                        FullTeacherResult.Data.Add(Teacher);
                    }
                }

                Progress.Invoke(20);

                TeacherBusysResult = new SIntegrationResult<STeacherBusy>();
                TeacherBusysResult.Data = new List<STeacherBusy>();

                XElement elmTeacherBusy = Element.Element("TeacherBusys");

                if (elmTeacherBusy != null)
                {
                    foreach (XElement SubElement in  elmTeacherBusy.Elements("TeacherBusy"))
                    {
                        STeacherBusy TeacherBusy = new STeacherBusy();
                        TeacherBusy.Load(SubElement);
                        TeacherBusysResult.Data.Add(TeacherBusy);
                    }
                }

                Progress.Invoke(30);

                CourseSectionResult = new SIntegrationResult<SCourseSection>();
                CourseSectionResult.Data = new List<SCourseSection>();

                XElement elmCourseSections = Element.Element("CourseSections");

                if (elmCourseSections != null)
                {
                    foreach (XElement SubElement in elmCourseSections.Elements("CourseSection"))
                    {
                        SCourseSection CourseSection = new SCourseSection();
                        CourseSection.Load(SubElement);
                        CourseSectionResult.Data.Add(CourseSection);
                    }
                }

                Progress.Invoke(40);

                ClassroomResult = new SIntegrationResult<SClassroom>();
                ClassroomResult.Data = new List<SClassroom>();

                XElement elmClassrooms = Element.Element("Classrooms");

                if (elmClassrooms!=null)
                {
                    foreach (XElement SubElement in elmClassrooms.Elements("Classroom"))
                    {
                        SClassroom Classroom = new SClassroom();

                        Classroom.Load(SubElement);

                        ClassroomResult.Data.Add(Classroom);
                    }
                }

                Progress.Invoke(50);

                ClassroomBusysResult = new SIntegrationResult<SClassroomBusy>();
                ClassroomBusysResult.Data = new List<SClassroomBusy>();

                XElement elmClassroomBusys = Element.Element("ClassroomBusys");

                if (elmClassroomBusys != null)
                {
                    foreach (XElement SubElement in elmClassroomBusys.Elements("ClassroomBusy"))
                    {
                        SClassroomBusy ClassroomBusy = new SClassroomBusy();
                        ClassroomBusy.Load(SubElement);
                        ClassroomBusysResult.Data.Add(ClassroomBusy);
                    }
                }

                Progress.Invoke(60);

                LocationResult = new SIntegrationResult<SLocation>();
                LocationResult.Data = new List<SLocation>();

                XElement elmLocations = Element.Element("Locations");

                if (elmLocations != null)
                {
                    foreach (XElement SubElement in elmLocations.Elements("Location"))
                    {
                        SLocation Location = new SLocation();

                        Location.Load(SubElement);

                        LocationResult.Data.Add(Location);
                    }
                }

                Progress.Invoke(70);

                TimeTableResult = new SIntegrationResult<STimeTable>();
                TimeTableResult.Data = new List<STimeTable>();

                XElement elmTimeTables = Element.Element("TimeTables");

                if (elmTimeTables != null)
                {
                    foreach (XElement SubElement in elmTimeTables.Elements("TimeTable"))
                    {
                        STimeTable TimeTable = new STimeTable();

                        TimeTable.Load(SubElement);

                        TimeTableResult.Data.Add(TimeTable);
                    }
                }

                Progress.Invoke(80);

                TimeTableSecsResult = new SIntegrationResult<STimeTableSec>();
                TimeTableSecsResult.Data = new List<STimeTableSec>();

                XElement elmTimeTableSecs = Element.Element("TimeTableSecs");

                if (elmTimeTableSecs != null)
                {
                    foreach (XElement SubElement in elmTimeTableSecs.Elements("TimeTableSec"))
                    {
                        STimeTableSec TimeTableSec = new STimeTableSec();

                        TimeTableSec.Load(SubElement);

                        TimeTableSecsResult.Data.Add(TimeTableSec);
                    }
                }

                Progress.Invoke(90);

                Subjects = CourseSectionResult.Data.Select(x => x.Subject).Distinct().ToList();

                Progress.Invoke(100);

                IsSuccess = true;
            }
            catch (Exception e)
            {
                IsSuccess = false;

                throw e;
            }
        }

        /// <summary>
        /// 開啟用Base64編碼的排課離線檔案
        /// </summary>
        /// <param name="Filepath"></param>
        /// <param name="Progress"></param>
        public void OpenByBase64(string Filepath,string Password, Action<int> Progress)
        {
            try
            {
                TextReader reader = new StreamReader(Filepath);

                string Content = reader.ReadToEnd();

                reader.Close();

                string DecodeContent = Encoding.UTF8.GetString(Convert.FromBase64String(Content));

                StringReader strReader = new StringReader(DecodeContent);

                XElement Element = XElement.Load(strReader);

                string HashPassword = PasswordHash.Compute(Password);                
                string SavePassword = Element.AttributeText("Password");

                if (!SavePassword.Equals(HashPassword))
                    throw new Exception("開啟密碼錯誤！");

                SetElement(Element, Progress);
            }
            catch (Exception e)
            {
                IsSuccess = true;
                throw e;
            }
        }
        
        /// <summary>
        /// 開啟排課資料檔
        /// </summary>
        public void Open(string Filepath,Action<int> Progress)
        {
            Progress.Invoke(0);

            try
            {
                XElement Element = XElement.Load(System.IO.File.OpenRead(Filepath));
                SetElement(Element, Progress);
            }
            catch (Exception e)
            {
                IsSuccess = true;
                throw e;
            }
        }

        /// <summary>
        /// 取得排課結果
        /// </summary>
        /// <param name="CourseSections"></param>
        /// <param name="Filepath"></param>
        /// <param name="Progress"></param>
        /// <returns></returns>
        private XElement GetElement(List<SCourseSection> CourseSections, string Filepath, Action<int> Progress)
        {
            Progress.Invoke(0);

            XElement Element = new XElement("Scheduler");

            #region Class Related Resource
            XElement ClassElement = new XElement("Classes");

            ClassResult.Data.ForEach(x => ClassElement.Add(x.ToXML()));

            Element.Add(ClassElement);

            Progress.Invoke(10);
            #endregion

            #region ClassBusy Related Resource
            XElement ClassBusyElement = new XElement("ClassBusys");

            ClassBusysResult.Data.ForEach(x => ClassBusyElement.Add(x.ToXML()));

            Element.Add(ClassBusyElement);

            Progress.Invoke(10);
            #endregion

            #region Teacher Related Resource
            XElement TeacherElement = new XElement("Teachers");

            TeacherResult.Data.ForEach(x => TeacherElement.Add(x.ToXML()));

            Element.Add(TeacherElement);

            XElement FullTeacherElement = new XElement("FullTeachers");

            if (FullTeacherResult != null)
                FullTeacherResult.Data.ForEach(x => FullTeacherElement.Add(x.ToXML()));

            Element.Add(FullTeacherElement);

            Progress.Invoke(20);

            XElement TeacherBusyElement = new XElement("TeacherBusys");

            TeacherBusysResult.Data.ForEach(x => TeacherBusyElement.Add(x.ToXML()));

            Element.Add(TeacherBusyElement);

            Progress.Invoke(30);
            #endregion

            #region CourseSection Related Resource
            XElement CourseSectionElement = new XElement("CourseSections");

            CourseSectionResult.Data = CourseSections;

            CourseSectionResult.Data.ForEach(x => CourseSectionElement.Add(x.ToXML()));

            Element.Add(CourseSectionElement);

            Progress.Invoke(40);
            #endregion

            #region Classroom Related Resource
            XElement ClassroomElement = new XElement("Classrooms");

            ClassroomResult.Data.ForEach(x => ClassroomElement.Add(x.ToXML()));

            Element.Add(ClassroomElement);

            Progress.Invoke(50);

            XElement ClassroomBusyElement = new XElement("ClassroomBusys");

            ClassroomBusysResult.Data.ForEach(x => ClassroomBusyElement.Add(x.ToXML()));

            Element.Add(ClassroomBusyElement);

            Progress.Invoke(60);

            XElement LocationElement = new XElement("Locations");

            LocationResult.Data.ForEach(x => LocationElement.Add(x.ToXML()));

            Element.Add(LocationElement);

            Progress.Invoke(70);
            #endregion

            #region TimeTable Related Resource
            XElement TimeTableElement = new XElement("TimeTables");

            TimeTableResult.Data.ForEach(x => TimeTableElement.Add(x.ToXML()));

            Element.Add(TimeTableElement);

            Progress.Invoke(80);

            XElement TimeTableSecElement = new XElement("TimeTableSecs");

            TimeTableSecsResult.Data.ForEach(x => TimeTableSecElement.Add(x.ToXML()));

            Element.Add(TimeTableSecElement);

            Progress.Invoke(90);
            #endregion 

            return Element;
        }

        /// <summary>
        /// 用Base64儲存排課離線檔案
        /// </summary>
        /// <param name="CourseSections"></param>
        /// <param name="Filepath"></param>
        /// <param name="Progress"></param>
        public void SaveByBase64(List<SCourseSection> CourseSections, 
            string Filepath, 
            string Password,
            Action<int> Progress)
        {
            XElement Element = GetElement(CourseSections, Filepath, Progress);

            string HashPassword = PasswordHash.Compute(Password);

            Element.SetAttributeValue("Password", HashPassword);

            string strElement = Element.ToString();

            string Output = Convert.ToBase64String(Encoding.UTF8.GetBytes(strElement));

            File.WriteAllText(Filepath, Output,Encoding.UTF8);

            Progress.Invoke(100);
        }

        /// <summary>
        /// 儲存排課資料檔
        /// </summary>
        public void Save(List<SCourseSection> CourseSections, string Filepath, Action<int> Progress)
        {
            XElement Element = GetElement(CourseSections, Filepath, Progress);

            Element.Save(Filepath);

            Progress.Invoke(100);
        }

        /// <summary>
        /// 關閉
        /// </summary>
        public void Close()
        {
            this.ClassResult = null;

            this.ClassroomBusysResult = null;

            this.ClassroomResult = null;

            this.CourseSectionResult = null;

            this.LocationResult = null;

            this.TeacherBusysResult = null;

            this.TeacherResult = null;

            this.TimeTableResult = null;

            this.TimeTableSecsResult = null;

            this.Subjects = null;

            ContractService.CloseConnection();

            IsSuccess = false;
        }
    }

    /// <summary>
    /// 上傳到各校資料結構
    /// </summary>
    internal class UploadData
    {
        /// <summary>
        /// 建構式
        /// </summary>
        public UploadData()
        {
            CourseSections = new List<SchedulerCourseSection>();
            TeacherBusys = new List<STeacherBusy>();
            ClassBusys = new List<SClassBusy>();
            ClassroomBusys = new List<SClassroomBusy>();
            Courses = new Dictionary<string, UploadCourse>();
        }

        /// <summary>
        /// 學校連線資訊
        /// </summary>
        public Connection Connection { get; set; }

        /// <summary>
        /// 課程分段
        /// </summary>
        public List<SchedulerCourseSection> CourseSections { get; private set; }

        /// <summary>
        /// 教師不排課時段
        /// </summary>
        public List<STeacherBusy> TeacherBusys { get; private set; }

        /// <summary>
        /// 班級不排課時段
        /// </summary>
        public List<SClassBusy> ClassBusys { get; private set; }

        /// <summary>
        /// 場地不排課時段
        /// </summary>
        public List<SClassroomBusy> ClassroomBusys { get; private set; }

        /// <summary>
        /// 課程資料
        /// </summary>
        public Dictionary<string,UploadCourse> Courses { get; private set; }
    }

    /// <summary>
    /// 要上傳的課程資料
    /// </summary>
    internal class UploadCourse
    {
        /// <summary>
        /// 課程系統編號
        /// </summary>
        public string CourseID { get; set; }

        /// <summary>
        /// 允許重覆
        /// </summary>
        public bool AllowDup { get; set; }

        /// <summary>
        /// 課程主要授課教師
        /// </summary>
        public string TeacherID { get; set; }
    }
}