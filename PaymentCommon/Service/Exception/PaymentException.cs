using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Panduo.Web.PaymentCommon.Service.Exception
{
    /// <summary>
    /// 业务异常类
    /// </summary> 
    public class PaymentException : System.Exception
    {
        public PaymentException(string message)
            : base(message)
        {

        }

        public PaymentException(string message, System.Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
