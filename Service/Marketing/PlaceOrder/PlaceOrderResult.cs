using System;

namespace Com.Panduo.Service.Marketing.PlaceOrder
{
    /// <summary>
    /// 客户下单支付完成后：送coupon\送礼给客户，
    /// 在前台界面需要显示信息提示给客户送了哪些东西 
    /// </summary>
    [Serializable]
    public class PlaceOrderResult
    {
        /// <summary>
        /// 订单客户
        /// </summary>
        public virtual int CustomerId { get; set; }

        /// <summary>
        /// 订单Id
        /// </summary>
        public virtual int OrderId { get; set; }

        /// <summary>
        /// 送的结果类型
        /// </summary>
        public virtual MarketingPlaceOrderResultType ResultType { get; set; }

        /// <summary>
        /// 送礼级数：与ERP设置的送礼级数对应。默认只提供最大为5级即:ERP中设置送礼级数，每一级都代表对于要送的礼品，物品编号在CRM或导单过程中解析
        /// </summary>
        public virtual string GiftLevel { get; set; }

        /// <summary>
        /// 送的Coupon编号
        /// </summary>
        public virtual string CouponCode { get; set; }

        /// <summary>
        /// 币种Id
        /// </summary>
        public virtual int? CurrencyId { get; set; }
    }

    public enum MarketingPlaceOrderResultType
    {
        /// <summary>
        /// 送礼
        /// </summary>
        Gift, 
        /// <summary>
        /// 送Coupon
        /// </summary>
        Coupon, 
        /// <summary>
        /// 既送礼又送Coupon
        /// </summary>
        GiftAndCoupon
    }
}
