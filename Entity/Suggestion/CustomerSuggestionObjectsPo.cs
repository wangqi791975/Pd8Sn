using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Suggestion
{
    /// <summary>
    ///描述：客户评分对象表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:38:55
    /// </summary>
    [Class(Table = "t_customer_suggestion_objects", Lazy = false, NameType = typeof(CustomerSuggestionObjectsPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class CustomerSuggestionObjectsPo
    {
        /// <summary>
        /// 自增ID
        /// </summary>

        [Id(1, Name = "ObjectId", Column = "object_id")]
        [Generator(2, Class = "native")]

        public virtual int ObjectId
        {
            get;
            set;
        }
        /// <summary>
        /// 评分项ID
        /// </summary>
        [Property(Column = "item_id")]
        public virtual int ItemId
        {
            get;
            set;
        }
        /// <summary>
        /// 名称
        /// </summary>
        [Property(Column = "title")]
        public virtual string Title
        {
            get;
            set;
        }
        /// <summary>
        /// 满分数值
        /// </summary>
        [Property(Column = "full_number")]
        public virtual int FullNumber
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
        /// 添加时间
        /// </summary>
        [Property(Column = "date_created")]
        public virtual DateTime DateCreated
        {
            get;
            set;
        }
    }
}

