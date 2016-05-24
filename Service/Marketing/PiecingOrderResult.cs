using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Service.Order.ShoppingCart;

namespace Com.Panduo.Service.Marketing
{
    /// <summary>
    /// 凑单提示对象
    /// </summary>
    [Serializable]
    public class PiecingOrderResult 
    {
        /// <summary>
        /// 是否有凑单提醒
        /// </summary>
        public virtual bool HasPiecingOrderTip { get; set; }

        /// <summary>
        /// 是否是Club凑单免运费
        /// </summary>
        public virtual bool IsClubFreeShipping { get; set; }

        /// <summary>
        /// 凑单差额
        /// </summary>
        public virtual decimal OrderBalance { get; set; }
        /// <summary>
        /// 订单可享受折扣
        /// </summary>
        public virtual decimal OrderDiscount { get; set; }

    }

}
