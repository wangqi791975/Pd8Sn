
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Customer.Club
{
    /// <summary>
    ///描述：club客户年费表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-02-05 14:57:34
    /// </summary>
    [Class(Table = "t_club_fee", Lazy = false, NameType = typeof(ClubFeePo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class ClubFeePo
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
        /// 客户id
        /// </summary>
        [Property(Column = "customer_id")]
        public virtual int CustomerId
        {
            get;
            set;
        }
        /// <summary>
        /// 年费
        /// </summary>
        [Property(Column = "fee")]
        public virtual decimal Fee
        {
            get;
            set;
        }
        /// <summary>
        /// 添加时间
        /// </summary>
        [Property(Column = "date_created")]
        public virtual DateTime? DateCreated
        {
            get;
            set;
        }
    }
}

