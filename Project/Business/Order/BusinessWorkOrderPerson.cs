using System;
using System.Data;
namespace project.Business.Order
{
    /// <summary>
    /// 用户信息的业务类
    /// </summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    public sealed class BusinessWorkOrderPerson : project.Business.AbstractPmBusiness
    {
        private project.Entity.Order.EntityWorkOrderPerson _entity = new project.Entity.Order.EntityWorkOrderPerson();
        public string orderstr = "OrderNo";
        Data objdata = new Data();

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public BusinessWorkOrderPerson() { }

        /// <summary>
        /// 带参数的构函数
        /// </summary>
        /// <param name="entity">实体类</param>
        public BusinessWorkOrderPerson(project.Entity.Order.EntityWorkOrderPerson entity)
        {
            this._entity = entity;
        }

        /// <summary>
        /// 与实体类(EntityWorkOrderPerson)关联
        /// </summary>
        public project.Entity.Order.EntityWorkOrderPerson Entity
        {
            get { return _entity as project.Entity.Order.EntityWorkOrderPerson; }
        }

        /// </summary>
        ///load 方法 pid主键
        /// </summary>
        public void load(string id)
        {
            DataRow dr = objdata.ExecuteDataSet("select * from WO_WorkOrder_Person where RowPointer='" + id + "'").Tables[0].Rows[0];
            _entity.InnerEntityOID = dr["RowPointer"].ToString();
            _entity.AccID = dr["AccID"].ToString();
            _entity.OrderNo = dr["OrderNo"].ToString();
            _entity.UserNo = dr["UserNo"].ToString();
            _entity.ResponseTime = ParseDateTimeForString(dr["ResponseTime"].ToString());
            _entity.AppoIntTime = ParseDateTimeForString(dr["AppoIntTime"].ToString());
            _entity.SignTime = ParseDateTimeForString(dr["SignTime"].ToString());
            _entity.WorkTime = ParseDateTimeForString(dr["WorkTime"].ToString());
            _entity.FinishTime = ParseDateTimeForString(dr["FinishTime"].ToString());
            _entity.CloseTime = ParseDateTimeForString(dr["CloseTime"].ToString());
            _entity.ConfirmTime = ParseDateTimeForString(dr["ConfirmTime"].ToString());
            _entity.CreateUser = dr["CreateUser"].ToString();
            _entity.CreateDate = ParseDateTimeForString(dr["CreateDate"].ToString());
            _entity.UpdateUser = dr["UpdateUser"].ToString();
            _entity.UpdateDate = ParseDateTimeForString(dr["UpdateDate"].ToString());
            _entity.IsBack = bool.Parse(dr["IsBack"].ToString());
            _entity.BackDate = ParseDateTimeForString(dr["BackDate"].ToString());
            _entity.BackReason = dr["BackReason"].ToString();
            _entity.IsHangUp = bool.Parse(dr["IsHangUp"].ToString());
            _entity.HangUpDate = ParseDateTimeForString(dr["HangUpDate"].ToString());
            _entity.HangUpReason = dr["HangUpReason"].ToString();
            _entity.IsDel = bool.Parse(dr["IsDel"].ToString());
        }
        
        /// </summary>
        ///Save方法
        /// </summary>
        public int Save()
        {
            string sqlstr = "";
            if (Entity.InnerEntityOID == null)
                sqlstr = @"insert into WO_WorkOrder_Person(RowPointer,AccID,OrderNo,UserNo," +
                    "ResponseTime,AppoIntTime,SignTime,WorkTime,FinishTime,CloseTime,ConfirmTime,"+
                    "CreateUser,CreateDate,UpdateUser,UpdateDate,"+
                    "IsBack,BackDate,BackReason,IsHangUp,HangUpDate,HangUpReason,IsDel)" +
                    "values(newid()," + "'" + Entity.AccID + "'" + "," + "'" + Entity.OrderNo + "'" + "," + "'" + Entity.UserNo + "'" + "," +
                    "null,null,null,null,null,null,null," +
                    "'" + Entity.CreateUser + "'" + "," + "'" + Entity.CreateDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," +
                    "'" + Entity.UpdateUser + "'" + "," + "'" + Entity.UpdateDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," +
                    "0,null,'',0,null,''," + (Entity.IsDel == true ? 1 : 0) + ")";
            else
                sqlstr = "update WO_WorkOrder_Person" +
                    " set OrderNo=" + "'" + Entity.OrderNo + "'" + "," + "UserNo=" + "'" + Entity.UserNo + "'" + "," +
                    "UpdateUser=" + "'" + Entity.UpdateUser + "'" + "," + "UpdateDate=" + "'" + Entity.UpdateDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                    " where RowPointer='" + Entity.InnerEntityOID.ToString() + "'";
            return objdata.ExecuteNonQuery(sqlstr);
        }
        
        /// <summary>
        /// 按条件查询，支持分页
        /// </summary>
        /// <param name="AccID">账套ID</param>
        /// <param name="OrderNo">工单编号</param>
        /// <param name="UserNo">用户编号</param>
        /// <param name="IsBack">是否退回</param>
        /// <param name="IsDel">是否删除</param>
        /// <returns></returns>
        public System.Collections.ICollection GetWorkOrderPersonListQuery(String AccID, String OrderNo, String UserNo, bool? IsBack, bool? IsDel, int startRow, int pageSize)
        {
            if (startRow < 0 || pageSize <= 0)
            {
                throw new Exception();
            }

            return GetListHelper(AccID, OrderNo, UserNo, IsBack, IsDel, startRow, pageSize);
        }

        /// <summary>
        /// 按条件查询，不支持分页
        /// </summary>
        /// <param name="AccID">账套ID</param>
        /// <param name="OrderNo">工单编号</param>
        /// <param name="UserNo">用户编号</param>
        /// <param name="IsBack">是否退回</param>
        /// <param name="IsDel">是否删除</param>
        /// <returns></returns>
        public System.Collections.ICollection GetWorkOrderPersonListQuery(String AccID, String OrderNo, String UserNo, bool? IsBack, bool? IsDel)
        {
            return GetListHelper(AccID, OrderNo, UserNo, IsBack, IsDel, START_ROW_INIT, START_ROW_INIT);
        }

        /// <summary>
        /// 返回集合的大小
        /// </summary>
        /// <param name="AccID">账套ID</param>
        /// <param name="OrderNo">工单编号</param>
        /// <param name="UserNo">用户编号</param>
        /// <param name="IsBack">是否退回</param>
        /// <param name="IsDel">是否删除</param>
        /// <returns></returns>
        public int GetWorkOrderPersonListCount(String AccID, String OrderNo, String UserNo, bool? IsBack, bool? IsDel)
        {
            string wherestr = "";
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and AccID='" + AccID + "'";
            }
            if (OrderNo != string.Empty)
            {
                wherestr = wherestr + " and OrderNo like '%" + OrderNo + "%'";
            }
            if (UserNo != string.Empty)
            {
                wherestr = wherestr + " and UserNo='" + UserNo + "'";
            }
            if (IsBack != null)
            {
                wherestr = wherestr + " and IsBack=" + (IsBack == true ? "1" : "0");
            }
            if (IsDel != null)
            {
                wherestr = wherestr + " and IsDel=" + (IsDel == true ? "1" : "0");
            }

            string count = objdata.ExecuteDataSet("select count(*) as cnt from WO_WorkOrder_Person where 1=1 " + wherestr).Tables[0].Rows[0]["cnt"].ToString();
            return int.Parse(count);
        }

        /// <summary>
        /// 按条件查询，返回符合条件的集合
        /// </summary>
        /// <param name="AccID">账套ID</param>
        /// <param name="OrderNo">工单编号</param>
        /// <param name="UserNo">用户编号</param>
        /// <param name="IsBack">是否退回</param>
        /// <param name="IsDel">是否删除</param>
        /// <returns></returns>
        private System.Collections.ICollection GetListHelper(String AccID, String OrderNo, String UserNo, bool? IsBack, bool? IsDel, int startRow, int pageSize)
        {
            string wherestr = "";
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and AccID='" + AccID + "'";
            }
            if (OrderNo != string.Empty)
            {
                wherestr = wherestr + " and OrderNo like '%" + OrderNo + "%'";
            }
            if (UserNo != string.Empty)
            {
                wherestr = wherestr + " and UserNo='" + UserNo + "'";
            }
            if (IsBack != null)
            {
                wherestr = wherestr + " and IsBack=" + (IsBack == true ? "1" : "0");
            }
            if (IsDel != null)
            {
                wherestr = wherestr + " and IsDel=" + (IsDel == true ? "1" : "0");
            }

            System.Collections.IList entitys = null;
            if (startRow > START_ROW_INIT && pageSize > START_ROW_INIT)
            {
                entitys = Query(objdata.ExecSelect("WO_WorkOrder_Person", wherestr, startRow, pageSize, orderstr));
            }
            else
            {
                entitys = Query(objdata.ExecSelect("WO_WorkOrder_Person", wherestr, START_ROW_INIT, START_ROW_INIT, orderstr));
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
                project.Entity.Order.EntityWorkOrderPerson entity = new project.Entity.Order.EntityWorkOrderPerson();
                entity.InnerEntityOID = dr["RowPointer"].ToString();
                entity.AccID = dr["AccID"].ToString();
                entity.OrderNo = dr["OrderNo"].ToString();
                entity.UserNo = dr["UserNo"].ToString();
                entity.ResponseTime = ParseDateTimeForString(dr["ResponseTime"].ToString());
                entity.AppoIntTime = ParseDateTimeForString(dr["AppoIntTime"].ToString());
                entity.SignTime = ParseDateTimeForString(dr["SignTime"].ToString());
                entity.WorkTime = ParseDateTimeForString(dr["WorkTime"].ToString());
                entity.FinishTime = ParseDateTimeForString(dr["FinishTime"].ToString());
                entity.CloseTime = ParseDateTimeForString(dr["CloseTime"].ToString());
                entity.ConfirmTime = ParseDateTimeForString(dr["ConfirmTime"].ToString());
                entity.CreateUser = dr["CreateUser"].ToString();
                entity.UpdateDate = ParseDateTimeForString(dr["UpdateDate"].ToString());
                entity.UpdateUser = dr["UpdateUser"].ToString();
                entity.UpdateUser = dr["UpdateUser"].ToString();
                entity.IsBack = bool.Parse(dr["IsBack"].ToString());
                entity.BackDate = ParseDateTimeForString(dr["BackDate"].ToString());
                entity.BackReason = dr["BackReason"].ToString();
                entity.IsHangUp = bool.Parse(dr["IsHangUp"].ToString());
                entity.HangUpDate = ParseDateTimeForString(dr["HangUpDate"].ToString());
                entity.HangUpReason = dr["HangUpReason"].ToString();
                entity.IsDel = bool.Parse(dr["IsDel"].ToString());
                result.Add(entity);
            }
            return result;
        }

    }
}
