
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Help
{
    /// <summary>
    ///描述：网站帮助中心分类表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:39:04
    /// </summary>
    [Class(Table = "v_help_category", Lazy = false, NameType = typeof(VHelpCategoryPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class VHelpCategoryPo
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        [Id(1, Name = "HelpCategoryId", Column = "help_category_id")]
        [Generator(2, Class = "native")]
        public virtual int HelpCategoryId
        {
            get;
            set;
        }
        /// <summary>
        /// 分类类型(0:Customer Care,10:FAQ)
        /// </summary>
        [Property(Column = "category_type")]
        public virtual int CategoryType
        {
            get;
            set;
        }
        /// <summary>
        /// 类别父ID
        /// </summary>
        [Property(Column = "parent_id")]
        public virtual int ParentId
        {
            get;
            set;
        }
        /// <summary>
        /// 分类逐级路径
        /// </summary>
        [Property(Column = "category_path")]
        public virtual string CategoryPath
        {
            get;
            set;
        }
        /// <summary>
        /// 是否显示在上一级
        /// </summary>
        [Property(Column = "is_show_parent")]
        public virtual bool IsShowParent
        {
            get;
            set;
        }
        /// <summary>
        /// 状态(0:已废弃,1:启用)
        /// </summary>
        [Property(Column = "`status`")]
        public virtual bool Status
        {
            get;
            set;
        }
        /// <summary>
        /// 排序
        /// </summary>
        [Property(Column = "sort_order")]
        public virtual int SortOrder
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
        /// 修改时间
        /// </summary>
        [Property(Column = "date_modified")]
        public virtual DateTime DateModified
        {
            get;
            set;
        }
        /// <summary>
        /// 语种ID
        /// </summary>
        [Property(Column = "language_id")]
        public virtual int LanguageId
        {
            get;
            set;
        }
        /// <summary>
        /// 名称
        /// </summary>
        [Property(Column = "category_name")]
        public virtual string CategoryName
        {
            get;
            set;
        }
        /// <summary>
        /// 名称
        /// </summary>
        [Property(Column = "en_category_name")]
        public virtual string EnCategoryName
        {
            get;
            set;
        }
    }
}

