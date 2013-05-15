
namespace Sunset.Data
{
    /// <summary>
    /// 科目
    /// </summary>
    public class Subject
    {
        private string mWhatID;
        private string mName;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="WhatID">科目編號</param>
        /// <param name="Name">科目名稱</param>
        public Subject(string WhatID, string Name)
        {
            //指定科目編號及名稱
            this.mWhatID = WhatID;
            this.mName = Name;
        }

        /// <summary>
        /// 科目編號
        /// </summary>
        public string WhatID { get { return this.mWhatID; } }

        /// <summary>
        /// 科目名稱
        /// </summary>
        public string Name { get { return this.mName; } }
    }
}