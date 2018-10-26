using System;
namespace project.Entity.Base
{
    /// <summary>�����ֵ�</summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    [System.Serializable]
    public class EntityDict
    {
        private string _dictNo;
        private string _accID;
        private string _dictName;
        private string _dictType;
        private string _remark;

        /// <summary>ȱʡ���캯��</summary>
        public EntityDict() {}

        /// <summary>�ڲ�ӳ������</summary>
        public string DictNo
        {
            get { return _dictNo; }
            set { _dictNo = value; }
        }

        /// <summary>
        /// �����������ֵ�����
        /// ���ȣ�50
        /// ����Ϊ�գ���
        /// </summary>
        public string DictName
        {
            get { return _dictName; }
            set { _dictName = value; }
        }

        /// <summary>
        /// �������������ױ��
        /// ���ȣ�20
        /// ����Ϊ�գ���
        /// </summary>
        public string AccID
        {
            get { return _accID; }
            set { _accID = value; }
        }

        /// <summary>
        /// �����������ֵ�����
        /// ���ȣ�30
        /// ����Ϊ�գ���
        /// </summary>
        public string DictType
        {
            get { return _dictType; }
            set { _dictType = value; }
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
    }
}
