
using System;

namespace Com.Panduo.Service.DataExport
{
    /// <summary>
    ///描述：客户注册数据
    ///创建人: 万天文
    ///创建时间：04/08/2015 16:04:01
    /// </summary>
    [Serializable]
    public class ClubCustomer : RegisterCustomer
    {
        /// <summary>
        /// 平均订单金额
        /// </summary>
        public virtual decimal AverageOrderAmount { get; set; }

        /// <summary>
        /// 订单数
        /// </summary>
        public virtual int OrderTotal { get; set; }

        /// <summary>
        /// 订单总金额
        /// </summary>
        public virtual decimal OrderAmount { get; set; }
    }
}

