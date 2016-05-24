using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Com.Panduo.Service.Payment.PayConfig;

namespace Com.Panduo.Web.Models.Payment
{
    /// <summary>
    /// paypal提交参数
    /// </summary>
    [Serializable]
    public class PaypalSubmitParm
    {
        /// <summary>
        /// Payapl配置
        /// </summary>
        public virtual PaypalConfig PaypalConfig { get; set; }

        /// <summary>
        /// 是否支付Club会员
        /// </summary>
        public virtual bool IsPayForClubFee { get; set; }
        /// <summary>
        /// 客户
        /// </summary>
        public virtual Service.Customer.Customer Customer { get; set; }
        /// <summary>
        /// 订单
        /// </summary>
        public virtual Service.Order.Order Order { get; set; }
        /// <summary>
        /// 订单账单地址
        /// </summary>
        public virtual Service.Order.OrderBillingAddress BillingAddress { get; set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        public virtual decimal PaymentAmount { get; set; }
        /// <summary>
        /// 支付币种
        /// </summary>
        public virtual string CurrencyCode { get; set; }
    }
}