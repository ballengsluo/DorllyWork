<%@ Page Language="C#" AutoEventWireup="true" Inherits="project.Presentation.Order.WorkOrderCost,project"  %>
<!DOCTYPE>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="head1" runat="server">
    <title>工单费用审核</title>
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
    <nav class="breadcrumb"><i class="Hui-iconfont">&#xe67f;</i> 首页 <span class="c-gray en">&gt;</span> 工单管理 <span class="c-gray en">&gt;</span> 工单费用审核 <a class="btn btn-success radius r mr-20" style="line-height:1.6em;margin-top:2px" href="javascript:location.replace(location.href);" title="刷新" ><i class="Hui-iconfont">&#xe68f;</i></a></nav>
    <div id="list" class="pt-5 pr-20 pb-5 pl-20">
	    <div class="cl pd-5 bg-1 bk-gray mt-5">
            <a href="javascript:;" onclick="detail()" class="btn btn-primary radius"><i class="Hui-iconfont">&#xe60c;</i> 费用明细</a> &nbsp;&nbsp;
            <a href="javascript:;" onclick="approve()" class="btn btn-primary radius"><i class="Hui-iconfont">&#xe6e1;</i> 审核/反审</a> &nbsp;&nbsp;
		    工单号&nbsp;<input type="text" class="input-text size-S" style="width:110px" placeholder="工单号" id="OrderNo" name="">&nbsp;
		    工单日期&nbsp;<input type="text" class="input-text required size-S" id="MinCostDate">&nbsp;
		    至&nbsp;<input type="text" class="input-text required size-S" id="MaxCostDate">&nbsp;            
		    状态&nbsp;<select class="input-text required size-S" style="width:80px" id="Status" data-valid="" data-error="">
                        <option value='' selected>请选择</option>
                        <option value='OPEN'>新建</option>
                        <option value='APPROVE'>已审核</option>
		              </select>&nbsp;
		    <button type="submit" class="btn btn-success radius" onclick="select()"><i class="Hui-iconfont">&#xe665;</i> 检索</button>
            <input type="hidden" id="selectKey" />
	    </div>
	    <div class="mt-5" id="outerlist">
	    <%=list %>
	    </div>
    </div>
    <div id="edit" class="pt-5 pr-20 pb-5 pl-20" style="display:none;">
      <div class="form form-horizontal" id="editlist">
      </div>
      <div class="form form-horizontal">
        <div class="row cl">
        <div class="formControls col-8" style="margin-left:30px;"><a href="javascript:;" onclick="cancel()" class="btn btn-primary radius"><i class="Hui-iconfont">&#xe66b;</i> 返回</a></div>
        </div>
      </div>
    </div>
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
                    $("#list").css("display", "");
                    $("#edit").css("display", "none");
                    $("#selectKey").val("");
                    reflist();
                }
            }
            if (vjson.type == "detail") {
                if (vjson.flag == "1") {
                    $("#editlist").html(vjson.liststr);
                    $("#list").css("display", "none");
                    $("#edit").css("display", "");
                }
                else {
                    layer.alert("数据操作出错！");
                }
            }
            if (vjson.type == "save") {
                if (vjson.flag == "1") {
                    layer.msg("保存成功！", { icon: 3, time: 500 });
                }
                else if (vjson.flag == "3") {
                    layer.alert("当前记录已复核，不能修改费用信息！");
                }
                else {
                    layer.alert("数据操作出错！");
                }
            }
            if (vjson.type == "approve") {
                if (vjson.flag == "1") {
                    $("#outerlist").html(vjson.liststr);
                    $("#selectKey").val("");
                    reflist();
                }
                else if (vjson.flag == "3") {
                    layer.alert("当前记录已复核，不能审核/反审！");
                }
                else {
                    layer.alert("数据操作出错！");
                }
            }
        }

        function detail() {
            if ($("#selectKey").val() == "") {
                layer.msg("请先选择一条记录", { icon: 3, time: 1000 });
                return;
            }
            var submitData = new Object();
            submitData.Type = "detail";
            submitData.id = $("#selectKey").val();
            transmitData(datatostr(submitData));
            layer.close(index);
            return;
        }

        function approve() {
            if ($("#selectKey").val() == "") {
                layer.msg("请先选择一条记录", { icon: 3, time: 1000 });
                return;
            }
            layer.confirm('确认要审核/反审吗？', function (index) {
                var submitData = new Object();
                submitData.Type = "approve";
                submitData.id = $("#selectKey").val();
                submitData.MinCostDate = $("#MinCostDate").val();
                submitData.MaxCostDate = $("#MaxCostDate").val();
                submitData.OrderNo = $("#OrderNo").val();
                submitData.Status = $("#Status").val();
                submitData.page = page;
                transmitData(datatostr(submitData));
                layer.close(index);
            });
            return;
        }

        function save(id) {
            var submitData = new Object();
            submitData.Type = "save";
            submitData.id = $("#selectKey").val();
            submitData.detailid = id;
            submitData.Context = $("#Cont"+id).val();
            submitData.Amount = $("#Amt" + id).val();
            transmitData(datatostr(submitData));
            return;
        }
        function cancel() {
            var submitData = new Object();
            submitData.Type = "select";
            submitData.MinCostDate = $("#MinCostDate").val();
            submitData.MaxCostDate = $("#MaxCostDate").val();
            submitData.OrderNo = $("#OrderNo").val();
            submitData.Status = $("#Status").val();
            submitData.page = page;
            transmitData(datatostr(submitData));
            return;
        }

        function select() {
            var submitData = new Object();
            submitData.Type = "select";
            submitData.MinCostDate = $("#MinCostDate").val();
            submitData.MaxCostDate = $("#MaxCostDate").val();
            submitData.OrderNo = $("#OrderNo").val();
            submitData.Status = $("#Status").val();
            submitData.page = 1;
            transmitData(datatostr(submitData));
            return;
        }

        var page = 1;
        function jump(pageindex) {
            page = pageindex;
            var submitData = new Object();
            submitData.Type = "select";
            submitData.MinCostDate = $("#MinCostDate").val();
            submitData.MaxCostDate = $("#MaxCostDate").val();
            submitData.OrderNo = $("#OrderNo").val();
            submitData.Status = $("#Status").val();
            submitData.page = page;
            transmitData(datatostr(submitData));
            return;
        }

        jQuery(function () {
            var MinCostDate = new JsInputDate("MinCostDate");
            MinCostDate.setDisabled(false);
            MinCostDate.setHeight("26px");
            MinCostDate.setWidth("100px");
            MinCostDate.setValue(getfirstday());

            var MaxCostDate = new JsInputDate("MaxCostDate");
            MaxCostDate.setDisabled(false);
            MaxCostDate.setHeight("26px");
            MaxCostDate.setWidth("100px");
            MaxCostDate.setValue(gettoday());
        });

        var trid = "";
        reflist();
</script>
</body>
</html>