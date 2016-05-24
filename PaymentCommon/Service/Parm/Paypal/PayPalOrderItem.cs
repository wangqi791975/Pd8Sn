using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Com.Panduo.Web.PaymentCommon.Service.Parm.Paypal
{
    /// <summary>
    /// 明细
    /// </summary>
    [Serializable]
    public class PayPalOrderItem
    { 
        public string Name { set; get; }
        public string Number { set; get; }
        public string Description { set; get; }
        public int Qty { set; get; }
        public decimal Amount { set; get; }
    }
}