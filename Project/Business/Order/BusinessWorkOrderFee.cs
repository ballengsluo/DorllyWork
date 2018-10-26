using System;
using System.Data;
namespace project.Business.Order
{
    /// <summary>
    /// 工单收款的业务类
    /// </summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    public sealed class BusinessWorkOrderFee : project.Business.AbstractPmBusiness
    {
        private project.Entity.Order.EntityWorkOrderFee _entity = new project.Entity.Order.EntityWorkOrderFee();
        public string orderstr = "a.FeeDate desc";
        Data objdata = new Data();

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public BusinessWorkOrderFee() { }

        /// <summary>
        /// 带参数的构函数
        /// </summary>
        /// <param name="entity">实体类</param>
        public BusinessWorkOrderFee(project.Entity.Order.EntityWorkOrderFee entity)
        {
            this._entity = entity;
        }

        /// <summary>
        /// 与实体类(EntityWorkOrderFee)关联
        /// </summary>
        public project.Entity.Order.EntityWorkOrderFee Entity
        {
            get { return _entity as project.Entity.Order.EntityWorkOrderFee; }
        }

        /// </summary>
        ///load 方法 pid主键
        /// </summary>
        public void load(string id, string accID)
        {
            DataRow dr = objdata.ExecuteDataSet("select a.*,b.OrderName,b.OrderDate,b.Status as OrderStatus,c.StatusName as OrderStatusName from WO_WorkOrder_Fee a " +
                "left join WO_WorkOrder b on a.OrderNo=b.OrderNo left join Base_Status c on c.StatusNo=b.Status "+
                "where a.RowPointer='" + id + "' and a.AccID='" + accID + "'").Tables[0].Rows[0];
            _entity.InnerEntityOID = dr["RowPointer"].ToString();
            _entity.AccID = dr["AccID"].ToString();
            _entity.FeeNo = dr["FeeNo"].ToString();
            _entity.OrderNo = dr["OrderNo"].ToString();
            _entity.OrderName = dr["OrderName"].ToString();
            _entity.OrderDate = ParseDateTimeForString(dr["OrderDate"].ToString());
            _entity.OrderStatus = dr["OrderStatus"].ToString();
            _entity.OrderStatusName = dr["OrderStatusName"].ToString();
            _entity.FeeDate = ParseDateTimeForString(dr["FeeDate"].ToString());
            _entity.FeeAmount = ParseDecimalForString(dr["FeeAmount"].ToString());
            _entity.Status = dr["Status"].ToString();
            _entity.CreateDate = ParseDateTimeForString(dr["CreateDate"].ToString());
            _entity.CreateUser = dr["CreateUser"].ToString();
        }

        /// </summary>
        ///Save方法
        /// </summary>
        public int Save()
        {
            string sqlstr = "";
            if (Entity.InnerEntityOID == null)
                sqlstr = "insert into WO_WorkOrder_Fee(RowPointer,AccID,FeeNo,OrderNo,FeeDate,FeeAmount,Status,CreateDate,CreateUser)" +
                    "values(NewID()," + "'" + Entity.AccID + "'" + "," + "'" + Entity.FeeNo + "'" + "," + "'" + Entity.OrderNo + "'" + "," +
                    "'" + Entity.FeeDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + Entity.FeeAmount + "'" + "," +
                    "'" + Entity.Status + "'" + "," + "'" + Entity.CreateDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + Entity.CreateUser + "'" +
                    ")";
            else
                sqlstr = "update WO_WorkOrder_Fee" +
                    " set FeeDate=" + "'" + Entity.FeeDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," +
                    "FeeAmount=" + "'" + Entity.FeeAmount + "'" + "," + "Status=" + "'" + Entity.Status + "'" +
                    " where RowPointer='" + Entity.InnerEntityOID + "'";
            return objdata.ExecuteNonQuery(sqlstr);
        }

        /// </summary>
        ///Delete 方法 
        /// </summary>
        public int delete()
        {
            return objdata.ExecuteNonQuery("delete from WO_WorkOrder_Fee where RowPointer='" + Entity.InnerEntityOID + "' and AccID=" + "'" + Entity.AccID + "'");
        }

        /// <summary>
        /// 按条件查询，支持分页
        /// </summary>
        /// <param name="AccID">账套</param>
        /// <param name="FeeNo">收款单号</param>
        /// <param name="OrderNo">工单号</param>
        /// <param name="Status">状态</param>
        /// <param name="OrderDate">工单日期</param>
        /// <param name="Type">类型（1制单/审核 2审核/复核）</param>
        /// <returns></returns>
        public System.Collections.ICollection GetWorkOrderFeeQuery(String AccID, String FeeNo, String OrderNo, String Status, DateTime MinOrderDate, DateTime MaxOrderDate, String Type, int startRow, int pageSize)
        {
            if (startRow < 0 || pageSize <= 0)
            {
                throw new Exception();
            }

            return GetListHelper(AccID, FeeNo, OrderNo, Status, MinOrderDate, MaxOrderDate, Type, startRow, pageSize);
        }

        /// <summary>
        /// 按条件查询，不支持分页
        /// </summary>
        /// <param name="AccID">账套</param>
        /// <param name="FeeNo">收款单号</param>
        /// <param name="OrderNo">工单号</param>
        /// <param name="Status">状态</param>
        /// <param name="OrderDate">工单日期</param>
        /// <param name="Type">类型（1制单/审核 2审核/复核）</param>
        /// <returns></returns>
        public System.Collections.ICollection GetWorkOrderFeeQuery(String AccID, String FeeNo, String OrderNo, String Status, DateTime MinOrderDate, DateTime MaxOrderDate, String Type)
        {
            return GetListHelper(AccID, FeeNo, OrderNo, Status, MinOrderDate, MaxOrderDate, Type, START_ROW_INIT, START_ROW_INIT);
        }

        /// <summary>
        /// 返回集合的大小
        /// </summary>
        /// <param name="AccID">账套</param>
        /// <param name="FeeNo">收款单号</param>
        /// <param name="OrderNo">工单号</param>
        /// <param name="Status">状态</param>
        /// <param name="OrderDate">工单日期</param>
        /// <param name="Type">类型（1制单/审核 2审核/复核）</param>
        /// <returns></returns>
        public int GetWorkOrderFeeCount(String AccID, String FeeNo, String OrderNo, String Status, DateTime MinOrderDate, DateTime MaxOrderDate, String Type)
        {
            string wherestr = "";
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and a.AccID='" + AccID + "'";
            }
            if (FeeNo != string.Empty)
            {
                wherestr = wherestr + " and a.FeeNo like '%" + FeeNo + "%'";
            }
            if (OrderNo != string.Empty)
            {
                wherestr = wherestr + " and a.OrderNo like '%" + OrderNo + "%'";
            }
            if (Status != string.Empty)
            {
                wherestr = wherestr + " and a.Status like '" + Status + "'";
            }
            if (Type != string.Empty)
            {
                if (Type == "1")
                    wherestr = wherestr + " and a.Status in('OPEN','APPROVE')";
                else
                    wherestr = wherestr + " and a.Status in('CONFIRM','APPROVE')";
            }
            if (MinOrderDate != default(DateTime))
            {
                wherestr = wherestr + " and convert(nvarchar(10),b.OrderDate,121)>='" + MinOrderDate.ToString("yyyy-MM-dd") + "'";
            }
            if (MaxOrderDate != default(DateTime))
            {
                wherestr = wherestr + " and convert(nvarchar(10),b.OrderDate,121)<='" + MaxOrderDate.ToString("yyyy-MM-dd") + "'";
            }
            wherestr = wherestr + " and b.Status like 'CLOSE'";

            string count = objdata.ExecuteDataSet("select count(*) as cnt from WO_WorkOrder_Fee a left join WO_WorkOrder b on a.OrderNo=b.OrderNo where 1=1 " + wherestr).Tables[0].Rows[0]["cnt"].ToString();
            return int.Parse(count);
        }

        /// <summary>
        /// 按条件查询，返回符合条件的集合
        /// </summary>
        /// <param name="AccID">账套</param>
        /// <param name="FeeNo">收款单号</param>
        /// <param name="OrderNo">工单号</param>
        /// <param name="Status">状态</param>
        /// <param name="OrderDate">工单日期</param>
        /// <param name="Type">类型（1制单/审核 2审核/复核）</param>
        /// <returns></returns>
        private System.Collections.ICollection GetListHelper(String AccID, String FeeNo, String OrderNo, String Status, DateTime MinOrderDate, DateTime MaxOrderDate, String Type, int startRow, int pageSize)
        {
            string wherestr = "";
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and a.AccID='" + AccID + "'";
            }
            if (FeeNo != string.Empty)
            {
                wherestr = wherestr + " and a.FeeNo like '%" + FeeNo + "%'";
            }
            if (OrderNo != string.Empty)
            {
                wherestr = wherestr + " and a.OrderNo like '%" + OrderNo + "%'";
            }
            if (Status != string.Empty)
            {
                wherestr = wherestr + " and a.Status like '" + Status + "'";
            }
            if (Type != string.Empty)
            {
                if (Type == "1")
                    wherestr = wherestr + " and a.Status in('OPEN','APPROVE')";
                else
                    wherestr = wherestr + " and a.Status in('CONFIRM','APPROVE')";
            }
            if (MinOrderDate != default(DateTime))
            {
                wherestr = wherestr + " and convert(nvarchar(10),b.OrderDate,121)>='" + MinOrderDate.ToString("yyyy-MM-dd") + "'";
            }
            if (MaxOrderDate != default(DateTime))
            {
                wherestr = wherestr + " and convert(nvarchar(10),b.OrderDate,121)<='" + MaxOrderDate.ToString("yyyy-MM-dd") + "'";
            }
            wherestr = wherestr + " and b.Status like 'CLOSE'";

            System.Collections.IList entitys = null;
            if (startRow > START_ROW_INIT && pageSize > START_ROW_INIT)
            {
                entitys = Query(objdata.ExecSelect("WO_WorkOrder_Fee a left join WO_WorkOrder b on a.OrderNo=b.OrderNo left join Base_Status c on c.StatusNo=b.Status",
                    "a.*,b.OrderName,b.OrderDate,b.Status as OrderStatus,c.StatusName as OrderStatusName", wherestr, startRow, pageSize, orderstr));
            }
            else
            {
                entitys = Query(objdata.ExecSelect("WO_WorkOrder_Fee a left join WO_WorkOrder b on a.OrderNo=b.OrderNo left join Base_Status c on c.StatusNo=b.Status",
                    "a.*,b.OrderName,b.OrderDate,b.Status as OrderStatus,c.StatusName as OrderStatusName", wherestr, START_ROW_INIT, START_ROW_INIT, orderstr));
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
                project.Entity.Order.EntityWorkOrderFee entity = new project.Entity.Order.EntityWorkOrderFee();
                entity.InnerEntityOID = dr["RowPointer"].ToString();
                entity.AccID = dr["AccID"].ToString();
                entity.FeeNo = dr["FeeNo"].ToString();
                entity.OrderNo = dr["OrderNo"].ToString();
                entity.OrderName = dr["OrderName"].ToString();
                entity.OrderDate = ParseDateTimeForString(dr["OrderDate"].ToString());
                entity.OrderStatus = dr["OrderStatus"].ToString();
                entity.OrderStatusName = dr["OrderStatusName"].ToString();
                entity.FeeDate = ParseDateTimeForString(dr["FeeDate"].ToString());
                entity.FeeAmount = ParseDecimalForString(dr["FeeAmount"].ToString());
                entity.Status = dr["Status"].ToString();
                entity.CreateDate = ParseDateTimeForString(dr["CreateDate"].ToString());
                entity.CreateUser = dr["CreateUser"].ToString();
                result.Add(entity);
            }
            return result;
        }

    }
}
