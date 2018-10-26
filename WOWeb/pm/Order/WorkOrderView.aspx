<%@ Page Language="C#" AutoEventWireup="true" Inherits="project.Presentation.Order.WorkOrder,project"  %>
<!DOCTYPE>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="head1" runat="server">
    <title>工单查看</title>
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
    <nav class="breadcrumb"><i class="Hui-iconfont">&#xe67f;</i> 首页 <span class="c-gray en">&gt;</span> 工单管理 <span class="c-gray en">&gt;</span> 工单查看 <a class="btn btn-success radius r mr-20" style="line-height:1.6em;margin-top:2px" href="javascript:location.replace(location.href);" title="刷新" ><i class="Hui-iconfont">&#xe68f;</i></a></nav>
    <div id="list" class="pt-5 pr-20 pb-5 pl-20">
	    <div class="cl pd-5 bg-1 bk-gray mt-5"> 
		    工单单号&nbsp;<input type="text" class="input-text size-S" style="width:110px" placeholder="工单号" id="OrderNoS" name="">&nbsp;
		    工单内容&nbsp;<input type="text" class="input-text size-S" style="width:160px" placeholder="工单内容" id="OrderNameS" name="">&nbsp;
		    工单日期&nbsp;<input type="text" class="input-text required size-S" id="MinOrderDate">&nbsp;
		    至&nbsp;<input type="text" class="input-text required size-S" id="MaxOrderDate">&nbsp;
		    工单状态&nbsp;<%=status %>&nbsp;
            是否挂起&nbsp;<select class="input-text required size-S" style="width:70px" id="IsHangUpS" data-valid="" data-error="">
                        <option value='' selected>请选择</option>
                        <option value='true'>已挂起</option>
                        <option value='false'>未挂起</option></select>&nbsp;
		    工单类型&nbsp;<%=orderTypeS %>&nbsp;
            <br />
		    销售单号&nbsp;<input type="text" class="input-text size-S" style="width:110px" placeholder="销售单号" id="SaleNoS" name="">&nbsp;
		    客户名称&nbsp;<input type="text" class="input-text size-S" style="width:110px" readonly id="CustNameS" name="">
              <a href="javascript:void();" class="btn btn-primary radius upload-btn size-S" onclick="ChooseBasic('CustNoS', 'cust')" >选择</a>
              <input type="hidden" id="CustNoS" />&nbsp;
		    处理部门&nbsp;<%=alloDept %>&nbsp;
		    处理人&nbsp;<%=alloUser %>&nbsp;
		    地区&nbsp;<%=region %>&nbsp;
            是否退回&nbsp;<select class="input-text required size-S" style="width:70px" id="IsBackS" data-valid="" data-error="">
                        <option value='' selected>请选择</option>
                        <option value='true'>已退回</option>
                        <option value='false'>未退回</option></select>&nbsp;
		    <button type="submit" class="btn btn-success radius" onclick="select()"><i class="Hui-iconfont">&#xe665;</i> 检索</button>
	    </div>
	    <div class="cl pd-5 bg-1 bk-gray mt-5"> 
            <span class="l">
                <a href="javascript:;" onclick="view()" class="btn btn-primary radius"><i class="Hui-iconfont">&#xe6c6;</i> 查看工单</a> &nbsp;
                <a href="javascript:;" onclick="showimg()" class="btn btn-primary radius"><i class="Hui-iconfont">&#xe613;</i> 查看图片</a> 
                <input type="hidden" id="selectKey" />
            </span> 
	    </div>
	    <div class="mt-5" id="outerlist">
	    <%=list %>
	    </div>
    </div>
    <div id="edit" class="pt-5 pr-20 pb-5 pl-20" style="display:none;">
      <div class="form form-horizontal" id="editlist">
        <div class="row cl" style="line-height:20px;"><div class="col-10 c-blue" style="font-weight:bold;">客户工单内容</div></div>
        <div class="row cl" style="line-height:20px;"><div class="col-10 line"></div></div>
        <div class="row cl">
          <label class="form-label col-2"><span class="c-red">*</span>工单号：</label>
          <div class="formControls col-2">
            <input type="text" class="input-text required" id="OrderNo" disabled  data-valid="between:0-30" data-error=""/>
          </div>
          <div class="col-3"></div>
        </div>
        <div class="row cl">
          <label class="form-label col-2"><span class="c-red">*</span>客户名称：</label>
          <div class="formControls col-4">
              <input type="text" class="input-text required upload-url" id="CustName" readonly style="width:380px;" data-valid="isNonEmpty||between:1-200" data-error="客户名称不能为空||客户名称长度为1-200位" />
              <a href="javascript:void();" class="btn btn-primary radius upload-btn" onclick="ChooseBasic('CustNo', 'cust')" >选择</a>
              <input type="hidden" id="CustNo" />
          </div>
          <div class="col-3"></div>
        </div>
        <div class="row cl">
          <label class="form-label col-2"><span class="c-red">*</span>工单内容：</label>
          <div class="formControls col-4">
            <input type="text" class="input-text required" placeholder="工单内容" id="OrderName" data-valid="isNonEmpty||between:1-200" data-error="工单内容不能为空||工单内容长度为1-200位"/>
          </div>
          <div class="col-3"></div>
        </div>
        <div class="row cl">
          <label class="form-label col-2"><span class="c-red">*</span>工单类型：</label>
          <div class="formControls col-2">
              <%=orderType %>
          </div>
          <div class="col-3"></div>
        </div>
        <div class="row cl">
          <label class="form-label col-2">要求服务时间：</label>
          <div class="formControls col-4">
            <input type="text" class="input-text required" placeholder="要求服务时间" id="CustneedTime" data-valid="between:0-30" data-error=""/>&nbsp;
            <select class="input-text required" style="width:75px" id="CustneedHour" data-valid="between:0-2" data-error="请选择时间">
                <option value="00">00</option><option value="01">01</option><option value="02">02</option><option value="03">03</option>
                <option value="04">04</option><option value="05">05</option><option value="06">06</option><option value="07">07</option>
                <option value="08">08</option><option value="09">09</option><option value="10">10</option><option value="11">11</option>
                <option value="12">12</option><option value="13">13</option><option value="14">14</option><option value="15">15</option>
                <option value="16">16</option><option value="17">17</option><option value="18">18</option><option value="19">19</option>
                <option value="20">20</option><option value="21">21</option><option value="22">22</option><option value="23">23</option>
            </select>&nbsp;
            <select class="input-text required" style="width:75px" id="CustneedMinute" data-valid="between:0-2" data-error="请选择时间">
                <option value="00">00</option><option value="01">01</option><option value="02">02</option><option value="03">03</option>
                <option value="04">04</option><option value="05">05</option><option value="06">06</option><option value="07">07</option>
                <option value="08">08</option><option value="09">09</option><option value="10">10</option><option value="11">11</option>
                <option value="12">12</option><option value="13">13</option><option value="14">14</option><option value="15">15</option>
                <option value="16">16</option><option value="17">17</option><option value="18">18</option><option value="19">19</option>
                <option value="20">20</option><option value="21">21</option><option value="22">22</option><option value="23">23</option>
                <option value="24">24</option><option value="25">25</option><option value="26">26</option><option value="27">27</option>
                <option value="28">28</option><option value="29">29</option><option value="30">30</option><option value="31">31</option>
                <option value="32">32</option><option value="33">33</option><option value="34">34</option><option value="35">35</option>
                <option value="36">36</option><option value="37">37</option><option value="38">38</option><option value="39">39</option>
                <option value="40">40</option><option value="41">41</option><option value="42">42</option><option value="43">43</option>
                <option value="44">44</option><option value="45">45</option><option value="46">46</option><option value="47">47</option>
                <option value="48">48</option><option value="49">49</option><option value="50">50</option><option value="51">51</option>
                <option value="52">52</option><option value="53">53</option><option value="54">54</option><option value="55">55</option>
                <option value="56">56</option><option value="57">57</option><option value="58">58</option><option value="59">59</option>
            </select>&nbsp;
          </div>
          <div class="col-3"></div>
        </div>
        <div class="row cl">
          <label class="form-label col-2">联系人：</label>
          <div class="formControls col-2">
            <input type="text" class="input-text required" placeholder="联系人" id="LinkMan" data-valid="between:0-30" data-error="联系人长度为0-30位"/>
          </div>
          <div class="col-3"></div>
        </div>
        <div class="row cl">
          <label class="form-label col-2">联系人电话：</label>
          <div class="formControls col-4">
            <input type="text" class="input-text required" placeholder="联系人电话" id="LinkTel" data-valid="between:0-30" data-error="联系人电话长度为0-30位"/>
          </div>
          <div class="col-3"></div>
        </div>
        <div class="row cl">
          <label class="form-label col-2">地区：</label>
          <div class="formControls col-3">
              <input type="text" class="input-text upload-url" id="RegionName" placeholder="地区" readonly style="width:215px;" />
              <a href="javascript:void();" class="btn btn-primary radius upload-btn" onclick="ChooseBasic('Region', 'region')" >选择</a>
              <input type="hidden" id="Region" />
          </div>
        </div>
        <div class="row cl">
          <label class="form-label col-2">地址：</label>
          <div class="formControls col-5">
            <textarea cols="" rows="" class="textarea required" placeholder="地址" id="Addr" data-valid="between:0-500" data-error="地址长度为0-500位"></textarea>
          </div>
          <div class="col-2"></div>
        </div>
        <div class="row cl" style="line-height:20px;"><div class="col-10 c-blue" style="font-weight:bold;">客服处理内容</div></div>
        <div class="row cl" style="line-height:20px;"><div class="col-10 line"></div></div>
        <div class="row cl">
          <label class="form-label col-2">销售订单号：</label>
          <div class="formControls col-2">
            <input type="text" class="input-text required" placeholder="销售订单号" id="SaleNo" data-valid="between:0-30" data-error="销售订单号长度为0-30位"/>
          </div>
          <div class="col-3"></div>
        </div>
        <div class="row cl">
          <label class="form-label col-2"><span class="c-red">*</span>工单生效日期：</label>
          <div class="formControls col-4">
            <input type="text" class="input-text required" placeholder="工单日期" id="OrderDate" data-valid="between:1-30" data-error="请选择工单日期"/>&nbsp;
            <select class="input-text required" style="width:75px" id="OrderHour" data-valid="between:0-2" data-error="请选择时间">
                <option value="00">00</option><option value="01">01</option><option value="02">02</option><option value="03">03</option>
                <option value="04">04</option><option value="05">05</option><option value="06">06</option><option value="07">07</option>
                <option value="08">08</option><option value="09">09</option><option value="10">10</option><option value="11">11</option>
                <option value="12">12</option><option value="13">13</option><option value="14">14</option><option value="15">15</option>
                <option value="16">16</option><option value="17">17</option><option value="18">18</option><option value="19">19</option>
                <option value="20">20</option><option value="21">21</option><option value="22">22</option><option value="23">23</option>
            </select>&nbsp;
            <select class="input-text required" style="width:75px" id="OrderMinute" data-valid="between:0-2" data-error="请选择时间">
                <option value="00">00</option><option value="01">01</option><option value="02">02</option><option value="03">03</option>
                <option value="04">04</option><option value="05">05</option><option value="06">06</option><option value="07">07</option>
                <option value="08">08</option><option value="09">09</option><option value="10">10</option><option value="11">11</option>
                <option value="12">12</option><option value="13">13</option><option value="14">14</option><option value="15">15</option>
                <option value="16">16</option><option value="17">17</option><option value="18">18</option><option value="19">19</option>
                <option value="20">20</option><option value="21">21</option><option value="22">22</option><option value="23">23</option>
                <option value="24">24</option><option value="25">25</option><option value="26">26</option><option value="27">27</option>
                <option value="28">28</option><option value="29">29</option><option value="30">30</option><option value="31">31</option>
                <option value="32">32</option><option value="33">33</option><option value="34">34</option><option value="35">35</option>
                <option value="36">36</option><option value="37">37</option><option value="38">38</option><option value="39">39</option>
                <option value="40">40</option><option value="41">41</option><option value="42">42</option><option value="43">43</option>
                <option value="44">44</option><option value="45">45</option><option value="46">46</option><option value="47">47</option>
                <option value="48">48</option><option value="49">49</option><option value="50">50</option><option value="51">51</option>
                <option value="52">52</option><option value="53">53</option><option value="54">54</option><option value="55">55</option>
                <option value="56">56</option><option value="57">57</option><option value="58">58</option><option value="59">59</option>
            </select>&nbsp;
          </div>
          <div class="col-3"></div>
        </div>
        <div class="row cl">
          <label class="form-label col-2">执行部门：</label>
          <div class="formControls col-3">
              <input type="text" class="input-text upload-url" id="AlloDeptName" readonly style="width:218px;" />
              <a href="javascript:void();" class="btn btn-primary radius upload-btn" onclick="ChooseBasic('AlloDept', 'dept')" >选择</a>
              <input type="hidden" id="AlloDept" />
          </div>
        </div>
        <div class="row cl">
          <label class="form-label col-2">部门负责人：</label>
          <div class="formControls col-2">
              <input type="text" class="input-text required" id="AlloUserName" readonly  data-valid="between:0-300" data-error=""/>
              <input type="hidden" id="AlloUser" />
          </div>
          <div class="col-3"></div>
        </div>
        <div class="row cl">
          <label class="form-label col-2">执行人：</label>
          <div class="formControls col-7">
              <input type="text" class="input-text upload-url" id="TreatUserName" readonly style="width:380px;" />
              <a href="javascript:void();" class="btn btn-primary radius upload-btn" onclick="ChooseBasicCheck('TreatUser', 'user')" >选择</a>
              <input type="hidden" id="TreatUser" />
          </div>
        </div>
        <div class="row cl">
          <div class="col-9 col-offset-3">
            <input class="btn btn-primary radius" type="button" onclick="submit()" id="submit" value="&nbsp;&nbsp;提&nbsp;&nbsp;交&nbsp;&nbsp;" />
			<input class="btn btn-default radius" type="button" onclick="cancel()" id="cancel" value="&nbsp;&nbsp;取&nbsp;&nbsp;消&nbsp;&nbsp;" />
          </div>
        </div>
      </div>
    </div>
    <div id="imgdiv" class="pt-5 pr-20 pd-5 pl-20" style="display:none;">
      <div class="form form-horizontal" id="imglist">
      </div>
      <div class="form form-horizontal">
        <div class="row cl">
        <div class="formControls col-8" style="margin-left:30px;"><a href="javascript:;" onclick="cancelimg()" class="btn btn-primary radius"><i class="Hui-iconfont">&#xe66b;</i> 返回</a></div>
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
                    showMsg("CustNo", "此客户编号已存在", "1");
                }
                else {
                    layer.alert("数据操作出错！");
                }
                return;
            }
            if (vjson.type == "update") {
                if (vjson.flag == "1") {
                    $("#OrderNo").val(vjson.OrderNo);
                    $("#OrderName").val(vjson.OrderName);
                    $("#OrderType").val(vjson.OrderType);
                    $("#OrderDate").val(vjson.OrderDate);
                    $("#SaleNo").val(vjson.SaleNo);
                    $("#CustNo").val(vjson.CustNo);
                    $("#CustName").val(vjson.CustName);
                    $("#Region").val(vjson.Region);
                    $("#RegionName").val(vjson.RegionName);
                    $("#LinkMan").val(vjson.LinkMan);
                    $("#LinkTel").val(vjson.LinkTel);
                    $("#Addr").val(vjson.Addr);
                    $("#AlloDept").val(vjson.AlloDept);
                    $("#AlloDeptName").val(vjson.AlloDeptName);
                    $("#AlloUser").val(vjson.AlloUser);
                    $("#AlloUserName").val(vjson.AlloUserName);
                    $("#TreatUser").val(vjson.Person);
                    $("#TreatUserName").val(vjson.PersonName);
                    $("#OrderHour").val(vjson.OrderHour);
                    $("#OrderMinute").val(vjson.OrderMinute);
                    $("#CustneedTime").val(vjson.CustneedTime);
                    $("#CustneedHour").val(vjson.CustneedHour);
                    $("#CustneedMinute").val(vjson.CustneedMinute);

                    $("#editlist ").find("*").each(function () {
                        if (this.id != "cancel") $(this).removeAttr("disabled");
                    });
                    $("#submit").css("display", "");
                    $("#OrderNo").attr("disabled", true);
                    $("#list").css("display", "none");
                    $("#edit").css("display", "");
                }
                else if (vjson.flag == "3") {
                    layer.alert("当前工单非制单状态，不能修改！");
                }
                else {
                    layer.alert("数据操作出错！");
                }
                return;
            }
            if (vjson.type == "view") {
                if (vjson.flag == "1") {
                    $("#OrderNo").val(vjson.OrderNo);
                    $("#OrderName").val(vjson.OrderName);
                    $("#OrderType").val(vjson.OrderType);
                    $("#OrderDate").val(vjson.OrderDate);
                    $("#SaleNo").val(vjson.SaleNo);
                    $("#CustNo").val(vjson.CustNo);
                    $("#CustName").val(vjson.CustName);
                    $("#Region").val(vjson.Region);
                    $("#RegionName").val(vjson.RegionName);
                    $("#LinkMan").val(vjson.LinkMan);
                    $("#LinkTel").val(vjson.LinkTel);
                    $("#Addr").val(vjson.Addr);
                    $("#AlloDept").val(vjson.AlloDept);
                    $("#AlloDeptName").val(vjson.AlloDeptName);
                    $("#AlloUser").val(vjson.AlloUser);
                    $("#AlloUserName").val(vjson.AlloUserName);
                    $("#TreatUser").val(vjson.Person);
                    $("#TreatUserName").val(vjson.PersonName);
                    $("#OrderHour").val(vjson.OrderHour);
                    $("#OrderMinute").val(vjson.OrderMinute);
                    $("#CustneedTime").val(vjson.CustneedTime);
                    $("#CustneedHour").val(vjson.CustneedHour);
                    $("#CustneedMinute").val(vjson.CustneedMinute);
                    
                    $("#editlist ").find("*").each(function () {
                        if(this.id != "cancel") $(this).attr("disabled",true);
                    });
                    $("#submit").css("display", "none");
                    $("#list").css("display", "none");
                    $("#edit").css("display", "");
                }
                else {
                    layer.alert("数据操作出错！");
                }
                return;
            }
            if (vjson.type == "img") {
                if (vjson.flag == "1") {
                    $("#imglist").html(vjson.liststr);
                    $("#list").css("display", "none");
                    $('#imgdiv').css("display", "");
                }
                else {
                    layer.alert("数据操作出错！");
                }
                return;
            }
            if (vjson.type == "confirm") {
                if (vjson.flag == "1") {
                    $("#outerlist").html(vjson.liststr);
                    $("#selectKey").val("");
                    reflist();
                }
                else if (vjson.flag == "3") {
                    layer.alert(vjson.info);
                }
                else {
                    layer.alert("数据操作出错！");
                }
                return;
            }
            if (vjson.type == "allodept") {
                if (vjson.flag == "1") {
                    $("#AlloUser").val(vjson.AlloUser);
                    $("#AlloUserName").val(vjson.AlloUserName);
                }
                else {
                    layer.alert("数据操作出错！");
                }
                return;
            }
        }

        function insert() {
            $('#editlist').validate('reset');
            type = "insert";
            $("#editlist ").find("*").each(function () {
                if (this.id != "cancel") $(this).removeAttr("disabled");
            });
            $("#submit").css("display", "");
            $("#OrderNo").attr("disabled", true);

            $("#OrderNo").val("");
            $("#OrderName").val("");
            $("#OrderType").val("");
            $("#OrderDate").val("");
            $("#SaleNo").val("");
            $("#CustNo").val("");
            $("#CustName").val("");
            $("#Region").val("");
            $("#RegionName").val("");
            $("#LinkMan").val("");
            $("#LinkTel").val("");
            $("#Addr").val("");
            $("#AlloDept").val("");
            $("#AlloDeptName").val("");
            $("#AlloUser").val("");
            $("#AlloUserName").val("");
            $("#TreatUser").val("");
            $("#TreatUserName").val("");

            var now = new Date();
            var hour = now.getHours();
            var minu = now.getMinutes();
            if (hour < 10) hour = "0" + hour;
            if (minu < 10) minu = "0" + minu;

            $("#OrderDate").val(gettoday());
            $("#OrderHour").val(hour);
            $("#OrderMinute").val(minu);
            $("#CustneedTime").val(gettoday());
            $("#CustneedHour").val(hour);
            $("#CustneedMinute").val(minu);

            $("#list").css("display", "none");
            $("#edit").css("display", "");
            return;
        }
        function update() {
            if ($("#selectKey").val() == "") {
                layer.msg("请先选择一条记录", { icon: 3, time: 1000 });
                return;
            }
            $('#editlist').validate('reset');
            type = "update";
            var submitData = new Object();
            submitData.Type = "update";
            submitData.id = $("#selectKey").val();

            transmitData(datatostr(submitData));
            return;
        }
        function view() {
            if ($("#selectKey").val() == "") {
                layer.msg("请先选择一条记录", { icon: 3, time: 1000 });
                return;
            }
            $('#editlist').validate('reset');
            var submitData = new Object();
            submitData.Type = "view";
            submitData.id = $("#selectKey").val();
            transmitData(datatostr(submitData));
            return;
        }
        function submit() {
            if ($('#editlist').validate('submitValidate')) {
                var submitData = new Object();
                submitData.Type = "submit";
                submitData.id = $("#selectKey").val();
                submitData.OrderNo = $("#OrderNo").val();
                submitData.OrderName = $("#OrderName").val();
                submitData.OrderType = $("#OrderType").val();
                submitData.OrderDate = $("#OrderDate").val();
                submitData.SaleNo = $("#SaleNo").val();
                submitData.CustNo = $("#CustNo").val();
                submitData.LinkMan = $("#LinkMan").val();
                submitData.LinkTel = $("#LinkTel").val();
                submitData.Addr = $("#Addr").val();
                submitData.AlloDept = $("#AlloDept").val();
                submitData.AlloUser = $("#AlloUser").val();
                submitData.TreatUser = $("#TreatUser").val();
                submitData.Region = $("#Region").val();
                submitData.OrderHour = $("#OrderHour").val();
                submitData.OrderMinute = $("#OrderMinute").val(); 
                submitData.CustneedTime = $("#CustneedTime").val();
                submitData.CustneedHour = $("#CustneedHour").val();
                submitData.CustneedMinute = $("#CustneedMinute").val();

                submitData.tp = type;
                submitData.OrderNoS = $("#OrderNoS").val();
                submitData.OrderNameS = $("#OrderNameS").val();
                submitData.OrderTypeS = $("#OrderTypeS").val();
                submitData.MinOrderDate = $("#MinOrderDate").val();
                submitData.MaxOrderDate = $("#MaxOrderDate").val();
                submitData.StatusS = $("#StatusS").val();
                submitData.IsHangUpS = $("#IsHangUpS").val();
                submitData.IsBackS = $("#IsBackS").val();
                submitData.SaleNoS = $("#SaleNoS").val();
                submitData.CustNoS = $("#CustNoS").val();
                submitData.AlloDeptS = $("#AlloDeptS").val();
                submitData.AlloUserS = $("#AlloUserS").val();
                submitData.RegionS = $("#RegionS").val();
                submitData.page = page;
                transmitData(datatostr(submitData));
            }
            return;
        }
        function del() {
            if ($("#selectKey").val() == "") {
                layer.msg("请先选择一条记录", { icon: 3, time: 1000 });
                return;
            }
            layer.confirm('确认要删除吗？', function (index) {
                var submitData = new Object();
                submitData.Type = "delete";
                submitData.id = $("#selectKey").val();
                submitData.OrderNoS = $("#OrderNoS").val();
                submitData.OrderNameS = $("#OrderNameS").val();
                submitData.OrderTypeS = $("#OrderTypeS").val();
                submitData.MinOrderDate = $("#MinOrderDate").val();
                submitData.MaxOrderDate = $("#MaxOrderDate").val();
                submitData.StatusS = $("#StatusS").val();
                submitData.IsHangUpS = $("#IsHangUpS").val();
                submitData.IsBackS = $("#IsBackS").val();
                submitData.SaleNoS = $("#SaleNoS").val();
                submitData.CustNoS = $("#CustNoS").val();
                submitData.AlloDeptS = $("#AlloDeptS").val();
                submitData.AlloUserS = $("#AlloUserS").val();
                submitData.RegionS = $("#RegionS").val();
                submitData.page = page;
                transmitData(datatostr(submitData));
                layer.close(index);
            });
            return;
        }
        function confirm() {
            if ($("#selectKey").val() == "") {
                layer.msg("请先选择一条记录", { icon: 3, time: 1000 });
                return;
            }
            layer.confirm('确定要确认销单吗？', function (index) {
                var submitData = new Object();
                submitData.Type = "confirm";
                submitData.id = $("#selectKey").val();

                submitData.OrderNoS = $("#OrderNoS").val();
                submitData.OrderNameS = $("#OrderNameS").val();
                submitData.OrderTypeS = $("#OrderTypeS").val();
                submitData.MinOrderDate = $("#MinOrderDate").val();
                submitData.MaxOrderDate = $("#MaxOrderDate").val();
                submitData.StatusS = $("#StatusS").val();
                submitData.IsHangUpS = $("#IsHangUpS").val();
                submitData.IsBackS = $("#IsBackS").val();
                submitData.SaleNoS = $("#SaleNoS").val();
                submitData.CustNoS = $("#CustNoS").val();
                submitData.AlloDeptS = $("#AlloDeptS").val();
                submitData.AlloUserS = $("#AlloUserS").val();
                submitData.RegionS = $("#RegionS").val();
                submitData.page = page;
                transmitData(datatostr(submitData));
                layer.close(index);
            });
            return;
        }
        function cost() {
            if ($("#selectKey").val() == "") {
                layer.msg("请先选择一条记录", { icon: 3, time: 1000 });
                return;
            }
            var submitData = new Object();
            submitData.Type = "cost";
            submitData.id = $("#selectKey").val();
            transmitData(datatostr(submitData));
            layer.close(index);
            return;
        }
        function select() {
            var submitData = new Object();
            submitData.Type = "select";
            submitData.OrderNoS = $("#OrderNoS").val();
            submitData.OrderNameS = $("#OrderNameS").val();
            submitData.OrderTypeS = $("#OrderTypeS").val();
            submitData.MinOrderDate = $("#MinOrderDate").val();
            submitData.MaxOrderDate = $("#MaxOrderDate").val();
            submitData.StatusS = $("#StatusS").val();
            submitData.IsHangUpS = $("#IsHangUpS").val();
            submitData.IsBackS = $("#IsBackS").val();
            submitData.SaleNoS = $("#SaleNoS").val();
            submitData.CustNoS = $("#CustNoS").val();
            submitData.AlloDeptS = $("#AlloDeptS").val();
            submitData.AlloUserS = $("#AlloUserS").val();
            submitData.RegionS = $("#RegionS").val();
            submitData.page = 1;
            transmitData(datatostr(submitData));
            return;
        }
        function cancel() {
            $("#list").css("display", "");
            $("#edit").css("display", "none");
            return;
        }
        var page = 1;
        function jump(pageindex) {
            page = pageindex;
            var submitData = new Object();
            submitData.Type = "select";
            submitData.OrderNoS = $("#OrderNoS").val();
            submitData.OrderNameS = $("#OrderNameS").val();
            submitData.OrderTypeS = $("#OrderTypeS").val();
            submitData.MinOrderDate = $("#MinOrderDate").val();
            submitData.MaxOrderDate = $("#MaxOrderDate").val();
            submitData.StatusS = $("#StatusS").val();
            submitData.IsHangUpS = $("#IsHangUpS").val();
            submitData.IsBackS = $("#IsBackS").val();
            submitData.SaleNoS = $("#SaleNoS").val();
            submitData.CustNoS = $("#CustNoS").val();
            submitData.AlloDeptS = $("#AlloDeptS").val();
            submitData.AlloUserS = $("#AlloUserS").val();
            submitData.RegionS = $("#RegionS").val();
            submitData.page = page;
            transmitData(datatostr(submitData));
            return;
        }

        function showimg() {
            if ($("#selectKey").val() == "") {
                layer.msg("请先选择一条记录", { icon: 3, time: 1000 });
                return;
            }
            var submitData = new Object();
            submitData.Type = "img";
            submitData.id = $("#selectKey").val();
            transmitData(datatostr(submitData));
            return;
        }

        function cancelimg() {
            $("#list").css("display", "");
            $('#imgdiv').css("display", "none");
            return;
        }

        function adduser() {
            if ($("#selectKey").val() == "") {
                layer.msg("请先选择一条记录", { icon: 3, time: 1000 });
                return;
            }
            var temp = "../Base/ChooseBasicCheck.aspx?id=AddUser&type=user";
            layer_show("选择页面", temp, 560, 480);
            return;
        }

        function choose(id, labels, values) {
            if (labels != "" && labels != undefined && labels != "undefined") {
                if (id == "CustNo") {
                    $("#CustName").val(values);
                    $("#CustNo").val(labels);
                }
                else if (id == "CustNoS") {
                    $("#CustNameS").val(values);
                    $("#CustNoS").val(labels);
                }
                else if (id == "AddUser") {
                    var submitData = new Object();
                    submitData.Type = "adduser";
                    submitData.id = $("#selectKey").val();
                    submitData.labels = labels;
                    transmitData(datatostr(submitData));
                }
                else if (id == "AlloDept") {
                    $("#" + id + "Name").val(values);
                    $("#" + id).val(labels);

                    var submitData = new Object();
                    submitData.Type = "allodept";
                    submitData.DeptNo = labels;
                    transmitData(datatostr(submitData));
                }
                else{
                    $("#" + id + "Name").val(values);
                    $("#" + id).val(labels);
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

        jQuery(function () {
            var MinOrderDate = new JsInputDate("MinOrderDate");
            MinOrderDate.setDisabled(false);
            MinOrderDate.setHeight("26px");
            MinOrderDate.setWidth("100px");
            MinOrderDate.setValue(getfirstday());

            var MaxOrderDate = new JsInputDate("MaxOrderDate");
            MaxOrderDate.setDisabled(false);
            MaxOrderDate.setHeight("26px");
            MaxOrderDate.setWidth("100px");
            MaxOrderDate.setValue(gettoday());

            var OrderDate = new JsInputDate("OrderDate");
            OrderDate.setDisabled(false);
            OrderDate.setHeight("30px");
            OrderDate.setWidth("180px");

            var OrderDate = new JsInputDate("CustneedTime");
            CustneedTime.setDisabled(false);
            CustneedTime.setHeight("30px");
            CustneedTime.setWidth("180px");
        });
        
        var trid = "";
        reflist();
</script>
</body>
</html>