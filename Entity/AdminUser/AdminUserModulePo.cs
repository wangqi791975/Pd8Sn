
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.AdminUser
{
    /// <summary>
    ///描述：管理员表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-02-05 14:57:32
    /// </summary>
    [Class(Table = "t_admin_user_module", Lazy = false, NameType = typeof(AdminUserModulePo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class AdminUserModulePo
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
        /// 后台用户Id
        /// </summary>
        [Property(Column = "admin_id")]
        public virtual int AdminId
        {
            get;
            set;
        }
        /// <summary>
        /// 模块编号
        /// </summary>
        [Property(Column = "module_code")]
        public virtual string ModuleCode
        {
            get;
            set;
        }
    }
}

