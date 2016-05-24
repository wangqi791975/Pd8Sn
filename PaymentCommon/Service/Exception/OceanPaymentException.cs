using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.paypal.sdk.util;

namespace Com.Panduo.Web.PaymentCommon.Service.Exception
{
    /// <summary>
    /// Paypal支付日志
    /// </summary>
    public class OceanPaymentException : PaymentException
    { 
        public string Code { internal set; get; }

        public OceanPaymentException(string message, string code)
            : base(message)
        { 
            this.Code = code;
        }

        public OceanPaymentException(string message, string code, System.Exception innerException)
            : base(message, innerException)
        { 
            this.Code = code;
        }
    }
}
