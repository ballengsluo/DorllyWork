using System;
using System.Data;
namespace project.Business.Sys
{
    /// <summary>
    /// ������Ϣ��ҵ����
    /// </summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    public sealed class BusinessAccInfo : project.Business.AbstractPmBusiness
    {
        private project.Entity.Sys.EntityAccInfo _entity = new project.Entity.Sys.EntityAccInfo() ;
        public string orderstr = "AccID";
        Data objdata = new Data();

        /// <summary>
        /// ȱʡ���캯��
        /// </summary>
        public BusinessAccInfo() {}

        /// <summary>
        /// �������Ĺ�����
        /// </summary>
        /// <param name="entity">ʵ����</param>
        public BusinessAccInfo(project.Entity.Sys.EntityAccInfo entity)
        {
            this._entity = entity;
        }

        /// <summary>
        /// ��ʵ����(EntityAccInfo)����
        /// </summary>
        public project.Entity.Sys.EntityAccInfo Entity
        {
            get { return _entity as project.Entity.Sys.EntityAccInfo; }
        }

        /// </summary>
        ///load ���� pid����
        /// </summary>
        public void load(string pid)
        {
            DataRow dr = objdata.ExecuteDataSet("select * from Sys_Acc_Info where RowPointer='" + pid + "'").Tables[0].Rows[0];
            _entity.InnerEntityOID = dr["RowPointer"].ToString();
            _entity.AccID = dr["AccID"].ToString();
            _entity.AccName = dr["AccName"].ToString();
            _entity.AccBrfName = dr["AccBrfName"].ToString();
            _entity.Addr = dr["Addr"].ToString();
            _entity.Tel = dr["Tel"].ToString();
            _entity.Fax = dr["Fax"].ToString();
            _entity.Website = dr["Website"].ToString();
            _entity.Contact = dr["Contact"].ToString();
            _entity.ContactTel = dr["ContactTel"].ToString();
            _entity.Pic = dr["Pic"].ToString();
            _entity.Remark = dr["Remark"].ToString();
            _entity.UserCount = ParseIntForString(dr["UserCount"].ToString());
            _entity.RegDate = ParseDateTimeForString(dr["RegDate"].ToString());
            _entity.LimitedDate = ParseDateTimeForString(dr["LimitedDate"].ToString());
        }

        /// </summary>
        ///Save����
        /// </summary>
        public int Save()
        {
            string sqlstr = "";
            if (Entity.InnerEntityOID == null)
                sqlstr = "insert into Sys_Acc_Info(RowPointer,AccID,AccName,AccBrfName,Addr,Tel,Fax,Website,Contact,ContactTel,Pic,Remark,UserCount,RegDate,LimitedDate)" +
                    "values(newid()," + "'" + Entity.AccID + "'" + "," + "'" + Entity.AccName + "'" + "," + "'" + Entity.AccBrfName + "'" + "," + "'" + Entity.Addr + "'" + "," + "'" + Entity.Tel + "'" + "," + "'" + Entity.Fax + "'" + "," + "'" + Entity.Website + "'" + "," + "'" + Entity.Contact + "'" + "," + "'" + Entity.ContactTel + "'" + "," + "'" + Entity.Pic + "'" + "," + "'" + Entity.Remark + "'" + "," +
                    Entity.UserCount + "," + "'" + Entity.RegDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "'" + Entity.LimitedDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" + ")";
            else
                sqlstr = "update Sys_Acc_Info" +
                    " set AccID=" + "'" + Entity.AccID + "'" + "," + "AccName=" + "'" + Entity.AccName + "'" + "," + "AccBrfName=" + "'" + Entity.AccBrfName + "'" + "," + "Addr=" + "'" + Entity.Addr + "'" + "," + 
                    "Tel=" + "'" + Entity.Tel + "'" + "," + "Fax=" + "'" + Entity.Fax + "'" + "," + "Website=" + "'" + Entity.Website + "'" + "," + "Contact=" + "'" + Entity.Contact + "'" + "," + "ContactTel=" + "'" + Entity.ContactTel + "'" + "," + "Pic=" + "'" + Entity.Pic + "'" + "," + "Remark=" + "'" + Entity.Remark + "'" + "," +
                    "UserCount=" + Entity.UserCount + "," + "RegDate=" + "'" + Entity.RegDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "," + "LimitedDate=" + "'" + Entity.LimitedDate.ToString("yyyy-MM-dd HH:mm:ss") + "'" + 
                    " where RowPointer='" + Entity.EntityOID.ToString() + "'";
            return objdata.ExecuteNonQuery(sqlstr);
        }

        /// </summary>
        ///Delete ���� 
        /// </summary>
        public int delete()
        {
            return objdata.ExecuteNonQuery("delete from Sys_Acc_Info where RowPointer='"+Entity.EntityOID.ToString()+"'");
        }

        /// <summary>
        /// ��������ѯ��֧�ַ�ҳ
        /// </summary>
        /// <param name="AccIDEquals">��˾ID</param>
        /// <param name="AccNameEquals">��˾����</param>
        /// <param name="AccBrfNameEquals">��˾���</param>
        /// <returns></returns>
        public System.Collections.ICollection GetAccInfoListQuery(String AccID,String AccName,String AccBrfName,int startRow, int pageSize)
        {
            if (startRow < 0 || pageSize <= 0)
            {
                throw new Exception();
            }

            return GetListHelper(AccID,AccName,AccBrfName,startRow, pageSize);
        }

        /// <summary>
        /// ��������ѯ����֧�ַ�ҳ
        /// </summary>
        /// <param name="AccIDEquals">��˾ID</param>
        /// <param name="AccNameEquals">��˾����</param>
        /// <param name="AccBrfNameEquals">��˾���</param>
        /// <returns></returns>
        public System.Collections.ICollection GetAccInfoListQuery(String AccID,String AccName,String AccBrfName)
        {
            return GetListHelper(AccID,AccName,AccBrfName,START_ROW_INIT, START_ROW_INIT);
        }

        /// <summary>
        /// ���ؼ��ϵĴ�С
        /// </summary>
        /// <param name="AccIDEquals">��˾ID</param>
        /// <param name="AccNameEquals">��˾����</param>
        /// <param name="AccBrfNameEquals">��˾���</param>
        /// <returns></returns>
        public int GetAccInfoListCount(String AccID,String AccName,String AccBrfName)
        {
            string wherestr="";
            if (AccID != string.Empty)
            {
                wherestr=wherestr+" and AccID like '%"+AccID+"%'";
            }
            if (AccName != string.Empty)
            {
                wherestr=wherestr+" and AccName like '%"+AccName+"%'";
            }
            if (AccBrfName != string.Empty)
            {
                wherestr=wherestr+" and AccBrfName like '%"+AccBrfName+"%'";
            }

            string count = objdata.ExecuteDataSet("select count(*) as cnt from Sys_Acc_Info where 1=1 " + wherestr).Tables[0].Rows[0]["cnt"].ToString();
            return int.Parse(count);
        }

        /// <summary>
        /// ��������ѯ�����ط��������ļ���
        /// </summary>
        /// <param name="AccIDEquals">��˾ID</param>
        /// <param name="AccNameEquals">��˾����</param>
        /// <param name="AccBrfNameEquals">��˾���</param>
        /// <returns></returns>
        private System.Collections.ICollection GetListHelper(String AccID,String AccName,String AccBrfName,int startRow, int pageSize)
        {
            string wherestr="";
            if (AccID != string.Empty)
            {
                wherestr=wherestr+" and AccID like '%"+AccID+"%'";
            }
            if (AccName != string.Empty)
            {
                wherestr=wherestr+" and AccName like '%"+AccName+"%'";
            }
            if (AccBrfName != string.Empty)
            {
                wherestr=wherestr+" and AccBrfName like '%"+AccBrfName+"%'";
            }
            System.Collections.IList entitys = null;
            if (startRow > START_ROW_INIT && pageSize > START_ROW_INIT)
            {
                entitys=Query(objdata.ExecSelect("Sys_Acc_Info", wherestr ,startRow,pageSize, orderstr));
            }
            else
            {
                entitys=Query(objdata.ExecSelect("Sys_Acc_Info", wherestr , START_ROW_INIT ,START_ROW_INIT, orderstr));
            }
            return entitys;
        }
        /// </summary>
        ///Query ���� dt��ѯ���
        /// </summary>
        public System.Collections.IList Query(System.Data.DataTable dt)
        {
            System.Collections.IList result = new System.Collections.ArrayList();
            foreach(System.Data.DataRow dr in dt.Rows)
            {
                project.Entity.Sys.EntityAccInfo entity=new project.Entity.Sys.EntityAccInfo();
              
                entity.InnerEntityOID=dr["RowPointer"].ToString();
                entity.AccID=dr["AccID"].ToString();
                entity.AccName=dr["AccName"].ToString();
                entity.AccBrfName=dr["AccBrfName"].ToString();
                entity.Addr=dr["Addr"].ToString();
                entity.Tel=dr["Tel"].ToString();
                entity.Fax=dr["Fax"].ToString();
                entity.Website=dr["Website"].ToString();
                entity.Contact=dr["Contact"].ToString();
                entity.ContactTel=dr["ContactTel"].ToString();
                entity.Pic=dr["Pic"].ToString();
                entity.Remark=dr["Remark"].ToString();
                entity.UserCount=ParseIntForString(dr["UserCount"].ToString());
                entity.RegDate=ParseDateTimeForString(dr["RegDate"].ToString());
                entity.LimitedDate=ParseDateTimeForString(dr["LimitedDate"].ToString());
                result.Add(entity);
            }
            return result;
        }

    }
}
