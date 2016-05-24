
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Marketing
{
    /// <summary>
    ///描述：运费活动 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-01-29 16:07:16
    /// </summary>
    [Class(Table = "t_marketing_shipping", Lazy = false, NameType = typeof(MarketingShippingPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class MarketingShippingPo
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
        /// 活动Id：t_Marketing表主键
        /// </summary>
        [Property(Column = "MarketingId")]
        public virtual int Marketingid
        {
            get;
            set;
        }
        /// <summary>
        /// 存运送方式ID字符串，用|分隔。 为空表示所有
        /// </summary>
        [Property(Column = "ShippingIds")]
        public virtual string Shippingids
        {
            get;
            set;
        }
        /// <summary>
        /// default 0：折扣、1：运送方式升级 、2：freeshipping
        /// </summary>
        [Property(Column = "RewardType")]
        public virtual int Rewardtype
        {
            get;
            set;
        }
        /// <summary>
        /// 重量类型
        /// </summary>
        [Property(Column = "WeightType")]
        public virtual int WeightType
        {
            get;
            set;
        }
        /// <summary>
        /// 重量（克）超出该重量部分就乘freeShipping手续费‎
        /// </summary>
        [Property(Column = "WeightLimit")]
        public virtual decimal WeightLimit
        {
            get;
            set;
        }
    }
}

