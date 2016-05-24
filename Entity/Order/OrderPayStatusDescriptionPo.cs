using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Order
{
    /// <summary>
    ///描述：订单支付状态从表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:39:39
    /// </summary>
    [Class(Table = "t_order_pay_status_description", Lazy = false, NameType = typeof(OrderPayStatusDescriptionPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class OrderPayStatusDescriptionPo
    {
        [CompositeId(1, Name = "Id", ClassType = typeof(OrderPayStatusDescriptionPk))]
        [KeyProperty(2, Name = "PayStatus", Column = "pay_status")]
        [KeyProperty(3, Name = "LanguageId", Column = "language_id")]
        public virtual OrderPayStatusDescriptionPk Id
        {
            get; 
            set;
        }

        /// <summary>
        /// 状态名称
        /// </summary>
        [Property(Column = "orders_status_name")]
        public virtual string OrdersStatusName
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

