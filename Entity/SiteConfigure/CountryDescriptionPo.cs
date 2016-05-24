using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.SiteConfigure
{
    /// <summary>
    ///描述：t_country_description ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2014-12-19 15:04:15
    /// </summary>
    [Class(Table = "t_country_description", Lazy = false, NameType = typeof(CountryDescriptionPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class CountryDescriptionPo
    {
        /// <summary>
        /// description_id
        /// </summary>
        [Id(1, Name = "DescriptionId", Column = "description_id")]
        [Generator(2, Class = "native")]
        public virtual int DescriptionId
        {
            get;
            set;
        }
        /// <summary>
        /// country_id
        /// </summary>
        [Property(Column = "country_id")]
        public virtual int CountryId
        {
            get;
            set;
        }
        /// <summary>
        /// language_id
        /// </summary>
        [Property(Column = "language_id")]
        public virtual int LanguageId
        {
            get;
            set;
        }
        /// <summary>
        /// country_name
        /// </summary>
        [Property(Column = "country_name")]
        public virtual string CountryName
        {
            get;
            set;
        }
    }
}

	
