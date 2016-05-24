using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Order
{
    /// <summary>
    /// 订单支付状态
    /// </summary>
    [Serializable]
    public class OrderPaidStatus
    {
        /// <summary>
        /// 状态Name
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 语种
        /// </summary>
        public virtual int LanguageId { get; set; }

        /// <summary>
        /// 状态Value
        /// </summary>
        public virtual int Value { get; set; }

    }
}
