
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.SiteConfigure
{
    /// <summary>
    ///描述：国家相关[省份描述]表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:38:24
    /// </summary>
    [Class(Table = "t_country_province_description", Lazy = false, NameType = typeof(CountryProvinceDescriptionPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class CountryProvinceDescriptionPo
    {
        /// <summary>
        /// 自增ID
        /// </summary>

        [Id(1, Name = "DescriptionId", Column = "description_id")]
        [Generator(2, Class = "native")]

        public virtual int DescriptionId
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
        /// 语种ID
        /// </summary>
        [Property(Column = "language_id")]
        public virtual int LanguageId
        {
            get;
            set;
        }
        /// <summary>
        /// 区域名称
        /// </summary>
        [Property(Column = "province_name")]
        public virtual string ProvinceName
        {
            get;
            set;
        }
    }
}

