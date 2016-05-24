using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Com.Panduo.Web.Models.Payment
{
    /// <summary>
    /// Paypal快速支付结果
    /// </summary>
    [Serializable]
    public class DoPaypalExpressCheckoutPaymentResult
    { 
        /// <summary>
        /// 结果类型
        /// </summary>
        public virtual PaypalExpressResultType PaypalExpressResultType { get; set; }
        /// <summary>
        /// 额外数据
        /// </summary>
        public virtual string Data { get; set; }
        /// <summary>
        /// 错误消息
        /// </summary>
        public virtual string ErrorMessage { get; set; }
        /// <summary>
        /// 快速支付界面
        /// </summary>
        public virtual string PayPalExpressCheckoutUrl { get; set; }
    }

    public enum PaypalExpressResultType
    {
        /// <summary>
        /// 未知
        /// </summary>
        UnKnowError = -10,
        /// <summary>
        /// 金额超限
        /// </summary>
        MaxAmountOutOfRange = -20,
        /// <summary>
        /// Paypal异常
        /// </summary>
        PayPalException = -30,
        /// <summary>
        /// 支付成功
        /// </summary>
        PaySuccess = 100,
        /// <summary>
        /// 支付和业务处理都成功
        /// </summary>
        Success = 200
    }
}