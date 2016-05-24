
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.SiteConfigure
{
    /// <summary>
    ///描述：国家相关[高危国家]表 ORM 映射类 
    ///创建人:万天文
    ///创建时间：04/20/2015 10:26:13
    /// </summary>
    [Class(Table = "t_country_high_risk", Lazy = false, NameType = typeof(CountryHighRiskPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class CountryHighRiskPo
    {
        /// <summary>
        /// 国家ID
        /// </summary>
        [Id(1, Name = "CountryId", Column = "country_id")]
        [Generator(2, Class = "assigned")]
        public virtual int CountryId
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
        /// 创建时间
        /// </summary>
        [Property(Column = "date_created")]
        public virtual DateTime DateCreated
        {
            get;
            set;
        }

        /// <summary>
        /// 管理员ID
        /// </summary>
        [Property(Column = "admin_id")]
        public virtual int AdminId
        {
            get;
            set;
        }
    }
}

