<%@ Page Language="C#" AutoEventWireup="true" Inherits="project.Presentation.Sys.UserType,project"  %>
<!DOCTYPE>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="head1" runat="server">
    <title>用户类别设置</title>
    <!--[if lt IE 9]>
    <script type="text/javascript" src="../../jscript/html5.js"></script>
    <script type="text/javascript" src="../../jscript/respond.min.js"></script>
    <![endif]-->
    <link href="../../css/H-ui.min.css" rel="stylesheet" type="text/css" />
    <link href="../../css/H-ui.admin.css" rel="stylesheet" type="text/css" />
    <link href="../../lib/iconfont/iconfont.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function transmitData(submitData) {
            var data = submitData;
            <%=ClientScript.GetCallbackEventReference(this, "data", "BandResuleData", null) %>
        }
    </script>
</head>
<body>
    <form id="form1" runat="server"></form>
    <div>
    <nav class="breadcrumb"><i class="Hui-iconfont">&#xe67f;</i> 首页 <span class="c-gray en">&gt;</span> 系统设置 <span class="c-gray en">&gt;</span> 用户类别设置 <a class="btn btn-success radius r mr-20" style="line-height:1.6em;margin-top:2px" href="javascript:location.replace(location.href);" title="刷新" ><i class="Hui-iconfont">&#xe68f;</i></a></nav>
    <div id="list" class="pt-5 pr-20 pb-5 pl-20">  
        <div class="cl pd-5 bg-1 bk-gray mt-5"> 
            <span class="l">
                <a href="javascript:;" onclick="insert()" class="btn btn-primary radius"><i class="Hui-iconfont">&#xe600;</i> 添加</a>
                <a href="javascript:;" onclick="update()" class="btn btn-primary radius"><i class="Hui-iconfont">&#xe60c;</i> 修改</a> 
                <a href="javascript:;" onclick="del()" class="btn btn-danger radius"><i class="Hui-iconfont">&#xe6e2;</i> 删除</a> 
                <input type="hidden" id="selectKey" />
            </span> 
	    </div>
	    <div class="mt-5" id="outerlist">
	    <%=list %>
	    </div>
    </div>
    <div id="edit" class="pt-5 pr-20 pb-5 pl-20" style="display:none;">
      <div class="form form-horizontal" id="editlist">
        <div class="row cl">
          <label class="form-label col-2"><span class="c-red">*</span>用户类型编号：</label>
          <div class="formControls col-4">
            <input type="text" class="input-text required" placeholder="用户类型编号" id="UserTypeNo" data-valid="isNonEmpty||between:1-30" data-error="用户类型编号不能为空||用户类型编号长度为0-30位"/>
          </div>
          <div class="col-3"></div>
        </div>
        <div class="row cl">
          <label class="form-label col-2"><span class="c-red">*</span>用户类型名称：</label>
          <div class="formControls col-4">
            <input type="text" class="input-text required" placeholder="用户类型名称" id="UserTypeName" data-valid="isNonEmpty||between:1-50" data-error="用户类型编号不能为空||用户类型编号为0-50位"/>
          </div>
          <div class="col-3"></div>
        </div>
        <div class="row cl">
          <label class="form-label col-2">处理工单类型：</label>
          <div class="formControls col-7">
              <input type="text" class="input-text upload-url" id="OrderTypeName" readonly style="width:450px;" />
              <a href="javascript:void();" class="btn btn-primary radius upload-btn" onclick="ChooseBasicCheck('OrderType', 'ordertype')" >选择</a>
              <input type="hidden" id="OrderType" />
          </div>
        </div>
        <div class="row cl">
          <div class="col-9 col-offset-3">
            <input class="btn btn-primary radius" type="button" onclick="submit()" value="&nbsp;&nbsp;提&nbsp;&nbsp;交&nbsp;&nbsp;" />
			<input class="btn btn-default radius" type="button" onclick="cancel()" value="&nbsp;&nbsp;取&nbsp;&nbsp;消&nbsp;&nbsp;" />
          </div>
        </div>
      </div>
    </div>
    </div>
    
    <script type="text/javascript" src="../../jscript/jquery-1.10.2.js"></script> 
    <script type="text/javascript" src="../../jscript/script_ajax.js"></script>
    <script type="text/javascript" src="../../jscript/script_common.js" charset="gb2312"></script>
    <script type="text/javascript" src="../../jscript/json2.js"></script>
    <script type="text/javascript" src="../../jscript/H-ui.js"></script> 
    <script type="text/javascript" src="../../jscript/H-ui.admin.js"></script> 
    <script type="text/javascript" src="../../lib/layer/layer.js"></script> 
    <script type="text/javascript" src="../../lib/validate/jquery.validate.js"></script>
    <script type="text/javascript">
        function BandResuleData(temp) {
            var vjson = JSON.parse(temp);
            if (vjson.type == "select") {
                if (vjson.flag == "1") {
                    $("#outerlist").html(vjson.liststr);
                    $("#selectKey").val("");
                    reflist();
                }
            }
            if (vjson.type == "delete") {
                if (vjson.flag == "1") {
                    $("#outerlist").html(vjson.liststr);
                    $("#selectKey").val("");
                    reflist();
                }
                else if (vjson.flag == "3") {
                    layer.alert("用户类型已使用，不能删除！");
                }
                else if (vjson.flag == "4") {
                    layer.alert("管理员类型不能删除！");
                }
                else {
                    layer.alert("数据操作出错！");
                }
                return;
            }
            if (vjson.type == "submit") {
                if (vjson.flag == "1") {
                    $("#outerlist").html(vjson.liststr);
                    $("#selectKey").val("");
                    $("#list").css("display", "");
                    $("#edit").css("display", "none");
                    reflist();
                }
                else if (vjson.flag == "3") {
                    showMsg("UserTypeNo", "用户类型在系统已存在", "1");
                }
                else {
                    layer.msg("数据操作出错！");
                }
                return;
            }
            if (vjson.type == "update") {
                if (vjson.flag == "1") {
                    $("#UserTypeNo").val(vjson.UserTypeNo);
                    $("#UserTypeName").val(vjson.UserTypeName);
                    $("#OrderType").val(vjson.OrderType);
                    $("#OrderTypeName").val(vjson.OrderTypeName);

                    $("#UserTypeNo").attr("disabled", true);
                    $("#list").css("display", "none");
                    $("#edit").css("display", "");
                }
                else {
                    layer.alert("数据操作出错！");
                }
                return;
            }
        }

        function insert() {
            $('#editlist').validate('reset');
            id = "";
            type = "insert";
            $("#UserTypeNo").attr("disabled", false);
            $("#UserTypeNo").val("");
            $("#UserTypeName").val("");
            $("#OrderType").val("");
            $("#OrderTypeName").val("");

            $("#list").css("display", "none");
            $("#edit").css("display", "");
            return;
        }
        function update() {
            if ($("#selectKey").val() == "") {
                layer.msg("请先选择要修改的信息", { icon: 3, time: 1000 });
                return;
            }
            $('#editlist').validate('reset');

            id = $("#selectKey").val();
            type = "update";
            var submitData = new Object();
            submitData.Type = "update";
            submitData.id = id;

            transmitData(datatostr(submitData));
            return;
        }
        function submit() {
            if ($('#editlist').validate('submitValidate')) {
                var submitData = new Object();
                submitData.Type = "submit";
                submitData.id = id;
                submitData.UserTypeNo = $("#UserTypeNo").val();
                submitData.UserTypeName = $("#UserTypeName").val();
                submitData.OrderType = $("#OrderType").val();
                submitData.OrderTypeName = $("#OrderTypeName").val();

                submitData.tp = type;
                transmitData(datatostr(submitData));
            }
            return;
        }
        function del() {
            if ($("#selectKey").val() == "") {
                layer.msg("请先选择要删除的信息", { icon: 3, time: 1000 });
                return;
            }
            layer.confirm('确认要删除吗？', function (index) {
                var submitData = new Object();
                submitData.Type = "delete";
                submitData.id = $("#selectKey").val();
                transmitData(datatostr(submitData));
                layer.close(index);
            });
        }
        function select() {
            var submitData = new Object();
            submitData.Type = "select";
            transmitData(datatostr(submitData));
        }
        function cancel() {
            id = "";
            $("#list").css("display", "");
            $("#edit").css("display", "none");
            return;
        }

        function choose(id, labels, values) {
            if (labels != "" && labels != undefined && labels != "undefined") {
                $("#" + id + "Name").val(values);
                $("#" + id).val(labels);
            }
        }
        
        $('#editlist').validate({
            onFocus: function () {
                this.parent().addClass('active');
                return false;
            },
            onBlur: function () {
                var $parent = this.parent();
                var _status = parseInt(this.attr('data-status'));
                $parent.removeClass('active');
                if (!_status) {
                    $parent.addClass('error');
                }
                return false;
            }
        }, tiptype = "1");


        var trid = "";
        reflist();
</script>
</body>
</html>