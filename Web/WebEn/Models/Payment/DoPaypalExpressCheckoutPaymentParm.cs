using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Com.Panduo.Web.Models.Payment
{
    /// <summary>
    /// Paypal快速支付参数
    /// </summary>
    [Serializable]
    public class DoPaypalExpressCheckoutPaymentParm
    { 
        public virtual string CurrencyCode { get; set; }
        public virtual decimal PaymentAmount { get; set; }
        public virtual string OrderNo { get; set; }
        public virtual int? OrderId { get; set; }
    }
}