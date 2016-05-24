using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product
{
    /// <summary>
    /// 相似商品属性值视图
    /// </summary>
    [Class(Table = "v_product_similar_ignore", Lazy = false, NameType = typeof(ProductSimilarIgnoreViewPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class ProductSimilarIgnoreViewPo
    {
        /// <summary>
        /// 自增id
        /// </summary>

        [Id(1, Name = "ProductPropertyValueId", Column = "product_property_value_id")]
        [Generator(2, Class = "native")]

        public virtual int ProductPropertyValueId
        {
            get;
            set;
        }
        /// <summary>
        /// 产品id
        /// </summary>
        [Property(Column = "product_id")]
        public virtual int ProductId
        {
            get;
            set;
        }
        /// <summary>
        /// 属性id
        /// </summary>
        [Property(Column = "property_id")]
        public virtual int PropertyId
        {
            get;
            set;
        }
        /// <summary>
        /// 属性值id
        /// </summary>
        [Property(Column = "property_value_id")]
        public virtual int PropertyValueId
        {
            get;
            set;
        }
        
        /// <summary>
        /// 类别ID
        /// </summary>
        [Property(Column = "category_id")]
        public virtual int CategoryId
        {
            get;
            set;
        }

        /// <summary>
        /// 类别路径
        /// </summary>
        [Property(Column = "category_path")]
        public virtual string CategoryPath
        {
            get;
            set;
        } 
    }
}
