namespace ischedule
{
    partial class usrTeacherList
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtSearch = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chkName = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkTotalHour = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkUnAlloc = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkWhat = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.btnAddToTemp = new DevComponents.DotNetBar.ButtonX();
            this.panel3 = new System.Windows.Forms.Panel();
            this.treeWho = new DevComponents.AdvTree.AdvTree();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuOpenNewLPView = new System.Windows.Forms.ToolStripMenuItem();
            this.menuExpand = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCollapse = new System.Windows.Forms.ToolStripMenuItem();
            this.node1 = new DevComponents.AdvTree.Node();
            this.nodeConnector1 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeWho)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.txtSearch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 25);
            this.panel1.TabIndex = 2;
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
            this.txtSearch.WatermarkText = "依教師姓名搜尋";
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.chkName);
            this.panel2.Controls.Add(this.chkTotalHour);
            this.panel2.Controls.Add(this.chkUnAlloc);
            this.panel2.Controls.Add(this.chkWhat);
            this.panel2.Controls.Add(this.btnAddToTemp);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 380);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 120);
            this.panel2.TabIndex = 3;
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
            this.chkName.Text = "按照姓名";
            // 
            // chkTotalHour
            // 
            // 
            // 
            // 
            this.chkTotalHour.BackgroundStyle.Class = "";
            this.chkTotalHour.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkTotalHour.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.chkTotalHour.Location = new System.Drawing.Point(11, 85);
            this.chkTotalHour.Name = "chkTotalHour";
            this.chkTotalHour.Size = new System.Drawing.Size(100, 23);
            this.chkTotalHour.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkTotalHour.TabIndex = 4;
            this.chkTotalHour.Text = "按照總時數";
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
            // chkWhat
            // 
            // 
            // 
            // 
            this.chkWhat.BackgroundStyle.Class = "";
            this.chkWhat.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkWhat.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.chkWhat.Location = new System.Drawing.Point(11, 45);
            this.chkWhat.Name = "chkWhat";
            this.chkWhat.Size = new System.Drawing.Size(100, 23);
            this.chkWhat.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkWhat.TabIndex = 2;
            this.chkWhat.Text = "按照科目";
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
            // panel3
            // 
            this.panel3.Controls.Add(this.treeWho);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 25);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(200, 355);
            this.panel3.TabIndex = 4;
            // 
            // treeWho
            // 
            this.treeWho.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.treeWho.AllowDrop = true;
            this.treeWho.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.treeWho.BackgroundStyle.Class = "TreeBorderKey";
            this.treeWho.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.treeWho.ContextMenuStrip = this.contextMenuStrip1;
            this.treeWho.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeWho.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.treeWho.Location = new System.Drawing.Point(0, 0);
            this.treeWho.MultiSelect = true;
            this.treeWho.Name = "treeWho";
            this.treeWho.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.node1});
            this.treeWho.NodesConnector = this.nodeConnector1;
            this.treeWho.NodeStyle = this.elementStyle1;
            this.treeWho.PathSeparator = ";";
            this.treeWho.Size = new System.Drawing.Size(200, 355);
            this.treeWho.Styles.Add(this.elementStyle1);
            this.treeWho.TabIndex = 0;
            this.treeWho.Text = "advTree1";
            this.treeWho.AfterNodeSelect += new DevComponents.AdvTree.AdvTreeNodeEventHandler(this.treeWho_AfterNodeSelect);
            this.treeWho.NodeMouseDown += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.treeWho_NodeMouseDown);
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
            // usrTeacherList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "usrTeacherList";
            this.Size = new System.Drawing.Size(200, 500);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeWho)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private DevComponents.DotNetBar.Controls.TextBoxX txtSearch;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private DevComponents.AdvTree.AdvTree treeWho;
        private DevComponents.AdvTree.Node node1;
        private DevComponents.AdvTree.NodeConnector nodeConnector1;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
        private DevComponents.DotNetBar.ButtonX btnAddToTemp;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkWhat;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkUnAlloc;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkTotalHour;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuOpenNewLPView;
        private System.Windows.Forms.ToolStripMenuItem menuExpand;
        private System.Windows.Forms.ToolStripMenuItem menuCollapse;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkName;

    }
}
