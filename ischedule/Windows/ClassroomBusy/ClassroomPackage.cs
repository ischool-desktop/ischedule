using System.Collections.Generic;
using Sunset.Data;

namespace ischedule
{
    public class ClassroomPackage
    {
        /// <summary>
        /// 場地
        /// </summary>
        public Classroom Classroom { get; set; }

        /// <summary>
        /// 場地不排課時段
        /// </summary>
        public List<Appointment> ClassroomBusys { get; set; }
    }
}