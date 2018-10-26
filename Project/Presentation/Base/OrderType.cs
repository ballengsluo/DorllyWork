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
    public partial class OrderType : AbstractPmPage, System.Web.UI.ICallbackEventHandler
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
                    CheckRight(user.Entity, "pm/Base/OrderType.aspx");

                    if (!Page.IsCallback)
                    {
                        list = createList();

                        flow = "<select class=\"input-text required\" id=\"FlowNo\" data-valid=\"isNonEmpty\" data-error=\"请选择流程类型\">";
                        flow += "<option value=\"\" selected>请选择流程类型</option>";

                        Business.Base.BusinessFlow fw = new project.Business.Base.BusinessFlow();
                        foreach (Entity.Base.EntityFlow it in fw.GetFlowListQuery(string.Empty, string.Empty,user.Entity.AccID))
                        {
                            flow += "<option value='" + it.FlowNo + "'>" + it.FlowName + "</option>";
                        }
                        flow += "</select>";
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
        protected string flow = "";

        private string createList()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder("");

            sb.Append("<table class=\"table table-border table-bordered table-hover table-bg\" id=\"tablelist\">");
            sb.Append("<thead>");
            sb.Append("<tr class=\"text-c\">");
            sb.Append("<th width=\"30\">序号</th>");
            sb.Append("<th width='100'>工单类型编号</th>");
            sb.Append("<th width='150'>工单类型名称</th>");
            sb.Append("<th width='100'>所属流程</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");

            int r = 1;
            sb.Append("<tbody>");
            Business.Base.BusinessOrderType bc = new Business.Base.BusinessOrderType();
            foreach (Entity.Base.EntityOrderType it in bc.GetOrderTypeListQuery(string.Empty, string.Empty, string.Empty, user.Entity.AccID))
            {
                sb.Append("<tr class=\"text-c\" id=\"" + it.OrderTypeNo + "\">");
                sb.Append("<td align='center'>" + r.ToString() + "</td>");
                sb.Append("<td>" + it.OrderTypeNo + "</td>");
                sb.Append("<td>" + it.OrderTypeName + "</td>");
                sb.Append("<td>" + it.FlowName + "</td>");
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
                Business.Base.BusinessOrderType bc = new project.Business.Base.BusinessOrderType();
                bc.load(jp.getValue("id"), user.Entity.AccID);

                collection.Add(new JsonStringValue("OrderTypeNo", bc.Entity.OrderTypeNo));
                collection.Add(new JsonStringValue("OrderTypeName", bc.Entity.OrderTypeName));
                collection.Add(new JsonStringValue("FlowNo", bc.Entity.FlowNo));
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
                Business.Base.BusinessOrderType bc = new project.Business.Base.BusinessOrderType();
                bc.load(jp.getValue("id"), user.Entity.AccID);
                if (obj.ExecuteDataSet("select 1 from WO_WorkOrder where OrderType='" + bc .Entity.OrderTypeNo + "' and AccID='"+user.Entity.AccID+"'").Tables[0].Rows.Count > 0)
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
                Business.Base.BusinessOrderType bc = new project.Business.Base.BusinessOrderType();
                if (jp.getValue("tp") == "update")
                {
                    bc.load(jp.getValue("id"),user.Entity.AccID);
                    bc.Entity.OrderTypeName = jp.getValue("OrderTypeName");
                    bc.Entity.FlowNo = jp.getValue("FlowNo");
                    int r = bc.Save("update");
                    if (r <= 0)
                        flag = "2";
                }
                else
                {
                    Data obj = new Data();
                    DataTable dt = obj.ExecuteDataSet("select 1 from Base_Order_Type where OrderTypeNo=N'" + jp.getValue("OrderTypeNo") + "' and AccID='"+user.Entity.AccID+"'").Tables[0];
                    if (dt.Rows.Count>0)
                        flag = "3";
                    else
                    {
                        bc.Entity.OrderTypeNo = jp.getValue("OrderTypeNo");
                        bc.Entity.OrderTypeName = jp.getValue("OrderTypeName");
                        bc.Entity.FlowNo = jp.getValue("FlowNo");
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