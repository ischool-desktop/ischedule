using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevComponents.AdvTree;
using Sunset.Data;

namespace ischedule
{
    /// <summary>
    /// 班級清單
    /// </summary>
    public partial class usrClassList : UserControl
    {
        private Scheduler schLocal = Scheduler.Instance;
        private string idSaveWhom = string.Empty;

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public usrClassList()
        {
            InitializeComponent();

            //自動排程完成，更新所有內容
            schLocal.AutoScheduleComplete += (sender, e) => RefreshAll();

            //刪除之前將班級編號先儲存起來
            schLocal.EventBeforeDelete += (sender, e) => SaveWhom(e.EventID);

            //刪除後根據儲存的班級編號更新班級清單
            schLocal.EventDeleted += (sender, e) => RefreshSaveWhom();

            //新增分課表更新班級清單
            schLocal.EventInserted += (sender, e) => RefreshEvent(e.EventID);

            //分課表排定後更新班級清單
            schLocal.EventScheduled += (sender, e) => RefreshEvent(e.EventID);

            //釋放分課表更新所有班級清單
            schLocal.EventsFreed += (sender, e) => RefreshAll();

            //更新所有班級清單
            RefreshAll();

            chkName.CheckedChanged += (sender, e) => RefreshAll();
            chkWhat.CheckedChanged += (sender, e) => RefreshAll();
            chkTotalHour.CheckedChanged += (sender, e) => RefreshAll();
            chkUnAlloc.CheckedChanged += (sender, e) => RefreshAll();

            btnAddToTemp.Click += (sender, e) => MainFormBL.Instance.OpenClassEventsView(Constants.evCustom, string.Empty, "待處理");
            
            menuOpenNewLPView.Click += (sender, e) =>
            {
                string AssocID = GetSelectedValue();

                if (!string.IsNullOrEmpty(AssocID))
                    if (!AssocID.StartsWith("所有"))
                        MainFormBL.Instance.OpenClassSchedule(Constants.lvWhom, AssocID, true);
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

            MainFormBL.Instance.ClassEventsView.TempUpdate += (sender, e) => btnAddToTemp.Text = "待處理分課表(" + e.TotCount + ")";
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
        /// <param name="Whom"></param>
        /// <returns></returns>
        private bool IsAddWhom(string Whom)
        {
            if (!string.IsNullOrWhiteSpace(Whom) && !Whom.Equals("無"))
            {
                //若搜尋字串為空白則全部顯示
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                    return true;
                else
                    return Whom.Contains(txtSearch.Text);
            }
            else
                return false;
        }

        /// <summary>
        /// 根據年級更新班級表，同時考量到關鍵字搜尋
        /// </summary>
        private void GradeYear(string idWhom)
        {
            int Total = 0;

            //根據分課的年級進行分類
            SortedDictionary<string, List<string>> GradeYearWhoms = new SortedDictionary<string, List<string>>();

            foreach (Class vWhom in schLocal.Classes)
            {
                //取得年級
                string GradeYear = vWhom.GradeYear;

                //若年級名稱不在字典中則新增
                if (!GradeYearWhoms.ContainsKey(GradeYear))
                    GradeYearWhoms.Add(GradeYear, new List<string>());

                if (IsAddWhom(vWhom.ClassID))
                    if (!GradeYearWhoms[GradeYear].Contains(vWhom.ClassID))
                        GradeYearWhoms[GradeYear].Add(vWhom.ClassID);
            }

            nodeTree.Nodes.Clear();

            Node nodeRoot = new Node("所有年級");
            nodeRoot.TagString = "所有年級";
            Node nodeNull = new Node("無班級分課");
            nodeNull.TagString = "無";

            nodeTree.Nodes.Add(nodeRoot);
            nodeTree.Nodes.Add(nodeNull);

            foreach (string GradeYear in GradeYearWhoms.Keys)
            {
                Node nodeGradeYear = new Node(GradeYear);
                List<string> Names = new List<string>();

                foreach (string WhomID in GradeYearWhoms[GradeYear])
                {
                    if (schLocal.Classes.Exists(WhomID))
                    {
                        Class whomPaint = schLocal.Classes[WhomID];

                        int UnAllocHour = whomPaint.TotalHour - whomPaint.AllocHour;

                        Node nodeWho = new Node(whomPaint.Name + "(" + UnAllocHour + "/" + whomPaint.TotalHour + ")");
                        nodeWho.TagString = whomPaint.ClassID;
                        nodeGradeYear.Nodes.Add(nodeWho);
                        Names.Add(whomPaint.ClassID);
                        Total++;
                    }
                }

                if (nodeGradeYear.Nodes.Count > 0)
                {
                    nodeGradeYear.Text = nodeGradeYear.Text + "(" + nodeGradeYear.Nodes.Count + ")";
                    nodeGradeYear.TagString = string.Join(";", Names.ToArray());
                    nodeRoot.Nodes.Add(nodeGradeYear);
                    nodeRoot.Expand();
                }
            }

            nodeRoot.Text = nodeRoot.Text + "(" + (schLocal.Classes.Count - 1) + ")";
            nodeRoot.ExpandAll();
        }

        /// <summary>
        /// 根據未排時數更新班級列表，同時考量到關鍵字搜尋
        /// </summary>
        private void RefreshByUnAlloc(string idWho)
        {
            int Total = 0;

            //根據分課的未排時數進行分類
            SortedDictionary<int, List<string>> UnAllocWhos = new SortedDictionary<int, List<string>>();

            foreach (Class Whom in schLocal.Classes)
            {
                if (Whom.Name.Equals("無"))
                    continue;

                int UnAlloc = Whom.TotalHour - Whom.AllocHour;

                //若未排時數不在字典中則新增
                if (!UnAllocWhos.ContainsKey(UnAlloc))
                    UnAllocWhos.Add(UnAlloc, new List<string>());

                if (IsAddWhom(Whom.ClassID))
                    if (!UnAllocWhos[UnAlloc].Contains(Whom.ClassID))
                        UnAllocWhos[UnAlloc].Add(Whom.ClassID);
            }

            nodeTree.Nodes.Clear();

            Node nodeRoot = new Node("所有班級");
            nodeRoot.TagString = "所有班級";

            Node nodeNull = new Node("無班級分課");
            nodeNull.TagString = "無";

            nodeTree.Nodes.Add(nodeRoot);
            nodeTree.Nodes.Add(nodeNull);

            foreach (int vUnAlloc in UnAllocWhos.Keys.ToList().OrderByDescending(x => x))
            {
                Node UnAlloc = new Node("" + vUnAlloc);
                List<string> Names = new List<string>();

                foreach (string vWhomID in UnAllocWhos[vUnAlloc])
                {
                    if (schLocal.Classes.Exists(vWhomID))
                    {
                        Class whomPaint = schLocal.Classes[vWhomID];

                        int UnAllocHour = whomPaint.TotalHour - whomPaint.AllocHour;

                        Node nodeWhom = new Node(whomPaint.Name + "(" + UnAllocHour + "/" + whomPaint.TotalHour + ")");
                        nodeWhom.TagString = whomPaint.ClassID;
                        Names.Add(whomPaint.ClassID);
                        UnAlloc.Nodes.Add(nodeWhom);
                        Total++;
                    }
                }

                if (UnAlloc.Nodes.Count > 0)
                {
                    UnAlloc.Text = UnAlloc.Text + "(" + UnAlloc.Nodes.Count + ")";
                    UnAlloc.TagString = string.Join(";", Names.ToArray());
                    nodeRoot.Nodes.Add(UnAlloc);
                    nodeRoot.Expand();
                }
            }

            nodeRoot.Text = nodeRoot.Text + "(" + (schLocal.Classes.Count - 1) + ")";
            nodeRoot.ExpandAll();
        }

        /// <summary>
        /// 根據總時數更新班級列表，同時考量到關鍵字搜尋
        /// </summary>
        private void RefreshByTotalHour(string idWhom)
        {
            int Total = 0;

            //根據分課的總時數進行分類
            SortedDictionary<int, List<string>> TotalWhoms = new SortedDictionary<int, List<string>>();

            foreach (Class Whom in schLocal.Classes)
            {
                if (Whom.Name.Equals("無"))
                    continue;

                int TotalHour = Whom.TotalHour;

                if (!TotalWhoms.ContainsKey(TotalHour))
                    TotalWhoms.Add(TotalHour, new List<string>());

                if (IsAddWhom(Whom.Name))
                    if (!TotalWhoms[TotalHour].Contains(Whom.ClassID))
                        TotalWhoms[TotalHour].Add(Whom.ClassID);
            }

            nodeTree.Nodes.Clear();

            Node nodeRoot = new Node("所有班級");
            nodeRoot.TagString = "所有班級";

            Node nodeNull = new Node("無班級分課");
            nodeNull.TagString = "無";

            nodeTree.Nodes.Add(nodeRoot);
            nodeTree.Nodes.Add(nodeNull);

            foreach (int vTotalHour in TotalWhoms.Keys.ToList().OrderByDescending(x => x))
            {
                Node nodeTotalHour = new Node("" + vTotalHour);
                List<string> Names = new List<string>();

                foreach (string vWhomID in TotalWhoms[vTotalHour])
                {
                    if (schLocal.Classes.Exists(vWhomID))
                    {
                        Class whomPaint = schLocal.Classes[vWhomID];

                        int UnAllocHour = whomPaint.TotalHour - whomPaint.AllocHour;

                        Node nodeWhom = new Node(whomPaint.Name + "(" + UnAllocHour + "/" + whomPaint.TotalHour + ")");
                        nodeWhom.TagString = whomPaint.ClassID;
                        Names.Add(whomPaint.ClassID);
                        nodeTotalHour.Nodes.Add(nodeWhom);
                        Total++;
                    }
                }

                if (nodeTotalHour.Nodes.Count > 0)
                {
                    nodeTotalHour.Text = nodeTotalHour.Text + "(" + nodeTotalHour.Nodes.Count + ")";
                    nodeTotalHour.TagString = string.Join(";", Names.ToArray());
                    nodeRoot.Nodes.Add(nodeTotalHour);
                    nodeRoot.Expand();
                }
            }

            nodeRoot.Text = nodeRoot.Text + "(" + (schLocal.Classes.Count - 1) + ")";
            nodeRoot.ExpandAll();
        }

        /// <summary>
        /// 根據班級名稱更新班級列表，同時考量到關鍵字搜尋
        /// </summary>
        private void RefreshByName(string idWhom)
        {
            //根據分課的名稱進行分類
            SortedDictionary<string, string> Names = new SortedDictionary<string, string>();

            foreach (Class Whom in schLocal.Classes)
            {
                int TotalHour = Whom.TotalHour;

                //若班級名稱不在字典中則新增
                if (!Names.ContainsKey(Whom.Name))
                    Names.Add(Whom.Name,Whom.ClassID);
            }

            nodeTree.Nodes.Clear();

            Node nodeAll = new Node("所有班級");
            nodeAll.TagString = "所有班級";

            Node nodeNull = new Node("無班級");
            nodeNull.TagString = "無";

            nodeTree.Nodes.Add(nodeAll);
            nodeTree.Nodes.Add(nodeNull);

            foreach (string WhomName in Names.Keys)
            {
                if (WhomName.Equals("無"))
                    continue;

                if (IsAddWhom(WhomName))
                {
                    Class whomPaint = schLocal.Classes[Names[WhomName]];

                    int UnAllocHour = whomPaint.TotalHour - whomPaint.AllocHour;

                    Node nodeWhom = new Node(whomPaint.Name + "(" + UnAllocHour + "/" + whomPaint.TotalHour + ")");
                    nodeWhom.TagString = whomPaint.ClassID;
                    nodeAll.Nodes.Add(nodeWhom);
                }
            }

            nodeAll.Text = nodeAll.Text + "(" + (schLocal.Classrooms.Count - 1) + ")";
            nodeAll.Expand();
        }

        /// <summary>
        /// 根據班級編號更新班級清單，若傳入空白則更新所有班級清單
        /// </summary>
        /// <param name="idWhom">教師編號</param>
        private void RefreshContent(string idWhom)
        {
            if (chkName.Checked)
                RefreshByName(idWhom);
            else if (chkWhat.Checked)
                GradeYear(idWhom);
            else if (chkUnAlloc.Checked)
                RefreshByUnAlloc(idWhom);
            else if (chkTotalHour.Checked)
                RefreshByTotalHour(idWhom);
        }

        /// <summary>
        /// 根據分課表編號更新班級清單
        /// </summary>
        /// <param name="EventID">分課表編號</param>
        private void RefreshEvent(string EventID)
        {
            //目前先所有更新，暫不支援單筆更新
            RefreshAll();
        }

        /// <summary>
        /// 儲存分課表的班級編號
        /// </summary>
        /// <param name="EventID">分課表編號</param>
        private void SaveWhom(string EventID)
        {
             idSaveWhom = schLocal.CEvents[EventID].ClassID;
        }

        /// <summary>
        /// 根據教師編號更新班級清單
        /// </summary>
        private void RefreshSaveWhom()
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
            MainFormBL.Instance.OpenClassEventsView(Constants.evCustom, string.Empty, "待處理");
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
        /// 節點被選取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nodeTree_AfterNodeSelect(object sender, AdvTreeNodeEventArgs e)
        {
            if (e.Node == null)
                return;

            if (nodeTree.SelectedNodes.Count > 1)
                return;

            string AssocID = e.Node.TagString;

            if (!string.IsNullOrEmpty(AssocID))
            {
                if (AssocID.StartsWith("所有"))
                {
                    MainFormBL.Instance.OpenClassEventsView(Constants.evAll, string.Empty, "所有");
                    MainFormBL.Instance.ClearClassDefaultSchedule();
                }
                else
                {
                    MainFormBL.Instance.OpenClassEventsView(Constants.evWhom, AssocID.Equals("無") ? string.Empty : AssocID, e.Node.Text);

                    string[] IDs = AssocID.Split(new char[] { ';' });

                    if (!AssocID.Equals("無") && IDs.Length == 1)
                        MainFormBL.Instance.OpenClassSchedule(Constants.lvWhom, AssocID);
                    else if (AssocID.Equals("無"))
                        MainFormBL.Instance.ClearClassDefaultSchedule();
                }
            }
        }

        /// <summary>
        /// 取得選取系統編號
        /// </summary>
        /// <returns></returns>
        public List<string> GetSelectedIDs()
        {
            List<string> result = new List<string>();

            foreach (Node vNode in nodeTree.SelectedNodes)
            {
                string AssocID = vNode.TagString;

                if (!string.IsNullOrEmpty(AssocID))
                {
                    if (!AssocID.StartsWith("所有"))
                    {
                        string[] IDs = AssocID.Split(new char[] { ';' });

                        if (!AssocID.Equals("無") && IDs.Length == 1)
                            result.Add(AssocID);
                    }
                }
            }

            return result;
        }
        #endregion
    }
}
