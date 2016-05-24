using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Com.Panduo.Service.Payment.PayConfig;
using Com.Panduo.Web.PaymentCommon.Service.Parm.OceanPayment;

namespace Com.Panduo.Web.Models.Payment
{
    /// <summary>
    /// 钱海支付
    /// </summary>
    [Serializable]
    public class OceanPaymentSubmitParm
    {
        /// <summary>
        /// 服务地址
        /// </summary>
        public virtual string ServiceUrl { get; set; }
        /// <summary>
        /// 钱海支付请求配置
        /// </summary>
        public virtual OceanPaymentParm OceanPaymentConfig { get; set; } 
        /// <summary>
        /// 订单
        /// </summary>
        public virtual Service.Order.Order Order { get; set; }
        /// <summary>
        /// 订单账单地址
        /// </summary>
        public virtual Service.Order.OrderBillingAddress BillingAddress { get; set; }
    }
}