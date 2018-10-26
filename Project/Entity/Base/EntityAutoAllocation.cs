using System;
namespace project.Entity.Base
{
    /// <summary>数据字典</summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    [System.Serializable]
    public class EntityAutoAllocation
    {
        private string _rowPointer;
        private string _accID;
        private string _orderType;
        private string _regionNo;
        private string _deptNo;
        private string _userNo;
        private string _userName;

        /// <summary>缺省构造函数</summary>
        public EntityAutoAllocation() { }

        /// <summary>内部映射主键</summary>
        public string RowPointer
        {
            get { return _rowPointer; }
            set { _rowPointer = value; }
        }

        /// <summary>
        /// 功能描述：账套编号
        /// 长度：20
        /// 不能为空：否
        /// </summary>
        public string AccID
        {
            get { return _accID; }
            set { _accID = value; }
        }

        /// <summary>
        /// 功能描述：订单类型
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string OrderType
        {
            get { return _orderType; }
            set { _orderType = value; }
        }

        /// <summary>
        /// 功能描述：订单类型名称【非维护字段】
        /// </summary>
        public string OrderTypeName
        {
            get
            {
                if (_orderType != "" && _orderType != null)
                {
                    try
                    {
                        Business.Base.BusinessOrderType bt = new Business.Base.BusinessOrderType();
                        bt.load(_orderType, _accID);
                        return bt.Entity.OrderTypeName;
                    }
                    catch { return ""; }
                }
                return "";
            }
        }

        /// <summary>
        /// 功能描述：区域编号
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string RegionNo
        {
            get { return _regionNo; }
            set { _regionNo = value; }
        }

        /// <summary>
        /// 功能描述：区域名称【非维护字段】
        /// </summary>
        public string RegionName
        {
            get
            {
                if (_regionNo != "" && _regionNo != null)
                {
                    try
                    {
                        Business.Base.BusinessRegion br = new Business.Base.BusinessRegion();
                        br.load(_regionNo, _accID);
                        return br.Entity.RegionName;
                    }
                    catch { return ""; }
                }
                return "";
            }
        }

        /// <summary>
        /// 功能描述：分配部门
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string DeptNo
        {
            get { return _deptNo; }
            set { _deptNo = value; }
        }

        /// <summary>
        /// 功能描述：分配部门名称【非维护字段】
        /// </summary>
        public string DeptName
        {
            get
            {
                if (_deptNo != "" && _deptNo != null)
                {
                    try
                    {
                        Business.Sys.BusinessDept bd = new Business.Sys.BusinessDept();
                        bd.load(_deptNo, _accID);
                        return bd.Entity.DeptName;
                    }
                    catch { return ""; }
                }
                return "";
            }
        }

        /// <summary>
        /// 功能描述：分配用户
        /// 长度：300
        /// 不能为空：否
        /// </summary>
        public string UserNo
        {
            get { return _userNo; }
            set { _userNo = value; }
        }

        /// <summary>
        /// 功能描述：分配用户名称
        /// 长度：800
        /// 不能为空：否
        /// </summary>
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }
    }
}
