
namespace Sunset
{
    /// <summary>
    /// 排課使用課程延伸資訊
    /// </summary>
    [FISCA.UDT.TableName("scheduler.course_extension")]
    public class CourseExtension : FISCA.UDT.ActiveRecord
    {
        /// <summary>
        /// 課程系統編號
        /// </summary>
        [FISCA.UDT.Field(Field = "ref_course_id")]
        public int CourseID { get; set; }

        /// <summary>
        /// 課程所屬時間表編號
        /// </summary>
        [FISCA.UDT.Field(Field = "ref_timetable_id")]
        public int? TimeTableID { get; set; }

        /// <summary>
        /// 課程分段分割設定
        /// </summary>
        [FISCA.UDT.Field(Field = "split_spec")]
        public string SplitSpec { get; set; }

        /// <summary>
        /// 課程分段樣版，單雙週
        /// </summary>
        [FISCA.UDT.Field(Field = "week_flag")]
        public int WeekFlag { get; set; }

        /// <summary>
        /// 課程分段樣版，是否允許課程分段排在同一天
        /// </summary>
        [FISCA.UDT.Field(Field = "allow_duplicate")]
        public bool AllowDup { get; set; }

        /// <summary>
        /// 課程分段樣版，教室編號
        /// </summary>
        [FISCA.UDT.Field(Field = "ref_classroom_id")]
        public int? ClassroomID { get; set; }

        /// <summary>
        /// 課程分段樣版，是否允許排中午時段
        /// </summary>
        [FISCA.UDT.Field(Field = "long_break")]
        public bool LongBreak { get; set; }

        /// <summary>
        /// 課程分段樣版，星期條件
        /// </summary>
        [FISCA.UDT.Field(Field = "weekday_condition")]
        public string WeekDayCond { get; set; }

        /// <summary>
        /// 課程分段樣版，節次條件
        /// </summary>
        [FISCA.UDT.Field(Field = "period_condition")]
        public string PeriodCond { get; set; }
    }
}