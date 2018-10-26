using System;
namespace project.Entity.Order
{
    /// <summary>工作操作日志信息</summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    [System.Serializable]
    public class EntityWorkOrderLog
    {
        private string _entityOID;
        private string _AccID;
        private string _OrderNo;
        private DateTime _LogDate;
        private string _GPS_X;
        private string _GPS_Y;
        private string _LogType;
        private string _LogUser;
        private string _CustNo;
        private string _CustName;
        private string _Remark;

        /// <summary>缺省构造函数</summary>
        public EntityWorkOrderLog() { }

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
        /// 功能描述：工单编号
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string OrderNo
        {
            get { return _OrderNo; }
            set { _OrderNo = value; }
        }

        /// <summary>
        /// 功能描述：日志日期
        /// 不能为空：否
        /// </summary>
        public DateTime LogDate
        {
            get { return _LogDate; }
            set { _LogDate = value; }
        }

        /// <summary>
        /// 功能描述：坐标X
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string GPS_X
        {
            get { return _GPS_X; }
            set { _GPS_X = value; }
        }

        /// <summary>
        /// 功能描述：坐标Y
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string GPS_Y
        {
            get { return _GPS_Y; }
            set { _GPS_Y = value; }
        }

        /// <summary>
        /// 功能描述：日志类型
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string LogType
        {
            get { return _LogType; }
            set { _LogType = value; }
        }

        /// <summary>
        /// 功能描述：日志类型
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
                    case "Add":
                        _LogTypeName = "新建";
                        break;
                    case "HangUp":
                        _LogTypeName = "挂起";
                        break;
                    case "UnHangUp":
                        _LogTypeName = "取消挂起";
                        break;
                    case "Back":
                        _LogTypeName = "退回";
                        break;
                    case "Apply":
                        _LogTypeName = "申请支援";
                        break;
                    case "Response":
                        _LogTypeName = "响应";
                        break;
                    case "AppoInt":
                        _LogTypeName = "预约";
                        break;
                    case "Sign":
                        _LogTypeName = "签到";
                        break;
                    case "Work":
                        _LogTypeName = "执行";
                        break;
                    case "Finish":
                        _LogTypeName = "完成";
                        break;
                    case "Close":
                        _LogTypeName = "销单";
                        break;
                    case "Confirm":
                        _LogTypeName = "确认销单";
                        break;
                }
                return _LogTypeName;            
            }
        }

        /// <summary>
        /// 功能描述：操作人
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string LogUser
        {
            get { return _LogUser; }
            set { _LogUser = value; }
        }

        /// <summary>
        /// 功能描述：操作人
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string LogUserName
        {
            get {
                string _LogUserName = "";

                if (_LogUser != "") {
                    try
                    {
                        Business.Sys.BusinessUserInfo us = new Business.Sys.BusinessUserInfo();
                        us.loadUserNo(_LogUser, _AccID);
                        _LogUserName = us.Entity.UserName;
                    }
                    catch { }
                }
                return _LogUserName;
            }
        }

        /// <summary>
        /// 功能描述：客户
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string CustNo
        {
            get { return _CustNo; }
            set { _CustNo = value; }
        }

        /// <summary>
        /// 功能描述：客户名称
        /// 长度：80
        /// 不能为空：否
        /// </summary>
        public string CustName
        {
            get { return _CustName; }
            set { _CustName = value; }
        }

        /// <summary>
        /// 功能描述：备注
        /// 不能为空：否
        /// </summary>
        public string Remark
        {
            get { return _Remark; }
            set { _Remark = value; }
        }
    }
}
