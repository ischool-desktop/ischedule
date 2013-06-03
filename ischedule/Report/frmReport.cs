using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aspose.Cells;
using ReportHelper;
using Sunset.Data;

namespace ischedule
{
    public partial class frmReport : BaseForm
    {
        private string Filename = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\CustomizeTemplate";
        private List<string> AssocIDs;
        private Scheduler schLocal = Scheduler.Instance;
        private Appointments apsCur;
        private TimeTable ttCur;
        private string LPViewName = string.Empty;
        private string LPViewType = string.Empty;
        private string LPViewClassTeacherName = string.Empty;
        private string mstrCustomizeTemplate;
        private MemoryStream mDefaultTemplateStream = new MemoryStream(Properties.Resources.功課表);
        private MemoryStream mCustomizeTemplateStream;
        private byte[] mCustomizeTemplateBuffer;
        private Task mLoadPreference;
        private Task mSavePreference;

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public frmReport()
        {
            InitializeComponent();

            lnkViewDefault.Click += (sender, e) =>
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "另存新檔";
                sfd.FileName = "功課表.xls";
                sfd.Filter = "相容於 Excel 2003 檔案 (*.xls)|*.xls|所有檔案 (*.*)|*.*";
                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK == true)
                {
                    try
                    {
                        DownloadTemplate(sfd, Properties.Resources.功課表);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            };

            lnkViewCustom.Click += (sender, e) =>
            {
                mLoadPreference.Wait();

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "另存新檔";
                sfd.FileName = "功課表.xls";
                sfd.Filter = "相容於 Excel 2003 檔案 (*.xls)|*.xls|所有檔案 (*.*)|*.*";
                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        DownloadTemplate(sfd, mCustomizeTemplateBuffer);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            };

            lnkUploadCustom.Click += (sender, e) =>
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Title = "上傳自訂功課表範本";
                ofd.Filter = "相容於 Excel 2003 檔案 (*.xls)|*.xls";
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        bool IsUpload = false;
                        string Base64String = string.Empty;

                        UploadTemplate(ofd.FileName, ref IsUpload, ref Base64String);

                        mstrCustomizeTemplate = Base64String;
                        mCustomizeTemplateBuffer = Convert.FromBase64String(Base64String);
                        mCustomizeTemplateStream = new MemoryStream(mCustomizeTemplateBuffer);

                        mSavePreference = Task.Factory.StartNew(SavePreference);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            };
        }


        /// <summary>
        /// 載入設定
        /// </summary>
        private void LoadPreference()
        {
            if (!File.Exists(Filename))
                File.CreateText(Filename).Close();

            mstrCustomizeTemplate = File.ReadAllText(Filename);

            if (!string.IsNullOrEmpty(mstrCustomizeTemplate))
            {
                mCustomizeTemplateBuffer = Convert.FromBase64String(mstrCustomizeTemplate);
                mCustomizeTemplateStream = new MemoryStream(mCustomizeTemplateBuffer);
            }
        }

        /// <summary>
        /// 儲存設定
        /// </summary>
        private void SavePreference()
        {
            File.WriteAllText(Filename, mstrCustomizeTemplate);
        }

        /// <summary>
        /// 下載模組
        /// </summary>
        /// <param name="sfd"></param>
        /// <param name="fileData"></param>
        private void DownloadTemplate(SaveFileDialog sfd, byte[] fileData)
        {
            if ((fileData == null) || (fileData.Length == 0))
            {
                throw new Exception("檔案不存在，無法檢視。");
            }

            try
            {
                FileStream fs = new FileStream(sfd.FileName, FileMode.Create);

                fs.Write(fileData, 0, fileData.Length);
                fs.Close();
                System.Diagnostics.Process.Start(sfd.FileName);
            }
            catch
            {
                throw new Exception("指定路徑無法存取。");
            }
        }

        /// <summary>
        /// 上傳樣版
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="uploadIndex"></param>
        /// <param name="uploadData"></param>
        private void UploadTemplate(string fileName, ref bool uploadIndex, ref string uploadData)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open);

            byte[] tempBuffer = new byte[fs.Length];
            fs.Read(tempBuffer, 0, tempBuffer.Length);

            MemoryStream ms = new MemoryStream(tempBuffer);

            try
            {
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();

                wb.Open(ms, Aspose.Cells.FileFormatType.Excel2003);
                wb = null;
            }
            catch
            {
                throw new Exception("此範本限用相容於 Excel 2003 檔案。");
            }

            try
            {
                uploadData = Convert.ToBase64String(tempBuffer);
                uploadIndex = true;

                fs.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 載入表單
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmReport_Load(object sender, EventArgs e)
        {
            mLoadPreference = Task.Factory.StartNew(LoadPreference);

            #region 初始化時間表
            cboTimeTable.Items.Clear();
            List<string> TimeTableIDs = new List<string>();

            AssocIDs = new List<string>();

            #region 取得關連的系統編號
            switch (MainFormBL.Instance.GetSelectedType())
            {
                //取得所有選取教師系統編號
                case "Teacher":
                    AssocIDs = MainFormBL.Instance.TeacherList.SelectedIDs;
                    break;
                //取得所有選取班級系統編號
                case "Class":
                    AssocIDs = MainFormBL.Instance.ClassList.SelectedIDs;
                    break;
                //取得所有選取場地系統編號
                case "Classroom":
                    AssocIDs = MainFormBL.Instance.ClassroomList.SelectedIDs;
                    break;
            }
            #endregion

            AssocIDs.ForEach(x =>
            {
                switch (MainFormBL.Instance.GetSelectedType())
                {
                    case "Teacher":
                        schLocal.Teachers[x].UseAppointments(0);    //預設使用第一個行事曆
                        foreach (Appointment app in schLocal.Teachers[x].Appointments)
                        {
                            if (!string.IsNullOrEmpty(app.EventID))
                            {
                                string TimeTableID = GetTimeTableID(app.EventID);
                                if (!TimeTableIDs.Contains(TimeTableID))
                                    TimeTableIDs.Add(TimeTableID);
                            }
                        }
                        break;
                    case "Class":
                        foreach (Appointment app in schLocal.Classes[x].Appointments)
                        {
                            if (!string.IsNullOrEmpty(app.EventID))
                            {
                                string TimeTableID = GetTimeTableID(app.EventID);
                                if (!TimeTableIDs.Contains(TimeTableID))
                                    TimeTableIDs.Add(TimeTableID);
                            }
                        }
                        break;
                    case "Classroom":
                        schLocal.Classrooms[x].UseAppointments(0); //預設使用第一個行事曆
                        foreach (Appointment app in schLocal.Classrooms[x].Appointments)
                        {
                            if (!string.IsNullOrEmpty(app.EventID))
                            {
                                string TimeTableID = GetTimeTableID(app.EventID);
                                if (!TimeTableIDs.Contains(TimeTableID))
                                    TimeTableIDs.Add(TimeTableID);
                            }
                        }
                        break;
                }
            });

            TimeTableIDs.ForEach(x => cboTimeTable.Items.Add(schLocal.TimeTables[x]));

            cboTimeTable.SelectedIndex = 0;

            chkMergeTimeTable.CheckedChanged += (vsender, ve) =>
            {
                cboTimeTable.Enabled = chkMergeTimeTable.Checked;
                chkTeacherBusyDesc.Enabled = chkMergeTimeTable.Checked;
            };
            #endregion

            #region 報表顯示項目
            switch (MainFormBL.Instance.GetSelectedType())
            {
                case "Teacher":
                    chkTeacher.Checked = false;
                    chkClass.Checked = true;
                    chkClassroom.Checked = false;
                    chkSubject.Checked = true;
                    chkSubjectAlias.Checked = false;
                    chkCourseName.Checked = false;
                    break;
                case "Class":
                    chkTeacher.Checked = true;
                    chkClass.Checked = false;
                    chkClassroom.Checked = false;
                    chkSubject.Checked = true;
                    chkSubjectAlias.Checked = false;
                    chkCourseName.Checked = false;
                    break;
                case "Classroom":
                    chkTeacher.Checked = true;
                    chkClass.Checked = false;
                    chkClassroom.Checked = false;
                    chkSubject.Checked = true;
                    chkSubjectAlias.Checked = false;
                    chkCourseName.Checked = false;
                    break;
            }
            #endregion
        }

        /// <summary>
        /// 列印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            mLoadPreference.Wait();

            ttCur = cboTimeTable.SelectedItem as TimeTable;

            if (ttCur == null)
            {
                MessageBox.Show("無時間表設定，無法列印！");
                return;
            }

            Print();
        }

        /// <summary>
        /// 離開
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 列印報表
        /// </summary>
        private void Print()
        {
            SortedDictionary<string, List<DataSet>> LPViews = new SortedDictionary<string, List<DataSet>>();

            AssocIDs.ForEach(x =>
            {
                DataSet LPView = new DataSet("DataSection");
                string BasicLength = string.Empty;
                string ExtraLength = string.Empty;
                string CounselingLength = string.Empty;
                string Comment = string.Empty;

                switch (MainFormBL.Instance.GetSelectedType())
                {
                    case "Teacher":
                        schLocal.Teachers[x].UseAppointments(0);    //預設使用第一個行事曆
                        apsCur = schLocal.Teachers[x].Appointments; //取得資源已被排定的約會
                        LPViewName = schLocal.Teachers[x].Name;
                        LPViewType = "教師";
                        LPViewClassTeacherName = string.Empty;
                        BasicLength = schLocal.Teachers[x].BasicLength.HasValue? ""+schLocal.Teachers[x].BasicLength:string.Empty;
                        ExtraLength = schLocal.Teachers[x].ExtraLength.HasValue? ""+schLocal.Teachers[x].ExtraLength:string.Empty;
                        CounselingLength = schLocal.Teachers[x].CounselingLength.HasValue? ""+schLocal.Teachers[x].CounselingLength:string.Empty;
                        Comment = schLocal.Teachers[x].Comment;
                        break;
                    case "Class":
                        apsCur = schLocal.Classes[x].Appointments;
                        LPViewName = schLocal.Classes[x].Name;
                        LPViewType = "班級";
                        LPViewClassTeacherName = schLocal.Classes[x].TeacherName;
                        break;
                    case "Classroom":
                        schLocal.Classrooms[x].UseAppointments(0); //預設使用第一個行事曆
                        apsCur = schLocal.Classrooms[x].Appointments;
                        LPViewName = schLocal.Classrooms[x].Name;
                        LPViewType = "場地";
                        LPViewClassTeacherName = string.Empty;
                        break;
                }

                if (!LPViews.ContainsKey(LPViewName))
                    LPViews.Add(LPViewName, new List<DataSet>());

                DataTable tabSchoolYear = ("" + schLocal.SchoolYear).ToDataTable("SchoolYear", "學年度");
                DataTable tabSemester = ("" + schLocal.Semester).ToDataTable("Semester", "學期");
                DataTable tabLPViewName = LPViewName.ToDataTable("ScheduleName", "資源名稱");
                DataTable tabLPViewType = LPViewType.ToDataTable("ScheduleType", "資源類別");
                DataTable tabLPViewClassTeacherName = LPViewClassTeacherName.ToDataTable("ClassTeacherName", "班導師姓名");

                DataTable tabBasicLength = BasicLength.ToDataTable("TeacherBasicLength", "教師基本時數");
                DataTable tabExtraLength = ExtraLength.ToDataTable("TeacherExtraLength", "教師兼課時數");
                DataTable tabCounselingLength = CounselingLength.ToDataTable("TeacherCounselingLength", "教師輔導時數");
                DataTable tabComment = Comment.ToDataTable("TeacherComment", "教師註解");

                LPView.Tables.Add(tabSchoolYear);
                LPView.Tables.Add(tabSemester);
                LPView.Tables.Add(tabLPViewName);
                LPView.Tables.Add(tabLPViewType);
                LPView.Tables.Add(tabLPViewClassTeacherName);
                LPView.Tables.Add(tabComment);

                DataTable tabScheduleDetail = GetLPViewDetail();

                LPView.Tables.Add(tabScheduleDetail);

                LPViews[LPViewName].Add(LPView);
            }
            );

            try
            {
                Dictionary<string, List<DataSet>> result = new Dictionary<string, List<DataSet>>();

                foreach (string Key in LPViews.Keys)
                {
                    if (!result.ContainsKey(Key))
                        result.Add(Key, LPViews[Key]);
                }

                MemoryStream Stream = new MemoryStream();

                if (rdoDefualt.Checked)
                    Stream = mDefaultTemplateStream;
                else
                    Stream = mCustomizeTemplateStream;

                Workbook wb = Report.Produce(result, Stream);

                string mSaveFilePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\Reports\\功課表.xls";

                ReportSaver.SaveWorkbook(wb, mSaveFilePath);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// 更新時間表
        /// </summary>
        /// <param name="nTimeTableID"></param>
        private void ChangeTimeTable(string nTimeTableID)
        {
            //Update combo box
            //針對每個時間表項目
            for (int nIndex = 0; nIndex < cboTimeTable.Items.Count; nIndex++)
            {
                dynamic ttItem = cboTimeTable.Items[nIndex];

                //若是時間表編號等於目前時間表編號
                if (ttItem.TimeTableID == nTimeTableID)
                {
                    //將ComboBox的時間表編號設為現在的
                    cboTimeTable.SelectedIndex = nIndex;
                    break;
                }
            }

            ttCur = schLocal.TimeTables[nTimeTableID];
        }

        /// <summary>
        /// 根據事件編號取得時間表編號
        /// </summary>
        /// <param name="EventID">事件編號</param>
        /// <returns>時間表編號</returns>
        private string GetTimeTableID(string EventID)
        {
            return schLocal.CEvents[EventID].TimeTableID;
        }

        /// <summary>
        /// 根據分課表編號同步TimeTable編號
        /// </summary>
        /// <param name="EventID">分課表編號</param>
        public void SyncTimeTable(string EventID)
        {
            ChangeTimeTable(GetTimeTableID(EventID));
        }

        /// <summary>
        /// 取得單雙週顯示字串
        /// </summary>
        /// <param name="EventIDS"></param>
        /// <param name="EventIDD"></param>
        /// <returns></returns>
        private string GetEventDisplayStr(string EventIDS, string EventIDD)
        {
            string strEvtInfo = string.Empty;

            strEvtInfo += GetEventDisplayStr(EventIDS);

            strEvtInfo += GetEventDisplayStr(EventIDD);

            return strEvtInfo;
        }

        /// <summary>
        /// 取得事件顯示字串
        /// </summary>
        /// <returns></returns>
        private string GetEventDisplayStr(string EventID)
        {
            string strEvtInfo = string.Empty;

            CEvent eventLocal = schLocal.CEvents[EventID];

            if (chkTeacher.Checked) strEvtInfo = eventLocal.GetTeacherString();

            if (chkClass.Checked && !schLocal.Classrooms[eventLocal.ClassroomID].Name.Equals("無"))
            {
                if (!string.IsNullOrEmpty(strEvtInfo))
                    strEvtInfo += System.Environment.NewLine;
                strEvtInfo += schLocal.Classrooms[eventLocal.ClassroomID].Name;
            }

            if (chkClass.Checked)
            {
                if (!string.IsNullOrEmpty(strEvtInfo))
                    strEvtInfo += System.Environment.NewLine;
                strEvtInfo += schLocal.Classes[eventLocal.ClassID].Name;
            }

            if (chkSubject.Checked)
            {
                if (!string.IsNullOrEmpty(strEvtInfo))
                    strEvtInfo += System.Environment.NewLine;
                strEvtInfo += schLocal.Subjects[eventLocal.SubjectID].Name + eventLocal.WeekFlag.GetWeekFlagStr();
            }

            if (chkSubjectAlias.Checked)
            {
                if (!string.IsNullOrEmpty(strEvtInfo))
                    strEvtInfo += System.Environment.NewLine + eventLocal.WeekFlag.GetWeekFlagStr();
                strEvtInfo += eventLocal.SubjectAlias;
            }

            if (chkCourseName.Checked)
            {
                if (!string.IsNullOrEmpty(strEvtInfo))
                    strEvtInfo += System.Environment.NewLine + eventLocal.WeekFlag.GetWeekFlagStr();
                strEvtInfo += eventLocal.CourseName;
            }

            if (chkComment.Checked)
            {
                if (!string.IsNullOrEmpty(strEvtInfo))
                    strEvtInfo += System.Environment.NewLine;
                strEvtInfo += eventLocal.Comment;
            }

            return strEvtInfo;
        }

        /// <summary>
        /// 取得合併功課表
        /// </summary>
        /// <returns></returns>
        private DataTable GetMergeLPView()
        {
            string strEvtInfo = string.Empty;
            SortedDictionary<int, LPViewReportItem> LPViewItems = new SortedDictionary<int, LPViewReportItem>();
            DataTable tabSchedule = new DataTable("ScheduleDetail");

            int Period;

            if (int.TryParse(txtPeriod.Text, out Period))
            {
                for (int i = 1; i <= Period; i++)
                {
                    if (!LPViewItems.ContainsKey(i))
                    {
                        //新增節次物件
                        LPViewReportItem itemPeriod = new LPViewReportItem();

                        //指定節次物件的節次
                        itemPeriod.PeriodNo = i;
                        itemPeriod.Time = string.Empty;

                        //將節次物件加到集合中
                        LPViewItems.Add(i, itemPeriod);
                    }
                }
            }

            //找出所有的分課表系統編號
            List<string> EventIDs = new List<string>();
            List<string> TimeTableIDs = new List<string>();
            int MaxWeekDay = -1;

            foreach (Appointment apCur in apsCur)
                if (!string.IsNullOrEmpty(apCur.EventID))
                    if (!EventIDs.Contains(apCur.EventID))
                        EventIDs.Add(apCur.EventID);

            //針對每筆分課表系統編號
            foreach (string EventID in EventIDs)
            {
                CEvent evtLocal = schLocal.CEvents[EventID];

                #region 取得分課時間表，並新增節次物件
                TimeTable ttLocal = schLocal.TimeTables[evtLocal.TimeTableID];

                if (!TimeTableIDs.Contains(evtLocal.TimeTableID))
                {
                    TimeTableIDs.Add(evtLocal.TimeTableID);
                    //取得最大星期
                    if (ttLocal.Periods.MaxWeekDay > MaxWeekDay)
                        MaxWeekDay = ttLocal.Periods.MaxWeekDay;

                    foreach (Period prdLocal in ttLocal.Periods)
                    {
                        if (prdLocal.PeriodNo != 0)
                        {
                            if (!LPViewItems.ContainsKey(prdLocal.PeriodNo))
                            {
                                //新增節次物件
                                LPViewReportItem itemPeriod = new LPViewReportItem();

                                //指定節次物件的節次
                                itemPeriod.PeriodNo = prdLocal.PeriodNo;
                                itemPeriod.Time = string.Empty;

                                //將節次物件加到集合中
                                LPViewItems.Add(prdLocal.PeriodNo, itemPeriod);
                            }
                        }
                    }
                }
                #endregion

                for (int i = evtLocal.PeriodNo; i < evtLocal.PeriodNo + evtLocal.Length; i++)
                {
                    LPViewReportItem itemCurrent = LPViewItems[i];

                    strEvtInfo = GetEventDisplayStr(evtLocal.EventID);
                    string strWeekDayText = itemCurrent.GetWeekDayText(evtLocal.WeekDay);

                    if (string.IsNullOrEmpty(strWeekDayText))
                        strWeekDayText = strEvtInfo;
                    else
                        strWeekDayText += System.Environment.NewLine + strEvtInfo;

                    itemCurrent.SetWeekDayText(evtLocal.WeekDay, strWeekDayText);
                }
            }

            tabSchedule.Columns.Add("PeriodNo");

            for (int i = 1; i <= MaxWeekDay; i++)
                tabSchedule.Columns.Add("" + i);

            foreach (LPViewReportItem Item in LPViewItems.Values)
            {
                DataRow Row = tabSchedule.NewRow();

                Row.SetField("PeriodNo", Item.PeriodNo);

                for (int i = 1; i <= MaxWeekDay; i++)
                {
                    string Value = Item.GetWeekDayText(i);
                    Row.SetField("" + i, Value);
                }

                tabSchedule.Rows.Add(Row);
            }

            return tabSchedule;
        }

        /// <summary>
        /// 取得單一時間表功課表
        /// </summary>
        /// <returns></returns>
        private DataTable GetTimeTableLPView()
        {
            Appointments appTests = null;
            Appointment appTest = null;
            string strEvtInfo = string.Empty;
            SortedDictionary<int, LPViewReportItem> LPViewItems = new SortedDictionary<int, LPViewReportItem>();
            DataTable tabSchedule = new DataTable("ScheduleDetail");

            #region 針對每筆時間表分段
            foreach (Period prdMember in ttCur.Periods)
            {
                //若節次不為0才繼續
                //if (prdMember.PeriodNo != 0)
                //{
                if (!LPViewItems.ContainsKey(int.Parse(prdMember.BeginTime.ToString("HHmm"))))
                {
                    //新增節次物件
                    LPViewReportItem itemPeriod = new LPViewReportItem();

                    //指定節次物件的節次
                    itemPeriod.PeriodNo = prdMember.PeriodNo;
                    itemPeriod.Time = " " + prdMember.BeginTime.ToString("HH：mm") +
                        System.Environment.NewLine + "|" +
                        System.Environment.NewLine + prdMember.BeginTime.AddMinutes(prdMember.Duration).ToString("HH：mm");

                    //將節次物件加到集合中
                    LPViewItems.Add(int.Parse(prdMember.BeginTime.ToString("HHmm")), itemPeriod);
                }

                //取得對應節次內容物件，並設定節次
                LPViewReportItem itemCurrent = LPViewItems[int.Parse(prdMember.BeginTime.ToString("HHmm"))];
                itemCurrent.PeriodNo = prdMember.PeriodNo;

                //根據Period檢查Appointments是否有對應的空閒時間
                appTests = apsCur.GetAppointments(
                    prdMember.WeekDay,
                    prdMember.BeginTime,
                    prdMember.Duration);

                if (prdMember.Disable == true)
                {
                    //itemCurrent.SetWeekDayText(prdMember.WeekDay, prdMember.DisableMessage);
                }
                else if (appTests.Count == 0)
                {
                    itemCurrent.SetWeekDayText(prdMember.WeekDay, string.Empty);
                }
                else if (appTests.Count == 1)
                {
                    appTest = appTests[0];

                    if (string.IsNullOrEmpty(appTest.EventID))
                    {
                        if (chkTeacherBusyDesc.Checked == true)
                            itemCurrent.SetWeekDayText(prdMember.WeekDay, appTest.Description);
                        else
                            itemCurrent.SetWeekDayText(prdMember.WeekDay, string.Empty);
                    }
                    else
                    {
                        if (GetTimeTableID(appTest.EventID) == ttCur.TimeTableID)
                        {
                            strEvtInfo = GetEventDisplayStr(appTest.EventID);
                            itemCurrent.SetWeekDayText(prdMember.WeekDay, strEvtInfo);
                        }
                        else //當資源的TimeTableID不等於Appointment的TimeTableID，那麼設定顯示名稱為Appointment的TimeTableID
                        {
                            strEvtInfo = GetEventDisplayStr(appTest.EventID);
                            itemCurrent.SetWeekDayText(prdMember.WeekDay, strEvtInfo);
                        }
                    }
                }
                else if (appTests.IsMultipleEvents())
                {
                    strEvtInfo = GetEventDisplayStr(appTests[0].EventID, appTests[1].EventID);
                    itemCurrent.SetWeekDayText(prdMember.WeekDay, strEvtInfo);
                }
                //}
            }
            #endregion

            int MaxWeekDay = ttCur.Periods.MaxWeekDay;

            tabSchedule.Columns.Add("PeriodNo");
            tabSchedule.Columns.Add("Time");

            for (int i = 1; i <= MaxWeekDay; i++)
                tabSchedule.Columns.Add("" + i);

            foreach (LPViewReportItem Item in LPViewItems.Values)
            {
                DataRow Row = tabSchedule.NewRow();

                Row.SetField("PeriodNo", Item.PeriodNo == 0 ? "中午" : "" + Item.PeriodNo);
                Row.SetField("Time", Item.Time);

                for (int i = 1; i <= MaxWeekDay; i++)
                {
                    string Value = Item.GetWeekDayText(i);
                    Row.SetField("" + i, Value);
                }

                tabSchedule.Rows.Add(Row);
            }

            return tabSchedule;
        }

        /// <summary>
        /// 列印單張報表
        /// </summary>
        private DataTable GetLPViewDetail()
        {
            DataTable tabSchedule = null;

            //取得合併時間表
            if (chkMergeTimeTable.Checked == true)
                tabSchedule = GetMergeLPView();
            else //取得單一時間表
                tabSchedule = GetTimeTableLPView();

            return tabSchedule;
        }
    }
}
