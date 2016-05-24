
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Marketing
{
    /// <summary>
    ///描述：订单折扣活动 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-01-29 16:07:13
    /// </summary>
    [Class(Table = "t_marketing_order_discount", Lazy = false, NameType = typeof(MarketingOrderDiscountPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class MarketingOrderDiscountPo
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
        /// 外键 活动Id：t_Marketing表主键
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

