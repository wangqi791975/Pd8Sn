using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Order
{
    /// <summary>
    ///描述：t_order_status_history ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-02-03 14:11:04
    /// </summary>
    [Class(Table = "t_order_status_history", Lazy = false, NameType = typeof(OrderStatusHistoryPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class OrderStatusHistoryPo
    {
        /// <summary>
        /// id
        /// </summary>
        [Id(1, Name = "Id", Column = "id")]
        [Generator(2, Class = "native")]
        public virtual int Id
        {
            get;
            set;
        }

        /// <summary>
        /// 订单Id
        /// </summary>
        [Property(Column = "order_id")]
        public virtual int OrderId
        {
            get;
            set;
        }

        /// <summary>
        /// 是否邮件通知客户
        /// </summary>
        [Property(Column = "notify_customer")]
        public virtual bool NotifyCustomer
        {
            get;
            set;
        }

        /// <summary>
        /// 发送邮件时是否附加comments
        /// </summary>
        [Property(Column = "notify_email_with_comments")]
        public virtual bool NotifyEmailWithComments
        {
            get;
            set;
        }

        /// <summary>
        /// 状态变更时间
        /// </summary>
        [Property(Column = "date_updated")]
        public virtual DateTime ChangeDate
        {
            get;
            set;
        }

        /// <summary>
        /// 状态
        /// </summary>
        [Property(Column = "`status`")]
        public virtual int Status
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        [Property(Column = "comments")]
        public virtual string Comments
        {
            get;
            set;
        }
    }
}

