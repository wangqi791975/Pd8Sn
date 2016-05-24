using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Customer.Club
{
    /// <summary>
    /// Club会费信息
    /// </summary>
    [Serializable]
    public class ClubShippingFee
    {
        /// <summary>
        /// 非club的运费
        /// </summary>
        public virtual decimal ShippingFeeBefore { get; set; }

        /// <summary>
        /// club的运费
        /// </summary>
        public virtual decimal ShippingFeeAfter { get; set; }

        /// <summary>
        /// club专属优惠券
        /// </summary>
        public virtual decimal ExclusiveCoupon { get; set; }

        /// <summary>
        /// 手续费
        /// </summary>
        public virtual decimal HandlingFee { get; set; }

        /// <summary>
        /// 非club实际花费
        /// </summary>
        public virtual decimal ActuallySpentBefore { get { return ShippingFeeBefore; } }

        /// <summary>
        /// club实际花费
        /// </summary>
        public virtual decimal ActuallySpentAfter { get { return ShippingFeeAfter - ExclusiveCoupon; } }

        /// <summary>
        /// club实际节省
        /// </summary>
        public virtual decimal ActuallySave { get { return ActuallySpentBefore - ActuallySpentAfter - HandlingFee; } }
    }
}
