
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Customer.Club
{
    /// <summary>
    ///描述：club客户黑名单
    ///创建人:wq
    ///创建时间：2015-03-18 14:57:34
    /// </summary>
    [Class(Table = "t_club_blacklist", Lazy = false, NameType = typeof(ClubBlackListPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class ClubBlackListPo
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
        /// 客户邮箱
        /// </summary>
        [Property(Column = "customer_email")]
        public virtual string CustomerEmail
        {
            get;
            set;
        }
    }
}

