using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Order.ShippingOption
{
    /// <summary>
    /// 偏远城市
    /// </summary>
    [Serializable]
    public class RemoteCity
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 配送方式编码
        /// </summary>
        public virtual string ShippingCode { get; set; }

        /// <summary>
        /// 国家编码
        /// </summary>
        public virtual string CountryCode { get; set; }

        /// <summary>
        /// 国家英文名称
        /// </summary>
        public virtual string CountryEnglishName { get; set; }

        /// <summary>
        /// 城市名称
        /// </summary>
        public virtual string City { get; set; }
        
    }
}
