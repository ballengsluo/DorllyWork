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
    public partial class Dict_Bak : AbstractPmPage, System.Web.UI.ICallbackEventHandler
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
                    CheckRight(user.Entity, "base/dict.aspx");

                    if (!Page.IsCallback)
                    {
                        list = createList("UserType");
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
        protected string TypeStr = "";
        private string createList(string dictType)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder("");

            sb.Append("<table class=\"table table-border table-bordered table-hover table-bg\" id=\"tablelist\">");
            sb.Append("<thead>");
            sb.Append("<tr class=\"text-c\">");
            sb.Append("<th width=\"30\">序号</th>");
            sb.Append("<th width='100'>编号</th>");
            sb.Append("<th width='150'>名称</th>");
            sb.Append("<th width='300'>备注</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");

            int r = 1;
            sb.Append("<tbody>");
            Business.Base.BusinessDict bc = new Business.Base.BusinessDict();
            foreach (Entity.Base.EntityDict it in bc.GetDictListQuery(string.Empty,string.Empty,user.Entity.AccID,dictType))
            {
                sb.Append("<tr class=\"text-c\" id=\"" + it.DictNo + "\">");
                sb.Append("<td align='center'>" + r.ToString() + "</td>");
                sb.Append("<td>" + it.DictNo + "</td>");
                sb.Append("<td>" + it.DictName + "</td>");
                sb.Append("<td>" + it.Remark + "</td>");
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
            else if (jp.getValue("Type") == "get")
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
                Business.Base.BusinessDict bc = new project.Business.Base.BusinessDict();
                bc.load(jp.getValue("id"), user.Entity.AccID, jp.getValue("DictType"));

                collection.Add(new JsonStringValue("DictNo", bc.Entity.DictNo));
                collection.Add(new JsonStringValue("DictName", bc.Entity.DictName));
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
                Business.Base.BusinessDict bc = new project.Business.Base.BusinessDict();
                bc.load(jp.getValue("id"), user.Entity.AccID, jp.getValue("DictType"));
                int r = bc.delete();
                if (r <= 0)
                    flag = "2";
            }
            catch { flag = "2"; }

            collection.Add(new JsonStringValue("type", "delete"));
            collection.Add(new JsonStringValue("flag", flag));
            collection.Add(new JsonStringValue("liststr", createList(jp.getValue("DictType"))));

            return collection.ToString();
        }

        private string submitaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";
            try
            {
                Business.Base.BusinessDict bc = new project.Business.Base.BusinessDict();
                if (jp.getValue("tp") == "update")
                {
                    bc.load(jp.getValue("id"), user.Entity.AccID, jp.getValue("DictType"));
                    bc.Entity.DictName = jp.getValue("DictName");
                    bc.Entity.Remark = jp.getValue("Remark");
                    int r = bc.Save("update");
                    if (r <= 0)
                        flag = "2";
                }
                else
                {
                    Data obj = new Data();
                    DataTable dt = obj.ExecuteDataSet("select cnt=COUNT(*) from Base_Dict where DictNo=N'" + jp.getValue("DictNo") + "' and DictType='" + jp.getValue("DictType") + "'").Tables[0];
                    if (int.Parse(dt.Rows[0]["cnt"].ToString()) > 0)
                        flag = "3";
                    else
                    {
                        bc.Entity.DictNo = jp.getValue("DictNo");
                        bc.Entity.DictName = jp.getValue("DictName");
                        bc.Entity.Remark = jp.getValue("Remark");
                        bc.Entity.DictType = jp.getValue("DictType");
                        int r = bc.Save("insert");
                        if (r <= 0)
                            flag = "2";
                    }
                }
            }
            catch { flag = "2"; }

            collection.Add(new JsonStringValue("type", "submit"));
            collection.Add(new JsonStringValue("flag", flag));
            collection.Add(new JsonStringValue("liststr", createList(jp.getValue("DictType"))));

            return collection.ToString();
        }

        private string selectaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";

            collection.Add(new JsonStringValue("type", "select"));
            collection.Add(new JsonStringValue("flag", flag));
            collection.Add(new JsonStringValue("liststr", createList(jp.getValue("DictType"))));

            return collection.ToString();
        }        
    }
}