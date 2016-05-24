
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.SiteConfigure
{
    /// <summary>
    ///描述：国家相关[区域]表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:38:07
    /// </summary>
    [Class(Table = "t_country_area", Lazy = false, NameType = typeof(CountryAreaPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class CountryAreaPo
    {
        /// <summary>
        /// 自增ID
        /// </summary>

        [Id(1, Name = "AreaId", Column = "area_id")]
        [Generator(2, Class = "native")]

        public virtual int AreaId
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

