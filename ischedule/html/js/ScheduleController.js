var ScheduleController = function () {

    var rowHeaderWidth = 20;
    var colHeaderHeight = 20;
    var colCount = 5;
    var rowCount = 8;
    var targetWidth = 0;
    var targetHeight = 0;
    var schedulerType = 't';                        //功課表類型，有 t: 教師課表, c:班級課表，p: 場地課表

    var dicPeriods = {};                                 //上課時間表的節次清單
    var dicCourseSections = {};                   // 課程分段清單
    var dicBusyPeriods = {};                         // 因不排課時段而不能排課的節次清單
    var dicResourceConflictPeriods = {};   // 資源衝突導致不能排課的節次清單

    var arySelectedPeriodKeys = [];          // 被選取的節次的 key 的清單

    var isChromeDebug = true;      //如果要在 Chrome 中打開 debug，則設為  true

    return {

        /// 計算格子大小
        calculateCellSize: function () {
            targetWidth = ($(window).width() - rowHeaderWidth - 16) / colCount;
            targetHeight = ($(window).height() - colHeaderHeight - 18) / rowCount;
        },

        ///調整大小符合視窗
        resize: function () {
            this.calculateCellSize();
            $('td.columnHeader').width(targetWidth).height(colHeaderHeight);
            $('td.rowHeader').width(rowHeaderWidth).height(targetHeight);
            $('td.dataCell').width(targetWidth).height(targetHeight);
            $('tr.dataRow').height(targetHeight);
        },

        ///設定上課時間表
        setTimeTable: function (weekdays, periods) {
            colCount = weekdays;
            rowCount = periods;

            this.calculateCellSize();
            dicPeriods = {};

            $('#scheduler').html('');

            //create header
            var tr0 = $("<tr></tr>").height(colHeaderHeight).appendTo('#scheduler'); ;
            var td0 = $("<td></td>").width(rowHeaderWidth).height(colHeaderHeight).appendTo(tr0);

            var i = 0;
            for (i = 0; i < weekdays; i++) {
                var tdH = $("<td class='columnHeader' id='" + (i + 1) + "_0'></td>").width(targetWidth).height(colHeaderHeight).appendTo(tr0).click(function () {

                }).html(i + 1);
            }

            var j = 0;
            for (j = 0; j < periods; j++) {
                var tr = $("<tr class='dataRow'></tr>").height(targetHeight).appendTo('#scheduler');
                var tdH = $("<td class='rowHeader'  id='0_" + (j + 1) + "'></td>").width(rowHeaderWidth).height(targetHeight).appendTo(tr).html(j + 1);
                for (i = 0; i < weekdays; i++) {
                    var key = (i + 1) + "_" + (j + 1);
                    var td = $("<td class='dataCell'><div class='cellContainer' id='" + key + "'></div></td>").width(targetWidth).height(targetHeight).appendTo(tr).click(function () {
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
                this.setCourseSections("");
                this.setBusyPeriods("");
                this.refillData();
                this.setSelectedPeriods("['2_1', '2_2']");
            }
        },

        /// 指定課程分段，會根據已排課的星期節次顯現 */
        setCourseSections: function (json) {
            if (isChromeDebug)
                json = '[{"ID":"ffbfdfc6-b457-4a0b-9017-4211f8441678","Text":"f4cd2413-d","WeekDay":"1","Period":"1","IsLocked":false},{"ID":"4e9c8a20-c3bc-4588-a94e-5a8d8d591de7","Text":"a2edf5cd-5","WeekDay":"","Period":"","IsLocked":false},{"ID":"c4a1d8cf-bd0b-49de-9739-203d4fdee2a2","Text":"598f4cf0-d","WeekDay":"1","Period":"3","IsLocked":true},{"ID":"b46b0842-049c-44f1-b33b-dcf549b43ae9","Text":"b4c2274e-8","WeekDay":"1","Period":"4","IsLocked":false},{"ID":"4ff938f7-30b8-4e7d-944a-7d7d966950bf","Text":"884cffd1-b","WeekDay":"1","Period":"6","IsLocked":true},{"ID":"4a241dd2-be5f-4129-9f86-341619caecf9","Text":"58c1a692-3","WeekDay":"1","Period":"7","IsLocked":false},{"ID":"be4370b9-8d87-40ba-9328-5b859f6537ed","Text":"a28fd671-2","WeekDay":"","Period":"","IsLocked":false},{"ID":"b7f9b5f0-9739-4c98-86e3-cc08107f6984","Text":"b007b9f1-3","WeekDay":"","Period":"","IsLocked":false},{"ID":"1f6b3780-674f-45a0-889d-099375354318","Text":"ce40a8fb-6","WeekDay":"2","Period":"2","IsLocked":false},{"ID":"b2de4bb1-2da7-42e6-8b26-3b319fa507a6","Text":"d89b64e1-2","WeekDay":"2","Period":"3","IsLocked":true},{"ID":"9591418f-1f95-4c1b-87c8-6f5406cc0446","Text":"bbbc7cba-9","WeekDay":"2","Period":"5","IsLocked":false},{"ID":"6cf56842-8679-4758-990d-8ed589f03b41","Text":"6271f915-5","WeekDay":"2","Period":"6","IsLocked":true},{"ID":"429f0ebe-3c1d-4414-9206-8a46f80a6295","Text":"1e9de82e-8","WeekDay":"","Period":"","IsLocked":false},{"ID":"da97ad30-435d-4d76-93c4-177539e291ca","Text":"23e9c1fb-4","WeekDay":"2","Period":"8","IsLocked":false},{"ID":"6ca2de36-2d67-4927-94cd-210cb842c5f9","Text":"8c46fd83-c","WeekDay":"3","Period":"1","IsLocked":false},{"ID":"f02d9df1-d0ca-44de-8d53-fd39c1ce9731","Text":"9a20d643-6","WeekDay":"3","Period":"2","IsLocked":false},{"ID":"33f8ee8f-3716-4751-85e3-31b7fb9eb865","Text":"65b3512e-3","WeekDay":"3","Period":"4","IsLocked":false},{"ID":"2c8d7b2b-19dd-4668-b56f-ca7fcd3f3728","Text":"5884bb18-d","WeekDay":"3","Period":"5","IsLocked":false},{"ID":"6e990390-9fb2-4ed1-95ac-5b97a18ba67c","Text":"610afd89-6","WeekDay":"","Period":"","IsLocked":false},{"ID":"d7c10542-a876-4cd4-a69b-011a2c35bf24","Text":"84d751fe-c","WeekDay":"3","Period":"7","IsLocked":false},{"ID":"f2e3b5b8-bcd4-4997-9179-0bad10b86c14","Text":"077afb44-1","WeekDay":"3","Period":"8","IsLocked":false},{"ID":"7721fcc1-307f-4a8c-8e17-5de8310d3083","Text":"862034a5-d","WeekDay":"4","Period":"1","IsLocked":false},{"ID":"76b4c5da-a88c-44b2-95d0-06a88df2572a","Text":"eab1bb32-7","WeekDay":"4","Period":"3","IsLocked":true},{"ID":"2e38afff-48dc-4fd3-b95a-f1cf57d1d8c9","Text":"eb4b8e97-a","WeekDay":"4","Period":"4","IsLocked":false},{"ID":"c0d523a0-6bd7-42bc-87f3-ce13d86937d3","Text":"2fcd4978-8","WeekDay":"","Period":"","IsLocked":false},{"ID":"c1e499f4-5795-4b25-b434-5c51b4ef602f","Text":"cd268a91-7","WeekDay":"4","Period":"6","IsLocked":true},{"ID":"be7e97de-0ac5-417a-86c9-7969f8fec4b7","Text":"394d4e44-0","WeekDay":"4","Period":"7","IsLocked":false},{"ID":"0204bfac-4c94-46da-99ea-6c02e586c898","Text":"e3ab00a7-2","WeekDay":"5","Period":"2","IsLocked":false},{"ID":"96d94eb4-1106-4e24-9c6e-035401938cd3","Text":"91115ea7-9","WeekDay":"5","Period":"3","IsLocked":true},{"ID":"8a88bd8a-dc2c-4f6b-8a04-ceb7c25c3160","Text":"dc6a0036-d","WeekDay":"","Period":"","IsLocked":false},{"ID":"d890d19a-29f9-4f6c-b30c-ef6c7c854498","Text":"10a110c5-3","WeekDay":"5","Period":"5","IsLocked":false},{"ID":"98df875c-7aa0-431f-908c-0b2bc1fe9c97","Text":"abbd532c-0","WeekDay":"5","Period":"6","IsLocked":true},{"ID":"2caf4d17-5752-419e-9b79-fe58584cf955","Text":"ec229fe3-5","WeekDay":"5","Period":"8","IsLocked":false}]';
            var sections = eval("(" + json + ")");
            dicCourseSections = {};
            $(sections).each(function (index, item) {
                if (item.WeekDay && item.Period) {
                    var key = item.WeekDay + "_" + item.Period;
                    dicCourseSections[key] = item;
                }
            });
        },

        /// 設定不排課時段  */
        setBusyPeriods: function (json) {
            if (isChromeDebug)
                json = '[{"ID":"cb9f137c-8355-4a34-8885-ebf7ee29c07d","Message":"不排課時段","WeekDay":"1","Period":"5"},{"ID":"209b87cd-95c3-42ab-b579-9f308a8d1c95","Message":"不排課時段","WeekDay":"2","Period":"4"},{"ID":"2f5f3fc4-d044-488b-a2ec-181f1820aa41","Message":"不排課時段","WeekDay":"3","Period":"3"},{"ID":"f5c4c305-882b-4b01-8cad-4b24507eb8c4","Message":"不排課時段","WeekDay":"4","Period":"2"},{"ID":"ac433e50-14a1-4c9d-b211-8d8855db3543","Message":"不排課時段","WeekDay":"4","Period":"8"},{"ID":"1ccce923-1232-4ea4-837e-e240710e3c1f","Message":"不排課時段","WeekDay":"5","Period":"1"},{"ID":"056c7997-5682-40b4-8c76-dca91ffe36d8","Message":"不排課時段","WeekDay":"5","Period":"7"}]';
            var periods = eval("(" + json + ")");
            dicBusyPeriods = {};
            $(periods).each(function (index, item) {
                var key = item.WeekDay + "_" + item.Period;
                dicBusyPeriods[key] = item;
            });
        },


        ///* 重填資料 */
        refillData: function () {
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
        },

        ///* 設定課表類型 */
        setSchedulerType: function (sType) {
            schedulerType = sType;
        },

        /* 設定要被選取的節次 */
        setSelectedPeriods: function (jsonPeriodKeys) {
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

            //記錄這次設定的值，供下次設定時直接取消選取，就不需對全部取消選取，加快效能用的。
            arySelectedPeriodKeys = keys;
        },

        test: function (data) {
            alert(data);
        }
    };  //end of return
} ();