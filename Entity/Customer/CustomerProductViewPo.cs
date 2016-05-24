using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Customer
{
    /// <summary>
    ///描述：客户产品视图 ORM 映射类 
    ///创建人:wq
    ///创建时间：2015-04-23 14:29:30
    /// </summary>
    [Class(Table = "v_customer_product", Lazy = false, NameType = typeof(CustomerProductViewPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class CustomerProductViewPo
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
        /// 客户Id
        /// </summary>
        [Property(Column = "customer_id")]
        public virtual int CustomerId
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
        /// 产品编号
        /// </summary>
        [Property(Column = "product_model")]
        public virtual string ProductModel
        {
            get;
            set;
        }
        /// <summary>
        /// 语言Id
        /// </summary>
        [Property(Column = "language_id")]
        public virtual int LanguageId
        {
            get;
            set;
        }
        /// <summary>
        /// 产品名称
        /// </summary>
        [Property(Column = "name")]
        public virtual string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 图片
        /// </summary>
        [Property(Column = "image")]
        public virtual string Image
        {
            get;
            set;
        }
        /// <summary>
        /// 最高单价
        /// </summary>
        [Property(Column = "profit_rate")]
        public virtual decimal ProfitRate
        {
            get;
            set;
        }
    }
}