using System;
namespace project.Entity.Sys
{
    /// <summary>�û���Ϣ</summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    [System.Serializable]
    public class EntityUserInfo
    {
        private string _entityOID;
        private string _userType;
        private string _accID;
        private string _userNo;
        private string _nickName;
        private string _userName;
        private string _password;
        private string _tel;
        private string _email;
        private string _addr;
        private System.DateTime _regDate;
        private bool _valid;
        private string _deptNo;
        private string _manager;
        private string _picture;

        /// <summary>ȱʡ���캯��</summary>
        public EntityUserInfo() {}
        
        /// <summary>�ڲ�ӳ������</summary>
        public string InnerEntityOID
        {
            get { return _entityOID; }
            set { _entityOID = value; }
        }

        /// <summary>
        /// �����������û�����
        /// ���ȣ�20
        /// ����Ϊ�գ���
        /// </summary>
        public string UserType
        {
            get { return _userType; }
            set { _userType = value; }
        }

        /// <summary>
        /// �����������û��������
        /// ���ȣ�100
        /// ����Ϊ�գ���
        /// </summary>
        public string UserTypeName
        {
            get
            {
                if (_userType != "" && _userType != null)
                {
                    try
                    {
                        Business.Sys.BusinessUserType usertype = new Business.Sys.BusinessUserType();
                        usertype.load(_userType, _accID);
                        return usertype.Entity.UserTypeName;
                    }
                    catch { return ""; }
                }
                return "";
            }
        }

        /// <summary>
        /// ��������������ID
        /// ���ȣ�20
        /// ����Ϊ�գ���
        /// </summary>
        public string AccID
        {
            get { return _accID; }
            set { _accID = value; }
        }

        /// <summary>
        /// �����������û����
        /// ���ȣ�30
        /// ����Ϊ�գ���
        /// </summary>
        public string UserNo
        {
            get { return _userNo; }
            set { _userNo = value; }
        }

        /// <summary>
        /// �����������ǳ�
        /// ���ȣ�30
        /// ����Ϊ�գ���
        /// </summary>
        public string NickName
        {
            get { return _nickName; }
            set { _nickName = value; }
        }

        /// <summary>
        /// �����������û�����
        /// ���ȣ�50
        /// ����Ϊ�գ���
        /// </summary>
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        /// <summary>
        /// ��������������
        /// ���ȣ�50
        /// ����Ϊ�գ���
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { _password = value; }
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
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        /// <summary>
        /// ������������ַ
        /// ���ȣ�200
        /// ����Ϊ�գ���
        /// </summary>
        public string Addr
        {
            get { return _addr; }
            set { _addr = value; }
        }

        /// <summary>
        /// ����������ע������
        /// ����Ϊ�գ���
        /// </summary>
        public System.DateTime RegDate
        {
            get { return _regDate; }
            set { _regDate = value; }
        }

        /// <summary>
        /// �����������Ƿ���Ч
        /// ����Ϊ�գ���
        /// </summary>
        public bool Valid
        {
            get { return _valid; }
            set { _valid = value; }
        }

        /// <summary>
        /// �������������ű��
        /// ���ȣ�30
        /// ����Ϊ�գ���
        /// </summary>
        public string DeptNo
        {
            get { return _deptNo; }
            set { _deptNo = value; }
        }

        /// <summary>
        /// ������������������
        /// ���ȣ�50
        /// ����Ϊ�գ���
        /// </summary>
        public string DeptName
        {
            get
            {
                if (_deptNo != "" && _deptNo != null)
                {
                    try
                    {
                        Business.Sys.BusinessDept dept = new Business.Sys.BusinessDept();
                        dept.load(_deptNo, _accID);
                        return dept.Entity.DeptName;
                    }
                    catch { return ""; }
                }
                return "";
            }
        }

        /// <summary>
        /// �����������ϼ�����
        /// ���ȣ�30
        /// ����Ϊ�գ���
        /// </summary>
        public string Manager
        {
            get { return _manager; }
            set { _manager = value; }
        }

        /// <summary>
        /// ������������������
        /// ���ȣ�50
        /// ����Ϊ�գ���
        /// </summary>
        public string ManagerName
        {
            get
            {
                if (_manager != "" && _manager != null)
                {
                    try
                    {
                        Business.Sys.BusinessUserInfo user = new Business.Sys.BusinessUserInfo();
                        user.loadUserNo(_manager, _accID);
                        return user.Entity.UserName;
                    }
                    catch { return ""; }
                }
                return "";
            }
        }

        /// <summary>
        /// ������������Ƭ
        /// ���ȣ�30
        /// ����Ϊ�գ���
        /// </summary>
        public string Picture
        {
            get { return _picture; }
            set { _picture = value; }
        }
    }
}