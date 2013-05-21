using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevComponents.AdvTree;
using Sunset.Data;

namespace ischedule
{
    /// <summary>
    /// 場地清單
    /// </summary>
    public partial class usrClassroomList : UserControl
    {
        private Scheduler schLocal = Scheduler.Instance;
        private string idSaveWhere = string.Empty;

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public usrClassroomList()        
        {
            InitializeComponent();

            //自動排程完成，更新所有內容
            schLocal.AutoScheduleComplete += (sender, e) => RefreshAll();

            //刪除之前將場地編號先儲存起來
            schLocal.EventBeforeDelete += (sender, e) => SaveWhere(e.EventID);

            //刪除後根據儲存的場地編號更新教師清單
            schLocal.EventDeleted += (sender, e) => RefreshSaveWhere();

            //新增分課表更新場地清單
            schLocal.EventInserted += (sender, e) => RefreshEvent(e.EventID);

            //屬性變更前將教師編號先儲存起來
            schLocal.EventPropertyBeforeChange += (sender, e) =>
            {
                if ((e.ChangeFlag & MaskOptions.maskWhere) == MaskOptions.maskWhere)
                    SaveWhere(e.EventID);
            };

            //屬性變更後更新場地清單
            schLocal.EventPropertyChanged += (sender, e) =>
            {
                if ((e.ChangeFlag & MaskOptions.maskWhere) == MaskOptions.maskWhere)
                    RefreshEvent(e.EventID);
            };

            //分課表排定後更新場地清單
            schLocal.EventScheduled += (sender, e) => RefreshEvent(e.EventID);

            //釋放分課表更新所有場地清單
            schLocal.EventsFreed += (sender, e) => RefreshAll();

            //更新所有場地清單
            RefreshAll();

            chkName.CheckedChanged += (sender, e) => RefreshAll();
            chkCapacity.CheckedChanged += (sender, e) => RefreshAll();
            chkTotalAlloc.CheckedChanged += (sender, e) => RefreshAll();
            chkUnAlloc.CheckedChanged += (sender, e) => RefreshAll();

            btnAddToTemp.Click += (sender, e) => MainFormBL.Instance.OpenTeacherEventsView(Constants.evCustom, string.Empty, "待處理");
            menuOpenNewLPView.Click += (sender, e) =>
            {
                string AssocID = GetSelectedValue();

                if (!string.IsNullOrEmpty(AssocID))
                    if (!AssocID.StartsWith("所有"))
                        MainFormBL.Instance.OpenTeacherSchedule(Constants.lvWho, AssocID, true);
            };

            menuExpand.Click += (sender, e) =>
            {
                Node nodeRoot = nodeTree.Nodes[0];

                nodeRoot.Expand();

                nodeRoot.ExpandAll();

            };

            menuCollapse.Click += (sender, e) =>
            {
                nodeTree.Nodes[0].Collapse();
            };

            MainFormBL.Instance.TeacherEventsView.TempUpdate += (sender, e) => btnAddToTemp.Text = "待處理分課表(" + e.TotCount + ")";
        }

        #region private function
        /// <summary>
        /// 更新所有場地清單
        /// </summary>
        private void RefreshAll()
        {
            RefreshContent(string.Empty);
        }

        /// <summary>
        /// 判斷是否新增場地到左方TreeView中
        /// </summary>
        /// <param name="Where"></param>
        /// <returns></returns>
        private bool IsAddWhere(string Where)
        {
            if (!string.IsNullOrWhiteSpace(Where) && !Where.Equals("無"))
            {
                //若搜尋字串為空白則全部顯示
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                    return true;
                else
                    return Where.Contains(txtSearch.Text);
            }
            else
                return false;
        }

        /// <summary>
        /// 根據容納數更新場地列表，同時考量到關鍵字搜尋
        /// </summary>
        private void RefreshByCapacity(string idWhere)
        {
            int Total = 0;

            //根據分課的容納數進行分類
            SortedDictionary<int, List<string>> CapacityWheres = new SortedDictionary<int, List<string>>();

            foreach (Classroom vWhere in schLocal.Classrooms)
            {
                //若容納數名稱不在字典中則新增
                if (!CapacityWheres.ContainsKey(vWhere.Capacity))
                    CapacityWheres.Add(vWhere.Capacity, new List<string>());

                if (IsAddWhere(vWhere.Name))
                    if (!CapacityWheres[vWhere.Capacity].Contains(vWhere.WhereID))
                        CapacityWheres[vWhere.Capacity].Add(vWhere.WhereID);
            }

            nodeTree.Nodes.Clear();

            Node nodeRoot = new Node("所有場地");
            nodeRoot.TagString = "所有場地";
            Node nodeNull = new Node("無場地分課");
            nodeNull.TagString = "無";

            nodeTree.Nodes.Add(nodeRoot);
            nodeTree.Nodes.Add(nodeNull);

            foreach (int Capacity in CapacityWheres.Keys)
            {
                Node nodeCapacity = new Node(""+Capacity);
                List<string> Names = new List<string>();

                foreach (string vWhereID in CapacityWheres[Capacity])
                {
                    if (schLocal.Classrooms.Exists(vWhereID))
                    {
                        Classroom wherePaint = schLocal.Classrooms[vWhereID];

                        int UnAllocHour = wherePaint.TotalHour - wherePaint.AllocHour;

                        Node nodeWhere = new Node(wherePaint.Name + "(" + UnAllocHour + "/" + wherePaint.TotalHour + ")");
                        nodeWhere.TagString = wherePaint.Name;
                        nodeCapacity.Nodes.Add(nodeWhere);
                        Names.Add(wherePaint.Name);

                        Total++;
                    }
                }

                if (nodeCapacity.Nodes.Count > 0)
                {
                    nodeCapacity.Text = nodeCapacity.Text + "(" + nodeCapacity.Nodes.Count + ")";
                    nodeCapacity.TagString = string.Join(";", Names.ToArray());
                    nodeRoot.Nodes.Add(nodeCapacity);
                    nodeRoot.Expand();
                }
            }

            nodeRoot.Text = nodeRoot.Text + "(" + (schLocal.Classrooms.Count - 1) + ")";
            nodeRoot.ExpandAll();
        }

        /// <summary>
        /// 根據未排時數更新場地列表，同時考量到關鍵字搜尋
        /// </summary>
        private void RefreshByUnAlloc(string idWhere)
        {
            int Total = 0;

            SortedDictionary<int, List<string>> UnAllocWheres = new SortedDictionary<int, List<string>>();

            foreach (Classroom Where in schLocal.Classrooms)
            {
                if (Where.Name.Equals("無"))
                    continue;

                int UnAlloc = Where.TotalHour - Where.AllocHour;

                if (!UnAllocWheres.ContainsKey(UnAlloc))
                    UnAllocWheres.Add(UnAlloc, new List<string>());

                if (IsAddWhere(Where.WhereID))
                    if (!UnAllocWheres[UnAlloc].Contains(Where.WhereID))
                        UnAllocWheres[UnAlloc].Add(Where.WhereID);
            }

            nodeTree.Nodes.Clear();

            Node nodeRoot = new Node("所有場地");
            nodeRoot.TagString = "所有場地";

            Node nodeNull = new Node("無場地分課");
            nodeNull.TagString = "無";

            nodeTree.Nodes.Add(nodeRoot);
            nodeTree.Nodes.Add(nodeNull);

            foreach (int vUnAlloc in UnAllocWheres.Keys.ToList().OrderByDescending(x => x))
            {
                Node nodeUnAlloc = new Node("" + vUnAlloc);
                List<string> Names = new List<string>();

                foreach (string vWhere in UnAllocWheres[vUnAlloc])
                {
                    if (schLocal.Classrooms.Exists(vWhere))
                    {
                        Classroom wherePaint = schLocal.Classrooms[vWhere];

                        int UnAllocHour = wherePaint.TotalHour - wherePaint.AllocHour;

                        Node nodeWho = new Node(wherePaint.Name + "(" + UnAllocHour + "/" + wherePaint.TotalHour + ")");
                        nodeWho.TagString = wherePaint.Name;
                        Names.Add(wherePaint.Name);
                        nodeUnAlloc.Nodes.Add(nodeWho);

                        Total++;
                    }
                }

                if (nodeUnAlloc.Nodes.Count > 0)
                {
                    nodeUnAlloc.Text = nodeUnAlloc.Text + "(" + nodeUnAlloc.Nodes.Count + ")";
                    nodeUnAlloc.TagString = string.Join(";", Names.ToArray());
                    nodeRoot.Nodes.Add(nodeUnAlloc);
                    nodeRoot.Expand();
                }
            }

            nodeRoot.Text = nodeRoot.Text + "(" + (schLocal.Classrooms.Count - 1) + ")";
            nodeRoot.ExpandAll();
        }

        /// <summary>
        /// 根據總時數更新場地列表，同時考量到關鍵字搜尋
        /// </summary>
        private void RefreshByTotalHour(string idWhere)
        {
            int Total = 0;

            SortedDictionary<int, List<string>> TotalWheres = new SortedDictionary<int, List<string>>();

            foreach(Classroom Where in schLocal.Classrooms)
            {
                if (Where.Name.Equals("無"))
                    continue;

                int TotalHour = Where.TotalHour;

                if (!TotalWheres.ContainsKey(TotalHour))
                    TotalWheres.Add(TotalHour, new List<string>());

                if (IsAddWhere(Where.Name))
                    if (!TotalWheres[TotalHour].Contains(Where.WhereID))
                        TotalWheres[TotalHour].Add(Where.WhereID);
            }

            nodeTree.Nodes.Clear();

            Node nodeRoot = new Node("所有場地");
            nodeRoot.TagString = "所有場地";

            Node nodeNull = new Node("無場地分課");
            nodeNull.TagString = "無";

            nodeTree.Nodes.Add(nodeRoot);
            nodeTree.Nodes.Add(nodeNull);

            foreach (int vTotal in TotalWheres.Keys.ToList().OrderByDescending(x => x))
            {
                Node nodeTotal = new Node("" + vTotal);
                List<string> Names = new List<string>();

                foreach (string vWhereID in TotalWheres[vTotal])
                {
                    if (schLocal.Classrooms.Exists(vWhereID))
                    {
                        Classroom wherePaint = schLocal.Classrooms[vWhereID];

                        int UnAllocHour = wherePaint.TotalHour - wherePaint.AllocHour;

                        Node nodeWhere = new Node(wherePaint.Name + "(" + UnAllocHour + "/" + wherePaint.TotalHour + ")");
                        nodeWhere.TagString = wherePaint.Name;
                        Names.Add(wherePaint.Name);
                        nodeTotal.Nodes.Add(nodeWhere);

                        Total++;
                    }
                }

                if (nodeTotal.Nodes.Count > 0)
                {
                    nodeTotal.Text = nodeTotal.Text + "(" + nodeTotal.Nodes.Count + ")";
                    nodeTotal.TagString = string.Join(";", Names.ToArray());
                    nodeRoot.Nodes.Add(nodeTotal);
                    nodeRoot.Expand();
                }
            }

            nodeRoot.Text = nodeRoot.Text + "(" + (schLocal.Classrooms.Count - 1) + ")";
            nodeRoot.ExpandAll();
        }

        /// <summary>
        /// 根據姓名更新場地列表，同時考量到關鍵字搜尋
        /// </summary>
        private void RefreshByName(string idWhere)
        {
            //根據分課的科目進行分類
            SortedDictionary<string, string> ClassroomNames = new SortedDictionary<string, string>();

            foreach (Classroom Where in schLocal.Classrooms)
            {
                int TotalHour = Where.TotalHour;

                if (!ClassroomNames.ContainsKey(Where.Name))
                    ClassroomNames.Add(Where.Name, Where.WhereID);
            }

            nodeTree.Nodes.Clear();

            Node nodeAll = new Node("所有場地");
            nodeAll.TagString = "所有場地";

            Node nodeNull = new Node("無場地");
            nodeNull.TagString = "無";

            nodeTree.Nodes.Add(nodeAll);
            nodeTree.Nodes.Add(nodeNull);

            foreach (string WhereName in ClassroomNames.Keys)
            {
                if (WhereName.Equals("無"))
                    continue;

                if (IsAddWhere(WhereName))
                {
                    Classroom wherePaint = schLocal.Classrooms[ClassroomNames[WhereName]];

                    int UnAllocHour = wherePaint.TotalHour - wherePaint.AllocHour;

                    Node nodeWhere = new Node(wherePaint.Name + "(" + UnAllocHour + "/" + wherePaint.TotalHour + ")");
                    nodeWhere.TagString = wherePaint.Name;
                    nodeAll.Nodes.Add(nodeWhere);
                }
            }

            nodeAll.Text = nodeAll.Text + "(" + (schLocal.Classrooms.Count - 1) + ")";
            nodeAll.Expand();
        }

        /// <summary>
        /// 根據場地編號更新教師清單，若傳入空白則更新所有教師清單
        /// </summary>
        /// <param name="idWhere">教師編號</param>
        private void RefreshContent(string idWhere)
        {
            if (chkName.Checked)
                RefreshByName(idWhere);
            else if (chkCapacity.Checked)
                RefreshByCapacity(idWhere);
            else if (chkUnAlloc.Checked)
                RefreshByUnAlloc(idWhere);
            else if (chkTotalAlloc.Checked)
                RefreshByTotalHour(idWhere);
        }

        /// <summary>
        /// 根據分課表編號更新場地清單
        /// </summary>
        /// <param name="EventID">分課表編號</param>
        private void RefreshEvent(string EventID)
        {
            //目前先所有更新，暫不支援單筆更新
            RefreshAll();
        }

        /// <summary>
        /// 儲存分課表的場地編號
        /// </summary>
        /// <param name="EventID">分課表編號</param>
        private void SaveWhere(string EventID)
        {
            idSaveWhere = schLocal.CEvents[EventID].ClassID;
        }

        /// <summary>
        /// 根據場地編號更新場地清單
        /// </summary>
        private void RefreshSaveWhere()
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
            Node SelectNode = nodeTree.SelectedNode;

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
            MainFormBL.Instance.OpenClassroomEventsView(Constants.evCustom, string.Empty, "待處理");
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
        /// 選取節點
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nodeTree_AfterNodeSelect(object sender, AdvTreeNodeEventArgs e)
        {
            if (e.Node == null)
                return;

            string AssocID = e.Node.TagString;

            if (!string.IsNullOrEmpty(AssocID))
            {
                if (AssocID.StartsWith("所有"))
                {
                    MainFormBL.Instance.OpenClassroomEventsView(Constants.evAll, string.Empty, "所有");
                    MainFormBL.Instance.ClearClassroomDefaultSchedule();
                }
                else
                {
                    MainFormBL.Instance.OpenClassroomEventsView(Constants.evWhere, AssocID.Equals("無") ? string.Empty : AssocID, e.Node.Text);

                    string[] IDs = AssocID.Split(new char[] { ';' });

                    if (!AssocID.Equals("無") && IDs.Length == 1)
                        MainFormBL.Instance.OpenClassroomSchedule(Constants.lvWhere, AssocID);
                    else if (AssocID.Equals("無"))
                        MainFormBL.Instance.ClearClassroomDefaultSchedule();
                }
            }
        }
        #endregion
    }
}