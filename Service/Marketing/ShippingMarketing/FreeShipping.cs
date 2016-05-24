using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Marketing.ShippingMarketing
{
    /// <summary>
    /// 运费活动奖励：‎freeshipping(免运费)
    /// </summary>
    [Serializable]
    public class FreeShipping
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 外键 t_MarketingShipping表主键
        /// </summary>
        public virtual int Marketingshippingid { get; set; }

        /// <summary>
        /// 产品金额：大于该金额就满足条件‎
        /// 如果该字段为0，则忽略该条件
        /// </summary>
        public virtual decimal Amount { get; set; }

        /// <summary>
        /// 基准运送方式Id
        /// </summary>
        public virtual int Baseshippingid { get; set; }

        /// <summary>
        /// FreeShipping手续费
        /// </summary>
        public virtual decimal FreeShippingFee { get; set; }
    }
}
