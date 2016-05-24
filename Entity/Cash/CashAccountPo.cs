using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Cash
{
    /// <summary>
    ///描述：客户CashAccount 
    ///创建人：Tianwen.Wan
    ///创建时间：2015-02-11 18:29:28
    /// </summary>
    [Class(Table = "t_cash_account", Lazy = false, NameType = typeof(CashAccountPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class CashAccountPo
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
        /// 客户Email
        /// </summary>
        [Property(Column = "customer_email")]
        public virtual string CustomerEmail
        {
            get;
            set;
        }

        /// <summary>
        /// 余额
        /// </summary>
        [Property(Column = "amount")]
        public virtual decimal Amount
        {
            get;
            set;
        }

        /// <summary>
        /// 币种
        /// </summary>
        [Property(Column = "currency_code")]
        public virtual string CurrencyCode
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        [Property(Column = "comment")]
        public virtual string Comment
        {
            get;
            set;
        }
    }
}
