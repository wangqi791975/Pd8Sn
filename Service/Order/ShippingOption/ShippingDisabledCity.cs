using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Order.ShippingOption
{
    /// <summary>
    /// 配送方式排除城市
    /// </summary>
    [Serializable]
    public class ShippingDisabledCity
    {
        /// <summary>
        /// 唯一标识ID
        /// </summary>
        public virtual int CityId { get; set; }

        /// <summary>
        /// 配送方式ID
        /// </summary>
        public virtual int ShippingId { get; set; }

        /// <summary>
        /// 国家二级简码
        /// </summary>
        public virtual string CountryIsoCode2 { get; set; }

        /// <summary>
        /// 城市名称
        /// </summary>
        public virtual string City { get; set; }
    }
}
