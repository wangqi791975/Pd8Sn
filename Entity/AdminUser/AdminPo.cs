
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.AdminUser
{
    /// <summary>
    ///描述：管理员表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-02-05 14:57:32
    /// </summary>
    [Class(Table = "t_admin", Lazy = false, NameType = typeof(AdminPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class AdminPo
    {
        /// <summary>
        /// 自增id
        /// </summary>
        [Id(1, Name = "AdminId", Column = "admin_id")]
        [Generator(2, Class = "native")]
        public virtual int AdminId
        {
            get;
            set;
        }
        /// <summary>
        /// 名称
        /// </summary>
        [Property(Column = "name")]
        public virtual string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 账号
        /// </summary>
        [Property(Column = "account_email")]
        public virtual string AccountEmail
        {
            get;
            set;
        }
        /// <summary>
        /// 密码
        /// </summary>
        [Property(Column = "password")]
        public virtual string Password
        {
            get;
            set;
        }
        /// <summary>
        /// 角色id
        /// </summary>
        [Property(Column = "role_id")]
        public virtual int RoleId
        {
            get;
            set;
        }
        /// <summary>
        /// 是否可看见客户邮箱
        /// </summary>
        [Property(Column = "is_view_email")]
        public virtual bool IsViewEmail
        {
            get;
            set;
        }
        /// <summary>
        /// 是否超级管理员
        /// </summary>
        [Property(Column = "is_root")]
        public virtual bool IsRoot
        {
            get;
            set;
        }
        /// <summary>
        /// 状态
        /// </summary>
        [Property(Column = "status")]
        public virtual int Status
        {
            get;
            set;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Property(Column = "date_created")]
        public virtual DateTime DateCreated
        {
            get;
            set;
        }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        [Property(Column = "date_last_login")]
        public virtual DateTime? DateLastLogin
        {
            get;
            set;
        }
        /// <summary>
        /// 备注
        /// </summary>
        [Property(Column = "remark")]
        public virtual string Remark
        {
            get;
            set;
        }

        /// <summary>
        /// 修改密码时间
        /// </summary>
        [Property(Column = "update_password_time")]
        public virtual DateTime? UpdatePasswordTime
        {
            get;
            set;
        }
    }
}

