using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using FISCA.DSAClient;
using Sunset.Data.Integration;

namespace ischedule
{
    /// <summary>
    /// 登入表單
    /// </summary>
    public partial class frmLogin : BaseForm
    {
        private string Filename = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\Preference.xml";
        /// <summary>
        /// 無參數建構式
        /// </summary>
        public frmLogin()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 確認
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEMail.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("帳號或密碼不能為空白！");

                return;
            }

            Tuple<PassportToken,string> result = ContractService.GetPassportToken(
                txtEMail.Text, 
                txtPassword.Text);

            if (!string.IsNullOrEmpty(result.Item2))
            {

                if (result.Item2.Contains("Invalid credentials"))
                    MessageBox.Show("您輸入的帳號或密碼有誤！","排課系統");
                else
                    MessageBox.Show(result.Item2, "排課系統");

                return;
            }
            else
            {
                Global.Passport = result.Item1;
                Global.EMail = txtEMail.Text;

                //取得ischool web連線學校清單
                Dictionary<string, string> DSNSNames = ContractService.GetAvailSchoolAndDSNSNames(Global.Passport);

                List<SchoolDSNSName> SchoolDSNSNames = new List<SchoolDSNSName>();

                foreach (string SchoolName in DSNSNames.Keys)
                {
                    SchoolDSNSName vSchoolDSNSName = new SchoolDSNSName();

                    vSchoolDSNSName.SchoolName = SchoolName;
                    vSchoolDSNSName.DSNSName = DSNSNames[SchoolName];

                    //#region 此DSNS嘗試連線到Contract，若可以連線，並且也有權限才加入清單中
                    //if (!string.IsNullOrWhiteSpace(vSchoolDSNSName.DSNSName))
                    //{
                    //    Tuple<Connection, string> Connection = ContractService.GetConnection(Global.Passport, vSchoolDSNSName.DSNSName);
                    //    if (string.IsNullOrWhiteSpace(Connection.Item2))
                    //        SchoolDSNSNames.Add(vSchoolDSNSName);
                    //}
                    //#endregion

                    SchoolDSNSNames.Add(vSchoolDSNSName);
                }

                SchoolDSNSName SchoolDSNSName = new SchoolDSNSName();
                SchoolDSNSName.DSNSName = "ischool@debug";
                SchoolDSNSName.SchoolName = "ischool@debug";
                SchoolDSNSNames.Add(SchoolDSNSName);

                Global.AvailDSNSNames = SchoolDSNSNames;

                if (File.Exists(Filename))
                {
                    XElement Element = XElement.Load(Filename);

                    Element.SetAttributeValue("EMail", Global.EMail);

                    Element.Save(Filename);
                }
                else
                {
                    XElement Element = new XElement("Preference");

                    Element.SetAttributeValue("EMail", Global.EMail);

                    Element.Save(Filename);
                }

                MainFormBL.Instance.SetTitle("ischedule(" + txtEMail.Text + "已登入)");

                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            if (File.Exists(Filename))
            {
                XElement Element = XElement.Load(Filename);

                string EMail = Element.AttributeText("EMail");

                txtEMail.Text = EMail;
            }
        }
    }
}