using System;
using System.Collections.Generic;
using System.Linq;
using Sunset.Data;

namespace ischedule
{
    /// <summary>
    /// 屬性修改
    /// </summary>
    public partial class frmEventProperty : BaseForm
    {
        private CEvent evtChange;
        private Scheduler schLocal = Scheduler.Instance;
        bool bReentrantFlag;

        /// <summary>
        /// 建構式，傳入事件編號
        /// </summary>
        /// <param name="EventID"></param>
        public frmEventProperty(string EventID)
        {
            InitializeComponent();

            int nIndex1;
            int nIndex2;
            int nIndex3;

            bReentrantFlag = true;
            evtChange = schLocal.CEvents[EventID];

            //populate dialog controls' value
            //txtEventID.Text = evtChange.EventID;
            txtLength.Text = "" + evtChange.Length;
            txtPriority.Text = "" + evtChange.Priority;
            txtWDCondition.Text = evtChange.WeekDayCondition;
            txtPDCondition.Text = evtChange.PeriodCondition;
            txtComment.Text = evtChange.Comment;

            chkLongBreak.Checked = evtChange.AllowLongBreak;
            chkDup.Checked = evtChange.AllowDuplicate;

            #region 加入教師清單，並指定目前教師
            nIndex1 = -1;
            nIndex2 = -1;
            nIndex3 = -1;

            foreach (Teacher whoMember in schLocal.Teachers)
            {
                string[] EventIDs = evtChange.EventID.Split(new char[] { ',' });

                //假設教師有在該分課的學校授教才加入該教師
                if (whoMember.SourceIDs.Select(x => x.DSNS).Contains(EventIDs[0]))
                {
                    cboWho1.Items.Add(whoMember.WhoID);
                    if (whoMember.WhoID == evtChange.TeacherID1)
                        nIndex1 = cboWho1.Items.Count - 1;
                }

                if (whoMember.SourceIDs.Select(x => x.DSNS).Contains(EventIDs[0]))
                {
                    cboWho2.Items.Add(whoMember.WhoID);
                    if (whoMember.WhoID == evtChange.TeacherID2)
                        nIndex2 = cboWho2.Items.Count - 1;
                }

                if (whoMember.SourceIDs.Select(x => x.DSNS).Contains(EventIDs[0]))
                {
                    cboWho3.Items.Add(whoMember.WhoID);
                    if (whoMember.WhoID == evtChange.TeacherID3)
                        nIndex3 = cboWho3.Items.Count - 1;
                }
            }

            cboWho1.SelectedIndex = nIndex1;
            cboWho2.SelectedIndex = nIndex2;
            cboWho3.SelectedIndex = nIndex3;
            #endregion

            #region 加入班級清單，並指定目前班級
            nIndex1 = 0;

            foreach (Class whomMember in schLocal.Classes)
            {
                cboWhom.Items.Add(whomMember.Name);
                if (whomMember.WhomID == evtChange.ClassID)
                    nIndex1 = cboWhom.Items.Count - 1;
            }

            cboWhom.SelectedIndex = nIndex1;
            #endregion

            #region 加入場地清單，並指定目前場地
            nIndex1 = 0;

            foreach (Classroom whereMember in schLocal.Classrooms)
            {
                cboWhere.Items.Add(whereMember.WhereID);
                if (whereMember.WhereID == evtChange.ClassroomID)
                    nIndex1 = cboWhere.Items.Count - 1;
            }

            cboWhere.SelectedIndex = nIndex1;
            #endregion

            #region 加入科目清單
            nIndex1 = 0;

            foreach (Subject whatMember in schLocal.Subjects)
            {
                cboWhat.Items.Add(whatMember.WhatID);
                if (whatMember.WhatID == evtChange.SubjectID)
                    nIndex1 = cboWhat.Items.Count - 1;
            }

            cboWhat.SelectedIndex = nIndex1;
            #endregion

            #region 加入單雙週
            nIndex1 = 0;

            cboWeekFlag.Items.Add("單週");
            cboWeekFlag.Items.Add("雙週");
            cboWeekFlag.Items.Add("單雙週");
            cboWeekFlag.SelectedIndex = evtChange.WeekFlag - 1;
            #endregion

            //Populate split info listbox
            bool blnContainsSchEvent = false;
            int intRelatedEventCount = 0;

            #region 加入相關分課
            List<object> RelatedEvetns = new List<object>();

            foreach (CEvent evtMember in schLocal.CEvents)
            {
                if ((evtMember.CourseID == evtChange.CourseID) &&
                    (evtMember.EventID != evtChange.EventID))
                {
                    if (evtMember.WeekDay == 0)
                    {
                        if (evtMember.ClassroomID == evtChange.ClassroomID)
                        {
                            RelatedEvetns.Add(new
                            {
                                RelatedEvent = evtMember.Length,
                                EventID = evtMember.EventID
                            });

                            intRelatedEventCount++;
                        }
                    }
                    else
                        blnContainsSchEvent = true;
                }
            }

            lstSplit.DisplayMember = "RelatedEvent";
            lstSplit.ValueMember = "EventID";
            lstSplit.Items.AddRange(RelatedEvetns.ToArray());
            #endregion

            //If other events that belong to the same Course does not
            //available for scheduling, disable the ability to change teacher.
            //cboWho.IsEnabled = !blnContainsSchEvent;
            //chkDup.IsEnabled = !blnContainsSchEvent;

            //修改成只有當課程下的分課只有一個才能修改授課教師及本日是否重覆屬性
            cboWho1.Enabled = intRelatedEventCount == 0;
            cboWho2.Enabled = intRelatedEventCount == 0;
            cboWho3.Enabled = intRelatedEventCount == 0;
            chkDup.Enabled = intRelatedEventCount == 0;

            //Disable controls if it is a scheduled event
            btnMerge.Enabled = false;

            if (evtChange.WeekDay != 0)
            {
                cboWho1.Enabled = false;
                cboWho2.Enabled = false;
                cboWho3.Enabled = false;
                cboWhom.Enabled = false;
                cboWhere.Enabled = false;
                cboWhat.Enabled = false;
                cboWeekFlag.Enabled = false;
                txtWDCondition.Enabled = false;
                txtPDCondition.Enabled = false;
                txtLength.Enabled = false;
                txtPriority.Enabled = false;
                chkLongBreak.Enabled = false;
                chkDup.Enabled = false;
                btnConfirm.Enabled = false;
                btnCancel.Enabled = false;
                btnSplit.Enabled = false;
            }
            else
            {
                if (evtChange.Length < 2)
                    btnSplit.Enabled = false;

                if (lstSplit.Items.Count == 0)
                    lstSplit.Enabled = false;
            }

            bReentrantFlag = false;

            btnCancel.Click += (sender, e) => this.Close();

            cboWhat.SelectionChangeCommitted += (sender, e) =>
            {
                if (bReentrantFlag) return;

                if (!("" + cboWhat.SelectedValue).Equals(evtChange.SubjectID))
                    DisableSplitMergeControl();
            };

            cboWhere.SelectionChangeCommitted += (sender, e) =>
            {
                if (bReentrantFlag) return;

                if (!("" + cboWhere.SelectedValue).Equals(evtChange.ClassroomID))
                    DisableSplitMergeControl();
            };

            cboWho1.SelectionChangeCommitted += (sender, e) =>
            {
                if (bReentrantFlag) return;

                if (!("" + cboWho1.SelectedValue).Equals(evtChange.TeacherID1))
                    DisableSplitMergeControl();
            };

            cboWho2.SelectionChangeCommitted += (sender, e) =>
            {
                if (bReentrantFlag) return;

                if (!("" + cboWho2.SelectedValue).Equals(evtChange.TeacherID1))
                    DisableSplitMergeControl();
            };

            cboWho3.SelectionChangeCommitted += (sender, e) =>
            {
                if (bReentrantFlag) return;

                if (!("" + cboWho3.SelectedValue).Equals(evtChange.TeacherID1))
                    DisableSplitMergeControl();
            };

            cboWhom.SelectionChangeCommitted += (sender, e) =>
            {
                if (bReentrantFlag) return;

                if (!("" + cboWhom.SelectedValue).Equals("" + evtChange.ClassID))
                    DisableSplitMergeControl();
            };
        }

        /// <summary>
        /// 將合併及分割相關元件失效
        /// </summary>
        private void DisableSplitMergeControl()
        {
            lstSplit.Enabled = false;
            btnSplit.Enabled = false;
            btnMerge.Enabled = false;
        }

        /// <summary>
        /// 分割
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSplit_Click(object sender, EventArgs e)
        {
            int Length;

            if (int.TryParse(txtLength.Text, out Length))
            {
                frmEventSplit EventSplit = new frmEventSplit(Length);

                EventSplit.SplitEvent += (vsender, ve) =>
                {
                    CEvent evtNew;
                    int nSecCnt;

                    nSecCnt = EventSplit.SectionCount;

                    if (nSecCnt == 1) return;

                    //Change current event length
                    schLocal.ChangeEventLength(evtChange.EventID, EventSplit[1]);

                    txtLength.Text = "" + EventSplit[1];

                    for (int i = 2; i <= nSecCnt; i++)
                    {
                        evtNew = new CEvent();
                        evtNew.EventID = evtChange.EventID;
                        evtNew.TeacherID1 = evtChange.TeacherID1;
                        evtNew.TeacherID2 = evtChange.TeacherID2;
                        evtNew.TeacherID3 = evtChange.TeacherID3;
                        evtNew.ClassID = evtChange.ClassID;
                        evtNew.ClassroomID = evtChange.ClassroomID;
                        evtNew.SubjectID = evtChange.SubjectID;
                        evtNew.Length = EventSplit[i];
                        evtNew.WeekFlag = evtChange.WeekFlag;
                        evtNew.Priority = evtChange.Priority;
                        evtNew.WeekDayCondition = evtChange.WeekDayCondition;
                        evtNew.PeriodCondition = evtChange.PeriodCondition;
                        evtNew.AllowLongBreak = evtChange.AllowLongBreak;
                        evtNew.CourseID = evtChange.CourseID;
                        evtNew.TimeTableID = evtChange.TimeTableID;
                        evtNew.AllowDuplicate = evtChange.AllowDuplicate;
                        evtNew.CourseGroup = evtChange.CourseGroup;
                        evtNew.CourseName = evtChange.CourseName;
                        evtNew.LimitNextDay = evtChange.LimitNextDay;
                        evtNew.SubjectAlias = evtChange.SubjectAlias;
                        evtNew.Comment = evtChange.Comment;

                        schLocal.InsertEvent(evtNew);

                        lstSplit.Items.Add(new
                        {
                            RelatedEvent = evtNew.Length,
                            EventID = evtNew.EventID
                        });
                    }

                    lstSplit.Enabled = true;
                };

                EventSplit.ShowDialog();
            }
        }

        /// <summary>
        /// 合併，測試OK
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMerge_Click(object sender, EventArgs e)
        {
            int nSecCnt = lstSplit.SelectedItems.Count;
            int nPrdsAdded = 0;
            string idEvent;
            List<object> SelectItems = new List<object>();

            #region 將要合併的分課長度加起來，並且將合併分課刪除
            foreach (dynamic SelectItem in lstSplit.SelectedItems)
            {
                idEvent = SelectItem.EventID;
                nPrdsAdded += schLocal.CEvents[idEvent].Length;
                schLocal.DeleteEvent(idEvent);
                SelectItems.Add(SelectItem);
            }

            SelectItems.ForEach(x => lstSplit.Items.Remove(x));
            #endregion

            if (lstSplit.SelectedItems.Count == 0)
                btnMerge.Enabled = false;

            #region 實際修改事件長度
            schLocal.ChangeEventLength(evtChange.EventID, evtChange.Length + nPrdsAdded);

            txtLength.Text = "" + evtChange.Length;
            #endregion

            if (evtChange.Length > 1)
                btnSplit.Enabled = true;
        }

        /// <summary>
        /// 選取改變
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstSplit_SelectedValueChanged(object sender, EventArgs e)
        {
            btnMerge.Enabled = lstSplit.SelectedItems.Count == 0 ? false : true;
        }

        /// <summary>
        /// 確認
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            //dynamic SelectedWho1 = cboWho1.SelectedItem;
            //dynamic SelectedWho2 = cboWho2.SelectedItem;
            //dynamic SelectedWho3 = cboWho3.SelectedItem;
            //dynamic SelectedWhere = cboWhere.SelectedItem;

            string WhoID1 = ""+cboWho1.SelectedItem; //SelectedWho1 != null ? SelectedWho1.WhoID : string.Empty;
            string WhoID2 = "" + cboWho2.SelectedItem; //SelectedWho2 != null ? SelectedWho2.WhoID : string.Empty;
            string WhoID3 = "" + cboWho3.SelectedItem;  //SelectedWho3 != null ? SelectedWho3.WhoID : string.Empty;

            string WhereID = "" + cboWhere.SelectedItem; //SelectedWhere.WhereID;
            byte WeekFlag = (byte)(cboWeekFlag.SelectedIndex + 1);
            bool IsLongBreak = chkLongBreak.Checked ? chkLongBreak.Checked : false;
            bool IsDuplicate = chkDup.Checked ? chkDup.Checked : false;
            string WeekDayCondition = txtWDCondition.Text;
            string PeriodCondition = txtPDCondition.Text;
            string Comment = txtComment.Text;

            schLocal.ChangeEventProperty(evtChange.EventID,
                                         WhoID1,
                                         WhoID2,
                                         WhoID3,
                                         WhereID,
                                         WeekFlag,
                                         WeekDayCondition,
                                         PeriodCondition,
                                         IsLongBreak,
                                         IsDuplicate,
                                         Comment);
            evtChange = null;

            this.Close();
        }
    }
}