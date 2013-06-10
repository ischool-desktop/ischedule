using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using FISCA.DSAClient;
using ischedule.Properties;
using Sunset.Data;
using Sunset.Data.Integration;

namespace ischedule
{
    /// <summary>
    /// 排課主表單
    /// </summary>
    public partial class MainForm : Office2007RibbonForm
    {
        private Scheduler schLocal = Scheduler.Instance;
        public EventsViewBL TeacherEventsView { get; set; }
        public EventsViewBL ClassEventsView { get; set; }
        public EventsViewBL ClassroomEventsView { get; set; }
        private string Password = string.Empty;
        private string Filepath = string.Empty;
        public usrTeacherList TeacherList { get; set; }
        public usrClassList ClassList { get; set; }
        public usrClassroomList ClassroomList { get; set; }

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 關閉功課表頁籤
        /// </summary>
        /// <param name="vTabControl"></param>
        private void CloseLPView(DevComponents.DotNetBar.TabControl vTabControl)
        {
            List<TabItem> Items = new List<TabItem>();

            for (int i = 1; i < vTabControl.Tabs.Count; i++)
                Items.Add(vTabControl.Tabs[i]);

            Items.ForEach(x => vTabControl.Tabs.Remove(x));

            vTabControl.Tabs[0].AttachedControl.Controls.Clear();
            vTabControl.Tabs[0].Text = "功課表";
        }

        /// <summary>
        /// 載入表單
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            if (!System.IO.File.Exists(Path.Combine(System.Windows.Forms.Application.StartupPath, "開發模式")))
                Program.Update();

            this.Icon = Resources.ischedule_logo;

            #region 設定Aspose License
            Aspose.Cells.License License = new Aspose.Cells.License();

            FileStream Stream = new FileStream(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\Aspose.Total.lic", FileMode.Open);

            License.SetLicense(Stream);
            #endregion

            SetScheduleSourceCloseMenu();

            schLocal.ImportSourceComplete += (vsender, ve) => SetScheduleSourceOpenMenu();

            //測試OK
            btnClose.Click += (vsender, ve) =>
            {
                //關閉時將密碼及路徑清空
                Password = string.Empty;
                Filepath = string.Empty;

                //關閉左方資源列表
                pnlTeacher.Controls.Clear();
                pnlClass.Controls.Clear();
                pnlClassroom.Controls.Clear();

                //關閉分課表
                TeacherEventsView.Close();
                ClassEventsView.Close();
                ClassroomEventsView.Close();

                //關閉功課表
                CloseLPView(tabTeacherLPView);
                CloseLPView(tabClassLPView);
                CloseLPView(tabClassroomLPView);

                //關閉選單
                SetScheduleSourceCloseMenu();

                //關閉資料
                schLocal.Close();
            };

            //測試OK
            LeftNavigationPanel.NavigationBar.Visible = false;

            #region 當頁籤切換時的控制
            btnTeacher.Checked = true;
            btnExit.Click += (vsender, ve) => Application.Exit();
            ribbonTabTeacher.Click += (vsender, ve) =>
            {
                btnTeacher.Checked = true;
                tabContent.SelectedTab = tabTeacher;
            };
            ribbonTabClass.Click += (vsender, ve) =>
            {
                btnClass.Checked = true;
                tabContent.SelectedTab = tabClass;
            };
            ribbonTabClassroom.Click += (vsender, ve) =>
            {
                btnClassroom.Checked = true;
                tabContent.SelectedTab = tabClassroom;
            };
            #endregion

            this.TeacherEventsView = new EventsViewBL(Constants.lvWho, grdTeacherEvent, lblTeacher,
                btnTeacherAutoSchedule,
                btnTeacherLock,
                btnTeacherUnlock,
                btnTeacherFree,
                btnTeacherProperty,
                btnTeacherPrint);

            this.ClassEventsView = new EventsViewBL(Constants.lvWhom, grdClassEvent, lblClass,
                btnClassAutoSchedule,
                btnClassLock,
                btnClassUnLock,
                btnClassFree,
                btnClassProperty,
                btnClassPrint);

            this.ClassroomEventsView = new EventsViewBL(Constants.lvWhere, grdClassroomEvent, lblClassroom,
                btnClassroomAutoSchedule,
                btnClassroomLock,
                btnClassroomUnLock,
                btnClassroomFree,
                btnClassroomProperty,
                btnClassroomPrint);

            this.btnTeacherEventExpand.Click += (vsender, ve) =>
            {
                if (splTeacher.Expanded)
                {
                    //寫入紀錄(目前顯示欄位)
                    btnTeacherEventExpand.Text = "<<";
                    btnTeacherEventExpand.Tooltip = "還原";
                    splTeacher.Expanded = false;
                }
                else
                {
                    btnTeacherEventExpand.Text = ">>";
                    btnTeacherEventExpand.Tooltip = "最大化";
                    splTeacher.Expanded = true;
                }
            };

            this.btnClassEventExpand.Click += (vsender, ve) =>
            {
                if (splClass.Expanded)
                {
                    //寫入紀錄(目前顯示欄位)
                    btnClassEventExpand.Text = "<<";
                    btnClassEventExpand.Tooltip = "還原";
                    splClass.Expanded = false;
                }
                else
                {
                    btnClassEventExpand.Text = ">>";
                    btnClassEventExpand.Tooltip = "最大化";
                    splClass.Expanded = true;
                }
            };

            this.btnClassroomEventExpand.Click += (vsender, ve) =>
            {
                if (splClassroom.Expanded)
                {
                    //寫入紀錄(目前顯示欄位)
                    btnClassroomEventExpand.Text = "<<";
                    btnClassroomEventExpand.Tooltip = "還原";
                    splClassroom.Expanded = false;
                }
                else
                {
                    btnClassroomEventExpand.Text = ">>";
                    btnClassroomEventExpand.Tooltip = "最大化";
                    splClassroom.Expanded = true;
                }
            };

            ribbonTabTeacher.Checked = true;

            this.pnlWhoLPView.Width = 600;
            this.pnlWhomLPView.Width = 600;
            this.pnlWhereLPView.Width = 600;

            //ServerModule.AutoManaged("http://module.ischool.com.tw/module/89/ischedule/udm.xml");

            #region 測試用程式碼
            //schLocal.Open("C:\\101學年度第2學期0210.xml");

            //LoadResourceList();
            #endregion
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            frmLogin frmLogin = new frmLogin();

            if (frmLogin.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

            }
        }

        /// <summary>
        /// 開啟舊的排課檔案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenOld_Click(object sender, EventArgs e)
        {
            string vFilepath = string.Empty;

            #region 開啟檔案
            OpenFileDialog dlgCommonDialog = new OpenFileDialog();

            dlgCommonDialog.InitialDirectory = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            dlgCommonDialog.Filter = "(*.xml)|*.xml";

            DialogResult userClickedOK = dlgCommonDialog.ShowDialog();
            #endregion

            #region 判斷使用者是否按取消，或檔案長度為0則離開
            if (userClickedOK == System.Windows.Forms.DialogResult.Cancel)
                return;

            if (dlgCommonDialog.FileName.Length == 0)
                return;
            #endregion

            #region 實際開啟排課資料
            vFilepath = dlgCommonDialog.FileName;

            try
            {
                schLocal.Open(vFilepath);

                LoadResourceList();


            }
            catch (Exception ve)
            {
                MessageBox.Show(ve.Message);
            }
            #endregion
        }

        /// <summary>
        /// 開啟舊檔
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpen_Click(object sender, EventArgs e)
        {
            string vFilepath = string.Empty;

            #region 開啟檔案
            OpenFileDialog dlgCommonDialog = new OpenFileDialog();

            dlgCommonDialog.InitialDirectory = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            dlgCommonDialog.Filter = "(*.sch)|*.sch";

            DialogResult userClickedOK = dlgCommonDialog.ShowDialog();
            #endregion

            #region 判斷使用者是否按取消，或檔案長度為0則離開
            if (userClickedOK == System.Windows.Forms.DialogResult.Cancel)
                return;

            if (dlgCommonDialog.FileName.Length == 0)
                return;
            #endregion

            #region 實際開啟排課資料
            vFilepath = dlgCommonDialog.FileName;

            try
            {
                frmPasspordInput PasswordInput = new frmPasspordInput(string.Empty, "請輸入排課檔案開啟密碼",false);

                if (PasswordInput.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //開啟時將使用者的路徑及密碼儲存
                    Password = PasswordInput.Password;
                    Filepath = vFilepath;

                    schLocal.OpenByBase64(vFilepath,Password);

                    LoadResourceList();
                }
            }
            catch (Exception ve)
            {
                MessageBox.Show(ve.Message);
            }
            #endregion
        }

        /// <summary>
        /// 下載資料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDownload_Click(object sender, EventArgs e)
        {
            Download();
        }

        /// <summary>
        /// 上傳資料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpload_Click(object sender, EventArgs e)
        {
            Upload();
        }

        /// <summary>
        /// 取得使用者選取的輸入路徑
        /// </summary>
        /// <returns></returns>
        private string GetUserInputFilePath()
        {
            #region 開啟檔案
            SaveFileDialog dlgCommonDialog = new SaveFileDialog();

            dlgCommonDialog.InitialDirectory = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            dlgCommonDialog.Filter = "(*.sch)|*.sch";
            dlgCommonDialog.FileName = "ischedule_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".sch";

            DialogResult userClickedOK = dlgCommonDialog.ShowDialog();
            #endregion

            #region 判斷使用者是否按取消，或檔案長度為0則離開
            if (userClickedOK == DialogResult.Cancel)
                return string.Empty;

            if (dlgCommonDialog.FileName.Length == 0)
                return string.Empty;
            #endregion

            return dlgCommonDialog.FileName;
        }

        /// <summary>
        /// 儲存檔案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            //若預設儲存路徑不存在，則讓使用者選取路徑
            if (string.IsNullOrEmpty(Filepath))
            {
                Filepath = GetUserInputFilePath();

                if (string.IsNullOrEmpty(Filepath))
                    return;
            }

            //若密碼不存在，則讓使用者輸入密碼；若密碼沒有值，則將路徑清空
            if (string.IsNullOrEmpty(Password))
            {
                frmPasspordInput PasswordInput = new frmPasspordInput(Password, "請設定排課檔案開啟密碼",true);

                if (PasswordInput.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    Password = PasswordInput.Password;
                else
                    Filepath = string.Empty;
            }

            //若路徑及密碼不為空白才儲存檔案
            if (!string.IsNullOrEmpty(Filepath) && !string.IsNullOrEmpty(Password))
                schLocal.SaveByBase64(Filepath, Password);            
        }

        /// <summary>
        /// 另存新檔
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            //取得使用者輸入路徑
            Filepath = GetUserInputFilePath();

            //若路徑為空白則跳出
            if (string.IsNullOrEmpty(Filepath))
                return;

            #region 讓使用者輸入密碼確認存檔
            frmPasspordInput PasswordInput = new frmPasspordInput(Password, "請設定排課檔案開啟密碼",true);

            if (PasswordInput.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Password = PasswordInput.Password;
                schLocal.SaveByBase64(Filepath, Password);
            }
            #endregion
        }

        #region private method
        /// <summary>
        /// 載入資源列表
        /// </summary>
        private void LoadResourceList()
        {
            TeacherList = new usrTeacherList();
            TeacherList.Dock = DockStyle.Fill;
            pnlTeacher.Controls.Add(TeacherList);

            ClassList = new usrClassList();
            ClassList.Dock = DockStyle.Fill;
            pnlClass.Controls.Add(ClassList);


            ClassroomList = new usrClassroomList();
            ClassroomList.Dock = DockStyle.Fill;
            pnlClassroom.Controls.Add(ClassroomList);
        }

        /// <summary>
        /// 設定當排課資料關閉時的選單
        /// </summary>
        private void SetScheduleSourceCloseMenu()
        {
            btnOpen.Enabled = true;
            btnOpenOld.Enabled = true;
            btnSave.Enabled = false;
            btnSaveAs.Enabled = false;
            btnDownload.Enabled = true;
            btnUpload.Enabled = false;
            btnClose.Enabled = false;
            btnExit.Enabled = true;

            btnTeacherAutoSchedule.Enabled = false;
            btnTeacherLock.Enabled = false;
            btnTeacherUnlock.Enabled = false;
            btnTeacherFree.Enabled = false;
            btnTeacherProperty.Enabled = false;
            btnTeacherBusy.Enabled = false;
            btnTeacherPrint.Enabled = false;

            btnClassAutoSchedule.Enabled = false;
            btnClassLock.Enabled = false;
            btnClassUnLock.Enabled = false;
            btnClassFree.Enabled = false;
            btnClassProperty.Enabled = false;
            btnClassBusy.Enabled = false;
            btnClassPrint.Enabled = false;

            btnClassroomAutoSchedule.Enabled = false;
            btnClassroomLock.Enabled = false;
            btnClassroomUnLock.Enabled = false;
            btnClassroomFree.Enabled = false;
            btnClassroomProperty.Enabled = false;
            btnClassroomBusy.Enabled = false;
            btnClassroomPrint.Enabled = false;
        }

        /// <summary>
        /// 設定當排課資料開啟時的選單
        /// </summary>
        private void SetScheduleSourceOpenMenu()
        {
            btnOpen.Enabled = false;
            btnOpenOld.Enabled = false;
            btnSave.Enabled = true;
            btnSaveAs.Enabled = true;
            btnDownload.Enabled = false;
            btnUpload.Enabled = true;
            btnClose.Enabled = true;
            btnExit.Enabled = true;

            btnTeacherPrint.Enabled = true;
            btnClassPrint.Enabled = true;
            btnClassroomPrint.Enabled = true;

            btnTeacherBusy.Enabled = true;
            btnClassBusy.Enabled = true;
            btnClassroomBusy.Enabled = true;
        }
        #endregion

        #region public method

        /// <summary>
        /// 取得目前選取的類別
        /// </summary>
        /// <returns></returns>
        public string GetSelectedType()
        {
            if (ribbonTabTeacher.Checked)
                return "Teacher";
            else if (ribbonTabClass.Checked)
                return "Class";
            else if (ribbonTabClassroom.Checked)
                return "Classroom";
            else
                return string.Empty;
        }

        /// <summary>
        /// 設定標題
        /// </summary>
        /// <param name="Text"></param>
        public void SetTitle(string Text)
        {
            ribbonControl1.TitleText = Text;
        }

        /// <summary>
        /// 上傳資料
        /// </summary>
        public void Upload()
        {
            try
            {
                if (!Global.IsValidatePassport())
                {
                    frmLogin frmLogin = new frmLogin();

                    if (frmLogin.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                        return;
                }

                frmUploadConfirm frmUploadConfirm = new frmUploadConfirm();

                List<string> DSNSNames = SchedulerSource.Source.DSNSNames;

                if (frmUploadConfirm.TestConnection(DSNSNames) == DialogResult.OK)
                {
                    Tuple<bool, string> UploadResult = schLocal.Upload(frmUploadConfirm.Connections);

                    if (UploadResult.Item1)
                    {
                        MessageBox.Show(UploadResult.Item2, "上傳成功!");
                    }
                    else
                    {
                        MessageBox.Show(UploadResult.Item2, "上傳失敗= =");
                    }
                }
            }
            catch (Exception ve)
            {
                MessageBox.Show(ve.Message);
            } 
        }

        /// <summary>
        /// 下載資料
        /// </summary>
        public void Download()
        {
            if (!Global.IsValidatePassport())
            {
                frmLogin frmLogin = new frmLogin();

                if (frmLogin.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                    return;
            }

            frmTestConnection frmTestConnection = new frmTestConnection();

            if (frmTestConnection.TestConnection() == System.Windows.Forms.DialogResult.OK)
            {
                List<Connection> Connections = frmTestConnection.Connections;
                string SchoolYear = frmTestConnection.SchoolYear;
                string Semester = frmTestConnection.Semester;

                try
                {
                    schLocal.Download(Connections, SchoolYear, Semester);

                    frmDownloadConfirm frmDownloadConfirm = new frmDownloadConfirm();

                    if (frmDownloadConfirm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        schLocal.Import();

                        string SaveFilename = "ischedule_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".sch";

                        schLocal.SaveByBase64(SaveFilename,"1234");

                        LoadResourceList();
                    }
                    else
                        SchedulerSource.Source.Close();
                }
                catch (Exception ve)
                {
                    MessageBox.Show(ve.Message);
                }
            }
        }
        #endregion

        #region 分課表及功課表相關操作
        public void SetCommandButtonEnable(bool Undo, bool Redo)
        {
            this.btnUndo.Enabled = Undo;
            this.btnRedo.Enabled = Redo;
        }

        /// <summary>
        /// 開啟教師分課表
        /// </summary>
        /// <param name="AssocType"></param>
        /// <param name="AssocID"></param>
        /// <param name="IsOpenNewLPView"></param>
        public void OpenTeacherEventsView(int AssocType, string AssocID ,string AssocText)
        {
            Stopwatch watch = new Stopwatch();

            watch.Start();

            TeacherEventsView.SetAssocObject(AssocType, AssocID,AssocText);

            watch.Stop();

            Console.WriteLine("" + watch.Elapsed.TotalSeconds);
        }

        /// <summary>
        /// 開啟班級分課表
        /// </summary>
        /// <param name="AssocType"></param>
        /// <param name="AssocID"></param>
        /// <param name="AssocText"></param>
        public void OpenClassEventsView(int AssocType, string AssocID, string AssocText)
        {
            Stopwatch watch = new Stopwatch();

            watch.Start();

            ClassEventsView.SetAssocObject(AssocType, AssocID, AssocText);

            watch.Stop();

            Console.WriteLine("" + watch.Elapsed.TotalSeconds); 
        }

        /// <summary>
        /// 開啟場地分課表
        /// </summary>
        /// <param name="AssocType"></param>
        /// <param name="AssocID"></param>
        /// <param name="AssocText"></param>
        public void OpenClassroomEventsView(int AssocType, string AssocID, string AssocText)
        {
            Stopwatch watch = new Stopwatch();

            watch.Start();

            ClassroomEventsView.SetAssocObject(AssocType, AssocID, AssocText);

            watch.Stop();

            Console.WriteLine("" + watch.Elapsed.TotalSeconds);
        }

        /// <summary>
        /// 清除教師預設功課表
        /// </summary>
        public void ClearTeacherDefaultSchedule()
        {
            tabTeacherLPView.Tabs[0].Text = "功課表";
            tabTeacherLPView.Tabs[0].AttachedControl.Controls.Clear();
        }

        /// <summary>
        /// 清除班級預設功課表
        /// </summary>
        public void ClearClassDefaultSchedule()
        {
            tabClassLPView.Tabs[0].Text = "功課表";
            tabClassLPView.Tabs[0].AttachedControl.Controls.Clear();
        }

        /// <summary>
        /// 清除場地預設功課表
        /// </summary>
        public void ClearClassroomDefaultSchedule()
        {
            tabClassroomLPView.Tabs[0].Text = "功課表";
            tabClassroomLPView.Tabs[0].AttachedControl.Controls.Clear();
        }

        /// <summary>
        /// 根據資源類別及資源系統編號開啟功課表
        /// </summary>
        /// <param name="AssocType"></param>
        /// <param name="AssocID"></param>
        public DecScheduler OpenTeacherSchedule(int AssocType, 
            string AssocID, 
            bool OpenNew=false,
            string SyncTimeTableEventID = "",
            bool SetTestEvent=false)
        {
            TabItem SelectedTab = null;
            TabControlPanel SelectedPanel = null;
            string AssocName = string.Empty;
            DecScheduler LPView = null;

            //假設不是開新分頁，都指到第一頁。
            if (!OpenNew)
            {
                SelectedTab = tabTeacherLPView.Tabs[0];
                SelectedPanel = SelectedTab.AttachedControl as TabControlPanel;
                tabTeacherLPView.SelectedTab = SelectedTab;
            }
            else
            {
                SelectedTab = tabTeacherLPView.CreateTab(AssocID);
                SelectedTab.CloseButtonVisible = true;
                tabTeacherLPView.SelectedTab = SelectedTab;
                SelectedPanel = tabTeacherLPView.SelectedPanel;
            }

            //1.沒有要開新分頁。
            //2.就不開啟
            if (!OpenNew && SelectedTab.Text.Equals(AssocID))
            {
                //判斷是否要同步事件
                if (SelectedPanel != null)
                {
                    LPView = SelectedPanel.Tag as DecScheduler;

                    if (!string.IsNullOrEmpty(SyncTimeTableEventID))
                    {
                        if (SetTestEvent)
                            LPView.SelectTestEvent(SyncTimeTableEventID);
                        else
                        {
                            string EventTimeTableID = schLocal.CEvents[SyncTimeTableEventID].TimeTableID;

                            string LPViewTimeTableID = LPView.CurrentTimeTableID;

                            if (!EventTimeTableID.Equals(LPViewTimeTableID))
                                LPView.SyncTimeTable(SyncTimeTableEventID);
                        }
                    }
                    else
                        LPView.SelectTestEvent(string.Empty);

                    return LPView;
                }
                else
                    return null;
            }

            List<CEvent> RelatedEvents = null;

            List<string> TimeTableIDs = new List<string>();

            #region 取得相關事件列表
            switch (AssocType)
            {
                case Constants.lvWho:
                    if (string.IsNullOrEmpty(AssocID)) return null;
                    RelatedEvents = schLocal.CEvents.GetEventsByTeacherID(AssocID);
                    AssocName = schLocal.Teachers[AssocID].Name;
                    break;
                case Constants.lvWhom:
                    if (string.IsNullOrEmpty(AssocID)) return null;
                    RelatedEvents = schLocal.CEvents.GetEventsByClassID(AssocID);
                    AssocName = schLocal.Classes[AssocID].Name;
                    break;
                case Constants.lvWhere:
                    if (string.IsNullOrEmpty(AssocID) || schLocal.Classrooms[AssocID].LocOnly) return null;
                    RelatedEvents = schLocal.CEvents.GetEventsByClassroomID(AssocID);
                    AssocName = schLocal.Classrooms[AssocID].Name;
                    break;
            }
            #endregion

            if (SelectedPanel.Tag == null)
            {
                LPView = new DecScheduler(SelectedPanel);
                SelectedPanel.Tag = LPView;
            }

            SelectedTab.Text = AssocName;

            //取得功課表
            LPView = SelectedPanel.Tag as DecScheduler;

            //設定相關資源
            LPView.SetAssocObject(AssocType, AssocID);

            if (!string.IsNullOrEmpty(SyncTimeTableEventID))
            {
                if (SetTestEvent)
                    LPView.SelectTestEvent(SyncTimeTableEventID);
                else
                    LPView.SyncTimeTable(SyncTimeTableEventID);
            }
            else if (RelatedEvents.Count > 0)
                LPView.SyncTimeTable(RelatedEvents[0].EventID);
            else //當沒有分課時就把內容清空
                SelectedPanel.Controls.Clear();

            return LPView;
        }

        /// <summary>
        /// 根據資源類別及資源系統編號開啟功課表
        /// </summary>
        /// <param name="AssocType"></param>
        /// <param name="AssocID"></param>
        public DecScheduler OpenClassSchedule(int AssocType,
            string AssocID,
            bool OpenNew = false,
            string SyncTimeTableEventID = "",
            bool SetTestEvent = false)
        {
            TabItem SelectedTab = null;
            TabControlPanel SelectedPanel = null;
            string AssocName = string.Empty;
            DecScheduler LPView = null;

            //假設不是開新分頁，都指到第一頁。
            if (!OpenNew)
            {
                SelectedTab = tabClassLPView.Tabs[0];
                SelectedPanel = SelectedTab.AttachedControl as TabControlPanel;
                tabClassLPView.SelectedTab = SelectedTab;
            }
            else
            {
                SelectedTab = tabClassLPView.CreateTab(AssocID);
                SelectedTab.CloseButtonVisible = true;
                tabClassLPView.SelectedTab = SelectedTab;
                SelectedPanel = tabClassLPView.SelectedPanel;
            }

            //1.沒有要開新分頁。
            //2.就不開啟
            if (!OpenNew && SelectedTab.Text.Equals(AssocID))
            {
                //判斷是否要同步事件
                if (SelectedPanel != null)
                {
                    LPView = SelectedPanel.Tag as DecScheduler;

                    if (!string.IsNullOrEmpty(SyncTimeTableEventID))
                    {
                        if (SetTestEvent)
                            LPView.SelectTestEvent(SyncTimeTableEventID);
                        else
                        {
                            string EventTimeTableID = schLocal.CEvents[SyncTimeTableEventID].TimeTableID;

                            string LPViewTimeTableID = LPView.CurrentTimeTableID;

                            if (!EventTimeTableID.Equals(LPViewTimeTableID))
                                LPView.SyncTimeTable(SyncTimeTableEventID);
                        }
                    }
                    else
                        LPView.SelectTestEvent(string.Empty);

                    return LPView;
                }
                else
                    return null;
            }

            List<CEvent> RelatedEvents = null;

            List<string> TimeTableIDs = new List<string>();

            #region 取得相關事件列表
            switch (AssocType)
            {
                case Constants.lvWho:
                    if (string.IsNullOrEmpty(AssocID)) return null;
                    RelatedEvents = schLocal.CEvents.GetEventsByTeacherID(AssocID);
                    AssocName = schLocal.Teachers[AssocID].Name;
                    break;
                case Constants.lvWhom:
                    if (string.IsNullOrEmpty(AssocID)) return null;
                    RelatedEvents = schLocal.CEvents.GetEventsByClassID(AssocID);
                    AssocName = schLocal.Classes[AssocID].Name;
                    break;
                case Constants.lvWhere:
                    if (string.IsNullOrEmpty(AssocID) || schLocal.Classrooms[AssocID].LocOnly) return null;
                    RelatedEvents = schLocal.CEvents.GetEventsByClassroomID(AssocID);
                    AssocName = schLocal.Classrooms[AssocID].Name;
                    break;
            }
            #endregion

            if (SelectedPanel.Tag == null)
            {
                LPView = new DecScheduler(SelectedPanel);
                SelectedPanel.Tag = LPView;
            }

            SelectedTab.Text = AssocName;

            //取得功課表
            LPView = SelectedPanel.Tag as DecScheduler;

            //設定相關資源
            LPView.SetAssocObject(AssocType, AssocID);

            if (!string.IsNullOrEmpty(SyncTimeTableEventID))
            {
                if (SetTestEvent)
                    LPView.SelectTestEvent(SyncTimeTableEventID);
                else
                    LPView.SyncTimeTable(SyncTimeTableEventID);
            }
            else if (RelatedEvents.Count > 0)
                LPView.SyncTimeTable(RelatedEvents[0].EventID);
            else //當沒有分課時就把內容清空
                SelectedPanel.Controls.Clear();

            return LPView;
        }

        /// <summary>
        /// 根據資源類別及資源系統編號開啟功課表
        /// </summary>
        /// <param name="AssocType"></param>
        /// <param name="AssocID"></param>
        public DecScheduler OpenClassroomSchedule(int AssocType, 
            string AssocID, 
            bool OpenNew=false,
            string SyncTimeTableEventID = "",
            bool SetTestEvent=false)
        {
            TabItem SelectedTab = null;
            TabControlPanel SelectedPanel = null;
            string AssocName = string.Empty;
            DecScheduler LPView = null;

            //假設不是開新分頁，都指到第一頁。
            if (!OpenNew)
            {
                SelectedTab = tabClassroomLPView.Tabs[0];
                SelectedPanel = SelectedTab.AttachedControl as TabControlPanel;
                tabClassroomLPView.SelectedTab = SelectedTab;
            }
            else
            {
                SelectedTab = tabClassroomLPView.CreateTab(AssocID);
                SelectedTab.CloseButtonVisible = true;
                tabClassroomLPView.SelectedTab = SelectedTab;
                SelectedPanel = tabClassroomLPView.SelectedPanel;
            }

            //1.沒有要開新分頁。
            //2.就不開啟
            if (!OpenNew && SelectedTab.Text.Equals(AssocID))
            {
                //判斷是否要同步事件
                if (SelectedPanel != null)
                {
                    LPView = SelectedPanel.Tag as DecScheduler;

                    if (!string.IsNullOrEmpty(SyncTimeTableEventID))
                    {
                        if (SetTestEvent)
                            LPView.SelectTestEvent(SyncTimeTableEventID);
                        else
                        {
                            string EventTimeTableID = schLocal.CEvents[SyncTimeTableEventID].TimeTableID;

                            string LPViewTimeTableID = LPView.CurrentTimeTableID;

                            if (!EventTimeTableID.Equals(LPViewTimeTableID))
                                LPView.SyncTimeTable(SyncTimeTableEventID);
                        }
                    }
                    else
                        LPView.SelectTestEvent(string.Empty);

                    return LPView;
                }
                else
                    return null;
            }

            List<CEvent> RelatedEvents = null;

            List<string> TimeTableIDs = new List<string>();

            #region 取得相關事件列表
            switch (AssocType)
            {
                case Constants.lvWho:
                    if (string.IsNullOrEmpty(AssocID)) return null;
                    RelatedEvents = schLocal.CEvents.GetEventsByTeacherID(AssocID);
                    AssocName = schLocal.Teachers[AssocID].Name;
                    break;
                case Constants.lvWhom:
                    if (string.IsNullOrEmpty(AssocID)) return null;
                    RelatedEvents = schLocal.CEvents.GetEventsByClassID(AssocID);
                    AssocName = schLocal.Classes[AssocID].Name;
                    break;
                case Constants.lvWhere:
                    if (string.IsNullOrEmpty(AssocID) || !schLocal.Classrooms.Exists(AssocID))
                        return null;
                    if (schLocal.Classrooms[AssocID].LocOnly) return null;
                    RelatedEvents = schLocal.CEvents.GetEventsByClassroomID(AssocID);
                    AssocName = schLocal.Classrooms[AssocID].Name;
                    break;
            }
            #endregion

            if (SelectedPanel.Tag == null)
            {
                LPView = new DecScheduler(SelectedPanel);
                SelectedPanel.Tag = LPView;
            }

            SelectedTab.Text = AssocName;

            //取得功課表
            LPView = SelectedPanel.Tag as DecScheduler;

            //設定相關資源
            LPView.SetAssocObject(AssocType, AssocID);

            if (!string.IsNullOrEmpty(SyncTimeTableEventID))
            {
                if (SetTestEvent)
                    LPView.SelectTestEvent(SyncTimeTableEventID);
                else
                    LPView.SyncTimeTable(SyncTimeTableEventID);
            }
            else if (RelatedEvents.Count > 0)
                LPView.SyncTimeTable(RelatedEvents[0].EventID);
            else //當沒有分課時就把內容清空
                SelectedPanel.Controls.Clear();

            return LPView;
        }
        #endregion

        /// <summary>
        /// 教師不排課時段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTeacherBusy_Click(object sender, EventArgs e)
        {
            TeacherEditor vTeacherEditor = new TeacherEditor();
            TeacherPackageDataAccess vTeacherDataAccess = new TeacherPackageDataAccess();
            winConfiguration<TeacherPackage> configTeacher = new winConfiguration<TeacherPackage>(vTeacherDataAccess, vTeacherEditor);

            vTeacherEditor.Prepare();
            configTeacher.ShowDialog();
        }

        /// <summary>
        /// 班級不排課時段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClassBusy_Click(object sender, EventArgs e)
        {
            ClassEditor vClassEditor = new ClassEditor();
            ClassPackageDataAccess vTeacherDataAccess = new ClassPackageDataAccess();
            winConfiguration<ClassPackage> configTeacher = new winConfiguration<ClassPackage>(vTeacherDataAccess, vClassEditor);

            vClassEditor.Prepare();
            configTeacher.ShowDialog();
        }

        /// <summary>
        /// 場地不排課時段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClassroomBusy_Click(object sender, EventArgs e)
        {
            ClassroomEditor vClassroomEditor = new ClassroomEditor();
            ClassroomPackageDataAccess vClassroomDataAccess = new ClassroomPackageDataAccess();
            winConfiguration<ClassroomPackage> configClassroom = new winConfiguration<ClassroomPackage>(vClassroomDataAccess, vClassroomEditor);

            vClassroomEditor.Prepare();
            configClassroom.ShowDialog();
        }

        /// <summary>
        /// 回復
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUndo_Click(object sender, EventArgs e)
        {
            MainFormBL.Undo();
        }

        /// <summary>
        /// 重做
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRedo_Click(object sender, EventArgs e)
        {
            MainFormBL.Redo();
        }
    }
}