<%@ Page Language="C#" AutoEventWireup="true" Inherits="project.Presentation.Platform.ChangePwd,project"  %>
<!DOCTYPE>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="head1" runat="server">
    <title>修改密码</title>
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
    <nav class="breadcrumb"><i class="Hui-iconfont">&#xe67f;</i> 首页 <span class="c-gray en">&gt;</span> 系统管理 <span class="c-gray en">&gt;</span> 修改密码 <a class="btn btn-success radius r mr-20" style="line-height:1.6em;margin-top:2px" href="javascript:location.replace(location.href);" title="刷新" ><i class="Hui-iconfont">&#xe68f;</i></a></nav>
    <div id="edit" class="pt-5 pr-20 pb-5 pl-20">
      <div class="form form-horizontal" id="editlist">
        <div class="row cl">
          <label class="form-label col-2"><span class="c-red">*</span>旧密码：</label>
          <div class="formControls col-4">
            <input type="password" class="input-text required" placeholder="旧密码" id="OldPwd" data-valid="isNonEmpty||between:6-16" data-error="密码不能为空||密码长度为6-16位" />
          </div>
            <div class="col-3"></div>
        </div>
        <div class="row cl">
          <label class="form-label col-2"><span class="c-red">*</span>新密码：</label>
          <div class="formControls col-4">
            <input type="password" class="input-text required" placeholder="新密码" id="NewPwd" data-valid="isNonEmpty||between:6-16" data-error="密码不能为空||密码长度为6-16位"/>
          </div>
          <div class="col-3"></div>
        </div>
        <div class="row cl">
          <label class="form-label col-2"><span class="c-red">*</span>确认密码：</label>
          <div class="formControls col-4">
            <input type="password" class="input-text required" placeholder="确认密码" id="NewPwd1" data-valid="isNonEmpty||between:6-16" data-error="密码不能为空||密码长度为6-16位"/>
          </div>
          <div class="col-3"></div>
        </div>
        <div class="row cl">
          <div class="col-9 col-offset-3">
            <input class="btn btn-primary radius" type="button" onclick="submit()" value="&nbsp;&nbsp;修&nbsp;&nbsp;改&nbsp;&nbsp;" />
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
            if (vjson.type == "submit") {
                if (vjson.flag == "1") {
                    layer.msg("密码修改成功！", { icon: 3, time: 3000 });
                    location.replace(location.href);
                }
                else if (vjson.flag == "3") {
                    showMsg("OldPwd", "密码输入错误", "1");
                }
                else {
                    layer.msg("修改密码出现异常！");
                }
                return;
            }
        }

        function submit() {
            if ($('#editlist').validate('submitValidate')) {
                if ($("#NewPwd").val() != $("#NewPwd1").val()) {
                    showMsg("NewPwd1", "密码输入不一致", "1");
                    return;
                }
                var submitData = new Object();
                submitData.Type = "submit";
                submitData.oldpwd = $("#OldPwd").val();
                submitData.newpwd = $("#NewPwd").val();
                submitData.UserNo = $("#UserNo").val();
                submitData.UserName = $("#UserName").val();

                transmitData(datatostr(submitData));
            }
            return;
        }
        function cancel() {
            id = "";
            $("#list").css("display", "");
            $("#edit").css("display", "none");
            return;
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
</script>
</body>
</html>