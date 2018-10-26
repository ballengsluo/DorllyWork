using System;
using System.Data;
namespace project.Business.Base
{
    /// <summary>
    /// 意见反馈的业务类
    /// </summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    public sealed class BusinessOpinion : project.Business.AbstractPmBusiness
    {
        private project.Entity.Base.EntityOpinion _entity = new project.Entity.Base.EntityOpinion();
        public string orderstr = "OpNo";
        Data objdata = new Data();

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public BusinessOpinion() { }

        /// <summary>
        /// 带参数的构函数
        /// </summary>
        /// <param name="entity">实体类</param>
        public BusinessOpinion(project.Entity.Base.EntityOpinion entity)
        {
            this._entity = entity;
        }

        /// <summary>
        /// 与实体类(EntityOpinion)关联
        /// </summary>
        public project.Entity.Base.EntityOpinion Entity
        {
            get { return _entity as project.Entity.Base.EntityOpinion; }
        }

        /// </summary>
        ///load 方法 pid主键
        /// </summary>
        public void load(string id)
        {
            DataRow dr = objdata.ExecuteDataSet("select * from Base_Opinion where RowPointer='" + id + "'").Tables[0].Rows[0];
            _entity.RowPointer = dr["RowPointer"].ToString();
            _entity.AccID = dr["AccID"].ToString();
            _entity.FilesName = dr["FilesName"].ToString();
            _entity.Content = dr["Content"].ToString();
            _entity.CreateUser = dr["CreateUser"].ToString();
            _entity.CreateDate = ParseDateTimeForString(dr["CreateDate"].ToString());
            _entity.IsSolved = bool.Parse(dr["IsSolved"].ToString());
            _entity.SolveUser = dr["SolveUser"].ToString();
            _entity.SolveDate = ParseDateTimeForString(dr["SolveDate"].ToString());
        }
        
        /// </summary>
        ///Save方法
        /// </summary>
        public int Save()
        {
            string sqlstr = "";
            if (Entity.RowPointer == null)
                sqlstr = "insert into Base_Opinion(RowPointer,AccID,FilesName,Content,CreateUser,CreateDate,IsSolved)" +
                    "values(NEWID(),'" + Entity.AccID + "'," + "'" + Entity.FilesName + "'" + "," + "'" + Entity.Content + "'" + "," +
                    "'" + Entity.CreateUser + "'" + "," + "'" + Entity.CreateDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" + ",0)";
            else
                sqlstr = "update Base_Opinion" +
                    " set FilesName=" + "'" + Entity.FilesName + "'" + "," + "Content=" + "'" + Entity.Content + "'" +
                    " where RowPointer='" + Entity.RowPointer + "'";
            return objdata.ExecuteNonQuery(sqlstr);
        }

        /// </summary>
        ///Delete 方法 
        /// </summary>
        public int delete()
        {
            return objdata.ExecuteNonQuery("delete from Base_Opinion where RowPointer='" + Entity.RowPointer + "'");
        }

        /// </summary>
        ///Delete 方法 
        /// </summary>
        public int solve()
        {
            return objdata.ExecuteNonQuery("update Base_Opinion set IsSolved=1,SolveUser='" + Entity.SolveUser + "',"+
                "SolveDate='" + Entity.SolveDate.ToString("yyyy-MM-dd HH:mm:ss") + "' "+
                "where RowPointer='" + Entity.RowPointer + "'");
        }

        /// <summary>
        /// 按条件查询，支持分页
        /// </summary>
        /// <param name="MinCreateDate">起始日期</param>
        /// <param name="MaxCreateDate">截止日期</param>
        /// <param name="IsSolved">是否处理</param>
        /// <param name="AccID">账套</param>
        /// <param name="startRow"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public System.Collections.ICollection GetOpListQuery(DateTime MinCreateDate, DateTime MaxCreateDate, bool? IsSolved, string AccID, int startRow, int pageSize)
        {
            if (startRow < 0 || pageSize <= 0)
            {
                throw new Exception();
            }

            return GetListHelper(MinCreateDate, MaxCreateDate, IsSolved, AccID, startRow, pageSize);
        }

        /// <summary>
        /// 按条件查询，不支持分页
        /// </summary>
        /// <param name="MinCreateDate">起始日期</param>
        /// <param name="MaxCreateDate">截止日期</param>
        /// <param name="IsSolved">是否处理</param>
        /// <param name="AccID">账套</param>
        /// <returns></returns>
        public System.Collections.ICollection GetOpListQuery(DateTime MinCreateDate, DateTime MaxCreateDate, bool? IsSolved, string AccID)
        {
            return GetListHelper(MinCreateDate, MaxCreateDate, IsSolved, AccID, START_ROW_INIT, START_ROW_INIT);
        }

        /// <summary>
        /// 返回集合的大小
        /// </summary>
        /// <param name="MinCreateDate">起始日期</param>
        /// <param name="MaxCreateDate">截止日期</param>
        /// <param name="IsSolved">是否处理</param>
        /// <param name="AccID">账套</param>
        /// <returns></returns>
        public int GetOpListCount(DateTime MinCreateDate, DateTime MaxCreateDate, bool? IsSolved, string AccID)
        {
            string wherestr = "";
            if (MinCreateDate != default(DateTime))
            {
                wherestr = wherestr + " and CONVERT(NVARCHAR(10),CreateDate,121) >= '" + MinCreateDate.ToString("yyyy-MM-dd") + "'";
            }
            if (MaxCreateDate != default(DateTime))
            {
                wherestr = wherestr + " and CONVERT(NVARCHAR(10),CreateDate,121) <= '" + MaxCreateDate.ToString("yyyy-MM-dd") + "'";
            }
            if (IsSolved != null)
            {
                wherestr = wherestr + " and IsSolved=" + (IsSolved == true ? "1" : "0");
            }
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and AccID='" + AccID + "'";
            }

            string count = objdata.ExecuteDataSet("select count(*) as cnt from Base_Opinion where 1=1 " + wherestr).Tables[0].Rows[0]["cnt"].ToString();
            return int.Parse(count);
        }

        /// <summary>
        /// 按条件查询，返回符合条件的集合
        /// </summary>
        /// <param name="MinCreateDate">起始日期</param>
        /// <param name="MaxCreateDate">截止日期</param>
        /// <param name="IsSolved">是否处理</param>
        /// <param name="AccID">账套</param>
        /// <returns></returns>
        private System.Collections.ICollection GetListHelper(DateTime MinCreateDate, DateTime MaxCreateDate, bool? IsSolved, string AccID, int startRow, int pageSize)
        {
            string wherestr = "";
            if (MinCreateDate != default(DateTime))
            {
                wherestr = wherestr + " and CONVERT(NVARCHAR(10),CreateDate,121) >= '" + MinCreateDate.ToString("yyyy-MM-dd") + "'";
            }
            if (MaxCreateDate != default(DateTime))
            {
                wherestr = wherestr + " and CONVERT(NVARCHAR(10),CreateDate,121) <= '" + MaxCreateDate.ToString("yyyy-MM-dd") + "'";
            }
            if (IsSolved != null)
            {
                wherestr = wherestr + " and IsSolved=" + (IsSolved == true ? "1" : "0");
            }
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and AccID='" + AccID + "'";
            }

            System.Collections.IList entitys = null;
            if (startRow > START_ROW_INIT && pageSize > START_ROW_INIT)
            {
                entitys = Query(objdata.ExecSelect("Base_Opinion", wherestr, startRow, pageSize, orderstr));
            }
            else
            {
                entitys = Query(objdata.ExecSelect("Base_Opinion", wherestr, START_ROW_INIT, START_ROW_INIT, orderstr));
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
                project.Entity.Base.EntityOpinion entity = new project.Entity.Base.EntityOpinion();
                entity.RowPointer = dr["RowPointer"].ToString();
                entity.AccID = dr["AccID"].ToString();
                entity.FilesName = dr["FilesName"].ToString();
                entity.Content = dr["Content"].ToString();
                entity.CreateUser = dr["CreateUser"].ToString();
                entity.CreateDate = ParseDateTimeForString(dr["CreateDate"].ToString());
                entity.IsSolved = bool.Parse(dr["IsSolved"].ToString());
                entity.SolveUser = dr["SolveUser"].ToString();
                entity.SolveDate = ParseDateTimeForString(dr["SolveDate"].ToString());
                result.Add(entity);
            }
            return result;
        }

    }
}
