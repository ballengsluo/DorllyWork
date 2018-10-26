using System;
using System.Data;
namespace project.Business.Sys
{
    /// <summary>
    /// 用户信息的业务类
    /// </summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    public sealed class BusinessUserInfo : project.Business.AbstractPmBusiness
    {
        private project.Entity.Sys.EntityUserInfo _entity = new project.Entity.Sys.EntityUserInfo() ;
        public string orderstr = "UserNo";
        Data objdata = new Data();

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public BusinessUserInfo() {}

        /// <summary>
        /// 带参数的构函数
        /// </summary>
        /// <param name="entity">实体类</param>
        public BusinessUserInfo(project.Entity.Sys.EntityUserInfo entity)
        {
            this._entity = entity;
        }

        /// <summary>
        /// 与实体类(EntityUserInfo)关联
        /// </summary>
        public project.Entity.Sys.EntityUserInfo Entity
        {
            get { return _entity as project.Entity.Sys.EntityUserInfo; }
        }

        /// </summary>
        ///load 方法 pid主键
        /// </summary>
        public void load(string userId)
        {
            DataRow dr = objdata.ExecuteDataSet("select * from Sys_User_Info where UserID='" + userId + "'").Tables[0].Rows[0];
            _entity.InnerEntityOID=dr["UserID"].ToString();
            _entity.UserType=dr["UserType"].ToString();
            _entity.AccID=dr["AccID"].ToString();
            _entity.UserNo=dr["UserNo"].ToString();
            _entity.NickName=dr["NickName"].ToString();
            _entity.UserName=dr["UserName"].ToString();
            _entity.Password=dr["Password"].ToString();
            _entity.Tel=dr["Tel"].ToString();
            _entity.Email=dr["Email"].ToString();
            _entity.Addr=dr["Addr"].ToString();
            _entity.RegDate=ParseDateTimeForString(dr["RegDate"].ToString());
            _entity.Valid = bool.Parse(dr["Valid"].ToString());
            _entity.AccID = dr["AccID"].ToString();
            _entity.DeptNo = dr["DeptNo"].ToString();
            _entity.Manager = dr["Manager"].ToString();
            _entity.Picture = dr["Picture"].ToString();
        }

        /// </summary>
        ///load 方法 pid主键
        /// </summary>
        public void loadUserNo(string userNo,string AccID)
        {
            DataRow dr = objdata.ExecuteDataSet("select * from Sys_User_Info where UserNo='" + userNo + "' and AccID='" + AccID + "'").Tables[0].Rows[0];
            _entity.InnerEntityOID = dr["UserID"].ToString();
            _entity.UserType = dr["UserType"].ToString();
            _entity.AccID = dr["AccID"].ToString();
            _entity.UserNo = dr["UserNo"].ToString();
            _entity.NickName = dr["NickName"].ToString();
            _entity.UserName = dr["UserName"].ToString();
            _entity.Password = dr["Password"].ToString();
            _entity.Tel = dr["Tel"].ToString();
            _entity.Email = dr["Email"].ToString();
            _entity.Addr = dr["Addr"].ToString();
            _entity.RegDate = ParseDateTimeForString(dr["RegDate"].ToString());
            _entity.Valid = bool.Parse(dr["Valid"].ToString());
            _entity.AccID = dr["AccID"].ToString();
            _entity.DeptNo = dr["DeptNo"].ToString();
            _entity.Manager = dr["Manager"].ToString();
            _entity.Picture = dr["Picture"].ToString();
        }

        /// </summary>
        ///Save方法
        /// </summary>
        public int Save()
        {
            string sqlstr="";
            if (Entity.InnerEntityOID == null)
                sqlstr = "insert into Sys_User_Info(UserID,UserType,AccID,DeptNo,Manager,UserNo,NickName,UserName,Password,Tel,Email,Addr,RegDate,Valid,Picture)" +
                    "values(newid()," + "'" + Entity.UserType + "'" + "," + "'" + Entity.AccID + "'" + "," + "'" + Entity.DeptNo + "'" + "," + "'" + Entity.Manager + "'" + "," +
                    "'" + Entity.UserNo + "'" + "," + "'" + Entity.NickName + "'" + "," + "'" + Entity.UserName + "'" + "," + "'" + Entity.Password + "'" + "," +
                    "'" + Entity.Tel + "'" + "," + "'" + Entity.Email + "'" + "," + "'" + Entity.Addr + "'" + "," +
                    "'" + Entity.RegDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + (Entity.Valid == true ? 1 : 0) + ",'" + Entity.Picture + "')";
            else
                sqlstr = "update Sys_User_Info" +
                    " set UserType=" + "'" + Entity.UserType + "'" + "," +
                    "DeptNo=" + "'" + Entity.DeptNo + "'" + "," + "Manager=" + "'" + Entity.Manager + "'" + "," +
                    "NickName=" + "'" + Entity.NickName + "'" + "," + "UserName=" + "'" + Entity.UserName + "'" + "," +
                    "Tel=" + "'" + Entity.Tel + "'" + "," + "Email=" + "'" + Entity.Email + "'" + "," +
                    "Addr=" + "'" + Entity.Addr + "'" + "," + "Picture=" + "'" + Entity.Picture + "'" +
                    " where UserID='" + Entity.InnerEntityOID + "'";
            return objdata.ExecuteNonQuery(sqlstr);
        }

        /// </summary>
        ///Delete 方法 
        /// </summary>
        public int delete()
        {
            return objdata.ExecuteNonQuery("delete from Sys_User_Info where UserID='" + Entity.InnerEntityOID + "'");
        }

        /// </summary>
        /// 启用/停用 
        /// </summary>
        public int valid()
        {
            return objdata.ExecuteNonQuery("update Sys_User_Info set Valid=" + (Entity.Valid == true ? 1 : 0) + " where UserID='" + Entity.InnerEntityOID + "'");
        }

        /// </summary>
        /// 启用/停用 
        /// </summary>
        public int changepwd()
        {
            return objdata.ExecuteNonQuery("update Sys_User_Info set Password='" + Entity.Password + "' where UserID='" + Entity.InnerEntityOID + "'");
        }

        /// </summary>
        ///修改昵称
        /// </summary>
        public int updateNickName(string id, string nickName)
        {
            return objdata.ExecuteNonQuery("update base_custuser set NickName='" + nickName + "' where RowPointer='" + id + "'");
        }

        /// <summary>
        /// 按条件查询，支持分页
        /// </summary>
        /// <param name="UserType">用户类型</param>
        /// <param name="AccID">账套ID</param>
        /// <param name="DeptNo">部门</param>
        /// <param name="UserName">用户名称</param>
        /// <returns></returns>
        public System.Collections.ICollection GetUserInfoListQuery(String UserType, String AccID, String DeptNo, String UserName, int startRow, int pageSize)
        {
            if (startRow < 0 || pageSize <= 0)
            {
                throw new Exception();
            }

            return GetListHelper(UserType, AccID, DeptNo, UserName, startRow, pageSize);
        }

        /// <summary>
        /// 按条件查询，不支持分页
        /// </summary>
        /// <param name="UserType">用户类型</param>
        /// <param name="AccID">账套ID</param>
        /// <param name="DeptNo">部门</param>
        /// <param name="UserName">用户名称</param>
        /// <returns></returns>
        public System.Collections.ICollection GetUserInfoListQuery(String UserType, String AccID, String DeptNo, String UserName)
        {
            return GetListHelper(UserType, AccID, DeptNo, UserName, START_ROW_INIT, START_ROW_INIT);
        }

        /// <summary>
        /// 返回集合的大小
        /// </summary>
        /// <param name="UserType">用户类型</param>
        /// <param name="AccID">账套ID</param>
        /// <param name="DeptNo">部门</param>
        /// <param name="UserName">用户名称</param>
        /// <returns></returns>
        public int GetUserInfoListCount(String UserType, String AccID, String DeptNo, String UserName)
        {
            string wherestr="";
            if (UserType != string.Empty)
            {
                wherestr=wherestr+" and UserType='"+UserType+"'";
            }
            if (AccID != string.Empty)
            {
                wherestr=wherestr+" and AccID='"+AccID+"'";
            }
            if (DeptNo != string.Empty)
            {
                wherestr = wherestr + " and DeptNo='" + DeptNo + "'";
            }
            if (UserName != string.Empty)
            {
                wherestr=wherestr+" and UserName like '%"+UserName+"%'";
            }

            string count = objdata.ExecuteDataSet("select count(*) as cnt from Sys_User_Info where 1=1 " + wherestr).Tables[0].Rows[0]["cnt"].ToString();
            return int.Parse(count);
        }

        /// <summary>
        /// 按条件查询，返回符合条件的集合
        /// </summary>
        /// <param name="UserType">用户类型</param>
        /// <param name="AccID">账套ID</param>
        /// <param name="DeptNo">部门</param>
        /// <param name="UserName">用户名称</param>
        /// <returns></returns>
        private System.Collections.ICollection GetListHelper(String UserType, String AccID, String DeptNo, String UserName, int startRow, int pageSize)
        {
            string wherestr="";
            if (UserType != string.Empty)
            {
                wherestr=wherestr+" and UserType='"+UserType+"'";
            }
            if (AccID != string.Empty)
            {
                wherestr=wherestr+" and AccID='"+AccID+"'";
            }
            if (DeptNo != string.Empty)
            {
                wherestr = wherestr + " and DeptNo='" + DeptNo + "'";
            }
            if (UserName != string.Empty)
            {
                wherestr=wherestr+" and UserName like '%"+UserName+"%'";
            }

            System.Collections.IList entitys = null;
            if (startRow > START_ROW_INIT && pageSize > START_ROW_INIT)
            {
                entitys=Query(objdata.ExecSelect("Sys_User_Info", wherestr ,startRow,pageSize, orderstr));
            }
            else
            {
                entitys=Query(objdata.ExecSelect("Sys_User_Info", wherestr , START_ROW_INIT ,START_ROW_INIT, orderstr));
            }
            return entitys;
        }

        /// <summary>
        /// 按条件查询，返回符合条件的集合
        /// </summary>
        /// <param name="UserNo">用户名称</param>
        /// <param name="Password">用户名称</param>
        /// <returns></returns>
        public System.Collections.ICollection Login(String UserNo, String Password)
        {
            string wherestr = "";
            wherestr = wherestr + " and (UserNo='" + UserNo + "' or NickName='" + UserNo + "')";
            wherestr = wherestr + " and Password='" + Password + "'";
            wherestr = wherestr + " and Valid=1";
            System.Collections.IList entitys = null;

            entitys = Query(objdata.ExecSelect("Sys_User_Info", wherestr, START_ROW_INIT, START_ROW_INIT, orderstr));
            return entitys;
        }

        /// </summary>
        ///Query 方法 dt查询结果
        /// </summary>
        public System.Collections.IList Query(System.Data.DataTable dt)
        {
            System.Collections.IList result = new System.Collections.ArrayList();
            foreach(System.Data.DataRow dr in dt.Rows)
            {
                project.Entity.Sys.EntityUserInfo entity=new project.Entity.Sys.EntityUserInfo();
              
                entity.InnerEntityOID=dr["UserID"].ToString();
                entity.UserType=dr["UserType"].ToString();
                entity.AccID=dr["AccID"].ToString();
                entity.UserNo=dr["UserNo"].ToString();
                entity.NickName=dr["NickName"].ToString();
                entity.UserName=dr["UserName"].ToString();
                entity.Password=dr["Password"].ToString();
                entity.Tel=dr["Tel"].ToString();
                entity.Email=dr["Email"].ToString();
                entity.Addr=dr["Addr"].ToString();
                entity.RegDate=ParseDateTimeForString(dr["RegDate"].ToString());
                entity.Valid = bool.Parse(dr["Valid"].ToString());
                entity.AccID = dr["AccID"].ToString();
                entity.DeptNo = dr["DeptNo"].ToString();
                entity.Manager = dr["Manager"].ToString();
                entity.Picture = dr["Picture"].ToString();
                result.Add(entity);
            }
            return result;
        }

    }
}
