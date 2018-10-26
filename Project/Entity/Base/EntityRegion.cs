using System;
namespace project.Entity.Base
{
    /// <summary>地区信息表</summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    [System.Serializable]
    public class EntityRegion
    {
        private string _RegionNo;
        private string _RegionName;
        private string _Parent;
        private int _Level;
        private string _AccID;

        /// <summary>缺省构造函数</summary>
        public EntityRegion() { }

        /// <summary>地区编号</summary>
        public string RegionNo
        {
            get { return _RegionNo; }
            set { _RegionNo = value; }
        }

        /// <summary>
        /// 功能描述：地区名称
        /// 长度：50
        /// 不能为空：否
        /// </summary>
        public string RegionName
        {
            get { return _RegionName; }
            set { _RegionName = value; }
        }

        /// <summary>
        /// 功能描述：上级地区
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string Parent
        {
            get { return _Parent; }
            set { _Parent = value; }
        }

        /// <summary>
        /// 功能描述：上级地区
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string ParentName
        {
            get
            {
                string _ParentName = "";
                try
                {
                    if (_Parent != "")
                    {
                        Business.Base.BusinessRegion Region = new Business.Base.BusinessRegion();
                        Region.load(_Parent, _AccID);
                        _ParentName = Region.Entity.RegionName;
                    }
                }
                catch { }
                return _ParentName;
            }
        }

        /// <summary>
        /// 功能描述：层级
        /// 不能为空：否
        /// </summary>
        public int Level
        {
            get { return _Level; }
            set { _Level = value; }
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
    }
}
