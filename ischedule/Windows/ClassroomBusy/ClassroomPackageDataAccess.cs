using System.Collections.Generic;
using Sunset.Data;

namespace ischedule
{
    /// <summary>
    /// 場地資料存取
    /// </summary>
    public class ClassroomPackageDataAccess : IConfigurationDataAccess<ClassroomPackage>
    {
        private Scheduler schLocal = Scheduler.Instance;

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public ClassroomPackageDataAccess()
        {

        }

        #region IConfigurationDataAccess<ClassroomPackage> 成員

        #region 未使用到的方法
        /// <summary>
        /// 更新場地，目前未用到
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="NewKey"></param>
        /// <returns></returns>
        public string Update(string Key, string NewKey)
        {
            return string.Empty;
        }

        /// <summary>
        /// 新增場地，目前未用到
        /// </summary>
        /// <param name="NewKey"></param>
        /// <param name="CopyKey"></param>
        /// <returns></returns>
        public string Insert(string NewKey, string CopyKey)
        {
            return string.Empty;
        }
   
        /// <summary>
        /// 刪除場地，目前未用到
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
            get { return "場地不排課時段"; }
        }

        /// <summary>
        /// 取得所有場地名稱
        /// </summary>
        /// <returns></returns>
        public List<string> SelectKeys()
        {
            List<string> Result = new List<string>();

            foreach (Classroom vClassroom in schLocal.Classrooms)
                if (!vClassroom.Name.Equals("無"))
                    Result.Add(vClassroom.Name);

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

            foreach (Classroom vClassroom in schLocal.Classrooms)
            {
                if (vClassroom.Name.Contains(SearchText))
                    Result.Add(vClassroom.Name);
            }

            return Result;
        }

        /// <summary>
        /// 根據場地名稱取得不排課時段
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public ClassroomPackage Select(string Key)
        {
            ClassroomPackage vClassroomPackage = new ClassroomPackage();
            vClassroomPackage.Classroom = schLocal.Classrooms[Key];
            vClassroomPackage.ClassroomBusys = vClassroomPackage.Classroom.GetAppointments();

            return vClassroomPackage;
        }

        /// <summary>
        /// 更新不排課時段
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public string Save(ClassroomPackage Value)
        {
            Value.Classroom.UpdateBusyAppointments(Value.ClassroomBusys);

            return string.Empty;
        }
        #endregion
    }
}