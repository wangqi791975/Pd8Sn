
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.SiteConfigure
{
    /// <summary>
    ///描述：国家相关区域描述]表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:38:09
    /// </summary>
    [Class(Table = "t_country_area_description", Lazy = false, NameType = typeof(CountryAreaDescriptionPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class CountryAreaDescriptionPo
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
        /// 区域ID
        /// </summary>
        [Property(Column = "area_id")]
        public virtual int AreaId
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
        [Property(Column = "area_name")]
        public virtual string AreaName
        {
            get;
            set;
        }
    }
}

