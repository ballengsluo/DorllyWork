using System;
namespace project.Entity.Order
{
    /// <summary>工单信息</summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    [System.Serializable]
    public class EntityWorkOrder
    {
        private string _entityOID;
        private string _AccID;
        private string _OrderNo;
        private string _OrderName;
        private DateTime _OrderDate;
        private string _OrderType;
        private string _Status;
        private string _SaleNo;
        private string _AlloDept;
        private string _AlloUser;
        private string _CustNo;
        private string _LinkMan;
        private string _LinkTel;
        private string _Addr;
        private string _Region;
        private DateTime _CreateTime;
        private DateTime _CustneedTime;
        private DateTime _ResponseTime;
        private DateTime _AppoIntTime;
        private DateTime _SignTime;
        private DateTime _WorkTime;
        private DateTime _FinishTime;
        private DateTime _CloseTime;
        private DateTime _ConfirmTime;
        private string _Remark;
        private string _CreateUser;
        private bool _IsHangUp;
        private DateTime _HangUpDate;
        private string _HangUpReason;
        private bool _IsApply;
        private DateTime _ApplyDate;
        private string _ApplyReason;
        private bool _IsBack;
        private DateTime _BackDate;
        private string _BackReason;
        private bool _IsDel;
        private DateTime _UpdateDate;
        private string _UpdateUser;
        private string _DONo;
        
        /// <summary>缺省构造函数</summary>
        public EntityWorkOrder() { }

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
        /// 功能描述：工单名称
        /// 长度：200
        /// 不能为空：否
        /// </summary>
        public string OrderName
        {
            get { return _OrderName; }
            set { _OrderName = value; }
        }

        /// <summary>
        /// 功能描述：工单日期
        /// 不能为空：否
        /// </summary>
        public DateTime OrderDate
        {
            get { return _OrderDate; }
            set { _OrderDate = value; }
        }

        /// <summary>
        /// 功能描述：工单类型
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string OrderType
        {
            get { return _OrderType; }
            set { _OrderType = value; }
        }

        /// <summary>
        /// 功能描述：工单类型名称
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string OrderTypeName
        {
            get
            {
                if (_OrderType != "" && _OrderType != null)
                {
                    try
                    {
                        Business.Base.BusinessOrderType type = new Business.Base.BusinessOrderType();
                        type.load(_OrderType, _AccID);
                        return type.Entity.OrderTypeName;
                    }
                    catch { return ""; }
                }
                return "";
            }
        }

        /// <summary>
        /// 功能描述：状态
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        /// <summary>
        /// 功能描述：状态(1新建 2响应 3预约 4签到 5执行 6执行 7完成 8销单 9确认销单)
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string StatusName
        {
            get
            {
                string _StatusName = "";
                if (_Status != "")
                {
                    try
                    {
                        Business.Base.BusinessStatus st = new Business.Base.BusinessStatus();
                        st.load(_Status, _AccID);
                        _StatusName = st.Entity.StatusName;
                    }
                    catch { }
                }
                return _StatusName;
            }
        }

        /// <summary>
        /// 功能描述：节点
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string NodeNo
        {
            get
            {
                string _NodeNo = "";
                if (_Status != "")
                {
                    try
                    {
                        Business.Base.BusinessStatus st = new Business.Base.BusinessStatus();
                        st.load(_Status, _AccID);
                        _NodeNo = st.Entity.NodeNo;
                    }
                    catch { }
                }
                return _NodeNo;
            }
        }

        /// <summary>
        /// 功能描述：销售单号
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string SaleNo
        {
            get { return _SaleNo; }
            set { _SaleNo = value; }
        }

        /// <summary>
        /// 功能描述：分配部门
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string AlloDept
        {
            get { return _AlloDept; }
            set { _AlloDept = value; }
        }

        /// <summary>
        /// 功能描述：分配部门名称
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string AlloDeptName
        {
            get
            {
                if (_AlloDept != "" && _AlloDept != null)
                {
                    try
                    {
                        Business.Sys.BusinessDept dept = new Business.Sys.BusinessDept();
                        dept.load(_AlloDept, _AccID);
                        return dept.Entity.DeptName;
                    }
                    catch { return ""; }
                }
                return "";
            }
        }

        /// <summary>
        /// 功能描述：分配人员
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string AlloUser
        {
            get { return _AlloUser; }
            set { _AlloUser = value; }
        }

        /// <summary>
        /// 功能描述：分配人员名称
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string AlloUserName
        {
            get
            {
                if (_AlloUser != "" && _AlloUser != null)
                {
                    try
                    {
                        Business.Sys.BusinessUserInfo user = new Business.Sys.BusinessUserInfo();
                        user.loadUserNo(_AlloUser, _AccID);
                        return user.Entity.UserName;
                    }
                    catch { return ""; }
                }
                return "";
            }
        }

        /// <summary>
        /// 功能描述：工单处理人【非维护字段】
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string Person
        {
            get
            {
                string _person="";
                try
                {
                    Business.Order.BusinessWorkOrderPerson bp = new Business.Order.BusinessWorkOrderPerson();
                    foreach(Entity.Order.EntityWorkOrderPerson it in bp.GetWorkOrderPersonListQuery(_AccID,_OrderNo,string.Empty,null,false)){
                        if (_person == "") _person = it.UserNo;
                        else _person +=";" + it.UserNo;
                    }
                }
                catch { _person = ""; }
                return _person;
            }
        }

        /// <summary>
        /// 功能描述：工单处理人【非维护字段】
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string PersonName
        {
            get
            {
                string _personName = "";
                try
                {
                    Business.Order.BusinessWorkOrderPerson bp = new Business.Order.BusinessWorkOrderPerson();
                    foreach (Entity.Order.EntityWorkOrderPerson it in bp.GetWorkOrderPersonListQuery(_AccID, _OrderNo, string.Empty, null, false))
                    {
                        if (_personName == "") _personName = it.UserName;
                        else _personName += ";" + it.UserName;
                    }
                }
                catch { _personName = ""; }
                return _personName;
            }
        }

        /// <summary>
        /// 功能描述：客户编号
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
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string CustName
        {
            get
            {
                if (_CustNo != "" && _CustNo != null)
                {
                    try
                    {
                        Business.Base.BusinessCustInfo cust = new Business.Base.BusinessCustInfo();
                        cust.loadCustNo(_CustNo, _AccID);
                        return cust.Entity.CustName;
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
        public string LinkMan
        {
            get { return _LinkMan; }
            set { _LinkMan = value; }
        }

        /// <summary>
        /// 功能描述：联系人电话
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string LinkTel
        {
            get { return _LinkTel; }
            set { _LinkTel = value; }
        }

        /// <summary>
        /// 功能描述：地址
        /// 长度：200
        /// 不能为空：否
        /// </summary>
        public string Addr
        {
            get { return _Addr; }
            set { _Addr = value; }
        }

        /// <summary>
        /// 功能描述：地区
        /// 长度：50
        /// 不能为空：否
        /// </summary>
        public string Region
        {
            get { return _Region; }
            set { _Region = value; }
        }

        /// <summary>
        /// 功能描述：地区名称
        /// 长度：50
        /// 不能为空：否
        /// </summary>
        public string RegionName
        {
            get
            {
                string _regionName = "";
                if (_Region != "")
                {
                    try
                    {
                        Business.Base.BusinessRegion region = new Business.Base.BusinessRegion();
                        region.load(_Region, _AccID);
                        _regionName = region.Entity.RegionName;
                    }
                    catch { }
                }

                return _regionName;
            }
        }

        /// <summary>
        /// 功能描述：创建日期
        /// 不能为空：否
        /// </summary>
        public DateTime CreateTime
        {
            get { return _CreateTime; }
            set { _CreateTime = value; }
        }

        /// <summary>
        /// 功能描述：客户要求日期
        /// 不能为空：否
        /// </summary>
        public DateTime CustneedTime
        {
            get { return _CustneedTime; }
            set { _CustneedTime = value; }
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
        /// 长度：200
        /// 不能为空：否
        /// </summary>
        public string Remark
        {
            get { return _Remark; }
            set { _Remark = value; }
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
        /// 功能描述：创建人
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string CreateUserName
        {
            get
            {
                if (_CreateUser != "" && _CreateUser != null)
                {
                    try
                    {
                        Business.Sys.BusinessUserInfo user = new Business.Sys.BusinessUserInfo();
                        user.loadUserNo(_CreateUser, _AccID);
                        return user.Entity.UserName;
                    }
                    catch { return ""; }
                }
                return "";
            }
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
        /// 功能描述：是否申请支援
        /// 不能为空：否
        /// </summary>
        public bool IsApply
        {
            get { return _IsApply; }
            set { _IsApply = value; }
        }

        /// <summary>
        /// 功能描述：申请支援日期
        /// 不能为空：否
        /// </summary>
        public DateTime ApplyDate
        {
            get { return _ApplyDate; }
            set { _ApplyDate = value; }
        }

        /// <summary>
        /// 功能描述：支援原因
        /// 长度：200
        /// 不能为空：否
        /// </summary>
        public string ApplyReason
        {
            get { return _ApplyReason; }
            set { _ApplyReason = value; }
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
        /// 功能描述：是否删除
        /// 不能为空：否
        /// </summary>
        public bool IsDel
        {
            get { return _IsDel; }
            set { _IsDel = value; }
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
        /// 功能描述：订单号
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string DONo
        {
            get { return _DONo; }
            set { _DONo = value; }
        }

    }
}
