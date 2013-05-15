using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Web.Script.Serialization;
using Sunset.Data;

namespace ischedule
{
    /// <summary>
    /// 此類別負責 Winform 與 Browser 之間的溝通。
    /// </summary>
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public class JSScriptManager
    {
        private WebBrowser _wbr;
        private System.Diagnostics.Stopwatch w;
        private SchedulerType _schedulerType;
        private string currentTimeTableID ="";


        #region ===== event delcaration =====
        //1. 選擇已排課程節次
        //2. 解除鎖定 
        //3. 指定課程分段的星期節次
        #endregion

        public JSScriptManager(WebBrowser wbr, SchedulerType sType)
        {
            this._wbr = wbr;
            this._schedulerType = sType;
            string path = string.Format("file:///{0}/html/Scheduler.htm", Application.StartupPath);
            w = new System.Diagnostics.Stopwatch();
            w.Start();
            this._wbr.ObjectForScripting = this;

            this._wbr.Navigate(path);


            this._wbr.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted);
        }

        void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string sType = "t";
            if (this._schedulerType == SchedulerType.Class) sType = "c";
            if (this._schedulerType == SchedulerType.Place) sType = "p";

            object[] obj = new object[] { sType };
            this._wbr.Document.InvokeScript("setSchedulerType", obj);
        }
        #region ========  將資料傳入 web browser 的方法 ==============
        /* Methods to push data into browser  */
        public void SetTimeTable(TimeTable timeTable)
        {
            if (timeTable.TimeTableID != this.currentTimeTableID)
            {
                int weekdays = timeTable.Periods.MaxWeekDay;
                int periods = timeTable.Periods.MaxPeriodNo;
                string json = new JavaScriptSerializer().Serialize(timeTable.Periods.ToArray<Period>());
                object[] obj = new object[] { weekdays, periods, json };
                this._wbr.Document.InvokeScript("setTimeTable", obj);

                this.currentTimeTableID = timeTable.TimeTableID;    //reset flag
            }
        }

        //設定所有課程分段
        public void SetCourseSection(List<CEvent> data )
        {
            string json = new JavaScriptSerializer().Serialize(data.ToArray<CEvent>());
            //Console.WriteLine(json);

            object[] obj = new object[] { json };
            this._wbr.Document.InvokeScript("setCourseSections", obj);
        }

        //設定不排課時段
        public void SetBusyPeriods(List<Period> periods)
        {
            string json = new JavaScriptSerializer().Serialize(periods.ToArray<Period>());
            //Console.WriteLine(json);

            object[] obj = new object[] { json };
            this._wbr.Document.InvokeScript("setBusyPeriods", obj);
        }

        //設定資源衝突時段
        public void SetResourceConflictPeriods(List<Period> periods)
        {

        }

        public void ReFillData()
        {
            this._wbr.Document.InvokeScript("refillData");
        }

        public void Refresh()
        {
            this._wbr.Refresh();
        }

        /* 設定要被標示為選取的節次 */
        public void SetSelectedPeriods(List<string> periodKeys)
        {
            string json = new JavaScriptSerializer().Serialize(periodKeys);
            //Console.WriteLine(json);

            object[] obj = new object[] { json };
            this._wbr.Document.InvokeScript("setSelectedPeriods", obj);
        }

        #endregion


        #region ==============  讓 javascript 呼叫的方法  ==============
        /*   ======  Methods which javascript can invoke  ==== */
        public void LabelClicked(string lableType, string id)
        {
            MessageBox.Show(string.Format("{0}, {1}", lableType, id));
        }

        /* 某個已排課的課程分段被點選時 */
        public void CourseSectionClicked(string courseSectionID, string weekday, string periodIndex)
        {
            //1. 找出是否有同時段的需要選取
            List<string> keys = new List<string>();
            keys.Add(string.Format("{0}_{1}", weekday, periodIndex));
            keys.Add(string.Format("{0}_{1}", weekday, (int.Parse(periodIndex) +1).ToString()));

            string json = new JavaScriptSerializer().Serialize(keys);
            Console.WriteLine(json);

            object[] obj = new object[] { json };
            this._wbr.Document.InvokeScript("setSelectedPeriods", obj);

            //2. 找出資源衝突時段
            //3. 找出可排課時段
        }

        public void testData(string data)
        {
            object[] obj = new object[] { data };
            this._wbr.Document.InvokeScript("test", obj);
        }

        #endregion
    }



    /// <summary>
    /// 功課表類型，有：教師課表、班級課表，以及場地課表
    /// </summary>
    public enum SchedulerType
    {
        Teacher, Class, Place
    }
}
