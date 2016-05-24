using System;

namespace Com.Panduo.Service.Marketing.PlaceOrder
{
    /// <summary>
    /// 下单活动
    /// </summary>
    [Serializable]
    public class PlaceOrderDetail
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 外键活动Id：t_Marketing表主键
        /// </summary>
        public virtual int MarketingId { get; set; }

        /// <summary>
        /// 起始金额
        /// </summary>
        public virtual decimal Amount { get; set; }

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
}
