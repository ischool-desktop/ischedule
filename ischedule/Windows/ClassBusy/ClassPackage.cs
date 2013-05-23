using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sunset.Data;

namespace ischedule
{
    public class ClassPackage
    {
        /// <summary>
        /// 班級
        /// </summary>
        public Class Class { get; set; }

        /// <summary>
        /// 教師不排課時段
        /// </summary>
        public List<Appointment> ClassBusys { get; set; }
    }
}