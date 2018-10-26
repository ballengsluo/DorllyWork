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
    public partial class FlowNode : AbstractPmPage, System.Web.UI.ICallbackEventHandler
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
                    CheckRight(user.Entity, "pm/Base/FlowNode.aspx");

                    if (!Page.IsCallback)
                    {
                        list = createList();
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

        private string createList()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder("");

            sb.Append("<table class=\"table table-border table-bordered table-hover table-bg\" id=\"tablelist\">");
            sb.Append("<thead>");
            sb.Append("<tr class=\"text-c\">");
            sb.Append("<th width=\"30\">序号</th>");
            sb.Append("<th width='100'>流程编号</th>");
            sb.Append("<th width='100'>流程名称</th>");
            sb.Append("<th width='200'>操作内容</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");

            int r = 1;
            sb.Append("<tbody>");
            Business.Base.BusinessFlowNode bc = new Business.Base.BusinessFlowNode();
            foreach (Entity.Base.EntityFlowNode it in bc.GetFlowNodeListQuery(string.Empty, string.Empty, user.Entity.AccID))
            {
                if (it.NodeNo == "N") continue;
                sb.Append("<tr class=\"text-c\" id=\"" + it.NodeNo + "\">");
                sb.Append("<td align='center'>" + r.ToString() + "</td>");
                sb.Append("<td>" + it.NodeNo + "</td>");
                sb.Append("<td>" + it.NodeName + "</td>");
                sb.Append("<td>" + it.OpName + "</td>");
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
            if (jp.getValue("Type") == "update")
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
                Business.Base.BusinessFlowNode bc = new project.Business.Base.BusinessFlowNode();
                bc.load(jp.getValue("id"), user.Entity.AccID);

                collection.Add(new JsonStringValue("NodeNo", bc.Entity.NodeNo));
                collection.Add(new JsonStringValue("NodeName", bc.Entity.NodeName));
                collection.Add(new JsonStringValue("OpNo", bc.Entity.OpNo));
                collection.Add(new JsonStringValue("OpName", bc.Entity.OpName));
            }
            catch
            { flag = "2"; }

            collection.Add(new JsonStringValue("type", "update"));
            collection.Add(new JsonStringValue("flag", flag));

            result = collection.ToString();

            return result;
        }

        private string submitaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";
            try
            {
                Business.Base.BusinessFlowNode bc = new project.Business.Base.BusinessFlowNode();

                bc.load(jp.getValue("id"), user.Entity.AccID);
                bc.Entity.OpNo = jp.getValue("OpNo");
                bc.Entity.OpName = jp.getValue("OpName");
                int r = bc.Save("update");
                if (r <= 0)
                    flag = "2";
            }
            catch { flag = "2"; }

            collection.Add(new JsonStringValue("type", "submit"));
            collection.Add(new JsonStringValue("flag", flag));
            collection.Add(new JsonStringValue("liststr", createList()));

            return collection.ToString();
        }

    }
}