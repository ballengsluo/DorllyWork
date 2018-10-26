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
    public partial class WorkOrderCostConfirm : AbstractPmPage, System.Web.UI.ICallbackEventHandler
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
                    CheckRight(user.Entity, "order/WorkOrderCostConfirm.aspx");

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
        private string createList(string OrderNo, string Status, string MinCostDate, string MaxCostDate, int page)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder("");

            sb.Append("<table class=\"table table-border table-bordered table-hover table-bg table-sort\" id=\"tablelist\">");
            sb.Append("<thead>");
            sb.Append("<tr class=\"text-c\">");
            sb.Append("<th width=\"5%\">序号</th>");
            sb.Append("<th width='17%'>工单日期</th>");
            sb.Append("<th width='17%'>工单号</th>");
            sb.Append("<th width='32%'>工单内容</th>");
            sb.Append("<th width='17%'>费用金额</th>");
            sb.Append("<th width='12%'>状态</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");

            DateTime MinCostDateS = default(DateTime);
            DateTime MaxCostDateS = default(DateTime);
            if (MinCostDate != "") MinCostDateS = ParseDateForString(MinCostDate);
            if (MaxCostDate != "") MaxCostDateS = ParseDateForString(MaxCostDate);

            int r = 1;
            sb.Append("<tbody>");
            Business.Order.BusinessWorkOrderCost bc = new project.Business.Order.BusinessWorkOrderCost();
            foreach (Entity.Order.EntityWorkOrderCost it in bc.GetWorkOrderCostQuery(user.Entity.AccID, string.Empty, OrderNo, Status, MinCostDateS, MaxCostDateS, "2", page, pageSize))
            {
                sb.Append("<tr class=\"text-c\" id=\"" + it.InnerEntityOID + "\">");
                sb.Append("<td align='center'>" + r.ToString() + "</td>");
                sb.Append("<td>" + it.OrderDate.ToString("MM-dd HH:mm") + "</td>");
                sb.Append("<td>" + it.OrderNo + "</td>");
                sb.Append("<td>" + it.OrderName + "</td>");
                sb.Append("<td>" + it.CostAmount.ToString("0.##") + "</td>");
                sb.Append("<td class=\"td-status\"><span class=\"label " + (it.Status == "APPROVE" ? "" : "label-success") + " radius\">" + it.StatusName + "</span></td>");
                sb.Append("</tr>");

                r++;
            }
            sb.Append("</tbody>");
            sb.Append("</table>");

            sb.Append(Paginat(bc.GetWorkOrderCostCount(user.Entity.AccID, string.Empty, OrderNo, Status, MinCostDateS, MaxCostDateS, "2"), pageSize, page, 7));
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
            else if (jp.getValue("Type") == "confirm")
                result = confirmaction(jp);
            return result;
        }

        private string selectaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";

            collection.Add(new JsonStringValue("type", "select"));
            collection.Add(new JsonStringValue("flag", flag));
            collection.Add(new JsonStringValue("liststr", createList(jp.getValue("OrderNo"), jp.getValue("Status"), jp.getValue("MinCostDate"), jp.getValue("MaxCostDate"), int.Parse(jp.getValue("page")))));

            return collection.ToString();
        }
        private string detailaction(JsonArrayParse jp)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder("");
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";
            try
            {
                Business.Order.BusinessWorkOrderCost bc = new project.Business.Order.BusinessWorkOrderCost();
                bc.load(jp.getValue("id"), user.Entity.AccID);

                sb.Append("<table class=\"table table-border table-bordered table-hover table-bg table-sort\" id=\"tablelist\">");
                sb.Append("<thead>");
                sb.Append("<tr class=\"text-c\">");
                sb.Append("<th width=\"5%\">序号</th>");
                sb.Append("<th width='15%'>填写人</th>");
                sb.Append("<th width='16%'>费用项目</th>");
                sb.Append("<th width='30%'>说明</th>");
                sb.Append("<th width='16%'>费用金额</th>");
                sb.Append("<th width='18%'>操作</th>");
                sb.Append("</tr>");
                sb.Append("</thead>");

                int r = 1;
                sb.Append("<tbody>");
                Business.Order.BusinessWorkOrderCostDetail bc1 = new project.Business.Order.BusinessWorkOrderCostDetail();
                foreach (Entity.Order.EntityWorkOrderCostDetail it in bc1.GetWorkOrderCostDetailQuery(user.Entity.AccID, bc.Entity.CostNo, bc.Entity.OrderNo, string.Empty, string.Empty, default(DateTime), default(DateTime)))
                {
                    sb.Append("<tr class=\"text-c\" id=\"" + it.InnerEntityOID + "\">");
                    sb.Append("<td align='center'>" + r.ToString() + "</td>");
                    sb.Append("<td>" + it.UserName + "</td>");
                    sb.Append("<td>" + it.CostTypeName + "</td>");
                    if (bc.Entity.Status == "APPROVE")
                    {
                        sb.Append("<td><input type=\"text\" class=\"input-text size-S\" style=\"width:100%\" placeholder=\"\" id=\"Cont" + it.InnerEntityOID + "\" value=\"" + it.Context + "\"></td>");
                        sb.Append("<td><input type=\"text\" class=\"input-text size-S\" style=\"width:100%\" placeholder=\"\" onblur=\"validDecimal(this.id)\" id=\"Amt" + it.InnerEntityOID + "\" value=\"" + it.CostAmount.ToString("0.##") + "\"></td>");
                        sb.Append("<td><a href=\"javascript:;\" onclick=\"save('" + it.InnerEntityOID + "')\" class=\"btn btn-primary radius\"><i class=\"Hui-iconfont\">&#xe632;</i> 保存</a></td>");
                    }
                    else
                    {
                        sb.Append("<td>" + it.Context + "</td>");
                        sb.Append("<td>" + it.CostAmount.ToString("0.##") + "</td>");
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
                Business.Order.BusinessWorkOrderCost bc = new project.Business.Order.BusinessWorkOrderCost();
                bc.load(jp.getValue("id"), user.Entity.AccID);

                if (bc.Entity.Status.ToUpper() == "APPROVE")
                {
                    flag = "3";
                }
                else
                {
                    Business.Order.BusinessWorkOrderCostDetail detail = new Business.Order.BusinessWorkOrderCostDetail();
                    detail.load(jp.getValue("detailid"), user.Entity.AccID);
                    detail.Entity.Context = jp.getValue("Context");
                    detail.Entity.CostAmount = ParseDecimalForString(jp.getValue("Amount"));
                    detail.Entity.UpdateDate = GetDate();
                    detail.Entity.UpdateUser = user.Entity.UserNo;
                    int row = detail.Save();
                    if (row <= 0)
                        flag = "2";
                    else
                    {
                        obj.ExecuteNonQuery("update WO_WorkOrder_Cost set CostAmount = isnull((select SUM(CostAmount) from WO_WorkOrder_Cost_Detail where CostNo='" + bc.Entity.CostNo + "'),0) " +
                            "where CostNo = '" + bc.Entity.CostNo + "'");
                    }
                }
            }
            catch { flag = "2"; }

            collection.Add(new JsonStringValue("type", "save"));
            collection.Add(new JsonStringValue("flag", flag));

            return collection.ToString();
        }
        private string confirmaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";
            try
            {
                Business.Order.BusinessWorkOrderCost bc = new project.Business.Order.BusinessWorkOrderCost();
                bc.load(jp.getValue("id"), user.Entity.AccID);

                if (bc.Entity.Status.ToUpper() == "OPEN")
                {
                    flag = "3";
                }
                else
                {
                    if (bc.Entity.Status == "CONFIRM") bc.Entity.Status = "APPROVE";
                    else bc.Entity.Status = "CONFIRM";
                    int row = bc.Save();
                    if (row <= 0)
                        flag = "2";
                    else
                        collection.Add(new JsonStringValue("liststr", createList(jp.getValue("OrderNo"), jp.getValue("Status"), jp.getValue("MinCostDate"), jp.getValue("MaxCostDate"), int.Parse(jp.getValue("page")))));
                }
            }
            catch { flag = "2"; }

            collection.Add(new JsonStringValue("type", "confirm"));
            collection.Add(new JsonStringValue("flag", flag));

            return collection.ToString();
        }
    }
}