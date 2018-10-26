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
    public partial class UserType : AbstractPmPage, System.Web.UI.ICallbackEventHandler
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
                    CheckRight(user.Entity, "pm/Platform/UserType.aspx");

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
            sb.Append("<th width='100'>类型编号</th>");
            sb.Append("<th width='150'>类型名称</th>");
            sb.Append("<th width='300'>处理工单类型</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");

            int r = 1;
            sb.Append("<tbody>");
            Business.Sys.BusinessUserType bc = new Business.Sys.BusinessUserType();
            foreach (Entity.Sys.EntityUserType it in bc.GetUserTypeListQuery(string.Empty, string.Empty, user.Entity.AccID, string.Empty))
            {
                sb.Append("<tr class=\"text-c\" id=\"" + it.UserTypeNo + "\">");
                sb.Append("<td align='center'>" + r.ToString() + "</td>");
                sb.Append("<td>" + it.UserTypeNo + "</td>");
                sb.Append("<td>" + it.UserTypeName + "</td>");
                sb.Append("<td>" + it.OrderTypeName + "</td>");
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
            return result;
        }

        private string updateaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";
            string result = ""; ;
            try
            {
                Business.Sys.BusinessUserType bc = new project.Business.Sys.BusinessUserType();
                bc.load(jp.getValue("id"), user.Entity.AccID);

                collection.Add(new JsonStringValue("UserTypeNo", bc.Entity.UserTypeNo));
                collection.Add(new JsonStringValue("UserTypeName", bc.Entity.UserTypeName));
                collection.Add(new JsonStringValue("OrderType", bc.Entity.OrderType));
                collection.Add(new JsonStringValue("OrderTypeName", bc.Entity.OrderTypeName));
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
                if (jp.getValue("id").ToUpper() == "ADMIN")
                {
                    flag = "4";
                }
                else
                {
                    Business.Sys.BusinessUserType bc = new project.Business.Sys.BusinessUserType();
                    bc.load(jp.getValue("id"), user.Entity.AccID);
                    if (obj.ExecuteDataSet("select 1 from Sys_User_Info where UserType='" + bc.Entity.UserTypeNo + "' and AccID='" + user.Entity.AccID + "'").Tables[0].Rows.Count > 0)
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
            try
            {
                Business.Sys.BusinessUserType bc = new project.Business.Sys.BusinessUserType();
                if (jp.getValue("tp") == "update")
                {
                    bc.load(jp.getValue("id"), user.Entity.AccID);
                    bc.Entity.UserTypeName = jp.getValue("UserTypeName");
                    bc.Entity.OrderType = jp.getValue("OrderType");
                    bc.Entity.OrderTypeName = jp.getValue("OrderTypeName");
                    int r = bc.Save("update");
                    if (r <= 0)
                        flag = "2";
                }
                else
                {
                    Data obj = new Data();
                    DataTable dt = obj.ExecuteDataSet("select 1 from Sys_User_Type where UserTypeNo=N'" + jp.getValue("UserTypeNo") + "' and AccID='" + user.Entity.AccID + "'").Tables[0];
                    if (dt.Rows.Count > 0)
                        flag = "3";
                    else
                    {
                        bc.Entity.UserTypeNo = jp.getValue("UserTypeNo");
                        bc.Entity.UserTypeName = jp.getValue("UserTypeName");
                        bc.Entity.OrderType = jp.getValue("OrderType");
                        bc.Entity.OrderTypeName = jp.getValue("OrderTypeName");
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