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
    public partial class Flow : AbstractPmPage, System.Web.UI.ICallbackEventHandler
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
                    CheckRight(user.Entity, "pm/Base/Flow.aspx");

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
            sb.Append("<th width='200'>备注</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");

            int r = 1;
            sb.Append("<tbody>");
            Business.Base.BusinessFlow bc = new Business.Base.BusinessFlow();
            foreach (Entity.Base.EntityFlow it in bc.GetFlowListQuery(string.Empty, string.Empty, user.Entity.AccID))
            {
                sb.Append("<tr class=\"text-c\" id=\"" + it.FlowNo + "\">");
                sb.Append("<td align='center'>" + r.ToString() + "</td>");
                sb.Append("<td>" + it.FlowNo + "</td>");
                sb.Append("<td>" + it.FlowName + "</td>");
                sb.Append("<td>" + it.Remark + "</td>");
                sb.Append("</tr>");
                r++;
            }
            sb.Append("</tbody>");
            sb.Append("</table>");
            return sb.ToString();
        }
        private string createdetailList(string FlowNo)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder("");

            sb.Append("<table class=\"table table-border table-bordered table-hover table-bg\">");
            sb.Append("<thead>");
            sb.Append("<tr class=\"text-c\">");
            sb.Append("<th width=\"30\">序号</th>");
            sb.Append("<th width='100'>节点编号</th>");
            sb.Append("<th width='100'>节点名称</th>");
            sb.Append("<th width='60'>操作</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");

            int r = 1;
            sb.Append("<tbody>");
            Business.Base.BusinessFlowDetail bc = new Business.Base.BusinessFlowDetail();
            foreach (Entity.Base.EntityFlowDetail it in bc.GetFlowDetailListQuery(FlowNo, user.Entity.AccID))
            {
                sb.Append("<tr class=\"text-c\" id=\"" + it.FlowNo + "\">");
                sb.Append("<td align='center'>" + r.ToString() + "</td>");
                sb.Append("<td>" + it.NodeNo + "</td>");
                sb.Append("<td>" + it.NodeName + "</td>");
                sb.Append("<td><a href=\"javascript:;\" onclick=\"deldetail('" + it.InnerEntityOID + "')\" class=\"btn btn-primary radius\"><i class=\"Hui-iconfont\">&#xe6e2;</i> 删除</a></td>");
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
            else if (jp.getValue("Type") == "detail")
                result = detailaction(jp);
            else if (jp.getValue("Type") == "adddetail")
                result = adddetailaction(jp);
            else if (jp.getValue("Type") == "deldetail")
                result = deldetailaction(jp);
            return result;
        }

        private string updateaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";
            string result = ""; ;
            try
            {
                Business.Base.BusinessFlow bc = new project.Business.Base.BusinessFlow();
                bc.load(jp.getValue("id"), user.Entity.AccID);

                collection.Add(new JsonStringValue("FlowNo", bc.Entity.FlowNo));
                collection.Add(new JsonStringValue("FlowName", bc.Entity.FlowName));
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
                Business.Base.BusinessFlow bc = new project.Business.Base.BusinessFlow();
                bc.load(jp.getValue("id"), user.Entity.AccID);
                if (obj.ExecuteDataSet("select 1 from Base_Order_Type where FlowNo='" + bc.Entity.FlowNo + "' and AccID='" + user.Entity.AccID + "'").Tables[0].Rows.Count > 0)
                {
                    flag = "3";
                }
                else
                {
                    int r = bc.delete();
                    if (r <= 0)
                        flag = "2";
                    else
                        obj.ExecuteNonQuery("delete from Base_Flow_Detail where FlowNo='" + bc.Entity.FlowNo + "' and AccID='" + user.Entity.AccID + "'");
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
                Business.Base.BusinessFlow bc = new project.Business.Base.BusinessFlow();
                if (jp.getValue("tp") == "update")
                {
                    bc.load(jp.getValue("id"), user.Entity.AccID);
                    bc.Entity.FlowName = jp.getValue("FlowName");
                    bc.Entity.Remark = jp.getValue("Remark");
                    int r = bc.Save("update");
                    if (r <= 0)
                        flag = "2";
                }
                else
                {
                    Data obj = new Data();
                    DataTable dt = obj.ExecuteDataSet("select 1 from Base_Flow where FlowNo=N'" + jp.getValue("FlowNo") + "' and AccID='" + user.Entity.AccID + "'").Tables[0];
                    if (dt.Rows.Count > 0)
                        flag = "3";
                    else
                    {
                        bc.Entity.FlowNo = jp.getValue("FlowNo");
                        bc.Entity.FlowName = jp.getValue("FlowName");
                        bc.Entity.Remark = jp.getValue("Remark");
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

        private string detailaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";
            try
            {
                collection.Add(new JsonStringValue("liststr", createdetailList(jp.getValue("id"))));
            }
            catch { flag = "2"; }

            collection.Add(new JsonStringValue("type", "detail"));
            collection.Add(new JsonStringValue("flag", flag));

            return collection.ToString();
        }

        private string adddetailaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";
            try
            {
                foreach (string node in jp.getValue("nodes").Split(';'))
                {
                    if (node.Trim() == "") continue;

                    if (obj.ExecuteDataSet("select 1 as CNT from Base_Flow_Detail where AccID='" + user.Entity.AccID + "' and FlowNo='" + jp.getValue("id") + "' and NodeNo='" + node + "'").Tables[0].Rows.Count == 0)
                    {
                        Business.Base.BusinessFlowDetail detail = new Business.Base.BusinessFlowDetail();
                        detail.Entity.AccID = user.Entity.AccID;
                        detail.Entity.NodeNo = node;
                        detail.Entity.FlowNo = jp.getValue("id");
                        detail.Save();
                    }
                }
                collection.Add(new JsonStringValue("liststr", createdetailList(jp.getValue("id"))));
            }
            catch { flag = "2"; }

            collection.Add(new JsonStringValue("type", "detail"));
            collection.Add(new JsonStringValue("flag", flag));

            return collection.ToString();
        }
        private string deldetailaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";
            try
            {
                Business.Base.BusinessFlowDetail detail = new Business.Base.BusinessFlowDetail();
                detail.load(jp.getValue("detailid"), user.Entity.AccID);
                int r = detail.delete();

                if (r > 0)
                    collection.Add(new JsonStringValue("liststr", createdetailList(jp.getValue("id"))));
                else
                    flag = "3";
            }
            catch { flag = "2"; }

            collection.Add(new JsonStringValue("type", "detail"));
            collection.Add(new JsonStringValue("flag", flag));

            return collection.ToString();
        }
    }
}