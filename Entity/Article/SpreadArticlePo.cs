
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Article
{
    /// <summary>
    ///描述：t_spread_article ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-04-13 11:34:05
    /// </summary>
    [Class(Table = "t_spread_article", Lazy = false, NameType = typeof(SpreadArticlePo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class SpreadArticlePo
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        [Id(1, Name = "ArticleId", Column = "article_id")]
        [Generator(2, Class = "native")]
        public virtual int ArticleId
        {
            get;
            set;
        }
        /// <summary>
        /// 中文名称
        /// </summary>
        [Property(Column = "chinese_title")]
        public virtual string ChineseTitle
        {
            get;
            set;
        }
        /// <summary>
        /// 英文名称(同时也用于生成URL)
        /// </summary>
        [Property(Column = "english_title")]
        public virtual string EnglishTitle
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
        /// 管理员ID
        /// </summary>
        [Property(Column = "admin_id")]
        public virtual int AdminId
        {
            get;
            set;
        }
    }
}

