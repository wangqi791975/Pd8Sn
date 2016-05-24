
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Order
{
    /// <summary>
    ///描述：订单状态主表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:39:47
    /// </summary>
    [Class(Table = "t_order_status", Lazy = false, NameType = typeof(OrderStatusPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class OrderStatusPo
    {
        /// <summary>
        /// 状态编码(如1、10、20)
        /// </summary>
        [Id(1, Name = "OrderStatus", Column = "order_status")]
        [Generator(2, Class = "assigned")]
        public virtual int OrderStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 状态名称(中文)
        /// </summary>
        [Property(Column = "status_name")]
        public virtual string StatusName
        {
            get;
            set;
        }
        /// <summary>
        /// 汇总状态
        /// </summary>
        [Property(Column = "status_display")]
        public virtual int? StatusDisplay
        {
            get;
            set;
        }
    }
}

