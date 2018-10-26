using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Data.SqlClient;

public class Data
{
    public Data()
    {
    }

    public static SqlConnection Conn()
    {
        return new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString);
    }

    /// <summary>
    /// 执行SQL语句并返回受影响的行的数目
    /// </summary>
    /// <param name="cmdText">数据库命令字符串</param>
    /// <returns>受影响的行的数目</returns>
    public int ExecuteNonQuery(string cmdText)
    {
        SqlConnection con = Conn();
        SqlCommand cmd = null;
        try
        {
            cmd = new SqlCommand(cmdText, con);
            con.Open();
            return cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
            return 0;
        }
        finally
        {
            if (cmd != null)
                cmd.Dispose();
            if (con != null)
                con.Dispose();
        }
    }
    /// <summary>
    /// 执行SQL语句并返回数据行
    /// </summary>
    /// <param name="cmdText">数据库命令字符串</param>
    /// <returns>数据读取器接口</returns>
    public SqlDataReader ExecuteReader(string cmdText)
    {
        SqlConnection con = null;
        SqlCommand cmd = null;
        try
        {
            con = Conn();
            cmd = new SqlCommand(cmdText, con);
            con.Open();
            return cmd.ExecuteReader();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
            return null;
        }
    }

    /// <summary>
    /// 填充一个数据集对象并返回
    /// </summary>
    /// <param name="cmdText">数据库命令字符串</param>
    /// <returns>数据集对象</returns>
    public DataSet ExecuteDataSet(string cmdText)
    {
        SqlConnection con = null;
        SqlCommand cmd = null;
        SqlDataAdapter da = null;
        DataSet ds = null;
        try
        {
            con = Conn();
            cmd = new SqlCommand(cmdText, con);
            da = new SqlDataAdapter(cmd);
            ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
            return null;
        }
        finally
        {
            if (cmd != null)
                cmd.Dispose();
            if (con != null)
                con.Dispose();
        }
    }

    /// <summary>
    /// 记录通过存储过程exec_select查询
    /// </summary>
    /// <param name="table">表名</param>
    /// <param name="wherestr">where条件</param>
    /// <param name="pageSize">每页显示行数</param>
    /// <param name="pageIndex">第几页</param>
    /// <param name="orderstr">排序字段</param>
    /// <returns></returns>
    public System.Data.DataTable ExecSelect(string table, string wherestr, int pageIndex, int pageSize, string orderstr)
    {
        SqlConnection con = null;
        SqlCommand cmd = null;
        DataSet ds = new DataSet();
        try
        {
            con = Conn();
            cmd = new SqlCommand("GetRecordList", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter[] parameter = new SqlParameter[] { 
                new SqlParameter("@table",SqlDbType.NVarChar,2000),
                new SqlParameter("@pageSize",SqlDbType.Int),
                new SqlParameter("@pageIndex",SqlDbType.Int),
                new SqlParameter("@fieldstr",SqlDbType.NVarChar,2000),
                new SqlParameter("@wherestr",SqlDbType.NVarChar,2000),
                new SqlParameter("@orderstr",SqlDbType.NVarChar,2000)
            };
            parameter[0].Value = table;
            parameter[1].Value = pageSize;
            parameter[2].Value = pageIndex;
            parameter[3].Value = "*";
            parameter[4].Value = wherestr;
            parameter[5].Value = orderstr;
            cmd.Parameters.AddRange(parameter);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);

            return ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
            return null;
        }
        finally
        {
            if (cmd != null)
                cmd.Dispose();
            if (con != null)
                con.Dispose();
        }
    }

    /// <summary>
    /// 记录通过存储过程exec_select查询
    /// </summary>
    /// <param name="table">表名</param>
    /// <param name="fieldstr">字段内容</param>
    /// <param name="wherestr">where条件</param>
    /// <param name="pageSize">每页显示行数</param>
    /// <param name="pageIndex">第几页</param>
    /// <param name="orderstr">排序字段</param>
    /// <returns></returns>
    public System.Data.DataTable ExecSelect(string table, string fieldstr, string wherestr, int pageIndex, int pageSize, string orderstr)
    {
        SqlConnection con = null;
        SqlCommand cmd = null;
        DataSet ds = new DataSet();

        try
        {
            con = Conn();
            cmd = new SqlCommand("GetRecordList", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter[] parameter = new SqlParameter[] { 
                new SqlParameter("@table",SqlDbType.NVarChar,2000),
                new SqlParameter("@pageSize",SqlDbType.Int),
                new SqlParameter("@pageIndex",SqlDbType.Int),
                new SqlParameter("@fieldstr",SqlDbType.NVarChar,2000),
                new SqlParameter("@wherestr",SqlDbType.NVarChar,2000),
                new SqlParameter("@orderstr",SqlDbType.NVarChar,2000)
            };
            parameter[0].Value = table;
            parameter[1].Value = pageSize;
            parameter[2].Value = pageIndex;
            parameter[3].Value = fieldstr;
            parameter[4].Value = wherestr;
            parameter[5].Value = orderstr;
            cmd.Parameters.AddRange(parameter);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);

            return ds.Tables[0];
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
            return null;
        }
        finally
        {
            if (cmd != null)
                cmd.Dispose();
            if (con != null)
                con.Dispose();
        }
    }
}
