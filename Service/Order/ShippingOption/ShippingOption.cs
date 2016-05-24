using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Service.Marketing;

namespace Com.Panduo.Service.Order.ShippingOption
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ShippingOption
    {

        /// <summary>
        /// 运送方式对象
        /// </summary>
        public virtual Shipping Shipping { get; set; }

        /*/// <summary>
        /// 运费折扣：营销活动设置的运费折扣
        /// </summary>
        public virtual decimal ShippingCostDiscount { get; set; }*/

        /// <summary>
        /// 基础运费
        /// </summary>
        public virtual decimal BaseShippingCost { get; set; }

        /// <summary>
        /// 偏远费
        /// </summary>
        public virtual decimal FarawayCost { get; set; }

        /// <summary>
        /// Club手续费
        /// </summary>
        public virtual decimal ClubFee { get; set; }

        /// <summary>
        /// Club运费差额
        /// </summary>
        public virtual decimal ClubDiffAmt { get; set; }
        
        /// <summary>
        /// 手续费：FreeShipping手续费等
        /// </summary>
        public virtual decimal FreeShippingFee { get; set; }

        /// <summary>
        /// FreeShipping差额
        /// </summary>
        public virtual decimal FreeShippingDiffAmt { get; set; }

        /// <summary>
        /// 总运费:计算列[基础运费+偏远费+FreeShipping差额+Club运费差额]
        /// </summary>
        public virtual decimal TotalShippingCost { get; set; }
    }
}
