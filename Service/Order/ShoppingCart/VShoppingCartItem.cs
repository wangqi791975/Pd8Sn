using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Panduo.Service.Product;

namespace Com.Panduo.Service.Order.ShoppingCart
{
    /// <summary>
    /// 购物车产品
    /// </summary>
    [Serializable]
    public class VShoppingCartItem : ShoppingCartItem
    {
        /// <summary>
        /// 产品当前语种名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 产品当前语种名称
        /// </summary>
        public string ProductEnName { get; set; }

        /// <summary>
        /// 产品主图
        /// </summary>
        public virtual string MainImage { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        public virtual string ProductCode { get; set; }

        /// <summary>
        /// 产品类别ID
        /// </summary>
        public virtual int CategoryId { get; set; }

        /// <summary>
        /// 重量(g)
        /// </summary>
        public virtual decimal Weight { get; set; }

        /// <summary>
        /// 体积重(g)
        /// </summary>
        public virtual decimal VolumeWeight { get; set; }

        public virtual int Tip { get; set; }
        public virtual ProdDiscountType ProdDiscountType { get; set; }
        public virtual decimal Discount { get; set; }
        public virtual decimal OriginalPrice { get; set; }
        public virtual decimal Price { get; set; }
        public virtual string BackorderDays { get; set; }
        public virtual bool IsLimitStock { get; set; }
        public virtual decimal ProductSubTotal { get; set; }
        public virtual bool IsBackorder { get; set; }
        public virtual int StockQty { get; set; }
        public virtual string LabelName { get; set; }

        /// <summary>
        /// 大小包装类型
        /// </summary>
        public virtual ProductOtherPackType OtherPackType
        {
            get { return ProductCode.EndsWith("H") ? ProductOtherPackType.Big : ProductCode.EndsWith("S") ? ProductOtherPackType.Small : ProductOtherPackType.None; }
        }

    }

    public enum ProdDiscountType
    {
        /// <summary>
        /// 空
        /// </summary>
        None = -1,
        /// <summary>
        /// 优先促销商品
        /// </summary>
        Sale = 0,
        /// <summary>
        /// 其次是Club折扣商品
        /// </summary>
        Club,
        /// <summary>
        /// 最后是VIP折扣
        /// </summary>
        Vip
        //OrderDiscount
    }
}
