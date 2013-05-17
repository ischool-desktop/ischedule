using System.Collections.Generic;
using System.Windows.Forms;
using Sunset.Data;

namespace ischedule
{
    /// <summary>
    /// 事件選取
    /// </summary>
    public partial class frmEventSelector : BaseForm
    {
        private Scheduler schLocal = Scheduler.Instance;

        /// <summary>
        /// 傳入事件列表
        /// </summary>
        /// <param name="EventIDs"></param>
        public frmEventSelector(List<string> EventIDs)
        {
            InitializeComponent();

            List<CEvent> Events = new List<CEvent>();

            foreach (string EventID in EventIDs)
            {
                if (!string.IsNullOrEmpty(EventID))
                {
                    CEvent Event = schLocal.CEvents[EventID];

                    if (!Event.ManualLock)
                        Events.Add(Event);
                }
            }

            grdTestEvent.AutoGenerateColumns = false;
            grdTestEvent.DataSource = Events;
        }

        /// <summary>
        /// 選擇類別
        /// </summary>
        public int SelectorType { get; private set; }

        /// <summary>
        /// 選擇事件
        /// </summary>
        public string SelectedEventID
        {
            get
            {
                return SelectedEventIDs.Count > 0 ? SelectedEventIDs[0] : string.Empty;
            }
        }

        /// <summary>
        /// 選擇事件清單
        /// </summary>
        public List<string> SelectedEventIDs
        {
            get
            {
                List<string> EventIDs = new List<string>();

                foreach (DataGridViewRow Row in grdTestEvent.SelectedRows)
                {
                    string EventID = "" + Row.Cells[6].Value;

                    EventIDs.Add(EventID);
                }

                return EventIDs;
            }
        }

        /// <summary>
        /// 顯示可排課
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCandidate_Click(object sender, System.EventArgs e)
        {
            SelectorType = 1;

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        /// <summary>
        /// 回復至未排課
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFree_Click(object sender, System.EventArgs e)
        {
            SelectorType = 2;

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
    }
}