using System;
using System.Data;
namespace project.Business.Base
{
    /// <summary>
    /// 用户信息的业务类
    /// </summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    public sealed class BusinessCustInfo : project.Business.AbstractPmBusiness
    {
        private project.Entity.Base.EntityCustInfo _entity = new project.Entity.Base.EntityCustInfo();
        public string orderstr = "CustNo";
        Data objdata = new Data();

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public BusinessCustInfo() { }

        /// <summary>
        /// 带参数的构函数
        /// </summary>
        /// <param name="entity">实体类</param>
        public BusinessCustInfo(project.Entity.Base.EntityCustInfo entity)
        {
            this._entity = entity;
        }

        /// <summary>
        /// 与实体类(EntityCustInfo)关联
        /// </summary>
        public project.Entity.Base.EntityCustInfo Entity
        {
            get { return _entity as project.Entity.Base.EntityCustInfo; }
        }

        /// </summary>
        ///load 方法 pid主键
        /// </summary>
        public void load(string id)
        {
            DataRow dr = objdata.ExecuteDataSet("select * from Base_Cust_Info where RowPointer='" + id + "'").Tables[0].Rows[0];
            _entity.InnerEntityOID = dr["RowPointer"].ToString();            
            _entity.AccID = dr["AccID"].ToString();
            _entity.CustNo = dr["CustNo"].ToString();
            _entity.CustName = dr["CustName"].ToString();
            _entity.CustType = dr["CustType"].ToString();
            _entity.Contact = dr["Contact"].ToString();
            _entity.Tel = dr["Tel"].ToString();
            _entity.Addr = dr["Addr"].ToString();
            _entity.Website = dr["Website"].ToString();
            _entity.Remark = dr["Remark"].ToString();
            _entity.RegDate = ParseDateTimeForString(dr["RegDate"].ToString());
            _entity.Valid = bool.Parse(dr["Valid"].ToString());
        }

        /// </summary>
        ///load 方法 pid主键
        /// </summary>
        public void loadCustNo(string custNo, string AccID)
        {
            DataRow dr = objdata.ExecuteDataSet("select * from Base_Cust_Info where CustNo='" + custNo + "' and AccID='" + AccID + "'").Tables[0].Rows[0];
            _entity.InnerEntityOID = dr["RowPointer"].ToString();
            _entity.AccID = dr["AccID"].ToString();
            _entity.CustNo = dr["CustNo"].ToString();
            _entity.CustName = dr["CustName"].ToString();
            _entity.CustType = dr["CustType"].ToString();
            _entity.Contact = dr["Contact"].ToString();
            _entity.Tel = dr["Tel"].ToString();
            _entity.Addr = dr["Addr"].ToString();
            _entity.Website = dr["Website"].ToString();
            _entity.Remark = dr["Remark"].ToString();
            _entity.RegDate = ParseDateTimeForString(dr["RegDate"].ToString());
            _entity.Valid = bool.Parse(dr["Valid"].ToString());
        }

        /// </summary>
        ///Save方法
        /// </summary>
        public int Save()
        {
            string sqlstr = "";
            if (Entity.InnerEntityOID == null)
                sqlstr = "insert into Base_Cust_Info(RowPointer,AccID,CustNo,CustName,CustType,Contact,Tel,Addr,Website,Remark,RegDate,Valid)" +
                    "values(newid()," + "'" + Entity.AccID + "'" + "," + "'" + Entity.CustNo + "'" + "," + "'" + Entity.CustName + "'" + "," +
                    "'" + Entity.CustType + "'" + "," + "'" + Entity.Contact + "'" + "," + "'" + Entity.Tel + "'" + "," +
                    "'" + Entity.Addr + "'" + "," + "'" + Entity.Website + "'" + "," + "'" + Entity.Remark + "'" + "," +
                    "'" + Entity.RegDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + (Entity.Valid == true ? 1 : 0) + ")";
            else
                sqlstr = "update Base_Cust_Info" +
                    " set CustName=" + "'" + Entity.CustName + "'" + "," + "CustType=" + "'" + Entity.CustType + "'" + "," +
                    "Contact=" + "'" + Entity.Contact + "'" + "," + "Tel=" + "'" + Entity.Tel + "'" + "," +
                    "Addr=" + "'" + Entity.Addr + "'" + "," + "Website=" + "'" + Entity.Website + "'" + "," +
                    "Remark=" + "'" + Entity.Remark + "'" + 
                    " where RowPointer='" + Entity.InnerEntityOID.ToString() + "'";
            return objdata.ExecuteNonQuery(sqlstr);
        }

        /// </summary>
        ///Delete 方法 
        /// </summary>
        public int delete()
        {
            return objdata.ExecuteNonQuery("delete from Base_Cust_Info where RowPointer='" + Entity.InnerEntityOID.ToString() + "'");
        }

        /// </summary>
        /// 启用/停用 
        /// </summary>
        public int valid()
        {
            return objdata.ExecuteNonQuery("update Base_Cust_Info set Valid=" + (Entity.Valid == true ? 1 : 0) + " where RowPointer='" + Entity.InnerEntityOID + "'");
        }
        
        /// <summary>
        /// 按条件查询，支持分页
        /// </summary>
        /// <param name="CustType">客户类型</param>
        /// <param name="AccID">账套ID</param>
        /// <param name="CustNo">客户编号</param>
        /// <param name="CustName">客户名称</param>
        /// <param name="Addr">地址</param>
        /// <param name="Valid">是否有效</param>
        /// <returns></returns>
        public System.Collections.ICollection GetCustInfoListQuery(String CustType, String AccID, String CustNo, String CustName, String Addr, bool? Valid, int startRow, int pageSize)
        {
            if (startRow < 0 || pageSize <= 0)
            {
                throw new Exception();
            }

            return GetListHelper(CustType, AccID, CustNo, CustName, Addr, Valid, startRow, pageSize);
        }

        /// <summary>
        /// 按条件查询，不支持分页
        /// </summary>
        /// <param name="CustType">客户类型</param>
        /// <param name="AccID">账套ID</param>
        /// <param name="CustNo">客户编号</param>
        /// <param name="CustName">客户名称</param>
        /// <param name="Addr">地址</param>
        /// <param name="Valid">是否有效</param>
        /// <returns></returns>
        public System.Collections.ICollection GetCustInfoListQuery(String CustType, String AccID, String CustNo, String CustName, String Addr, bool? Valid)
        {
            return GetListHelper(CustType, AccID, CustNo, CustName, Addr, Valid, START_ROW_INIT, START_ROW_INIT);
        }

        /// <summary>
        /// 返回集合的大小
        /// </summary>
        /// <param name="CustType">客户类型</param>
        /// <param name="AccID">账套ID</param>
        /// <param name="CustNo">客户编号</param>
        /// <param name="CustName">客户名称</param>
        /// <param name="Addr">地址</param>
        /// <param name="Valid">是否有效</param>
        /// <returns></returns>
        public int GetCustInfoListCount(String CustType, String AccID, String CustNo, String CustName, String Addr, bool? Valid)
        {
            string wherestr = "";
            if (CustType != string.Empty)
            {
                wherestr = wherestr + " and CustType='" + CustType + "'";
            }
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and AccID='" + AccID + "'";
            }
            if (CustNo != string.Empty)
            {
                wherestr = wherestr + " and CustNo='%" + CustNo + "%'";
            }
            if (CustName != string.Empty)
            {
                wherestr = wherestr + " and CustName like '%" + CustName + "%'";
            }
            if (Addr != string.Empty)
            {
                wherestr = wherestr + " and Addr like '%" + Addr + "%'";
            }
            if (Valid != null)
            {
                wherestr = wherestr + " and Valid=" + (Valid == true ? "1" : "0");
            }

            string count = objdata.ExecuteDataSet("select count(*) as cnt from Base_Cust_Info where 1=1 " + wherestr).Tables[0].Rows[0]["cnt"].ToString();
            return int.Parse(count);
        }

        /// <summary>
        /// 按条件查询，返回符合条件的集合
        /// </summary>
        /// <param name="CustType">客户类型</param>
        /// <param name="AccID">账套ID</param>
        /// <param name="CustNo">客户编号</param>
        /// <param name="CustName">客户名称</param>
        /// <param name="Addr">地址</param>
        /// <param name="Valid">是否有效</param>
        /// <returns></returns>
        private System.Collections.ICollection GetListHelper(String CustType, String AccID, String CustNo, String CustName, String Addr, bool? Valid, int startRow, int pageSize)
        {
            string wherestr = "";
            if (CustType != string.Empty)
            {
                wherestr = wherestr + " and CustType='" + CustType + "'";
            }
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and AccID='" + AccID + "'";
            }
            if (CustNo != string.Empty)
            {
                wherestr = wherestr + " and CustNo='%" + CustNo + "%'";
            }
            if (CustName != string.Empty)
            {
                wherestr = wherestr + " and CustName like '%" + CustName + "%'";
            }
            if (Addr != string.Empty)
            {
                wherestr = wherestr + " and Addr like '%" + Addr + "%'";
            }
            if (Valid != null)
            {
                wherestr = wherestr + " and Valid=" + (Valid == true ? "1" : "0");
            }

            System.Collections.IList entitys = null;
            if (startRow > START_ROW_INIT && pageSize > START_ROW_INIT)
            {
                entitys = Query(objdata.ExecSelect("Base_Cust_Info", wherestr, startRow, pageSize, orderstr));
            }
            else
            {
                entitys = Query(objdata.ExecSelect("Base_Cust_Info", wherestr, START_ROW_INIT, START_ROW_INIT, orderstr));
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
                project.Entity.Base.EntityCustInfo entity = new project.Entity.Base.EntityCustInfo();

                entity.InnerEntityOID = dr["RowPointer"].ToString();
                entity.AccID = dr["AccID"].ToString();
                entity.CustNo = dr["CustNo"].ToString();
                entity.CustName = dr["CustName"].ToString();
                entity.CustType = dr["CustType"].ToString();
                entity.Contact = dr["Contact"].ToString();
                entity.Tel = dr["Tel"].ToString();
                entity.Addr = dr["Addr"].ToString();
                entity.Website = dr["Website"].ToString();
                entity.Remark = dr["Remark"].ToString();
                entity.RegDate = ParseDateTimeForString(dr["RegDate"].ToString());
                entity.Valid = bool.Parse(dr["Valid"].ToString());
                result.Add(entity);
            }
            return result;
        }

    }
}
