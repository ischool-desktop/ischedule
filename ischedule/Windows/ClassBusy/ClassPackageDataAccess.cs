using System.Collections.Generic;
using Sunset.Data;

namespace ischedule
{
    /// <summary>
    /// 班級資料存取
    /// </summary>
    public class ClassPackageDataAccess : IConfigurationDataAccess<ClassPackage>
    {
        private Scheduler schLocal = Scheduler.Instance;

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public ClassPackageDataAccess()
        {

        }

        #region IConfigurationDataAccess<TeacherPackage> 成員

        #region 未使用到的方法
        /// <summary>
        /// 更新班級，目前未用到
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="NewKey"></param>
        /// <returns></returns>
        public string Update(string Key, string NewKey)
        {
            return string.Empty;
        }

        /// <summary>
        /// 新增班級，目前未用到
        /// </summary>
        /// <param name="NewKey"></param>
        /// <param name="CopyKey"></param>
        /// <returns></returns>
        public string Insert(string NewKey, string CopyKey)
        {
            return string.Empty;
        }
   
        /// <summary>
        /// 刪除班級，目前未用到
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public string Delete(string Key)
        {
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

        /// <summary>
        /// 顯示名稱
        /// </summary>
        public string DisplayName
        {
            get { return "班級不排課時段"; }
        }

        /// <summary>
        /// 取得所有班級名稱
        /// </summary>
        /// <returns></returns>
        public List<string> SelectKeys()
        {
            List<string> Result = new List<string>();

            foreach (Class vClass in schLocal.Classes)
                if (!vClass.Name.Equals("無"))
                {
                    string[] ClassIDs = vClass.ClassID.Split(new char[] { ',' });

                    if (ClassIDs.Length == 2)
                    {
                        if (!ClassIDs[1].Equals(vClass.Name))
                            Result.Add(vClass.Name);
                    }
                }

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

            foreach (Class vClass in schLocal.Classes)
            {
                if (vClass.Name.Contains(SearchText))
                    Result.Add(vClass.Name);
            }

            return Result;
        }

        /// <summary>
        /// 根據班級名稱取得不排課時段
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public ClassPackage Select(string Key)
        {
            ClassPackage vClassPackage = new ClassPackage();
            vClassPackage.Class = schLocal.Classes.GetByName(Key);
            vClassPackage.ClassBusys = vClassPackage.Class.GetAppointments();

            return vClassPackage;
        }

        /// <summary>
        /// 更新不排課時段
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public string Save(ClassPackage Value)
        {
            Value.Class.UpdateBusyAppointments(Value.ClassBusys);

            return string.Empty;
        }
        #endregion
    }
}