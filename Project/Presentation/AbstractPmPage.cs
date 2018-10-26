using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Json;
using System.Text;
using System.Web;
using System.Web.Security;

namespace project.Presentation
{
    /// <summary>
    /// ����ҳ��Ļ��࣬������BasePage
    /// </summary>
    /// <author>tz</author>
    /// <date>2011-07-28</date>
    public class AbstractPmPage : System.Web.UI.Page
    {
        //��Ŀ¼
        public static readonly string mpath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
        //������תҳ��
        public string errorpage = "<script type='text/javascript'>window.parent.window.location.href='login.aspx';</script>";
        public string norightpage = "<script type='text/javascript'>location.href='../errorpage.htm';</script>";
        public string localpath = "D:\\WOWeb\\downfile\\";
        //public string localpath = "E:\\Project\\WordOrder\\WOWeb\\downfile\\";
        public string dopath = "http://120.76.154.6/Order/api/Service.asmx";

        public int pageSize = 15;
        Data obj = new Data();
        protected virtual void Page_Load(object sender, EventArgs e)
        {
        }

        public void GotoErrorPage()
        {
            Response.Write(errorpage);
            return;
        }

        public void GotoNoRightsPage()
        {
            Response.Write(norightpage);
            return;
        }
        public static string WebRootPath
        {
            get
            {
                string result = System.Web.HttpContext.Current.Request.ApplicationPath + "/";
                return (result == "//" ? "/" : result);
            }
        }
        protected DateTime GetDate()
        {
            return DateTime.Parse(obj.ExecuteDataSet("select DT=getdate()").Tables[0].Rows[0]["DT"].ToString());
        }

        protected System.Web.HttpCookie getCookie(string lg)
        {
            if(lg=="1")
                return Request.Cookies["__WORKORDER__SYSTEM__GUID__"];
            else
                return Request.Cookies["__WORKORDER__CUSTOM__GUID__"];
        }
        protected string[] ParseStringForUploadXml(System.Xml.XmlDocument doc)
        {
            if (doc == null)
                return new string[] { "", "" };

            return new string[] { doc.SelectSingleNode("/file/label").InnerText, doc.SelectSingleNode("/file/value").InnerText };
        }
        protected System.Xml.XmlDocument ParseUploadXmlForStrings(string label, string value)
        {
            System.Xml.XmlDocument result = new System.Xml.XmlDocument();
            result.LoadXml("<file><value>" + value + "</value><label>" + label + "</label></file>");

            return result;
        }
        protected System.DateTime ParseDateForString(string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                return DateTime.MinValue.AddYears(1900);
            }

            return DateTime.Parse(val);
        }
        protected System.DateTime ParseSearchDateForString(string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                return default(DateTime);
            }

            return DateTime.Parse(val);
        }
        protected string ParseStringForDate(System.DateTime? date)
        {
            if (null == date)
                return "";
            if (DateTime.MinValue.AddYears(1900).Equals(date))
                return "";

            return ((System.DateTime)date).ToString("yyyy-MM-dd", null);
        }
        protected string ParseStringForDateTime(System.DateTime? date)
        {
            if (null == date)
                return "";
            if (DateTime.MinValue.AddYears(1900).Equals(date))
                return "";


            return ((System.DateTime)date).ToString("yyyy-MM-dd HH:mm", null);
        }
        protected decimal ParseDecimalForString(string val)
        {
            if (string.IsNullOrEmpty(val))
                return 0;

            return decimal.Parse(val);
        }        
        protected int ParseIntForString(string val)
        {
            if (string.IsNullOrEmpty(val))
                return 0;

            return Int32.Parse(val);
        }        
        protected string creatFileName(string expandedName)
        {
            Random rand = new Random();
            char[] code = "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456789".ToCharArray();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int j = 0; j < 10; j++)
            {
                sb.Append(code[rand.Next(code.Length)]);
            }
            string fileName = sb.ToString() +"."+ expandedName;
            return fileName;
        }
        protected string getRandom()
        {
            Random rand = new Random();
            char[] code = "1234567890".ToCharArray();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int j = 0; j < 6; j++)
            {
                sb.Append(code[rand.Next(code.Length)]);
            }
            return sb.ToString();
        }
        protected string getRandom(int length)
        {
            Random rand = new Random();
            char[] code = "1234567890".ToCharArray();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int j = 0; j < length; j++)
            {
                sb.Append(code[rand.Next(code.Length)]);
            }
            return sb.ToString();
        }

        public static bool isnull(string str)
        {
            bool flag = false;
            if ((str == null) || (str == ""))
            {
                flag = true;
            }
            return flag;
        }

        public static int ChEn_length(string str)
        {
            int length = 0;
            int num2 = 0;
            if (!isnull(str))
            {
                length = str.Trim().Length;
                for (int i = 0; i < length; i++)
                {
                    int num4 = Convert.ToChar(str.Substring(i, 1));
                    if ((num4 > 0xff) || (num4 < 0))
                    {
                        num2 += 2;
                    }
                    else
                    {
                        num2++;
                    }
                }
            }
            return num2;
        }
        
        public static string lenCHEN(string str, int lennum)
        {
            int num = 0;
            string str2 = "";
            if (ChEn_length(str) <= lennum)
            {
                return str;
            }
            num = 0;
            int length = 0;
            while (num < lennum)
            {
                length++;
                string str3 = str.Substring(length - 1, 1);
                int num3 = Convert.ToChar(str.Substring(length - 1, 1));
                if ((num3 > 0xff) || (num3 < 0))
                {
                    num += 2;
                }
                else
                {
                    num++;
                }
                str2 = str.Trim().Substring(0, length) + "...";
            }
            return str2;
        }


        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <param name="rows">������</param>
        /// <param name="pagerow">ÿҳ����</param>
        /// <param name="page">�ڼ�ҳ</param>
        /// <param name="onepagecnt">��ʾ����ͼ��</param>
        /// <returns></returns>
        public string Paginat(int rows, int pagerow, int page, int onepagecnt)
        {
            StringBuilder sb = new StringBuilder("");
            int pages = int.Parse(System.Math.Ceiling(decimal.Parse(rows.ToString()) / decimal.Parse(pagerow.ToString())).ToString()); //����ҳ��
            int num = int.Parse(System.Math.Floor(decimal.Parse(onepagecnt.ToString()) / 2).ToString());   //����ڼ�����
            int firstpage = 1;
            int endpage = 1;

            //Ĭ�ϵ�һҳ���ǵ�һҳ������ڼ�ҳ��ʼ
            if (page > num)
                firstpage = page - num;

            //�����������ֹ���ҳ��Ϊֹ����Ȼ�����ֹҳ
            if (page + num > pages)
                endpage = pages;
            else if (pages > onepagecnt)
                endpage = firstpage + onepagecnt - 1;
            else
                endpage = pages;

            if (endpage == pages && pages > num)
                firstpage = endpage - onepagecnt + 1;
            if (firstpage <= 0)
                firstpage = 1;

            sb.Append("<div style='clear:both;'></div>");
            sb.Append("<div class='paginat'>");
            sb.Append("<ul>");
            sb.Append("<li><b>�� " + rows.ToString() + " ������ �� " + pages.ToString() + " ҳ</b></li>");
            sb.Append("<li class='nextpage'><a onclick='jump(1)'>��ҳ</a></li>");
            if (page != 1 && page != 0)
                sb.Append("<li class='nextpage'><a onclick='jump(" + (page - 1) + ")'>��һҳ</a></li>");
            else if (page != 0)
                sb.Append("<li class='currentpage'>��һҳ</li>");
            for (int i = firstpage; i <= endpage; i++)
            {
                if (i == page)
                    sb.Append("<li class='currentpage'>" + i.ToString() + "</li>");
                else
                    sb.Append("<li><a onclick='jump(" + i.ToString() + ")'>" + i.ToString() + "</a></li>");
            }

            if (page != pages && page != 0)
                sb.Append("<li class='nextpage'><a onclick='jump(" + (page + 1) + ")'>��һҳ</a></li>");
            else if (page != 0)
                sb.Append("<li class='currentpage'>��һҳ</li>");
            sb.Append("<li class='nextpage'><a onclick='jump(" + pages + ")'>βҳ</a></li>");
            sb.Append("<li><input id=\"paget\" type=\"text\" onblur=\"validInt(this.id)\" class=\"input-text size-MINI\" style=\"width:50px; height:25px;\" /><input type=\"button\"  class=\"btn btn-primary jumpbtn\" onclick=\"jump($(\"#paget\").val())\" value=\"��ת\" /></li>");
            sb.Append("</ul>");
            sb.Append("</div>");
            return sb.ToString();
        }
        
        public string check(string tp, string val, JsonObjectCollection collection)
        {
            Data obj = new Data();
            string flag = "1";
            try
            {
                if (tp == "producttype")
                {
                    DataTable dt = obj.ExecuteDataSet("select top 1 * from base_producttype where (Code=N'" + val + "'" + " or Name=N'" + val + "') and isnull(Parent,'')='' order by Code").Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        collection.Add(new JsonStringValue("Code", dt.Rows[0]["Code"].ToString()));
                        collection.Add(new JsonStringValue("Name", dt.Rows[0]["Name"].ToString()));
                    }
                    else
                    {
                        DataTable dt1 = obj.ExecuteDataSet("select top 1 * from base_producttype where (Code like N'%" + val + "%'" + " or Name like N'%" + val + "%') and isnull(Parent,'')='' order by Code").Tables[0];
                        if (dt1.Rows.Count > 0)
                        {
                            collection.Add(new JsonStringValue("Code", dt1.Rows[0]["Code"].ToString()));
                            collection.Add(new JsonStringValue("Name", dt1.Rows[0]["Name"].ToString()));
                        }
                        else
                        {
                            flag = "3";
                        }
                    }
                }
            }
            catch { flag = "2"; }

            return flag;
        }
                
        public void CheckRight(Entity.Sys.EntityUserInfo user,string PathName)
        {
            if (user.UserType != "8")
            {
                Data obj = new Data();
                try
                {
                    string sqlstr = "select a.MenuId from sys_userright a left join sys_menu b on a.MenuId=b.menucode where a.UserType='" + user.UserType + 
                        "' and menupath='" + PathName + "'";
                    DataTable dt = obj.ExecuteDataSet(sqlstr).Tables[0];

                    if (dt.Rows.Count == 0)
                        GotoNoRightsPage();
                }
                catch { }
            }
        }
        
        public static SmsStat SendSMS(string Tel, string Content,int type)
        {
            SmsStat sstat = new SmsStat();
            string sendurl = "http://api.sms.cn/mt/";
            string uid = "zhengruyue";
            string pwd = "yue8709147";
            string Pass = FormsAuthentication.HashPasswordForStoringInConfigFile(pwd + uid, "MD5"); //�������MD5����

            string _url = "uid=" + uid + "&pwd=" + Pass + "&mobile=" + Tel + "&content=" + Content;
            byte[] bTemp = System.Text.Encoding.GetEncoding("GBK").GetBytes(_url);
            string _result = doPostRequest(sendurl, bTemp);

            sstat.stat = _result.Split('&')[1].Replace("stat=", "").ToString();

            return sstat;
        }

        private static string doPostRequest(string url, byte[] bData)
        {
            System.Net.HttpWebRequest hwRequest;
            System.Net.HttpWebResponse hwResponse;

            string strResult = string.Empty;
            try
            {
                hwRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                hwRequest.Timeout = 5000;
                hwRequest.Method = "POST";
                hwRequest.ContentType = "application/x-www-form-urlencoded";
                hwRequest.ContentLength = bData.Length;

                System.IO.Stream smWrite = hwRequest.GetRequestStream();
                smWrite.Write(bData, 0, bData.Length);
                smWrite.Close();
            }
            catch{ return strResult; }

            try
            {
                hwResponse = (HttpWebResponse)hwRequest.GetResponse();
                StreamReader srReader = new StreamReader(hwResponse.GetResponseStream(), Encoding.ASCII);
                strResult = srReader.ReadToEnd();
                srReader.Close();
                hwResponse.Close();
            }
            catch { }

            return strResult;
        }
    }

    public class SmsStat {
        public SmsStat() { }

        private string _stat;
        
        public string stat
        {
            get { return _stat; }
            set { _stat = value; }
        }
        public string message
        {
            get
            {
                string msg = "";
                switch (stat)
                {
                    case "100":
                        msg = "���ͳɹ�";
                        break;
                    case "101":
                        msg = "��֤ʧ��";
                        break;
                    case "102":
                        msg = "���Ų���";
                        break;
                    case "103":
                        msg = "����ʧ��";
                        break;
                    case "104":
                        msg = "�Ƿ��ַ�";
                        break;
                    case "105":
                        msg = "���ݹ���";
                        break;
                    case "106":
                        msg = "�������";
                        break;
                    case "107":
                        msg = "Ƶ�ʹ���";
                        break;
                    case "108":
                        msg = "�������ݿ�";
                        break;
                }
                return msg;
            }
        }
    }

    /// <summary>
    /// �����и��ԣ�����ʽ���ڵ��ַ���
    /// </summary>
    public class JsonArrayParse
    {
        private string _strId;
        private char _ch1=':';
        private char _ch2=';';
        public JsonArrayParse(string strId)
        {
            this._strId = strId;
        }
        public JsonArrayParse(string strId,char ch1,char ch2)
        {
            this._strId = strId;
            this._ch1 = ch1;
            this._ch2 = ch2;
        }

        public string getValue(string id)
        {
            foreach (string it in _strId.Split(_ch2))
            {
                string[] s = it.Split(_ch1);
                if (s[0].ToString() == id)
                    return s[1].ToString().Replace("[~&*!^%]", ":").Replace("[^%$#*]", ";");
            }
            return "";
        }
    }



}
