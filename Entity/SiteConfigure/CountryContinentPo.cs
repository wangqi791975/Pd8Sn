
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.SiteConfigure
{
    /// <summary>
    ///描述：国家相关[洲]表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:38:16
    /// </summary>
    [Class(Table = "t_country_continent", Lazy = false, NameType = typeof(CountryContinentPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class CountryContinentPo
    {
        /// <summary>
        /// 自增ID
        /// </summary>

        [Id(1, Name = "ContinentId", Column = "continent_id")]
        [Generator(2, Class = "native")]

        public virtual int ContinentId
        {
            get;
            set;
        }
        /// <summary>
        /// 洲名称
        /// </summary>
        [Property(Column = "continent_name")]
        public virtual string ContinentName
        {
            get;
            set;
        }
    }
}

