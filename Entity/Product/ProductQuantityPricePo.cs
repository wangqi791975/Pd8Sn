
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product
{
    /// <summary>
    ///描述：产品价格段表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:48:10
    /// </summary>
    [Class(Table = "t_product_quantity_price", Lazy = false, NameType = typeof(ProductQuantityPricePo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class ProductQuantityPricePo
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
        /// 产品数量
        /// </summary>
        [Property(Column = "quantity")]
        public virtual int Quantity
        {
            get;
            set;
        }
        /// <summary>
        /// 利润率
        /// </summary>
        [Property(Column = "profit_rate")]
        public virtual decimal ProfitRate
        {
            get;
            set;
        }
    }
}

