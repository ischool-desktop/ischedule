using System.Collections.Generic;

namespace ischedule
{
    public class MainFormBL
    {
        private static List<IScheduleCommand> actionList = new List<IScheduleCommand>();
        private static List<IScheduleCommand> reDoList = new List<IScheduleCommand>();
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

        /// <summary>
        /// 設定命令的介面
        /// </summary>
        private static void SetCommandButtonEnable()
        {
            Instance.SetCommandButtonEnable(actionList.Count > 0, reDoList.Count > 0);
        }

        /// <summary>
        /// 新增動作
        /// </summary>
        /// <param name="Command"></param>
        public static void AddCommand(IScheduleCommand Command)
        {
            actionList.Add(Command);
            SetCommandButtonEnable();
        }

        /// <summary>
        /// 回復
        /// </summary>
        public static void Undo()
        {
            if (actionList.Count > 0)
            {
                IScheduleCommand cmd = actionList[actionList.Count - 1];
                cmd.Undo();
                reDoList.Add(cmd);
                actionList.Remove(cmd);
                SetCommandButtonEnable();
            }
        }

        /// <summary>
        /// 重做
        /// </summary>
        public static void Redo()
        {
            if (reDoList.Count > 0)
            {
                IScheduleCommand cmd = reDoList[reDoList.Count - 1];
                cmd.Do();
                reDoList.Remove(cmd);
                actionList.Add(cmd);
                SetCommandButtonEnable();
            }
        }
    }
}