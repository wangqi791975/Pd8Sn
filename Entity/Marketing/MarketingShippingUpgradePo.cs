
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Marketing
{
    /// <summary>
    ///描述：运送方式升级活动 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-01-29 16:07:19
    /// </summary>
    [Class(Table = "t_marketing_shipping_upgrade", Lazy = false, NameType = typeof(MarketingShippingUpgradePo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class MarketingShippingUpgradePo
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
        /// MarketingShippingId
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
        /// 原运送方式Id
        /// </summary>
        [Property(Column = "ShippingId")]
        public virtual int Shippingid
        {
            get;
            set;
        }
        /// <summary>
        /// 可升级的运送方式
        /// </summary>
        [Property(Column = "UpShippingId")]
        public virtual int Upshippingid
        {
            get;
            set;
        }
    }
}

