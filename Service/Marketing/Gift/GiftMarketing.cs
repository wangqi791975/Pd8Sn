using System;

namespace Com.Panduo.Service.Marketing.Gift
{
    /// <summary>
    /// 送礼活动
    /// </summary>
    [Serializable]
    public class GiftMarketing : Marketing
    {
        /// <summary>
        /// GiftMarketingId
        /// </summary>
        public virtual int GiftMarketingId { get; set; }

        /// <summary>
        /// 外键：t_Marketing表主键
        /// </summary>
        public virtual int MarketingId { get; set; }

        /// <summary>
        /// 场景
        /// </summary>
        public virtual int RewardType { get; set; }

        /// <summary>
        /// 礼物等级
        /// </summary>
        public virtual string GiftLevel { get; set; }
    }

    /// <summary>
    /// 0：注册送礼
    /// </summary>
    public enum GiftMarketingRewardType
    {
        /// <summary>
        /// 注册
        /// </summary>
        Register = 1
    }
}

