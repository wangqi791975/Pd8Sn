
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.AdminUser
{
    /// <summary>
    ///描述：管理员表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-02-05 14:57:32
    /// </summary>
    [Class(Table = "t_admin_module", Lazy = false, NameType = typeof(AdminModulePo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class AdminModulePo
    {
        /// <summary>
        /// 模块编号
        /// </summary>
        [Id(1, Name = "ModuleCode", Column = "module_code")]
        public virtual string ModuleCode
        {
            get;
            set;
        }
        /// <summary>
        /// 菜单编号
        /// </summary>
        [Property(Column = "menu_code")]
        public virtual string MenuCode
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
        /// 控制器
        /// </summary>
        [Property(Column = "controller")]
        public virtual string Controller
        {
            get;
            set;
        }
        /// <summary>
        /// action
        /// </summary>
        [Property(Column = "action")]
        public virtual string Action
        {
            get;
            set;
        }
        /// <summary>
        /// 排序
        /// </summary>
        [Property(Column = "sort")]
        public virtual string Sort
        {
            get;
            set;
        }
    }
}

