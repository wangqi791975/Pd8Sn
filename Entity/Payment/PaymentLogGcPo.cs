
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Payment
{
    /// <summary>
    ///描述：GC支付日志表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-03-03 13:57:04
    /// </summary>
    [Class(Table = "t_payment_log_gc", Lazy = false, NameType = typeof(PaymentLogGcPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class PaymentLogGcPo
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
        /// 网站订单Id
        /// </summary>
        [Property(Column = "target_id")]
        public virtual int? TargetId
        {
            get;
            set;
        }
        /// <summary>
        /// 网站订单号
        /// </summary>
        [Property(Column = "order_no")]
        public virtual string OrderNo
        {
            get;
            set;
        }
        /// <summary>
        /// GC订单号
        /// </summary>
        [Property(Column = "gc_order_id")]
        public virtual string GcOrderId
        {
            get;
            set;
        }
        /// <summary>
        /// 支付金额
        /// </summary>
        [Property(Column = "amount")]
        public virtual decimal? Amount
        {
            get;
            set;
        }
        /// <summary>
        /// 支付币种,比如USD,JPY
        /// </summary>
        [Property(Column = "currency")]
        public virtual string Currency
        {
            get;
            set;
        }
        /// <summary>
        /// 商户自定义参数
        /// </summary>
        [Property(Column = "merchantreference")]
        public virtual string Merchantreference
        {
            get;
            set;
        }
        /// <summary>
        /// 信用卡交易号
        /// </summary>
        [Property(Column = "effortid")]
        public virtual string Effortid
        {
            get;
            set;
        }
        /// <summary>
        /// 信用卡卡号
        /// </summary>
        [Property(Column = "credit_card_no")]
        public virtual string CreditCardNo
        {
            get;
            set;
        }
        /// <summary>
        /// 信用卡类型:
        /// </summary>
        [Property(Column = "global_collect_type")]
        public virtual int? GlobalCollectType
        {
            get;
            set;
        }
        /// <summary>
        /// 信用卡交易状态
        /// </summary>
        [Property(Column = "status_id")]
        public virtual string StatusId
        {
            get;
            set;
        }
        /// <summary>
        /// 支付的URL
        /// </summary>
        [Property(Column = "pay_url")]
        public virtual string PayUrl
        {
            get;
            set;
        }
        /// <summary>
        /// Ref
        /// </summary>
        [Property(Column = "ref_data")]
        public virtual string RefData
        {
            get;
            set;
        }
        /// <summary>
        /// MAC
        /// </summary>
        [Property(Column = "mac")]
        public virtual string Mac
        {
            get;
            set;
        }
        /// <summary>
        /// ResultState
        /// </summary>
        [Property(Column = "result_state")]
        public virtual string ResultState
        {
            get;
            set;
        }
        /// <summary>
        /// Code
        /// </summary>
        [Property(Column = "code")]
        public virtual string Code
        {
            get;
            set;
        }
        /// <summary>
        /// 账单地址-姓
        /// </summary>
        [Property(Column = "firstname")]
        public virtual string Firstname
        {
            get;
            set;
        }
        /// <summary>
        /// 账单地址-名
        /// </summary>
        [Property(Column = "lastname")]
        public virtual string Lastname
        {
            get;
            set;
        }
        /// <summary>
        /// 账单地址-国家
        /// </summary>
        [Property(Column = "country")]
        public virtual string Country
        {
            get;
            set;
        }
        /// <summary>
        /// 账单地址-街道
        /// </summary>
        [Property(Column = "street")]
        public virtual string Street
        {
            get;
            set;
        }
        /// <summary>
        /// 账单地址-城市
        /// </summary>
        [Property(Column = "city")]
        public virtual string City
        {
            get;
            set;
        }
        /// <summary>
        /// 账单地址-州
        /// </summary>
        [Property(Column = "state")]
        public virtual string State
        {
            get;
            set;
        }
        /// <summary>
        /// 账单地址-邮编
        /// </summary>
        [Property(Column = "zip")]
        public virtual string Zip
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

