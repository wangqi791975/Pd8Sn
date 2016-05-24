using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Panduo.Service.Product
{
    /// <summary>
    /// 产品上货Excel对象
    /// </summary>
    [Serializable]
    public class ProductExcelRow
    {
        /// <summary>
        /// 产品线(类别编码)
        /// </summary>
        public virtual string CategoryCode { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        public virtual string ProductModel { get; set; }

        /// <summary>
        /// 上货价格
        /// </summary>
        public virtual decimal ProductPrice { get; set; }

        /// <summary>
        /// 属性名
        /// </summary>
        public virtual string PropertyName { get; set; }

        /// <summary>
        /// 价格段(如3、4)
        /// </summary>
        public virtual int ProductPriceSegments { get; set; }

        /// <summary>
        /// 产品净重(每组)
        /// </summary>
        public virtual decimal ProductGrossWeight { get; set; }

        /// <summary>
        /// 产品体积重(每组)
        /// </summary>
        public virtual decimal ProductVolumeWeight { get; set; }

        /// <summary>
        /// 属性值
        /// </summary>
        public virtual string PropertyValue { get; set; }

        /// <summary>
        /// 产品语种信息
        /// </summary>
        public virtual IList<ProductLanguage> ProductLanguages { get; set; }

        /// <summary>
        /// 是否限制库存
        /// </summary>
        public virtual int LimitStock { get; set; }

        /// <summary>
        /// 库存数量
        /// </summary>
        public virtual int ProductQuantity { get; set; }
    }
}
