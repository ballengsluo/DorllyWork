<%@ Page Language="C#" AutoEventWireup="true" Inherits="project.Presentation.Order.WorkOrderMsg,project"  %>
<!DOCTYPE>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="head1" runat="server">
    <title>系统消息</title>
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
    <nav class="breadcrumb"><i class="Hui-iconfont">&#xe67f;</i> 首页 <span class="c-gray en">&gt;</span> 系统消息 <a class="btn btn-success radius r mr-20" style="line-height:1.6em;margin-top:2px" href="javascript:location.replace(location.href);" title="刷新" ><i class="Hui-iconfont">&#xe68f;</i></a></nav>
    <div id="list" class="pt-5 pr-20 pb-5 pl-20">
	    <div class="cl pd-5 bg-1 bk-gray mt-5">
            <a href="javascript:;" onclick="read()" class="btn btn-primary radius"><i class="Hui-iconfont">&#xe60c;</i> 标记已读</a>
            <a href="javascript:;" onclick="del()" class="btn btn-danger radius"><i class="Hui-iconfont">&#xe6e2;</i> 删除</a> 
	    </div>
	    <div class="mt-5" id="outerlist">
	    <%=list %>
	    </div>
    </div>
    <div id="edit" class="pt-5 pr-20 pb-5 pl-20" style="display:none;">
      <div class="form form-horizontal" id="editlist">
        <div class="row cl">
          <label class="form-label col-2">标题：</label>
          <div class="formControls col-4">
            <input type="text" class="input-text required" id="Subject" disabled  data-valid="between:0-500" data-error=""/>
          </div>
          <div class="col-3"></div>
        </div>
        <div class="row cl">
          <label class="form-label col-2">内容：</label>
          <div class="formControls col-5">
            <textarea cols="" rows="" class="textarea required" id="Context" disabled data-valid="between:0-3000" data-error=""></textarea>
          </div>
          <div class="col-3"></div>
        </div>
        <div class="row cl">
          <label class="form-label col-2">发送日期：</label>
          <div class="formControls col-4">
            <input type="text" class="input-text required" id="SendDate" disabled  data-valid="between:0-500" data-error=""/>
          </div>
          <div class="col-2"></div>
        </div>
        <div class="row cl">
          <label class="form-label col-2">发送人：</label>
          <div class="formControls col-4">
            <input type="text" class="input-text required" id="Sender" disabled  data-valid="between:0-500" data-error=""/>
          </div>
          <div class="col-2"></div>
        </div>
        <div class="row cl">
          <div class="col-9 col-offset-3">
            <input class="btn btn-primary radius" type="button" onclick="cancel()" id="submit" value=" 确 认 " />
          </div>
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
                    layer.closeAll('loading');
                    $("#outerlist").html(vjson.liststr);
                    return;
                }
            }
            if (vjson.type == "view") {
                if (vjson.flag == "1") {
                    $("#Subject").val(vjson.Subject);
                    $("#Context").val(vjson.Context);
                    $("#SendDate").val(vjson.SendDate);
                    $("#Sender").val(vjson.Sender);

                    $("#outerlist").html(vjson.liststr);
                    $("#list").css("display", "none");
                    $("#edit").css("display", "");
                }
                else {
                    layer.alert("数据操作出错！");
                }
                return;
            }
        }

        function read() {
            var details = "";
            var itemNo = jQuery("#tablelist input:checkbox:checked");
            if (itemNo.length <= 0) {
                layer.msg("请至少选择一条记录", { icon: 3, time: 1000 });
                return;
            }
            for (var i = 0; i < itemNo.length; i++) {
                if (i == itemNo.length)
                    details += itemNo[i].id;
                else
                    details += itemNo[i].id + ";";
            }
            layer.load(2);
            var submitData = new Object();
            submitData.Type = "read";
            submitData.details = details;
            submitData.page = page;
            transmitData(datatostr(submitData));
            return;
        }

        function del() {
            var details = "";
            var itemNo = jQuery("#tablelist input:checkbox:checked");
            if (itemNo.length <= 0) {
                layer.msg("请至少选择一条记录", { icon: 3, time: 1000 });
                return;
            }
            for (var i = 0; i < itemNo.length; i++) {
                if (i == itemNo.length)
                    details += itemNo[i].id;
                else
                    details += itemNo[i].id + ";";
            }
            layer.load(2);
            var submitData = new Object();
            submitData.Type = "delete";
            submitData.details = details;
            submitData.page = page;
            transmitData(datatostr(submitData));
            return;
        }

        function view(id) {
            var submitData = new Object();
            submitData.Type = "view";
            submitData.id = id;
            submitData.page = page;
            transmitData(datatostr(submitData));
            return;
        }

        function cancel() {
            $("#list").css("display", "");
            $("#edit").css("display", "none");
            return;
        }

        function checkall() {
            if ($("#checkAll").is(":checked"))
                $("input[name='chk_list']").prop("checked", true);
            else
                $("input[name='chk_list']").prop("checked", false);
        }

        var page = 1;
        function jump(pageindex) {
            page = pageindex;
            var submitData = new Object();
            submitData.Type = "select";
            submitData.page = page;
            transmitData(datatostr(submitData));
            return;
        }
</script>
</body>
</html>