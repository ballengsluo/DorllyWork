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
    public partial class WorkOrderLog : AbstractPmPage, System.Web.UI.ICallbackEventHandler
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
                    CheckRight(user.Entity, "order/WorkOrderLog.aspx");

                    if (!Page.IsCallback)
                    {
                        list = createList(string.Empty, string.Empty, string.Empty, string.Empty, GetDate().ToString("yyyy-MM-dd"), GetDate().ToString("yyyy-MM-dd"), 1);
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
        private string createList(string OrderNo, string LogUser, string CustNo, string LogType, string MinLogDate, string MaxLogDate, int page)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder("");

            sb.Append("<table class=\"table table-border table-bordered table-hover table-bg table-sort\" id=\"tablelist\">");
            sb.Append("<thead>");
            sb.Append("<tr class=\"text-c\">");
            sb.Append("<th width=\"5%\">序号</th>");
            sb.Append("<th width='10%'>日志日期</th>");
            sb.Append("<th width='10%'>日志类型</th>");
            sb.Append("<th width='10%'>操作人</th>");
            sb.Append("<th width='10%'>工单号</th>");
            sb.Append("<th width='14%'>客户</th>");
            sb.Append("<th width='10%'>X坐标</th>");
            sb.Append("<th width='10%'>Y坐标</th>");
            sb.Append("<th width='23%'>说明</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            
            DateTime MinLogDateS = default(DateTime);
            DateTime MaxLogDateS = default(DateTime);
            if (MinLogDate != "") MinLogDateS = ParseDateForString(MinLogDate);
            if (MaxLogDate != "") MaxLogDateS = ParseDateForString(MaxLogDate);
            
            int r = 1;
            sb.Append("<tbody>");
            Business.Order.BusinessWorkOrderLog bc = new project.Business.Order.BusinessWorkOrderLog();
            foreach (Entity.Order.EntityWorkOrderLog it in bc.GetWorkOrderLogQuery(user.Entity.AccID, OrderNo, LogUser, CustNo, LogType, MinLogDateS, MaxLogDateS, page, pageSize))
            {
                sb.Append("<tr class=\"text-c\" id=\"" + it.InnerEntityOID + "\">");
                sb.Append("<td align='center'>" + r.ToString() + "</td>");
                sb.Append("<td>" + it.LogDate.ToString("MM-dd HH:ss") + "</td>");
                sb.Append("<td>" + it.LogTypeName + "</td>");
                sb.Append("<td>" + it.LogUserName + "</td>");
                sb.Append("<td>" + it.OrderNo + "</td>");
                sb.Append("<td>" + it.CustName + "</td>");
                sb.Append("<td>" + it.GPS_X + "</td>");
                sb.Append("<td>" + it.GPS_Y + "</td>");
                sb.Append("<td>" + it.Remark + "</td>");
                sb.Append("</tr>");

                r++;
            }
            sb.Append("</tbody>");
            sb.Append("</table>");

            sb.Append(Paginat(bc.GetWorkOrderLogCount(user.Entity.AccID, OrderNo, LogUser, CustNo, LogType, MinLogDateS, MaxLogDateS), pageSize, page, 7));
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
            string flag = "1";

            collection.Add(new JsonStringValue("type", "select"));
            collection.Add(new JsonStringValue("flag", flag));
            collection.Add(new JsonStringValue("liststr", createList(jp.getValue("OrderNo"), jp.getValue("LogUser"), jp.getValue("CustNo"), 
                jp.getValue("LogType"), jp.getValue("MinLogDate"), jp.getValue("MaxLogDate"), int.Parse(jp.getValue("page")) )));

            return collection.ToString();
        }        
    }
}