using System;
using System.Data;
namespace project.Business.Sys
{
    /// <summary>
    /// 部门信息的业务类
    /// </summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    public sealed class BusinessDept : project.Business.AbstractPmBusiness
    {
        private project.Entity.Sys.EntityDept _entity = new project.Entity.Sys.EntityDept();
        public string Userstr = "DeptNo";
        Data objdata = new Data();

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public BusinessDept() { }

        /// <summary>
        /// 带参数的构函数
        /// </summary>
        /// <param name="entity">实体类</param>
        public BusinessDept(project.Entity.Sys.EntityDept entity)
        {
            this._entity = entity;
        }

        /// <summary>
        /// 与实体类(EntityDept)关联
        /// </summary>
        public project.Entity.Sys.EntityDept Entity
        {
            get { return _entity as project.Entity.Sys.EntityDept; }
        }

        /// </summary>
        ///load 方法 pid主键
        /// </summary>
        public void load(string DeptNo, string AccID)
        {
            DataRow dr = objdata.ExecuteDataSet("select * from Sys_Dept where DeptNo='" + DeptNo + "' and AccID='" + AccID + "'").Tables[0].Rows[0];
            _entity.DeptNo = dr["DeptNo"].ToString();
            _entity.DeptName = dr["DeptName"].ToString();
            _entity.Parent = dr["Parent"].ToString();
            _entity.Manager = dr["Manager"].ToString();
            _entity.Remark = dr["Remark"].ToString();
            _entity.AccID = dr["AccID"].ToString();
            _entity.Level = ParseIntForString(dr["Level"].ToString());
        }

        /// </summary>
        ///Save方法
        /// </summary>
        public int Save(string type)
        {
            string sqlstr = "";
            if (type == "insert")
                sqlstr = "insert into Sys_Dept(DeptNo,DeptName,AccID,Parent,Manager,Remark,Level)" +
                    "values('" + Entity.DeptNo + "'," + "'" + Entity.DeptName + "'" + "," + "'" + Entity.AccID + "'" + "," +
                    "'" + Entity.Parent + "'" + "," + "'" + Entity.Manager + "'" + "," + "'" + Entity.Remark + "'" + "," + Entity.Level + ")";
            else
                sqlstr = "update Sys_Dept" +
                    " set DeptName=" + "'" + Entity.DeptName + "'" + "," + "Parent=" + "'" + Entity.Parent + "'" + "," +
                    "Manager=" + "'" + Entity.Manager + "'" + "," + "Remark=" + "'" + Entity.Remark + "'" + "," + "Level=" + Entity.Level +
                    " where DeptNo='" + Entity.DeptNo + "'";
            return objdata.ExecuteNonQuery(sqlstr);
        }

        /// </summary>
        ///Delete 方法 
        /// </summary>
        public int delete()
        {
            return objdata.ExecuteNonQuery("delete from Sys_Dept where DeptNo='" + Entity.DeptNo + "' and AccID='" + Entity.AccID + "'");
        }

        /// <summary>
        /// 按条件查询，支持分页
        /// </summary>
        /// <param name="DeptNo">用户类型编号</param>
        /// <param name="DeptName">用户类型名称</param>
        /// <param name="AccID">账户</param>
        /// <param name="Parent">上级部门</param>
        /// <returns></returns>
        public System.Collections.ICollection GetDeptListQuery(String DeptNo, String DeptName, String AccID, String Parent, int startRow, int pageSize)
        {
            if (startRow < 0 || pageSize <= 0)
            {
                throw new Exception();
            }

            return GetListHelper(DeptNo, DeptName, AccID, Parent, startRow, pageSize);
        }

        /// <summary>
        /// 按条件查询，不支持分页
        /// </summary>
        /// <param name="DeptNo">用户类型编号</param>
        /// <param name="DeptName">用户类型名称</param>
        /// <param name="AccID">账户</param>
        /// <param name="Parent">上级部门</param>
        /// <returns></returns>
        public System.Collections.ICollection GetDeptListQuery(String DeptNo, String DeptName, String AccID, String Parent)
        {
            return GetListHelper(DeptNo, DeptName, AccID, Parent, START_ROW_INIT, START_ROW_INIT);
        }

        /// <summary>
        /// 返回集合的大小
        /// </summary>
        /// <param name="DeptNo">用户类型编号</param>
        /// <param name="DeptName">用户类型名称</param>
        /// <param name="AccID">账户</param>
        /// <param name="Parent">上级部门</param>
        /// <returns></returns>
        public int GetDeptListCount(String DeptNo, String DeptName, String AccID, String Parent)
        {
            string wherestr = "";
            if (DeptNo != string.Empty)
            {
                wherestr = wherestr + " and DeptNo like '%" + DeptNo + "%'";
            }
            if (DeptName != string.Empty)
            {
                wherestr = wherestr + " and DeptName like '%" + DeptName + "%'";
            }
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and AccID like '" + AccID + "'";
            }
            if (Parent != string.Empty)
            {
                if (Parent == "null")
                    wherestr = wherestr + " and isnull(Parent,'')=''";
                else
                    wherestr = wherestr + " and Parent like '" + Parent + "'";
            }

            string count = objdata.ExecuteDataSet("select count(*) as cnt from Sys_Dept where 1=1 " + wherestr).Tables[0].Rows[0]["cnt"].ToString();
            return int.Parse(count);
        }

        /// <summary>
        /// 按条件查询，返回符合条件的集合
        /// </summary>
        /// <param name="DeptNo">用户类型编号</param>
        /// <param name="DeptName">用户类型名称</param>
        /// <param name="AccID">账户</param>
        /// <param name="Parent">上级部门</param>
        /// <returns></returns>
        private System.Collections.ICollection GetListHelper(String DeptNo, String DeptName, String AccID, String Parent, int startRow, int pageSize)
        {
            string wherestr = "";
            if (DeptNo != string.Empty)
            {
                wherestr = wherestr + " and DeptNo like '%" + DeptNo + "%'";
            }
            if (DeptName != string.Empty)
            {
                wherestr = wherestr + " and DeptName like '%" + DeptName + "%'";
            }
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and AccID like '" + AccID + "'";
            }
            if (Parent != string.Empty)
            {
                if (Parent == "null")
                    wherestr = wherestr + " and isnull(Parent,'')=''";
                else
                    wherestr = wherestr + " and Parent like '" + Parent + "'";
            }

            System.Collections.IList entitys = null;
            if (startRow > START_ROW_INIT && pageSize > START_ROW_INIT)
            {
                entitys = Query(objdata.ExecSelect("Sys_Dept", wherestr, startRow, pageSize, Userstr));
            }
            else
            {
                entitys = Query(objdata.ExecSelect("Sys_Dept", wherestr, START_ROW_INIT, START_ROW_INIT, Userstr));
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
                project.Entity.Sys.EntityDept entity = new project.Entity.Sys.EntityDept();

                entity.DeptNo = dr["DeptNo"].ToString();
                entity.DeptName = dr["DeptName"].ToString();
                entity.Parent = dr["Parent"].ToString();
                entity.Manager = dr["Manager"].ToString();
                entity.Remark = dr["Remark"].ToString();
                entity.AccID = dr["AccID"].ToString();
                entity.Level = ParseIntForString(dr["Level"].ToString()); ;
                result.Add(entity);
            }
            return result;
        }

    }
}
