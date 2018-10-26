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
    public partial class ChooseBasic : AbstractPmPage, System.Web.UI.ICallbackEventHandler
    {
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
                    userid = Encrypt.DecryptDES(str,"1");
                    user.load(userid);
                    id = Request.QueryString["id"].ToString();

                    if (!Page.IsCallback)
                        list = createList(null, 1);
                }
                else
                    GotoErrorPage();
            }
            catch
            {
                GotoErrorPage();
            }
        }

        Business.Sys.BusinessUserInfo user = new project.Business.Sys.BusinessUserInfo();
        Data obj = new Data();
        protected string list = "";
        private string createList(string Name, int page)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder("");

            if (Request.QueryString["type"] == "dept")
            {
                sb.Append("<table class=\"table table-border table-bordered table-hover table-bg table-sort\" id=\"tablelist\">");
                sb.Append("<thead>");
                sb.Append("<tr class=\"text-c\">");
                sb.Append("<th width='35%'>部门编号</th>");
                sb.Append("<th width='65%'>部门名称</th>");
                sb.Append("</tr>");
                sb.Append("</thead>");

                sb.Append("<tbody>");
                Business.Sys.BusinessDept pt = new project.Business.Sys.BusinessDept();
                foreach (Entity.Sys.EntityDept it in pt.GetDeptListQuery(string.Empty, Name,user.Entity.AccID, string.Empty, page, 15))
                {
                    sb.Append("<tr class=\"text-c\" id='" + it.DeptNo + "' onclick='submit(\""+it.DeptNo+"\")'>");
                    sb.Append("<td style='white-space: nowrap;'>" + it.DeptNo + "<input type='hidden' id='it" + it.DeptNo + "' value='" + it.DeptName + "' /></td>");
                    sb.Append("<td style='white-space: nowrap;'>" + it.DeptName + "</td>");
                    sb.Append("</tr>");
                }
                sb.Append("</tbody>");
                sb.Append("</table>");
                sb.Append(Paginat(pt.GetDeptListCount(string.Empty, Name, user.Entity.AccID, string.Empty), 15, page, 5));
            }
            else if (Request.QueryString["type"] == "user")
            {
                sb.Append("<table class=\"table table-border table-bordered table-hover table-bg table-sort\" id=\"tablelist\">");
                sb.Append("<thead>");
                sb.Append("<tr class=\"text-c\">");
                sb.Append("<th width='25%'>用户编号</th>");
                sb.Append("<th width='35%'>用户名称</th>");
                sb.Append("<th width='40%'>所属部门</th>");
                sb.Append("</tr>");
                sb.Append("</thead>");

                sb.Append("<tbody>");
                Business.Sys.BusinessUserInfo pt = new project.Business.Sys.BusinessUserInfo();
                foreach (Entity.Sys.EntityUserInfo it in pt.GetUserInfoListQuery(string.Empty, user.Entity.AccID, string.Empty, Name, page, 15))
                {
                    sb.Append("<tr class=\"text-c\" id='" + it.UserNo + "' onclick='submit(\"" + it.UserNo + "\")'>");
                    sb.Append("<td style='white-space: nowrap;'>" + it.UserNo + "<input type='hidden' id='it" + it.UserNo + "' value='" + it.UserName + "' /></td>");
                    sb.Append("<td style='white-space: nowrap;'>" + it.UserName + "</td>");
                    sb.Append("<td style='white-space: nowrap;'>" + it.DeptName + "</td>");
                    sb.Append("</tr>");
                }
                sb.Append("</tbody>");
                sb.Append("</table>");
                sb.Append(Paginat(pt.GetUserInfoListCount(string.Empty, user.Entity.AccID, string.Empty, Name), 15, page, 5));
            }
            else if (Request.QueryString["type"] == "region")
            {
                sb.Append("<table class=\"table table-border table-bordered table-hover table-bg table-sort\" id=\"tablelist\">");
                sb.Append("<thead>");
                sb.Append("<tr class=\"text-c\">");
                sb.Append("<th width='35%'>地区编号</th>");
                sb.Append("<th width='65%'>地区名称</th>");
                sb.Append("</tr>");
                sb.Append("</thead>");

                sb.Append("<tbody>");
                Business.Base.BusinessRegion pt = new project.Business.Base.BusinessRegion();
                foreach (Entity.Base.EntityRegion it in pt.GetRegionListQuery(string.Empty, Name, user.Entity.AccID, string.Empty, page, 15))
                {
                    sb.Append("<tr class=\"text-c\" id='" + it.RegionNo + "' onclick='submit(\"" + it.RegionNo + "\")'>");
                    sb.Append("<td style='white-space: nowrap;'>" + it.RegionNo + "<input type='hidden' id='it" + it.RegionNo + "' value='" + it.RegionName + "' /></td>");
                    sb.Append("<td style='white-space: nowrap;'>" + it.RegionName + "</td>");
                    sb.Append("</tr>");
                }
                sb.Append("</tbody>");
                sb.Append("</table>");
                sb.Append(Paginat(pt.GetRegionListCount(string.Empty, Name, user.Entity.AccID, string.Empty), 15, page, 5));
            }
            else if (Request.QueryString["type"] == "cust")
            {
                sb.Append("<table class=\"table table-border table-bordered table-hover table-bg table-sort\" id=\"tablelist\">");
                sb.Append("<thead>");
                sb.Append("<tr class=\"text-c\">");
                sb.Append("<th width='35%'>客户编号</th>");
                sb.Append("<th width='65%'>客户名称</th>");
                sb.Append("</tr>");
                sb.Append("</thead>");

                sb.Append("<tbody>");
                Business.Base.BusinessCustInfo pt = new project.Business.Base.BusinessCustInfo();
                foreach (Entity.Base.EntityCustInfo it in pt.GetCustInfoListQuery(string.Empty, user.Entity.AccID, string.Empty, Name, "", true, page, 15))
                {
                    sb.Append("<tr class=\"text-c\" id='" + it.CustNo + "' onclick='submit(\"" + it.CustNo + "\")'>");
                    sb.Append("<td style='white-space: nowrap;'>" + it.CustNo + "<input type='hidden' id='it" + it.CustNo + "' value='" + it.CustName + "' /></td>");
                    sb.Append("<td style='white-space: nowrap;'>" + it.CustName + "</td>");
                    sb.Append("</tr>");
                }
                sb.Append("</tbody>");
                sb.Append("</table>");
                sb.Append(Paginat(pt.GetCustInfoListCount(string.Empty, user.Entity.AccID, string.Empty, Name, "", true), 15, page, 5));
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
            else if (jp.getValue("Type") == "jump")
                result = jumpaction(jp);
            return result;
        }

        private string selectaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string isok = "1";
            try
            {
                collection.Add(new JsonStringValue("type", "select"));
                collection.Add(new JsonStringValue("liststr", createList(jp.getValue("Name"), int.Parse(jp.getValue("page")))));
            }
            catch
            { isok = "0"; }
            collection.Add(new JsonStringValue("flag", isok));

            return collection.ToString();
        }
        private string jumpaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string isok = "1";
            try
            {
                collection.Add(new JsonStringValue("type", "jump"));
                collection.Add(new JsonStringValue("liststr", createList(jp.getValue("Name"), int.Parse(jp.getValue("page")))));
            }
            catch
            { isok = "0"; }
            collection.Add(new JsonStringValue("flag", isok));

            return collection.ToString();
        }

    }
}