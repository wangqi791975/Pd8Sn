
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Product
{
    /// <summary>
    ///描述：产品库存表 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:48:02
    /// </summary>
    [Class(Table = "t_product_stock", Lazy = false, NameType = typeof(ProductStockPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class ProductStockPo
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
        /// 产品id
        /// </summary>
        [Property(Column = "product_id")]
        public virtual int ProductId
        {
            get;
            set;
        }
        /// <summary>
        /// 库存数量
        /// </summary>
        [Property(Column = "quantity")]
        public virtual int Quantity
        {
            get;
            set;
        }
        /// <summary>
        /// 0不绑定，1绑定&下货，2绑定&预定&不限制购买，3绑定&预定&限制购买
        /// </summary>
        [Property(Column = "limit_stock")]
        public virtual int LimitStock
        {
            get;
            set;
        }
        /// <summary>
        /// 回货周期
        /// </summary>
        [Property(Column = "day_return")]
        public virtual int? DayReturn
        {
            get;
            set;
        }
        /// <summary>
        /// 回货日期
        /// </summary>
        [Property(Column = "date_return")]
        public virtual DateTime? DateReturn
        {
            get;
            set;
        }
    }
}

