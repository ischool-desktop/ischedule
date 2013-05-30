using System;
using System.Collections.Generic;
using Sunset.Data;

namespace ischedule
{
    /// <summary>
    /// 教師資料存取
    /// </summary>
    public class TeacherPackageDataAccess : IConfigurationDataAccess<TeacherPackage>
    {
        private Scheduler schLocal = Scheduler.Instance;

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public TeacherPackageDataAccess()
        {

        }

        #region IConfigurationDataAccess<TeacherPackage> 成員

        #region 未使用到的方法
        public string Update(string Key, string NewKey)
        {
            return string.Empty;
        }

        /// <summary>
        /// 新增教師
        /// </summary>
        /// <param name="NewKey">新增教師名稱</param>
        /// <param name="CopyKey">要複製的教師名稱</param>
        /// <returns>傳回新增成功或失敗訊息</returns>
        public string Insert(string NewKey, string CopyKey)
        {
            return string.Empty;
        }

        /// <summary>
        /// 根據鍵值刪除教師及教師不排課時段
        /// </summary>
        /// <param name="Key">教師名稱</param>
        /// <returns>成功或失敗的訊息</returns>
        public string Delete(string Key)
        {
            return string.Empty;
        }
        #endregion

        #region private function
        private Tuple<string, string> GetTeacherDetailName(string Name)
        {
            if (Name.Contains("(") && Name.EndsWith(")"))
            {
                int LeftIndex = Name.IndexOf("(");
                int RightIndex = Name.IndexOf(")");

                if (RightIndex > LeftIndex)
                {
                    string TeacherName = Name.Substring(0, LeftIndex);
                    string NickName = Name.Substring(LeftIndex + 1, RightIndex - LeftIndex - 1);

                    return new Tuple<string, string>(TeacherName, NickName);
                }
                else
                    return new Tuple<string, string>(Name, string.Empty);
            }
            else
                return new Tuple<string, string>(Name, string.Empty);
        }

        private string GetNicknameCondition(string Nickname)
        {
            if (string.IsNullOrEmpty(Nickname))
                return "nickname is null";
            else
                return "nickname='" + Nickname + "'";
        }
        #endregion

        /// <summary>
        /// 顯示名稱
        /// </summary>
        public string DisplayName
        {
            get { return "教師不排課時段"; }
        } 

        /// <summary>
        /// 取得所有教師名稱
        /// </summary>
        /// <returns></returns>
        public List<string> SelectKeys()
        {
            List<string> Result = new List<string>();

            foreach (Teacher vTeacher in schLocal.Teachers)
                if (!vTeacher.Name.Equals("無"))
                    if (!vTeacher.Name.Equals(vTeacher.SourceIDs[0].ID))
                        Result.Add(vTeacher.Name);

            return Result;
        }

        /// <summary>
        /// 搜尋
        /// </summary>
        /// <param name="SearchText">搜尋文字</param>
        /// <returns></returns>
        public List<string> Search(string SearchText)
        {
            List<string> Result = new List<string>();

            foreach (Teacher vTeacher in schLocal.Teachers)
            {
                if (vTeacher.Name.Contains(SearchText))
                    Result.Add(vTeacher.Name);
            }

            return Result;
        }

        /// <summary>
        /// 根據教師名稱取得教師及教師不排課時段
        /// </summary>
        /// <param name="Key">教師名稱</param>
        /// <returns>成功或失敗的訊息</returns>
        public TeacherPackage Select(string Key)
        {
            #region 產生預設的TeacherPackage，將Teacher為null，並將TeacherBusys產生為空集合
            TeacherPackage vTeacherPackage = new TeacherPackage();
            vTeacherPackage.Teacher = schLocal.Teachers[Key];
            vTeacherPackage.TeacherBusys = vTeacherPackage.Teacher.GetAppointments();
            #endregion

            return vTeacherPackage;
        }

        /// <summary>
        /// 根據TeacherPackage物件更新
        /// </summary>
        /// <param name="Value">TeacherPackage</param>
        /// <returns>成功或失敗的訊息</returns>
        public string Save(TeacherPackage Value)
        {
            Value.Teacher.UpdateBusyAppointments(Value.TeacherBusys);

            return string.Empty;
        }

        /// <summary>
        /// 其他指令
        /// </summary>
        public List<ICommand> ExtraCommands
        {
            get
            {
                List<ICommand> Commands = new List<ICommand>();

                return Commands;
            }
        }
        #endregion
    }
}