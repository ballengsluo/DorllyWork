using System;
namespace project.Entity.Sys
{
    /// <summary>账套信息</summary>
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

        /// <summary>缺省构造函数</summary>
        public EntityAccInfo() {}

        /// <summary>主键，只读属性</summary>
        public System.Guid EntityOID
        {
            get { return new System.Guid(_entityOID); }
        }

        /// <summary>内部映射主键</summary>
        public string InnerEntityOID
        {
            get { return _entityOID; }
            set { _entityOID = value; }
        }

        /// <summary>
        /// 功能描述：公司ID
        /// 长度：20
        /// 不能为空：否
        /// </summary>
        public string AccID
        {
            get { return _accID; }
            set { _accID = value; }
        }

        /// <summary>
        /// 功能描述：公司名称
        /// 长度：200
        /// 不能为空：否
        /// </summary>
        public string AccName
        {
            get { return _accName; }
            set { _accName = value; }
        }

        /// <summary>
        /// 功能描述：公司简称
        /// 长度：200
        /// 不能为空：否
        /// </summary>
        public string AccBrfName
        {
            get { return _accBrfName; }
            set { _accBrfName = value; }
        }

        /// <summary>
        /// 功能描述：地址
        /// 长度：500
        /// 不能为空：否
        /// </summary>
        public string Addr
        {
            get { return _addr; }
            set { _addr = value; }
        }

        /// <summary>
        /// 功能描述：电话
        /// 长度：50
        /// 不能为空：否
        /// </summary>
        public string Tel
        {
            get { return _tel; }
            set { _tel = value; }
        }

        /// <summary>
        /// 功能描述：传真
        /// 长度：50
        /// 不能为空：否
        /// </summary>
        public string Fax
        {
            get { return _fax; }
            set { _fax = value; }
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
        /// 功能描述：联系人
        /// 长度：50
        /// 不能为空：否
        /// </summary>
        public string Contact
        {
            get { return _contact; }
            set { _contact = value; }
        }

        /// <summary>
        /// 功能描述：联系人电话
        /// 长度：50
        /// 不能为空：否
        /// </summary>
        public string ContactTel
        {
            get { return _contactTel; }
            set { _contactTel = value; }
        }

        /// <summary>
        /// 功能描述：图片
        /// 长度：50
        /// 不能为空：否
        /// </summary>
        public string Pic
        {
            get { return _pic; }
            set { _pic = value; }
        }

        /// <summary>
        /// 功能描述：备注
        /// 长度：3000
        /// 不能为空：否
        /// </summary>
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }

        /// <summary>
        /// 功能描述：用户数量
        /// 不能为空：否
        /// </summary>
        public int UserCount
        {
            get { return _userCount; }
            set { _userCount = value; }
        }

        /// <summary>
        /// 功能描述：登记日期
        /// 不能为空：否
        /// </summary>
        public System.DateTime RegDate
        {
            get { return _regDate; }
            set { _regDate = value; }
        }

        /// <summary>
        /// 功能描述：过期日期
        /// 不能为空：否
        /// </summary>
        public System.DateTime LimitedDate
        {
            get { return _limitedDate; }
            set { _limitedDate = value; }
        }
    }
}
