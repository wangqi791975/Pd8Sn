using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Product
{
    /// <summary>
    /// 产品图片
    /// </summary>
    [Serializable]
    public class ProductImages
    {
        /// <summary>
        /// 产品Id
        /// </summary>
        public virtual int ProductId { get; set; }

        /// <summary>
        /// 图片名称
        /// </summary>
        public virtual string ImageName { get; set; }

        /// <summary>
        /// 是否主图
        /// </summary>
        public virtual Boolean IsMainImage { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public virtual int DisplayOrder { get; set; }

    }
}
