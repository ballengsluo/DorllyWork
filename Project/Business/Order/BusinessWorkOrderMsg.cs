using System;
using System.Data;
namespace project.Business.Order
{
    /// <summary>
    /// 工单日志记录的业务类
    /// </summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    public sealed class BusinessWorkOrderMsg : project.Business.AbstractPmBusiness
    {
        private project.Entity.Order.EntityWorkOrderMsg _entity = new project.Entity.Order.EntityWorkOrderMsg();
        public string orderstr = "CreateDate desc";
        Data objdata = new Data();

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public BusinessWorkOrderMsg() { }

        /// <summary>
        /// 带参数的构函数
        /// </summary>
        /// <param name="entity">实体类</param>
        public BusinessWorkOrderMsg(project.Entity.Order.EntityWorkOrderMsg entity)
        {
            this._entity = entity;
        }

        /// <summary>
        /// 与实体类(EntityWorkOrderMsg)关联
        /// </summary>
        public project.Entity.Order.EntityWorkOrderMsg Entity
        {
            get { return _entity as project.Entity.Order.EntityWorkOrderMsg; }
        }

        /// </summary>
        ///load 方法 pid主键
        /// </summary>
        public void load(string id, string accID)
        {
            DataRow dr = objdata.ExecuteDataSet("select * from WO_WorkOrder_Msg where RowPointer='" + id + "' and AccID='" + accID + "'").Tables[0].Rows[0];
            _entity.InnerEntityOID = dr["RowPointer"].ToString();
            _entity.AccID = dr["AccID"].ToString();
            _entity.MsgType = dr["MsgType"].ToString();
            _entity.Sender = dr["Sender"].ToString();
            _entity.SendDate = ParseDateTimeForString(dr["SendDate"].ToString());
            _entity.Subject = dr["Subject"].ToString();
            _entity.Context = dr["Context"].ToString();
            _entity.ToUser = dr["ToUser"].ToString();
            _entity.IsRead = bool.Parse(dr["IsRead"].ToString());
            _entity.ReadDate = ParseDateTimeForString(dr["ReadDate"].ToString());
            _entity.RefNo = dr["RefNo"].ToString();
            _entity.CreateDate = ParseDateTimeForString(dr["CreateDate"].ToString());
            _entity.CreateUser = dr["CreateUser"].ToString();
            _entity.IsDel = bool.Parse(dr["IsDel"].ToString());
        }

        /// </summary>
        ///Save方法
        /// </summary>
        public int Save()
        {
            string sqlstr = "";
            if (Entity.InnerEntityOID == null)
                sqlstr = "insert into WO_WorkOrder_Msg(RowPointer,AccID,MsgType,Sender,SendDate,Subject,Context,ToUser,IsRead,ReadDate,RefNo,CreateDate,CreateUser,IsDel)" +
                    "values(NewID()," + "'" + Entity.AccID + "'" + "," + "'" + Entity.MsgType + "'" + "," + "'" + Entity.Sender + "'" + "," +
                    "'" + Entity.SendDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + Entity.Subject + "'" + "," +
                    "'" + Entity.Context + "'" + "," + "'" + Entity.ToUser + "'" + ",0,null," +
                    "'" + Entity.RefNo + "'" + "," + "'" + Entity.CreateDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + Entity.CreateUser + "'" + ",0" +
                    ")";
            else
                sqlstr = "update WO_WorkOrder_Msg" +
                    " set Subject=" + "'" + Entity.Subject + "'" + "," +
                    "Context=" + "'" + Entity.Context + "'" + "," +
                    "IsRead=" + (Entity.IsRead == true ? "1" : "0") + "," + 
                    "ReadDate=" + "'" + Entity.ReadDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," +
                    "RefNo=" + "'" + Entity.RefNo + "'" + 
                    " where RowPointer='" + Entity.InnerEntityOID + "'";
            return objdata.ExecuteNonQuery(sqlstr);
        }

        /// </summary>
        ///Delete 方法 
        /// </summary>
        public int delete()
        {
            return objdata.ExecuteNonQuery("update WO_WorkOrder_Msg set IsDel=1 where RowPointer='" + Entity.InnerEntityOID + "' and AccID=" + "'" + Entity.AccID + "'");
        }

        /// <summary>
        /// 按条件查询，支持分页
        /// </summary>
        /// <param name="AccID">账套</param>
        /// <param name="Sender">发送人</param>
        /// <param name="SendDate">发送日期</param>
        /// <param name="ToUser">接收人</param>
        /// <param name="IsRead">是否已读</param>
        /// <param name="RefNo">相关单号</param>
        /// <param name="IsDel">是否删除</param>
        /// <returns></returns>
        public System.Collections.ICollection GetWorkOrderMsgQuery(String AccID, String Sender, DateTime MinSendDate, DateTime MaxSendDate, String ToUser, bool? IsRead, String RefNo, bool? IsDel, int startRow, int pageSize)
        {
            if (startRow < 0 || pageSize <= 0)
            {
                throw new Exception();
            }

            return GetListHelper(AccID, Sender, MinSendDate, MaxSendDate, ToUser, IsRead, RefNo, IsDel, startRow, pageSize);
        }

        /// <summary>
        /// 按条件查询，不支持分页
        /// </summary>
        /// <param name="AccID">账套</param>
        /// <param name="Sender">发送人</param>
        /// <param name="SendDate">发送日期</param>
        /// <param name="ToUser">接收人</param>
        /// <param name="IsRead">是否已读</param>
        /// <param name="RefNo">相关单号</param>
        /// <param name="IsDel">是否删除</param>
        /// <returns></returns>
        public System.Collections.ICollection GetWorkOrderMsgQuery(String AccID, String Sender, DateTime MinSendDate, DateTime MaxSendDate, String ToUser, bool? IsRead, String RefNo, bool? IsDel)
        {
            return GetListHelper(AccID, Sender, MinSendDate, MaxSendDate, ToUser, IsRead, RefNo, IsDel, START_ROW_INIT, START_ROW_INIT);
        }

        /// <summary>
        /// 返回集合的大小
        /// </summary>
        /// <param name="AccID">账套</param>
        /// <param name="Sender">发送人</param>
        /// <param name="SendDate">发送日期</param>
        /// <param name="ToUser">接收人</param>
        /// <param name="IsRead">是否已读</param>
        /// <param name="RefNo">相关单号</param>
        /// <param name="IsDel">是否删除</param>
        /// <returns></returns>
        public int GetWorkOrderMsgCount(String AccID, String Sender, DateTime MinSendDate, DateTime MaxSendDate, String ToUser, bool? IsRead, String RefNo, bool? IsDel)
        {
            string wherestr = "";
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and AccID='" + AccID + "'";
            }
            if (Sender != string.Empty)
            {
                wherestr = wherestr + " and Sender like '" + Sender + "'";
            }
            if (MinSendDate != default(DateTime))
            {
                wherestr = wherestr + " and convert(nvarchar(10),SendDate,121)>='" + MinSendDate.ToString("yyyy-MM-dd") + "'";
            }
            if (MaxSendDate != default(DateTime))
            {
                wherestr = wherestr + " and convert(nvarchar(10),SendDate,121)<='" + MaxSendDate.ToString("yyyy-MM-dd") + "'";
            }
            if (ToUser != string.Empty)
            {
                wherestr = wherestr + " and ToUser like '" + ToUser + "'";
            }
            if (IsRead != null)
            {
                wherestr = wherestr + " and IsRead like '" + (IsRead==true?"1":"0") + "'";
            }
            if (RefNo != string.Empty)
            {
                wherestr = wherestr + " and RefNo like '%" + RefNo + "%'";
            }
            if (IsDel != null)
            {
                wherestr = wherestr + " and IsDel like '" + (IsDel == true ? "1" : "0") + "'";
            }

            string count = objdata.ExecuteDataSet("select count(*) as cnt from WO_WorkOrder_Msg where 1=1 " + wherestr).Tables[0].Rows[0]["cnt"].ToString();
            return int.Parse(count);
        }

        /// <summary>
        /// 按条件查询，返回符合条件的集合
        /// </summary>
        /// <param name="AccID">账套</param>
        /// <param name="Sender">发送人</param>
        /// <param name="SendDate">发送日期</param>
        /// <param name="ToUser">接收人</param>
        /// <param name="IsRead">是否已读</param>
        /// <param name="RefNo">相关单号</param>
        /// <param name="IsDel">是否删除</param>
        /// <returns></returns>
        private System.Collections.ICollection GetListHelper(String AccID, String Sender, DateTime MinSendDate, DateTime MaxSendDate, String ToUser, bool? IsRead, String RefNo, bool? IsDel, int startRow, int pageSize)
        {
            string wherestr = "";
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and AccID='" + AccID + "'";
            }
            if (Sender != string.Empty)
            {
                wherestr = wherestr + " and Sender like '" + Sender + "'";
            }
            if (MinSendDate != default(DateTime))
            {
                wherestr = wherestr + " and convert(nvarchar(10),SendDate,121)>='" + MinSendDate.ToString("yyyy-MM-dd") + "'";
            }
            if (MaxSendDate != default(DateTime))
            {
                wherestr = wherestr + " and convert(nvarchar(10),SendDate,121)<='" + MaxSendDate.ToString("yyyy-MM-dd") + "'";
            }
            if (ToUser != string.Empty)
            {
                wherestr = wherestr + " and ToUser like '" + ToUser + "'";
            }
            if (IsRead != null)
            {
                wherestr = wherestr + " and IsRead like '" + (IsRead == true ? "1" : "0") + "'";
            }
            if (RefNo != string.Empty)
            {
                wherestr = wherestr + " and RefNo like '%" + RefNo + "%'";
            }
            if (IsDel != null)
            {
                wherestr = wherestr + " and IsDel like '" + (IsDel == true ? "1" : "0") + "'";
            }

            System.Collections.IList entitys = null;
            if (startRow > START_ROW_INIT && pageSize > START_ROW_INIT)
            {
                entitys = Query(objdata.ExecSelect("WO_WorkOrder_Msg ", wherestr, startRow, pageSize, orderstr));
            }
            else
            {
                entitys = Query(objdata.ExecSelect("WO_WorkOrder_Msg ", wherestr, START_ROW_INIT, START_ROW_INIT, orderstr));
            }
            return entitys;
        }
        /// </summary>
        ///Query 方法 dt查询结果
        /// </summary>
        public System.Collections.IList Query(System.Data.DataTable dt)
        {
            System.Collections.IList result = new System.Collections.ArrayList();
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                project.Entity.Order.EntityWorkOrderMsg entity = new project.Entity.Order.EntityWorkOrderMsg();
                entity.InnerEntityOID = dr["RowPointer"].ToString();
                entity.AccID = dr["AccID"].ToString();
                entity.MsgType = dr["MsgType"].ToString();
                entity.Sender = dr["Sender"].ToString();
                entity.SendDate = ParseDateTimeForString(dr["SendDate"].ToString());
                entity.Subject = dr["Subject"].ToString();
                entity.Context = dr["Context"].ToString();
                entity.ToUser = dr["ToUser"].ToString();
                entity.IsRead = bool.Parse(dr["IsRead"].ToString());
                entity.ReadDate = ParseDateTimeForString(dr["ReadDate"].ToString());
                entity.RefNo = dr["RefNo"].ToString();
                entity.CreateDate = ParseDateTimeForString(dr["CreateDate"].ToString());
                entity.CreateUser = dr["CreateUser"].ToString();
                entity.IsDel = bool.Parse(dr["IsDel"].ToString());
                result.Add(entity);
            }
            return result;
        }

    }
}
