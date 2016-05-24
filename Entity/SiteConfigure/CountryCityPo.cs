
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.SiteConfigure
{
    /// <summary>
    ///描述：国家相关[城市]表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:38:13
    /// </summary>
    [Class(Table = "t_country_city", Lazy = false, NameType = typeof(CountryCityPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class CountryCityPo
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        [Id(1, Name = "CityId", Column = "city_id")]
        [Generator(2, Class = "native")]
        public virtual int CityId
        {
            get;
            set;
        }
        /// <summary>
        /// 省份ID
        /// </summary>
        [Property(Column = "province_id")]
        public virtual int ProvinceId
        {
            get;
            set;
        }

        /// <summary>
        /// 城市名称
        /// </summary>
        [Property(Column = "city_name")]
        public virtual string CityName
        {
            get;
            set;
        }
    }
}

