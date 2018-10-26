using System;
namespace project.Entity.Order
{
    /// <summary>工单费用</summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    [System.Serializable]
    public class EntityWorkOrderCost
    {
        private string _entityOID;
        private string _AccID;
        private string _CostNo;
        private string _OrderNo;
        private string _OrderName;
        private DateTime _OrderDate;
        private string _OrderStatus;
        private string _OrderStatusName;
        private DateTime _CostDate;
        private decimal _CostAmount;
        private string _Status;
        private DateTime _CreateDate;
        private string _CreateUser;

        /// <summary>缺省构造函数</summary>
        public EntityWorkOrderCost() { }

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
        /// 功能描述：工单内容 - 非维护字段
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string OrderName
        {
            get { return _OrderName; }
            set { _OrderName = value; }
        }

        /// <summary>
        /// 功能描述：工单日期 - 非维护字段
        /// 不能为空：否
        /// </summary>
        public DateTime OrderDate
        {
            get { return _OrderDate; }
            set { _OrderDate = value; }
        }

        /// <summary>
        /// 功能描述：工单状态 - 非维护字段
        /// 长度：20
        /// 不能为空：否
        /// </summary>
        public string OrderStatus
        {
            get { return _OrderStatus; }
            set { _OrderStatus = value; }
        }

        /// <summary>
        /// 功能描述：工单状态名称 - 非维护字段
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string OrderStatusName
        {
            get { return _OrderStatusName; }
            set { _OrderStatusName = value; }
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
        /// 功能描述：状态
        /// 长度：20
        /// 不能为空：否
        /// </summary>
        public string Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        /// <summary>
        /// 功能描述：状态
        /// 长度：20
        /// 不能为空：否
        /// </summary>
        public string StatusName
        {
            get
            {
                string _StatusName = "";
                switch (_Status)
                {
                    case "OPEN":
                        _StatusName = "新建";
                        break;
                    case "APPROVE":
                        _StatusName = "已审核";
                        break;
                    case "CONFIRM":
                        _StatusName = "已复核";
                        break;
                }
                return _StatusName;
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
    }
}
