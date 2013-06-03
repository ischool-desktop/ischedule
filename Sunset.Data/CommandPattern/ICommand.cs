using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunset.Data
{
    /// <summary>
    /// Command模式
    /// </summary>
    public interface ICommand
    {
        void Do();
        void Undo();
        string Description();
    }
}