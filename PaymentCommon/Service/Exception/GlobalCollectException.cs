using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.paypal.sdk.util;

namespace Com.Panduo.Web.PaymentCommon.Service.Exception
{
    /// <summary>
    /// GlobalCollect payment exception 
    /// </summary>
    public class GlobalCollectException : PaymentException
    {
        public string ErrorCode { private set; get; }
        /// <summary>
        /// 请求参数
        /// </summary>
        public string RequestParms { private set; get; }
        public string ResponseXml { private set; get; }

        public GlobalCollectException(string message, string errorCode, string requestParms, string responseXml)
            : base(message)
        {
            this.ErrorCode = errorCode;
            this.RequestParms = requestParms;
            this.ResponseXml = responseXml;
        }

        public GlobalCollectException(string message, string errorCode, string requestParms, string responseXml, System.Exception innerException)
            : base(message, innerException)
        {
            this.ErrorCode = errorCode;
            this.RequestParms = requestParms;
            this.ResponseXml = responseXml;
        }
    }
}
