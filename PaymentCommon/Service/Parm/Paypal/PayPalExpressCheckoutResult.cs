using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Com.Panduo.Web.PaymentCommon.Service.Parm.Paypal
{
    [Serializable]
    public struct PayPalExpressCheckoutResult
    {
        /// <summary>
        /// 时间戳
        /// </summary>
        public string Token;
        /// <summary>
        /// 支付URL
        /// </summary>
        public string PayUrl;
        /// <summary>
        /// 应答信息
        /// </summary>
        public string Ack;
        /// <summary>
        /// 是否成功
        /// </summary>
        /// <returns></returns>
        public bool IsSuccess()
        {
            if (string.IsNullOrEmpty(Ack)) return false;
            return Ack.Equals("success", StringComparison.OrdinalIgnoreCase) || Ack.Equals("successwithwarning", StringComparison.OrdinalIgnoreCase);
        }
    }
}