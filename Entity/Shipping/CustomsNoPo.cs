
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Shipping
{
    /// <summary>
    ///描述：配送方式关联国家关税表 ORM 映射类 
    ///创建人:万天文
    ///创建时间：2015-03-02 16:43:01
    /// </summary>
    [Class(Table = "t_customs_no", Lazy = false, NameType = typeof(CustomsNoPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class CustomsNoPo
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        [Id(1, Name = "Id", Column = "id")]
        [Generator(2, Class = "native")]
        public virtual int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 配送方式ID
        /// </summary>
        [Property(Column = "shipping_id")]
        public virtual int ShippingId
        {
            get;
            set;
        }
        /// <summary>
        /// 国家Id
        /// </summary>
        [Property(Column = "country_id")]
        public virtual int CountryId
        {
            get;
            set;
        }
        /// <summary>
        /// 是否必填项
        /// </summary>
        [Property(Column = "is_required")]
        public virtual bool IsRequired
        {
            get;
            set;
        }
        /// <summary>
        /// 关税类型
        /// </summary>
        [Property(Column = "type")]
        public virtual int Type
        {
            get;
            set;
        }
        /// <summary>
        /// 描述(和语言包拼接)
        /// </summary>
        [Property(Column = "note")]
        public virtual int Note
        {
            get;
            set;
        }
    }
}

