
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.SiteConfigure
{
    /// <summary>
    ///描述：省份表，老数据库对应zen_zones ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:38:22
    /// </summary>
    [Class(Table = "t_country_province", Lazy = false, NameType = typeof(CountryProvincePo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class CountryProvincePo
    {
        /// <summary>
        /// 自增ID
        /// </summary>

        [Id(1, Name = "ProvinceId", Column = "province_id")]
        [Generator(2, Class = "native")]

        public virtual int ProvinceId
        {
            get;
            set;
        }
        /// <summary>
        /// 国家ID
        /// </summary>
        [Property(Column = "country_id")]
        public virtual int CountryId
        {
            get;
            set;
        }
        /// <summary>
        /// 省份编码
        /// </summary>
        [Property(Column = "province_code")]
        public virtual string ProvinceCode
        {
            get;
            set;
        }
        /// <summary>
        /// 省份名称
        /// </summary>
        [Property(Column = "province_name")]
        public virtual string ProvinceName
        {
            get;
            set;
        }
    }
}

