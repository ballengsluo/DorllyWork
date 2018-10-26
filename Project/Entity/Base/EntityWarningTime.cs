using System;
namespace project.Entity.Base
{
    /// <summary>预警时间设置</summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    [System.Serializable]
    public class EntityWarningTime
    {
        private string _entityOID;
        private string _AccID;
        private string _ParaNo;
        private string _ParaName;
        private int _Time;
        
        /// <summary>缺省构造函数</summary>
        public EntityWarningTime() { }

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
        /// 功能描述：节点编号
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string ParaNo
        {
            get { return _ParaNo; }
            set { _ParaNo = value; }
        }

        /// <summary>
        /// 功能描述：节点名称
        /// 长度：50
        /// 不能为空：否
        /// </summary>
        public string ParaName
        {
            get { return _ParaName; }
            set { _ParaName = value; }
        }

        /// <summary>
        /// 功能描述：预警时长（分）
        /// 不能为空：否
        /// </summary>
        public int Time
        {
            get { return _Time; }
            set { _Time = value; }
        }
    }
}
