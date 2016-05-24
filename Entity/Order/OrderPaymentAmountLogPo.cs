
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Order
{
    /// <summary>
    ///描述：t_order_payment_amount_log ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-04-13 11:27:44
    /// </summary>
    [Class(Table = "t_order_payment_amount_log", Lazy = false, NameType = typeof(OrderPaymentAmountLogPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class OrderPaymentAmountLogPo
    {

        /// <summary>
        /// 自增ID
        /// </summary>
        [Id(1, Name = "LogId", Column = "log_id")]
        [Generator(2, Class = "native")]
        public virtual int LogId
        {
            get;
            set;
        }

        /// <summary>
        /// 订单ID
        /// </summary>
        [Property(Column = "order_id")]
        public virtual int OrderId
        {
            get;
            set;
        }

        /// <summary>
        /// 原始金额
        /// </summary>
        [Property(Column = "original_amount")]
        public virtual decimal OriginalAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 更改后最终金额
        /// </summary>
        [Property(Column = "new_amount")]
        public virtual decimal NewAmount
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

