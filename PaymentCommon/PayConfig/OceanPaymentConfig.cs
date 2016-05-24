using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Web.PaymentCommon.PayConfig
{
    /// <summary>
    /// 钱海支付配置
    /// </summary>
    [Serializable]
    public class OceanPaymentConfig
    {   
        /// <summary>
        /// 服务地址
        /// </summary>
        public virtual string ServiceUrl { get; set; }
        /// <summary>
        /// 所有的支付接口,Key为支付方式Method(比如Webmoney、Yandex、Credit Card、QiWi),Value为支付配置
        /// </summary>
        public virtual IDictionary<string, OceanPaymentMethod> MethodMap { get; set; }
        /// <summary>
        /// 支付限额,Key为币种标准码（USD、AUD),Value为对应的限额
        /// </summary>
        public virtual IDictionary<string, decimal> MaxAmounts { get; set; }

        /// <summary>
        /// 根据支付方式渠道名称获取配置
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public virtual OceanPaymentMethod TryGetMethod(string method)
        {
            OceanPaymentMethod oceanPaymentMethod = null;
            if (!string.IsNullOrEmpty(method))
            {
                oceanPaymentMethod = MethodMap.Where(c => string.Equals(c.Key.ToLower(), method.ToLower())).Select(c => c.Value).FirstOrDefault();
            }

            return oceanPaymentMethod;
        }
    }

    /// <summary>
    /// 钱海支付方式
    /// </summary>
    [Serializable]
    public class OceanPaymentMethod
    { 
        /// <summary>
        /// 是否启用
        /// </summary>
        public virtual bool IsEnable { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public virtual string Method { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public virtual string Account { get; set; }
        /// <summary>
        /// 终端号
        /// </summary>
        public virtual string Terminal { get; set; }
        /// <summary>
        /// 终端号验证码
        /// </summary>
        public virtual string SecureCode { get; set; }
    }
}
