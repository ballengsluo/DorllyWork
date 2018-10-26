using System;
namespace project.Entity.Base
{
    /// <summary>工单操作按钮</summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    [System.Serializable]
    public class EntityOperate
    {
        private string _opNo;
        private string _opName;
        private string _accID;

        /// <summary>缺省构造函数</summary>
        public EntityOperate() { }

        /// <summary>内部映射主键</summary>
        public string OpNo
        {
            get { return _opNo; }
            set { _opNo = value; }
        }

        /// <summary>
        /// 功能描述：操作
        /// 长度：50
        /// 不能为空：否
        /// </summary>
        public string OpName
        {
            get { return _opName; }
            set { _opName = value; }
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
    }
}
