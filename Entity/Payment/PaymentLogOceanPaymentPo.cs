
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Payment
{
    /// <summary>
    ///描述：OceanPayment钱海支付日志表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-03-03 13:57:08
    /// </summary>
    [Class(Table = "t_payment_log_oceanpayment", Lazy = false, NameType = typeof(PaymentLogOceanPaymentPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class PaymentLogOceanPaymentPo
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
        /// 支付目标对象ID(订单ID)
        /// </summary>
        [Property(Column = "target_id")]
        public virtual int? TargetId
        {
            get;
            set;
        }
        /// <summary>
        /// 响应类型:0-浏览器响应 1-服务器响应
        /// </summary>
        [Property(Column = "response_type")]
        public virtual string ResponseType
        {
            get;
            set;
        }
        /// <summary>
        /// 支付通道,比如Webmoney、Yandex、Credit Card、QiWi
        /// </summary>
        [Property(Column = "method")]
        public virtual string Method
        {
            get;
            set;
        }
        /// <summary>
        /// 收款人账户
        /// </summary>
        [Property(Column = "account")]
        public virtual string Account
        {
            get;
            set;
        }
        /// <summary>
        /// 收款人终端号
        /// </summary>
        [Property(Column = "terminal")]
        public virtual string Terminal
        {
            get;
            set;
        }
        /// <summary>
        /// 签名
        /// </summary>
        [Property(Column = "signvalue")]
        public virtual string Signvalue
        {
            get;
            set;
        }
        /// <summary>
        /// 网站订单号
        /// </summary>
        [Property(Column = "order_number")]
        public virtual string OrderNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 支付币种,比如USD,JPY
        /// </summary>
        [Property(Column = "order_currency")]
        public virtual string OrderCurrency
        {
            get;
            set;
        }
        /// <summary>
        /// 支付金额
        /// </summary>
        [Property(Column = "order_amount")]
        public virtual decimal? OrderAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 订单备注信息
        /// </summary>
        [Property(Column = "order_notes")]
        public virtual string OrderNotes
        {
            get;
            set;
        }
        /// <summary>
        /// 持卡人的信用卡卡号 ，截取前6位和后4位
        /// </summary>
        [Property(Column = "card_number")]
        public virtual string CardNumber
        {
            get;
            set;
        }
        /// <summary>
        /// Oceanpayment唯一的支付交易号
        /// </summary>
        [Property(Column = "payment_id")]
        public virtual string PaymentId
        {
            get;
            set;
        }
        /// <summary>
        /// 交易类型：0-消费交易,1-预授权交易
        /// </summary>
        [Property(Column = "payment_auth_type")]
        public virtual string PaymentAuthType
        {
            get;
            set;
        }
        /// <summary>
        /// 交易状态: -1: 待处理,0: 失败,1: 成功
        /// </summary>
        [Property(Column = "payment_status")]
        public virtual string PaymentStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 支付详情
        /// </summary>
        [Property(Column = "payment_details")]
        public virtual string PaymentDetails
        {
            get;
            set;
        }
        /// <summary>
        /// 响应错误代码
        /// </summary>
        [Property(Column = "error_code")]
        public virtual string ErrorCode
        {
            get;
            set;
        }
        /// <summary>
        /// 未通过的风控规则
        /// </summary>
        [Property(Column = "payment_risk")]
        public virtual string PaymentRisk
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

