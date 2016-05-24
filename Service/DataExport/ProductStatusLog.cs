
using System;

namespace Com.Panduo.Service.DataExport
{
    /// <summary>
    ///描述：产品状态数据统计
    ///创建人: 万天文
    ///创建时间：04/08/2015 16:04:01
    /// </summary>
    [Serializable]
    public class ProductStatusLog
    {
        /// <summary>
        /// 时间区间
        /// </summary>
        public virtual string DateTimeInterval { get; set; }

        /// <summary>
        /// 网站
        /// </summary>
        public virtual string WebSite{ get; set; }

        /// <summary>
        /// 记录时间
        /// </summary>
        public virtual DateTime LogDateTime { get; set; }

        /// <summary>
        /// BackOrder数量
        /// </summary>
        public virtual int BackOrderNumber { get; set; }

        /// <summary>
        /// 正常销售数量
        /// </summary>
        public virtual int OnSaleNumber { get; set; }

        /// <summary>
        /// 下架数量
        /// </summary>
        public virtual int OffLineNumber { get; set; }

        /// <summary>
        /// 所有数量
        /// </summary>
        public virtual int AllNumber { get; set; }

    }
}

