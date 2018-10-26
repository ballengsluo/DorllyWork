using System;
using System.Data;
namespace project.Business.Sys
{
    /// <summary>
    /// 用户类型表的业务类
    /// </summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    public sealed class BusinessUserType : project.Business.AbstractPmBusiness
    {
        private project.Entity.Sys.EntityUserType _entity = new project.Entity.Sys.EntityUserType();
        public string Userstr = "UserTypeNo";
        Data objdata = new Data();

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public BusinessUserType() { }

        /// <summary>
        /// 带参数的构函数
        /// </summary>
        /// <param name="entity">实体类</param>
        public BusinessUserType(project.Entity.Sys.EntityUserType entity)
        {
            this._entity = entity;
        }

        /// <summary>
        /// 与实体类(EntityUserType)关联
        /// </summary>
        public project.Entity.Sys.EntityUserType Entity
        {
            get { return _entity as project.Entity.Sys.EntityUserType; }
        }

        /// </summary>
        ///load 方法 pid主键
        /// </summary>
        public void load(string UserTypeNo,string AccID)
        {
            DataRow dr = objdata.ExecuteDataSet("select * from Sys_User_Type where UserTypeNo='" + UserTypeNo + "' and AccID='" + AccID + "'").Tables[0].Rows[0];
            _entity.UserTypeNo = dr["UserTypeNo"].ToString();
            _entity.UserTypeName = dr["UserTypeName"].ToString();
            _entity.OrderType = dr["OrderType"].ToString();
            _entity.OrderTypeName = dr["OrderTypeName"].ToString();
            _entity.AccID = dr["AccID"].ToString();
        }

        /// </summary>
        ///Save方法
        /// </summary>
        public int Save(string type)
        {
            string sqlstr = "";
            if (type == "insert")
                sqlstr = "insert into Sys_User_Type(UserTypeNo,UserTypeName,AccID,OrderType,OrderTypeName)" +
                    "values('" + Entity.UserTypeNo + "'," + "'" + Entity.UserTypeName + "'" + "," + "'" + Entity.AccID + "'" + "," + 
                    "'" + Entity.OrderType + "'" + "," + "'" + Entity.OrderTypeName + "'" + ")";
            else
                sqlstr = "update Sys_User_Type" +
                    " set UserTypeName=" + "'" + Entity.UserTypeName + "'" + "," +
                    "OrderType=" + "'" + Entity.OrderType + "'" + "," + "OrderTypeName=" + "'" + Entity.OrderTypeName + "'" +
                    " where UserTypeNo='" + Entity.UserTypeNo + "'";
            return objdata.ExecuteNonQuery(sqlstr);
        }

        /// </summary>
        ///Delete 方法 
        /// </summary>
        public int delete()
        {
            return objdata.ExecuteNonQuery("delete from Sys_User_Type where UserTypeNo='" + Entity.UserTypeNo + "' and AccID='" + Entity.AccID + "'");
        }

        /// <summary>
        /// 按条件查询，支持分页
        /// </summary>
        /// <param name="UserTypeNoEquals">用户类型编号</param>
        /// <param name="UserTypeNameEquals">用户类型名称</param>
        /// <param name="AccID">账户</param>
        /// <param name="OrderType">工单类型</param>
        /// <returns></returns>
        public System.Collections.ICollection GetUserTypeListQuery(String UserTypeNo, String UserTypeName, String AccID, String OrderType, int startRow, int pageSize)
        {
            if (startRow < 0 || pageSize <= 0)
            {
                throw new Exception();
            }

            return GetListHelper(UserTypeNo, UserTypeName, AccID, OrderType, startRow, pageSize);
        }

        /// <summary>
        /// 按条件查询，不支持分页
        /// </summary>
        /// <param name="UserTypeNoEquals">用户类型编号</param>
        /// <param name="UserTypeNameEquals">用户类型名称</param>
        /// <param name="AccID">账户</param>
        /// <param name="OrderType">工单类型</param>
        /// <returns></returns>
        public System.Collections.ICollection GetUserTypeListQuery(String UserTypeNo, String UserTypeName, String AccID, String OrderType)
        {
            return GetListHelper(UserTypeNo, UserTypeName, AccID, OrderType, START_ROW_INIT, START_ROW_INIT);
        }

        /// <summary>
        /// 返回集合的大小
        /// </summary>
        /// <param name="UserTypeNoEquals">用户类型编号</param>
        /// <param name="UserTypeNameEquals">用户类型名称</param>
        /// <param name="AccID">账户</param>
        /// <param name="OrderType">工单类型</param>
        /// <returns></returns>
        public int GetUserTypeListCount(String UserTypeNo, String UserTypeName, String AccID, String OrderType)
        {
            string wherestr = "";
            if (UserTypeNo != string.Empty)
            {
                wherestr = wherestr + " and UserTypeNo like '%" + UserTypeNo + "%'";
            }
            if (UserTypeName != string.Empty)
            {
                wherestr = wherestr + " and UserTypeName like '%" + UserTypeName + "%'";
            }
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and AccID like '" + AccID + "'";
            }
            if (OrderType != string.Empty)
            {
                wherestr = wherestr + " and charindex('" + OrderType + "',OrderType) > 0";
            }

            string count = objdata.ExecuteDataSet("select count(*) as cnt from Sys_User_Type where 1=1 " + wherestr).Tables[0].Rows[0]["cnt"].ToString();
            return int.Parse(count);
        }

        /// <summary>
        /// 按条件查询，返回符合条件的集合
        /// </summary>
        /// <param name="UserTypeNoEquals">用户类型编号</param>
        /// <param name="UserTypeNameEquals">用户类型名称</param>
        /// <param name="AccID">账户</param>
        /// <param name="OrderType">工单类型</param>
        /// <returns></returns>
        private System.Collections.ICollection GetListHelper(String UserTypeNo, String UserTypeName, String AccID, String OrderType, int startRow, int pageSize)
        {
            string wherestr = "";
            if (UserTypeNo != string.Empty)
            {
                wherestr = wherestr + " and UserTypeNo like '%" + UserTypeNo + "%'";
            }
            if (UserTypeName != string.Empty)
            {
                wherestr = wherestr + " and UserTypeName like '%" + UserTypeName + "%'";
            }
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and AccID like '" + AccID + "'";
            }
            if (OrderType != string.Empty)
            {
                wherestr = wherestr + " and charindex('" + OrderType + "',OrderType) > 0";
            }

            System.Collections.IList entitys = null;
            if (startRow > START_ROW_INIT && pageSize > START_ROW_INIT)
            {
                entitys = Query(objdata.ExecSelect("Sys_User_Type", wherestr, startRow, pageSize, Userstr));
            }
            else
            {
                entitys = Query(objdata.ExecSelect("Sys_User_Type", wherestr, START_ROW_INIT, START_ROW_INIT, Userstr));
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
                project.Entity.Sys.EntityUserType entity = new project.Entity.Sys.EntityUserType();

                entity.UserTypeNo = dr["UserTypeNo"].ToString();
                entity.UserTypeName = dr["UserTypeName"].ToString();
                entity.OrderType = dr["OrderType"].ToString();
                entity.OrderTypeName = dr["OrderTypeName"].ToString();
                entity.AccID = dr["AccID"].ToString();
                result.Add(entity);
            }
            return result;
        }

    }
}
