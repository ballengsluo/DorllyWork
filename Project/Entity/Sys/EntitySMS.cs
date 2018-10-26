using System;
namespace project.Entity.Admin
{
    /// <summary>短信</summary>
    /// <author>tz</author>
    /// <date>2015-3-15</date>
    [System.Serializable]
    public class EntitySMS
    {
        private string _entityOID;
        private string _SendType;
        private string _SendPerson;
        private string _Tel;
        private string _Content;
        private DateTime _CreateDate;
        private DateTime _SendDate;
        private string _SendStat;
        private string _SendResult;
        private string _RefNo;
        private bool _IsSend;
        private string _VerifyNo;

        /// <summary>缺省构造函数</summary>
        public EntitySMS() { }

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
        /// 功能描述：短信类型
        /// 长度：50
        /// 不能为空：否
        /// </summary>
        public string SendType
        {
            get { return _SendType; }
            set { _SendType = value; }
        }

        /// <summary>
        /// 功能描述：发送人
        /// 长度：50
        /// 不能为空：否
        /// </summary>
        public string SendPerson
        {
            get { return _SendPerson; }
            set { _SendPerson = value; }
        }

        /// <summary>
        /// 功能描述：发送号码
        /// 长度：50
        /// 不能为空：否
        /// </summary>
        public string Tel
        {
            get { return _Tel; }
            set { _Tel = value; }
        }

        /// <summary>
        /// 功能描述：短信内容
        /// 不能为空：否
        /// </summary>
        public string Content
        {
            get { return _Content; }
            set { _Content = value; }
        }

        /// <summary>
        /// 功能描述：创建日期
        /// 不能为空：否
        /// </summary>
        public DateTime CreateDate
        {
            get { return _CreateDate; }
            set { _CreateDate = value; }
        }

        /// <summary>
        /// 功能描述：发送日期
        /// 不能为空：否
        /// </summary>
        public DateTime SendDate
        {
            get { return _SendDate; }
            set { _SendDate = value; }
        }

        /// <summary>
        /// 功能描述：发送状态
        /// 长度：50
        /// 不能为空：否
        /// </summary>
        public string SendStat
        {
            get { return _SendStat; }
            set { _SendStat = value; }
        }

        /// <summary>
        /// 功能描述：发送结果
        /// 长度：50
        /// 不能为空：否
        /// </summary>
        public string SendResult
        {
            get { return _SendResult; }
            set { _SendResult = value; }
        }

        /// <summary>
        /// 功能描述：关联单号
        /// 长度：50
        /// 不能为空：否
        /// </summary>
        public string RefNo
        {
            get { return _RefNo; }
            set { _RefNo = value; }
        }
        /// <summary>
        /// 功能描述：验证码
        /// 长度：50
        /// 不能为空：否
        /// </summary>
        public string VerifyNo
        {
            get { return _VerifyNo; }
            set { _VerifyNo = value; }
        }        
        /// <summary>
        /// 功能描述：是否发送
        /// 不能为空：否
        /// </summary>
        public bool IsSend
        {
            get { return _IsSend; }
            set { _IsSend = value; }
        }
    }
}
