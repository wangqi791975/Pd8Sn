
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product
{
    /// <summary>
    ///描述：产品-类别关联表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:48:05
    /// </summary>
    [Class(Table = "t_product_category", Lazy = false, NameType = typeof(ProductCategoriePo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class ProductCategoriePo
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
        /// 类别id
        /// </summary>
        [Property(Column = "category_id")]
        public virtual int CategoryId
        {
            get;
            set;
        }
        /// <summary>
        /// 100,200,300
        /// </summary>
        [Property(Column = "category_path")]
        public virtual string CategoryPath
        {
            get;
            set;
        }
    }
}

