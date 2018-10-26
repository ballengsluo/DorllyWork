<%@ Page Language="C#" AutoEventWireup="true" Inherits="project.Presentation.index,project"  %>
<!DOCTYPE HTML>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>通用工单系统 V1.0</title>
    <!--[if lt IE 9]>
    <script type="text/javascript" src="jscript/html5.js"></script>
    <script type="text/javascript" src="jscript/respond.min.js"></script>
    <![endif]-->
    <link href="css/H-ui.min.css" rel="stylesheet" type="text/css" />
    <link href="css/H-ui.admin.css" rel="stylesheet" type="text/css" />
    <link href="skin/blue/skin.css" rel="stylesheet" type="text/css" id="skin" />
    <link href="lib/iconfont/iconfont.css" rel="stylesheet" type="text/css" />
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function transmitData(submitData) {
            var data = submitData;
            <%=ClientScript.GetCallbackEventReference(this, "data", "BandResuleData", null) %>
        }
    </script>
</head>
<body>
    <form id="form1" runat="server"></form>
    <header class="Hui-header cl"> <a class="Hui-logo l" title="通用工单系统 V1.0" href="/">通用工单系统 </a> <span class="Hui-subtitle l">V1.0</span>
	    <ul class="Hui-userbar">
		    <li><%=user.Entity.UserTypeName %>&nbsp;</li>
		    <li class="dropDown dropDown_hover"><a href="#" class="dropDown_A"><%=user.Entity.UserName %> <i class="Hui-iconfont">&#xe6d5;</i></a>
			    <ul class="dropDown-menu radius box-shadow">
				    <li><a href="javascript:void(0)" onclick="myinfo()">个人信息</a></li>
				    <li><a href="javascript:void(0)" onclick="swap()">切换账户</a></li>
					<li><a href="javascript:void(0)" onclick="quit()">退出</a></li>
			    </ul>
		    </li>
		    <li id="Hui-msg"> <a  href="javascript:void(0)" id="mymessage" title="我的消息"><span class="badge badge-danger" id="msgnum"><%=msgnum.ToString() %></span><i class="Hui-iconfont" style="font-size:18px">&#xe68a;</i></a> </li>
		    <li id="Hui-skin" class="dropDown right dropDown_hover"><a href="javascript:;" title="换肤"><i class="Hui-iconfont" style="font-size:18px">&#xe62a;</i></a>
			    <ul class="dropDown-menu radius box-shadow">
				    <li><a href="javascript:;" data-val="blue" title="默认（蓝色）">默认（蓝色）</a></li>
				    <li><a href="javascript:;" data-val="default" title="黑色">黑色</a></li>
				    <li><a href="javascript:;" data-val="green" title="绿色">绿色</a></li>
				    <li><a href="javascript:;" data-val="red" title="红色">红色</a></li>
				    <li><a href="javascript:;" data-val="yellow" title="黄色">黄色</a></li>
				    <li><a href="javascript:;" data-val="orange" title="绿色">橙色</a></li>
			    </ul>
		    </li>
	    </ul>
	    <a aria-hidden="false" class="Hui-nav-toggle" href="#"></a> </header>
    <aside class="Hui-aside">
	    <input runat="server" id="divScrollValue" type="hidden" value="" />
	    <div class="menu_dropdown bk_2">
            <%=menulist %>
	    </div>
    </aside>
    <div class="dislpayArrow"><a class="pngfix" href="javascript:void(0);" onclick="displaynavbar(this)"></a></div>
    <section class="Hui-article-box">
	    <div id="Hui-tabNav" class="Hui-tabNav">
		    <div class="Hui-tabNav-wp">
			    <ul id="min_title_list" class="acrossTab cl">
				    <li class="active"><span title="我的桌面" data-href="main.aspx">我的桌面</span><em></em></li>
			    </ul>
		    </div>
		    <div class="Hui-tabNav-more btn-group"><a id="js-tabNav-prev" class="btn radius btn-default size-S" href="javascript:;"><i class="Hui-iconfont">&#xe6d4;</i></a><a id="js-tabNav-next" class="btn radius btn-default size-S" href="javascript:;"><i class="Hui-iconfont">&#xe6d7;</i></a></div>
	    </div>
	    <div id="iframe_box" class="Hui-article">
		    <div class="show_iframe">
			    <div style="display:none" class="loading"></div>
			    <iframe scrolling="yes" frameborder="0" src="main.aspx"></iframe>
		    </div>
	    </div>
    </section>
    <script type="text/javascript" src="jscript/jquery-1.10.2.js"></script> 
    <script type="text/javascript" src="jscript/H-ui.js"></script> 
    <script type="text/javascript" src="jscript/H-ui.admin.js"></script> 
    <script type="text/javascript" src="jscript/script_ajax.js"></script>
    <script type="text/javascript" src="jscript/script_common.js" charset="gb2312"></script>
    <script type="text/javascript" src="jscript/json2.js"></script>
    <script type="text/javascript">
        function BandResuleData(temp) {
            var vjson = JSON.parse(temp);
            if (vjson.type == "getmsg") {
                if (vjson.flag == "1") {
                    $("#msgnum").html(vjson.msgnum);
                    window.setTimeout("msg()", 10 * 1000);
                    return;
                }
            }
        }

        $("#mymessage").on("click", function () {
            var bStop = false;
            var bStopIndex = 0;
            var _href = "pm/order/WorkOrderMsg.aspx";
            var _titleName = "我的消息";
            var topWindow = $(window.parent.document);
            var show_navLi = topWindow.find("#min_title_list li");
            show_navLi.each(function () {
                if ($(this).find('spa').attr("data-href") == _href) {
                    bStop = true;
                    bStopIndex = show_navLi.index($(this));
                    return false;
                }
            });
            
            if (!bStop) {
                var topWindow = $(window.parent.document);
                var show_nav = topWindow.find('#min_title_list');
                show_nav.find('li').removeClass("active");
                var iframe_box = topWindow.find('#iframe_box');
                show_nav.append('<li class="active"><span data-href="' + _href + '">' + _titleName + '</span><i></i><em></em></li>');
                tabNavallwidth();
                var iframeBox = iframe_box.find('.show_iframe');
                iframeBox.hide();
                iframe_box.append('<div class="show_iframe"><div class="loading"></div><iframe frameborder="0" src=' + _href + '></iframe></div>');
                var showBox = iframe_box.find('.show_iframe:visible');
                showBox.find('iframe').attr("src", _href).load(function () {
                    showBox.find('.loading').hide();
                });
                var topWindow = $(window.parent.document);
                var show_nav = topWindow.find("#min_title_list");
                var aLi = show_nav.find("li");
            }
            else {
                show_navLi.removeClass("active").eq(bStopIndex).addClass("active");
                var iframe_box = topWindow.find("#iframe_box");
                iframe_box.find(".show_iframe").hide().eq(bStopIndex).show();
            }
        });

        function swap() {
            clearCookie(ajaxresponse("type=3&lg=1"));
            window.location = "login.aspx";
        }

        function quit() {
            clearCookie(ajaxresponse("type=3&lg=1"));
            window.close();
        }

        function msg() {
            var submitData = new Object();
            submitData.Type = "getmsg";
            transmitData(datatostr(submitData));
        }

        function minute() {
            window.setTimeout("minute()", 5 * 60 * 1000);
            setCookie(ajaxresponse("type=3&lg=1"), getCookie(ajaxresponse("type=3&lg=1")), 6);
        }

        jQuery(function () { minute(); });

    </script>
</body>
</html>