
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Shipping
{
    /// <summary>
    ///描述：配送方式排除城市表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-01-29 10:42:35
    /// </summary>
    [Class(Table = "t_shipping_disabled_city", Lazy = false, NameType = typeof(ShippingDisabledCityPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class ShippingDisabledCityPo
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
        /// 城市
        /// </summary>
        [Property(Column = "city")]
        public virtual string City
        {
            get;
            set;
        }
    }
}

