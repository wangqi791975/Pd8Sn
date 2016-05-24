
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Help
{
    /// <summary>
    ///描述：多语种 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:39:03
    /// </summary>
    [Class(Table = "t_help_article_description", Lazy = false, NameType = typeof(HelpArticleDescriptionPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class HelpArticleDescriptionPo
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
        /// 文章ID
        /// </summary>
        [Property(Column = "article_id")]
        public virtual int ArticleId
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
        [Property(Column = "article_name")]
        public virtual string ArticleName
        {
            get;
            set;
        }
        /// <summary>
        /// 关键词
        /// </summary>
        [Property(Column = "keywords")]
        public virtual string Keywords
        {
            get;
            set;
        }
        /// <summary>
        /// 内容
        /// </summary>
        [Property(Column = "content")]
        public virtual string Content
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
        /// 状态(0：隐藏,1:显示)
        /// </summary>
        [Property(Column = "`status`")]
        public virtual bool Status
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
    }
}

