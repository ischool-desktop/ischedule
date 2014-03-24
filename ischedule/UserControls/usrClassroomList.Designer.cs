namespace ischedule
{
    partial class usrClassroomList
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.chkCapacity = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.panel3 = new System.Windows.Forms.Panel();
            this.nodeTree = new DevComponents.AdvTree.AdvTree();
            this.node1 = new DevComponents.AdvTree.Node();
            this.nodeConnector1 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
            this.txtSearch = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chkName = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkTotalAlloc = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkUnAlloc = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.btnAddToTemp = new DevComponents.DotNetBar.ButtonX();
            this.panel1 = new System.Windows.Forms.Panel();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuOpenNewLPView = new System.Windows.Forms.ToolStripMenuItem();
            this.menuExpand = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCollapse = new System.Windows.Forms.ToolStripMenuItem();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nodeTree)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkCapacity
            // 
            // 
            // 
            // 
            this.chkCapacity.BackgroundStyle.Class = "";
            this.chkCapacity.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkCapacity.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.chkCapacity.Location = new System.Drawing.Point(11, 45);
            this.chkCapacity.Name = "chkCapacity";
            this.chkCapacity.Size = new System.Drawing.Size(100, 23);
            this.chkCapacity.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkCapacity.TabIndex = 2;
            this.chkCapacity.Text = "按照容納數";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.nodeTree);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 25);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(200, 360);
            this.panel3.TabIndex = 10;
            // 
            // nodeTree
            // 
            this.nodeTree.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.nodeTree.AllowDrop = true;
            this.nodeTree.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.nodeTree.BackgroundStyle.Class = "TreeBorderKey";
            this.nodeTree.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.nodeTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nodeTree.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.nodeTree.Location = new System.Drawing.Point(0, 0);
            this.nodeTree.MultiSelect = true;
            this.nodeTree.Name = "nodeTree";
            this.nodeTree.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.node1});
            this.nodeTree.NodesConnector = this.nodeConnector1;
            this.nodeTree.NodeStyle = this.elementStyle1;
            this.nodeTree.PathSeparator = ";";
            this.nodeTree.Size = new System.Drawing.Size(200, 360);
            this.nodeTree.Styles.Add(this.elementStyle1);
            this.nodeTree.TabIndex = 0;
            this.nodeTree.Text = "advTree1";
            this.nodeTree.AfterNodeSelect += new DevComponents.AdvTree.AdvTreeNodeEventHandler(this.nodeTree_AfterNodeSelect);
            this.nodeTree.NodeMouseDown += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.treeWho_NodeMouseDown);
            // 
            // node1
            // 
            this.node1.Expanded = true;
            this.node1.Name = "node1";
            this.node1.Text = "node1";
            // 
            // nodeConnector1
            // 
            this.nodeConnector1.LineColor = System.Drawing.SystemColors.ControlText;
            // 
            // elementStyle1
            // 
            this.elementStyle1.Class = "";
            this.elementStyle1.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.elementStyle1.Name = "elementStyle1";
            this.elementStyle1.TextColor = System.Drawing.SystemColors.ControlText;
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.txtSearch.Border.Class = "TextBoxBorder";
            this.txtSearch.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtSearch.Location = new System.Drawing.Point(11, 3);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(177, 22);
            this.txtSearch.TabIndex = 2;
            this.txtSearch.WatermarkText = "依場地名稱搜尋";
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.chkName);
            this.panel2.Controls.Add(this.chkTotalAlloc);
            this.panel2.Controls.Add(this.chkUnAlloc);
            this.panel2.Controls.Add(this.chkCapacity);
            this.panel2.Controls.Add(this.btnAddToTemp);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 385);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 115);
            this.panel2.TabIndex = 9;
            // 
            // chkName
            // 
            // 
            // 
            // 
            this.chkName.BackgroundStyle.Class = "";
            this.chkName.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkName.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.chkName.Checked = true;
            this.chkName.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkName.CheckValue = "Y";
            this.chkName.Location = new System.Drawing.Point(11, 25);
            this.chkName.Name = "chkName";
            this.chkName.Size = new System.Drawing.Size(100, 23);
            this.chkName.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkName.TabIndex = 5;
            this.chkName.Text = "按照名稱";
            // 
            // chkTotalAlloc
            // 
            // 
            // 
            // 
            this.chkTotalAlloc.BackgroundStyle.Class = "";
            this.chkTotalAlloc.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkTotalAlloc.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.chkTotalAlloc.Location = new System.Drawing.Point(11, 85);
            this.chkTotalAlloc.Name = "chkTotalAlloc";
            this.chkTotalAlloc.Size = new System.Drawing.Size(100, 23);
            this.chkTotalAlloc.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkTotalAlloc.TabIndex = 4;
            this.chkTotalAlloc.Text = "按照校別";
            // 
            // chkUnAlloc
            // 
            // 
            // 
            // 
            this.chkUnAlloc.BackgroundStyle.Class = "";
            this.chkUnAlloc.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkUnAlloc.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.chkUnAlloc.Location = new System.Drawing.Point(11, 65);
            this.chkUnAlloc.Name = "chkUnAlloc";
            this.chkUnAlloc.Size = new System.Drawing.Size(100, 23);
            this.chkUnAlloc.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkUnAlloc.TabIndex = 3;
            this.chkUnAlloc.Text = "按照未排時數";
            // 
            // btnAddToTemp
            // 
            this.btnAddToTemp.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAddToTemp.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnAddToTemp.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAddToTemp.Location = new System.Drawing.Point(0, 0);
            this.btnAddToTemp.Name = "btnAddToTemp";
            this.btnAddToTemp.Size = new System.Drawing.Size(200, 23);
            this.btnAddToTemp.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnAddToTemp.TabIndex = 1;
            this.btnAddToTemp.Text = "待處理分課表";
            this.btnAddToTemp.Click += new System.EventHandler(this.btnAddToTemp_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtSearch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 25);
            this.panel1.TabIndex = 8;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuOpenNewLPView,
            this.menuExpand,
            this.menuCollapse});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(131, 70);
            // 
            // menuOpenNewLPView
            // 
            this.menuOpenNewLPView.Name = "menuOpenNewLPView";
            this.menuOpenNewLPView.Size = new System.Drawing.Size(130, 22);
            this.menuOpenNewLPView.Text = "開新功課表";
            // 
            // menuExpand
            // 
            this.menuExpand.Name = "menuExpand";
            this.menuExpand.Size = new System.Drawing.Size(130, 22);
            this.menuExpand.Text = "展開";
            // 
            // menuCollapse
            // 
            this.menuCollapse.Name = "menuCollapse";
            this.menuCollapse.Size = new System.Drawing.Size(130, 22);
            this.menuCollapse.Text = "收合";
            // 
            // usrClassroomList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "usrClassroomList";
            this.Size = new System.Drawing.Size(200, 500);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nodeTree)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.CheckBoxX chkCapacity;
        private System.Windows.Forms.Panel panel3;
        private DevComponents.AdvTree.AdvTree nodeTree;
        private DevComponents.AdvTree.Node node1;
        private DevComponents.AdvTree.NodeConnector nodeConnector1;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
        private DevComponents.DotNetBar.Controls.TextBoxX txtSearch;
        private System.Windows.Forms.Panel panel2;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkTotalAlloc;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkUnAlloc;
        private DevComponents.DotNetBar.ButtonX btnAddToTemp;
        private System.Windows.Forms.Panel panel1;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkName;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuOpenNewLPView;
        private System.Windows.Forms.ToolStripMenuItem menuExpand;
        private System.Windows.Forms.ToolStripMenuItem menuCollapse;
    }
}
