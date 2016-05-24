using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.AdminUser
{
    /// <summary>
    /// 用户状态
    /// </summary>
    public enum AdminUserStatus
    {
        /// <summary>
        /// 启用
        /// </summary>
        Active = 1,
        /// <summary>
        /// 禁用
        /// </summary>
        Lock = 2
    }
    [Serializable]
    public class AdminUser
    {
        /// <summary>
        /// 自增id
        /// </summary> 
        public virtual int AdminId
        {
            get;
            set;
        }
        /// <summary>
        /// 名称
        /// </summary> 
        public virtual string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 账号
        /// </summary> 
        public virtual string AccountEmail
        {
            get;
            set;
        }
        /// <summary>
        /// 密码
        /// </summary> 
        public virtual string Password
        {
            get;
            set;
        }
        /// <summary>
        /// 角色id
        /// </summary> 
        public virtual int RoleId
        {
            get;
            set;
        }
        /// <summary>
        /// 是否可看见客户邮箱
        /// </summary>
        public virtual bool IsViewEmail
        {
            get;
            set;
        }
        /// <summary>
        /// 是否超级管理员
        /// </summary> 
        public virtual bool IsRoot
        {
            get;
            set;
        }
        /// <summary>
        /// 状态
        /// </summary> 
        public virtual AdminUserStatus AdminUserStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 创建时间
        /// </summary> 
        public virtual DateTime DateCreated
        {
            get;
            set;
        }
        /// <summary>
        /// 最后登录时间
        /// </summary> 
        public virtual DateTime? DateLastLogin
        {
            get;
            set;
        }
        /// <summary>
        /// 备注
        /// </summary> 
        public virtual string Remark
        {
            get;
            set;
        }

        /// <summary>
        /// 修改密码时间
        /// </summary>
        public virtual DateTime? UpdatePasswordTime
        {
            get;
            set;
        }
    }
}
