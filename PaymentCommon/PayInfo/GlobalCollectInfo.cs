using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Web.PaymentCommon.PayConfig;

namespace Com.Panduo.Web.PaymentCommon.PayInfo
{
    /// <summary>
    /// GC付款支付信息
    /// </summary>
    [Serializable]
    public class GlobalCollectInfo : BasePayInfo
    {
        /// <summary>
        /// 支付状态：表示待确认状态 ，当支付肯存在欺诈时候出现 
        /// </summary>
        public static readonly string GC_PAY_STATUS_CHALLENGED = "525";
        /// <summary>
        /// 支付状态：表示已准备状态 
        /// </summary>
        public static readonly string GC_PAY_STATUS_PENDING = "600";
        /// <summary>
        /// 支付状态：表示支付已完成
        /// </summary>
        public static readonly string GC_PAY_STATUS_READY = "800";

        /// <summary>
        /// 自增长ID
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 网站订单号
        /// </summary>
        public virtual string OrderNo { get; set; }
        /// <summary>
        /// GC订单号
        /// </summary>
        public virtual string GcOrderId { get; set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        public virtual decimal Amount { get; set; }
        /// <summary>
        /// 支付币种,比如USD,JPY
        /// </summary>
        public virtual string Currency { get; set; }
        /// <summary>
        /// 商户自定义参数
        /// </summary>
        public virtual string MerchantReference { get; set; }
        /// <summary>
        /// 信用卡交易号
        /// </summary>
        public virtual string EffortId { get; set; }
        /// <summary>
        /// 信用卡卡号
        /// </summary>
        public virtual string CreditCardNo { get; set; }
        /// <summary>
        /// 信用卡类型
        /// </summary>
        public virtual GlobalCollectType GlobalCollectType { get; set; }
        /// <summary>
        /// 信用卡交易状态
        /// </summary>
        public virtual string StatusId { get; set; }
        /// <summary>
        /// 支付的URL
        /// </summary>
        public virtual string PayUrl { get; set; }
        /// <summary>
        /// Ref
        /// </summary>
        public virtual string RefData { get; set; }
        /// <summary>
        /// MAC
        /// </summary>
        public virtual string Mac { get; set; }
        /// <summary>
        /// ResultState
        /// </summary>
        public virtual string ResultState { get; set; }
        /// <summary>
        /// Code
        /// </summary>
        public virtual string Code { get; set; }
        /// <summary>
        /// Message
        /// </summary>
        public virtual string Message { get; set; }

        #region 账单地址信息
        /// <summary>
        /// 姓
        /// </summary>
        public virtual string FirstName { get; set; }
        /// <summary>
        /// 名
        /// </summary>
        public virtual string LastName { get; set; }
        /// <summary>
        /// 国家
        /// </summary>
        public virtual string Country { get; set; }
        /// <summary>
        /// 街道
        /// </summary>
        public virtual string Street { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public virtual string City { get; set; }
        /// <summary>
        /// 州
        /// </summary>
        public virtual string State { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        public virtual string Zip { get; set; }
        #endregion
    }
}
