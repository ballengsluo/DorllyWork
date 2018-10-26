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

namespace project.Presentation.Api
{
    public partial class AppService : AbstractPmPage
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            JsonObjectCollection collection = new JsonObjectCollection();
            Data obj = new Data();
            Business.Sys.BusinessUserInfo user = new project.Business.Sys.BusinessUserInfo();

            int flag = 1;
            string info = "参数有误！";
            string str = "";
            string userID = "";
            try
            {
                System.IO.Stream sm = Request.InputStream;
                int count = 0;
                byte[] buffer = new byte[20480];
                while ((count = sm.Read(buffer, 0, 20480)) > 0)
                {
                    str += System.Text.Encoding.UTF8.GetString(buffer, 0, count);
                }

                str = @"[" + str + "]";


                #region [图片上传 fileUpload]
                if (Request.Params["method"] == "fileUpload")
                {
                    System.IO.Stream stream = Request.Files[0].InputStream;
                    Byte[] MeaningFile;
                    int size = Convert.ToInt32(stream.Length);
                    MeaningFile = new Byte[size];
                    stream.Read(MeaningFile, 0, size);
                    stream.Close();
                    FileStream fos = null;

                    string fileName = Request.Params["fileName"];
                    string fileExt = fileName.Substring(fileName.LastIndexOf(".") + 1);
                    if (fileExt == "jpg" || fileExt == "png" || fileExt == "jpeg" || fileExt == "bmp" || fileExt == "gif")
                    {
                        try
                        {
                            string filePath = Server.MapPath("~/upload/");
                            if (!Directory.Exists(filePath))
                            {
                                Directory.CreateDirectory(filePath);
                            }

                            fos = new FileStream(filePath + fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                            fos.Write(MeaningFile, 0, MeaningFile.Length);
                            fos.Close();
                        }
                        catch
                        {
                            flag = 4;
                            info = "文件上传出错，请查看源文件是否存在！";
                        }
                        finally
                        {
                            if (fos != null)
                            {
                                try
                                {
                                    fos.Close();
                                }
                                catch { }
                            }
                        }
                    }
                    else
                    {
                        flag = 3;
                        info = "文件格式不正确！";
                    }
                }
                #endregion

                #region 电表读数图片上传
                if (Request.Params["method"] == "readoutUpload")
                {
                    System.IO.Stream stream = Request.Files[0].InputStream;
                    Byte[] MeaningFile;
                    int size = Convert.ToInt32(stream.Length);
                    MeaningFile = new Byte[size];
                    stream.Read(MeaningFile, 0, size);
                    stream.Close();
                    FileStream fos = null;

                    string fileName = Request.Params["fileName"];
                    string fileExt = fileName.Substring(fileName.LastIndexOf(".") + 1);
                    if (fileExt == "jpg" || fileExt == "png" || fileExt == "jpeg" || fileExt == "bmp" || fileExt == "gif")
                    {
                        try
                        {
                            string filePath = Server.MapPath("~/upload/");
                            if (!Directory.Exists(filePath))
                            {
                                Directory.CreateDirectory(filePath);
                            }

                            fos = new FileStream(filePath + fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                            fos.Write(MeaningFile, 0, MeaningFile.Length);
                            fos.Close();
                        }
                        catch
                        {
                            flag = 4;
                            info = "文件上传出错，请查看源文件是否存在！";
                        }
                        finally
                        {
                            if (fos != null)
                            {
                                try
                                {
                                    fos.Close();
                                }
                                catch { }
                            }
                        }
                    }
                    else
                    {
                        flag = 3;
                        info = "文件格式不正确！";
                    }
                }

                #endregion

                if (Request.Headers["method"] != null && Request.Headers["method"] != "")
                {
                    string method = Request.Headers["method"].ToString();
                    try
                    {
                        //登录、上传文件、修改密码、发生验证码，无需验证、获取日期
                        if (method != "login" && method != "changePwd"
                            && method != "getVerifyCode" && method != "fileUpload" && method != "getDate")
                        {
                            user.load(Encrypt.DecryptDES(Request.Headers["userID"], "1"));
                        }

                        #region[获取当前日期 getDate]
                        if (method == "getDate")
                        {
                            try
                            {
                                collection.Add(new JsonStringValue("date", GetDate().ToString("yyyy-MM-dd HH:mm:ss")));
                                flag = 0;
                                info = "";
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                        }
                        #endregion
                        #region[获取起始日期（当前默认15天前） getBeforeDate]
                        if (method == "getBeforeDate")
                        {
                            try
                            {
                                collection.Add(new JsonStringValue("date", GetDate().AddDays(-15).ToString("yyyy-MM-dd HH:mm:ss")));
                                flag = 0;
                                info = "";
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                        }
                        #endregion
                        #region [登录]
                        else if (method == "login")
                        {
                            LoginUserInfo userinfo = JSON.Login(str)[0];
                            if (userinfo.userName != "" && userinfo.userName != null && userinfo.password != "" && userinfo.password != null)
                            {
                                Business.Sys.BusinessUserInfo us = new project.Business.Sys.BusinessUserInfo();
                                ArrayList list = new ArrayList(us.Login(userinfo.userName, Encrypt.EncryptDES(userinfo.password, "1")));
                                if (list.Count > 0)
                                {
                                    project.Entity.Sys.EntityUserInfo u = (project.Entity.Sys.EntityUserInfo)(list[0]);

                                    try
                                    {
                                        Business.Sys.BusinessUserLog log = new Business.Sys.BusinessUserLog();
                                        log.Entity.AccID = u.AccID;
                                        log.Entity.LogUser = u.UserNo;
                                        log.Entity.LogDate = GetDate();
                                        log.Entity.LogIP = "";
                                        log.Entity.LogType = "2";
                                        log.Save();
                                    }
                                    catch { }

                                    userID = Encrypt.EncryptDES(u.InnerEntityOID, "1");
                                    flag = 0;
                                    info = "";
                                    collection.Add(new JsonStringValue("userType", u.UserType));
                                }
                                else
                                {
                                    flag = 3;
                                    info = "用户名或密码有误！";
                                }
                            }
                            else
                            {
                                flag = 2;
                                info = "用户名或密码不能为空！";
                            }
                            collection.Add(new JsonStringValue("userID", userID));
                        }
                        #endregion
                        #region [发送验证码]
                        else if (method == "getVerifyCode")
                        {
                            try
                            {
                                string Tel = "";
                                string SendPerson = "";

                                GetUserId userId = JSON.GetUserId(str)[0];
                                DataTable dt = obj.ExecuteDataSet("select Tel from Sys_User_Info where (UserNo='" + userId.userID + "' or NickName='" + userId.userID + "') and isnull(Valid,0)=1").Tables[0];
                                if (dt.Rows.Count != 0)
                                {
                                    Tel = dt.Rows[0]["Tel"].ToString();
                                    SendPerson = userId.userID;
                                }
                                else
                                {
                                    flag = 4;
                                    info = "用户名不存在！";
                                }

                                if (flag != 4)
                                {
                                    string Mobile = Tel;
                                    if (Mobile.Length < 11)
                                    {
                                        flag = 5;
                                        info = "手机号码不可用！";
                                    }
                                    else
                                    {
                                        if (Mobile.Length > 5)
                                            Mobile = Mobile.Substring(0, 3) + "****" + Mobile.Substring(Mobile.Length - 2, 2);

                                        string verifyCode = getRandom();

                                        project.Business.Sys.BusinessSMS sms = new project.Business.Sys.BusinessSMS();
                                        sms.Entity.Tel = Tel;
                                        sms.Entity.Content = "您好，您的验证码为：" + verifyCode + "，验证码5分钟有效，请在页面中提交验证码完成验证。【多丽】";
                                        sms.Entity.CreateDate = GetDate();
                                        sms.Entity.RefNo = "";
                                        sms.Entity.SendResult = "";
                                        sms.Entity.SendType = "修改密码";
                                        sms.Entity.IsSend = true;
                                        sms.Entity.SendDate = GetDate();
                                        sms.Entity.SendPerson = SendPerson;
                                        sms.Entity.VerifyNo = verifyCode;
                                        try
                                        {
                                            project.Presentation.SmsStat stat = project.Presentation.AbstractPmPage.SendSMS(sms.Entity.Tel, sms.Entity.Content, 1);
                                            sms.Entity.SendStat = stat.stat;
                                            sms.Entity.SendResult = stat.message;
                                            sms.Save();

                                            if (stat.stat != "100")
                                            {
                                                flag = 6;
                                                info = "短信发送失败！";
                                            }
                                            else
                                            {
                                                flag = 0;
                                                info = "验证码将以短信形式发送到你手机[" + Mobile + "]，请注意查收！";
                                            }
                                        }
                                        catch
                                        {
                                            flag = 6;
                                            info = "短信发送失败！";
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                        }
                        #endregion
                        #region [获取客服电话]
                        if (Request.Headers["method"] == "getHotline")
                        {
                            try
                            {
                                try
                                {
                                    Business.Sys.BusinessSetting bc = new project.Business.Sys.BusinessSetting();
                                    bc.load("HotLine");
                                    collection.Add(new JsonStringValue("hotline", bc.Entity.StringValue));
                                    flag = 0;
                                    info = "";
                                }
                                catch
                                {
                                    flag = 3;
                                    info = "系统操作异常！";
                                }
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }

                        }
                        #endregion
                        #region [忘记密码]
                        else if (method == "changePwd")
                        {
                            ChangePwd change = JSON.ChangePwd(str)[0];
                            if (change.userID != "" && change.userID != null && change.newPWD != "" && change.newPWD != null && change.verifyCode != "" && change.verifyCode != null)
                            {
                                try
                                {
                                    DataTable dt2 = obj.ExecuteDataSet("select * from Sys_User_Info where (UserNo='" + change.userID + "' or NickName='" + change.userID + "') and isnull(Valid,0)=1").Tables[0];
                                    if (dt2.Rows.Count != 0)
                                    {
                                        DataTable dt = obj.ExecuteDataSet("select top 1 * from Sys_Sms where SendType='修改密码' and VerifyNo='" + change.verifyCode +
                                            "' and SendDate> DATEADD(MINUTE,-15,GETDATE()) and SendPerson='" + change.userID + "' and SendStat='100' Order By SendDate desc").Tables[0];
                                        if (dt.Rows.Count == 0)
                                        {
                                            flag = 5;
                                            info = "验证码输入有误！";
                                        }
                                        else
                                        {
                                            user.load(dt2.Rows[0]["UserID"].ToString());
                                            user.Entity.Password = Encrypt.EncryptDES(change.newPWD, "1");
                                            int r = user.changepwd();
                                            if (r == 0)
                                            {
                                                flag = 6;
                                                info = "密码修改不成功，请稍候再试！";
                                            }
                                            else
                                            {
                                                flag = 0;
                                                info = "";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        flag = 4;
                                        info = "用户名不存在！";
                                    }
                                }
                                catch
                                {
                                    flag = 3;
                                    info = "系统操作异常！";
                                }
                            }
                            else
                            {
                                flag = 2;
                                info = "参数不完整！";
                            }
                        }
                        #endregion
                        #region [修改昵称]
                        else if (method == "updateNickName")
                        {
                            UpdateNickName nick = JSON.UpdateNickName(str)[0];
                            if (nick.userType != "" && nick.userType != null)
                            {
                                try
                                {
                                    if (nick.userType == "1")
                                    {
                                        user.load(Encrypt.DecryptDES(Request.Headers["userID"], "1"));

                                        DataTable dt = obj.ExecuteDataSet("select * from Sys_User_Info where NickName='" + nick.nickName + "' and RowPointer<>'" + user.Entity.InnerEntityOID + "'").Tables[0];
                                        if (dt.Rows.Count > 0)
                                        {
                                            int r = user.updateNickName(user.Entity.InnerEntityOID, nick.nickName);
                                            if (r <= 0)
                                            {
                                                flag = 6;
                                                info = "修改昵称失败！";
                                            }
                                            else
                                            {
                                                flag = 0;
                                                info = "";
                                            }
                                        }
                                        else
                                        {
                                            flag = 5;
                                            info = "此昵称已存在！";
                                        }
                                    }
                                    else
                                    {
                                        flag = 4;
                                        info = "参数有误！";
                                    }
                                }
                                catch
                                {
                                    flag = 3;
                                    info = "系统操作异常！";
                                }
                            }
                            else
                            {
                                flag = 2;
                                info = "参数不完整！";
                            }
                        }
                        #endregion
                        #region [获取用户信息]
                        else if (method == "getUserInfo")
                        {
                            try
                            {
                                collection.Add(new JsonStringValue("userID", user.Entity.UserNo));
                                collection.Add(new JsonStringValue("userName", user.Entity.UserName));
                                collection.Add(new JsonStringValue("tel", user.Entity.Tel));
                                collection.Add(new JsonStringValue("deptName", user.Entity.DeptName));
                                collection.Add(new JsonStringValue("picture", user.Entity.Picture));
                                flag = 0;
                                info = "";
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                        }
                        #endregion
                        #region [修改用户信息]
                        else if (method == "changeUserInfo")
                        {
                            try
                            {
                                ChangeInfo chinfo = JSON.ChangeInfo(str)[0];
                                user.Entity.UserName = chinfo.userName;
                                int r = user.Save();

                                if (r <= 0)
                                {
                                    flag = 4;
                                    info = "修改信息出错！";
                                }
                                else
                                {
                                    flag = 0;
                                    info = "";
                                }
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                        }
                        #endregion
                        #region [重置密码]
                        else if (method == "resetPwd")
                        {
                            try
                            {
                                ResetPwd reset = JSON.ResetPwd(str)[0];
                                if (user.Entity.Password != Encrypt.EncryptDES(reset.oldPwd, "1"))
                                {
                                    flag = 4;
                                    info = "旧密码输入有误！";
                                }
                                else
                                {
                                    user.Entity.Password = Encrypt.EncryptDES(reset.newPwd, "1");
                                    int r = user.changepwd();
                                    if (r <= 0)
                                    {
                                        flag = 5;
                                        info = "修改密码出错！";
                                    }
                                    else
                                    {
                                        flag = 0;
                                        info = "";
                                    }
                                }
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                        }
                        #endregion

                        #region [getOrderType 获取工单类型]
                        else if (method == "getOrderType")
                        {
                            try
                            {
                                int row = 1;
                                Business.Base.BusinessOrderType dict = new Business.Base.BusinessOrderType();
                                foreach (Entity.Base.EntityOrderType it in dict.GetOrderTypeListQuery(string.Empty, string.Empty, string.Empty, user.Entity.AccID))
                                {
                                    JsonObjectCollection collection1 = new JsonObjectCollection();
                                    collection1.Add(new JsonStringValue("orderTypeNo", it.OrderTypeNo));
                                    collection1.Add(new JsonStringValue("orderTypeName", it.OrderTypeName));

                                    collection.Add(new JsonStringValue(row.ToString(), collection1.ToString()));
                                    row++;
                                }

                                flag = 0;
                                info = "";
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                        }
                        #endregion
                        #region [getOrderType 获取工单类型1]
                        else if (method == "getOrderType1")
                        {
                            try
                            {
                                JsonObjectCollection collection2 = new JsonObjectCollection();
                                int row = 1;
                                Business.Base.BusinessOrderType dict = new Business.Base.BusinessOrderType();
                                foreach (Entity.Base.EntityOrderType it in dict.GetOrderTypeListQuery(string.Empty, string.Empty, string.Empty, user.Entity.AccID))
                                {
                                    JsonObjectCollection collection1 = new JsonObjectCollection();
                                    collection1.Add(new JsonStringValue("orderTypeNo", it.OrderTypeNo));
                                    collection1.Add(new JsonStringValue("orderTypeName", it.OrderTypeName));

                                    collection2.Add(new JsonStringValue(row.ToString(), collection1.ToString()));
                                    row++;
                                }

                                collection.Add(new JsonStringValue("info", collection2.ToString()));
                                collection.Add(new JsonStringValue("rows", (row - 1).ToString()));
                                flag = 0;
                                info = "";
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                        }
                        #endregion
                        #region [getOrderList 查询工单信息]
                        else if (method == "getOrderList")
                        {
                            SqlConnection con = null;
                            SqlCommand cmd = null;
                            DataSet ds = new DataSet();
                            try
                            {
                                GetOrderList list = JSON.GetOrderList(str)[0];

                                string startDate = "2000-01-01";
                                string endDate = GetDate().ToString("yyyy-MM-dd");
                                if (list.startDate != "") startDate = list.startDate;
                                if (list.endDate != "") endDate = list.endDate;

                                con = Data.Conn();
                                cmd = new SqlCommand("GetWorkOrderList", con);
                                cmd.CommandType = CommandType.StoredProcedure;

                                SqlParameter[] parameter = new SqlParameter[] { 
                                    new SqlParameter("@AccID",SqlDbType.NVarChar,20),
                                    new SqlParameter("@UserID",SqlDbType.NVarChar,30),
                                    new SqlParameter("@WOType",SqlDbType.NVarChar,30),
                                    new SqlParameter("@OrderType",SqlDbType.NVarChar,30),
                                    new SqlParameter("@OrderNo",SqlDbType.NVarChar,30),
                                    new SqlParameter("@MinWODate",SqlDbType.NVarChar,10),
                                    new SqlParameter("@MaxWODate",SqlDbType.NVarChar,10),
                                    new SqlParameter("@PageSize",SqlDbType.Int),
                                    new SqlParameter("@PageIndex",SqlDbType.Int)
                                };
                                parameter[0].Value = "A";
                                parameter[1].Value = user.Entity.UserNo;
                                parameter[2].Value = "1";
                                parameter[3].Value = list.orderType;
                                parameter[4].Value = list.orderNo;
                                parameter[5].Value = startDate;
                                parameter[6].Value = endDate;
                                parameter[7].Value = list.pageSize;
                                parameter[8].Value = list.pageIndex;
                                cmd.Parameters.AddRange(parameter);
                                SqlDataAdapter da = new SqlDataAdapter(cmd);
                                da.Fill(ds);
                                DataTable dt = ds.Tables[0];

                                int row = 1;
                                foreach (DataRow it in dt.Rows)
                                {
                                    JsonObjectCollection collection1 = new JsonObjectCollection();
                                    collection1.Add(new JsonStringValue("orderNo", it["OrderNo"].ToString()));
                                    collection1.Add(new JsonStringValue("orderName", it["OrderName"].ToString()));
                                    collection1.Add(new JsonStringValue("orderType", it["OrderType"].ToString()));
                                    collection1.Add(new JsonStringValue("orderTypeName", it["OrderTypeName"].ToString()));
                                    collection1.Add(new JsonStringValue("statusName", it["StatusName"].ToString()));
                                    collection1.Add(new JsonStringValue("orderDate", it["OrderDate"].ToString()));
                                    collection1.Add(new JsonStringValue("createTime", it["CreateTime"].ToString()));
                                    collection1.Add(new JsonStringValue("custneedTime", it["CustneedTime"].ToString()));
                                    collection1.Add(new JsonStringValue("custNo", it["CustNo"].ToString()));
                                    collection1.Add(new JsonStringValue("custName", it["CustName"].ToString()));
                                    collection1.Add(new JsonStringValue("linkMan", it["LinkMan"].ToString()));
                                    collection1.Add(new JsonStringValue("linkTel", it["LinkTel"].ToString()));
                                    collection1.Add(new JsonStringValue("addr", it["Addr"].ToString()));
                                    bool isback = bool.Parse(it["IsBack"].ToString());
                                    collection1.Add(new JsonStringValue("isBack", (isback == true ? "1" : "0")));
                                    bool ishangup = bool.Parse(it["IsHangUp"].ToString());
                                    collection1.Add(new JsonStringValue("isHangUp", (ishangup == true ? "1" : "0")));
                                    collection.Add(new JsonStringValue(row.ToString(), collection1.ToString()));

                                    row++;
                                }
                                flag = 0;
                                info = "";
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                            finally
                            {
                                if (cmd != null)
                                    cmd.Dispose();
                                if (con != null)
                                    con.Dispose();
                            }
                        }
                        #endregion
                        #region [getOrderList 查询工单信息]
                        else if (method == "getOrderList1")
                        {
                            SqlConnection con = null;
                            SqlCommand cmd = null;
                            DataSet ds = new DataSet();
                            try
                            {
                                GetOrderList list = JSON.GetOrderList(str)[0];

                                string startDate = "2000-01-01";
                                string endDate = GetDate().ToString("yyyy-MM-dd");
                                if (list.startDate != "") startDate = list.startDate;
                                if (list.endDate != "") endDate = list.endDate;

                                con = Data.Conn();
                                cmd = new SqlCommand("GetWorkOrderList", con);
                                cmd.CommandType = CommandType.StoredProcedure;

                                SqlParameter[] parameter = new SqlParameter[] { 
                                    new SqlParameter("@AccID",SqlDbType.NVarChar,20),
                                    new SqlParameter("@UserID",SqlDbType.NVarChar,30),
                                    new SqlParameter("@WOType",SqlDbType.NVarChar,30),
                                    new SqlParameter("@OrderType",SqlDbType.NVarChar,30),
                                    new SqlParameter("@OrderNo",SqlDbType.NVarChar,30),
                                    new SqlParameter("@MinWODate",SqlDbType.NVarChar,10),
                                    new SqlParameter("@MaxWODate",SqlDbType.NVarChar,10),
                                    new SqlParameter("@PageSize",SqlDbType.Int),
                                    new SqlParameter("@PageIndex",SqlDbType.Int)
                                };
                                parameter[0].Value = "A";
                                parameter[1].Value = user.Entity.UserNo;
                                parameter[2].Value = "1";
                                parameter[3].Value = list.orderType;
                                parameter[4].Value = list.orderNo;
                                parameter[5].Value = startDate;
                                parameter[6].Value = endDate;
                                parameter[7].Value = list.pageSize;
                                parameter[8].Value = list.pageIndex;
                                cmd.Parameters.AddRange(parameter);
                                SqlDataAdapter da = new SqlDataAdapter(cmd);
                                da.Fill(ds);
                                DataTable dt = ds.Tables[0];

                                JsonObjectCollection collection2 = new JsonObjectCollection();
                                int row = 1;
                                foreach (DataRow it in dt.Rows)
                                {
                                    JsonObjectCollection collection1 = new JsonObjectCollection();
                                    collection1.Add(new JsonStringValue("orderNo", it["OrderNo"].ToString()));
                                    collection1.Add(new JsonStringValue("orderName", it["OrderName"].ToString()));
                                    collection1.Add(new JsonStringValue("orderType", it["OrderType"].ToString()));
                                    collection1.Add(new JsonStringValue("orderTypeName", it["OrderTypeName"].ToString()));
                                    collection1.Add(new JsonStringValue("statusName", it["StatusName"].ToString()));
                                    collection1.Add(new JsonStringValue("orderDate", it["OrderDate"].ToString()));
                                    collection1.Add(new JsonStringValue("createTime", it["CreateTime"].ToString()));
                                    collection1.Add(new JsonStringValue("custneedTime", it["CustneedTime"].ToString()));
                                    collection1.Add(new JsonStringValue("custNo", it["CustNo"].ToString()));
                                    collection1.Add(new JsonStringValue("custName", it["CustName"].ToString()));
                                    collection1.Add(new JsonStringValue("linkMan", it["LinkMan"].ToString()));
                                    collection1.Add(new JsonStringValue("linkTel", it["LinkTel"].ToString()));
                                    collection1.Add(new JsonStringValue("addr", it["Addr"].ToString()));
                                    bool isback = bool.Parse(it["IsBack"].ToString());
                                    collection1.Add(new JsonStringValue("isBack", (isback == true ? "1" : "0")));
                                    bool ishangup = bool.Parse(it["IsHangUp"].ToString());
                                    collection1.Add(new JsonStringValue("isHangUp", (ishangup == true ? "1" : "0")));
                                    collection2.Add(new JsonStringValue(row.ToString(), collection1.ToString()));

                                    row++;
                                }

                                collection.Add(new JsonStringValue("info", collection2.ToString()));
                                collection.Add(new JsonStringValue("rows", (row - 1).ToString()));
                                flag = 0;
                                info = "";
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                            finally
                            {
                                if (cmd != null)
                                    cmd.Dispose();
                                if (con != null)
                                    con.Dispose();
                            }
                        }
                        #endregion
                        #region [getOrderList_Handle 查询工单信息]
                        else if (method == "getOrderList_Handle")
                        {
                            SqlConnection con = null;
                            SqlCommand cmd = null;
                            DataSet ds = new DataSet();
                            try
                            {
                                GetOrderList list = JSON.GetOrderList(str)[0];

                                string startDate = "2000-01-01";
                                string endDate = GetDate().ToString("yyyy-MM-dd");
                                if (list.startDate != "") startDate = list.startDate;
                                if (list.endDate != "") endDate = list.endDate;

                                con = Data.Conn();
                                cmd = new SqlCommand("GetWorkOrderList", con);
                                cmd.CommandType = CommandType.StoredProcedure;

                                SqlParameter[] parameter = new SqlParameter[] { 
                                    new SqlParameter("@AccID",SqlDbType.NVarChar,20),
                                    new SqlParameter("@UserID",SqlDbType.NVarChar,30),
                                    new SqlParameter("@WOType",SqlDbType.NVarChar,30),
                                    new SqlParameter("@OrderType",SqlDbType.NVarChar,30),
                                    new SqlParameter("@OrderNo",SqlDbType.NVarChar,30),
                                    new SqlParameter("@MinWODate",SqlDbType.NVarChar,10),
                                    new SqlParameter("@MaxWODate",SqlDbType.NVarChar,10),
                                    new SqlParameter("@PageSize",SqlDbType.Int),
                                    new SqlParameter("@PageIndex",SqlDbType.Int)
                                };
                                parameter[0].Value = "A";
                                parameter[1].Value = user.Entity.UserNo;
                                parameter[2].Value = "3";
                                parameter[3].Value = list.orderType;
                                parameter[4].Value = list.orderNo;
                                parameter[5].Value = startDate;
                                parameter[6].Value = endDate;
                                parameter[7].Value = list.pageSize;
                                parameter[8].Value = list.pageIndex;
                                cmd.Parameters.AddRange(parameter);
                                SqlDataAdapter da = new SqlDataAdapter(cmd);
                                da.Fill(ds);
                                DataTable dt = ds.Tables[0];

                                int row = 1;
                                foreach (DataRow it in dt.Rows)
                                {
                                    JsonObjectCollection collection1 = new JsonObjectCollection();
                                    collection1.Add(new JsonStringValue("orderNo", it["OrderNo"].ToString()));
                                    collection1.Add(new JsonStringValue("orderName", it["OrderName"].ToString()));
                                    collection1.Add(new JsonStringValue("orderType", it["OrderType"].ToString()));
                                    collection1.Add(new JsonStringValue("orderTypeName", it["OrderTypeName"].ToString()));
                                    collection1.Add(new JsonStringValue("statusName", it["StatusName"].ToString()));
                                    collection1.Add(new JsonStringValue("orderDate", it["OrderDate"].ToString()));
                                    collection1.Add(new JsonStringValue("createTime", it["CreateTime"].ToString()));
                                    collection1.Add(new JsonStringValue("custneedTime", it["CustneedTime"].ToString()));
                                    collection1.Add(new JsonStringValue("custNo", it["CustNo"].ToString()));
                                    collection1.Add(new JsonStringValue("custName", it["CustName"].ToString()));
                                    collection1.Add(new JsonStringValue("linkMan", it["LinkMan"].ToString()));
                                    collection1.Add(new JsonStringValue("linkTel", it["LinkTel"].ToString()));
                                    collection1.Add(new JsonStringValue("addr", it["Addr"].ToString()));
                                    bool isback = bool.Parse(it["IsBack"].ToString());
                                    collection1.Add(new JsonStringValue("isBack", (isback == true ? "1" : "0")));
                                    bool ishangup = bool.Parse(it["IsHangUp"].ToString());
                                    collection1.Add(new JsonStringValue("isHangUp", (ishangup == true ? "1" : "0")));
                                    collection.Add(new JsonStringValue(row.ToString(), collection1.ToString()));

                                    row++;
                                }
                                flag = 0;
                                info = "";
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                            finally
                            {
                                if (cmd != null)
                                    cmd.Dispose();
                                if (con != null)
                                    con.Dispose();
                            }
                        }
                        #endregion
                        #region [getOrderList_Handle1 查询工单信息]
                        else if (method == "getOrderList_Handle1")
                        {
                            SqlConnection con = null;
                            SqlCommand cmd = null;
                            DataSet ds = new DataSet();
                            try
                            {
                                GetOrderList list = JSON.GetOrderList(str)[0];

                                string startDate = "2000-01-01";
                                string endDate = GetDate().ToString("yyyy-MM-dd");
                                if (list.startDate != "") startDate = list.startDate;
                                if (list.endDate != "") endDate = list.endDate;

                                con = Data.Conn();
                                cmd = new SqlCommand("GetWorkOrderList", con);
                                cmd.CommandType = CommandType.StoredProcedure;

                                SqlParameter[] parameter = new SqlParameter[] { 
                                    new SqlParameter("@AccID",SqlDbType.NVarChar,20),
                                    new SqlParameter("@UserID",SqlDbType.NVarChar,30),
                                    new SqlParameter("@WOType",SqlDbType.NVarChar,30),
                                    new SqlParameter("@OrderType",SqlDbType.NVarChar,30),
                                    new SqlParameter("@OrderNo",SqlDbType.NVarChar,30),
                                    new SqlParameter("@MinWODate",SqlDbType.NVarChar,10),
                                    new SqlParameter("@MaxWODate",SqlDbType.NVarChar,10),
                                    new SqlParameter("@PageSize",SqlDbType.Int),
                                    new SqlParameter("@PageIndex",SqlDbType.Int)
                                };
                                parameter[0].Value = "A";
                                parameter[1].Value = user.Entity.UserNo;
                                parameter[2].Value = "3";
                                parameter[3].Value = list.orderType;
                                parameter[4].Value = list.orderNo;
                                parameter[5].Value = startDate;
                                parameter[6].Value = endDate;
                                parameter[7].Value = list.pageSize;
                                parameter[8].Value = list.pageIndex;
                                cmd.Parameters.AddRange(parameter);
                                SqlDataAdapter da = new SqlDataAdapter(cmd);
                                da.Fill(ds);
                                DataTable dt = ds.Tables[0];

                                JsonObjectCollection collection2 = new JsonObjectCollection();
                                int row = 1;
                                foreach (DataRow it in dt.Rows)
                                {
                                    JsonObjectCollection collection1 = new JsonObjectCollection();
                                    collection1.Add(new JsonStringValue("orderNo", it["OrderNo"].ToString()));
                                    collection1.Add(new JsonStringValue("orderName", it["OrderName"].ToString()));
                                    collection1.Add(new JsonStringValue("orderType", it["OrderType"].ToString()));
                                    collection1.Add(new JsonStringValue("orderTypeName", it["OrderTypeName"].ToString()));
                                    collection1.Add(new JsonStringValue("statusName", it["StatusName"].ToString()));
                                    collection1.Add(new JsonStringValue("orderDate", it["OrderDate"].ToString()));
                                    collection1.Add(new JsonStringValue("createTime", it["CreateTime"].ToString()));
                                    collection1.Add(new JsonStringValue("custneedTime", it["CustneedTime"].ToString()));
                                    collection1.Add(new JsonStringValue("custNo", it["CustNo"].ToString()));
                                    collection1.Add(new JsonStringValue("custName", it["CustName"].ToString()));
                                    collection1.Add(new JsonStringValue("linkMan", it["LinkMan"].ToString()));
                                    collection1.Add(new JsonStringValue("linkTel", it["LinkTel"].ToString()));
                                    collection1.Add(new JsonStringValue("addr", it["Addr"].ToString()));
                                    bool isback = bool.Parse(it["IsBack"].ToString());
                                    collection1.Add(new JsonStringValue("isBack", (isback == true ? "1" : "0")));
                                    bool ishangup = bool.Parse(it["IsHangUp"].ToString());
                                    collection1.Add(new JsonStringValue("isHangUp", (ishangup == true ? "1" : "0")));
                                    collection2.Add(new JsonStringValue(row.ToString(), collection1.ToString()));

                                    row++;
                                }

                                collection.Add(new JsonStringValue("info", collection2.ToString()));
                                collection.Add(new JsonStringValue("rows", (row - 1).ToString()));
                                flag = 0;
                                info = "";
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                            finally
                            {
                                if (cmd != null)
                                    cmd.Dispose();
                                if (con != null)
                                    con.Dispose();
                            }
                        }
                        #endregion
                        #region [getOrderList_History 查询历史工单信息]
                        else if (method == "getOrderList_History")
                        {
                            SqlConnection con = null;
                            SqlCommand cmd = null;
                            DataSet ds = new DataSet();
                            try
                            {
                                GetOrderList list = JSON.GetOrderList(str)[0];

                                string startDate = "2000-01-01";
                                string endDate = GetDate().ToString("yyyy-MM-dd");
                                if (list.startDate != "") startDate = list.startDate;
                                if (list.endDate != "") endDate = list.endDate;

                                con = Data.Conn();
                                cmd = new SqlCommand("GetWorkOrderList", con);
                                cmd.CommandType = CommandType.StoredProcedure;

                                SqlParameter[] parameter = new SqlParameter[] { 
                                    new SqlParameter("@AccID",SqlDbType.NVarChar,20),
                                    new SqlParameter("@UserID",SqlDbType.NVarChar,30),
                                    new SqlParameter("@WOType",SqlDbType.NVarChar,10),
                                    new SqlParameter("@OrderType",SqlDbType.NVarChar,30),
                                    new SqlParameter("@OrderNo",SqlDbType.NVarChar,30),
                                    new SqlParameter("@MinWODate",SqlDbType.NVarChar,10),
                                    new SqlParameter("@MaxWODate",SqlDbType.NVarChar,10),
                                    new SqlParameter("@PageSize",SqlDbType.Int),
                                    new SqlParameter("@PageIndex",SqlDbType.Int)
                                };
                                parameter[0].Value = "A";
                                parameter[1].Value = user.Entity.UserNo;
                                parameter[2].Value = "2";
                                parameter[3].Value = list.orderType;
                                parameter[4].Value = list.orderNo;
                                parameter[5].Value = startDate;
                                parameter[6].Value = endDate;
                                parameter[7].Value = list.pageSize;
                                parameter[8].Value = list.pageIndex;
                                cmd.Parameters.AddRange(parameter);
                                SqlDataAdapter da = new SqlDataAdapter(cmd);
                                da.Fill(ds);
                                DataTable dt = ds.Tables[0];

                                int row = 1;
                                foreach (DataRow it in dt.Rows)
                                {
                                    JsonObjectCollection collection1 = new JsonObjectCollection();
                                    collection1.Add(new JsonStringValue("orderNo", it["OrderNo"].ToString()));
                                    collection1.Add(new JsonStringValue("orderName", it["OrderName"].ToString()));
                                    collection1.Add(new JsonStringValue("orderType", it["OrderType"].ToString()));
                                    collection1.Add(new JsonStringValue("orderTypeName", it["OrderTypeName"].ToString()));
                                    collection1.Add(new JsonStringValue("statusName", it["StatusName"].ToString()));
                                    collection1.Add(new JsonStringValue("orderDate", it["OrderDate"].ToString()));
                                    collection1.Add(new JsonStringValue("createTime", it["CreateTime"].ToString()));
                                    collection1.Add(new JsonStringValue("custneedTime", it["CustneedTime"].ToString()));
                                    collection1.Add(new JsonStringValue("custNo", it["CustNo"].ToString()));
                                    collection1.Add(new JsonStringValue("custName", it["CustName"].ToString()));
                                    collection1.Add(new JsonStringValue("linkMan", it["LinkMan"].ToString()));
                                    collection1.Add(new JsonStringValue("linkTel", it["LinkTel"].ToString()));
                                    collection1.Add(new JsonStringValue("addr", it["Addr"].ToString()));
                                    bool isback = bool.Parse(it["IsBack"].ToString());
                                    collection1.Add(new JsonStringValue("isBack", (isback == true ? "1" : "0")));
                                    bool ishangup = bool.Parse(it["IsHangUp"].ToString());
                                    collection1.Add(new JsonStringValue("isHangUp", (ishangup == true ? "1" : "0")));
                                    collection.Add(new JsonStringValue(row.ToString(), collection1.ToString()));

                                    row++;
                                }
                                flag = 0;
                                info = "";
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                            finally
                            {
                                if (cmd != null)
                                    cmd.Dispose();
                                if (con != null)
                                    con.Dispose();
                            }
                        }
                        #endregion
                        #region [getOrderList_History1 查询历史工单信息]
                        else if (method == "getOrderList_History1")
                        {
                            SqlConnection con = null;
                            SqlCommand cmd = null;
                            DataSet ds = new DataSet();
                            try
                            {
                                GetOrderList list = JSON.GetOrderList(str)[0];

                                string startDate = "2000-01-01";
                                string endDate = GetDate().ToString("yyyy-MM-dd");
                                if (list.startDate != "") startDate = list.startDate;
                                if (list.endDate != "") endDate = list.endDate;

                                con = Data.Conn();
                                cmd = new SqlCommand("GetWorkOrderList", con);
                                cmd.CommandType = CommandType.StoredProcedure;

                                SqlParameter[] parameter = new SqlParameter[] { 
                                    new SqlParameter("@AccID",SqlDbType.NVarChar,20),
                                    new SqlParameter("@UserID",SqlDbType.NVarChar,30),
                                    new SqlParameter("@WOType",SqlDbType.NVarChar,10),
                                    new SqlParameter("@OrderType",SqlDbType.NVarChar,30),
                                    new SqlParameter("@OrderNo",SqlDbType.NVarChar,30),
                                    new SqlParameter("@MinWODate",SqlDbType.NVarChar,10),
                                    new SqlParameter("@MaxWODate",SqlDbType.NVarChar,10),
                                    new SqlParameter("@PageSize",SqlDbType.Int),
                                    new SqlParameter("@PageIndex",SqlDbType.Int)
                                };
                                parameter[0].Value = "A";
                                parameter[1].Value = user.Entity.UserNo;
                                parameter[2].Value = "2";
                                parameter[3].Value = list.orderType;
                                parameter[4].Value = list.orderNo;
                                parameter[5].Value = startDate;
                                parameter[6].Value = endDate;
                                parameter[7].Value = list.pageSize;
                                parameter[8].Value = list.pageIndex;
                                cmd.Parameters.AddRange(parameter);
                                SqlDataAdapter da = new SqlDataAdapter(cmd);
                                da.Fill(ds);
                                DataTable dt = ds.Tables[0];

                                JsonObjectCollection collection2 = new JsonObjectCollection();
                                int row = 1;
                                foreach (DataRow it in dt.Rows)
                                {
                                    JsonObjectCollection collection1 = new JsonObjectCollection();
                                    collection1.Add(new JsonStringValue("orderNo", it["OrderNo"].ToString()));
                                    collection1.Add(new JsonStringValue("orderName", it["OrderName"].ToString()));
                                    collection1.Add(new JsonStringValue("orderType", it["OrderType"].ToString()));
                                    collection1.Add(new JsonStringValue("orderTypeName", it["OrderTypeName"].ToString()));
                                    collection1.Add(new JsonStringValue("statusName", it["StatusName"].ToString()));
                                    collection1.Add(new JsonStringValue("orderDate", it["OrderDate"].ToString()));
                                    collection1.Add(new JsonStringValue("createTime", it["CreateTime"].ToString()));
                                    collection1.Add(new JsonStringValue("custneedTime", it["CustneedTime"].ToString()));
                                    collection1.Add(new JsonStringValue("custNo", it["CustNo"].ToString()));
                                    collection1.Add(new JsonStringValue("custName", it["CustName"].ToString()));
                                    collection1.Add(new JsonStringValue("linkMan", it["LinkMan"].ToString()));
                                    collection1.Add(new JsonStringValue("linkTel", it["LinkTel"].ToString()));
                                    collection1.Add(new JsonStringValue("addr", it["Addr"].ToString()));
                                    bool isback = bool.Parse(it["IsBack"].ToString());
                                    collection1.Add(new JsonStringValue("isBack", (isback == true ? "1" : "0")));
                                    bool ishangup = bool.Parse(it["IsHangUp"].ToString());
                                    collection1.Add(new JsonStringValue("isHangUp", (ishangup == true ? "1" : "0")));
                                    collection2.Add(new JsonStringValue(row.ToString(), collection1.ToString()));

                                    row++;
                                }

                                collection.Add(new JsonStringValue("info", collection2.ToString()));
                                collection.Add(new JsonStringValue("rows", (row - 1).ToString()));
                                flag = 0;
                                info = "";
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                            finally
                            {
                                if (cmd != null)
                                    cmd.Dispose();
                                if (con != null)
                                    con.Dispose();
                            }
                        }
                        #endregion
                        #region [getOrderInfo 获取工单状态]
                        else if (method == "getOrderInfo")
                        {
                            try
                            {
                                getOrderInfo list = JSON.getOrderInfo(str)[0];
                                Business.Order.BusinessWorkOrder order = new Business.Order.BusinessWorkOrder();
                                order.loadOrderNo(list.orderNo, user.Entity.AccID);

                                collection.Add(new JsonStringValue("orderNo", order.Entity.OrderNo));
                                collection.Add(new JsonStringValue("orderName", order.Entity.OrderName));
                                collection.Add(new JsonStringValue("saleNo", order.Entity.SaleNo));
                                collection.Add(new JsonStringValue("status", order.Entity.Status));
                                collection.Add(new JsonStringValue("statusName", order.Entity.StatusName));
                                collection.Add(new JsonStringValue("orderType", order.Entity.OrderType));
                                collection.Add(new JsonStringValue("orderTypeName", order.Entity.OrderTypeName));
                                collection.Add(new JsonStringValue("region", order.Entity.RegionName));
                                collection.Add(new JsonStringValue("alloDept", order.Entity.AlloDeptName));
                                collection.Add(new JsonStringValue("alloUser", order.Entity.AlloUserName));
                                collection.Add(new JsonStringValue("createTime", order.Entity.CreateTime.ToString("yyyy-MM-dd")));
                                collection.Add(new JsonStringValue("custneedTime", order.Entity.CustneedTime.ToString("yyyy-MM-dd HH:mm")));
                                collection.Add(new JsonStringValue("custNo", order.Entity.CustNo));
                                collection.Add(new JsonStringValue("custName", order.Entity.CustName));
                                collection.Add(new JsonStringValue("linkMan", order.Entity.LinkMan));
                                collection.Add(new JsonStringValue("linkTel", order.Entity.LinkTel));
                                collection.Add(new JsonStringValue("addr", order.Entity.Addr));
                                collection.Add(new JsonStringValue("isBack", (order.Entity.IsBack == true ? "1" : "0")));
                                collection.Add(new JsonStringValue("isHangUp", (order.Entity.IsHangUp == true ? "1" : "0")));
                                collection.Add(new JsonStringValue("nodeNo", order.Entity.NodeNo)); //当前节点

                                //1.部门负责人，2.处理人，3.同时是部门负责人和处理人
                                if (user.Entity.UserNo == order.Entity.AlloUser && !order.Entity.Person.Contains(user.Entity.UserNo))
                                    collection.Add(new JsonStringValue("userType", "1"));
                                else if (user.Entity.UserNo != order.Entity.AlloUser && order.Entity.Person.Contains(user.Entity.UserNo))
                                    collection.Add(new JsonStringValue("userType", "2"));
                                else
                                    collection.Add(new JsonStringValue("userType", "3"));


                                string sqlstr = "select top 1 b.NodeNo,c.NodeName,c.ProcMode from Base_Order_Type a " +
                                        "left join Base_Flow_Detail b on a.FlowNo=b.FlowNo " +
                                        "left join Base_Flow_Node c on b.NodeNo=c.NodeNo " +
                                        "where a.OrderTypeNo='" + order.Entity.OrderType + "' and b.NodeNo>'" + order.Entity.NodeNo + "' " +
                                        "order by b.NodeNo asc";
                                DataTable dt = obj.ExecuteDataSet(sqlstr).Tables[0];

                                string nodeno = "";
                                if (dt.Rows.Count > 0)
                                {
                                    nodeno = dt.Rows[0]["NodeNo"].ToString();
                                    collection.Add(new JsonStringValue("nextNodeNo", dt.Rows[0]["NodeNo"].ToString())); //下一个节点
                                    collection.Add(new JsonStringValue("nextNodeName", dt.Rows[0]["NodeName"].ToString())); //下一个节点
                                    collection.Add(new JsonStringValue("procMode", dt.Rows[0]["ProcMode"].ToString())); //处理方式

                                    Business.Base.BusinessFlowNode node = new Business.Base.BusinessFlowNode();
                                    node.load(nodeno, user.Entity.AccID);
                                    collection.Add(new JsonStringValue("opNo", node.Entity.OpNo));
                                }
                                else
                                {
                                    collection.Add(new JsonStringValue("nextNodeNo", ""));//下一个节点
                                    collection.Add(new JsonStringValue("nextNodeName", "")); //下一个节点
                                    collection.Add(new JsonStringValue("procMode", ""));  //处理方式
                                    collection.Add(new JsonStringValue("opNo", ""));
                                }


                                flag = 0;
                                info = "";
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                        }
                        #endregion

                        #region [hangUpOrder 挂起]
                        else if (method == "hangUpOrder")
                        {
                            try
                            {
                                hangUpOrderInfo list = JSON.hangUpOrderInfo(str)[0];
                                Business.Order.BusinessWorkOrder order = new Business.Order.BusinessWorkOrder();
                                order.loadOrderNo(list.orderNo, user.Entity.AccID);

                                string InfoMsg = order.HangUp(user.Entity.AccID, list.orderNo, list.hangUpReason, list.GPS_X, list.GPS_Y, user.Entity.UserNo, user.Entity.UserName);

                                if (InfoMsg != "")
                                {
                                    flag = 1;
                                    info = InfoMsg;
                                }
                                else
                                {
                                    flag = 0;
                                    info = "";
                                }
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                        }
                        #endregion
                        #region [unHangUpOrder 取消挂起]
                        else if (method == "unHangUpOrder")
                        {
                            try
                            {
                                treateOrderInfo list = JSON.treateOrderInfo(str)[0];
                                Business.Order.BusinessWorkOrder order = new Business.Order.BusinessWorkOrder();
                                order.loadOrderNo(list.orderNo, user.Entity.AccID);

                                string InfoMsg = order.UnHangUp(user.Entity.AccID, list.orderNo, list.GPS_X, list.GPS_Y, user.Entity.UserNo, user.Entity.UserName);

                                if (InfoMsg != "")
                                {
                                    flag = 1;
                                    info = InfoMsg;
                                }
                                else
                                {
                                    flag = 0;
                                    info = "";
                                }
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                        }
                        #endregion
                        #region [backOrder 退回]
                        else if (method == "backOrder")
                        {
                            try
                            {
                                backOrderInfo list = JSON.backOrderInfo(str)[0];
                                Business.Order.BusinessWorkOrder order = new Business.Order.BusinessWorkOrder();
                                order.loadOrderNo(list.orderNo, user.Entity.AccID);

                                string InfoMsg = order.back(user.Entity.AccID, list.orderNo, list.backReason, list.GPS_X, list.GPS_Y, user.Entity.UserNo, user.Entity.UserName);

                                if (InfoMsg != "")
                                {
                                    flag = 1;
                                    info = InfoMsg;
                                }
                                else
                                {
                                    flag = 0;
                                    info = "";
                                }
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                        }
                        #endregion
                        #region [applyOrder 申请支援]
                        else if (method == "applyOrder")
                        {
                            try
                            {
                                applyOrderInfo list = JSON.applyOrderInfo(str)[0];
                                Business.Order.BusinessWorkOrder order = new Business.Order.BusinessWorkOrder();
                                order.loadOrderNo(list.orderNo, user.Entity.AccID);

                                string InfoMsg = order.apply(user.Entity.AccID, list.orderNo, list.applyReason, list.GPS_X, list.GPS_Y, user.Entity.UserNo, user.Entity.UserName);

                                if (InfoMsg != "")
                                {
                                    flag = 1;
                                    info = InfoMsg;
                                }
                                else
                                {
                                    flag = 0;
                                    info = "";
                                }
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                        }
                        #endregion
                        #region [responseOrder 响应]
                        else if (method == "responseOrder")
                        {
                            try
                            {
                                treateOrderInfo list = JSON.treateOrderInfo(str)[0];
                                Business.Order.BusinessWorkOrder order = new Business.Order.BusinessWorkOrder();
                                order.loadOrderNo(list.orderNo, user.Entity.AccID);

                                string InfoMsg = order.response(user.Entity.AccID, list.orderNo, list.GPS_X, list.GPS_Y, user.Entity.UserNo, user.Entity.UserName);

                                if (InfoMsg != "")
                                {
                                    flag = 1;
                                    info = InfoMsg;
                                }
                                else
                                {
                                    flag = 0;
                                    info = "";
                                }
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                        }
                        #endregion
                        #region [appoIntOrder 预约]
                        else if (method == "appoIntOrder")
                        {
                            try
                            {
                                appoIntOrderInfo list = JSON.appoIntOrderInfo(str)[0];
                                Business.Order.BusinessWorkOrder order = new Business.Order.BusinessWorkOrder();
                                order.loadOrderNo(list.orderNo, user.Entity.AccID);

                                string InfoMsg = order.appoInt(user.Entity.AccID, list.orderNo, list.appoIntTime, list.GPS_X, list.GPS_Y, user.Entity.UserNo, user.Entity.UserName);

                                if (InfoMsg != "")
                                {
                                    flag = 1;
                                    info = InfoMsg;
                                }
                                else
                                {
                                    flag = 0;
                                    info = "";
                                }
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                        }
                        #endregion
                        #region [signOrder 签到]
                        else if (method == "signOrder")
                        {
                            try
                            {
                                treateOrderInfo list = JSON.treateOrderInfo(str)[0];
                                Business.Order.BusinessWorkOrder order = new Business.Order.BusinessWorkOrder();
                                order.loadOrderNo(list.orderNo, user.Entity.AccID);

                                string InfoMsg = order.sign(user.Entity.AccID, list.orderNo, list.GPS_X, list.GPS_Y, user.Entity.UserNo, user.Entity.UserName);

                                if (InfoMsg != "")
                                {
                                    flag = 1;
                                    info = InfoMsg;
                                }
                                else
                                {
                                    flag = 0;
                                    info = "";
                                }
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                        }
                        #endregion
                        #region [workOrder 执行]
                        else if (method == "workOrder")
                        {
                            try
                            {
                                treateOrderInfo list = JSON.treateOrderInfo(str)[0];
                                Business.Order.BusinessWorkOrder order = new Business.Order.BusinessWorkOrder();
                                order.loadOrderNo(list.orderNo, user.Entity.AccID);

                                string InfoMsg = order.work(user.Entity.AccID, list.orderNo, list.GPS_X, list.GPS_Y, user.Entity.UserNo, user.Entity.UserName);

                                if (InfoMsg != "")
                                {
                                    flag = 1;
                                    info = InfoMsg;
                                }
                                else
                                {
                                    flag = 0;
                                    info = "";
                                }
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                        }
                        #endregion
                        #region [finishOrder 完成]
                        else if (method == "finishOrder")
                        {
                            try
                            {
                                treateOrderInfo list = JSON.treateOrderInfo(str)[0];
                                Business.Order.BusinessWorkOrder order = new Business.Order.BusinessWorkOrder();
                                order.loadOrderNo(list.orderNo, user.Entity.AccID);

                                string InfoMsg = order.finish(user.Entity.AccID, list.orderNo, list.GPS_X, list.GPS_Y, user.Entity.UserNo, user.Entity.UserName);

                                if (InfoMsg != "")
                                {
                                    flag = 1;
                                    info = InfoMsg;
                                }
                                else
                                {
                                    flag = 0;
                                    info = "";
                                }
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                        }
                        #endregion
                        #region [closeOrder 销单]
                        else if (method == "closeOrder")
                        {
                            try
                            {
                                treateOrderInfo list = JSON.treateOrderInfo(str)[0];
                                Business.Order.BusinessWorkOrder order = new Business.Order.BusinessWorkOrder();
                                order.loadOrderNo(list.orderNo, user.Entity.AccID);

                                string InfoMsg = order.close(user.Entity.AccID, list.orderNo, list.GPS_X, list.GPS_Y, user.Entity.UserNo, user.Entity.UserName);

                                if (InfoMsg != "")
                                {
                                    flag = 1;
                                    info = InfoMsg;
                                }
                                else
                                {
                                    flag = 0;
                                    info = "";
                                }
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                        }
                        #endregion

                        #region [getUserList 获取用户列表]
                        else if (method == "getUserList")
                        {
                            int row = 1;
                            try
                            {
                                getUserList list = JSON.getUserList(str)[0];

                                Business.Order.BusinessWorkOrderPerson person = new Business.Order.BusinessWorkOrderPerson();
                                foreach (Entity.Order.EntityWorkOrderPerson it in person.GetWorkOrderPersonListQuery(user.Entity.AccID, list.orderNo, string.Empty, null, false))
                                {
                                    JsonObjectCollection collection1 = new JsonObjectCollection();
                                    collection1.Add(new JsonStringValue("id", it.InnerEntityOID));
                                    collection1.Add(new JsonStringValue("userNo", it.UserNo));
                                    collection1.Add(new JsonStringValue("userName", it.UserName));
                                    collection1.Add(new JsonStringValue("isCheck", "Y"));

                                    collection.Add(new JsonStringValue(row.ToString(), collection1.ToString()));
                                    row++;
                                }

                                Business.Sys.BusinessUserInfo uslist = new Business.Sys.BusinessUserInfo();
                                foreach (Entity.Sys.EntityUserInfo it in uslist.GetUserInfoListQuery(string.Empty, user.Entity.AccID, user.Entity.DeptNo, string.Empty))
                                {
                                    JsonObjectCollection collection1 = new JsonObjectCollection();
                                    if (person.GetWorkOrderPersonListCount(user.Entity.AccID, list.orderNo, it.UserNo, null, false) == 0)
                                    {
                                        collection1.Add(new JsonStringValue("id", it.InnerEntityOID));
                                        collection1.Add(new JsonStringValue("userNo", it.UserNo));
                                        collection1.Add(new JsonStringValue("userName", it.UserName));
                                        collection1.Add(new JsonStringValue("isCheck", "N"));
                                        collection.Add(new JsonStringValue(row.ToString(), collection1.ToString()));
                                        row++;
                                    }
                                }

                                collection.Add(new JsonStringValue("count", (row - 1).ToString()));

                                flag = 0;
                                info = "";
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                        }
                        #endregion
                        #region [getUserList1 获取用户列表]
                        else if (method == "getUserList1")
                        {
                            int row = 1;
                            try
                            {
                                getUserList list = JSON.getUserList(str)[0];

                                JsonObjectCollection collection2 = new JsonObjectCollection();
                                Business.Order.BusinessWorkOrderPerson person = new Business.Order.BusinessWorkOrderPerson();
                                foreach (Entity.Order.EntityWorkOrderPerson it in person.GetWorkOrderPersonListQuery(user.Entity.AccID, list.orderNo, string.Empty, null, false))
                                {
                                    JsonObjectCollection collection1 = new JsonObjectCollection();
                                    collection1.Add(new JsonStringValue("id", it.InnerEntityOID));
                                    collection1.Add(new JsonStringValue("userNo", it.UserNo));
                                    collection1.Add(new JsonStringValue("userName", it.UserName));
                                    collection1.Add(new JsonStringValue("isCheck", "Y"));

                                    collection2.Add(new JsonStringValue(row.ToString(), collection1.ToString()));
                                    row++;
                                }

                                Business.Sys.BusinessUserInfo uslist = new Business.Sys.BusinessUserInfo();
                                foreach (Entity.Sys.EntityUserInfo it in uslist.GetUserInfoListQuery(string.Empty, user.Entity.AccID, user.Entity.DeptNo, string.Empty))
                                {
                                    JsonObjectCollection collection1 = new JsonObjectCollection();
                                    if (person.GetWorkOrderPersonListCount(user.Entity.AccID, list.orderNo, it.UserNo, null, false) == 0)
                                    {
                                        collection1.Add(new JsonStringValue("id", it.InnerEntityOID));
                                        collection1.Add(new JsonStringValue("userNo", it.UserNo));
                                        collection1.Add(new JsonStringValue("userName", it.UserName));
                                        collection1.Add(new JsonStringValue("isCheck", "N"));

                                        collection2.Add(new JsonStringValue(row.ToString(), collection1.ToString()));
                                        row++;
                                    }
                                }

                                collection.Add(new JsonStringValue("info", collection2.ToString()));
                                collection.Add(new JsonStringValue("rows", (row - 1).ToString()));

                                flag = 0;
                                info = "";
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                        }
                        #endregion
                        #region [addOrderUsers 工单增加用户]
                        else if (method == "addOrderUsers")
                        {
                            try
                            {
                                foreach (addOrderUsers it in JSON.addOrderUsers(str.Replace("[[", "[").Replace("]]", "]")))
                                {
                                    Business.Order.BusinessWorkOrder bc = new project.Business.Order.BusinessWorkOrder();
                                    bc.loadOrderNo(it.orderNo, user.Entity.AccID);

                                    Business.Order.BusinessWorkOrderPerson person = new Business.Order.BusinessWorkOrderPerson();
                                    if (person.GetWorkOrderPersonListCount(user.Entity.AccID, bc.Entity.OrderNo, it.userNo, null, false) == 0)
                                    {
                                        person.Entity.AccID = user.Entity.AccID;
                                        person.Entity.CreateDate = GetDate();
                                        person.Entity.CreateUser = user.Entity.UserName;
                                        person.Entity.IsBack = false;
                                        person.Entity.IsDel = false;
                                        person.Entity.OrderNo = bc.Entity.OrderNo;
                                        person.Entity.UserNo = it.userNo;
                                        person.Entity.UpdateUser = user.Entity.UserName;
                                        person.Entity.UpdateDate = GetDate();
                                        int row = person.Save();

                                        if (row > 0)
                                        {
                                            Business.Order.BusinessWorkOrderMsg msg = new Business.Order.BusinessWorkOrderMsg();
                                            msg.Entity.AccID = user.Entity.AccID;
                                            msg.Entity.Sender = user.Entity.UserNo;
                                            msg.Entity.SendDate = GetDate();
                                            msg.Entity.ToUser = person.Entity.UserNo;
                                            msg.Entity.Subject = "您有一张新的工单！";
                                            msg.Entity.Context = "你有新的工单需要处理，工单号：" + bc.Entity.OrderNo;
                                            msg.Entity.RefNo = bc.Entity.OrderNo;
                                            msg.Entity.MsgType = "1";
                                            msg.Entity.IsDel = false;
                                            msg.Entity.IsRead = false;
                                            msg.Entity.CreateDate = GetDate();
                                            msg.Entity.CreateUser = user.Entity.UserNo;
                                            msg.Save();

                                            flag = 0;
                                            info = "";

                                            //修改退回状态
                                            obj.ExecuteNonQuery("update WO_WorkOrder set IsBack=0,BackDate=null,backReason='' where RowPointer='" + bc.Entity.InnerEntityOID + "' and AccID='" + bc.Entity.AccID + "'");
                                            obj.ExecuteNonQuery("update WO_WorkOrder_Person set IsBack=0 where OrderNo='" + bc.Entity.OrderNo + "' and AccID='" + bc.Entity.AccID + "'");
                                        }
                                        else
                                        {
                                            flag = 4;
                                            info = "指派不成功！";
                                        }
                                    }
                                    else
                                    {
                                        obj.ExecuteNonQuery("Update WO_WorkOrder_Person set IsDel=1 where OrderNo='" + bc.Entity.OrderNo + "' and AccID='" + user.Entity.AccID + "' and UserNo ='" + it.userNo + "'");
                                    }
                                }

                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                        }
                        #endregion
                        #region [uploadImg 图片记录]
                        else if (method == "uploadImg")
                        {
                            try
                            {
                                uploadImg list = JSON.uploadImg(str)[0];
                                Business.Order.BusinessWorkOrderImages img = new Business.Order.BusinessWorkOrderImages();
                                img.Entity.AccID = user.Entity.AccID;
                                img.Entity.OrderNo = list.orderNo;
                                img.Entity.NodeNo = list.nodeNo;
                                img.Entity.Img = list.img;
                                img.Entity.UploadDate = GetDate();
                                img.Entity.UploadUser = user.Entity.UserNo;
                                int r = img.Save();

                                if (r > 0)
                                {
                                    flag = 0;
                                    info = "";
                                }
                                else
                                {
                                    flag = 4;
                                    info = "上传出错！";
                                }
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                        }
                        #endregion
                        #region [getMessageList 获取系统消息]
                        else if (method == "getMessageList")
                        {
                            try
                            {
                                getMessageList list = JSON.getMessageList(str)[0];

                                DateTime startDate = default(DateTime);
                                DateTime endDate = default(DateTime);
                                if (list.startDate != "") startDate = DateTime.Parse(list.startDate);
                                if (list.endDate != "") endDate = DateTime.Parse(list.endDate);

                                int row = 1;
                                Business.Order.BusinessWorkOrderMsg cost = new Business.Order.BusinessWorkOrderMsg();
                                foreach (Entity.Order.EntityWorkOrderMsg it in cost.GetWorkOrderMsgQuery(user.Entity.AccID, string.Empty, startDate, endDate, user.Entity.UserNo, null, list.orderNo, false, list.pageIndex, list.pageSize))
                                {
                                    JsonObjectCollection collection1 = new JsonObjectCollection();
                                    collection1.Add(new JsonStringValue("id", it.InnerEntityOID));
                                    collection1.Add(new JsonStringValue("sender", it.SenderName));
                                    collection1.Add(new JsonStringValue("senderDate", it.CreateDate.ToString("yyyy-MM-dd HH:mm:ss")));
                                    collection1.Add(new JsonStringValue("subject", it.Subject));
                                    collection1.Add(new JsonStringValue("context", it.Context));
                                    collection1.Add(new JsonStringValue("isRead", (it.IsRead == true ? "Y" : "N")));
                                    collection1.Add(new JsonStringValue("refNo", it.RefNo));

                                    collection.Add(new JsonStringValue(row.ToString(), collection1.ToString()));
                                    row++;
                                }

                                flag = 0;
                                info = "";
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                        }
                        #endregion
                        #region [getMessageList1 获取系统消息]
                        else if (method == "getMessageList1")
                        {
                            try
                            {
                                getMessageList list = JSON.getMessageList(str)[0];

                                DateTime startDate = default(DateTime);
                                DateTime endDate = default(DateTime);
                                if (list.startDate != "") startDate = DateTime.Parse(list.startDate);
                                if (list.endDate != "") endDate = DateTime.Parse(list.endDate);

                                int row = 1;
                                JsonObjectCollection collection2 = new JsonObjectCollection();
                                Business.Order.BusinessWorkOrderMsg cost = new Business.Order.BusinessWorkOrderMsg();
                                foreach (Entity.Order.EntityWorkOrderMsg it in cost.GetWorkOrderMsgQuery(user.Entity.AccID, string.Empty, startDate, endDate, user.Entity.UserNo, null, list.orderNo, false, list.pageIndex, list.pageSize))
                                {
                                    JsonObjectCollection collection1 = new JsonObjectCollection();
                                    collection1.Add(new JsonStringValue("id", it.InnerEntityOID));
                                    collection1.Add(new JsonStringValue("sender", it.SenderName));
                                    collection1.Add(new JsonStringValue("senderDate", it.CreateDate.ToString("yyyy-MM-dd HH:mm:ss")));
                                    collection1.Add(new JsonStringValue("subject", it.Subject));
                                    collection1.Add(new JsonStringValue("context", it.Context));
                                    collection1.Add(new JsonStringValue("isRead", (it.IsRead == true ? "Y" : "N")));
                                    collection1.Add(new JsonStringValue("refNo", it.RefNo));

                                    collection2.Add(new JsonStringValue(row.ToString(), collection1.ToString()));
                                    row++;
                                }
                                collection.Add(new JsonStringValue("info", collection2.ToString()));
                                collection.Add(new JsonStringValue("rows", (row - 1).ToString()));

                                flag = 0;
                                info = "";
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                        }
                        #endregion
                        #region [viewMessageList 获取系统消息]
                        else if (method == "viewMessageList")
                        {
                            try
                            {
                                viewMessageList list = JSON.viewMessageList(str)[0];
                                Business.Order.BusinessWorkOrderMsg msg = new Business.Order.BusinessWorkOrderMsg();
                                msg.load(list.id, user.Entity.AccID);
                                msg.Entity.IsRead = true;
                                msg.Entity.ReadDate = GetDate();
                                int row = msg.Save();
                                if (row <= 0)
                                {
                                    flag = 4;
                                    info = "消息读取出错！";
                                }
                                else
                                {
                                    flag = 0;
                                    info = "";
                                }
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                        }
                        #endregion

                        #region [isExpense 判断工单是否计费]
                        else if (method == "isExpense")
                        {
                            try
                            {
                                getOrderInfo list = JSON.getOrderInfo(str)[0];
                                Business.Order.BusinessWorkOrderCostDetail cost = new Business.Order.BusinessWorkOrderCostDetail();
                                int row = cost.GetWorkOrderCostDetailCount(user.Entity.AccID, string.Empty, list.orderNo, string.Empty, string.Empty, default(DateTime), default(DateTime));
                                if (row > 0)
                                    collection.Add(new JsonStringValue("isExpense", "Y"));
                                else
                                    collection.Add(new JsonStringValue("isExpense", "N"));

                                flag = 0;
                                info = "";
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                        }
                        #endregion
                        #region [getCostType 费用类别]
                        else if (method == "getCostType")
                        {
                            try
                            {
                                int row = 1;
                                Business.Base.BusinessDict dict = new Business.Base.BusinessDict();
                                foreach (Entity.Base.EntityDict it in dict.GetDictListQuery(string.Empty, string.Empty, user.Entity.AccID, "CostType"))
                                {
                                    JsonObjectCollection collection1 = new JsonObjectCollection();
                                    collection1.Add(new JsonStringValue("costTypeNo", it.DictNo));
                                    collection1.Add(new JsonStringValue("costTypeName", it.DictName));

                                    collection.Add(new JsonStringValue(row.ToString(), collection1.ToString()));
                                    row++;
                                }
                                collection.Add(new JsonStringValue("count", (row - 1).ToString()));

                                flag = 0;
                                info = "";
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                        }
                        #endregion
                        #region [entryOrderCost 录入费用]
                        else if (method == "entryOrderCost")
                        {
                            try
                            {
                                entryOrderCost list = JSON.entryOrderCost(str)[0];

                                string costNo = "";
                                DataTable dt = obj.ExecuteDataSet("select CostNo,Status from WO_WorkOrder_Cost where OrderNo='" + list.orderNo + "'").Tables[0];
                                if (dt.Rows.Count > 0)
                                {
                                    if (dt.Rows[0]["Status"].ToString() != "OPEN")
                                    {
                                        flag = 4;
                                        info = "当前费用已审核，不能添加！";
                                    }
                                    else
                                        costNo = dt.Rows[0]["CostNo"].ToString();
                                }
                                else
                                {
                                    string today = GetDate().ToString("yyMMdd");
                                    DataTable dt1 = obj.ExecuteDataSet("select top 1 CostNo from WO_WorkOrder_Cost where CostNo like 'C" + today + "%' and AccID='" + user.Entity.AccID + "' order by OrderNo desc").Tables[0];
                                    if (dt1.Rows.Count > 0)
                                        costNo = "C" + (long.Parse(dt1.Rows[0]["CostNo"].ToString().Replace("C", "")) + 1).ToString();
                                    else
                                        costNo = "C" + today + "0001";

                                    Business.Order.BusinessWorkOrderCost cost = new Business.Order.BusinessWorkOrderCost();
                                    cost.Entity.AccID = user.Entity.AccID;
                                    cost.Entity.OrderNo = list.orderNo;
                                    cost.Entity.CostNo = costNo;
                                    cost.Entity.CostDate = GetDate();
                                    cost.Entity.CostAmount = decimal.Parse(list.costAmount);
                                    cost.Entity.Status = "OPEN";
                                    cost.Entity.CreateDate = GetDate();
                                    cost.Entity.CreateUser = user.Entity.UserNo;
                                    cost.Save();
                                }

                                if (flag != 4)
                                {
                                    Business.Order.BusinessWorkOrderCostDetail detail = new Business.Order.BusinessWorkOrderCostDetail();
                                    detail.Entity.AccID = user.Entity.AccID;
                                    detail.Entity.CostNo = costNo;
                                    detail.Entity.OrderNo = list.orderNo;
                                    detail.Entity.CostType = list.costType;
                                    detail.Entity.Context = list.context;
                                    detail.Entity.CostDate = GetDate();
                                    detail.Entity.CostAmount = decimal.Parse(list.costAmount);
                                    detail.Entity.UserNo = user.Entity.UserNo;
                                    detail.Entity.CreateDate = GetDate();
                                    detail.Entity.CreateUser = user.Entity.UserNo;
                                    detail.Entity.UpdateDate = GetDate();
                                    detail.Entity.UpdateUser = user.Entity.UserNo;
                                    int row = detail.Save();
                                    if (row > 0)
                                    {
                                        obj.ExecuteNonQuery("update WO_WorkOrder_Cost set CostAmount = isnull((select SUM(CostAmount) from WO_WorkOrder_Cost_Detail where OrderNo='" + list.orderNo + "'),0) " +
                                            "where OrderNo = '" + list.orderNo + "'");
                                        flag = 0;
                                        info = "";
                                    }
                                    else
                                    {
                                        flag = 5;
                                        info = "保存数据出错！";
                                    }
                                }
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                        }
                        #endregion
                        #region [deleteOrderCost 删除费用记录]
                        else if (method == "deleteOrderCost")
                        {
                            try
                            {
                                deleteOrderCost list = JSON.deleteOrderCost(str)[0];
                                Business.Order.BusinessWorkOrderCostDetail detail = new Business.Order.BusinessWorkOrderCostDetail();
                                detail.load(list.id, user.Entity.AccID);

                                DataTable dt = obj.ExecuteDataSet("select Status from WO_WorkOrder_Cost where OrderNo='" + detail.Entity.OrderNo + "'").Tables[0];
                                if (dt.Rows.Count == 0)
                                {
                                    flag = 4;
                                    info = "当前工单信息有误，请确认！";
                                }
                                else
                                {
                                    if (dt.Rows[0]["Status"].ToString() != "OPEN")
                                    {
                                        flag = 5;
                                        info = "费用信息已审核，不能修改！";
                                    }
                                    else
                                    {
                                        int row = detail.delete();
                                        if (row <= 0)
                                        {
                                            flag = 6;
                                            info = "费用信息出错！";
                                        }
                                        else
                                        {
                                            obj.ExecuteNonQuery("update WO_WorkOrder_Cost set CostAmount = isnull((select SUM(CostAmount) from WO_WorkOrder_Cost_Detail where OrderNo='" + detail.Entity.OrderNo + "'),0) " +
                                                "where OrderNo = '" + detail.Entity.OrderNo + "'");
                                            flag = 0;
                                            info = "";
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                        }
                        #endregion
                        #region [updateOrderCost 修改费用记录]
                        else if (method == "updateOrderCost")
                        {
                            try
                            {
                                updateOrderCost list = JSON.updateOrderCost(str)[0];
                                Business.Order.BusinessWorkOrderCostDetail detail = new Business.Order.BusinessWorkOrderCostDetail();
                                detail.load(list.id, user.Entity.AccID);

                                DataTable dt = obj.ExecuteDataSet("select Status from WO_WorkOrder_Cost where OrderNo='" + detail.Entity.OrderNo + "'").Tables[0];
                                if (dt.Rows.Count == 0)
                                {
                                    flag = 4;
                                    info = "当前工单信息有误，请确认！";
                                }
                                else
                                {
                                    if (dt.Rows[0]["Status"].ToString() != "OPEN")
                                    {
                                        flag = 5;
                                        info = "费用信息已审核，不能修改！";
                                    }
                                    else
                                    {
                                        detail.Entity.CostType = list.costType;
                                        detail.Entity.Context = list.context;
                                        detail.Entity.CostAmount = decimal.Parse(list.costAmount);
                                        detail.Entity.UpdateDate = GetDate();
                                        detail.Entity.UpdateUser = user.Entity.UserNo;
                                        int row = detail.Save();
                                        if (row <= 0)
                                        {
                                            flag = 6;
                                            info = "费用信息出错！";
                                        }
                                        else
                                        {
                                            obj.ExecuteNonQuery("update WO_WorkOrder_Cost set CostAmount = isnull((select SUM(CostAmount) from WO_WorkOrder_Cost_Detail where OrderNo='" + detail.Entity.OrderNo + "'),0) " +
                                                "where OrderNo = '" + detail.Entity.OrderNo + "'");
                                            flag = 0;
                                            info = "";
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                        }
                        #endregion
                        #region [getCostList 查询费用记录]
                        else if (method == "getCostList")
                        {
                            try
                            {
                                getOrderInfo list = JSON.getOrderInfo(str)[0];
                                string status = "";
                                DataTable dt = obj.ExecuteDataSet("select Status from WO_WorkOrder_Cost where OrderNo='" + list.orderNo + "'").Tables[0];
                                if (dt.Rows.Count > 0) status = dt.Rows[0]["Status"].ToString();
                                collection.Add(new JsonStringValue("status", status));

                                int row = 1;
                                Business.Order.BusinessWorkOrderCostDetail cost = new Business.Order.BusinessWorkOrderCostDetail();
                                foreach (Entity.Order.EntityWorkOrderCostDetail it in cost.GetWorkOrderCostDetailQuery(user.Entity.AccID, string.Empty, list.orderNo, string.Empty, string.Empty, default(DateTime), default(DateTime)))
                                {
                                    JsonObjectCollection collection1 = new JsonObjectCollection();
                                    collection1.Add(new JsonStringValue("id", it.InnerEntityOID));
                                    collection1.Add(new JsonStringValue("userName", it.UserName));
                                    collection1.Add(new JsonStringValue("costType", it.CostTypeName));
                                    collection1.Add(new JsonStringValue("context", it.Context));
                                    collection1.Add(new JsonStringValue("costAmount", it.CostAmount.ToString("0.##")));
                                    collection1.Add(new JsonStringValue("costDate", it.CostDate.ToString("yyyy-MM-dd HH:mm:ss")));

                                    collection.Add(new JsonStringValue(row.ToString(), collection1.ToString()));
                                    row++;
                                }
                                collection.Add(new JsonStringValue("count", (row - 1).ToString()));

                                flag = 0;
                                info = "";
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                        }
                        #endregion

                        #region [getFeeType 收款项目]
                        else if (method == "getFeeType")
                        {
                            try
                            {
                                int row = 1;
                                Business.Base.BusinessDict dict = new Business.Base.BusinessDict();
                                foreach (Entity.Base.EntityDict it in dict.GetDictListQuery(string.Empty, string.Empty, user.Entity.AccID, "FeeType"))
                                {
                                    JsonObjectCollection collection1 = new JsonObjectCollection();
                                    collection1.Add(new JsonStringValue("feeTypeNo", it.DictNo));
                                    collection1.Add(new JsonStringValue("feeTypeName", it.DictName));

                                    collection.Add(new JsonStringValue(row.ToString(), collection1.ToString()));
                                    row++;
                                }
                                collection.Add(new JsonStringValue("count", (row - 1).ToString()));

                                flag = 0;
                                info = "";
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                        }
                        #endregion
                        #region [entryOrderFee 收款登记]
                        else if (method == "entryOrderFee")
                        {
                            try
                            {
                                entryOrderFee list = JSON.entryOrderFee(str)[0];

                                string FeeNo = "";
                                DataTable dt = obj.ExecuteDataSet("select FeeNo,Status from WO_WorkOrder_Fee where OrderNo='" + list.orderNo + "'").Tables[0];
                                if (dt.Rows.Count > 0)
                                {
                                    if (dt.Rows[0]["Status"].ToString() != "OPEN")
                                    {
                                        flag = 4;
                                        info = "当前费用已审核，不能添加！";
                                    }
                                    else
                                        FeeNo = dt.Rows[0]["FeeNo"].ToString();
                                }
                                else
                                {
                                    string today = GetDate().ToString("yyMMdd");
                                    DataTable dt1 = obj.ExecuteDataSet("select top 1 FeeNo from WO_WorkOrder_Fee where FeeNo like 'C" + today + "%' and AccID='" + user.Entity.AccID + "' order by OrderNo desc").Tables[0];
                                    if (dt1.Rows.Count > 0)
                                        FeeNo = "C" + (long.Parse(dt1.Rows[0]["FeeNo"].ToString().Replace("C", "")) + 1).ToString();
                                    else
                                        FeeNo = "C" + today + "0001";

                                    Business.Order.BusinessWorkOrderFee Fee = new Business.Order.BusinessWorkOrderFee();
                                    Fee.Entity.AccID = user.Entity.AccID;
                                    Fee.Entity.OrderNo = list.orderNo;
                                    Fee.Entity.FeeNo = FeeNo;
                                    Fee.Entity.FeeDate = GetDate();
                                    Fee.Entity.FeeAmount = decimal.Parse(list.feeAmount);
                                    Fee.Entity.Status = "OPEN";
                                    Fee.Entity.CreateDate = GetDate();
                                    Fee.Entity.CreateUser = user.Entity.UserNo;
                                    Fee.Save();
                                }

                                if (flag != 4)
                                {
                                    Business.Order.BusinessWorkOrderFeeDetail detail = new Business.Order.BusinessWorkOrderFeeDetail();
                                    detail.Entity.AccID = user.Entity.AccID;
                                    detail.Entity.FeeNo = FeeNo;
                                    detail.Entity.OrderNo = list.orderNo;
                                    detail.Entity.FeeType = list.feeType;
                                    detail.Entity.Context = list.context;
                                    detail.Entity.FeeDate = GetDate();
                                    detail.Entity.FeeAmount = decimal.Parse(list.feeAmount);
                                    detail.Entity.UserNo = user.Entity.UserNo;
                                    detail.Entity.CreateDate = GetDate();
                                    detail.Entity.CreateUser = user.Entity.UserNo;
                                    detail.Entity.UpdateDate = GetDate();
                                    detail.Entity.UpdateUser = user.Entity.UserNo;
                                    int row = detail.Save();
                                    if (row > 0)
                                    {
                                        obj.ExecuteNonQuery("update WO_WorkOrder_Fee set FeeAmount = isnull((select SUM(FeeAmount) from WO_WorkOrder_Fee_Detail where OrderNo='" + list.orderNo + "'),0) " +
                                            "where OrderNo = '" + list.orderNo + "'");
                                        flag = 0;
                                        info = "";
                                    }
                                    else
                                    {
                                        flag = 5;
                                        info = "保存数据出错！";
                                    }
                                }
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                        }
                        #endregion
                        #region [deleteOrderFee 删除收款记录]
                        else if (method == "deleteOrderFee")
                        {
                            try
                            {
                                deleteOrderFee list = JSON.deleteOrderFee(str)[0];
                                Business.Order.BusinessWorkOrderFeeDetail detail = new Business.Order.BusinessWorkOrderFeeDetail();
                                detail.load(list.id, user.Entity.AccID);

                                DataTable dt = obj.ExecuteDataSet("select Status from WO_WorkOrder_Fee where OrderNo='" + detail.Entity.OrderNo + "'").Tables[0];
                                if (dt.Rows.Count == 0)
                                {
                                    flag = 4;
                                    info = "当前工单信息有误，请确认！";
                                }
                                else
                                {
                                    if (dt.Rows[0]["Status"].ToString() != "OPEN")
                                    {
                                        flag = 5;
                                        info = "费用信息已审核，不能修改！";
                                    }
                                    else
                                    {
                                        int row = detail.delete();
                                        if (row <= 0)
                                        {
                                            flag = 6;
                                            info = "费用信息出错！";
                                        }
                                        else
                                        {
                                            obj.ExecuteNonQuery("update WO_WorkOrder_Fee set FeeAmount = isnull((select SUM(FeeAmount) from WO_WorkOrder_Fee_Detail where OrderNo='" + detail.Entity.OrderNo + "'),0) " +
                                                "where OrderNo = '" + detail.Entity.OrderNo + "'");
                                            flag = 0;
                                            info = "";
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                        }
                        #endregion
                        #region [updateOrderFee 修改收款记录]
                        else if (method == "updateOrderFee")
                        {
                            try
                            {
                                updateOrderFee list = JSON.updateOrderFee(str)[0];
                                Business.Order.BusinessWorkOrderFeeDetail detail = new Business.Order.BusinessWorkOrderFeeDetail();
                                detail.load(list.id, user.Entity.AccID);

                                DataTable dt = obj.ExecuteDataSet("select Status from WO_WorkOrder_Fee where OrderNo='" + detail.Entity.OrderNo + "'").Tables[0];
                                if (dt.Rows.Count == 0)
                                {
                                    flag = 4;
                                    info = "当前工单信息有误，请确认！";
                                }
                                else
                                {
                                    if (dt.Rows[0]["Status"].ToString() != "OPEN")
                                    {
                                        flag = 5;
                                        info = "费用信息已审核，不能修改！";
                                    }
                                    else
                                    {
                                        detail.Entity.FeeType = list.feeType;
                                        detail.Entity.Context = list.context;
                                        detail.Entity.FeeAmount = decimal.Parse(list.feeAmount);
                                        detail.Entity.UpdateDate = GetDate();
                                        detail.Entity.UpdateUser = user.Entity.UserNo;
                                        int row = detail.Save();
                                        if (row <= 0)
                                        {
                                            flag = 6;
                                            info = "费用信息出错！";
                                        }
                                        else
                                        {
                                            obj.ExecuteNonQuery("update WO_WorkOrder_Fee set FeeAmount = isnull((select SUM(FeeAmount) from WO_WorkOrder_Fee_Detail where OrderNo='" + detail.Entity.OrderNo + "'),0) " +
                                                "where OrderNo = '" + detail.Entity.OrderNo + "'");
                                            flag = 0;
                                            info = "";
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                        }
                        #endregion
                        #region [getFeeList 查询收款记录]
                        else if (method == "getFeeList")
                        {
                            try
                            {
                                getOrderInfo list = JSON.getOrderInfo(str)[0];
                                string status = "";
                                DataTable dt = obj.ExecuteDataSet("select Status from WO_WorkOrder_Fee where OrderNo='" + list.orderNo + "'").Tables[0];
                                if (dt.Rows.Count > 0) status = dt.Rows[0]["Status"].ToString();
                                collection.Add(new JsonStringValue("status", status));

                                int row = 1;
                                Business.Order.BusinessWorkOrderFeeDetail cost = new Business.Order.BusinessWorkOrderFeeDetail();
                                foreach (Entity.Order.EntityWorkOrderFeeDetail it in cost.GetWorkOrderFeeDetailQuery(user.Entity.AccID, string.Empty, list.orderNo, string.Empty, string.Empty, default(DateTime), default(DateTime)))
                                {
                                    JsonObjectCollection collection1 = new JsonObjectCollection();
                                    collection1.Add(new JsonStringValue("id", it.InnerEntityOID));
                                    collection1.Add(new JsonStringValue("userName", it.UserName));
                                    collection1.Add(new JsonStringValue("feeType", it.FeeTypeName));
                                    collection1.Add(new JsonStringValue("context", it.Context));
                                    collection1.Add(new JsonStringValue("feeAmount", it.FeeAmount.ToString("0.##")));
                                    collection1.Add(new JsonStringValue("feeDate", it.FeeDate.ToString("yyyy-MM-dd HH:mm:ss")));

                                    collection.Add(new JsonStringValue(row.ToString(), collection1.ToString()));
                                    row++;
                                }
                                collection.Add(new JsonStringValue("count", (row - 1).ToString()));

                                flag = 0;
                                info = "";
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                        }
                        #endregion

                        #region [isFee 判断工单是否设置费用金额]
                        else if (method == "isFee")
                        {
                            try
                            {
                                getOrderInfo list = JSON.getOrderInfo(str)[0];
                                Business.Order.BusinessWorkOrderFeeDetail cost = new Business.Order.BusinessWorkOrderFeeDetail();
                                int row = cost.GetWorkOrderFeeDetailCount(user.Entity.AccID, string.Empty, list.orderNo, string.Empty, string.Empty, default(DateTime), default(DateTime));
                                if (row > 0)
                                    collection.Add(new JsonStringValue("isFee", "Y"));
                                else
                                    collection.Add(new JsonStringValue("isFee", "N"));

                                flag = 0;
                                info = "";
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                        }
                        #endregion
                        #region [setFee 设置费用金额_线上]
                        else if (method == "setFee")
                        {
                            try
                            {
                                setFee list = JSON.setFee(str)[0];

                                string FeeNo = "";
                                Business.Order.BusinessWorkOrderFeeDetail cost = new Business.Order.BusinessWorkOrderFeeDetail();
                                int row = cost.GetWorkOrderFeeDetailCount(user.Entity.AccID, string.Empty, list.orderNo, string.Empty, string.Empty, default(DateTime), default(DateTime));
                                if (row > 0)
                                {
                                    flag = 4;
                                }
                                else
                                {
                                    string today = GetDate().ToString("yyMMdd");
                                    DataTable dt1 = obj.ExecuteDataSet("select top 1 FeeNo from WO_WorkOrder_Fee where FeeNo like 'C" + today + "%' and AccID='" + user.Entity.AccID + "' order by OrderNo desc").Tables[0];
                                    if (dt1.Rows.Count > 0)
                                        FeeNo = "C" + (long.Parse(dt1.Rows[0]["FeeNo"].ToString().Replace("C", "")) + 1).ToString();
                                    else
                                        FeeNo = "C" + today + "0001";

                                    Business.Order.BusinessWorkOrderFee Fee = new Business.Order.BusinessWorkOrderFee();
                                    Fee.Entity.AccID = user.Entity.AccID;
                                    Fee.Entity.OrderNo = list.orderNo;
                                    Fee.Entity.FeeNo = FeeNo;
                                    Fee.Entity.FeeDate = GetDate();
                                    Fee.Entity.FeeAmount = decimal.Parse(list.materialFee) + decimal.Parse(list.serviceFee);
                                    Fee.Entity.Status = "OPEN";
                                    Fee.Entity.CreateDate = GetDate();
                                    Fee.Entity.CreateUser = user.Entity.UserNo;
                                    Fee.Save();

                                    Business.Order.BusinessWorkOrderFeeDetail detail = new Business.Order.BusinessWorkOrderFeeDetail();
                                    detail.Entity.AccID = user.Entity.AccID;
                                    detail.Entity.FeeNo = FeeNo;
                                    detail.Entity.OrderNo = list.orderNo;
                                    detail.Entity.FeeType = "MaterialFee";
                                    detail.Entity.Context = list.context;
                                    detail.Entity.FeeDate = GetDate();
                                    detail.Entity.FeeAmount = decimal.Parse(list.materialFee);
                                    detail.Entity.UserNo = user.Entity.UserNo;
                                    detail.Entity.CreateDate = GetDate();
                                    detail.Entity.CreateUser = user.Entity.UserNo;
                                    detail.Entity.UpdateDate = GetDate();
                                    detail.Entity.UpdateUser = user.Entity.UserNo;
                                    detail.Save();

                                    Business.Order.BusinessWorkOrderFeeDetail detail1 = new Business.Order.BusinessWorkOrderFeeDetail();
                                    detail1.Entity.AccID = user.Entity.AccID;
                                    detail1.Entity.FeeNo = FeeNo;
                                    detail1.Entity.OrderNo = list.orderNo;
                                    detail1.Entity.FeeType = "ServiceFee";
                                    detail1.Entity.Context = list.context;
                                    detail1.Entity.FeeDate = GetDate();
                                    detail1.Entity.FeeAmount = decimal.Parse(list.serviceFee);
                                    detail1.Entity.UserNo = user.Entity.UserNo;
                                    detail1.Entity.CreateDate = GetDate();
                                    detail1.Entity.CreateUser = user.Entity.UserNo;
                                    detail1.Entity.UpdateDate = GetDate();
                                    detail1.Entity.UpdateUser = user.Entity.UserNo;
                                    detail1.Save();

                                    Business.Order.BusinessWorkOrder bw = new Business.Order.BusinessWorkOrder();
                                    bw.loadOrderNo(list.orderNo, "A");

                                    if (bw.Entity.SaleNo != "")
                                    {
                                        ButlerService.AppService service = new ButlerService.AppService();
                                        string InfoMsg = service.SetCompRepairFee(bw.Entity.SaleNo, decimal.Parse(list.materialFee),
                                            decimal.Parse(list.serviceFee), "5218E3ED752A49D4");
                                        if (InfoMsg != "")
                                        {
                                            obj.ExecuteNonQuery("delete from WO_WorkOrder_Fee where FeeNo='" + FeeNo + "' and AccID='" + user.Entity.AccID + "'");
                                            obj.ExecuteNonQuery("delete from WO_WorkOrder_Fee_Detail where FeeNo='" + FeeNo + "' and AccID='" + user.Entity.AccID + "'");

                                            flag = 3;
                                            info = InfoMsg;
                                        }
                                        else
                                        {
                                            flag = 0;
                                            info = "";
                                        }
                                    }
                                    else
                                    {
                                        flag = 3;
                                        info = "当前工单不允许线上收款！";
                                    }
                                }
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                        }
                        #endregion
                        #region [setFee 设置费用金额 - 线下]
                        else if (method == "setFee_OffLine")
                        {
                            try
                            {
                                setFee list = JSON.setFee(str)[0];

                                string FeeNo = "";
                                Business.Order.BusinessWorkOrderFeeDetail cost = new Business.Order.BusinessWorkOrderFeeDetail();
                                int row = cost.GetWorkOrderFeeDetailCount(user.Entity.AccID, string.Empty, list.orderNo, string.Empty, string.Empty, default(DateTime), default(DateTime));
                                if (row > 0)
                                {
                                    flag = 4;
                                }
                                else
                                {
                                    string today = GetDate().ToString("yyMMdd");
                                    DataTable dt1 = obj.ExecuteDataSet("select top 1 FeeNo from WO_WorkOrder_Fee where FeeNo like 'C" + today + "%' and AccID='" + user.Entity.AccID + "' order by OrderNo desc").Tables[0];
                                    if (dt1.Rows.Count > 0)
                                        FeeNo = "C" + (long.Parse(dt1.Rows[0]["FeeNo"].ToString().Replace("C", "")) + 1).ToString();
                                    else
                                        FeeNo = "C" + today + "0001";

                                    Business.Order.BusinessWorkOrderFee Fee = new Business.Order.BusinessWorkOrderFee();
                                    Fee.Entity.AccID = user.Entity.AccID;
                                    Fee.Entity.OrderNo = list.orderNo;
                                    Fee.Entity.FeeNo = FeeNo;
                                    Fee.Entity.FeeDate = GetDate();
                                    Fee.Entity.FeeAmount = decimal.Parse(list.materialFee) + decimal.Parse(list.serviceFee);
                                    Fee.Entity.Status = "OPEN";
                                    Fee.Entity.CreateDate = GetDate();
                                    Fee.Entity.CreateUser = user.Entity.UserNo;
                                    Fee.Save();

                                    Business.Order.BusinessWorkOrderFeeDetail detail = new Business.Order.BusinessWorkOrderFeeDetail();
                                    detail.Entity.AccID = user.Entity.AccID;
                                    detail.Entity.FeeNo = FeeNo;
                                    detail.Entity.OrderNo = list.orderNo;
                                    detail.Entity.FeeType = "MaterialFee";
                                    detail.Entity.Context = list.context;
                                    detail.Entity.FeeDate = GetDate();
                                    detail.Entity.FeeAmount = decimal.Parse(list.materialFee);
                                    detail.Entity.UserNo = user.Entity.UserNo;
                                    detail.Entity.CreateDate = GetDate();
                                    detail.Entity.CreateUser = user.Entity.UserNo;
                                    detail.Entity.UpdateDate = GetDate();
                                    detail.Entity.UpdateUser = user.Entity.UserNo;
                                    detail.Save();

                                    Business.Order.BusinessWorkOrderFeeDetail detail1 = new Business.Order.BusinessWorkOrderFeeDetail();
                                    detail1.Entity.AccID = user.Entity.AccID;
                                    detail1.Entity.FeeNo = FeeNo;
                                    detail1.Entity.OrderNo = list.orderNo;
                                    detail1.Entity.FeeType = "ServiceFee";
                                    detail1.Entity.Context = list.context;
                                    detail1.Entity.FeeDate = GetDate();
                                    detail1.Entity.FeeAmount = decimal.Parse(list.serviceFee);
                                    detail1.Entity.UserNo = user.Entity.UserNo;
                                    detail1.Entity.CreateDate = GetDate();
                                    detail1.Entity.CreateUser = user.Entity.UserNo;
                                    detail1.Entity.UpdateDate = GetDate();
                                    detail1.Entity.UpdateUser = user.Entity.UserNo;
                                    detail1.Save();

                                    Business.Order.BusinessWorkOrder bw = new Business.Order.BusinessWorkOrder();
                                    bw.loadOrderNo(list.orderNo, "A");

                                    string DONo = "";
                                    OrderService.Service service = new OrderService.Service();
                                    service.Url = dopath;
                                    string InfoMsg = service.GenOrderFromCompRepairForWO(bw.Entity.CustNo,
                                        decimal.Parse(list.materialFee) + decimal.Parse(list.serviceFee), "线下付款", "6E5F0C851FD4", out DONo);
                                    if (InfoMsg != "")
                                    {
                                        obj.ExecuteNonQuery("delete from WO_WorkOrder_Fee where FeeNo='" + FeeNo + "' and AccID='" + user.Entity.AccID + "'");
                                        obj.ExecuteNonQuery("delete from WO_WorkOrder_Fee_Detail where FeeNo='" + FeeNo + "' and AccID='" + user.Entity.AccID + "'");

                                        flag = 3;
                                        info = InfoMsg;
                                    }
                                    else
                                    {
                                        obj.ExecuteNonQuery("update WO_WorkOrder set DONo = '" + DONo + "' where OrderNo='" + list.orderNo + "'");
                                        flag = 0;
                                        info = "";
                                    }
                                }
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                        }
                        #endregion


                        #region [getMeterInfo 获取表记资料]
                        else if (method == "getMeterInfo")
                        {
                            try
                            {
                                getMeterInfo list = JSON.getMeterInfo(str)[0];

                                string meterRMID = "";
                                decimal readout = 0;
                                int digit = 0;
                                OrderService.Service service = new OrderService.Service();
                                //service.Url = dopath;
                                string InfoMsg = service.GetMeterInfo(list.meterNo, "6E5F0C851FD4", out meterRMID, out readout, out digit);
                                if (InfoMsg != "")
                                {
                                    flag = 4;
                                    info = InfoMsg;
                                }
                                else
                                {
                                    flag = 0;
                                    info = "";
                                    collection.Add(new JsonStringValue("meterRMID", meterRMID));
                                    collection.Add(new JsonStringValue("readout", readout.ToString("0.##")));
                                    collection.Add(new JsonNumericValue("digit", digit));
                                }
                            }
                            catch
                            {
                                flag = 3;
                                info = "系统操作异常！";
                            }
                        }
                        #endregion
                        #region [meterReadout 抄表]
                        else if (method == "meterReadout")
                        {
                            try
                            {
                                if (Request.Files.Count > 0)
                                {
                                    //文件类型判断
                                    string mime = Request.Files[0].ContentType.ToLower();
                                    if (mime.Contains("image"))
                                    {
                                        Stream imgStream = Request.Files[0].InputStream;
                                        byte[] imgByte = new byte[imgStream.Length];
                                        imgStream.Read(imgByte, 0, (int)imgStream.Length);
                                        imgStream.Close();
                                        //表记数据处理
                                        //meterReadout list = JSON.meterReadout(str)[0];
                                        //OrderService.Service service = new OrderService.Service();
                                        //string InfoMsg = service.MeterReadout(list.meterNo, list.readoutType, list.lastReadout, list.readout, list.joinReadings, list.readings, user.Entity.UserName, "6E5F0C851FD4", imgByte);

                                        string meterNo = Request.Params["meterNo"].ToString();
                                        string readoutType = Request.Params["readoutType"].ToString();
                                        decimal lastReadout = ParseDecimalForString(Request.Params["lastReadout"].ToString());
                                        decimal readout = ParseDecimalForString(Request.Params["readout"].ToString());
                                        decimal joinReadings = ParseDecimalForString(Request.Params["joinReadings"].ToString());
                                        decimal readings = ParseDecimalForString(Request.Params["readings"].ToString());
                                        OrderService.Service service = new OrderService.Service();
                                        string InfoMsg = service.MeterReadout(meterNo,
                                            readoutType,
                                            lastReadout,
                                            readout,
                                            joinReadings,
                                            readings,
                                            user.Entity.UserName,
                                            "6E5F0C851FD4",
                                            imgByte);
                                        if (InfoMsg != "")
                                        {
                                            flag = 4;
                                            info = InfoMsg;
                                        }
                                        else
                                        {
                                            flag = 0;
                                            info = "";
                                        }
                                    }
                                    else
                                    {
                                        info = "图片格式有误";
                                    }


                                }
                                else
                                {
                                    meterReadout list = JSON.meterReadout(str)[0];
                                    OrderService.Service service = new OrderService.Service();
                                    //service.Url = dopath;
                                    string InfoMsg = service.MeterReadout(list.meterNo, list.readoutType, list.lastReadout, list.readout, list.joinReadings, list.readings, user.Entity.UserName, "6E5F0C851FD4", null);
                                    if (InfoMsg != "")
                                    {
                                        flag = 4;
                                        info = InfoMsg;
                                    }
                                    else
                                    {
                                        flag = 0;
                                        info = "抄表成功！";
                                    }
                                }
                            }
                            catch
                            {
                                flag = 3;
                                info = "抄表数据异常！";
                            }
                        }
                        #endregion
                    }
                    catch
                    {
                        flag = 1;
                        info = "参数有误！";
                    }
                }
            }
            catch
            {
                flag = 1;
                info = "参数有误！";
            }

            collection.Add(new JsonNumericValue("retCode", flag));
            collection.Add(new JsonStringValue("retInfo", info));

            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Write(collection.ToString());
        }
    }
}