using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Web.PaymentCommon.PayInfo
{
    /// <summary>
    /// Paypal支付信息
    /// </summary>
    [Serializable]
    public class PaypalInfo : BasePayInfo
    {
        /// <summary>
        ///  Paypal支付完成状态标识
        /// </summary>
        public static readonly string PAYPAL_STATUS_COMPLETED = "Completed";
        /// <summary>
        /// 表自增长ID
        /// </summary>
        public virtual int Id { get; set; }
        /// <summary>
        /// 支付目标对象ID(OrderId或者客户ID)
        /// </summary>
        public virtual int TargetId { get; set; }
        /// <summary>
        /// 是否Paypal快速支付
        /// </summary>
        public virtual bool IsExpressCheckOut { get; set; } 
        /// <summary>
        /// 传递给paypal的产品名称
        /// </summary>
        public virtual string ItemName { get; set; }
        /// <summary>
        /// 传递给paypal的网站订单号
        /// </summary>
        public virtual string ItemNumber { get; set; }
        /// <summary>
        /// 产品数量
        /// </summary>
        public virtual string Quantity { get; set; }
        /// <summary>
        /// 付款币种编码,比如USD，EUR
        /// </summary>
        public virtual string McCurrency { get; set; }
        /// <summary>
        /// 付款金额
        /// </summary>
        public virtual decimal McGross { get; set; }
        /// <summary>
        /// 交易费
        /// </summary>
        public virtual string McFee { get; set; }
        /// <summary>
        /// 扣除交易费之前的客户付款全部美元金额
        /// </summary>
        public virtual string PaymentGross { get; set; }
        /// <summary>
        /// 与付款相关的美元交易费
        /// </summary>
        public virtual string PaymentFee { get; set; }
        /// <summary>
        /// 运费
        /// </summary>
        public virtual string Shipping { get; set; }
        /// <summary>
        /// 对付款收取的税费金额
        /// </summary>
        public virtual decimal Tax { get; set; }
        /// <summary>
        /// 付款时间
        /// </summary>
        public virtual string PaymentDate { get; set; }
        /// <summary>
        /// 付款状态:Canceled_Reversal、Completed、Denied、Failed、Pending (echeck)、Pending (multicurrency)、Pending (None)、Refunded、Reversed
        /// </summary>
        public virtual string PaymentStatus { get; set; }
        /// <summary>
        /// 字符集
        /// </summary>
        public virtual string Charset { get; set; }

        /// <summary>
        /// 支付人客户唯一号
        /// </summary>
        public virtual string PayerId { get; set; }
        /// <summary>
        /// 支付人邮件
        /// </summary>
        public virtual string PayerEmail { get; set; }
        /// <summary>
        /// 支付人客户认证状态:空字符串、未认证unverified、已认证verified
        /// </summary>
        public virtual string PayerStatus { get; set; }
        /// <summary>
        /// 收款人账号
        /// </summary>
        public virtual string Business { get; set; }
        /// <summary>
        /// 收款人（即商家）的唯一账户号
        /// </summary>
        public virtual string ReceiverId { get; set; }
        /// <summary>
        /// 收款人Email地址
        /// </summary>
        public virtual string ReceiverEmail { get; set; }

        /// <summary>
        /// 两位 ISO 3166 国家或地区代码
        /// </summary>
        public virtual string ResidenceCountry { get; set; }
        /// <summary>
        /// 账单地址-付款人firstName
        /// </summary>
        public virtual string FirstName { get; set; }

        /// <summary>
        /// 账单地址-付款人LastName
        /// </summary>
        public virtual string LastName { get; set; }

        /// <summary>
        /// 账单地址-地址名称
        /// </summary>
        public virtual string AddressName { get; set; }

        /// <summary>
        /// 账单地址-地址状态:空字符串、Confirmed已确认、Unconfirmed未确认
        /// </summary>
        public virtual string AddressStatus { get; set; }

        /// <summary>
        /// 账单地址-国家编码
        /// </summary>
        public virtual string AddressCountryCode { get; set; }

        /// <summary>
        /// 账单地址-国家名称
        /// </summary>
        public virtual string AddressCountryName { get; set; }

        /// <summary>
        /// 账单地址-州、省、郡
        /// </summary>
        public virtual string AddressState { get; set; }

        /// <summary>
        /// 账单地址-城市
        /// </summary>
        public virtual string AddressCity { get; set; }

        /// <summary>
        /// 账单地址-街道地址1
        /// </summary>
        public virtual string AddressStreet1 { get; set; }

        /// <summary>
        /// 账单地址-街道地址2
        /// </summary>
        public virtual string AddressStreet2 { get; set; }

        /// <summary>
        /// 账单地址-邮政编码
        /// </summary>
        public string AddressZip { get; set; }

        /// <summary>
        /// 账单地址-电话
        /// </summary>
        public virtual string PhoneNumber { get; set; }

        /// <summary>
        /// 通知使用的接口版本
        /// </summary>
        public virtual string NotifyVersion { get; set; }

        /// <summary>
        /// 签名标识
        /// </summary>
        public virtual string VerifySign { get; set; }

        /// <summary>
        /// 额外传递到paypal的信息
        /// </summary>
        public virtual string Custom { get; set; }

        /// <summary>
        /// PayPal系统生成的唯一交易号
        /// </summary>
        public string TxnId { get; set; }

        /// <summary>
        /// 交易类型：cart、cleared-echeck、denied、echeck-denied、expresscheckout、None、parent、unique、web_accept
        /// </summary>
        public virtual string TxnType { get; set; }

        /// <summary>
        /// 付款类型
        /// </summary>
        public virtual string PaymentType { get; set; }

        /// <summary>
        /// Paypal所有返回信息组成的key-value用空格分隔起来的信息
        /// </summary>
        public virtual string TotalInfo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Commonts { get; set; }

        /// <summary>
        /// Pending原因,只有在payment_status=Pending时，才会设置此变量:address,authorization,echeck,intl,multi-currency,unilateral,upgrade,verify,,other
        /// </summary>
        public virtual string PendingReason { get; set; }
        /// <summary>
        /// 只有在payment_status = Reversed 或Refunded时，才会设置此变量chargeback,guarantee,buyer-complaint,refund,other
        /// </summary>
        public virtual string ReasonCode { get; set; }

        /// <summary>
        /// 快速支付Ack
        /// </summary>
        public string Ack { set; get; }

        /// <summary>
        /// 快速支付Token
        /// </summary>
        public string Token { set; get; }
        /// <summary>
        /// 快速支付描述
        /// </summary>
        public string Descritpion { set; get; }

        /// <summary>
        /// 是否成功完成交易
        /// </summary>
        public bool IsCompleted
        {
            get
            {
                return string.IsNullOrEmpty(PaymentStatus) ? false : string.Equals(PAYPAL_STATUS_COMPLETED, PaymentStatus.Trim(), StringComparison.InvariantCultureIgnoreCase);
            }
        }
    }
}
