using System;

namespace Com.Panduo.Service.Customer.Product
{
    /// <summary>
    /// 客户产品
    /// </summary>
    [Serializable]
    public class CustomerProduct
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        ///客户Id
        /// </summary>
        public virtual int CustomerId { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public virtual int ProductId { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        public virtual string ProductModel { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime? DateCreated { get; set; }
    }
}
