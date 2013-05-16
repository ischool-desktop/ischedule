using System;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;

namespace ischedule
{
    /// <summary>
    /// 分割事件
    /// </summary>
    public partial class frmEventSplit : BaseForm
    {
        int nTotPeriods;    //總節數
        int nSectionCount;  //切割數
        bool bReentrantLock;

        /// <summary>
        /// 分割完成事件
        /// </summary>
        public event EventHandler SplitEvent;

        /// <summary>
        /// 分割完成事件
        /// </summary>
        /// <param name="nTot"></param>
        public frmEventSplit(int nTot)
        {
            InitializeComponent();

            nSectionCount = 1;
            nTotPeriods = nTot;

            lblPeriodsLeft.Text = ""+nTot; //剩餘節數

            #region Populate combo box
            //將節數加到清單中
            for (int i = 1; i <= nTot; i++)
                cboSelection0.Items.Add(i);

            //選到最後一個
            cboSelection0.SelectedIndex = cboSelection0.Items.Count - 1;
            #endregion

            #region Disable combos
            cboSelection1.Enabled = false;
            cboSelection2.Enabled = false;
            cboSelection3.Enabled = false;

            bReentrantLock = false;
            #endregion
        }

        /// <summary>
        /// 分割事件數量
        /// </summary>
        public int SectionCount
        {
            get { return nSectionCount; }
        }



        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (SplitEvent != null)
                SplitEvent(this, new EventArgs());

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 取得分割事件的節數長度
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int this[int index]
        {
            get
            {
                if (index > nSectionCount)
                    return 0;
                else
                {
                    switch (index)
                    {
                        case 1: return cboSelection0.SelectedIndex + 1;
                        case 2: return cboSelection1.SelectedIndex + 1;
                        case 3: return cboSelection2.SelectedIndex + 1;
                        case 4: return cboSelection3.SelectedIndex + 1;
                        default: return 0;
                    }
                }
            }
        }

        /// <summary>
        /// 取得Combox模擬物件陣列
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private ComboBoxEx GetComboxBox(int i)
        {
            switch (i)
            {
                case 0:
                    return cboSelection0;
                case 1:
                    return cboSelection1;
                case 2:
                    return cboSelection2;
                case 3:
                    return cboSelection3;
                default:
                    return null;
            }
        }


        private void cboSelection_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int nTotAlloc = 0;
            int nPrdsLeft;

            //避免重覆進入事件
            if (bReentrantLock)
                return;

            //Prevent reentrance
            bReentrantLock = true;

            Control Element = (sender as Control);

            int Index = int.Parse(Element.Name.Substring(Element.Name.Length - 1, 1));

            //Clear combo to the right of this combo
            for (int i = Index + 1; i <= 3; i++)
            {
                ComboBox cboSelection = GetComboxBox(i);

                if (cboSelection != null)
                {
                    cboSelection.Items.Clear();
                    cboSelection.Enabled = false;
                }
            }

            //Calc total allocated periods number
            for (int i = 0; i <= Index; i++)
            {
                ComboBox cboSelection = GetComboxBox(i);

                if (cboSelection != null)
                {
                    nTotAlloc += cboSelection.SelectedIndex + 1;
                }
            }

            nSectionCount = Index + 1;

            #region Populate the next combo

            nPrdsLeft = nTotPeriods - nTotAlloc;

            if (nPrdsLeft > 0)
            {
                if (Index < 3)
                {
                    ComboBox NextComboBox = GetComboxBox(Index + 1);

                    if (NextComboBox != null)
                    {
                        //加入剩餘節數
                        for (int i = 1; i <= nPrdsLeft; i++)
                            NextComboBox.Items.Add(i);

                        NextComboBox.Enabled = true;
                        NextComboBox.SelectedIndex = nPrdsLeft - 1;
                        NextComboBox.SelectedItem = nPrdsLeft;
                        nSectionCount++;
                    }
                }
            }
            #endregion

            #region Update "periods left" caption and command button
            if (Index <= 3)
            {
                lblPeriodsLeft.Text = "" + (nTotPeriods - nTotAlloc);
                btnConfirm.Enabled = true;
                //btnConfirm.Enabled = (nTotPeriods - nTotAlloc) == 0;
            }
            else
            {
                lblPeriodsLeft.Text = "0";
                btnConfirm.Enabled = true;
            }

            bReentrantLock = false;
            #endregion
        }
    }
}