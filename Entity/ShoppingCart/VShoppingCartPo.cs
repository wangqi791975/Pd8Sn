
using System;
using NHibernate.Mapping.Attributes;

namespace Com.Panduo.Entity.ShoppingCart
{
    /// <summary>
    ///描述：客户购物车 ORM 映射类 
    ///创建人:罗海明
    ///创建时间：12/12/2014 16:38:39
    /// </summary>
    [Class(Table = "v_shoppingcart", Lazy = false, NameType = typeof(VShoppingCartPo), DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.NonStrictReadWrite)]
    public class VShoppingCartPo
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

        /// <summary>
        /// 产品当前语种名称
        /// </summary>
        [Property(Column = "ProductName")]
        public virtual string ProductName { get; set; }

        /// <summary>
        /// 英文名称
        /// </summary>
        [Property(Column = "ProductEnName")]
        public virtual string ProductEnName { get; set; }

        /// <summary>
        /// 产品主图
        /// </summary>
        [Property(Column = "MainImage")]
        public virtual string MainImage { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        [Property(Column = "ProductCode")]
        public virtual string ProductCode { get; set; }

        /// <summary>
        /// 产品类别ID
        /// </summary>
        [Property(Column = "CategoryId")]
        public virtual int CategoryId { get; set; }

        /// <summary>
        /// 重量(g)
        /// </summary>
        [Property(Column = "Weight")]
        public virtual decimal Weight { get; set; }

        /// <summary>
        /// 体积重(g)
        /// </summary>
        [Property(Column = "VolumeWeight")]
        public virtual decimal VolumeWeight { get; set; }
        [Property(Column = "Tip")]
        public virtual int Tip { get; set; }
        [Property(Column = "ProdDiscountType")]
        public virtual int ProdDiscountType { get; set; }
        [Property(Column = "Discount")]
        public virtual decimal Discount { get; set; }
        [Property(Column = "OriginalPrice")]
        public virtual decimal OriginalPrice { get; set; }
        [Property(Column = "Price")]
        public virtual decimal Price { get; set; }
        [Property(Column = "BackorderDays")]
        public virtual string BackorderDays { get; set; }
        [Property(Column = "ProductSubTotal")]
        public virtual decimal ProductSubTotal { get; set; }
        [Property(Column = "IsLimitStock")]
        public virtual bool IsLimitStock { get; set; }
        [Property(Column = "IsBackorder")]
        public virtual bool IsBackorder { get; set; }
        /// <summary>
        /// 可用数量
        /// </summary>
        [Property(Column = "StockQty")]
        public virtual int StockQty { get; set; }

        [Property(Column = "LabelName")]
        public virtual string LabelName { get; set; }
    }
}

