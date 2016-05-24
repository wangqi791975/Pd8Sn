using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Marketing.ShippingMarketing
{
    /// <summary>
    /// 运费活动奖励：‎运费折扣
    /// </summary>
    [Serializable]
    public class ShippingDiscount
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
        /// 起始金额‎
        /// </summary>
        public virtual decimal Amount { get; set; }

        /// <summary>
        /// 运费折扣值
        /// </summary>
        public virtual decimal Discount { get; set; }
    }
}
