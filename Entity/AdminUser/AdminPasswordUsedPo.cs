using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.AdminUser
{

    [Class(Table = "t_admin_password_used", Lazy = false, NameType = typeof(AdminPasswordUsedPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class AdminPasswordUsedPo
    {
        /// <summary>
        /// 自增id
        /// </summary>
        [Id(1, Name = "Id", Column = "id")]
        [Generator(2, Class = "native")]
        public virtual int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 用户名Id
        /// </summary>
        [Property(Column = "admin_id")]
        public virtual int AdminId
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
        /// 修改密码时间
        /// </summary>
        [Property(Column = "date_created")]
        public virtual DateTime DateCreated
        {
            get;
            set;
        }
    }

}