using System;
using System.Data;
namespace project.Business.Base
{
    /// <summary>
    /// �����ֵ��ҵ����
    /// </summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    public sealed class BusinessDict : project.Business.AbstractPmBusiness
    {
        private project.Entity.Base.EntityDict _entity = new project.Entity.Base.EntityDict() ;
        public string orderstr = "DictNo";
        Data objdata = new Data();

        /// <summary>
        /// ȱʡ���캯��
        /// </summary>
        public BusinessDict() {}

        /// <summary>
        /// �������Ĺ�����
        /// </summary>
        /// <param name="entity">ʵ����</param>
        public BusinessDict(project.Entity.Base.EntityDict entity)
        {
            this._entity = entity;
        }

        /// <summary>
        /// ��ʵ����(EntityDict)����
        /// </summary>
        public project.Entity.Base.EntityDict Entity
        {
            get { return _entity as project.Entity.Base.EntityDict; }
        }

        /// </summary>
        ///load ���� pid����
        /// </summary>
        public void load(string dictNo, string dictType,string accID)
        {
            DataRow dr = objdata.ExecuteDataSet("select * from Base_Dict where DictNo='" + dictNo + "' and DictType='" + dictType + "' and AccID='" + accID + "'").Tables[0].Rows[0];
            _entity.DictNo=dr["DictNo"].ToString();
            _entity.DictName=dr["DictName"].ToString();
            _entity.AccID = dr["AccID"].ToString();
            _entity.DictType = dr["DictType"].ToString();
            _entity.Remark=dr["Remark"].ToString();
        }

        /// </summary>
        ///Save����
        /// </summary>
        public int Save(string type)
        {
            string sqlstr = "";
            if (type == "insert")
                sqlstr = "insert into Base_Dict(DictNo,DictName,AccID,DictType,Remark)" +
                    "values('" + Entity.DictNo + "'," + "'" + Entity.DictName + "'" + "," + "'" + Entity.AccID + "'" + "," + "'" + Entity.DictType + "'" + "," + "'" + Entity.Remark + "'" + ")";
            else
                sqlstr = "update Base_Dict" +
                    " set DictName=" + "'" + Entity.DictName + "'" + "," + "Remark=" + "'" + Entity.Remark + "'" +
                    " where DictNo='" + Entity.DictNo + "'" + " and DictType=" + "'" + Entity.DictType + "'" + " and AccID=" + "'" + Entity.AccID + "'";
            return objdata.ExecuteNonQuery(sqlstr);
        }

        /// </summary>
        ///Delete ���� 
        /// </summary>
        public int delete()
        {
            return objdata.ExecuteNonQuery("delete from Base_Dict where DictNo='" + Entity.DictNo + "' and AccID=" + "'" + Entity.AccID + "' and DictType=" + "'" + Entity.DictType + "'");
        }

        /// <summary>
        /// ��������ѯ��֧�ַ�ҳ
        /// </summary>
        /// <param name="DictNo">�����ֵ���</param>
        /// <param name="DictName">�����ֵ�����</param>
        /// <param name="AccID">����</param>
        /// <param name="DictType">�����ֵ�����</param>
        /// <returns></returns>
        public System.Collections.ICollection GetDictListQuery(String DictNo, String DictName, String AccID, String DictType, int startRow, int pageSize)
        {
            if (startRow < 0 || pageSize <= 0)
            {
                throw new Exception();
            }

            return GetListHelper(DictNo, DictName, AccID, DictType, startRow, pageSize);
        }

        /// <summary>
        /// ��������ѯ����֧�ַ�ҳ
        /// </summary>
        /// <param name="DictNo">�����ֵ���</param>
        /// <param name="DictName">�����ֵ�����</param>
        /// <param name="AccID">����</param>
        /// <param name="DictType">�����ֵ�����</param>
        /// <returns></returns>
        public System.Collections.ICollection GetDictListQuery(String DictNo, String DictName, String AccID, String DictType)
        {
            return GetListHelper(DictNo, DictName, AccID, DictType, START_ROW_INIT, START_ROW_INIT);
        }

        /// <summary>
        /// ���ؼ��ϵĴ�С
        /// </summary>
        /// <param name="DictNo">�����ֵ���</param>
        /// <param name="DictName">�����ֵ�����</param>
        /// <param name="AccID">����</param>
        /// <param name="DictType">�����ֵ�����</param>
        /// <returns></returns>
        public int GetDictListCount(String DictNo, String DictName, String AccID, String DictType)
        {
            string wherestr="";
            if (DictNo != string.Empty)
            {
                wherestr=wherestr+" and DictNo like '%"+DictNo+"%'";
            }
            if (DictName != string.Empty)
            {
                wherestr=wherestr+" and DictName like '%"+DictName+"%'";
            }
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and AccID='" + AccID + "'";
            }
            if (DictType != string.Empty)
            {
                wherestr = wherestr + " and DictType='" + DictType + "'";
            }

            string count = objdata.ExecuteDataSet("select count(*) as cnt from Base_Dict where 1=1 " + wherestr).Tables[0].Rows[0]["cnt"].ToString();
            return int.Parse(count);
        }

        /// <summary>
        /// ��������ѯ�����ط��������ļ���
        /// </summary>
        /// <param name="DictNo">�����ֵ���</param>
        /// <param name="DictName">�����ֵ�����</param>
        /// <param name="AccID">����</param>
        /// <param name="DictType">�����ֵ�����</param>
        /// <returns></returns>
        private System.Collections.ICollection GetListHelper(String DictNo, String DictName, String AccID, String DictType, int startRow, int pageSize)
        {
            string wherestr="";
            if (DictNo != string.Empty)
            {
                wherestr=wherestr+" and DictNo like '%"+DictNo+"%'";
            }
            if (DictName != string.Empty)
            {
                wherestr=wherestr+" and DictName like '%"+DictName+"%'";
            }
            if (AccID != string.Empty)
            {
                wherestr = wherestr + " and AccID='" + AccID + "'";
            }
            if (DictType != string.Empty)
            {
                wherestr = wherestr + " and DictType='" + DictType + "'";
            }

            System.Collections.IList entitys = null;
            if (startRow > START_ROW_INIT && pageSize > START_ROW_INIT)
            {
                entitys=Query(objdata.ExecSelect("Base_Dict", wherestr ,startRow,pageSize, orderstr));
            }
            else
            {
                entitys=Query(objdata.ExecSelect("Base_Dict", wherestr , START_ROW_INIT ,START_ROW_INIT, orderstr));
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
                project.Entity.Base.EntityDict entity=new project.Entity.Base.EntityDict();
                entity.DictNo = dr["DictNo"].ToString();
                entity.DictName = dr["DictName"].ToString();
                entity.AccID = dr["AccID"].ToString();
                entity.DictType=dr["DictType"].ToString();
                entity.Remark=dr["Remark"].ToString();
                result.Add(entity);
            }
            return result;
        }

    }
}
