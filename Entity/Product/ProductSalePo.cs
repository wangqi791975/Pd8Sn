
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product
{
    /// <summary>
    ///描述：产品销量表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:48:11
    /// </summary>
    [Class(Table = "t_product_sale", Lazy = false, NameType = typeof(ProductSalePo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class ProductSalePo
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
        public virtual int? ProductId
        {
            get;
            set;
        }
        /// <summary>
        /// 周销量
        /// </summary>
        [Property(Column = "week")]
        public virtual int? Week
        {
            get;
            set;
        }
        /// <summary>
        /// 月销量
        /// </summary>
        [Property(Column = "month")]
        public virtual int? Month
        {
            get;
            set;
        }
        /// <summary>
        /// 季度销量
        /// </summary>
        [Property(Column = "season")]
        public virtual int? Season
        {
            get;
            set;
        }
        /// <summary>
        /// 年销量
        /// </summary>
        [Property(Column = "year")]
        public virtual int? Year
        {
            get;
            set;
        }
        /// <summary>
        /// 总销量
        /// </summary>
        [Property(Column = "total")]
        public virtual int? Total
        {
            get;
            set;
        }
    }
}

