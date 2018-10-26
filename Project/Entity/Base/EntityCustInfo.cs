using System;
namespace project.Entity.Base
{
    /// <summary>客户资料</summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    [System.Serializable]
    public class EntityCustInfo
    {
        private string _entityOID;
        private string _accID;
        private string _custNo;
        private string _custName;
        private string _custType;
        private string _Contact;
        private string _tel;
        private string _addr;
        private string _website;
        private string _remark;
        private DateTime _regDate;
        private bool _valid;

        /// <summary>缺省构造函数</summary>
        public EntityCustInfo() { }

        /// <summary>内部映射主键</summary>
        public string InnerEntityOID
        {
            get { return _entityOID; }
            set { _entityOID = value; }
        }

        /// <summary>
        /// 功能描述：账套
        /// 长度：20
        /// 不能为空：否
        /// </summary>
        public string AccID
        {
            get { return _accID; }
            set { _accID = value; }
        }

        /// <summary>
        /// 功能描述：客户编号
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string CustNo
        {
            get { return _custNo; }
            set { _custNo = value; }
        }

        /// <summary>
        /// 功能描述：客户名称
        /// 长度：80
        /// 不能为空：否
        /// </summary>
        public string CustName
        {
            get { return _custName; }
            set { _custName = value; }
        }

        /// <summary>
        /// 功能描述：客户类型
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string CustType
        {
            get { return _custType; }
            set { _custType = value; }
        }

        /// <summary>
        /// 功能描述：客户类型名称
        /// 长度：50
        /// 不能为空：否
        /// </summary>
        public string CustTypeName
        {
            get
            {
                if (_custType != "" && _custType != null)
                {
                    try
                    {
                        Business.Base.BusinessDict dict = new Business.Base.BusinessDict();
                        dict.load(_custType, "CustType", _accID);
                        return dict.Entity.DictName;
                    }
                    catch { return ""; }
                }
                return "";
            }
        }

        /// <summary>
        /// 功能描述：联系人
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string Contact
        {
            get { return _Contact; }
            set { _Contact = value; }
        }

        /// <summary>
        /// 功能描述：电话
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string Tel
        {
            get { return _tel; }
            set { _tel = value; }
        }

        /// <summary>
        /// 功能描述：地址
        /// 长度：80
        /// 不能为空：否
        /// </summary>
        public string Addr
        {
            get { return _addr; }
            set { _addr = value; }
        }

        /// <summary>
        /// 功能描述：网址
        /// 长度：50
        /// 不能为空：否
        /// </summary>
        public string Website
        {
            get { return _website; }
            set { _website = value; }
        }

        /// <summary>
        /// 功能描述：备注
        /// 长度：200
        /// 不能为空：否
        /// </summary>
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }

        /// <summary>
        /// 功能描述：注册日期
        /// 不能为空：否
        /// </summary>
        public DateTime RegDate
        {
            get { return _regDate; }
            set { _regDate = value; }
        }

        /// <summary>
        /// 功能描述：是否有效
        /// 不能为空：否
        /// </summary>
        public bool Valid
        {
            get { return _valid; }
            set { _valid = value; }
        }
    }
}
