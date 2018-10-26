using System;
namespace project.Entity.Order
{
    /// <summary>个人消息</summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    [System.Serializable]
    public class EntityWorkOrderMsg
    {
        private string _entityOID;
        private string _AccID;
        private string _MsgType;
        private string _Sender;
        private DateTime _SendDate;
        private string _Subject;
        private string _Context;
        private string _ToUser;
        private bool _IsRead;
        private DateTime _ReadDate;
        private string _RefNo;
        private DateTime _CreateDate;
        private string _CreateUser;
        private bool _IsDel;

        /// <summary>缺省构造函数</summary>
        public EntityWorkOrderMsg() { }

        /// <summary>主键</summary>
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
            get { return _AccID; }
            set { _AccID = value; }
        }

        /// <summary>
        /// 功能描述：消息类型
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string MsgType
        {
            get { return _MsgType; }
            set { _MsgType = value; }
        }

        /// <summary>
        /// 功能描述：消息类型名称
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string MsgTypeName
        {
            get
            {
                string _MsgTypeName = "";
                switch (_MsgType)
                {
                    case "1":
                        _MsgTypeName = "新工单提醒";
                        break;
                    case "2":
                        _MsgTypeName = "申请支援提醒";
                        break;
                }
                return _MsgTypeName;
            }
        }

        /// <summary>
        /// 功能描述：发送人
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string Sender
        {
            get { return _Sender; }
            set { _Sender = value; }
        }

        /// <summary>
        /// 功能描述：操作人
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string SenderName
        {
            get
            {
                string _SenderName = "";
                if (_Sender != "")
                {
                    try
                    {
                        Business.Sys.BusinessUserInfo us = new Business.Sys.BusinessUserInfo();
                        us.loadUserNo(_Sender, _AccID);
                        _SenderName = us.Entity.UserName;
                    }
                    catch { }
                }
                return _SenderName;
            }
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
        /// 功能描述：主题
        /// 长度：200
        /// 不能为空：否
        /// </summary>
        public string Subject
        {
            get { return _Subject; }
            set { _Subject = value; }
        }

        /// <summary>
        /// 功能描述：内容
        /// 长度：3000
        /// 不能为空：否
        /// </summary>
        public string Context
        {
            get { return _Context; }
            set { _Context = value; }
        }

        /// <summary>
        /// 功能描述：接收人
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string ToUser
        {
            get { return _ToUser; }
            set { _ToUser = value; }
        }

        /// <summary>
        /// 功能描述：操作人
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string ToUserName
        {
            get
            {
                string _ToUserName = "";

                if (_ToUser != "")
                {
                    try
                    {
                        Business.Sys.BusinessUserInfo us = new Business.Sys.BusinessUserInfo();
                        us.loadUserNo(_ToUser, _AccID);
                        _ToUserName = us.Entity.UserName;
                    }
                    catch { }
                }
                return _ToUserName;
            }
        }

        /// <summary>
        /// 功能描述：是否已读
        /// 不能为空：否
        /// </summary>
        public bool IsRead
        {
            get { return _IsRead; }
            set { _IsRead = value; }
        }

        /// <summary>
        /// 功能描述：阅读日期
        /// 不能为空：否
        /// </summary>
        public DateTime ReadDate
        {
            get { return _ReadDate; }
            set { _ReadDate = value; }
        }

        /// <summary>
        /// 功能描述：相关单号
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string RefNo
        {
            get { return _RefNo; }
            set { _RefNo = value; }
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
        /// 功能描述：创建用户
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string CreateUser
        {
            get { return _CreateUser; }
            set { _CreateUser = value; }
        }

        /// <summary>
        /// 功能描述：是否删除
        /// 不能为空：否
        /// </summary>
        public bool IsDel
        {
            get { return _IsDel; }
            set { _IsDel = value; }
        }
    }
}
