using System;
namespace project.Entity.Sys
{
    /// <summary>用户信息</summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    [System.Serializable]
    public class EntityUserInfo
    {
        private string _entityOID;
        private string _userType;
        private string _accID;
        private string _userNo;
        private string _nickName;
        private string _userName;
        private string _password;
        private string _tel;
        private string _email;
        private string _addr;
        private System.DateTime _regDate;
        private bool _valid;
        private string _deptNo;
        private string _manager;
        private string _picture;

        /// <summary>缺省构造函数</summary>
        public EntityUserInfo() {}
        
        /// <summary>内部映射主键</summary>
        public string InnerEntityOID
        {
            get { return _entityOID; }
            set { _entityOID = value; }
        }

        /// <summary>
        /// 功能描述：用户类型
        /// 长度：20
        /// 不能为空：否
        /// </summary>
        public string UserType
        {
            get { return _userType; }
            set { _userType = value; }
        }

        /// <summary>
        /// 功能描述：用户类别名称
        /// 长度：100
        /// 不能为空：否
        /// </summary>
        public string UserTypeName
        {
            get
            {
                if (_userType != "" && _userType != null)
                {
                    try
                    {
                        Business.Sys.BusinessUserType usertype = new Business.Sys.BusinessUserType();
                        usertype.load(_userType, _accID);
                        return usertype.Entity.UserTypeName;
                    }
                    catch { return ""; }
                }
                return "";
            }
        }

        /// <summary>
        /// 功能描述：账套ID
        /// 长度：20
        /// 不能为空：否
        /// </summary>
        public string AccID
        {
            get { return _accID; }
            set { _accID = value; }
        }

        /// <summary>
        /// 功能描述：用户编号
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string UserNo
        {
            get { return _userNo; }
            set { _userNo = value; }
        }

        /// <summary>
        /// 功能描述：昵称
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string NickName
        {
            get { return _nickName; }
            set { _nickName = value; }
        }

        /// <summary>
        /// 功能描述：用户名称
        /// 长度：50
        /// 不能为空：否
        /// </summary>
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        /// <summary>
        /// 功能描述：密码
        /// 长度：50
        /// 不能为空：否
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        /// <summary>
        /// 功能描述：电话
        /// 长度：50
        /// 不能为空：否
        /// </summary>
        public string Tel
        {
            get { return _tel; }
            set { _tel = value; }
        }

        /// <summary>
        /// 功能描述：邮箱
        /// 长度：50
        /// 不能为空：否
        /// </summary>
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        /// <summary>
        /// 功能描述：地址
        /// 长度：200
        /// 不能为空：否
        /// </summary>
        public string Addr
        {
            get { return _addr; }
            set { _addr = value; }
        }

        /// <summary>
        /// 功能描述：注册日期
        /// 不能为空：否
        /// </summary>
        public System.DateTime RegDate
        {
            get { return _regDate; }
            set { _regDate = value; }
        }

        /// <summary>
        /// 功能描述：是否有效
        /// 不能为空：否
        /// </summary>
        public bool Valid
        {
            get { return _valid; }
            set { _valid = value; }
        }

        /// <summary>
        /// 功能描述：部门编号
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string DeptNo
        {
            get { return _deptNo; }
            set { _deptNo = value; }
        }

        /// <summary>
        /// 功能描述：部门名称
        /// 长度：50
        /// 不能为空：否
        /// </summary>
        public string DeptName
        {
            get
            {
                if (_deptNo != "" && _deptNo != null)
                {
                    try
                    {
                        Business.Sys.BusinessDept dept = new Business.Sys.BusinessDept();
                        dept.load(_deptNo, _accID);
                        return dept.Entity.DeptName;
                    }
                    catch { return ""; }
                }
                return "";
            }
        }

        /// <summary>
        /// 功能描述：上级经理
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string Manager
        {
            get { return _manager; }
            set { _manager = value; }
        }

        /// <summary>
        /// 功能描述：部门名称
        /// 长度：50
        /// 不能为空：否
        /// </summary>
        public string ManagerName
        {
            get
            {
                if (_manager != "" && _manager != null)
                {
                    try
                    {
                        Business.Sys.BusinessUserInfo user = new Business.Sys.BusinessUserInfo();
                        user.loadUserNo(_manager, _accID);
                        return user.Entity.UserName;
                    }
                    catch { return ""; }
                }
                return "";
            }
        }

        /// <summary>
        /// 功能描述：照片
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string Picture
        {
            get { return _picture; }
            set { _picture = value; }
        }
    }
}