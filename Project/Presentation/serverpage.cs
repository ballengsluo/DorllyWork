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
using System.Net.Mail;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Json;

namespace project.Presentation
{
    public partial class serverpage : AbstractPmPage
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            //type 2 用户登录  3获取session名称  4上传 5删除文件
            string type = Request.QueryString["type"];
            if (type == "2")
                login();
            else if (type == "3")
                getSessinName();
            else if (type == "4")
                upload();
            else if (type == "5")
                deleteFile();
        }

        protected void login()
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            string flag = "0";
            string userid = "";
            try
            {
                string u_name = Request.QueryString["uname"];
                string u_password = Request.QueryString["upassword"];
                if (u_name != "" && u_name != null && u_password != "" && u_password != null)
                {
                    Business.Sys.BusinessUserInfo bu = new project.Business.Sys.BusinessUserInfo();
                    ArrayList list = new ArrayList(bu.Login(u_name, Encrypt.EncryptDES(u_password, "1")));
                    if (list.Count > 0)
                    {
                        try {
                            Business.Sys.BusinessUserLog log = new Business.Sys.BusinessUserLog();
                            log.Entity.AccID = ((Entity.Sys.EntityUserInfo)(list[0])).AccID;
                            log.Entity.LogUser = ((Entity.Sys.EntityUserInfo)(list[0])).UserNo;
                            log.Entity.LogDate = GetDate();
                            log.Entity.LogIP = "";
                            log.Entity.LogType = "1";
                            log.Save();                        
                        }
                        catch { }
                        flag = "1" ;
                        userid = Encrypt.EncryptDES(((Entity.Sys.EntityUserInfo)(list[0])).InnerEntityOID, "1");
                    }
                }
            }
            catch { }
            collection.Add(new JsonStringValue("flag", flag));
            collection.Add(new JsonStringValue("id", userid));
            Response.Write(collection.ToString());
        }

        protected void getSessinName()
        {
            try
            {
                if (Request.QueryString["lg"] == "1")
                    Response.Write("__WORKORDER__SYSTEM__GUID__");
                else if (Request.QueryString["lg"] == "2")
                    Response.Write("__WORKORDER__CUSTOM__GUID__");
            }
            catch { }
        }
        //上传
        protected void upload()
        {
            string flag = "0";
            string fileName = "";
            string fileName1 = "";
            string newname = "";
            string url = "";
            decimal filesize = 0;
            try
            {
                fileName = Request.Files[0].FileName;
                fileName1 = fileName.Substring(fileName.LastIndexOf("\\") + 1);
                string lastname = fileName.Substring(fileName.LastIndexOf(".") + 1);

                if (lastname == "jpg" && lastname == "jpeg" && lastname == "png" && lastname == "bmp" && lastname == "gif")///格式
                {
                    flag = "-1";
                }
                else
                {
                    newname = creatFileName(lastname); //随机名称
                    url = Server.MapPath("~/upload/");
                    if (!Directory.Exists(url))
                    {
                        Directory.CreateDirectory(url);
                    }
                    HttpPostedFile postFile = Request.Files[0];

                    if (postFile.InputStream.Length <= 1 * 1024 * 1024)
                    {
                        if (postFile.FileName != String.Empty)
                        {
                            postFile.SaveAs(url + newname);
                            filesize = postFile.InputStream.Length / 1024;
                        }
                        flag = "1";
                    }
                    else
                        flag = "-2";
                }
            }
            catch { flag = "-3"; }

            Response.Write("<script type=\"text/javascript\">window.parent.upstate('" + flag + "@" + fileName1 + "@" + newname + "@" + filesize.ToString("0") + "');</script>");
        }
        //删除
        protected void deleteFile()
        {
            string isok = "0";
            try
            {
                string filename = Request.QueryString["filename"];
                if (filename != null)
                {
                    string url = Server.MapPath("~/upload/");

                    if (File.Exists(url + filename))
                    {
                        File.Delete(url + filename);
                    }
                }
            }
            catch { isok = "-1"; }
            Response.Write(isok);
        }
    }

}