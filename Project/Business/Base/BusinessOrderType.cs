using System;
using System.Data;
namespace project.Business.Base
{
    /// <summary>
    /// 工单类型表的业务类
    /// </summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    public sealed class BusinessOrderType : project.Business.AbstractPmBusiness
    {
        private project.Entity.Base.EntityOrderType _entity = new project.Entity.Base.EntityOrderType();
        public string orderstr = "a.OrderTypeNo";
        Data objdata = new Data();

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public BusinessOrderType() { }

        /// <summary>
        /// 带参数的构函数
        /// </summary>
        /// <param name="entity">实体类</param>
        public BusinessOrderType(project.Entity.Base.EntityOrderType entity)
        {
            this._entity = entity;
        }

        /// <summary>
        /// 与实体类(EntityOrderType)关联
        /// </summary>
        public project.Entity.Base.EntityOrderType Entity
        {
            get { return _entity as project.Entity.Base.EntityOrderType; }
        }

        /// </summary>
        ///load 方法 pid主键
        /// </summary>
        public void load(string OrderTypeNo, string AccID)
        {
            DataRow dr = objdata.ExecuteDataSet("select a.*,b.FlowName from Base_Order_Type a left join (select * from Base_Flow where AccID='" + AccID + "') b on a.FlowNo=b.FlowNo " +
                "where a.OrderTypeNo='" + OrderTypeNo + "' and a.AccID='" + AccID + "'").Tables[0].Rows[0];
            _entity.OrderTypeNo = dr["OrderTypeNo"].ToString();
            _entity.OrderTypeName = dr["OrderTypeName"].ToString();
            _entity.FlowNo = dr["FlowNo"].ToString();
            _entity.FlowName = dr["FlowName"].ToString();
            _entity.AccID = dr["AccID"].ToString();
        }

        /// </summary>
        ///Save方法
        /// </summary>
        public int Save(string type)
        {
            string sqlstr = "";
            if (type == "insert")
                sqlstr = "insert into Base_Order_Type(OrderTypeNo,OrderTypeName,FlowNo,AccID)" +
                    "values('" + Entity.OrderTypeNo + "'," + "'" + Entity.OrderTypeName + "'" + "," + "'" + Entity.FlowNo + "'" + "," + "'" + Entity.AccID + "'" + ")";
            else
                sqlstr = "update Base_Order_Type" +
                    " set OrderTypeName=" + "'" + Entity.OrderTypeName + "'" + "," + "FlowNo=" + "'" + Entity.FlowNo + "'" +
                    " where OrderTypeNo='" + Entity.OrderTypeNo + "'";
            return objdata.ExecuteNonQuery(sqlstr);
        }

        /// </summary>
        ///Delete 方法 
        /// </summary>
        public int delete()
        {
            return objdata.ExecuteNonQuery("delete from Base_Order_Type where OrderTypeNo='" + Entity.OrderTypeNo + "' and AccID='" + Entity.AccID + "'");
        }

        /// <summary>
        /// 按条件查询，支持分页
        /// </summary>
        /// <param name="OrderTypeNoEquals">工单类型编号</param>
        /// <param name="OrderTypeNameEquals">工单类型名称</param>
        /// <param name="FlowNo">流程编号</param>
        /// <param name="AccID">账户</param>
        /// <returns></returns>
        public System.Collections.ICollection GetOrderTypeListQuery(String OrderTypeNo, String OrderTypeName, String FlowNo, String AccID, int startRow, int pageSize)
        {
            if (startRow < 0 || pageSize <= 0)
            {
                throw new Exception();
            }

            return GetListHelper(OrderTypeNo, OrderTypeName, FlowNo, AccID, startRow, pageSize);
        }

        /// <summary>
        /// 按条件查询，不支持分页
        /// </summary>
        /// <param name="OrderTypeNoEquals">工单类型编号</param>
        /// <param name="OrderTypeNameEquals">工单类型名称</param>
        /// <param name="FlowNo">流程编号</param>
        /// <param name="AccID">账户</param>
        /// <returns></returns>
        public System.Collections.ICollection GetOrderTypeListQuery(String OrderTypeNo, String OrderTypeName, String FlowNo, String AccID)
        {
            return GetListHelper(OrderTypeNo, OrderTypeName, FlowNo, AccID, START_ROW_INIT, START_ROW_INIT);
        }

        /// <summary>
        /// 返回集合的大小
        /// </summary>
        /// <param name="OrderTypeNoEquals">工单类型编号</param>
        /// <param name="OrderTypeNameEquals">工单类型名称</param>
        /// <param name="FlowNo">流程编号</param>
        /// <param name="AccID">账户</param>
        /// <returns></returns>
        public int GetOrderTypeListCount(String OrderTypeNo, String OrderTypeName, String FlowNo, String AccID)
        {
            string wherestr = "";
            if (OrderTypeNo != string.Empty)
            {
                wherestr = wherestr + " and a.OrderTypeNo like '%" + OrderTypeNo + "%'";
            }
            if (OrderTypeName != string.Empty)
            {
                wherestr = wherestr + " and a.OrderTypeName like '%" + OrderTypeName + "%'";
            }
            if (FlowNo != string.Empty)
            {
                wherestr = wherestr + " and a.FlowNo like '" + FlowNo + "'";
            }
            if (FlowNo != string.Empty)
            {
                wherestr = wherestr + " and a.AccID like '" + AccID + "'";
            }

            string count = objdata.ExecuteDataSet("select count(*) as cnt from Base_Order_Type a where 1=1 " + wherestr).Tables[0].Rows[0]["cnt"].ToString();
            return int.Parse(count);
        }

        /// <summary>
        /// 按条件查询，返回符合条件的集合
        /// </summary>
        /// <param name="OrderTypeNoEquals">工单类型编号</param>
        /// <param name="OrderTypeNameEquals">工单类型名称</param>
        /// <param name="FlowNo">流程编号</param>
        /// <param name="AccID">账户</param>
        /// <returns></returns>
        private System.Collections.ICollection GetListHelper(String OrderTypeNo, String OrderTypeName, String FlowNo, String AccID, int startRow, int pageSize)
        {
            string wherestr = "";
            if (OrderTypeNo != string.Empty)
            {
                wherestr = wherestr + " and a.OrderTypeNo like '%" + OrderTypeNo + "%'";
            }
            if (OrderTypeName != string.Empty)
            {
                wherestr = wherestr + " and a.OrderTypeName like '%" + OrderTypeName + "%'";
            }
            if (FlowNo != string.Empty)
            {
                wherestr = wherestr + " and a.FlowNo like '" + FlowNo + "'";
            }
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and a.AccID like '" + AccID + "'";
            }

            System.Collections.IList entitys = null;
            if (startRow > START_ROW_INIT && pageSize > START_ROW_INIT)
            {
                entitys = Query(objdata.ExecSelect("Base_Order_Type a left join (select * from Base_Flow where AccID='"+AccID+"') b on a.FlowNo=b.FlowNo", "a.*,b.FlowName", wherestr, startRow, pageSize, orderstr));
            }
            else
            {
                entitys = Query(objdata.ExecSelect("Base_Order_Type a left join (select * from Base_Flow where AccID='" + AccID + "') b on a.FlowNo=b.FlowNo", "a.*,b.FlowName", wherestr, START_ROW_INIT, START_ROW_INIT, orderstr));
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
                project.Entity.Base.EntityOrderType entity = new project.Entity.Base.EntityOrderType();

                entity.OrderTypeNo = dr["OrderTypeNo"].ToString();
                entity.OrderTypeName = dr["OrderTypeName"].ToString();
                entity.FlowNo = dr["FlowNo"].ToString();
                entity.FlowName = dr["FlowName"].ToString();
                entity.AccID = dr["AccID"].ToString();
                result.Add(entity);
            }
            return result;
        }

    }
}
