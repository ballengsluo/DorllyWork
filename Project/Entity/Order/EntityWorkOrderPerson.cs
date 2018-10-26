using System;
namespace project.Entity.Order
{
    /// <summary>工单指派人信息</summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    [System.Serializable]
    public class EntityWorkOrderPerson
    {
        private string _entityOID;
        private string _AccID;
        private string _OrderNo;
        private string _UserNo;
        private bool _IsBack;
        private DateTime _BackDate;
        private string _BackReason;
        private bool _IsHangUp;
        private DateTime _HangUpDate;
        private string _HangUpReason;
        private DateTime _ResponseTime;
        private DateTime _AppoIntTime;
        private DateTime _SignTime;
        private DateTime _WorkTime;
        private DateTime _FinishTime;
        private DateTime _CloseTime;
        private DateTime _ConfirmTime;
        private DateTime _CreateDate;
        private string _CreateUser;
        private DateTime _UpdateDate;
        private string _UpdateUser;
        private bool _IsDel;
        
        /// <summary>缺省构造函数</summary>
        public EntityWorkOrderPerson() { }

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
        /// 功能描述：工程师ID
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string UserNo
        {
            get { return _UserNo; }
            set { _UserNo = value; }
        }

        /// <summary>
        /// 功能描述：工程师ID
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string UserName
        {
            get
            {
                if (_UserNo != "" && _UserNo != null)
                {
                    try
                    {
                        Business.Sys.BusinessUserInfo user = new Business.Sys.BusinessUserInfo();
                        user.loadUserNo(_UserNo, _AccID);
                        return user.Entity.UserName;
                    }
                    catch { return ""; }
                }
                return "";
            }
        }

        /// <summary>
        /// 功能描述：是否退回
        /// 不能为空：否
        /// </summary>
        public bool IsBack
        {
            get { return _IsBack; }
            set { _IsBack = value; }
        }

        /// <summary>
        /// 功能描述：退回日期
        /// 不能为空：否
        /// </summary>
        public DateTime BackDate
        {
            get { return _BackDate; }
            set { _BackDate = value; }
        }

        /// <summary>
        /// 功能描述：退回原因
        /// 长度：200
        /// 不能为空：否
        /// </summary>
        public string BackReason
        {
            get { return _BackReason; }
            set { _BackReason = value; }
        }

        /// <summary>
        /// 功能描述：是否挂起
        /// 不能为空：否
        /// </summary>
        public bool IsHangUp
        {
            get { return _IsHangUp; }
            set { _IsHangUp = value; }
        }

        /// <summary>
        /// 功能描述：挂起日期
        /// 不能为空：否
        /// </summary>
        public DateTime HangUpDate
        {
            get { return _HangUpDate; }
            set { _HangUpDate = value; }
        }

        /// <summary>
        /// 功能描述：挂起原因
        /// 长度：200
        /// 不能为空：否
        /// </summary>
        public string HangUpReason
        {
            get { return _HangUpReason; }
            set { _HangUpReason = value; }
        }
        
        /// <summary>
        /// 功能描述：响应日期
        /// 不能为空：否
        /// </summary>
        public DateTime ResponseTime
        {
            get { return _ResponseTime; }
            set { _ResponseTime = value; }
        }

        /// <summary>
        /// 功能描述：预约日期
        /// 不能为空：否
        /// </summary>
        public DateTime AppoIntTime
        {
            get { return _AppoIntTime; }
            set { _AppoIntTime = value; }
        }

        /// <summary>
        /// 功能描述：签到日期
        /// 不能为空：否
        /// </summary>
        public DateTime SignTime
        {
            get { return _SignTime; }
            set { _SignTime = value; }
        }

        /// <summary>
        /// 功能描述：执行日期
        /// 不能为空：否
        /// </summary>
        public DateTime WorkTime
        {
            get { return _WorkTime; }
            set { _WorkTime = value; }
        }

        /// <summary>
        /// 功能描述：完成日期
        /// 不能为空：否
        /// </summary>
        public DateTime FinishTime
        {
            get { return _FinishTime; }
            set { _FinishTime = value; }
        }

        /// <summary>
        /// 功能描述：销单日期
        /// 不能为空：否
        /// </summary>
        public DateTime CloseTime
        {
            get { return _CloseTime; }
            set { _CloseTime = value; }
        }

        /// <summary>
        /// 功能描述：确认销单日期
        /// 不能为空：否
        /// </summary>
        public DateTime ConfirmTime
        {
            get { return _ConfirmTime; }
            set { _ConfirmTime = value; }
        }

        /// <summary>
        /// 功能描述：备注
        /// 不能为空：否
        /// </summary>
        public DateTime CreateDate
        {
            get { return _CreateDate; }
            set { _CreateDate = value; }
        }

        /// <summary>
        /// 功能描述：创建人
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string CreateUser
        {
            get { return _CreateUser; }
            set { _CreateUser = value; }
        }

        /// <summary>
        /// 功能描述：最后修改日期
        /// 不能为空：否
        /// </summary>
        public DateTime UpdateDate
        {
            get { return _UpdateDate; }
            set { _UpdateDate = value; }
        }

        /// <summary>
        /// 功能描述：最后修改人
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string UpdateUser
        {
            get { return _UpdateUser; }
            set { _UpdateUser = value; }
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
