using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Order
{
    /// <summary>
    ///描述：订单表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:39:32
    /// </summary>
    [Class(Table = "t_order", Lazy = false, NameType = typeof(OrderPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class OrderPo
    {
        /// <summary>
        /// 订单ID
        /// </summary>

        [Id(1, Name = "OrderId", Column = "order_id")]
        [Generator(2, Class = "native")]
        public virtual int OrderId
        {
            get;
            set;
        }
        /// <summary>
        /// 客户ID
        /// </summary>
        [Property(Column = "customer_id")]
        public virtual int CustomerId
        {
            get;
            set;
        }
        /// <summary>
        /// 订单编号
        /// </summary>
        [Property(Column = "order_number")]
        public virtual string OrderNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 配送方式ID
        /// </summary>
        [Property(Column = "shipping_id")]
        public virtual int ShippingId
        {
            get;
            set;
        }
        /// <summary>
        /// 支付方式ID
        /// </summary>
        [Property(Column = "payment_id")]
        public virtual int PaymentId
        {
            get;
            set;
        }
        /// <summary>
        /// 信用卡类型
        /// </summary>
        [Property(Column = "collect_type")]
        public virtual int? CollectType
        {
            get;
            set;
        }
        /// <summary>
        /// 汇率
        /// </summary>
        [Property(Column = "exchange_rate")]
        public virtual decimal ExchangeRate
        {
            get;
            set;
        }
        /// <summary>
        /// 币种
        /// </summary>
        [Property(Column = "currency_code")]
        public virtual string CurrencyCode
        {
            get;
            set;
        }
        /// <summary>
        /// 订单状态
        /// </summary>
        [Property(Column = "order_status")]
        public virtual int OrderStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 支付状态
        /// </summary>
        [Property(Column = "pay_status")]
        public virtual int PayStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 是否升级快递方式
        /// </summary>
        [Property(Column = "is_upgrade_shipping_method")]
        public virtual bool IsUpgradeShippingMethod
        {
            get;
            set;
        }
        /// <summary>
        /// 升级配送方式前的配送方式
        /// </summary>
        [Property(Column = "upgrade_shipping_method_id")]
        public virtual int? UpgradeShippingMethodId
        {
            get;
            set;
        }
        /// <summary>
        /// 升级配送方式前的运费
        /// </summary>
        [Property(Column = "upgrade_shipping_method_money")]
        public virtual decimal? UpgradeShippingMethodMoney
        {
            get;
            set;
        }
        /// <summary>
        /// 报关金额
        /// </summary>
        [Property(Column = "report_type")]
        public virtual decimal ReportType
        {
            get;
            set;
        }
        /// <summary>
        /// 报关币种
        /// </summary>
        [Property(Column = "report_currency_code")]
        public virtual string ReportCurrencyCode
        {
            get;
            set;
        }
        /// <summary>
        /// 报关物品金额
        /// </summary>
        [Property(Column = "report_product_money")]
        public virtual decimal ReportProductMoney
        {
            get;
            set;
        }
        /// <summary>
        /// 报关运费金额
        /// </summary>
        [Property(Column = "report_product_money")]
        public virtual decimal ReportShippingMoney
        {
            get;
            set;
        }
        /// <summary>
        /// 税号
        /// </summary>
        [Property(Column = "tax_number")]
        public virtual string TaxNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 税号类型
        /// </summary>
        [Property(Column = "tax_type")]
        public virtual int TaxType
        {
            get;
            set;
        }
        /// <summary>
        /// 缺货等等类型
        /// </summary>
        [Property(Column = "out_of_stock_wait_type")]
        public virtual int OutOfStockWaitType
        {
            get;
            set;
        }
        /// <summary>
        /// 订单类型(0:email,1:facebook)
        /// </summary>
        [Property(Column = "order_type")]
        public virtual int OrderType
        {
            get;
            set;
        }
        /// <summary>
        /// 包裹重量
        /// </summary>
        [Property(Column = "package_weight")]
        public virtual decimal PackageWeight
        {
            get;
            set;
        }
        /// <summary>
        /// 实际重量
        /// </summary>
        [Property(Column = "weight")]
        public virtual decimal Weight
        {
            get;
            set;
        }
        /// <summary>
        /// 最终重量(实际重和体积重取大)
        /// </summary>
        [Property(Column = "shipping_weight")]
        public virtual decimal? ShippingWeight
        {
            get;
            set;
        }
        /// <summary>
        /// 下单日期
        /// </summary>
        [Property(Column = "order_time")]
        public virtual DateTime? OrderTime
        {
            get;
            set;
        }
        /// <summary>
        /// 最后支付时间
        /// </summary>
        [Property(Column = "last_pay_time")]
        public virtual DateTime? LastPayTime
        {
            get;
            set;
        }
        /// <summary>
        /// 订单来源(0:PC,1:手机)
        /// </summary>
        [Property(Column = "order_source")]
        public virtual int OrderSource
        {
            get;
            set;
        }
        /// <summary>
        /// 订单备注
        /// </summary>
        [Property(Column = "order_remark")]
        public virtual string OrderRemark
        {
            get;
            set;
        }
        /// <summary>
        /// 送礼品等级
        /// </summary>
        [Property(Column = "gift_level")]
        public virtual string GiftLevel
        {
            get;
            set;
        }
        /// <summary>
        /// 是否已全部评论
        /// </summary>
        [Property(Column = "is_review_all")]
        public virtual bool IsReviewAll
        {
            get;
            set;
        }
        /// <summary>
        /// 语种code
        /// </summary>
        [Property(Column = "language_code")]
        public virtual string LanguageCode
        {
            get;
            set;
        }
        /// <summary>
        /// 下单时的IP
        /// </summary>
        [Property(Column = "order_ip_address")]
        public virtual string OrderIpAddress
        {
            get;
            set;
        }
    }
}

