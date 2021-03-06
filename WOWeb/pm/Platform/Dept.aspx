﻿<%@ Page Language="C#" AutoEventWireup="true" Inherits="project.Presentation.Sys.Dept,project"  %>
<!DOCTYPE>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="head1" runat="server">
    <title>用户部门信息</title>
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
    <nav class="breadcrumb"><i class="Hui-iconfont">&#xe67f;</i> 首页 <span class="c-gray en">&gt;</span> 系统设置 <span class="c-gray en">&gt;</span> 用户部门信息 <a class="btn btn-success radius r mr-20" style="line-height:1.6em;margin-top:2px" href="javascript:location.replace(location.href);" title="刷新" ><i class="Hui-iconfont">&#xe68f;</i></a></nav>
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
          <label class="form-label col-2"><span class="c-red">*</span>部门编号：</label>
          <div class="formControls col-4">
            <input type="text" class="input-text required" placeholder="部门编号" id="DeptNo" data-valid="isNonEmpty||between:1-30" data-error="部门编号不能为空||部门编号长度为0-30位"/>
          </div>
          <div class="col-3"></div>
        </div>
        <div class="row cl">
          <label class="form-label col-2"><span class="c-red">*</span>部门名称：</label>
          <div class="formControls col-4">
            <input type="text" class="input-text required" placeholder="部门名称" id="DeptName" data-valid="isNonEmpty||between:1-50" data-error="部门名称不能为空||部门名称为0-50位"/>
          </div>
          <div class="col-3"></div>
        </div>
        <div class="row cl">
          <label class="form-label col-2">上级部门：</label>
          <div class="formControls col-7">
              <input type="text" class="input-text upload-url" id="ParentName" readonly style="width:450px;" />
              <a href="javascript:void();" class="btn btn-primary radius upload-btn" onclick="ChooseBasic('Parent', 'dept')" >选择</a>
              <input type="hidden" id="Parent" />
          </div>
        </div>
        <div class="row cl">
          <label class="form-label col-2">负责人：</label>
          <div class="formControls col-7">
              <input type="text" class="input-text upload-url" id="ManagerName" readonly style="width:450px;" />
              <a href="javascript:void();" class="btn btn-primary radius upload-btn" onclick="ChooseBasic('Manager', 'user')" >选择</a>
              <input type="hidden" id="Manager" />
          </div>
        </div>
        <div class="row cl">
          <label class="form-label col-2">备注：</label>
          <div class="formControls col-5">
            <textarea cols="" rows="" class="textarea required" placeholder="备注" id="Remark" data-valid="between:0-200" data-error="地址长度为0-200位"></textarea>
          </div>
          <div class="col-2"></div>
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
                    layer.alert("部门已在使用中，不能删除！");
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
                    $("#DeptNo").val(vjson.DeptNo);
                    $("#DeptName").val(vjson.DeptName);
                    $("#Parent").val(vjson.Parent);
                    $("#ParentName").val(vjson.ParentName);
                    $("#Manager").val(vjson.Manager);
                    $("#ManagerName").val(vjson.ManagerName);
                    $("#Remark").val(vjson.Remark);

                    $("#DeptNo").attr("disabled", true);
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
            $("#DeptNo").attr("disabled", false);
            $("#DeptNo").val("");
            $("#DeptName").val("");
            $("#Parent").val("");
            $("#ParentName").val("");
            $("#Manager").val("");
            $("#ManagerName").val("");
            $("#Remark").val("");

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
                submitData.DeptNo = $("#DeptNo").val();
                submitData.DeptName = $("#DeptName").val();
                submitData.Parent = $("#Parent").val();
                submitData.Manager = $("#Manager").val();
                submitData.Remark = $("#Remark").val();

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