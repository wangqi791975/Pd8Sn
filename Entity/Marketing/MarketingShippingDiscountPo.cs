
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Marketing
{
    /// <summary>
    ///描述：运费折扣活动 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-01-29 16:07:17
    /// </summary>
    [Class(Table = "t_marketing_shipping_discount", Lazy = false, NameType = typeof(MarketingShippingDiscountPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class MarketingShippingDiscountPo
    {
        /// <summary>
        /// Id
        /// </summary>
        [Id(1, Name = "Id", Column = "Id")]
        [Generator(2, Class = "native")]
        public virtual int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 外键：t_MarketingShipping表主键
        /// </summary>
        [Property(Column = "MarketingShippingId")]
        public virtual int Marketingshippingid
        {
            get;
            set;
        }
        /// <summary>
        /// 起始金额
        /// </summary>
        [Property(Column = "Amount")]
        public virtual decimal Amount
        {
            get;
            set;
        }
        /// <summary>
        /// 折扣值
        /// </summary>
        [Property(Column = "Discount")]
        public virtual decimal Discount
        {
            get;
            set;
        }
    }
}

