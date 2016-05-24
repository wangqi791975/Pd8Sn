using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.Customer
{
    [Class(Table = "t_wishlist", Lazy = false, NameType = typeof(WishListPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class WishListPo
    {
        /// <summary>
        /// 自增id
        /// </summary>
        [Id(1, Name = "WishListId", Column = "wishlist_id")]
        [Generator(2, Class = "native")]
        public virtual int WishListId
        {
            get;
            set;
        }

        /// <summary>
        /// 客户Id
        /// </summary>
        [Property(Column = "customer_id")]
        public virtual int CustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// 产品Id
        /// </summary>
        [Property(Column = "product_id")]
        public virtual int ProductId
        {
            get;
            set;
        }

        /// <summary>
        /// 产品数量
        /// </summary>
        [Property(Column = "product_quantity")]
        public virtual int ProductQuantity
        {
            get;
            set;
        }

        /// <summary>
        /// 喜爱类型
        /// </summary>
        [Property(Column = "`classification`")]
        public virtual int Classification
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