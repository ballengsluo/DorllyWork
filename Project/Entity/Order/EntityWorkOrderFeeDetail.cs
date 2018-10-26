using System;
namespace project.Entity.Order
{
    /// <summary>工单收款明细</summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    [System.Serializable]
    public class EntityWorkOrderFeeDetail
    {
        private string _entityOID;
        private string _AccID;
        private string _FeeNo;
        private string _OrderNo;
        private string _FeeType;
        private string _Context;
        private DateTime _FeeDate;
        private decimal _FeeAmount;
        private string _UserNo;
        private DateTime _CreateDate;
        private string _CreateUser;
        private DateTime _UpdateDate;
        private string _UpdateUser;
        
        /// <summary>缺省构造函数</summary>
        public EntityWorkOrderFeeDetail() { }

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
        /// 功能描述：收款单号
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string FeeNo
        {
            get { return _FeeNo; }
            set { _FeeNo = value; }
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
        /// 功能描述：收款类型
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string FeeType
        {
            get { return _FeeType; }
            set { _FeeType = value; }
        }

        /// <summary>
        /// 功能描述：收款类型名称
        /// 长度：50
        /// 不能为空：否
        /// </summary>
        public string FeeTypeName
        {
            get
            {
                string _FeeTypeName = "";
                if (_FeeType != "")
                {
                    try
                    {
                        Business.Base.BusinessDict dict = new Business.Base.BusinessDict();
                        dict.load(_FeeType, "FeeType", _AccID);
                        _FeeTypeName = dict.Entity.DictName;
                    }
                    catch { }
                }
                return _FeeTypeName;
            }
        }

        /// <summary>
        /// 功能描述：收款描述
        /// 长度：500
        /// 不能为空：否
        /// </summary>
        public string Context
        {
            get { return _Context; }
            set { _Context = value; }
        }

        /// <summary>
        /// 功能描述：收款日期
        /// 不能为空：否
        /// </summary>
        public DateTime FeeDate
        {
            get { return _FeeDate; }
            set { _FeeDate = value; }
        }

        /// <summary>
        /// 功能描述：收款金额
        /// 不能为空：否
        /// </summary>
        public decimal FeeAmount
        {
            get { return _FeeAmount; }
            set { _FeeAmount = value; }
        }

        /// <summary>
        /// 功能描述：申请用户
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string UserNo
        {
            get { return _UserNo; }
            set { _UserNo = value; }
        }

        /// <summary>
        /// 功能描述：申请用户姓名
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string UserName
        {
            get
            {
                string _UserName = "";
                if (_UserNo != "")
                {
                    try
                    {
                        Business.Sys.BusinessUserInfo us = new Business.Sys.BusinessUserInfo();
                        us.loadUserNo(_UserNo, _AccID);
                        _UserName = us.Entity.UserName;
                    }
                    catch { }
                }
                return _UserName;
            }
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
        /// 功能描述：最后更新日期
        /// 不能为空：否
        /// </summary>
        public DateTime UpdateDate
        {
            get { return _UpdateDate; }
            set { _UpdateDate = value; }
        }

        /// <summary>
        /// 功能描述：最后更新人
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string UpdateUser
        {
            get { return _UpdateUser; }
            set { _UpdateUser = value; }
        }
    }
}
