<%@ Page Language="C#" AutoEventWireup="true" Inherits="project.Presentation.Platform.UserInfo,project"  %>
<!DOCTYPE>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="head1" runat="server">
    <title>用户管理</title>
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
    <nav class="breadcrumb"><i class="Hui-iconfont">&#xe67f;</i> 首页 <span class="c-gray en">&gt;</span> 系统管理 <span class="c-gray en">&gt;</span> 用户管理 <a class="btn btn-success radius r mr-20" style="line-height:1.6em;margin-top:2px" href="javascript:location.replace(location.href);" title="刷新" ><i class="Hui-iconfont">&#xe68f;</i></a></nav>
    <div id="list" class="pt-5 pr-20 pb-5 pl-20">
	    <div class="cl pd-5 bg-1 bk-gray mt-5"> 
		    用户姓名&nbsp;<input type="text" class="input-text" style="width:150px" placeholder="用户姓名" id="UserNameS" name="">&nbsp;
		    <button type="submit" class="btn btn-success radius" onclick="select()"><i class="Hui-iconfont">&#xe665;</i> 检索</button>
	    </div>
	    <div class="cl pd-5 bg-1 bk-gray mt-5"> 
            <span class="l">
                <a href="javascript:;" onclick="insert()" class="btn btn-primary radius"><i class="Hui-iconfont">&#xe600;</i> 添加用户</a>
                <a href="javascript:;" onclick="update()" class="btn btn-primary radius"><i class="Hui-iconfont">&#xe60c;</i> 修改</a> 
                <a href="javascript:;" onclick="del()" class="btn btn-danger radius"><i class="Hui-iconfont">&#xe6e2;</i> 删除</a> 
                <a href="javascript:;" onclick="newpassword()" class="btn btn-primary radius"><i class="Hui-iconfont">&#xe615;</i> 生成新密码</a>
                <a href="javascript:;" onclick="valid()" class="btn btn-primary radius"><i class="Hui-iconfont">&#xe615;</i> 启用(停止)</a>
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
          <label class="form-label col-2"><span class="c-red">*</span>用户ID：</label>
          <div class="formControls col-4">
            <input type="text" class="input-text required" placeholder="用户ID" id="UserNo" data-valid="isNonEmpty||between:3-16" data-error="用户ID不能为空||用户ID长度为3-16位" />
          </div>
            <div class="col-3"></div>
        </div>
        <div class="row cl">
          <label class="form-label col-2"><span class="c-red">*</span>用户姓名：</label>
          <div class="formControls col-4">
            <input type="text" class="input-text required" placeholder="用户姓名" id="UserName" data-valid="isNonEmpty||between:1-16" data-error="用户姓名不能为空||用户姓名长度为1-16位"/>
          </div>
          <div class="col-3"></div>
        </div>
        <div class="row cl">
          <label class="form-label col-2"><span class="c-red">*</span>用户类型：</label>
          <div class="formControls col-4">
              <%=userType %>
          </div>
          <div class="col-3"></div>
        </div>
        <div class="row cl">
          <label class="form-label col-2"><span class="c-red">*</span>部门：</label>
          <div class="formControls col-4">
              <%=dept %>
          </div>
          <div class="col-3"></div>
        </div>
        <div class="row cl">
          <label class="form-label col-2">上级经理：</label>
          <div class="formControls col-7">
              <input type="text" class="input-text upload-url" id="ManagerName" readonly style="width:450px;" />
              <a href="javascript:void();" class="btn btn-primary radius upload-btn" onclick="ChooseBasic('Manager', 'user')" >选择</a>
              <input type="hidden" id="Manager" />
          </div>
        </div>
        <div class="row cl">
          <label class="form-label col-2">手机：</label>
          <div class="formControls col-4">
            <input type="text" class="input-text required" placeholder="手机" id="Tel" data-valid="between:0-30" data-error="手机长度为0-30位"/>
          </div>
          <div class="col-3"></div>
        </div>
        <div class="row cl">
          <label class="form-label col-2">邮箱：</label>
          <div class="formControls col-4">
            <input type="text" class="input-text required" placeholder="邮箱" id="Email" data-valid="between:0-30" data-error="邮箱长度为0-30位"/>
          </div>
          <div class="col-3"></div>
        </div>
        <div class="row cl">
          <label class="form-label col-2">地址：</label>
          <div class="formControls col-5">
            <textarea cols="" rows="" class="textarea required" placeholder="地址" id="Addr" data-valid="between:0-500" data-error="地址长度为0-500位"></textarea>
          </div>
          <div class="col-2"></div>
        </div>
        <div class="row cl">
          <label class="form-label col-2">照片：</label>
          <div class="formControls col-5">
            <img id="PictureImg" src="" width="120" height="120" alt="" title="" />
            <input type="hidden" id="Picture" /><br /><br />
            <input type="button" class="btn btn-primary radius" value="上传照片" onclick="showUpload('Picture')" />
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
                    showMsg("UserNo", "此用户ID已存在","1");
                }
                else {
                    layer.msg("数据操作出错！");
                }
                return;
            }
            if (vjson.type == "update") {
                if (vjson.flag == "1") {
                    $("#UserNo").val(vjson.UserNo);
                    $("#UserName").val(vjson.UserName);
                    $("#UserType").val(vjson.UserType);
                    $("#DeptNo").val(vjson.DeptNo);
                    $("#Tel").val(vjson.Tel);
                    $("#Email").val(vjson.Email);
                    $("#Addr").val(vjson.Addr);
                    $("#Manager").val(vjson.Manager);
                    $("#ManagerName").val(vjson.ManagerName);
                    $("#Picture").val(vjson.Picture);
                    $("#PictureImg").attr("src", "../../Images/UserPic/" + vjson.Picture);

                    $("#UserNo").attr("disabled", true);
                    $("#list").css("display", "none");
                    $("#edit").css("display", "");
                }
                else {
                    layer.alert("数据操作出错！");
                }
                return;
            }
            if (vjson.type == "newpassword") {
                if (vjson.flag == "1") {
                    layer.alert("新密码生产成功！\r\n新密码为：" + vjson.newpassword);
                }
                else {
                    layer.alert("生成密码出现异常！");
                }
                return;
            }
            if (vjson.type == "valid") {
                if (vjson.flag == "1") {
                    $("#" + vjson.id).find(".td-status").html(vjson.stat);
                    layer.alert(vjson.stat + "成功！");
                }
                else if (vjson.flag == "3") {
                    layer.alert("超级管理员不能被停用！");
                }
                else {
                    layer.alert("停止(启用)出现异常！");
                }
                return;
            }
        }

        function insert() {
            $('#editlist').validate('reset');
            id = "";
            type = "insert";
            $("#UserNo").attr("disabled", false);
            $("#UserNo").val("");
            $("#UserName").val("");
            $("#UserType").val("");
            $("#DeptNo").val("");
            $("#Tel").val("");
            $("#Email").val("");
            $("#Addr").val("");
            $("#Manager").val("");
            $("#ManagerName").val("");
            $("#Picture").val("");
            $("#PictureImg").attr("src", "");

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
                submitData.UserNo = $("#UserNo").val();
                submitData.UserName = $("#UserName").val();
                submitData.UserType = $("#UserType").val();
                submitData.DeptNo = $("#DeptNo").val();
                submitData.Tel = $("#Tel").val();
                submitData.Email = $("#Email").val();
                submitData.Addr = $("#Addr").val();
                submitData.Manager = $("#Manager").val();
                submitData.Picture = $("#Picture").val();

                submitData.tp = type;
                submitData.UserNameS = $("#UserNameS").val();
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
                submitData.UserNameS = $("#UserNameS").val();
                transmitData(datatostr(submitData));
                layer.close(index);
            });
            return;
        }
        function select() {
            var submitData = new Object();
            submitData.Type = "select";
            submitData.UserNameS = $("#UserNameS").val();
            transmitData(datatostr(submitData));
            return;
        }        
        function newpassword(){
            if ($("#selectKey").val() == "")
            {
                layer.msg("请先选择要重新生成密码的信息！");
                return;
            }
             
            layer.confirm('你确定重新生成密码吗？', function (index) {
                var submitData = new Object();
                submitData.Type = "newpassword";
                submitData.id = $("#selectKey").val();

                submitData.UserNameS = $("#UserNameS").val();
                transmitData(datatostr(submitData));
            });
            return  ;
        } 
        function valid(){
            if ($("#selectKey").val() == "") {
                layer.msg("请先选择要停止(启用)的信息！");
                return;
            }
            
            layer.confirm('你确定停止(启用)吗？', function (index) {
                var submitData = new Object();
                submitData.Type = "valid";
                submitData.id = $("#selectKey").val();

                transmitData(datatostr(submitData));
            });
            return;
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

        function showUpload(id) {
            var temp = "../Base/upload.html?action=uploadpic&id=" + id;
            layer_show("文件上传", temp, 400, 200);
        }
        function showPic(name, id) {
            $("#" + id + "Img").attr("src", "../../Images/UserPic/" + name);
            $("#" + id).val(name);
            layer.closeAll();
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
        },tiptype="1");

        var trid = "";
        reflist();
</script>
</body>
</html>