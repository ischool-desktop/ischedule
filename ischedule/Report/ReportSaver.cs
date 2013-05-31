using System;
using System.IO;
using System.Windows.Forms;

namespace ischedule
{
    public class ReportSaver
    {
        /// <summary>
        /// 儲存 Excel 報表。
        /// </summary>
        /// <param name="workbook">要儲存的報表物件</param>
        /// <param name="filename">儲存的檔案名稱</param>
        public static void SaveWorkbook(Aspose.Cells.Workbook workbook, string filename)
        {
            string path = CreatePath(filename, ".xls");

            try
            {
                workbook.Save(path);
                OpenFile(path, path);
            }
            catch (Exception ex)
            {
                //MsgBox.Show("儲存失敗" + ex.Message);
            }
        }

        private static string CreatePath(string filename, string ext)
        {
            string path = Path.Combine(Application.StartupPath, "Reports");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            string fullname = filename.EndsWith(ext) ? filename : filename + ext;
            path = Path.Combine(path, fullname);

            #region 如果檔案已經存在
            if (File.Exists(path))
            {
                int i = 1;
                while (true)
                {
                    string newPath = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path) + (i++) + Path.GetExtension(path));
                    if (!File.Exists(newPath))
                    {
                        path = newPath;
                        break;
                    }
                }
            }
            #endregion

            return path;
        }

        private static void OpenFile(string path, string filename)
        {
            if (MessageBox.Show(filename + "產生完成，是否立刻開啟？", "ischool", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    System.Diagnostics.Process.Start(path);
                }
                catch (Exception)
                {
                    MessageBox.Show("開啟檔案發生錯誤，您可能沒有相關的應用程式可以開啟此類型檔案。");
                }
            }
        }
    }
}