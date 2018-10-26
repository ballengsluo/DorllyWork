using System;
using System.Data;
namespace project.Business.Base
{
    /// <summary>
    /// 数据字典的业务类
    /// </summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    public sealed class BusinessAutoAllocation : project.Business.AbstractPmBusiness
    {
        private project.Entity.Base.EntityAutoAllocation _entity = new project.Entity.Base.EntityAutoAllocation();
        public string orderstr = "OrderType,RegionNo,DeptNo,UserNo";
        Data objdata = new Data();

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public BusinessAutoAllocation() { }

        /// <summary>
        /// 带参数的构函数
        /// </summary>
        /// <param name="entity">实体类</param>
        public BusinessAutoAllocation(project.Entity.Base.EntityAutoAllocation entity)
        {
            this._entity = entity;
        }

        /// <summary>
        /// 与实体类(EntityDict)关联
        /// </summary>
        public project.Entity.Base.EntityAutoAllocation Entity
        {
            get { return _entity as project.Entity.Base.EntityAutoAllocation; }
        }

        /// </summary>
        ///load 方法 pid主键
        /// </summary>
        public void load(string id)
        {
            DataRow dr = objdata.ExecuteDataSet("select * from Base_AutoAllocation where RowPointer='" + id + "'").Tables[0].Rows[0];
            _entity.RowPointer = dr["RowPointer"].ToString();
            _entity.AccID = dr["AccID"].ToString();
            _entity.OrderType = dr["OrderType"].ToString();
            _entity.RegionNo = dr["RegionNo"].ToString();
            _entity.DeptNo = dr["DeptNo"].ToString();
            _entity.UserNo = dr["UserNo"].ToString();
            _entity.UserName = dr["UserName"].ToString();
        }

        /// </summary>
        ///Save方法
        /// </summary>
        public int Save()
        {
            string sqlstr = "";
            if (Entity.RowPointer == null)
                sqlstr = "insert into Base_AutoAllocation(RowPointer,AccID,OrderType,RegionNo,DeptNo,UserNo,UserName)" +
                    "values(NEWID(),'" + Entity.AccID + "'," + "'" + Entity.OrderType + "'" + "," + "'" + Entity.RegionNo + "'" + "," +
                    "'" + Entity.DeptNo + "'" + "," + "'" + Entity.UserNo + "'" + "," + "'" + Entity.UserName + "'" + ")";
            else
                sqlstr = "update Base_AutoAllocation" +
                    " set OrderType=" + "'" + Entity.OrderType + "'" + "," + "RegionNo=" + "'" + Entity.RegionNo + "'" + "," +
                    "DeptNo=" + "'" + Entity.DeptNo + "'" + "," + "UserNo=" + "'" + Entity.UserNo + "'" + "," + "UserName=" + "'" + Entity.UserName + "'" +
                    " where RowPointer='" + Entity.RowPointer + "'" + " and AccID=" + "'" + Entity.AccID + "'";
            return objdata.ExecuteNonQuery(sqlstr);
        }

        /// </summary>
        ///Delete 方法 
        /// </summary>
        public int delete()
        {
            return objdata.ExecuteNonQuery("delete from Base_AutoAllocation where RowPointer='" + Entity.RowPointer + "' and AccID=" + "'" + Entity.AccID + "'");
        }

        /// <summary>
        /// 按条件查询，支持分页
        /// </summary>
        /// <param name="AccID">账套</param>
        /// <param name="OrderType">订单类型</param>
        /// <param name="RegionNo">区域编号</param>
        /// <param name="DeptNo">分配部门</param>
        /// <param name="UserNo">分配用户</param>
        /// <param name="startRow">第几页</param>
        /// <param name="pageSize">每页几行</param>
        /// <returns></returns>
        public System.Collections.ICollection GetDictListQuery(String AccID, String OrderType, String RegionNo, String DeptNo, String UserNo, int startRow, int pageSize)
        {
            if (startRow < 0 || pageSize <= 0)
            {
                throw new Exception();
            }

            return GetListHelper(AccID, OrderType, RegionNo, DeptNo, UserNo, startRow, pageSize);
        }

        /// <summary>
        /// 按条件查询，不支持分页
        /// </summary>
        /// <param name="AccID">账套</param>
        /// <param name="OrderType">订单类型</param>
        /// <param name="RegionNo">区域编号</param>
        /// <param name="DeptNo">分配部门</param>
        /// <param name="UserNo">分配用户</param>
        /// <returns></returns>
        public System.Collections.ICollection GetDictListQuery(String AccID, String OrderType, String RegionNo, String DeptNo, String UserNo)
        {
            return GetListHelper(AccID, OrderType, RegionNo, DeptNo, UserNo, START_ROW_INIT, START_ROW_INIT);
        }

        /// <summary>
        /// 返回集合的大小
        /// </summary>
        /// <param name="AccID">账套</param>
        /// <param name="OrderType">订单类型</param>
        /// <param name="RegionNo">区域编号</param>
        /// <param name="DeptNo">分配部门</param>
        /// <param name="UserNo">分配用户</param>
        /// <returns></returns>
        public int GetDictListCount(String AccID, String OrderType, String RegionNo, String DeptNo, String UserNo)
        {
            string wherestr = "";
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and AccID='" + AccID + "'";
            }
            if (OrderType != string.Empty)
            {
                wherestr = wherestr + " and OrderType = '" + OrderType + "'";
            }
            if (RegionNo != string.Empty)
            {
                wherestr = wherestr + " and RegionNo = '" + RegionNo + "'";
            }
            if (DeptNo != string.Empty)
            {
                wherestr = wherestr + " and DeptNo = '" + DeptNo + "'";
            }
            if (UserNo != string.Empty)
            {
                wherestr = wherestr + " and UserNo = '" + UserNo + "'";
            }

            string count = objdata.ExecuteDataSet("select count(*) as cnt from Base_AutoAllocation where 1=1 " + wherestr).Tables[0].Rows[0]["cnt"].ToString();
            return int.Parse(count);
        }

        /// <summary>
        /// 按条件查询，返回符合条件的集合
        /// </summary>
        /// <param name="AccID">账套</param>
        /// <param name="OrderType">订单类型</param>
        /// <param name="RegionNo">区域编号</param>
        /// <param name="DeptNo">分配部门</param>
        /// <param name="UserNo">分配用户</param>
        /// <returns></returns>
        private System.Collections.ICollection GetListHelper(String AccID, String OrderType, String RegionNo, String DeptNo, String UserNo, int startRow, int pageSize)
        {
            string wherestr = "";
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and AccID='" + AccID + "'";
            }
            if (OrderType != string.Empty)
            {
                wherestr = wherestr + " and OrderType = '" + OrderType + "'";
            }
            if (RegionNo != string.Empty)
            {
                wherestr = wherestr + " and RegionNo = '" + RegionNo + "'";
            }
            if (DeptNo != string.Empty)
            {
                wherestr = wherestr + " and DeptNo = '" + DeptNo + "'";
            }
            if (UserNo != string.Empty)
            {
                wherestr = wherestr + " and UserNo = '" + UserNo + "'";
            }

            System.Collections.IList entitys = null;
            if (startRow > START_ROW_INIT && pageSize > START_ROW_INIT)
            {
                entitys = Query(objdata.ExecSelect("Base_AutoAllocation", wherestr, startRow, pageSize, orderstr));
            }
            else
            {
                entitys = Query(objdata.ExecSelect("Base_AutoAllocation", wherestr, START_ROW_INIT, START_ROW_INIT, orderstr));
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
                project.Entity.Base.EntityAutoAllocation entity = new project.Entity.Base.EntityAutoAllocation(); 
                entity.RowPointer = dr["RowPointer"].ToString();
                entity.AccID = dr["AccID"].ToString();
                entity.OrderType = dr["OrderType"].ToString();
                entity.RegionNo = dr["RegionNo"].ToString();
                entity.DeptNo = dr["DeptNo"].ToString();
                entity.UserNo = dr["UserNo"].ToString();
                entity.UserName = dr["UserName"].ToString();
                result.Add(entity);
            }
            return result;
        }

    }
}
