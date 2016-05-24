
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Marketing
{
    [Class(Table = "t_marketing_coupon", Lazy = false, NameType = typeof(MarketingCouponPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class MarketingCouponPo
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
        /// 外键：t_Marketing表主键
        /// </summary>
        [Property(Column = "MarketingId")]
        public virtual int MarketingId
        {
            get;
            set;
        }
        /// <summary>
        /// 0：注册送Coupon、1：生日送
        /// </summary>
        [Property(Column = "RewardType")]
        public virtual int RewardType
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
    }
}

