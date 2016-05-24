
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Payment
{
    /// <summary>
    ///描述：Hsbc支付日志表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-03-03 13:57:05
    /// </summary>
    [Class(Table = "t_payment_log_hsbc", Lazy = false, NameType = typeof(PaymentLogHsbcPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class PaymentLogHsbcPo
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
        /// 订单号
        /// </summary>
        [Property(Column = "order_no")]
        public virtual string OrderNo
        {
            get;
            set;
        }
        /// <summary>
        /// 是否标准币种
        /// </summary>
        [Property(Column = "is_standard_currency")]
        public virtual bool IsStandardCurrency
        {
            get;
            set;
        }
        /// <summary>
        /// 币种编码，比如USD,JPY
        /// </summary>
        [Property(Column = "currency_code")]
        public virtual string CurrencyCode
        {
            get;
            set;
        }
        /// <summary>
        /// 转账金额
        /// </summary>
        [Property(Column = "amount")]
        public virtual decimal? Amount
        {
            get;
            set;
        }
        /// <summary>
        /// 转账日期
        /// </summary>
        [Property(Column = "payment_date")]
        public virtual DateTime? PaymentDate
        {
            get;
            set;
        }
        /// <summary>
        /// 转账凭证,存储凭证附件的相对路径
        /// </summary>
        [Property(Column = "payment_receipt")]
        public virtual string PaymentReceipt
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

