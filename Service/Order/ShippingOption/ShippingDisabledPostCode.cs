using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Order.ShippingOption
{
    /// <summary>
    /// 配送方式排除邮编
    /// </summary>
    [Serializable]
    public class ShippingDisabledPostCode
    {
        /// <summary>
        /// 唯一标识ID
        /// </summary>
        public virtual int PostCodeId { get; set; }

        /// <summary>
        /// 配送方式ID
        /// </summary>
        public virtual int ShippingId { get; set; }

        /// <summary>
        /// 国家二级简码
        /// </summary>
        public virtual string CountryIsoCode2 { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        public virtual string PostCode { get; set; }
    }
}
