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
    public partial class Region : AbstractPmPage, System.Web.UI.ICallbackEventHandler
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
                    CheckRight(user.Entity, "pm/Base/Region.aspx");

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
            sb.Append("<th width='30%'>地区编号</th>");
            sb.Append("<th width='60%'>地区名称</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");

            int r = 1;
            sb.Append("<tbody>");
            Business.Base.BusinessRegion bc = new Business.Base.BusinessRegion();
            foreach (Entity.Base.EntityRegion it in bc.GetRegionListQuery(string.Empty, string.Empty, user.Entity.AccID, "null"))
            {
                sb.Append("<tr class=\"text-c\" id=\"" + it.RegionNo + "\">");
                sb.Append("<td align='center'>" + r.ToString() + "</td>");
                sb.Append("<td>" + it.RegionNo + "</td>");
                sb.Append("<td style='text-align:left'>" + it.RegionName + "</td>");
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
                Business.Base.BusinessRegion bc = new project.Business.Base.BusinessRegion();
                bc.load(jp.getValue("id"), user.Entity.AccID);

                collection.Add(new JsonStringValue("RegionNo", bc.Entity.RegionNo));
                collection.Add(new JsonStringValue("RegionName", bc.Entity.RegionName));
                //collection.Add(new JsonStringValue("Parent", bc.Entity.Parent));
                //collection.Add(new JsonStringValue("ParentName", bc.Entity.ParentName));
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
                Business.Base.BusinessRegion bc = new project.Business.Base.BusinessRegion();
                bc.load(jp.getValue("id"), user.Entity.AccID);
                if (obj.ExecuteDataSet("select 1 from WO_WorkOrder where Region='" + bc.Entity.RegionNo + "' and AccID='" + user.Entity.AccID + "'").Tables[0].Rows.Count > 0)
                {
                    flag = "3";
                }
                else
                {
                    if (obj.ExecuteDataSet("select 1 from Base_Region where Parent='" + bc.Entity.RegionNo + "' and AccID='" + user.Entity.AccID + "'").Tables[0].Rows.Count > 0)
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
            int level = 1;
            try
            {
                //if (jp.getValue("Parent") != "")
                //{
                //    Business.Base.BusinessRegion parent = new project.Business.Base.BusinessRegion();
                //    parent.load(jp.getValue("Parent"), user.Entity.AccID);
                //    level = parent.Entity.Level;
                //}

                Business.Base.BusinessRegion bc = new project.Business.Base.BusinessRegion();
                if (jp.getValue("tp") == "update")
                {
                    bc.load(jp.getValue("id"), user.Entity.AccID);
                    bc.Entity.RegionName = jp.getValue("RegionName");
                    //bc.Entity.Parent = jp.getValue("Parent");
                    bc.Entity.Level = level;
                    int r = bc.Save("update");
                    if (r <= 0)
                        flag = "2";
                }
                else
                {
                    Data obj = new Data();
                    DataTable dt = obj.ExecuteDataSet("select 1 from Base_Region where RegionNo=N'" + jp.getValue("RegionNo") + "' and AccID='" + user.Entity.AccID + "'").Tables[0];
                    if (dt.Rows.Count > 0)
                        flag = "3";
                    else
                    {
                        bc.Entity.RegionNo = jp.getValue("RegionNo");
                        bc.Entity.RegionName = jp.getValue("RegionName");
                        //bc.Entity.Parent = jp.getValue("Parent");
                        bc.Entity.Level = level;
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