
var PeriodController = function (divPeriod, periodType, weekday, periodIndex) {
    var _divRoot = divPeriod;
    var _cellKey = _divRoot.attr('id');
    var _courseSection;
    var _periodState;    //節次狀態，有: busy:不排課、finish:已排課、lock:已鎖定、conflict:其他資源衝突、empty:空白。
    var _periodType;        //節次類型， 't' or  'c' or 'p'，可能是教師課表、班級課表或場地課表的節次
    var _weekday = weekday;             //星期
    var _periodIndex = periodIndex;       //節次
    var _busyMsg;    //資源衝突的原因
    var _icon;
    var _canSelected = false;

    var lbl_1;
    var lbl_2;
    var lbl_3;

    var addLabels = function () {
        lbl_1 = $("<div class='cellLabel' id='" + _cellKey + "_lbl1'></div>").appendTo(_divRoot);
        lbl_2 = $("<div class='cellLabel' id='" + _cellKey + "_lbl2'></div>").appendTo(_divRoot).hide();
        lbl_3 = $("<div class='cellLabel' id='" + _cellKey + "_lbl3'></div>").appendTo(_divRoot).hide();
    };

    var addIcon = function () {
        _icon = $("<img class='icon locked' src='img/lock_3.png' />").appendTo(_divRoot).hide();
    };

    addLabels();
    addIcon();

    var hideIcon = function () {
        if (_icon)
            _icon.hide();
    };

    var clearEvents = function () {
        _divRoot.unbind('click');  //clear content
        lbl_1.unbind('click').unbind('dblClick').unbind('mouseover').unbind('mouseout');
    };


    //設定鎖定的 icon，並註冊解除鎖定事件
    var setLockedIcon = function () {
        _icon.attr('src', 'img/lock_3.png').show();
    };


    //設定移除排課結果的 icon，並註冊移除排課結果事件
    var setRemoveIcon = function () {
        _icon.attr('src', 'img/delete.png');
    };

    //設定不排課時段的 icon
    var setBusyIcon = function () {
        _icon.attr('src', 'img/busy.png').show();
    };

    return {
        setCourseSection: function (courseSection) {
            _courseSection = courseSection;
            //_canSelected = true;
            this.canSelected(true);
            hideIcon();
            _divRoot.unbind('click');  //clear content
            if (!_courseSection) {
                _periodState = 'empty';
                return;
            }
            //加入第一個 label 
            lbl_1.mouseover(function () {
                $(this).addClass('cellLabel_mi');
            }).mouseout(function () {
                $(this).removeClass('cellLabel_mi');
            }).html(_courseSection.Text);

            if (!_courseSection.IsLocked) {
                //加入移除 icon
                setRemoveIcon();
                _periodState = 'finish';

                //註冊 Period.click 事件
                _divRoot.click(function () {
                    window.external.CourseSectionClicked(_courseSection.ID, _weekday, _periodIndex);
                });

                //註冊 lable click 事件
                lbl_1.dblclick(function () {
                    if (window.getSelection)
                        window.getSelection().removeAllRanges();
                    else if (document.selection)
                        document.selection.empty();
                    window.external.LabelClicked('c', $(this).attr('id'));

                });
            }
            else {
                //_canSelected = false;
                this.canSelected(false);
                _periodState = 'lock';
                setLockedIcon();
            }
        },

        //
        setBusyPeriod: function (busyPeriod) {

            if (!busyPeriod) {
                return;
            }
            _periodState = 'busy';
            hideIcon();
            clearEvents();
            this.canSelected(false);
            //_canSelected = false;
            //加入第一個 label 
            lbl_1.html(busyPeriod.Message);

            //Set Busy Icon
            setBusyIcon();
        },

        setResourceConflictPeriod: function (conflictPeriod) {
            _canSelected = false;
        },

        //設定是否被選取
        setSelected: function (isSelected) {
            if (!_canSelected)
                return;

            if (isSelected) {
                _divRoot.parent().addClass('selected');
                if (_periodState == 'finish')
                     _icon.show();
            }
            else {
                _divRoot.parent().removeClass('selected');
                _icon.hide();
            }
        },

        getWeekday: function () {
            return _weekday;
        },

        getPeriodIndex: function () {
            return _periodIndex;
        },

        canSelected: function (can_selected) {
            _canSelected = can_selected;
            if (!_canSelected)
                _divRoot.parent().addClass('unselectable');
            else
                _divRoot.parent().removeClass('unselectable');
        }

    }
};