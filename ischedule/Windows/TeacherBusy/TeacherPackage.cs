using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sunset.Data;

namespace ischedule
{
    /// <summary>
    /// 教師及教師不排課時段
    /// </summary>
    public class TeacherPackage
    {
        /// <summary>
        /// 教師
        /// </summary>
        public Teacher Teacher { get; set; }

        /// <summary>
        /// 教師不排課時段
        /// </summary>
        public List<Appointment> TeacherBusys { get; set; }
    }
}