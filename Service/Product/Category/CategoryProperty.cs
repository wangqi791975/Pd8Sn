using System;
using Com.Panduo.Service.Product.Property;

namespace Com.Panduo.Service.Product.Category
{
    /// <summary>
    /// 类别属性
    /// </summary>
    [Serializable]
    public class CategoryProperty
    {

        /// <summary>
        /// 类别属性Id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 类别ID
        /// </summary>
        public virtual int CategoryId { get; set; }

        /// <summary>
        /// 属性ID
        /// </summary>
        public virtual int PropertyId { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public virtual int DisplayOrder { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public virtual bool IsDisplay { get; set; }

        /// <summary>
        /// 属性值排序类型
        /// </summary>
        public virtual PropertyValueSortType SortType { get; set; }
    }
}
