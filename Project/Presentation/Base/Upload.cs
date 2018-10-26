using System;
using System.Web;
using System.Data;
using System.Net.Json;
using System.Data.SqlClient;
using System.Collections;
using System.IO;

namespace project.Presentation.Base
{
    public class Upload : IHttpHandler
    {
        project.Business.Sys.BusinessUserInfo user = new project.Business.Sys.BusinessUserInfo();
        Data obj = new Data();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                HttpCookie hc = context.Request.Cookies["__WORKORDER__SYSTEM__GUID__"];
                if (hc != null)
                {
                    string str = hc.Value.Replace("%3D", "=");
                    string userid = Encrypt.DecryptDES(str, "1");
                    user.load(userid);
                    string username = user.Entity.UserName;

                    string action = context.Request["action"];
                    if (action == "upload")
                        upload(context);
                    else if (action == "uploadpic")
                        uploadpic(context);

                }
                else
                {
                    JsonObjectCollection collection = new JsonObjectCollection();
                    collection.Add(new JsonNumericValue("retCode", 100));
                    context.Response.Write(collection.ToString());
                }
            }
            catch
            {
                JsonObjectCollection collection = new JsonObjectCollection();
                collection.Add(new JsonNumericValue("retCode", 100));
                context.Response.Write(collection.ToString());
            }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private void upload(HttpContext context)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            int code = 0;
            string info = "";

            string fileName = "";
            string fileName1 = "";
            string newname = "";
            string url = "";
            decimal filesize = 0;
            try
            {
                try
                {
                    fileName = context.Server.HtmlDecode(context.Request["filename"]);
                    fileName1 = fileName.Substring(fileName.LastIndexOf("\\") + 1);
                    string lastname = fileName.Substring(fileName.LastIndexOf(".") + 1);

                    if (lastname != "doc" && lastname != "docx" && lastname != "xls" && lastname != "xlsx")///格式
                    {
                        code = -1;
                        info = "文档格式只能为Word或Excel";
                    }
                    else
                    {
                        //newname = creatFileName(lastname);
                        newname = DateTime.Now.ToString("yyMMddHHmmss") + "-" + fileName1;

                        url = context.Server.MapPath("~/upload/");
                        if (!Directory.Exists(url))
                        {
                            Directory.CreateDirectory(url);
                        }
                        HttpPostedFile postFile = context.Request.Files[0];

                        if (postFile.InputStream.Length <= 10 * 1024 * 1024)
                        {
                            if (postFile.FileName != string.Empty)
                            {
                                postFile.SaveAs(url + newname);
                                filesize = postFile.InputStream.Length / 1024;
                            }
                        }
                        else
                        {
                            code = -2;
                            info = "文件大于10M，不能上传";
                        }
                    }
                }
                catch
                {
                    code = -3;
                    info = "上传出现故障，请确认文件是否删除";
                }

            }
            catch
            {
                code = 1;
                info = "记录操作失败！";
            }

            collection.Add(new JsonNumericValue("retCode", code));
            collection.Add(new JsonStringValue("retInfo", info));
            collection.Add(new JsonStringValue("newname", newname));
            context.Response.Write(collection.ToString());
        }


        private void uploadpic(HttpContext context)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            int code = 0;
            string info = "";

            string fileName = "";
            string fileName1 = "";
            string newname = "";
            string url = "";
            decimal filesize = 0;
            try
            {
                try
                {
                    fileName = context.Server.HtmlDecode(context.Request["filename"]);
                    fileName1 = fileName.Substring(fileName.LastIndexOf("\\") + 1);
                    string lastname = fileName.Substring(fileName.LastIndexOf(".") + 1);

                    if (lastname != "jpg" && lastname != "jpeg" && lastname != "png" && lastname != "bmp" && lastname != "gif")///格式
                    {
                        code = -1;
                        info = "图片格式不正确";
                    }
                    else
                    {
                        newname = creatFileName(lastname);
                        url = context.Server.MapPath("~/Images/UserPic/");
                        if (!Directory.Exists(url))
                        {
                            Directory.CreateDirectory(url);
                        }
                        HttpPostedFile postFile = context.Request.Files[0];

                        if (postFile.InputStream.Length <= 2 * 1024 * 1024)
                        {
                            if (postFile.FileName != string.Empty)
                            {
                                postFile.SaveAs(url + newname);
                                filesize = postFile.InputStream.Length / 1024;
                            }
                        }
                        else
                        {
                            code = -2;
                            info = "文件大于2M，不能上传";
                        }
                    }
                }
                catch
                {
                    code = -3;
                    info = "上传出现故障，请确认文件是否删除";
                }

            }
            catch
            {
                code = 1;
                info = "记录操作失败！";
            }

            collection.Add(new JsonNumericValue("retCode", code));
            collection.Add(new JsonStringValue("retInfo", info));
            collection.Add(new JsonStringValue("newname", newname));
            context.Response.Write(collection.ToString());
        }

        protected string creatFileName(string expandedName)
        {
            Random rand = new Random();
            char[] code = "abcdefghijklmnopqrstuvwxyz123456789".ToCharArray();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int j = 0; j < 8; j++)
            {
                sb.Append(code[rand.Next(code.Length)]);
            }
            string fileName = sb.ToString() + "." + expandedName;
            return fileName;
        }
    }
}