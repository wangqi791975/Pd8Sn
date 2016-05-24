
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Marketing
{
    /// <summary>
    ///描述：下单送礼活动 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-01-29 16:07:14
    /// </summary>
    [Class(Table = "t_marketing_place_order", Lazy = false, NameType = typeof(MarketingPlaceOrderPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class MarketingPlaceOrderPo
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
        /// 外键活动Id：t_Marketing表主键
        /// </summary>
        [Property(Column = "MarketingId")]
        public virtual int MarketingId
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
        /// 送礼级数：与ERP设置的送礼级数对应。默认只提供最大为5级即:ERP中设置送礼级数，每一级都代表对于要送的礼品，物品编号在CRM或导单过程中解析
        /// </summary>
        [Property(Column = "GiftLevel")]
        public virtual string GiftLevel
        {
            get;
            set;
        }
        /// <summary>
        /// 送的Coupon编号
        /// </summary>
        [Property(Column = "CouponCode")]
        public virtual string CouponCode
        {
            get;
            set;
        }
        /// <summary>
        /// 币种Id
        /// </summary>
        [Property(Column = "CurrencyId")]
        public virtual int? CurrencyId
        {
            get;
            set;
        }
    }
}

