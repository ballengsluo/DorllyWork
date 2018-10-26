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

namespace project.Presentation.Order
{
    public partial class WorkOrderMsg : AbstractPmPage, System.Web.UI.ICallbackEventHandler
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
                    CheckRight(user.Entity, "order/WorkOrderMsg.aspx");

                    if (!Page.IsCallback)
                    {
                        list = createList(1);
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
        protected string alloDept = "";
        protected string alloUser = "";
        protected string region = "";
        private string createList(int page)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder("");

            sb.Append("<table class=\"table table-border table-bordered table-hover table-bg\" id=\"tablelist\">");
            sb.Append("<thead>");
            sb.Append("<tr class=\"text-c\">");
            sb.Append("<th width=\"5%\" align='center'><input type=\"checkbox\" class=\"check-box\" id=\"chekall\" /></th>");
            sb.Append("<th width=\"5%\">序号</th>");
            sb.Append("<th width='20%'>标题</th>");
            sb.Append("<th width='40%'>内容</th>");
            sb.Append("<th width='10%'>发送人</th>");
            sb.Append("<th width='10%'>发送日期</th>");
            sb.Append("<th width='10%'>状态</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");

            int r = 1;
            sb.Append("<tbody>");
            Business.Order.BusinessWorkOrderMsg bc = new project.Business.Order.BusinessWorkOrderMsg();
            foreach (Entity.Order.EntityWorkOrderMsg it in bc.GetWorkOrderMsgQuery(user.Entity.AccID,string.Empty,default(DateTime),default(DateTime),user.Entity.UserNo,null,string.Empty,false, page, pageSize))
            {
                if (it.IsRead)
                {
                    sb.Append("<tr class=\"text-c\">");
                    sb.Append("<td align='center'><input type=\"checkbox\" class=\"check-box\" id=\"" + it.InnerEntityOID + "\" name=\"chk_list\" /></td>");
                    sb.Append("<td align='center'>" + r.ToString() + "</td>");
                    sb.Append("<td><a href=\"javascript:void();\" onclick=\"view('" + it.InnerEntityOID + "')\">" + lenCHEN(it.Subject, 30) + "</a></td>");
                    sb.Append("<td><a href=\"javascript:void();\" onclick=\"view('" + it.InnerEntityOID + "')\">" + lenCHEN(it.Context, 50) + "</a></td>");
                    sb.Append("<td>" + it.SenderName + "</td>");
                    sb.Append("<td>" + it.SendDate.ToString("MM:dd HH:mm") + "</td>");
                    sb.Append("<td class=\"td-status\"><span class=\"label label-success radius\">已读</span></td>");
                    sb.Append("</tr>");
                }
                else
                {
                    sb.Append("<tr class=\"text-c\">");
                    sb.Append("<td align='center'><input type=\"checkbox\" class=\"check-box\" id=\"" + it.InnerEntityOID + "\" name=\"chk_list\" /></td>");
                    sb.Append("<td align='center'><b>" + r.ToString() + "</b></td>");
                    sb.Append("<td><b><a href=\"javascript:void();\" onclick=\"view('" + it.InnerEntityOID + "')\">" + lenCHEN(it.Subject, 30) + "</a></b></td>");
                    sb.Append("<td><b><a href=\"javascript:void();\" onclick=\"view('"+it.InnerEntityOID+"')\">" + lenCHEN(it.Context, 50) + "</a></b></td>");
                    sb.Append("<td><b>" + it.SenderName + "</b></td>");
                    sb.Append("<td><b>" + it.SendDate.ToString("MM:dd HH:mm") + "</b></td>");
                    sb.Append("<td class=\"td-status\"><span class=\"label radius\">未读</span></td>");
                    sb.Append("</tr>");
                }
                r++;
            }
            sb.Append("</tbody>");
            sb.Append("</table>");

            sb.Append(Paginat(bc.GetWorkOrderMsgCount(user.Entity.AccID, string.Empty, default(DateTime), default(DateTime), user.Entity.UserNo, null, string.Empty, false), pageSize, page, 7));
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
            if (jp.getValue("Type") == "select")
                result = selectaction(jp);
            else if (jp.getValue("Type") == "read")
                result = readaction(jp);
            else if (jp.getValue("Type") == "delete")
                result = deleteaction(jp);
            else if (jp.getValue("Type") == "view")
                result = viewaction(jp);
            return result;
        }

        private string selectaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";

            collection.Add(new JsonStringValue("type", "select"));
            collection.Add(new JsonStringValue("flag", flag));
            collection.Add(new JsonStringValue("liststr", createList(int.Parse(jp.getValue("page")))));

            return collection.ToString();
        }

        private string readaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";

            foreach (string it in jp.getValue("details").Split(';'))
            {
                if (it == "") continue;
                Business.Order.BusinessWorkOrderMsg msg = new Business.Order.BusinessWorkOrderMsg();
                msg.load(it, user.Entity.AccID);
                if (msg.Entity.IsRead == false)
                {
                    msg.Entity.IsRead = true;
                    msg.Entity.ReadDate = GetDate();
                    msg.Save();
                }
            }

            collection.Add(new JsonStringValue("type", "select"));
            collection.Add(new JsonStringValue("flag", flag));
            collection.Add(new JsonStringValue("liststr", createList(int.Parse(jp.getValue("page")))));

            return collection.ToString();
        }

        private string deleteaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";

            foreach (string it in jp.getValue("details").Split(';'))
            {
                if (it == "") continue;
                Business.Order.BusinessWorkOrderMsg msg = new Business.Order.BusinessWorkOrderMsg();
                msg.load(it, user.Entity.AccID);
                if (msg.Entity.IsDel == false)
                    msg.delete();
            }

            collection.Add(new JsonStringValue("type", "select"));
            collection.Add(new JsonStringValue("flag", flag));
            collection.Add(new JsonStringValue("liststr", createList(int.Parse(jp.getValue("page")))));

            return collection.ToString();
        }

        private string viewaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";

            Business.Order.BusinessWorkOrderMsg bc = new project.Business.Order.BusinessWorkOrderMsg();
            bc.load(jp.getValue("id"), user.Entity.AccID);
            collection.Add(new JsonStringValue("MsgType", bc.Entity.MsgTypeName));
            collection.Add(new JsonStringValue("Subject", bc.Entity.Subject));
            collection.Add(new JsonStringValue("Context", bc.Entity.Context));
            collection.Add(new JsonStringValue("SendDate", bc.Entity.SendDate.ToString("yyyy-MM-dd HH:ss")));
            collection.Add(new JsonStringValue("Sender", bc.Entity.SenderName));
            bc.Entity.IsRead = true;
            bc.Entity.ReadDate = GetDate();
            bc.Save();

            collection.Add(new JsonStringValue("type", "view"));
            collection.Add(new JsonStringValue("flag", flag));
            collection.Add(new JsonStringValue("liststr", createList(int.Parse(jp.getValue("page")))));

            return collection.ToString();
        }
    }
}