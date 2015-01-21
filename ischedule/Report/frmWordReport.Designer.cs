namespace ischedule
{
    partial class frmWordReport
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl1 = new DevComponents.DotNetBar.TabControl();
            this.tabControlPanel2 = new DevComponents.DotNetBar.TabControlPanel();
            this.btnExit = new DevComponents.DotNetBar.ButtonX();
            this.btnPrint = new DevComponents.DotNetBar.ButtonX();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lnkUploadCustom = new DevComponents.DotNetBar.LabelX();
            this.lnkViewCustom = new DevComponents.DotNetBar.LabelX();
            this.lnkViewDefault = new DevComponents.DotNetBar.LabelX();
            this.rdoCustomize = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.rdoDefualt = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgv = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.colPrintItem = new DevComponents.DotNetBar.Controls.DataGridViewComboBoxExColumn();
            this.txtPeriod = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkTeacherBusyDesc = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkMergeTimeTable = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.cboTimeTable = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.tabPrint = new DevComponents.DotNetBar.TabItem(this.components);
            this.tabControlPanel1 = new DevComponents.DotNetBar.TabControlPanel();
            this.grdNameList = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.colID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabSelectName = new DevComponents.DotNetBar.TabItem(this.components);
            this.chkOdd = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkEven = new DevComponents.DotNetBar.Controls.CheckBoxX();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabControlPanel2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tabControlPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdNameList)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.BackColor = System.Drawing.Color.Transparent;
            this.tabControl1.CanReorderTabs = true;
            this.tabControl1.Controls.Add(this.tabControlPanel2);
            this.tabControl1.Controls.Add(this.tabControlPanel1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedTabFont = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold);
            this.tabControl1.SelectedTabIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(344, 524);
            this.tabControl1.TabIndex = 7;
            this.tabControl1.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox;
            this.tabControl1.Tabs.Add(this.tabSelectName);
            this.tabControl1.Tabs.Add(this.tabPrint);
            this.tabControl1.Text = "tabControl1";
            // 
            // tabControlPanel2
            // 
            this.tabControlPanel2.Controls.Add(this.btnExit);
            this.tabControlPanel2.Controls.Add(this.btnPrint);
            this.tabControlPanel2.Controls.Add(this.groupBox3);
            this.tabControlPanel2.Controls.Add(this.groupBox2);
            this.tabControlPanel2.Controls.Add(this.groupBox1);
            this.tabControlPanel2.Controls.Add(this.cboTimeTable);
            this.tabControlPanel2.Controls.Add(this.labelX1);
            this.tabControlPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel2.Location = new System.Drawing.Point(0, 29);
            this.tabControlPanel2.Name = "tabControlPanel2";
            this.tabControlPanel2.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel2.Size = new System.Drawing.Size(344, 495);
            this.tabControlPanel2.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(179)))), ((int)(((byte)(231)))));
            this.tabControlPanel2.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(237)))), ((int)(((byte)(254)))));
            this.tabControlPanel2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel2.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.tabControlPanel2.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right) 
            | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.tabControlPanel2.Style.GradientAngle = 90;
            this.tabControlPanel2.TabIndex = 2;
            this.tabControlPanel2.TabItem = this.tabPrint;
            // 
            // btnExit
            // 
            this.btnExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExit.Location = new System.Drawing.Point(245, 460);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(66, 23);
            this.btnExit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnExit.TabIndex = 24;
            this.btnExit.Text = "離開";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnPrint.Location = new System.Drawing.Point(180, 460);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(59, 23);
            this.btnPrint.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnPrint.TabIndex = 23;
            this.btnPrint.Text = "列印";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
            this.groupBox3.Controls.Add(this.lnkUploadCustom);
            this.groupBox3.Controls.Add(this.lnkViewCustom);
            this.groupBox3.Controls.Add(this.lnkViewDefault);
            this.groupBox3.Controls.Add(this.rdoCustomize);
            this.groupBox3.Controls.Add(this.rdoDefualt);
            this.groupBox3.ForeColor = System.Drawing.SystemColors.Desktop;
            this.groupBox3.Location = new System.Drawing.Point(29, 372);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(282, 82);
            this.groupBox3.TabIndex = 18;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "範本設定";
            // 
            // lnkUploadCustom
            // 
            this.lnkUploadCustom.AutoSize = true;
            // 
            // 
            // 
            this.lnkUploadCustom.BackgroundStyle.Class = "";
            this.lnkUploadCustom.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lnkUploadCustom.Location = new System.Drawing.Point(153, 51);
            this.lnkUploadCustom.Name = "lnkUploadCustom";
            this.lnkUploadCustom.Size = new System.Drawing.Size(34, 21);
            this.lnkUploadCustom.TabIndex = 4;
            this.lnkUploadCustom.Text = "上傳";
            // 
            // lnkViewCustom
            // 
            this.lnkViewCustom.AutoSize = true;
            // 
            // 
            // 
            this.lnkViewCustom.BackgroundStyle.Class = "";
            this.lnkViewCustom.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lnkViewCustom.Location = new System.Drawing.Point(104, 51);
            this.lnkViewCustom.Name = "lnkViewCustom";
            this.lnkViewCustom.Size = new System.Drawing.Size(34, 21);
            this.lnkViewCustom.TabIndex = 3;
            this.lnkViewCustom.Text = "檢視";
            // 
            // lnkViewDefault
            // 
            this.lnkViewDefault.AutoSize = true;
            // 
            // 
            // 
            this.lnkViewDefault.BackgroundStyle.Class = "";
            this.lnkViewDefault.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lnkViewDefault.Location = new System.Drawing.Point(104, 26);
            this.lnkViewDefault.Name = "lnkViewDefault";
            this.lnkViewDefault.Size = new System.Drawing.Size(34, 21);
            this.lnkViewDefault.TabIndex = 2;
            this.lnkViewDefault.Text = "檢視";
            // 
            // rdoCustomize
            // 
            this.rdoCustomize.AutoSize = true;
            // 
            // 
            // 
            this.rdoCustomize.BackgroundStyle.Class = "";
            this.rdoCustomize.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.rdoCustomize.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.rdoCustomize.Location = new System.Drawing.Point(6, 51);
            this.rdoCustomize.Name = "rdoCustomize";
            this.rdoCustomize.Size = new System.Drawing.Size(80, 21);
            this.rdoCustomize.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.rdoCustomize.TabIndex = 1;
            this.rdoCustomize.Text = "自訂範本";
            // 
            // rdoDefualt
            // 
            // 
            // 
            // 
            this.rdoDefualt.BackgroundStyle.Class = "";
            this.rdoDefualt.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.rdoDefualt.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.rdoDefualt.Checked = true;
            this.rdoDefualt.CheckState = System.Windows.Forms.CheckState.Checked;
            this.rdoDefualt.CheckValue = "Y";
            this.rdoDefualt.Location = new System.Drawing.Point(6, 24);
            this.rdoDefualt.Name = "rdoDefualt";
            this.rdoDefualt.Size = new System.Drawing.Size(100, 23);
            this.rdoDefualt.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.rdoDefualt.TabIndex = 0;
            this.rdoDefualt.Text = "預設範本";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.dgv);
            this.groupBox2.Controls.Add(this.txtPeriod);
            this.groupBox2.Controls.Add(this.labelX2);
            this.groupBox2.ForeColor = System.Drawing.SystemColors.Desktop;
            this.groupBox2.Location = new System.Drawing.Point(29, 158);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(282, 208);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "顯示設定";
            // 
            // dgv
            // 
            this.dgv.BackgroundColor = System.Drawing.Color.White;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPrintItem});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.Desktop;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.Desktop;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgv.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgv.Location = new System.Drawing.Point(15, 24);
            this.dgv.Name = "dgv";
            this.dgv.RowTemplate.Height = 24;
            this.dgv.Size = new System.Drawing.Size(261, 143);
            this.dgv.TabIndex = 25;
            // 
            // colPrintItem
            // 
            this.colPrintItem.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colPrintItem.DisplayMember = "Text";
            this.colPrintItem.DropDownHeight = 106;
            this.colPrintItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.colPrintItem.DropDownWidth = 121;
            this.colPrintItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colPrintItem.HeaderText = "列印順序";
            this.colPrintItem.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.colPrintItem.IntegralHeight = false;
            this.colPrintItem.ItemHeight = 17;
            this.colPrintItem.Name = "colPrintItem";
            this.colPrintItem.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colPrintItem.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // txtPeriod
            // 
            // 
            // 
            // 
            this.txtPeriod.Border.Class = "TextBoxBorder";
            this.txtPeriod.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtPeriod.Location = new System.Drawing.Point(89, 173);
            this.txtPeriod.Name = "txtPeriod";
            this.txtPeriod.Size = new System.Drawing.Size(34, 25);
            this.txtPeriod.TabIndex = 8;
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelX2.Location = new System.Drawing.Point(10, 173);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(74, 21);
            this.labelX2.TabIndex = 7;
            this.labelX2.Text = "顯示節次：";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.chkEven);
            this.groupBox1.Controls.Add(this.chkOdd);
            this.groupBox1.Controls.Add(this.chkTeacherBusyDesc);
            this.groupBox1.Controls.Add(this.chkMergeTimeTable);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.Desktop;
            this.groupBox1.Location = new System.Drawing.Point(29, 56);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(282, 85);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "選項設定";
            // 
            // chkTeacherBusyDesc
            // 
            this.chkTeacherBusyDesc.AutoSize = true;
            // 
            // 
            // 
            this.chkTeacherBusyDesc.BackgroundStyle.Class = "";
            this.chkTeacherBusyDesc.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkTeacherBusyDesc.Location = new System.Drawing.Point(15, 53);
            this.chkTeacherBusyDesc.Name = "chkTeacherBusyDesc";
            this.chkTeacherBusyDesc.Size = new System.Drawing.Size(147, 21);
            this.chkTeacherBusyDesc.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkTeacherBusyDesc.TabIndex = 1;
            this.chkTeacherBusyDesc.Text = "顯示教師不排課時段";
            // 
            // chkMergeTimeTable
            // 
            // 
            // 
            // 
            this.chkMergeTimeTable.BackgroundStyle.Class = "";
            this.chkMergeTimeTable.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkMergeTimeTable.Location = new System.Drawing.Point(15, 24);
            this.chkMergeTimeTable.Name = "chkMergeTimeTable";
            this.chkMergeTimeTable.Size = new System.Drawing.Size(100, 23);
            this.chkMergeTimeTable.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkMergeTimeTable.TabIndex = 0;
            this.chkMergeTimeTable.Text = "合併時間表";
            // 
            // cboTimeTable
            // 
            this.cboTimeTable.DisplayMember = "Name";
            this.cboTimeTable.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboTimeTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTimeTable.FormattingEnabled = true;
            this.cboTimeTable.ItemHeight = 19;
            this.cboTimeTable.Location = new System.Drawing.Point(85, 14);
            this.cboTimeTable.Name = "cboTimeTable";
            this.cboTimeTable.Size = new System.Drawing.Size(170, 25);
            this.cboTimeTable.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cboTimeTable.TabIndex = 15;
            this.cboTimeTable.ValueMember = "TimeTableID";
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(28, 14);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(60, 21);
            this.labelX1.TabIndex = 14;
            this.labelX1.Text = "時間表：";
            // 
            // tabPrint
            // 
            this.tabPrint.AttachedControl = this.tabControlPanel2;
            this.tabPrint.Name = "tabPrint";
            this.tabPrint.Text = "步驟二：列印選項";
            this.tabPrint.Click += new System.EventHandler(this.tabPrint_Click);
            // 
            // tabControlPanel1
            // 
            this.tabControlPanel1.Controls.Add(this.grdNameList);
            this.tabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel1.Location = new System.Drawing.Point(0, 29);
            this.tabControlPanel1.Name = "tabControlPanel1";
            this.tabControlPanel1.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel1.Size = new System.Drawing.Size(344, 495);
            this.tabControlPanel1.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(179)))), ((int)(((byte)(231)))));
            this.tabControlPanel1.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(237)))), ((int)(((byte)(254)))));
            this.tabControlPanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel1.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.tabControlPanel1.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right) 
            | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.tabControlPanel1.Style.GradientAngle = 90;
            this.tabControlPanel1.TabIndex = 1;
            this.tabControlPanel1.TabItem = this.tabSelectName;
            // 
            // grdNameList
            // 
            this.grdNameList.AllowUserToAddRows = false;
            this.grdNameList.AllowUserToDeleteRows = false;
            this.grdNameList.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.grdNameList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdNameList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colID,
            this.colName});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdNameList.DefaultCellStyle = dataGridViewCellStyle2;
            this.grdNameList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.grdNameList.Location = new System.Drawing.Point(10, 16);
            this.grdNameList.Name = "grdNameList";
            this.grdNameList.ReadOnly = true;
            this.grdNameList.RowTemplate.Height = 24;
            this.grdNameList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdNameList.Size = new System.Drawing.Size(323, 402);
            this.grdNameList.TabIndex = 0;
            // 
            // colID
            // 
            this.colID.HeaderText = "系統編號";
            this.colID.Name = "colID";
            this.colID.ReadOnly = true;
            this.colID.Visible = false;
            // 
            // colName
            // 
            this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colName.HeaderText = "名稱";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // tabSelectName
            // 
            this.tabSelectName.AttachedControl = this.tabControlPanel1;
            this.tabSelectName.Name = "tabSelectName";
            this.tabSelectName.Text = "步驟一：選擇名單";
            // 
            // chkOdd
            // 
            // 
            // 
            // 
            this.chkOdd.BackgroundStyle.Class = "";
            this.chkOdd.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkOdd.Location = new System.Drawing.Point(166, 24);
            this.chkOdd.Name = "chkOdd";
            this.chkOdd.Size = new System.Drawing.Size(100, 23);
            this.chkOdd.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkOdd.TabIndex = 2;
            this.chkOdd.Text = "單週列印";
            // 
            // chkEven
            // 
            // 
            // 
            // 
            this.chkEven.BackgroundStyle.Class = "";
            this.chkEven.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkEven.Location = new System.Drawing.Point(166, 51);
            this.chkEven.Name = "chkEven";
            this.chkEven.Size = new System.Drawing.Size(100, 23);
            this.chkEven.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkEven.TabIndex = 3;
            this.chkEven.Text = "雙週列印";
            // 
            // frmWordReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 524);
            this.Controls.Add(this.tabControl1);
            this.DoubleBuffered = true;
            this.Name = "frmWordReport";
            this.Text = "列印功課表(Word版)";
            this.Load += new System.EventHandler(this.frmWordReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tabControl1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabControlPanel2.ResumeLayout(false);
            this.tabControlPanel2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControlPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdNameList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.TabControl tabControl1;
        private DevComponents.DotNetBar.TabControlPanel tabControlPanel1;
        private DevComponents.DotNetBar.TabItem tabSelectName;
        private DevComponents.DotNetBar.TabControlPanel tabControlPanel2;
        private System.Windows.Forms.GroupBox groupBox3;
        private DevComponents.DotNetBar.LabelX lnkUploadCustom;
        private DevComponents.DotNetBar.LabelX lnkViewCustom;
        private DevComponents.DotNetBar.LabelX lnkViewDefault;
        private DevComponents.DotNetBar.Controls.CheckBoxX rdoCustomize;
        private DevComponents.DotNetBar.Controls.CheckBoxX rdoDefualt;
        private System.Windows.Forms.GroupBox groupBox2;
        private DevComponents.DotNetBar.Controls.TextBoxX txtPeriod;
        private DevComponents.DotNetBar.LabelX labelX2;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkTeacherBusyDesc;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkMergeTimeTable;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cboTimeTable;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.TabItem tabPrint;
        private DevComponents.DotNetBar.ButtonX btnExit;
        private DevComponents.DotNetBar.ButtonX btnPrint;
        private DevComponents.DotNetBar.Controls.DataGridViewX grdNameList;
        private System.Windows.Forms.DataGridViewTextBoxColumn colID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private DevComponents.DotNetBar.Controls.DataGridViewX dgv;
        private DevComponents.DotNetBar.Controls.DataGridViewComboBoxExColumn colPrintItem;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkEven;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkOdd;

    }
}