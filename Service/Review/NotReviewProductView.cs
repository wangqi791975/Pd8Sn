using System;

namespace Com.Panduo.Service.Review
{
    public class NotReviewProductView
    {
        /// <summary>
        /// 订单详细Id
        /// </summary>
        public virtual int OrderProductId { get; set; }

        /// <summary>
        /// 客户Id
        /// </summary>
        public virtual int CustomerId { get; set; }

        /// <summary>
        /// 全名
        /// </summary>
        public virtual string FullName { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public virtual int ProductId { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        public virtual string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public virtual string ProductName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public virtual int Status { get; set; }

    }
}