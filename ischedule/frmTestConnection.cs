using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using FISCA.DSAClient;
using Sunset.Data.Integration;

namespace ischedule
{
    public partial class frmTestConnection : BaseForm
    {
        private string Filename = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\Preference.xml";

        /// <summary>
        /// DSNS名稱列表
        /// </summary>
        public List<SchoolDSNSName> DSNSNames
        {
            get
            {
                List<SchoolDSNSName> Result = new List<SchoolDSNSName>();

                foreach (DataGridViewRow Row in grdSchool.Rows)
                {
                    DataGridViewCell Cell = Row.Cells[0] as DataGridViewCell;

                    string SchoolName = "" + Cell.Value;

                    SchoolDSNSName vSchool =  Global.AvailDSNSNames.Find(x => x.SchoolName.Equals(SchoolName));

					if (vSchool == null)
					{
						vSchool = new SchoolDSNSName();
						vSchool.DSNSName = SchoolName;
						vSchool.SchoolName = SchoolName;
					}

                    if (vSchool != null)
                        Result.Add(vSchool);
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


        public frmTestConnection()
        {
            InitializeComponent();
        }

        private void frmTestConnection_Load(object sender, EventArgs e)
        {
            try
            {
                grdSchool.DataError += (vsender, ve) => { };

                InitialAvailDSNSNames();

                InitialSchoolYearAndSemester();

                LoadPreference();
            }
            catch (Exception ve)
            {
                MessageBox.Show("取得可連線學校列表錯誤：" + ve.Message);
            }
        }

        /// <summary>
        /// 初始化DSNS
        /// </summary>
        private void InitialAvailDSNSNames()
        {
            Global.AvailDSNSNames.ForEach(x => colSchool.Items.Add(x.SchoolName));
        }

        /// <summary>
        /// 初始化學年度及學期
        /// </summary>
        private void InitialSchoolYearAndSemester()
        {
            int CurrentSchoolYear = DateTime.Now.Year - 1911;

            for (int i = CurrentSchoolYear; i > CurrentSchoolYear - 3; i--)
                cmbSchoolYear.Items.Add("" + i);

            cmbSemester.Items.Add("" + 1);
            cmbSemester.Items.Add("" + 2);
        }

        #region private function
        /// <summary>
        /// 傳入DSNS名稱列表以測試連線
        /// </summary>
        /// <returns></returns>
        public DialogResult TestConnection()
        {
            return TestConnection(new List<string>() { });
        }

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
                int RowIndex = grdSchool.Rows.Add();

                grdSchool.Rows[RowIndex].Cells[0].Value = "" + DSNSName;
            }

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

            var DuplicateDSNSNames = DSNSNames.Select(x=>x.DSNSName)
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
                foreach (SchoolDSNSName SchoolDSNSName in DSNSNames)
                {
                    if (!string.IsNullOrWhiteSpace(SchoolDSNSName.DSNSName))
                    {
                        Tuple<Connection, string> Connection = ContractService.GetConnection(Global.Passport, SchoolDSNSName.DSNSName);
                        if (!string.IsNullOrWhiteSpace(Connection.Item2))
                        {
                            IsConnect = false;

                            if (Connection.Item2.Contains("Permission"))
                                strBuilder.AppendLine("權限不足，無法使用排課系統！");
                            else
                                strBuilder.AppendLine(Connection.Item2);

                            ErrorSchoolNames.Add(SchoolDSNSName.SchoolName);
                        }
                        else
                            Connections.Add(Connection.Item1);
                    }
                } 
            }

            //cmbSchoolYear.Text = string.IsNullOrWhiteSpace(cmbSchoolYear.Text) ? "0" : cmbSchoolYear.Text; //取得畫面上輸入值，若是空白則當做0看待。
            //cmbSemester.Text = string.IsNullOrWhiteSpace(cmbSemester.Text) ? "0" : cmbSemester.Text;       //取得畫面上輸入值，若是空白則當做0看待。

            int intSchoolYear;

            //若兩者為數字才解析成功
            if (int.TryParse(cmbSchoolYear.Text, out intSchoolYear))
            {
                SchoolYear = "" + intSchoolYear;
            }
            else
            {
                IsConnect = false;
                strBuilder.AppendLine("學年度必須為數字！");
            }

            if (string.IsNullOrEmpty(cmbSemester.Text))
            {
                IsConnect = false;
                strBuilder.AppendLine("學期不得空白！");
            }

            Semester = cmbSemester.Text;

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


        private void btnTest_Click(object sender, EventArgs e)
        {
            Tuple<bool, string> TestConnectionResult = TestConnections();

            if (TestConnectionResult.Item1)
                MessageBox.Show("測試連線成功!");
            else
                MessageBox.Show(TestConnectionResult.Item2);
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            Tuple<bool, string> TestResult = TestConnections();

            if (TestResult.Item1)
            {
                List<string> DSNS = new List<string>();

                for (int i = 0; i < Connections.Count; i++)
                    DSNS.Add(Connections[i].AccessPoint.Name);

                SavePreference();

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            else
                MessageBox.Show(TestResult.Item2);
        }

        private void LoadPreference()
        {
            if (File.Exists(Filename))
            {
                XElement Element = XElement.Load(Filename);

                string vSchoolYear = Element.ElementText("SchoolYear");
                string vSemester = Element.ElementText("Semester");
                List<string> vDSNSNames = new List<string>();

                cmbSchoolYear.Text = vSchoolYear;
                cmbSemester.Text = vSemester;

                grdSchool.Rows.Clear();

                XElement elmDSNSNames = Element.Element("DSNSNames");

                if (elmDSNSNames != null)
                {
                    foreach (XElement elmDSNSName in elmDSNSNames.Elements("DSNSName"))
                    {
                        string SchoolName = elmDSNSName.AttributeText("SchoolName");
                        string DSNSName = elmDSNSName.Value;

                        SchoolDSNSName School = new SchoolDSNSName();

                        School.SchoolName = SchoolName;
                        School.DSNSName = DSNSName;

                        int RowIndex = grdSchool.Rows.Add();

                        DataGridViewComboBoxCell Cell = grdSchool.Rows[RowIndex].Cells[0] as DataGridViewComboBoxCell;

                        Cell.Value = School.SchoolName;
                        Cell.Tag = School;
                    }
                }
                else
                {
                    foreach (SchoolDSNSName School in Global.AvailDSNSNames)
                    {
                        int RowIndex = grdSchool.Rows.Add();

                        DataGridViewComboBoxCell Cell = grdSchool.Rows[RowIndex].Cells[0] as DataGridViewComboBoxCell;

                        Cell.Value = School.SchoolName;
                        Cell.Tag = School;
                    }
                }
            }
            else
            {
                grdSchool.Rows.Clear();

                foreach (SchoolDSNSName School in Global.AvailDSNSNames)
                {
                    int RowIndex = grdSchool.Rows.Add();

                    DataGridViewComboBoxCell Cell = grdSchool.Rows[RowIndex].Cells[0] as DataGridViewComboBoxCell;

                    Cell.Value = School.SchoolName;
                    Cell.Tag = School;
                }
            }
        }

        private void SavePreference()
        {
            XElement Element = new XElement("Preference");

            Element.SetAttributeValue("EMail", Global.EMail);

            Element.Add(new XElement("SchoolYear", SchoolYear));

            Element.Add(new XElement("Semester", Semester));

            XElement elmDSNSNames = new XElement("DSNSNames");

            foreach (SchoolDSNSName SchoolDSNS in DSNSNames)
            {
                XElement elmDSNSName = new XElement("DSNSName", SchoolDSNS.DSNSName);
                elmDSNSName.SetAttributeValue("SchoolName", SchoolDSNS.SchoolName);

                elmDSNSNames.Add(elmDSNSName);
            }

            Element.Add(elmDSNSNames);

            Element.Save(Filename);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}