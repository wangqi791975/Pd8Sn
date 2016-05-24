
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Payment
{
    /// <summary>
    ///描述：Paypal支付日志表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-03-03 13:57:09
    /// </summary>
    [Class(Table = "t_payment_log_paypal", Lazy = false, NameType = typeof(PaymentLogPaypalPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class PaymentLogPaypalPo
    {
        /// <summary>
        /// 主键,自增长
        /// </summary>
        [Id(1, Name = "Id", Column = "id")]
        [Generator(2, Class = "native")]
        public virtual int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 支付目标对象ID(OrderId或者客户ID)
        /// </summary>
        [Property(Column = "target_id")]
        public virtual int TargetId
        {
            get;
            set;
        }
        /// <summary>
        /// 是否Paypal快速支付
        /// </summary>
        [Property(Column = "is_express_checkout")]
        public virtual bool IsExpressCheckout
        {
            get;
            set;
        }
        /// <summary>
        /// 支付目标类型：Order-订单,Club
        /// </summary>
        [Property(Column = "paypal_target_type")]
        public virtual int? PaypalTargetType
        {
            get;
            set;
        }
        /// <summary>
        /// 传递给paypal的产品名称
        /// </summary>
        [Property(Column = "item_name")]
        public virtual string ItemName
        {
            get;
            set;
        }
        /// <summary>
        /// 传递给paypal的网站订单号
        /// </summary>
        [Property(Column = "item_number")]
        public virtual string ItemNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 产品数量
        /// </summary>
        [Property(Column = "quantity")]
        public virtual string Quantity
        {
            get;
            set;
        }
        /// <summary>
        /// 付款币种编码,比如USD，EUR
        /// </summary>
        [Property(Column = "mc_currency")]
        public virtual string McCurrency
        {
            get;
            set;
        }
        /// <summary>
        /// 付款金额
        /// </summary>
        [Property(Column = "mc_gross")]
        public virtual decimal? McGross
        {
            get;
            set;
        }
        /// <summary>
        /// 交易费
        /// </summary>
        [Property(Column = "mc_fee")]
        public virtual string McFee
        {
            get;
            set;
        }
        /// <summary>
        /// 扣除交易费之前的客户付款全部美元金额
        /// </summary>
        [Property(Column = "payment_gross")]
        public virtual string PaymentGross
        {
            get;
            set;
        }
        /// <summary>
        /// 与付款相关的美元交易费
        /// </summary>
        [Property(Column = "payment_fee")]
        public virtual string PaymentFee
        {
            get;
            set;
        }
        /// <summary>
        /// 运费
        /// </summary>
        [Property(Column = "shipping")]
        public virtual string Shipping
        {
            get;
            set;
        }
        /// <summary>
        /// 对付款收取的税费金额
        /// </summary>
        [Property(Column = "tax")]
        public virtual decimal? Tax
        {
            get;
            set;
        }
        /// <summary>
        /// 付款时间
        /// </summary>
        [Property(Column = "payment_date")]
        public virtual string PaymentDate
        {
            get;
            set;
        }
        /// <summary>
        /// 付款状态:Canceled_Reversal、Completed、Denied、Failed、Pending (echeck)、Pending (multicurrency)、Pending (None)、Refunded、Reversed
        /// </summary>
        [Property(Column = "payment_status")]
        public virtual string PaymentStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 字符集
        /// </summary>
        [Property(Column = "charset")]
        public virtual string Charset
        {
            get;
            set;
        }
        /// <summary>
        /// 支付人客户唯一号
        /// </summary>
        [Property(Column = "payer_id")]
        public virtual string PayerId
        {
            get;
            set;
        }
        /// <summary>
        /// 支付人邮件
        /// </summary>
        [Property(Column = "payer_email")]
        public virtual string PayerEmail
        {
            get;
            set;
        }
        /// <summary>
        /// 支付人客户认证状态:空字符串、未认证unverified、已认证verified
        /// </summary>
        [Property(Column = "payer_status")]
        public virtual string PayerStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 收款人账号
        /// </summary>
        [Property(Column = "business")]
        public virtual string Business
        {
            get;
            set;
        }
        /// <summary>
        /// 收款人（即商家）的唯一账户号
        /// </summary>
        [Property(Column = "receiver_id")]
        public virtual string ReceiverId
        {
            get;
            set;
        }
        /// <summary>
        /// 收款人Email地址
        /// </summary>
        [Property(Column = "receiver_email")]
        public virtual string ReceiverEmail
        {
            get;
            set;
        }
        /// <summary>
        /// 两位 ISO 3166 国家或地区代码
        /// </summary>
        [Property(Column = "residence_country")]
        public virtual string ResidenceCountry
        {
            get;
            set;
        }
        /// <summary>
        /// 账单地址-付款人firstName
        /// </summary>
        [Property(Column = "first_name")]
        public virtual string FirstName
        {
            get;
            set;
        }
        /// <summary>
        /// 账单地址-付款人LastName
        /// </summary>
        [Property(Column = "last_name")]
        public virtual string LastName
        {
            get;
            set;
        }
        /// <summary>
        /// 账单地址-地址名称
        /// </summary>
        [Property(Column = "address_name")]
        public virtual string AddressName
        {
            get;
            set;
        }
        /// <summary>
        /// 账单地址-地址状态:空字符串、Confirmed已确认、Unconfirmed未确认
        /// </summary>
        [Property(Column = "address_status")]
        public virtual string AddressStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 账单地址-国家编码
        /// </summary>
        [Property(Column = "address_country_code")]
        public virtual string AddressCountryCode
        {
            get;
            set;
        }
        /// <summary>
        /// 账单地址-国家名称
        /// </summary>
        [Property(Column = "address_country_name")]
        public virtual string AddressCountryName
        {
            get;
            set;
        }
        /// <summary>
        /// 账单地址-州、省、郡
        /// </summary>
        [Property(Column = "address_state")]
        public virtual string AddressState
        {
            get;
            set;
        }
        /// <summary>
        /// 账单地址-城市
        /// </summary>
        [Property(Column = "address_city")]
        public virtual string AddressCity
        {
            get;
            set;
        }
        /// <summary>
        /// 账单地址-街道地址1
        /// </summary>
        [Property(Column = "address_street1")]
        public virtual string AddressStreet1
        {
            get;
            set;
        }
        /// <summary>
        /// 账单地址-街道地址2
        /// </summary>
        [Property(Column = "address_street2")]
        public virtual string AddressStreet2
        {
            get;
            set;
        }
        /// <summary>
        /// 账单地址-邮政编码
        /// </summary>
        [Property(Column = "address_zip")]
        public virtual string AddressZip
        {
            get;
            set;
        }
        /// <summary>
        /// 账单地址-电话
        /// </summary>
        [Property(Column = "phone_number")]
        public virtual string PhoneNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 通知使用的接口版本
        /// </summary>
        [Property(Column = "notify_version")]
        public virtual string NotifyVersion
        {
            get;
            set;
        }
        /// <summary>
        /// 签名标识
        /// </summary>
        [Property(Column = "verify_sign")]
        public virtual string VerifySign
        {
            get;
            set;
        }
        /// <summary>
        /// 额外传递到paypal的信息
        /// </summary>
        [Property(Column = "txn_id")]
        public virtual string TxnId
        {
            get;
            set;
        }
        /// <summary>
        /// PayPal系统生成的唯一交易号
        /// </summary>
        [Property(Column = "txn_type")]
        public virtual string TxnType
        {
            get;
            set;
        }
        /// <summary>
        /// 交易类型：cart、cleared-echeck、denied、echeck-denied、expresscheckout、None、parent、unique、web_accept
        /// </summary>
        [Property(Column = "payment_type")]
        public virtual string PaymentType
        {
            get;
            set;
        }
        /// <summary>
        /// 付款类型
        /// </summary>
        [Property(Column = "custom")]
        public virtual string Custom
        {
            get;
            set;
        }
        /// <summary>
        /// Paypal所有返回信息组成的key-value用空格分隔起来的信息
        /// </summary>
        [Property(Column = "total_info")]
        public virtual string TotalInfo
        {
            get;
            set;
        }
        /// <summary>
        /// 备注
        /// </summary>
        [Property(Column = "commonts")]
        public virtual string Commonts
        {
            get;
            set;
        }

        /// <summary>
        /// Pending原因,只有在payment_status=Pending时，才会设置此变量:address,authorization,echeck,intl,multi-currency,unilateral,upgrade,verify,,other
        /// </summary>
        [Property(Column = "pending_reason")] 
        public virtual string PendingReason { get; set; }

        /// <summary>
        /// 只有在payment_status = Reversed 或Refunded时，才会设置此变量chargeback,guarantee,buyer-complaint,refund,other
        /// </summary>
        [Property(Column = "reason_code")] 
        public virtual string ReasonCode { get; set; }

        /// <summary>
        /// 快速支付应答
        /// </summary>
        [Property(Column = "ack")]
        public virtual string Ack
        {
            get;
            set;
        }
        /// <summary>
        /// 快速支付Token
        /// </summary>
        [Property(Column = "token")]
        public virtual string Token
        {
            get;
            set;
        }
        /// <summary>
        /// 快速支付描述
        /// </summary>
        [Property(Column = "descritpion")]
        public virtual string Descritpion
        {
            get;
            set;
        }
        /// <summary>
        /// 记录创建时间
        /// </summary>
        [Property(Column = "create_date")]
        public virtual DateTime? CreateDate
        {
            get;
            set;
        }
    }
}

