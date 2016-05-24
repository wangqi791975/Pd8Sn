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
    public class PayPalException : PaymentException
    {
        public NVPCodec Parms { internal set; get; }
        public NVPCodec Response { internal set; get; }
        public string Code { internal set; get; }

        public PayPalException(string message, string code, NVPCodec parms, NVPCodec response)
            : base(message)
        {
            this.Parms = parms;
            this.Response = response;
            this.Code = code;
        }

        public PayPalException(string message, string code, NVPCodec parms, NVPCodec response, System.Exception innerException)
            : base(message, innerException)
        {
            this.Parms = parms;
            this.Response = response;
            this.Code = code;
        }
    }
}
