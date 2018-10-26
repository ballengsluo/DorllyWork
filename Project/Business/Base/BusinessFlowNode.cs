using System;
using System.Data;
namespace project.Business.Base
{
    /// <summary>
    /// 工单流程表的业务类
    /// </summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    public sealed class BusinessFlowNode : project.Business.AbstractPmBusiness
    {
        private project.Entity.Base.EntityFlowNode _entity = new project.Entity.Base.EntityFlowNode();
        public string orderstr = "NodeNo";
        Data objdata = new Data();

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public BusinessFlowNode() { }

        /// <summary>
        /// 带参数的构函数
        /// </summary>
        /// <param name="entity">实体类</param>
        public BusinessFlowNode(project.Entity.Base.EntityFlowNode entity)
        {
            this._entity = entity;
        }

        /// <summary>
        /// 与实体类(EntityFlowNode)关联
        /// </summary>
        public project.Entity.Base.EntityFlowNode Entity
        {
            get { return _entity as project.Entity.Base.EntityFlowNode; }
        }

        /// </summary>
        ///load 方法 pid主键
        /// </summary>
        public void load(string NodeNo, string AccID)
        {
            DataRow dr = objdata.ExecuteDataSet("select * from Base_Flow_Node where NodeNo='" + NodeNo + "' and AccID='" + AccID + "'").Tables[0].Rows[0];
            _entity.NodeNo = dr["NodeNo"].ToString();
            _entity.NodeName = dr["NodeName"].ToString();
            _entity.AccID = dr["AccID"].ToString();
            _entity.Status = dr["Status"].ToString();
            _entity.ProcMode = dr["ProcMode"].ToString();
            _entity.OpNo = dr["OpNo"].ToString();
            _entity.OpName = dr["OpName"].ToString();
        }

        /// </summary>
        ///Save方法
        /// </summary>
        public int Save(string type)
        {
            string sqlstr = "";
            if (type == "insert")
                sqlstr = "insert into Base_Flow_Node(NodeNo,NodeName,AccID,Status,Remark,OpNo,OpName)" +
                    "values('" + Entity.NodeNo + "'," + "'" + Entity.NodeName + "'" + "," + "'" + Entity.AccID + "'" + "," +
                    "'" + Entity.Status + "'" + "," + "'" + Entity.ProcMode + "'" + ")";
            else
                sqlstr = "update Base_Flow_Node" +
                    " set NodeName=" + "'" + Entity.NodeName + "'" + "," +
                    "Status=" + "'" + Entity.Status + "'" + "," +
                    "ProcMode=" + "'" + Entity.ProcMode + "'" + "," +
                    "OpNo=" + "'" + Entity.OpNo + "'" + "," +
                    "OpName=" + "'" + Entity.OpName + "'" +
                    " where NodeNo='" + Entity.NodeNo + "'";
            return objdata.ExecuteNonQuery(sqlstr);
        }

        /// </summary>
        ///Delete 方法 
        /// </summary>
        public int delete()
        {
            return objdata.ExecuteNonQuery("delete from Base_Flow_Node where NodeNo='" + Entity.NodeNo + "' and AccID='" + Entity.AccID + "'");
        }

        /// <summary>
        /// 按条件查询，支持分页
        /// </summary>
        /// <param name="NodeNo">工单流程编号</param>
        /// <param name="NodeName">工单流程名称</param>
        /// <param name="AccID">账套</param>
        /// <returns></returns>
        public System.Collections.ICollection GetFlowNodeListQuery(String NodeNo, String NodeName, String AccID, int startRow, int pageSize)
        {
            if (startRow < 0 || pageSize <= 0)
            {
                throw new Exception();
            }

            return GetListHelper(NodeNo, NodeName, AccID, startRow, pageSize);
        }

        /// <summary>
        /// 按条件查询，不支持分页
        /// </summary>
        /// <param name="NodeNo">工单流程编号</param>
        /// <param name="NodeName">工单流程名称</param>
        /// <param name="AccID">账套</param>
        /// <returns></returns>
        public System.Collections.ICollection GetFlowNodeListQuery(String NodeNo, String NodeName, String AccID)
        {
            return GetListHelper(NodeNo, NodeName, AccID, START_ROW_INIT, START_ROW_INIT);
        }

        /// <summary>
        /// 返回集合的大小
        /// </summary>
        /// <param name="NodeNo">工单流程编号</param>
        /// <param name="NodeName">工单流程名称</param>
        /// <param name="AccID">账套</param>
        /// <returns></returns>
        public int GetFlowNodeListCount(String NodeNo, String NodeName, String AccID)
        {
            string wherestr = "";
            if (NodeNo != string.Empty)
            {
                wherestr = wherestr + " and NodeNo like '%" + NodeNo + "%'";
            }
            if (NodeName != string.Empty)
            {
                wherestr = wherestr + " and NodeName like '%" + NodeName + "%'";
            }
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and AccID like '" + AccID + "'";
            }

            string count = objdata.ExecuteDataSet("select count(*) as cnt from Base_Flow_Node where 1=1 " + wherestr).Tables[0].Rows[0]["cnt"].ToString();
            return int.Parse(count);
        }

        /// <summary>
        /// 按条件查询，返回符合条件的集合
        /// </summary>
        /// <param name="NodeNo">工单流程编号</param>
        /// <param name="NodeName">工单流程名称</param>
        /// <param name="AccID">账套</param>
        /// <returns></returns>
        private System.Collections.ICollection GetListHelper(String NodeNo, String NodeName, String AccID, int startRow, int pageSize)
        {
            string wherestr = "";
            if (NodeNo != string.Empty)
            {
                wherestr = wherestr + " and NodeNo like '%" + NodeNo + "%'";
            }
            if (NodeName != string.Empty)
            {
                wherestr = wherestr + " and NodeName like '%" + NodeName + "%'";
            }
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and AccID like '" + AccID + "'";
            }

            System.Collections.IList entitys = null;
            if (startRow > START_ROW_INIT && pageSize > START_ROW_INIT)
            {
                entitys = Query(objdata.ExecSelect("Base_Flow_Node", wherestr, startRow, pageSize, orderstr));
            }
            else
            {
                entitys = Query(objdata.ExecSelect("Base_Flow_Node", wherestr, START_ROW_INIT, START_ROW_INIT, orderstr));
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
                project.Entity.Base.EntityFlowNode entity = new project.Entity.Base.EntityFlowNode();
                entity.NodeNo = dr["NodeNo"].ToString();
                entity.NodeName = dr["NodeName"].ToString();
                entity.AccID = dr["AccID"].ToString();
                entity.Status = dr["Status"].ToString();
                entity.ProcMode = dr["ProcMode"].ToString();
                entity.OpNo = dr["OpNo"].ToString();
                entity.OpName = dr["OpName"].ToString();
                result.Add(entity);
            }
            return result;
        }

    }
}
