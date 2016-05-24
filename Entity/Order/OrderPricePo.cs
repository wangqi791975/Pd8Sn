
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Order
{
    /// <summary>
    ///描述：订单各个金额的汇总
    ///订单总额=物品金额+总运费+CLUB手续费+CLUB运费差额+业务附加费-退款 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:39:41
    /// </summary>
    [Class(Table = "t_order_price", Lazy = false, NameType = typeof(OrderPricePo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class OrderPricePo
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        [Id(1, Name = "OrderPriceId", Column = "order_price_id")]
        [Generator(2, Class = "native")]
        public virtual int OrderPriceId
        {
            get;
            set;
        }
        /// <summary>
        /// 订单ID
        /// </summary>
        [Property(Column = "order_id")]
        public virtual int OrderId
        {
            get;
            set;
        }
        /// <summary>
        /// 正价商品金额
        /// </summary>
        [Property(Column = "product_amount_normal")]
        public virtual decimal ProductAmountNormal
        {
            get;
            set;
        }
        /// <summary>
        /// 原始物品金额
        /// </summary>
        [Property(Column = "product_original_amount")]
        public virtual decimal ProductAmountOriginal
        {
            get;
            set;
        }
        /// <summary>
        /// 折扣物品金额
        /// </summary>
        [Property(Column = "product_amount_discount")]
        public virtual decimal ProductAmountDiscount
        {
            get;
            set;
        }
        /// <summary>
        /// 基本运费(不包含偏远费)
        /// </summary>
        [Property(Column = "shipping_amount_base")]
        public virtual decimal ShippingAmountBase
        {
            get;
            set;
        }
        /// <summary>
        /// 偏远运费
        /// </summary>
        [Property(Column = "shipping_amount_remote")]
        public virtual decimal? ShippingAmountRemote
        {
            get;
            set;
        }
        /// <summary>
        /// 免运费差额
        /// </summary>
        [Property(Column = "free_shipping_balance")]
        public virtual decimal? FreeShippingBalance
        {
            get;
            set;
        }
        /// <summary>
        /// CLUB运费差额
        /// </summary>
        [Property(Column = "club_shipping_balance")]
        public virtual decimal? ClubShippingBalance
        {
            get;
            set;
        }
        /// <summary>
        /// CLUB手续费(值)
        /// </summary>
        [Property(Column = "club_handling_fee")]
        public virtual decimal ClubHandlingFee
        {
            get;
            set;
        }
        /// <summary>
        /// 如正常客户下单，该订单金额是300，但由于他是VIP该订单只需要200，那么该优惠金额是100
        /// </summary>
        [Property(Column = "product_discount")]
        public virtual decimal ProductDiscount
        {
            get;
            set;
        }
        /// <summary>
        /// VIP优惠金额
        /// </summary>
        [Property(Column = "vip_discount_money")]
        public virtual decimal VipDiscountMoney
        {
            get;
            set;
        }
        /// <summary>
        /// 订单折扣优惠金额
        /// </summary>
        [Property(Column = "order_discount_money")]
        public virtual decimal OrderDiscountMoney
        {
            get;
            set;
        }
        /// <summary>
        /// 运费手续费
        /// </summary>
        [Property(Column = "shipping_handling_fee")]
        public virtual decimal ShippingHandlingFee
        {
            get;
            set;
        }
        /// <summary>
        /// VIP折扣(如60%折扣存0.4)
        /// </summary>
        [Property(Column = "vip_discount_percent")]
        public virtual decimal VipDiscountPercent
        {
            get;
            set;
        }
        /// <summary>
        /// 订单折扣(如60%折扣存0.4)
        /// </summary>
        [Property(Column = "order_discount_percent")]
        public virtual decimal OrderDiscountPercent
        {
            get;
            set;
        }
        /// <summary>
        /// 运费折扣(如60%折扣存0.4)
        /// </summary>
        [Property(Column = "shipping_discount_percent")]
        public virtual decimal ShippingDiscountPercent
        {
            get;
            set;
        }
        /// <summary>
        /// 业务优惠金额
        /// </summary>
        [Property(Column = "business_derate_amount")]
        public virtual decimal BusinessDerateAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 业务附加费(值)
        /// </summary>
        [Property(Column = "business_added_amount")]
        public virtual decimal BusinessAddedAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 物品总额(计算列)
        /// </summary>
        [Property(Column = "product_amount_total")]
        public virtual decimal? ProductAmountTotal
        {
            get;
            set;
        }
        /// <summary>
        /// 总运费(计算列)
        /// </summary>
        [Property(Column = "shipping_amount_total")]
        public virtual decimal? ShippingAmountTotal
        {
            get;
            set;
        }
        /// <summary>
        /// 订单总额(计算列)
        /// </summary>
        [Property(Column = "order_amount_total")]
        public virtual decimal? OrderAmountTotal
        {
            get;
            set;
        }
        /// <summary>
        /// COUPON金额
        /// </summary>
        [Property(Column = "coupon_amount")]
        public virtual decimal CouponAmount
        {
            get;
            set;
        }
        /// <summary>
        /// cash支付
        /// </summary>
        [Property(Column = "use_cash")]
        public virtual decimal? UseCash
        {
            get;
            set;
        }
        /// <summary>
        /// 实际支付
        /// </summary>
        [Property(Column = "payment_amount")]
        public virtual decimal? PaymentAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 退款金额(正值)
        /// </summary>
        [Property(Column = "refund_amount")]
        public virtual decimal? RefundAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        [Property(Column = "date_modified")]
        public virtual DateTime? DateModified
        {
            get;
            set;
        }
    }
}

