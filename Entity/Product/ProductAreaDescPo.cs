
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product
{
    /// <summary>
    ///描述：专区名称表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2014-12-19 09:32:10
    /// </summary>
    [Class(Table = "t_product_area_desc", Lazy = false, NameType = typeof(ProductAreaDescPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class ProductAreaDescPo
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
        /// 语言别
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
        /// 专区首页 HTML
        /// </summary>
        [Property(Column = "home")]
        public virtual string Home
        {
            get;
            set;
        }
    }
}

