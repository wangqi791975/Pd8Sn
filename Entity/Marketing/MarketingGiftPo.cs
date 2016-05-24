using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Marketing
{
    [Class(Table = "t_marketing_gift", Lazy = false, NameType = typeof(MarketingGiftPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class MarketingGiftPo
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
        /// 场景
        /// </summary>
        [Property(Column = "RewardType")]
        public virtual int RewardType
        {
            get;
            set;
        }
        /// <summary>
        /// 礼物等级
        /// </summary>
        [Property(Column = "GiftLevel")]
        public virtual string GiftLevel
        {
            get;
            set;
        }
    }
}

