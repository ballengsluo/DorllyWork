using System;
using System.Data;
namespace project.Business.Sys
{
    /// <summary>
    /// 用户登录日志的业务类
    /// </summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    public sealed class BusinessUserLog : project.Business.AbstractPmBusiness
    {
        private project.Entity.Sys.EntityUserLog _entity = new project.Entity.Sys.EntityUserLog();
        public string Userstr = "LogDate desc";
        Data objdata = new Data();

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public BusinessUserLog() { }

        /// <summary>
        /// 带参数的构函数
        /// </summary>
        /// <param name="entity">实体类</param>
        public BusinessUserLog(project.Entity.Sys.EntityUserLog entity)
        {
            this._entity = entity;
        }

        /// <summary>
        /// 与实体类(EntityUserLog)关联
        /// </summary>
        public project.Entity.Sys.EntityUserLog Entity
        {
            get { return _entity as project.Entity.Sys.EntityUserLog; }
        }

        /// </summary>
        ///load 方法 pid主键
        /// </summary>
        public void load(string id, string AccID)
        {
            DataRow dr = objdata.ExecuteDataSet("select * from Sys_User_Log where RowPointer='" + id + "' and AccID='" + AccID + "'").Tables[0].Rows[0];
            _entity.InnerEntityOID = dr["RowPointer"].ToString();
            _entity.LogUser = dr["LogUser"].ToString();
            _entity.AccID = dr["AccID"].ToString();
            _entity.LogType = dr["LogType"].ToString();
            _entity.LogDate = ParseDateTimeForString(dr["LogDate"].ToString());
            _entity.LogIP = dr["LogIP"].ToString();
        }

        /// </summary>
        ///Save方法
        /// </summary>
        public int Save()
        {
            string sqlstr = "";
            if (Entity.InnerEntityOID == null)
                sqlstr = "insert into Sys_User_Log(RowPointer,LogUser,AccID,LogType,LogDate,LogIP)" +
                    "values(NEWID(),'" + Entity.LogUser + "'," + "'" + Entity.AccID + "'" + "," + "'" + Entity.LogType + "'" + "," +
                    "'" + Entity.LogDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + Entity.LogIP + "'" + ")";
            else
                sqlstr = "update Sys_User_Log" +
                    " set LogUser=" + "'" + Entity.LogUser + "'" + "," +
                    "LogType=" + "'" + Entity.LogType + "'" + "," +
                    "LogDate=" + "'" + Entity.LogDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," +
                    "LogIP=" + "'" + Entity.LogIP + "'" +
                    " where RowPointer='" + Entity.InnerEntityOID + "'";
            return objdata.ExecuteNonQuery(sqlstr);
        }

        /// </summary>
        ///Delete 方法 
        /// </summary>
        public int delete()
        {
            return objdata.ExecuteNonQuery("delete from Sys_User_Log where RowPointer='" + Entity.InnerEntityOID + "' and AccID='" + Entity.AccID + "'");
        }

        /// <summary>
        /// 按条件查询，支持分页
        /// </summary>
        /// <param name="AccID">账户</param>
        /// <param name="LogUser">登录用户</param>
        /// <param name="LogType">用户类型</param>
        /// <param name="LogDate">登录日期</param>
        /// <returns></returns>
        public System.Collections.ICollection GetUserLogListQuery(String AccID, String LogUser, String LogType, DateTime MinLogDate, DateTime MaxLogDate, int startRow, int pageSize)
        {
            if (startRow < 0 || pageSize <= 0)
            {
                throw new Exception();
            }

            return GetListHelper(AccID, LogUser, LogType, MinLogDate, MaxLogDate, startRow, pageSize);
        }

        /// <summary>
        /// 按条件查询，不支持分页
        /// </summary>
        /// <param name="AccID">账户</param>
        /// <param name="LogUser">登录用户</param>
        /// <param name="LogType">用户类型</param>
        /// <param name="LogDate">登录日期</param>
        /// <returns></returns>
        public System.Collections.ICollection GetUserLogListQuery(String AccID, String LogUser, String LogType, DateTime MinLogDate, DateTime MaxLogDate)
        {
            return GetListHelper(AccID, LogUser, LogType, MinLogDate, MaxLogDate, START_ROW_INIT, START_ROW_INIT);
        }

        /// <summary>
        /// 返回集合的大小
        /// </summary>
        /// <param name="AccID">账户</param>
        /// <param name="LogUser">登录用户</param>
        /// <param name="LogType">用户类型</param>
        /// <param name="LogDate">登录日期</param>
        /// <returns></returns>
        public int GetUserLogListCount(String AccID, String LogUser, String LogType, DateTime MinLogDate, DateTime MaxLogDate)
        {
            string wherestr = "";
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and AccID like '" + AccID + "'";
            }
            if (LogUser != string.Empty)
            {
                wherestr = wherestr + " and LogUser like '" + LogUser + "'";
            }
            if (LogType != string.Empty)
            {
                wherestr = wherestr + " and LogType like '" + LogType + "'";
            }
            if (MinLogDate != default(DateTime))
            {
                wherestr = wherestr + " and convert(nvarchar(10),LogDate,121)>='" + MinLogDate.ToString("yyyy-MM-dd") + "'";
            }
            if (MaxLogDate != default(DateTime))
            {
                wherestr = wherestr + " and convert(nvarchar(10),LogDate,121)<='" + MaxLogDate.ToString("yyyy-MM-dd") + "'";
            }

            string count = objdata.ExecuteDataSet("select count(*) as cnt from Sys_User_Log where 1=1 " + wherestr).Tables[0].Rows[0]["cnt"].ToString();
            return int.Parse(count);
        }

        /// <summary>
        /// 按条件查询，返回符合条件的集合
        /// </summary>
        /// <param name="AccID">账户</param>
        /// <param name="LogUser">登录用户</param>
        /// <param name="LogType">用户类型</param>
        /// <param name="LogDate">登录日期</param>
        /// <returns></returns>
        private System.Collections.ICollection GetListHelper(String AccID, String LogUser, String LogType, DateTime MinLogDate, DateTime MaxLogDate, int startRow, int pageSize)
        {
            string wherestr = "";
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and AccID like '" + AccID + "'";
            }
            if (LogUser != string.Empty)
            {
                wherestr = wherestr + " and LogUser like '" + LogUser + "'";
            }
            if (LogType != string.Empty)
            {
                wherestr = wherestr + " and LogType like '" + LogType + "'";
            }
            if (MinLogDate != default(DateTime))
            {
                wherestr = wherestr + " and convert(nvarchar(10),LogDate,121)>='" + MinLogDate.ToString("yyyy-MM-dd") + "'";
            }
            if (MaxLogDate != default(DateTime))
            {
                wherestr = wherestr + " and convert(nvarchar(10),LogDate,121)<='" + MaxLogDate.ToString("yyyy-MM-dd") + "'";
            }

            System.Collections.IList entitys = null;
            if (startRow > START_ROW_INIT && pageSize > START_ROW_INIT)
            {
                entitys = Query(objdata.ExecSelect("Sys_User_Log", wherestr, startRow, pageSize, Userstr));
            }
            else
            {
                entitys = Query(objdata.ExecSelect("Sys_User_Log", wherestr, START_ROW_INIT, START_ROW_INIT, Userstr));
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
                project.Entity.Sys.EntityUserLog entity = new project.Entity.Sys.EntityUserLog();
                entity.InnerEntityOID = dr["RowPointer"].ToString();
                entity.LogUser = dr["LogUser"].ToString();
                entity.AccID = dr["AccID"].ToString();
                entity.LogType = dr["LogType"].ToString();
                entity.LogDate = ParseDateTimeForString(dr["LogDate"].ToString());
                entity.LogIP = dr["LogIP"].ToString();
                result.Add(entity);
            }
            return result;
        }

    }
}
