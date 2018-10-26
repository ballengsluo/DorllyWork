using System;
namespace project.Entity.Base
{
    /// <summary>数据字典</summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    [System.Serializable]
    public class EntityDict
    {
        private string _dictNo;
        private string _accID;
        private string _dictName;
        private string _dictType;
        private string _remark;

        /// <summary>缺省构造函数</summary>
        public EntityDict() {}

        /// <summary>内部映射主键</summary>
        public string DictNo
        {
            get { return _dictNo; }
            set { _dictNo = value; }
        }

        /// <summary>
        /// 功能描述：字典名称
        /// 长度：50
        /// 不能为空：否
        /// </summary>
        public string DictName
        {
            get { return _dictName; }
            set { _dictName = value; }
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
        /// 功能描述：字典类型
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string DictType
        {
            get { return _dictType; }
            set { _dictType = value; }
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
