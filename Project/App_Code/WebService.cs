using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// WebService 的摘要说明
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
// [System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService {

    public WebService () {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string GenOrderFormButler(string serviceNo,string srvName,string linkMan,string linkTel,string addr,string needTime,
        string custNo,string userName,string orderType, string alloUser, string KEY)
    {
        if (KEY != "B2D82F84140D") return "KEY参数有误！";

        string InfoMsg = "";
        try
        {
            string OrderNo = "";
            string today = GetDate().ToString("yyMMdd");
            Data obj = new Data();
            DataTable dt = obj.ExecuteDataSet("select top 1 OrderNo from WO_WorkOrder where OrderNo like N'" + today + "%' and AccID='A' order by OrderNo desc").Tables[0];
            if (dt.Rows.Count > 0)
                OrderNo = (long.Parse(dt.Rows[0]["OrderNo"].ToString()) + 1).ToString();
            else
                OrderNo = today + "0001";

            string alloDept = "";
            try
            {
                if (alloUser != "")
                {
                    project.Business.Sys.BusinessUserInfo user = new project.Business.Sys.BusinessUserInfo();
                    user.loadUserNo(alloUser,"A");
                    alloDept = user.Entity.DeptNo;
                }
            }
            catch {
                alloDept = "";
                alloUser = "";
            }

            project.Business.Order.BusinessWorkOrder bc = new project.Business.Order.BusinessWorkOrder();
            bc.Entity.OrderNo = OrderNo;
            bc.Entity.OrderName = srvName;
            bc.Entity.OrderDate = GetDate();
            bc.Entity.OrderType = orderType;
            bc.Entity.SaleNo = serviceNo;
            bc.Entity.AlloDept = alloDept;
            bc.Entity.AlloUser = alloUser;
            bc.Entity.CustNo = custNo;
            bc.Entity.LinkMan = linkMan;
            bc.Entity.LinkTel = linkTel;
            bc.Entity.Addr = addr;
            bc.Entity.Region = "";
            bc.Entity.CustneedTime = ParseDateForString(needTime);

            bc.Entity.AccID = "A";
            bc.Entity.Status = "OPEN";
            bc.Entity.CreateTime = GetDate();
            bc.Entity.CreateUser = "izgj";
            bc.Entity.UpdateDate = GetDate();
            bc.Entity.UpdateUser = userName;

            int r = bc.Save();
            if (r > 0) {
                if (alloUser != "")
                {
                    project.Business.Order.BusinessWorkOrderPerson person = new project.Business.Order.BusinessWorkOrderPerson();
                    person.Entity.AccID = "A";
                    person.Entity.CreateDate = GetDate();
                    person.Entity.CreateUser = "izgj";
                    person.Entity.IsBack = false;
                    person.Entity.IsDel = false;
                    person.Entity.OrderNo = bc.Entity.OrderNo;
                    person.Entity.UserNo = alloUser;
                    person.Entity.UpdateUser = userName;
                    person.Entity.UpdateDate = GetDate();
                    int row = person.Save();

                    if (row > 0)
                    {
                        project.Business.Order.BusinessWorkOrderMsg msg = new project.Business.Order.BusinessWorkOrderMsg();
                        msg.Entity.AccID = "A";
                        msg.Entity.Sender = alloUser;
                        msg.Entity.SendDate = GetDate();
                        msg.Entity.ToUser = person.Entity.UserNo;
                        msg.Entity.Subject = "您有一张新的工单！";
                        msg.Entity.Context = "你有新的工单需要处理，工单号：" + bc.Entity.OrderNo;
                        msg.Entity.RefNo = bc.Entity.OrderNo;
                        msg.Entity.MsgType = "1";
                        msg.Entity.IsDel = false;
                        msg.Entity.IsRead = false;
                        msg.Entity.CreateDate = GetDate();
                        msg.Entity.CreateUser = "izgj";
                        msg.Save();
                    }
                }
            }
        }
        catch(Exception ex) {
            InfoMsg = ex.Message;
        }

        return InfoMsg;
    }


    /// <summary>
    /// 来自订单系统
    /// </summary>
    /// <param name="CustNo"></param>
    /// <param name="CustName"></param>
    /// <param name="CustShortName"></param>
    /// <param name="CustType"></param>
    /// <param name="Representative"></param>
    /// <param name="BusinessScope"></param>
    /// <param name="CustLicenseNo"></param>
    /// <param name="RepIDCard"></param>
    /// <param name="CustContact"></param>
    /// <param name="CustTel"></param>
    /// <param name="CustContactMobile"></param>
    /// <param name="CustEmail"></param>
    /// <param name="CustBankTitle"></param>
    /// <param name="CustBankAccount"></param>
    /// <param name="CustBank"></param>
    /// <param name="IsExternal"></param>
    /// <param name="Type"></param>
    /// <param name="Key"></param>
    /// <returns></returns>
    [WebMethod]
    public string SetCustomer(string CustNo, string CustName, string CustShortName, string CustType, string Representative,
        string BusinessScope, string CustLicenseNo, string RepIDCard, string CustContact, string CustTel, string CustContactMobile,
        string CustEmail, string CustBankTitle, string CustBankAccount, string CustBank, bool IsExternal, string UserName,
        string Type, string Key)
    {
        if (Key != "5218E3ED752A49D4") return "";
        string InfoMsg = "";
        try
        {
            if (Type == "insert")
            {
                project.Business.Base.BusinessCustInfo bc = new project.Business.Base.BusinessCustInfo();
                bc.Entity.AccID = "A";
                bc.Entity.CustNo = CustNo;
                bc.Entity.CustName = CustName;
                bc.Entity.CustType = CustType;
                bc.Entity.Contact = CustContact;
                bc.Entity.Tel = CustTel;

                bc.Entity.RegDate = GetDate();
                bc.Entity.Valid = true;
                bc.Save();
            }
            else
            {
                project.Business.Base.BusinessCustInfo bc = new project.Business.Base.BusinessCustInfo();
                bc.loadCustNo(CustNo,"A");
                bc.Entity.CustName = CustName;
                bc.Entity.CustType = CustType;
                bc.Entity.Contact = CustContact;
                bc.Entity.Tel = CustTel;

                bc.Entity.RegDate = GetDate();
                bc.Entity.Valid = true;
                bc.Save();
            }
        }
        catch (Exception ex)
        {
            InfoMsg = ex.Message;
        }
        return InfoMsg;
    }


    private System.DateTime ParseDateForString(string val)
    {
        if (string.IsNullOrEmpty(val))
        {
            return DateTime.MinValue.AddYears(1900);
        }

        return DateTime.Parse(val);
    }
    private DateTime GetDate()
    {
        Data obj = new Data();
        return DateTime.Parse(obj.ExecuteDataSet("select DT=getdate()").Tables[0].Rows[0]["DT"].ToString());
    }
}
