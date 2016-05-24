using System;
using System.Collections.Generic;

namespace Com.Panduo.Service.Marketing.Coupon
{
    /// <summary>
    /// 下单活动
    /// </summary>
    [Serializable]
    public class CouponMarketing : Marketing
    {
        public virtual int CouponMarketingId { get; set; }

        /// <summary>
        /// 1：注册送Coupon、2：生日送
        /// </summary>
        public virtual int RewardType { get; set; }

        /// <summary>
        /// Coupon编号
        /// </summary>
        public virtual string CouponCode { get; set; }
    }

    /// <summary>
    /// 1：注册送Coupon、2：生日送、3：Club每月送
    /// </summary>
    public enum CouponMarketingRewardType
    {
        /// <summary>
        /// 注册
        /// </summary>
        Register = 1,
        /// <summary>
        /// 生日
        /// </summary>
        Birthday = 2,
        /// <summary>
        /// Club每月coupon
        /// </summary>
        Club = 3
    }
}
