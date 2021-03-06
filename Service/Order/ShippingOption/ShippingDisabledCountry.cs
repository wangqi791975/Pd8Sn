﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Order.ShippingOption
{
    /// <summary>
    /// 配送方式排除国家
    /// </summary>
    [Serializable]
    public class ShippingDisabledCountry
    {
        /// <summary>
        /// 唯一标识ID
        /// </summary>
        public virtual int CountryId { get; set; }

        /// <summary>
        /// 配送方式ID
        /// </summary>
        public virtual int ShippingId { get; set; }

        /// <summary>
        /// 国家二级简码
        /// </summary>
        public virtual string CountryIsoCode2 { get; set; }
    }
}
