using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Suggestion
{
    /// <summary>
    ///描述：客户评分项 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:38:54
    /// </summary>
    [Class(Table = "t_customer_suggestion_item", Lazy = false, NameType = typeof(CustomerSuggestionItemPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class CustomerSuggestionItemPo
    {
        /// <summary>
        /// 自增ID
        /// </summary>

        [Id(1, Name = "ItemId", Column = "item_id")]
        [Generator(2, Class = "native")]

        public virtual int ItemId
        {
            get;
            set;
        }
        /// <summary>
        /// 项目名称
        /// </summary>
        [Property(Column = "item_name")]
        public virtual string ItemName
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

