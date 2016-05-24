using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Com.Panduo.Web.PaymentCommon.PayInfo;

namespace Com.Panduo.Web.PaymentCommon.Service.Parm.Paypal
{
    /// <summary>
    /// paypal标准支付
    /// </summary>
    public class PaypalNotifyResult
    {
        public virtual bool IsValid { get; set; }

        public virtual PaypalInfo PaypalInfo { get; set; }
    }
}