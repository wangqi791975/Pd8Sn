using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Customer
{
    [Class(Table = "t_customer_high_risk", Lazy = false, NameType = typeof(CustomerHighRiskPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class CustomerHighRiskPo
    {
        /// <summary>
        /// 客户ID
        /// </summary>
        [Id(1, Name = "CustomerId", Column = "customer_id")]
        [Generator(2, Class = "assigned")]
        public virtual int CustomerId
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
        /// 客户邮箱
        /// </summary>
        [Property(Column = "admin_email")]
        public virtual string AdminEmail
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
    }
}
