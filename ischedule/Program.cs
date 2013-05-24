using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using FISCA;
using FISCA.Deployment;

namespace ischedule
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (!System.IO.File.Exists(Path.Combine(System.Windows.Forms.Application.StartupPath, "開發模式")))
            {
                string updatePath = Path.Combine(System.Windows.Forms.Application.StartupPath, "update_padding.xml");

                if (System.IO.File.Exists(updatePath))
                {
                    FISCA.Deployment.UpdateHelper.ExecuteScript(updatePath, true);
                    return;
                }
            }

            Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(MainFormBL.Instance);
            //Application.Run(new test.frmTest());
        }

        /// 需要在某個時機呼叫這個，一般來說是在 MainForm Load 的事件中。
        /// </summary>
        internal static void Update()
        {
            string basePath = System.Windows.Forms.Application.StartupPath;
            //更新位置。
            string appUrl = "http://module.ischool.com.tw/module/89/MOD_ischedule/app.xml";
            string script_path = Path.Combine(basePath, "update_padding.xml");
            ManifestResolver resolver = new ManifestResolver(appUrl, VersionOption.Stable);
            resolver.VerifySignature = true;

            UpdateHelper uc = new UpdateHelper();
            uc.Resolver = resolver;
            uc.Install = new InstallDescriptor(basePath);

            //更新過程的訊息。
            resolver.ProgressMessage += new EventHandler<ProgressMessageEventArgs>(au_ProgressMessage);

            uc.DownloadStarted += new EventHandler(au_DownloadStarted);
            uc.DownloadProgressChanged += new EventHandler<DownloadProgressEventHandler>(au_DownloadProgressChanged);
            uc.DownloadCompleted += new EventHandler(au_DownloadCompleted);
            uc.ProgressMessage += new EventHandler<ProgressMessageEventArgs>(au_ProgressMessage);

            //MainForm.SetBarMessage("檢查更新…");
            ThreadPool.QueueUserWorkItem(arg =>
            {
                resolver.Resolve();

                if (uc.CheckUpdate())
                {
                    PaddingScript script = new PaddingScript();
                    script.WaitRelease(System.Reflection.Assembly.GetExecutingAssembly().Location);

                    uc.Update(script);

                    script.Delete(uc.Install.TemporalFolder);
                    script.Delete(script_path);
                    script.DeleteEmpty(basePath);
                    script.StartProcess(System.Reflection.Assembly.GetExecutingAssembly().Location);

                    script.Save(script_path);

                    MessageBox.Show("系統自動更新完成，將重新啟動。");

                    Application.Restart();
                }
                else
                {
                    //MessageBox.Show("已經是最新版。");
                }
            });
        }

        static void au_ProgressMessage(object sender, ProgressMessageEventArgs e)
        {
           // MainForm.SetBarMessage(e.Message);
        }

        static void au_DownloadCompleted(object sender, EventArgs e)
        {
            //MainForm.SetBarMessage("下載完成。");
        }

        static void au_DownloadProgressChanged(object sender, DownloadProgressEventHandler e)
        {
            //MainForm.Progress.Value = e.ProgressPercentage;
        }

        static void au_DownloadStarted(object sender, EventArgs e)
        {
            //MainForm.SetBarMessage("開始下載新版本...");
            //MainForm.Progress.Value = 0;
        }
    }
}