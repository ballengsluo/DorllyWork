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

namespace project.Presentation.Platform
{
    public partial class UserInfo : AbstractPmPage, System.Web.UI.ICallbackEventHandler
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
                    userid = Encrypt.DecryptDES(str,"1");
                    user.load(userid);
                    CheckRight(user.Entity, "pm/Platform/UserInfo.aspx");

                    if (!Page.IsCallback)
                    {
                        list = createList(string.Empty, string.Empty);
                        userType = "<select class=\"input-text required\" id=\"UserType\" data-valid=\"isNonEmpty\" data-error=\"请选择用户类型\">";
                        userType += "<option value=\"\" selected>请选择用户类型</option>";

                        Business.Sys.BusinessUserType tp = new project.Business.Sys.BusinessUserType();
                        foreach (Entity.Sys.EntityUserType it in tp.GetUserTypeListQuery(string.Empty, string.Empty, user.Entity.AccID, string.Empty))
                        {
                            userType += "<option value='" + it.UserTypeNo + "'>" + it.UserTypeName + "</option>";
                        }
                        userType += "</select>";

                        dept = "<select class=\"input-text required\" id=\"DeptNo\" data-valid=\"isNonEmpty\" data-error=\"请选择部门\">";
                        dept += "<option value=\"\" selected>请选择部门</option>";

                        Business.Sys.BusinessDept dt = new project.Business.Sys.BusinessDept();
                        foreach (Entity.Sys.EntityDept it in dt.GetDeptListQuery(string.Empty, string.Empty, user.Entity.AccID, string.Empty))
                        {
                            dept += "<option value='" + it.DeptNo + "'>" + it.DeptName + "</option>";
                        }
                        dept += "</select>";
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
        protected string userType = "";
        protected string dept = "";
        private string createList(string DeptNo, string UserName)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder("");

            sb.Append("<table class=\"table table-border table-bordered table-hover table-bg table-sort\" id=\"tablelist\">");
            sb.Append("<thead>");
            sb.Append("<tr class=\"text-c\">");
            sb.Append("<th width=\"5%\">序号</th>");
            sb.Append("<th width='10%'>用户ID</th>");
            sb.Append("<th width='10%'>用户姓名</th>");
            sb.Append("<th width='10%'>用户类别</th>");
            sb.Append("<th width='10%'>部门</th>");
            sb.Append("<th width='10%'>上级经理</th>");
            sb.Append("<th width='15%'>电话</th>");
            sb.Append("<th width='15%'>Email</th>");
            sb.Append("<th width='10%'>创建日期</th>");
            sb.Append("<th width='5%'>状态</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");

            int r = 1;
            sb.Append("<tbody>");
            Business.Sys.BusinessUserInfo bc = new project.Business.Sys.BusinessUserInfo();
            foreach (Entity.Sys.EntityUserInfo it in bc.GetUserInfoListQuery(string.Empty, user.Entity.AccID, DeptNo, UserName))
            {
                sb.Append("<tr class=\"text-c\" id=\"" + it.InnerEntityOID + "\">");
                sb.Append("<td align='center'>" + r.ToString() + "</td>");
                sb.Append("<td>" + it.UserNo + "</td>");
                sb.Append("<td>" + it.UserName + "</td>");
                sb.Append("<td>" + it.UserTypeName + "</td>");
                sb.Append("<td>" + it.DeptName + "</td>");
                sb.Append("<td>" + it.ManagerName + "</td>");
                sb.Append("<td>" + it.Tel + "</td>");
                sb.Append("<td>" + it.Email + "</td>");
                sb.Append("<td>" + ParseStringForDate(it.RegDate) + "</td>");
                sb.Append("<td class=\"td-status\"><span class=\"label " + (it.Valid ? "label-success" : "") + " radius\">" + (it.Valid ? "有效" : "已失效") + "</span></td>");
                sb.Append("</tr>");
                r++;
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
            else if (jp.getValue("Type") == "select")
                result = selectaction(jp);
            else if (jp.getValue("Type") == "newpassword")
                result = newpasswordaction(jp);
            else if (jp.getValue("Type") == "valid")
                result = validaction(jp);
            return result;
        }

        private string updateaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";
            string result = ""; ;
            try
            {
                Business.Sys.BusinessUserInfo bc = new project.Business.Sys.BusinessUserInfo();
                bc.load(jp.getValue("id"));

                collection.Add(new JsonStringValue("UserNo", bc.Entity.UserNo));
                collection.Add(new JsonStringValue("UserName", bc.Entity.UserName));
                collection.Add(new JsonStringValue("UserType", bc.Entity.UserType));
                collection.Add(new JsonStringValue("DeptNo", bc.Entity.DeptNo));
                collection.Add(new JsonStringValue("Tel", bc.Entity.Tel));
                collection.Add(new JsonStringValue("Email", bc.Entity.Email));
                collection.Add(new JsonStringValue("Addr", bc.Entity.Addr));
                collection.Add(new JsonStringValue("Manager", bc.Entity.Manager));
                collection.Add(new JsonStringValue("ManagerName", bc.Entity.ManagerName));
                collection.Add(new JsonStringValue("Picture", bc.Entity.Picture));
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
                Business.Sys.BusinessUserInfo bc = new project.Business.Sys.BusinessUserInfo();
                bc.load(jp.getValue("id"));
                if (bc.Entity.UserNo.ToUpper() == "ADMIN")
                    flag = "3";
                else
                {
                    int r = bc.delete();
                    if (r <= 0)
                        flag = "2";
                }
            }
            catch { flag = "2"; }

            collection.Add(new JsonStringValue("type", "delete"));
            collection.Add(new JsonStringValue("flag", flag));
            collection.Add(new JsonStringValue("liststr", createList(string.Empty, jp.getValue("UserNameS"))));

            return collection.ToString();
        }
        private string submitaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";
            try
            {
                Business.Sys.BusinessUserInfo bc = new project.Business.Sys.BusinessUserInfo();
                if (jp.getValue("tp") == "update")
                {
                    bc.load(jp.getValue("id"));
                    bc.Entity.UserName = jp.getValue("UserName");
                    bc.Entity.UserType = jp.getValue("UserType");
                    bc.Entity.Tel = jp.getValue("Tel");
                    bc.Entity.Addr = jp.getValue("Addr");
                    bc.Entity.Email = jp.getValue("Email");
                    bc.Entity.DeptNo = jp.getValue("DeptNo");
                    bc.Entity.Manager = jp.getValue("Manager");
                    bc.Entity.Picture = jp.getValue("Picture");
                    int r = bc.Save();
                    if (r <= 0)
                        flag = "2";
                }
                else
                {
                    Data obj = new Data();
                    DataTable dt = obj.ExecuteDataSet("select cnt=COUNT(*) from Sys_User_Info where UserNo=N'" + jp.getValue("UserNo") + "' and AccID='"+user.Entity.AccID+"'").Tables[0];
                    if (int.Parse(dt.Rows[0]["cnt"].ToString()) > 0)
                        flag = "3";
                    else
                    {
                        bc.Entity.UserNo = jp.getValue("UserNo");
                        bc.Entity.UserName = jp.getValue("UserName");
                        bc.Entity.UserType = jp.getValue("UserType");
                        bc.Entity.Tel = jp.getValue("Tel");
                        bc.Entity.Addr = jp.getValue("Addr");
                        bc.Entity.Email = jp.getValue("Email");
                        bc.Entity.DeptNo = jp.getValue("DeptNo");
                        bc.Entity.Manager = jp.getValue("Manager");
                        bc.Entity.Picture = jp.getValue("Picture");
                        bc.Entity.AccID = user.Entity.AccID;
                        bc.Entity.Valid = true;
                        bc.Entity.RegDate=GetDate();

                        string password = getRandom();

                        bc.Entity.Password = Encrypt.EncryptDES(password, "1");
                        int r = bc.Save();
                        if (r <= 0)
                            flag = "2";

                        collection.Add(new JsonStringValue("password", password));
                    }
                }
            }
            catch { flag = "2"; }

            collection.Add(new JsonStringValue("type", "submit"));
            collection.Add(new JsonStringValue("flag", flag));
            collection.Add(new JsonStringValue("liststr", createList(string.Empty, jp.getValue("UserNameS"))));

            return collection.ToString();
        }

        private string selectaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";

            collection.Add(new JsonStringValue("type", "select"));
            collection.Add(new JsonStringValue("flag", flag));
            collection.Add(new JsonStringValue("liststr", createList(string.Empty, jp.getValue("UserNameS"))));

            return collection.ToString();
        }

        private string newpasswordaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";
            try
            {
                Business.Sys.BusinessUserInfo bc = new project.Business.Sys.BusinessUserInfo();
                bc.load(jp.getValue("id"));
                string newpwd = getRandom();

                bc.Entity.Password = Encrypt.EncryptDES(newpwd, "1");
                int r = bc.changepwd();
                if (r <= 0)
                    flag = "2";

                collection.Add(new JsonStringValue("type", "newpassword"));
                collection.Add(new JsonStringValue("flag", flag));
                collection.Add(new JsonStringValue("newpassword", newpwd));
            }
            catch
            { flag = "2"; }
            return collection.ToString();
        }

        private string validaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";
            try
            {
                Business.Sys.BusinessUserInfo bc = new project.Business.Sys.BusinessUserInfo();
                bc.load(jp.getValue("id"));
                bc.Entity.Valid = !bc.Entity.Valid;

                int r = bc.valid();
                if (r <= 0) flag = "2";
                if (bc.Entity.Valid)
                    collection.Add(new JsonStringValue("stat", "<span class=\"label label-success radius\">有效</span>"));
                else
                    collection.Add(new JsonStringValue("stat", "<span class=\"label radius\">已失效</span>"));
                collection.Add(new JsonStringValue("id", jp.getValue("id")));
            }
            catch { flag = "2"; }

            collection.Add(new JsonStringValue("flag", flag));
            collection.Add(new JsonStringValue("type", "valid"));
            return collection.ToString();
        }
        
    }
}