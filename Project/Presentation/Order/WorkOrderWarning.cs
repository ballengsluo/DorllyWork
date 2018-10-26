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
    public partial class WorkOrderWarning : AbstractPmPage, System.Web.UI.ICallbackEventHandler
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
                    CheckRight(user.Entity, "order/WorkOrderWarning.aspx");

                    if (!Page.IsCallback)
                    {
                        orderType = "<select class=\"input-text required size-S\" id=\"OrderType\" style=\"width:120px\" data-valid=\"\" data-error=\"\">";
                        orderType += "<option value=\"\" selected>请选择工单类型</option>";
                        Business.Base.BusinessOrderType type = new project.Business.Base.BusinessOrderType();
                        foreach (Entity.Base.EntityOrderType it in type.GetOrderTypeListQuery(string.Empty, string.Empty, string.Empty, user.Entity.AccID))
                        {
                            orderType += "<option value='" + it.OrderTypeNo + "'>" + it.OrderTypeName + "</option>";
                        }
                        orderType += "</select>";

                        list = createList(string.Empty, string.Empty, string.Empty, string.Empty, GetDate().ToString("yyyy-MM-dd"), GetDate().ToString("yyyy-MM-dd"));
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
        private string createList(string OrderType, string CustNo, string OrderNo, string UserNo, string MinOrderDate, string MaxOrderDate)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder("");

            sb.Append("<table class=\"table table-border table-bordered table-hover table-bg table-sort\" id=\"tablelist\">");
            sb.Append("<thead>");
            sb.Append("<tr class=\"text-c\">");
            sb.Append("<th width=\"5%\">序号</th>");
            sb.Append("<th width='10%'>工单内容</th>");
            sb.Append("<th width='10%'>工单类型</th>");
            sb.Append("<th width='10%'>工单日期</th>");
            sb.Append("<th width='10%'>处理人</th>");
            sb.Append("<th width='10%'>状态</th>");
            sb.Append("<th width='9%'>响应</th>");
            sb.Append("<th width='9%'>预约</th>");
            sb.Append("<th width='9%'>签到</th>");
            sb.Append("<th width='9%'>完成</th>");
            sb.Append("<th width='9%'>消单</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");

            string AlloUser = "%";

            //非管理员登录，只能看到当前用户为部门负责人的单
            if (user.Entity.UserType.ToUpper() != "ADMIN" && (user.Entity.UserType.ToUpper() == "03"
                    || user.Entity.UserType.ToUpper() == "07" || user.Entity.UserType.ToUpper() == "08"))
            {
                AlloUser = user.Entity.UserNo;
            }
            
            SqlConnection con = null;
            SqlCommand cmd = null;
            DataSet ds = new DataSet();
            try
            {
                con = Data.Conn();
                cmd = new SqlCommand("GetEarlyWarning", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter[] parameter = new SqlParameter[] { 
                    new SqlParameter("@AccID",SqlDbType.NVarChar,20),
                    new SqlParameter("@OrderType",SqlDbType.NVarChar,30),
                    new SqlParameter("@CustNo",SqlDbType.NVarChar,30),
                    new SqlParameter("@OrderNo",SqlDbType.NVarChar,30),
                    new SqlParameter("@UserNo",SqlDbType.NVarChar,30),
                    new SqlParameter("@AlloUser",SqlDbType.NVarChar,30),
                    new SqlParameter("@MinOrderDate",SqlDbType.NVarChar,10),
                    new SqlParameter("@MaxOrderDate",SqlDbType.NVarChar,10)
                };
                if (OrderType == "") OrderType = "%";
                if (CustNo == "") CustNo = "%";
                if (OrderNo == "") OrderNo = "%";
                if (UserNo == "") UserNo = "%";
                parameter[0].Value = user.Entity.AccID;
                parameter[1].Value = OrderType;
                parameter[2].Value = CustNo;
                parameter[3].Value = OrderNo;
                parameter[4].Value = UserNo;
                parameter[5].Value = AlloUser;
                parameter[6].Value = MinOrderDate;
                parameter[7].Value = MaxOrderDate;
                cmd.Parameters.AddRange(parameter);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                DataTable dt = ds.Tables[0];

                int ResponseTime = 0;
                int AppoIntTime = 0;
                int SignTime = 0;
                int FinishTime = 0;
                int CloseTime = 0;
                Business.Base.BusinessWarningTime wt = new Business.Base.BusinessWarningTime();
                wt.loadParaNo("ResponseTime", user.Entity.AccID);
                ResponseTime = wt.Entity.Time;
                wt.loadParaNo("AppoIntTime", user.Entity.AccID);
                AppoIntTime = wt.Entity.Time;
                wt.loadParaNo("SignTime", user.Entity.AccID);
                SignTime = wt.Entity.Time;
                wt.loadParaNo("FinishTime", user.Entity.AccID);
                FinishTime = wt.Entity.Time;
                wt.loadParaNo("CloseTime", user.Entity.AccID);
                CloseTime = wt.Entity.Time;

                int r = 1;
                sb.Append("<tbody>");
                foreach (DataRow it in dt.Rows)
                {
                    sb.Append("<tr class=\"text-c\" id=\"" + it["OrderNo"] + "\">");
                    sb.Append("<td align='center'>" + r.ToString() + "</td>");
                    sb.Append("<td>" + it["OrderName"].ToString() + "</td>");
                    sb.Append("<td>" + it["OrderTypeName"].ToString() + "</td>");
                    sb.Append("<td>" + ParseDateForString(it["OrderDate"].ToString()).ToString("yyyy-MM-dd HH:mm") + "</td>");
                    sb.Append("<td>" + it["TeateUser"].ToString() + "</td>");
                    sb.Append("<td>" + it["StatusName"].ToString() + "</td>");
                    if (it["ResponseMM"].ToString() != "") {
                        if (ParseIntForString(it["ResponseMM"].ToString()) > ResponseTime)
                            sb.Append("<td style=\"background-color:red;\">" + (ParseIntForString(it["ResponseMM"].ToString()) - ResponseTime).ToString() + "</td>");
                        else
                            sb.Append("<td>" + (ParseIntForString(it["ResponseMM"].ToString()) - ResponseTime).ToString() + "</td>");
                    }
                    else
                        sb.Append("<td></td>");

                    if (it["AppoIntMM"].ToString() != "")
                    {
                        if (ParseIntForString(it["AppoIntMM"].ToString()) > AppoIntTime)
                            sb.Append("<td style=\"background-color:red;\">" + (ParseIntForString(it["AppoIntMM"].ToString()) - AppoIntTime).ToString() + "</td>");
                        else
                            sb.Append("<td>" + (ParseIntForString(it["AppoIntMM"].ToString()) - AppoIntTime).ToString() + "</td>");
                    }
                    else
                        sb.Append("<td></td>");


                    if (it["SignMM"].ToString() != "")
                    {
                        if (ParseIntForString(it["SignMM"].ToString()) > SignTime)
                            sb.Append("<td style=\"background-color:red;\">" + (ParseIntForString(it["SignMM"].ToString()) - SignTime).ToString() + "</td>");
                        else
                            sb.Append("<td>" + (ParseIntForString(it["SignMM"].ToString()) - SignTime).ToString() + "</td>");
                    }
                    else
                        sb.Append("<td></td>");


                    if (it["FinishMM"].ToString() != "")
                    {
                        if (ParseIntForString(it["FinishMM"].ToString()) > FinishTime)
                            sb.Append("<td style=\"background-color:red;\">" + (ParseIntForString(it["FinishMM"].ToString()) - FinishTime).ToString() + "</td>");
                        else
                            sb.Append("<td>" + (ParseIntForString(it["FinishMM"].ToString()) - FinishTime).ToString() + "</td>");
                    }
                    else
                        sb.Append("<td></td>");


                    if (it["CloseMM"].ToString() != "")
                    {
                        if (ParseIntForString(it["CloseMM"].ToString()) > CloseTime)
                            sb.Append("<td style=\"background-color:red;\">" + (ParseIntForString(it["CloseMM"].ToString()) - CloseTime).ToString() + "</td>");
                        else
                            sb.Append("<td>" + (ParseIntForString(it["CloseMM"].ToString()) - CloseTime).ToString() + "</td>");
                    }
                    else
                        sb.Append("<td></td>");

                    sb.Append("</tr>");

                    r++;
                }
                sb.Append("</tbody>");

            }
            catch { }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (con != null)
                    con.Dispose();
            }
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
            collection.Add(new JsonStringValue("liststr", createList(jp.getValue("OrderType"), jp.getValue("CustNo"), jp.getValue("OrderNo"),
                jp.getValue("UserNo"), jp.getValue("MinOrderDate"), jp.getValue("MaxOrderDate"))));

            return collection.ToString();
        }
    }
}