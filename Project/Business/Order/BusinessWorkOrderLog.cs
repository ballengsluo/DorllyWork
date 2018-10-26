using System;
using System.Data;
namespace project.Business.Order
{
    /// <summary>
    /// 工单日志记录的业务类
    /// </summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    public sealed class BusinessWorkOrderLog : project.Business.AbstractPmBusiness
    {
        private project.Entity.Order.EntityWorkOrderLog _entity = new project.Entity.Order.EntityWorkOrderLog() ;
        public string orderstr = "LogDate desc";
        Data objdata = new Data();

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public BusinessWorkOrderLog() {}

        /// <summary>
        /// 带参数的构函数
        /// </summary>
        /// <param name="entity">实体类</param>
        public BusinessWorkOrderLog(project.Entity.Order.EntityWorkOrderLog entity)
        {
            this._entity = entity;
        }

        /// <summary>
        /// 与实体类(EntityWorkOrderLog)关联
        /// </summary>
        public project.Entity.Order.EntityWorkOrderLog Entity
        {
            get { return _entity as project.Entity.Order.EntityWorkOrderLog; }
        }

        /// </summary>
        ///load 方法 pid主键
        /// </summary>
        public void load(string id,string accID)
        {
            DataRow dr = objdata.ExecuteDataSet("select a.*,b.CustNo,c.CustName from WO_WorkOrder_Log a left join WO_WorkOrder b on a.OrderNo=b.OrderNo left join Base_Cust_Info c on b.CustNo=c.CustNo where a.RowPointer='" + id + "' and a.AccID='" + accID + "'").Tables[0].Rows[0];
            _entity.InnerEntityOID = dr["RowPointer"].ToString();
            _entity.AccID = dr["AccID"].ToString();
            _entity.OrderNo=dr["OrderNo"].ToString();
            _entity.LogDate=ParseDateTimeForString(dr["LogDate"].ToString());
            _entity.GPS_X = dr["GPS_X"].ToString();
            _entity.GPS_Y = dr["GPS_Y"].ToString();
            _entity.LogType = dr["LogType"].ToString();
            _entity.LogUser = dr["LogUser"].ToString();
            _entity.CustNo = dr["CustNo"].ToString();
            _entity.CustName = dr["CustName"].ToString();
            _entity.Remark=dr["Remark"].ToString();
        }

        /// </summary>
        ///Save方法
        /// </summary>
        public int Save()
        {
            string sqlstr = "";
            if (Entity.InnerEntityOID == null)
                sqlstr = "insert into WO_WorkOrder_Log(RowPointer,AccID,OrderNo,LogDate,GPS_X,GPS_Y,LogType,LogUser,Remark)" +
                    "values(NewID()," + "'" + Entity.AccID + "'" + "," + "'" + Entity.OrderNo + "'" + "," +
                    "'" + Entity.LogDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + Entity.GPS_X + "'" + "," +
                    "'" + Entity.GPS_Y + "'" + "," + "'" + Entity.LogType + "'" + "," + "'" + Entity.LogUser + "'" + "," +
                    "'" + Entity.Remark + "'" +
                    ")";
            else
                sqlstr = "update WO_WorkOrder_Log" +
                    " set OrderNo=" + "'" + Entity.OrderNo + "'" + "," + "LogDate=" + "'" + Entity.LogDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," +
                    "GPS_X=" + "'" + Entity.GPS_X + "'" + "," + "GPS_Y=" + "'" + Entity.GPS_Y + "'" + "," +
                    "LogType=" + "'" + Entity.LogType + "'" + "," + "LogUser=" + "'" + Entity.LogUser + "'" + "," +
                    "Remark=" + "'" + Entity.Remark + "'" +
                    " where RowPointer='" + Entity.InnerEntityOID + "'";
            return objdata.ExecuteNonQuery(sqlstr);
        }

        /// </summary>
        ///Delete 方法 
        /// </summary>
        public int delete()
        {
            return objdata.ExecuteNonQuery("delete from WO_WorkOrder_Log where RowPointer='" + Entity.InnerEntityOID + "' and AccID=" + "'" + Entity.AccID + "'");
        }

        /// <summary>
        /// 按条件查询，支持分页
        /// </summary>
        /// <param name="AccID">账套</param>
        /// <param name="OrderNo">订单号</param>
        /// <param name="LogUser">日志用户</param>
        /// <param name="CustNo">客户编号</param>
        /// <param name="LogType">日志类型</param>
        /// <param name="LogDate">日志日期</param>
        /// <returns></returns>
        public System.Collections.ICollection GetWorkOrderLogQuery(String AccID, String OrderNo, String LogUser, String CustNo, String LogType, DateTime MinLogDate, DateTime MaxLogDate, int startRow, int pageSize)
        {
            if (startRow < 0 || pageSize <= 0)
            {
                throw new Exception();
            }

            return GetListHelper(AccID, OrderNo, LogUser, CustNo, LogType, MinLogDate, MaxLogDate, startRow, pageSize);
        }

        /// <summary>
        /// 按条件查询，不支持分页
        /// </summary>
        /// <param name="AccID">账套</param>
        /// <param name="OrderNo">订单号</param>
        /// <param name="LogUser">日志用户</param>
        /// <param name="CustNo">客户编号</param>
        /// <param name="LogType">日志类型</param>
        /// <param name="LogDate">日志日期</param>
        /// <returns></returns>
        public System.Collections.ICollection GetWorkOrderLogQuery(String AccID, String OrderNo, String LogUser, String CustNo, String LogType, DateTime MinLogDate, DateTime MaxLogDate)
        {
            return GetListHelper(AccID, OrderNo, LogUser, CustNo, LogType, MinLogDate, MaxLogDate, START_ROW_INIT, START_ROW_INIT);
        }

        /// <summary>
        /// 返回集合的大小
        /// </summary>
        /// <param name="AccID">账套</param>
        /// <param name="OrderNo">订单号</param>
        /// <param name="LogUser">日志用户</param>
        /// <param name="CustNo">客户编号</param>
        /// <param name="LogType">日志类型</param>
        /// <param name="LogDate">日志日期</param>
        /// <returns></returns>
        public int GetWorkOrderLogCount(String AccID, String OrderNo, String LogUser, String CustNo, String LogType, DateTime MinLogDate, DateTime MaxLogDate)
        {
            string wherestr="";
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and a.AccID='" + AccID + "'";
            }
            if (OrderNo != string.Empty)
            {
                wherestr = wherestr + " and a.OrderNo like '%" + OrderNo + "%'";
            }
            if (LogUser != string.Empty)
            {
                wherestr = wherestr + " and a.LogUser like '" + LogUser + "'";
            }
            if (CustNo != string.Empty)
            {
                wherestr = wherestr + " and b.CustNo like '" + CustNo + "'";
            }
            if (LogType != string.Empty)
            {
                wherestr = wherestr + " and a.LogType='" + LogType + "'";
            }
            if (MinLogDate != default(DateTime))
            {
                wherestr = wherestr + " and convert(nvarchar(10),a.LogDate,121)>='" + MinLogDate.ToString("yyyy-MM-dd") + "'";
            }
            if (MaxLogDate != default(DateTime))
            {
                wherestr = wherestr + " and convert(nvarchar(10),a.LogDate,121)<='" + MaxLogDate.ToString("yyyy-MM-dd") + "'";
            }

            string count = objdata.ExecuteDataSet("select count(*) as cnt from WO_WorkOrder_Log a left join WO_WorkOrder b on a.OrderNo=b.OrderNo where 1=1 " + wherestr).Tables[0].Rows[0]["cnt"].ToString();
            return int.Parse(count);
        }

        /// <summary>
        /// 按条件查询，返回符合条件的集合
        /// </summary>
        /// <param name="AccID">账套</param>
        /// <param name="OrderNo">订单号</param>
        /// <param name="LogUser">日志用户</param>
        /// <param name="CustNo">客户编号</param>
        /// <param name="LogType">日志类型</param>
        /// <param name="LogDate">日志日期</param>
        /// <returns></returns>
        private System.Collections.ICollection GetListHelper(String AccID, String OrderNo, String LogUser, String CustNo, String LogType, DateTime MinLogDate, DateTime MaxLogDate, int startRow, int pageSize)
        {
            string wherestr = "";
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and a.AccID='" + AccID + "'";
            }
            if (OrderNo != string.Empty)
            {
                wherestr = wherestr + " and a.OrderNo like '%" + OrderNo + "%'";
            }
            if (LogUser != string.Empty)
            {
                wherestr = wherestr + " and a.LogUser like '" + LogUser + "'";
            }
            if (CustNo != string.Empty)
            {
                wherestr = wherestr + " and b.CustNo like '" + CustNo + "'";
            }
            if (LogType != string.Empty)
            {
                wherestr = wherestr + " and a.LogType='" + LogType + "'";
            }
            if (MinLogDate != default(DateTime))
            {
                wherestr = wherestr + " and convert(nvarchar(10),a.LogDate,121)>='" + MinLogDate.ToString("yyyy-MM-dd") + "'";
            }
            if (MaxLogDate != default(DateTime))
            {
                wherestr = wherestr + " and convert(nvarchar(10),a.LogDate,121)<='" + MaxLogDate.ToString("yyyy-MM-dd") + "'";
            }

            System.Collections.IList entitys = null;
            if (startRow > START_ROW_INIT && pageSize > START_ROW_INIT)
            {
                entitys = Query(objdata.ExecSelect("WO_WorkOrder_Log a left join WO_WorkOrder b on a.OrderNo=b.OrderNo left join Base_Cust_Info c on b.CustNo=c.CustNo", "a.*,b.CustNo,c.CustName", wherestr, startRow, pageSize, orderstr));
            }
            else
            {
                entitys = Query(objdata.ExecSelect("WO_WorkOrder_Log a left join WO_WorkOrder b on a.OrderNo=b.OrderNo left join Base_Cust_Info c on b.CustNo=c.CustNo", "a.*,b.CustNo,c.CustName", wherestr, START_ROW_INIT, START_ROW_INIT, orderstr));
            }
            return entitys;
        }
        /// </summary>
        ///Query 方法 dt查询结果
        /// </summary>
        public System.Collections.IList Query(System.Data.DataTable dt)
        {
            System.Collections.IList result = new System.Collections.ArrayList();
            foreach(System.Data.DataRow dr in dt.Rows)
            {
                project.Entity.Order.EntityWorkOrderLog entity = new project.Entity.Order.EntityWorkOrderLog();
                entity.InnerEntityOID = dr["RowPointer"].ToString();
                entity.AccID = dr["AccID"].ToString();
                entity.OrderNo = dr["OrderNo"].ToString();
                entity.LogDate = ParseDateTimeForString(dr["LogDate"].ToString());
                entity.GPS_X = dr["GPS_X"].ToString();
                entity.GPS_Y = dr["GPS_Y"].ToString();
                entity.LogType = dr["LogType"].ToString();
                entity.LogUser = dr["LogUser"].ToString();
                entity.CustNo = dr["CustNo"].ToString();
                entity.CustName = dr["CustName"].ToString();
                entity.Remark = dr["Remark"].ToString();
                result.Add(entity);
            }
            return result;
        }

    }
}
