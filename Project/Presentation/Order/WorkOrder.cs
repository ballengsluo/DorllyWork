using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Net.Json;
using NPOI.HSSF.UserModel;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace project.Presentation.Order
{
    public partial class WorkOrder : AbstractPmPage, System.Web.UI.ICallbackEventHandler
    {
        protected string userid = "";
        Business.Sys.BusinessUserInfo user = new project.Business.Sys.BusinessUserInfo();
        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                HttpCookie hc = getCookie("1");
                if (hc != null)
                {
                    string str = hc.Value.Replace("%3D", "=");
                    userid = Encrypt.DecryptDES(str, "1");
                    user.load(userid);

                    if (!Page.IsCallback)
                    {
                        CheckRight(user.Entity, "Order/WorkOrder.aspx");

                        list = createList(string.Empty, string.Empty, GetDate().AddDays(-GetDate().Day + 1).ToString("yyyy-MM-dd"), GetDate().ToString("yyyy-MM-dd"),
                                string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "", "", 1);

                        orderType = "<select class=\"input-text required\" id=\"OrderType\" data-valid=\"isNonEmpty\" data-error=\"请选择工单类型\">";
                        orderType += "<option value=\"\" selected>请选择工单类型</option>";

                        orderTypeS = "<select class=\"input-text required\" id=\"OrderTypeS\" style=\"width:100px\">";
                        orderTypeS += "<option value=\"\" selected>全部</option>";
                        Business.Base.BusinessOrderType type = new project.Business.Base.BusinessOrderType();
                        foreach (Entity.Base.EntityOrderType it in type.GetOrderTypeListQuery(string.Empty, string.Empty, string.Empty, user.Entity.AccID))
                        {
                            orderType += "<option value='" + it.OrderTypeNo + "'>" + it.OrderTypeName + "</option>";
                            orderTypeS += "<option value='" + it.OrderTypeNo + "'>" + it.OrderTypeName + "</option>";
                        }
                        orderType += "</select>";
                        orderTypeS += "</select>";

                        alloDept = "<select class=\"input-text required size-S\" id=\"AlloDeptS\" style=\"width:100px\" data-valid=\"isNonEmpty\" data-error=\"请选择\">";
                        alloDept += "<option value=\"\" selected>请选择</option>";
                        Business.Sys.BusinessDept allodt = new project.Business.Sys.BusinessDept();
                        foreach (Entity.Sys.EntityDept it in allodt.GetDeptListQuery(string.Empty, string.Empty, user.Entity.AccID, string.Empty))
                        {
                            alloDept += "<option value='" + it.DeptNo + "'>" + it.DeptName + "</option>";
                        }
                        alloDept += "</select>";

                        alloUser = "<select class=\"input-text required size-S\" id=\"AlloUserS\" style=\"width:100px\" data-valid=\"isNonEmpty\" data-error=\"请选择\">";
                        alloUser += "<option value=\"\" selected>请选择</option>";
                        Business.Sys.BusinessUserInfo allous = new project.Business.Sys.BusinessUserInfo();
                        foreach (Entity.Sys.EntityUserInfo it in allous.GetUserInfoListQuery(string.Empty, user.Entity.AccID, string.Empty, string.Empty))
                        {
                            alloUser += "<option value='" + it.UserNo + "'>" + it.UserName + "</option>";
                        }
                        alloUser += "</select>";

                        region = "<select class=\"input-text required size-S\" id=\"RegionS\" style=\"width:80px\" data-valid=\"isNonEmpty\" data-error=\"请选择\">";
                        region += "<option value=\"\" selected>请选择</option>";
                        Business.Base.BusinessRegion reg = new project.Business.Base.BusinessRegion();
                        foreach (Entity.Base.EntityRegion it in reg.GetRegionListQuery(string.Empty, string.Empty, user.Entity.AccID, string.Empty))
                        {
                            region += "<option value='" + it.RegionNo + "'>" + it.RegionName + "</option>";
                        }
                        region += "</select>";

                        status = "<select class=\"input-text required size-S\" id=\"StatusS\" style=\"width:80px\" data-valid=\"\" data-error=\"\">";
                        status += "<option value=\"\" selected>请选择</option>";
                        Business.Base.BusinessStatus st = new project.Business.Base.BusinessStatus();
                        foreach (Entity.Base.EntityStatus it in st.GetStatusListQuery(string.Empty, string.Empty, user.Entity.AccID))
                        {
                            status += "<option value='" + it.StatusNo + "'>" + it.StatusName + "</option>";
                        }
                        status += "</select>";
                    }
                }
                else
                    GotoErrorPage();
            }
            catch
            {
                GotoErrorPage();
            }
        }

        Data obj = new Data();
        protected string list = "";
        protected string orderType = "";
        protected string orderTypeS = "";
        protected string alloDept = "";
        protected string alloUser = "";
        protected string region = "";
        protected string status = "";
        private string createList(string OrderNo, string OrderName, string MinOrderDate, string MaxOrderDate, string Status,
            string SaleNo,string Region, string AlloDept, string AlloUser, string CustNo,string OrderType, string IsBack, string IsHangUp,int page)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder("");

            sb.Append("<table class=\"table table-border table-bordered table-hover table-bg table-sort\" id=\"tablelist\">");
            sb.Append("<thead>");
            sb.Append("<tr class=\"text-c\">");
            sb.Append("<th width=\"4%\">序号</th>");
            sb.Append("<th width='13%'>客户名称</th>");
            sb.Append("<th width='9%'>工单号</th>");
            sb.Append("<th width='18%'>工作内容</th>");
            sb.Append("<th width='10%'>工单日期</th>");
            sb.Append("<th width='9%'>工单类型</th>");
            sb.Append("<th width='7%'>联系人</th>");
            sb.Append("<th width='8%'>电话</th>");
            sb.Append("<th width='7%'>发起人</th>");
            sb.Append("<th width='5%'>状态</th>");
            sb.Append("<th width='5%'>挂起</th>");
            sb.Append("<th width='5%'>退回</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            
            DateTime MinOrderDateS = default(DateTime);
            DateTime MaxOrderDateS = default(DateTime);
            if (MinOrderDate != "") MinOrderDateS = ParseDateForString(MinOrderDate);
            if (MinOrderDate != "") MinOrderDateS = ParseDateForString(MinOrderDate);

            bool? IsBackS = null;
            bool? IsHangUpS = null;
            if (IsBack != "") IsBackS = bool.Parse(IsBack);
            if (IsHangUp != "") IsHangUpS = bool.Parse(IsHangUp);

            //非管理员登录，只能看到当前用户为部门负责人的单
            if (user.Entity.UserType.ToUpper() == "03" || user.Entity.UserType.ToUpper() == "07" || user.Entity.UserType.ToUpper() == "08")
            {
                AlloUser = user.Entity.UserNo;
            }

            int r = 1;
            sb.Append("<tbody>");
            Business.Order.BusinessWorkOrder bc = new project.Business.Order.BusinessWorkOrder();
            foreach (Entity.Order.EntityWorkOrder it in bc.GetWorkOrderListQuery(user.Entity.AccID, OrderNo, OrderName, MinOrderDateS, MaxOrderDateS, Status,
                SaleNo, Region, AlloDept, AlloUser, CustNo, OrderType, IsBackS, IsHangUpS, false, page, 15))
            {
                sb.Append("<tr class=\"text-c\" id=\"" + it.InnerEntityOID + "\">");
                sb.Append("<td align='center'>" + r.ToString() + "</td>");
                sb.Append("<td>" + it.CustName + "</td>");
                sb.Append("<td>" + it.OrderNo + "</td>");
                sb.Append("<td>" + it.OrderName + "</td>");
                sb.Append("<td>" + it.OrderDate.ToString("yyyy-MM-dd HH:mm") + "</td>");
                sb.Append("<td>" + it.OrderTypeName + "</td>");
                sb.Append("<td>" + it.LinkMan + "</td>");
                sb.Append("<td>" + it.LinkTel + "</td>");
                sb.Append("<td>" + it.CreateUserName + "</td>");
                sb.Append("<td>" + it.StatusName + "</td>");
                sb.Append("<td class=\"td-status\"><span class=\"label " + (it.IsHangUp ? "" : "label-success") + " radius\">" + (it.IsHangUp ? "挂起" : "正常") + "</span></td>");
                sb.Append("<td class=\"td-status\"><span class=\"label " + (it.IsBack ? "" : "label-success") + " radius\">" + (it.IsBack ? "退回" : "正常") + "</span></td>");
                sb.Append("</tr>");
                
                r++;
            }
            sb.Append("</tbody>");
            sb.Append("</table>");

            sb.Append(Paginat(bc.GetWorkOrderListCount(user.Entity.AccID, OrderNo, OrderName, MinOrderDateS, MaxOrderDateS, Status,
                SaleNo, Region, AlloDept, AlloUser, CustNo, OrderType, IsBackS, IsHangUpS, false), pageSize, page, 7));
            return sb.ToString();
        }
        /// <summary>
        /// 服务器端ajax调用响应请求方法
        /// </summary>
        /// <param name="eventArgument">客户端回调参数</param>
        void System.Web.UI.ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            this._clientArgument = eventArgument;
        }
        private string _clientArgument = "";

        string System.Web.UI.ICallbackEventHandler.GetCallbackResult()
        {
            string result = "";
            JsonArrayParse jp = new JsonArrayParse(this._clientArgument);
            if (jp.getValue("Type") == "delete")
                result = deleteaction(jp);
            else if (jp.getValue("Type") == "update")
                result = updateaction(jp);
            else if (jp.getValue("Type") == "submit")
                result = submitaction(jp);
            else if (jp.getValue("Type") == "view")
                result = viewaction(jp);
            else if (jp.getValue("Type") == "print")
                result = printaction(jp);
            else if (jp.getValue("Type") == "select")
                result = selectaction(jp);
            else if (jp.getValue("Type") == "img")
                result = imgaction(jp);
            else if (jp.getValue("Type") == "adduser")
                result = adduseraction(jp);
            else if (jp.getValue("Type") == "confirm")
                result = confirmaction(jp);
            else if (jp.getValue("Type") == "allodept")
                result = allodeptaction(jp);
            else if (jp.getValue("Type") == "getDeptUser")
                result = getDeptUseraction(jp);
            else if (jp.getValue("Type") == "getNewMessage")
                result = getNewMessageaction(jp);
            else if (jp.getValue("Type") == "exportexcel")
                result = exportexceleaction(jp);
            return result;
        }

        private string updateaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";
            string result = ""; ;
            try
            {
                Business.Order.BusinessWorkOrder bc = new project.Business.Order.BusinessWorkOrder();
                bc.load(jp.getValue("id"));
                if (bc.Entity.Status != "OPEN")
                {
                    flag = "3";
                }
                else
                {
                    collection.Add(new JsonStringValue("CustNo", bc.Entity.CustNo));
                    collection.Add(new JsonStringValue("CustName", bc.Entity.CustName));
                    collection.Add(new JsonStringValue("OrderNo", bc.Entity.OrderNo));
                    collection.Add(new JsonStringValue("OrderName", bc.Entity.OrderName));
                    collection.Add(new JsonStringValue("OrderDate", ParseStringForDate(bc.Entity.OrderDate)));
                    collection.Add(new JsonStringValue("OrderType", bc.Entity.OrderType));
                    collection.Add(new JsonStringValue("SaleNo", bc.Entity.SaleNo));
                    collection.Add(new JsonStringValue("LinkMan", bc.Entity.LinkMan));
                    collection.Add(new JsonStringValue("LinkTel", bc.Entity.LinkTel));
                    collection.Add(new JsonStringValue("Addr", bc.Entity.Addr));
                    collection.Add(new JsonStringValue("Remark", bc.Entity.Remark));
                    collection.Add(new JsonStringValue("AlloDept", bc.Entity.AlloDept));
                    collection.Add(new JsonStringValue("AlloDeptName", bc.Entity.AlloDeptName));
                    collection.Add(new JsonStringValue("AlloUser", bc.Entity.AlloUser));
                    collection.Add(new JsonStringValue("AlloUserName", bc.Entity.AlloUserName));
                    collection.Add(new JsonStringValue("Person", bc.Entity.Person));
                    collection.Add(new JsonStringValue("PersonName", bc.Entity.PersonName));
                    collection.Add(new JsonStringValue("Region", bc.Entity.Region));
                    collection.Add(new JsonStringValue("RegionName", bc.Entity.RegionName));
                    collection.Add(new JsonStringValue("OrderHour", (bc.Entity.OrderDate.Hour < 10 ? "0" + bc.Entity.OrderDate.Hour.ToString() : bc.Entity.OrderDate.Hour.ToString())));
                    collection.Add(new JsonStringValue("OrderMinute", (bc.Entity.OrderDate.Minute < 10 ? "0" + bc.Entity.OrderDate.Minute.ToString() : bc.Entity.OrderDate.Minute.ToString())));

                    collection.Add(new JsonStringValue("CustneedTime", ParseStringForDate(bc.Entity.CustneedTime)));
                    collection.Add(new JsonStringValue("CustneedHour", (bc.Entity.CustneedTime.Hour < 10 ? "0" + bc.Entity.CustneedTime.Hour.ToString() : bc.Entity.CustneedTime.Hour.ToString())));
                    collection.Add(new JsonStringValue("CustneedMinute", (bc.Entity.CustneedTime.Minute < 10 ? "0" + bc.Entity.CustneedTime.Minute.ToString() : bc.Entity.CustneedTime.Minute.ToString())));
                }
            }
            catch
            { flag = "2"; }

            collection.Add(new JsonStringValue("type", "update"));
            collection.Add(new JsonStringValue("flag", flag));

            result = collection.ToString();

            return result;
        }
        private string deleteaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";
            try
            {
                Business.Order.BusinessWorkOrder bc = new project.Business.Order.BusinessWorkOrder();
                bc.load(jp.getValue("id"));
                bc.Entity.IsDel = !bc.Entity.IsDel;

                int r = bc.delete();
                if (r <= 0) flag = "2";
                else
                {
                    collection.Add(new JsonStringValue("liststr", createList(jp.getValue("OrderNoS"), jp.getValue("OrderNameS"), jp.getValue("MinOrderDate"), jp.getValue("MaxOrderDate"),
                            jp.getValue("StatusS"), jp.getValue("SaleNoS"), jp.getValue("RegionS"), jp.getValue("AlloDeptS"), jp.getValue("AlloUserS"),
                            jp.getValue("CustNoS"), jp.getValue("OrderTypeS"), jp.getValue("IsBackS"), jp.getValue("IsHangUpS"), int.Parse(jp.getValue("page")))));
                }
            }
            catch { flag = "2"; }

            collection.Add(new JsonStringValue("type", "delete"));
            collection.Add(new JsonStringValue("flag", flag));

            return collection.ToString();
        }
        private string submitaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";
            try
            {
                Business.Order.BusinessWorkOrder bc = new project.Business.Order.BusinessWorkOrder();
                if (jp.getValue("tp") == "update")
                {
                    bc.load(jp.getValue("id"));
                    bc.Entity.OrderName = jp.getValue("OrderName");
                    bc.Entity.OrderDate = ParseDateForString(jp.getValue("OrderDate") + " " + jp.getValue("OrderHour") + ":" + jp.getValue("OrderMinute")+":00");
                    bc.Entity.OrderType = jp.getValue("OrderType");
                    bc.Entity.SaleNo = jp.getValue("SaleNo");
                    bc.Entity.AlloDept = jp.getValue("AlloDept");
                    bc.Entity.AlloUser = jp.getValue("AlloUser");
                    bc.Entity.CustNo = jp.getValue("CustNo");
                    bc.Entity.LinkMan = jp.getValue("LinkMan");
                    bc.Entity.LinkTel = jp.getValue("LinkTel");
                    bc.Entity.Addr = jp.getValue("Addr");
                    bc.Entity.Region = jp.getValue("Region");
                    string CustneedTime = "";
                    if (jp.getValue("CustneedTime") !="") CustneedTime =jp.getValue("CustneedTime") + " " + jp.getValue("CustneedHour") + ":" + jp.getValue("CustneedMinute") + ":00";
                    bc.Entity.CustneedTime = ParseDateForString(CustneedTime);

                    //bc.Entity.Remark = jp.getValue("Remark");
                    int r = bc.Save();
                    if (r <= 0)
                        flag = "2";
                    else
                    {
                        foreach (string it in jp.getValue("TreatUser").Split(';'))
                        {
                            Business.Order.BusinessWorkOrderPerson person = new Business.Order.BusinessWorkOrderPerson();
                            if (person.GetWorkOrderPersonListCount(user.Entity.AccID, bc.Entity.OrderNo, it, null, false) == 0)
                            {
                                person.Entity.AccID = user.Entity.AccID;
                                person.Entity.CreateDate = GetDate();
                                person.Entity.CreateUser = user.Entity.UserName;
                                person.Entity.IsBack = false;
                                person.Entity.IsDel = false;
                                person.Entity.OrderNo = bc.Entity.OrderNo;
                                person.Entity.UserNo = it;
                                person.Entity.UpdateUser = user.Entity.UserName;
                                person.Entity.UpdateDate = GetDate();
                                int row = person.Save();

                                if (row > 0)
                                {
                                    Business.Order.BusinessWorkOrderMsg msg = new Business.Order.BusinessWorkOrderMsg();
                                    msg.Entity.AccID = user.Entity.AccID;
                                    msg.Entity.Sender = user.Entity.UserNo;
                                    msg.Entity.SendDate = GetDate();
                                    msg.Entity.ToUser = person.Entity.UserNo;
                                    msg.Entity.Subject = "您有一张新的工单！";
                                    msg.Entity.Context = "你有新的工单需要处理，工单号：" + bc.Entity.OrderNo;
                                    msg.Entity.RefNo = bc.Entity.OrderNo;
                                    msg.Entity.MsgType = "1";
                                    msg.Entity.IsDel = false;
                                    msg.Entity.IsRead = false;
                                    msg.Entity.CreateDate = GetDate();
                                    msg.Entity.CreateUser = user.Entity.UserNo;
                                    msg.Save();
                                }
                            }
                        }
                    }
                }
                else
                {
                    string OrderNo = "";
                    string today = GetDate().ToString("yyMMdd");
                    DataTable dt = obj.ExecuteDataSet("select top 1 OrderNo from WO_WorkOrder where OrderNo like N'" + today + "%' and AccID='" + user.Entity.AccID + "' order by OrderNo desc").Tables[0];
                    if (dt.Rows.Count > 0)
                        OrderNo = (long.Parse(dt.Rows[0]["OrderNo"].ToString()) + 1).ToString();
                    else
                        OrderNo = today + "0001";

                    bc.Entity.OrderNo = OrderNo;
                    bc.Entity.OrderName = jp.getValue("OrderName");
                    bc.Entity.OrderDate = ParseDateForString(jp.getValue("OrderDate") + " " + jp.getValue("OrderHour") + ":" + jp.getValue("OrderMinute") + ":00");
                    bc.Entity.OrderType = jp.getValue("OrderType");
                    bc.Entity.SaleNo = jp.getValue("SaleNo");
                    bc.Entity.AlloDept = jp.getValue("AlloDept");
                    bc.Entity.AlloUser = jp.getValue("AlloUser");
                    bc.Entity.CustNo = jp.getValue("CustNo");
                    bc.Entity.LinkMan = jp.getValue("LinkMan");
                    bc.Entity.LinkTel = jp.getValue("LinkTel");
                    bc.Entity.Addr = jp.getValue("Addr");
                    bc.Entity.Region = jp.getValue("Region");
                    //bc.Entity.Remark = jp.getValue("Remark");
                    string CustneedTime = "";
                    if (jp.getValue("CustneedTime") != "") CustneedTime = jp.getValue("CustneedTime") + " " + jp.getValue("CustneedHour") + ":" + jp.getValue("CustneedMinute") + ":00";
                    bc.Entity.CustneedTime = ParseDateForString(CustneedTime);
                    bc.Entity.AccID = user.Entity.AccID;
                    bc.Entity.Status = "OPEN";
                    bc.Entity.CreateTime = GetDate();
                    bc.Entity.CreateUser = user.Entity.UserNo;
                    bc.Entity.UpdateDate = GetDate();
                    bc.Entity.UpdateUser = user.Entity.UserName;

                    int r = bc.Save();
                    if (r <= 0)
                        flag = "2";
                    else
                    {
                        foreach (string it in jp.getValue("TreatUser").Split(';'))
                        {
                            Business.Order.BusinessWorkOrderPerson person = new Business.Order.BusinessWorkOrderPerson();
                            if (person.GetWorkOrderPersonListCount(user.Entity.AccID, bc.Entity.OrderNo, it, null, false) == 0)
                            {
                                person.Entity.AccID = user.Entity.AccID;
                                person.Entity.CreateDate = GetDate();
                                person.Entity.CreateUser = user.Entity.UserName;
                                person.Entity.IsBack = false;
                                person.Entity.IsDel = false;
                                person.Entity.OrderNo = bc.Entity.OrderNo;
                                person.Entity.UserNo = it;
                                person.Entity.UpdateUser = user.Entity.UserName;
                                person.Entity.UpdateDate = GetDate();
                                int row = person.Save();

                                if (row > 0)
                                {
                                    Business.Order.BusinessWorkOrderMsg msg = new Business.Order.BusinessWorkOrderMsg();
                                    msg.Entity.AccID = user.Entity.AccID;
                                    msg.Entity.Sender = user.Entity.UserNo;
                                    msg.Entity.SendDate = GetDate();
                                    msg.Entity.ToUser = person.Entity.UserNo;
                                    msg.Entity.Subject = "您有一张新的工单！";
                                    msg.Entity.Context = "你有新的工单需要处理，工单号：" + bc.Entity.OrderNo;
                                    msg.Entity.RefNo = bc.Entity.OrderNo;
                                    msg.Entity.MsgType = "1";
                                    msg.Entity.IsDel = false;
                                    msg.Entity.IsRead = false;
                                    msg.Entity.CreateDate = GetDate();
                                    msg.Entity.CreateUser = user.Entity.UserNo;
                                    msg.Save();
                                }
                            }
                        }

                        Business.Order.BusinessWorkOrderLog log = new Business.Order.BusinessWorkOrderLog();
                        log.Entity.AccID = user.Entity.AccID;
                        log.Entity.OrderNo = bc.Entity.OrderNo;
                        log.Entity.LogDate = GetDate();
                        log.Entity.LogType = "Add";
                        log.Entity.LogUser = user.Entity.UserNo;
                        log.Entity.Remark = "新建工单:" + bc.Entity.OrderNo;
                        log.Save();
                    }
                }
            }
            catch { flag = "2"; }

            collection.Add(new JsonStringValue("type", "submit"));
            collection.Add(new JsonStringValue("flag", flag));
            if (flag == "1")
            {
                collection.Add(new JsonStringValue("liststr", createList(jp.getValue("OrderNoS"), jp.getValue("OrderNameS"), jp.getValue("MinOrderDate"), jp.getValue("MaxOrderDate"),
                        jp.getValue("StatusS"), jp.getValue("SaleNoS"), jp.getValue("RegionS"), jp.getValue("AlloDeptS"), jp.getValue("AlloUserS"),
                        jp.getValue("CustNoS"), jp.getValue("OrderTypeS"), jp.getValue("IsBackS"), jp.getValue("IsHangUpS"), int.Parse(jp.getValue("page")))));
            }

            return collection.ToString();
        }
        private string viewaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";
            string result = ""; ;
            try
            {
                Business.Order.BusinessWorkOrder bc = new project.Business.Order.BusinessWorkOrder();
                bc.load(jp.getValue("id"));

                collection.Add(new JsonStringValue("CustNo", bc.Entity.CustNo));
                collection.Add(new JsonStringValue("CustName", bc.Entity.CustName));
                collection.Add(new JsonStringValue("OrderNo", bc.Entity.OrderNo));
                collection.Add(new JsonStringValue("OrderName", bc.Entity.OrderName));
                collection.Add(new JsonStringValue("OrderDate", ParseStringForDate(bc.Entity.OrderDate)));
                collection.Add(new JsonStringValue("OrderType", bc.Entity.OrderType));
                collection.Add(new JsonStringValue("SaleNo", bc.Entity.SaleNo));
                collection.Add(new JsonStringValue("LinkMan", bc.Entity.LinkMan));
                collection.Add(new JsonStringValue("LinkTel", bc.Entity.LinkTel));
                collection.Add(new JsonStringValue("Addr", bc.Entity.Addr));
                collection.Add(new JsonStringValue("Remark", bc.Entity.Remark));
                collection.Add(new JsonStringValue("AlloDept", bc.Entity.AlloDept));
                collection.Add(new JsonStringValue("AlloDeptName", bc.Entity.AlloDeptName));
                collection.Add(new JsonStringValue("AlloUser", bc.Entity.AlloUser));
                collection.Add(new JsonStringValue("AlloUserName", bc.Entity.AlloUserName));
                collection.Add(new JsonStringValue("Person", bc.Entity.Person));
                collection.Add(new JsonStringValue("PersonName", bc.Entity.PersonName));
                collection.Add(new JsonStringValue("OrderHour", (bc.Entity.OrderDate.Hour < 10 ? "0" + bc.Entity.OrderDate.Hour.ToString() : bc.Entity.OrderDate.Hour.ToString())));
                collection.Add(new JsonStringValue("OrderMinute", (bc.Entity.OrderDate.Minute < 10 ? "0" + bc.Entity.OrderDate.Minute.ToString() : bc.Entity.OrderDate.Minute.ToString())));
            }
            catch
            { flag = "2"; }

            collection.Add(new JsonStringValue("type", "view"));
            collection.Add(new JsonStringValue("flag", flag));

            result = collection.ToString();

            return result;
        }
        private string printaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";
            string pathName = "";
            string newName = "";
            try
            {
                Business.Order.BusinessWorkOrder bc = new Business.Order.BusinessWorkOrder();
                bc.load(jp.getValue("id"));
                pathName = "工单" + bc.Entity.OrderNo + ".pdf";

                //A4纸张竖向
                Document doc = new Document(PageSize.A4.Rotate(), 40, 40, 20, 20);
                newName = "工单" + creatFileName("pdf");
                PdfWriter.GetInstance(doc, new FileStream(WOPrint.Path + newName, FileMode.Create));
                doc.Open();
                WOPrint.Print(doc, bc.Entity);

                try
                {
                    doc.Close();
                    File.Copy(WOPrint.Path + newName, WOPrint.Path + pathName, true);
                }
                catch { }
            }
            catch (Exception ex)
            {
                flag = "2";
                collection.Add(new JsonStringValue("ex", ex.ToString()));
            }

            collection.Add(new JsonStringValue("type", "print"));
            collection.Add(new JsonStringValue("flag", flag));
            collection.Add(new JsonStringValue("path", pathName));
            return collection.ToString();
        }
        private string selectaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";

            collection.Add(new JsonStringValue("type", "select"));
            collection.Add(new JsonStringValue("flag", flag));
            collection.Add(new JsonStringValue("liststr", createList(jp.getValue("OrderNoS"), jp.getValue("OrderNameS"), jp.getValue("MinOrderDate"), jp.getValue("MaxOrderDate"),
                    jp.getValue("StatusS"), jp.getValue("SaleNoS"), jp.getValue("RegionS"), jp.getValue("AlloDeptS"), jp.getValue("AlloUserS"),
                    jp.getValue("CustNoS"), jp.getValue("OrderTypeS"), jp.getValue("IsBackS"), jp.getValue("IsHangUpS"), int.Parse(jp.getValue("page")))));
            return collection.ToString();
        }
        private string imgaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            System.Text.StringBuilder sb = new System.Text.StringBuilder("");
            string flag = "1";
            try
            {
                Business.Order.BusinessWorkOrder bc = new project.Business.Order.BusinessWorkOrder();
                bc.load(jp.getValue("id"));

                Business.Base.BusinessFlowNode node = new Business.Base.BusinessFlowNode();
                foreach (Entity.Base.EntityFlowNode it in node.GetFlowNodeListQuery(string.Empty, string.Empty, user.Entity.AccID))
                {
                    Business.Order.BusinessWorkOrderImages bd = new Business.Order.BusinessWorkOrderImages();
                    if (bd.GetWorkOrderImagesCount(user.Entity.AccID, bc.Entity.OrderNo, it.NodeNo) > 0)
                    {
                        sb.Append("<div class=\"row cl\" style=\"border-bottom:solid 1px #AFD4E2; height:150px;\">");
                        sb.Append("<label class=\"form-label col-1\">" + it.NodeName + "</label>");
                        sb.Append("<div class=\"formControls col-8\">");

                        foreach (Entity.Order.EntityWorkOrderImages it1 in bd.GetWorkOrderImagesQuery(user.Entity.AccID, bc.Entity.OrderNo, it.NodeNo))
                        {
                            sb.Append("<img style=\"width:120px; height:120px; margin:10px;\" alt=\"\" src=\"../../upload/" + it1.Img + "\" />");
                        }
                        sb.Append("</div>");
                        sb.Append("</div>");
                    }
                }

                if (sb.ToString() == "")
                {
                    sb.Append("<div class=\"row cl\">");
                    sb.Append("<div class=\"formControls col-8\" style=\"margin-left:30px;\">当前工单没有图片信息！</div>");
                    sb.Append("</div>");                
                }
            }
            catch { flag = "2"; }

            collection.Add(new JsonStringValue("liststr", sb.ToString()));

            collection.Add(new JsonStringValue("flag", flag));
            collection.Add(new JsonStringValue("type", "img"));

            return collection.ToString();
        }
        private string adduseraction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";
            try
            {
                Business.Order.BusinessWorkOrder bc = new project.Business.Order.BusinessWorkOrder();
                bc.load(jp.getValue("id"));

                foreach (string it in jp.getValue("labels").Split(';'))
                {
                    Business.Order.BusinessWorkOrderPerson person = new Business.Order.BusinessWorkOrderPerson();
                    if (person.GetWorkOrderPersonListCount(user.Entity.AccID, bc.Entity.OrderNo, it, null, false) == 0)
                    {
                        person.Entity.AccID = user.Entity.AccID;
                        person.Entity.CreateDate = GetDate();
                        person.Entity.CreateUser = user.Entity.UserName;
                        person.Entity.IsBack = false;
                        person.Entity.IsDel = false;
                        person.Entity.OrderNo = bc.Entity.OrderNo;
                        person.Entity.UserNo = it;
                        person.Entity.UpdateUser = user.Entity.UserName;
                        person.Entity.UpdateDate = GetDate();
                        int row = person.Save();

                        if (row > 0)
                        {
                            Business.Order.BusinessWorkOrderMsg msg = new Business.Order.BusinessWorkOrderMsg();
                            msg.Entity.AccID = user.Entity.AccID;
                            msg.Entity.Sender = user.Entity.UserNo;
                            msg.Entity.SendDate = GetDate();
                            msg.Entity.ToUser = person.Entity.UserNo;
                            msg.Entity.Subject = "您有一张新的工单！";
                            msg.Entity.Context = "你有新的工单需要处理，工单号：" + bc.Entity.OrderNo;
                            msg.Entity.RefNo = bc.Entity.OrderNo;
                            msg.Entity.MsgType = "1";
                            msg.Entity.IsDel = false;
                            msg.Entity.IsRead = false;
                            msg.Entity.CreateDate = GetDate();
                            msg.Entity.CreateUser = user.Entity.UserNo;
                            msg.Save();
                        }
                    }
                }

                //修改退回状态
                obj.ExecuteNonQuery("update WO_WorkOrder set IsBack=0,BackDate=null,backReason='' where RowPointer='" + bc.Entity.InnerEntityOID + "' and AccID='" + bc.Entity.AccID + "'");
                obj.ExecuteNonQuery("update WO_WorkOrder_Person set IsBack=0 where OrderNo='" + bc.Entity.OrderNo + "' and AccID='" + bc.Entity.AccID + "'");
            }
            catch { flag = "2"; }

            collection.Add(new JsonStringValue("type", "adduser"));
            collection.Add(new JsonStringValue("flag", flag));

            return collection.ToString();
        }
        private string confirmaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";
            try
            {
                Business.Order.BusinessWorkOrder bc = new project.Business.Order.BusinessWorkOrder();
                bc.load(jp.getValue("id"));

                string InfoMsg = bc.confirm(user.Entity.AccID, bc.Entity.OrderNo, "", "", user.Entity.UserNo, user.Entity.UserName);
                if (InfoMsg != "")
                {
                    flag = "3";
                    collection.Add(new JsonStringValue("info", InfoMsg));
                }
                else {
                    collection.Add(new JsonStringValue("liststr", createList(jp.getValue("OrderNoS"), jp.getValue("OrderNameS"), jp.getValue("MinOrderDate"), jp.getValue("MaxOrderDate"),
                            jp.getValue("StatusS"), jp.getValue("SaleNoS"), jp.getValue("RegionS"), jp.getValue("AlloDeptS"), jp.getValue("AlloUserS"),
                            jp.getValue("CustNoS"), jp.getValue("OrderTypeS"), jp.getValue("IsBackS"), jp.getValue("IsHangUpS"), int.Parse(jp.getValue("page")))));
                }
            }
            catch { flag = "2"; }

            collection.Add(new JsonStringValue("type", "confirm"));
            collection.Add(new JsonStringValue("flag", flag));

            return collection.ToString();
        }
        private string allodeptaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";
            string result = ""; ;
            try
            {
                Business.Sys.BusinessDept bc = new Business.Sys.BusinessDept();
                bc.load(jp.getValue("DeptNo"), user.Entity.AccID);
                collection.Add(new JsonStringValue("AlloUser", bc.Entity.Manager));
                collection.Add(new JsonStringValue("AlloUserName", bc.Entity.ManagerName));

            }
            catch
            { flag = "2"; }

            collection.Add(new JsonStringValue("type", "allodept"));
            collection.Add(new JsonStringValue("flag", flag));

            result = collection.ToString();

            return result;
        }
        private string getDeptUseraction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";
            string result = ""; ;
            try
            {
                DataTable dt = obj.ExecuteDataSet("select a.*,b.DeptName from Base_AutoAllocation a left join (select * from Sys_Dept where AccID='" + user.Entity.AccID + "') b on a.DeptNo=b.DeptNo " +
                    "where a.AccID='" + user.Entity.AccID + "' and a.OrderType='" + jp.getValue("OrderType") + "' and  a.RegionNo='" + jp.getValue("RegionNo") + "'").Tables[0];
                if (dt.Rows.Count > 0)
                {
                    collection.Add(new JsonStringValue("DeptNo", dt.Rows[0]["DeptNo"].ToString()));
                    collection.Add(new JsonStringValue("DeptName", dt.Rows[0]["DeptName"].ToString()));
                    collection.Add(new JsonStringValue("UserNo", dt.Rows[0]["UserNo"].ToString()));
                    collection.Add(new JsonStringValue("UserName", dt.Rows[0]["UserName"].ToString()));

                    Business.Sys.BusinessDept bc = new Business.Sys.BusinessDept();
                    bc.load(dt.Rows[0]["DeptNo"].ToString(), user.Entity.AccID);
                    collection.Add(new JsonStringValue("AlloUser", bc.Entity.Manager));
                    collection.Add(new JsonStringValue("AlloUserName", bc.Entity.ManagerName));
                }
                else
                {
                    flag = "3"; 
                }
            }
            catch
            { flag = "2"; }

            collection.Add(new JsonStringValue("type", "getDeptUser"));
            collection.Add(new JsonStringValue("flag", flag));

            result = collection.ToString();

            return result;
        }
        private string getNewMessageaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";
            string result = ""; ;
            try
            {
                string sqlstr = "select COUNT(1) as cnt from WO_WorkOrder where CreateTime>=DateAdd(SECOND,-5,GetDate())";
                if (user.Entity.UserType.ToUpper() == "03" || user.Entity.UserType.ToUpper() == "07" || user.Entity.UserType.ToUpper() == "08")
                    sqlstr = "select COUNT(1) as cnt from WO_WorkOrder where CreateTime>=DateAdd(SECOND,-5,GetDate()) and AlloUser='" + user.Entity.UserType + "'";

                DataTable dt = obj.ExecuteDataSet(sqlstr).Tables[0];
                if (ParseIntForString(dt.Rows[0]["cnt"].ToString()) > 0)
                {
                    flag = "0"; 
                }
                else
                {
                    flag = "3"; 
                }
            }
            catch
            { flag = "2"; }

            collection.Add(new JsonStringValue("type", "getNewMessage"));
            collection.Add(new JsonStringValue("flag", flag));

            result = collection.ToString();

            return result;
        }
        private string exportexceleaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "0";
            string pathName = "";
            try
            {
                pathName = "工单列表" + GetDate().ToString("yyMMddHHmmss") + getRandom(4) + ".xls";

                HSSFWorkbook workbook = new HSSFWorkbook();
                HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet("工单列表");
                HSSFRow headerRow = (HSSFRow)sheet.CreateRow(0);
                headerRow.CreateCell(0).SetCellValue("工单编号");
                headerRow.CreateCell(1).SetCellValue("工单内容");
                headerRow.CreateCell(2).SetCellValue("工单日期");
                headerRow.CreateCell(3).SetCellValue("工单类型");
                headerRow.CreateCell(4).SetCellValue("要求服务时间");
                headerRow.CreateCell(5).SetCellValue("联系人");
                headerRow.CreateCell(6).SetCellValue("联系电话");
                headerRow.CreateCell(7).SetCellValue("地址");
                headerRow.CreateCell(8).SetCellValue("服务单号");
                headerRow.CreateCell(9).SetCellValue("执行部门");
                headerRow.CreateCell(10).SetCellValue("部门负责人");
                headerRow.CreateCell(11).SetCellValue("执行人");
                headerRow.CreateCell(12).SetCellValue("状态");
                headerRow.CreateCell(13).SetCellValue("是否挂起");
                headerRow.CreateCell(14).SetCellValue("是否退回");
                headerRow.CreateCell(15).SetCellValue("是否申请支援");
                headerRow.CreateCell(16).SetCellValue("响应时间");
                headerRow.CreateCell(17).SetCellValue("预约时间");
                headerRow.CreateCell(18).SetCellValue("签到时间");
                headerRow.CreateCell(19).SetCellValue("执行时间");
                headerRow.CreateCell(20).SetCellValue("完成时间");
                headerRow.CreateCell(21).SetCellValue("消单时间");
                headerRow.CreateCell(22).SetCellValue("确认消单时间");
                
                DateTime MinOrderDateS = default(DateTime);
                DateTime MaxOrderDateS = default(DateTime);
                if (jp.getValue("MinOrderDate") != "") MinOrderDateS = ParseDateForString(jp.getValue("MinOrderDate"));
                if (jp.getValue("MaxOrderDate") != "") MaxOrderDateS = ParseDateForString(jp.getValue("MaxOrderDate"));

                bool? IsBackS = null;
                bool? IsHangUpS = null;
                if (jp.getValue("IsBackS") != "") IsBackS = bool.Parse(jp.getValue("IsBackS"));
                if (jp.getValue("IsHangUpS") != "") IsHangUpS = bool.Parse(jp.getValue("IsHangUpS"));

                string AlloUser = jp.getValue("AlloUserS");
                //非管理员登录，只能看到当前用户为部门负责人的单
                if (user.Entity.UserType.ToUpper() == "03" || user.Entity.UserType.ToUpper() == "07" || user.Entity.UserType.ToUpper() == "08")
                {
                    AlloUser = user.Entity.UserNo;
                }

                int rowIndex = 1;
                project.Business.Order.BusinessWorkOrder bc = new project.Business.Order.BusinessWorkOrder();
                foreach (project.Entity.Order.EntityWorkOrder it in bc.GetWorkOrderListQuery(
                    "A", jp.getValue("OrderNoS"), jp.getValue("OrderNameS"), MinOrderDateS, MaxOrderDateS, jp.getValue("StatusS"),
                    jp.getValue("SaleNoS"), jp.getValue("RegionS"), jp.getValue("AlloDeptS"), AlloUser, 
                    jp.getValue("CustNoS"), jp.getValue("OrderTypeS"), IsBackS, IsHangUpS, false))
                {
                    HSSFRow dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
                    dataRow.CreateCell(0).SetCellValue(it.OrderNo);
                    dataRow.CreateCell(1).SetCellValue(it.OrderName);
                    dataRow.CreateCell(2).SetCellValue(ParseStringForDateTime(it.OrderDate));
                    dataRow.CreateCell(3).SetCellValue(it.OrderTypeName);
                    dataRow.CreateCell(4).SetCellValue(ParseStringForDateTime(it.CustneedTime));
                    dataRow.CreateCell(5).SetCellValue(it.LinkMan);
                    dataRow.CreateCell(6).SetCellValue(it.LinkTel);
                    dataRow.CreateCell(7).SetCellValue(it.Addr);
                    dataRow.CreateCell(8).SetCellValue(it.SaleNo);
                    dataRow.CreateCell(9).SetCellValue(it.AlloDeptName);
                    dataRow.CreateCell(10).SetCellValue(it.AlloUserName);
                    dataRow.CreateCell(11).SetCellValue(it.PersonName);
                    dataRow.CreateCell(12).SetCellValue(it.StatusName);
                    dataRow.CreateCell(13).SetCellValue(it.IsHangUp?"是":"否");
                    dataRow.CreateCell(14).SetCellValue(it.IsBack?"是":"否");
                    dataRow.CreateCell(15).SetCellValue(it.IsApply?"是":"否");
                    dataRow.CreateCell(16).SetCellValue(ParseStringForDateTime(it.ResponseTime));
                    dataRow.CreateCell(17).SetCellValue(ParseStringForDateTime(it.AppoIntTime));
                    dataRow.CreateCell(18).SetCellValue(ParseStringForDateTime(it.SignTime));
                    dataRow.CreateCell(19).SetCellValue(ParseStringForDateTime(it.WorkTime));
                    dataRow.CreateCell(20).SetCellValue(ParseStringForDateTime(it.FinishTime));
                    dataRow.CreateCell(21).SetCellValue(ParseStringForDateTime(it.CloseTime));
                    dataRow.CreateCell(22).SetCellValue(ParseStringForDateTime(it.ConfirmTime));
                    dataRow = null;
                    rowIndex++;
                }

                MemoryStream ms = new MemoryStream();
                workbook.Write(ms);
                headerRow = null;
                sheet = null;
                workbook = null;
                FileStream fs = new FileStream(localpath + pathName, FileMode.OpenOrCreate);
                BinaryWriter w = new BinaryWriter(fs);
                w.Write(ms.ToArray());
                fs.Close();
                ms.Close();
                ms.Dispose();
            }
            catch (Exception ex)
            {
                flag = "2";
                collection.Add(new JsonStringValue("ex", ex.ToString()));
            }

            collection.Add(new JsonStringValue("type", "exportexcel"));
            collection.Add(new JsonStringValue("flag", flag));
            collection.Add(new JsonStringValue("path", pathName));
            return collection.ToString();
        }

    }
}