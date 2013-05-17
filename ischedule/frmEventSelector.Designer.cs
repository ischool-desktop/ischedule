namespace ischedule
{
    partial class frmEventSelector
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grdTestEvent = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.btnCandidate = new DevComponents.DotNetBar.ButtonX();
            this.btnFree = new DevComponents.DotNetBar.ButtonX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.colWhomName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSubject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWhoName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWhereName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWeekFlag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEventID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grdTestEvent)).BeginInit();
            this.SuspendLayout();
            // 
            // grdTestEvent
            // 
            this.grdTestEvent.AllowUserToAddRows = false;
            this.grdTestEvent.AllowUserToDeleteRows = false;
            this.grdTestEvent.AllowUserToResizeColumns = false;
            this.grdTestEvent.AllowUserToResizeRows = false;
            this.grdTestEvent.BackgroundColor = System.Drawing.Color.White;
            this.grdTestEvent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdTestEvent.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colWhomName,
            this.colSubject,
            this.colLen,
            this.colWhoName,
            this.colWhereName,
            this.colWeekFlag,
            this.colEventID});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft JhengHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdTestEvent.DefaultCellStyle = dataGridViewCellStyle1;
            this.grdTestEvent.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.grdTestEvent.Location = new System.Drawing.Point(16, 13);
            this.grdTestEvent.Name = "grdTestEvent";
            this.grdTestEvent.RowHeadersVisible = false;
            this.grdTestEvent.RowTemplate.Height = 24;
            this.grdTestEvent.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdTestEvent.Size = new System.Drawing.Size(525, 208);
            this.grdTestEvent.TabIndex = 0;
            // 
            // btnCandidate
            // 
            this.btnCandidate.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCandidate.BackColor = System.Drawing.Color.Transparent;
            this.btnCandidate.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCandidate.Location = new System.Drawing.Point(259, 234);
            this.btnCandidate.Name = "btnCandidate";
            this.btnCandidate.Size = new System.Drawing.Size(93, 23);
            this.btnCandidate.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnCandidate.TabIndex = 1;
            this.btnCandidate.Text = "顯示可排節次";
            this.btnCandidate.Click += new System.EventHandler(this.btnCandidate_Click);
            // 
            // btnFree
            // 
            this.btnFree.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnFree.BackColor = System.Drawing.Color.Transparent;
            this.btnFree.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnFree.Location = new System.Drawing.Point(358, 234);
            this.btnFree.Name = "btnFree";
            this.btnFree.Size = new System.Drawing.Size(99, 23);
            this.btnFree.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnFree.TabIndex = 2;
            this.btnFree.Text = "回復至未排課";
            this.btnFree.Click += new System.EventHandler(this.btnFree_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(463, 234);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // colWhomName
            // 
            this.colWhomName.DataPropertyName = "DisplayClassName";
            this.colWhomName.HeaderText = "班級";
            this.colWhomName.MinimumWidth = 80;
            this.colWhomName.Name = "colWhomName";
            this.colWhomName.Width = 80;
            // 
            // colSubject
            // 
            this.colSubject.DataPropertyName = "DisplaySubjectName";
            this.colSubject.HeaderText = "科目";
            this.colSubject.Name = "colSubject";
            // 
            // colLen
            // 
            this.colLen.DataPropertyName = "Length";
            this.colLen.FillWeight = 50F;
            this.colLen.HeaderText = "節數";
            this.colLen.MinimumWidth = 60;
            this.colLen.Name = "colLen";
            this.colLen.Width = 60;
            // 
            // colWhoName
            // 
            this.colWhoName.DataPropertyName = "DisplayTeacherName";
            this.colWhoName.HeaderText = "教師";
            this.colWhoName.MinimumWidth = 80;
            this.colWhoName.Name = "colWhoName";
            this.colWhoName.Width = 80;
            // 
            // colWhereName
            // 
            this.colWhereName.DataPropertyName = "DisplayClassroomName";
            this.colWhereName.HeaderText = "場地";
            this.colWhereName.Name = "colWhereName";
            // 
            // colWeekFlag
            // 
            this.colWeekFlag.DataPropertyName = "DisplayWeekFlag";
            this.colWeekFlag.HeaderText = "單雙週";
            this.colWeekFlag.Name = "colWeekFlag";
            // 
            // colEventID
            // 
            this.colEventID.DataPropertyName = "EventID";
            this.colEventID.HeaderText = "事件系統編號";
            this.colEventID.Name = "colEventID";
            this.colEventID.Visible = false;
            // 
            // frmEventSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 273);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnFree);
            this.Controls.Add(this.btnCandidate);
            this.Controls.Add(this.grdTestEvent);
            this.MaximumSize = new System.Drawing.Size(560, 300);
            this.MinimumSize = new System.Drawing.Size(560, 300);
            this.Name = "frmEventSelector";
            this.Text = "分課調整";
            ((System.ComponentModel.ISupportInitialize)(this.grdTestEvent)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.DataGridViewX grdTestEvent;
        private DevComponents.DotNetBar.ButtonX btnCandidate;
        private DevComponents.DotNetBar.ButtonX btnFree;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWhomName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSubject;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLen;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWhoName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWhereName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWeekFlag;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEventID;
    }
}