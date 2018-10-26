using System;
namespace project.Entity.Sys
{
    /// <summary>用户类型表</summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    [System.Serializable]
    public class EntityUserType
    {
        private string _UserTypeNo;
        private string _UserTypeName;
        private string _OrderType;
        private string _OrderTypeName;
        private string _AccID;

        /// <summary>缺省构造函数</summary>
        public EntityUserType() { }

        /// <summary>用户类型编号</summary>
        public string UserTypeNo
        {
            get { return _UserTypeNo; }
            set { _UserTypeNo = value; }
        }

        /// <summary>
        /// 功能描述：用户类型名称
        /// 长度：50
        /// 不能为空：否
        /// </summary>
        public string UserTypeName
        {
            get { return _UserTypeName; }
            set { _UserTypeName = value; }
        }

        /// <summary>
        /// 功能描述：工单类型（多选）
        /// 长度：3000
        /// 不能为空：否
        /// </summary>
        public string OrderType
        {
            get { return _OrderType; }
            set { _OrderType = value; }
        }

        /// <summary>
        /// 功能描述：工单类型（多选）
        /// 长度：3000
        /// 不能为空：否
        /// </summary>
        public string OrderTypeName
        {
            get { return _OrderTypeName; }
            set { _OrderTypeName = value; }
        }

        /// <summary>
        /// 功能描述：账号
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
