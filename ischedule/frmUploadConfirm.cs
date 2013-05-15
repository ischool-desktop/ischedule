using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.DSAClient;
using Sunset.Data.Integration;

namespace ischedule
{
    /// <summary>
    /// 上傳確認表單
    /// </summary>
    public partial class frmUploadConfirm : BaseForm
    {
        /// <summary>
        /// DSNS名稱列表
        /// </summary>
        public List<string> DSNSNames
        {
            get
            {
                List<string> Result = new List<string>();

                foreach (DataGridViewRow Row in grdSchool.Rows)
                {
                    DataGridViewCell Cell = Row.Cells[0] as DataGridViewCell;

                    string DSNSName = "" + Cell.Tag;

                    if (!string.IsNullOrEmpty(DSNSName))
                        Result.Add(DSNSName);
                }

                return Result;
            }
        }

        /// <summary>
        /// 學年度
        /// </summary>
        public string SchoolYear { get; private set; }

        /// <summary>
        /// 學期
        /// </summary>
        public string Semester { get; private set; }

        /// <summary>
        /// 所有連線資訊
        /// </summary>
        public List<Connection> Connections { get; private set; }

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public frmUploadConfirm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 載入表單
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmTestConnection_Load(object sender, EventArgs e)
        {
            try
            {
                grdSchool.DataError += (vsender, ve) => { };
            }
            catch (Exception ve)
            {
                MessageBox.Show("取得可連線學校列表錯誤：" + ve.Message);
            }
        }

        #region private function
        /// <summary>
        /// 傳入DSNS名稱列表以測試連線
        /// </summary>
        /// <param name="DSNSNames"></param>
        /// <returns></returns>
        public DialogResult TestConnection(List<string> DSNSNames)
        {
            grdSchool.Rows.Clear();

            foreach(string DSNSName in DSNSNames)
            {
                SchoolDSNSName vSchoolDSNSName = Global.AvailDSNSNames.Find(x => x.DSNSName.Equals(DSNSName));

                int RowIndex = grdSchool.Rows.Add();

                grdSchool.Rows[RowIndex].Cells[0].Value = vSchoolDSNSName != null ? vSchoolDSNSName.SchoolName : DSNSName;
                grdSchool.Rows[RowIndex].Cells[0].Tag = DSNSName;
            }

            grdSchool.ReadOnly = true;

            return this.ShowDialog();
        }

        /// <summary>
        /// 測試連線
        /// </summary>
        /// <returns></returns>
        private Tuple<bool, string> TestConnections()
        {
            bool IsConnect = true;
            StringBuilder strBuilder = new StringBuilder();
            Connections = new List<Connection>();
            List<string> ErrorSchoolNames = new List<string>();

            var DuplicateDSNSNames = DSNSNames
                .GroupBy(i => i)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key);

            if (DuplicateDSNSNames.Count() > 0)
            {
                strBuilder.AppendLine("重覆的連線名稱!");
                foreach (string DuplicateDSNSName in DuplicateDSNSNames)
                    strBuilder.AppendLine(DuplicateDSNSName);
                IsConnect = false;
            }
            else if (DSNSNames.Count == 0)
            {
                strBuilder.AppendLine("請設定連線資訊!");
                IsConnect = false;
            }
            else
            {
                foreach (string DSNSName in DSNSNames)
                {
                    if (!string.IsNullOrWhiteSpace(DSNSName))
                    {
                        Tuple<Connection, string> Connection = ContractService.GetConnection(Global.Passport,DSNSName);
                        if (!string.IsNullOrWhiteSpace(Connection.Item2))
                        {
                            IsConnect = false;

                            if (Connection.Item2.Contains("Permission"))
                                strBuilder.AppendLine("權限不足，無法使用排課系統！");
                            else
                                strBuilder.AppendLine(Connection.Item2);

                            ErrorSchoolNames.Add(DSNSName);
                        }
                        else
                            Connections.Add(Connection.Item1);
                    }
                } 
            }

            foreach (DataGridViewRow Row in grdSchool.Rows)
            {
                string CellValue = "" + Row.Cells[0].Value;

                if (ErrorSchoolNames.Contains(CellValue))
                    Row.Cells[0].Style.BackColor = System.Drawing.Color.Red;
                else
                    Row.Cells[0].Style.BackColor = System.Drawing.Color.White;
            }

            return new Tuple<bool, string>(IsConnect, strBuilder.ToString());
        }
        #endregion

        /// <summary>
        /// 測試連線
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTest_Click(object sender, EventArgs e)
        {
            Tuple<bool, string> TestConnectionResult = TestConnections();

            if (TestConnectionResult.Item1)
                MessageBox.Show("測試連線成功!");
            else
                MessageBox.Show(TestConnectionResult.Item2);
        }

        /// <summary>
        /// 確認
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            Tuple<bool, string> TestResult = TestConnections();

            if (TestResult.Item1)
            {
                List<string> DSNS = new List<string>();

                for (int i = 0; i < Connections.Count; i++)
                    DSNS.Add(Connections[i].AccessPoint.Name);

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            else
                MessageBox.Show(TestResult.Item2);
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}