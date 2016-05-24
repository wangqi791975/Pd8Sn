
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.ShoppingCart
{
    /// <summary>
    ///描述：客户购物车 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:38:39
    /// </summary>
    [Class(Table = "t_customer_basket", Lazy = false, NameType = typeof(ShoppingCartPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class ShoppingCartPo
    {
        /// <summary>
        /// 自增ID
        /// </summary>

        [Id(1, Name = "CustomerBasketId", Column = "customer_basket_id")]
        [Generator(2, Class = "native")]

        public virtual int CustomerBasketId
        {
            get;
            set;
        }
        /// <summary>
        /// 客户ID(为负数时为cookie_id)
        /// </summary>
        [Property(Column = "customer_id")]
        public virtual int CustomerId
        {
            get;
            set;
        }
        /// <summary>
        /// 商品ID
        /// </summary>
        [Property(Column = "product_id")]
        public virtual int ProductId
        {
            get;
            set;
        }
        /// <summary>
        /// 数量
        /// </summary>
        [Property(Column = "product_quantity")]
        public virtual int ProductQuantity
        {
            get;
            set;
        }
        /// <summary>
        /// 备注
        /// </summary>
        [Property(Column = "remark")]
        public virtual string Remark
        {
            get;
            set;
        }
        /// <summary>
        /// 添加时间
        /// </summary>
        [Property(Column = "date_created")]
        public virtual DateTime DateCreated
        {
            get;
            set;
        }
        /// <summary>
        /// 修改时间
        /// </summary>
        [Property(Column = "date_modified")]
        public virtual DateTime? DateModified
        {
            get;
            set;
        }
        /// <summary>
        /// 下架物品是否checked
        /// </summary>
        [Property(Column = "remove_checked")]
        public virtual Boolean RemoveChecked
        {
            get;
            set;
        }
    }
}

