using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ischedule
{
    /// <summary>
    /// Command模式
    /// </summary>
    public interface IScheduleCommand
    {
        void Do();
        void Undo();
        string Description();
    }
}