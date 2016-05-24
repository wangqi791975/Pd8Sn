using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Com.Panduo.Web.Models.Payment
{
    /// <summary>
    /// 线下支付信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class OfflinePaymentInfo<T>
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public virtual string OrderNo { get; set; }
        /// <summary>
        /// 是否有Cash余额
        /// </summary>
        public virtual bool HasCash { get; set; }
        /// <summary>
        /// 金额信息(包含币种)
        /// </summary>
        public virtual string Amount { get; set; }
        /// <summary>
        /// 收款配置信息
        /// </summary>
        public virtual T PaymentInfo { get; set; }
    }
}