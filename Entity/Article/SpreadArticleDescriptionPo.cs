
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Article
{
    /// <summary>
    ///描述：t_spread_article_description ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-04-13 11:34:06
    /// </summary>
    [Class(Table = "t_spread_article_description", Lazy = false, NameType = typeof(SpreadArticleDescriptionPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class SpreadArticleDescriptionPo
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
        /// 关联文章ID
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
        /// 标题
        /// </summary>
        [Property(Column = "title")]
        public virtual string Title
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
        /// 0:显示，1:隐藏
        /// </summary>
        [Property(Column = "status")]
        public virtual bool Status
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

