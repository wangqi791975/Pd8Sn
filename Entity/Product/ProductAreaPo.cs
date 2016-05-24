
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product
{
    /// <summary>
    ///描述：专区表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2014-12-19 09:32:09
    /// </summary>
    [Class(Table = "t_product_area", Lazy = false, NameType = typeof(ProductAreaPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class ProductAreaPo
    {
        /// <summary>
        /// 自增id
        /// </summary>
        [Id(1, Name = "ProductAreaId", Column = "product_area_id")]
        [Generator(2, Class = "native")]
        public virtual int ProductAreaId
        {
            get;
            set;
        }
        /// <summary>
        /// 中文名称
        /// </summary>
        [Property(Column = "chinese_name")]
        public virtual string ChineseName
        {
            get;
            set;
        }
        /// <summary>
        /// 英文名称
        /// </summary>
        [Property(Column = "english_name")]
        public virtual string EnglishName
        {
            get;
            set;
        }
        /// <summary>
        /// 1显示，0不显示
        /// </summary>
        [Property(Column = "show_index")]
        public virtual bool ShowIndex
        {
            get;
            set;
        }
        /// <summary>
        /// 1有效，0无效
        /// </summary>
        [Property(Column = "`status`")]
        public virtual bool Status
        {
            get;
            set;
        }
    }
}

