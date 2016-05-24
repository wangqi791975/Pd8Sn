using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.SiteConfigure.Payment
{
    [Serializable]
    public class QiwiConfig
    {
         /// <summary>
        /// 商户ID，如：7983
        /// </summary>
        public virtual string Merchantid { get; set; }
        /// <summary>
        /// 商户参考信息，如：285197
        /// </summary>
        public virtual string MerchantRef { get; set; }

        /// <summary>
        /// 支付产品ID，如：8580
        /// </summary>
        public virtual string PaymentProductId { get; set; }

        /// <summary>
        /// 如：69.64.82.55
        /// </summary>
        public virtual string IpAddress { get; set; }

        /// <summary>
        /// 版本，如：2.0
        /// </summary>
        public virtual string Version { get; set; }

        /// <summary>
        /// 交易信息提交地址，如：https://ps.gcsip.nl/hpp/hpp
        /// </summary>
        public virtual string RequestUrl { get; set; }

        /// <summary>
        /// 付款成功后浏览器跳转URL(含有POST信息)
        /// </summary>
        public virtual string ResponseUrl { get; set; }

    }
}
