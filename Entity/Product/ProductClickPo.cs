
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product
{
    /// <summary>
    ///描述：产品点击次数表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:48:06
    /// </summary>
    [Class(Table = "t_product_click", Lazy = false, NameType = typeof(ProductClickPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class ProductClickPo
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
        /// 周点击
        /// </summary>
        [Property(Column = "week")]
        public virtual int? Week
        {
            get;
            set;
        }
        /// <summary>
        /// 月点击
        /// </summary>
        [Property(Column = "month")]
        public virtual int? Month
        {
            get;
            set;
        }
        /// <summary>
        /// 季度点击
        /// </summary>
        [Property(Column = "season")]
        public virtual int? Season
        {
            get;
            set;
        }
        /// <summary>
        /// 年点击
        /// </summary>
        [Property(Column = "year")]
        public virtual int? Year
        {
            get;
            set;
        }
        /// <summary>
        /// 总点击
        /// </summary>
        [Property(Column = "total")]
        public virtual int? Total
        {
            get;
            set;
        }
    }
}

