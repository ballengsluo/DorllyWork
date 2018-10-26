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

namespace project.Presentation.Base
{
    public partial class ChooseBasicCheck : AbstractPmPage, System.Web.UI.ICallbackEventHandler
    {
        Business.Sys.BusinessUserInfo user = new project.Business.Sys.BusinessUserInfo();
        protected string userid = "";
        protected string id = "";
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

                    id = Request.QueryString["id"].ToString();
                    if (!Page.IsCallback)
                    {
                        list = createList(string.Empty);
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

        protected string list = "";
        private string createList(string Name)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder("");

            if (Request.QueryString["type"] == "dept")
            {
                sb.Append("<table class=\"table table-border table-bordered table-hover table-bg table-sort\" id=\"tablelist\">");
                sb.Append("<thead>");
                sb.Append("<tr class=\"text-c\">");
                sb.Append("<th width='10%'>勾选</th>");
                sb.Append("<th width='30%'>部门编号</th>");
                sb.Append("<th width='60%'>部门名称</th>");
                sb.Append("</tr>");
                sb.Append("</thead>");

                sb.Append("<tbody>");
                Business.Sys.BusinessDept pt = new project.Business.Sys.BusinessDept();
                foreach (Entity.Sys.EntityDept it in pt.GetDeptListQuery(string.Empty, Name, user.Entity.AccID, string.Empty))
                {
                    sb.Append("<tr class=\"text-c\">");
                    sb.Append("<td align='center'><input type='checkbox' name='chk' id='" + it.DeptNo + "' value='" + it.DeptName + "' /></td>");
                    sb.Append("<td style='white-space: nowrap;'>" + it.DeptNo + "</td>");
                    sb.Append("<td style='white-space: nowrap;'>" + it.DeptName + "</td>");
                    sb.Append("</tr>");
                }
                sb.Append("</tbody>");
                sb.Append("</table>");
            }
            if (Request.QueryString["type"] == "user")
            {
                sb.Append("<table class=\"table table-border table-bordered table-hover table-bg table-sort\" id=\"tablelist\">");
                sb.Append("<thead>");
                sb.Append("<tr class=\"text-c\">");
                sb.Append("<th width='10%'>勾选</th>");
                sb.Append("<th width='20%'>用户编号</th>");
                sb.Append("<th width='30%'>用户名称</th>");
                sb.Append("<th width='40%'>所属部门</th>");
                sb.Append("</tr>");
                sb.Append("</thead>");

                sb.Append("<tbody>");
                Business.Sys.BusinessUserInfo pt = new project.Business.Sys.BusinessUserInfo();
                foreach (Entity.Sys.EntityUserInfo it in pt.GetUserInfoListQuery(string.Empty, user.Entity.AccID, string.Empty, Name))
                {
                    sb.Append("<tr class=\"text-c\">");
                    sb.Append("<td align='center'><input type='checkbox' name='chk' id='" + it.UserNo + "' value='" + it.UserName + "' /></td>");
                    sb.Append("<td style='white-space: nowrap;'>" + it.UserNo + "</td>");
                    sb.Append("<td style='white-space: nowrap;'>" + it.UserName + "</td>");
                    sb.Append("<td style='white-space: nowrap;'>" + it.DeptName + "</td>");
                    sb.Append("</tr>");
                }
                sb.Append("</tbody>");
                sb.Append("</table>");
            }
            if (Request.QueryString["type"] == "ordertype")
            {
                sb.Append("<table class=\"table table-border table-bordered table-hover table-bg table-sort\" id=\"tablelist\">");
                sb.Append("<thead>");
                sb.Append("<tr class=\"text-c\">");
                sb.Append("<th width='10%'>勾选</th>");
                sb.Append("<th width='30%'>工单类型编号</th>");
                sb.Append("<th width='60%'>工单类型名称</th>");
                sb.Append("</tr>");
                sb.Append("</thead>");

                sb.Append("<tbody>");
                Business.Base.BusinessOrderType bc = new Business.Base.BusinessOrderType();
                foreach (Entity.Base.EntityOrderType it in bc.GetOrderTypeListQuery(string.Empty, Name, string.Empty,user.Entity.AccID))
                {
                    sb.Append("<tr class=\"text-c\">");
                    sb.Append("<td align='center'><input type='checkbox' name='chk' id='" + it.OrderTypeNo + "' value='" + it.OrderTypeName + "' /></td>");
                    sb.Append("<td style='white-space: nowrap;'>" + it.OrderTypeNo + "</td>");
                    sb.Append("<td style='white-space: nowrap;'>" + it.OrderTypeName + "</td>");
                    sb.Append("</tr>");
                }
                sb.Append("</tbody>");
                sb.Append("</table>");
            }
            if (Request.QueryString["type"] == "node")
            {
                sb.Append("<table class=\"table table-border table-bordered table-hover table-bg table-sort\" id=\"tablelist\">");
                sb.Append("<thead>");
                sb.Append("<tr class=\"text-c\">");
                sb.Append("<th width='10%'>勾选</th>");
                sb.Append("<th width='30%'>节点编号</th>");
                sb.Append("<th width='60%'>节点名称</th>");
                sb.Append("</tr>");
                sb.Append("</thead>");

                sb.Append("<tbody>");
                Business.Base.BusinessFlowNode bc = new Business.Base.BusinessFlowNode();
                foreach (Entity.Base.EntityFlowNode it in bc.GetFlowNodeListQuery(string.Empty, Name, user.Entity.AccID))
                {
                    if (it.NodeNo == "N") continue;
                    sb.Append("<tr class=\"text-c\">");
                    sb.Append("<td align='center'><input type='checkbox' name='chk' id='" + it.NodeNo + "' value='" + it.NodeName + "' /></td>");
                    sb.Append("<td style='white-space: nowrap;'>" + it.NodeNo + "</td>");
                    sb.Append("<td style='white-space: nowrap;'>" + it.NodeName + "</td>");
                    sb.Append("</tr>");
                }
                sb.Append("</tbody>");
                sb.Append("</table>");
            }
            if (Request.QueryString["type"] == "op")
            {
                sb.Append("<table class=\"table table-border table-bordered table-hover table-bg table-sort\" id=\"tablelist\">");
                sb.Append("<thead>");
                sb.Append("<tr class=\"text-c\">");
                sb.Append("<th width='10%'>勾选</th>");
                sb.Append("<th width='90%'>操作</th>");
                sb.Append("</tr>");
                sb.Append("</thead>");

                sb.Append("<tbody>");
                Business.Base.BusinessOperate bc = new Business.Base.BusinessOperate();
                foreach (Entity.Base.EntityOperate it in bc.GetOpListQuery(string.Empty, Name, user.Entity.AccID))
                {
                    sb.Append("<tr class=\"text-c\">");
                    sb.Append("<td align='center'><input type='checkbox' name='chk' id='" + it.OpNo + "' value='" + it.OpName + "' /></td>");
                    sb.Append("<td style='white-space: nowrap;'>" + it.OpName + "</td>");
                    sb.Append("</tr>");
                }
                sb.Append("</tbody>");
                sb.Append("</table>");
            }
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
            return result;
        }

        private string selectaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string isok = "1";
            try
            {
                collection.Add(new JsonStringValue("type", "select"));
                collection.Add(new JsonStringValue("liststr", createList(jp.getValue("Name"))));
            }
            catch
            { isok = "0"; }
            collection.Add(new JsonStringValue("flag", isok));

            return collection.ToString();
        }

    }
}