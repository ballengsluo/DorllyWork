using System;
namespace project.Entity.Order
{
    /// <summary>工作操作日志信息</summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    [System.Serializable]
    public class EntityWorkOrderImages
    {
        private string _entityOID;
        private string _AccID;
        private string _OrderNo;
        private string _NodeNo;
        private string _Img;
        private DateTime _UploadDate;
        private string _UploadUser;

        /// <summary>缺省构造函数</summary>
        public EntityWorkOrderImages() { }

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
        /// 功能描述：工单编号
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string OrderNo
        {
            get { return _OrderNo; }
            set { _OrderNo = value; }
        }

        /// <summary>
        /// 功能描述：节点
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string NodeNo
        {
            get { return _NodeNo; }
            set { _NodeNo = value; }
        }

        /// <summary>
        /// 功能描述：图片
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string Img
        {
            get { return _Img; }
            set { _Img = value; }
        }

        /// <summary>
        /// 功能描述：上传日期
        /// 不能为空：否
        /// </summary>
        public DateTime UploadDate
        {
            get { return _UploadDate; }
            set { _UploadDate = value; }
        }

        /// <summary>
        /// 功能描述：上传用户
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string UploadUser
        {
            get { return _UploadUser; }
            set { _UploadUser = value; }
        }
    }
}
