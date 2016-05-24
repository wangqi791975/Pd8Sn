using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Com.Panduo.Web.PaymentCommon.Service.Exception
{

    /// <summary>
    /// 超过最大金额异常
    /// </summary>
    public class MaxAmountOutOfRangeException : System.Exception
    {
        public string Currency { private set; get; }
        public decimal PayAmount { private set; get; }
        public decimal MaxAmount { private set; get; }

        public MaxAmountOutOfRangeException(string currency, decimal maxAmount):this(currency,maxAmount,0)
        {

        }

        public MaxAmountOutOfRangeException(string currency, decimal maxAmount, decimal payAmount)
            : this(currency, maxAmount, 0,string.Empty)
        {

        }

        public MaxAmountOutOfRangeException(string currency, decimal maxAmount, string message): this(currency, maxAmount, 0,message)
        {

        }

        public MaxAmountOutOfRangeException(string currency, decimal maxAmount, decimal payAmount, string message)
            : base(message)
        {
            this.Currency = currency;
            this.MaxAmount = maxAmount;
            this.PayAmount = payAmount;
        }

        
    }
}