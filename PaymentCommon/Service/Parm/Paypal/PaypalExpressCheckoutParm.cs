using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Com.Panduo.Web.PaymentCommon.Service.Parm.Paypal
{
    /// <summary>
    /// Paypal快速支付参数
    /// </summary>
    [Serializable]
    public class PaypalExpressCheckoutParm
    {
        /// <summary>
        /// 支付币种
        /// </summary>
        public string CurrencyCode;
        /// <summary>
        /// 成功返回URL
        /// </summary>
        public string SuccessUrl;
        /// <summary>
        /// 取消支付返回URL
        /// </summary>
        public string CancelUrl;
        /// <summary>
        /// Logo URL
        /// </summary>
        public string LogoUrl;
        /// <summary>
        /// 地址信息
        /// </summary>
        public PayPalAddress Address;
        /// <summary>
        /// 运费
        /// </summary>
        public decimal ShippingAmount;
        /// <summary>
        /// 支付总金额()
        /// </summary>
        public decimal PayAmount;

        /// <summary>
        /// 明细
        /// </summary>
        public ICollection<PayPalOrderItem> OrderItems;

        /// <summary>
        /// 语言Code
        /// </summary>
        public string LocalCode;

        /// <summary>
        /// 验证参数
        /// </summary>
        public void Check()
        {
            if (OrderItems == null || OrderItems.Count == 0) throw new ArgumentOutOfRangeException("OrderItems", "OrderItems is null or empty.");
        }
    }
}