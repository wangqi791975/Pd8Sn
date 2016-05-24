
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Shipping
{
    /// <summary>
    ///描述：配送方式排除邮编表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-01-29 10:42:37
    /// </summary>
    [Class(Table = "t_shipping_disabled_postcode", Lazy = false, NameType = typeof(ShippingDisabledPostcodePo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class ShippingDisabledPostcodePo
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
        /// 国家编码
        /// </summary>
        [Property(Column = "country_iso_code_2")]
        public virtual string CountryIsoCode2
        {
            get;
            set;
        }
        /// <summary>
        /// 邮编
        /// </summary>
        [Property(Column = "postcode")]
        public virtual string Postcode
        {
            get;
            set;
        }
    }
}

