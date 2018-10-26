using System;
namespace project.Entity.Order
{
    /// <summary>工单费用明细</summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    [System.Serializable]
    public class EntityWorkOrderCostDetail
    {
        private string _entityOID;
        private string _AccID;
        private string _CostNo;
        private string _OrderNo;
        private string _CostType;
        private string _Context;
        private DateTime _CostDate;
        private decimal _CostAmount;
        private string _UserNo;
        private DateTime _CreateDate;
        private string _CreateUser;
        private DateTime _UpdateDate;
        private string _UpdateUser;
        
        /// <summary>缺省构造函数</summary>
        public EntityWorkOrderCostDetail() { }

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
        /// 功能描述：费用单号
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string CostNo
        {
            get { return _CostNo; }
            set { _CostNo = value; }
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
        /// 功能描述：费用类型
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string CostType
        {
            get { return _CostType; }
            set { _CostType = value; }
        }

        /// <summary>
        /// 功能描述：费用类型名称
        /// 长度：50
        /// 不能为空：否
        /// </summary>
        public string CostTypeName
        {
            get
            {
                string _CostTypeName = "";
                if (_CostType != "")
                {
                    try
                    {
                        Business.Base.BusinessDict dict = new Business.Base.BusinessDict();
                        dict.load(_CostType, "CostType", _AccID);
                        _CostTypeName = dict.Entity.DictName;
                    }
                    catch { }
                }
                return _CostTypeName;
            }
        }

        /// <summary>
        /// 功能描述：费用描述
        /// 长度：500
        /// 不能为空：否
        /// </summary>
        public string Context
        {
            get { return _Context; }
            set { _Context = value; }
        }

        /// <summary>
        /// 功能描述：费用日期
        /// 不能为空：否
        /// </summary>
        public DateTime CostDate
        {
            get { return _CostDate; }
            set { _CostDate = value; }
        }

        /// <summary>
        /// 功能描述：费用金额
        /// 不能为空：否
        /// </summary>
        public decimal CostAmount
        {
            get { return _CostAmount; }
            set { _CostAmount = value; }
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
