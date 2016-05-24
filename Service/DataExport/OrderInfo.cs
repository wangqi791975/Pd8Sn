
using System;

namespace Com.Panduo.Service.DataExport
{
    /// <summary>
    ///描述：客户注册数据
    ///创建人: 万天文
    ///创建时间：04/08/2015 16:04:01
    /// </summary>
    [Serializable]
    public class OrderInfo : ClubCustomer
    {
        /// <summary>
        /// 时间区间
        /// </summary>
        public virtual string DateTimeInterval { get; set; }

        /// <summary>
        /// 网站
        /// </summary>
        public virtual string WebSite { get; set; }

        /// <summary>
        /// 客户数
        /// </summary>
        public virtual int CustomerNumber { get; set; }

        /// <summary>
        /// 同比订单数
        /// </summary>
        public virtual int OrderTotalYear { get; set; }

        /// <summary>
        /// 同比订单总金额
        /// </summary>
        public virtual decimal OrderAmountYear { get; set; }
    }
}

