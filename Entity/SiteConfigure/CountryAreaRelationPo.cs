
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.SiteConfigure
{
    /// <summary>
    ///描述：多对多关系 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:38:11
    /// </summary>
    [Class(Table = "t_country_area_relation", Lazy = false, NameType = typeof(CountryAreaRelationPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class CountryAreaRelationPo
    {
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
        /// 国家ID
        /// </summary>
        [Property(Column = "country_id")]
        public virtual int CountryId
        {
            get;
            set;
        }
    }
}

