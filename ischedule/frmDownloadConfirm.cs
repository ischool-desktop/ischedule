using System;
using System.Windows.Forms;
using Sunset.Data.Integration;

namespace ischedule
{
    /// <summary>
    /// 下載資料確認
    /// </summary>
    public partial class frmDownloadConfirm : BaseForm
    {
        private SchedulerSource schSource = SchedulerSource.Source;

        public frmDownloadConfirm()
        {
            InitializeComponent();
        }

        private void frmDownloadConfirm_Load(object sender, EventArgs e)
        {
            RefreshUI();
        }

        /// <summary>
        /// 取得布林字串
        /// </summary>
        /// <param name="IsSuccess"></param>
        /// <returns></returns>
        private string GetSuccessString(bool IsSuccess)
        {
            return IsSuccess ? "成功" : "失敗";
        }

        /// <summary>
        /// 重新整理介面
        /// </summary>
        private void RefreshUI()
        {
            btnCancel.Click += (sender, e) =>
            {
                schSource.Close();
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Close();
            };

            if (schSource.IsSuccess)
            {
                btnImport.Text = "確認下載";
                btnImport.Click += (sender, e) =>
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                };
            }
            else
            {
                btnImport.Text = "重新下載";
                btnImport.Click += (sender, e) =>
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    this.Close();
                    MainFormBL.Instance.Download();
                };
            }

            int RowIndex = -1;

            grdResult.DoubleClick += (sender, e) =>
            {
                if (grdResult.SelectedRows.Count == 1)
                {
                    string Message = "" + grdResult.SelectedRows[0].Tag;

                    new frmDownloadDetail(Message).ShowDialog(); 
                }                
            };

            #region 班級
            RowIndex = grdResult.Rows.Add();
            grdResult.Rows[RowIndex].SetValues("班級", GetSuccessString(schSource.ClassResult.IsSuccess),"已下載" + schSource.ClassResult.Data.Count + "筆");
            grdResult.Rows[RowIndex].Tag = schSource.ClassResult.Message;
            #endregion

            #region 班級不排課時段
            RowIndex = grdResult.Rows.Add();
            grdResult.Rows[RowIndex].SetValues("班級不排課時段", GetSuccessString(schSource.ClassBusysResult.IsSuccess), "已下載" + schSource.ClassBusysResult.Data.Count + "筆");
            grdResult.Rows[RowIndex].Tag = schSource.ClassBusysResult.Message;
            #endregion

            #region 教師
            RowIndex = grdResult.Rows.Add();
            grdResult.Rows[RowIndex].SetValues("教師", GetSuccessString(schSource.TeacherResult.IsSuccess), "已下載" + schSource.TeacherResult.Data.Count + "筆");
            grdResult.Rows[RowIndex].Tag = schSource.TeacherResult.Message;
            #endregion

            #region 教師不排課時段
            RowIndex = grdResult.Rows.Add();
            grdResult.Rows[RowIndex].SetValues("教師不排課時段", GetSuccessString(schSource.TeacherBusysResult.IsSuccess), "已下載" + schSource.TeacherBusysResult.Data.Count + "筆");
            grdResult.Rows[RowIndex].Tag = schSource.TeacherBusysResult.Message;
            #endregion

            #region 場地
            RowIndex = grdResult.Rows.Add();
            grdResult.Rows[RowIndex].SetValues("場地", GetSuccessString(schSource.ClassroomResult.IsSuccess), "已下載" + schSource.ClassroomResult.Data.Count + "筆");
            grdResult.Rows[RowIndex].Tag = schSource.ClassroomResult.Message;
            #endregion

            #region 場地不排課時段
            RowIndex = grdResult.Rows.Add();
            grdResult.Rows[RowIndex].SetValues("場地不排課時段", GetSuccessString(schSource.ClassroomBusysResult.IsSuccess), "已下載" + schSource.ClassroomBusysResult.Data.Count + "筆");
            grdResult.Rows[RowIndex].Tag = schSource.ClassroomBusysResult.Message;
            #endregion

            #region 時間表
            RowIndex = grdResult.Rows.Add();
            grdResult.Rows[RowIndex].SetValues("時間表", GetSuccessString(schSource.TimeTableResult.IsSuccess), "已下載" + schSource.TimeTableResult.Data.Count + "筆");
            grdResult.Rows[RowIndex].Tag = schSource.TimeTableResult.Message;
            #endregion

            #region 時間表分段
            RowIndex = grdResult.Rows.Add();
            grdResult.Rows[RowIndex].SetValues("時間表分段", GetSuccessString(schSource.TimeTableSecsResult.IsSuccess), "已下載" + schSource.TimeTableSecsResult.Data.Count + "筆");
            grdResult.Rows[RowIndex].Tag = schSource.TimeTableSecsResult.Message;
            #endregion

            #region 課程分段
            RowIndex = grdResult.Rows.Add();
            grdResult.Rows[RowIndex].SetValues("課程分段", GetSuccessString(schSource.CourseSectionResult.IsSuccess), "已下載" + schSource.CourseSectionResult.Data.Count + "筆");
            grdResult.Rows[RowIndex].Tag = schSource.CourseSectionResult.Message;
            #endregion
        }

        private void btnConnection_Click(object sender, EventArgs e)
        {
            MainFormBL.Instance.Download();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {

        }
    }
}