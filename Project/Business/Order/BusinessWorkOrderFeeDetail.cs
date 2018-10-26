using System;
using System.Data;
namespace project.Business.Order
{
    /// <summary>
    /// 工单收入收款的业务类
    /// </summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    public sealed class BusinessWorkOrderFeeDetail : project.Business.AbstractPmBusiness
    {
        private project.Entity.Order.EntityWorkOrderFeeDetail _entity = new project.Entity.Order.EntityWorkOrderFeeDetail();
        public string orderstr = "FeeDate desc";
        Data objdata = new Data();

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public BusinessWorkOrderFeeDetail() { }

        /// <summary>
        /// 带参数的构函数
        /// </summary>
        /// <param name="entity">实体类</param>
        public BusinessWorkOrderFeeDetail(project.Entity.Order.EntityWorkOrderFeeDetail entity)
        {
            this._entity = entity;
        }

        /// <summary>
        /// 与实体类(EntityWorkOrderFeeDetail)关联
        /// </summary>
        public project.Entity.Order.EntityWorkOrderFeeDetail Entity
        {
            get { return _entity as project.Entity.Order.EntityWorkOrderFeeDetail; }
        }

        /// </summary>
        ///load 方法 pid主键
        /// </summary>
        public void load(string id, string accID)
        {
            DataRow dr = objdata.ExecuteDataSet("select * from WO_WorkOrder_Fee_Detail where RowPointer='" + id + "' and AccID='" + accID + "'").Tables[0].Rows[0];
            _entity.InnerEntityOID = dr["RowPointer"].ToString();
            _entity.AccID = dr["AccID"].ToString();
            _entity.FeeNo = dr["FeeNo"].ToString();
            _entity.OrderNo = dr["OrderNo"].ToString();
            _entity.FeeType = dr["FeeType"].ToString();
            _entity.Context = dr["Context"].ToString();
            _entity.FeeDate = ParseDateTimeForString(dr["FeeDate"].ToString());
            _entity.FeeAmount = ParseDecimalForString(dr["FeeAmount"].ToString());
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
                sqlstr = "insert into WO_WorkOrder_Fee_Detail(RowPointer,AccID,FeeNo,OrderNo,FeeType,Context,FeeDate,FeeAmount,UserNo,CreateDate,CreateUser,UpdateDate,UpdateUser)" +
                    "values(NewID()," + "'" + Entity.AccID + "'" + "," + "'" + Entity.FeeNo + "'" + "," + "'" + Entity.OrderNo + "'" + "," +
                    "'" + Entity.FeeType + "'" + "," + "'" + Entity.Context + "'" + "," +
                    "'" + Entity.FeeDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + Entity.FeeAmount + "'" + "," +
                    "'" + Entity.UserNo + "'" + "," + "'" + Entity.CreateDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + Entity.CreateUser + "'" + "," +
                    "'" + Entity.UpdateDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + Entity.UpdateUser + "'" + 
                    ")";
            else
                sqlstr = "update WO_WorkOrder_Fee_Detail" +
                    " set FeeDate=" + "'" + Entity.FeeDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," +
                    "FeeType=" + "'" + Entity.FeeType + "'" + "," + "Context=" + "'" + Entity.Context + "'" + "," +
                    "FeeAmount=" + "'" + Entity.FeeAmount + "'" + "," + "UserNo=" + "'" + Entity.UserNo + "'" + "," +
                    "UpdateDate=" + "'" + Entity.UpdateDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "UpdateUser=" + "'" + Entity.UpdateUser + "'" + 
                    " where RowPointer='" + Entity.InnerEntityOID + "'";
            return objdata.ExecuteNonQuery(sqlstr);
        }

        /// </summary>
        ///Delete 方法 
        /// </summary>
        public int delete()
        {
            return objdata.ExecuteNonQuery("delete from WO_WorkOrder_Fee_Detail where RowPointer='" + Entity.InnerEntityOID + "' and AccID=" + "'" + Entity.AccID + "'");
        }

        /// <summary>
        /// 按条件查询，支持分页
        /// </summary>
        /// <param name="AccID">账套</param>
        /// <param name="FeeNo">收款单号</param>
        /// <param name="OrderNo">订单号</param>
        /// <param name="UserNo">收款用户</param>
        /// <param name="FeeType">收款类型</param>
        /// <param name="FeeDate">收款日期</param>
        /// <returns></returns>
        public System.Collections.ICollection GetWorkOrderFeeDetailQuery(String AccID, String FeeNo, String OrderNo, String UserNo, String FeeType, DateTime MinFeeDate, DateTime MaxFeeDate, int startRow, int pageSize)
        {
            if (startRow < 0 || pageSize <= 0)
            {
                throw new Exception();
            }

            return GetListHelper(AccID, FeeNo, OrderNo, UserNo, FeeType, MinFeeDate, MaxFeeDate, startRow, pageSize);
        }

        /// <summary>
        /// 按条件查询，不支持分页
        /// </summary>
        /// <param name="AccID">账套</param>
        /// <param name="FeeNo">收款单号</param>
        /// <param name="OrderNo">订单号</param>
        /// <param name="UserNo">收款用户</param>
        /// <param name="FeeType">收款类型</param>
        /// <param name="FeeDate">收款日期</param>
        /// <returns></returns>
        public System.Collections.ICollection GetWorkOrderFeeDetailQuery(String AccID, String FeeNo, String OrderNo, String UserNo, String FeeType, DateTime MinFeeDate, DateTime MaxFeeDate)
        {
            return GetListHelper(AccID, FeeNo, OrderNo, UserNo, FeeType, MinFeeDate, MaxFeeDate, START_ROW_INIT, START_ROW_INIT);
        }

        /// <summary>
        /// 返回集合的大小
        /// </summary>
        /// <param name="AccID">账套</param>
        /// <param name="FeeNo">收款单号</param>
        /// <param name="OrderNo">订单号</param>
        /// <param name="UserNo">收款用户</param>
        /// <param name="FeeType">收款类型</param>
        /// <param name="FeeDate">收款日期</param>
        /// <returns></returns>
        public int GetWorkOrderFeeDetailCount(String AccID, String FeeNo, String OrderNo, String UserNo, String FeeType, DateTime MinFeeDate, DateTime MaxFeeDate)
        {
            string wherestr = "";
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and AccID='" + AccID + "'";
            }
            if (FeeNo != string.Empty)
            {
                wherestr = wherestr + " and FeeNo like '" + FeeNo + "'";
            }
            if (OrderNo != string.Empty)
            {
                wherestr = wherestr + " and OrderNo like '%" + OrderNo + "%'";
            }
            if (UserNo != string.Empty)
            {
                wherestr = wherestr + " and UserNo like '" + UserNo + "'";
            }
            if (FeeType != string.Empty)
            {
                wherestr = wherestr + " and FeeType='" + FeeType + "'";
            }
            if (MinFeeDate != default(DateTime))
            {
                wherestr = wherestr + " and convert(nvarchar(10),FeeDate,121)>='" + MinFeeDate.ToString("yyyy-MM-dd") + "'";
            }
            if (MaxFeeDate != default(DateTime))
            {
                wherestr = wherestr + " and convert(nvarchar(10),a.FeeDate,121)<='" + MaxFeeDate.ToString("yyyy-MM-dd") + "'";
            }

            string count = objdata.ExecuteDataSet("select count(*) as cnt from WO_WorkOrder_Fee_Detail where 1=1 " + wherestr).Tables[0].Rows[0]["cnt"].ToString();
            return int.Parse(count);
        }

        /// <summary>
        /// 按条件查询，返回符合条件的集合
        /// </summary>
        /// <param name="AccID">账套</param>
        /// <param name="FeeNo">收款单号</param>
        /// <param name="OrderNo">订单号</param>
        /// <param name="UserNo">收款用户</param>
        /// <param name="CustNo">客户编号</param>
        /// <param name="FeeType">收款类型</param>
        /// <param name="FeeDate">收款日期</param>
        /// <returns></returns>
        private System.Collections.ICollection GetListHelper(String AccID, String FeeNo, String OrderNo, String UserNo, String FeeType, DateTime MinFeeDate, DateTime MaxFeeDate, int startRow, int pageSize)
        {
            string wherestr = "";
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and AccID='" + AccID + "'";
            }
            if (FeeNo != string.Empty)
            {
                wherestr = wherestr + " and FeeNo like '" + FeeNo + "'";
            }
            if (OrderNo != string.Empty)
            {
                wherestr = wherestr + " and OrderNo like '%" + OrderNo + "%'";
            }
            if (UserNo != string.Empty)
            {
                wherestr = wherestr + " and UserNo like '" + UserNo + "'";
            }
            if (FeeType != string.Empty)
            {
                wherestr = wherestr + " and FeeType='" + FeeType + "'";
            }
            if (MinFeeDate != default(DateTime))
            {
                wherestr = wherestr + " and convert(nvarchar(10),FeeDate,121)>='" + MinFeeDate.ToString("yyyy-MM-dd") + "'";
            }
            if (MaxFeeDate != default(DateTime))
            {
                wherestr = wherestr + " and convert(nvarchar(10),FeeDate,121)<='" + MaxFeeDate.ToString("yyyy-MM-dd") + "'";
            }

            System.Collections.IList entitys = null;
            if (startRow > START_ROW_INIT && pageSize > START_ROW_INIT)
            {
                entitys = Query(objdata.ExecSelect("WO_WorkOrder_Fee_Detail", wherestr, startRow, pageSize, orderstr));
            }
            else
            {
                entitys = Query(objdata.ExecSelect("WO_WorkOrder_Fee_Detail", wherestr, START_ROW_INIT, START_ROW_INIT, orderstr));
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
                project.Entity.Order.EntityWorkOrderFeeDetail entity = new project.Entity.Order.EntityWorkOrderFeeDetail();
                entity.InnerEntityOID = dr["RowPointer"].ToString();
                entity.AccID = dr["AccID"].ToString();
                entity.FeeNo = dr["FeeNo"].ToString();
                entity.OrderNo = dr["OrderNo"].ToString();
                entity.FeeType = dr["FeeType"].ToString();
                entity.Context = dr["Context"].ToString();
                entity.FeeDate = ParseDateTimeForString(dr["FeeDate"].ToString());
                entity.FeeAmount = ParseDecimalForString(dr["FeeAmount"].ToString());
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
