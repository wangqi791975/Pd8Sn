using System;
using System.Collections.Generic;

namespace Com.Panduo.Service.Marketing.ShippingMarketing
{
    /// <summary>
    /// 运费活动奖励：运送方式升级
    /// </summary>
    [Serializable]
    public class ShippingUpgrade
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 外键：t_MarketingShipping表主键
        /// </summary>
        public virtual int Marketingshippingid { get; set; }

        /// <summary>
        /// 起始金额
        /// </summary>
        public virtual decimal Amount { get; set; }

        /// <summary>
        /// 原运送方式Id
        /// </summary>
        public virtual int ShippingId { get; set; }

        /// <summary>
        /// 可升级的运送方式
        /// </summary>
        public virtual int Upshippingid { get; set; }
    }
}
