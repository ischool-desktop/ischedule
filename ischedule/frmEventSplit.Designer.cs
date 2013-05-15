namespace ischedule
{
    partial class frmEventSplit
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
            this.lblPeriodsLeft = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.cboSelection3 = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cboSelection2 = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cboSelection1 = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cboSelection0 = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.btnConfirm = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // lblPeriodsLeft
            // 
            this.lblPeriodsLeft.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lblPeriodsLeft.BackgroundStyle.Class = "";
            this.lblPeriodsLeft.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblPeriodsLeft.Location = new System.Drawing.Point(78, 69);
            this.lblPeriodsLeft.Name = "lblPeriodsLeft";
            this.lblPeriodsLeft.Size = new System.Drawing.Size(10, 16);
            this.lblPeriodsLeft.TabIndex = 15;
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
            this.labelX2.Location = new System.Drawing.Point(13, 66);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(60, 21);
            this.labelX2.TabIndex = 14;
            this.labelX2.Text = "剩餘節數";
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
            this.labelX1.Location = new System.Drawing.Point(13, 3);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(60, 21);
            this.labelX1.TabIndex = 13;
            this.labelX1.Text = "課時時段";
            // 
            // cboSelection3
            // 
            this.cboSelection3.DisplayMember = "Text";
            this.cboSelection3.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboSelection3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSelection3.FormattingEnabled = true;
            this.cboSelection3.ItemHeight = 19;
            this.cboSelection3.Location = new System.Drawing.Point(256, 31);
            this.cboSelection3.Name = "cboSelection3";
            this.cboSelection3.Size = new System.Drawing.Size(75, 25);
            this.cboSelection3.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cboSelection3.TabIndex = 12;
            this.cboSelection3.SelectionChangeCommitted += new System.EventHandler(this.cboSelection_SelectionChangeCommitted);
            // 
            // cboSelection2
            // 
            this.cboSelection2.DisplayMember = "Text";
            this.cboSelection2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboSelection2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSelection2.FormattingEnabled = true;
            this.cboSelection2.ItemHeight = 19;
            this.cboSelection2.Location = new System.Drawing.Point(175, 31);
            this.cboSelection2.Name = "cboSelection2";
            this.cboSelection2.Size = new System.Drawing.Size(75, 25);
            this.cboSelection2.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cboSelection2.TabIndex = 11;
            this.cboSelection2.SelectionChangeCommitted += new System.EventHandler(this.cboSelection_SelectionChangeCommitted);
            // 
            // cboSelection1
            // 
            this.cboSelection1.DisplayMember = "Text";
            this.cboSelection1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboSelection1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSelection1.FormattingEnabled = true;
            this.cboSelection1.ItemHeight = 19;
            this.cboSelection1.Location = new System.Drawing.Point(94, 31);
            this.cboSelection1.Name = "cboSelection1";
            this.cboSelection1.Size = new System.Drawing.Size(75, 25);
            this.cboSelection1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cboSelection1.TabIndex = 10;
            this.cboSelection1.SelectionChangeCommitted += new System.EventHandler(this.cboSelection_SelectionChangeCommitted);
            // 
            // cboSelection0
            // 
            this.cboSelection0.DisplayMember = "Text";
            this.cboSelection0.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboSelection0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSelection0.FormattingEnabled = true;
            this.cboSelection0.ItemHeight = 19;
            this.cboSelection0.Location = new System.Drawing.Point(13, 32);
            this.cboSelection0.Name = "cboSelection0";
            this.cboSelection0.Size = new System.Drawing.Size(75, 25);
            this.cboSelection0.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cboSelection0.TabIndex = 9;
            this.cboSelection0.SelectionChangeCommitted += new System.EventHandler(this.cboSelection_SelectionChangeCommitted);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(253, 64);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnConfirm.BackColor = System.Drawing.Color.Transparent;
            this.btnConfirm.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnConfirm.Location = new System.Drawing.Point(172, 64);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(75, 23);
            this.btnConfirm.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnConfirm.TabIndex = 7;
            this.btnConfirm.Text = "確認";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // frmEventSplit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(337, 93);
            this.Controls.Add(this.lblPeriodsLeft);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.cboSelection3);
            this.Controls.Add(this.cboSelection2);
            this.Controls.Add(this.cboSelection1);
            this.Controls.Add(this.cboSelection0);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.MinimumSize = new System.Drawing.Size(345, 120);
            this.Name = "frmEventSplit";
            this.Text = "";
            this.TitleText = "分割課時";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.ButtonX btnConfirm;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cboSelection0;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cboSelection1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cboSelection2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cboSelection3;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX lblPeriodsLeft;
    }
}