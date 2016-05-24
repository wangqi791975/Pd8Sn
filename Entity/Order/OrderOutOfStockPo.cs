using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Order
{
    /// <summary>
    ///描述：订单缺货表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:39:34
    /// </summary>
    [Class(Table = "t_order_out_of_stock", Lazy = false, NameType = typeof(OrderOutOfStockPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class OrderOutOfStockPo
    {
        /// <summary>
        /// 自增ID
        /// </summary>

        [Id(1, Name = "OutId", Column = "out_id")]
        [Generator(2, Class = "native")]

        public virtual int OutId
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
        /// 产品编号
        /// </summary>
        [Property(Column = "product_model")]
        public virtual string ProductModel
        {
            get;
            set;
        }
        /// <summary>
        /// 缺货数量
        /// </summary>
        [Property(Column = "out_of_stock_number")]
        public virtual int OutOfStockNumber
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

