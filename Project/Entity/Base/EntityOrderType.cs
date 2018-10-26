using System;
namespace project.Entity.Base
{
    /// <summary>工单类型表</summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    [System.Serializable]
    public class EntityOrderType
    {
        private string _OrderTypeNo;
        private string _OrderTypeName;
        private string _FlowNo;
        private string _FlowName;
        private string _AccID;

        /// <summary>缺省构造函数</summary>
        public EntityOrderType() { }

        /// <summary>工单类型编号</summary>
        public string OrderTypeNo
        {
            get { return _OrderTypeNo; }
            set { _OrderTypeNo = value; }
        }

        /// <summary>
        /// 功能描述：工单类型名称
        /// 长度：50
        /// 不能为空：否
        /// </summary>
        public string OrderTypeName
        {
            get { return _OrderTypeName; }
            set { _OrderTypeName = value; }
        }

        /// <summary>
        /// 功能描述：流程编号
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string FlowNo
        {
            get { return _FlowNo; }
            set { _FlowNo = value; }
        }

        /// <summary>
        /// 功能描述：流程名称
        /// 长度：50
        /// 不能为空：否
        /// </summary>
        public string FlowName
        {
            get { return _FlowName; }
            set { _FlowName = value; }
        }

        /// <summary>
        /// 功能描述：账户
        /// 长度：20
        /// 不能为空：否
        /// </summary>
        public string AccID
        {
            get { return _AccID; }
            set { _AccID = value; }
        }
    }
}
