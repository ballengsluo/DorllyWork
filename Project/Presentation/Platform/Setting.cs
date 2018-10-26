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
    public partial class Setting : AbstractPmPage, System.Web.UI.ICallbackEventHandler
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
                    CheckRight(user.Entity, "pm/Platform/Setting.aspx");

                    if (!Page.IsCallback)
                    {
                        list = createList();
                    }
                }
                else
                {
                    Response.Write(errorpage);
                    return;
                }
            }
            catch
            {
                Response.Write(errorpage);
                return;
            }
        }

        Data obj = new Data();
        protected string list = "";
        private string createList()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder("");

            sb.Append("<table class=\"table table-border table-bordered table-hover table-bg table-sort\" id=\"tablelist\">");
            sb.Append("<thead>");
            sb.Append("<tr class=\"text-c\">");
            sb.Append("<th width=\"10%\">序号</th>");
            sb.Append("<th width='30%'>项目</th>");
            sb.Append("<th width='30%'>内容</th>");
            sb.Append("<th width='30%'>操作</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");

            int r = 1;
            sb.Append("<tbody>");
            Business.Sys.BusinessSetting bc = new project.Business.Sys.BusinessSetting();
            foreach (Entity.Sys.EntitySetting it in bc.GetListQuery())
            {
                sb.Append("<tr class=\"text-c\" id=\"" + it.SettingCode + "\">");
                sb.Append("<td>" + r.ToString() + "</td>");
                sb.Append("<td>" + it.SettingName + "</td>");
                if (it.SettingType == "String")
                    sb.Append("<td><input class=\"input-text size-S\" type=\"text\" id=\"Val" + it.SettingCode + "\" value=\"" + it.StringValue + "\" /></td>");
                else if (it.SettingType == "Int")
                    sb.Append("<td><input class=\"input-text size-S\" type=\"text\" id=\"Val" + it.SettingCode + "\" value=\"" + it.IntValue.ToString() + "\" onblur=\"validInt(this.id)\" /></td>");
                else if (it.SettingType == "Decimal")
                    sb.Append("<td><input class=\"input-text size-S\" type=\"text\" id=\"Val" + it.SettingCode + "\" value=\"" + it.DecimalValue.ToString("0.####") + "\" onblur=\"validDecimal(this.id)\" /></td>");
                else
                    sb.Append("<td><input type=\"hidden\" id=\"Val" + it.SettingCode + "\" /></td>");
                sb.Append("<td><input class=\"btn btn-primary radius size-S\" type=\"button\" onclick=\"save('" + it.SettingCode + "')\" value=\"&nbsp;保&nbsp;&nbsp;存&nbsp;\" /></td>");
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
            if (jp.getValue("Type") == "submit")
                result = submitaction(jp);
            return result;
        }
        private string submitaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";
            try
            {
                Business.Sys.BusinessSetting bc = new project.Business.Sys.BusinessSetting();

                bc.load(jp.getValue("id"));
                if (bc.Entity.SettingType == "String")
                    bc.Entity.StringValue = jp.getValue("val");
                else if (bc.Entity.SettingType == "Int")
                    bc.Entity.IntValue = ParseIntForString(jp.getValue("val"));
                else if (bc.Entity.SettingType == "Decimal")
                    bc.Entity.DecimalValue = ParseDecimalForString(jp.getValue("val"));

                int r = bc.Save("update");

                if (r <= 0)
                    flag = "2";
            }
            catch { flag = "2"; }


            collection.Add(new JsonStringValue("type", "submit"));
            collection.Add(new JsonStringValue("flag", flag));
            return collection.ToString();
        }
    }
}