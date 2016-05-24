using System;
using System.Collections.Generic;

namespace Com.Panduo.Service.Product.ProductArea
{
    /// <summary>
    /// 营销专区
    /// </summary>
    [Serializable]
    public class ProductArea
    {
        /// <summary>
        /// 专区Id
        /// </summary>
        public virtual int AreaId { set; get; }

        /// <summary>
        /// 专区中文名称
        /// </summary>
        public virtual string AreaName { set; get; }

        /// <summary>
        /// 专区多语种
        /// </summary>
        public virtual List<ProductAreaLanguage> ProductAreaLanguages { get; set; }

        /// <summary>
        /// 是否生效
        /// </summary>
        public virtual bool IsValid { set; get; }

        /// <summary>
        /// 是否存在商品
        /// </summary>
        public virtual bool HasProduct { set; get; }

        /// <summary>
        /// 是否显示专区首页
        /// </summary>
        public virtual bool IsShowHome { set; get; }
    }
}
