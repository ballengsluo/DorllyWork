<%@ Page Language="C#" Inherits="project.Presentation.login,project" %>
<!DOCTYPE HTML>
<html>
<head>
<meta name="renderer" content="webkit|ie-comp|ie-stand">
<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
<meta name="viewport" content="width=device-width,initial-scale=1,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
<meta http-equiv="Cache-Control" content="no-siteapp" />
    <!--[if lt IE 9]>
    <script type="text/javascript" src="lib/html5.js"></script>
    <script type="text/javascript" src="lib/respond.min.js"></script>
    <![endif]-->
    <link href="css/H-ui.min.css" rel="stylesheet" type="text/css" />
    <link href="css/H-ui.login.css" rel="stylesheet" type="text/css" />
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <link href="lib/iconfont/iconfont.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        function login() {
            if ($("#UserNo").val() == "") {
                alert('用户名不能为空！');
                $("#UserNo").focus();
                return;
            }

            if ($("#Password").val() == "") {
                alert('密码不能为空！');
                $("#Password").focus();
                return;
            }
            doLogin();
        }        
    </script>
<title>通用工单系统</title>
</head>
<body>
<input type="hidden" id="TenantId" name="TenantId" value="" />
<div class="header"><div style="float:left; color:white;font-size:24pt; margin-left:10px; margin-top:5px; font-family:'Microsoft YaHei'">通用工单系统</div></div>
<div class="loginWraper">
  <div id="loginform" class="loginBox">
    <form class="form form-horizontal">
      <div class="row cl"></div>
      <div class="row cl">
        <label class="form-label col-3"><i class="Hui-iconfont">&#xe60d;</i></label>
        <div class="formControls col-8">
          <input id="UserNo" name="" type="text" placeholder="用户名" class="input-text size-L" value="Admin">
        </div>
      </div>
      <div class="row cl"></div>
      <div class="row cl">
        <label class="form-label col-3"><i class="Hui-iconfont">&#xe60e;</i></label>
        <div class="formControls col-8">
          <input id="Password" name="" type="password" placeholder="密码" class="input-text size-L" value="admin1">
        </div>
      </div>
      <div class="row cl"></div>
      <div class="row">
        <div class="formControls col-8 col-offset-3">
          <input name="" type="button" class="btn btn-success radius size-L" value="&nbsp;登&nbsp;&nbsp;&nbsp;&nbsp;录&nbsp;" onclick="login()">
        </div>
      </div>
    </form>
  </div>
</div>
<div class="footer">Copyright 轩仑科技有限公司</div>
    <script type="text/javascript" src="jscript/jquery-1.10.2.js"></script> 
    <script type="text/javascript" src="jscript/H-ui.js"></script>
    <script type="text/javascript" src="jscript/script_ajax.js"></script>
    <script type="text/javascript" src="jscript/script_common.js" charset="gb2312"></script>
    <script type="text/javascript" src="jscript/json2.js"></script>
</body>
</html>