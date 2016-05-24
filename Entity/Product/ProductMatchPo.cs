
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product
{
    /// <summary>
    ///描述：产品match表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:48:08
    /// </summary>
    [Class(Table = "t_product_match", Lazy = false, NameType = typeof(ProductMatchPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class ProductMatchPo
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
        /// match产品id
        /// </summary>
        [Property(Column = "match_product_id")]
        public virtual int MatchProductId
        {
            get;
            set;
        }
    }
}

