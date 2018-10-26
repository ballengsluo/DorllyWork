using System;
namespace project.Entity.Base
{
    /// <summary>工单流程</summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    [System.Serializable]
    public class EntityFlow
    {
        private string _flowNo;
        private string _flowName;
        private string _accID;
        private string _ordNo;
        private string _remark;

        /// <summary>缺省构造函数</summary>
        public EntityFlow() { }

        /// <summary>流程编号</summary>
        public string FlowNo
        {
            get { return _flowNo; }
            set { _flowNo = value; }
        }

        /// <summary>
        /// 功能描述：流程名称
        /// 长度：50
        /// 不能为空：否
        /// </summary>
        public string FlowName
        {
            get { return _flowName; }
            set { _flowName = value; }
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
        /// 功能描述：排序字段
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string OrdNo
        {
            get { return _ordNo; }
            set { _ordNo = value; }
        }

        /// <summary>
        /// 功能描述：备注
        /// 长度：3000
        /// 不能为空：否
        /// </summary>
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
    }
}
