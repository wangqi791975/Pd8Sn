using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Marketing
{
    /// <summary>
    /// 业务活动奖励：订单折扣（客户下单的时候打折）、支付后送(送coupon或多倍积分)、评论送
    /// </summary>
    [Serializable]
    public class MarketingReward
    {
        
        /// <summary>
        /// 营销活动Id
        /// </summary>
        public virtual int MarketingId { get; set; }
        /// <summary>
        /// 奖励类型 单选：枚举[送Coupon、送积分、折扣]
        /// </summary>
        public virtual int RewardType { get; set; }
        /// <summary>
        /// Coupon编号 该活动送的送的Coupon编号
        /// </summary>
        public virtual string CouponCode { get; set; }
        /// <summary>
        /// 积分倍数
        /// </summary>
        public virtual decimal IntegralTime { get; set; }
        /// <summary>
        /// 折扣值
        /// </summary>
        public virtual decimal Discount { get; set; }

        #region 条件

        /// <summary>
        /// 产品金额：大于该金额就满足条件‎
        /// 如果该字段为0，则忽略该条件
        /// </summary>
        public virtual decimal Amount { get; set; }
        #endregion
    }
}
