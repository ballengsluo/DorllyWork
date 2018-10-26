using System;
using System.Data;
namespace project.Business.Sys
{
    /// <summary>
    /// 兑换物品的业务类
    /// </summary>
    /// <author>tz</author>
    /// <date>2012-02-15</date>
    public sealed class BusinessSMS : project.Business.AbstractPmBusiness
    {
        private project.Entity.Admin.EntitySMS _entity = new project.Entity.Admin.EntitySMS();
        public string orderstr = "CreateDate asc";
        Data objdata = new Data();

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        public BusinessSMS() { }

        /// <summary>
        /// 带参数的构函数
        /// </summary>
        /// <param name="entity">实体类</param>
        public BusinessSMS(project.Entity.Admin.EntitySMS entity)
        {
            this._entity = entity;
        }

        /// <summary>
        /// 与实体类(EntitySMS)关联
        /// </summary>
        public project.Entity.Admin.EntitySMS Entity
        {
            get { return _entity as project.Entity.Admin.EntitySMS; }
        }

        /// </summary>
        ///Load 方法 pid主键
        /// </summary>
        public void load(string pid)
        {
            DataRow dr = objdata.ExecuteDataSet("select * from Sys_Sms where RowPointer='" + pid + "'").Tables[0].Rows[0];
            _entity.InnerEntityOID = dr["RowPointer"].ToString();
            _entity.SendType = dr["SendType"].ToString();
            _entity.SendPerson = dr["SendPerson"].ToString();
            _entity.Tel = dr["Tel"].ToString();
            _entity.Content = dr["Content"].ToString();
            _entity.CreateDate = ParseDateTimeForString(dr["CreateDate"].ToString());
            _entity.SendDate = ParseDateTimeForString(dr["SendDate"].ToString());
            _entity.IsSend = bool.Parse(dr["IsSend"].ToString());
            _entity.SendStat = dr["SendStat"].ToString();
            _entity.SendResult = dr["SendResult"].ToString();
            _entity.RefNo = dr["RefNo"].ToString();
            _entity.VerifyNo = dr["VerifyNo"].ToString();
        }

        /// </summary>
        ///Save方法
        /// </summary>
        public int Save()
        {
            string sqlstr = "";
            string SendDate = "null";
            if (Entity.SendDate.Year > 1900)
                SendDate = "'" + Entity.SendDate.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            if (Entity.InnerEntityOID == null)
                sqlstr = "insert into Sys_Sms(RowPointer,SendType,SendPerson,Tel,Content,CreateDate,SendDate,IsSend,SendStat,SendResult,RefNo,VerifyNo)" +
                    "values(NEWID()," + "'" + Entity.SendType + "'" + "," + "'" + Entity.SendPerson + "'" + "," +
                    "'" + Entity.Tel + "'" + "," + "'" + Entity.Content + "'" + "," + "'" + Entity.CreateDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," +
                    SendDate + "," + (Entity.IsSend ? "1" : "0") + "," + "'" + Entity.SendStat + "'" + "," +
                    "'" + Entity.SendResult + "'" + "," + "'" + Entity.RefNo + "'" + "," + "'" + Entity.VerifyNo + "'" + ")";
            else
                sqlstr = "update Sys_Sms" +
                    " set SendType=" + "'" + Entity.SendType + "'" + "," + "SendPerson=" + "'" + Entity.SendPerson + "'" + "," + 
                    "Tel=" + "'" + Entity.Tel + "'" + "," + "Content=" + "'" + Entity.Content + "'" + "," + 
                    "CreateDate=" + "'" + Entity.CreateDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "SendDate=" + SendDate + "," + 
                    "SendResult=" + "'" + Entity.SendResult + "'" + "," + "IsSend=" + (Entity.IsSend ? "1" : "0") + "," + 
                    "RefNo=" + "'" + Entity.RefNo + "'" + "," + "SendStat=" + "'" + Entity.SendStat + "'" + "," +
                    "VerifyNo=" + "'" + Entity.VerifyNo + "'" +
                    " where RowPointer='" + Entity.EntityOID.ToString() + "'";
            return objdata.ExecuteNonQuery(sqlstr);
        }

        /// </summary>
        ///Delete 方法 
        /// </summary>
        public int delete()
        {
            return objdata.ExecuteNonQuery("delete from Sys_Sms where RowPointer='" + Entity.EntityOID.ToString() + "'");
        }

        /// 按条件查询，支持分页
        /// </summary>
        /// <param name="SendDate">发送日期</param>
        /// <param name="SendType">发送类型</param>
        /// <param name="IsSend">是否发送</param>
        /// <param name="SendStat">发送结果</param>
        /// <returns></returns>
        public System.Collections.ICollection GetSMSListQuery(DateTime MinSendDate, DateTime MaxSendDate, String SendType, bool? IsSend, String SendStat, int startRow, int pageSize)
        {
            if (startRow < 0 || pageSize <= 0)
            {
                throw new Exception();
            }

            return GetListHelper(MinSendDate, MaxSendDate, SendType, IsSend, SendStat, startRow, pageSize);
        }

        /// <summary>
        /// 按条件查询，不支持分页
        /// </summary>
        /// <param name="SendDate">发送日期</param>
        /// <param name="SendType">发送类型</param>
        /// <param name="IsSend">是否发送</param>
        /// <param name="SendStat">发送结果</param>
        /// <returns></returns>
        public System.Collections.ICollection GetSMSListQuery(DateTime MinSendDate, DateTime MaxSendDate, String SendType, bool? IsSend, String SendStat)
        {
            return GetListHelper(MinSendDate, MaxSendDate, SendType, IsSend, SendStat, START_ROW_INIT, START_ROW_INIT);
        }

        /// <summary>
        /// 返回集合的大小
        /// </summary>
        /// <param name="SendDate">发送日期</param>
        /// <param name="SendType">发送类型</param>
        /// <param name="IsSend">是否发送</param>
        /// <param name="SendResult">发送结果</param>
        /// <returns></returns>
        public int GetSMSListCount(DateTime MinSendDate, DateTime MaxSendDate, String SendType, bool? IsSend, String SendStat)
        {
            string wherestr = "";
            if (MinSendDate != default(DateTime))
            {
                wherestr = wherestr + " and convert(varchar(13),SendDate,121) >='" + MinSendDate.ToString("yyyy-MM-dd HH") + "'";
            }
            if (MaxSendDate != default(DateTime))
            {
                wherestr = wherestr + " and convert(varchar(13),SendDate,121) <='" + MaxSendDate.ToString("yyyy-MM-dd HH") + "'";
            }
            if (SendType != string.Empty)
            {
                wherestr = wherestr + " and SendType like '" + SendType + "'";
            }
            if (IsSend != null)
            {
                wherestr = wherestr + " and IsSend =" + (IsSend == true ? "1" : "0");
            }
            if (SendStat != string.Empty)
            {
                wherestr = wherestr + " and SendStat like '" + SendStat + "'";
            }

            DataTable ds = objdata.ExecSelect("Sys_Sms", wherestr, START_ROW_INIT, START_ROW_INIT, orderstr);
            int count = ds.Rows.Count;
            return count;
        }

        /// <summary>
        /// 按条件查询，返回符合条件的集合
        /// </summary>
        /// <param name="SendDate">发送日期</param>
        /// <param name="SendType">发送类型</param>
        /// <param name="IsSend">是否发送</param>
        /// <param name="SendResult">发送结果</param>
        /// <returns></returns>
        private System.Collections.ICollection GetListHelper(DateTime MinSendDate, DateTime MaxSendDate, String SendType, bool? IsSend, String SendStat, int startRow, int pageSize)
        {
            string wherestr = "";
            if (MinSendDate != default(DateTime))
            {
                wherestr = wherestr + " and convert(varchar(13),SendDate,121) >='" + MinSendDate.ToString("yyyy-MM-dd HH") + "'";
            }
            if (MaxSendDate != default(DateTime))
            {
                wherestr = wherestr + " and convert(varchar(13),SendDate,121) <='" + MaxSendDate.ToString("yyyy-MM-dd HH") + "'";
            }
            if (SendType != string.Empty)
            {
                wherestr = wherestr + " and SendType like '" + SendType + "'";
            }
            if (IsSend != null)
            {
                wherestr = wherestr + " and IsSend =" + (IsSend == true ? "1" : "0");
            }
            if (SendStat != string.Empty)
            {
                wherestr = wherestr + " and SendResult like '" + SendStat + "'";
            }

            System.Collections.IList entitys = null;
            if (startRow > START_ROW_INIT && pageSize > START_ROW_INIT)
            {
                entitys = Query(objdata.ExecSelect("Sys_Sms", wherestr, startRow, pageSize, orderstr));
            }
            else
            {
                entitys = Query(objdata.ExecSelect("Sys_Sms", wherestr, START_ROW_INIT, START_ROW_INIT, orderstr));
            }
            return entitys;
        }
        /// </summary>
        ///Query 方法 dt查询结果
        /// </summary>
        private System.Collections.IList Query(System.Data.DataTable dt)
        {
            System.Collections.IList result = new System.Collections.ArrayList();
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                project.Entity.Admin.EntitySMS entity = new project.Entity.Admin.EntitySMS();

                entity.InnerEntityOID = dr["RowPointer"].ToString();
                entity.SendType = dr["SendType"].ToString();
                entity.SendPerson = dr["SendPerson"].ToString();
                entity.Tel = dr["Tel"].ToString();
                entity.Content = dr["Content"].ToString();
                entity.CreateDate = ParseDateTimeForString(dr["CreateDate"].ToString());
                entity.IsSend = bool.Parse(dr["IsSend"].ToString());
                entity.SendDate = ParseDateTimeForString(dr["SendDate"].ToString());
                entity.SendStat = dr["SendStat"].ToString();
                entity.SendResult = dr["SendResult"].ToString();
                entity.RefNo = dr["RefNo"].ToString();
                entity.VerifyNo = dr["VerifyNo"].ToString();
                result.Add(entity);
            }
            return result;
        }

    }
}
