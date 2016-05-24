
using NHibernate.Mapping;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.SiteConfigure
{
    /// <summary>
    ///描述：国家表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:38:05
    /// </summary>
    [Class(Table = "t_country", Lazy = true, NameType = typeof(CountryPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class CountryPo
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        [Id(1, Name = "CountryId", Column = "country_id")]
        [Generator(2, Class = "native")]

        public virtual int CountryId
        {
            get;
            set;
        }
        /// <summary>
        /// 洲ID
        /// </summary>
        [ManyToOne(ClassType = typeof(CountryContinentPo), Column = "continent_id")]
        public virtual CountryContinentPo ContinentId
        {
            get;
            set;
        }
        /// <summary>
        /// 地址格式化
        /// </summary>
        [ManyToOne(ClassType = typeof(AddressFormatPo),Column = "address_format_id")]
        public virtual AddressFormatPo AddressFormat
        {
            get;
            set;
        }
        /// <summary>
        /// 国家英文名称
        /// </summary>
        [Property(Column = "country_name")]
        public virtual string CountryName
        {
            get;
            set;
        }
        /// <summary>
        /// 国家二级简码
        /// </summary>
        [Property(Column = "country_iso_code_2")]
        public virtual string CountryIsoCode2
        {
            get;
            set;
        }
        /// <summary>
        /// 国家三级科码
        /// </summary>
        [Property(Column = "country_iso_code_3")]
        public virtual string CountryIsoCode3
        {
            get;
            set;
        }
        /// <summary>
        /// 是否常用国家
        /// </summary>
        [Property(Column = "is_common")]
        public virtual bool IsCommon
        {
            get;
            set;
        }
        /// <summary>
        /// 排序
        /// </summary>
        [Property(Column = "sort_order")]
        public virtual int SortOrder
        {
            get;
            set;
        }
        /// <summary>
        /// 状态(0:已废弃,1:显示)
        /// </summary>
        [Property(Column = "`status`")]
        public virtual bool Status
        {
            get;
            set;
        }
    }
}

