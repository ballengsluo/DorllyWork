using System;
using System.Data;
namespace project.Business.Base
{
    /// <summary>
    /// 地区信息的业务类
    /// </summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    public sealed class BusinessRegion : project.Business.AbstractPmBusiness
    {
        private project.Entity.Base.EntityRegion _entity = new project.Entity.Base.EntityRegion();
        public string Userstr = "RegionNo";
        Data objdata = new Data();

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public BusinessRegion() { }

        /// <summary>
        /// 带参数的构函数
        /// </summary>
        /// <param name="entity">实体类</param>
        public BusinessRegion(project.Entity.Base.EntityRegion entity)
        {
            this._entity = entity;
        }

        /// <summary>
        /// 与实体类(EntityRegion)关联
        /// </summary>
        public project.Entity.Base.EntityRegion Entity
        {
            get { return _entity as project.Entity.Base.EntityRegion; }
        }

        /// </summary>
        ///load 方法 pid主键
        /// </summary>
        public void load(string RegionNo, string AccID)
        {
            DataRow dr = objdata.ExecuteDataSet("select * from Base_Region where RegionNo='" + RegionNo + "' and AccID='" + AccID + "'").Tables[0].Rows[0];
            _entity.RegionNo = dr["RegionNo"].ToString();
            _entity.RegionName = dr["RegionName"].ToString();
            _entity.Parent = dr["Parent"].ToString();
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
                sqlstr = "insert into Base_Region(RegionNo,RegionName,AccID,Parent,Level)" +
                    "values('" + Entity.RegionNo + "'," + "'" + Entity.RegionName + "'" + "," + "'" + Entity.AccID + "'" + "," +
                    "'" + Entity.Parent + "'" + "," + Entity.Level + ")";
            else
                sqlstr = "update Base_Region" +
                    " set RegionName=" + "'" + Entity.RegionName + "'" + "," + "Parent=" + "'" + Entity.Parent + "'" + "," + "Level=" + Entity.Level +
                    " where RegionNo='" + Entity.RegionNo + "'";
            return objdata.ExecuteNonQuery(sqlstr);
        }

        /// </summary>
        ///Delete 方法 
        /// </summary>
        public int delete()
        {
            return objdata.ExecuteNonQuery("delete from Base_Region where RegionNo='" + Entity.RegionNo + "' and AccID='" + Entity.AccID + "'");
        }

        /// <summary>
        /// 按条件查询，支持分页
        /// </summary>
        /// <param name="RegionNo">用户类型编号</param>
        /// <param name="RegionName">用户类型名称</param>
        /// <param name="AccID">账户</param>
        /// <param name="Parent">上级地区</param>
        /// <returns></returns>
        public System.Collections.ICollection GetRegionListQuery(String RegionNo, String RegionName, String AccID, String Parent, int startRow, int pageSize)
        {
            if (startRow < 0 || pageSize <= 0)
            {
                throw new Exception();
            }

            return GetListHelper(RegionNo, RegionName, AccID, Parent, startRow, pageSize);
        }

        /// <summary>
        /// 按条件查询，不支持分页
        /// </summary>
        /// <param name="RegionNo">用户类型编号</param>
        /// <param name="RegionName">用户类型名称</param>
        /// <param name="AccID">账户</param>
        /// <param name="Parent">上级地区</param>
        /// <returns></returns>
        public System.Collections.ICollection GetRegionListQuery(String RegionNo, String RegionName, String AccID, String Parent)
        {
            return GetListHelper(RegionNo, RegionName, AccID, Parent, START_ROW_INIT, START_ROW_INIT);
        }

        /// <summary>
        /// 返回集合的大小
        /// </summary>
        /// <param name="RegionNo">用户类型编号</param>
        /// <param name="RegionName">用户类型名称</param>
        /// <param name="AccID">账户</param>
        /// <param name="Parent">上级地区</param>
        /// <returns></returns>
        public int GetRegionListCount(String RegionNo, String RegionName, String AccID, String Parent)
        {
            string wherestr = "";
            if (RegionNo != string.Empty)
            {
                wherestr = wherestr + " and RegionNo like '%" + RegionNo + "%'";
            }
            if (RegionName != string.Empty)
            {
                wherestr = wherestr + " and RegionName like '%" + RegionName + "%'";
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

            string count = objdata.ExecuteDataSet("select count(*) as cnt from Base_Region where 1=1 " + wherestr).Tables[0].Rows[0]["cnt"].ToString();
            return int.Parse(count);
        }

        /// <summary>
        /// 按条件查询，返回符合条件的集合
        /// </summary>
        /// <param name="RegionNo">用户类型编号</param>
        /// <param name="RegionName">用户类型名称</param>
        /// <param name="AccID">账户</param>
        /// <param name="Parent">上级地区</param>
        /// <returns></returns>
        private System.Collections.ICollection GetListHelper(String RegionNo, String RegionName, String AccID, String Parent, int startRow, int pageSize)
        {
            string wherestr = "";
            if (RegionNo != string.Empty)
            {
                wherestr = wherestr + " and RegionNo like '%" + RegionNo + "%'";
            }
            if (RegionName != string.Empty)
            {
                wherestr = wherestr + " and RegionName like '%" + RegionName + "%'";
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
                entitys = Query(objdata.ExecSelect("Base_Region", wherestr, startRow, pageSize, Userstr));
            }
            else
            {
                entitys = Query(objdata.ExecSelect("Base_Region", wherestr, START_ROW_INIT, START_ROW_INIT, Userstr));
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
                project.Entity.Base.EntityRegion entity = new project.Entity.Base.EntityRegion();

                entity.RegionNo = dr["RegionNo"].ToString();
                entity.RegionName = dr["RegionName"].ToString();
                entity.Parent = dr["Parent"].ToString();
                entity.AccID = dr["AccID"].ToString();
                entity.Level = ParseIntForString(dr["Level"].ToString()); ;
                result.Add(entity);
            }
            return result;
        }

    }
}
