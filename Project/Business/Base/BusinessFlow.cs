using System;
using System.Data;
namespace project.Business.Base
{
    /// <summary>
    /// 工单流程表的业务类
    /// </summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    public sealed class BusinessFlow : project.Business.AbstractPmBusiness
    {
        private project.Entity.Base.EntityFlow _entity = new project.Entity.Base.EntityFlow();
        public string orderstr = "OrdNo";
        Data objdata = new Data();

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public BusinessFlow() { }

        /// <summary>
        /// 带参数的构函数
        /// </summary>
        /// <param name="entity">实体类</param>
        public BusinessFlow(project.Entity.Base.EntityFlow entity)
        {
            this._entity = entity;
        }

        /// <summary>
        /// 与实体类(EntityFlow)关联
        /// </summary>
        public project.Entity.Base.EntityFlow Entity
        {
            get { return _entity as project.Entity.Base.EntityFlow; }
        }

        /// </summary>
        ///load 方法 pid主键
        /// </summary>
        public void load(string FlowNo, string AccID)
        {
            DataRow dr = objdata.ExecuteDataSet("select * from Base_Flow where FlowNo='" + FlowNo + "' and AccID='" + AccID + "'").Tables[0].Rows[0];
            _entity.FlowNo = dr["FlowNo"].ToString();
            _entity.FlowName = dr["FlowName"].ToString();
            _entity.AccID = dr["AccID"].ToString();
            _entity.OrdNo = dr["OrdNo"].ToString();
            _entity.Remark = dr["Remark"].ToString();
        }

        /// </summary>
        ///Save方法
        /// </summary>
        public int Save(string type)
        {
            string sqlstr = "";
            if (type == "insert")
                sqlstr = "insert into Base_Flow(FlowNo,FlowName,AccID,OrdNo,Remark)" +
                    "values('" + Entity.FlowNo + "'," + "'" + Entity.FlowName + "'" + "," + "'" + Entity.AccID + "'" + "," +
                    "'" + Entity.OrdNo + "'" + "," + "'" + Entity.Remark + "'" + ")";
            else
                sqlstr = "update Base_Flow" +
                    " set FlowName=" + "'" + Entity.FlowName + "'" + "," +
                    "OrdNo=" + "'" + Entity.OrdNo + "'" + "," +
                    "Remark=" + "'" + Entity.Remark + "'" +
                    " where FlowNo='" + Entity.FlowNo + "'";
            return objdata.ExecuteNonQuery(sqlstr);
        }

        /// </summary>
        ///Delete 方法 
        /// </summary>
        public int delete()
        {
            return objdata.ExecuteNonQuery("delete from Base_Flow where FlowNo='" + Entity.FlowNo + "' and AccID='" + Entity.AccID + "'");
        }

        /// <summary>
        /// 按条件查询，支持分页
        /// </summary>
        /// <param name="FlowNo">工单流程编号</param>
        /// <param name="FlowName">工单流程名称</param>
        /// <param name="AccID">账套</param>
        /// <returns></returns>
        public System.Collections.ICollection GetFlowListQuery(String FlowNo, String FlowName, String AccID, int startRow, int pageSize)
        {
            if (startRow < 0 || pageSize <= 0)
            {
                throw new Exception();
            }

            return GetListHelper(FlowNo, FlowName, AccID, startRow, pageSize);
        }

        /// <summary>
        /// 按条件查询，不支持分页
        /// </summary>
        /// <param name="FlowNo">工单流程编号</param>
        /// <param name="FlowName">工单流程名称</param>
        /// <param name="AccID">账套</param>
        /// <returns></returns>
        public System.Collections.ICollection GetFlowListQuery(String FlowNo, String FlowName, String AccID)
        {
            return GetListHelper(FlowNo, FlowName, AccID, START_ROW_INIT, START_ROW_INIT);
        }

        /// <summary>
        /// 返回集合的大小
        /// </summary>
        /// <param name="FlowNo">工单流程编号</param>
        /// <param name="FlowName">工单流程名称</param>
        /// <param name="AccID">账套</param>
        /// <returns></returns>
        public int GetFlowListCount(String FlowNo, String FlowName, String AccID)
        {
            string wherestr = "";
            if (FlowNo != string.Empty)
            {
                wherestr = wherestr + " and FlowNo like '%" + FlowNo + "%'";
            }
            if (FlowName != string.Empty)
            {
                wherestr = wherestr + " and FlowName like '%" + FlowName + "%'";
            }
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and AccID like '" + AccID + "'";
            }
            
            string count = objdata.ExecuteDataSet("select count(*) as cnt from Base_Flow where 1=1 " + wherestr).Tables[0].Rows[0]["cnt"].ToString();
            return int.Parse(count);
        }

        /// <summary>
        /// 按条件查询，返回符合条件的集合
        /// </summary>
        /// <param name="FlowNo">工单流程编号</param>
        /// <param name="FlowName">工单流程名称</param>
        /// <param name="AccID">账套</param>
        /// <returns></returns>
        private System.Collections.ICollection GetListHelper(String FlowNo, String FlowName, String AccID, int startRow, int pageSize)
        {
            string wherestr = "";
            if (FlowNo != string.Empty)
            {
                wherestr = wherestr + " and FlowNo like '%" + FlowNo + "%'";
            }
            if (FlowName != string.Empty)
            {
                wherestr = wherestr + " and FlowName like '%" + FlowName + "%'";
            }
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and AccID like '" + AccID + "'";
            }

            System.Collections.IList entitys = null;
            if (startRow > START_ROW_INIT && pageSize > START_ROW_INIT)
            {
                entitys = Query(objdata.ExecSelect("Base_Flow", wherestr, startRow, pageSize, orderstr));
            }
            else
            {
                entitys = Query(objdata.ExecSelect("Base_Flow", wherestr, START_ROW_INIT, START_ROW_INIT, orderstr));
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
                project.Entity.Base.EntityFlow entity = new project.Entity.Base.EntityFlow();

                entity.FlowNo = dr["FlowNo"].ToString();
                entity.FlowName = dr["FlowName"].ToString();
                entity.AccID = dr["AccID"].ToString();
                entity.OrdNo = dr["OrdNo"].ToString();
                entity.Remark = dr["Remark"].ToString();
                result.Add(entity);
            }
            return result;
        }

    }
}
