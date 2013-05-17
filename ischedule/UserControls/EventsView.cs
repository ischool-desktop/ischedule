using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using Sunset.Data;

namespace ischedule
{
    /// <summary>
    /// 處理分課表 Grid 的操作邏輯
    /// </summary>
    public class EventsViewBL
    {
        private const int colLock = 0;
        private const int colSolutionCount = 1;
        private const int colWeekday = 2;
        private const int colPeriod = 3;
        private const int colWhoName = 4;
        private const int colWhomName = 5;
        private const int colWhereName = 6;
        private const int colWhatName = 7;
        private const int colWhatAliasName = 8;
        private const int colCourseName = 9;
        private const int colCourseGroup = 10;
        private const int colLength = 11;
        private const int colWeekDayCondition = 12;
        private const int colPeriodCondition = 13;
        private const int colAllowLongBreak = 14;
        private const int colAllowDuplicate = 15;
        private const int colLimitNextDay = 16;
        private const int colWeekFlag = 17;
        private const int colPriority = 18;
        private const int colTimeTable = 19;
        private const int colColorIndex = 20;
        private const int colEventID = 21;

        #region 分課表相關變數
        private int mType;    //Associate object type
        private List<string> mObjIDs = new List<string>();  //Private mObjID As Long
        private int mLPViewType;
        private CEvents evtsCustom = new CEvents();
        #endregion

        #region 分課表資源類別
        private const int evAll = 0;
        private const int evWho = 1;
        private const int evWhom = 2;
        private const int evWhere = 3;
        private const int evWhat = 4;
        private const int evCustom = 5;
        #endregion

        #region 更新分課表內容動作
        public enum RCActions
        {
            rcRefresh = 1,
            rcInsert = 2,
            rcRemove = 3,
            rcSolutionCount = 4
        };
        #endregion

        private CEventBindingList evtsTransfers = new CEventBindingList();
        private CEvents evtsTemp = new CEvents();
        private Scheduler schLocal = Scheduler.Instance;
        private DataGridViewX grdEvents;
        private LabelX lblTitle;
        private frmProgress frmASProgress = new frmProgress();
        private Stopwatch mStopwatch = new Stopwatch();
        private bool IsSelectionChanged = false;
        private ButtonItem btnAutoSchedule;
        private ButtonItem btnLock;
        private ButtonItem btnUnLock;
        private ButtonItem btnFree;
        private ButtonItem btnProperty;
        private ButtonItem btnPrint;

        #region TempWhoUpdate
        /// <summary>
        /// 待處理更新參數
        /// </summary>
        public class TempUpdateEventArgs : EventArgs
        {
            public int TotCount { get; set; }

            public TempUpdateEventArgs(int nTotCount)
            {
                this.TotCount = nTotCount;
            }
        }

        /// <summary>
        /// 待處理更新事件
        /// </summary>
        public event EventHandler<TempUpdateEventArgs> TempUpdate;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public void Close()
        {
            this.mObjIDs.Clear();
            this.evtsCustom.Clear();
            this.evtsTemp.Clear();
            this.grdEvents.DataSource = null;
            this.lblTitle.Text = string.Empty;
        }

        /// <summary>
        /// 建構式，傳入分課表實體
        /// </summary>
        /// <param name="grdEvents"></param>
        public EventsViewBL(int LPViewType,DataGridViewX grdEvents,LabelX lblTitle,
            ButtonItem btnAutoSchedule,
            ButtonItem btnLock,
            ButtonItem btnUnLock,
            ButtonItem btnFree,
            ButtonItem btnProperty,
            ButtonItem btnPrint)
        {
            if (grdEvents == null || lblTitle==null)
                throw new Exception("DataGrid及LabelX不得為null");

            this.grdEvents = grdEvents;
            this.grdEvents.AutoGenerateColumns = false;
            this.lblTitle = lblTitle;
            this.btnAutoSchedule = btnAutoSchedule;
            this.btnLock = btnLock;
            this.btnUnLock = btnUnLock;
            this.btnFree = btnFree;
            this.btnProperty = btnProperty;
            this.btnPrint = btnPrint;
            this.mLPViewType = LPViewType;

            this.btnAutoSchedule.Click -= new EventHandler(btnAutoSchedule_Click);
            this.btnLock.Click -= new EventHandler(btnLock_Click);
            this.btnUnLock.Click -= new EventHandler(btnUnLock_Click);
            this.btnFree.Click -= new EventHandler(btnFree_Click);
            this.btnProperty.Click -= new EventHandler(btnProperty_Click);
            this.btnPrint.Click -= new EventHandler(btnPrint_Click);

            this.grdEvents.CellFormatting -= new DataGridViewCellFormattingEventHandler(grdEvents_CellFormatting);
            this.grdEvents.SelectionChanged -= new EventHandler(grdEvents_SelectionChanged);

            this.btnAutoSchedule.Click += new EventHandler(btnAutoSchedule_Click);
            this.btnLock.Click += new EventHandler(btnLock_Click);
            this.btnUnLock.Click += new EventHandler(btnUnLock_Click);
            this.btnFree.Click += new EventHandler(btnFree_Click);
            this.btnProperty.Click += new EventHandler(btnProperty_Click);
            this.btnPrint.Click += new EventHandler(btnPrint_Click);

            this.grdEvents.CellFormatting += new DataGridViewCellFormattingEventHandler(grdEvents_CellFormatting);
            this.grdEvents.SelectionChanged += new EventHandler(grdEvents_SelectionChanged);

            //當自動排課完成時更新
            schLocal.AutoScheduleComplete += (sender, e) =>
            {
                if (frmASProgress != null)
                    frmASProgress.ChangeProgress(e.EventList.Count);

                RefreshEvents(RCActions.rcRefresh, e.EventList);
            };

            //當自動排課中顯示進度
            schLocal.AutoScheduleProgress += (sender, e) =>
            {
                if (frmASProgress != null)
                {
                    frmASProgress.ChangeProgress(e.nCurIndex);
                    e.Cancel = frmASProgress.UserAbort;
                }

                #region VB
                //    DoEvents
                //    If Not (frmASProgress Is Nothing) Then
                //        frmASProgress.ChangeProgress nCurIndex
                //        Cancel = frmASProgress.UserAbort
                //    End If
                #endregion
            };

            //當分課表移除事件時更新畫面
            schLocal.EventBeforeDelete += (sender, e) => RemoveEvent(e.EventID);

            //當新增分課表時更新畫面
            schLocal.EventInserted += (sender, e) => InsertEvent(e.EventID);

            //當分課表屬性改變更前移除分課表的分課
            schLocal.EventPropertyBeforeChange += (sender, e) =>
            {
                if (IsRelatedEvent(e.EventID))
                {
                    switch (mType)
                    {
                        case evWho:
                            if ((e.ChangeFlag & MaskOptions.maskWho) == MaskOptions.maskWho)
                                RemoveEvent(e.EventID);
                            break;
                        case evWhere:
                            if ((e.ChangeFlag & MaskOptions.maskWhere) == MaskOptions.maskWhere)
                                RemoveEvent(e.EventID);
                            break;
                    }
                }
            };

            //當分課表屬性改變後新增分課表分課
            schLocal.EventPropertyChanged += (sender, e) =>
            {
                if (IsRelatedEvent(e.EventID))
                {
                    switch (mType)
                    {
                        case evWho:
                            if ((e.ChangeFlag & MaskOptions.maskWho) == MaskOptions.maskWho)
                                InsertEvent(e.EventID);
                            break;
                        case evWhere:
                            if ((e.ChangeFlag & MaskOptions.maskWhere) == MaskOptions.maskWhere)
                                InsertEvent(e.EventID);
                            break;
                    }
                    RefreshEvent(e.EventID);
                }
            };

            //當分課表已被排定後更新畫面
            schLocal.EventScheduled += (sender, e) =>
            {
                if (IsRelatedEvent(e.EventID))
                    RefreshEvent(e.EventID);
            };

            //當分課表已被釋被時更新畫面
            schLocal.EventsFreed += (sender, e) =>
            {
                RefreshEvents(RCActions.rcRefresh, e.EventList);
            };

            //當計算好解決方案更新畫面
            schLocal.EventSolCountUpdated += (sender, e) =>
            {
                RefreshEvents(RCActions.rcSolutionCount, e.Eventlist);
            };

            //當分課表鎖定時更新畫面
            schLocal.EventLocked += (sender, e) =>
            {
                if (IsRelatedEvent(e.EventID))
                    RefreshEvent(e.EventID);
            };

            //當分課表解鎖定時更新畫面
            schLocal.EventUnlocked += (sender, e) =>
            {
                if (IsRelatedEvent(e.EventID))
                    RefreshEvent(e.EventID);
            };
        }

        void grdEvents_SelectionChanged(object sender, EventArgs e)
        {
            if (!IsSelectionChanged)
                return;

            this.btnAutoSchedule.Enabled = grdEvents.SelectedRows.Count > 0;
            this.btnLock.Enabled = grdEvents.SelectedRows.Count > 0;
            this.btnUnLock.Enabled = grdEvents.SelectedRows.Count > 0;
            this.btnFree.Enabled = grdEvents.SelectedRows.Count > 0;
            this.btnProperty.Enabled = grdEvents.SelectedRows.Count == 1;
            this.btnPrint.Enabled = grdEvents.SelectedRows.Count > 0;

            //當選取分課為一門時進行作業
            if (grdEvents.SelectedRows.Count == 1)
            {
                CEvent EventTransfer = grdEvents.SelectedRows[0].DataBoundItem as CEvent;

                CEvent Event = schLocal.CEvents[EventTransfer.EventID];

                btnProperty.Enabled = Event.WeekDay == 0;

                if (Event != null)
                {
                    //判斷目前是屬於哪個資源，教師、班級或是場地
                    switch (mLPViewType)
                    {
                        case Constants.lvWho:
                            MainFormBL.Instance.OpenTeacherSchedule(
                            Constants.lvWho,
                            Event.TeacherID1,
                            false,
                            Event.EventID, true);
                            break;
                        case Constants.lvWhom:
                            MainFormBL.Instance.OpenClassSchedule(
                                Constants.lvWhom,
                                Event.ClassID,
                                false,
                                Event.EventID, true);
                            break;
                        case Constants.lvWhere:
                            MainFormBL.Instance.OpenClassroomSchedule(
                                Constants.lvWhere,
                                Event.ClassroomID,
                                false,
                                Event.EventID, true);
                            break;
                    }
                }
            }
        }

        void grdEvents_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridViewX dgv = sender as DataGridViewX;
            CEvent evtTransfer = dgv.Rows[e.RowIndex].DataBoundItem as CEvent;

            if (evtTransfer == null)
                return;

            if (evtTransfer.ColorIndex == 0)
            {
                e.CellStyle.BackColor = Color.White;
                e.CellStyle.ForeColor = Color.Black;
            }
            else if (evtTransfer.ColorIndex == 1)
            {
                e.CellStyle.BackColor = Color.Red;
                e.CellStyle.ForeColor = Color.White;
                e.CellStyle.SelectionForeColor = Color.Red;
            }
            else if (evtTransfer.ColorIndex == 2)
            {
                e.CellStyle.BackColor = Color.FromArgb(254, 252, 128);
                e.CellStyle.ForeColor = Color.Black;
            }
            else if (evtTransfer.ColorIndex == 3)
            {
                e.CellStyle.BackColor = Color.White;
                e.CellStyle.ForeColor = Color.Black;
            }
        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            Print();
        }

        void btnProperty_Click(object sender, EventArgs e)
        {
            ChangeProperty();
        }

        void btnFree_Click(object sender, EventArgs e)
        {
            FreeEvents();
        }

        void btnUnLock_Click(object sender, EventArgs e)
        {
            UnLockEvents();
        }

        void btnLock_Click(object sender, EventArgs e)
        {
            LockEvents();
        }

        void btnAutoSchedule_Click(object sender, EventArgs e)
        {
            AutoSchedule();
        }

        #region Methods
        /// <summary>
        /// 設定分課表所屬的資源型態及資源編號並依此設定分課表內容。
        /// </summary>
        /// <param name="AssocType">資源型態，分為evAll、evWho、evWhom、evWhere、evWhat、evCustom。</param>
        /// <param name="AssocID">資源編號，若是evWho則其值為教師編號。</param>
        public void SetAssocObject(int AssocType, string AssocID,string AssocText)
        {
            lblTitle.Text = "資料準備中...";
            lblTitle.Refresh();

            mType = AssocType;
            mObjIDs = new List<string>();

            string[] AssocIDs = AssocID.Split(new char[] { ';' });

            for (int i = 0; i < AssocIDs.Length; i++)
                mObjIDs.Add(AssocIDs[i]);

            if (mType == evCustom)
                evtsCustom = new CEvents();

            switch (mType)
            {
                case evAll:
                    lblTitle.Text = "所有分課表";
                    break;
                case evWho:
                    lblTitle.Text = AssocText + "分課表";
                    break;
                case evWhom:
                    lblTitle.Text = AssocText + "分課表";
                    break;
                case evWhere:
                    lblTitle.Text = AssocText + "分課表";
                    break;
                case evWhat:
                    lblTitle.Text = AssocText + "分課表";
                    break;
                case evCustom:
                    lblTitle.Text = "待處理分課表";
                    break;
            }

            InitContent();
            InitContextMenu();
        }

        /// <summary>
        /// 初始化選單
        /// </summary>
        private void InitContextMenu()
        {
            ContextMenuStrip ContextMenuStrip = new ContextMenuStrip();

            if (mType == evCustom)
            {
                ContextMenuStrip.Items.Add("移出待處理", null, (sender, e) =>
                {
                    RemoveEvents(GetSelectedEvents());

                    //foreach (CEvent Event in GetSelectedEvents())
                    //    if (evtsTemp.Exists(Event.EventID))
                    //    {
                    //        evtsTemp.RemoveID(Event.EventID);
                    //        RemoveEvent(Event.EventID);
                    //    }

                    if (TempUpdate != null)
                        TempUpdate(null, new TempUpdateEventArgs(evtsTemp.Count));
                });
            }
            else
            {
                ContextMenuStrip.Items.Add("加入至待處理", null, (sender, e) =>
                {
                    foreach (CEvent Event in GetSelectedEvents())
                        if (!evtsTemp.Exists(Event.EventID))
                            evtsTemp.Add(Event);

                    if (TempUpdate != null)
                        TempUpdate(null, new TempUpdateEventArgs(evtsTemp.Count));
                }); 
            }

            this.grdEvents.ContextMenuStrip = ContextMenuStrip;
        }

        /// <summary>
        /// 取得選取事件數量
        /// </summary>
        public int SelectedCount
        {
            get
            {
                return grdEvents.SelectedRows.Count;
            }
        }

        /// <summary>
        /// 取得選取事件
        /// </summary>
        /// <returns></returns>
        public CEvents GetSelectedEvents()
        {
            CEvents evtsSelected = new CEvents();            

            foreach (DataGridViewRow SelectedItem in grdEvents.SelectedRows)
            {
                string SelectedEventID = "" + SelectedItem.Cells[colEventID].Value;

                evtsSelected.Add(schLocal.CEvents[SelectedEventID]);
            }

            return evtsSelected;
        }

        /// <summary>
        /// 加入自訂分課表
        /// </summary>
        /// <param name="evtsNew"></param>
        public void AddCustomEvents(CEvents evtsNew)
        {
            if (mType != evCustom)
                return;

            foreach (CEvent evtMember in evtsNew)
            {
                if (evtsCustom.Add(evtMember) != null)
                    AddGridItem(evtMember, -1);
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// 分課表類別
        /// </summary>
        public int AssocObjType
        {
            get { return mType; }

            #region VB
            //Public Property Get AssocObjType() As Integer
            //    AssocObjType = mType
            //End Property
            #endregion
        }

        /// <summary>
        /// 分課表類別所屬資源系統編號
        /// </summary>
        public List<string> AssocObjID
        {
            get { return mObjIDs; }
        }
        #endregion


        #region Private subs and functions
        /// <summary>
        /// 根據事件編號判斷事件的資源屬性是否與ObjID相等
        /// </summary>
        /// <param name="EventID"></param>
        /// <returns></returns>
        private bool IsRelatedEvent(string EventID)
        {
            CEvent evtTest;

            //Determine if this event is related to this eventlist

            evtTest = schLocal.CEvents[EventID];

            switch (mType)
            {
                case evAll:
                    return true;
                case evWho:
                    return mObjIDs.Contains(evtTest.TeacherID1) || mObjIDs.Contains(evtTest.TeacherID2) || mObjIDs.Contains(evtTest.TeacherID3);
                case evWhom:
                    return mObjIDs.Contains(evtTest.ClassID);
                case evWhere:
                    return mObjIDs.Contains(evtTest.ClassroomID);
                case evWhat:
                    return mObjIDs.Contains(evtTest.SubjectID);
                case evCustom:

                    return true;
                    //return evtsCustom.Exists(mObjID);
                default:
                    return false;
            }

            #region VB
            //Private Function IsRelatedEvent(ByVal EventID As Long) As Boolean
            //    Dim evtMember As CEvent
            //    Dim evtTest As CEvent

            //    'Determine if this event is related to this eventlist
            //    Set evtTest = schLocal.CEvents(CStr(EventID))
            //    With evtTest
            //    Select Case mType
            //        Case evAll
            //            IsRelatedEvent = True
            //        Case evWho
            //            IsRelatedEvent = (.WhoID = mObjID)
            //        Case evWhom
            //            IsRelatedEvent = (.WhomID = mObjID)
            //        Case evWhere
            //            IsRelatedEvent = (.WhereID = mObjID)
            //        Case evWhat
            //            IsRelatedEvent = (.WhatID = mObjID)
            //        Case evCustom
            //            IsRelatedEvent = evtsCustom.Exists(.EventID)
            //    End Select
            //    End With
            //    Set evtTest = Nothing
            //End Function
            #endregion
        }

        /// <summary>
        /// 根據單一事件更新DataGrid
        /// </summary>
        /// <param name="idTarget"></param>
        private void RefreshEvent(string idTarget)
        {
            CEvents evtsRefresh = new CEvents();

            CEvent evtRefresh = schLocal.CEvents[idTarget];

            evtsRefresh.Add(evtRefresh);

            RefreshEvents(RCActions.rcRefresh, evtsRefresh);

            //CEvents evtsSolutionUpdate = new CEvents();

            //foreach (CEvent evtPaint in schLocal.CEvents)           
            //{
            //    if (!string.IsNullOrEmpty(evtRefresh.WhoID1))
            //        if (evtRefresh.WhoID1.Equals(evtPaint.WhoID1))
            //            evtsSolutionUpdate.Add(evtPaint);

            //    if (!string.IsNullOrEmpty(evtRefresh.WhoID2))
            //        if (evtRefresh.WhoID2.Equals(evtPaint.WhoID2))
            //            evtsSolutionUpdate.Add(evtPaint);

            //    if (!string.IsNullOrEmpty(evtRefresh.WhoID3))
            //        if (evtRefresh.WhoID3.Equals(evtPaint.WhoID3))
            //            evtsSolutionUpdate.Add(evtPaint);

            //    if (!string.IsNullOrEmpty(evtRefresh.WhomID))
            //        if (evtRefresh.WhomID.Equals(evtPaint.WhomID))
            //            evtsSolutionUpdate.Add(evtPaint);

            //    if (!string.IsNullOrEmpty(evtRefresh.WhereID))
            //        if (evtRefresh.WhereID.Equals(evtPaint.WhereID))
            //            evtsSolutionUpdate.Add(evtPaint);
            //}

            //evtsSolutionUpdate.Add(evtRefresh);

            //if (evtsSolutionUpdate.Count > 0)
            //{
            //    mStopwatch.Restart();
            //    schLocal.GetSolutionCounts(evtsSolutionUpdate);
            //    mStopwatch.Stop();
            //    Console.WriteLine(""+mStopwatch.ElapsedMilliseconds);
            //}

            #region VB
            //Private Sub RefreshEvent(ByVal idTarget As Long)
            //    Dim evtsRefresh As CEvents

            //    Set evtsRefresh = New CEvents
            //    evtsRefresh.Add schLocal.CEvents(CStr(idTarget))
            //    RefreshEvents rcRefresh, evtsRefresh
            //End Sub
            #endregion
        }

        /// <summary>
        /// 新增事件至DataGrid
        /// </summary>
        /// <param name="idTarget"></param>
        private void InsertEvent(string idTarget)
        {
            CEvents evtsRefresh = new CEvents();

            evtsRefresh.Add(schLocal.CEvents[idTarget]);

            RefreshEvents(RCActions.rcInsert, evtsRefresh);

            #region VB
            //Private Sub InsertEvent(ByVal idTarget As Long)
            //    Dim evtsRefresh As CEvents

            //    Set evtsRefresh = New CEvents
            //    evtsRefresh.Add schLocal.CEvents(CStr(idTarget))
            //    RefreshEvents rcInsert, evtsRefresh
            //End Sub
            #endregion
        }

        /// <summary>
        /// 移除事件列表
        /// </summary>
        /// <param name="Events"></param>
        private void RemoveEvents(CEvents Events)
        {
            RefreshEvents(RCActions.rcRemove,Events);
        }

        /// <summary>
        /// 將事件至DataGrid中移除
        /// </summary>
        /// <param name="idTarget"></param>
        private void RemoveEvent(string idTarget)
        {
            CEvents evtsRefresh = new CEvents();

            evtsRefresh.Add(schLocal.CEvents[idTarget]);

            RefreshEvents(RCActions.rcRemove, evtsRefresh);

            #region VB
            //Private Sub RemoveEvent(ByVal idTarget As Long)
            //    Dim evtsRefresh As CEvents

            //    Set evtsRefresh = New CEvents
            //    evtsRefresh.Add schLocal.CEvents(CStr(idTarget))
            //    RefreshEvents rcRemove, evtsRefresh
            //End Sub
            #endregion
        }

        /// <summary>
        /// 根據事件列表更新DataGrid，待做詳細測試
        /// </summary>
        /// <param name="Action"></param>
        /// <param name="evtsRefresh"></param>
        private void RefreshEvents(RCActions Action, CEvents evtsRefresh)
        {
            if (evtsTransfers == null || evtsTransfers.Count ==0)
                return;

            mStopwatch.Restart();
            grdEvents.SuspendLayout();
            IsSelectionChanged = false;

            bool bAdd = false;
            int nIndex = -1;
            int UpdateIndex;
            CEvent evtNewTransfer;
            CEvent evtExistTransfer;


            foreach (CEvent evtRefresh in evtsRefresh)
            {
                #region 判斷此事件是否屬於目前分課表的分類
                switch (mType)
                {
                    case evAll: bAdd = true; break;
                    case evWho: bAdd = (mObjIDs.Contains(evtRefresh.TeacherID1)) || (mObjIDs.Contains(evtRefresh.TeacherID2)) || (mObjIDs.Contains(evtRefresh.TeacherID3)); break;
                    case evWhom: bAdd = (mObjIDs.Contains(evtRefresh.ClassID)); break;
                    case evWhere: bAdd = (mObjIDs.Contains(evtRefresh.ClassroomID)); break;
                    case evCustom: bAdd = true; break;
                }
                #endregion

                if (bAdd)
                {
                    switch (Action)
                    {
                        //新增OK
                        case RCActions.rcInsert:
                            AddGridItem(evtRefresh, nIndex);
                            break;
                        //移除OK
                        case RCActions.rcRemove:
                            #region 根據事件編號尋找後移除
                            CEvent evtTransfer = evtsTransfers
                                .Single(x => x.EventID.Equals(evtRefresh.EventID));

                            if (evtTransfer != null)
                            {
                                evtsTransfers.Remove(evtTransfer);
                                nIndex--;
                            }
                            #endregion
                            break;
                        case RCActions.rcRefresh:
                            #region 先移除後再新增
                            evtNewTransfer = evtRefresh;
                            evtExistTransfer = evtsTransfers
                                .Single(x => x.EventID.Equals(evtRefresh.EventID));


                            UpdateIndex = evtsTransfers.IndexOf(evtExistTransfer);
                            evtsTransfers.RemoveAt(UpdateIndex);

                            mStopwatch.Stop();
                            Console.WriteLine("" + mStopwatch.Elapsed.TotalSeconds);


                            evtsTransfers.Insert(UpdateIndex, evtNewTransfer);
                            #endregion
                            break;
                        case RCActions.rcSolutionCount: //已測試，OK
                            evtNewTransfer = evtRefresh;
                            evtExistTransfer = evtsTransfers
                                .Single(x => x.EventID.Equals(evtRefresh.EventID));

                            UpdateIndex = evtsTransfers.IndexOf(evtExistTransfer);

                            evtsTransfers.RemoveAt(UpdateIndex);

                            evtsTransfers.Insert(UpdateIndex, evtNewTransfer);
                            break;
                        default:
                            break;
                    }
                }               
            }


            if (Action == RCActions.rcRemove)
            {
                foreach (CEvent evtRefresh in evtsRefresh)
                {
                    if (evtsTemp.Exists(evtRefresh.EventID))
                        evtsTemp.RemoveID(evtRefresh.EventID);

                    if (evtsCustom!=null)
                        if (evtsCustom.Exists(evtRefresh.EventID))
                            evtsCustom.RemoveID(evtRefresh.EventID);

                    if (evtsTransfers.Contains(evtRefresh))
                        evtsTransfers.Remove(evtRefresh);
                }
            }

            grdEvents.ClearSelection();
            IsSelectionChanged = true;
            grdEvents.ResumeLayout();

            mStopwatch.Stop();
            Console.WriteLine("" + mStopwatch.Elapsed.TotalSeconds);
        }

        /// <summary>
        /// 將事件新增到DataGrid上
        /// </summary>
        /// <param name="evtAdd"></param>
        /// <param name="nPos"></param>
        private void AddGridItem(CEvent evtAdd, int nPos)
        {           
            //若是nPos為-1則新增到最後面
            nPos = nPos < 0 ? evtsTransfers.Count : nPos;

            CEvent evtTransfer = evtAdd;

            evtsTransfers.Insert(nPos, evtTransfer);

            #region SetRow的作法
            //int RowIndex = fgEvents.Rows.Add();

            //SetRow(fgEvents.Rows[RowIndex], evtAdd);
            #endregion

            #region VB
            //Private Sub AddGridItem(evtAdd As CEvent, ByVal nPos As Long)
            //    nPos = IIf(nPos < 0, fgEvents.Rows, nPos)
            //    With evtAdd
            //    fgEvents.AddItem IIf(.ManualLock, "*", "") & vbTab & _
            //                     IIf(.SolutionCount = -1, "-", .SolutionCount) & vbTab & _
            //                     schLocal.Whos(CStr(.WhoID)).Name & vbTab & _
            //                     schLocal.Whoms(CStr(.WhomID)).Name & vbTab & _
            //                     schLocal.Whats(CStr(.WhatID)).Name & vbTab & _
            //                     schLocal.Wheres(CStr(.WhereID)).Name & vbTab & _
            //                     .WeekDay & vbTab & _
            //                     .PeriodNo & vbTab & _
            //                     .Length & vbTab & _
            //                     .WeekDayCondition & vbTab & _
            //                     .PeriodCondition & vbTab & _
            //                     IIf(.AllowLongBreak, LoadResString(rsYes), LoadResString(rsNo)) & vbTab & _
            //                     IIf(.AllowDuplicate, LoadResString(rsYes), LoadResString(rsNo)) & vbTab & _
            //                     IIf(.WeekFlag = 1, LoadResString(rsOdd), IIf(.WeekFlag = 2, LoadResString(rsEven), IIf(.WeekFlag = 3, LoadResString(rsBoth), LoadResString(rsErr)))) & vbTab & _
            //                     .Priority & vbTab & _
            //                     .EventID, nPos
            //    fgEvents.RowData(nPos) = .EventID
            //    fgEvents.Row = nPos
            //    fgEvents.ColSel = colLast
            //    fgEvents.FillStyle = flexFillRepeat
            //    If .WeekDay <> 0 Then
            //        fgEvents.CellBackColor = clrScheduledBC
            //        fgEvents.CellForeColor = clrScheduledFC
            //    ElseIf .SolutionCount = 0 Then
            //        fgEvents.CellBackColor = clrNoSolutionBC
            //        fgEvents.CellForeColor = clrNoSolutionFC
            //    ElseIf .SolutionCount > 0 Then
            //        fgEvents.CellBackColor = clrHasSolutionBC
            //        fgEvents.CellForeColor = clrHasSolutionFC
            //    Else
            //        fgEvents.CellBackColor = clrFreeBC
            //        fgEvents.CellForeColor = clrFreeFC
            //    End If
            //    End With
            //End Sub 
            #endregion
        }

        /// <summary>
        /// 初始化內容，被SetAssocObject呼叫
        /// </summary>
        private void InitContent()
        {
            bool bAddEvent = false; //是否新增CEvent至畫面上

            CEvents evtPaints = new CEvents();

            foreach (CEvent evtPaint in schLocal.CEvents)
            {
                switch (mType)
                {
                    case evAll:
                        bAddEvent = true;
                        break;
                    case evWho:
                        bAddEvent = mObjIDs.Contains(evtPaint.TeacherID1) || mObjIDs.Contains(evtPaint.TeacherID2) || mObjIDs.Contains(evtPaint.TeacherID3);
                        break;
                    case evWhom:
                        bAddEvent = mObjIDs.Contains(evtPaint.ClassID);
                        break;
                    case evWhere:
                        bAddEvent = mObjIDs.Contains(evtPaint.ClassroomID);
                        break;
                    case evWhat:
                        bAddEvent = mObjIDs.Contains(evtPaint.SubjectID);
                        break;
                    case evCustom:
                        bAddEvent = false;
                        break;
                }

                if (bAddEvent)
                    evtPaints.Add(evtPaint);
            }

            if (mType == evCustom)
            {
                evtPaints = evtsTemp;
                evtsCustom = evtsTemp;
            }

            grdEvents.Rows.Clear();

            grdEvents.SuspendLayout();

            Stopwatch watch = new Stopwatch();

            watch.Start();

            evtsTransfers = new CEventBindingList();

            foreach (CEvent evtPaint in evtPaints)
                evtsTransfers.Add(evtPaint);

            IsSelectionChanged = false;

            this.grdEvents.DataSource = evtsTransfers;

            this.grdEvents.ClearSelection();

            IsSelectionChanged = true;

            watch.Stop();

            this.grdEvents.ResumeLayout();

            Console.WriteLine("" + watch.Elapsed.TotalMilliseconds);       
        }
        #endregion

        #region public subs and function
        /// <summary>
        /// 解除鎖定多筆事件
        /// </summary>
        public void UnLockEvents()
        {
            if (grdEvents.SelectedRows.Count > 0)
            {
                List<string> EventIDs = new List<string>();

                foreach (DataGridViewRow SelectedItem in grdEvents.SelectedRows)
                {
                    string EventID = "" + SelectedItem.Cells[21].Value;

                    if (schLocal.CEvents[EventID].ManualLock)
                        EventIDs.Add(EventID);
                }

                if (EventIDs.Count > 0)
                    EventIDs.ForEach(x => schLocal.UnlockEvent(x));
            }
        }

        /// <summary>
        /// 鎖定多筆事件
        /// </summary>
        public void LockEvents()
        {
            if (grdEvents.SelectedRows.Count > 0)
            {
                List<string> EventIDs = new List<string>();

                foreach (DataGridViewRow SelectedItem in grdEvents.SelectedRows)
                {
                    string EventID = "" + SelectedItem.Cells[colEventID].Value;
                    if (!schLocal.CEvents[EventID].ManualLock)
                        EventIDs.Add(EventID);
                }

                if (EventIDs.Count > 0)
                    EventIDs.ForEach(x => schLocal.LockEvent(x));
            }
        }

        /// <summary>
        /// 釋放事件
        /// </summary>
        public void FreeEvents()
        {
            Stopwatch mWatch = new Stopwatch();

            mWatch.Start();

            if (grdEvents.SelectedRows.Count > 0)
            {
                CEvents evtsFree = new CEvents();

                foreach (DataGridViewRow SelectedItem in grdEvents.SelectedRows)
                {
                    string EventID = "" + SelectedItem.Cells[colEventID].Value;

                    evtsFree.Add(schLocal.CEvents[EventID]);
                }

                schLocal.FreeEvents(evtsFree);
            }

            mWatch.Stop();

            Console.WriteLine(mWatch.Elapsed.TotalMilliseconds);
        }

        /// <summary>
        /// 自動排課
        /// </summary>
        public void AutoSchedule()
        {
            CEvents evtsAuto = new CEvents();

            //Prepare autoschedule eventlist
            foreach (CEvent evtNew in GetSelectedEvents())
                if (!evtNew.ManualLock)
                    evtsAuto.Add(evtNew);

            //Test if there is any event in evtsAuto
            if (evtsAuto.Count <= 0) return;

            //Display progress dialog box
            frmASProgress = new frmProgress();

            frmASProgress.Start("自動排課中...", 1, evtsAuto.Count);

            frmASProgress.Show();

            schLocal.GetSolutionCounts(evtsAuto);

            evtsAuto = schLocal.SortEvent(evtsAuto);

            if (!string.IsNullOrEmpty(schLocal.AutoSchedule(evtsAuto)))
            {
                if (!frmASProgress.UserAbort)
                {
                    MessageBox.Show("終告不治，請另求他途！");
                }
            }

            frmASProgress.Close();
            frmASProgress = null;
        }

        /// <summary>
        /// 計算解決方案
        /// </summary>
        public void CalculateSolutionCount()
        {           
            Cursor.Current = Cursors.WaitCursor;

            CEvents evtsCalc = GetSelectedEvents();

            schLocal.GetSolutionCounts(evtsCalc);

            Cursor.Current = Cursors.Arrow;
        }

        /// <summary>
        /// 修改事件屬性
        /// </summary>
        public void ChangeProperty()
        {
            if (grdEvents.SelectedRows.Count == 1)
            {
                string SelectedEventID = "" + grdEvents.SelectedRows[0].Cells[colEventID].Value;

                frmEventProperty frmProperty = new frmEventProperty(SelectedEventID);

                frmProperty.ShowDialog();
            }
        }

        /// <summary>
        /// 代課查詢
        /// </summary>
        public void QueryCandidates()
        {
            if (grdEvents.SelectedRows.Count == 1)
            {
                string SelectedEventID = "" + grdEvents.SelectedRows[0].Cells[colEventID].Value;

                frmCandidates frmCand = new frmCandidates(SelectedEventID);

                frmCand.ShowDialog();
            }
        }

        /// <summary>
        /// 列印分課表
        /// </summary>
        public void Print()
        {
            //待補
            //PrintHelper.NormalPrinting(this);
        }
        #endregion
    }
}