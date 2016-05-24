
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Help
{
    /// <summary>
    ///描述：多语种 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:39:06
    /// </summary>
    [Class(Table = "t_help_category_description", Lazy = false, NameType = typeof(HelpCategoryDescriptionPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class HelpCategoryDescriptionPo
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        [Id(1, Name = "DescriptionId", Column = "description_id")]
        [Generator(2, Class = "native")]
        public virtual int DescriptionId
        {
            get;
            set;
        }
        /// <summary>
        /// 分类ID
        /// </summary>
        [Property(Column = "help_category_id")]
        public virtual int HelpCategoryId
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
        /// 是否显示
        /// </summary>
        [Property(Column = "`status`")]
        public virtual bool Status
        {
            get;
            set;
        }
    }
}

