using System;
using System.Data;
namespace project.Business.Sys
{
    /// <summary>
    /// 用户权限的业务类
    /// </summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    public sealed class BusinessUserRight : project.Business.AbstractPmBusiness
    {
        private project.Entity.Sys.EntityUserInfoRight _entity = new project.Entity.Sys.EntityUserInfoRight() ;
        public string orderstr = "MenuID";
        Data objdata = new Data();

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public BusinessUserRight() {}

        /// <summary>
        /// 带参数的构函数
        /// </summary>
        /// <param name="entity">实体类</param>
        public BusinessUserRight(project.Entity.Sys.EntityUserInfoRight entity)
        {
            this._entity = entity;
        }

        /// <summary>
        /// 与实体类(EntityUserInfoRight)关联
        /// </summary>
        public project.Entity.Sys.EntityUserInfoRight Entity
        {
            get { return _entity as project.Entity.Sys.EntityUserInfoRight; }
        }

        /// </summary>
        ///load 方法 pid主键
        /// </summary>
        public void load(string pid)
        {
            DataRow dr = objdata.ExecuteDataSet("select * from Sys_User_Right where RightID='" + pid + "'").Tables[0].Rows[0];
            _entity.InnerEntityOID=dr["RightID"].ToString();
            _entity.MenuID=dr["MenuID"].ToString();
            _entity.UserType = dr["UserType"].ToString();
            _entity.AccID = dr["AccID"].ToString();
        }

        /// </summary>
        ///Save方法
        /// </summary>
        public int Save()
        {
            string sqlstr="";
            if (Entity.InnerEntityOID == null)
                sqlstr = "insert into Sys_User_Right(RightID,MenuID,UserType,AccID)" +
                    "values(newid()," + "'" + Entity.MenuID + "'" + "," + "'" + Entity.UserType + "'" + "," + "'" + Entity.AccID + "'" + ")";
            else
                sqlstr = "update Sys_User_Right" +
                    " set MenuID=" + "'" + Entity.MenuID + "'" + "," + "UserType=" + "'" + Entity.UserType + "'" +
                    " where RightID='" + Entity.EntityOID.ToString() + "'";
            return objdata.ExecuteNonQuery(sqlstr);
        }

        /// </summary>
        ///Delete 方法 
        /// </summary>
        public int delete()
        {
            return objdata.ExecuteNonQuery("delete from Sys_User_Right where RightID='"+Entity.EntityOID.ToString()+"'");
        }

        /// <summary>
        /// 按条件查询，支持分页
        /// </summary>
        /// <param name="UserTypeEquals">用户类型</param>
        /// <param name="AccID">账户</param>
        /// <returns></returns>
        public System.Collections.ICollection GetUserRightListQuery(String UserTypeEquals, String AccID, int startRow, int pageSize)
        {
            if (startRow < 0 || pageSize <= 0)
            {
                throw new Exception();
            }

            return GetListHelper(UserTypeEquals, AccID,startRow, pageSize);
        }

        /// <summary>
        /// 按条件查询，不支持分页
        /// </summary>
        /// <param name="UserTypeEquals">用户类型</param>
        /// <param name="AccID">账户</param>
        /// <returns></returns>
        public System.Collections.ICollection GetUserRightListQuery(String UserTypeEquals, String AccID)
        {
            return GetListHelper(UserTypeEquals, AccID, START_ROW_INIT, START_ROW_INIT);
        }

        /// <summary>
        /// 返回集合的大小
        /// </summary>
        /// <param name="UserTypeEquals">用户类型</param>
        /// <param name="AccID">账户</param>
        /// <returns></returns>
        public int GetUserRightListCount(String UserTypeEquals, String AccID)
        {
            string wherestr="";
            if (UserTypeEquals != string.Empty)
            {
                wherestr=wherestr+" and UserType='"+UserTypeEquals+"'";
            }
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and AccID='" + AccID + "'";
            }
            
            string count = objdata.ExecuteDataSet("select count(*) as cnt from Sys_User_Right where 1=1 " + wherestr).Tables[0].Rows[0]["cnt"].ToString();
            return int.Parse(count);
        }

        /// <summary>
        /// 按条件查询，返回符合条件的集合
        /// </summary>
        /// <param name="UserTypeEquals">用户类型</param>
        /// <param name="AccID">账户</param>
        /// <returns></returns>
        private System.Collections.ICollection GetListHelper(String UserTypeEquals, String AccID, int startRow, int pageSize)
        {
            string wherestr="";
            if (UserTypeEquals != string.Empty)
            {
                wherestr=wherestr+" and UserType='"+UserTypeEquals+"'";
            }
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and AccID='" + AccID + "'";
            }

            System.Collections.IList entitys = null;
            if (startRow > START_ROW_INIT && pageSize > START_ROW_INIT)
            {
                entitys=Query(objdata.ExecSelect("Sys_User_Right", wherestr ,startRow,pageSize, orderstr));
            }
            else
            {
                entitys=Query(objdata.ExecSelect("Sys_User_Right", wherestr , START_ROW_INIT ,START_ROW_INIT, orderstr));
            }
            return entitys;
        }

        /// <summary>
        /// 按条件查询，返回符合条件的集合
        /// </summary>
        /// <param name="UserType">用户类型</param>
        /// <param name="MenuType">菜单类型</param>
        /// <param name="AccID">账户</param>
        /// <returns></returns>
        public System.Collections.ICollection GetUserRightInfo(String UserType, string MenuType, string AccID)
        {
            System.Collections.IList result = new System.Collections.ArrayList();
            Data obj = new Data();
            DataTable dt = obj.ExecuteDataSet("select a.*,b.RightId from Sys_Menu a left join ( select * from Sys_User_Right where UserType='" + UserType +
                "' and AccID='" + AccID + "')b on a.MenuId=b.MenuId where a.MenuType='" + MenuType + "' and a.AccID='" + AccID + "' order by a.OrderNo").Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                project.Entity.Sys.EntityUserInfoRights entity = new project.Entity.Sys.EntityUserInfoRights();

                entity.InnerEntityOID = dr["MenuID"].ToString();
                entity.MenuName = dr["MenuName"].ToString();
                entity.MenuType = dr["MenuType"].ToString();
                entity.MenuPath = dr["MenuPath"].ToString();
                entity.Flag = ParseIntForString(dr["Flag"].ToString());
                entity.Parent = dr["Parent"].ToString();
                entity.OrderNo = dr["OrderNo"].ToString();
                if (dr["rightid"].ToString() == "")
                    entity.Right = false;
                else
                    entity.Right = true;
                result.Add(entity);
            }
            return result;
        }

        /// </summary>
        ///Query 方法 dt查询结果
        /// </summary>
        public System.Collections.IList Query(System.Data.DataTable dt)
        {
            System.Collections.IList result = new System.Collections.ArrayList();
            foreach(System.Data.DataRow dr in dt.Rows)
            {
                project.Entity.Sys.EntityUserInfoRight entity=new project.Entity.Sys.EntityUserInfoRight();
              
                entity.InnerEntityOID=dr["RightID"].ToString();
                entity.MenuID=dr["MenuID"].ToString();
                entity.UserType = dr["UserType"].ToString();
                entity.AccID = dr["AccID"].ToString();
                result.Add(entity);
            }
            return result;
        }

    }
}
