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
    public partial class CustInfo : AbstractPmPage, System.Web.UI.ICallbackEventHandler
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
                    CheckRight(user.Entity, "platform/UserInfo.aspx");

                    if (!Page.IsCallback)
                    {
                        list = createList(string.Empty,1);
                        custType = "<select class=\"input-text required\" id=\"CustType\" data-valid=\"isNonEmpty\" data-error=\"请选择客户类型\">";
                        custType += "<option value=\"\" selected>请选择客户类型</option>";

                        Business.Base.BusinessDict dict = new project.Business.Base.BusinessDict();
                        foreach (Entity.Base.EntityDict it in dict.GetDictListQuery(string.Empty, string.Empty, user.Entity.AccID, "CustType"))
                        {
                            custType += "<option value='" + it.DictNo + "'>" + it.DictName + "</option>";
                        }
                        custType += "</select>";
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
        protected string custType = "";
        private string createList(string CustName,int page)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder("");

            sb.Append("<table class=\"table table-border table-bordered table-hover table-bg table-sort\" id=\"tablelist\">");
            sb.Append("<thead>");
            sb.Append("<tr class=\"text-c\">");
            sb.Append("<th width=\"5%\">序号</th>");
            sb.Append("<th width='15%'>客户编号</th>");
            sb.Append("<th width='20%'>客户名称</th>");
            sb.Append("<th width='15%'>客户类型</th>");
            sb.Append("<th width='15%'>联系人</th>");
            sb.Append("<th width='15%'>电话</th>");
            sb.Append("<th width='10%'>创建日期</th>");
            sb.Append("<th width='5%'>状态</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");

            int r = 1;
            sb.Append("<tbody>");
            Business.Base.BusinessCustInfo bc = new project.Business.Base.BusinessCustInfo();
            foreach (Entity.Base.EntityCustInfo it in bc.GetCustInfoListQuery(string.Empty, user.Entity.AccID, string.Empty, CustName , "" , null, page, pageSize))
            {
                sb.Append("<tr class=\"text-c\" id=\"" + it.InnerEntityOID + "\">");
                sb.Append("<td align='center'>" + r.ToString() + "</td>");
                sb.Append("<td>" + it.CustNo + "</td>");
                sb.Append("<td>" + it.CustName + "</td>");
                sb.Append("<td>" + it.CustTypeName + "</td>");
                sb.Append("<td>" + it.Contact + "</td>");
                sb.Append("<td>" + it.Tel + "</td>");
                sb.Append("<td>" + ParseStringForDate(it.RegDate) + "</td>");
                sb.Append("<td class=\"td-status\"><span class=\"label " + (it.Valid ? "label-success" : "") + " radius\">" + (it.Valid ? "有效" : "已失效") + "</span></td>");
                sb.Append("</tr>");
                r++;
            }
            sb.Append("</tbody>");
            sb.Append("</table>");

            sb.Append(Paginat(bc.GetCustInfoListCount(string.Empty, user.Entity.AccID, string.Empty, CustName, "", null), pageSize, page, 7));
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
            else if (jp.getValue("Type") == "valid")
                result = validaction(jp);
            return result;
        }

        private string updateaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";
            string result = ""; ;
            try
            {
                Business.Base.BusinessCustInfo bc = new project.Business.Base.BusinessCustInfo();
                bc.load(jp.getValue("id"));

                collection.Add(new JsonStringValue("CustNo", bc.Entity.CustNo));
                collection.Add(new JsonStringValue("CustName", bc.Entity.CustName));
                collection.Add(new JsonStringValue("CustType", bc.Entity.CustType));
                collection.Add(new JsonStringValue("Contact", bc.Entity.Contact));
                collection.Add(new JsonStringValue("Tel", bc.Entity.Tel));
                collection.Add(new JsonStringValue("Addr", bc.Entity.Addr));
                collection.Add(new JsonStringValue("Website", bc.Entity.Website));
                collection.Add(new JsonStringValue("Remark", bc.Entity.Remark));
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
                Business.Base.BusinessCustInfo bc = new project.Business.Base.BusinessCustInfo();
                bc.load(jp.getValue("id"));

                if (obj.ExecuteDataSet("select 1 from WO_WorkOrder where CustNo='" + bc.Entity.CustNo + "' and AccID='" + user.Entity.AccID + "'").Tables[0].Rows.Count > 0)
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
            collection.Add(new JsonStringValue("liststr", createList(jp.getValue("CustNameS"), int.Parse(jp.getValue("page")))));

            return collection.ToString();
        }
        private string submitaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";
            try
            {
                Business.Base.BusinessCustInfo bc = new project.Business.Base.BusinessCustInfo();
                if (jp.getValue("tp") == "update")
                {
                    bc.load(jp.getValue("id"));
                    bc.Entity.CustName = jp.getValue("CustName");
                    bc.Entity.CustType = jp.getValue("CustType");
                    bc.Entity.Contact = jp.getValue("Contact");
                    bc.Entity.Tel = jp.getValue("Tel");
                    bc.Entity.Addr = jp.getValue("Addr");
                    bc.Entity.Website = jp.getValue("Website");
                    bc.Entity.Remark = jp.getValue("Remark");
                    int r = bc.Save();
                    if (r <= 0)
                        flag = "2";
                }
                else
                {
                    Data obj = new Data();
                    DataTable dt = obj.ExecuteDataSet("select cnt=COUNT(*) from Base_Cust_Info where CustNo=N'" + jp.getValue("CustNo") + "' and AccID='" + user.Entity.AccID + "'").Tables[0];
                    if (int.Parse(dt.Rows[0]["cnt"].ToString()) > 0)
                        flag = "3";
                    else
                    {
                        bc.Entity.CustNo = jp.getValue("CustNo");
                        bc.Entity.CustName = jp.getValue("CustName");
                        bc.Entity.CustType = jp.getValue("CustType");
                        bc.Entity.Contact = jp.getValue("Contact");
                        bc.Entity.Tel = jp.getValue("Tel");
                        bc.Entity.Addr = jp.getValue("Addr");
                        bc.Entity.Website = jp.getValue("Website");
                        bc.Entity.Remark = jp.getValue("Remark");
                        bc.Entity.AccID = user.Entity.AccID;
                        bc.Entity.Valid = true;
                        bc.Entity.RegDate = GetDate();
                        int r = bc.Save();
                        if (r <= 0)
                            flag = "2";
                    }
                }
            }
            catch { flag = "2"; }

            collection.Add(new JsonStringValue("type", "submit"));
            collection.Add(new JsonStringValue("flag", flag));
            collection.Add(new JsonStringValue("liststr", createList(jp.getValue("CustNameS"), int.Parse(jp.getValue("page")))));

            return collection.ToString();
        }

        private string selectaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";

            collection.Add(new JsonStringValue("type", "select"));
            collection.Add(new JsonStringValue("flag", flag));
            collection.Add(new JsonStringValue("liststr", createList(jp.getValue("CustNameS"), int.Parse(jp.getValue("page")))));

            return collection.ToString();
        }

        private string validaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";
            try
            {
                Business.Base.BusinessCustInfo bc = new project.Business.Base.BusinessCustInfo();
                bc.load(jp.getValue("id"));
                bc.Entity.Valid = !bc.Entity.Valid;

                int r = bc.valid();
                if (r <= 0) flag = "2";
                if (bc.Entity.Valid)
                    collection.Add(new JsonStringValue("stat", "<span class=\"label label-success radius\">有效</span>"));
                else
                    collection.Add(new JsonStringValue("stat", "<span class=\"label radius\">已失效</span>"));
                collection.Add(new JsonStringValue("id", jp.getValue("id")));
            }
            catch { flag = "2"; }

            collection.Add(new JsonStringValue("flag", flag));
            collection.Add(new JsonStringValue("type", "valid"));
            return collection.ToString();
        }

    }
}