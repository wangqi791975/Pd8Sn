using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Customer
{
    [Class(Table = "t_customer_remark_by_admin", Lazy = false, NameType = typeof(CustomerRemarkPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class CustomerRemarkPo
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
        /// 客户Id
        /// </summary>
        [Property(Column="customer_id")]
        public virtual int CustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        [Property(Column = "remark")]
        public virtual string Remark
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
        /// adminId
        /// </summary>
        [Property(Column = "admin_id")]
        public virtual int AdminId
        {
            get;
            set;
        }
    }
}
