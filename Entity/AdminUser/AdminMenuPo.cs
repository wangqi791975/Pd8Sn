
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.AdminUser
{
    /// <summary>
    ///描述：管理员表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-02-05 14:57:32
    /// </summary>
    [Class(Table = "t_admin_menu", Lazy = false, NameType = typeof(AdminMenuPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class AdminMenuPo
    {
        /// <summary>
        /// 菜单编号
        /// </summary>
        [Id(1, Name = "Code", Column = "code")]
        public virtual string Code
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
    }
}

