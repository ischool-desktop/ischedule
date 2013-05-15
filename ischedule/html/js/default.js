/* 
$(document).ready(function () {
$(window).resize(function () {
ScheduleController.resize();
});

ScheduleController.setTimeTable(5, 8);
});
*/


var rowHeaderWidth = 20;
var colHeaderHeight = 20;
var colCount = 5;
var rowCount = 8;
var padding = 5;	//cell padding = 5
var targetWidth = 0;
var targetHeight = 0;
var schedulerType = 't';                        //功課表類型，有 t: 教師課表, c:班級課表，p: 場地課表

var dicPeriods = {};                                 // 實際有效的節次清單
var dicTimeTablePeriods = {};               // 上課時間表的節次清單，由 Client 傳入 json
var dicCourseSections = {};                   // 課程分段清單
var dicBusyPeriods = {};                         // 因不排課時段而不能排課的節次清單
var dicResourceConflictPeriods = {};   // 資源衝突導致不能排課的節次清單

var arySelectedPeriodKeys = [];          // 被選取的節次的 key 的清單

var isChromeDebug = false;                   //如果要使用 Chrome Debug 時請 turn on

$(document).ready(function () {
    $(window).resize(function () {
        calculateCellSize();
        $('td.columnHeader').width(targetWidth).height(colHeaderHeight);
        $('td.rowHeader').width(rowHeaderWidth).height(targetHeight);
        $('td.dataCell').width(targetWidth).height(targetHeight);
        $('tr.dataRow').height(targetHeight);
    });

    setTimeTable(5, 8);
});

var calculateCellSize = function () {
    targetWidth = ($(window).width() - rowHeaderWidth -16) / colCount - padding * 2 + 1;
    targetHeight = (($(window).height() - colHeaderHeight - 18) / rowCount) - padding * 2;
};

var setTimeTable = function (weekdays, periods, json) {
    colCount = weekdays;
    rowCount = periods;

    if (isChromeDebug) {
        json = '[{"WeekDay":1,"PeriodNo":1,"DisplayPeriod":0,"BeginTime":"/Date(-2208987600000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208984600000)/"},{"WeekDay":1,"PeriodNo":2,"DisplayPeriod":0,"BeginTime":"/Date(-2208984000000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208981000000)/"},{"WeekDay":1,"PeriodNo":3,"DisplayPeriod":0,"BeginTime":"/Date(-2208980400000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208977400000)/"},{"WeekDay":1,"PeriodNo":4,"DisplayPeriod":0,"BeginTime":"/Date(-2208976800000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208973800000)/"},{"WeekDay":1,"PeriodNo":5,"DisplayPeriod":0,"BeginTime":"/Date(-2208970800000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208967800000)/"},{"WeekDay":1,"PeriodNo":6,"DisplayPeriod":0,"BeginTime":"/Date(-2208967200000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208964200000)/"},{"WeekDay":1,"PeriodNo":7,"DisplayPeriod":0,"BeginTime":"/Date(-2208963600000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208960600000)/"},{"WeekDay":1,"PeriodNo":8,"DisplayPeriod":0,"BeginTime":"/Date(-2208960000000)/","Duration":45,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208957300000)/"},{"WeekDay":1,"PeriodNo":9,"DisplayPeriod":0,"BeginTime":"/Date(-2208957000000)/","Duration":45,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208954300000)/"},{"WeekDay":1,"PeriodNo":10,"DisplayPeriod":0,"BeginTime":"/Date(-2208954000000)/","Duration":40,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208951600000)/"},{"WeekDay":2,"PeriodNo":1,"DisplayPeriod":0,"BeginTime":"/Date(-2208987600000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208984600000)/"},{"WeekDay":2,"PeriodNo":2,"DisplayPeriod":0,"BeginTime":"/Date(-2208984000000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208981000000)/"},{"WeekDay":2,"PeriodNo":3,"DisplayPeriod":0,"BeginTime":"/Date(-2208980400000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208977400000)/"},{"WeekDay":2,"PeriodNo":4,"DisplayPeriod":0,"BeginTime":"/Date(-2208976800000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208973800000)/"},{"WeekDay":2,"PeriodNo":5,"DisplayPeriod":0,"BeginTime":"/Date(-2208970800000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208967800000)/"},{"WeekDay":2,"PeriodNo":6,"DisplayPeriod":0,"BeginTime":"/Date(-2208967200000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208964200000)/"},{"WeekDay":2,"PeriodNo":7,"DisplayPeriod":0,"BeginTime":"/Date(-2208963600000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208960600000)/"},{"WeekDay":2,"PeriodNo":8,"DisplayPeriod":0,"BeginTime":"/Date(-2208960000000)/","Duration":45,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208957300000)/"},{"WeekDay":2,"PeriodNo":9,"DisplayPeriod":0,"BeginTime":"/Date(-2208957000000)/","Duration":45,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208954300000)/"},{"WeekDay":2,"PeriodNo":10,"DisplayPeriod":0,"BeginTime":"/Date(-2208954000000)/","Duration":40,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208951600000)/"},{"WeekDay":3,"PeriodNo":1,"DisplayPeriod":0,"BeginTime":"/Date(-2208987600000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208984600000)/"},{"WeekDay":3,"PeriodNo":2,"DisplayPeriod":0,"BeginTime":"/Date(-2208984000000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208981000000)/"},{"WeekDay":3,"PeriodNo":3,"DisplayPeriod":0,"BeginTime":"/Date(-2208980400000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208977400000)/"},{"WeekDay":3,"PeriodNo":4,"DisplayPeriod":0,"BeginTime":"/Date(-2208976800000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208973800000)/"},{"WeekDay":3,"PeriodNo":5,"DisplayPeriod":0,"BeginTime":"/Date(-2208970800000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208967800000)/"},{"WeekDay":3,"PeriodNo":6,"DisplayPeriod":0,"BeginTime":"/Date(-2208967200000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208964200000)/"},{"WeekDay":3,"PeriodNo":7,"DisplayPeriod":0,"BeginTime":"/Date(-2208963600000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208960600000)/"},{"WeekDay":4,"PeriodNo":1,"DisplayPeriod":0,"BeginTime":"/Date(-2208987600000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208984600000)/"},{"WeekDay":4,"PeriodNo":2,"DisplayPeriod":0,"BeginTime":"/Date(-2208984000000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208981000000)/"},{"WeekDay":4,"PeriodNo":3,"DisplayPeriod":0,"BeginTime":"/Date(-2208980400000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208977400000)/"},{"WeekDay":4,"PeriodNo":4,"DisplayPeriod":0,"BeginTime":"/Date(-2208976800000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208973800000)/"},{"WeekDay":4,"PeriodNo":5,"DisplayPeriod":0,"BeginTime":"/Date(-2208970800000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208967800000)/"},{"WeekDay":4,"PeriodNo":6,"DisplayPeriod":0,"BeginTime":"/Date(-2208967200000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208964200000)/"},{"WeekDay":4,"PeriodNo":7,"DisplayPeriod":0,"BeginTime":"/Date(-2208963600000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208960600000)/"},{"WeekDay":4,"PeriodNo":8,"DisplayPeriod":0,"BeginTime":"/Date(-2208960000000)/","Duration":45,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208957300000)/"},{"WeekDay":4,"PeriodNo":9,"DisplayPeriod":0,"BeginTime":"/Date(-2208957000000)/","Duration":45,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208954300000)/"},{"WeekDay":4,"PeriodNo":10,"DisplayPeriod":0,"BeginTime":"/Date(-2208954000000)/","Duration":40,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208951600000)/"},{"WeekDay":5,"PeriodNo":1,"DisplayPeriod":0,"BeginTime":"/Date(-2208987600000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208984600000)/"},{"WeekDay":5,"PeriodNo":2,"DisplayPeriod":0,"BeginTime":"/Date(-2208984000000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208981000000)/"},{"WeekDay":5,"PeriodNo":3,"DisplayPeriod":0,"BeginTime":"/Date(-2208980400000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208977400000)/"},{"WeekDay":5,"PeriodNo":4,"DisplayPeriod":0,"BeginTime":"/Date(-2208976800000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208973800000)/"},{"WeekDay":5,"PeriodNo":5,"DisplayPeriod":0,"BeginTime":"/Date(-2208970800000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208967800000)/"},{"WeekDay":5,"PeriodNo":6,"DisplayPeriod":0,"BeginTime":"/Date(-2208967200000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208964200000)/"},{"WeekDay":5,"PeriodNo":7,"DisplayPeriod":0,"BeginTime":"/Date(-2208963600000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208960600000)/"},{"WeekDay":5,"PeriodNo":8,"DisplayPeriod":0,"BeginTime":"/Date(-2208960000000)/","Duration":45,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208957300000)/"},{"WeekDay":5,"PeriodNo":9,"DisplayPeriod":0,"BeginTime":"/Date(-2208957000000)/","Duration":45,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208954300000)/"},{"WeekDay":5,"PeriodNo":10,"DisplayPeriod":0,"BeginTime":"/Date(-2208954000000)/","Duration":40,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208951600000)/"},{"WeekDay":6,"PeriodNo":1,"DisplayPeriod":0,"BeginTime":"/Date(-2208987600000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208984600000)/"},{"WeekDay":6,"PeriodNo":2,"DisplayPeriod":0,"BeginTime":"/Date(-2208984000000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208981000000)/"},{"WeekDay":6,"PeriodNo":3,"DisplayPeriod":0,"BeginTime":"/Date(-2208980400000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208977400000)/"},{"WeekDay":6,"PeriodNo":4,"DisplayPeriod":0,"BeginTime":"/Date(-2208976800000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208973800000)/"}]';
        colCount = 6;
        rowCount = 10;
    }
    calculateCellSize();
    dicPeriods = {};
    dicTimeTablePeriods = {};
    var timeTablePeriods = eval("(" + json + ")");
    $(timeTablePeriods).each(function (index, item) {
        var key = item.WeekDay + "_" + item.PeriodNo;
        //log(key);
        dicTimeTablePeriods[key] = item;
    });
    $('#scheduler').html('');

    //create header
    var tr0 = $("<tr></tr>").height(colHeaderHeight).appendTo('#scheduler'); ;
    var td0 = $("<th></th>").width(rowHeaderWidth).height(colHeaderHeight).appendTo(tr0);

    var i = 0;
    for (i = 0; i < colCount; i++) {
        var tdH = $("<th class='columnHeader' id='" + (i + 1) + "_0'></th>").width(targetWidth).height(colHeaderHeight).appendTo(tr0).click(function () {

        }).html(i + 1);
    }

    var j = 0;
    for (j = 0; j < rowCount; j++) {
        var tr = $("<tr class='dataRow'></tr>").height(targetHeight).appendTo('#scheduler');
        var tdH = $("<th class='rowHeader'  id='0_" + (j + 1) + "'></th>").width(rowHeaderWidth).height(targetHeight).appendTo(tr).html(j + 1);
        for (i = 0; i < colCount; i++) {
            var key = (i + 1) + "_" + (j + 1);
            var tdHTML = (dicTimeTablePeriods[key]) ? "<td class='dataCell'>" : "<td class='invalid'>";
            var td = $(tdHTML + "<div class='cellContainer' id='" + key + "'></div></td>").width(targetWidth).height(targetHeight).appendTo(tr).click(function () {
                //$('td.dataCell').removeClass('selected');
                //$(this).addClass('selected');
            }).mouseover(function () {
                $(this).addClass('mi');
            }).mouseout(function () {
                $(this).removeClass('mi');
            });

            dicPeriods[key] = PeriodController($('#' + key), schedulerType, (i + 1), (j + 1));   //store the period controller in a dictionary.

        }
    }

    if (isChromeDebug) {
        setBusyPeriods("");
        setCourseSections("");
        refillData();
        setSelectedPeriods("['2_1', '2_2']");
    }
};

/// 指定課程分段，會根據已排課的星期節次顯現 /
var setCourseSections = function (json) {
    if (isChromeDebug)
        json = '[{"EventID":"chhs.hcc.edu.tw,1812865","TeacherID1":"簡煥卿","TeacherID2":"","TeacherID3":"","ClassroomID":"","SubjectID":"公民與社會IV","SubjectAlias":"","CourseName":"LIP五忠 公民與社會Ⅳ","Comment":"","ClassID":"chhs.hcc.edu.tw,1514","PeriodNo":3,"WeekDay":1,"WeekFlag":3,"WeekDayOp":0,"WeekDayVar":"","PeriodOp":0,"PeriodVar":"","AllowLongBreak":false,"AllowDuplicate":false,"ManualLock":false,"CourseID":"chhs.hcc.edu.tw,19783","TimeTableID":"chhs.hcc.edu.tw,107192","Priority":1,"SolutionCount":-1,"Length":2,"Week":null,"Date":null,"Message":null,"LimitNextDay":false,"CourseGroup":"","WeekDayCondition":"","PeriodCondition":"","DisplayManualLock":"","DisplaySolutionCount":"-","DisplayTeacherName":"簡煥卿","DisplayClassName":"LIP五忠","DisplaySubjectName":"公民與社會IV","DisplayClassroomName":"無","DisplayAllowLongBreak":"否","DisplayAllowDuplicate":"否","DisplayLimitNextDay":"否","DisplayWeekFlag":"單雙","DispalyTimeTableName":"日校加強實驗班(含週六)","ColorIndex":0},{"EventID":"j.chhs.hcc.edu.tw,341540","TeacherID1":"簡煥卿","TeacherID2":"","TeacherID3":"","ClassroomID":"","SubjectID":"公民VI","SubjectAlias":"","CourseName":"全三忠 公民Ⅵ","Comment":"","ClassID":"j.chhs.hcc.edu.tw,5582","PeriodNo":6,"WeekDay":4,"WeekFlag":3,"WeekDayOp":0,"WeekDayVar":"","PeriodOp":0,"PeriodVar":"","AllowLongBreak":false,"AllowDuplicate":false,"ManualLock":false,"CourseID":"j.chhs.hcc.edu.tw,7213","TimeTableID":"j.chhs.hcc.edu.tw,11962","Priority":1,"SolutionCount":-1,"Length":2,"Week":null,"Date":null,"Message":null,"LimitNextDay":false,"CourseGroup":"","WeekDayCondition":"","PeriodCondition":"","DisplayManualLock":"","DisplaySolutionCount":"-","DisplayTeacherName":"簡煥卿","DisplayClassName":"全三忠","DisplaySubjectName":"公民VI","DisplayClassroomName":"無","DisplayAllowLongBreak":"否","DisplayAllowDuplicate":"否","DisplayLimitNextDay":"否","DisplayWeekFlag":"單雙","DispalyTimeTableName":"日校加強實驗班(含週六)","ColorIndex":0}]';
        var sections = eval("(" + json + ")");
        dicCourseSections = {};
        $(sections).each(function (index, item) {
            if (item.WeekDay && item.PeriodNo) {
                var key = item.WeekDay + "_" + item.PeriodNo;
                dicCourseSections[key] = item;
            }
        });
};

///* 設定不排課時段  /
var setBusyPeriods = function (json) {
    if (isChromeDebug)
        json = '[{"WeekDay":1,"PeriodNo":1,"DisplayPeriod":0,"BeginTime":"/Date(-2208987600000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208984600000)/"},{"WeekDay":1,"PeriodNo":2,"DisplayPeriod":0,"BeginTime":"/Date(-2208984000000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208981000000)/"},{"WeekDay":1,"PeriodNo":3,"DisplayPeriod":0,"BeginTime":"/Date(-2208980400000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208977400000)/"},{"WeekDay":1,"PeriodNo":4,"DisplayPeriod":0,"BeginTime":"/Date(-2208976800000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208973800000)/"},{"WeekDay":1,"PeriodNo":5,"DisplayPeriod":0,"BeginTime":"/Date(-2208970800000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208967800000)/"},{"WeekDay":1,"PeriodNo":6,"DisplayPeriod":0,"BeginTime":"/Date(-2208967200000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208964200000)/"},{"WeekDay":1,"PeriodNo":7,"DisplayPeriod":0,"BeginTime":"/Date(-2208963600000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208960600000)/"},{"WeekDay":1,"PeriodNo":8,"DisplayPeriod":0,"BeginTime":"/Date(-2208960000000)/","Duration":45,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208957300000)/"},{"WeekDay":1,"PeriodNo":9,"DisplayPeriod":0,"BeginTime":"/Date(-2208957000000)/","Duration":45,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208954300000)/"},{"WeekDay":1,"PeriodNo":10,"DisplayPeriod":0,"BeginTime":"/Date(-2208954000000)/","Duration":40,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208951600000)/"},{"WeekDay":3,"PeriodNo":1,"DisplayPeriod":0,"BeginTime":"/Date(-2208987600000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208984600000)/"},{"WeekDay":3,"PeriodNo":2,"DisplayPeriod":0,"BeginTime":"/Date(-2208984000000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208981000000)/"},{"WeekDay":3,"PeriodNo":3,"DisplayPeriod":0,"BeginTime":"/Date(-2208980400000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208977400000)/"},{"WeekDay":3,"PeriodNo":4,"DisplayPeriod":0,"BeginTime":"/Date(-2208976800000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208973800000)/"},{"WeekDay":3,"PeriodNo":5,"DisplayPeriod":0,"BeginTime":"/Date(-2208970800000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208967800000)/"},{"WeekDay":3,"PeriodNo":6,"DisplayPeriod":0,"BeginTime":"/Date(-2208967200000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208964200000)/"},{"WeekDay":3,"PeriodNo":7,"DisplayPeriod":0,"BeginTime":"/Date(-2208963600000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208960600000)/"},{"WeekDay":4,"PeriodNo":1,"DisplayPeriod":0,"BeginTime":"/Date(-2208987600000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208984600000)/"},{"WeekDay":4,"PeriodNo":2,"DisplayPeriod":0,"BeginTime":"/Date(-2208984000000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208981000000)/"},{"WeekDay":4,"PeriodNo":3,"DisplayPeriod":0,"BeginTime":"/Date(-2208980400000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208977400000)/"},{"WeekDay":4,"PeriodNo":4,"DisplayPeriod":0,"BeginTime":"/Date(-2208976800000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208973800000)/"},{"WeekDay":4,"PeriodNo":5,"DisplayPeriod":0,"BeginTime":"/Date(-2208970800000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208967800000)/"},{"WeekDay":4,"PeriodNo":6,"DisplayPeriod":0,"BeginTime":"/Date(-2208967200000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208964200000)/"},{"WeekDay":4,"PeriodNo":7,"DisplayPeriod":0,"BeginTime":"/Date(-2208963600000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208960600000)/"},{"WeekDay":4,"PeriodNo":8,"DisplayPeriod":0,"BeginTime":"/Date(-2208960000000)/","Duration":45,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208957300000)/"},{"WeekDay":4,"PeriodNo":9,"DisplayPeriod":0,"BeginTime":"/Date(-2208957000000)/","Duration":45,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208954300000)/"},{"WeekDay":4,"PeriodNo":10,"DisplayPeriod":0,"BeginTime":"/Date(-2208954000000)/","Duration":40,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208951600000)/"},{"WeekDay":5,"PeriodNo":1,"DisplayPeriod":0,"BeginTime":"/Date(-2208987600000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208984600000)/"},{"WeekDay":5,"PeriodNo":2,"DisplayPeriod":0,"BeginTime":"/Date(-2208984000000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208981000000)/"},{"WeekDay":5,"PeriodNo":3,"DisplayPeriod":0,"BeginTime":"/Date(-2208980400000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208977400000)/"},{"WeekDay":5,"PeriodNo":4,"DisplayPeriod":0,"BeginTime":"/Date(-2208976800000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208973800000)/"},{"WeekDay":5,"PeriodNo":5,"DisplayPeriod":0,"BeginTime":"/Date(-2208970800000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208967800000)/"},{"WeekDay":5,"PeriodNo":6,"DisplayPeriod":0,"BeginTime":"/Date(-2208967200000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208964200000)/"},{"WeekDay":5,"PeriodNo":7,"DisplayPeriod":0,"BeginTime":"/Date(-2208963600000)/","Duration":50,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208960600000)/"},{"WeekDay":5,"PeriodNo":8,"DisplayPeriod":0,"BeginTime":"/Date(-2208960000000)/","Duration":45,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208957300000)/"},{"WeekDay":5,"PeriodNo":9,"DisplayPeriod":0,"BeginTime":"/Date(-2208957000000)/","Duration":45,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208954300000)/"},{"WeekDay":5,"PeriodNo":10,"DisplayPeriod":0,"BeginTime":"/Date(-2208954000000)/","Duration":40,"Disable":false,"DisableMessage":"","LocID":"","EndTime":"/Date(-2208951600000)/"}]"';
    var periods = eval("(" + json + ")");
    dicBusyPeriods = {};
    $(periods).each(function (index, item) {
        var key = item.WeekDay + "_" + item.PeriodNo;
        dicBusyPeriods[key] = item;
    });
};


///* 重填資料 /
var refillData = function () {
    var i = 0;
    var j = 0;
    for (i = 0; i < colCount; i++) {
        for (j = 0; j < rowCount; j++) {
            var key = (i + 1) + "_" + (j + 1);
            var ctrl = dicPeriods[key];
            //1. check Course Section
            if (dicCourseSections[key])
                ctrl.setCourseSection(dicCourseSections[key]);
            else if (dicBusyPeriods[key])
                ctrl.setBusyPeriod(dicBusyPeriods[key]);
            else if (dicResourceConflictPeriods[key])
                ctrl.setResourceConflictPeriod(dicResourceConflictPeriods[key]);
            else
                ctrl.setCourseSection(undefined);
        }
    }
}

///* 設定課表類型 /
var setSchedulerType = function (sType) {
    schedulerType = sType;
};

///* 設定要被選取的節次 /
var setSelectedPeriods = function (jsonPeriodKeys) {
    //alert(jsonPeriodKeys);
    var keys = eval("(" + jsonPeriodKeys + ")");
    //alert(keys.length);
    $(arySelectedPeriodKeys).each(function (index, key) {
        var ctrl = dicPeriods[key];
        if (ctrl)
            ctrl.setSelected(false);
    });
    $(keys).each(function (index, key) {
        var ctrl = dicPeriods[key];
        //alert(ctrl.getWeekday() + "," + ctrl.getPeriodIndex());
        if (ctrl)
            ctrl.setSelected(true);
    });

    //記錄這次設定的值，供下次設定時直接取消選取，就不用對全部取消選取，加快效能用的。
    arySelectedPeriodKeys = keys;
};

var test= function (data) {
    alert(data);
};

var log = function (msg) {
    $('#msg').html($('#msg').html() + msg + "<br/>");
};

/* */