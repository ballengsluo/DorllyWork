<%@ Page Language="C#" AutoEventWireup="true" Inherits="project.Presentation.Base.ChooseCust,project"  %>
<!DOCTYPE>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="head1" runat="server">
    <title>选择客户</title>
    <!--[if lt IE 9]>
    <script type="text/javascript" src="../../jscript/html5.js"></script>
    <script type="text/javascript" src="../../jscript/respond.min.js"></script>
    <![endif]-->
    <link href="../../css/H-ui.min.css" rel="stylesheet" type="text/css" />
    <link href="../../css/H-ui.admin.css" rel="stylesheet" type="text/css" />
    <link href="../../lib/iconfont/iconfont.css" rel="stylesheet" type="text/css" />
    <link href="../../css/style.css" rel="stylesheet" type="text/css" />
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
    <div id="list" class="pt-5 pr-20 pb-5 pl-20">  
	    <div class="cl pd-5 bg-1 bk-gray mt-5"> 
		    名称&nbsp;<input type="text" class="input-text" style="width:150px" placeholder="名称" id="Name" name="">&nbsp;
		    地址&nbsp;<input type="text" class="input-text" style="width:150px" placeholder="地址" id="Addr" name="">&nbsp;
		    <button type="submit" class="btn btn-success radius" onclick="select()"><i class="Hui-iconfont">&#xe665;</i> 检索</button>
	    </div>
	    <div class="mt-5" id="outerlist">
	    <%=list %>
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

        function transmitData(submitData) {
            var temp = submitData;
          <%=ClientScript.GetCallbackEventReference(this, "temp", "BandResuleData", null) %>
        }

        function BandResuleData(temp) {
            var vjson = JSON.parse(temp);
            if (vjson.type == "select") {
                if (vjson.flag == "1") {
                    $("#outerlist").html(vjson.liststr);
                    $("#selectKey").val("");
                    reflist();
                    page = 1;
                    return;
                }
            }
            else if (vjson.type == "jump") {
                if (vjson.flag == "1") {
                    $("#outerlist").html(vjson.liststr);
                    $("#selectKey").val("");
                    reflist();
                    return;
                }
            }
        }

        function select() {
            var submitData = new Object();
            submitData.Type = "select";
            submitData.Name = $("#Name").val();
            submitData.Addr = $("#Addr").val();
            submitData.page = 1;
            transmitData(datatostr(submitData));
            return;
        }
        var page = 1;
        function jump(pageindex) {
            page = pageindex;
            var submitData = new Object();
            submitData.Type = "select";
            submitData.Name = $("#Name").val();
            submitData.Addr = $("#Addr").val();
            submitData.page = page;
            transmitData(datatostr(submitData));
            return;
        }

        function cancel() {
            var index = parent.layer.getFrameIndex(window.name);
            parent.layer.close(index);
            return;
        }

        function submit(id) {
            parent.choose("<%=id %>", id, $("#it" + id).val());
            var index = parent.layer.getFrameIndex(window.name);
            parent.layer.close(index);
            return;
        }
    </script>
</body>
</html>