using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Service.Order.ShippingOption;

namespace Com.Panduo.Service.Order
{
    /// <summary>
    /// 下单过程 传输实体
    /// </summary>
    [Serializable]
    public class CheckoutDraft
    {

        /// <summary>
        ///购物车ID
        /// </summary>
        public virtual int ShoppingCartId { get; set; }

        /// <summary>
        /// Club等级
        /// </summary>
        public virtual int ClubLevel { get; set; }

        /// <summary>
        /// 收货地址ID
        /// </summary>
        public virtual int ReceivingAddressId { get; set; }

        /// <summary>
        /// 账单地址ID
        /// </summary>
        public virtual int BillAddressId { get; set; }

        /// <summary>
        /// 运送方式ID
        /// </summary>
        public virtual int ShippingId { get; set; }

        /// <summary>
        /// 订单来源(手机、网站)
        /// </summary>
        public virtual int OrderSource { get; set; }

        /// <summary>
        /// 语种
        /// </summary>
        public virtual string LanguageCode { get; set; }

        /// <summary>
        /// 币种
        /// </summary>
        public virtual string CurrencyCode { get; set; }

        /// <summary>
        /// 报关类型
        /// </summary>
        public virtual int ReportType { get; set; }

        /// <summary>
        /// 报关币种
        /// </summary>
        public virtual string ReportCurrencyCode { get; set; }

        /// <summary>
        /// 报关物品金额
        /// </summary>
        public virtual decimal ReportProductMoney { get; set; }

        /// <summary>
        /// 报关运费
        /// </summary>
        public virtual decimal ReportShippingMoney { get; set; }

        /// <summary>
        /// 税号类型
        /// </summary>
        public virtual CustomsNoType CustomsNoType { get; set; }

        /// <summary>
        /// 税号
        /// </summary>
        public virtual string CustomsNoNumber { get; set; }

        /// <summary>
        /// 订单备注
        /// </summary>
        public virtual string OrderRemark { get; set; }

        /// <summary>
        /// 缺货等待类型
        /// </summary>
        public virtual OutOfStockWaitType OutOfStockWaitType { get; set; }

        /// <summary>
        /// 客户选择的CouponId
        /// </summary>
        public virtual int? CouponCustomerId { get; set; }

        /// <summary>
        /// 下单时的IP
        /// </summary>
        public string OrderIpAddress { get; set; }

    }

    public enum OutOfStockWaitType
    {
        /// <summary>
        /// 所有的物品全到齐后再发货
        /// </summary>
        AllItemsAvailable,

        /// <summary>
        /// 部分先发货
        /// </summary>
        SendPart
    }
}
