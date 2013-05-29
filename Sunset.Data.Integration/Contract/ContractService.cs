using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using FISCA.Authentication;
using FISCA.DSAClient;
using Sunset.NewCourse;

namespace Sunset.Data.Integration
{
    /// <summary>
    /// 直接存取Contract服務
    /// </summary>
    public class ContractService
    {
        private static string DefaultContractName = "ischool.sunset.schedule";
        private static Dictionary<string, Connection> mConnections = null;
        private static List<Timer> mTimers = null;

        /// <summary>
        /// 關閉所有連線
        /// </summary>
        public static void CloseConnection()
        {
            mConnections = null;
            mTimers = null;
        }

        public static void InitialConnection()
        {
            GetConnection(FISCA.Authentication.DSAServices.AccessPoint);

            GetConnection(DSAServices.GreeningAccessPoint, "user");
        }

        /// <summary>
        /// 根據帳號及密碼取得PassportToken
        /// </summary>
        /// <param name="EMail"></param>
        /// <param name="Passpord"></param>
        /// <returns></returns>
        public static Tuple<PassportToken, string> GetPassportToken(string EMail,string Passpord)
        {
            #region PassportToken
            //<SecurityToken Type="Passport">
            //    <DSAPassport Encoding="UTF-8" Version="1.0">
            //        <Content>
            //            <PassportID />
            //            <Issuer>greening.shared.user</Issuer>
            //            <IssueInstant>2013/04/08 14:52:01</IssueInstant>
            //            <ValidTo>2013/04/09 15:52:01</ValidTo>
            //            <Subject>kunhsiang.chang@ischool.com.tw</Subject>
            //            <AuthMethod>Basic</AuthMethod>
            //            <RequesterIP>140.126.57.153</RequesterIP>
            //            <Roles>
            //                <Role>USER</Role>
            //            </Roles>
            //            <Attributes>
            //                <ID>89</ID>
            //                <LoginArgs />
            //                <UserUUID>7289778b-add5-4e92-a065-a0dbda105c9c</UserUUID>
            //                <DomainID>3</DomainID>
            //                <LoginType>google</LoginType>
            //            </Attributes>
            //        </Content>
            //        <ds:Signature xmlns:ds="http://www.w3.org/2000/09/xmldsig#">V9TU8FQwQ1Fug8X6PCP0eXI/mHQcUxDJyyTr+ZbMwS66Ag7y7nmf/qiABqaoo9eJicB80RPUf8z01nm82+PHTm4gfpZt7I15s3xgQjEC7+oha/sMZGCXgV/9fTcO10krE+eOA16yIddtNSmfecrxurzIIFaYTdqGebWrCjHJnm0=</ds:Signature>
            //    </DSAPassport>
            //</SecurityToken>
            #endregion

            try
            {
                Connection vConnection = new Connection();

                //<Request>
                //   <User>kunhsiang.chang@ishcool.com.tw</User>
                //</Request>

                vConnection.Connect(FISCA.Authentication.DSAServices.GreeningAccessPoint,
                    FISCA.Authentication.DSAServices.GreeningContract, 
                    EMail , 
                    Passpord);

                XElement elmRequest = new XElement("Request");

                elmRequest.Add(new XElement("User",EMail));

                XElement elmPassport = SendRequest(vConnection,"GetPassport",elmRequest);

                XmlHelper helper = new XmlHelper(elmPassport.Element("DSAPassport").ToXmlElement());

                PassportToken p = new PassportToken(helper);

                return new Tuple<PassportToken, string>(p, string.Empty);
            }
            catch (Exception ve)
            {
                return new Tuple<PassportToken, string>(null, ve.Message);
            }
        }

        /// <summary>
        /// 用Passport連線至AccessPoint，並傳回對應的連線資訊。
        /// </summary>
        /// <param name="AccessPoint"></param>
        /// <returns></returns>
        public static Tuple<Connection, string> GetConnection(string AccessPoint)
        {
            return GetConnection(AccessPoint, DefaultContractName);
        }

        /// <summary>
        /// 用Passport連線至AccessPoint，並傳回對應的連線資訊；並且會放到連線區中，之後可重覆使用。
        /// </summary>
        /// <param name="AccessPoint"></param>
        /// <returns></returns>
        public static Tuple<Connection, string> GetConnection(string AccessPoint,string ContractName)
        {
            if (mConnections == null)
                mConnections = new Dictionary<string, Connection>();

            if (mTimers == null)
                mTimers = new List<Timer>();

            if (mConnections.ContainsKey(AccessPoint))
                return new Tuple<Connection, string>(mConnections[AccessPoint], string.Empty);

            try
            {
                #region Connection一開始會用Passport連線，之後強制使用Session連線，並定期送Request確保Session不會過期
                Connection vConnection = new Connection();
                vConnection.EnableSession = true;
                vConnection.Connect(AccessPoint, ContractName, FISCA.Authentication.DSAServices.PassportToken);

                //Timer mTimer = new Timer(x =>
                //    {
                //        //try
                //        //{
                //        vConnection.SendRequest("DS.Base.Connect", new Envelope());
                //        //}
                //        //catch (Exception e)
                //        //{
                //        //    FISCA.ErrorBox.Show(e.Message, e);
                //        //}
                //    }, null, 1000 * 60, 1000 * 60);

                //mTimers.Add(mTimer);

                mConnections.Add(AccessPoint, vConnection);
                #endregion

                return new Tuple<Connection, string>(vConnection, string.Empty);
            }
            catch (Exception ve)
            {
                return new Tuple<Connection,string>(null,"無法連線至『" + AccessPoint + "』主機" + System.Environment.NewLine + "訊息：『" + ve.Message + "』");
            }
        }

        public static Tuple<Connection, string> GetConnection(PassportToken Passport, string AccessPoint)
        {
            return GetConnection(Passport, AccessPoint, DefaultContractName);
        }

        /// <summary>
        /// 用Passport連線至AccessPoint，並傳回對應的連線資訊；並且會放到連線區中，之後可重覆使用。
        /// </summary>
        /// <param name="AccessPoint"></param>
        /// <returns></returns>
        public static Tuple<Connection, string> GetConnection(PassportToken Passport,string AccessPoint, string ContractName)
        {
            if (mConnections == null)
                mConnections = new Dictionary<string, Connection>();

            if (mTimers == null)
                mTimers = new List<Timer>();

            if (mConnections.ContainsKey(AccessPoint))
                return new Tuple<Connection, string>(mConnections[AccessPoint], string.Empty);

            try
            {
                #region Connection一開始會用Passport連線，之後強制使用Session連線，並定期送Request確保Session不會過期
                Connection vConnection = new Connection();
                vConnection.EnableSession = true;
                vConnection.Connect(AccessPoint, ContractName, Passport);
                mConnections.Add(AccessPoint, vConnection);
                #endregion

                return new Tuple<Connection, string>(vConnection, string.Empty);
            }
            catch (Exception ve)
            {
                return new Tuple<Connection, string>(null, "無法連線至『" + AccessPoint + "』主機" + System.Environment.NewLine + "訊息：『" + ve.Message + "』");
            }
        }


        /// <summary>
        /// 送出文件
        /// </summary>
        /// <param name="Connection"></param>
        /// <param name="ServiceName"></param>
        /// <param name="RequestElement"></param>
        /// <returns></returns>
        private static XElement SendRequest(Connection Conn,string ServiceName,XElement RequestElement)
        {
            try
            {
                Envelope Request = new Envelope();

                Request.Body = new XmlStringHolder(RequestElement.ToString());

                Envelope Response = Conn.SendRequest(ServiceName, Request);

                XElement Element = XElement.Load(new StringReader(Response.Body.XmlString));

                return Element;  
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 取得獨立課程的課程分段
        /// </summary>
        /// <param name="Connection">連線物件</param>
        /// <param name="SchoolYear">學年度</param>
        /// <param name="Semester">學期</param>
        /// <returns></returns>
        public static XElement GetCourseSectionNew(Connection Connection, string SchoolYear, string Semester)
        {
            #region 範例
            //<CourseSection>
            //    <Uid>539333</Uid>
            //    <CourseID>538368</CourseID>
            //    <CourseName>LIP五仁 公民與社會Ⅲ</CourseName>
            //    <SubjectAliasName/>
            //    <Weekday>0</Weekday>
            //    <Period>0</Period>
            //    <Length>1</Length>
            //    <WeekdayCondition/>
            //    <PeriodCondition/>
            //    <WeekFlag>3</WeekFlag>
            //    <LongBreak>f</LongBreak>
            //    <Lock>f</Lock>
            //    <ClassroomID/>
            //    <ClassroomName/>
            //    <Subject>公民與社會</Subject>
            //    <SubjectLevel/>
            //    <ClassID/>
            //    <AllowDuplicate>f</AllowDuplicate>
            //    <TimeTableID>85680</TimeTableID>
            //    <TimeTableName>日校實驗班</TimeTableName>
            //    <TeacherName1/>
            //    <TeacherName2/>
            //    <TeacherName3/>
            //    <Group/>
            //    <LimitNextDay>f</LimitNextDay>
            //    <Comment/>
            //</CourseSection>
            #endregion

            XElement RequestElement = XElement
.Load(new StringReader("<Request><Condition><SchoolYear>" + SchoolYear + "</SchoolYear><Semester>" + Semester + "</Semester></Condition></Request>"));

            XElement ResponseElement = SendRequest(Connection, "_.GetCourseSectionNew", RequestElement);

            return ResponseElement;
        }

        /// <summary>
        /// 取得課程分段
        /// </summary>
        /// <param name="Connection">連線物件</param>
        /// <param name="SchoolYear">學年度</param>
        /// <param name="Semester">學期</param>
        /// <returns></returns>
        public static XElement GetCourseSection(Connection Connection, int SchoolYear, int Semester)
        {
            #region 範例
            //<Response>
            //    <CourseSection>
            //        <Uid>400189</Uid>
            //        <CourseID>11737</CourseID>
            //        <CourseName>普一甲 國文 I</CourseName>
            //        <SubjectAliasName>國文</SubjectAliasName>
            //        <Weekday>0</Weekday>
            //        <Period>0</Period>
            //        <Length>2</Length>
            //        <WeekdayCondition>=4</WeekdayCondition>
            //        <PeriodCondition/>
            //        <WeekFlag>3</WeekFlag>
            //        <LongBreak>f</LongBreak>
            //        <Lock>f</Lock>
            //        <ClassroomID>85579</ClassroomID>
            //        <ClassroomName>114</ClassroomName>
            //        <Subject>國文</Subject>
            //        <SubjectLevel>1</SubjectLevel>
            //        <ClassID>1486</ClassID>
            //        <AllowDuplicate>t</AllowDuplicate>
            //        <TimeTableID>85679</TimeTableID>
            //        <TimeTableName>日校一般班</TimeTableName>
            //        <TeacherName>江小恆</TeacherName>
            //        <TeacherNickName/>
            //        <Group/>
            //        <LimitNextDay/>
            //    </CourseSection>
            //<Response>\
            #endregion

            XElement RequestElement = XElement
.Load(new StringReader("<Request><Condition><SchoolYear>" + SchoolYear + "</SchoolYear><Semester>" + Semester + "</Semester></Condition></Request>"));

            XElement ResponseElement = SendRequest(Connection,"_.GetCourseSectionEx", RequestElement);

            return ResponseElement;
        }

        /// <summary>
        /// 取得時間表
        /// </summary>
        /// <param name="Connection"></param>
        /// <returns></returns>
        public static XElement GetTimeTable(Connection Connection)
        {
            XElement Element = SendRequest(Connection,"_.GetTimeTable",new XElement("Request"));

            return Element; 
        }

        /// <summary>
        /// 取得時間表分段
        /// </summary>
        /// <param name="Connection"></param>
        /// <returns></returns>
        public static XElement GetTimeTableSec(Connection Connection)
        {
            XElement Element = SendRequest(Connection,"_.GetTimeTableSec", new XElement("Request"));

            return Element;
        }

        /// <summary>
        /// 取得場地不排課時段
        /// </summary>
        /// <param name="Connection"></param>
        /// <returns></returns>
        public static XElement GetClassroomBusy(Connection Connection)
        {
            #region 範例
            //<Response>
            //    <ClassroomBusy>
            //        <Uid>400206</Uid>
            //        <ClassroomID>85579</ClassroomID>
            //        <ClassroomName>114</ClassroomName>
            //        <Weekday>1</Weekday>
            //        <BeginTime>1900/1/1 08:13</BeginTime>
            //        <WeekFlag>3</WeekFlag>
            //        <Duration>58</Duration>
            //    </ClassroomBusy>
            // </Response>
            #endregion

            XElement Element = SendRequest(Connection,"_.GetClassroomBusy", new XElement("Request"));

            return Element;
        }

        /// <summary>
        /// 根據連線取得場地名稱
        /// </summary>
        /// <param name="Connection"></param>
        /// <returns></returns>
        public static XElement GetClassroom(Connection Connection)
        {
            #region 範例
            //<Response>
            //    <Classroom>
            //        <Uid>400584</Uid>
            //        <Name>001</Name>
            //        <Capacity>1</Capacity>
            //        <LocationID/>
            //        <LocationOnly>f</LocationOnly>
            //        <LocationName/>
            //    </Classroom>
            //</Response>
            #endregion

            XElement Element = SendRequest(Connection,"_.GetClassroom", new XElement("Request"));

            return Element;
        }

        /// <summary>
        /// 根據連線取得教師不排課時段（舊的ischool）
        /// </summary>
        /// <param name="Connection"></param>
        /// <returns></returns>
        public static XElement GetTeacherBusy(Connection Connection)
        {
            #region 範例
            //<Response>
            //    <TeacherBusy>
            //        <Uid>400820</Uid>
            //        <TeacherID>9079</TeacherID>
            //        <TeacherName>Chang Chia Hua</TeacherName>
            //        <TeacherNickName/>
            //        <Weekday>1</Weekday>
            //        <BeginTime>1900/1/1 08:10</BeginTime>
            //        <Duration>120</Duration>
            //        <LocationID/>
            //        <LocationName/>
            //        <BusyDescription/>
            //    </TeacherBusy>
            //</Response> 
            #endregion

            XElement Element = SendRequest(Connection,"_.GetTeacherBusy", new XElement("Request"));

            return Element;
        }

        /// <summary>
        /// 根據連線取得教師不排課時段（排課獨立清單）
        /// </summary>
        /// <param name="Connection"></param>
        /// <returns></returns>
        public static XElement GetTeacherExBusy(Connection Connection)
        {
            XElement Element = SendRequest(Connection, "_.GetTeacherExBusy", new XElement("Request"));

            return Element;
        }

        /// <summary>
        /// 根據連線取得班級不排課時段（排課獨立清單）
        /// </summary>
        /// <param name="Connection"></param>
        /// <returns></returns>
        public static XElement GetClassExBusy(Connection Connection)
        {
            XElement Element = SendRequest(Connection, "_.GetClassExBusy", new XElement("Request"));

            return Element;
        }

        /// <summary>
        /// 從課程分段上取得教師
        /// </summary>
        /// <param name="Connection"></param>
        /// <returns></returns>
        public static XElement GetTeacherByCourseSection(Connection Connection)
        {
            XElement Element = SendRequest(Connection, "_.GetCourseSectionTeacher", new XElement("Request"));

            return Element;
        }

        /// <summary>
        /// 從課程分段上取得教師
        /// </summary>
        /// <param name="Connection"></param>
        /// <returns></returns>
        public static XElement GetTeacherByCourseSectionNew(Connection Connection,string SchoolYear,string Semester)
        {
            XElement RequestElement = XElement
.Load(new StringReader("<Request><Condition><SchoolYear>" + SchoolYear + "</SchoolYear><Semester>" + Semester + "</Semester></Condition></Request>"));

            XElement ResponseElement = SendRequest(Connection, "_.GetCourseSectionTeacherNew", RequestElement);

            return ResponseElement;
        }

        /// <summary>
        /// 根據連線取得教師
        /// </summary>
        /// <param name="Connection"></param>
        public static XElement GetTeacher(Connection Connection)
        {
            #region 範例
            //<Response>
            //    <Teacher>
            //        <Id>9186</Id>
            //        <TeacherName>Beryce</TeacherName>
            //        <TeacherNickName/>
            //    </Teacher>
            //</Response>
            #endregion

            XElement Element = SendRequest(Connection,"_.GetTeacherEx", new XElement("Request"));

            return Element;
        }

        /// <summary>
        /// 取得班級名稱對應系統編號
        /// </summary>
        /// <param name="Connection"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetClassNameIDs(Connection Connection)
        {
            Dictionary<string, string> ClassNameIDs = new Dictionary<string, string>();            

            XElement RequestElement = XElement
    .Load(new StringReader("<Request><Field><ID/><ClassName/></Field></Request>"));

            XElement Element = SendRequest(Connection, "_.GetClass", new XElement("Request"));

            foreach (XElement elmClass in Element.Elements("Class"))
            {
                string DSNS = Connection.AccessPoint.Name;
                string ClassID = elmClass.ElementText("ID");
                string ClassName = elmClass.ElementText("ClassName");
                string FullClassName = DSNS + "," + ClassName;

                if (!ClassNameIDs.ContainsKey(FullClassName))
                    ClassNameIDs.Add(FullClassName, ClassID);
            }

            //<Response>
            // <Class>
            //     <ID>1437</ID>
            //     <ClassName>301</ClassName>
            // </Class>
            // <Class>
            //     <ID>1426</ID>
            //     <ClassName>LIP{4}信</ClassName>
            // </Class>
            //</Response>

            return ClassNameIDs;
        }

        /// <summary>
        /// 取得班級列表
        /// </summary>
        /// <param name="Connection"></param>
        /// <param name="SchoolYear"></param>
        /// <param name="Semester"></param>
        public static XElement GetClassEx(Connection Connection)
        {
            XElement RequestElement = new XElement("Request",string.Empty);

            XElement Element = SendRequest(Connection,"_.GetClassEx",RequestElement);

            return Element;
        }

        /// <summary>
        /// 根據連線及學年度學期取得班級列表
        /// </summary>
        /// <param name="Connection">連線</param>
        /// <param name="SchoolYear">學年度</param>
        /// <param name="Semester">學期</param>
        /// <returns></returns>
        public static XElement GetClass(Connection Connection, string SchoolYear,string Semester)
        {
            #region 範例
            //<Request>
            //    <Condition>
            //        <SchoolYear>100</SchoolYear>
            //        <Semester>1</Semester>
            //    </Condition>
            //</Request>

            //<Response>
            //    <Class>
            //        <ID>1437</ID>
            //        <ClassName>301</ClassName>
            //        <TimeTableID>85711</TimeTableID>
            //        <TimeTableName>日校實驗班51</TimeTableName>
            //    </Class>
            //</Response>
            #endregion

            XElement RequestElement = XElement
                .Load(new StringReader("<Request><Condition><SchoolYear>" + SchoolYear + "</SchoolYear><Semester>" + Semester + "</Semester></Condition></Request>"));

            XElement Element = SendRequest(Connection,"_.GetClassNew",RequestElement);

            return Element;
        }

        /// <summary>
        /// 根據連線取得地點列表
        /// </summary>
        /// <param name="Connection">連線物件</param>
        /// <returns></returns>
        public static XElement GetLocation(Connection Connection)
        {
            XElement Element = SendRequest(Connection,"_.GetLocation", new XElement("Request"));

            return Element;
        }

        /// <summary>
        /// 取得在Greening上可連線的DSNS
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAvailDSNSNames(PassportToken Passport)
        {
            List<string> Names = new List<string>();

            try
            {
                Tuple<Connection, string> result = GetConnection(Passport,DSAServices.GreeningAccessPoint, "user");

                if (!string.IsNullOrEmpty(result.Item2))
                    throw new Exception(result.Item2);

                Envelope Response = result.Item1.SendRequest("GetMyDomainInfo", new Envelope());

                XElement Element = XElement.Load(new StringReader(Response.Body.XmlString));

                foreach (XElement SubElement in Element
                    .Element("APUrl")
                    .Elements("url"))
                    Names.Add(SubElement.Value);
            }
            catch (Exception ve)
            {
                throw ve;
            }

            return Names;
        }

        /// <summary>
        /// 取得在Greening上可連線的DSNS
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string,string> GetAvailSchoolAndDSNSNames(PassportToken Passport)
        {
            Dictionary<string, string> Names = new Dictionary<string, string>();

            //Service:
            //GetApplicationListRef

            //Request(內容固定):
            //<Request>
            //   <Type>dynpkg</Type>
            //</Request>

            //Response sample:
            //<Response>
            //    <User>
            //        <App AccessPoint="test.kh.edu.tw" Title="高雄測試國中" Type="dynpkg" User="yaoming.huang@ischool.com.tw">
            //            <Memo />
            //        </App>
            //        <App AccessPoint="dev.sh_d_" Title="" Type="dynpkg" User="yaoming.huang@ischool.com.tw">
            //            <Memo />
            //        </App>
            //    <Domain />
            //</Response>

            try
            {
                Tuple<Connection, string> result = GetConnection(Passport, DSAServices.GreeningAccessPoint, "user");

                if (!string.IsNullOrEmpty(result.Item2))
                    throw new Exception(result.Item2);

                XElement elmRequest = new XElement("Request");
                elmRequest.Add(new XElement("Type","dynpkg"));

                XElement Element = SendRequest(
                    result.Item1,
                    "GetApplicationListRef",
                    elmRequest);

                foreach (XElement SubElement in Element
                    .Element("User")
                    .Elements("App"))
                {
                    string NDSNSame = SubElement.AttributeText("AccessPoint");
                    string SchoolName = SubElement.AttributeText("Title");

                    if (string.IsNullOrEmpty(SchoolName))
                        SchoolName = NDSNSame;

                    if (!Names.ContainsKey(SchoolName))
                        Names.Add(SchoolName, NDSNSame);
                }

                foreach (XElement SubElement in Element
                    .Element("Domain")
                    .Elements("App"))
                {
                    string DSNSName = SubElement.AttributeText("AccessPoint");
                    string SchoolName = SubElement.AttributeText("Title");

                    if (string.IsNullOrEmpty(SchoolName))
                        SchoolName = DSNSName;

                    if (!Names.ContainsKey(SchoolName))
                        Names.Add(SchoolName, DSNSName);
                }
            }
            catch (Exception ve)
            {
                throw ve;
            }

            return Names;
        }

        /// <summary>
        /// 取得在Greening上可連線的DSNS
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAvailDSNSNames()
        {
            List<string> Names = new List<string>();

            try
            {
                Tuple<Connection,string> result = GetConnection(DSAServices.GreeningAccessPoint, "user");

                if (!string.IsNullOrEmpty(result.Item2))
                    throw new Exception(result.Item2);

              

                Envelope Response = result.Item1.CallService("GetMyDomainInfo", new Envelope());

                XElement Element = XElement.Load(new StringReader(Response.Body.XmlString));

                foreach (XElement SubElement in Element
                    .Element("APUrl")
                    .Elements("url"))
                    Names.Add(SubElement.Value);
            }
            catch (Exception ve)
            {
                throw ve;
            }

            return Names;
        }

        /// <summary>
        /// 取得預設學年度及學期
        /// </summary>
        /// <returns></returns>
        public static Tuple<int, int> GetDefaultSchoolYearAndSemester()
        {
            XElement RequestElement = XElement
    .Load(new StringReader("<Request><Condition><Name>系統設定</Name></Condition></Request>"));

            Tuple<Connection,string> result = GetConnection(FISCA.Authentication.DSAServices.AccessPoint);

            if (!string.IsNullOrEmpty(result.Item2))
                throw new Exception(result.Item2);

            XElement ResponseElement = SendRequest(result.Item1,"_.GetSystemConfig", RequestElement)
                .Element("List")
                .Element("Content")
                .Element("SystemConfig");
         
            return new Tuple<int, int>(Int.Parse(ResponseElement.ElementText("DefaultSchoolYear")), Int.Parse(ResponseElement.ElementText("DefaultSemester")));

            #region 舊版程式
            //QueryHelper Helper = new QueryHelper();

            //DataTable Table = Helper.Select("select * from list where name='系統設定'");

            //if (Table.Rows.Count > 0)
            //{
            //    DataRow Row = Table.Rows[0];

            //    string strContent = Row.Field<string>("content");

            //    XElement Element = XElement.Load(new StringReader(strContent));

            //    //<SystemConfig>
            //    //    <DefaultSchoolYear>98</DefaultSchoolYear>
            //    //    <DefaultSemester>2</DefaultSemester>
            //    //</SystemConfig>
            //    DefaultSchoolYear = Element.ElementText("DefaultSchoolYear");
            //    DefaultSemester = Element.ElementText("DefaultSemester");
            //}
            #endregion
        }

        /// <summary>
        /// 更新課程是否可重覆排課
        /// </summary>
        /// <param name="Conn"></param>
        /// <param name="CourseAllowDups"></param>
        public static void UpdateCourseAllowDup(Connection Conn,Dictionary<string, bool> CourseAllowDups)
        {
            StringBuilder strBuilder = new StringBuilder();

            foreach (string CourseID in CourseAllowDups.Keys)
            {
                strBuilder.AppendLine("<CourseExtension>");
                strBuilder.AppendLine("<AllowDuplicate>" + CourseAllowDups[CourseID] + "</AllowDuplicate>");
                strBuilder.AppendLine("<CourseID>" + CourseID + "</CourseID>");
                strBuilder.AppendLine("</CourseExtension>");
            }

            XElement RequestElement = XElement
                .Load(new StringReader("<Request>" + strBuilder.ToString() + "</Request>"));

            SendRequest(Conn, "_.UpdateCourseExtension", RequestElement);  
        }

        /// <summary>
        /// 刪除課程分段
        /// </summary>
        /// <param name="CourseIDs"></param>
        public static void DeleteCourseSections(Connection Conn,List<string> CourseIDs)
        {
            StringBuilder strBuilder = new StringBuilder();

            foreach(string CourseID in CourseIDs)
            {
                strBuilder.AppendLine("<CourseID>" + CourseID +"</CourseID>");
            }            		

            XElement RequestElement = XElement
                .Load(new StringReader("<Request><CourseSection><Condition>" + strBuilder.ToString()  + "</Condition></CourseSection></Request>"));

            SendRequest(Conn,"_.DeleteCourseSectionNew", RequestElement); 
        }

        /// <summary>
        /// 刪除所有教師不排課時段
        /// </summary>
        /// <param name="Conn"></param>
        public static void DeleteTeacherBusy(Connection Conn)
        {
            //<Request>
            //    <TeacherBusy>
            //        <Condition></Condition>
            //    </TeacherBusy>
            //</Request>

            XElement Request = XElement.Load(new StringReader("<Request/>"));

            XElement elmTeacher = new XElement("TeacherBusy");

            XElement elmCondition = new XElement("Condition");

            elmTeacher.Add(elmCondition);

            Request.Add(elmTeacher);           

            SendRequest(Conn,"_.DeleteTeacherBusy", Request);
        }
        
        /// <summary>
        /// 新增教師不排課時段
        /// </summary>
        /// <param name="Conn"></param>
        /// <param name="TeacherBusys"></param>
        public static void InsertTeacherBusy(Connection Conn,List<STeacherBusy> TeacherBusys)
        {
            XElement Request = XElement.Load(new StringReader("<Request/>"));

            foreach(STeacherBusy vTeacherBusy in TeacherBusys)
            {
                XElement Element = new XElement("TeacherBusy");

                XElement elmField = new XElement("Field");

                elmField.Add(new XElement("BeginTime",vTeacherBusy.BeginTime.ToString("yyyy/MM/dd HH:mm:ss")));
                elmField.Add(new XElement("BusyDescription",vTeacherBusy.Description));
                elmField.Add(new XElement("Duration",vTeacherBusy.Duration));
                elmField.Add(new XElement("TeacherID",vTeacherBusy.TeacherID));
                elmField.Add(new XElement("LocationID", vTeacherBusy.LocationID));
                elmField.Add(new XElement("Weekday",vTeacherBusy.WeekDay));

                Element.Add(elmField);

                Request.Add(Element);
            }

            SendRequest(Conn, "_.InsertTeacherBusy", Request);
        }

        /// <summary>
        /// 刪除所有班級不排課時段
        /// </summary>
        /// <param name="Conn"></param>
        public static void DeleteClassBusy(Connection Conn)
        {
            //<Request>
            //    <ClassBusy>
            //        <Condition></Condition>
            //    </ClassBusy>
            //</Request>

            XElement Request = XElement.Load(new StringReader("<Request/>"));

            XElement elmClass = new XElement("ClassBusy");

            XElement elmCondition = new XElement("Condition");

            elmClass.Add(elmCondition);

            Request.Add(elmClass);

            SendRequest(Conn, "_.DeleteClassBusy", Request);
        }

        /// <summary>
        /// 新增班級不排課時段
        /// </summary>
        /// <param name="Conn"></param>
        /// <param name="ClassBusys"></param>
        public static void InsertClassBusy(Connection Conn, List<SClassBusy> ClassBusys)
        {
            XElement Request = XElement.Load(new StringReader("<Request/>"));

            foreach (SClassBusy vClassBusy in ClassBusys)
            {
                XElement Element = new XElement("ClassBusy");

                XElement elmField = new XElement("Field");

                elmField.Add(new XElement("BeginTime", vClassBusy.BeginTime.ToString("yyyy/MM/dd HH:mm:ss")));
                elmField.Add(new XElement("BusyDescription", vClassBusy.Description));
                elmField.Add(new XElement("Duration", vClassBusy.Duration));
                elmField.Add(new XElement("ClassID", vClassBusy.ClassID));
                elmField.Add(new XElement("Weekday", vClassBusy.WeekDay));

                Element.Add(elmField);

                Request.Add(Element);
            }

            SendRequest(Conn, "_.InsertClassBusy", Request);
        }

        /// <summary>
        /// 刪除所有場地不排課時段
        /// </summary>
        /// <param name="Conn"></param>
        public static void DeleteClassroomBusy(Connection Conn)
        {
            //<Request>
            //    <ClassroomBusyBusy>
            //        <Condition></Condition>
            //    </ClassroomBusyBusy>
            //</Request>

            XElement Request = XElement.Load(new StringReader("<Request/>"));

            XElement elmClassroom = new XElement("ClassroomBusy");

            XElement elmCondition = new XElement("Condition");

            elmClassroom.Add(elmCondition);

            Request.Add(elmClassroom);

            SendRequest(Conn, "_.DeleteClassroomBusy", Request);
        }

        /// <summary>
        /// 新增場地不排課時段
        /// </summary>
        /// <param name="Conn"></param>
        /// <param name="ClassroomBusys"></param>
        public static void InsertClassroomBusy(Connection Conn, List<SClassroomBusy> ClassroomBusys)
        {
            XElement Request = XElement.Load(new StringReader("<Request/>"));

            foreach (SClassroomBusy vClassroomBusy in ClassroomBusys)
            {
                XElement Element = new XElement("ClassroomBusy");

                XElement elmField = new XElement("Field");

                elmField.Add(new XElement("BeginTime", vClassroomBusy.BeginTime.ToString("yyyy/MM/dd HH:mm:ss")));
                elmField.Add(new XElement("BusyDescription", vClassroomBusy.Description));
                elmField.Add(new XElement("Duration", vClassroomBusy.Duration));
                elmField.Add(new XElement("ClassroomID", vClassroomBusy.ClassroomID));
                elmField.Add(new XElement("Weekday", vClassroomBusy.WeekDay));
                elmField.Add(new XElement("WeekFlag", vClassroomBusy.WeekFlag));

                Element.Add(elmField);

                Request.Add(Element);
            }

            SendRequest(Conn, "_.InsertClassroomBusy", Request);
        }

        /// <summary>
        /// 更新課程排課資料
        /// </summary>
        /// <param name="Conn"></param>
        /// <param name="Element"></param>
        public static void UpdateCourseExtension(Connection Conn,XElement Element)
        {
            SendRequest(Conn, "_.UpdateCourseExtensionNew", Element); 
        }

        /// <summary>
        /// 新增課程分段
        /// </summary>
        /// <param name="Sections"></param>
        public static void InsertCourseSections(Connection Conn,List<SchedulerCourseSection> Sections)
        {
            XElement RequestElement = XElement.Load(new StringReader("<Request/>"));

            foreach (SchedulerCourseSection Section in Sections)
            {
                XElement CourseElement = new XElement("CourseSection");

                XElement FieldElement = new XElement("Field");

                FieldElement.Add(new XElement("Length", Section.Length));
                FieldElement.Add(new XElement("Lock", Section.Lock));
                FieldElement.Add(new XElement("LongBreak", Section.LongBreak));
                FieldElement.Add(new XElement("Weekday", Section.WeekDay));
                FieldElement.Add(new XElement("Period", Section.Period));
                FieldElement.Add(new XElement("WeekdayCondition", Section.WeekDayCond));
                FieldElement.Add(new XElement("PeriodCondition", Section.PeriodCond));
                FieldElement.Add(new XElement("RefClassroomID", Section.ClassroomID));
                FieldElement.Add(new XElement("RefCourseID", Section.CourseID));
                FieldElement.Add(new XElement("WeekFlag", Section.WeekFlag));

                FieldElement.Add(new XElement("TeacherName1", Section.TeacherName1));
                FieldElement.Add(new XElement("TeacherName2", Section.TeacherName2));
                FieldElement.Add(new XElement("TeacherName3", Section.TeacherName3));

                FieldElement.Add(new XElement("TeacherID1", Section.TeacherID1));
                FieldElement.Add(new XElement("TeacherID2", Section.TeacherID2));
                FieldElement.Add(new XElement("TeacherID3", Section.TeacherID3));

                FieldElement.Add(new XElement("Comment", Section.Comment));

                CourseElement.Add(FieldElement);

                RequestElement.Add(CourseElement);
            }

            SendRequest(Conn,"_.InsertCourseSectionNew", RequestElement);
        }

        /// <summary>
        /// 設定DSNS
        /// </summary>
        /// <returns></returns>
        public static void SetDSNSConfig(string Name,string Value)
        {
            #region 範例
            //<Request>
            //    <Config>
            //        <!--以下為非必要欄位-->
            //        <LastUpdate></LastUpdate>
            //        <Name></Name>
            //        <Value></Value>
            //    </Config>
            //</Request>
            #endregion

            Tuple<Connection,string> result = GetConnection(FISCA.Authentication.DSAServices.AccessPoint);

            XElement RequestElement = new XElement("Request");
            XElement ConfigElement = new XElement("Config");
            ConfigElement.Add(new XElement("Name",Name));
            ConfigElement.Add(new XElement("Value",Value));

            RequestElement.Add(ConfigElement);
            SendRequest(result.Item1,"_.SetConfig", RequestElement);
        }

        /// <summary>
        /// 取得DSNS設定
        /// </summary>
        /// <returns></returns>
        public static string GetDSNSConfig()
        {
            #region 範例
            //<Request>
            //    <Field>
            //        <Value/>
            //    </Field>
            //    <Condition>
            //        <Name>DSNS</Name>
            //    </Condition>
            //</Request>

            //<Response>
            //    <Config>
            //        <Uid>364014</Uid>
            //        <LastUpdate>2012-03-29 15:36:35.27266</LastUpdate>
            //        <Name>DSNS</Name>
            //        <Value>http://test.iteacher.tw/cs4/test_jh_hs</Value>
            //    </Config>
            //</Response>
            #endregion

            XElement RequestElement = XElement
                .Load(new StringReader("<Request><Field><Value/></Field><Condition><Name>DSNS</Name></Condition></Request>"));

            Tuple<Connection,string> result = GetConnection(FISCA.Authentication.DSAServices.AccessPoint);

            try
            {
                XElement ResponseElement = SendRequest(result.Item1,"_.GetConfig",RequestElement) 
                    .Element("Config")
                    .Element("Value");

                return ResponseElement.Value;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}