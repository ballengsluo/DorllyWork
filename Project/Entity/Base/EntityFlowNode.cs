using System;
namespace project.Entity.Base
{
    /// <summary>工单流程节点</summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    [System.Serializable]
    public class EntityFlowNode
    {
        private string _nodeNo;
        private string _nodeName;
        private string _accID;
        private string _status;
        private string _procMode;
        private string _opNo;
        private string _opName;

        /// <summary>缺省构造函数</summary>
        public EntityFlowNode() { }

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
        /// 功能描述：状态【暂不用，默认1】
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        /// <summary>
        /// 功能描述：执行模式【决定当前页面有多少执行动作】
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string ProcMode
        {
            get { return _procMode; }
            set { _procMode = value; }
        }

        /// <summary>
        /// 功能描述：操作编号
        /// 长度：100
        /// 不能为空：否
        /// </summary>
        public string OpNo
        {
            get { return _opNo; }
            set { _opNo = value; }
        }

        /// <summary>
        /// 功能描述：操作名称
        /// 长度：500
        /// 不能为空：否
        /// </summary>
        public string OpName
        {
            get { return _opName; }
            set { _opName = value; }
        }
    }
}
