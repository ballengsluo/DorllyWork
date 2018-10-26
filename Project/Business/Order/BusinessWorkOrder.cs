using System;
using System.Data;
using System.Data.SqlClient;
namespace project.Business.Order
{
    /// <summary>
    /// 用户信息的业务类
    /// </summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    public sealed class BusinessWorkOrder : project.Business.AbstractPmBusiness
    {
        private project.Entity.Order.EntityWorkOrder _entity = new project.Entity.Order.EntityWorkOrder();
        public string orderstr = "OrderNo DESC";
        Data objdata = new Data();

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public BusinessWorkOrder() { }

        /// <summary>
        /// 带参数的构函数
        /// </summary>
        /// <param name="entity">实体类</param>
        public BusinessWorkOrder(project.Entity.Order.EntityWorkOrder entity)
        {
            this._entity = entity;
        }

        /// <summary>
        /// 与实体类(EntityWorkOrder)关联
        /// </summary>
        public project.Entity.Order.EntityWorkOrder Entity
        {
            get { return _entity as project.Entity.Order.EntityWorkOrder; }
        }

        /// </summary>
        ///load 方法 pid主键
        /// </summary>
        public void load(string id)
        {
            DataRow dr = objdata.ExecuteDataSet("select * from WO_WorkOrder where RowPointer='" + id + "'").Tables[0].Rows[0];
            _entity.InnerEntityOID = dr["RowPointer"].ToString();
            _entity.AccID = dr["AccID"].ToString();
            _entity.OrderNo = dr["OrderNo"].ToString();
            _entity.OrderName = dr["OrderName"].ToString();
            _entity.OrderDate = ParseDateTimeForString(dr["OrderDate"].ToString());
            _entity.OrderType = dr["OrderType"].ToString();
            _entity.Status = dr["Status"].ToString();
            _entity.SaleNo = dr["SaleNo"].ToString();
            _entity.AlloDept = dr["AlloDept"].ToString();
            _entity.AlloUser = dr["AlloUser"].ToString();
            _entity.CustNo = dr["CustNo"].ToString();
            _entity.LinkMan = dr["LinkMan"].ToString();
            _entity.LinkTel = dr["LinkTel"].ToString();
            _entity.Addr = dr["Addr"].ToString();
            _entity.Remark = dr["Remark"].ToString();
            _entity.Region = dr["Region"].ToString();
            _entity.CreateTime = ParseDateTimeForString(dr["CreateTime"].ToString());
            _entity.CustneedTime = ParseDateTimeForString(dr["CustneedTime"].ToString());
            _entity.ResponseTime = ParseDateTimeForString(dr["ResponseTime"].ToString());
            _entity.AppoIntTime = ParseDateTimeForString(dr["AppoIntTime"].ToString());
            _entity.SignTime = ParseDateTimeForString(dr["SignTime"].ToString());
            _entity.WorkTime = ParseDateTimeForString(dr["WorkTime"].ToString());
            _entity.FinishTime = ParseDateTimeForString(dr["FinishTime"].ToString());
            _entity.CloseTime = ParseDateTimeForString(dr["CloseTime"].ToString());
            _entity.ConfirmTime = ParseDateTimeForString(dr["ConfirmTime"].ToString());
            _entity.CreateUser = dr["CreateUser"].ToString();
            _entity.UpdateDate = ParseDateTimeForString(dr["UpdateDate"].ToString());
            _entity.UpdateUser = dr["UpdateUser"].ToString();
            _entity.IsHangUp = bool.Parse(dr["IsHangUp"].ToString());
            _entity.HangUpDate = ParseDateTimeForString(dr["HangUpDate"].ToString());
            _entity.HangUpReason = dr["HangUpReason"].ToString();
            _entity.IsApply = bool.Parse(dr["IsApply"].ToString());
            _entity.ApplyDate = ParseDateTimeForString(dr["ApplyDate"].ToString());
            _entity.ApplyReason = dr["ApplyReason"].ToString();
            _entity.IsBack = bool.Parse(dr["IsBack"].ToString());
            _entity.BackDate = ParseDateTimeForString(dr["BackDate"].ToString());
            _entity.BackReason = dr["BackReason"].ToString();
            _entity.IsDel = bool.Parse(dr["IsDel"].ToString());
            _entity.DONo = dr["DONo"].ToString();
        }

       /// <summary>
       /// Load
       /// </summary>
       /// <param name="orderNo"></param>
       /// <param name="accID"></param>
        public void loadOrderNo(string orderNo, string accID)
        {
            DataRow dr = objdata.ExecuteDataSet("select * from WO_WorkOrder where OrderNo='" + orderNo + "' and AccID='" + accID + "'").Tables[0].Rows[0];
            _entity.InnerEntityOID = dr["RowPointer"].ToString();
            _entity.AccID = dr["AccID"].ToString();
            _entity.OrderNo = dr["OrderNo"].ToString();
            _entity.OrderName = dr["OrderName"].ToString();
            _entity.OrderDate = ParseDateTimeForString(dr["OrderDate"].ToString());
            _entity.OrderType = dr["OrderType"].ToString();
            _entity.Status = dr["Status"].ToString();
            _entity.SaleNo = dr["SaleNo"].ToString();
            _entity.AlloDept = dr["AlloDept"].ToString();
            _entity.AlloUser = dr["AlloUser"].ToString();
            _entity.CustNo = dr["CustNo"].ToString();
            _entity.LinkMan = dr["LinkMan"].ToString();
            _entity.LinkTel = dr["LinkTel"].ToString();
            _entity.Region = dr["Region"].ToString();
            _entity.Addr = dr["Addr"].ToString();
            _entity.Remark = dr["Remark"].ToString();
            _entity.CreateTime = ParseDateTimeForString(dr["CreateTime"].ToString());
            _entity.CustneedTime = ParseDateTimeForString(dr["CustneedTime"].ToString());
            _entity.ResponseTime = ParseDateTimeForString(dr["ResponseTime"].ToString());
            _entity.AppoIntTime = ParseDateTimeForString(dr["AppoIntTime"].ToString());
            _entity.SignTime = ParseDateTimeForString(dr["SignTime"].ToString());
            _entity.WorkTime = ParseDateTimeForString(dr["WorkTime"].ToString());
            _entity.FinishTime = ParseDateTimeForString(dr["FinishTime"].ToString());
            _entity.CloseTime = ParseDateTimeForString(dr["CloseTime"].ToString());
            _entity.ConfirmTime = ParseDateTimeForString(dr["ConfirmTime"].ToString());
            _entity.CreateUser = dr["CreateUser"].ToString();
            _entity.UpdateDate = ParseDateTimeForString(dr["UpdateDate"].ToString());
            _entity.UpdateUser = dr["UpdateUser"].ToString();
            _entity.IsHangUp = bool.Parse(dr["IsHangUp"].ToString());
            _entity.HangUpDate = ParseDateTimeForString(dr["HangUpDate"].ToString());
            _entity.HangUpReason = dr["HangUpReason"].ToString();
            _entity.IsApply = bool.Parse(dr["IsApply"].ToString());
            _entity.ApplyDate = ParseDateTimeForString(dr["ApplyDate"].ToString());
            _entity.ApplyReason = dr["ApplyReason"].ToString();
            _entity.IsBack = bool.Parse(dr["IsBack"].ToString());
            _entity.BackDate = ParseDateTimeForString(dr["BackDate"].ToString());
            _entity.BackReason = dr["BackReason"].ToString();
            _entity.IsDel = bool.Parse(dr["IsDel"].ToString());
            _entity.DONo = dr["DONo"].ToString();
        }

        /// </summary>
        ///Save方法
        /// </summary>
        public int Save()
        {
            string CustneedTime = "null";
            if (Entity.CustneedTime.Year > 2000)
                CustneedTime = "'" + Entity.CustneedTime.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            string sqlstr = "";
            if (Entity.InnerEntityOID == null)
                sqlstr = @"insert into WO_WorkOrder(RowPointer,AccID,OrderNo,OrderName,OrderDate,OrderType,Status,SaleNo,AlloDept,AlloUser,CustNo,LinkMan,LinkTel,Addr,Region," +
                    "CreateTime,CustneedTime,ResponseTime,AppoIntTime,SignTime,WorkTime,FinishTime,CloseTime,ConfirmTime,Remark,CreateUser,UpdateUser,UpdateDate," +
                    "IsHangUp,HangUpDate,HangUpReason,IsApply,ApplyDate,ApplyReason,IsBack,BackDate,BackReason,IsDel,DONo)" +
                    "values(newid()," + "'" + Entity.AccID + "'" + "," + "'" + Entity.OrderNo + "'" + "," + "'" + Entity.OrderName + "'" + "," +
                    "'" + Entity.OrderDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + Entity.OrderType + "'" + "," + "'" + Entity.Status + "'" + "," +
                    "'" + Entity.SaleNo + "'" + "," + "'" + Entity.AlloDept + "'" + "," + "'" + Entity.AlloUser + "'" + "," +
                    "'" + Entity.CustNo + "'" + "," + "'" + Entity.LinkMan + "'" + "," + "'" + Entity.LinkTel + "'" + "," + "'" + Entity.Addr + "'" + "," +
                    "'" + Entity.Region + "'" + "," + "'" + Entity.CreateTime.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," +
                     CustneedTime + "," +
                    "null,null,null,null,null,null,null," +
                    "'" + Entity.Remark + "'" + "," + "'" + Entity.CreateUser + "'" + "," +
                    "'" + Entity.UpdateUser + "'" + "," + "'" + Entity.UpdateDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," +
                    "0,null,null,0,null,null,0,null,null,0," + "'" + Entity.DONo + "')";
            else
                sqlstr = "update WO_WorkOrder" +
                    " set OrderName=" + "'" + Entity.OrderName + "'" + "," + "OrderDate=" + "'" + Entity.OrderDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," +
                    "OrderType=" + "'" + Entity.OrderType + "'" + "," + "SaleNo=" + "'" + Entity.SaleNo + "'" + "," +
                    "AlloDept=" + "'" + Entity.AlloDept + "'" + "," + "AlloUser=" + "'" + Entity.AlloUser + "'" + "," +
                    "CustNo=" + "'" + Entity.CustNo + "'" + "," + "LinkMan=" + "'" + Entity.LinkMan + "'" + "," + "LinkTel=" + "'" + Entity.LinkTel + "'" + "," +
                    "Addr=" + "'" + Entity.Addr + "'" + "," + "Region=" + "'" + Entity.Region + "'" + "," + "Remark=" + "'" + Entity.Remark + "'" + "," +
                    "CustneedTime=" + CustneedTime + "," +
                    "UpdateUser=" + "'" + Entity.UpdateUser + "'" + "," + "UpdateDate=" + "'" + Entity.UpdateDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                    " where RowPointer='" + Entity.InnerEntityOID.ToString() + "'";
            return objdata.ExecuteNonQuery(sqlstr);
        }

        /// <summary>
        /// 响应
        /// </summary>
        /// <returns></returns>
        public string response(string AccID, string OrderNo, string GPS_X, string GPS_Y, string UserID, string UserName)
        {
            string InfoMsg = "";
            SqlConnection con = null;
            SqlCommand command = null;
            try
            {
                con = Data.Conn();
                command = new SqlCommand("ExecResponseOrder", con);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@AccID", SqlDbType.NVarChar, 30).Value = AccID;
                command.Parameters.Add("@OrderNo", SqlDbType.NVarChar, 30).Value = OrderNo;
                command.Parameters.Add("@GPS_X", SqlDbType.NVarChar, 30).Value = GPS_X;
                command.Parameters.Add("@GPS_Y", SqlDbType.NVarChar, 30).Value = GPS_Y;
                command.Parameters.Add("@UserID", SqlDbType.NVarChar, 30).Value = UserID;
                command.Parameters.Add("@UserName", SqlDbType.NVarChar, 30).Value = UserName;
                command.Parameters.Add("@InfoMsg", SqlDbType.NVarChar, 3000).Direction = ParameterDirection.Output;
                con.Open();
                command.ExecuteNonQuery();
                InfoMsg = command.Parameters["@InfoMsg"].Value.ToString();

            }
            catch (Exception ex)
            {
                InfoMsg = ex.ToString();
            }
            finally
            {
                if (command != null)
                    command.Dispose();
                if (con != null)
                    con.Dispose();
            }
            return @InfoMsg;
        }

        /// <summary>
        /// 预约
        /// </summary>
        /// <returns></returns>
        public string appoInt(string AccID, string OrderNo, string AppoIntTime, string GPS_X, string GPS_Y, string UserID, string UserName)
        {
            string InfoMsg = "";
            SqlConnection con = null;
            SqlCommand command = null;
            try
            {
                con = Data.Conn();
                command = new SqlCommand("ExecAppoIntOrder", con);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@AccID", SqlDbType.NVarChar, 30).Value = AccID;
                command.Parameters.Add("@OrderNo", SqlDbType.NVarChar, 30).Value = OrderNo;
                command.Parameters.Add("@AppoIntTime", SqlDbType.NVarChar, 30).Value = AppoIntTime;
                command.Parameters.Add("@GPS_X", SqlDbType.NVarChar, 30).Value = GPS_X;
                command.Parameters.Add("@GPS_Y", SqlDbType.NVarChar, 30).Value = GPS_Y;
                command.Parameters.Add("@UserID", SqlDbType.NVarChar, 30).Value = UserID;
                command.Parameters.Add("@UserName", SqlDbType.NVarChar, 30).Value = UserName;
                command.Parameters.Add("@InfoMsg", SqlDbType.NVarChar, 3000).Direction = ParameterDirection.Output;
                con.Open();
                command.ExecuteNonQuery();
                InfoMsg = command.Parameters["@InfoMsg"].Value.ToString();

            }
            catch (Exception ex)
            {
                InfoMsg = ex.ToString();
            }
            finally
            {
                if (command != null)
                    command.Dispose();
                if (con != null)
                    con.Dispose();
            }
            return @InfoMsg;
        }

        /// <summary>
        /// 签到
        /// </summary>
        /// <returns></returns>
        public string sign(string AccID, string OrderNo, string GPS_X, string GPS_Y, string UserID, string UserName)
        {
            string InfoMsg = "";
            SqlConnection con = null;
            SqlCommand command = null;
            try
            {
                con = Data.Conn();
                command = new SqlCommand("ExecSignOrder", con);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@AccID", SqlDbType.NVarChar, 30).Value = AccID;
                command.Parameters.Add("@OrderNo", SqlDbType.NVarChar, 30).Value = OrderNo;
                command.Parameters.Add("@GPS_X", SqlDbType.NVarChar, 30).Value = GPS_X;
                command.Parameters.Add("@GPS_Y", SqlDbType.NVarChar, 30).Value = GPS_Y;
                command.Parameters.Add("@UserID", SqlDbType.NVarChar, 30).Value = UserID;
                command.Parameters.Add("@UserName", SqlDbType.NVarChar, 30).Value = UserName;
                command.Parameters.Add("@InfoMsg", SqlDbType.NVarChar, 3000).Direction = ParameterDirection.Output;
                con.Open();
                command.ExecuteNonQuery();
                InfoMsg = command.Parameters["@InfoMsg"].Value.ToString();

            }
            catch (Exception ex)
            {
                InfoMsg = ex.ToString();
            }
            finally
            {
                if (command != null)
                    command.Dispose();
                if (con != null)
                    con.Dispose();
            }
            return @InfoMsg;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <returns></returns>
        public string work(string AccID, string OrderNo, string GPS_X, string GPS_Y, string UserID, string UserName)
        {
            string InfoMsg = "";
            SqlConnection con = null;
            SqlCommand command = null;
            try
            {
                con = Data.Conn();
                command = new SqlCommand("ExecWorkOrder", con);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@AccID", SqlDbType.NVarChar, 30).Value = AccID;
                command.Parameters.Add("@OrderNo", SqlDbType.NVarChar, 30).Value = OrderNo;
                command.Parameters.Add("@GPS_X", SqlDbType.NVarChar, 30).Value = GPS_X;
                command.Parameters.Add("@GPS_Y", SqlDbType.NVarChar, 30).Value = GPS_Y;
                command.Parameters.Add("@UserID", SqlDbType.NVarChar, 30).Value = UserID;
                command.Parameters.Add("@UserName", SqlDbType.NVarChar, 30).Value = UserName;
                command.Parameters.Add("@InfoMsg", SqlDbType.NVarChar, 3000).Direction = ParameterDirection.Output;
                con.Open();
                command.ExecuteNonQuery();
                InfoMsg = command.Parameters["@InfoMsg"].Value.ToString();

            }
            catch (Exception ex)
            {
                InfoMsg = ex.ToString();
            }
            finally
            {
                if (command != null)
                    command.Dispose();
                if (con != null)
                    con.Dispose();
            }
            return @InfoMsg;
        }

        /// <summary>
        /// 完成
        /// </summary>
        /// <returns></returns>
        public string finish(string AccID, string OrderNo, string GPS_X, string GPS_Y, string UserID, string UserName)
        {
            string InfoMsg = "";
            SqlConnection con = null;
            SqlCommand command = null;
            try
            {
                con = Data.Conn();
                command = new SqlCommand("ExecFinishOrder", con);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@AccID", SqlDbType.NVarChar, 30).Value = AccID;
                command.Parameters.Add("@OrderNo", SqlDbType.NVarChar, 30).Value = OrderNo;
                command.Parameters.Add("@GPS_X", SqlDbType.NVarChar, 30).Value = GPS_X;
                command.Parameters.Add("@GPS_Y", SqlDbType.NVarChar, 30).Value = GPS_Y;
                command.Parameters.Add("@UserID", SqlDbType.NVarChar, 30).Value = UserID;
                command.Parameters.Add("@UserName", SqlDbType.NVarChar, 30).Value = UserName;
                command.Parameters.Add("@InfoMsg", SqlDbType.NVarChar, 3000).Direction = ParameterDirection.Output;
                con.Open();
                command.ExecuteNonQuery();
                InfoMsg = command.Parameters["@InfoMsg"].Value.ToString();

            }
            catch (Exception ex)
            {
                InfoMsg = ex.ToString();
            }
            finally
            {
                if (command != null)
                    command.Dispose();
                if (con != null)
                    con.Dispose();
            }
            return @InfoMsg;
        }

        /// <summary>
        /// 销单
        /// </summary>
        /// <returns></returns>
        public string close(string AccID, string OrderNo, string GPS_X, string GPS_Y, string UserID, string UserName)
        {
            string InfoMsg = "";
            SqlConnection con = null;
            SqlCommand command = null;
            try
            {
                con = Data.Conn();
                command = new SqlCommand("ExecCloseOrder", con);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@AccID", SqlDbType.NVarChar, 30).Value = AccID;
                command.Parameters.Add("@OrderNo", SqlDbType.NVarChar, 30).Value = OrderNo;
                command.Parameters.Add("@GPS_X", SqlDbType.NVarChar, 30).Value = GPS_X;
                command.Parameters.Add("@GPS_Y", SqlDbType.NVarChar, 30).Value = GPS_Y;
                command.Parameters.Add("@UserID", SqlDbType.NVarChar, 30).Value = UserID;
                command.Parameters.Add("@UserName", SqlDbType.NVarChar, 30).Value = UserName;
                command.Parameters.Add("@InfoMsg", SqlDbType.NVarChar, 3000).Direction = ParameterDirection.Output;
                con.Open();
                command.ExecuteNonQuery();
                InfoMsg = command.Parameters["@InfoMsg"].Value.ToString();

            }
            catch (Exception ex)
            {
                InfoMsg = ex.ToString();
            }
            finally
            {
                if (command != null)
                    command.Dispose();
                if (con != null)
                    con.Dispose();
            }
            return @InfoMsg;
        }

        /// <summary>
        /// 确认销单
        /// </summary>
        /// <returns></returns>
        public string confirm(string AccID, string OrderNo, string GPS_X, string GPS_Y, string UserID, string UserName)
        {
            string InfoMsg = "";
            SqlConnection con = null;
            SqlCommand command = null;
            try
            {
                con = Data.Conn();
                command = new SqlCommand("ExecConfirmOrder", con);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@AccID", SqlDbType.NVarChar, 30).Value = AccID;
                command.Parameters.Add("@OrderNo", SqlDbType.NVarChar, 30).Value = OrderNo;
                command.Parameters.Add("@GPS_X", SqlDbType.NVarChar, 30).Value = GPS_X;
                command.Parameters.Add("@GPS_Y", SqlDbType.NVarChar, 30).Value = GPS_Y;
                command.Parameters.Add("@UserID", SqlDbType.NVarChar, 30).Value = UserID;
                command.Parameters.Add("@UserName", SqlDbType.NVarChar, 30).Value = UserName;
                command.Parameters.Add("@InfoMsg", SqlDbType.NVarChar, 3000).Direction = ParameterDirection.Output;
                con.Open();
                command.ExecuteNonQuery();
                InfoMsg = command.Parameters["@InfoMsg"].Value.ToString();

            }
            catch (Exception ex)
            {
                InfoMsg = ex.ToString();
            }
            finally
            {
                if (command != null)
                    command.Dispose();
                if (con != null)
                    con.Dispose();
            }
            return @InfoMsg;
        }

        /// <summary>
        /// 挂起
        /// </summary>
        /// <returns></returns>
        public string HangUp(string AccID, string OrderNo,string HangUpReason, string GPS_X, string GPS_Y, string UserID, string UserName)
        {
            string InfoMsg = "";
            SqlConnection con = null;
            SqlCommand command = null;
            try
            {
                con = Data.Conn();
                command = new SqlCommand("ExecHangUpOrder", con);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@AccID", SqlDbType.NVarChar, 30).Value = AccID;
                command.Parameters.Add("@OrderNo", SqlDbType.NVarChar, 30).Value = OrderNo;
                command.Parameters.Add("@Type", SqlDbType.NVarChar, 30).Value = "HangUp";
                command.Parameters.Add("@HangUpReason", SqlDbType.NVarChar, 30).Value = HangUpReason;
                command.Parameters.Add("@GPS_X", SqlDbType.NVarChar, 30).Value = GPS_X;
                command.Parameters.Add("@GPS_Y", SqlDbType.NVarChar, 30).Value = GPS_Y;
                command.Parameters.Add("@UserID", SqlDbType.NVarChar, 30).Value = UserID;
                command.Parameters.Add("@UserName", SqlDbType.NVarChar, 30).Value = UserName;
                command.Parameters.Add("@InfoMsg", SqlDbType.NVarChar, 3000).Direction = ParameterDirection.Output;
                con.Open();
                command.ExecuteNonQuery();
                InfoMsg = command.Parameters["@InfoMsg"].Value.ToString();

            }
            catch (Exception ex)
            {
                InfoMsg = ex.ToString();
            }
            finally
            {
                if (command != null)
                    command.Dispose();
                if (con != null)
                    con.Dispose();
            }
            return @InfoMsg;
        }
        /// <summary>
        /// 取消挂起
        /// </summary>
        /// <returns></returns>
        public string UnHangUp(string AccID, string OrderNo, string GPS_X, string GPS_Y, string UserID, string UserName)
        {
            string InfoMsg = "";
            SqlConnection con = null;
            SqlCommand command = null;
            try
            {
                con = Data.Conn();
                command = new SqlCommand("ExecHangUpOrder", con);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@AccID", SqlDbType.NVarChar, 30).Value = AccID;
                command.Parameters.Add("@OrderNo", SqlDbType.NVarChar, 30).Value = OrderNo;
                command.Parameters.Add("@Type", SqlDbType.NVarChar, 30).Value = "UnHangUp";
                command.Parameters.Add("@HangUpReason", SqlDbType.NVarChar, 30).Value = "";
                command.Parameters.Add("@GPS_X", SqlDbType.NVarChar, 30).Value = GPS_X;
                command.Parameters.Add("@GPS_Y", SqlDbType.NVarChar, 30).Value = GPS_Y;
                command.Parameters.Add("@UserID", SqlDbType.NVarChar, 30).Value = UserID;
                command.Parameters.Add("@UserName", SqlDbType.NVarChar, 30).Value = UserName;
                command.Parameters.Add("@InfoMsg", SqlDbType.NVarChar, 3000).Direction = ParameterDirection.Output;
                con.Open();
                command.ExecuteNonQuery();
                InfoMsg = command.Parameters["@InfoMsg"].Value.ToString();

            }
            catch (Exception ex)
            {
                InfoMsg = ex.ToString();
            }
            finally
            {
                if (command != null)
                    command.Dispose();
                if (con != null)
                    con.Dispose();
            }
            return @InfoMsg;
        }

        /// <summary>
        /// 退回
        /// </summary>
        /// <returns></returns>
        public string back(string AccID, string OrderNo, string BackReason, string GPS_X, string GPS_Y, string UserID, string UserName)
        {
            string InfoMsg = "";
            SqlConnection con = null;
            SqlCommand command = null;
            try
            {
                con = Data.Conn();
                command = new SqlCommand("ExecBackOrder", con);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@AccID", SqlDbType.NVarChar, 30).Value = AccID;
                command.Parameters.Add("@OrderNo", SqlDbType.NVarChar, 30).Value = OrderNo;
                command.Parameters.Add("@BackReason", SqlDbType.NVarChar, 30).Value = BackReason;
                command.Parameters.Add("@GPS_X", SqlDbType.NVarChar, 30).Value = GPS_X;
                command.Parameters.Add("@GPS_Y", SqlDbType.NVarChar, 30).Value = GPS_Y;
                command.Parameters.Add("@UserID", SqlDbType.NVarChar, 30).Value = UserID;
                command.Parameters.Add("@UserName", SqlDbType.NVarChar, 30).Value = UserName;
                command.Parameters.Add("@InfoMsg", SqlDbType.NVarChar, 3000).Direction = ParameterDirection.Output;
                con.Open();
                command.ExecuteNonQuery();
                InfoMsg = command.Parameters["@InfoMsg"].Value.ToString();

            }
            catch (Exception ex)
            {
                InfoMsg = ex.ToString();
            }
            finally
            {
                if (command != null)
                    command.Dispose();
                if (con != null)
                    con.Dispose();
            }
            return InfoMsg;
        }

        /// <summary>
        /// 申请支援
        /// </summary>
        /// <returns></returns>
        public string apply(string AccID, string OrderNo, string ApplyReason, string GPS_X, string GPS_Y, string UserID, string UserName)
        {
            string InfoMsg = "";
            SqlConnection con = null;
            SqlCommand command = null;
            try
            {
                con = Data.Conn();
                command = new SqlCommand("ExecApplyOrder", con);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@AccID", SqlDbType.NVarChar, 30).Value = AccID;
                command.Parameters.Add("@OrderNo", SqlDbType.NVarChar, 30).Value = OrderNo;
                command.Parameters.Add("@ApplyReason", SqlDbType.NVarChar, 30).Value = ApplyReason;
                command.Parameters.Add("@GPS_X", SqlDbType.NVarChar, 30).Value = GPS_X;
                command.Parameters.Add("@GPS_Y", SqlDbType.NVarChar, 30).Value = GPS_Y;
                command.Parameters.Add("@UserID", SqlDbType.NVarChar, 30).Value = UserID;
                command.Parameters.Add("@UserName", SqlDbType.NVarChar, 30).Value = UserName;
                command.Parameters.Add("@InfoMsg", SqlDbType.NVarChar, 3000).Direction = ParameterDirection.Output;
                con.Open();
                command.ExecuteNonQuery();
                InfoMsg = command.Parameters["@InfoMsg"].Value.ToString();

            }
            catch (Exception ex)
            {
                InfoMsg = ex.ToString();
            }
            finally
            {
                if (command != null)
                    command.Dispose();
                if (con != null)
                    con.Dispose();
            }
            return InfoMsg;
        }
        
        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public int delete()
        {
            return objdata.ExecuteNonQuery("update WO_WorkOrder set IsDel=1," +
                "UpdateUser='" + Entity.UpdateUser + "',UpdateDate=GetDate() " +
                "where RowPointer='" + Entity.InnerEntityOID + "'");
        }


        /// <summary>
        /// 按条件查询，支持分页
        /// </summary>
        /// <param name="AccID">账套ID</param>
        /// <param name="OrderNo">工单编号</param>
        /// <param name="OrderName">工单名称</param>
        /// <param name="OrderDate">工单日期</param>
        /// <param name="Status">状态</param>
        /// <param name="SaleNo">销售单号</param>
        /// <param name="Region">地区</param>
        /// <param name="AlloDept">分配部门</param>
        /// <param name="AlloUser">分配人员</param>
        /// <param name="CustNo">客户</param>
        /// <param name="OrderType">工单类型</param>
        /// <param name="IsBack">是否退回</param>
        /// <param name="IsHangUp">是否挂起</param>
        /// <param name="IsDel">是否删除</param>
        /// <returns></returns>
        public System.Collections.ICollection GetWorkOrderListQuery(String AccID, String OrderNo, String OrderName, DateTime MinOrderDate, DateTime MaxOrderDate, String Status,
            String SaleNo, String Region, String AlloDept, String AlloUser, String CustNo, string OrderType, bool? IsBack, bool? IsHangUp, bool? IsDel, int startRow, int pageSize)
        {
            if (startRow < 0 || pageSize <= 0)
            {
                throw new Exception();
            }

            return GetListHelper(AccID, OrderNo, OrderName, MinOrderDate, MaxOrderDate, Status, SaleNo, Region, AlloDept, AlloUser, CustNo, OrderType, IsBack, IsHangUp, IsDel, startRow, pageSize);
        }

        /// <summary>
        /// 按条件查询，不支持分页
        /// </summary>
        /// <param name="AccID">账套ID</param>
        /// <param name="OrderNo">工单编号</param>
        /// <param name="OrderName">工单名称</param>
        /// <param name="OrderDate">工单日期</param>
        /// <param name="Status">状态</param>
        /// <param name="SaleNo">销售单号</param>
        /// <param name="Region">地区</param>
        /// <param name="AlloDept">分配部门</param>
        /// <param name="AlloUser">分配人员</param>
        /// <param name="CustNo">客户</param>
        /// <param name="OrderType">工单类型</param>
        /// <param name="IsBack">是否退回</param>
        /// <param name="IsHangUp">是否挂起</param>
        /// <param name="IsDel">是否删除</param>
        /// <returns></returns>
        public System.Collections.ICollection GetWorkOrderListQuery(String AccID, String OrderNo, String OrderName, DateTime MinOrderDate, DateTime MaxOrderDate, String Status,
            String SaleNo, String Region, String AlloDept, String AlloUser, String CustNo, string OrderType, bool? IsBack, bool? IsHangUp, bool? IsDel)
        {
            return GetListHelper(AccID, OrderNo, OrderName, MinOrderDate, MaxOrderDate, Status, SaleNo, Region, AlloDept, AlloUser, CustNo, OrderType, IsBack, IsHangUp, IsDel, START_ROW_INIT, START_ROW_INIT);
        }

        /// <summary>
        /// 返回集合的大小
        /// </summary>
        /// <param name="AccID">账套ID</param>
        /// <param name="OrderNo">工单编号</param>
        /// <param name="OrderName">工单名称</param>
        /// <param name="OrderDate">工单日期</param>
        /// <param name="Status">状态</param>
        /// <param name="SaleNo">销售单号</param>
        /// <param name="Region">地区</param>
        /// <param name="AlloDept">分配部门</param>
        /// <param name="AlloUser">分配人员</param>
        /// <param name="CustNo">客户</param>
        /// <param name="OrderType">工单类型</param>
        /// <param name="IsBack">是否退回</param>
        /// <param name="IsHangUp">是否挂起</param>
        /// <param name="IsDel">是否删除</param>
        /// <returns></returns>
        public int GetWorkOrderListCount(String AccID, String OrderNo, String OrderName, DateTime MinOrderDate, DateTime MaxOrderDate, String Status,
            String SaleNo, String Region, String AlloDept, String AlloUser, String CustNo, string OrderType, bool? IsBack, bool? IsHangUp, bool? IsDel)
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
            if (OrderName != string.Empty)
            {
                wherestr = wherestr + " and OrderName like '%" + OrderName + "%'";
            }
            if (MinOrderDate != default(DateTime))
            {
                wherestr = wherestr + " and convert(nvarchar(10),OrderDate,121) >= '" + MinOrderDate.ToString("yyyy-MM-dd") + "'";
            }
            if (MaxOrderDate != default(DateTime))
            {
                wherestr = wherestr + " and convert(nvarchar(10),OrderDate,121) <= '" + MaxOrderDate.ToString("yyyy-MM-dd") + "'";
            }
            if (Status != string.Empty)
            {
                wherestr = wherestr + " and Status='" + Status + "'";
            }
            if (SaleNo != string.Empty)
            {
                wherestr = wherestr + " and SaleNo='%" + SaleNo + "%'";
            }
            if (Region != string.Empty)
            {
                wherestr = wherestr + " and Region='" + Region + "'";
            }
            if (AlloDept != string.Empty)
            {
                wherestr = wherestr + " and AlloDept='" + AlloDept + "'";
            }
            if (AlloUser != string.Empty)
            {
                wherestr = wherestr + " and AlloUser='" + AlloUser + "'";
            }
            if (CustNo != string.Empty)
            {
                wherestr = wherestr + " and CustNo='" + CustNo + "'";
            }
            if (OrderType != string.Empty)
            {
                wherestr = wherestr + " and OrderType='" + OrderType + "'";
            }
            if (IsBack != null)
            {
                wherestr = wherestr + " and IsBack=" + (IsBack == true ? "1" : "0");
            }
            if (IsHangUp != null)
            {
                wherestr = wherestr + " and IsHangUp=" + (IsHangUp == true ? "1" : "0");
            }
            if (IsDel != null)
            {
                wherestr = wherestr + " and IsDel=" + (IsDel == true ? "1" : "0");
            }

            string count = objdata.ExecuteDataSet("select count(*) as cnt from WO_WorkOrder where 1=1 " + wherestr).Tables[0].Rows[0]["cnt"].ToString();
            return int.Parse(count);
        }

        /// <summary>
        /// 按条件查询，返回符合条件的集合
        /// </summary>
        /// <param name="AccID">账套ID</param>
        /// <param name="OrderNo">工单编号</param>
        /// <param name="OrderName">工单名称</param>
        /// <param name="OrderDate">工单日期</param>
        /// <param name="Status">状态</param>
        /// <param name="SaleNo">销售单号</param>
        /// <param name="Region">地区</param>
        /// <param name="AlloDept">分配部门</param>
        /// <param name="AlloUser">分配人员</param>
        /// <param name="CustNo">客户</param>
        /// <param name="OrderType">工单类型</param>
        /// <param name="IsBack">是否退回</param>
        /// <param name="IsHangUp">是否挂起</param>
        /// <param name="IsDel">是否删除</param>
        /// <returns></returns>
        private System.Collections.ICollection GetListHelper(String AccID, String OrderNo, String OrderName, DateTime MinOrderDate, DateTime MaxOrderDate, String Status,
            String SaleNo, String Region, String AlloDept, String AlloUser, String CustNo, string OrderType, bool? IsBack, bool? IsHangUp, bool? IsDel, int startRow, int pageSize)
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
            if (OrderName != string.Empty)
            {
                wherestr = wherestr + " and OrderName like '%" + OrderName + "%'";
            }
            if (MinOrderDate != default(DateTime))
            {
                wherestr = wherestr + " and convert(nvarchar(10),OrderDate,121) >= '" + MinOrderDate.ToString("yyyy-MM-dd") + "'";
            }
            if (MaxOrderDate != default(DateTime))
            {
                wherestr = wherestr + " and convert(nvarchar(10),OrderDate,121) <= '" + MaxOrderDate.ToString("yyyy-MM-dd") + "'";
            }
            if (Status != string.Empty)
            {
                wherestr = wherestr + " and Status='" + Status + "'";
            }
            if (SaleNo != string.Empty)
            {
                wherestr = wherestr + " and SaleNo='%" + SaleNo + "%'";
            }
            if (Region != string.Empty)
            {
                wherestr = wherestr + " and Region='" + Region + "'";
            }
            if (AlloDept != string.Empty)
            {
                wherestr = wherestr + " and AlloDept='" + AlloDept + "'";
            }
            if (AlloUser != string.Empty)
            {
                wherestr = wherestr + " and AlloUser='" + AlloUser + "'";
            }
            if (CustNo != string.Empty)
            {
                wherestr = wherestr + " and CustNo='" + CustNo + "'";
            }
            if (OrderType != string.Empty)
            {
                wherestr = wherestr + " and OrderType='" + OrderType + "'";
            }
            if (IsBack != null)
            {
                wherestr = wherestr + " and IsBack=" + (IsBack == true ? "1" : "0");
            }
            if (IsHangUp != null)
            {
                wherestr = wherestr + " and IsHangUp=" + (IsHangUp == true ? "1" : "0");
            }
            if (IsDel != null)
            {
                wherestr = wherestr + " and IsDel=" + (IsDel == true ? "1" : "0");
            }

            System.Collections.IList entitys = null;
            if (startRow > START_ROW_INIT && pageSize > START_ROW_INIT)
            {
                entitys = Query(objdata.ExecSelect("WO_WorkOrder", wherestr, startRow, pageSize, orderstr));
            }
            else
            {
                entitys = Query(objdata.ExecSelect("WO_WorkOrder", wherestr, START_ROW_INIT, START_ROW_INIT, orderstr));
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
                project.Entity.Order.EntityWorkOrder entity = new project.Entity.Order.EntityWorkOrder();
                entity.InnerEntityOID = dr["RowPointer"].ToString();
                entity.AccID = dr["AccID"].ToString();
                entity.OrderNo = dr["OrderNo"].ToString();
                entity.OrderName = dr["OrderName"].ToString();
                entity.OrderDate = ParseDateTimeForString(dr["OrderDate"].ToString());
                entity.OrderType = dr["OrderType"].ToString();
                entity.Status = dr["Status"].ToString();
                entity.SaleNo = dr["SaleNo"].ToString();
                entity.AlloDept = dr["AlloDept"].ToString();
                entity.AlloUser = dr["AlloUser"].ToString();
                entity.CustNo = dr["CustNo"].ToString();
                entity.LinkMan = dr["LinkMan"].ToString();
                entity.LinkTel = dr["LinkTel"].ToString();
                entity.Region = dr["Region"].ToString();
                _entity.Addr = dr["Addr"].ToString();
                entity.Remark = dr["Remark"].ToString();
                entity.CreateTime = ParseDateTimeForString(dr["CreateTime"].ToString());
                entity.CustneedTime = ParseDateTimeForString(dr["CustneedTime"].ToString());
                entity.ResponseTime = ParseDateTimeForString(dr["ResponseTime"].ToString());
                entity.AppoIntTime = ParseDateTimeForString(dr["AppoIntTime"].ToString());
                entity.SignTime = ParseDateTimeForString(dr["SignTime"].ToString());
                entity.WorkTime = ParseDateTimeForString(dr["WorkTime"].ToString());
                entity.FinishTime = ParseDateTimeForString(dr["FinishTime"].ToString());
                entity.CloseTime = ParseDateTimeForString(dr["CloseTime"].ToString());
                entity.ConfirmTime = ParseDateTimeForString(dr["ConfirmTime"].ToString());
                entity.CreateUser = dr["CreateUser"].ToString();
                entity.UpdateDate = ParseDateTimeForString(dr["UpdateDate"].ToString());
                entity.UpdateUser = dr["UpdateUser"].ToString();
                entity.IsHangUp = bool.Parse(dr["IsHangUp"].ToString());
                entity.HangUpDate = ParseDateTimeForString(dr["HangUpDate"].ToString());
                entity.HangUpReason = dr["HangUpReason"].ToString();
                entity.IsApply = bool.Parse(dr["IsApply"].ToString());
                entity.ApplyDate = ParseDateTimeForString(dr["ApplyDate"].ToString());
                entity.ApplyReason = dr["ApplyReason"].ToString();
                entity.IsBack = bool.Parse(dr["IsBack"].ToString());
                entity.BackDate = ParseDateTimeForString(dr["BackDate"].ToString());
                entity.BackReason = dr["BackReason"].ToString();
                entity.IsDel = bool.Parse(dr["IsDel"].ToString());
                entity.DONo = dr["DONo"].ToString();
                result.Add(entity);
            }
            return result;
        }

    }
}
