using System;
using System.Data;
namespace project.Business.Base
{
    /// <summary>
    /// 工单操作的业务类
    /// </summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    public sealed class BusinessOperate : project.Business.AbstractPmBusiness
    {
        private project.Entity.Base.EntityOperate _entity = new project.Entity.Base.EntityOperate();
        public string orderstr = "OpNo";
        Data objdata = new Data();

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public BusinessOperate() { }

        /// <summary>
        /// 带参数的构函数
        /// </summary>
        /// <param name="entity">实体类</param>
        public BusinessOperate(project.Entity.Base.EntityOperate entity)
        {
            this._entity = entity;
        }

        /// <summary>
        /// 与实体类(EntityOperate)关联
        /// </summary>
        public project.Entity.Base.EntityOperate Entity
        {
            get { return _entity as project.Entity.Base.EntityOperate; }
        }

        /// </summary>
        ///load 方法 pid主键
        /// </summary>
        public void load(string OpNo, string accID)
        {
            DataRow dr = objdata.ExecuteDataSet("select * from Base_Operate where OpNo='" + OpNo + "' and AccID='" + accID + "'").Tables[0].Rows[0];
            _entity.OpNo = dr["OpNo"].ToString();
            _entity.OpName = dr["OpName"].ToString();
            _entity.AccID = dr["AccID"].ToString();
        }

        /// </summary>
        ///Save方法
        /// </summary>
        public int Save(string type)
        {
            string sqlstr = "";
            if (type == "insert")
                sqlstr = "insert into Base_Operate(OpNo,OpName,AccID)" +
                    "values('" + Entity.OpNo + "'," + "'" + Entity.OpName + "'" + "," + "'" + Entity.AccID + "'" + ")";
            else
                sqlstr = "update Base_Operate" +
                    " set OpName=" + "'" + Entity.OpName + "'" +
                    " where OpNo='" + Entity.OpNo + "'" + " and AccID=" + "'" + Entity.AccID + "'";
            return objdata.ExecuteNonQuery(sqlstr);
        }

        /// </summary>
        ///Delete 方法 
        /// </summary>
        public int delete()
        {
            return objdata.ExecuteNonQuery("delete from Base_Operate where OpNo='" + Entity.OpNo + "' and AccID=" + "'" + Entity.AccID + "'");
        }

        /// <summary>
        /// 按条件查询，支持分页
        /// </summary>
        /// <param name="OpNo">数据字典编号</param>
        /// <param name="OpName">数据字典名称</param>
        /// <param name="AccID">账套</param>
        /// <returns></returns>
        public System.Collections.ICollection GetOpListQuery(String OpNo, String OpName, String AccID, int startRow, int pageSize)
        {
            if (startRow < 0 || pageSize <= 0)
            {
                throw new Exception();
            }

            return GetListHelper(OpNo, OpName, AccID, startRow, pageSize);
        }

        /// <summary>
        /// 按条件查询，不支持分页
        /// </summary>
        /// <param name="OpNo">数据字典编号</param>
        /// <param name="OpName">数据字典名称</param>
        /// <param name="AccID">账套</param>
        /// <returns></returns>
        public System.Collections.ICollection GetOpListQuery(String OpNo, String OpName, String AccID)
        {
            return GetListHelper(OpNo, OpName, AccID, START_ROW_INIT, START_ROW_INIT);
        }

        /// <summary>
        /// 返回集合的大小
        /// </summary>
        /// <param name="OpNo">数据字典编号</param>
        /// <param name="OpName">数据字典名称</param>
        /// <param name="AccID">账套</param>
        /// <returns></returns>
        public int GetOpListCount(String OpNo, String OpName, String AccID)
        {
            string wherestr = "";
            if (OpNo != string.Empty)
            {
                wherestr = wherestr + " and OpNo like '%" + OpNo + "%'";
            }
            if (OpName != string.Empty)
            {
                wherestr = wherestr + " and OpName like '%" + OpName + "%'";
            }
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and AccID='" + AccID + "'";
            }

            string count = objdata.ExecuteDataSet("select count(*) as cnt from Base_Operate where 1=1 " + wherestr).Tables[0].Rows[0]["cnt"].ToString();
            return int.Parse(count);
        }

        /// <summary>
        /// 按条件查询，返回符合条件的集合
        /// </summary>
        /// <param name="OpNo">数据字典编号</param>
        /// <param name="OpName">数据字典名称</param>
        /// <param name="AccID">账套</param>
        /// <returns></returns>
        private System.Collections.ICollection GetListHelper(String OpNo, String OpName, String AccID, int startRow, int pageSize)
        {
            string wherestr = "";
            if (OpNo != string.Empty)
            {
                wherestr = wherestr + " and OpNo like '%" + OpNo + "%'";
            }
            if (OpName != string.Empty)
            {
                wherestr = wherestr + " and OpName like '%" + OpName + "%'";
            }
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and AccID='" + AccID + "'";
            }

            System.Collections.IList entitys = null;
            if (startRow > START_ROW_INIT && pageSize > START_ROW_INIT)
            {
                entitys = Query(objdata.ExecSelect("Base_Operate", wherestr, startRow, pageSize, orderstr));
            }
            else
            {
                entitys = Query(objdata.ExecSelect("Base_Operate", wherestr, START_ROW_INIT, START_ROW_INIT, orderstr));
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
                project.Entity.Base.EntityOperate entity = new project.Entity.Base.EntityOperate();
                entity.OpNo = dr["OpNo"].ToString();
                entity.OpName = dr["OpName"].ToString();
                entity.AccID = dr["AccID"].ToString();
                result.Add(entity);
            }
            return result;
        }

    }
}
