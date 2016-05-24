using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Payment.PayConfig
{
    /// <summary>
    /// Paypal标准支付配置
    /// </summary>
    [Serializable]
    public class PaypalConfig
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public virtual bool IsEnable { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public virtual string Account { get; set; }
        /// <summary>
        /// 提交地址
        /// </summary>
        public virtual string SubmitUrl { get; set; }
        /// <summary>
        /// 主机头
        /// </summary>
        public virtual string Host { get; set; }
        /// <summary>
        /// Logo地址
        /// </summary>
        public virtual string LogoUrl { get; set; }
        /// <summary>
        /// 描述前缀
        /// </summary>
        public virtual string DescPrefix { get; set; }
        /// <summary>
        /// 描述后缀
        /// </summary>
        public virtual string DescSubfix { get; set; } 
        /// <summary>
        /// 支付限额,Key为币种标准码（USD、AUD),Value为对应的限额
        /// </summary>
        public virtual IDictionary<string, decimal> MaxAmounts { get; set; } 
    }
}
