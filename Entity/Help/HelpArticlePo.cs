
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Help
{
    /// <summary>
    ///描述：网站帮助中心帮助文章表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:39:02
    /// </summary>
    [Class(Table = "t_help_article", Lazy = false, NameType = typeof(HelpArticlePo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class HelpArticlePo
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
        /// 类别ID
        /// </summary>
        [Property(Column = "help_category_id")]
        public virtual int HelpCategoryId
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
        /// 标题
        /// </summary>
        [Property(Column = "title")]
        public virtual string Title
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
        /// 状态(0:已废弃,1:启用)
        /// </summary>
        [Property(Column = "`status`")]
        public virtual bool Status
        {
            get;
            set;
        }
        /// <summary>
        /// 是否推荐
        /// </summary>
        [Property(Column = "is_recommend")]
        public virtual bool IsRecommend
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
        /// 有用次数
        /// </summary>
        [Property(Column = "useful_number")]
        public virtual int UsefulNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 无用次数
        /// </summary>
        [Property(Column = "unuseful_number")]
        public virtual int UnusefulNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 浏览次数
        /// </summary>
        [Property(Column = "browse_number")]
        public virtual int BrowseNumber
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
    }
}

