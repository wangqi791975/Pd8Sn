
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Marketing
{
    /// <summary>
    ///描述：免运费活动 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-01-29 16:07:12
    /// </summary>
    [Class(Table = "t_marketing_free_shipping", Lazy = false, NameType = typeof(MarketingFreeShippingPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class MarketingFreeShippingPo
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
        /// 外键 t_marketing_shipping表主键
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
        /// 基准运送方式Id
        /// </summary>
        [Property(Column = "BaseShippingId")]
        public virtual int Baseshippingid
        {
            get;
            set;
        }
        /// <summary>
        /// freeShipping手续费 USD：多少钱1kg
        /// </summary>
        [Property(Column = "freeShippingFee")]
        public virtual decimal Freeshippingfee
        {
            get;
            set;
        }
    }
}

