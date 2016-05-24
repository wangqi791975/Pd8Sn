
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.SiteConfigure
{
    /// <summary>
    ///描述：区域国家表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:37:45
    /// </summary>
    [Class(Table = "t_area_country", Lazy = false, NameType = typeof(AreaCountryPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class AreaCountryPo
    {
        /// <summary>
        /// 自增ID
        /// </summary>

        [Id(1, Name = "AreaCountryId", Column = "area_country_id")]
        [Generator(2, Class = "native")]

        public virtual int AreaCountryId
        {
            get;
            set;
        }
        /// <summary>
        /// 国家二级编码
        /// </summary>
        [Property(Column = "country_iso_code_2")]
        public virtual string CountryIsoCode2
        {
            get;
            set;
        }
        /// <summary>
        /// 国家区域ID
        /// </summary>
        [Property(Column = "country_area_id")]
        public virtual int CountryAreaId
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
    }
}

