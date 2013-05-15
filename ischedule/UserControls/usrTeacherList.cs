using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevComponents.AdvTree;
using Sunset.Data;

namespace ischedule
{
    /// <summary>
    /// 教師清單
    /// </summary>
    public partial class usrTeacherList : UserControl
    {
        private Scheduler schLocal = Scheduler.Instance;
        private string idSaveWho1 = string.Empty;
        private string idSaveWho2 = string.Empty;
        private string idSaveWho3 = string.Empty;

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public usrTeacherList()
        {
            InitializeComponent();

            //自動排程完成，更新所有內容
            schLocal.AutoScheduleComplete += (sender, e) => RefreshAll();

            //刪除之前將教師編號先儲存起來
            schLocal.EventBeforeDelete += (sender, e) => SaveWho(e.EventID);

            //刪除後根據儲存的教師編號更新教師清單
            schLocal.EventDeleted += (sender, e) => RefreshSaveWho();

            //新增分課表更新教師清單
            schLocal.EventInserted += (sender, e) => RefreshEvent(e.EventID);

            //屬性變更前將教師編號先儲存起來
            schLocal.EventPropertyBeforeChange += (sender, e) =>
            {
                if ((e.ChangeFlag & MaskOptions.maskWho) == MaskOptions.maskWho)
                    SaveWho(e.EventID);
            };

            //屬性變更後更新教師清單
            schLocal.EventPropertyChanged += (sender, e) =>
            {
                if ((e.ChangeFlag & MaskOptions.maskWho) == MaskOptions.maskWho)
                    RefreshEvent(e.EventID);
            };

            //分課表排定後更新教師清單
            schLocal.EventScheduled += (sender, e) => RefreshEvent(e.EventID);

            //釋放分課表更新所有教師清單
            schLocal.EventsFreed += (sender, e) => RefreshAll();

            //更新所有教師清單
            RefreshAll();

            chkName.CheckedChanged += (sender, e) => RefreshAll();
            chkWhat.CheckedChanged += (sender, e) => RefreshAll();
            chkTotalHour.CheckedChanged += (sender, e) => RefreshAll();
            chkUnAlloc.CheckedChanged += (sender, e) => RefreshAll();

            btnAddToTemp.Click += (sender, e) => MainFormBL.Instance.OpenTeacherEventsView(Constants.evCustom, string.Empty,"待處理");
            menuOpenNewLPView.Click += (sender,e)=>
            {
                string AssocID = GetSelectedValue();

                if (!string.IsNullOrEmpty(AssocID))
                    if (!AssocID.StartsWith("所有"))
                        MainFormBL.Instance.OpenTeacherLPView(Constants.lvWho, AssocID,true);
            };
            
            menuExpand.Click += (sender,e) =>
            {
                Node nodeRoot = treeWho.Nodes[0];

                nodeRoot.Expand();

                nodeRoot.ExpandAll();

            };

            menuCollapse.Click += (sender, e) =>
            {
                treeWho.Nodes[0].Collapse();
            };

            MainFormBL.Instance.TeacherEventsView.TempUpdate += (sender, e) => btnAddToTemp.Text = "待處理分課表(" + e.TotCount + ")";
        }

        #region private function 
        /// <summary>
        /// 更新所有教師清單
        /// </summary>
        private void RefreshAll()
        {
            RefreshContent(string.Empty);
        }

        /// <summary>
        /// 判斷是否新增教師到左方TreeView中
        /// </summary>
        /// <param name="Who"></param>
        /// <returns></returns>
        private bool IsAddWho(string Who)
        {
            if (!string.IsNullOrWhiteSpace(Who) && !Who.Equals("無"))
            {
                //若搜尋字串為空白則全部顯示
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                    return true;
                else
                    return Who.Contains(txtSearch.Text);
            }else
                return false;
        }

        /// <summary>
        /// 根據科目更新教師列表，同時考量到關鍵字搜尋
        /// </summary>
        private void RefreshBySubject(string idWho)
        {
            int Total = 0;

            //根據分課的科目進行分類
            Dictionary<string, List<string>> WhatWhos = new Dictionary<string, List<string>>();

            foreach (CEvent Event in schLocal.CEvents)
            {
                //取得科目名稱
                string What = Event.SubjectID;

                //若科目名稱不在字典中則新增
                if (!WhatWhos.ContainsKey(What))
                    WhatWhos.Add(What, new List<string>());

                if (IsAddWho(Event.TeacherID1))
                    if (!WhatWhos[What].Contains(Event.TeacherID1))
                        WhatWhos[What].Add(Event.TeacherID1);

                if (IsAddWho(Event.TeacherID2))
                    if (!WhatWhos[What].Contains(Event.TeacherID2))
                        WhatWhos[What].Add(Event.TeacherID2);

                if (IsAddWho(Event.TeacherID3))
                    if (!WhatWhos[What].Contains(Event.TeacherID3))
                        WhatWhos[What].Add(Event.TeacherID3);
            }

            treeWho.Nodes.Clear();

            Node nodeRoot = new Node("所有教師");
            nodeRoot.TagString = "所有教師";
            Node nodeNull = new Node("無授課教師分課");
            nodeNull.TagString = "無";

            treeWho.Nodes.Add(nodeRoot);
            treeWho.Nodes.Add(nodeNull);

            foreach (string strWhat in WhatWhos.Keys)
            {
                Node nodeWhat = new Node(strWhat);
                List<string> Names = new List<string>();

                foreach (string strWho in WhatWhos[strWhat])
                {
                    if (schLocal.Teachers.Exists(strWho))
                    {
                        Teacher whoPaint = schLocal.Teachers[strWho];

                        int UnAllocHour = whoPaint.TotalHour - whoPaint.AllocHour;

                        Node nodeWho = new Node(whoPaint.Name + "(" + UnAllocHour + "/" + whoPaint.TotalHour + ")");
                        nodeWho.TagString = whoPaint.Name;
                        nodeWhat.Nodes.Add(nodeWho);
                        Names.Add(whoPaint.Name);

                        Total++;
                    }
                }

                if (nodeWhat.Nodes.Count > 0)
                {
                    nodeWhat.Text = nodeWhat.Text + "(" + nodeWhat.Nodes.Count + ")";
                    nodeWhat.TagString = string.Join(";", Names.ToArray());
                    nodeRoot.Nodes.Add(nodeWhat);
                    nodeRoot.Expand();
                }
            }

            nodeRoot.Text = nodeRoot.Text + "(" + (schLocal.Teachers.Count-1) + ")";
            nodeRoot.ExpandAll();
        }

        /// <summary>
        /// 根據未排時數更新教師列表，同時考量到關鍵字搜尋
        /// </summary>
        private void RefreshByUnAlloc(string idWho)
        {
            int Total = 0;

            //根據分課的科目進行分類
            SortedDictionary<int, List<string>> UnAllocWhos = new SortedDictionary<int, List<string>>();

            foreach (Teacher Who in schLocal.Teachers)
            {
                if (Who.Name.Equals("無"))
                    continue;

                int UnAlloc = Who.TotalHour - Who.AllocHour;

                //若科目名稱不在字典中則新增
                if (!UnAllocWhos.ContainsKey(UnAlloc))
                    UnAllocWhos.Add(UnAlloc, new List<string>());

                if (IsAddWho(Who.Name))
                    if (!UnAllocWhos[UnAlloc].Contains(Who.Name))
                        UnAllocWhos[UnAlloc].Add(Who.Name);
            }

            treeWho.Nodes.Clear();

            Node nodeRoot = new Node("所有教師");
            nodeRoot.TagString = "所有教師";

            Node nodeNull = new Node("無授課教師分課");
            nodeNull.TagString = "無";

            treeWho.Nodes.Add(nodeRoot);
            treeWho.Nodes.Add(nodeNull);

            foreach (int strWhat in UnAllocWhos.Keys.ToList().OrderByDescending(x=>x))
            {
                Node nodeWhat = new Node("" + strWhat);
                List<string> Names = new List<string>();

                foreach (string strWho in UnAllocWhos[strWhat])
                {
                    if (schLocal.Teachers.Exists(strWho))
                    {
                        Teacher whoPaint = schLocal.Teachers[strWho];

                        int UnAllocHour = whoPaint.TotalHour - whoPaint.AllocHour;

                        Node nodeWho = new Node(whoPaint.Name + "(" + UnAllocHour + "/" + whoPaint.TotalHour + ")");
                        nodeWho.TagString = whoPaint.Name;
                        Names.Add(whoPaint.Name);
                        nodeWhat.Nodes.Add(nodeWho);

                        Total++;
                    }
                }

                if (nodeWhat.Nodes.Count > 0)
                {
                    nodeWhat.Text = nodeWhat.Text + "(" + nodeWhat.Nodes.Count + ")";
                    nodeWhat.TagString = string.Join(";", Names.ToArray());
                    nodeRoot.Nodes.Add(nodeWhat);
                    nodeRoot.Expand();
                }
            }

            nodeRoot.Text = nodeRoot.Text + "(" + (schLocal.Teachers.Count -1) + ")";
            nodeRoot.ExpandAll();
        }

        /// <summary>
        /// 根據總時數更新教師列表，同時考量到關鍵字搜尋
        /// </summary>
        private void RefreshByTotalHour(string idWho)
        {
            int Total = 0;

            //根據分課的科目進行分類
            SortedDictionary<int, List<string>> TotalWhos = new SortedDictionary<int, List<string>>();

            foreach (Teacher Who in schLocal.Teachers)
            {
                if (Who.Name.Equals("無"))
                    continue;

                int TotalHour = Who.TotalHour;

                //若科目名稱不在字典中則新增
                if (!TotalWhos.ContainsKey(TotalHour))
                    TotalWhos.Add(TotalHour, new List<string>());

                if (IsAddWho(Who.Name))
                    if (!TotalWhos[TotalHour].Contains(Who.Name))
                        TotalWhos[TotalHour].Add(Who.Name);
            }

            treeWho.Nodes.Clear();

            Node nodeRoot = new Node("所有教師");
            nodeRoot.TagString = "所有教師";

            Node nodeNull = new Node("無授課教師分課");
            nodeNull.TagString = "無";

            treeWho.Nodes.Add(nodeRoot);
            treeWho.Nodes.Add(nodeNull);

            foreach (int strWhat in TotalWhos.Keys.ToList().OrderByDescending(x=>x))
            {
                Node nodeWhat = new Node("" + strWhat);
                List<string> Names = new List<string>();

                foreach (string strWho in TotalWhos[strWhat])
                {
                    if (schLocal.Teachers.Exists(strWho))
                    {
                        Teacher whoPaint = schLocal.Teachers[strWho];

                        int UnAllocHour = whoPaint.TotalHour - whoPaint.AllocHour;

                        Node nodeWho = new Node(whoPaint.Name + "(" + UnAllocHour + "/" + whoPaint.TotalHour + ")");
                        nodeWho.TagString = whoPaint.Name;
                        Names.Add(whoPaint.Name);
                        nodeWhat.Nodes.Add(nodeWho);

                        Total++;
                    }
                }

                if (nodeWhat.Nodes.Count > 0)
                {
                    nodeWhat.Text = nodeWhat.Text + "(" + nodeWhat.Nodes.Count + ")";
                    nodeWhat.TagString = string.Join(";", Names.ToArray());
                    nodeRoot.Nodes.Add(nodeWhat);
                    nodeRoot.Expand();
                }
            }

            nodeRoot.Text = nodeRoot.Text + "(" + (schLocal.Teachers.Count-1) + ")";
            nodeRoot.ExpandAll();
        }

        /// <summary>
        /// 根據姓名更新教師列表，同時考量到關鍵字搜尋
        /// </summary>
        private void RefreshByName(string idWho)
        {
            //根據分課的科目進行分類
            SortedDictionary<string,string> TeacherNames = new SortedDictionary<string,string>();

            foreach (Teacher Who in schLocal.Teachers)
            {
                int TotalHour = Who.TotalHour;

                //若科目名稱不在字典中則新增
                if (!TeacherNames.ContainsKey(Who.Name))
                    TeacherNames.Add(Who.Name, Who.Name);
            }

            treeWho.Nodes.Clear();

            Node nodeAll = new Node("所有教師");
            nodeAll.TagString = "所有教師";

            Node nodeNull = new Node("無授課教師");
            nodeNull.TagString = "無";

            treeWho.Nodes.Add(nodeAll);
            treeWho.Nodes.Add(nodeNull);

            foreach (string WhoName in TeacherNames.Keys)
            {
                if (WhoName.Equals("無"))
                    continue;

                if (IsAddWho(WhoName))
                {
                    Teacher whoPaint = schLocal.Teachers[WhoName];

                    int UnAllocHour = whoPaint.TotalHour - whoPaint.AllocHour;

                    Node nodeWho = new Node(whoPaint.Name + "(" + UnAllocHour + "/" + whoPaint.TotalHour + ")");
                    nodeWho.TagString = whoPaint.Name;
                    nodeAll.Nodes.Add(nodeWho);
                }
            }

            nodeAll.Text = nodeAll.Text + "(" + (schLocal.Teachers.Count - 1) + ")";
            nodeAll.Expand();
        }

        /// <summary>
        /// 根據教師編號更新教師清單，若傳入空白則更新所有教師清單
        /// </summary>
        /// <param name="idWho">教師編號</param>
        private void RefreshContent(string idWho)
        {
            //目前先採取更新所有教師方式
            if (chkName.Checked)
                RefreshByName(idWho);
            else if (chkWhat.Checked)
                RefreshBySubject(idWho);
            else if (chkUnAlloc.Checked)
                RefreshByUnAlloc(idWho);
            else if (chkTotalHour.Checked)
                RefreshByTotalHour(idWho);


        }

                /// <summary>
        /// 根據分課表編號更新教師清單
        /// </summary>
        /// <param name="EventID">分課表編號</param>
        private void RefreshEvent(string EventID)
        {
            //目前先所有更新，暫不支援單筆更新
            RefreshAll();        
        }
        
        /// <summary>
        /// 儲存分課表的教師編號
        /// </summary>
        /// <param name="EventID">分課表編號</param>
        private void SaveWho(string EventID)
        {
            idSaveWho1 = schLocal.CEvents[EventID].TeacherID1;
            idSaveWho2 = schLocal.CEvents[EventID].TeacherID2;
            idSaveWho3 = schLocal.CEvents[EventID].TeacherID3;
        }     

        /// <summary>
        /// 根據教師編號更新教師清單
        /// </summary>
        private void RefreshSaveWho()
        {
            //目前先更新所有，不逐筆更新
            RefreshAll();
        }

        /// <summary>
        /// 取得選取值
        /// </summary>
        /// <returns></returns>
        private string GetSelectedValue()
        {
            Node SelectNode = treeWho.SelectedNode;

            if (SelectNode == null)
                return string.Empty;

            return SelectNode.TagString;
        }
        #endregion


        #region Event Handler
        /// <summary>
        /// 關鍵字改變時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            RefreshContent(string.Empty);
        }

        /// <summary>
        /// 待處理檢視
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddToTemp_Click(object sender, EventArgs e)
        {
            MainFormBL.Instance.OpenTeacherEventsView(Constants.evCustom, string.Empty,"待處理");
        }

        /// <summary>
        /// 處理點選右鍵
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeWho_NodeMouseDown(object sender, TreeNodeMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                string TagString = e.Node.TagString;

                if (e.Node.Text.StartsWith("所有"))
                {
                    menuExpand.Visible = true;
                    menuCollapse.Visible = true;
                    menuOpenNewLPView.Visible = false;
                }
                else if (!string.IsNullOrEmpty(TagString))
                {
                    menuExpand.Visible = false;
                    menuCollapse.Visible = false;
                    menuOpenNewLPView.Visible = true;
                }
                else
                {
                    menuExpand.Visible = false;
                    menuCollapse.Visible = false;
                    menuOpenNewLPView.Visible = false;
                }
            }
        }

        /// <summary>
        /// 點下節點
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeWho_NodeClick(object sender, TreeNodeMouseEventArgs e)
        {
            string AssocID = e.Node.TagString;

            if (!string.IsNullOrEmpty(AssocID))
            {
                if (AssocID.StartsWith("所有"))
                    MainFormBL.Instance.OpenTeacherEventsView(Constants.evAll, string.Empty,"所有");
                else
                {
                    MainFormBL.Instance.OpenTeacherEventsView(Constants.evWho, AssocID,e.Node.Text);

                    if (!AssocID.Equals("無"))
                        MainFormBL.Instance.OpenTeacherLPView(Constants.lvWho, AssocID);
                }
            }
        }
        #endregion
    }
}