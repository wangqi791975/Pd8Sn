
using System;

namespace Com.Panduo.Service.DataExport
{
    /// <summary>
    ///描述：客户注册数据
    ///创建人: 万天文
    ///创建时间：04/08/2015 16:04:01
    /// </summary>
    [Serializable]
    public class SaleInfo
    {
        /// <summary>
        /// Vip等级
        /// </summary>
        public virtual string VipLevel { get; set; }

        /// <summary>
        /// 网站
        /// </summary>
        public virtual string WebSite { get; set; }

        /// <summary>
        /// 客户数量
        /// </summary>
        public virtual decimal CustomerNumber { get; set; }

        /// <summary>
        /// 三个月内有订单客户数
        /// </summary>
        public virtual int HasOrderInThreeMonthsCustomerNumbert { get; set; }

        /// <summary>
        /// 三个月内订单总额
        /// </summary>
        public virtual decimal OrderInThreeMonthsAmount { get; set; }

        /// <summary>
        /// 两周内有订单客户数
        /// </summary>
        public virtual int HasOrderInTwoWeeksCustomerNumbert { get; set; }

        /// <summary>
        /// 两周内订单总额
        /// </summary>
        public virtual decimal OrderInTwoWeeksAmount { get; set; }

        /// <summary>
        /// 5.01注册未下单客户数
        /// </summary>
        public virtual int RegisteredNotOrderNumber { get; set; }

        /// <summary>
        /// 5.01注册有下单客户数
        /// </summary>
        public virtual int RegisteredHastOrderNumber { get; set; }
    }
}

