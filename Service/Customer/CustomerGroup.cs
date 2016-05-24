using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Customer
{
    /// <summary>
    /// 客户等级
    /// </summary>
    [Serializable]
    public class CustomerGroup
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 最小金额（大于等于该值）
        /// </summary>
        public virtual decimal MinAmount { get; set; }

        /// <summary>
        /// 最大金额（小于该值）
        /// </summary>
        public virtual decimal MaxAmount { get; set; }

        /// <summary>
        /// 折扣（20%存0.8）
        /// </summary>
        public virtual decimal Discount { get; set; }

        /// <summary>
        /// VipAndRcd 折扣（20%存0.8）
        /// </summary>
        public virtual decimal VipAndRcdDiscount { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public virtual List<CustomerGroupDesc> CustomerGroupDesc { get; set; }
    }
}
