
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Marketing
{
    /// <summary>
    ///描述：活动客户条件 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-01-29 16:07:10
    /// </summary>
    [Class(Table = "t_marketing_customer_claim", Lazy = false, NameType = typeof(MarketingCustomerClaimPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class MarketingCustomerClaimPo
    {
        /// <summary>
        /// Id
        /// </summary>
        [Id(1, Name = "Id", Column = "Id")]
        [Generator(2, Class = "native")]
        public virtual int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 活动Id：t_Marketing表主键
        /// </summary>
        [Property(Column = "MarketingId")]
        public virtual int MarketingId
        {
            get;
            set;
        }
        /// <summary>
        /// 客户Id
        /// </summary>
        [Property(Column = "CustomerId")]
        public virtual int CustomerId
        {
            get;
            set;
        }
        /// <summary>
        /// 客户Email
        /// </summary>
        [Property(Column = "Email")]
        public virtual string Email
        {
            get;
            set;
        }
        /// <summary>
        /// 导入时间
        /// </summary>
        [Property(Column = "ImportTime")]
        public virtual DateTime? ImportTime
        {
            get;
            set;
        }
    }
}

