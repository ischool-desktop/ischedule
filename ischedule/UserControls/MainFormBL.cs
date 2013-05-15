using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ischedule
{
    public class MainFormBL
    {
        private static MainForm mMainForm = null;

        /// <summary>
        /// 實際表單。設計成 Singleton 以便在其他地方可以存取。
        /// </summary>
        public static MainForm Instance
        {
            get
            {
                if (mMainForm == null)
                    mMainForm = new MainForm();

                return mMainForm;
            }
        }
    }
}