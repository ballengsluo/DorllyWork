<%@ Page Language="C#" AutoEventWireup="true" Inherits="project.Presentation.Base.FlowNode,project"  %>
<!DOCTYPE>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="head1" runat="server">
    <title>节点的操作设置</title>
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
        <nav class="breadcrumb"><i class="Hui-iconfont">&#xe67f;</i> 首页 <span class="c-gray en">&gt;</span> 基础资料 <span class="c-gray en">&gt;</span> 节点的操作设置 <a class="btn btn-success radius r mr-20" style="line-height:1.6em;margin-top:2px" href="javascript:location.replace(location.href);" title="刷新" ><i class="Hui-iconfont">&#xe68f;</i></a></nav>
        <div id="list" class="pt-5 pr-20 pb-5 pl-20">  
            <div class="cl pd-5 bg-1 bk-gray mt-5"> 
                <span class="l">
                    <a href="javascript:;" onclick="update()" class="btn btn-primary radius"><i class="Hui-iconfont">&#xe60c;</i> 修改</a> 
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
          <label class="form-label col-2"><span class="c-red">*</span>流程编号：</label>
          <div class="formControls col-4">
            <input type="text" class="input-text required" placeholder="流程编号" id="NodeNo" data-valid="isNonEmpty||between:1-30" data-error="流程编号不能为空||流程编号长度为1-30位"/>
          </div>
          <div class="col-3"></div>
        </div>
        <div class="row cl">
          <label class="form-label col-2"><span class="c-red">*</span>流程名称：</label>
          <div class="formControls col-4">
            <input type="text" class="input-text required" placeholder="流程名称" id="NodeName" data-valid="isNonEmpty||between:1-50" data-error="流程名称不能为空||流程名称为1-50位"/>
          </div>
          <div class="col-3"></div>
        </div>
        <div class="row cl">
          <label class="form-label col-2">流程操作：</label>
          <div class="formControls col-7">
              <input type="text" class="input-text upload-url" id="OpName" readonly style="width:380px;" />
              <a href="javascript:void();" class="btn btn-primary radius upload-btn" onclick="ChooseBasicCheck('OpNo', 'op')" >选择</a>
              <input type="hidden" id="OpNo" />
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
            if (vjson.type == "submit") {
                if (vjson.flag == "1") {
                    $("#outerlist").html(vjson.liststr);
                    $("#selectKey").val("");
                    $("#list").css("display", "");
                    $("#edit").css("display", "none");
                    reflist();
                }
                else if (vjson.flag == "3") {
                    showMsg("FlowNo", "流程在系统已存在", "1");
                }
                else {
                    layer.alert("数据操作出错！");
                }
                return;
            }
            if (vjson.type == "update") {
                if (vjson.flag == "1") {
                    $("#NodeNo").val(vjson.NodeNo);
                    $("#NodeName").val(vjson.NodeName);
                    $("#OpNo").val(vjson.OpNo);
                    $("#OpName").val(vjson.OpName);

                    $("#NodeNo").attr("disabled", true);
                    $("#NodeName").attr("disabled", true);
                    $("#list").css("display", "none");
                    $("#edit").css("display", "");
                }
                else {
                    layer.alert("数据操作出错！");
                }
                return;
            }
        }

        function update() {
            if ($("#selectKey").val() == "") {
                layer.msg("请先选择一条记录", { icon: 3, time: 1000 });
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
                submitData.OpNo = $("#OpNo").val();
                submitData.OpName = $("#OpName").val();

                submitData.tp = type;
                transmitData(datatostr(submitData));
            }
            return;
        }
        function select() {
            var submitData = new Object();
            submitData.Type = "select";
            transmitData(datatostr(submitData));
        }
        function cancel() {
            id = "";
            $("#edit").css("display", "none");
            $("#list").css("display", "");
            return;
        }

        function choose(id, labels, values) {
            if (labels != undefined && labels != "undefined") {
                if (id == "OpNo") {
                    $("#OpNo").val(labels);
                    $("#OpName").val(values);
                }
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