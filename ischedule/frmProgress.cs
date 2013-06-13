
namespace ischedule
{
    /// <summary>
    /// 顯示進度
    /// </summary>
    public partial class frmProgress : BaseForm
    {
        private bool bCancel = false;

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public frmProgress()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// 改變進度
        /// </summary>
        /// <param name="Value"></param>
        public void ChangeProgress(int Value)
        {
            pbProgress.Value = Value>100?100:Value;
            pbProgress.Refresh();

            if ((double)Value == pbProgress.Maximum)
            {
                btnCancel.Enabled = false;
                btnCancel.Visible = false;
            }

            System.Windows.Forms.Application.DoEvents();
        }

        /// <summary>
        /// 開始
        /// </summary>
        /// <param name="DispText"></param>
        /// <param name="Min"></param>
        /// <param name="Max"></param>
        public void Start(string DispText, int Min, int Max)
        {
            lblNotice.Text = DispText;
            pbProgress.Minimum = Min;
            if (Max <= Min) Max = Min + 1;
            pbProgress.Maximum = Max;
            pbProgress.Value = Min;
            bCancel = false;
        }

        /// <summary>
        /// 使用者是否放棄
        /// </summary>
        public bool UserAbort
        {
            get { return bCancel; }
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.bCancel = true;
            this.Close();
        }
    }
}