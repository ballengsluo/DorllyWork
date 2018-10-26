using System;
using System.Data;
namespace project.Business.Base
{
    /// <summary>
    /// 工单流程表的业务类
    /// </summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    public sealed class BusinessWarningTime : project.Business.AbstractPmBusiness
    {
        private project.Entity.Base.EntityWarningTime _entity = new project.Entity.Base.EntityWarningTime();
        public string orderstr = "OrdNo";
        Data objdata = new Data();

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public BusinessWarningTime() { }

        /// <summary>
        /// 带参数的构函数
        /// </summary>
        /// <param name="entity">实体类</param>
        public BusinessWarningTime(project.Entity.Base.EntityWarningTime entity)
        {
            this._entity = entity;
        }

        /// <summary>
        /// 与实体类(EntityWarningTime)关联
        /// </summary>
        public project.Entity.Base.EntityWarningTime Entity
        {
            get { return _entity as project.Entity.Base.EntityWarningTime; }
        }

        /// </summary>
        ///load 方法 pid主键
        /// </summary>
        public void load(string id, string AccID)
        {
            DataRow dr = objdata.ExecuteDataSet("select * from Base_WarningTime where RowPointer='" + id + "' and AccID='" + AccID + "'").Tables[0].Rows[0];
            _entity.InnerEntityOID = dr["RowPointer"].ToString();
            _entity.AccID = dr["AccID"].ToString();
            _entity.ParaNo = dr["ParaNo"].ToString();
            _entity.ParaName = dr["ParaName"].ToString();
            _entity.Time = int.Parse(dr["Time"].ToString());
        }

        /// </summary>
        ///load 方法 pid主键
        /// </summary>
        public void loadParaNo(string ParaNo, string AccID)
        {
            DataRow dr = objdata.ExecuteDataSet("select * from Base_WarningTime where ParaNo='" + ParaNo + "' and AccID='" + AccID + "'").Tables[0].Rows[0];
            _entity.InnerEntityOID = dr["RowPointer"].ToString();
            _entity.AccID = dr["AccID"].ToString();
            _entity.ParaNo = dr["ParaNo"].ToString();
            _entity.ParaName = dr["ParaName"].ToString();
            _entity.Time = int.Parse(dr["Time"].ToString());
        }


        /// </summary>
        ///Save方法
        /// </summary>
        public int Save()
        {
            string sqlstr = "";
            if (Entity.InnerEntityOID == null)
                sqlstr = "insert into Base_WarningTime(RowPointer,AccID,ParaNo,ParaName,Time)" +
                    "values(NEWID()," + "'" + Entity.AccID + "'" + "," +
                    "'" + Entity.ParaNo + "'" + "," + "'" + Entity.ParaName + "'" + "," + Entity.Time + ")";
            else
                sqlstr = "update Base_WarningTime" +
                    " set ParaNo=" + "'" + Entity.ParaNo + "'" + "," +
                    "ParaName=" + "'" + Entity.ParaName + "'" + "," +
                    "Time=" + Entity.Time + 
                    " where RowPointer='" + Entity.InnerEntityOID + "'";
            return objdata.ExecuteNonQuery(sqlstr);
        }

        /// </summary>
        ///Delete 方法 
        /// </summary>
        public int delete()
        {
            return objdata.ExecuteNonQuery("delete from Base_WarningTime where RowPointer='" + Entity.InnerEntityOID + "'");
        }

        /// <summary>
        /// 按条件查询，支持分页
        /// </summary>
        /// <param name="AccID">账套</param>
        /// <returns></returns>
        public System.Collections.ICollection GetWarningTimeListQuery(String AccID, int startRow, int pageSize)
        {
            if (startRow < 0 || pageSize <= 0)
            {
                throw new Exception();
            }

            return GetListHelper(AccID, startRow, pageSize);
        }

        /// <summary>
        /// 按条件查询，不支持分页
        /// </summary>
        /// <param name="AccID">账套</param>
        /// <returns></returns>
        public System.Collections.ICollection GetWarningTimeListQuery(String AccID)
        {
            return GetListHelper( AccID, START_ROW_INIT, START_ROW_INIT);
        }

        /// <summary>
        /// 返回集合的大小
        /// </summary>
        /// <param name="AccID">账套</param>
        /// <returns></returns>
        public int GetWarningTimeListCount(String AccID)
        {
            string wherestr = "";
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and AccID like '" + AccID + "'";
            }

            string count = objdata.ExecuteDataSet("select count(*) as cnt from Base_WarningTime where 1=1 " + wherestr).Tables[0].Rows[0]["cnt"].ToString();
            return int.Parse(count);
        }

        /// <summary>
        /// 按条件查询，返回符合条件的集合
        /// </summary>
        /// <param name="AccID">账套</param>
        /// <returns></returns>
        private System.Collections.ICollection GetListHelper(String AccID, int startRow, int pageSize)
        {
            string wherestr = "";
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and AccID like '" + AccID + "'";
            }

            System.Collections.IList entitys = null;
            if (startRow > START_ROW_INIT && pageSize > START_ROW_INIT)
            {
                entitys = Query(objdata.ExecSelect("Base_WarningTime", wherestr, startRow, pageSize, orderstr));
            }
            else
            {
                entitys = Query(objdata.ExecSelect("Base_WarningTime", wherestr, START_ROW_INIT, START_ROW_INIT, orderstr));
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
                project.Entity.Base.EntityWarningTime entity = new project.Entity.Base.EntityWarningTime();
                entity.InnerEntityOID = dr["RowPointer"].ToString();
                entity.AccID = dr["AccID"].ToString();
                entity.ParaNo = dr["ParaNo"].ToString();
                entity.ParaName = dr["ParaName"].ToString();
                entity.Time = int.Parse(dr["Time"].ToString());
                result.Add(entity);
            }
            return result;
        }

    }
}
