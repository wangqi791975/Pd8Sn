using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Order.ShippingOption
{
    /// <summary>
    /// 配送方式天数
    /// </summary>
    [Serializable]
    public class ShippingDay
    {
        /// <summary>
        /// 唯一标识ID
        /// </summary>
        public virtual int ShippingDayId { get; set; }

        /// <summary>
        /// 配送方式ID
        /// </summary>
        public virtual int ShippingId { get; set; }

        /// <summary>
        /// 国家二级简码
        /// </summary>
        public virtual string CountryIsoCode2 { get; set; }

        /// <summary>
        /// 最快到达开数
        /// </summary>
        public virtual int DayLow { get; set; }

        /// <summary>
        /// 最长到达天数
        /// </summary>
        public virtual int DayHigh { get; set; }
    }
}
