using System;
using System.Collections.Generic;

namespace Com.Panduo.Service.Marketing.PlaceOrder
{
    /// <summary>
    /// 下单活动
    /// </summary>
    [Serializable]
    public class PlaceOrderMarketing : Marketing
    {


        public virtual List<PlaceOrderDetail> PlaceOrderDetails { set; get; }

        /// <summary>
        /// 下单送礼类型
        /// </summary>
        public virtual MarketingPlaceOrderResultType PlaceOrderRewardType { get; set; }
    }
}
