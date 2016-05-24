
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Suggestion
{
    /// <summary>
    ///描述：客户评分明细 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:38:52
    /// </summary>
    [Class(Table = "t_customer_suggestion_detail", Lazy = false, NameType = typeof(CustomerSuggestionDetailPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class CustomerSuggestionDetailPo
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        [Id(1, Name = "Id", Column = "id")]
        [Generator(2, Class = "native")]
        public virtual int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 评分项ID
        /// </summary>
        [Property(Column = "object_id")]
        public virtual int ObjectId
        {
            get;
            set;
        }
        /// <summary>
        /// 评分内容ID
        /// </summary>
        [Property(Column = "detail_id")]
        public virtual int DetailId
        {
            get;
            set;
        }
        /// <summary>
        /// 分值
        /// </summary>
        [Property(Column = "number")]
        public virtual int Number
        {
            get;
            set;
        }
    }
}

