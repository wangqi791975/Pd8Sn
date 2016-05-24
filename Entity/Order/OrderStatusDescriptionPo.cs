using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Order
{
    /// <summary>
    ///描述：订单状态从表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:39:48
    /// </summary>
    [Class(Table = "t_order_status_description", Lazy = false, NameType = typeof(OrderStatusDescriptionPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class OrderStatusDescriptionPo
    {
        /// <summary>
        /// 联合主键
        /// </summary>
        [CompositeId(1, Name = "Id", ClassType = typeof(OrderStatusDescriptionPk))] 
        [KeyProperty(2,Name = "OrderStatus", Column = "order_status")]
        [KeyProperty(3,Name = "LanguageId", Column = "language_id")]
        public virtual OrderStatusDescriptionPk Id { set; get; }

        /// <summary>
        /// 状态名称
        /// </summary>
        [Property(Column = "orders_status_name")]
        public virtual string OrdersStatusName
        {
            get;
            set;
        }
    }
}

