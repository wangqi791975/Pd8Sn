
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.AdminUser
{
    /// <summary>
    ///描述：t_admin_role ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-02-05 14:57:33
    /// </summary>
    [Class(Table = "t_admin_role", Lazy = false, NameType = typeof(AdminRolePo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class AdminRolePo
    {
        /// <summary>
        /// 自增id
        /// </summary>
        [Id(1, Name = "RoleId", Column = "role_id")]
        [Generator(2, Class = "native")]
        public virtual int RoleId
        {
            get;
            set;
        }
        /// <summary>
        /// 角色名称
        /// </summary>
        [Property(Column = "role_name")]
        public virtual string RoleName
        {
            get;
            set;
        }
        /// <summary>
        /// 状态, 1有效 0无效
        /// </summary>
        [Property(Column = "role_status")]
        public virtual bool RoleStatus
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
    }
}

