
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Payment
{
    /// <summary>
    ///描述：支付方式-GC屏蔽国家表 ORM 映射类 
    ///创建人:万天文
    ///创建时间：2015-04-13 17:33:11
    /// </summary>
    [Class(Table = "t_global_collect_disabled_country", Lazy = false, NameType = typeof(GlobalCollectDisabledCountryPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class GlobalCollectDisabledCountryPo
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
        /// 国家名称
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
        /// <summary>
        /// 管理员Email
        /// </summary>
        [Property(Column = "account_email")]
        public virtual string AccountEmail
        {
            get;
            set;
        }
    }
}

