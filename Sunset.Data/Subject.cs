
namespace Sunset.Data
{
    /// <summary>
    /// 科目
    /// </summary>
    public class Subject
    {
        private string mSubjectID;
        private string mName;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="SubjectID">科目編號</param>
        /// <param name="Name">科目名稱</param>
        public Subject(string SubjectID, string Name)
        {
            //指定科目編號及名稱
            this.mSubjectID = SubjectID;
            this.mName = Name;
        }

        /// <summary>
        /// 科目編號
        /// </summary>
        public string WhatID { get { return this.mSubjectID; } }

        /// <summary>
        /// 科目名稱
        /// </summary>
        public string Name { get { return this.mName; } }
    }
}