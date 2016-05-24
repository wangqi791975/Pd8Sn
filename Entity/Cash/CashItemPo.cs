using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Cash
{
    /// <summary>
    ///描述：客户Account历史记录
    ///创建人：Tianwen.Wan
    ///创建时间：2015-02-11 18:29:28
    /// </summary>
    [Class(Table = "t_cash_item", Lazy = false, NameType = typeof(CashItemPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class CashItemPo
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
        /// 客户full name
        /// </summary>
        [Property(Column = "full_name")]
        public virtual string FullName
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
        /// 收入
        /// </summary>
        [Property(Column = "amount_in")]
        public virtual decimal AmountIn
        {
            get;
            set;
        }

        /// <summary>
        /// 支出
        /// </summary>
        [Property(Column = "amount_out")]
        public virtual decimal AmountOut
        {
            get;
            set;
        }

        /// <summary>
        /// 当时汇率
        /// </summary>
        [Property(Column = "exchange_rate")]
        public virtual decimal ExchangeRate
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
        /// 余额
        /// </summary>
        [Property(Column = "amount_left")]
        public virtual decimal AmountLeft
        {
            get;
            set;
        }

        /// <summary>
        /// 操作类型(收入、支出)
        /// </summary>
        [Property(Column = "op_type")]
        public virtual int OpType
        {
            get;
            set;
        }

        /// <summary>
        /// 操作时间
        /// </summary>
        [Property(Column = "op_date")]
        public virtual DateTime OpDate
        {
            get;
            set;
        }

        /// <summary>
        /// 操作管理员ID
        /// </summary>
        [Property(Column = "op_admin")]
        public virtual int OpAdmin
        {
            get;
            set;
        }

        /// <summary>
        /// 操作人(客户Email或管理员Email)
        /// </summary>
        [Property(Column = "op_account_email")]
        public virtual string OpAccountEmail
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
