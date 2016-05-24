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
    public class OrderAmountDiscount 
    {

        public virtual int Id { get; set; }

        /// <summary>
        /// 运费折扣值
        /// </summary>
        public virtual decimal Amount { get; set; }
        /// <summary>
        /// 运费折扣值
        /// </summary>
        public virtual decimal Discount { get; set; }
    }
}
