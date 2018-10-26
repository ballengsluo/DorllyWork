using System;
namespace project.Entity.Base
{
    /// <summary>工单流程明细</summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    [System.Serializable]
    public class EntityFlowDetail
    {
        private string _entityOID;
        private string _nodeNo;
        private string _nodeName;
        private string _accID;
        private string _flowNo;

        /// <summary>缺省构造函数</summary>
        public EntityFlowDetail() { }

        /// <summary>主键</summary>
        public string InnerEntityOID
        {
            get { return _entityOID; }
            set { _entityOID = value; }
        }

        /// <summary>节点编号</summary>
        public string NodeNo
        {
            get { return _nodeNo; }
            set { _nodeNo = value; }
        }

        /// <summary>
        /// 功能描述：节点名称
        /// 长度：50
        /// 不能为空：否
        /// </summary>
        public string NodeName
        {
            get { return _nodeName; }
            set { _nodeName = value; }
        }

        /// <summary>
        /// 功能描述：账套
        /// 长度：20
        /// 不能为空：否
        /// </summary>
        public string AccID
        {
            get { return _accID; }
            set { _accID = value; }
        }

        /// <summary>
        /// 功能描述：流程编号
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string FlowNo
        {
            get { return _flowNo; }
            set { _flowNo = value; }
        }
    }
}
