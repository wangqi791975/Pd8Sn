
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product
{
    /// <summary>
    ///描述：产品名称表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:48:07
    /// </summary>
    [Class(Table = "t_product_desc", Lazy = false, NameType = typeof(ProductDescPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class ProductDescPo
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
        /// 语言别id
        /// </summary>
        [Property(Column = "language_id")]
        public virtual int LanguageId
        {
            get;
            set;
        }
        /// <summary>
        /// 名称
        /// </summary>
        [Property(Column = "`name`")]
        public virtual string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 简介
        /// </summary>
        [Property(Column = "description")]
        public virtual string Description
        {
            get;
            set;
        }
        /// <summary>
        /// 营销标题
        /// </summary>
        [Property(Column = "marketing_title")]
        public virtual string MarketingTitle
        {
            get;
            set;
        }
    }
}

