<%@ Page Language="C#" AutoEventWireup="true" Inherits="project.Presentation.Base.Dict_Bak,project"  %>
<!DOCTYPE>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="head1" runat="server">
    <title>数据资料管理</title>
    <!--[if lt IE 9]>
    <script type="text/javascript" src="../../jscript/html5.js"></script>
    <script type="text/javascript" src="../../jscript/respond.min.js"></script>
    <![endif]-->
    <link href="../../css/H-ui.min.css" rel="stylesheet" type="text/css" />
    <link href="../../css/H-ui.admin.css" rel="stylesheet" type="text/css" />
    <link href="../../lib/iconfont/iconfont.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../../lib/zTree/css/zTreeStyle/zTreeStyle.css" type="text/css">
    <script type="text/javascript">
        function transmitData(submitData) {
            var data = submitData;
            <%=ClientScript.GetCallbackEventReference(this, "data", "BandResuleData", null) %>
        }
    </script>
</head>
<body>
    <form id="form1" runat="server"></form>
    <div class="pos-a" style="width:150px;left:0;top:0; bottom:0; height:100%; border-right:1px solid #e5e5e5; background-color:#f5f5f5">
	<ul id="ztree" class="ztree"></ul>
    </div>	  
    <div style="margin-left:150px;">
    <nav class="breadcrumb"><i class="Hui-iconfont">&#xe67f;</i> 首页 <span class="c-gray en">&gt;</span> 基础资料 <span class="c-gray en">&gt;</span> 数据字典管理 <a class="btn btn-success radius r mr-20" style="line-height:1.6em;margin-top:2px" href="javascript:location.replace(location.href);" title="刷新" ><i class="Hui-iconfont">&#xe68f;</i></a></nav>
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
          <label class="form-label col-2"><span class="c-red">*</span>编号：</label>
          <div class="formControls col-4">
            <input type="text" class="input-text required" placeholder="编号" id="DictNo" data-valid="isNonEmpty||between:1-30" data-error="编号不能为空||编号长度为0-30位"/>
          </div>
          <div class="col-3"></div>
        </div>
        <div class="row cl">
          <label class="form-label col-2"><span class="c-red">*</span>名称：</label>
          <div class="formControls col-4">
            <input type="text" class="input-text required" placeholder="名称" id="DictName" data-valid="isNonEmpty||between:1-50" data-error="名称不能为空||名称为0-50位"/>
          </div>
          <div class="col-3"></div>
        </div>
        <div class="row cl">
          <label class="form-label col-2">备注：</label>
          <div class="formControls col-5">
            <textarea cols="" rows="" class="textarea required" placeholder="备注" id="Remark" data-valid="between:0-500" data-error="备注长度为0-500位"></textarea>
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
    <script type="text/javascript" src="../../lib/zTree/js/jquery.ztree.all-3.5.min.js"></script> 
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
                    showMsg("DictNo", "编号在系统已存在", "1");
                }
                else {
                    layer.msg("数据操作出错！");
                }
                return;
            }
            if (vjson.type == "update") {
                if (vjson.flag == "1") {
                    $("#DictNo").val(vjson.DictNo);
                    $("#DictName").val(vjson.DictName);
                    $("#Remark").val(vjson.Remark);

                    $("#DictNo").attr("disabled", true);
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
            $("#DictNo").attr("disabled", false);
            $("#DictNo").val("");
            $("#DictName").val("");
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
            submitData.DictType = dictType;

            transmitData(datatostr(submitData));
            return;
        }
        function submit() {
            if ($('#editlist').validate('submitValidate')) {
                var submitData = new Object();
                submitData.Type = "submit";
                submitData.id = id;
                submitData.DictNo = $("#DictNo").val();
                submitData.DictName = $("#DictName").val();
                submitData.Remark = $("#Remark").val();

                submitData.tp = type;
                submitData.DictType = dictType;
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
                submitData.DictType = dictType;
                transmitData(datatostr(submitData));
                layer.close(index);
            });
        }
        function select() {
            var submitData = new Object();
            submitData.Type = "select";
            submitData.DictType = dictType;
            transmitData(datatostr(submitData));
        }
        function cancel() {
            id = "";
            $("#list").css("display", "");
            $("#edit").css("display", "none");
            return;
        }

        var setting = {
            view: {
                dblClickExpand: false,
                showLine: false,
                selectedMulti: false
            },
            data: {
                simpleData: {
                    enable: true,
                    idKey: "id",
                    pIdKey: "pId",
                    rootPId: ""
                }
            },
            callback: {
                beforeClick: function (treeId, treeNode) {
                    var zTree = $.fn.zTree.getZTreeObj("tree");
                    if (treeNode.isParent) {
                        zTree.expandNode(treeNode);
                        return false;
                    } else {
                        dictType = treeNode.id;
                        select();
                        return true;
                    }
                }
            }
        };
        var dictType = "UserType";
        var zNodes = [
            { id: 0, pId: 0, name: "数据字典", open: true },
            { id: "UserType", pId: 0, name: "用户类型" },
            { id: "CustomType", pId: 0, name: "客户类型" }
        ];

        $(document).ready(function () {
            var t = $("#ztree");
            t = $.fn.zTree.init(t, setting, zNodes);
            var zTree = $.fn.zTree.getZTreeObj("ztree");
            zTree.selectNode(zTree.getNodeByParam("id", 'UserType'));
        });

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