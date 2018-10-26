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
    public partial class AutoAllocation : AbstractPmPage, System.Web.UI.ICallbackEventHandler
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
                    CheckRight(user.Entity, "pm/Base/AutoAllocation.aspx");

                    if (!Page.IsCallback)
                    {
                        list = createList(string.Empty, string.Empty);

                        orderType = "<select class=\"input-text required size-S\" id=\"OrderType\" data-valid=\"isNonEmpty\" data-error=\"请选择\">";
                        orderTypeS = "<select class=\"input-text size-S\" id=\"OrderTypeS\" style=\"width:120px;\">";
                        orderType += "<option value=\"\" selected></option>";
                        orderTypeS += "<option value=\"\" selected>全部</option>";
                        Business.Base.BusinessOrderType type = new project.Business.Base.BusinessOrderType();
                        foreach (Entity.Base.EntityOrderType it in type.GetOrderTypeListQuery(string.Empty, string.Empty, string.Empty, user.Entity.AccID))
                        {
                            orderType += "<option value='" + it.OrderTypeNo + "'>" + it.OrderTypeName + "</option>";
                            orderTypeS += "<option value='" + it.OrderTypeNo + "'>" + it.OrderTypeName + "</option>";
                        }
                        orderType += "</select>";
                        orderTypeS += "</select>";

                        region = "<select class=\"input-text required size-S\" id=\"RegionNo\" data-valid=\"isNonEmpty\" data-error=\"请选择\">";
                        regionS = "<select class=\"input-text size-S\" id=\"RegionNoS\" style=\"width:120px;\">";
                        region += "<option value=\"\" selected></option>";
                        regionS += "<option value=\"\" selected>全部</option>";
                        Business.Base.BusinessRegion reg = new project.Business.Base.BusinessRegion();
                        foreach (Entity.Base.EntityRegion it in reg.GetRegionListQuery(string.Empty, string.Empty, user.Entity.AccID, string.Empty))
                        {
                            region += "<option value='" + it.RegionNo + "'>" + it.RegionName + "</option>";
                            regionS += "<option value='" + it.RegionNo + "'>" + it.RegionName + "</option>";
                        }
                        region += "</select>";
                        regionS += "</select>";

                        dept = "<select class=\"input-text required size-S\" id=\"DeptNo\" data-valid=\"isNonEmpty\" data-error=\"请选择\">";
                        dept += "<option value=\"\" selected></option>";
                        Business.Sys.BusinessDept allodt = new project.Business.Sys.BusinessDept();
                        foreach (Entity.Sys.EntityDept it in allodt.GetDeptListQuery(string.Empty, string.Empty, user.Entity.AccID, string.Empty))
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
        protected string orderType = "";
        protected string orderTypeS = "";
        protected string region = "";
        protected string regionS = "";
        protected string dept = "";

        private string createList(string OrderType, string RegionNo)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder("");

            sb.Append("<table class=\"table table-border table-bordered table-hover table-bg\" id=\"tablelist\">");
            sb.Append("<thead>");
            sb.Append("<tr class=\"text-c\">");
            sb.Append("<th width='5%'>序号</th>");
            sb.Append("<th width='20%'>订单类型</th>");
            sb.Append("<th width='20%'>地区</th>");
            sb.Append("<th width='20%'>分配部门</th>");
            sb.Append("<th width='35%'>分配用户</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");

            int r = 1;
            sb.Append("<tbody>");
            Business.Base.BusinessAutoAllocation bc = new Business.Base.BusinessAutoAllocation();
            foreach (Entity.Base.EntityAutoAllocation it in bc.GetDictListQuery(user.Entity.AccID, OrderType, RegionNo, string.Empty, string.Empty))
            {
                sb.Append("<tr class=\"text-c\" id=\"" + it.RowPointer + "\">");
                sb.Append("<td align='center'>" + r.ToString() + "</td>");
                sb.Append("<td>" + it.OrderTypeName + "</td>");
                sb.Append("<td>" + it.RegionName + "</td>");
                sb.Append("<td>" + it.DeptName + "</td>");
                sb.Append("<td>" + it.UserName + "</td>");
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
            else if (jp.getValue("Type") == "select")
                result = selectaction(jp);
            return result;
        }

        private string updateaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";
            string result = ""; ;
            try
            {
                Business.Base.BusinessAutoAllocation bc = new project.Business.Base.BusinessAutoAllocation();
                bc.load(jp.getValue("id"));

                collection.Add(new JsonStringValue("OrderType", bc.Entity.OrderType));
                collection.Add(new JsonStringValue("RegionNo", bc.Entity.RegionNo));
                collection.Add(new JsonStringValue("DeptNo", bc.Entity.DeptNo));
                collection.Add(new JsonStringValue("UserNo", bc.Entity.UserNo));
                collection.Add(new JsonStringValue("UserName", bc.Entity.UserName));
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
                Business.Base.BusinessAutoAllocation bc = new project.Business.Base.BusinessAutoAllocation();
                bc.load(jp.getValue("id"));

                int r = bc.delete();
                if (r <= 0)
                    flag = "2";
            }
            catch { flag = "2"; }

            collection.Add(new JsonStringValue("type", "delete"));
            collection.Add(new JsonStringValue("flag", flag));
            collection.Add(new JsonStringValue("liststr", createList(jp.getValue("OrderTypeS"), jp.getValue("RegionNoS"))));
            return collection.ToString();
        }

        private string submitaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";
            try
            {
                Business.Base.BusinessAutoAllocation bc = new project.Business.Base.BusinessAutoAllocation();
                if (jp.getValue("tp") == "update")
                {
                    bc.load(jp.getValue("id"));
                    bc.Entity.OrderType = jp.getValue("OrderType");
                    bc.Entity.RegionNo = jp.getValue("RegionNo");
                    bc.Entity.DeptNo = jp.getValue("DeptNo");
                    bc.Entity.UserNo = jp.getValue("UserNo");
                    bc.Entity.UserName = jp.getValue("UserName");
                    int r = bc.Save();
                    if (r <= 0)
                        flag = "2";
                }
                else
                {
                    Data obj = new Data();
                    DataTable dt = obj.ExecuteDataSet("select cnt=COUNT(*) from Base_AutoAllocation "+
                            "where OrderType='" + jp.getValue("OrderType") + "' and RegionNo='" + jp.getValue("RegionNo") + "' "+
                            "and AccID='" + user.Entity.AccID + "'").Tables[0];
                    if (int.Parse(dt.Rows[0]["cnt"].ToString()) > 0)
                        flag = "3";
                    else
                    {
                        bc.Entity.AccID = user.Entity.AccID;
                        bc.Entity.OrderType = jp.getValue("OrderType");
                        bc.Entity.RegionNo = jp.getValue("RegionNo");
                        bc.Entity.DeptNo = jp.getValue("DeptNo");
                        bc.Entity.UserNo = jp.getValue("UserNo");
                        bc.Entity.UserName = jp.getValue("UserName");
                        int r = bc.Save();
                        if (r <= 0)
                            flag = "2";
                    }
                }
            }
            catch { flag = "2"; }

            collection.Add(new JsonStringValue("type", "submit"));
            collection.Add(new JsonStringValue("flag", flag));
            collection.Add(new JsonStringValue("liststr", createList(jp.getValue("OrderTypeS"), jp.getValue("RegionNoS")))); 
            return collection.ToString();
        }

        private string selectaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";

            collection.Add(new JsonStringValue("type", "select"));
            collection.Add(new JsonStringValue("flag", flag));
            collection.Add(new JsonStringValue("liststr", createList(jp.getValue("OrderTypeS"), jp.getValue("RegionNoS"))));
            return collection.ToString();
        }
    }
}