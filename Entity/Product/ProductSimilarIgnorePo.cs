using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product
{
    /// <summary>
    /// 相似商品排除属性
    /// </summary>
    [Class(Table = "t_product_similar_ignore", Lazy = false, NameType = typeof(ProductSimilarIgnorePo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)] 
    public class ProductSimilarIgnorePo
    {
        /// <summary>
        /// 自增id
        /// </summary> 
        [Id(1, Name = "Id", Column = "id")]
        [Generator(2, Class = "native")] 
        public virtual int Id
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

        /// <summary>
        /// 忽略的属性(逗号分隔多个属性ID)eg:123,232,43,211
        /// </summary>
        [Property(Column = "ignore_property_ids")]
        public virtual string IgnorePropertyIds
        {
            get;
            set;
        }
    }
}
