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
    public partial class WorkOrderFee : AbstractPmPage, System.Web.UI.ICallbackEventHandler
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
                    CheckRight(user.Entity, "order/WorkOrderFee.aspx");

                    if (!Page.IsCallback)
                    {
                        list = createList(string.Empty, string.Empty, GetDate().AddDays(-GetDate().Day + 1).ToString("yyyy-MM-dd"), GetDate().ToString("yyyy-MM-dd"), 1);
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
        private string createList(string OrderNo, string Status, string MinFeeDate, string MaxFeeDate, int page)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder("");

            sb.Append("<table class=\"table table-border table-bordered table-hover table-bg table-sort\" id=\"tablelist\">");
            sb.Append("<thead>");
            sb.Append("<tr class=\"text-c\">");
            sb.Append("<th width=\"5%\">序号</th>");
            sb.Append("<th width='17%'>工单日期</th>");
            sb.Append("<th width='17%'>工单号</th>");
            sb.Append("<th width='32%'>工单内容</th>");
            sb.Append("<th width='17%'>收款金额</th>");
            sb.Append("<th width='12%'>状态</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");

            DateTime MinFeeDateS = default(DateTime);
            DateTime MaxFeeDateS = default(DateTime);
            if (MinFeeDate != "") MinFeeDateS = ParseDateForString(MinFeeDate);
            if (MaxFeeDate != "") MaxFeeDateS = ParseDateForString(MaxFeeDate);

            int r = 1;
            sb.Append("<tbody>");
            Business.Order.BusinessWorkOrderFee bc = new project.Business.Order.BusinessWorkOrderFee();
            foreach (Entity.Order.EntityWorkOrderFee it in bc.GetWorkOrderFeeQuery(user.Entity.AccID, string.Empty, OrderNo, Status, MinFeeDateS, MaxFeeDateS, "1", page, pageSize))
            {
                sb.Append("<tr class=\"text-c\" id=\"" + it.InnerEntityOID + "\">");
                sb.Append("<td align='center'>" + r.ToString() + "</td>");
                sb.Append("<td>" + it.OrderDate.ToString("MM-dd HH:mm") + "</td>");
                sb.Append("<td>" + it.OrderNo + "</td>");
                sb.Append("<td>" + it.OrderName + "</td>");
                sb.Append("<td>" + it.FeeAmount.ToString("0.##") + "</td>");
                sb.Append("<td class=\"td-status\"><span class=\"label " + (it.Status=="OPEN" ? "" : "label-success") + " radius\">" + it.StatusName + "</span></td>");
                sb.Append("</tr>");

                r++;
            }
            sb.Append("</tbody>");
            sb.Append("</table>");

            sb.Append(Paginat(bc.GetWorkOrderFeeCount(user.Entity.AccID, string.Empty, OrderNo, Status, MinFeeDateS, MaxFeeDateS, "1"), pageSize, page, 7));
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
            else if (jp.getValue("Type") == "detail")
                result = detailaction(jp);
            else if (jp.getValue("Type") == "save")
                result = saveaction(jp);
            else if (jp.getValue("Type") == "approve")
                result = approveaction(jp);
            return result;
        }

        private string selectaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";

            collection.Add(new JsonStringValue("type", "select"));
            collection.Add(new JsonStringValue("flag", flag));
            collection.Add(new JsonStringValue("liststr", createList(jp.getValue("OrderNo"), jp.getValue("Status"), jp.getValue("MinFeeDate"), jp.getValue("MaxFeeDate"), int.Parse(jp.getValue("page")))));

            return collection.ToString();
        }
        private string detailaction(JsonArrayParse jp)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder("");
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";
            try
            {
                Business.Order.BusinessWorkOrderFee bc = new project.Business.Order.BusinessWorkOrderFee();
                bc.load(jp.getValue("id"), user.Entity.AccID);

                sb.Append("<table class=\"table table-border table-bordered table-hover table-bg table-sort\" id=\"tablelist\">");
                sb.Append("<thead>");
                sb.Append("<tr class=\"text-c\">");
                sb.Append("<th width=\"5%\">序号</th>");
                sb.Append("<th width='15%'>填写人</th>");
                sb.Append("<th width='16%'>收款项目</th>");
                sb.Append("<th width='30%'>说明</th>");
                sb.Append("<th width='16%'>收款金额</th>");
                sb.Append("<th width='18%'>操作</th>");
                sb.Append("</tr>");
                sb.Append("</thead>");

                int r = 1;
                sb.Append("<tbody>");
                Business.Order.BusinessWorkOrderFeeDetail bc1 = new project.Business.Order.BusinessWorkOrderFeeDetail();
                foreach (Entity.Order.EntityWorkOrderFeeDetail it in bc1.GetWorkOrderFeeDetailQuery(user.Entity.AccID, bc.Entity.FeeNo, bc.Entity.OrderNo, string.Empty,string.Empty,default(DateTime),default(DateTime)))
                {
                    sb.Append("<tr class=\"text-c\" id=\"" + it.InnerEntityOID + "\">");
                    sb.Append("<td align='center'>" + r.ToString() + "</td>");
                    sb.Append("<td>" + it.UserName + "</td>");
                    sb.Append("<td>" + it.FeeTypeName + "</td>");
                    if (bc.Entity.Status == "OPEN")
                    {
                        sb.Append("<td><input type=\"text\" class=\"input-text size-S\" style=\"width:100%\" placeholder=\"\" id=\"Cont" + it.InnerEntityOID + "\" value=\"" + it.Context + "\"></td>");
                        sb.Append("<td><input type=\"text\" class=\"input-text size-S\" style=\"width:100%\" placeholder=\"\" onblur=\"validDecimal(this.id)\" id=\"Amt" + it.InnerEntityOID + "\" value=\"" + it.FeeAmount.ToString("0.##") + "\"></td>");
                        sb.Append("<td><a href=\"javascript:;\" onclick=\"save('" + it.InnerEntityOID + "')\" class=\"btn btn-primary radius\"><i class=\"Hui-iconfont\">&#xe632;</i> 保存</a></td>");
                    }
                    else
                    {
                        sb.Append("<td>" + it.Context + "</td>");
                        sb.Append("<td>" + it.FeeAmount.ToString("0.##") + "</td>");
                        sb.Append("<td></td>");
                    }
                    sb.Append("</tr>");

                    r++;
                }
                sb.Append("</tbody>");
                sb.Append("</table>");

                collection.Add(new JsonStringValue("liststr", sb.ToString()));
            }
            catch { flag = "2"; }

            collection.Add(new JsonStringValue("type", "detail"));
            collection.Add(new JsonStringValue("flag", flag));

            return collection.ToString();
        }
        private string saveaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";
            try
            {
                Business.Order.BusinessWorkOrderFee bc = new project.Business.Order.BusinessWorkOrderFee();
                bc.load(jp.getValue("id"), user.Entity.AccID);

                if (bc.Entity.Status.ToUpper() != "OPEN")
                {
                    flag = "3";
                }
                else
                {
                    Business.Order.BusinessWorkOrderFeeDetail detail = new Business.Order.BusinessWorkOrderFeeDetail();
                    detail.load(jp.getValue("detailid"),user.Entity.AccID);
                    detail.Entity.Context = jp.getValue("Context");
                    detail.Entity.FeeAmount = ParseDecimalForString(jp.getValue("Amount"));
                    detail.Entity.UpdateDate = GetDate();
                    detail.Entity.UpdateUser = user.Entity.UserNo;
                    int row = detail.Save();
                    if (row <= 0)
                        flag = "2";
                    else
                    {
                        obj.ExecuteNonQuery("update WO_WorkOrder_Fee set FeeAmount = isnull((select SUM(FeeAmount) from WO_WorkOrder_Fee_Detail where FeeNo='" + bc.Entity.FeeNo + "'),0) " +
                            "where FeeNo = '" + bc.Entity.FeeNo + "'");
                    }
                }
            }
            catch { flag = "2"; }

            collection.Add(new JsonStringValue("type", "save"));
            collection.Add(new JsonStringValue("flag", flag));

            return collection.ToString();
        }
        private string approveaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";
            try
            {
                Business.Order.BusinessWorkOrderFee bc = new project.Business.Order.BusinessWorkOrderFee();
                bc.load(jp.getValue("id"),user.Entity.AccID);

                if (bc.Entity.Status.ToUpper() == "CONFIRM")
                {
                    flag = "3";
                }
                else
                {
                    if (bc.Entity.Status == "OPEN") bc.Entity.Status = "APPROVE";
                    else bc.Entity.Status = "OPEN";
                    int row = bc.Save();
                    if (row <= 0)
                        flag = "2";
                    else
                        collection.Add(new JsonStringValue("liststr", createList(jp.getValue("OrderNo"), jp.getValue("Status"), jp.getValue("MinFeeDate"), jp.getValue("MaxFeeDate"), int.Parse(jp.getValue("page")))));
                }
            }
            catch { flag = "2"; }

            collection.Add(new JsonStringValue("type", "approve"));
            collection.Add(new JsonStringValue("flag", flag));

            return collection.ToString();
        }
    }
}