using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Payment.PayConfig
{
    /// <summary>
    /// GC配置
    /// </summary>
    [Serializable]
    public class GlobalCollectConfig
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public virtual bool IsEnable { get; set; }
        /// <summary>
        /// 服务地址
        /// </summary>
        public virtual string ServiceUrl { get; set; } 
        /// <summary>
        /// 版本
        /// </summary>
        public virtual string Version { get; set; } 
        /// <summary>
        /// 交易语言,比如en
        /// </summary>
        public virtual string LanguageCode { get; set; } 
        /// <summary>
        /// HostedIndicator
        /// </summary>
        public virtual string HostedIndicator { get; set; } 
        /// <summary>
        /// Cvvindicator
        /// </summary>
        public virtual string Cvvindicator { get; set; }  
        /// <summary>
        /// MerchantId
        /// </summary>
        public virtual string MerchantId { get; set; }  
        /// <summary>
        /// IP
        /// </summary>
        public virtual string IpAddress { get; set; }
        /// <summary>
        /// 支付限额,Key为币种标准码（USD、AUD),Value为对应的限额
        /// </summary>
        public virtual IDictionary<string, decimal> MaxAmounts { get; set; }
        /// <summary>
        /// 信用卡类型启用标志,Key为信用卡类型,Value为是否启用
        /// </summary>
        public virtual IDictionary<GlobalCollectType, bool> GlobalCollectTypeSwitchMap { get; set; }
    }

    /// <summary>
    /// 信用卡类型
    /// </summary>
    public enum GlobalCollectType
    {
        Visa = 1,
        AmericanExpress = 2,
        MasterCard = 3,
        Jcb = 125
    }
}
