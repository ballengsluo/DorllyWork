using System;
using System.Data;
namespace project.Business.Base
{
    /// <summary>
    /// 工单流程明细的业务类
    /// </summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    public sealed class BusinessFlowDetail : project.Business.AbstractPmBusiness
    {
        private project.Entity.Base.EntityFlowDetail _entity = new project.Entity.Base.EntityFlowDetail();
        public string orderstr = "a.NodeNo";
        Data objdata = new Data();

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public BusinessFlowDetail() { }

        /// <summary>
        /// 带参数的构函数
        /// </summary>
        /// <param name="entity">实体类</param>
        public BusinessFlowDetail(project.Entity.Base.EntityFlowDetail entity)
        {
            this._entity = entity;
        }

        /// <summary>
        /// 与实体类(EntityFlowDetail)关联
        /// </summary>
        public project.Entity.Base.EntityFlowDetail Entity
        {
            get { return _entity as project.Entity.Base.EntityFlowDetail; }
        }

        /// </summary>
        ///load 方法 pid主键
        /// </summary>
        public void load(string id, string AccID)
        {
            DataRow dr = objdata.ExecuteDataSet("select a.*,b.NodeName from Base_Flow_Detail a left join (select * from Base_Flow_Node where AccID='" + AccID + "') b on a.NodeNo=b.NodeNo where a.RowPointer='" + id + "' and a.AccID='" + AccID + "'").Tables[0].Rows[0];
            _entity.InnerEntityOID = dr["RowPointer"].ToString();
            _entity.NodeNo = dr["NodeNo"].ToString();
            _entity.NodeName = dr["NodeName"].ToString();
            _entity.AccID = dr["AccID"].ToString();
            _entity.FlowNo = dr["FlowNo"].ToString();
        }

        /// </summary>
        ///Save方法
        /// </summary>
        public int Save()
        {
            string sqlstr = "";
            if (Entity.InnerEntityOID == null)
                sqlstr = "insert into Base_Flow_Detail(RowPointer,NodeNo,AccID,FlowNo)" +
                    "values(NEWID(),'" + Entity.NodeNo + "'," + "'" + Entity.AccID + "'" + "," + "'" + Entity.FlowNo + "'" + ")";
            else
                sqlstr = "update Base_Flow_Detail" +
                    " set NodeNo=" + "'" + Entity.NodeNo + "'" +
                    " where NodeNo='" + Entity.NodeNo + "'";
            return objdata.ExecuteNonQuery(sqlstr);
        }

        /// </summary>
        ///Delete 方法 
        /// </summary>
        public int delete()
        {
            return objdata.ExecuteNonQuery("delete from Base_Flow_Detail where RowPointer='" + Entity.InnerEntityOID + "' and AccID='" + Entity.AccID + "'");
        }

        /// <summary>
        /// 按条件查询，支持分页
        /// </summary>
        /// <param name="FlowNo">流程编号</param>
        /// <param name="AccID">账套</param>
        /// <returns></returns>
        public System.Collections.ICollection GetFlowDetailListQuery(String FlowNo, String AccID, int startRow, int pageSize)
        {
            if (startRow < 0 || pageSize <= 0)
            {
                throw new Exception();
            }

            return GetListHelper(FlowNo, AccID, startRow, pageSize);
        }

        /// <summary>
        /// 按条件查询，不支持分页
        /// </summary>
        /// <param name="FlowNo">流程编号</param>
        /// <param name="AccID">账套</param>
        /// <returns></returns>
        public System.Collections.ICollection GetFlowDetailListQuery(String FlowNo, String AccID)
        {
            return GetListHelper(FlowNo, AccID, START_ROW_INIT, START_ROW_INIT);
        }

        /// <summary>
        /// 返回集合的大小
        /// </summary>
        /// <param name="FlowNo">流程编号</param>
        /// <param name="AccID">账套</param>
        /// <returns></returns>
        public int GetFlowDetailListCount(String FlowNo, String AccID)
        {
            string wherestr = "";
            if (FlowNo != string.Empty)
            {
                wherestr = wherestr + " and a.FlowNo like '" + FlowNo + "'";
            }
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and a.AccID like '" + AccID + "'";
            }

            string count = objdata.ExecuteDataSet("select count(*) as cnt from Base_Flow_Detail a where 1=1 " + wherestr).Tables[0].Rows[0]["cnt"].ToString();
            return int.Parse(count);
        }

        /// <summary>
        /// 按条件查询，返回符合条件的集合
        /// </summary>
        /// <param name="FlowNo">流程编号</param>
        /// <param name="AccID">账套</param>
        /// <returns></returns>
        private System.Collections.ICollection GetListHelper(String FlowNo, String AccID, int startRow, int pageSize)
        {
            string wherestr = "";
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
                entitys = Query(objdata.ExecSelect("Base_Flow_Detail a left join (select * from Base_Flow_Node where AccID='" + AccID + "') b on a.NodeNo=b.NodeNo ", "a.*,b.NodeName", wherestr, startRow, pageSize, orderstr));
            }
            else
            {
                entitys = Query(objdata.ExecSelect("Base_Flow_Detail a left join (select * from Base_Flow_Node where AccID='" + AccID + "') b on a.NodeNo=b.NodeNo ", "a.*,b.NodeName", wherestr, START_ROW_INIT, START_ROW_INIT, orderstr));
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
                project.Entity.Base.EntityFlowDetail entity = new project.Entity.Base.EntityFlowDetail();
                entity.InnerEntityOID = dr["RowPointer"].ToString();
                entity.NodeNo = dr["NodeNo"].ToString();
                entity.NodeName = dr["NodeName"].ToString();
                entity.AccID = dr["AccID"].ToString();
                entity.FlowNo = dr["FlowNo"].ToString();

                result.Add(entity);
            }
            return result;
        }

    }
}
