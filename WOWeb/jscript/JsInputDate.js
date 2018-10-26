var __JSCALENDAR_LANGUAGE_MONTHS__ = new Array("一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月");
var __JSCALENDAR_LANGUAGE_WEEKS__ = new Array("日", "一", "二", "三", "四", "五", "六");
var __JSCALENDAR_LANGUAGE_TEXTS__ = new Array("今天", "周", "年", "月");
var __JSCALENDAR_LANGUAGE_FORMAT__ = "%Y-%m-%d";

function JsInputDate(classid) {
    var htmlRef = document.getElementById(classid);
    htmlRef._normalClass = "JsInputText_normal";
    htmlRef._focusClass = "JsInputText_focus";

    htmlRef.onFocus = null;
    htmlRef.onBlur = null;
    htmlRef.onChange = null;
    htmlRef.onClick = null;

    htmlRef.onReadly = null;
    htmlRef.onBegin = null;

    htmlRef.getNormalClass = function () { return htmlRef._normalClass; }
    htmlRef.setNormalClass = function (normalClass) { htmlRef._normalClass = normalClass; htmlRef.className = normalClass; }
    htmlRef.getFocusClass = function () { return htmlRef._focusClass; }
    htmlRef.setFocusClass = function (focusClass) { htmlRef._focusClass = focusClass; }
    htmlRef.getHeight = function () { return htmlRef.style.height; }
    htmlRef.setHeight = function (height) { htmlRef.style.height = height; }
    htmlRef.getWidth = function () { return htmlRef.style.width; }
    htmlRef.setWidth = function (width) { htmlRef.style.width = width; }
    htmlRef.getDisabled = function () { return htmlRef.disabled; }
    htmlRef.setDisabled = function (disabled) { htmlRef.disabled = disabled; }
    htmlRef.getValue = function () { return htmlRef.value; }
    htmlRef.setValue = function (value) { htmlRef.value = value; }


    $("#" + classid).attr("readonly", "readonly");
    $("#" + classid).css("backgroundImage", "url(http://mall.lechupei.com/images/JsInputDate_icon.bmp)");
    $("#" + classid).css("backgroundRepeat", "no-repeat");
    $("#" + classid).css("backgroundPosition", "right center");
    $("#" + classid).css("cursor", "pointer");
    $("#" + classid).css("paddingRight", "20px");
    $("#" + classid).css("className", htmlRef._normalClass);
    
    $("#" + classid).unbind().bind("focus", function () { if (htmlRef.onBegin != null) htmlRef.onBegin(htmlRef); if (htmlRef.onFocus != null) htmlRef.onFocus(htmlRef); htmlRef.className = htmlRef._focusClass; });
    $("#" + classid).unbind().bind('blur', function () { htmlRef.className = htmlRef._normalClass; });
    $("#" + classid).unbind().bind("change", function () { if (htmlRef.onChange != null) htmlRef.onChange(htmlRef); });

    var params = { "inputField": htmlRef, "button": htmlRef };

    function onSelect(cal) {
        var p = cal.params;
        var update = (cal.dateClicked || p.electric);

        if (update && p.inputField) {
            p.inputField.value = cal.date.print(cal.dateFormat);
            if (typeof p.inputField.onchange == "function") p.inputField.onchange();
        }

        if (update && typeof p.onUpdate == "function") p.onUpdate(cal);

        if (update && cal.dateClicked) cal.callCloseHandler();
    };

    function onClose(cal) {
        cal.hide();
    };

    params.button["onclick"] = function () {
        if (htmlRef.style.backgroundColor != "")
            return;

        if (htmlRef.onClick != null) htmlRef.onClick(htmlRef);
        htmlRef.focus();
        var dateEl = params.inputField;
        if (window.calendar == null) {
            window.calendar = new JsCalendar(onSelect, onClose);
            window.calendar.params = params;
            window.calendar.create();
        }
        else {
            window.calendar.hide();
            window.calendar.params = params;
        }

        var dateValue = dateEl.value;
        if (dateValue != "") {
            var parsedDate = Date.parseDate(dateValue, window.calendar.dateFormat);
            if (parsedDate != null) window.calendar.setDate(parsedDate);
        }
        else
            window.calendar.setDate(new Date());

        window.calendar.showAtElement(params.button);
        return false;
    };

    return htmlRef;
};

function JsCalendar(onSelected, onClose) {
    this.firstDayOfWeek = 0;
    this.showsOtherMonths = true;
    this.minYear = 1900;
    this.maxYear = 2050;
    this.minMonth = 0;
    this.maxMonth = 11;
    this.weekNumbers = false;
    this.noGrab = false;
    this.dragging = false;
    this.hidden = false;
    this.dateFormat = __JSCALENDAR_LANGUAGE_FORMAT__;
    this.isPopup = true;
    this.timeout = null;
    this.activeDiv = null;
    this.currentDateEl = null;
    this.getDateStatus = null;
    this.ar_days = null;
    this.dateStr = null;
    this.multiple = null;
    this.dateClicked = false;
    this.table = null;
    this.element = null;
    this.tbody = null;
    this.firstdayname = null;
    this.monthsCombo = null;
    this.hilitedMonth = null;
    this.activeMonth = null;
    this.yearsCombo = null;
    this.hilitedYear = null;
    this.activeYear = null;
    this.histCombo = null;
    this.hilitedHist = null;

    this.onSelected = onSelected || null;
    this.onClose = onClose || null;
};

JsCalendar.__wch_id = 0;
JsCalendar.is_opera = /opera/i.test(navigator.userAgent);
JsCalendar.is_ie = (/msie/i.test(navigator.userAgent) && !JsCalendar.is_opera);
JsCalendar.is_ie5 = (JsCalendar.is_ie && /msie 5\.0/i.test(navigator.userAgent));
JsCalendar.is_mac_ie = (/msie.*mac/i.test(navigator.userAgent) && !JsCalendar.is_opera);
JsCalendar.is_khtml = /Konqueror|Safari|KHTML/i.test(navigator.userAgent);
JsCalendar.is_konqueror = /Konqueror/i.test(navigator.userAgent);
JsCalendar.is_gecko = /Gecko/i.test(navigator.userAgent);
JsCalendar._C = null;

JsCalendar._add_evs = function (el) {
    var C = JsCalendar;
    JsCalendar_addEvent(el, "mouseover", C.dayMouseOver);
    JsCalendar_addEvent(el, "mousedown", C.dayMouseDown);
    JsCalendar_addEvent(el, "mouseout", C.dayMouseOut);
    if (JsCalendar.is_ie) JsCalendar_addEvent(el, "dblclick", C.dayMouseDblClick);
};

JsCalendar._del_evs = function (el) {
    var C = this;
    JsCalendar_removeEvent(el, "mouseover", C.dayMouseOver);
    JsCalendar_removeEvent(el, "mousedown", C.dayMouseDown);
    JsCalendar_removeEvent(el, "mouseout", C.dayMouseOut);
    if (JsCalendar.is_ie) JsCalendar_removeEvent(el, "dblclick", C.dayMouseDblClick);
};

JsCalendar.findMonth = function (el) {
    if (typeof el.month != "undefined") return el;
    else if (el.parentNode && typeof el.parentNode.month != "undefined") return el.parentNode;

    return null;
};

JsCalendar.findHist = function (el) {
    if (typeof el.histDate != "undefined") return el;
    else if (el.parentNode && typeof el.parentNode.histDate != "undefined") return el.parentNode;

    return null;
};

JsCalendar.findYear = function (el) {
    if (typeof el.year != "undefined") return el;
    else if (el.parentNode && typeof el.parentNode.year != "undefined") return el.parentNode;

    return null;
};

JsCalendar.showMonthsCombo = function () {
    var cal = JsCalendar._C;
    if (!cal) return false;

    var cd = cal.activeDiv;
    var mc = cal.monthsCombo;
    var date = cal.date,
		MM = cal.date.getMonth(),
		YY = cal.date.getFullYear(),
		min = (YY == cal.minYear),
		max = (YY == cal.maxYear);

    for (var i = mc.firstChild; i; i = i.nextSibling) {
        var m = i.month;
        JsCalendar_removeClass(i, "hilite");
        JsCalendar_removeClass(i, "active");
        JsCalendar_removeClass(i, "disabled");
        i.disabled = false;

        if ((min && m < cal.minMonth) || (max && m > cal.maxMonth)) {
            JsCalendar_addClass(i, "disabled");
            i.disabled = true;
        }

        if (m == MM) JsCalendar_addClass(cal.activeMonth = i, "active");
    }

    var s = mc.style;
    s.display = "block";

    if (cd.navtype < 0) s.left = cd.offsetLeft + "px";
    else {
        var mcw = mc.offsetWidth;
        if (typeof mcw == "undefined") mcw = 50;
        s.left = (cd.offsetLeft + cd.offsetWidth - mcw) + "px";
    }

    s.top = (cd.offsetTop + cd.offsetHeight) + "px";
    cal.updateWCH(mc);
};

JsCalendar.showYearsCombo = function (fwd) {
    var cal = JsCalendar._C;
    if (!cal) return false;

    var cd = cal.activeDiv;
    var yc = cal.yearsCombo;
    if (cal.hilitedYear) JsCalendar_removeClass(cal.hilitedYear, "hilite");
    if (cal.activeYear) JsCalendar_removeClass(cal.activeYear, "active");

    cal.activeYear = null;
    var Y = cal.date.getFullYear() + (fwd ? 1 : -1);
    var yr = yc.firstChild;
    var show = false;

    for (var i = 12; i > 0; --i) {
        if (Y >= cal.minYear && Y <= cal.maxYear) {
            yr.firstChild.data = Y;
            yr.year = Y;
            yr.style.display = "block";
            show = true;
        }
        else yr.style.display = "none";

        yr = yr.nextSibling;
        Y += fwd ? 1 : -1;
    }

    if (show) {
        var s = yc.style;
        s.display = "block";

        if (cd.navtype < 0) s.left = cd.offsetLeft + "px";
        else {
            var ycw = yc.offsetWidth;
            if (typeof ycw == "undefined") ycw = 50;
            s.left = (cd.offsetLeft + cd.offsetWidth - ycw) + "px";
        }

        s.top = (cd.offsetTop + cd.offsetHeight) + "px";
    }
    cal.updateWCH(yc);
};

JsCalendar.tableMouseUp = function (ev) {
    var cal = JsCalendar._C;
    if (!cal) return false;
    if (cal.timeout) clearTimeout(cal.timeout);

    var el = cal.activeDiv;
    if (!el) return false;

    var target = JsCalendar_getTargetElement(ev);

    ev || (ev = window.event);
    JsCalendar_removeClass(el, "active");

    if (target == el || target.parentNode == el) JsCalendar.cellClick(el, ev);

    var mon = JsCalendar.findMonth(target);
    var date = null;
    if (mon) {
        if (!mon.disabled) {
            date = new Date(cal.date);
            if (mon.month != date.getMonth()) {
                date.setMonth(mon.month);
                cal.setDate(date);
                cal.dateClicked = false;
                cal.callHandler();
            }
        }
    }
    else {
        var year = JsCalendar.findYear(target);
        if (year) {
            date = new Date(cal.date);
            if (year.year != date.getFullYear()) {
                date.setFullYear(year.year);
                cal.setDate(date);
                cal.dateClicked = false;
                cal.callHandler();
            }
        }
        else {
            var hist = JsCalendar.findHist(target);
            if (hist && !hist.histDate.dateEqualsTo(cal.date)) {
                date = new Date(hist.histDate);
                cal._init(cal.firstDayOfWeek, cal.date = date);
                cal.dateClicked = false;
                cal.callHandler();
            }
        }
    }

    JsCalendar_removeEvent(window.document, "mouseup", JsCalendar.tableMouseUp);
    JsCalendar_removeEvent(window.document, "mouseover", JsCalendar.tableMouseOver);
    JsCalendar_removeEvent(window.document, "mousemove", JsCalendar.tableMouseOver);
    cal._hideCombos();
    JsCalendar._C = null;
    return JsCalendar_stopEvent(ev);
};

JsCalendar.tableMouseOver = function (ev) {
    var cal = JsCalendar._C;
    if (!cal) return;

    var el = cal.activeDiv;
    var target = JsCalendar_getTargetElement(ev);

    if (target == el || target.parentNode == el) {
        JsCalendar_addClass(el, "hilite active");
        JsCalendar_addClass(el.parentNode, "rowhilite");
    }
    else {
        if (typeof el.navtype == "undefined" || (el.navtype != 50 && ((el.navtype == 0 && !cal.histCombo) || Math.abs(el.navtype) > 2))) JsCalendar_removeClass(el, "active");
        JsCalendar_removeClass(el, "hilite");
        JsCalendar_removeClass(el.parentNode, "rowhilite");
    }

    ev || (ev = window.event);
    if (el.navtype == 50 && target != el) {
        var pos = JsCalendar_getAbsolutePos(el);
        var w = el.offsetWidth;
        var x = ev.clientX;
        var dx;
        var decrease = true;

        if (x > pos.x + w) {
            dx = x - pos.x - w;
            decrease = false;
        }
        else dx = pos.x - x;

        if (dx < 0) dx = 0;
        var range = el._range;
        var current = el._current;
        var date = cal.date;
        var pm = (date.getHours() >= 12);
        var old = el.firstChild.data;
        var count = Math.floor(dx / 10) % range.length;

        for (var i = range.length; --i >= 0;) if (range[i] == current) break;

        while (count-- > 0)
            if (decrease) if (--i < 0) i = range.length - 1;
            else if (++i >= range.length) i = 0;

        var status = false;
        if (cal.getDateStatus) status = cal.getDateStatus(new_date, date.getFullYear(), date.getMonth(), date.getDate(), parseInt(hour, 10), parseInt(minute, 10));
        if (status == false) if (!((!cal.time24) && (range[i] == "pm") && (hour > 23))) el.firstChild.data = range[i];
        cal.onUpdateTime();
    }

    var mon = JsCalendar.findMonth(target);
    if (mon) {
        if (!mon.disabled) {
            if (mon.month != cal.date.getMonth()) {
                if (cal.hilitedMonth) JsCalendar_removeClass(cal.hilitedMonth, "hilite");
                JsCalendar_addClass(mon, "hilite");
                cal.hilitedMonth = mon;
            }
            else if (cal.hilitedMonth) JsCalendar_removeClass(cal.hilitedMonth, "hilite");
        }
    }
    else {
        if (cal.hilitedMonth) JsCalendar_removeClass(cal.hilitedMonth, "hilite");

        var year = JsCalendar.findYear(target);

        if (year) {
            if (year.year != cal.date.getFullYear()) {
                if (cal.hilitedYear) JsCalendar_removeClass(cal.hilitedYear, "hilite");
                JsCalendar_addClass(year, "hilite");
                cal.hilitedYear = year;
            }
            else if (cal.hilitedYear) JsCalendar_removeClass(cal.hilitedYear, "hilite");
        }
        else {
            if (cal.hilitedYear) JsCalendar_removeClass(cal.hilitedYear, "hilite");

            var hist = JsCalendar.findHist(target);
            if (hist) {
                if (!hist.histDate.dateEqualsTo(cal.date)) {
                    if (cal.hilitedHist) JsCalendar_removeClass(cal.hilitedHist, "hilite");
                    JsCalendar_addClass(hist, "hilite");
                    cal.hilitedHist = hist;
                }
                else if (cal.hilitedHist) JsCalendar_removeClass(cal.hilitedHist, "hilite");
            }
            else if (cal.hilitedHist) JsCalendar_removeClass(cal.hilitedHist, "hilite");

        }
    }

    return JsCalendar_stopEvent(ev);
};

JsCalendar.tableMouseDown = function (ev) {
    if (JsCalendar_getTargetElement(ev) == JsCalendar_getElement(ev)) return JsCalendar_stopEvent(ev);
};

JsCalendar.calDragIt = function (ev) {
    ev || (ev = window.event);
    var cal = JsCalendar._C;
    return JsCalendar_stopEvent(ev);
};

JsCalendar.calDragEnd = function (ev) {
    var cal = JsCalendar._C;
    cal.dragging = false;
    JsCalendar_removeEvent(window.document, "mouseover", JsCalendar.calDragIt);
    JsCalendar_removeEvent(window.document, "mouseup", JsCalendar.calDragEnd);
    JsCalendar.tableMouseUp(ev);
    cal.hideShowCovered();
};

JsCalendar.dayMouseDown = function (ev) {
    var el = JsCalendar_getElement(ev);
    if (el.disabled) return false;
    var cal = el.calendar;
    cal.activeDiv = el;
    JsCalendar._C = cal;

    if (el.navtype != 300) {
        if (el.navtype == 50) {
            el._current = el.firstChild.data;
            JsCalendar_addEvent(window.document, "mousemove", JsCalendar.tableMouseOver);
        }
        else JsCalendar_addEvent(window.document, JsCalendar.is_ie5 ? "mousemove" : "mouseover", JsCalendar.tableMouseOver);

        JsCalendar_addClass(el, "hilite active");
        JsCalendar_addEvent(window.document, "mouseup", JsCalendar.tableMouseUp);
    }
    else if (cal.isPopup) cal._dragStart(ev);

    if (el.navtype == -1 || el.navtype == 1) {
        if (cal.timeout) clearTimeout(cal.timeout);
        cal.timeout = setTimeout("JsCalendar.showMonthsCombo()", 250);
    }
    else if (el.navtype == -2 || el.navtype == 2) {
        if (cal.timeout) clearTimeout(cal.timeout);
        cal.timeout = setTimeout((el.navtype > 0) ? "JsCalendar.showYearsCombo(true)" : "JsCalendar.showYearsCombo(false)", 250);
    }
    else cal.timeout = null;
    return JsCalendar_stopEvent(ev);
};

JsCalendar.dayMouseDblClick = function (ev) {
    JsCalendar.cellClick(JsCalendar_getElement(ev), ev || window.event);
    if (JsCalendar.is_ie) window.document.selection.empty();
};

JsCalendar.dayMouseOver = function (ev) {
    var el = JsCalendar_getElement(ev), caldate = el.caldate;
    if (caldate) caldate = new Date(caldate[0], caldate[1], caldate[2]);
    if (JsCalendar_isRelated(el, ev) || JsCalendar._C || el.disabled) return false;

    if (el.navtype != 300) {
        JsCalendar_addClass(el, "hilite");
        if (caldate) JsCalendar_addClass(el.parentNode, "rowhilite");
    }

    return JsCalendar_stopEvent(ev);
};

JsCalendar.dayMouseOut = function (ev) {
    var el = JsCalendar_getElement(ev);
    if (JsCalendar_isRelated(el, ev) || JsCalendar._C || el.disabled) return false;

    JsCalendar_removeClass(el, "hilite");
    if (el.caldate) JsCalendar_removeClass(el.parentNode, "rowhilite");

    return JsCalendar_stopEvent(ev);
};

JsCalendar.cellClick = function (el, ev) {
    var cal = el.calendar;
    var closing = false;
    var newdate = false;
    var date = null;

    if (typeof el.navtype == "undefined") {
        if (cal.currentDateEl) {
            JsCalendar_removeClass(cal.currentDateEl, "selected");
            JsCalendar_addClass(el, "selected");
            closing = (cal.currentDateEl == el);
            if (!closing) cal.currentDateEl = el;
        }

        cal.date.setDateOnly(new Date(el.caldate[0], el.caldate[1], el.caldate[2]));
        date = cal.date;
        var other_month = !(cal.dateClicked = !el.otherMonth);
        if (!other_month && cal.multiple) cal._toggleMultipleDate(new Date(date));
        newdate = true;
        if (other_month) cal._init(cal.firstDayOfWeek, date);
    }
    else {
        if (el.navtype == 200) {
            JsCalendar_removeClass(el, "hilite");
            cal.callCloseHandler();
            return;
        }
        date = new Date(cal.date);
        if (el.navtype == 0) date.setDateOnly(new Date());
        cal.dateClicked = false;
        var year = date.getFullYear();
        var mon = date.getMonth();

        function setMonth(m) {
            var day = date.getDate();
            var max = date.getMonthDays(m);
            if (day > max) date.setDate(max);
            date.setMonth(m);
        };

        switch (el.navtype) {
            case 400:
                JsCalendar_removeClass(el, "hilite");
                cal.params.inputField.value = "";
                return;

            case -2:
                if (year > cal.minYear) date.setFullYear(year - 1);
                break;

            case -1:
                if (mon > 0) setMonth(mon - 1);
                else if (year-- > cal.minYear) {
                    date.setFullYear(year);
                    setMonth(11);
                }
                break;

            case 1:
                if (mon < 11) setMonth(mon + 1);
                else if (year < cal.maxYear) {
                    date.setFullYear(year + 1);
                    setMonth(0);
                }
                break;

            case 2:
                if (year < cal.maxYear) date.setFullYear(year + 1);
                break;

            case 0:
                if (cal.getDateStatus && cal.getDateStatus(date, date.getFullYear(), date.getMonth(), date.getDate())) return false;
                break;
        }

        if (!date.equalsTo(cal.date)) {
            cal.setDate(date);
            newdate = true;
        }
    }
    if (newdate) cal.callHandler();
    if (closing) {
        JsCalendar_removeClass(el, "hilite");
        cal.callCloseHandler();
    }
};

JsCalendar.prototype.create = function (_par) {
    var parent = null;
    if (!_par) {
        parent = window.document.getElementsByTagName("body")[0];
        this.isPopup = true;
        this.WCH = JsCalendar_createWCH();
    }
    else {
        parent = _par;
        this.isPopup = false;
    }
    this.date = this.dateStr ? new Date(this.dateStr) : new Date();

    var table = JsCalendar_createElement("table");
    this.table = table;
    table.cellSpacing = 1;
    table.cellPadding = 0;
    table.calendar = this;
    JsCalendar_addEvent(table, "mousedown", JsCalendar.tableMouseDown);

    var div = JsCalendar_createElement("div");
    this.element = div;
    div.className = "JsCalendar";
    if (this.isPopup) {
        div.style.position = "absolute";
        div.style.display = "none";
    }
    div.appendChild(table);

    var thead = JsCalendar_createElement("thead", table);
    var cell = null;
    var row = null;

    var cal = this;
    var hh = function (text, cs, navtype, width) {
        cell = JsCalendar_createElement("td", row);
        cell.colSpan = cs;
        cell.className = "button1";

        if ((Math.abs(navtype) <= 2) && (navtype != 0)) cell.style.background = "url(http://www.lechupei.com/images/JsCalendar_arrow.gif) no-repeat 100% 100%";
        JsCalendar._add_evs(cell);
        cell.calendar = cal;
        cell.navtype = navtype;
        cell.style.width = width;
        if (text.substr(0, 1) != "&") cell.appendChild(document.createTextNode(text));
        else cell.innerHTML = text;
        return cell;
    };

    row = JsCalendar_createElement("tr", thead);
    var title_length = 6;
    this.isPopup && --title_length;
    this.weekNumbers && ++title_length;

    hh("－", 1, 400, "20px");
    this.title = hh("", title_length, 300, "100px");
    this.title.className = "title";

    if (this.isPopup) {
        this.title.style.cursor = "default";
        hh("&#x00d7;", 1, 200, "20px");
    }

    row = JsCalendar_createElement("tr", thead);
    row.className = "headrow";

    this._nav_py = hh("&#x00ab;", 1, -2, "20px");
    this._nav_pm = hh("&#x2039;", 1, -1, "20px");
    this._nav_now = hh(__JSCALENDAR_LANGUAGE_TEXTS__[0], this.weekNumbers ? 4 : 3, 0, "60px");
    this._nav_nm = hh("&#x203a;", 1, 1, "20px");
    this._nav_ny = hh("&#x00bb;", 1, 2, "20px");

    row = JsCalendar_createElement("tr", thead);
    row.className = "daynames";
    if (this.weekNumbers) {
        cell = JsCalendar_createElement("td", row);
        cell.className = "name wn";
        cell.appendChild(window.document.createTextNode(__JSCALENDAR_LANGUAGE_TEXTS__[1]));
        var cal_wk = __JSCALENDAR_LANGUAGE_TEXTS__[1];
        if (cal_wk == null) cal_wk = "";
    }

    for (var i = 7; i > 0; --i) {
        cell = JsCalendar_createElement("td", row);
        cell.appendChild(window.document.createTextNode(""));
        if (!i) {
            cell.navtype = 100;
            cell.calendar = this;
            JsCalendar._add_evs(cell);
        }
    }

    this.firstdayname = row.childNodes[this.weekNumbers ? 1 : 0];
    this._displayWeekdays();

    var tbody = JsCalendar_createElement("tbody", table);
    this.tbody = tbody;

    for (i = 6; i > 0; --i) {
        row = JsCalendar_createElement("tr", tbody);
        if (this.weekNumbers) {
            cell = JsCalendar_createElement("td", row);
            cell.appendChild(document.createTextNode(""));
        }
        for (var j = 7; j > 0; --j) {
            cell = JsCalendar_createElement("td", row);
            cell.appendChild(document.createTextNode(""));
            cell.calendar = this;
            JsCalendar._add_evs(cell);
        }
    }

    div = this.monthsCombo = JsCalendar_createElement("div", this.element);
    div.className = "combo";
    for (i = 0; i < 12; ++i) {
        var mn = JsCalendar_createElement("div");
        mn.className = JsCalendar.is_ie ? "label-IEfix" : "label";
        mn.month = i;
        mn.appendChild(window.document.createTextNode(__JSCALENDAR_LANGUAGE_MONTHS__[i]));
        div.appendChild(mn);
    }

    div = this.yearsCombo = JsCalendar_createElement("div", this.element);
    div.className = "combo";
    for (i = 12; i > 0; --i) {
        var yr = JsCalendar_createElement("div");
        yr.className = JsCalendar.is_ie ? "label-IEfix" : "label";
        yr.appendChild(window.document.createTextNode(""));
        div.appendChild(yr);
    }

    div = this.histCombo = JsCalendar_createElement("div", this.element);
    div.className = "combo history";

    this._init(this.firstDayOfWeek, this.date);
    parent.appendChild(this.element);
};

JsCalendar._keyEvent = function (ev) {
    if (!window.calendar) return false;
    (JsCalendar.is_ie) && (ev = window.event);
    var cal = window.calendar;
    var act = (JsCalendar.is_ie || ev.type == "keypress");
    var K = ev.keyCode;

    if (ev.ctrlKey) {
        switch (K) {
            case 37:
                act && JsCalendar.cellClick(cal._nav_pm);
                break;
            case 38:
                act && JsCalendar.cellClick(cal._nav_py);
                break;
            case 39:
                act && JsCalendar.cellClick(cal._nav_nm);
                break;
            case 40:
                act && JsCalendar.cellClick(cal._nav_ny);
                break;
            default:
                return false;
        }
    }
    else
        switch (K) {
            case 32:
                JsCalendar.cellClick(cal._nav_now);
                break;
            case 27:
                act && cal.callCloseHandler();
                break;
            case 37:
            case 38:
            case 39:
            case 40:
                if (act) {
                    var prev, pos, x, y, ne, el;
                    prev = (K == 37) || (K == 38);
                    function setVars() {
                        el = cal.currentDateEl;
                        pos = el.pos;
                        y = pos[0];
                        x = pos[1];
                        ne = cal.ar_days[y][x];
                    };
                    setVars();

                    function prevMonth() {
                        var date = new Date(cal.date.getFullYear(), cal.date.getMonth() - 1, 1);
                        date.setDate(date.getMonthDays());
                        cal.setDate(date);
                    };

                    function nextMonth() {
                        var date = new Date(cal.date.getFullYear(), cal.date.getMonth() + 1, 1);
                        cal.setDate(date);
                    };

                    while (1) {
                        switch (K) {
                            case 37:
                                if (--x >= 0) ne = cal.ar_days[y][x];
                                else { x = 6; K = 38; continue; }
                                break;
                            case 38:
                                if (--y >= 0) ne = cal.ar_days[y][x];
                                else { prevMonth(); setVars(); }
                                break;
                            case 39:
                                if (++x < 7) ne = cal.ar_days[y][x];
                                else { x = 0; K = 40; continue; }
                                break;
                            case 40:
                                if (++y < cal.ar_days.length) ne = cal.ar_days[y][x];
                                else { nextMonth(); setVars(); }
                                break;
                        }
                        break;
                    }

                    if (ne) {
                        if (!ne.otherMonth) {
                            JsCalendar_removeClass(el, "selected");
                            JsCalendar_addClass(ne, "selected");
                            cal.date.setDateOnly(new Date(ne.caldate[0], ne.caldate[1], ne.caldate[2]));
                            cal.currentDateEl = ne;
                        }
                        else {
                            if (!ne.disabled) JsCalendar.cellClick(ne);
                            else if (prev) prevMonth();
                            else nextMonth();
                        }
                    }
                    cal.callHandler();
                }
                break;
            case 13:
                if (act) {
                    cal.callHandler();
                    cal.hide();
                }
                break;
            default:
                return false;
        }

    return JsCalendar_stopEvent(ev);
};

JsCalendar.prototype._init = function (firstDayOfWeek, date) {
    var today = new Date(), TD = today.getDate(), TY = today.getFullYear(), TM = today.getMonth();
    var year = date.getFullYear();
    var month = date.getMonth();

    if (year < this.minYear)
        date.setFullYear(year = this.minYear);
    else if (year > this.maxYear)
        date.setFullYear(year = this.maxYear);
    if (year == this.minYear && month < this.minMonth)
        date.setMonth(month = this.minMonth);
    else if (year == this.maxYear && month > this.maxMonth)
        date.setMonth(month = this.maxMonth);

    this.firstDayOfWeek = firstDayOfWeek;
    this.date = date;
    (this.date = new Date(this.date)).setDateOnly(date);
    var mday = date.getDate();
    var no_days = date.getMonthDays();

    date.setDate(1);
    var day1 = (date.getDay() - this.firstDayOfWeek) % 7;
    if (day1 < 0) day1 += 7;
    date.setDate(-day1);
    date.setDate(date.getDate() + 1);

    var row = this.tbody.firstChild;
    var MN = __JSCALENDAR_LANGUAGE_MONTHS__[month];
    var ar_days = this.ar_days = [];
    var weekend = "WEEKEND";
    var dates = this.multiple ? (this.datesCells = {}) : null;

    for (var i = 7; --i > 0; row = row.nextSibling) {
        var cell = row.firstChild;
        if (this.weekNumbers) {
            cell.className = "day wn";
            cell.innerHTML = date.getWeekNumber();
            cell = cell.nextSibling;
        }

        var ar_days_y = ar_days[ar_days.length] = [];
        row.className = "daysrow";
        var hasdays = false, iday;
        for (; cell && (iday = date.getDate()) ; date.setDate(iday + 1), cell = cell.nextSibling) {
            cell.pos = [6 - i, ar_days_y.length];
            ar_days_y[ar_days_y.length] = cell;
            var wday = date.getDay(), dmonth = date.getMonth(), dyear = date.getFullYear();
            cell.className = "day";
            var current_month = !(cell.otherMonth = !(dmonth == month));
            if (!current_month) {
                if (this.showsOtherMonths)
                    cell.className += " othermonth";
                else {
                    cell.className = "emptycell";
                    cell.innerHTML = "&nbsp;";
                    cell.disabled = true;
                    continue;
                }
            }
            else hasdays = true;

            cell.disabled = false;
            cell.innerHTML = iday;

            dates && (dates[date.print("%Y%m%d")] = cell);
            if (this.getDateStatus) {
                var status = this.getDateStatus(date, year, month, iday);

                if (status === true) {
                    cell.className += " disabled";
                    cell.disabled = true;
                }
                else {
                    if (/disabled/i.test(status)) cell.disabled = true;
                    cell.className += " " + status;
                }
            }
            if (!cell.disabled) {
                cell.caldate = [dyear, dmonth, iday];
                cell.ttip = "_";
                if (!this.multiple && current_month && iday == mday) {
                    cell.className += " selected";
                    this.currentDateEl = cell;
                }
                if (dyear == TY && dmonth == TM && iday == TD) cell.className += " today";
                if ((weekend != null) && (weekend.indexOf(wday.toString()) != -1)) cell.className += cell.otherMonth ? " oweekend" : " weekend";
            }
        }
        if (!(hasdays || this.showsOtherMonths)) row.className = "emptyrow";
    }

    this.title.innerHTML = year + __JSCALENDAR_LANGUAGE_TEXTS__[2] + (month + 1) + __JSCALENDAR_LANGUAGE_TEXTS__[3];
    this._initMultipleDates();
    this.updateWCH();
};

JsCalendar.prototype._initMultipleDates = function () {
    if (this.multiple) {
        for (var i in this.multiple) {
            var cell = this.datesCells[i];
            var d = this.multiple[i];
            if (!d) continue;
            if (cell) cell.className += " selected";
        }
    }
};

JsCalendar.prototype._toggleMultipleDate = function (date) {
    if (this.multiple) {
        var ds = date.print("%Y%m%d");
        var cell = this.datesCells[ds];
        if (cell) {
            var d = this.multiple[ds];
            if (!d) {
                JsCalendar_addClass(cell, "selected");
                this.multiple[ds] = date;
            }
            else {
                JsCalendar_removeClass(cell, "selected");
                delete this.multiple[ds];
            }
        }
    }
};

JsCalendar.prototype.setDate = function (date) {
    if (!date) date = new Date();
    if (!date.equalsTo(this.date)) {
        var year = date.getFullYear(), m = date.getMonth();
        this._init(this.firstDayOfWeek, date);
    }
};

JsCalendar.prototype.reinit = function () {
    this._init(this.firstDayOfWeek, this.date);
};

JsCalendar.prototype.refresh = function () {
    var p = this.isPopup ? null : this.element.parentNode;
    var x = parseInt(this.element.style.left);
    var y = parseInt(this.element.style.top);
    this.destroy();
    this.dateStr = this.date;
    this.create(p);

    if (this.isPopup) this.showAt(x, y);
    else this.show();
};

JsCalendar.prototype.setDateStatusHandler = JsCalendar.prototype.setDisabledHandler = function (unaryFunction) {
    this.getDateStatus = unaryFunction;
};

JsCalendar.prototype.setMultipleDates = function (multiple) {
    if (!multiple || typeof multiple == "undefined") return;

    this.multiple = {};
    for (var i = multiple.length; --i >= 0;) {
        var d = multiple[i];
        var ds = d.print("%Y%m%d");
        this.multiple[ds] = d;
    }
};

JsCalendar.prototype.callHandler = function () {
    if (this.onSelected) this.onSelected(this, this.date.print(this.dateFormat));
};

JsCalendar.prototype.callCloseHandler = function () {
    if (this.onClose) this.onClose(this);
    this.hideShowCovered();
};

JsCalendar.prototype.destroy = function () {
    this.hide();
    JsCalendar_destroy(this.element);
    JsCalendar_destroy(this.WCH);
    JsCalendar._C = null;
    window.calendar = null;
};

JsCalendar.prototype.reparent = function (new_parent) {
    var el = this.element;
    el.parentNode.removeChild(el);
    new_parent.appendChild(el);
};

JsCalendar._checkCalendar = function (ev) {
    if (!window.calendar) return false;
    var el = JsCalendar.is_ie ? JsCalendar_getElement(ev) : JsCalendar_getTargetElement(ev);
    for (; el != null && el != calendar.element; el = el.parentNode);
    if (el == null) {
        window.calendar.callCloseHandler();
        return JsCalendar_stopEvent(ev);
    }
};

JsCalendar.prototype.updateWCH = function (other_el) {
    JsCalendar_setupWCH_el(this.WCH, this.element, other_el);
};

JsCalendar.prototype.show = function () {
    var rows = this.table.getElementsByTagName("tr");
    for (var i = rows.length; i > 0;) {
        var row = rows[--i];
        JsCalendar_removeClass(row, "rowhilite");
        var cells = row.getElementsByTagName("td");
        for (var j = cells.length; j > 0;) {
            var cell = cells[--j];
            JsCalendar_removeClass(cell, "hilite");
            JsCalendar_removeClass(cell, "active");
        }
    }

    this.element.style.display = "block";
    this.hidden = false;
    if (this.isPopup) {
        this.updateWCH();
        window.calendar = this;
        if (!this.noGrab) {
            JsCalendar_addEvent(window.document, "keydown", JsCalendar._keyEvent);
            JsCalendar_addEvent(window.document, "keypress", JsCalendar._keyEvent);
            JsCalendar_addEvent(window.document, "mousedown", JsCalendar._checkCalendar);
        }
    }
    this.hideShowCovered();
};

JsCalendar.prototype.hide = function () {
    if (this.isPopup) {
        JsCalendar_removeEvent(window.document, "keydown", JsCalendar._keyEvent);
        JsCalendar_removeEvent(window.document, "keypress", JsCalendar._keyEvent);
        JsCalendar_removeEvent(window.document, "mousedown", JsCalendar._checkCalendar);
    }

    this.element.style.display = "none";
    JsCalendar_hideWCH(this.WCH);
    this.hidden = true;
    this.hideShowCovered();
};

JsCalendar.prototype.showAt = function (x, y) {
    var s = this.element.style;
    s.left = x + "px";
    s.top = y + "px";
    this.show();
};

JsCalendar.prototype.showAtElement = function (el, opts) {
    var self = this;
    var p = JsCalendar_getAbsolutePos(el);
    p.x = p.x + 1;
    if (!opts || typeof opts != "string") {
        this.showAt(p.x, p.y + el.offsetHeight);
        return true;
    }

    this.element.style.display = "block";
    var w = self.element.offsetWidth;
    var h = self.element.offsetHeight;
    self.element.style.display = "none";
    var valign = opts.substr(0, 1);
    var halign = "l";
    if (opts.length > 1) halign = opts.substr(1, 1);

    switch (valign) {
        case "T": p.y -= h; break;
        case "B": p.y += el.offsetHeight; break;
        case "C": p.y += (el.offsetHeight - h) / 2; break;
        case "t": p.y += el.offsetHeight - h; break;
        case "b": break;
    }

    switch (halign) {
        case "L": p.x -= w; break;
        case "R": p.x += el.offsetWidth; break;
        case "C": p.x += (el.offsetWidth - w) / 2; break;
        case "l": p.x += el.offsetWidth - w; break;
        case "r": break;
    }
    p.width = w;
    p.height = h + 40;
    self.monthsCombo.style.display = "none";
    JsCalendar_fixBoxPosition(p);
    self.showAt(p.x, p.y);
};

JsCalendar.prototype.setDateFormat = function (str) {
    this.dateFormat = str;
};

JsCalendar.prototype.parseDate = function (str, fmt) {
    if (!str) return this.setDate(this.date);
    if (!fmt) fmt = this.dateFormat;

    var date = Date.parseDate(str, fmt);
    return this.setDate(date);
};

JsCalendar.prototype.hideShowCovered = function () {
    if (!JsCalendar.is_ie5) return;
    var self = this;

    function getVisib(obj) {
        var value = obj.style.visibility;
        if (!value) {
            if (window.document.defaultView && typeof (window.document.defaultView.getComputedStyle) == "function") {
                if (!JsCalendar.is_khtml) value = window.document.defaultView.getComputedStyle(obj, "").getPropertyValue("visibility");
                else value = '';
            }
            else if (obj.currentStyle) value = obj.currentStyle.visibility;
            else value = '';
        }
        return value;
    };

    var tags = ["applet", "iframe", "select"];
    var el = self.element;

    var p = JsCalendar_getAbsolutePos(el);
    var EX1 = p.x;
    var EX2 = el.offsetWidth + EX1;
    var EY1 = p.y;
    var EY2 = el.offsetHeight + EY1;

    for (var k = tags.length; k > 0;) {
        var ar = window.document.getElementsByTagName(tags[--k]);
        var cc = null;

        for (var i = ar.length; i > 0;) {
            cc = ar[--i];
            p = JsCalendar_getAbsolutePos(cc);
            var CX1 = p.x;
            var CX2 = cc.offsetWidth + CX1;
            var CY1 = p.y;
            var CY2 = cc.offsetHeight + CY1;

            if (self.hidden || (CX1 > EX2) || (CX2 < EX1) || (CY1 > EY2) || (CY2 < EY1)) {
                if (!cc.__msh_save_visibility) cc.__msh_save_visibility = getVisib(cc);
                cc.style.visibility = cc.__msh_save_visibility;
            }
            else {
                if (!cc.__msh_save_visibility) cc.__msh_save_visibility = getVisib(cc);
                cc.style.visibility = "hidden";
            }
        }
    }
};

JsCalendar.prototype._displayWeekdays = function () {
    var cell = this.firstdayname;

    for (var i = 0; i < 7; ++i) {
        cell.className = "day name";
        cell.navtype = 100;
        cell.calendar = this;
        cell.innerHTML = __JSCALENDAR_LANGUAGE_WEEKS__[i];
        cell = cell.nextSibling;
    }
};

JsCalendar.prototype._hideCombos = function () {
    this.monthsCombo.style.display = "none";
    this.yearsCombo.style.display = "none";
    this.histCombo.style.display = "none";
    this.updateWCH();
};

JsCalendar.prototype._dragStart = function (ev) {
    ev || (ev = window.event);
    this.dragging = true;
    JsCalendar_addEvent(window.document, "mouseover", JsCalendar.calDragIt);
    JsCalendar_addEvent(window.document, "mouseup", JsCalendar.calDragEnd);
};

Date._MD = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
Date.SECOND = 1000;
Date.MINUTE = 60 * Date.SECOND;
Date.HOUR = 60 * Date.MINUTE;
Date.DAY = 24 * Date.HOUR;
Date.WEEK = 7 * Date.DAY;

Date.prototype.getMonthDays = function (month) {
    var year = this.getFullYear();
    if (typeof month == "undefined") month = this.getMonth();
    if (((0 == (year % 4)) && ((0 != (year % 100)) || (0 == (year % 400)))) && month == 1) return 29;
    else return Date._MD[month];
};

Date.prototype.getDayOfYear = function () {
    var now = new Date(this.getFullYear(), this.getMonth(), this.getDate(), 0, 0, 0);
    var then = new Date(this.getFullYear(), 0, 0, 0, 0, 0);
    var time = now - then;
    return Math.floor(time / Date.DAY);
};

Date.prototype.getWeekNumber = function () {
    var d = new Date(this.getFullYear(), this.getMonth(), this.getDate(), 0, 0, 0);
    var DoW = d.getDay();
    d.setDate(d.getDate() - (DoW + 6) % 7 + 3);
    var ms = d.valueOf();
    d.setMonth(0);
    d.setDate(4);
    return Math.round((ms - d.valueOf()) / (7 * 864e5)) + 1;
};

Date.prototype.equalsTo = function (date) {
    return ((this.getFullYear() == date.getFullYear()) &&
		     (this.getMonth() == date.getMonth()) &&
		     (this.getDate() == date.getDate()) &&
		     (this.getHours() == date.getHours()) &&
		     (this.getMinutes() == date.getMinutes())
		   );
};

Date.prototype.dateEqualsTo = function (date) {
    return ((this.getFullYear() == date.getFullYear()) &&
		     (this.getMonth() == date.getMonth()) &&
		     (this.getDate() == date.getDate())
		   );
};

Date.prototype.setDateOnly = function (date) {
    var tmp = new Date(date);
    this.setDate(1);
    this.setFullYear(tmp.getFullYear());
    this.setMonth(tmp.getMonth());
    this.setDate(tmp.getDate());
};

Date.prototype.print = function (str) {
    var m = this.getMonth();
    var d = this.getDate();
    var y = this.getFullYear();
    var wn = this.getWeekNumber();
    var w = this.getDay();
    var s = {};
    var hr = this.getHours();
    var pm = (hr >= 12);
    var ir = (pm) ? (hr - 12) : hr;
    var dy = this.getDayOfYear();
    if (ir == 0) ir = 12;
    var min = this.getMinutes();
    var sec = this.getSeconds();

    s["%a"] = __JSCALENDAR_LANGUAGE_WEEKS__[w];
    s["%A"] = __JSCALENDAR_LANGUAGE_WEEKS__[w];
    s["%b"] = __JSCALENDAR_LANGUAGE_MONTHS__[m];
    s["%B"] = __JSCALENDAR_LANGUAGE_MONTHS__[m];
    s["%C"] = 1 + Math.floor(y / 100);
    s["%d"] = (d < 10) ? ("0" + d) : d;
    s["%e"] = d;
    s["%H"] = (hr < 10) ? ("0" + hr) : hr;
    s["%I"] = (ir < 10) ? ("0" + ir) : ir;
    s["%j"] = (dy < 100) ? ((dy < 10) ? ("00" + dy) : ("0" + dy)) : dy;
    s["%k"] = hr ? hr : "0";
    s["%l"] = ir;
    s["%m"] = (m < 9) ? ("0" + (1 + m)) : (1 + m);
    s["%M"] = (min < 10) ? ("0" + min) : min;
    s["%n"] = "\n";
    s["%p"] = pm ? "PM" : "AM";
    s["%P"] = pm ? "pm" : "am";
    s["%s"] = Math.floor(this.getTime() / 1000);
    s["%S"] = (sec < 10) ? ("0" + sec) : sec;
    s["%t"] = "\t";
    s["%U"] = s["%W"] = s["%V"] = (wn < 10) ? ("0" + wn) : wn;
    s["%u"] = (w == 0) ? 7 : w;
    s["%w"] = w;
    s["%y"] = ('' + y).substr(2, 2);
    s["%Y"] = y;
    s["%%"] = "%";

    var re = /%./g;
    if (!JsCalendar.is_ie5 && !JsCalendar.is_khtml && !JsCalendar.is_mac_ie)
        return str.replace(re, function (par) { return s[par] || par; });

    var a = str.match(re);
    for (var i = 0; i < a.length; i++) {
        var tmp = s[a[i]];
        if (tmp) {
            re = new RegExp(a[i], 'g');
            str = str.replace(re, tmp);
        }
    }

    return str;
};

Date.parseDate = function (str, fmt) {
    if (!str) return new Date();

    var y = 0;
    var m = -1;
    var d = 0;
    var a = str.split(/\W+/);
    var b = fmt.match(/%./g);
    var i = 0, j = 0;
    var hr = 0;
    var min = 0;
    for (i = 0; i < a.length; ++i) {
        if (!a[i]) continue;

        switch (b[i]) {
            case "%d":
            case "%e":
                d = parseInt(a[i], 10);
                break;
            case "%m":
                m = parseInt(a[i], 10) - 1;
                break;
            case "%Y":
            case "%y":
                y = parseInt(a[i], 10);
                (y < 100) && (y += (y > 29) ? 1900 : 2000);
                break;
            case "%b":
            case "%B":
                for (j = 0; j < 12; ++j) if (__JSCALENDAR_LANGUAGE_MONTHS__[j].substr(0, a[i].length).toLowerCase() == a[i].toLowerCase()) { m = j; break; }
                break;
            case "%H":
            case "%I":
            case "%k":
            case "%l":
                hr = parseInt(a[i], 10);
                break;
            case "%P":
            case "%p":
                if (/pm/i.test(a[i]) && hr < 12) hr += 12;
                break;
            case "%M":
                min = parseInt(a[i], 10);
                break;
        }
    }

    if (y != 0 && m != -1 && d != 0) return new Date(y, m, d, hr, min, 0);
    y = 0; m = -1; d = 0;

    for (i = 0; i < a.length; ++i) {
        if (a[i].search(/[a-zA-Z]+/) != -1) {
            var t = -1;
            for (j = 0; j < 12; ++j) if (__JSCALENDAR_LANGUAGE_MONTHS__[j].substr(0, a[i].length).toLowerCase() == a[i].toLowerCase()) { t = j; break; }
            if (t != -1) {
                if (m != -1) d = m + 1;
                m = t;
            }
        }
        else if (parseInt(a[i], 10) <= 12 && m == -1) m = a[i] - 1;
        else if (parseInt(a[i], 10) > 31 && y == 0) {
            y = parseInt(a[i], 10);
            (y < 100) && (y += (y > 29) ? 1900 : 2000);
        }
        else if (d == 0) d = a[i];
    }

    if (y == 0) {
        var today = new Date();
        y = today.getFullYear();
    }

    if (m != -1 && d != 0) return new Date(y, m, d, hr, min, 0);
    return null;
};

Date.prototype.__msh_oldSetFullYear = Date.prototype.setFullYear;

Date.prototype.setFullYear = function (y) {
    var d = new Date(this);
    d.__msh_oldSetFullYear(y);
    if (d.getMonth() != this.getMonth()) this.setDate(28);
    this.__msh_oldSetFullYear(y);
};

window.calendar = null;

JsCalendar_addEvent = function (el, evname, func) {
    if (el.attachEvent) el.attachEvent("on" + evname, func);
    else if (el.addEventListener) el.addEventListener(evname, func, true);
    else el["on" + evname] = func;
};

JsCalendar_removeEvent = function (el, evname, func) {
    if (el.detachEvent) el.detachEvent("on" + evname, func);
    else if (el.removeEventListener) el.removeEventListener(evname, func, true);
    else el["on" + evname] = null;
};

JsCalendar_stopEvent = function (ev) {
    ev || (ev = window.event);
    if (JsCalendar.is_ie) { ev.cancelBubble = true; ev.returnValue = false; }
    else { ev.preventDefault(); ev.stopPropagation(); }

    return false;
};

JsCalendar_removeClass = function (el, className) {
    if (!(el && el.className)) return;
    var cls = el.className.split(" ");
    var ar = [];
    for (var i = cls.length; i > 0;) if (cls[--i] != className) ar[ar.length] = cls[i];
    el.className = ar.join(" ");
};

JsCalendar_addClass = function (el, className) {
    JsCalendar_removeClass(el, className);
    el.className += " " + className;
};

JsCalendar_getElement = function (ev) {
    if (JsCalendar.is_ie) return window.event.srcElement;
    else return ev.currentTarget;
};

JsCalendar_getTargetElement = function (ev) {
    if (JsCalendar.is_ie) return window.event.srcElement;
    else return ev.target;
};

JsCalendar_createElement = function (type, parent) {
    var el = null;
    if (window.self.document.createElementNS) el = window.self.document.createElementNS("http://www.w3.org/1999/xhtml", type);
    else el = window.self.document.createElement(type);

    if (typeof parent != "undefined") parent.appendChild(el);
    if (JsCalendar.is_ie) el.setAttribute("unselectable", true);
    if (JsCalendar.is_gecko) el.style.setProperty("-moz-user-select", "none", "");
    return el;
};

JsCalendar_getAbsolutePos = function (el) {
    var SL = 0, ST = 0;
    var is_div = /^div$/i.test(el.tagName);

    if (is_div && el.scrollLeft) SL = el.scrollLeft;
    if (is_div && el.scrollTop) ST = el.scrollTop;

    var r = { x: el.offsetLeft - SL, y: el.offsetTop - ST };
    if (el.offsetParent) {
        var tmp = JsCalendar_getAbsolutePos(el.offsetParent);
        r.x += tmp.x;
        r.y += tmp.y;
    }
    return r;
};

JsCalendar_fixBoxPosition = function (box) {
    if (box.x < 0) box.x = 0;
    if (box.y < 0) box.y = 0;

    var cp = JsCalendar_createElement("div");
    var s = cp.style;
    s.position = "absolute";
    s.right = s.bottom = s.width = s.height = "0px";
    window.document.body.appendChild(cp);
    var br = JsCalendar_getAbsolutePos(cp);
    window.document.body.removeChild(cp);

    if (JsCalendar.is_ie) {
        br.y += window.document.body.scrollTop;
        br.x += window.document.body.scrollLeft;
    }
    else {
        br.y += window.scrollY;
        br.x += window.scrollX;
    }

    var tmp = box.x + box.width - br.x;
    if (tmp > 0) box.x -= tmp;
    tmp = box.y + box.height - br.y;
    if (tmp > 0) box.y -= tmp;
};

JsCalendar_isRelated = function (el, evt) {
    evt || (evt = window.event);
    var related = evt.relatedTarget;
    if (!related) {
        var type = evt.type;
        if (type == "mouseover") related = evt.fromElement;
        else if (type == "mouseout") related = evt.toElement;
    }
    try {
        while (related) {
            if (related == el) return true;
            related = related.parentNode;
        }
    }
    catch (e) { };

    return false;
};

JsCalendar_createWCH = function () {
    var f = null;
    if (JsCalendar.is_ie && !JsCalendar.is_ie5) {
        var filter = 'filter:progid:DXImageTransform.Microsoft.alpha(style=0,opacity=0);';
        var id = "WCH" + (++JsCalendar.__wch_id);
        window.self.document.body.insertAdjacentHTML('beforeEnd', '<iframe id="' + id + '" scroll="no" frameborder="0" ' + 'style="z-index:0;position:absolute;visibility:hidden;' + filter + 'border:0;top:0;left:0;width:0;height:0;" ' + 'src="javascript:false;"></iframe>');
        f = window.self.document.getElementById(id);
    }
    return f;
};

JsCalendar_setupWCH_el = function (f, el, el2) {
    if (f) {
        var pos = JsCalendar_getAbsolutePos(el), X1 = pos.x, Y1 = pos.y, X2 = X1 + el.offsetWidth, Y2 = Y1 + el.offsetHeight;
        if (el2) {
            var p2 = JsCalendar_getAbsolutePos(el2), XX1 = p2.x, YY1 = p2.y, XX2 = XX1 + el2.offsetWidth, YY2 = YY1 + el2.offsetHeight;
            if (X1 > XX1) X1 = XX1;
            if (Y1 > YY1) Y1 = YY1;
            if (X2 < XX2) X2 = XX2;
            if (Y2 < YY2) Y2 = YY2;
        }
        JsCalendar_setupWCH(f, X1, Y1, X2 - X1, Y2 - Y1);
    }
};

JsCalendar_setupWCH = function (f, x, y, w, h) {
    if (f) {
        var s = f.style;
        (typeof x != "undefined") && (s.left = x + "px");
        (typeof y != "undefined") && (s.top = y + "px");
        (typeof w != "undefined") && (s.width = w + "px");
        (typeof h != "undefined") && (s.height = h + "px");
        s.visibility = "visible";
    }
};

JsCalendar_hideWCH = function (f) {
    if (f) f.style.visibility = "hidden";
};

JsCalendar_destroy = function (el) {
    if (el && el.parentNode) el.parentNode.removeChild(el);
};
