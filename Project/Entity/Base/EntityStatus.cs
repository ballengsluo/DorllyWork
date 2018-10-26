using System;
namespace project.Entity.Base
{
    /// <summary>工单状态</summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    [System.Serializable]
    public class EntityStatus
    {
        private string _statusNo;
        private string _statusName;
        private string _accID;
        private string _ordNo;
        private string _nodeNo;

        /// <summary>缺省构造函数</summary>
        public EntityStatus() { }

        /// <summary>状态编号</summary>
        public string StatusNo
        {
            get { return _statusNo; }
            set { _statusNo = value; }
        }

        /// <summary>
        /// 功能描述：状态名称
        /// 长度：50
        /// 不能为空：否
        /// </summary>
        public string StatusName
        {
            get { return _statusName; }
            set { _statusName = value; }
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
        /// 功能描述：节点
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string NodeNo
        {
            get { return _nodeNo; }
            set { _nodeNo = value; }
        }
    }
}
