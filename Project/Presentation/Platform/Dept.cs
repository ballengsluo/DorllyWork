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

namespace project.Presentation.Sys
{
    public partial class Dept : AbstractPmPage, System.Web.UI.ICallbackEventHandler
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
                    CheckRight(user.Entity, "pm/Platform/Dept.aspx");

                    if (!Page.IsCallback)
                        list = createList();
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
        private string createList()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder("");

            sb.Append("<table class=\"table table-border table-bordered table-hover table-bg\" id=\"tablelist\">");
            sb.Append("<thead>");
            sb.Append("<tr class=\"text-c\">");
            sb.Append("<th width=\"30\">序号</th>");
            sb.Append("<th width='100'>部门编号</th>");
            sb.Append("<th width='150'>部门名称</th>");
            sb.Append("<th width='150'>负责人</th>");
            sb.Append("<th width='300'>备注</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");

            int r = 1;
            sb.Append("<tbody>");
            Business.Sys.BusinessDept bc = new Business.Sys.BusinessDept();
            foreach (Entity.Sys.EntityDept it in bc.GetDeptListQuery(string.Empty, string.Empty, user.Entity.AccID, "null"))
            {
                sb.Append("<tr class=\"text-c\" id=\"" + it.DeptNo + "\">");
                sb.Append("<td align='center'>" + r.ToString() + "</td>");
                sb.Append("<td>" + it.DeptNo + "</td>");
                sb.Append("<td style='text-align:left'>" + it.DeptName + "</td>");
                sb.Append("<td>" + it.ManagerName + "</td>");
                sb.Append("<td>" + it.Remark + "</td>");
                sb.Append("</tr>");
                r++;

                foreach (Entity.Sys.EntityDept it1 in bc.GetDeptListQuery(string.Empty, string.Empty, user.Entity.AccID, it.DeptNo))
                {
                    sb.Append("<tr class=\"text-c\" id=\"" + it1.DeptNo + "\">");
                    sb.Append("<td align='center'>" + r.ToString() + "</td>");
                    sb.Append("<td>" + it1.DeptNo + "</td>");
                    sb.Append("<td style='text-align:left'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + it1.DeptName + "</td>");
                    sb.Append("<td>" + it1.ManagerName + "</td>");
                    sb.Append("<td>" + it1.Remark + "</td>");
                    sb.Append("</tr>");
                    r++;

                    foreach (Entity.Sys.EntityDept it2 in bc.GetDeptListQuery(string.Empty, string.Empty, user.Entity.AccID, it1.DeptNo))
                    {
                        sb.Append("<tr class=\"text-c\" id=\"" + it2.DeptNo + "\">");
                        sb.Append("<td align='center'>" + r.ToString() + "</td>");
                        sb.Append("<td>" + it2.DeptNo + "</td>");
                        sb.Append("<td style='text-align:left'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + it2.DeptName + "</td>");
                        sb.Append("<td>" + it2.ManagerName + "</td>");
                        sb.Append("<td>" + it2.Remark + "</td>");
                        sb.Append("</tr>");
                        r++;
                    }
                }

            }
            sb.Append("</tbody>");
            sb.Append("</table>");
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
            return result;
        }

        private string updateaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";
            string result = ""; ;
            try
            {
                Business.Sys.BusinessDept bc = new project.Business.Sys.BusinessDept();
                bc.load(jp.getValue("id"), user.Entity.AccID);

                collection.Add(new JsonStringValue("DeptNo", bc.Entity.DeptNo));
                collection.Add(new JsonStringValue("DeptName", bc.Entity.DeptName));
                collection.Add(new JsonStringValue("Parent", bc.Entity.Parent));
                collection.Add(new JsonStringValue("ParentName", bc.Entity.ParentName));
                collection.Add(new JsonStringValue("Manager", bc.Entity.Manager));
                collection.Add(new JsonStringValue("ManagerName", bc.Entity.ManagerName));
                collection.Add(new JsonStringValue("Remark", bc.Entity.Remark));
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
                Business.Sys.BusinessDept bc = new project.Business.Sys.BusinessDept();
                bc.load(jp.getValue("id"), user.Entity.AccID);
                if (obj.ExecuteDataSet("select 1 from Sys_User_Info where DeptNo='" + bc.Entity.DeptNo + "' and AccID='" + user.Entity.AccID + "'").Tables[0].Rows.Count > 0)
                {
                    flag = "3";
                }
                else
                {
                    if (obj.ExecuteDataSet("select 1 from Sys_Dept where Parent='" + bc.Entity.DeptNo + "' and AccID='" + user.Entity.AccID + "'").Tables[0].Rows.Count > 0)
                    {
                        flag = "3";
                    }
                    else
                    {
                        int r = bc.delete();
                        if (r <= 0)
                            flag = "2";
                    }
                }
            }
            catch { flag = "2"; }

            collection.Add(new JsonStringValue("type", "delete"));
            collection.Add(new JsonStringValue("flag", flag));
            collection.Add(new JsonStringValue("liststr", createList()));

            return collection.ToString();
        }

        private string submitaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";
            int level = 1;
            try
            {
                if (jp.getValue("Parent") != "")
                {
                    Business.Sys.BusinessDept parent = new project.Business.Sys.BusinessDept();
                    parent.load(jp.getValue("Parent"),user.Entity.AccID);
                    level = parent.Entity.Level;
                }

                Business.Sys.BusinessDept bc = new project.Business.Sys.BusinessDept();
                if (jp.getValue("tp") == "update")
                {
                    bc.load(jp.getValue("id"), user.Entity.AccID);
                    bc.Entity.DeptName = jp.getValue("DeptName");
                    bc.Entity.Manager = jp.getValue("Manager");
                    bc.Entity.Parent = jp.getValue("Parent");
                    bc.Entity.Remark = jp.getValue("Remark");
                    bc.Entity.Level = level;
                    int r = bc.Save("update");
                    if (r <= 0)
                        flag = "2";
                }
                else
                {
                    Data obj = new Data();
                    DataTable dt = obj.ExecuteDataSet("select 1 from Sys_Dept where DeptNo=N'" + jp.getValue("DeptNo") + "' and AccID='" + user.Entity.AccID + "'").Tables[0];
                    if (dt.Rows.Count > 0)
                        flag = "3";
                    else
                    {
                        bc.Entity.DeptNo = jp.getValue("DeptNo");
                        bc.Entity.DeptName = jp.getValue("DeptName");
                        bc.Entity.Manager = jp.getValue("Manager");
                        bc.Entity.Parent = jp.getValue("Parent");
                        bc.Entity.Remark = jp.getValue("Remark");
                        bc.Entity.Level = level;
                        bc.Entity.AccID = user.Entity.AccID;
                        int r = bc.Save("insert");
                        if (r <= 0)
                            flag = "2";
                    }
                }
            }
            catch { flag = "2"; }

            collection.Add(new JsonStringValue("type", "submit"));
            collection.Add(new JsonStringValue("flag", flag));
            collection.Add(new JsonStringValue("liststr", createList()));

            return collection.ToString();
        }
    }
}