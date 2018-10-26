using System;
namespace project.Entity.Sys
{
    /// <summary>部门信息表</summary>
    /// <author>tianz</author>
    /// <date>2016-02-18</date>
    [System.Serializable]
    public class EntityDept
    {
        private string _DeptNo;
        private string _DeptName;
        private string _Parent;
        private string _Manager;
        private string _Remark;
        private int _Level;
        private string _AccID;
        
        /// <summary>缺省构造函数</summary>
        public EntityDept() { }

        /// <summary>部门编号</summary>
        public string DeptNo
        {
            get { return _DeptNo; }
            set { _DeptNo = value; }
        }

        /// <summary>
        /// 功能描述：部门名称
        /// 长度：50
        /// 不能为空：否
        /// </summary>
        public string DeptName
        {
            get { return _DeptName; }
            set { _DeptName = value; }
        }

        /// <summary>
        /// 功能描述：上级部门
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string Parent
        {
            get { return _Parent; }
            set { _Parent = value; }
        }

        /// <summary>
        /// 功能描述：上级部门
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string ParentName
        {
            get {
                string _ParentName = "";
                try
                {
                    if (_Parent != "")
                    {
                        Business.Sys.BusinessDept dept = new Business.Sys.BusinessDept();
                        dept.load(_Parent, _AccID);
                        _ParentName = dept.Entity.DeptName;
                    }
                }
                catch { }
                return _ParentName; 
            }
        }

        /// <summary>
        /// 功能描述：部门主管
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string Manager
        {
            get { return _Manager; }
            set { _Manager = value; }
        }

        /// <summary>
        /// 功能描述：部门主管
        /// 长度：30
        /// 不能为空：否
        /// </summary>
        public string ManagerName
        {
            get
            {
                string _ManagerName = "";
                try
                {
                    if (_Manager != "")
                    {
                        Business.Sys.BusinessUserInfo user = new Business.Sys.BusinessUserInfo();
                        user.loadUserNo(_Manager, _AccID);
                        _ManagerName = user.Entity.UserName;
                    }
                }
                catch { }
                return _ManagerName;
            }
        }

        /// <summary>
        /// 功能描述：备注
        /// 长度：200
        /// 不能为空：否
        /// </summary>
        public string Remark
        {
            get { return _Remark; }
            set { _Remark = value; }
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
