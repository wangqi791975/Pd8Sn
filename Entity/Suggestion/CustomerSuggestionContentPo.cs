using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Suggestion
{
    /// <summary>
    ///描述：客户评分内容表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:38:51
    /// </summary>
    [Class(Table = "t_customer_suggestion_content", Lazy = false, NameType = typeof(CustomerSuggestionContentPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class CustomerSuggestionContentPo
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        [Id(1, Name = "DetailId", Column = "detail_id")]
        [Generator(2, Class = "native")]
        public virtual int DetailId
        {
            get;
            set;
        }
        /// <summary>
        /// 语言Id
        /// </summary>
        [Property(Column = "language_id")]
        public virtual int LanguageId
        {
            get;
            set;
        }
        /// <summary>
        /// 全名
        /// </summary>
        [Property(Column = "full_name")]
        public virtual string FullName
        {
            get;
            set;
        }
        /// <summary>
        /// 邮箱
        /// </summary>
        [Property(Column = "email")]
        public virtual string Email
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
        /// 答复时间
        /// </summary>
        [Property(Column = "reply_date")]
        public virtual DateTime ReplyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 答复内容
        /// </summary>
        [Property(Column = "reply_content")]
        public virtual string ReplyContent
        {
            get;
            set;
        }
    }
}

