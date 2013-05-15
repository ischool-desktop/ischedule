
namespace Sunset.Data.Integration
{
    /// <summary>
    /// 下載排課資料SQL指令
    /// </summary>
    public static  class NativeQuery
    {
        /// <summary>
        /// 根據學年度及學期下載班級資料
        /// </summary>
        //public const string ClassTemplateSQL = "select class.id,class.class_name,$scheduler.class_extension.ref_timetable_id,$scheduler.timetable.name as timetablename from class left outer join $scheduler.class_extension on $scheduler.class_extension.ref_class_id=class.id left outer join $scheduler.timetable on $scheduler.timetable.uid=$scheduler.class_extension.ref_timetable_id  where class.id in (select distinct class.id from course inner join class on course.ref_class_id=class.id  where subject!='' and school_year = {0} and semester ={1} ) order by class_name";

        /// <summary>
        /// 根據開始日期及結束日期從週課程分段下載班級資料
        /// </summary>
        //public const string WeekClassTemplateSQL = "select class.id,class.class_name,$scheduler.class_extension.ref_timetable_id,$scheduler.timetable.name as timetablename from class left outer join $scheduler.class_extension on $scheduler.class_extension.ref_class_id=class.id inner join $scheduler.timetable on $scheduler.timetable.uid=$scheduler.class_extension.ref_timetable_id  where class.id in (select distinct(course.ref_class_id) from course where id in (select distinct($scheduler.week_course_section.ref_course_id) from $scheduler.week_course_section where $scheduler.week_course_section.date>='{0}' and $scheduler.week_course_section.date<='{1}')) order by class_name";

        /// <summary>
        /// 根據開始日期及結束日期下載調課記錄
        /// </summary>
        public const string ExchangeCourseSectionTemplateSQL = "select uid,src_course_section_id,des_course_section_id,src_exchange_date,des_exchange_date,reason,to_char($scheduler.exchange_course_section.create_time,'yyyy/mm/dd HH24:MI') as create_time from $scheduler.exchange_course_section where $scheduler.exchange_course_section.src_exchange_date>='{0}' and $scheduler.exchange_course_section.src_exchange_date<='{1}' and $scheduler.exchange_course_section.des_exchange_date>='{0}' and $scheduler.exchange_course_section.des_exchange_date<='{1}'";

        /// <summary>
        /// 根據開始日期及結束日期下載代課記錄
        /// </summary>
        public const string SubstituteCourseSectionTemplateSQL = "select $scheduler.substitute_course_section.uid,$scheduler.substitute_course_section.ref_course_section_id,$scheduler.substitute_course_section.substitute_date,$scheduler.substitute_course_section.ref_teacher_id,$scheduler.substitute_course_section.absence_name,$scheduler.substitute_course_section.hourly_pay,$scheduler.substitute_course_section.reason,to_char($scheduler.substitute_course_section.create_time,'yyyy/mm/dd HH24:MI') as create_time,teacher.teacher_name from $scheduler.substitute_course_section inner join teacher on teacher.id=$scheduler.substitute_course_section.ref_teacher_id where $scheduler.substitute_course_section.substitute_date>='{0}' and $scheduler.substitute_course_section.substitute_date<='{1}'";

        /// <summary>
        /// 下載所有班級資料
        /// </summary>
        //public const string ClassTestSQL = "select class.id,class.class_name,$scheduler.class_extension.ref_timetable_id,$scheduler.timetable.name as timetablename from class left outer join $scheduler.class_extension on $scheduler.class_extension.ref_class_id=class.id inner join $scheduler.timetable on $scheduler.timetable.uid=$scheduler.class_extension.ref_timetable_id order by class_name";

        /// <summary>
        /// 根據學年度及學期下載課程分段
        /// </summary>
        //public const string CourseSectionTemplateSQL = "select $scheduler.course_section.uid,$scheduler.course_section.ref_course_id,course.course_name,$scheduler.course_extension.subject_alias_name,$scheduler.course_section.weekday,$scheduler.course_section.period,$scheduler.course_section.length,$scheduler.course_section.weekday_condition,$scheduler.course_section.period_condition,$scheduler.course_section.week_flag,$scheduler.course_section.long_break,$scheduler.course_section.lock,$scheduler.course_section.ref_classroom_id,$scheduler.classroom.name as classroomname,course.subject,course.subj_level,ref_class_id,$scheduler.course_extension.allow_duplicate,$scheduler.course_extension.ref_timetable_id,$scheduler.timetable.name as timetablename,teacher.teacher_name,teacher.nickname from $scheduler.course_section inner join course on  course.id =$scheduler.course_section.ref_course_id inner join $scheduler.course_extension on $scheduler.course_extension.ref_course_id=course.id inner join $scheduler.timetable on $scheduler.timetable.uid=$scheduler.course_extension.ref_timetable_id inner join tc_instruct on course.id = tc_instruct.ref_course_id inner join teacher on teacher.id=tc_instruct.ref_teacher_id left outer join $scheduler.classroom on $scheduler.classroom.uid=$scheduler.course_section.ref_classroom_id where tc_instruct.sequence=1 and course.subject!=''  and course.school_year={0} and course.semester={1} order by course_name,school_year,semester";

        /// <summary>
        /// 根據學年度及學期下載課程分段（星期及節次不為0，供調代課使用）
        /// </summary>
        //public const string ScheduledCourseSectionTemplateSQL = "select $scheduler.course_section.uid,$scheduler.course_section.ref_course_id,course.course_name,$scheduler.course_extension.subject_alias_name,$scheduler.course_section.weekday,$scheduler.course_section.period,$scheduler.course_section.length,$scheduler.course_section.weekday_condition,$scheduler.course_section.period_condition,$scheduler.course_section.week_flag,$scheduler.course_section.long_break,$scheduler.course_section.lock,$scheduler.course_section.ref_classroom_id,$scheduler.classroom.name as classroomname,course.subject,course.subj_level,ref_class_id,$scheduler.course_extension.allow_duplicate,$scheduler.course_extension.ref_timetable_id,$scheduler.timetable.name as timetablename,teacher.teacher_name,teacher.nickname from $scheduler.course_section inner join course on  course.id =$scheduler.course_section.ref_course_id inner join $scheduler.course_extension on $scheduler.course_extension.ref_course_id=course.id inner join $scheduler.timetable on $scheduler.timetable.uid=$scheduler.course_extension.ref_timetable_id inner join tc_instruct on course.id = tc_instruct.ref_course_id inner join teacher on teacher.id=tc_instruct.ref_teacher_id left outer join $scheduler.classroom on $scheduler.classroom.uid=$scheduler.course_section.ref_classroom_id where tc_instruct.sequence=1 and course.subject!=''  and course.school_year={0} and course.semester={1} and $scheduler.course_section.weekday<>0 and $scheduler.course_section.period<>0 order by course_name,school_year,semester";

        
        /// <summary>
        /// 根據日期下載週課程分段
        /// </summary>
        //public const string WeekCourseSectionTemplateSQL = "select $scheduler.week_course_section.uid,$scheduler.week_course_section.ref_course_id,course.course_name,$scheduler.course_extension.subject_alias_name,$scheduler.week_course_section.week,$scheduler.week_course_section.date,$scheduler.week_course_section.weekday,$scheduler.week_course_section.period,$scheduler.week_course_section.length,$scheduler.week_course_section.weekday_condition,$scheduler.week_course_section.period_condition,$scheduler.week_course_section.week_flag,$scheduler.week_course_section.long_break,$scheduler.week_course_section.lock,$scheduler.week_course_section.ref_classroom_id,$scheduler.classroom.name as classroomname,course.subject,course.subj_level,ref_class_id,$scheduler.course_extension.allow_duplicate,$scheduler.course_extension.ref_timetable_id,$scheduler.timetable.name as timetablename,teacher.teacher_name,teacher.nickname from $scheduler.week_course_section inner join course on  course.id =$scheduler.week_course_section.ref_course_id inner join $scheduler.course_extension on $scheduler.course_extension.ref_course_id=course.id inner join $scheduler.timetable on $scheduler.timetable.uid=$scheduler.course_extension.ref_timetable_id inner join tc_instruct on course.id = tc_instruct.ref_course_id inner join teacher on teacher.id=tc_instruct.ref_teacher_id left outer join $scheduler.classroom on $scheduler.classroom.uid=$scheduler.week_course_section.ref_classroom_id where tc_instruct.sequence=1 and course.subject!=''  and $scheduler.week_course_section.date>='{0}' and $scheduler.week_course_section.date<='{1}' order by course_name,$scheduler.week_course_section.date";

        /// <summary>
        /// 根據學年度及學期下載課程分段（99,2）
        /// </summary>
        //public const string CourseSectionTestSQL = "select $scheduler.course_section.uid,$scheduler.course_section.ref_course_id,course.course_name,$scheduler.course_extension.subject_alias_name,$scheduler.course_section.weekday,$scheduler.course_section.period,$scheduler.course_section.length,$scheduler.course_section.weekday_condition,$scheduler.course_section.period_condition,$scheduler.course_section.week_flag,$scheduler.course_section.long_break,$scheduler.course_section.lock,$scheduler.course_section.ref_classroom_id,$scheduler.classroom.name as classroomname,course.subject,course.subj_level,ref_class_id,$scheduler.course_extension.allow_duplicate,$scheduler.course_extension.ref_timetable_id,$scheduler.timetable.name as timetablename,teacher.teacher_name,teacher.nickname from $scheduler.course_section inner join course on  course.id =$scheduler.course_section.ref_course_id inner join $scheduler.course_extension on $scheduler.course_extension.ref_course_id=course.id inner join $scheduler.timetable on $scheduler.timetable.uid=$scheduler.course_extension.ref_timetable_id inner join tc_instruct on course.id = tc_instruct.ref_course_id inner join teacher on teacher.id=tc_instruct.ref_teacher_id left outer join $scheduler.classroom on $scheduler.classroom.uid=$scheduler.course_section.ref_classroom_id where tc_instruct.sequence=1 and course.subject!=''  and course.school_year=99 and course.semester=2 order by course_name,school_year,semester";

        /// <summary>
        /// 下載所有教師（狀態為一般）的SQL指令
        /// </summary>
        //public const string TeacherSQL = "select teacher.id,teacher.teacher_name,teacher.nickname from teacher where status = 1 order by teacher.teacher_name";

        /// <summary>
        /// 下載所有教師（狀態為一般）不排課時段的SQL指令
        /// </summary>
        //public const string TeacherBusySQL = "select $scheduler.teacher_busy.uid,$scheduler.teacher_busy.ref_teacher_id,$scheduler.teacher_busy.weekday,to_char($scheduler.teacher_busy.begin_time,'1900/1/1 HH24:MI') as begin_time,$scheduler.teacher_busy.duration,$scheduler.teacher_busy.ref_location_id,$scheduler.location.name as locationname,teacher.teacher_name,teacher.nickname,$scheduler.teacher_busy.busy_description from $scheduler.teacher_busy inner join teacher on teacher.id=$scheduler.teacher_busy.ref_teacher_id left outer join $scheduler.location  on $scheduler.teacher_busy.ref_location_id=$scheduler.location.uid where teacher.status=1";

        /// <summary>
        /// 下載所有場地SQL指令
        /// </summary>
        //public const string ClassroomSQL = "select $scheduler.classroom.uid,$scheduler.classroom.name,$scheduler.classroom.capacity,$scheduler.classroom.ref_location_id,$scheduler.classroom.location_only,$scheduler.location.name as locationname from $scheduler.classroom left outer join $scheduler.location on $scheduler.location.uid = $scheduler.classroom.ref_location_id order by $scheduler.classroom.name";

        /// <summary>
        /// 下載所有場地不排課時段指令
        /// </summary>
        //public const string ClassroomBusySQL = "select $scheduler.classroom_busy.uid,$scheduler.classroom_busy.ref_classroom_id,$scheduler.classroom.name,$scheduler.classroom_busy.weekday,to_char($scheduler.classroom_busy.begin_time,'1900/1/1 HH24:MI') as begin_time,$scheduler.classroom_busy.duration,$scheduler.classroom_busy.week_flag from $scheduler.classroom_busy left outer join $scheduler.classroom on $scheduler.classroom_busy.ref_classroom_id=$scheduler.classroom.uid";

        /// <summary>
        /// 下載所有時間表SQL指令
        /// </summary>
        //public const string TimeTableSQL = "select $scheduler.timetable.uid,$scheduler.timetable.name from $scheduler.timetable";

        /// <summary>
        /// 下載所有時間表分段SQL指令
        /// </summary>
        //public const string TimeTableSecSQL = "select $scheduler.timetable_section.ref_timetable_id,$scheduler.timetable_section.weekday,$scheduler.timetable_section.period,to_char($scheduler.timetable_section.begin_time,'1900/1/1 HH24:MI') as begin_time,$scheduler.timetable_section.duration,$scheduler.timetable_section.display_period,$scheduler.timetable_section.ref_location_id,$scheduler.timetable.name as timetablename,$scheduler.location.name as locationname from $scheduler.timetable_section inner join $scheduler.timetable on $scheduler.timetable_section.ref_timetable_id = $scheduler.timetable.uid left outer join $scheduler.location on $scheduler.location.uid=$scheduler.timetable_section.ref_location_id order by $scheduler.timetable.name,$scheduler.timetable_section.weekday,$scheduler.timetable_section.period";

        /// <summary>
        /// 下載所有地點SQL指令
        /// </summary>
        //public const string LocationSQL = "select $scheduler.location.uid,$scheduler.location.name from $scheduler.location";
    }
}