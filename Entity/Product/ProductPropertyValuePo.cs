
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product
{
    /// <summary>
    ///描述：产品-属性值关联表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:48:09
    /// </summary>
    [Class(Table = "t_product_property_value", Lazy = false, NameType = typeof(ProductPropertyValuePo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class ProductPropertyValuePo
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
        /// 属性值id
        /// </summary>
        [Property(Column = "property_value_id")]
        public virtual int PropertyValueId
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
    }
}

