using System;
using System.Data;
namespace project.Business.Order
{
    /// <summary>
    /// 工单费用的业务类
    /// </summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    public sealed class BusinessWorkOrderCostDetail : project.Business.AbstractPmBusiness
    {
        private project.Entity.Order.EntityWorkOrderCostDetail _entity = new project.Entity.Order.EntityWorkOrderCostDetail();
        public string orderstr = "CostDate desc";
        Data objdata = new Data();

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public BusinessWorkOrderCostDetail() { }

        /// <summary>
        /// 带参数的构函数
        /// </summary>
        /// <param name="entity">实体类</param>
        public BusinessWorkOrderCostDetail(project.Entity.Order.EntityWorkOrderCostDetail entity)
        {
            this._entity = entity;
        }

        /// <summary>
        /// 与实体类(EntityWorkOrderCostDetail)关联
        /// </summary>
        public project.Entity.Order.EntityWorkOrderCostDetail Entity
        {
            get { return _entity as project.Entity.Order.EntityWorkOrderCostDetail; }
        }

        /// </summary>
        ///load 方法 pid主键
        /// </summary>
        public void load(string id, string accID)
        {
            DataRow dr = objdata.ExecuteDataSet("select * from WO_WorkOrder_Cost_Detail where RowPointer='" + id + "' and AccID='" + accID + "'").Tables[0].Rows[0];
            _entity.InnerEntityOID = dr["RowPointer"].ToString();
            _entity.AccID = dr["AccID"].ToString();
            _entity.CostNo = dr["CostNo"].ToString();
            _entity.OrderNo = dr["OrderNo"].ToString();
            _entity.CostType = dr["CostType"].ToString();
            _entity.Context = dr["Context"].ToString();
            _entity.CostDate = ParseDateTimeForString(dr["CostDate"].ToString());
            _entity.CostAmount = ParseDecimalForString(dr["CostAmount"].ToString());
            _entity.UserNo = dr["UserNo"].ToString();
            _entity.CreateDate = ParseDateTimeForString(dr["CreateDate"].ToString());
            _entity.CreateUser = dr["CreateUser"].ToString();
            _entity.UpdateDate = ParseDateTimeForString(dr["UpdateDate"].ToString());
            _entity.UpdateUser = dr["UpdateUser"].ToString();            
        }

        /// </summary>
        ///Save方法
        /// </summary>
        public int Save()
        {
            string sqlstr = "";
            if (Entity.InnerEntityOID == null)
                sqlstr = "insert into WO_WorkOrder_Cost_Detail(RowPointer,AccID,CostNo,OrderNo,CostType,Context,CostDate,CostAmount,UserNo,CreateDate,CreateUser,UpdateDate,UpdateUser)" +
                    "values(NewID()," + "'" + Entity.AccID + "'" + "," + "'" + Entity.CostNo + "'" + "," + "'" + Entity.OrderNo + "'" + "," +
                    "'" + Entity.CostType + "'" + "," + "'" + Entity.Context + "'" + "," +
                    "'" + Entity.CostDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + Entity.CostAmount + "'" + "," +
                    "'" + Entity.UserNo + "'" + "," + "'" + Entity.CreateDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + Entity.CreateUser + "'" + "," +
                    "'" + Entity.UpdateDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + Entity.UpdateUser + "'" + 
                    ")";
            else
                sqlstr = "update WO_WorkOrder_Cost_Detail" +
                    " set CostDate=" + "'" + Entity.CostDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," +
                    "CostType=" + "'" + Entity.CostType + "'" + "," + "Context=" + "'" + Entity.Context + "'" + "," +
                    "CostAmount=" + "'" + Entity.CostAmount + "'" + "," + "UserNo=" + "'" + Entity.UserNo + "'" + "," +
                    "UpdateDate=" + "'" + Entity.UpdateDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "UpdateUser=" + "'" + Entity.UpdateUser + "'" + 
                    " where RowPointer='" + Entity.InnerEntityOID + "'";
            return objdata.ExecuteNonQuery(sqlstr);
        }

        /// </summary>
        ///Delete 方法 
        /// </summary>
        public int delete()
        {
            return objdata.ExecuteNonQuery("delete from WO_WorkOrder_Cost_Detail where RowPointer='" + Entity.InnerEntityOID + "' and AccID=" + "'" + Entity.AccID + "'");
        }

        /// <summary>
        /// 按条件查询，支持分页
        /// </summary>
        /// <param name="AccID">账套</param>
        /// <param name="CostNo">费用单号</param>
        /// <param name="OrderNo">订单号</param>
        /// <param name="UserNo">费用用户</param>
        /// <param name="CostType">费用类型</param>
        /// <param name="CostDate">费用日期</param>
        /// <returns></returns>
        public System.Collections.ICollection GetWorkOrderCostDetailQuery(String AccID, String CostNo, String OrderNo, String UserNo, String CostType, DateTime MinCostDate, DateTime MaxCostDate, int startRow, int pageSize)
        {
            if (startRow < 0 || pageSize <= 0)
            {
                throw new Exception();
            }

            return GetListHelper(AccID, CostNo, OrderNo, UserNo, CostType, MinCostDate, MaxCostDate, startRow, pageSize);
        }

        /// <summary>
        /// 按条件查询，不支持分页
        /// </summary>
        /// <param name="AccID">账套</param>
        /// <param name="CostNo">费用单号</param>
        /// <param name="OrderNo">订单号</param>
        /// <param name="UserNo">费用用户</param>
        /// <param name="CostType">费用类型</param>
        /// <param name="CostDate">费用日期</param>
        /// <returns></returns>
        public System.Collections.ICollection GetWorkOrderCostDetailQuery(String AccID, String CostNo, String OrderNo, String UserNo, String CostType, DateTime MinCostDate, DateTime MaxCostDate)
        {
            return GetListHelper(AccID, CostNo, OrderNo, UserNo, CostType, MinCostDate, MaxCostDate, START_ROW_INIT, START_ROW_INIT);
        }

        /// <summary>
        /// 返回集合的大小
        /// </summary>
        /// <param name="AccID">账套</param>
        /// <param name="CostNo">费用单号</param>
        /// <param name="OrderNo">订单号</param>
        /// <param name="UserNo">费用用户</param>
        /// <param name="CostType">费用类型</param>
        /// <param name="CostDate">费用日期</param>
        /// <returns></returns>
        public int GetWorkOrderCostDetailCount(String AccID, String CostNo, String OrderNo, String UserNo, String CostType, DateTime MinCostDate, DateTime MaxCostDate)
        {
            string wherestr = "";
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and AccID='" + AccID + "'";
            }
            if (CostNo != string.Empty)
            {
                wherestr = wherestr + " and CostNo like '" + CostNo + "'";
            }
            if (OrderNo != string.Empty)
            {
                wherestr = wherestr + " and OrderNo like '%" + OrderNo + "%'";
            }
            if (UserNo != string.Empty)
            {
                wherestr = wherestr + " and UserNo like '" + UserNo + "'";
            }
            if (CostType != string.Empty)
            {
                wherestr = wherestr + " and CostType='" + CostType + "'";
            }
            if (MinCostDate != default(DateTime))
            {
                wherestr = wherestr + " and convert(nvarchar(10),CostDate,121)>='" + MinCostDate.ToString("yyyy-MM-dd") + "'";
            }
            if (MaxCostDate != default(DateTime))
            {
                wherestr = wherestr + " and convert(nvarchar(10),a.CostDate,121)<='" + MaxCostDate.ToString("yyyy-MM-dd") + "'";
            }

            string count = objdata.ExecuteDataSet("select count(*) as cnt from WO_WorkOrder_Cost_Detail where 1=1 " + wherestr).Tables[0].Rows[0]["cnt"].ToString();
            return int.Parse(count);
        }

        /// <summary>
        /// 按条件查询，返回符合条件的集合
        /// </summary>
        /// <param name="AccID">账套</param>
        /// <param name="CostNo">费用单号</param>
        /// <param name="OrderNo">订单号</param>
        /// <param name="UserNo">费用用户</param>
        /// <param name="CustNo">客户编号</param>
        /// <param name="CostType">费用类型</param>
        /// <param name="CostDate">费用日期</param>
        /// <returns></returns>
        private System.Collections.ICollection GetListHelper(String AccID, String CostNo, String OrderNo, String UserNo, String CostType, DateTime MinCostDate, DateTime MaxCostDate, int startRow, int pageSize)
        {
            string wherestr = "";
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and AccID='" + AccID + "'";
            }
            if (CostNo != string.Empty)
            {
                wherestr = wherestr + " and CostNo like '" + CostNo + "'";
            }
            if (OrderNo != string.Empty)
            {
                wherestr = wherestr + " and OrderNo like '%" + OrderNo + "%'";
            }
            if (UserNo != string.Empty)
            {
                wherestr = wherestr + " and UserNo like '" + UserNo + "'";
            }
            if (CostType != string.Empty)
            {
                wherestr = wherestr + " and CostType='" + CostType + "'";
            }
            if (MinCostDate != default(DateTime))
            {
                wherestr = wherestr + " and convert(nvarchar(10),CostDate,121)>='" + MinCostDate.ToString("yyyy-MM-dd") + "'";
            }
            if (MaxCostDate != default(DateTime))
            {
                wherestr = wherestr + " and convert(nvarchar(10),CostDate,121)<='" + MaxCostDate.ToString("yyyy-MM-dd") + "'";
            }

            System.Collections.IList entitys = null;
            if (startRow > START_ROW_INIT && pageSize > START_ROW_INIT)
            {
                entitys = Query(objdata.ExecSelect("WO_WorkOrder_Cost_Detail", wherestr, startRow, pageSize, orderstr));
            }
            else
            {
                entitys = Query(objdata.ExecSelect("WO_WorkOrder_Cost_Detail", wherestr, START_ROW_INIT, START_ROW_INIT, orderstr));
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
                project.Entity.Order.EntityWorkOrderCostDetail entity = new project.Entity.Order.EntityWorkOrderCostDetail();
                entity.InnerEntityOID = dr["RowPointer"].ToString();
                entity.AccID = dr["AccID"].ToString();
                entity.CostNo = dr["CostNo"].ToString();
                entity.OrderNo = dr["OrderNo"].ToString();
                entity.CostType = dr["CostType"].ToString();
                entity.Context = dr["Context"].ToString();
                entity.CostDate = ParseDateTimeForString(dr["CostDate"].ToString());
                entity.CostAmount = ParseDecimalForString(dr["CostAmount"].ToString());
                entity.UserNo = dr["UserNo"].ToString();
                entity.CreateDate = ParseDateTimeForString(dr["CreateDate"].ToString());
                entity.CreateUser = dr["CreateUser"].ToString();
                entity.UpdateDate = ParseDateTimeForString(dr["UpdateDate"].ToString());
                entity.UpdateUser = dr["UpdateUser"].ToString();
                result.Add(entity);
            }
            return result;
        }

    }
}
