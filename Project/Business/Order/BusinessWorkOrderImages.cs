using System;
using System.Data;
namespace project.Business.Order
{
    /// <summary>
    /// 工单日志记录的业务类
    /// </summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    public sealed class BusinessWorkOrderImages : project.Business.AbstractPmBusiness
    {
        private project.Entity.Order.EntityWorkOrderImages _entity = new project.Entity.Order.EntityWorkOrderImages();
        public string orderstr = "UploadDate";
        Data objdata = new Data();

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public BusinessWorkOrderImages() { }

        /// <summary>
        /// 带参数的构函数
        /// </summary>
        /// <param name="entity">实体类</param>
        public BusinessWorkOrderImages(project.Entity.Order.EntityWorkOrderImages entity)
        {
            this._entity = entity;
        }

        /// <summary>
        /// 与实体类(EntityWorkOrderImages)关联
        /// </summary>
        public project.Entity.Order.EntityWorkOrderImages Entity
        {
            get { return _entity as project.Entity.Order.EntityWorkOrderImages; }
        }

        /// </summary>
        ///load 方法 pid主键
        /// </summary>
        public void load(string id, string accID)
        {
            DataRow dr = objdata.ExecuteDataSet("select * from WO_WorkOrder_Images where RowPointer='" + id + "' and AccID='" + accID + "'").Tables[0].Rows[0];
            _entity.InnerEntityOID = dr["RowPointer"].ToString();
            _entity.AccID = dr["AccID"].ToString();
            _entity.OrderNo = dr["OrderNo"].ToString();
            _entity.NodeNo = dr["NodeNo"].ToString();
            _entity.Img = dr["Img"].ToString();
            _entity.UploadDate = ParseDateTimeForString(dr["UploadDate"].ToString());
            _entity.UploadUser = dr["UploadUser"].ToString();
        }

        /// </summary>
        ///Save方法
        /// </summary>
        public int Save()
        {
            string sqlstr = "";
            if (Entity.InnerEntityOID == null)
                sqlstr = "insert into WO_WorkOrder_Images(RowPointer,AccID,OrderNo,NodeNo,Img,UploadDate,UploadUser)" +
                    "values(NewID()," + "'" + Entity.AccID + "'" + "," + "'" + Entity.OrderNo + "'" + "," +
                    "'" + Entity.NodeNo + "'" + "," + "'" + Entity.Img + "'" + "," +
                    "'" + Entity.UploadDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," +
                    "'" + Entity.UploadUser + "'" + ")";
            else
                sqlstr = "update WO_WorkOrder_Images" +
                    " set OrderNo=" + "'" + Entity.OrderNo + "'" + "," + "NodeNo=" + "'" + Entity.NodeNo + "'" + "," + "Img=" + "'" + Entity.Img + "'" + "," +
                    "UploadDate=" + "'" + Entity.UploadDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," +
                    "UploadUser=" + "'" + Entity.UploadUser + "'" +
                    " where RowPointer='" + Entity.InnerEntityOID + "'";
            return objdata.ExecuteNonQuery(sqlstr);
        }

        /// </summary>
        ///Delete 方法 
        /// </summary>
        public int delete()
        {
            return objdata.ExecuteNonQuery("delete from WO_WorkOrder_Images where RowPointer='" + Entity.InnerEntityOID + "' and AccID=" + "'" + Entity.AccID + "'");
        }

        /// <summary>
        /// 按条件查询，支持分页
        /// </summary>
        /// <param name="AccID">账套</param>
        /// <param name="OrderNo">订单号</param>
        /// <param name="NodeNo">节点</param>
        /// <returns></returns>
        public System.Collections.ICollection GetWorkOrderImagesQuery(String AccID, String OrderNo, String NodeNo, int startRow, int pageSize)
        {
            if (startRow < 0 || pageSize <= 0)
            {
                throw new Exception();
            }

            return GetListHelper(AccID, OrderNo, NodeNo, startRow, pageSize);
        }

        /// <summary>
        /// 按条件查询，不支持分页
        /// </summary>
        /// <param name="AccID">账套</param>
        /// <param name="OrderNo">订单号</param>
        /// <param name="NodeNo">节点</param>
        /// <returns></returns>
        public System.Collections.ICollection GetWorkOrderImagesQuery(String AccID, String OrderNo, String NodeNo)
        {
            return GetListHelper(AccID, OrderNo, NodeNo, START_ROW_INIT, START_ROW_INIT);
        }

        /// <summary>
        /// 返回集合的大小
        /// </summary>
        /// <param name="AccID">账套</param>
        /// <param name="OrderNo">订单号</param>
        /// <param name="NodeNo">节点</param>
        /// <returns></returns>
        public int GetWorkOrderImagesCount(String AccID, String OrderNo, String NodeNo)
        {
            string wherestr = "";
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and AccID='" + AccID + "'";
            }
            if (OrderNo != string.Empty)
            {
                wherestr = wherestr + " and OrderNo like '" + OrderNo + "'";
            }
            if (NodeNo != string.Empty)
            {
                wherestr = wherestr + " and NodeNo like '" + NodeNo + "'";
            }

            string count = objdata.ExecuteDataSet("select count(*) as cnt from WO_WorkOrder_Images where 1=1 " + wherestr).Tables[0].Rows[0]["cnt"].ToString();
            return int.Parse(count);
        }

        /// <summary>
        /// 按条件查询，返回符合条件的集合
        /// </summary>
        /// <param name="AccID">账套</param>
        /// <param name="OrderNo">订单号</param>
        /// <param name="NodeNo">节点</param>
        /// <returns></returns>
        private System.Collections.ICollection GetListHelper(String AccID, String OrderNo, String NodeNo, int startRow, int pageSize)
        {
            string wherestr = "";
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and AccID='" + AccID + "'";
            }
            if (OrderNo != string.Empty)
            {
                wherestr = wherestr + " and OrderNo like '%" + OrderNo + "%'";
            }
            if (NodeNo != string.Empty)
            {
                wherestr = wherestr + " and NodeNo like '" + NodeNo + "'";
            }

            System.Collections.IList entitys = null;
            if (startRow > START_ROW_INIT && pageSize > START_ROW_INIT)
            {
                entitys = Query(objdata.ExecSelect("WO_WorkOrder_Images", wherestr, startRow, pageSize, orderstr));
            }
            else
            {
                entitys = Query(objdata.ExecSelect("WO_WorkOrder_Images", wherestr, START_ROW_INIT, START_ROW_INIT, orderstr));
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
                project.Entity.Order.EntityWorkOrderImages entity = new project.Entity.Order.EntityWorkOrderImages();
                entity.InnerEntityOID = dr["RowPointer"].ToString();
                entity.AccID = dr["AccID"].ToString();
                entity.OrderNo = dr["OrderNo"].ToString();
                entity.NodeNo = dr["NodeNo"].ToString();
                entity.Img = dr["Img"].ToString();
                entity.UploadDate = ParseDateTimeForString(dr["UploadDate"].ToString());
                entity.UploadUser = dr["UploadUser"].ToString();
                result.Add(entity);
            }
            return result;
        }

    }
}
