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

namespace project.Presentation
{
    public partial class index : AbstractPmPage, System.Web.UI.ICallbackEventHandler
    {
        protected Business.Sys.BusinessUserInfo user = new project.Business.Sys.BusinessUserInfo();
        protected string menulist="";
        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                HttpCookie hc = getCookie("1");
                if (hc != null)
                {
                    string str = hc.Value.Replace("%3D", "=");
                    string userid = Encrypt.DecryptDES(str, "1");
                    user.load(userid);
                    if (!IsCallback)
                    {
                        createMenu(user.Entity.UserType);

                        Business.Order.BusinessWorkOrderMsg msg = new Business.Order.BusinessWorkOrderMsg();
                        msgnum = msg.GetWorkOrderMsgCount(user.Entity.AccID, string.Empty, default(DateTime), default(DateTime), user.Entity.UserNo, false, string.Empty, false);
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

        protected int msgnum = 0;
        protected System.Text.StringBuilder menustr = new System.Text.StringBuilder("");
        private void createMenu(string usertype)
        {
            int row = 1;
            Business.Sys.BusinessMenu menu = new Business.Sys.BusinessMenu();
            foreach (Entity.Sys.EntityMenu it in menu.GetRightMenuList("null", "sys", user.Entity.UserType, user.Entity.AccID))
            {
                string tit = "&#xe616;";
                if (it.MenuName == "系统管理") tit = "&#xe63c";
                else if (it.MenuName == "基础资料") tit = "&#xe62e";

                menustr.Append("<dl id=\""+it.InnerEntityOID+"\">\n");
                menustr.Append("<dt><i class=\"Hui-iconfont\">" + tit + "</i> " + it.MenuName + "<b class=\"Hui-iconfont menu_dropdown-arrow\">&#xe6d6;</b></dt>\n");
                menustr.Append("<dd>\n");
                menustr.Append("<ul>\n");

                Business.Sys.BusinessMenu item = new Business.Sys.BusinessMenu();
                foreach (Entity.Sys.EntityMenu sub in item.GetRightMenuList(it.InnerEntityOID, "sys", user.Entity.UserType, user.Entity.AccID))
                {
                    menustr.Append("<li><a _href=\"" + sub.MenuPath + "\" href=\"javascript:void(0)\">" + sub.MenuName + "</a></li>\n");
                }

                menustr.Append("</ul>\n");
                menustr.Append("</dd>\n");
                menustr.Append("</dl>\n");

                row++;
            }
            menulist = menustr.ToString();
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
            if (jp.getValue("Type") == "getmsg")
                result = getmsgaction(jp);
            return result;
        }

        private string getmsgaction(JsonArrayParse jp)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "1";
            string result = ""; ;
            try
            {
                Business.Order.BusinessWorkOrderMsg msg = new Business.Order.BusinessWorkOrderMsg();
                int msgnum = msg.GetWorkOrderMsgCount(user.Entity.AccID, string.Empty, default(DateTime), default(DateTime), user.Entity.UserNo, false, string.Empty, false);

                collection.Add(new JsonStringValue("msgnum", msgnum.ToString()));
            }
            catch
            { flag = "2"; }

            collection.Add(new JsonStringValue("type", "getmsg"));
            collection.Add(new JsonStringValue("flag", flag));

            result = collection.ToString();

            return result;
        }

    }
}