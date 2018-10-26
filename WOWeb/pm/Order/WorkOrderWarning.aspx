<%@ Page Language="C#" AutoEventWireup="true" Inherits="project.Presentation.Order.WorkOrderWarning,project"  %>
<!DOCTYPE>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="head1" runat="server">
    <title>工单预警记录查看</title>
    <!--[if lt IE 9]>
    <script type="text/javascript" src="../../jscript/html5.js"></script>
    <script type="text/javascript" src="../../jscript/respond.min.js"></script>
    <![endif]-->
    <link href="../../css/H-ui.min.css" rel="stylesheet" type="text/css" />
    <link href="../../css/H-ui.admin.css" rel="stylesheet" type="text/css" />
    <link href="../../lib/iconfont/iconfont.css" rel="stylesheet" type="text/css" />
    <link href="../../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/JsInput.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function transmitData(submitData) {
            var data = submitData;
            <%=ClientScript.GetCallbackEventReference(this, "data", "BandResuleData", null) %>
        }
    </script>
</head>
<body>
    <form id="form1" runat="server"></form>
    <nav class="breadcrumb"><i class="Hui-iconfont">&#xe67f;</i> 首页 <span class="c-gray en">&gt;</span> 工单管理 <span class="c-gray en">&gt;</span> 工单预警记录查看 <a class="btn btn-success radius r mr-20" style="line-height:1.6em;margin-top:2px" href="javascript:location.replace(location.href);" title="刷新" ><i class="Hui-iconfont">&#xe68f;</i></a></nav>
    <div id="list" class="pt-5 pr-20 pb-5 pl-20">
	    <div class="cl pd-5 bg-1 bk-gray mt-5"> 
		    日志类型&nbsp;<%=orderType %>&nbsp;
		    订单日期&nbsp;<input type="text" class="input-text required size-S" id="MinOrderDate">&nbsp;
		    至&nbsp;<input type="text" class="input-text required size-S" id="MaxOrderDate">&nbsp;
		    工单单号&nbsp;<input type="text" class="input-text size-S" style="width:110px" placeholder="工单号" id="OrderNo" name="">&nbsp;
		    操作人&nbsp;<input type="text" class="input-text size-S" style="width:110px" readonly id="UserName" name="">
              <a href="javascript:void();" class="btn btn-primary radius upload-btn size-S" onclick="ChooseBasic('UserNo', 'user')" >选择</a>
              <input type="hidden" id="UserNo" />&nbsp;
		    客户名称&nbsp;<input type="text" class="input-text size-S" style="width:110px" readonly id="CustName" name="">
              <a href="javascript:void();" class="btn btn-primary radius upload-btn size-S" onclick="ChooseBasic('CustNo', 'cust')" >选择</a>
              <input type="hidden" id="CustNo" />&nbsp;
		    <button type="submit" class="btn btn-success radius" onclick="select()"><i class="Hui-iconfont">&#xe665;</i> 检索</button>
	    </div>
	    <div class="mt-5" id="outerlist">
	    <%=list %>
	    </div>
    </div>
    <div id="edit" class="pt-5 pr-20 pb-5 pl-20" style="display:none;"></div>
    <script type="text/javascript" src="../../jscript/jquery-1.10.2.js"></script> 
    <script type="text/javascript" src="../../jscript/script_ajax.js"></script>
    <script type="text/javascript" src="../../jscript/script_common.js" charset="gb2312"></script>
    <script type="text/javascript" src="../../jscript/json2.js"></script>
    <script type="text/javascript" src="../../jscript/JsInputDate.js"></script>
    <script type="text/javascript" src="../../jscript/H-ui.js"></script> 
    <script type="text/javascript" src="../../jscript/H-ui.admin.js"></script> 
    <script type="text/javascript" src="../../lib/layer/layer.js"></script> 
    <script type="text/javascript">
        function BandResuleData(temp) {
            var vjson = JSON.parse(temp);
            if (vjson.type == "select") {
                if (vjson.flag == "1") {
                    $("#outerlist").html(vjson.liststr);
                }
            }
        }

        function select() {
            var submitData = new Object();
            submitData.Type = "select";
            submitData.OrderType = $("#OrderType").val();
            submitData.OrderNo = $("#OrderNo").val();
            submitData.CustNo = $("#CustNo").val();
            submitData.UserNo = $("#UserNo").val();
            submitData.MinOrderDate = $("#MinOrderDate").val();
            submitData.MaxOrderDate = $("#MaxOrderDate").val();
            transmitData(datatostr(submitData));
            return;
        }
        
        function choose(id, labels, values) {
            if (labels != "" && labels != undefined && labels != "undefined") {
                if (id == "CustNo") {
                    $("#CustName").val(values);
                    $("#CustNo").val(labels);
                }
                else if (id == "UserNo") {
                    $("#UserName").val(values);
                    $("#UserNo").val(labels);
                }
            }
        }

        jQuery(function () {
            var MinOrderDate = new JsInputDate("MinOrderDate");
            MinOrderDate.setDisabled(false);
            MinOrderDate.setHeight("26px");
            MinOrderDate.setWidth("100px");
            MinOrderDate.setValue(gettoday());

            var MaxOrderDate = new JsInputDate("MaxOrderDate");
            MaxOrderDate.setDisabled(false);
            MaxOrderDate.setHeight("26px");
            MaxOrderDate.setWidth("100px");
            MaxOrderDate.setValue(gettoday());
        });
</script>
</body>
</html>