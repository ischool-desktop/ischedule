using System;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using ischedule.Properties;

namespace ischedule
{
    /// <summary>
    /// 代表節次物件
    /// </summary>
    public partial class ucPeriod : UserControl
    {
        private const int labelCount = 7;
        private bool _isSelected;
        private Color _selectedColor = Color.Cornsilk;
        private Color _mouseoverColor = System.Drawing.SystemColors.GradientInactiveCaption;
        private Color lvSchedulableBackColor = Color.FromArgb(254, 252, 128);
        private Color lvSchedulableForeColor = Color.White;

        public Action<object, EventArgs> ActionEvent { get; set; }

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public ucPeriod()
        {
            InitializeComponent();
            this.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.picAction.Click += (sender, e) =>
            {
                if (ActionEvent != null)
                    ActionEvent.Invoke(sender, e);
            };
        }

        /// <summary>
        /// 設定樣式
        /// </summary>
        /// <param name="BackColor">背景顏色</param>
        /// <param name="ForeColor">前景顏色</param>
        /// <param name="Picture">圖片</param>
        public void SetStyle(Color BackColor, Color ForeColor, Bitmap Picture)
        {
            this.BackColor = BackColor;
            this.ForeColor = ForeColor;
            this.picAction.Image = Picture;
            this.picAction.Tag = string.Empty;
        }

        /// <summary>
        /// 設為可排課時段
        /// </summary>
        public void SetSchedulable()
        {
            this.BackColor = lvSchedulableBackColor;
            this.ForeColor = lvSchedulableForeColor;
            this.picAction.Image = Resources.blank;
            this.picAction.Tag = string.Empty;
        }

        /// <summary>
        /// 設定樣式
        /// </summary>
        /// <param name="BackColor">背景顏色</param>
        /// <param name="ForeColor">前景顏色</param>
        /// <param name="Picture">圖片</param>
        public void SetStyle(Color BackColor, Color ForeColor, Bitmap Picture,string Tag)
        {
            this.BackColor = BackColor;
            this.ForeColor = ForeColor;
            this.picAction.Image = Picture;
            this.picAction.Tag = Tag;
        }

        /// <summary>
        /// 設定樣式
        /// </summary>
        /// <param name="BackColor">背景顏色</param>
        /// <param name="ForeColor">前景顏色</param>
        /// <param name="Picture">圖片</param>
        /// <param name="BorderColor">線條顏色</param>
        /// <param name="BorderThickness">線條寬度</param>
        public void SetStyle(Color BackColor, Color ForeColor, Bitmap Picture, Color BorderColor, int BorderThickness,string Action)
        {
            this.BackColor = BackColor;
            this.ForeColor = ForeColor;
            this.picAction.Image = Picture;
            this.picAction.Tag = Action;
            //this.pnlBottom.Height = BorderThickness;
            //this.pnlBottom.BackColor = BorderColor;
            //this.pnlRight.Width = BorderThickness;
            //this.pnlRight.BackColor = BorderColor;
        }

        /// <summary>
        /// 設定樣式
        /// </summary>
        /// <param name="BackColor">背景顏色</param>
        /// <param name="ForeColor">前景顏色</param>
        /// <param name="Picture">圖片</param>
        /// <param name="BorderColor">線條顏色</param>
        /// <param name="BorderThickness">線條寬度</param>
        public void SetStyle(Color BackColor, Color ForeColor, Bitmap Picture, Color BorderColor, int BorderThickness)
        {
            this.BackColor = BackColor;
            this.ForeColor = ForeColor;
            this.picAction.Image = Picture;
            //this.pnlBottom.BackColor = BorderColor;
            //this.pnlRight.BackColor = BorderColor;

            //this.pnlBottom.Height = BorderThickness;
            //this.pnlRight.Width = BorderThickness;
        }

        /// <summary>
        /// 設定為空白
        /// </summary>
        public void SetBlank()
        {
            for (int i = 1; i <= labelCount; i++)
            {
                LabelX label = GetLabel(i);
                if (label != null)
                {
                    label.Text = string.Empty;
                }
            }
            
            picAction.Image = Resources.blank;
            //this.BackColor = DefaultBackColor;
            //this.ForeColor = DefaultForeColor;
        }

        /// <summary>
        /// 取得內容
        /// </summary>
        /// <param name="Index"></param>
        /// <returns></returns>
        public LabelX GetLabel(int Index)
        {
            LabelX result = null;

            switch (Index)
            {
                case 1:
                    result = labelX1;
                    break;
                case 2:
                    result = labelX2;
                    break;
                case 3:
                    result = labelX3;
                    break;
                case 4:
                    result = labelX4;
                    break;
                case 5:
                    result = labelX5;
                    break;
                case 6:
                    result = labelX6;
                    break;
                case 7:
                    result = labelX7;
                    break;
            }

            if (result != null)
                    result.Visible = true;

            return result;
        }

        /// <summary>
        /// 設定內容
        /// </summary>
        /// <param name="Index"></param>
        /// <param name="Content"></param>
        public void SetContent(int Index,string Content)
        {
            LabelX label = GetLabel(Index);

            if (label != null)
                label.Text = Content;
        }

        public void SetTopToZero()
        {
            pnlTop.Height = 0;
        }

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;

                if (!_isSelected)
                {
                    pnlBottom.BackColor = Color.Silver;
                    pnlBottom.Height = 1;

                    pnlRight.BackColor = pnlBottom.BackColor;
                    pnlRight.Width = pnlBottom.Height;

                    pnlTop.BackColor = pnlBottom.BackColor;
                    pnlTop.Height = 0;

                    pnlLeft.BackColor = pnlBottom.BackColor;
                    pnlLeft.Width = 0;               
                }
                else
                {
                    pnlBottom.BackColor = Color.Orange;
                    pnlBottom.Height = 3;

                    pnlRight.BackColor = pnlBottom.BackColor;
                    pnlRight.Width = pnlBottom.Height;

                    pnlTop.BackColor = pnlBottom.BackColor;
                    pnlTop.Height = pnlBottom.Height;

                    pnlLeft.BackColor = pnlBottom.BackColor;
                    pnlLeft.Width = pnlBottom.Height;                 
                }
            }
        }

        private void ucPeriod_MouseEnter(object sender, EventArgs e)
        {
            //this.BackColor = (this.IsSelected ? this._selectedColor : this._mouseoverColor );
            this.Cursor = Cursors.Hand;
        }

        private void ucPeriod_MouseLeave(object sender, EventArgs e)
        {
            //this.BackColor = (this.IsSelected ? this._selectedColor :  this.DefaultBackColor );
            this.Cursor = Cursors.Default;
            //Console.WriteLine(string.Format("{0}, isselected:{1}, backColor:{2}", this.courseName, this.IsSelected.ToString(), this.BackColor.ToString()));
        }

        private void ucPeriod_MouseHover(object sender, EventArgs e)
        {
            ucPeriod_MouseEnter(sender, e);
        }

        private void ucPeriod_MouseMove(object sender, MouseEventArgs e)
        {
            ucPeriod_MouseEnter(sender, e);
        }

        private void ucPeriod_Load(object sender, EventArgs e)
        {
            for (int i = 1; i <= labelCount; i++)
            {
                LabelX label = GetLabel(i);

                if (label != null)
                {
                    //label.MouseHover += new EventHandler(ucPeriod_MouseHover);
                    //label.MouseMove += new MouseEventHandler(ucPeriod_MouseMove);
                    //label.MouseLeave += new EventHandler(ucPeriod_MouseLeave);
                    label.Click += new EventHandler(label_Click);
                }
            }
        }

        private void label_Click(object sender, EventArgs e)
        {
            LabelX label = sender as LabelX;

            string Tag = "" + label.Tag;

            if (!string.IsNullOrEmpty(Tag))
            {
               string[] result = Tag.Split(new char[] { ':' });

               if (result.Length == 2)
               {
                   string AssocType = result[0];
                   string AssocID = result[1];

                   MainFormBL.Instance.OpenTeacherLPView(int.Parse(AssocType), AssocID, true, string.Empty, false);
               }
            }
        }
    }
}
