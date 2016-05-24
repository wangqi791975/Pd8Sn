
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product
{
    /// <summary>
    ///描述：专区产品表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2014-12-19 09:32:11
    /// </summary>
    [Class(Table = "t_product_area_relative", Lazy = false, NameType = typeof(ProductAreaRelativePo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class ProductAreaRelativePo
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
        /// 专区id
        /// </summary>
        [Property(Column = "product_area_id")]
        public virtual int ProductAreaId
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
    }
}

