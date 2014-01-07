using System;
using System.Windows.Forms;

namespace ischedule
{
    /// <summary>
    /// 密碼輸入
    /// </summary>
    public partial class frmPasspordInput : BaseForm
    {
        /// <summary>
        /// 密碼
        /// </summary>
        public string Password { get { return txtPassword.Text; } }
        private bool IsConfirm;

        /// <summary>
        /// 無參數建構式
        /// </summary>
        protected frmPasspordInput()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 預設密碼
        /// </summary>
        /// <param name="Password"></param>
        public frmPasspordInput(string Password,string Title,bool IsConfirm)
        {
            InitializeComponent();

            this.txtPassword.Text = Password;
            this.TitleText = Title;
            this.IsConfirm = IsConfirm;
        }

        /// <summary>
        /// 確認
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (this.IsConfirm)
            {
                if (string.IsNullOrEmpty(txtPassword.Text))
                {
                    MessageBox.Show("請輸入密碼欄位！");
                    return;
                }
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;

            Close();
        }

        private void frmPasspordInput_Load(object sender, EventArgs e)
        {
            if (!this.IsConfirm)
            {
                btnConfirm.Top = 43;
                btnCancel.Top = 43;
                this.Height = 100;
                this.MaximumSize = new System.Drawing.Size(this.Width,100); 
                this.MinimumSize = new System.Drawing.Size(this.Width,100);
            }
        }
    }
}
