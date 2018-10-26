using System;
namespace project.Entity.Base
{
    /// <summary>意见反馈</summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    [System.Serializable]
    public class EntityOpinion
    {
        private string _rowPointer;
        private string _accID;
        private string _filesName;
        private string _content;
        private string _createUser;
        private DateTime _createDate;
        private bool _isSolved;
        private string _solveUser;
        private DateTime _solveDate;
        
        /// <summary>缺省构造函数</summary>
        public EntityOpinion() { }

        /// <summary>内部映射主键</summary>
        public string RowPointer
        {
            get { return _rowPointer; }
            set { _rowPointer = value; }
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
        /// 功能描述：文件名称
        /// 长度：300
        /// 不能为空：否
        /// </summary>
        public string FilesName
        {
            get { return _filesName; }
            set { _filesName = value; }
        }

        /// <summary>
        /// 功能描述：内容
        /// 长度：300
        /// 不能为空：否
        /// </summary>
        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        /// <summary>
        /// 功能描述：创建人
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string CreateUser
        {
            get { return _createUser; }
            set { _createUser = value; }
        }

        /// <summary>
        /// 功能描述：创建日期
        /// </summary>
        public DateTime CreateDate
        {
            get { return _createDate; }
            set { _createDate = value; }
        }

        /// <summary>
        /// 功能描述：是否处理
        /// </summary>
        public bool IsSolved
        {
            get { return _isSolved; }
            set { _isSolved = value; }
        }
        
        /// <summary>
        /// 功能描述：处理人
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string SolveUser
        {
            get { return _solveUser; }
            set { _solveUser = value; }
        }

        /// <summary>
        /// 功能描述：处理日期
        /// </summary>
        public DateTime SolveDate
        {
            get { return _solveDate; }
            set { _solveDate = value; }
        }
    }
}
