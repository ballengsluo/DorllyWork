using System;
using System.Data;
namespace project.Business.Sys
{
    /// <summary>
    /// 系统菜单的业务类
    /// </summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    public sealed class BusinessMenu : project.Business.AbstractPmBusiness
    {
        private project.Entity.Sys.EntityMenu _entity = new project.Entity.Sys.EntityMenu() ;
        public string orderstr = "OrderNo";
        Data objdata = new Data();

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public BusinessMenu() {}

        /// <summary>
        /// 带参数的构函数
        /// </summary>
        /// <param name="entity">实体类</param>
        public BusinessMenu(project.Entity.Sys.EntityMenu entity)
        {
            this._entity = entity;
        }

        /// <summary>
        /// 与实体类(EntityMenu)关联
        /// </summary>
        public project.Entity.Sys.EntityMenu Entity
        {
            get { return _entity as project.Entity.Sys.EntityMenu; }
        }

        /// </summary>
        ///load 方法 pid主键
        /// </summary>
        public void load(string pid)
        {
            DataRow dr = objdata.ExecuteDataSet("select * from Sys_Menu where MenuID='"+pid+"'").Tables[0].Rows[0];
            _entity.InnerEntityOID=dr["MenuID"].ToString();
            _entity.MenuName=dr["MenuName"].ToString();
            _entity.MenuType=dr["MenuType"].ToString();
            _entity.MenuPath=dr["MenuPath"].ToString();
            _entity.Flag=ParseIntForString(dr["Flag"].ToString());
            _entity.Parent=dr["Parent"].ToString();
            _entity.OrderNo=dr["OrderNo"].ToString();
        }

        /// </summary>
        ///Save方法
        /// </summary>
        public int Save()
        {
            string sqlstr="";
            if(Entity.InnerEntityOID==null)
                sqlstr="insert into Sys_Menu(MenuID,MenuName,MenuType,MenuPath,Flag,Parent,OrderNo,AccID)"+
                    "values(newid()," + "'"+Entity.MenuName+"'" + "," + "'"+Entity.MenuType+"'" + "," + "'"+Entity.MenuPath+"'" + "," +
                    Entity.Flag + "," + "'" + Entity.Parent + "'" + "," + "'" + Entity.OrderNo + "'" + "," + "'" + Entity.AccID + "'" + ")";
            else
                sqlstr="update Sys_Menu"+
                    " set MenuName="+"'"+Entity.MenuName+"'" + "," + "MenuType="+"'"+Entity.MenuType+"'" + "," + "MenuPath="+"'"+Entity.MenuPath+"'" + "," + "Flag="+Entity.Flag + "," + "Parent="+"'"+Entity.Parent+"'" + "," + "OrderNo="+"'"+Entity.OrderNo+"'"+" where MenuID='"+Entity.EntityOID.ToString()+"'";
            return objdata.ExecuteNonQuery(sqlstr);
        }

        /// </summary>
        ///Delete 方法 
        /// </summary>
        public int delete()
        {
            return objdata.ExecuteNonQuery("delete from Sys_Menu where MenuID='"+Entity.EntityOID.ToString()+"'");
        }

        /// <summary>
        /// 按条件查询，支持分页
        /// </summary>
        /// <param name="MenuTypeEquals">菜单类型</param>
        /// <param name="ParentEquals">父节点</param>
        /// <param name="AccID">账套</param>
        /// <returns></returns>
        public System.Collections.ICollection GetMenuListQuery(String MenuTypeEquals,String ParentEquals,String AccID,int startRow, int pageSize)
        {
            if (startRow < 0 || pageSize <= 0)
            {
                throw new Exception();
            }

            return GetListHelper(MenuTypeEquals, ParentEquals, AccID, startRow, pageSize);
        }

        /// <summary>
        /// 按条件查询，不支持分页
        /// </summary>
        /// <param name="MenuTypeEquals">菜单类型</param>
        /// <param name="ParentEquals">父节点</param>
        /// <param name="AccID">账套</param>
        /// <returns></returns>
        public System.Collections.ICollection GetMenuListQuery(String MenuTypeEquals, String ParentEquals, String AccID)
        {
            return GetListHelper(MenuTypeEquals, ParentEquals, AccID, START_ROW_INIT, START_ROW_INIT);
        }

        /// <summary>
        /// 返回集合的大小
        /// </summary>
        /// <param name="MenuTypeEquals">菜单类型</param>
        /// <param name="ParentEquals">父节点</param>
        /// <param name="AccID">账套</param>
        /// <returns></returns>
        public int GetMenuListCount(String MenuTypeEquals, String ParentEquals, String AccID)
        {
            string wherestr="";
            if (MenuTypeEquals != string.Empty)
            {
                wherestr=wherestr+" and MenuType='"+MenuTypeEquals+"'";
            }
            if (ParentEquals != string.Empty)
            {
                if (ParentEquals == "null")
                    wherestr = wherestr + " and isnull(Parent,'')=''";
                else if (ParentEquals == "notnull")
                    wherestr = wherestr + " and isnull(Parent,'')<>''";
                else
                    wherestr = wherestr + " and Parent='" + ParentEquals + "'";
            }
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and AccID='" + AccID + "'";
            }

            string count = objdata.ExecuteDataSet("select count(*) as cnt from Sys_Menu where 1=1 " + wherestr).Tables[0].Rows[0]["cnt"].ToString();
            return int.Parse(count);
        }

        /// <summary>
        /// 按条件查询，返回符合条件的集合
        /// </summary>
        /// <param name="MenuTypeEquals">菜单类型</param>
        /// <param name="ParentEquals">父节点</param>
        /// <param name="AccID">账套</param>
        /// <returns></returns>
        private System.Collections.ICollection GetListHelper(String MenuTypeEquals, String ParentEquals, String AccID, int startRow, int pageSize)
        {
            string wherestr="";
            if (MenuTypeEquals != string.Empty)
            {
                wherestr=wherestr+" and MenuType='"+MenuTypeEquals+"'";
            }
            if (ParentEquals != string.Empty)
            {
                if (ParentEquals == "null")
                    wherestr = wherestr + " and isnull(Parent,'')=''";
                else if (ParentEquals == "notnull")
                    wherestr = wherestr + " and isnull(Parent,'')<>''";
                else
                    wherestr = wherestr + " and Parent='" + ParentEquals + "'";
            }
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and AccID='" + AccID + "'";
            }

            System.Collections.IList entitys = null;
            if (startRow > START_ROW_INIT && pageSize > START_ROW_INIT)
            {
                entitys=Query(objdata.ExecSelect("Sys_Menu", wherestr ,startRow,pageSize, orderstr));
            }
            else
            {
                entitys=Query(objdata.ExecSelect("Sys_Menu", wherestr , START_ROW_INIT ,START_ROW_INIT, orderstr));
            }
            return entitys;
        }


        /// <summary>
        /// 按条件查询，返回符合条件的集合
        /// </summary>
        /// <param name="ParentEquals">上级目录</param>
        /// <param name="MenutypeEquals">类型</param>
        /// <returns></returns>
        public System.Collections.ICollection GetRightMenuList(String ParentEquals, String MenutypeEquals, String UserType, String AccID)
        {
            string wherestr = "";
            if (ParentEquals != string.Empty)
            {
                if (ParentEquals == "null")
                    wherestr = wherestr + " and isnull(Parent,'')=''";
                else if (ParentEquals == "notnull")
                    wherestr = wherestr + " and isnull(Parent,'')<>''";
                else
                    wherestr = wherestr + " and Parent='" + ParentEquals + "'";
            }
            if (MenutypeEquals != string.Empty)
            {
                wherestr = wherestr + " and (MenuType='" + MenutypeEquals + "' or MenuType='')";
            }
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and b.AccID='" + AccID + "'";
            }

            string TabName = "Sys_Menu b";
            if (UserType.ToUpper() != "ADMIN")
            {
                TabName = "Sys_User_Right a left join Sys_Menu b on a.MenuID=b.MenuID";
                wherestr = wherestr + " and a.UserType='" + UserType + "'";
            }
            System.Collections.IList entitys = null;
            entitys = Query(objdata.ExecSelect(TabName, "b.*", wherestr, START_ROW_INIT, START_ROW_INIT, orderstr));

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
                project.Entity.Sys.EntityMenu entity=new project.Entity.Sys.EntityMenu();
              
                entity.InnerEntityOID=dr["MenuID"].ToString();
                entity.MenuName=dr["MenuName"].ToString();
                entity.MenuType=dr["MenuType"].ToString();
                entity.MenuPath=dr["MenuPath"].ToString();
                entity.Flag=ParseIntForString(dr["Flag"].ToString());
                entity.Parent=dr["Parent"].ToString();
                entity.OrderNo = dr["OrderNo"].ToString();
                entity.AccID = dr["AccID"].ToString();
                result.Add(entity);
            }
            return result;
        }

    }
}
