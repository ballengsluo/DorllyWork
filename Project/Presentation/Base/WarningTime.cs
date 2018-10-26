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
    public partial class WarningTime : AbstractPmPage, System.Web.UI.ICallbackEventHandler
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
                    CheckRight(user.Entity, "pm/Base/WarningTime.aspx");

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
            sb.Append("<th width=\"10%\">序号</th>");
            sb.Append("<th width='30%'>预警类型</th>");
            sb.Append("<th width='45%'>预警时长(分钟)</th>");
            sb.Append("<th width='15%'>操作</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");

            int r = 1;
            sb.Append("<tbody>");
            Business.Base.BusinessWarningTime bc = new Business.Base.BusinessWarningTime();
            foreach (Entity.Base.EntityWarningTime it in bc.GetWarningTimeListQuery(user.Entity.AccID))
            {
                sb.Append("<tr class=\"text-c\" id=\"" + it.InnerEntityOID + "\">");
                sb.Append("<td align='center'>" + r.ToString() + "</td>");
                sb.Append("<td>" + it.ParaName + "</td>");
                sb.Append("<td><input type=\"text\" class=\"input-text size-S\" style=\"width:200px\" placeholder=\"\" id=\"Time" + it.InnerEntityOID + "\" onblur=\"validInt(this.id)\" value=\"" + it.Time.ToString() + "\"></td>");
                sb.Append("<td><a href=\"javascript:;\" onclick=\"save('" + it.InnerEntityOID + "')\" class=\"btn btn-primary radius\"><i class=\"Hui-iconfont\">&#xe632;</i> 保存</a></td>");
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
            if (jp.getValue("Type") == "save")
                result = saveaction(jp);
            return result;
        }

        private string saveaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";
            try
            {
                Business.Base.BusinessWarningTime bc = new project.Business.Base.BusinessWarningTime();
                bc.load(jp.getValue("id"), user.Entity.AccID);
                bc.Entity.Time = int.Parse(jp.getValue("Time"));
                int r = bc.Save();
                if (r <= 0)
                    flag = "2";
            }
            catch { flag = "2"; }

            collection.Add(new JsonStringValue("type", "save"));
            collection.Add(new JsonStringValue("flag", flag));

            return collection.ToString();
        }
    }
}