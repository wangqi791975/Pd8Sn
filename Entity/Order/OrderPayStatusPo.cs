
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Order
{
    /// <summary>
    ///描述：订单支付状态 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:39:37
    /// </summary>
    [Class(Table = "t_order_pay_status", Lazy = false, NameType = typeof(OrderPayStatusPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class OrderPayStatusPo
    {
        /// <summary>
        /// 状态编码(如1、10、20)
        /// </summary>
        [Id(1, Name = "PayStatus", Column = "pay_status")]
        [Generator(2, Class = "assigned")]
        public virtual int PayStatus
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
    }
}

