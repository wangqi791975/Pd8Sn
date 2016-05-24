using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Com.Panduo.Web.PaymentCommon.PayConfig;

namespace Com.Panduo.Web.PaymentCommon.Service.Parm.GlobalCollect
{
    [Serializable]
    public class GlobalCollectParm
    {
        /// <summary>
        /// Id for merchant
        /// </summary>
        public string MerchantId { get; set; }
        /// <summary>
        /// 网站订单号
        /// </summary>
        public string WebOrderId { get; set; } 

        /// <summary>
        /// GC内部订单号
        /// </summary>
        public string GcOrderId { get; set; } 

        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        ///支付币种（USD、HKD）
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// 客户Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MerchantReference { get; set; }

        /// <summary>
        /// 信用卡类型
        /// </summary>
        public GlobalCollectType GlobalCollectType { get; set; }

        /// <summary>
        /// 支付结果状态值
        /// </summary>
        public string StatusId { get; set; }

        /// <summary>
        /// Number of the payment(for recurring orders),支付凭证
        /// </summary>
        public string EffortId { get; set; }

        /// <summary>
        /// GC支付页面的URL
        /// </summary>
        public string PayUrl { get; set; }

        /// <summary>
        /// Reference
        /// </summary>
        public string Ref { get; set; }

        /// <summary>
        /// Signature  
        /// </summary>
        public string Mac { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ReturnUrl { set; get; }

        public GlobalCollectBillingAddress BillingAddress { set; get; }

        public GlobalCollectShipingAddress ShippingAddress { set; get; }
    }
}

