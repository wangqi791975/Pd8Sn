using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Customer
{
    /// <summary>
    ///描述：渠道商表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:38:00
    /// </summary>
    [Class(Table = "t_channel", Lazy = false, NameType = typeof(ChannelPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class ChannelPo
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
        public virtual int? CustomerId
        {
            get;
            set;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Property(Column = "date_created")]
        public virtual DateTime? DateCreated
        {
            get;
            set;
        }
        /// <summary>
        /// 创建人
        /// </summary>
        [Property(Column = "admin_id")]
        public virtual int? AdminId
        {
            get;
            set;
        }
    }
}

