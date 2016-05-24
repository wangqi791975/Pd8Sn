using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Marketing
{
    /// <summary>
    /// 订单折扣活动奖励
    /// </summary>
    [Serializable]
    public class OrderDiscountMarketing : Marketing
    {

        public virtual List<OrderAmountDiscount> OrderAmountDiscounts { set; get; }
    }
}
