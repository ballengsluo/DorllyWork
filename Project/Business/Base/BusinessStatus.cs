using System;
using System.Data;
namespace project.Business.Base
{
    /// <summary>
    /// 工单状态的业务类
    /// </summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    public sealed class BusinessStatus : project.Business.AbstractPmBusiness
    {
        private project.Entity.Base.EntityStatus _entity = new project.Entity.Base.EntityStatus();
        public string orderstr = "OrdNo";
        Data objdata = new Data();

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public BusinessStatus() { }

        /// <summary>
        /// 带参数的构函数
        /// </summary>
        /// <param name="entity">实体类</param>
        public BusinessStatus(project.Entity.Base.EntityStatus entity)
        {
            this._entity = entity;
        }

        /// <summary>
        /// 与实体类(EntityStatus)关联
        /// </summary>
        public project.Entity.Base.EntityStatus Entity
        {
            get { return _entity as project.Entity.Base.EntityStatus; }
        }

        /// </summary>
        ///load 方法 pid主键
        /// </summary>
        public void load(string StatusNo, string AccID)
        {
            DataRow dr = objdata.ExecuteDataSet("select * from Base_Status where StatusNo='" + StatusNo + "' and AccID='" + AccID + "'").Tables[0].Rows[0];
            _entity.StatusNo = dr["StatusNo"].ToString();
            _entity.StatusName = dr["StatusName"].ToString();
            _entity.AccID = dr["AccID"].ToString();
            _entity.OrdNo = dr["OrdNo"].ToString();
            _entity.NodeNo = dr["NodeNo"].ToString();
        }

        /// </summary>
        ///Save方法
        /// </summary>
        public int Save(string type)
        {
            string sqlstr = "";
            if (type == "insert")
                sqlstr = "insert into Base_Status(StatusNo,StatusName,AccID,OrdNo,NodeNo)" +
                    "values('" + Entity.StatusNo + "'," + "'" + Entity.StatusName + "'" + "," + "'" + Entity.AccID + "'" + "," +
                    "'" + Entity.OrdNo + "'" + "," + "'" + Entity.NodeNo + "'" + ")";
            else
                sqlstr = "update Base_Status" +
                    " set StatusName=" + "'" + Entity.StatusName + "'" + "," +
                    "OrdNo=" + "'" + Entity.OrdNo + "'" + "," +
                    "NodeNo=" + "'" + Entity.NodeNo + "'" +
                    " where StatusNo='" + Entity.StatusNo + "'";
            return objdata.ExecuteNonQuery(sqlstr);
        }

        /// </summary>
        ///Delete 方法 
        /// </summary>
        public int delete()
        {
            return objdata.ExecuteNonQuery("delete from Base_Status where StatusNo='" + Entity.StatusNo + "' and AccID='" + Entity.AccID + "'");
        }

        /// <summary>
        /// 按条件查询，支持分页
        /// </summary>
        /// <param name="StatusNo">工单流程编号</param>
        /// <param name="StatusName">工单流程名称</param>
        /// <param name="AccID">账套</param>
        /// <returns></returns>
        public System.Collections.ICollection GetStatusListQuery(String StatusNo, String StatusName, String AccID, int startRow, int pageSize)
        {
            if (startRow < 0 || pageSize <= 0)
            {
                throw new Exception();
            }

            return GetListHelper(StatusNo, StatusName, AccID, startRow, pageSize);
        }

        /// <summary>
        /// 按条件查询，不支持分页
        /// </summary>
        /// <param name="StatusNo">工单流程编号</param>
        /// <param name="StatusName">工单流程名称</param>
        /// <param name="AccID">账套</param>
        /// <returns></returns>
        public System.Collections.ICollection GetStatusListQuery(String StatusNo, String StatusName, String AccID)
        {
            return GetListHelper(StatusNo, StatusName, AccID, START_ROW_INIT, START_ROW_INIT);
        }

        /// <summary>
        /// 返回集合的大小
        /// </summary>
        /// <param name="StatusNo">工单流程编号</param>
        /// <param name="StatusName">工单流程名称</param>
        /// <param name="AccID">账套</param>
        /// <returns></returns>
        public int GetStatusListCount(String StatusNo, String StatusName, String AccID)
        {
            string wherestr = "";
            if (StatusNo != string.Empty)
            {
                wherestr = wherestr + " and StatusNo like '%" + StatusNo + "%'";
            }
            if (StatusName != string.Empty)
            {
                wherestr = wherestr + " and StatusName like '%" + StatusName + "%'";
            }
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and AccID like '" + AccID + "'";
            }

            string count = objdata.ExecuteDataSet("select count(*) as cnt from Base_Status where 1=1 " + wherestr).Tables[0].Rows[0]["cnt"].ToString();
            return int.Parse(count);
        }

        /// <summary>
        /// 按条件查询，返回符合条件的集合
        /// </summary>
        /// <param name="StatusNo">工单流程编号</param>
        /// <param name="StatusName">工单流程名称</param>
        /// <param name="AccID">账套</param>
        /// <returns></returns>
        private System.Collections.ICollection GetListHelper(String StatusNo, String StatusName, String AccID, int startRow, int pageSize)
        {
            string wherestr = "";
            if (StatusNo != string.Empty)
            {
                wherestr = wherestr + " and StatusNo like '%" + StatusNo + "%'";
            }
            if (StatusName != string.Empty)
            {
                wherestr = wherestr + " and StatusName like '%" + StatusName + "%'";
            }
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and AccID like '" + AccID + "'";
            }

            System.Collections.IList entitys = null;
            if (startRow > START_ROW_INIT && pageSize > START_ROW_INIT)
            {
                entitys = Query(objdata.ExecSelect("Base_Status", wherestr, startRow, pageSize, orderstr));
            }
            else
            {
                entitys = Query(objdata.ExecSelect("Base_Status", wherestr, START_ROW_INIT, START_ROW_INIT, orderstr));
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
                project.Entity.Base.EntityStatus entity = new project.Entity.Base.EntityStatus();

                entity.StatusNo = dr["StatusNo"].ToString();
                entity.StatusName = dr["StatusName"].ToString();
                entity.AccID = dr["AccID"].ToString();
                entity.OrdNo = dr["OrdNo"].ToString();
                entity.NodeNo = dr["NodeNo"].ToString();
                result.Add(entity);
            }
            return result;
        }

    }
}
