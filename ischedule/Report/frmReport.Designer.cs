//namespace ischedule
//{
//    partial class frmReport
//    {
//        /// <summary>
//        /// Required designer variable.
//        /// </summary>
//        private System.ComponentModel.IContainer components = null;

//        /// <summary>
//        /// Clean up any resources being used.
//        /// </summary>
//        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing && (components != null))
//            {
//                components.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        #region Windows Form Designer generated code

//        /// <summary>
//        /// Required method for Designer support - do not modify
//        /// the contents of this method with the code editor.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            this.labelX1 = new DevComponents.DotNetBar.LabelX();
//            this.cboTimeTable = new DevComponents.DotNetBar.Controls.ComboBoxEx();
//            this.groupBox1 = new System.Windows.Forms.GroupBox();
//            this.chkTeacherBusyDesc = new DevComponents.DotNetBar.Controls.CheckBoxX();
//            this.chkMergeTimeTable = new DevComponents.DotNetBar.Controls.CheckBoxX();
//            this.groupBox2 = new System.Windows.Forms.GroupBox();
//            this.txtPeriod = new DevComponents.DotNetBar.Controls.TextBoxX();
//            this.labelX2 = new DevComponents.DotNetBar.LabelX();
//            this.chkComment = new DevComponents.DotNetBar.Controls.CheckBoxX();
//            this.chkCourseName = new DevComponents.DotNetBar.Controls.CheckBoxX();
//            this.chkClassroom = new DevComponents.DotNetBar.Controls.CheckBoxX();
//            this.chkSubject = new DevComponents.DotNetBar.Controls.CheckBoxX();
//            this.chkSubjectAlias = new DevComponents.DotNetBar.Controls.CheckBoxX();
//            this.chkClass = new DevComponents.DotNetBar.Controls.CheckBoxX();
//            this.chkTeacher = new DevComponents.DotNetBar.Controls.CheckBoxX();
//            this.groupBox3 = new System.Windows.Forms.GroupBox();
//            this.lnkUploadCustom = new DevComponents.DotNetBar.LabelX();
//            this.lnkViewCustom = new DevComponents.DotNetBar.LabelX();
//            this.lnkViewDefault = new DevComponents.DotNetBar.LabelX();
//            this.rdoCustomize = new DevComponents.DotNetBar.Controls.CheckBoxX();
//            this.rdoDefualt = new DevComponents.DotNetBar.Controls.CheckBoxX();
//            this.btnPrint = new DevComponents.DotNetBar.ButtonX();
//            this.btnExit = new DevComponents.DotNetBar.ButtonX();
//            this.groupBox1.SuspendLayout();
//            this.groupBox2.SuspendLayout();
//            this.groupBox3.SuspendLayout();
//            this.SuspendLayout();
//            // 
//            // labelX1
//            // 
//            this.labelX1.AutoSize = true;
//            this.labelX1.BackColor = System.Drawing.Color.Transparent;
//            // 
//            // 
//            // 
//            this.labelX1.BackgroundStyle.Class = "";
//            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
//            this.labelX1.Location = new System.Drawing.Point(12, 12);
//            this.labelX1.Name = "labelX1";
//            this.labelX1.Size = new System.Drawing.Size(60, 21);
//            this.labelX1.TabIndex = 0;
//            this.labelX1.Text = "時間表：";
//            // 
//            // cboTimeTable
//            // 
//            this.cboTimeTable.DisplayMember = "Text";
//            this.cboTimeTable.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
//            this.cboTimeTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
//            this.cboTimeTable.FormattingEnabled = true;
//            this.cboTimeTable.ItemHeight = 19;
//            this.cboTimeTable.Location = new System.Drawing.Point(69, 12);
//            this.cboTimeTable.Name = "cboTimeTable";
//            this.cboTimeTable.Size = new System.Drawing.Size(170, 25);
//            this.cboTimeTable.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
//            this.cboTimeTable.TabIndex = 1;
//            // 
//            // groupBox1
//            // 
//            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
//            this.groupBox1.Controls.Add(this.chkTeacherBusyDesc);
//            this.groupBox1.Controls.Add(this.chkMergeTimeTable);
//            this.groupBox1.ForeColor = System.Drawing.SystemColors.Desktop;
//            this.groupBox1.Location = new System.Drawing.Point(12, 43);
//            this.groupBox1.Name = "groupBox1";
//            this.groupBox1.Size = new System.Drawing.Size(227, 81);
//            this.groupBox1.TabIndex = 2;
//            this.groupBox1.TabStop = false;
//            this.groupBox1.Text = "選項設定";
//            // 
//            // chkTeacherBusyDesc
//            // 
//            this.chkTeacherBusyDesc.AutoSize = true;
//            // 
//            // 
//            // 
//            this.chkTeacherBusyDesc.BackgroundStyle.Class = "";
//            this.chkTeacherBusyDesc.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
//            this.chkTeacherBusyDesc.Location = new System.Drawing.Point(6, 53);
//            this.chkTeacherBusyDesc.Name = "chkTeacherBusyDesc";
//            this.chkTeacherBusyDesc.Size = new System.Drawing.Size(147, 21);
//            this.chkTeacherBusyDesc.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
//            this.chkTeacherBusyDesc.TabIndex = 1;
//            this.chkTeacherBusyDesc.Text = "顯示教師不排課時段";
//            // 
//            // chkMergeTimeTable
//            // 
//            // 
//            // 
//            // 
//            this.chkMergeTimeTable.BackgroundStyle.Class = "";
//            this.chkMergeTimeTable.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
//            this.chkMergeTimeTable.Location = new System.Drawing.Point(6, 24);
//            this.chkMergeTimeTable.Name = "chkMergeTimeTable";
//            this.chkMergeTimeTable.Size = new System.Drawing.Size(100, 23);
//            this.chkMergeTimeTable.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
//            this.chkMergeTimeTable.TabIndex = 0;
//            this.chkMergeTimeTable.Text = "合併時間表";
//            // 
//            // groupBox2
//            // 
//            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
//            this.groupBox2.Controls.Add(this.txtPeriod);
//            this.groupBox2.Controls.Add(this.labelX2);
//            this.groupBox2.Controls.Add(this.chkComment);
//            this.groupBox2.Controls.Add(this.chkCourseName);
//            this.groupBox2.Controls.Add(this.chkClassroom);
//            this.groupBox2.Controls.Add(this.chkSubject);
//            this.groupBox2.Controls.Add(this.chkSubjectAlias);
//            this.groupBox2.Controls.Add(this.chkClass);
//            this.groupBox2.Controls.Add(this.chkTeacher);
//            this.groupBox2.ForeColor = System.Drawing.SystemColors.Desktop;
//            this.groupBox2.Location = new System.Drawing.Point(12, 130);
//            this.groupBox2.Name = "groupBox2";
//            this.groupBox2.Size = new System.Drawing.Size(227, 112);
//            this.groupBox2.TabIndex = 3;
//            this.groupBox2.TabStop = false;
//            this.groupBox2.Text = "顯示設定";
//            // 
//            // txtPeriod
//            // 
//            // 
//            // 
//            // 
//            this.txtPeriod.Border.Class = "TextBoxBorder";
//            this.txtPeriod.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
//            this.txtPeriod.Location = new System.Drawing.Point(137, 77);
//            this.txtPeriod.Name = "txtPeriod";
//            this.txtPeriod.Size = new System.Drawing.Size(73, 25);
//            this.txtPeriod.TabIndex = 8;
//            // 
//            // labelX2
//            // 
//            this.labelX2.AutoSize = true;
//            // 
//            // 
//            // 
//            this.labelX2.BackgroundStyle.Class = "";
//            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
//            this.labelX2.ForeColor = System.Drawing.SystemColors.ControlText;
//            this.labelX2.Location = new System.Drawing.Point(64, 79);
//            this.labelX2.Name = "labelX2";
//            this.labelX2.Size = new System.Drawing.Size(74, 21);
//            this.labelX2.TabIndex = 7;
//            this.labelX2.Text = "顯示節次：";
//            // 
//            // chkComment
//            // 
//            this.chkComment.AutoSize = true;
//            // 
//            // 
//            // 
//            this.chkComment.BackgroundStyle.Class = "";
//            this.chkComment.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
//            this.chkComment.Location = new System.Drawing.Point(6, 78);
//            this.chkComment.Name = "chkComment";
//            this.chkComment.Size = new System.Drawing.Size(54, 21);
//            this.chkComment.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
//            this.chkComment.TabIndex = 6;
//            this.chkComment.Text = "註記";
//            // 
//            // chkCourseName
//            // 
//            this.chkCourseName.AutoSize = true;
//            // 
//            // 
//            // 
//            this.chkCourseName.BackgroundStyle.Class = "";
//            this.chkCourseName.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
//            this.chkCourseName.Location = new System.Drawing.Point(117, 51);
//            this.chkCourseName.Name = "chkCourseName";
//            this.chkCourseName.Size = new System.Drawing.Size(80, 21);
//            this.chkCourseName.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
//            this.chkCourseName.TabIndex = 5;
//            this.chkCourseName.Text = "課程名稱";
//            // 
//            // chkClassroom
//            // 
//            this.chkClassroom.AutoSize = true;
//            // 
//            // 
//            // 
//            this.chkClassroom.BackgroundStyle.Class = "";
//            this.chkClassroom.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
//            this.chkClassroom.Location = new System.Drawing.Point(57, 51);
//            this.chkClassroom.Name = "chkClassroom";
//            this.chkClassroom.Size = new System.Drawing.Size(54, 21);
//            this.chkClassroom.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
//            this.chkClassroom.TabIndex = 4;
//            this.chkClassroom.Text = "場地";
//            // 
//            // chkSubject
//            // 
//            this.chkSubject.AutoSize = true;
//            // 
//            // 
//            // 
//            this.chkSubject.BackgroundStyle.Class = "";
//            this.chkSubject.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
//            this.chkSubject.Location = new System.Drawing.Point(6, 51);
//            this.chkSubject.Name = "chkSubject";
//            this.chkSubject.Size = new System.Drawing.Size(54, 21);
//            this.chkSubject.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
//            this.chkSubject.TabIndex = 3;
//            this.chkSubject.Text = "科目";
//            // 
//            // chkSubjectAlias
//            // 
//            this.chkSubjectAlias.AutoSize = true;
//            // 
//            // 
//            // 
//            this.chkSubjectAlias.BackgroundStyle.Class = "";
//            this.chkSubjectAlias.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
//            this.chkSubjectAlias.Location = new System.Drawing.Point(117, 24);
//            this.chkSubjectAlias.Name = "chkSubjectAlias";
//            this.chkSubjectAlias.Size = new System.Drawing.Size(80, 21);
//            this.chkSubjectAlias.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
//            this.chkSubjectAlias.TabIndex = 2;
//            this.chkSubjectAlias.Text = "科目簡稱";
//            // 
//            // chkClass
//            // 
//            this.chkClass.AutoSize = true;
//            // 
//            // 
//            // 
//            this.chkClass.BackgroundStyle.Class = "";
//            this.chkClass.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
//            this.chkClass.Location = new System.Drawing.Point(57, 24);
//            this.chkClass.Name = "chkClass";
//            this.chkClass.Size = new System.Drawing.Size(54, 21);
//            this.chkClass.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
//            this.chkClass.TabIndex = 1;
//            this.chkClass.Text = "班級";
//            // 
//            // chkTeacher
//            // 
//            this.chkTeacher.AutoSize = true;
//            // 
//            // 
//            // 
//            this.chkTeacher.BackgroundStyle.Class = "";
//            this.chkTeacher.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
//            this.chkTeacher.Location = new System.Drawing.Point(6, 24);
//            this.chkTeacher.Name = "chkTeacher";
//            this.chkTeacher.Size = new System.Drawing.Size(54, 21);
//            this.chkTeacher.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
//            this.chkTeacher.TabIndex = 0;
//            this.chkTeacher.Text = "教師";
//            // 
//            // groupBox3
//            // 
//            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
//            this.groupBox3.Controls.Add(this.lnkUploadCustom);
//            this.groupBox3.Controls.Add(this.lnkViewCustom);
//            this.groupBox3.Controls.Add(this.lnkViewDefault);
//            this.groupBox3.Controls.Add(this.rdoCustomize);
//            this.groupBox3.Controls.Add(this.rdoDefualt);
//            this.groupBox3.ForeColor = System.Drawing.SystemColors.Desktop;
//            this.groupBox3.Location = new System.Drawing.Point(12, 248);
//            this.groupBox3.Name = "groupBox3";
//            this.groupBox3.Size = new System.Drawing.Size(227, 81);
//            this.groupBox3.TabIndex = 4;
//            this.groupBox3.TabStop = false;
//            this.groupBox3.Text = "選項設定";
//            // 
//            // lnkUploadCustom
//            // 
//            this.lnkUploadCustom.AutoSize = true;
//            // 
//            // 
//            // 
//            this.lnkUploadCustom.BackgroundStyle.Class = "";
//            this.lnkUploadCustom.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
//            this.lnkUploadCustom.Location = new System.Drawing.Point(153, 51);
//            this.lnkUploadCustom.Name = "lnkUploadCustom";
//            this.lnkUploadCustom.Size = new System.Drawing.Size(34, 21);
//            this.lnkUploadCustom.TabIndex = 4;
//            this.lnkUploadCustom.Text = "上傳";
//            // 
//            // lnkViewCustom
//            // 
//            this.lnkViewCustom.AutoSize = true;
//            // 
//            // 
//            // 
//            this.lnkViewCustom.BackgroundStyle.Class = "";
//            this.lnkViewCustom.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
//            this.lnkViewCustom.Location = new System.Drawing.Point(104, 51);
//            this.lnkViewCustom.Name = "lnkViewCustom";
//            this.lnkViewCustom.Size = new System.Drawing.Size(34, 21);
//            this.lnkViewCustom.TabIndex = 3;
//            this.lnkViewCustom.Text = "檢視";
//            // 
//            // lnkViewDefault
//            // 
//            this.lnkViewDefault.AutoSize = true;
//            // 
//            // 
//            // 
//            this.lnkViewDefault.BackgroundStyle.Class = "";
//            this.lnkViewDefault.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
//            this.lnkViewDefault.Location = new System.Drawing.Point(104, 26);
//            this.lnkViewDefault.Name = "lnkViewDefault";
//            this.lnkViewDefault.Size = new System.Drawing.Size(34, 21);
//            this.lnkViewDefault.TabIndex = 2;
//            this.lnkViewDefault.Text = "檢視";
//            // 
//            // rdoCustomize
//            // 
//            this.rdoCustomize.AutoSize = true;
//            // 
//            // 
//            // 
//            this.rdoCustomize.BackgroundStyle.Class = "";
//            this.rdoCustomize.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
//            this.rdoCustomize.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
//            this.rdoCustomize.Location = new System.Drawing.Point(6, 51);
//            this.rdoCustomize.Name = "rdoCustomize";
//            this.rdoCustomize.Size = new System.Drawing.Size(80, 21);
//            this.rdoCustomize.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
//            this.rdoCustomize.TabIndex = 1;
//            this.rdoCustomize.Text = "自訂範本";
//            // 
//            // rdoDefualt
//            // 
//            // 
//            // 
//            // 
//            this.rdoDefualt.BackgroundStyle.Class = "";
//            this.rdoDefualt.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
//            this.rdoDefualt.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
//            this.rdoDefualt.Location = new System.Drawing.Point(6, 24);
//            this.rdoDefualt.Name = "rdoDefualt";
//            this.rdoDefualt.Size = new System.Drawing.Size(100, 23);
//            this.rdoDefualt.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
//            this.rdoDefualt.TabIndex = 0;
//            this.rdoDefualt.Text = "預設範本";
//            // 
//            // btnPrint
//            // 
//            this.btnPrint.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
//            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
//            this.btnPrint.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
//            this.btnPrint.Location = new System.Drawing.Point(106, 338);
//            this.btnPrint.Name = "btnPrint";
//            this.btnPrint.Size = new System.Drawing.Size(59, 23);
//            this.btnPrint.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
//            this.btnPrint.TabIndex = 5;
//            this.btnPrint.Text = "列印";
//            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
//            // 
//            // btnExit
//            // 
//            this.btnExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
//            this.btnExit.BackColor = System.Drawing.Color.Transparent;
//            this.btnExit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
//            this.btnExit.Location = new System.Drawing.Point(171, 338);
//            this.btnExit.Name = "btnExit";
//            this.btnExit.Size = new System.Drawing.Size(66, 23);
//            this.btnExit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
//            this.btnExit.TabIndex = 6;
//            this.btnExit.Text = "離開";
//            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
//            // 
//            // frmReport
//            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.ClientSize = new System.Drawing.Size(248, 372);
//            this.Controls.Add(this.btnExit);
//            this.Controls.Add(this.btnPrint);
//            this.Controls.Add(this.groupBox3);
//            this.Controls.Add(this.groupBox2);
//            this.Controls.Add(this.groupBox1);
//            this.Controls.Add(this.cboTimeTable);
//            this.Controls.Add(this.labelX1);
//            this.Name = "frmReport";
//            this.Text = "列印功課表";
//            this.Load += new System.EventHandler(this.frmReport_Load);
//            this.groupBox1.ResumeLayout(false);
//            this.groupBox1.PerformLayout();
//            this.groupBox2.ResumeLayout(false);
//            this.groupBox2.PerformLayout();
//            this.groupBox3.ResumeLayout(false);
//            this.groupBox3.PerformLayout();
//            this.ResumeLayout(false);
//            this.PerformLayout();

//        }

//        #endregion

//        private DevComponents.DotNetBar.LabelX labelX1;
//        private DevComponents.DotNetBar.Controls.ComboBoxEx cboTimeTable;
//        private System.Windows.Forms.GroupBox groupBox1;
//        private DevComponents.DotNetBar.Controls.CheckBoxX chkMergeTimeTable;
//        private DevComponents.DotNetBar.Controls.CheckBoxX chkTeacherBusyDesc;
//        private System.Windows.Forms.GroupBox groupBox2;
//        private DevComponents.DotNetBar.Controls.CheckBoxX chkClass;
//        private DevComponents.DotNetBar.Controls.CheckBoxX chkTeacher;
//        private DevComponents.DotNetBar.Controls.CheckBoxX chkSubjectAlias;
//        private DevComponents.DotNetBar.Controls.CheckBoxX chkSubject;
//        private DevComponents.DotNetBar.Controls.CheckBoxX chkClassroom;
//        private DevComponents.DotNetBar.Controls.CheckBoxX chkCourseName;
//        private DevComponents.DotNetBar.Controls.CheckBoxX chkComment;
//        private DevComponents.DotNetBar.LabelX labelX2;
//        private DevComponents.DotNetBar.Controls.TextBoxX txtPeriod;
//        private System.Windows.Forms.GroupBox groupBox3;
//        private DevComponents.DotNetBar.Controls.CheckBoxX rdoCustomize;
//        private DevComponents.DotNetBar.Controls.CheckBoxX rdoDefualt;
//        private DevComponents.DotNetBar.LabelX lnkViewDefault;
//        private DevComponents.DotNetBar.LabelX lnkViewCustom;
//        private DevComponents.DotNetBar.LabelX lnkUploadCustom;
//        private DevComponents.DotNetBar.ButtonX btnPrint;
//        private DevComponents.DotNetBar.ButtonX btnExit;

//    }
//}