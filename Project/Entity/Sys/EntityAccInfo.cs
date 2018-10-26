using System;
namespace project.Entity.Sys
{
    /// <summary>������Ϣ</summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    [System.Serializable]
    public class EntityAccInfo
    {
        private string _entityOID;
        private string _accID;
        private string _accName;
        private string _accBrfName;
        private string _addr;
        private string _tel;
        private string _fax;
        private string _website;
        private string _contact;
        private string _contactTel;
        private string _pic;
        private string _remark;
        private int _userCount;
        private System.DateTime _regDate;
        private System.DateTime _limitedDate;

        /// <summary>ȱʡ���캯��</summary>
        public EntityAccInfo() {}

        /// <summary>������ֻ������</summary>
        public System.Guid EntityOID
        {
            get { return new System.Guid(_entityOID); }
        }

        /// <summary>�ڲ�ӳ������</summary>
        public string InnerEntityOID
        {
            get { return _entityOID; }
            set { _entityOID = value; }
        }

        /// <summary>
        /// ������������˾ID
        /// ���ȣ�20
        /// ����Ϊ�գ���
        /// </summary>
        public string AccID
        {
            get { return _accID; }
            set { _accID = value; }
        }

        /// <summary>
        /// ������������˾����
        /// ���ȣ�200
        /// ����Ϊ�գ���
        /// </summary>
        public string AccName
        {
            get { return _accName; }
            set { _accName = value; }
        }

        /// <summary>
        /// ������������˾���
        /// ���ȣ�200
        /// ����Ϊ�գ���
        /// </summary>
        public string AccBrfName
        {
            get { return _accBrfName; }
            set { _accBrfName = value; }
        }

        /// <summary>
        /// ������������ַ
        /// ���ȣ�500
        /// ����Ϊ�գ���
        /// </summary>
        public string Addr
        {
            get { return _addr; }
            set { _addr = value; }
        }

        /// <summary>
        /// �����������绰
        /// ���ȣ�50
        /// ����Ϊ�գ���
        /// </summary>
        public string Tel
        {
            get { return _tel; }
            set { _tel = value; }
        }

        /// <summary>
        /// ��������������
        /// ���ȣ�50
        /// ����Ϊ�գ���
        /// </summary>
        public string Fax
        {
            get { return _fax; }
            set { _fax = value; }
        }

        /// <summary>
        /// ������������ַ
        /// ���ȣ�50
        /// ����Ϊ�գ���
        /// </summary>
        public string Website
        {
            get { return _website; }
            set { _website = value; }
        }

        /// <summary>
        /// ������������ϵ��
        /// ���ȣ�50
        /// ����Ϊ�գ���
        /// </summary>
        public string Contact
        {
            get { return _contact; }
            set { _contact = value; }
        }

        /// <summary>
        /// ������������ϵ�˵绰
        /// ���ȣ�50
        /// ����Ϊ�գ���
        /// </summary>
        public string ContactTel
        {
            get { return _contactTel; }
            set { _contactTel = value; }
        }

        /// <summary>
        /// ����������ͼƬ
        /// ���ȣ�50
        /// ����Ϊ�գ���
        /// </summary>
        public string Pic
        {
            get { return _pic; }
            set { _pic = value; }
        }

        /// <summary>
        /// ������������ע
        /// ���ȣ�3000
        /// ����Ϊ�գ���
        /// </summary>
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }

        /// <summary>
        /// �����������û�����
        /// ����Ϊ�գ���
        /// </summary>
        public int UserCount
        {
            get { return _userCount; }
            set { _userCount = value; }
        }

        /// <summary>
        /// �����������Ǽ�����
        /// ����Ϊ�գ���
        /// </summary>
        public System.DateTime RegDate
        {
            get { return _regDate; }
            set { _regDate = value; }
        }

        /// <summary>
        /// ������������������
        /// ����Ϊ�գ���
        /// </summary>
        public System.DateTime LimitedDate
        {
            get { return _limitedDate; }
            set { _limitedDate = value; }
        }
    }
}
