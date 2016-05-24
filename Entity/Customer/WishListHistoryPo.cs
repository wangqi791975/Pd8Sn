using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Customer
{
    /// <summary>
    ///描述：用来保存wishlist历史数据 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：2015-01-29 14:18:01
    /// </summary>
    [Class(Table = "t_wishlist_history", Lazy = false, NameType = typeof(WishListHistoryPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class WishListHistoryPo
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        [Id(1, Name = "WishListId", Column = "wishlist_id")]
        [Generator(2, Class = "native")]
        public virtual int WishListId
        {
            get;
            set;
        }
        /// <summary>
        /// 客户ID
        /// </summary>
        [Property(Column = "customer_id")]
        public virtual int CustomerId
        {
            get;
            set;
        }
        /// <summary>
        /// 产品ID
        /// </summary>
        [Property(Column = "product_id")]
        public virtual int ProductId
        {
            get;
            set;
        }
        /// <summary>
        /// 商品数量
        /// </summary>
        [Property(Column = "product_quantity")]
        public virtual int ProductQuantity
        {
            get;
            set;
        }
        /// <summary>
        /// 心愿清单类型
        /// </summary>
        [Property(Column = "classification")]
        public virtual int Classification
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
    }
}

