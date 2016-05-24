using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Review
{
    [Class(Table = "v_not_review_product", Lazy = false, NameType = typeof(NotReviewProductViewPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class NotReviewProductViewPo
    {
        /// <summary>
        /// 订单详细Id
        /// </summary>
        [Id(1, Name = "OrderProductId", Column = "order_product_id")]
        [Generator(2, Class = "native")]
        public virtual int OrderProductId
        {
            get;
            set;
        }

        /// <summary>
        /// 客户Id
        /// </summary>
        [Property(Column = "customer_id")]
        public virtual int CustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// 全名
        /// </summary>
        [Property(Column = "full_name")]
        public virtual string FullName
        {
            get;
            set;
        }

        /// <summary>
        /// 产品Id
        /// </summary>
        [Property(Column = "product_id")]
        public virtual int ProductId
        {
            get;
            set;
        }

        /// <summary>
        /// 产品编号
        /// </summary>
        [Property(Column = "product_model")]
        public virtual string ProductCode
        {
            get;
            set;
        }

        /// <summary>
        /// 产品名称
        /// </summary>
        [Property(Column = "product_name")]
        public virtual string ProductName
        {
            get;
            set;
        }

        /// <summary>
        /// 状态
        /// </summary>
        [Property(Column = "`status`")]
        public virtual int Status
        {
            get;
            set;
        }

    }
}