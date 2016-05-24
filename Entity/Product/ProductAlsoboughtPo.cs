
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product
{
    /// <summary>
    ///描述：产品alsobought表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:48:04
    /// </summary>
    [Class(Table = "t_product_alsobought", Lazy = false, NameType = typeof(ProductAlsoboughtPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class ProductAlsoboughtPo
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
        /// 产品id
        /// </summary>
        [Property(Column = "product_id")]
        public virtual int ProductId
        {
            get;
            set;
        }
        /// <summary>
        /// alsobought产品id
        /// </summary>
        [Property(Column = "alsobought_product_id")]
        public virtual int AlsoboughtProductId
        {
            get;
            set;
        }
    }
}

