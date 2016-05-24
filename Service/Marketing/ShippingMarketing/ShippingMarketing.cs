using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Marketing.ShippingMarketing
{
    /// <summary>
    /// 运费活动奖励：‎运费折扣、运送方式升级、freeshipping(免运费)
    /// </summary>
    [Serializable]
    public class ShippingMarketing : Marketing
    {
        /// <summary>
        /// 营销活动Id => Marketing.Id
        /// </summary>
        //public virtual int MarketingId { get; set; }

        /// <summary>
        /// 运费活动Id
        /// </summary>
        public virtual int ShippingMarketingId { get; set; }

        /// <summary>
        /// 运费活动类型 单选：枚举
        /// </summary>
        public virtual ShippingRewardType RewardType { get; set; }

        /// <summary>
        /// 运送方式：单独针对运费折扣
        /// </summary>
        public virtual List<int> ShippingIds { get; set; }

        /// <summary>
        /// 重量（克）超出该重量部分就乘freeShipping手续费‎
        /// </summary>
        public virtual decimal WeightLimit { get; set; }

        /// <summary>
        /// 重量类型
        /// </summary>
        public virtual ShippingWeightType WeightType { get; set; }

        #region 冗余
        public virtual FreeShipping FreeShipping { get; set; }
        public virtual List<ShippingDiscount> ShippingDiscounts { get; set; }
        public virtual List<ShippingUpgrade> ShippingUpgrades { get; set; }
        #endregion
    }

    /// <summary>
    /// default 0：运费折扣、1：运送方式升级、2：免运费
    /// </summary>
    public enum ShippingRewardType
    {
        /// <summary>
        /// 运费折扣
        /// </summary>
        ShippingDiscount = 0,
        /// <summary>
        /// 运送方式升级
        /// </summary>
        ShippingUpgrade = 1,
        /// <summary>
        /// 免运费
        /// </summary>
        FreeShipping = 2
    }

    /// <summary>
    /// default 0：运费折扣、1：运送方式升级、2：免运费
    /// </summary>
    public enum ShippingWeightType
    {
        ShippingWeight,
        GrossWeight,
        VolumeWeight
    }
}
