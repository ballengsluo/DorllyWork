using System;
namespace project.Entity.Sys
{
    /// <summary>用户登录记录表</summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    [System.Serializable]
    public class EntityUserLog
    {
        private string _entityOID;
        private string _LogUser;
        private string _AccID;
        private string _LogType;
        private DateTime _LogDate;
        private string _LogIP;

        /// <summary>缺省构造函数</summary>
        public EntityUserLog() { }

        /// <summary>内部映射主键</summary>
        public string InnerEntityOID
        {
            get { return _entityOID; }
            set { _entityOID = value; }
        }

        /// <summary>
        /// 功能描述：登录用户编号
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string LogUser
        {
            get { return _LogUser; }
            set { _LogUser = value; }
        }

        /// <summary>
        /// 功能描述：登录用户姓名
        /// 长度：50
        /// 不能为空：否
        /// </summary>
        public string LogUserName
        {
            get
            {
                string _LogName = "";
                if (_LogUser != "")
                {
                    try
                    {
                        Business.Sys.BusinessUserInfo us = new Business.Sys.BusinessUserInfo();
                        us.loadUserNo(_LogUser, _AccID);
                        _LogName = us.Entity.UserName;
                    }
                    catch { }
                }
                return _LogName;
            }
        }

        /// <summary>
        /// 功能描述：账号
        /// 长度：20
        /// 不能为空：否
        /// </summary>
        public string AccID
        {
            get { return _AccID; }
            set { _AccID = value; }
        }

        /// <summary>
        /// 功能描述：登录类型（1电脑登录，2手机登录）
        /// 长度：10
        /// 不能为空：否
        /// </summary>
        public string LogType
        {
            get { return _LogType; }
            set { _LogType = value; }
        }

        /// <summary>
        /// 功能描述：登录类型名称（1电脑登录，2手机登录）
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string LogTypeName
        {
            get
            {
                string _LogTypeName = "";
                switch (_LogType)
                {
                    case "1":
                        _LogTypeName = "电脑登录";
                        break;
                    case "2":
                        _LogTypeName = "手机登录";
                        break;
                }
                return _LogTypeName;
            }
        }

        /// <summary>
        /// 功能描述：登录日期
        /// 不能为空：否
        /// </summary>
        public DateTime LogDate
        {
            get { return _LogDate; }
            set { _LogDate = value; }
        }

        /// <summary>
        /// 功能描述：登录IP
        /// 长度：20
        /// 不能为空：否
        /// </summary>
        public string LogIP
        {
            get { return _LogIP; }
            set { _LogIP = value; }
        }
    }
}
