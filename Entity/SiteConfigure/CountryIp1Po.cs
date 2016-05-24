
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.SiteConfigure
{
    /// <summary>
    ///描述：两张IP库表用来切换 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2014-12-23 15:24:01
    /// </summary>
    [Class(Table = "t_country_ip1", Lazy = false, NameType = typeof(CountryIp1Po), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class CountryIp1Po
    {
        /// <summary>
        /// 自增id
        /// </summary>
        [Id(1, Name = "Id", Column = "id")]
        [Generator(2, Class = "native")]
        public virtual int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 起始IP
        /// </summary>
        [Property(Column = "ip_from")]
        public virtual long IpFrom
        {
            get;
            set;
        }
        /// <summary>
        /// 结束IP
        /// </summary>
        [Property(Column = "ip_to")]
        public virtual long IpTo
        {
            get;
            set;
        }
        /// <summary>
        /// 国家编码
        /// </summary>
        [Property(Column = "country_iso_code_2")]
        public virtual string CountryIsoCode2
        {
            get;
            set;
        }
        /// <summary>
        /// 国家名称
        /// </summary>
        [Property(Column = "country_name")]
        public virtual string CountryName
        {
            get;
            set;
        }
    }
}

