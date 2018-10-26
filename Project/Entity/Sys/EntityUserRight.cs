using System;
namespace project.Entity.Sys
{
    /// <summary>�û�Ȩ��</summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    [System.Serializable]
    public class EntityUserInfoRight
    {
        private string _entityOID;
        private string _menuID;
        private string _userType;
        private string _accID;

        /// <summary>ȱʡ���캯��</summary>
        public EntityUserInfoRight() {}

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
        /// �����������˵�ID
        /// ���ȣ�50
        /// ����Ϊ�գ���
        /// </summary>
        public string MenuID
        {
            get { return _menuID; }
            set { _menuID = value; }
        }

        /// <summary>
        /// �����������û�����
        /// ���ȣ�30
        /// ����Ϊ�գ���
        /// </summary>
        public string UserType
        {
            get { return _userType; }
            set { _userType = value; }
        }

        /// <summary>
        /// �����������˻�
        /// ���ȣ�20
        /// ����Ϊ�գ���
        /// </summary>
        public string AccID
        {
            get { return _accID; }
            set { _accID = value; }
        }
    }
}
