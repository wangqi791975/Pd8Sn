using System;
using System.Collections.Generic;

namespace Com.Panduo.Service.Order.ShippingOption
{
    /// <summary>
    /// 配送方式(包含配送方式、配送方式多语种、配送方式到达国家天数)
    /// </summary>
    [Serializable]
    public class ShippingBaseInfo
    {
        /// <summary>
        /// Shipping
        /// </summary>
        public virtual Shipping Shipping { get; set; }

        /// <summary>
        /// 配送方式多语种
        /// </summary>
        public virtual IList<ShippingLanguage> ShippingLanguages { get; set; }

        /// <summary>
        /// 配送方式到达对应国家天数
        /// </summary>
        public virtual IList<ShippingDay> ShippingDay { get; set; }
    }
}
