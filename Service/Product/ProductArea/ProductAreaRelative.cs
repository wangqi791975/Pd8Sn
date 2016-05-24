using System;

namespace Com.Panduo.Service.Product.ProductArea
{
    /// <summary>
    /// 专区产品
    /// </summary>
    [Serializable]
    public class ProductAreaRelative
    {
        /// <summary>
        /// 专区Id
        /// </summary>
        public virtual int AreaId { set; get; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public virtual int ProductId { get; set; }
    }
}
